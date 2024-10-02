using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000779 RID: 1913
	public class SubsequentMessage : DerInteger
	{
		// Token: 0x0600449D RID: 17565 RVA: 0x0018E291 File Offset: 0x0018C491
		private SubsequentMessage(int value) : base(value)
		{
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x0018E29A File Offset: 0x0018C49A
		public static SubsequentMessage ValueOf(int value)
		{
			if (value == 0)
			{
				return SubsequentMessage.encrCert;
			}
			if (value == 1)
			{
				return SubsequentMessage.challengeResp;
			}
			throw new ArgumentException("unknown value: " + value, "value");
		}

		// Token: 0x04002CF9 RID: 11513
		public static readonly SubsequentMessage encrCert = new SubsequentMessage(0);

		// Token: 0x04002CFA RID: 11514
		public static readonly SubsequentMessage challengeResp = new SubsequentMessage(1);
	}
}
