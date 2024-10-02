using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005BC RID: 1468
	public class ShakeDigest : KeccakDigest, IXof, IDigest
	{
		// Token: 0x06003852 RID: 14418 RVA: 0x001618B4 File Offset: 0x0015FAB4
		private static int CheckBitLength(int bitLength)
		{
			if (bitLength == 128 || bitLength == 256)
			{
				return bitLength;
			}
			throw new ArgumentException(bitLength + " not supported for SHAKE", "bitLength");
		}

		// Token: 0x06003853 RID: 14419 RVA: 0x001618E2 File Offset: 0x0015FAE2
		public ShakeDigest() : this(128)
		{
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x001618EF File Offset: 0x0015FAEF
		public ShakeDigest(int bitLength) : base(ShakeDigest.CheckBitLength(bitLength))
		{
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x0016122A File Offset: 0x0015F42A
		public ShakeDigest(ShakeDigest source) : base(source)
		{
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06003856 RID: 14422 RVA: 0x001618FD File Offset: 0x0015FAFD
		public override string AlgorithmName
		{
			get
			{
				return "SHAKE" + this.fixedOutputLength;
			}
		}

		// Token: 0x06003857 RID: 14423 RVA: 0x00161914 File Offset: 0x0015FB14
		public override int DoFinal(byte[] output, int outOff)
		{
			return this.DoFinal(output, outOff, this.GetDigestSize());
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x00161924 File Offset: 0x0015FB24
		public virtual int DoFinal(byte[] output, int outOff, int outLen)
		{
			this.DoOutput(output, outOff, outLen);
			this.Reset();
			return outLen;
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x00161937 File Offset: 0x0015FB37
		public virtual int DoOutput(byte[] output, int outOff, int outLen)
		{
			if (!this.squeezing)
			{
				base.AbsorbBits(15, 4);
			}
			base.Squeeze(output, outOff, (long)outLen << 3);
			return outLen;
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x00161957 File Offset: 0x0015FB57
		protected override int DoFinal(byte[] output, int outOff, byte partialByte, int partialBits)
		{
			return this.DoFinal(output, outOff, this.GetDigestSize(), partialByte, partialBits);
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x0016196C File Offset: 0x0015FB6C
		protected virtual int DoFinal(byte[] output, int outOff, int outLen, byte partialByte, int partialBits)
		{
			if (partialBits < 0 || partialBits > 7)
			{
				throw new ArgumentException("must be in the range [0,7]", "partialBits");
			}
			int num = ((int)partialByte & (1 << partialBits) - 1) | 15 << partialBits;
			int num2 = partialBits + 4;
			if (num2 >= 8)
			{
				base.Absorb(new byte[]
				{
					(byte)num
				}, 0, 1);
				num2 -= 8;
				num >>= 8;
			}
			if (num2 > 0)
			{
				base.AbsorbBits(num, num2);
			}
			base.Squeeze(output, outOff, (long)outLen << 3);
			this.Reset();
			return outLen;
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x001619EC File Offset: 0x0015FBEC
		public override IMemoable Copy()
		{
			return new ShakeDigest(this);
		}
	}
}
