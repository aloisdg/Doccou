using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Counter.Documents.Helper;

namespace Counter.Documents
{
	internal sealed class Odt : ADocument
	{
		const string Path = "meta.xml";
		public override DocumentType Type { get { return DocumentType.Odt; } }
		public override uint Count { get; protected set; }

		public Odt(Stream stream)
		{
			Count = ExtractNumber(Indiana.ReadArchive(stream, Path));
		}

		// Could we improve this ?
		private static uint ExtractNumber(string content)
		{
			//var matched = Regex.Match(content, "(?<=page-count=\")[ A-Za-z0-9]*").Groups[0].Value;

			var xelement = XElement.Parse(content);
			var node = xelement.Descendants().Elements().First(x => x.Name.LocalName.Equals("document-statistic"));
			var value = node.Attributes().First(x => x.Name.LocalName.Equals("page-count")).Value;
			return Convert.ToUInt32(value);
		}
	}

	
}
