using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Misc
{
	// Token: 0x02000721 RID: 1825
	public class VerisignCzagExtension : DerIA5String
	{
		// Token: 0x06004263 RID: 16995 RVA: 0x00186038 File Offset: 0x00184238
		public VerisignCzagExtension(DerIA5String str) : base(str.GetString())
		{
		}

		// Token: 0x06004264 RID: 16996 RVA: 0x00186058 File Offset: 0x00184258
		public override string ToString()
		{
			return "VerisignCzagExtension: " + this.GetString();
		}
	}
}
