using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005C0 RID: 1472
	public class SM3Digest : GeneralDigest
	{
		// Token: 0x06003888 RID: 14472 RVA: 0x00162320 File Offset: 0x00160520
		static SM3Digest()
		{
			for (int i = 0; i < 16; i++)
			{
				uint num = 2043430169U;
				SM3Digest.T[i] = (num << i | num >> 32 - i);
			}
			for (int j = 16; j < 64; j++)
			{
				int num2 = j % 32;
				uint num3 = 2055708042U;
				SM3Digest.T[j] = (num3 << num2 | num3 >> 32 - num2);
			}
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x00162396 File Offset: 0x00160596
		public SM3Digest()
		{
			this.Reset();
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x001623CA File Offset: 0x001605CA
		public SM3Digest(SM3Digest t) : base(t)
		{
			this.CopyIn(t);
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x00162400 File Offset: 0x00160600
		private void CopyIn(SM3Digest t)
		{
			Array.Copy(t.V, 0, this.V, 0, this.V.Length);
			Array.Copy(t.inwords, 0, this.inwords, 0, this.inwords.Length);
			this.xOff = t.xOff;
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x0600388C RID: 14476 RVA: 0x0016244F File Offset: 0x0016064F
		public override string AlgorithmName
		{
			get
			{
				return "SM3";
			}
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x00154E34 File Offset: 0x00153034
		public override int GetDigestSize()
		{
			return 32;
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x00162456 File Offset: 0x00160656
		public override IMemoable Copy()
		{
			return new SM3Digest(this);
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x00162460 File Offset: 0x00160660
		public override void Reset(IMemoable other)
		{
			SM3Digest t = (SM3Digest)other;
			base.CopyIn(t);
			this.CopyIn(t);
		}

		// Token: 0x06003890 RID: 14480 RVA: 0x00162484 File Offset: 0x00160684
		public override void Reset()
		{
			base.Reset();
			this.V[0] = 1937774191U;
			this.V[1] = 1226093241U;
			this.V[2] = 388252375U;
			this.V[3] = 3666478592U;
			this.V[4] = 2842636476U;
			this.V[5] = 372324522U;
			this.V[6] = 3817729613U;
			this.V[7] = 2969243214U;
			this.xOff = 0;
		}

		// Token: 0x06003891 RID: 14481 RVA: 0x00162506 File Offset: 0x00160706
		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			Pack.UInt32_To_BE(this.V, output, outOff);
			this.Reset();
			return 32;
		}

		// Token: 0x06003892 RID: 14482 RVA: 0x00162524 File Offset: 0x00160724
		internal override void ProcessWord(byte[] input, int inOff)
		{
			uint num = Pack.BE_To_UInt32(input, inOff);
			this.inwords[this.xOff] = num;
			this.xOff++;
			if (this.xOff >= 16)
			{
				this.ProcessBlock();
			}
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x00162568 File Offset: 0x00160768
		internal override void ProcessLength(long bitLength)
		{
			if (this.xOff > 14)
			{
				this.inwords[this.xOff] = 0U;
				this.xOff++;
				this.ProcessBlock();
			}
			while (this.xOff < 14)
			{
				this.inwords[this.xOff] = 0U;
				this.xOff++;
			}
			uint[] array = this.inwords;
			int num = this.xOff;
			this.xOff = num + 1;
			array[num] = (uint)(bitLength >> 32);
			uint[] array2 = this.inwords;
			num = this.xOff;
			this.xOff = num + 1;
			array2[num] = (uint)bitLength;
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x00162600 File Offset: 0x00160800
		private uint P0(uint x)
		{
			uint num = x << 9 | x >> 23;
			uint num2 = x << 17 | x >> 15;
			return x ^ num ^ num2;
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x00162628 File Offset: 0x00160828
		private uint P1(uint x)
		{
			uint num = x << 15 | x >> 17;
			uint num2 = x << 23 | x >> 9;
			return x ^ num ^ num2;
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x0015812D File Offset: 0x0015632D
		private uint FF0(uint x, uint y, uint z)
		{
			return x ^ y ^ z;
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x00158120 File Offset: 0x00156320
		private uint FF1(uint x, uint y, uint z)
		{
			return (x & y) | (x & z) | (y & z);
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x0015812D File Offset: 0x0015632D
		private uint GG0(uint x, uint y, uint z)
		{
			return x ^ y ^ z;
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x00158116 File Offset: 0x00156316
		private uint GG1(uint x, uint y, uint z)
		{
			return (x & y) | (~x & z);
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x00162650 File Offset: 0x00160850
		internal override void ProcessBlock()
		{
			for (int i = 0; i < 16; i++)
			{
				this.W[i] = this.inwords[i];
			}
			for (int j = 16; j < 68; j++)
			{
				uint num = this.W[j - 3];
				uint num2 = num << 15 | num >> 17;
				uint num3 = this.W[j - 13];
				uint num4 = num3 << 7 | num3 >> 25;
				this.W[j] = (this.P1(this.W[j - 16] ^ this.W[j - 9] ^ num2) ^ num4 ^ this.W[j - 6]);
			}
			uint num5 = this.V[0];
			uint num6 = this.V[1];
			uint num7 = this.V[2];
			uint num8 = this.V[3];
			uint num9 = this.V[4];
			uint num10 = this.V[5];
			uint num11 = this.V[6];
			uint num12 = this.V[7];
			for (int k = 0; k < 16; k++)
			{
				uint num13 = num5 << 12 | num5 >> 20;
				uint num14 = num13 + num9 + SM3Digest.T[k];
				uint num15 = num14 << 7 | num14 >> 25;
				uint num16 = num15 ^ num13;
				uint num17 = this.W[k];
				uint num18 = num17 ^ this.W[k + 4];
				uint num19 = this.FF0(num5, num6, num7) + num8 + num16 + num18;
				uint x = this.GG0(num9, num10, num11) + num12 + num15 + num17;
				num8 = num7;
				num7 = (num6 << 9 | num6 >> 23);
				num6 = num5;
				num5 = num19;
				num12 = num11;
				num11 = (num10 << 19 | num10 >> 13);
				num10 = num9;
				num9 = this.P0(x);
			}
			for (int l = 16; l < 64; l++)
			{
				uint num20 = num5 << 12 | num5 >> 20;
				uint num21 = num20 + num9 + SM3Digest.T[l];
				uint num22 = num21 << 7 | num21 >> 25;
				uint num23 = num22 ^ num20;
				uint num24 = this.W[l];
				uint num25 = num24 ^ this.W[l + 4];
				uint num26 = this.FF1(num5, num6, num7) + num8 + num23 + num25;
				uint x2 = this.GG1(num9, num10, num11) + num12 + num22 + num24;
				num8 = num7;
				num7 = (num6 << 9 | num6 >> 23);
				num6 = num5;
				num5 = num26;
				num12 = num11;
				num11 = (num10 << 19 | num10 >> 13);
				num10 = num9;
				num9 = this.P0(x2);
			}
			this.V[0] ^= num5;
			this.V[1] ^= num6;
			this.V[2] ^= num7;
			this.V[3] ^= num8;
			this.V[4] ^= num9;
			this.V[5] ^= num10;
			this.V[6] ^= num11;
			this.V[7] ^= num12;
			this.xOff = 0;
		}

		// Token: 0x040024CE RID: 9422
		private const int DIGEST_LENGTH = 32;

		// Token: 0x040024CF RID: 9423
		private const int BLOCK_SIZE = 16;

		// Token: 0x040024D0 RID: 9424
		private uint[] V = new uint[8];

		// Token: 0x040024D1 RID: 9425
		private uint[] inwords = new uint[16];

		// Token: 0x040024D2 RID: 9426
		private int xOff;

		// Token: 0x040024D3 RID: 9427
		private uint[] W = new uint[68];

		// Token: 0x040024D4 RID: 9428
		private static readonly uint[] T = new uint[64];
	}
}
