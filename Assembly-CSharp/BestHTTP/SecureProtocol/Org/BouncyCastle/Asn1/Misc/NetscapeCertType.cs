using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Misc
{
	// Token: 0x0200071F RID: 1823
	public class NetscapeCertType : DerBitString
	{
		// Token: 0x0600425E RID: 16990 RVA: 0x0016F8CD File Offset: 0x0016DACD
		public NetscapeCertType(int usage) : base(usage)
		{
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x00178A8D File Offset: 0x00176C8D
		public NetscapeCertType(DerBitString usage) : base(usage.GetBytes(), usage.PadBits)
		{
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x00186004 File Offset: 0x00184204
		public override string ToString()
		{
			byte[] bytes = this.GetBytes();
			return "NetscapeCertType: 0x" + ((int)(bytes[0] & byte.MaxValue)).ToString("X");
		}

		// Token: 0x04002B3E RID: 11070
		public const int SslClient = 128;

		// Token: 0x04002B3F RID: 11071
		public const int SslServer = 64;

		// Token: 0x04002B40 RID: 11072
		public const int Smime = 32;

		// Token: 0x04002B41 RID: 11073
		public const int ObjectSigning = 16;

		// Token: 0x04002B42 RID: 11074
		public const int Reserved = 8;

		// Token: 0x04002B43 RID: 11075
		public const int SslCA = 4;

		// Token: 0x04002B44 RID: 11076
		public const int SmimeCA = 2;

		// Token: 0x04002B45 RID: 11077
		public const int ObjectSigningCA = 1;
	}
}
