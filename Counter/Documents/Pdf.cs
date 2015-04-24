using iTextSharp.text.pdf;
using System;
using System.IO;

namespace Counter.Documents
{
	internal sealed class Pdf : ADocument
	{
		public override DocumentType Type { get { return DocumentType.Pdf; } }

		public override uint Count { get; protected set; }

		// faster than regex.
		// so thread http://stackoverflow.com/q/320281/1248177
		public Pdf(Stream stream)
		{
			Count = Convert.ToUInt32(new PdfReader(stream).NumberOfPages);
		}
	}
}