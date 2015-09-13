using System;
using System.Collections.Generic;
using System.IO;

namespace Doccou.ViewModel.Extensions
{
	public static class TreeReader
	{
		public static IEnumerable<string> GetAllFiles(this string path)
		{
			var queue = new Queue<string>();
			queue.Enqueue(path);
			while (queue.Count > 0)
			{
				path = queue.Dequeue();
				if (String.IsNullOrEmpty(Path.GetExtension(path)))
				{
					try
					{
						foreach (var subDir in Directory.GetDirectories(path))
							queue.Enqueue(subDir);
					}
					catch (Exception ex)
					{
						Console.Error.WriteLine(ex);
					}
					string[] files = null;
					try
					{
						files = Directory.GetFiles(path);
					}
					catch (Exception ex)
					{
						Console.Error.WriteLine(ex);
					}
					if (files != null)
						foreach (var t in files)
							yield return t;
				}
				else
					yield return path;
			}
		}
	}
}