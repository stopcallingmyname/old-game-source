using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005F6 RID: 1526
	public interface CmsProcessable
	{
		// Token: 0x060039F9 RID: 14841
		void Write(Stream outStream);

		// Token: 0x060039FA RID: 14842
		[Obsolete]
		object GetContent();
	}
}
