using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doccou.Pcl.Documents
{
	internal sealed class Unknow : ADocument
	{
		public override DocumentType Type { get { return DocumentType.Unknow; }}
		public override uint Count { get; protected set; }

		public Unknow()
		{
			Count = 0;
		}
	}
}
