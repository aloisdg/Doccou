using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Doccou.Pcl.Documents.Archives
{
	internal sealed class Odt : AArchive
	{
		const string			Path = "meta.xml";
		public override DocumentType	Type { get { return DocumentType.Odt; } }
		public override uint		Count { get; protected set; }

		public Odt(Stream stream)
		{
			try
			{
				Count = ExtractNumber(ReadArchive(stream, Path));
			}
			catch (Exception ex)
			{
				throw new Exception("We can't create this ODT.", ex);
			}
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
