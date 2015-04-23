using System;
using System.Reflection;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;

namespace CountPages.Model.Documents
{
	public class Doc : ADocument
	{
		public override DocumentType Type { get { return DocumentType.Pdf; } }

		public Doc(string fullName) : base(fullName) { }

		public override uint Count()
		{
			const WdStatistic stat = WdStatistic.wdStatisticPages;
			_Application wordApp = new Application();

			object fileName = this.FullName;
			object readOnly = false;
			object isVisible = true;

			//  the way to handle parameters you don't care about in .NET
			object missing = Missing.Value;

			//   Make word visible, so you can see what's happening
			//wordApp.Visible = true;
			//   Open the document that was chosen by the dialog
			var aDoc = wordApp.Documents.Open(ref fileName,
						ref missing, ref readOnly, ref missing,
						ref missing, ref missing, ref missing,
						ref missing, ref missing, ref missing,
						ref missing, ref isVisible);

			return Convert.ToUInt32(aDoc.ComputeStatistics(stat, ref missing));
		}
	}
}
