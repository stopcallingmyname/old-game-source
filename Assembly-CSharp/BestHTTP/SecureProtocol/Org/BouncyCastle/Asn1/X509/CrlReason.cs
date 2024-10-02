using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000695 RID: 1685
	public class CrlReason : DerEnumerated
	{
		// Token: 0x06003E56 RID: 15958 RVA: 0x00176A79 File Offset: 0x00174C79
		public CrlReason(int reason) : base(reason)
		{
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x00176A82 File Offset: 0x00174C82
		public CrlReason(DerEnumerated reason) : base(reason.Value.IntValue)
		{
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x00176A98 File Offset: 0x00174C98
		public override string ToString()
		{
			int intValue = base.Value.IntValue;
			string str = (intValue < 0 || intValue > 10) ? "Invalid" : CrlReason.ReasonString[intValue];
			return "CrlReason: " + str;
		}

		// Token: 0x04002798 RID: 10136
		public const int Unspecified = 0;

		// Token: 0x04002799 RID: 10137
		public const int KeyCompromise = 1;

		// Token: 0x0400279A RID: 10138
		public const int CACompromise = 2;

		// Token: 0x0400279B RID: 10139
		public const int AffiliationChanged = 3;

		// Token: 0x0400279C RID: 10140
		public const int Superseded = 4;

		// Token: 0x0400279D RID: 10141
		public const int CessationOfOperation = 5;

		// Token: 0x0400279E RID: 10142
		public const int CertificateHold = 6;

		// Token: 0x0400279F RID: 10143
		public const int RemoveFromCrl = 8;

		// Token: 0x040027A0 RID: 10144
		public const int PrivilegeWithdrawn = 9;

		// Token: 0x040027A1 RID: 10145
		public const int AACompromise = 10;

		// Token: 0x040027A2 RID: 10146
		private static readonly string[] ReasonString = new string[]
		{
			"Unspecified",
			"KeyCompromise",
			"CACompromise",
			"AffiliationChanged",
			"Superseded",
			"CessationOfOperation",
			"CertificateHold",
			"Unknown",
			"RemoveFromCrl",
			"PrivilegeWithdrawn",
			"AACompromise"
		};
	}
}
