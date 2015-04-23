using System.IO;

namespace Counter
{
	internal interface IDocument
	{
		Stream Stream { get; set; }
		DocumentType Type { get; }

		uint Count();
	}
}
