using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005B9 RID: 1465
	public class Sha3Digest : KeccakDigest
	{
		// Token: 0x06003837 RID: 14391 RVA: 0x001611BC File Offset: 0x0015F3BC
		private static int CheckBitLength(int bitLength)
		{
			if (bitLength <= 256)
			{
				if (bitLength != 224 && bitLength != 256)
				{
					goto IL_2C;
				}
			}
			else if (bitLength != 384 && bitLength != 512)
			{
				goto IL_2C;
			}
			return bitLength;
			IL_2C:
			throw new ArgumentException(bitLength + " not supported for SHA-3", "bitLength");
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x0016120F File Offset: 0x0015F40F
		public Sha3Digest() : this(256)
		{
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x0016121C File Offset: 0x0015F41C
		public Sha3Digest(int bitLength) : base(Sha3Digest.CheckBitLength(bitLength))
		{
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x0016122A File Offset: 0x0015F42A
		public Sha3Digest(Sha3Digest source) : base(source)
		{
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x0600383B RID: 14395 RVA: 0x00161233 File Offset: 0x0015F433
		public override string AlgorithmName
		{
			get
			{
				return "SHA3-" + this.fixedOutputLength;
			}
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x0016124A File Offset: 0x0015F44A
		public override int DoFinal(byte[] output, int outOff)
		{
			base.AbsorbBits(2, 2);
			return base.DoFinal(output, outOff);
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x0016125C File Offset: 0x0015F45C
		protected override int DoFinal(byte[] output, int outOff, byte partialByte, int partialBits)
		{
			if (partialBits < 0 || partialBits > 7)
			{
				throw new ArgumentException("must be in the range [0,7]", "partialBits");
			}
			int num = ((int)partialByte & (1 << partialBits) - 1) | 2 << partialBits;
			int num2 = partialBits + 2;
			if (num2 >= 8)
			{
				base.Absorb(new byte[]
				{
					(byte)num
				}, 0, 1);
				num2 -= 8;
				num >>= 8;
			}
			return base.DoFinal(output, outOff, (byte)num, num2);
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x001612C6 File Offset: 0x0015F4C6
		public override IMemoable Copy()
		{
			return new Sha3Digest(this);
		}
	}
}
