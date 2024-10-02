using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005AC RID: 1452
	public class MD2Digest : IDigest, IMemoable
	{
		// Token: 0x0600375F RID: 14175 RVA: 0x00157B11 File Offset: 0x00155D11
		public MD2Digest()
		{
			this.Reset();
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x00157B46 File Offset: 0x00155D46
		public MD2Digest(MD2Digest t)
		{
			this.CopyIn(t);
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x00157B7C File Offset: 0x00155D7C
		private void CopyIn(MD2Digest t)
		{
			Array.Copy(t.X, 0, this.X, 0, t.X.Length);
			this.xOff = t.xOff;
			Array.Copy(t.M, 0, this.M, 0, t.M.Length);
			this.mOff = t.mOff;
			Array.Copy(t.C, 0, this.C, 0, t.C.Length);
			this.COff = t.COff;
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06003762 RID: 14178 RVA: 0x00157BFE File Offset: 0x00155DFE
		public string AlgorithmName
		{
			get
			{
				return "MD2";
			}
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x0012AD29 File Offset: 0x00128F29
		public int GetDigestSize()
		{
			return 16;
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x0012AD29 File Offset: 0x00128F29
		public int GetByteLength()
		{
			return 16;
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x00157C08 File Offset: 0x00155E08
		public int DoFinal(byte[] output, int outOff)
		{
			byte b = (byte)(this.M.Length - this.mOff);
			for (int i = this.mOff; i < this.M.Length; i++)
			{
				this.M[i] = b;
			}
			this.ProcessChecksum(this.M);
			this.ProcessBlock(this.M);
			this.ProcessBlock(this.C);
			Array.Copy(this.X, this.xOff, output, outOff, 16);
			this.Reset();
			return 16;
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x00157C88 File Offset: 0x00155E88
		public void Reset()
		{
			this.xOff = 0;
			for (int num = 0; num != this.X.Length; num++)
			{
				this.X[num] = 0;
			}
			this.mOff = 0;
			for (int num2 = 0; num2 != this.M.Length; num2++)
			{
				this.M[num2] = 0;
			}
			this.COff = 0;
			for (int num3 = 0; num3 != this.C.Length; num3++)
			{
				this.C[num3] = 0;
			}
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x00157D00 File Offset: 0x00155F00
		public void Update(byte input)
		{
			byte[] m = this.M;
			int num = this.mOff;
			this.mOff = num + 1;
			m[num] = input;
			if (this.mOff == 16)
			{
				this.ProcessChecksum(this.M);
				this.ProcessBlock(this.M);
				this.mOff = 0;
			}
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x00157D50 File Offset: 0x00155F50
		public void BlockUpdate(byte[] input, int inOff, int length)
		{
			while (this.mOff != 0)
			{
				if (length <= 0)
				{
					break;
				}
				this.Update(input[inOff]);
				inOff++;
				length--;
			}
			while (length > 16)
			{
				Array.Copy(input, inOff, this.M, 0, 16);
				this.ProcessChecksum(this.M);
				this.ProcessBlock(this.M);
				length -= 16;
				inOff += 16;
			}
			while (length > 0)
			{
				this.Update(input[inOff]);
				inOff++;
				length--;
			}
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x00157DD4 File Offset: 0x00155FD4
		internal void ProcessChecksum(byte[] m)
		{
			int num = (int)this.C[15];
			for (int i = 0; i < 16; i++)
			{
				byte[] c = this.C;
				int num2 = i;
				c[num2] ^= MD2Digest.S[((int)m[i] ^ num) & 255];
				num = (int)this.C[i];
			}
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x00157E24 File Offset: 0x00156024
		internal void ProcessBlock(byte[] m)
		{
			for (int i = 0; i < 16; i++)
			{
				this.X[i + 16] = m[i];
				this.X[i + 32] = (m[i] ^ this.X[i]);
			}
			int num = 0;
			for (int j = 0; j < 18; j++)
			{
				for (int k = 0; k < 48; k++)
				{
					byte[] x = this.X;
					int num2 = k;
					num = (int)(x[num2] ^= MD2Digest.S[num]);
					num &= 255;
				}
				num = (num + j) % 256;
			}
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x00157EB0 File Offset: 0x001560B0
		public IMemoable Copy()
		{
			return new MD2Digest(this);
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x00157EB8 File Offset: 0x001560B8
		public void Reset(IMemoable other)
		{
			MD2Digest t = (MD2Digest)other;
			this.CopyIn(t);
		}

		// Token: 0x0400242B RID: 9259
		private const int DigestLength = 16;

		// Token: 0x0400242C RID: 9260
		private const int BYTE_LENGTH = 16;

		// Token: 0x0400242D RID: 9261
		private byte[] X = new byte[48];

		// Token: 0x0400242E RID: 9262
		private int xOff;

		// Token: 0x0400242F RID: 9263
		private byte[] M = new byte[16];

		// Token: 0x04002430 RID: 9264
		private int mOff;

		// Token: 0x04002431 RID: 9265
		private byte[] C = new byte[16];

		// Token: 0x04002432 RID: 9266
		private int COff;

		// Token: 0x04002433 RID: 9267
		private static readonly byte[] S = new byte[]
		{
			41,
			46,
			67,
			201,
			162,
			216,
			124,
			1,
			61,
			54,
			84,
			161,
			236,
			240,
			6,
			19,
			98,
			167,
			5,
			243,
			192,
			199,
			115,
			140,
			152,
			147,
			43,
			217,
			188,
			76,
			130,
			202,
			30,
			155,
			87,
			60,
			253,
			212,
			224,
			22,
			103,
			66,
			111,
			24,
			138,
			23,
			229,
			18,
			190,
			78,
			196,
			214,
			218,
			158,
			222,
			73,
			160,
			251,
			245,
			142,
			187,
			47,
			238,
			122,
			169,
			104,
			121,
			145,
			21,
			178,
			7,
			63,
			148,
			194,
			16,
			137,
			11,
			34,
			95,
			33,
			128,
			127,
			93,
			154,
			90,
			144,
			50,
			39,
			53,
			62,
			204,
			231,
			191,
			247,
			151,
			3,
			byte.MaxValue,
			25,
			48,
			179,
			72,
			165,
			181,
			209,
			215,
			94,
			146,
			42,
			172,
			86,
			170,
			198,
			79,
			184,
			56,
			210,
			150,
			164,
			125,
			182,
			118,
			252,
			107,
			226,
			156,
			116,
			4,
			241,
			69,
			157,
			112,
			89,
			100,
			113,
			135,
			32,
			134,
			91,
			207,
			101,
			230,
			45,
			168,
			2,
			27,
			96,
			37,
			173,
			174,
			176,
			185,
			246,
			28,
			70,
			97,
			105,
			52,
			64,
			126,
			15,
			85,
			71,
			163,
			35,
			221,
			81,
			175,
			58,
			195,
			92,
			249,
			206,
			186,
			197,
			234,
			38,
			44,
			83,
			13,
			110,
			133,
			40,
			132,
			9,
			211,
			223,
			205,
			244,
			65,
			129,
			77,
			82,
			106,
			220,
			55,
			200,
			108,
			193,
			171,
			250,
			36,
			225,
			123,
			8,
			12,
			189,
			177,
			74,
			120,
			136,
			149,
			139,
			227,
			99,
			232,
			109,
			233,
			203,
			213,
			254,
			59,
			0,
			29,
			57,
			242,
			239,
			183,
			14,
			102,
			88,
			208,
			228,
			166,
			119,
			114,
			248,
			235,
			117,
			75,
			10,
			49,
			68,
			80,
			180,
			143,
			237,
			31,
			26,
			219,
			153,
			141,
			51,
			159,
			17,
			131,
			20
		};
	}
}
