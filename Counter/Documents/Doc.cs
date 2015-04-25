using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Counter.Documents.Helper;

namespace Counter.Documents
{
	internal sealed class Doc : ADocument
	{
		const string Path = "docProps/app.xml";
		public override DocumentType Type { get { return DocumentType.Pdf; } }
		public override uint Count { get; protected set; }

		// a pptx or a docx is a zip
		public Doc(Stream stream)
		{
			Count = ExtractNumber(Indiana.ReadArchive(stream, Path));
		}

		// Could we improve this ?
		private static uint ExtractNumber(string content)
		{
			//var matched = Regex.Match(content, @"(?<=\<Pages\>).*(?=\</Pages\>)").Groups[0].Value;

			var xelement = XElement.Parse(content);
			var node = xelement.Elements().First(x => x.Name.LocalName.Equals("Pages"));

			return Convert.ToUInt32(node.Value);
		}
	}
}