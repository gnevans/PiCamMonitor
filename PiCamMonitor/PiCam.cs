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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WinSCP;

namespace PiCamMonitor
{
	public class PiCam
	{
		ILogger _logger;
		//int _eventPort;
		PiCamConfiguration _config;
		Semaphore _downloaderSemaphore;

		bool _keepListening;

		public event EventHandler<PiCamEventArgs> EventReceived;
		public event EventHandler<DownloadCompleteEventArgs> DownloadComplete;

		public PiCam(ILogger logger, PiCamConfiguration config)
		{
			_logger = logger;
			//_eventPort = port;
			_config = config;
			_downloaderSemaphore = new Semaphore(1, 1);
		}

		public string LiveStreamUrl { get { return _config.LiveStreamUrl; } }

		public void StartListening()
		{
			_keepListening = true;

			Thread t = new Thread(EventListenerThread);
			t.IsBackground = true;
			t.Start();
		}

		public void StopListening()
		{
			_keepListening = false;
		}

		void EventListenerThread()
		{
			UdpClient udpClient;

			try
			{
				udpClient = new UdpClient(_config.EventPort);
			}
			catch (Exception ex)
			{
				_logger.Log(ex);
				throw ex;
			}

			IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, _config.EventPort);
			_logger.Log("Got end point");


			while (_keepListening)
			{
				try
				{
					_logger.Log("About to start listening");
					byte[] packet = udpClient.Receive(ref endPoint);
					string packetMessage = Encoding.ASCII.GetString(packet, 0, packet.Length);

					if (packetMessage.StartsWith("picam"))
					{
						if (EventReceived != null)
						{
							PiCamEventArgs eventArgs = new PiCamEventArgs();
							eventArgs.EventData = packetMessage;
							EventReceived(this, eventArgs);
						}
					}
				}
				catch (Exception ex)
				{
					_logger.Log(ex);
				}
			}
		}

		public bool StartDownloading()
		{
			if (_downloaderSemaphore.WaitOne(0))
			{
				//_logger.Log("Downloader started");
				Thread t = new Thread(DownloadThread);
				t.IsBackground = true;
				t.Start();
				return true;
			}
			else
			{
				_logger.Log("Downloader already running");
				return false;
			}
		}

		void DownloadThread()
		{
			int numDownloaded = 0;

			try
			{
				// Setup session options
				SessionOptions sessionOptions = new SessionOptions
				{
					Protocol = Protocol.Sftp,
					HostName = _config.RemoteHost,
					UserName = _config.RemoteUserName,
					Password = _config.RemotePassword,
					SshHostKeyFingerprint = _config.RemoteSshKeyFingerprint
				};

				using (Session session = new Session())
				{
					// Connect
					session.Open(sessionOptions);

					Stopwatch stopwatch = new Stopwatch();

					TransferOptions transferOptions = new TransferOptions();
					transferOptions.TransferMode = TransferMode.Binary;
					TransferOperationResult transferResult;

					stopwatch.Start();
					RemoteDirectoryInfo dirInfo = session.ListDirectory(_config.RemoteDirectoryPath);
					stopwatch.Stop();
					Console.WriteLine(string.Format("directory download took {0}ms", stopwatch.ElapsedMilliseconds));



					stopwatch.Reset();
					stopwatch.Start();
					int numFiles = 0;
					HashSet<string> subDirNames = new HashSet<string>();
					foreach (RemoteFileInfo fileInfo in dirInfo.Files)
					{
						if (NeedToDownloadFile(fileInfo.Name))
						{
							string subDirName = DestDirForFile(fileInfo.Name);
							numFiles++;
							subDirNames.Add(subDirName);
						}
					}
					stopwatch.Stop();
					Console.WriteLine(string.Format("directory scan took {0}ms", stopwatch.ElapsedMilliseconds));

					stopwatch.Reset();
					stopwatch.Start();

					StringBuilder sb = new StringBuilder();

					foreach (string subDirName in subDirNames)
					{
						string destDir = _config.LocalDirectoryPath + @"\" + subDirName + @"\";
						// make sure it exists
						if (!Directory.Exists(destDir))
						{
							Directory.CreateDirectory(destDir);
						}
						transferResult = session.GetFiles(_config.RemoteDirectoryPath + "/" + subDirName + "*.jpg", destDir, true, transferOptions);

						transferResult.Check();
						numDownloaded += transferResult.Transfers.Count;
					}

					stopwatch.Stop();
					Console.WriteLine(string.Format("file download took {0}ms", stopwatch.ElapsedMilliseconds));

					if (numFiles == 0)
					{
						_logger.Log("No files to download");
					}
					else if (numDownloaded != numFiles)
					{
						// something has gone wrong
						// shouldn't happen as transfer errors should throw exception
						_logger.Log(string.Format("Downloader expected {0} files, but downloaded {1} files", numFiles, numDownloaded));
					}
					else
					{
						string dirList = "";
						foreach (string subDirName in subDirNames)
						{
							if (dirList.Length > 0)
							{
								dirList += ", ";
							}
							dirList += subDirName;
						}
						_logger.Log(string.Format("Downloaded {0} files for {1}", numDownloaded, dirList));
					}

				}
			}
			catch (Exception ex)
			{
				_logger.Log(ex);
			}

			if (DownloadComplete != null)
			{
				DownloadCompleteEventArgs eventArgs = new DownloadCompleteEventArgs();
				eventArgs.FramesDownloaded = numDownloaded;
				DownloadComplete(this, eventArgs);
			}

			_downloaderSemaphore.Release();
		}

		bool NeedToDownloadFile(string filename)
		{
			string extension = Path.GetExtension(filename);
			return (string.Compare(extension, ".jpg", true) == 0);
		}

		string DestDirForFile(string filename)
		{
			// first 8 chars of filename is the date
			return filename.Substring(0, 8);
		}

		public string[] GetFramesetNames()
		{
			List<string> framesetNames = new List<string>();
			string[] subDirs = Directory.GetDirectories(_config.LocalDirectoryPath);
			foreach (string subDirPath in subDirs)
			{
				framesetNames.Add(SubDirPathToFramesetName(subDirPath));
			}

			return framesetNames.ToArray();
		}

		public string[] GetFilesForFrameset(string framesetName)
		{
			string subDirPath = FramesetNameToSubDirPath(framesetName);
			string[] files = Directory.GetFiles(subDirPath, "*.jpg");
			// make sure files are sorted
			Array.Sort(files);
			return files;
		}

		string SubDirPathToFramesetName(string subDirPath)
		{
			// first get the the subDir name - which is the date (YYYYMMDD)
			string framesetName = Path.GetFileName(subDirPath);
			// to make date easier to read, add dashes
			if (framesetName.Length == 8)
			{
				framesetName = framesetName.Substring(0, 4) + "-" + framesetName.Substring(4, 2) + "-" + framesetName.Substring(6, 2);
			}
			return framesetName;
		}

		string FramesetNameToSubDirPath(string framesetName)
		{
			// first remove any dashes from the name 
			framesetName = framesetName.Replace("-", "");
			// should now have the date (YYYYMMDD) which is the directory name
			return Path.Combine(_config.LocalDirectoryPath, framesetName);
		}
	}
}
