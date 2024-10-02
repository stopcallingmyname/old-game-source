using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005F4 RID: 1524
	[Serializable]
	public class CmsException : Exception
	{
		// Token: 0x060039E9 RID: 14825 RVA: 0x0008BF1D File Offset: 0x0008A11D
		public CmsException()
		{
		}

		// Token: 0x060039EA RID: 14826 RVA: 0x0008BF89 File Offset: 0x0008A189
		public CmsException(string msg) : base(msg)
		{
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x0008BF92 File Offset: 0x0008A192
		public CmsException(string msg, Exception e) : base(msg, e)
		{
		}
	}
}
