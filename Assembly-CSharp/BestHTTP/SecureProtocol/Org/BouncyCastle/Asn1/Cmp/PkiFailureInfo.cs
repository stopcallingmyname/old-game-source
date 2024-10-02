using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007BE RID: 1982
	public class PkiFailureInfo : DerBitString
	{
		// Token: 0x06004695 RID: 18069 RVA: 0x0016F8CD File Offset: 0x0016DACD
		public PkiFailureInfo(int info) : base(info)
		{
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x00178A8D File Offset: 0x00176C8D
		public PkiFailureInfo(DerBitString info) : base(info.GetBytes(), info.PadBits)
		{
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x00193CCC File Offset: 0x00191ECC
		public override string ToString()
		{
			return "PkiFailureInfo: 0x" + this.IntValue.ToString("X");
		}

		// Token: 0x04002E01 RID: 11777
		public const int BadAlg = 128;

		// Token: 0x04002E02 RID: 11778
		public const int BadMessageCheck = 64;

		// Token: 0x04002E03 RID: 11779
		public const int BadRequest = 32;

		// Token: 0x04002E04 RID: 11780
		public const int BadTime = 16;

		// Token: 0x04002E05 RID: 11781
		public const int BadCertId = 8;

		// Token: 0x04002E06 RID: 11782
		public const int BadDataFormat = 4;

		// Token: 0x04002E07 RID: 11783
		public const int WrongAuthority = 2;

		// Token: 0x04002E08 RID: 11784
		public const int IncorrectData = 1;

		// Token: 0x04002E09 RID: 11785
		public const int MissingTimeStamp = 32768;

		// Token: 0x04002E0A RID: 11786
		public const int BadPop = 16384;

		// Token: 0x04002E0B RID: 11787
		public const int CertRevoked = 8192;

		// Token: 0x04002E0C RID: 11788
		public const int CertConfirmed = 4096;

		// Token: 0x04002E0D RID: 11789
		public const int WrongIntegrity = 2048;

		// Token: 0x04002E0E RID: 11790
		public const int BadRecipientNonce = 1024;

		// Token: 0x04002E0F RID: 11791
		public const int TimeNotAvailable = 512;

		// Token: 0x04002E10 RID: 11792
		public const int UnacceptedPolicy = 256;

		// Token: 0x04002E11 RID: 11793
		public const int UnacceptedExtension = 8388608;

		// Token: 0x04002E12 RID: 11794
		public const int AddInfoNotAvailable = 4194304;

		// Token: 0x04002E13 RID: 11795
		public const int BadSenderNonce = 2097152;

		// Token: 0x04002E14 RID: 11796
		public const int BadCertTemplate = 1048576;

		// Token: 0x04002E15 RID: 11797
		public const int SignerNotTrusted = 524288;

		// Token: 0x04002E16 RID: 11798
		public const int TransactionIdInUse = 262144;

		// Token: 0x04002E17 RID: 11799
		public const int UnsupportedVersion = 131072;

		// Token: 0x04002E18 RID: 11800
		public const int NotAuthorized = 65536;

		// Token: 0x04002E19 RID: 11801
		public const int SystemUnavail = -2147483648;

		// Token: 0x04002E1A RID: 11802
		public const int SystemFailure = 1073741824;

		// Token: 0x04002E1B RID: 11803
		public const int DuplicateCertReq = 536870912;
	}
}
