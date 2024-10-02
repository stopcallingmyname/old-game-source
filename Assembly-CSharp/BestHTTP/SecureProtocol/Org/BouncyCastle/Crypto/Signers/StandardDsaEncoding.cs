using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x020004AA RID: 1194
	public class StandardDsaEncoding : IDsaEncoding
	{
		// Token: 0x06002ED7 RID: 11991 RVA: 0x00123074 File Offset: 0x00121274
		public virtual BigInteger[] Decode(BigInteger n, byte[] encoding)
		{
			Asn1Sequence asn1Sequence = (Asn1Sequence)Asn1Object.FromByteArray(encoding);
			if (asn1Sequence.Count == 2)
			{
				BigInteger bigInteger = this.DecodeValue(n, asn1Sequence, 0);
				BigInteger bigInteger2 = this.DecodeValue(n, asn1Sequence, 1);
				if (Arrays.AreEqual(this.Encode(n, bigInteger, bigInteger2), encoding))
				{
					return new BigInteger[]
					{
						bigInteger,
						bigInteger2
					};
				}
			}
			throw new ArgumentException("Malformed signature", "encoding");
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x001230D9 File Offset: 0x001212D9
		public virtual byte[] Encode(BigInteger n, BigInteger r, BigInteger s)
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.EncodeValue(n, r),
				this.EncodeValue(n, s)
			}).GetEncoded("DER");
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x00123106 File Offset: 0x00121306
		protected virtual BigInteger CheckValue(BigInteger n, BigInteger x)
		{
			if (x.SignValue < 0 || (n != null && x.CompareTo(n) >= 0))
			{
				throw new ArgumentException("Value out of range", "x");
			}
			return x;
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x0012312F File Offset: 0x0012132F
		protected virtual BigInteger DecodeValue(BigInteger n, Asn1Sequence s, int pos)
		{
			return this.CheckValue(n, ((DerInteger)s[pos]).Value);
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x00123149 File Offset: 0x00121349
		protected virtual DerInteger EncodeValue(BigInteger n, BigInteger x)
		{
			return new DerInteger(this.CheckValue(n, x));
		}

		// Token: 0x04001F4A RID: 8010
		public static readonly StandardDsaEncoding Instance = new StandardDsaEncoding();
	}
}
