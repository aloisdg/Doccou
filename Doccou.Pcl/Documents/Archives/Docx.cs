using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Doccou.Pcl.Documents.Archives
{
	internal sealed class Docx : AArchive
	{
		const string			Path = "docProps/app.xml";
		public override DocumentType	Type { get { return DocumentType.Docx; } }
		public override uint		Count { get; protected set; }

		// a pptx or a docx is a zip
		public Docx(Stream stream)
		{
			try
			{
				Count = ExtractNumber(ReadArchive(stream, Path));
			}
			catch (Exception ex)
			{
				throw new Exception("We can't create this DOCX.", ex);
			}
		}

		// Could we improve this ?
		private static uint ExtractNumber(string content)
		{
			var matched = Regex.Match(content, @"(?<=\<Pages\>).*(?=\</Pages\>)").Groups[0].Value;

			var xelement = XElement.Parse(content);
			var node = xelement.Elements().First(x => x.Name.LocalName.Equals("Pages"));

			return Convert.ToUInt32(node.Value);
		}
	}
}