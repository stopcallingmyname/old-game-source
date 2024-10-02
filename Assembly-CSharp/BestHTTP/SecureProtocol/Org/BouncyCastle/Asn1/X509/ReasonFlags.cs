using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006AD RID: 1709
	public class ReasonFlags : DerBitString
	{
		// Token: 0x06003F0F RID: 16143 RVA: 0x0016F8CD File Offset: 0x0016DACD
		public ReasonFlags(int reasons) : base(reasons)
		{
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x00178A8D File Offset: 0x00176C8D
		public ReasonFlags(DerBitString reasons) : base(reasons.GetBytes(), reasons.PadBits)
		{
		}

		// Token: 0x04002806 RID: 10246
		public const int Unused = 128;

		// Token: 0x04002807 RID: 10247
		public const int KeyCompromise = 64;

		// Token: 0x04002808 RID: 10248
		public const int CACompromise = 32;

		// Token: 0x04002809 RID: 10249
		public const int AffiliationChanged = 16;

		// Token: 0x0400280A RID: 10250
		public const int Superseded = 8;

		// Token: 0x0400280B RID: 10251
		public const int CessationOfOperation = 4;

		// Token: 0x0400280C RID: 10252
		public const int CertificateHold = 2;

		// Token: 0x0400280D RID: 10253
		public const int PrivilegeWithdrawn = 1;

		// Token: 0x0400280E RID: 10254
		public const int AACompromise = 32768;
	}
}
