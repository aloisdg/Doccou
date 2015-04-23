using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Counter.Documents
{
	internal sealed class Doc : ADocument
	{
		public override DocumentType	Type { get { return DocumentType.Pdf; } }
		public override uint		Count { get; protected set; }

		public Doc(Stream stream)
		{
			Count = GetPageCount(stream);
		}

		// a pptx or a docx is a zip
		private static uint GetPageCount(Stream stream)
		{
			var content = ReadArchive(stream);
			return ExtractNumber(content);
		}

		// we use Microsoft Compression
		private static string ReadArchive(Stream archiveStream)
		{
			const string path = "docProps/app.xml";
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

		// we cant use for know PCL Storage
		//private static async Task<Stream> OpenAsync(string fullName)
		//{
		//	var file = await FileSystem.Current.GetFileFromPathAsync(fullName);
		//	return await file.OpenAsync(FileAccess.Read);
		//}

		// Could we improve this ?
		// Benchmark XmlDocument with SelectSingleNode ?
		private static uint ExtractNumber(string content)
		{
			//var matched = Regex.Match(content, @"(?<=\<Pages\>).*(?=\</Pages\>)").Groups[1].Value;

			var xelement = XElement.Parse(content);
			var node = xelement.Elements().First(x => x.Name.LocalName.Equals("Pages"));

			return Convert.ToUInt32(node.Value);
		}
	}
}
