using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000624 RID: 1572
	[Serializable]
	public class Asn1Exception : IOException
	{
		// Token: 0x06003B4E RID: 15182 RVA: 0x000B7FDE File Offset: 0x000B61DE
		public Asn1Exception()
		{
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x000B7FE6 File Offset: 0x000B61E6
		public Asn1Exception(string message) : base(message)
		{
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x000B7FEF File Offset: 0x000B61EF
		public Asn1Exception(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
