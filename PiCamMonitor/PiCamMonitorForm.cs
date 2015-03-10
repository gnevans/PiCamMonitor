#region copyright
// This file is part of PiCamMonitor a Windows program to download
// and display images captured by a camera on a Raspberry Pi.
// Copyright (C) 2015  Gerald Evans
// 
// PiCamMonitor is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using AForge.Video;
using AForge.Video.DirectShow;
using PiCamMonitor.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSCP;

namespace PiCamMonitor
{
	public partial class PiCamMonitorForm : Form, ILogger
	{
		bool _shutDown = false;
		MJPEGStream _videoSource = new MJPEGStream();
		FrameSet _frameSet = new FrameSet();
		int _newFramesSinceLastView = 0;

		// only a single PiCan supported at the moment
		PiCamConfiguration _piCamConfig;
		PiCam _piCam;

		const string _autoStartKeyName = "GNE_PiCamMonitor";


		public PiCamMonitorForm()
		{
			InitializeComponent();

			UpdateAutoStart();

			InitialisePiCams();

			if (Settings.Default.UseNotificationArea)
			{
				// don't want Minimise button 
				//this.MinimizeBox = false;

				// Force window to be created (but not shown)
				// This is essential for InvokeRequired()/BeginInvoke() to work
				IntPtr dummy = Handle;
			}
			else
			{
				notifyIcon.Visible = false;
				ShowForm();
			}

			// test
			labelDownloadProgress.Text = "";
			EnableMediaControls(false);
			PopulateFramesetNamesCombo();

		}

		void InitialisePiCams()
		{
			_piCamConfig = new PiCamConfiguration();

			// following to support downloads
			_piCamConfig.RemoteHost = Settings.Default.PiHost;
			_piCamConfig.RemoteUserName = Settings.Default.PiUserName;
			_piCamConfig.RemotePassword = Settings.Default.PiPassword;
			_piCamConfig.RemoteSshKeyFingerprint = Settings.Default.PiSshHostKeyFingerprint;
			_piCamConfig.RemoteDirectoryPath = Settings.Default.PiMotionImagePath;
			_piCamConfig.LocalDirectoryPath = Settings.Default.LocalBaseDownloadPath;

			// following to support live streaming
			_piCamConfig.LiveStreamUrl = Settings.Default.PiStreamUrl;

			// following to support event broadcasts
			_piCamConfig.EventPort = Settings.Default.PiEventPort;

			_piCam = new PiCam(this, _piCamConfig);

			if (_piCamConfig.EventPort != 0)
			{
				_piCam.EventReceived += PiCam_EventReceived;
				_piCam.StartListening();
			}

			_piCam.DownloadComplete += PiCam_DownloadComplete;

			if (Settings.Default.DownloadAtStartup)
			{
				StartDownload();
			}
			StartPeriodicDownloads();
		}

		void PiCam_EventReceived(object sender, PiCamEventArgs e)
		{
			string eventData = e.EventData;

			EventReceived(eventData);
		}


