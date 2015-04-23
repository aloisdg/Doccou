using System;
using System.IO;
using iTextSharp.text.pdf;

namespace Counter.Documents
{
	internal class Pdf : ADocument
	{
		public override DocumentType Type { get { return DocumentType.Pdf; } }

		public Pdf(Stream stream) : base(stream) { }

		public override uint Count()
		{
			var pdfReader = new PdfReader(Stream);
			return Convert.ToUInt32(pdfReader.NumberOfPages);
		}
	}
}
