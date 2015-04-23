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
	internal class Doc : ADocument
	{
		public override DocumentType	Type { get { return DocumentType.Pdf; } }

		public Doc(Stream stream) : base(stream) { }

		public override uint Count()
		{
			//const WdStatistic stat = WdStatistic.wdStatisticPages;
			//_Application wordApp = new Application();

			//object fileName = FullName;
			//object readOnly = false;
			//object isVisible = true;

			////  the way to handle parameters you don't care about in .NET
			//object missing = Missing.Value;

			////   Make word visible, so you can see what's happening
			////wordApp.Visible = true;
			////   Open the document that was chosen by the dialog
			//var aDoc = wordApp.Documents.Open(ref fileName,
			//			ref missing, ref readOnly, ref missing,
			//			ref missing, ref missing, ref missing,
			//			ref missing, ref missing, ref missing,
			//			ref missing, ref isVisible);

			//return Convert.ToUInt32(aDoc.ComputeStatistics(stat, ref missing));

			uint count = GetPageCount(Stream);

			return count;
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

		// we use PCL Storage
		//private static async Task<Stream> OpenAsync(string fullName)
		//{
		//	var file = await FileSystem.Current.GetFileFromPathAsync(fullName);
		//	return await file.OpenAsync(FileAccess.Read);
		//}

		// Could we improve this ?
		private static uint ExtractNumber(string content)
		{
			//var matched = Regex.Match(content, @"(?<=\<Pages\>).*(?=\</Pages\>)").Groups[1].Value;

			var xelement = XElement.Parse(content);
			var node = xelement.Elements().First(x => x.Name.LocalName.Equals("Pages"));

			return Convert.ToUInt32(node.Value);
		}
	}
}
