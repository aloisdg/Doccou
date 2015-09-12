namespace Doccou.Pcl
{
	internal interface IDocument
	{
		DocumentType	Type { get; }
		uint		Count { get; }
	}
}