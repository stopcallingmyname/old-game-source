using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x020004A5 RID: 1189
	public class PlainDsaEncoding : IDsaEncoding
	{
		// Token: 0x06002E99 RID: 11929 RVA: 0x00121E58 File Offset: 0x00120058
		public virtual BigInteger[] Decode(BigInteger n, byte[] encoding)
		{
			int unsignedByteLength = BigIntegers.GetUnsignedByteLength(n);
			if (encoding.Length != unsignedByteLength * 2)
			{
				throw new ArgumentException("Encoding has incorrect length", "encoding");
			}
			return new BigInteger[]
			{
				this.DecodeValue(n, encoding, 0, unsignedByteLength),
				this.DecodeValue(n, encoding, unsignedByteLength, unsignedByteLength)
			};
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x00121EA4 File Offset: 0x001200A4
		public virtual byte[] Encode(BigInteger n, BigInteger r, BigInteger s)
		{
			int unsignedByteLength = BigIntegers.GetUnsignedByteLength(n);
			byte[] array = new byte[unsignedByteLength * 2];
			this.EncodeValue(n, r, array, 0, unsignedByteLength);
			this.EncodeValue(n, s, array, unsignedByteLength, unsignedByteLength);
			return array;
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x00121ED8 File Offset: 0x001200D8
		protected virtual BigInteger CheckValue(BigInteger n, BigInteger x)
		{
			if (x.SignValue < 0 || x.CompareTo(n) >= 0)
			{
				throw new ArgumentException("Value out of range", "x");
			}
			return x;
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x00121EFE File Offset: 0x001200FE
		protected virtual BigInteger DecodeValue(BigInteger n, byte[] buf, int off, int len)
		{
			return this.CheckValue(n, new BigInteger(1, buf, off, len));
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x00121F14 File Offset: 0x00120114
		protected virtual void EncodeValue(BigInteger n, BigInteger x, byte[] buf, int off, int len)
		{
			byte[] array = this.CheckValue(n, x).ToByteArrayUnsigned();
			int num = Math.Max(0, array.Length - len);
			int num2 = array.Length - num;
			int num3 = len - num2;
			Arrays.Fill(buf, off, off + num3, 0);
			Array.Copy(array, num, buf, off + num3, num2);
		}

		// Token: 0x04001F2C RID: 7980
		public static readonly PlainDsaEncoding Instance = new PlainDsaEncoding();
	}
}
