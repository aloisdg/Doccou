using System.IO;
using System.IO.Compression;

namespace Counter.Documents
{
	internal abstract class AArchive : ADocument
	{
		public string ReadArchive(Stream archiveStream, string path)
		{
			using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read))
			{
				var zipArchiveEntry = archive.GetEntry(path);
				using (var stream = zipArchiveEntry.Open())
				using (var reader = new StreamReader(stream))
				{
					var content = reader.ReadToEnd();
					return content;
				}
			}
		}
	}
}