		delegate void EventReceivedDelegate(string eventData);
		void EventReceived(string eventData)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new EventReceivedDelegate(EventReceived), eventData);
				return;
			}

			Log("Event: {0}", eventData);

			if (eventData.StartsWith("picam-event-start"))
			{
				if (Settings.Default.ShowLifeFeedOnEventStart)
				{
					// make sure we are visible
					ShowForm();

					// make sure live feed is selected
					radioButtonViewLiveFeed.Checked = true;
				}

				// audio event
				string soundFilename = Settings.Default.EventStartSound;
				if (!string.IsNullOrEmpty(soundFilename))
				{
					try
					{
						using (SoundPlayer player = new SoundPlayer(soundFilename))
						{
							player.Play();
						}
					}
					catch (Exception ex)
					{
						Log(ex);
					}
				}

				StartDownload();
			}
		}

		private void PiCamForm_Load(object sender, EventArgs e)
		{
			//labelDownloadProgress.Text = "";
			//EnableMediaControls(false);
			//PopulateFramesetNamesCombo();
		}

		void UpdateAutoStart()
		{
			if (AutoStart.SetAutoStart(_autoStartKeyName, Settings.Default.AutoStart))
			{
				// changes were made to the registry 
				// either changing from on to off
				// or changing the path of the executable
				Log("AutoStart changed to {0}", Settings.Default.AutoStart ? "on" : "off");
			}
		}

		bool WantNotificationShown()
		{
			// don't bother showing notification if form is already visible
			return !this.Visible;
		}

		private void PiCamForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// stop anything that is running
			StopLiveFeed();

			StopFramesetPlay();

			if (!Settings.Default.UseNotificationArea)
			{
				// we are running in standard mode (ie not in the Notification Area)
				// so when user closes the window, they want the program to close
				Application.Exit();
			}
			else if (_shutDown || e.CloseReason != CloseReason.UserClosing)
			{
				// user has requested (from the Exit menu item) that they want the program to really close
				Application.Exit();
			}
			else
			{
				// we are running in the notification area and user just wants the form closed (/hidden)
				// but for us to continue running
				this.Visible = false;
				e.Cancel = true;
			}
		}

		void StartLiveFeed(PiCam piCam)
		{
			_videoSource = new MJPEGStream(piCam.LiveStreamUrl);
			_videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(NewFrameEventHandler);
			_videoSource.Start();
		}

		void StopLiveFeed()
		{
			_videoSource.Stop();
			_videoSource.NewFrame -= new AForge.Video.NewFrameEventHandler(NewFrameEventHandler);
		}

		private void NewFrameEventHandler(object sender, AForge.Video.NewFrameEventArgs eventArgs)
		{
			Bitmap image = (Bitmap)eventArgs.Frame.Clone();
			ShowImage(image);
		}

		void ShowSelectedFrameset()
		{
			_frameSet.Clear();

			string selectedFramesetName = (string)comboBoxFramesets.SelectedItem;
			if (!string.IsNullOrEmpty(selectedFramesetName))
			{
				string[] files = _piCam.GetFilesForFrameset(selectedFramesetName);
				foreach (string filename in files)
				{
					_frameSet.AddFrame(filename);
				}
			}

			int numFrames = _frameSet.Count;
			trackBar.Minimum = 0;
			trackBar.Maximum = numFrames > 0 ? numFrames - 1 : 0;
			trackBar.Value = 0;
			ShowCurImage();
		}

		private void trackBar_ValueChanged(object sender, EventArgs e)
		{
			if (radioButtonViewDownloadedFrames.Checked)
			{
				ShowCurImage();
			}
		}

		void ShowCurImage()
		{
			int numFrames = _frameSet.Count;
			int curFrame = trackBar.Value;
			if (numFrames > 0)
			{
				labelFramePos.Text = string.Format("Frame {0} of {1}", curFrame + 1, numFrames);
				string filename = _frameSet.GetFrameFilename(curFrame);
				labelFrameName.Text = Path.GetFileName(filename);
				Image image = _frameSet.GetImage(curFrame);
				ShowImage(image);
			}
			else
			{
				labelFramePos.Text = "No frames";
			}
		}

		void ShowImage(Image image)
		{
			Image oldImage = pictureBox.Image;
			pictureBox.Image = image;
			if (oldImage != image)
			{
				if (oldImage != null)
				{
					oldImage.Dispose();
				}
			}
		}

		private void ViewChanged(object sender, EventArgs e)
		{
			// TODO: check if this gets fired twice?

			PiCam curPiCam = _piCam;

			// make sure everything is stopped first
			StopLiveFeed();
			pictureBox.Image = null;
			EnableMediaControls(false);

			if (radioButtonViewNothing.Checked)
			{
			}
			else if (radioButtonViewLiveFeed.Checked)
			{
				StartLiveFeed(curPiCam);
			}
			else if (radioButtonViewDownloadedFrames.Checked)
			{
				ShowSelectedFrameset();
				EnableMediaControls(true);
				//BuildFrameset();
				//ShowFrameset();
			}
		}

		private void buttonPlay_Click(object sender, EventArgs e)
		{
			if (timerPlayback.Enabled)
			{
				// already playing so pause
				StopFramesetPlay();
			}
			else
			{
				StartFramesetPlay();
			}
		}

		private void buttonFirst_Click(object sender, EventArgs e)
		{
			ShowFrame(0);
		}

		private void buttonPrevious_Click(object sender, EventArgs e)
		{
			ShowFrame(trackBar.Value - 1);
		}

		private void buttonNext_Click(object sender, EventArgs e)
		{
			ShowFrame(trackBar.Value + 1);
		}

		private void buttonLast_Click(object sender, EventArgs e)
		{
			ShowFrame(_frameSet.Count);
		}

		void StartFramesetPlay()
		{
			if (radioButtonViewDownloadedFrames.Checked)
			{
				if (_frameSet.Count > 1)
				{
					buttonPlay.Image = Resources.Pause;
					timerPlayback.Interval = 1000 / Properties.Settings.Default.PiFps;
					timerPlayback.Tick += timerPlayback_Tick;
					timerPlayback.Start();
				}
			}
		}

		void EnableMediaControls(bool enabled)
		{
			trackBar.Enabled = enabled;
			labelFramePos.Visible = enabled;
			labelFrameName.Visible = enabled;
			buttonPlay.Enabled = enabled;
			buttonFirst.Enabled = enabled;
			buttonPrevious.Enabled = enabled;
			buttonNext.Enabled = enabled;
			buttonLast.Enabled = enabled;
		}

		void StopFramesetPlay()
		{
			if (timerPlayback.Enabled)
			{
				buttonPlay.Image = Resources.Play;
				timerPlayback.Stop();
				timerPlayback.Tick -= timerPlayback_Tick;
			}
		}

		void timerPlayback_Tick(object sender, EventArgs e)
		{
			int nextFrameIndex = trackBar.Value + 1;
			if (nextFrameIndex >= _frameSet.Count)
			{
				StopFramesetPlay();
			}
			// ShowFrame() will ensure index is in range
			ShowFrame(nextFrameIndex);
		}

		void ShowFrame(int index)
		{
			if (radioButtonViewDownloadedFrames.Checked)
			{
				if (index >= _frameSet.Count)
				{
					index = _frameSet.Count - 1;
				}
				if (index < 0)
				{
					index = 0;
				}

				trackBar.Value = index;
			}
		}

		void PopulateFramesetNamesCombo()
		{
			string selectedFramesetName = (string)comboBoxFramesets.SelectedItem;

			comboBoxFramesets.BeginUpdate();
			comboBoxFramesets.Items.Clear();

			string[] framesetNames = _piCam.GetFramesetNames();
			Array.Sort(framesetNames, (f1, f2) => f2.CompareTo(f1));
			foreach (string framesetName in framesetNames)
			{
				comboBoxFramesets.Items.Add(framesetName);
			}

			// if an item was selected, try and keep the same item selected
			if (string.IsNullOrEmpty(selectedFramesetName))
			{
				// select first item
				if (comboBoxFramesets.Items.Count > 0)
				{
					comboBoxFramesets.SelectedIndex = 0;
				}
			}
			else
			{
				// An item was selected, so try and keep the same item selected
				// This will also trigger the re-building of the FrameSet
				comboBoxFramesets.SelectedItem = selectedFramesetName;
			}
			// one of the above should have fired off comboBoxFramesets_SelectedIndexChanged()
			// so no need to reset the frameset
			comboBoxFramesets.EndUpdate();
			
		}

		private void comboBoxFramesets_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (radioButtonViewDownloadedFrames.Checked)
			{
				ShowSelectedFrameset();
			}
		}

		void ShowForm()
		{
			this.Visible = true;
			this.Activate();
			SetNumNewFrames(0);
		}


		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_shutDown = true;
			this.Close();
			//Application.Exit();
		}

		private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ShowForm();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowForm();
		}

		private void viewNewFramesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int numNewFrames = _newFramesSinceLastView;
			ShowForm();

			if (numNewFrames > 0)
			{
				// need to start showing the new frames

				// make sure viewing downloaded frames is selected
				radioButtonViewDownloadedFrames.Checked = true;

				// select the first item 
				// This does mean we can't view any frames downloaded for previous days.
				comboBoxFramesets.SelectedIndex = 0;

				// update frameset/trackbar to match
				ShowSelectedFrameset();

				// calc where playing should stat from
				int frameNum = Math.Max(_frameSet.Count - numNewFrames, 0);
				ShowFrame(frameNum);

				// and start playing
				StartFramesetPlay();
			}
		}

		#region Downloading
		void StartPeriodicDownloads()
		{
			int downloadPeriod = Settings.Default.DownloadInterval;	// in minutes
			// use 0 if you don't want the files downloaded periodically 
			if (downloadPeriod > 0)
			{
				timerDownload.Interval = downloadPeriod * 60 * 1000;
				timerDownload.Tick += timerDownload_Tick;
				timerDownload.Start();
			}
		}

		void timerDownload_Tick(object sender, EventArgs e)
		{
			StartDownload();
		}

		private void buttonCheckForFiles_Click(object sender, EventArgs e)
		{
			StartDownload();
		}

		void StartDownload()
		{
			if (_piCam.StartDownloading())
			{
				labelDownloadProgress.Text = "Downloading...";
				buttonCheckForFiles.Enabled = false;
			}
		}

		void PiCam_DownloadComplete(object sender, DownloadCompleteEventArgs e)
		{
			DownloadComplete(e.FramesDownloaded);
		}

		delegate void DownloadCompleteDelegate(int framesDownloaded);
		void DownloadComplete(int framesDownloaded)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new DownloadCompleteDelegate(DownloadComplete), framesDownloaded);
				return;
			}
			labelDownloadProgress.Text = ""; // "Downloading Complete";
			buttonCheckForFiles.Enabled = true;
			if (framesDownloaded > 0)
			{
				PopulateFramesetNamesCombo();
				if (WantNotificationShown())
				{
					notifyIcon.BalloonTipTitle = "PiCamMonitor";
					notifyIcon.BalloonTipText = string.Format("{0} frames downloaded from PiCam", framesDownloaded);
					notifyIcon.ShowBalloonTip(5000);

					SetNumNewFrames(_newFramesSinceLastView + framesDownloaded);
				}
			}
		}

		void SetNumNewFrames(int numNewFrames)
		{
			_newFramesSinceLastView = numNewFrames;

			string text = "PiCamMonitor";
			if (_newFramesSinceLastView > 0)
			{
				text += string.Format(" - {0} new frames available", _newFramesSinceLastView);
			}
			notifyIcon.Text = text;
			viewNewFramesToolStripMenuItem.Enabled = _newFramesSinceLastView > 0;
		}
		#endregion

		#region Logging
		public void Log(string format, params object[] formatParameters)
		{
			AddMessage(string.Format(format, formatParameters));
		}

		public void Log(Exception ex)
		{
			AddMessage(ex.Message);
		}

		delegate void AddMessageDelegate(string message);
		void AddMessage(string message)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new AddMessageDelegate(AddMessage), new object[] { message });
				return;
			}
			// on UI thread
			listBoxPiCam.Items.Add(DateTime.Now.ToString() + " " + message);
			if (checkBoxTail.Checked)
			{
				// scroll newly added line into view
				ShowLogTail();
			}
		}

		private void checkBoxTail_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBoxTail.Checked)
			{
				// make sure we are showing tail
				ShowLogTail();
			}
		}

		void ShowLogTail()
		{
			listBoxPiCam.TopIndex = listBoxPiCam.Items.Count - 1;
		}
		#endregion
	}
}
