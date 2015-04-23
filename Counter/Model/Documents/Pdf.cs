using System;
using iTextSharp.text.pdf;

namespace Counter.Model.Documents
{
	internal class Pdf : ADocument
	{
		public override DocumentType Type { get { return DocumentType.Pdf; } }

		public Pdf(string fullName) : base(fullName) { }

		public override uint Count()
		{
			try
			{
				var pdfReader = new PdfReader(FullName);
				return Convert.ToUInt32(pdfReader.NumberOfPages);
			}
			catch (Exception ex)
			{
				// we could skip and return 0.
				throw new Exception(String.Format("There is a problem with {0}", FullName), ex);
			}
		}
	}
}
