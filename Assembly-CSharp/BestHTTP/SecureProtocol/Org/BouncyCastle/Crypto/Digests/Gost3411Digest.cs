using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005A9 RID: 1449
	public class Gost3411Digest : IDigest, IMemoable
	{
		// Token: 0x06003718 RID: 14104 RVA: 0x00155E08 File Offset: 0x00154008
		private static byte[][] MakeC()
		{
			byte[][] array = new byte[4][];
			for (int i = 0; i < 4; i++)
			{
				array[i] = new byte[32];
			}
			return array;
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x00155E34 File Offset: 0x00154034
		public Gost3411Digest()
		{
			this.sBox = Gost28147Engine.GetSBox("D-A");
			this.cipher.Init(true, new ParametersWithSBox(null, this.sBox));
			this.Reset();
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x00155F34 File Offset: 0x00154134
		public Gost3411Digest(byte[] sBoxParam)
		{
			this.sBox = Arrays.Clone(sBoxParam);
			this.cipher.Init(true, new ParametersWithSBox(null, this.sBox));
			this.Reset();
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x00156030 File Offset: 0x00154230
		public Gost3411Digest(Gost3411Digest t)
		{
			this.Reset(t);
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x0600371C RID: 14108 RVA: 0x00156108 File Offset: 0x00154308
		public string AlgorithmName
		{
			get
			{
				return "Gost3411";
			}
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x00154E34 File Offset: 0x00153034
		public int GetDigestSize()
		{
			return 32;
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x00156110 File Offset: 0x00154310
		public void Update(byte input)
		{
			byte[] array = this.xBuf;
			int num = this.xBufOff;
			this.xBufOff = num + 1;
			array[num] = input;
			if (this.xBufOff == this.xBuf.Length)
			{
				this.sumByteArray(this.xBuf);
				this.processBlock(this.xBuf, 0);
				this.xBufOff = 0;
			}
			this.byteCount += 1UL;
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x00156178 File Offset: 0x00154378
		public void BlockUpdate(byte[] input, int inOff, int length)
		{
			while (this.xBufOff != 0)
			{
				if (length <= 0)
				{
					break;
				}
				this.Update(input[inOff]);
				inOff++;
				length--;
			}
			while (length > this.xBuf.Length)
			{
				Array.Copy(input, inOff, this.xBuf, 0, this.xBuf.Length);
				this.sumByteArray(this.xBuf);
				this.processBlock(this.xBuf, 0);
				inOff += this.xBuf.Length;
				length -= this.xBuf.Length;
				this.byteCount += (ulong)this.xBuf.Length;
			}
			while (length > 0)
			{
				this.Update(input[inOff]);
				inOff++;
				length--;
			}
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x0015622C File Offset: 0x0015442C
		private byte[] P(byte[] input)
		{
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				this.K[num++] = input[i];
				this.K[num++] = input[8 + i];
				this.K[num++] = input[16 + i];
				this.K[num++] = input[24 + i];
			}
			return this.K;
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x00156294 File Offset: 0x00154494
		private byte[] A(byte[] input)
		{
			for (int i = 0; i < 8; i++)
			{
				this.a[i] = (input[i] ^ input[i + 8]);
			}
			Array.Copy(input, 8, input, 0, 24);
			Array.Copy(this.a, 0, input, 24, 8);
			return input;
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x001562DB File Offset: 0x001544DB
		private void E(byte[] key, byte[] s, int sOff, byte[] input, int inOff)
		{
			this.cipher.Init(true, new KeyParameter(key));
			this.cipher.ProcessBlock(input, inOff, s, sOff);
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x00156304 File Offset: 0x00154504
		private void fw(byte[] input)
		{
			Gost3411Digest.cpyBytesToShort(input, this.wS);
			this.w_S[15] = (this.wS[0] ^ this.wS[1] ^ this.wS[2] ^ this.wS[3] ^ this.wS[12] ^ this.wS[15]);
			Array.Copy(this.wS, 1, this.w_S, 0, 15);
			Gost3411Digest.cpyShortToBytes(this.w_S, input);
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x00156380 File Offset: 0x00154580
		private void processBlock(byte[] input, int inOff)
		{
			Array.Copy(input, inOff, this.M, 0, 32);
			this.H.CopyTo(this.U, 0);
			this.M.CopyTo(this.V, 0);
			for (int i = 0; i < 32; i++)
			{
				this.W[i] = (this.U[i] ^ this.V[i]);
			}
			this.E(this.P(this.W), this.S, 0, this.H, 0);
			for (int j = 1; j < 4; j++)
			{
				byte[] array = this.A(this.U);
				for (int k = 0; k < 32; k++)
				{
					this.U[k] = (array[k] ^ this.C[j][k]);
				}
				this.V = this.A(this.A(this.V));
				for (int l = 0; l < 32; l++)
				{
					this.W[l] = (this.U[l] ^ this.V[l]);
				}
				this.E(this.P(this.W), this.S, j * 8, this.H, j * 8);
			}
			for (int m = 0; m < 12; m++)
			{
				this.fw(this.S);
			}
			for (int n = 0; n < 32; n++)
			{
				this.S[n] = (this.S[n] ^ this.M[n]);
			}
			this.fw(this.S);
			for (int num = 0; num < 32; num++)
			{
				this.S[num] = (this.H[num] ^ this.S[num]);
			}
			for (int num2 = 0; num2 < 61; num2++)
			{
				this.fw(this.S);
			}
			Array.Copy(this.S, 0, this.H, 0, this.H.Length);
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x00156574 File Offset: 0x00154774
		private void finish()
		{
			Pack.UInt64_To_LE(this.byteCount * 8UL, this.L);
			while (this.xBufOff != 0)
			{
				this.Update(0);
			}
			this.processBlock(this.L, 0);
			this.processBlock(this.Sum, 0);
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x001565C0 File Offset: 0x001547C0
		public int DoFinal(byte[] output, int outOff)
		{
			this.finish();
			this.H.CopyTo(output, outOff);
			this.Reset();
			return 32;
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x001565E0 File Offset: 0x001547E0
		public void Reset()
		{
			this.byteCount = 0UL;
			this.xBufOff = 0;
			Array.Clear(this.H, 0, this.H.Length);
			Array.Clear(this.L, 0, this.L.Length);
			Array.Clear(this.M, 0, this.M.Length);
			Array.Clear(this.C[1], 0, this.C[1].Length);
			Array.Clear(this.C[3], 0, this.C[3].Length);
			Array.Clear(this.Sum, 0, this.Sum.Length);
			Array.Clear(this.xBuf, 0, this.xBuf.Length);
			Gost3411Digest.C2.CopyTo(this.C[2], 0);
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x001566A4 File Offset: 0x001548A4
		private void sumByteArray(byte[] input)
		{
			int num = 0;
			for (int num2 = 0; num2 != this.Sum.Length; num2++)
			{
				int num3 = (int)((this.Sum[num2] & byte.MaxValue) + (input[num2] & byte.MaxValue)) + num;
				this.Sum[num2] = (byte)num3;
				num = num3 >> 8;
			}
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x001566F0 File Offset: 0x001548F0
		private static void cpyBytesToShort(byte[] S, short[] wS)
		{
			for (int i = 0; i < S.Length / 2; i++)
			{
				wS[i] = (short)(((int)S[i * 2 + 1] << 8 & 65280) | (int)(S[i * 2] & byte.MaxValue));
			}
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x0015672C File Offset: 0x0015492C
		private static void cpyShortToBytes(short[] wS, byte[] S)
		{
			for (int i = 0; i < S.Length / 2; i++)
			{
				S[i * 2 + 1] = (byte)(wS[i] >> 8);
				S[i * 2] = (byte)wS[i];
			}
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x00154E34 File Offset: 0x00153034
		public int GetByteLength()
		{
			return 32;
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x0015675F File Offset: 0x0015495F
		public IMemoable Copy()
		{
			return new Gost3411Digest(this);
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x00156768 File Offset: 0x00154968
		public void Reset(IMemoable other)
		{
			Gost3411Digest gost3411Digest = (Gost3411Digest)other;
			this.sBox = gost3411Digest.sBox;
			this.cipher.Init(true, new ParametersWithSBox(null, this.sBox));
			this.Reset();
			Array.Copy(gost3411Digest.H, 0, this.H, 0, gost3411Digest.H.Length);
			Array.Copy(gost3411Digest.L, 0, this.L, 0, gost3411Digest.L.Length);
			Array.Copy(gost3411Digest.M, 0, this.M, 0, gost3411Digest.M.Length);
			Array.Copy(gost3411Digest.Sum, 0, this.Sum, 0, gost3411Digest.Sum.Length);
			Array.Copy(gost3411Digest.C[1], 0, this.C[1], 0, gost3411Digest.C[1].Length);
			Array.Copy(gost3411Digest.C[2], 0, this.C[2], 0, gost3411Digest.C[2].Length);
			Array.Copy(gost3411Digest.C[3], 0, this.C[3], 0, gost3411Digest.C[3].Length);
			Array.Copy(gost3411Digest.xBuf, 0, this.xBuf, 0, gost3411Digest.xBuf.Length);
			this.xBufOff = gost3411Digest.xBufOff;
			this.byteCount = gost3411Digest.byteCount;
		}

		// Token: 0x04002400 RID: 9216
		private const int DIGEST_LENGTH = 32;

		// Token: 0x04002401 RID: 9217
		private byte[] H = new byte[32];

		// Token: 0x04002402 RID: 9218
		private byte[] L = new byte[32];

		// Token: 0x04002403 RID: 9219
		private byte[] M = new byte[32];

		// Token: 0x04002404 RID: 9220
		private byte[] Sum = new byte[32];

		// Token: 0x04002405 RID: 9221
		private byte[][] C = Gost3411Digest.MakeC();

		// Token: 0x04002406 RID: 9222
		private byte[] xBuf = new byte[32];

		// Token: 0x04002407 RID: 9223
		private int xBufOff;

		// Token: 0x04002408 RID: 9224
		private ulong byteCount;

		// Token: 0x04002409 RID: 9225
		private readonly IBlockCipher cipher = new Gost28147Engine();

		// Token: 0x0400240A RID: 9226
		private byte[] sBox;

		// Token: 0x0400240B RID: 9227
		private byte[] K = new byte[32];

		// Token: 0x0400240C RID: 9228
		private byte[] a = new byte[8];

		// Token: 0x0400240D RID: 9229
		internal short[] wS = new short[16];

		// Token: 0x0400240E RID: 9230
		internal short[] w_S = new short[16];

		// Token: 0x0400240F RID: 9231
		internal byte[] S = new byte[32];

		// Token: 0x04002410 RID: 9232
		internal byte[] U = new byte[32];

		// Token: 0x04002411 RID: 9233
		internal byte[] V = new byte[32];

		// Token: 0x04002412 RID: 9234
		internal byte[] W = new byte[32];

		// Token: 0x04002413 RID: 9235
		private static readonly byte[] C2 = new byte[]
		{
			0,
			byte.MaxValue,
			0,
			byte.MaxValue,
			0,
			byte.MaxValue,
			0,
			byte.MaxValue,
			byte.MaxValue,
			0,
			byte.MaxValue,
			0,
			byte.MaxValue,
			0,
			byte.MaxValue,
			0,
			0,
			byte.MaxValue,
			byte.MaxValue,
			0,
			byte.MaxValue,
			0,
			0,
			byte.MaxValue,
			byte.MaxValue,
			0,
			0,
			0,
			byte.MaxValue,
			byte.MaxValue,
			0,
			byte.MaxValue
		};
	}
}
