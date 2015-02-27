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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WinSCP;

namespace PiCamMonitor
{
	public class FileDownloader
	{
		ILogger _logger;
		Semaphore _downloaderSemaphore;

		public event EventHandler<DownloadCompleteEventArgs> DownloadComplete;

		public FileDownloader(ILogger logger)
		{
			_logger = logger;
			_downloaderSemaphore = new Semaphore(1, 1);
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
					HostName = Properties.Settings.Default.PiHost,
					UserName = Properties.Settings.Default.PiUserName,
					Password = Properties.Settings.Default.PiPassword,
					SshHostKeyFingerprint = Properties.Settings.Default.PiSshHostKeyFingerprint
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
					RemoteDirectoryInfo dirInfo = session.ListDirectory(Properties.Settings.Default.PiMotionImagePath);
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
						string destDir = Properties.Settings.Default.LocalBaseDownloadPath + @"\" +  subDirName + @"\";
						// make sure it exists
						if (!Directory.Exists(destDir))
						{
							Directory.CreateDirectory(destDir);
						}
						transferResult = session.GetFiles(Properties.Settings.Default.PiMotionImagePath + "/" + subDirName + "*.jpg", destDir, true, transferOptions);

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
			string[] subDirs = Directory.GetDirectories(Properties.Settings.Default.LocalBaseDownloadPath);
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
			return Path.Combine(Properties.Settings.Default.LocalBaseDownloadPath, framesetName);
		}

	}
}
