using System;
using System.IO;

namespace Doccou.Pcl.Test
{
	public static class Helper
	{
		public static UInt32 ReadCount(string path)
		{
			return new Document(path, File.Open(path, FileMode.Open)).Count;
		}
	}
}
