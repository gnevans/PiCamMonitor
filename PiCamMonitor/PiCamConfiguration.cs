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
using System.Linq;
using System.Text;

namespace PiCamMonitor
{
	public class PiCamConfiguration
	{
		// following to support downloads
		public string RemoteHost { get; set; }
		public string RemoteUserName { get; set; }
		public string RemotePassword { get; set; }
		public string RemoteSshKeyFingerprint { get; set; }
		public string RemoteDirectoryPath { get; set; }
		public string LocalDirectoryPath { get; set; }

		// following to support live streaming
		public string  LiveStreamUrl { get; set; }

		// following to support event broadcasts
		public int EventPort { get; set; }
	}
}
