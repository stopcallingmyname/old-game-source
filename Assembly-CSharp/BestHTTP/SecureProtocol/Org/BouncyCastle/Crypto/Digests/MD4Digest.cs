using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005AD RID: 1453
	public class MD4Digest : GeneralDigest
	{
		// Token: 0x0600376E RID: 14190 RVA: 0x00157EEF File Offset: 0x001560EF
		public MD4Digest()
		{
			this.Reset();
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x00157F0A File Offset: 0x0015610A
		public MD4Digest(MD4Digest t) : base(t)
		{
			this.CopyIn(t);
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x00157F28 File Offset: 0x00156128
		private void CopyIn(MD4Digest t)
		{
			base.CopyIn(t);
			this.H1 = t.H1;
			this.H2 = t.H2;
			this.H3 = t.H3;
			this.H4 = t.H4;
			Array.Copy(t.X, 0, this.X, 0, t.X.Length);
			this.xOff = t.xOff;
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06003771 RID: 14193 RVA: 0x00157F93 File Offset: 0x00156193
		public override string AlgorithmName
		{
			get
			{
				return "MD4";
			}
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x0012AD29 File Offset: 0x00128F29
		public override int GetDigestSize()
		{
			return 16;
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x00157F9C File Offset: 0x0015619C
		internal override void ProcessWord(byte[] input, int inOff)
		{
			int[] x = this.X;
			int num = this.xOff;
			this.xOff = num + 1;
			x[num] = ((int)(input[inOff] & byte.MaxValue) | (int)(input[inOff + 1] & byte.MaxValue) << 8 | (int)(input[inOff + 2] & byte.MaxValue) << 16 | (int)(input[inOff + 3] & byte.MaxValue) << 24);
			if (this.xOff == 16)
			{
				this.ProcessBlock();
			}
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x00158006 File Offset: 0x00156206
		internal override void ProcessLength(long bitLength)
		{
			if (this.xOff > 14)
			{
				this.ProcessBlock();
			}
			this.X[14] = (int)(bitLength & (long)((ulong)-1));
			this.X[15] = (int)((ulong)bitLength >> 32);
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x00158034 File Offset: 0x00156234
		private void UnpackWord(int word, byte[] outBytes, int outOff)
		{
			outBytes[outOff] = (byte)word;
			outBytes[outOff + 1] = (byte)((uint)word >> 8);
			outBytes[outOff + 2] = (byte)((uint)word >> 16);
			outBytes[outOff + 3] = (byte)((uint)word >> 24);
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x00158058 File Offset: 0x00156258
		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			this.UnpackWord(this.H1, output, outOff);
			this.UnpackWord(this.H2, output, outOff + 4);
			this.UnpackWord(this.H3, output, outOff + 8);
			this.UnpackWord(this.H4, output, outOff + 12);
			this.Reset();
			return 16;
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x001580B4 File Offset: 0x001562B4
		public override void Reset()
		{
			base.Reset();
			this.H1 = 1732584193;
			this.H2 = -271733879;
			this.H3 = -1732584194;
			this.H4 = 271733878;
			this.xOff = 0;
			for (int num = 0; num != this.X.Length; num++)
			{
				this.X[num] = 0;
			}
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x00144EAA File Offset: 0x001430AA
		private int RotateLeft(int x, int n)
		{
			return x << n | (int)((uint)x >> 32 - n);
		}

		// Token: 0x06003779 RID: 14201 RVA: 0x00158116 File Offset: 0x00156316
		private int F(int u, int v, int w)
		{
			return (u & v) | (~u & w);
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x00158120 File Offset: 0x00156320
		private int G(int u, int v, int w)
		{
			return (u & v) | (u & w) | (v & w);
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x0015812D File Offset: 0x0015632D
		private int H(int u, int v, int w)
		{
			return u ^ v ^ w;
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x00158134 File Offset: 0x00156334
		internal override void ProcessBlock()
		{
			int num = this.H1;
			int num2 = this.H2;
			int num3 = this.H3;
			int num4 = this.H4;
			num = this.RotateLeft(num + this.F(num2, num3, num4) + this.X[0], 3);
			num4 = this.RotateLeft(num4 + this.F(num, num2, num3) + this.X[1], 7);
			num3 = this.RotateLeft(num3 + this.F(num4, num, num2) + this.X[2], 11);
			num2 = this.RotateLeft(num2 + this.F(num3, num4, num) + this.X[3], 19);
			num = this.RotateLeft(num + this.F(num2, num3, num4) + this.X[4], 3);
			num4 = this.RotateLeft(num4 + this.F(num, num2, num3) + this.X[5], 7);
			num3 = this.RotateLeft(num3 + this.F(num4, num, num2) + this.X[6], 11);
			num2 = this.RotateLeft(num2 + this.F(num3, num4, num) + this.X[7], 19);
			num = this.RotateLeft(num + this.F(num2, num3, num4) + this.X[8], 3);
			num4 = this.RotateLeft(num4 + this.F(num, num2, num3) + this.X[9], 7);
			num3 = this.RotateLeft(num3 + this.F(num4, num, num2) + this.X[10], 11);
			num2 = this.RotateLeft(num2 + this.F(num3, num4, num) + this.X[11], 19);
			num = this.RotateLeft(num + this.F(num2, num3, num4) + this.X[12], 3);
			num4 = this.RotateLeft(num4 + this.F(num, num2, num3) + this.X[13], 7);
			num3 = this.RotateLeft(num3 + this.F(num4, num, num2) + this.X[14], 11);
			num2 = this.RotateLeft(num2 + this.F(num3, num4, num) + this.X[15], 19);
			num = this.RotateLeft(num + this.G(num2, num3, num4) + this.X[0] + 1518500249, 3);
			num4 = this.RotateLeft(num4 + this.G(num, num2, num3) + this.X[4] + 1518500249, 5);
			num3 = this.RotateLeft(num3 + this.G(num4, num, num2) + this.X[8] + 1518500249, 9);
			num2 = this.RotateLeft(num2 + this.G(num3, num4, num) + this.X[12] + 1518500249, 13);
			num = this.RotateLeft(num + this.G(num2, num3, num4) + this.X[1] + 1518500249, 3);
			num4 = this.RotateLeft(num4 + this.G(num, num2, num3) + this.X[5] + 1518500249, 5);
			num3 = this.RotateLeft(num3 + this.G(num4, num, num2) + this.X[9] + 1518500249, 9);
			num2 = this.RotateLeft(num2 + this.G(num3, num4, num) + this.X[13] + 1518500249, 13);
			num = this.RotateLeft(num + this.G(num2, num3, num4) + this.X[2] + 1518500249, 3);
			num4 = this.RotateLeft(num4 + this.G(num, num2, num3) + this.X[6] + 1518500249, 5);
			num3 = this.RotateLeft(num3 + this.G(num4, num, num2) + this.X[10] + 1518500249, 9);
			num2 = this.RotateLeft(num2 + this.G(num3, num4, num) + this.X[14] + 1518500249, 13);
			num = this.RotateLeft(num + this.G(num2, num3, num4) + this.X[3] + 1518500249, 3);
			num4 = this.RotateLeft(num4 + this.G(num, num2, num3) + this.X[7] + 1518500249, 5);
			num3 = this.RotateLeft(num3 + this.G(num4, num, num2) + this.X[11] + 1518500249, 9);
			num2 = this.RotateLeft(num2 + this.G(num3, num4, num) + this.X[15] + 1518500249, 13);
			num = this.RotateLeft(num + this.H(num2, num3, num4) + this.X[0] + 1859775393, 3);
			num4 = this.RotateLeft(num4 + this.H(num, num2, num3) + this.X[8] + 1859775393, 9);
			num3 = this.RotateLeft(num3 + this.H(num4, num, num2) + this.X[4] + 1859775393, 11);
			num2 = this.RotateLeft(num2 + this.H(num3, num4, num) + this.X[12] + 1859775393, 15);
			num = this.RotateLeft(num + this.H(num2, num3, num4) + this.X[2] + 1859775393, 3);
			num4 = this.RotateLeft(num4 + this.H(num, num2, num3) + this.X[10] + 1859775393, 9);
			num3 = this.RotateLeft(num3 + this.H(num4, num, num2) + this.X[6] + 1859775393, 11);
			num2 = this.RotateLeft(num2 + this.H(num3, num4, num) + this.X[14] + 1859775393, 15);
			num = this.RotateLeft(num + this.H(num2, num3, num4) + this.X[1] + 1859775393, 3);
			num4 = this.RotateLeft(num4 + this.H(num, num2, num3) + this.X[9] + 1859775393, 9);
			num3 = this.RotateLeft(num3 + this.H(num4, num, num2) + this.X[5] + 1859775393, 11);
			num2 = this.RotateLeft(num2 + this.H(num3, num4, num) + this.X[13] + 1859775393, 15);
			num = this.RotateLeft(num + this.H(num2, num3, num4) + this.X[3] + 1859775393, 3);
			num4 = this.RotateLeft(num4 + this.H(num, num2, num3) + this.X[11] + 1859775393, 9);
			num3 = this.RotateLeft(num3 + this.H(num4, num, num2) + this.X[7] + 1859775393, 11);
			num2 = this.RotateLeft(num2 + this.H(num3, num4, num) + this.X[15] + 1859775393, 15);
			this.H1 += num;
			this.H2 += num2;
			this.H3 += num3;
			this.H4 += num4;
			this.xOff = 0;
			for (int num5 = 0; num5 != this.X.Length; num5++)
			{
				this.X[num5] = 0;
			}
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x001587EE File Offset: 0x001569EE
		public override IMemoable Copy()
		{
			return new MD4Digest(this);
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x001587F8 File Offset: 0x001569F8
		public override void Reset(IMemoable other)
		{
			MD4Digest t = (MD4Digest)other;
			this.CopyIn(t);
		}

		// Token: 0x04002434 RID: 9268
		private const int DigestLength = 16;

		// Token: 0x04002435 RID: 9269
		private int H1;

		// Token: 0x04002436 RID: 9270
		private int H2;

		// Token: 0x04002437 RID: 9271
		private int H3;

		// Token: 0x04002438 RID: 9272
		private int H4;

		// Token: 0x04002439 RID: 9273
		private int[] X = new int[16];

		// Token: 0x0400243A RID: 9274
		private int xOff;

		// Token: 0x0400243B RID: 9275
		private const int S11 = 3;

		// Token: 0x0400243C RID: 9276
		private const int S12 = 7;

		// Token: 0x0400243D RID: 9277
		private const int S13 = 11;

		// Token: 0x0400243E RID: 9278
		private const int S14 = 19;

		// Token: 0x0400243F RID: 9279
		private const int S21 = 3;

		// Token: 0x04002440 RID: 9280
		private const int S22 = 5;

		// Token: 0x04002441 RID: 9281
		private const int S23 = 9;

		// Token: 0x04002442 RID: 9282
		private const int S24 = 13;

		// Token: 0x04002443 RID: 9283
		private const int S31 = 3;

		// Token: 0x04002444 RID: 9284
		private const int S32 = 9;

		// Token: 0x04002445 RID: 9285
		private const int S33 = 11;

		// Token: 0x04002446 RID: 9286
		private const int S34 = 15;
	}
}
