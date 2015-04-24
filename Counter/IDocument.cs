namespace Counter
{
	internal interface IDocument
	{
		DocumentType Type { get; }
		uint Count { get; }
	}
}