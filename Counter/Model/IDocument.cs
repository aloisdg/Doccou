namespace Counter.Model
{
	internal interface IDocument
	{
		string FullName { get; set; }
		DocumentType Type { get; }

		uint Count();
	}
}
