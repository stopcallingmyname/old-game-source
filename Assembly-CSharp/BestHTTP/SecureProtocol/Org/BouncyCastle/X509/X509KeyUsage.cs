using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x02000249 RID: 585
	public class X509KeyUsage : Asn1Encodable
	{
		// Token: 0x0600152D RID: 5421 RVA: 0x000AD54A File Offset: 0x000AB74A
		public X509KeyUsage(int usage)
		{
			this.usage = usage;
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x000AD559 File Offset: 0x000AB759
		public override Asn1Object ToAsn1Object()
		{
			return new KeyUsage(this.usage);
		}

		// Token: 0x04001639 RID: 5689
		public const int DigitalSignature = 128;

		// Token: 0x0400163A RID: 5690
		public const int NonRepudiation = 64;

		// Token: 0x0400163B RID: 5691
		public const int KeyEncipherment = 32;

		// Token: 0x0400163C RID: 5692
		public const int DataEncipherment = 16;

		// Token: 0x0400163D RID: 5693
		public const int KeyAgreement = 8;

		// Token: 0x0400163E RID: 5694
		public const int KeyCertSign = 4;

		// Token: 0x0400163F RID: 5695
		public const int CrlSign = 2;

		// Token: 0x04001640 RID: 5696
		public const int EncipherOnly = 1;

		// Token: 0x04001641 RID: 5697
		public const int DecipherOnly = 32768;

		// Token: 0x04001642 RID: 5698
		private readonly int usage;
	}
}
