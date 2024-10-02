using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Iana
{
	// Token: 0x02000735 RID: 1845
	public abstract class IanaObjectIdentifiers
	{
		// Token: 0x04002BA8 RID: 11176
		public static readonly DerObjectIdentifier IsakmpOakley = new DerObjectIdentifier("1.3.6.1.5.5.8.1");

		// Token: 0x04002BA9 RID: 11177
		public static readonly DerObjectIdentifier HmacMD5 = new DerObjectIdentifier(IanaObjectIdentifiers.IsakmpOakley + ".1");

		// Token: 0x04002BAA RID: 11178
		public static readonly DerObjectIdentifier HmacSha1 = new DerObjectIdentifier(IanaObjectIdentifiers.IsakmpOakley + ".2");

		// Token: 0x04002BAB RID: 11179
		public static readonly DerObjectIdentifier HmacTiger = new DerObjectIdentifier(IanaObjectIdentifiers.IsakmpOakley + ".3");

		// Token: 0x04002BAC RID: 11180
		public static readonly DerObjectIdentifier HmacRipeMD160 = new DerObjectIdentifier(IanaObjectIdentifiers.IsakmpOakley + ".4");
	}
}
