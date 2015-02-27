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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiCamMonitor
{
	public class FrameSet
	{
		List<string> _filenames = new List<string>();

		public int Count { get { return _filenames.Count; } }

		public void Clear()
		{
			_filenames.Clear();
		}

		public void AddFrame(string filename)
		{
			// filename is full path
			// TODO: should this be inserted in correct position or leave to caller?
			_filenames.Add(filename);
		}

		public void DeleteFrame(int index)
		{
			CheckIndex("DeleteFrame", index);

			_filenames.RemoveAt(index);
		}

		public string GetFrameFilename(int index)
		{
			CheckIndex("GetFrameFilename", index);

			return _filenames[index];
		}

		public Image GetImage(int index)
		{
			CheckIndex("GetImage", index);

			string filename = _filenames[index];

			return Image.FromFile(filename);
		}

		void CheckIndex(string source, int index)
		{
			if (index < 0 || index >= _filenames.Count)
			{
				throw new ArgumentException(string.Format("FrameSet.{0}(index = {1}) is out of range", source, index));
			}
		}
	}
}
