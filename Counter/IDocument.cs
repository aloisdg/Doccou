using System;
using System.IO;

namespace Counter
{
	internal interface IDocument
	{
		DocumentType Type { get; }
		uint Count { get; }
	}
}
