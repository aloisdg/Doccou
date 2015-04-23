namespace CountPages.Model
{
	public interface IDocument
	{
		string FullName { get; set; }
		DocumentType Type { get; }

		uint Count();
	}
}
