using System.IO;

namespace Counter
{
	internal abstract class ADocument : IDocument
	{
		public Stream Stream { get; set;}
		abstract public DocumentType Type { get; }

		protected ADocument(Stream stream)
		{
			Stream = stream;
		}

		public abstract uint Count();
	}
}
