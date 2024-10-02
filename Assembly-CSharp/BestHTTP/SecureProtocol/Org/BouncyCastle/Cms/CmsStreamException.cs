using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000603 RID: 1539
	[Serializable]
	public class CmsStreamException : IOException
	{
		// Token: 0x06003A8E RID: 14990 RVA: 0x000B7FDE File Offset: 0x000B61DE
		public CmsStreamException()
		{
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x000B7FE6 File Offset: 0x000B61E6
		public CmsStreamException(string name) : base(name)
		{
		}

		// Token: 0x06003A90 RID: 14992 RVA: 0x000B7FEF File Offset: 0x000B61EF
		public CmsStreamException(string name, Exception e) : base(name, e)
		{
		}
	}
}
