using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Misc
{
	// Token: 0x02000720 RID: 1824
	public class NetscapeRevocationUrl : DerIA5String
	{
		// Token: 0x06004261 RID: 16993 RVA: 0x00186038 File Offset: 0x00184238
		public NetscapeRevocationUrl(DerIA5String str) : base(str.GetString())
		{
		}

		// Token: 0x06004262 RID: 16994 RVA: 0x00186046 File Offset: 0x00184246
		public override string ToString()
		{
			return "NetscapeRevocationUrl: " + this.GetString();
		}
	}
}
