using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Gnu
{
	// Token: 0x02000736 RID: 1846
	public abstract class GnuObjectIdentifiers
	{
		// Token: 0x04002BAD RID: 11181
		public static readonly DerObjectIdentifier Gnu = new DerObjectIdentifier("1.3.6.1.4.1.11591.1");

		// Token: 0x04002BAE RID: 11182
		public static readonly DerObjectIdentifier GnuPG = new DerObjectIdentifier("1.3.6.1.4.1.11591.2");

		// Token: 0x04002BAF RID: 11183
		public static readonly DerObjectIdentifier Notation = new DerObjectIdentifier("1.3.6.1.4.1.11591.2.1");

		// Token: 0x04002BB0 RID: 11184
		public static readonly DerObjectIdentifier PkaAddress = new DerObjectIdentifier("1.3.6.1.4.1.11591.2.1.1");

		// Token: 0x04002BB1 RID: 11185
		public static readonly DerObjectIdentifier GnuRadar = new DerObjectIdentifier("1.3.6.1.4.1.11591.3");

		// Token: 0x04002BB2 RID: 11186
		public static readonly DerObjectIdentifier DigestAlgorithm = new DerObjectIdentifier("1.3.6.1.4.1.11591.12");

		// Token: 0x04002BB3 RID: 11187
		public static readonly DerObjectIdentifier Tiger192 = new DerObjectIdentifier("1.3.6.1.4.1.11591.12.2");

		// Token: 0x04002BB4 RID: 11188
		public static readonly DerObjectIdentifier EncryptionAlgorithm = new DerObjectIdentifier("1.3.6.1.4.1.11591.13");

		// Token: 0x04002BB5 RID: 11189
		public static readonly DerObjectIdentifier Serpent = new DerObjectIdentifier("1.3.6.1.4.1.11591.13.2");

		// Token: 0x04002BB6 RID: 11190
		public static readonly DerObjectIdentifier Serpent128Ecb = new DerObjectIdentifier("1.3.6.1.4.1.11591.13.2.1");

		// Token: 0x04002BB7 RID: 11191
		public static readonly DerObjectIdentifier Serpent128Cbc = new DerObjectIdentifier("1.3.6.1.4.1.11591.13.2.2");

		// Token: 0x04002BB8 RID: 11192
		public static readonly DerObjectIdentifier Serpent128Ofb = new DerObjectIdentifier("1.3.6.1.4.1.11591.13.2.3");

		// Token: 0x04002BB9 RID: 11193
		public static readonly DerObjectIdentifier Serpent128Cfb = new DerObjectIdentifier("1.3.6.1.4.1.11591.13.2.4");

		// Token: 0x04002BBA RID: 11194
		public static readonly DerObjectIdentifier Serpent192Ecb = new DerObjectIdentifier("1.3.6.1.4.1.11591.13.2.21");

		// Token: 0x04002BBB RID: 11195
		public static readonly DerObjectIdentifier Serpent192Cbc = new DerObjectIdentifier("1.3.6.1.4.1.11591.13.2.22");

		// Token: 0x04002BBC RID: 11196
		public static readonly DerObjectIdentifier Serpent192Ofb = new DerObjectIdentifier("1.3.6.1.4.1.11591.13.2.23");

		// Token: 0x04002BBD RID: 11197
		public static readonly DerObjectIdentifier Serpent192Cfb = new DerObjectIdentifier("1.3.6.1.4.1.11591.13.2.24");

		// Token: 0x04002BBE RID: 11198
		public static readonly DerObjectIdentifier Serpent256Ecb = new DerObjectIdentifier("1.3.6.1.4.1.11591.13.2.41");

		// Token: 0x04002BBF RID: 11199
		public static readonly DerObjectIdentifier Serpent256Cbc = new DerObjectIdentifier("1.3.6.1.4.1.11591.13.2.42");

		// Token: 0x04002BC0 RID: 11200
		public static readonly DerObjectIdentifier Serpent256Ofb = new DerObjectIdentifier("1.3.6.1.4.1.11591.13.2.43");

		// Token: 0x04002BC1 RID: 11201
		public static readonly DerObjectIdentifier Serpent256Cfb = new DerObjectIdentifier("1.3.6.1.4.1.11591.13.2.44");

		// Token: 0x04002BC2 RID: 11202
		public static readonly DerObjectIdentifier Crc = new DerObjectIdentifier("1.3.6.1.4.1.11591.14");

		// Token: 0x04002BC3 RID: 11203
		public static readonly DerObjectIdentifier Crc32 = new DerObjectIdentifier("1.3.6.1.4.1.11591.14.1");

		// Token: 0x04002BC4 RID: 11204
		public static readonly DerObjectIdentifier EllipticCurve = new DerObjectIdentifier("1.3.6.1.4.1.11591.15");

		// Token: 0x04002BC5 RID: 11205
		public static readonly DerObjectIdentifier Ed25519 = GnuObjectIdentifiers.EllipticCurve.Branch("1");
	}
}
