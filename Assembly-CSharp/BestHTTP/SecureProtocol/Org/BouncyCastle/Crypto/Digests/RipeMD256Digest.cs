using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005B3 RID: 1459
	public class RipeMD256Digest : GeneralDigest
	{
		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x060037CE RID: 14286 RVA: 0x0015C82B File Offset: 0x0015AA2B
		public override string AlgorithmName
		{
			get
			{
				return "RIPEMD256";
			}
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x00154E34 File Offset: 0x00153034
		public override int GetDigestSize()
		{
			return 32;
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x0015C832 File Offset: 0x0015AA32
		public RipeMD256Digest()
		{
			this.Reset();
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x0015C84D File Offset: 0x0015AA4D
		public RipeMD256Digest(RipeMD256Digest t) : base(t)
		{
			this.CopyIn(t);
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x0015C86C File Offset: 0x0015AA6C
		private void CopyIn(RipeMD256Digest t)
		{
			base.CopyIn(t);
			this.H0 = t.H0;
			this.H1 = t.H1;
			this.H2 = t.H2;
			this.H3 = t.H3;
			this.H4 = t.H4;
			this.H5 = t.H5;
			this.H6 = t.H6;
			this.H7 = t.H7;
			Array.Copy(t.X, 0, this.X, 0, t.X.Length);
			this.xOff = t.xOff;
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x0015C908 File Offset: 0x0015AB08
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

		// Token: 0x060037D4 RID: 14292 RVA: 0x0015C972 File Offset: 0x0015AB72
		internal override void ProcessLength(long bitLength)
		{
			if (this.xOff > 14)
			{
				this.ProcessBlock();
			}
			this.X[14] = (int)(bitLength & (long)((ulong)-1));
			this.X[15] = (int)((ulong)bitLength >> 32);
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x00158034 File Offset: 0x00156234
		private void UnpackWord(int word, byte[] outBytes, int outOff)
		{
			outBytes[outOff] = (byte)word;
			outBytes[outOff + 1] = (byte)((uint)word >> 8);
			outBytes[outOff + 2] = (byte)((uint)word >> 16);
			outBytes[outOff + 3] = (byte)((uint)word >> 24);
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x0015C9A0 File Offset: 0x0015ABA0
		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			this.UnpackWord(this.H0, output, outOff);
			this.UnpackWord(this.H1, output, outOff + 4);
			this.UnpackWord(this.H2, output, outOff + 8);
			this.UnpackWord(this.H3, output, outOff + 12);
			this.UnpackWord(this.H4, output, outOff + 16);
			this.UnpackWord(this.H5, output, outOff + 20);
			this.UnpackWord(this.H6, output, outOff + 24);
			this.UnpackWord(this.H7, output, outOff + 28);
			this.Reset();
			return 32;
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x0015CA40 File Offset: 0x0015AC40
		public override void Reset()
		{
			base.Reset();
			this.H0 = 1732584193;
			this.H1 = -271733879;
			this.H2 = -1732584194;
			this.H3 = 271733878;
			this.H4 = 1985229328;
			this.H5 = -19088744;
			this.H6 = -1985229329;
			this.H7 = 19088743;
			this.xOff = 0;
			for (int num = 0; num != this.X.Length; num++)
			{
				this.X[num] = 0;
			}
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x00144EAA File Offset: 0x001430AA
		private int RL(int x, int n)
		{
			return x << n | (int)((uint)x >> 32 - n);
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x0015812D File Offset: 0x0015632D
		private int F1(int x, int y, int z)
		{
			return x ^ y ^ z;
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x00158116 File Offset: 0x00156316
		private int F2(int x, int y, int z)
		{
			return (x & y) | (~x & z);
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x001597F6 File Offset: 0x001579F6
		private int F3(int x, int y, int z)
		{
			return (x | ~y) ^ z;
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x001597FE File Offset: 0x001579FE
		private int F4(int x, int y, int z)
		{
			return (x & z) | (y & ~z);
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x0015CACE File Offset: 0x0015ACCE
		private int F1(int a, int b, int c, int d, int x, int s)
		{
			return this.RL(a + this.F1(b, c, d) + x, s);
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x0015CAE7 File Offset: 0x0015ACE7
		private int F2(int a, int b, int c, int d, int x, int s)
		{
			return this.RL(a + this.F2(b, c, d) + x + 1518500249, s);
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x0015CB06 File Offset: 0x0015AD06
		private int F3(int a, int b, int c, int d, int x, int s)
		{
			return this.RL(a + this.F3(b, c, d) + x + 1859775393, s);
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x0015CB25 File Offset: 0x0015AD25
		private int F4(int a, int b, int c, int d, int x, int s)
		{
			return this.RL(a + this.F4(b, c, d) + x + -1894007588, s);
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x0015CACE File Offset: 0x0015ACCE
		private int FF1(int a, int b, int c, int d, int x, int s)
		{
			return this.RL(a + this.F1(b, c, d) + x, s);
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x0015CB44 File Offset: 0x0015AD44
		private int FF2(int a, int b, int c, int d, int x, int s)
		{
			return this.RL(a + this.F2(b, c, d) + x + 1836072691, s);
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x0015CB63 File Offset: 0x0015AD63
		private int FF3(int a, int b, int c, int d, int x, int s)
		{
			return this.RL(a + this.F3(b, c, d) + x + 1548603684, s);
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x0015CB82 File Offset: 0x0015AD82
		private int FF4(int a, int b, int c, int d, int x, int s)
		{
			return this.RL(a + this.F4(b, c, d) + x + 1352829926, s);
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x0015CBA4 File Offset: 0x0015ADA4
		internal override void ProcessBlock()
		{
			int num = this.H0;
			int num2 = this.H1;
			int num3 = this.H2;
			int num4 = this.H3;
			int num5 = this.H4;
			int num6 = this.H5;
			int num7 = this.H6;
			int num8 = this.H7;
			num = this.F1(num, num2, num3, num4, this.X[0], 11);
			num4 = this.F1(num4, num, num2, num3, this.X[1], 14);
			num3 = this.F1(num3, num4, num, num2, this.X[2], 15);
			num2 = this.F1(num2, num3, num4, num, this.X[3], 12);
			num = this.F1(num, num2, num3, num4, this.X[4], 5);
			num4 = this.F1(num4, num, num2, num3, this.X[5], 8);
			num3 = this.F1(num3, num4, num, num2, this.X[6], 7);
			num2 = this.F1(num2, num3, num4, num, this.X[7], 9);
			num = this.F1(num, num2, num3, num4, this.X[8], 11);
			num4 = this.F1(num4, num, num2, num3, this.X[9], 13);
			num3 = this.F1(num3, num4, num, num2, this.X[10], 14);
			num2 = this.F1(num2, num3, num4, num, this.X[11], 15);
			num = this.F1(num, num2, num3, num4, this.X[12], 6);
			num4 = this.F1(num4, num, num2, num3, this.X[13], 7);
			num3 = this.F1(num3, num4, num, num2, this.X[14], 9);
			num2 = this.F1(num2, num3, num4, num, this.X[15], 8);
			num5 = this.FF4(num5, num6, num7, num8, this.X[5], 8);
			num8 = this.FF4(num8, num5, num6, num7, this.X[14], 9);
			num7 = this.FF4(num7, num8, num5, num6, this.X[7], 9);
			num6 = this.FF4(num6, num7, num8, num5, this.X[0], 11);
			num5 = this.FF4(num5, num6, num7, num8, this.X[9], 13);
			num8 = this.FF4(num8, num5, num6, num7, this.X[2], 15);
			num7 = this.FF4(num7, num8, num5, num6, this.X[11], 15);
			num6 = this.FF4(num6, num7, num8, num5, this.X[4], 5);
			num5 = this.FF4(num5, num6, num7, num8, this.X[13], 7);
			num8 = this.FF4(num8, num5, num6, num7, this.X[6], 7);
			num7 = this.FF4(num7, num8, num5, num6, this.X[15], 8);
			num6 = this.FF4(num6, num7, num8, num5, this.X[8], 11);
			num5 = this.FF4(num5, num6, num7, num8, this.X[1], 14);
			num8 = this.FF4(num8, num5, num6, num7, this.X[10], 14);
			num7 = this.FF4(num7, num8, num5, num6, this.X[3], 12);
			num6 = this.FF4(num6, num7, num8, num5, this.X[12], 6);
			int num9 = num;
			num = num5;
			num5 = num9;
			num = this.F2(num, num2, num3, num4, this.X[7], 7);
			num4 = this.F2(num4, num, num2, num3, this.X[4], 6);
			num3 = this.F2(num3, num4, num, num2, this.X[13], 8);
			num2 = this.F2(num2, num3, num4, num, this.X[1], 13);
			num = this.F2(num, num2, num3, num4, this.X[10], 11);
			num4 = this.F2(num4, num, num2, num3, this.X[6], 9);
			num3 = this.F2(num3, num4, num, num2, this.X[15], 7);
			num2 = this.F2(num2, num3, num4, num, this.X[3], 15);
			num = this.F2(num, num2, num3, num4, this.X[12], 7);
			num4 = this.F2(num4, num, num2, num3, this.X[0], 12);
			num3 = this.F2(num3, num4, num, num2, this.X[9], 15);
			num2 = this.F2(num2, num3, num4, num, this.X[5], 9);
			num = this.F2(num, num2, num3, num4, this.X[2], 11);
			num4 = this.F2(num4, num, num2, num3, this.X[14], 7);
			num3 = this.F2(num3, num4, num, num2, this.X[11], 13);
			num2 = this.F2(num2, num3, num4, num, this.X[8], 12);
			num5 = this.FF3(num5, num6, num7, num8, this.X[6], 9);
			num8 = this.FF3(num8, num5, num6, num7, this.X[11], 13);
			num7 = this.FF3(num7, num8, num5, num6, this.X[3], 15);
			num6 = this.FF3(num6, num7, num8, num5, this.X[7], 7);
			num5 = this.FF3(num5, num6, num7, num8, this.X[0], 12);
			num8 = this.FF3(num8, num5, num6, num7, this.X[13], 8);
			num7 = this.FF3(num7, num8, num5, num6, this.X[5], 9);
			num6 = this.FF3(num6, num7, num8, num5, this.X[10], 11);
			num5 = this.FF3(num5, num6, num7, num8, this.X[14], 7);
			num8 = this.FF3(num8, num5, num6, num7, this.X[15], 7);
			num7 = this.FF3(num7, num8, num5, num6, this.X[8], 12);
			num6 = this.FF3(num6, num7, num8, num5, this.X[12], 7);
			num5 = this.FF3(num5, num6, num7, num8, this.X[4], 6);
			num8 = this.FF3(num8, num5, num6, num7, this.X[9], 15);
			num7 = this.FF3(num7, num8, num5, num6, this.X[1], 13);
			num6 = this.FF3(num6, num7, num8, num5, this.X[2], 11);
			int num10 = num2;
			num2 = num6;
			num6 = num10;
			num = this.F3(num, num2, num3, num4, this.X[3], 11);
			num4 = this.F3(num4, num, num2, num3, this.X[10], 13);
			num3 = this.F3(num3, num4, num, num2, this.X[14], 6);
			num2 = this.F3(num2, num3, num4, num, this.X[4], 7);
			num = this.F3(num, num2, num3, num4, this.X[9], 14);
			num4 = this.F3(num4, num, num2, num3, this.X[15], 9);
			num3 = this.F3(num3, num4, num, num2, this.X[8], 13);
			num2 = this.F3(num2, num3, num4, num, this.X[1], 15);
			num = this.F3(num, num2, num3, num4, this.X[2], 14);
			num4 = this.F3(num4, num, num2, num3, this.X[7], 8);
			num3 = this.F3(num3, num4, num, num2, this.X[0], 13);
			num2 = this.F3(num2, num3, num4, num, this.X[6], 6);
			num = this.F3(num, num2, num3, num4, this.X[13], 5);
			num4 = this.F3(num4, num, num2, num3, this.X[11], 12);
			num3 = this.F3(num3, num4, num, num2, this.X[5], 7);
			num2 = this.F3(num2, num3, num4, num, this.X[12], 5);
			num5 = this.FF2(num5, num6, num7, num8, this.X[15], 9);
			num8 = this.FF2(num8, num5, num6, num7, this.X[5], 7);
			num7 = this.FF2(num7, num8, num5, num6, this.X[1], 15);
			num6 = this.FF2(num6, num7, num8, num5, this.X[3], 11);
			num5 = this.FF2(num5, num6, num7, num8, this.X[7], 8);
			num8 = this.FF2(num8, num5, num6, num7, this.X[14], 6);
			num7 = this.FF2(num7, num8, num5, num6, this.X[6], 6);
			num6 = this.FF2(num6, num7, num8, num5, this.X[9], 14);
			num5 = this.FF2(num5, num6, num7, num8, this.X[11], 12);
			num8 = this.FF2(num8, num5, num6, num7, this.X[8], 13);
			num7 = this.FF2(num7, num8, num5, num6, this.X[12], 5);
			num6 = this.FF2(num6, num7, num8, num5, this.X[2], 14);
			num5 = this.FF2(num5, num6, num7, num8, this.X[10], 13);
			num8 = this.FF2(num8, num5, num6, num7, this.X[0], 13);
			num7 = this.FF2(num7, num8, num5, num6, this.X[4], 7);
			num6 = this.FF2(num6, num7, num8, num5, this.X[13], 5);
			int num11 = num3;
			num3 = num7;
			num7 = num11;
			num = this.F4(num, num2, num3, num4, this.X[1], 11);
			num4 = this.F4(num4, num, num2, num3, this.X[9], 12);
			num3 = this.F4(num3, num4, num, num2, this.X[11], 14);
			num2 = this.F4(num2, num3, num4, num, this.X[10], 15);
			num = this.F4(num, num2, num3, num4, this.X[0], 14);
			num4 = this.F4(num4, num, num2, num3, this.X[8], 15);
			num3 = this.F4(num3, num4, num, num2, this.X[12], 9);
			num2 = this.F4(num2, num3, num4, num, this.X[4], 8);
			num = this.F4(num, num2, num3, num4, this.X[13], 9);
			num4 = this.F4(num4, num, num2, num3, this.X[3], 14);
			num3 = this.F4(num3, num4, num, num2, this.X[7], 5);
			num2 = this.F4(num2, num3, num4, num, this.X[15], 6);
			num = this.F4(num, num2, num3, num4, this.X[14], 8);
			num4 = this.F4(num4, num, num2, num3, this.X[5], 6);
			num3 = this.F4(num3, num4, num, num2, this.X[6], 5);
			num2 = this.F4(num2, num3, num4, num, this.X[2], 12);
			num5 = this.FF1(num5, num6, num7, num8, this.X[8], 15);
			num8 = this.FF1(num8, num5, num6, num7, this.X[6], 5);
			num7 = this.FF1(num7, num8, num5, num6, this.X[4], 8);
			num6 = this.FF1(num6, num7, num8, num5, this.X[1], 11);
			num5 = this.FF1(num5, num6, num7, num8, this.X[3], 14);
			num8 = this.FF1(num8, num5, num6, num7, this.X[11], 14);
			num7 = this.FF1(num7, num8, num5, num6, this.X[15], 6);
			num6 = this.FF1(num6, num7, num8, num5, this.X[0], 14);
			num5 = this.FF1(num5, num6, num7, num8, this.X[5], 6);
			num8 = this.FF1(num8, num5, num6, num7, this.X[12], 9);
			num7 = this.FF1(num7, num8, num5, num6, this.X[2], 12);
			num6 = this.FF1(num6, num7, num8, num5, this.X[13], 9);
			num5 = this.FF1(num5, num6, num7, num8, this.X[9], 12);
			num8 = this.FF1(num8, num5, num6, num7, this.X[7], 5);
			num7 = this.FF1(num7, num8, num5, num6, this.X[10], 15);
			num6 = this.FF1(num6, num7, num8, num5, this.X[14], 8);
			int num12 = num4;
			num4 = num8;
			num8 = num12;
			this.H0 += num;
			this.H1 += num2;
			this.H2 += num3;
			this.H3 += num4;
			this.H4 += num5;
			this.H5 += num6;
			this.H6 += num7;
			this.H7 += num8;
			this.xOff = 0;
			for (int num13 = 0; num13 != this.X.Length; num13++)
			{
				this.X[num13] = 0;
			}
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x0015D867 File Offset: 0x0015BA67
		public override IMemoable Copy()
		{
			return new RipeMD256Digest(this);
		}

		// Token: 0x060037E7 RID: 14311 RVA: 0x0015D870 File Offset: 0x0015BA70
		public override void Reset(IMemoable other)
		{
			RipeMD256Digest t = (RipeMD256Digest)other;
			this.CopyIn(t);
		}

		// Token: 0x0400246F RID: 9327
		private const int DigestLength = 32;

		// Token: 0x04002470 RID: 9328
		private int H0;

		// Token: 0x04002471 RID: 9329
		private int H1;

		// Token: 0x04002472 RID: 9330
		private int H2;

		// Token: 0x04002473 RID: 9331
		private int H3;

		// Token: 0x04002474 RID: 9332
		private int H4;

		// Token: 0x04002475 RID: 9333
		private int H5;

		// Token: 0x04002476 RID: 9334
		private int H6;

		// Token: 0x04002477 RID: 9335
		private int H7;

		// Token: 0x04002478 RID: 9336
		private int[] X = new int[16];

		// Token: 0x04002479 RID: 9337
		private int xOff;
	}
}
