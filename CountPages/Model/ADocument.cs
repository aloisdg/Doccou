namespace CountPages.Model
{
	public abstract class ADocument : IDocument
	{
		public string FullName { get; set;}
		abstract public DocumentType Type { get; }

		protected ADocument(string fullName)
		{
			FullName = fullName;
		}

		public abstract uint Count();
	}
}
