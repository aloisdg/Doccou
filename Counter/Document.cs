using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Counter.Model.Documents;

namespace Counter
{
    public class Document
    {
	    public string Extension { get; private set; }
	    public string FullName { get; private set; }

	    public Document(string fullName)
	    {
		    FullName = fullName;
		    Extension = Path.GetExtension(fullName);
	    }

	    public uint Count()
	    {
		    if (Extension.Equals(".doc") || Extension.Equals(".docx"))
			    return new Doc(FullName).Count();
		    if (Extension.Equals(".pdf"))
			    return new Pdf(FullName).Count();
		    return 0;
	    }
    }
}
