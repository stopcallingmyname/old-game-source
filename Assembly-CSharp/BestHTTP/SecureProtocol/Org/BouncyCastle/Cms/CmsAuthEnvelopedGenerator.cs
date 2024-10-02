using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005E8 RID: 1512
	internal class CmsAuthEnvelopedGenerator
	{
		// Token: 0x040025CF RID: 9679
		public static readonly string Aes128Ccm = NistObjectIdentifiers.IdAes128Ccm.Id;

		// Token: 0x040025D0 RID: 9680
		public static readonly string Aes192Ccm = NistObjectIdentifiers.IdAes192Ccm.Id;

		// Token: 0x040025D1 RID: 9681
		public static readonly string Aes256Ccm = NistObjectIdentifiers.IdAes256Ccm.Id;

		// Token: 0x040025D2 RID: 9682
		public static readonly string Aes128Gcm = NistObjectIdentifiers.IdAes128Gcm.Id;

		// Token: 0x040025D3 RID: 9683
		public static readonly string Aes192Gcm = NistObjectIdentifiers.IdAes192Gcm.Id;

		// Token: 0x040025D4 RID: 9684
		public static readonly string Aes256Gcm = NistObjectIdentifiers.IdAes256Gcm.Id;
	}
}
