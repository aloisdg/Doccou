using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Doccou.Pcl.Documents.Archives
{
	internal sealed class Pptx : AArchive
	{
		const string			Path = "docProps/app.xml";
		public override DocumentType	Type { get { return DocumentType.Pptx; } }
		public override uint		Count { get; protected set; }

		public Pptx(Stream stream)
		{
			try
			{
				Count = ExtractNumber(ReadArchive(stream, Path));
			}
			catch (Exception ex)
			{
				throw new Exception("We can't create this PPTX.", ex);
			}
		}

		private static uint ExtractNumber(string content)
		{
			return Convert.ToUInt32(Regex.Match(content, @"(?<=\<Slides\>).*(?=\</Slides\>)").Groups[0].Value);
		}
	}
}
