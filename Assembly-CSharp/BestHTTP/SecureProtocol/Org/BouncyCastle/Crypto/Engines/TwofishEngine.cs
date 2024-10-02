﻿using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000599 RID: 1433
	public sealed class TwofishEngine : IBlockCipher
	{
		// Token: 0x06003642 RID: 13890 RVA: 0x0014FB18 File Offset: 0x0014DD18
		public TwofishEngine()
		{
			int[] array = new int[2];
			int[] array2 = new int[2];
			int[] array3 = new int[2];
			for (int i = 0; i < 256; i++)
			{
				int num = (int)(TwofishEngine.P[0, i] & byte.MaxValue);
				array[0] = num;
				array2[0] = (this.Mx_X(num) & 255);
				array3[0] = (this.Mx_Y(num) & 255);
				num = (int)(TwofishEngine.P[1, i] & byte.MaxValue);
				array[1] = num;
				array2[1] = (this.Mx_X(num) & 255);
				array3[1] = (this.Mx_Y(num) & 255);
				this.gMDS0[i] = (array[1] | array2[1] << 8 | array3[1] << 16 | array3[1] << 24);
				this.gMDS1[i] = (array3[0] | array3[0] << 8 | array2[0] << 16 | array[0] << 24);
				this.gMDS2[i] = (array2[1] | array3[1] << 8 | array[1] << 16 | array3[1] << 24);
				this.gMDS3[i] = (array2[0] | array[0] << 8 | array3[0] << 16 | array2[0] << 24);
			}
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x0014FC8C File Offset: 0x0014DE8C
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("invalid parameter passed to Twofish init - " + Platform.GetTypeName(parameters));
			}
			this.encrypting = forEncryption;
			this.workingKey = ((KeyParameter)parameters).GetKey();
			this.k64Cnt = this.workingKey.Length / 8;
			this.SetKey(this.workingKey);
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06003644 RID: 13892 RVA: 0x0014FCEB File Offset: 0x0014DEEB
		public string AlgorithmName
		{
			get
			{
				return "Twofish";
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06003645 RID: 13893 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x0014FCF4 File Offset: 0x0014DEF4
		public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (this.workingKey == null)
			{
				throw new InvalidOperationException("Twofish not initialised");
			}
			Check.DataLength(input, inOff, 16, "input buffer too short");
			Check.OutputLength(output, outOff, 16, "output buffer too short");
			if (this.encrypting)
			{
				this.EncryptBlock(input, inOff, output, outOff);
			}
			else
			{
				this.DecryptBlock(input, inOff, output, outOff);
			}
			return 16;
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x0014FD53 File Offset: 0x0014DF53
		public void Reset()
		{
			if (this.workingKey != null)
			{
				this.SetKey(this.workingKey);
			}
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x0012AD29 File Offset: 0x00128F29
		public int GetBlockSize()
		{
			return 16;
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x0014FD6C File Offset: 0x0014DF6C
		private void SetKey(byte[] key)
		{
			int[] array = new int[4];
			int[] array2 = new int[4];
			int[] array3 = new int[4];
			this.gSubKeys = new int[40];
			if (this.k64Cnt < 1)
			{
				throw new ArgumentException("Key size less than 64 bits");
			}
			if (this.k64Cnt > 4)
			{
				throw new ArgumentException("Key size larger than 256 bits");
			}
			for (int i = 0; i < this.k64Cnt; i++)
			{
				int num = i * 8;
				array[i] = this.BytesTo32Bits(key, num);
				array2[i] = this.BytesTo32Bits(key, num + 4);
				array3[this.k64Cnt - 1 - i] = this.RS_MDS_Encode(array[i], array2[i]);
			}
			for (int j = 0; j < 20; j++)
			{
				int num2 = j * 33686018;
				int num3 = this.F32(num2, array);
				int num4 = this.F32(num2 + 16843009, array2);
				num4 = (num4 << 8 | (int)((uint)num4 >> 24));
				num3 += num4;
				this.gSubKeys[j * 2] = num3;
				num3 += num4;
				this.gSubKeys[j * 2 + 1] = (num3 << 9 | (int)((uint)num3 >> 23));
			}
			int x = array3[0];
			int x2 = array3[1];
			int x3 = array3[2];
			int x4 = array3[3];
			this.gSBox = new int[1024];
			int k = 0;
			while (k < 256)
			{
				int num8;
				int num7;
				int num6;
				int num5 = num6 = (num7 = (num8 = k));
				switch (this.k64Cnt & 3)
				{
				case 0:
					num6 = ((int)(TwofishEngine.P[1, num6] & byte.MaxValue) ^ this.M_b0(x4));
					num5 = ((int)(TwofishEngine.P[0, num5] & byte.MaxValue) ^ this.M_b1(x4));
					num7 = ((int)(TwofishEngine.P[0, num7] & byte.MaxValue) ^ this.M_b2(x4));
					num8 = ((int)(TwofishEngine.P[1, num8] & byte.MaxValue) ^ this.M_b3(x4));
					goto IL_2B4;
				case 1:
					this.gSBox[k * 2] = this.gMDS0[(int)(TwofishEngine.P[0, num6] & byte.MaxValue) ^ this.M_b0(x)];
					this.gSBox[k * 2 + 1] = this.gMDS1[(int)(TwofishEngine.P[0, num5] & byte.MaxValue) ^ this.M_b1(x)];
					this.gSBox[k * 2 + 512] = this.gMDS2[(int)(TwofishEngine.P[1, num7] & byte.MaxValue) ^ this.M_b2(x)];
					this.gSBox[k * 2 + 513] = this.gMDS3[(int)(TwofishEngine.P[1, num8] & byte.MaxValue) ^ this.M_b3(x)];
					break;
				case 2:
					goto IL_32C;
				case 3:
					goto IL_2B4;
				}
				IL_45A:
				k++;
				continue;
				IL_32C:
				this.gSBox[k * 2] = this.gMDS0[(int)(TwofishEngine.P[0, (int)(TwofishEngine.P[0, num6] & byte.MaxValue) ^ this.M_b0(x2)] & byte.MaxValue) ^ this.M_b0(x)];
				this.gSBox[k * 2 + 1] = this.gMDS1[(int)(TwofishEngine.P[0, (int)(TwofishEngine.P[1, num5] & byte.MaxValue) ^ this.M_b1(x2)] & byte.MaxValue) ^ this.M_b1(x)];
				this.gSBox[k * 2 + 512] = this.gMDS2[(int)(TwofishEngine.P[1, (int)(TwofishEngine.P[0, num7] & byte.MaxValue) ^ this.M_b2(x2)] & byte.MaxValue) ^ this.M_b2(x)];
				this.gSBox[k * 2 + 513] = this.gMDS3[(int)(TwofishEngine.P[1, (int)(TwofishEngine.P[1, num8] & byte.MaxValue) ^ this.M_b3(x2)] & byte.MaxValue) ^ this.M_b3(x)];
				goto IL_45A;
				IL_2B4:
				num6 = ((int)(TwofishEngine.P[1, num6] & byte.MaxValue) ^ this.M_b0(x3));
				num5 = ((int)(TwofishEngine.P[1, num5] & byte.MaxValue) ^ this.M_b1(x3));
				num7 = ((int)(TwofishEngine.P[0, num7] & byte.MaxValue) ^ this.M_b2(x3));
				num8 = ((int)(TwofishEngine.P[0, num8] & byte.MaxValue) ^ this.M_b3(x3));
				goto IL_32C;
			}
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x001501E8 File Offset: 0x0014E3E8
		private void EncryptBlock(byte[] src, int srcIndex, byte[] dst, int dstIndex)
		{
			int num = this.BytesTo32Bits(src, srcIndex) ^ this.gSubKeys[0];
			int num2 = this.BytesTo32Bits(src, srcIndex + 4) ^ this.gSubKeys[1];
			int num3 = this.BytesTo32Bits(src, srcIndex + 8) ^ this.gSubKeys[2];
			int num4 = this.BytesTo32Bits(src, srcIndex + 12) ^ this.gSubKeys[3];
			int num5 = 8;
			for (int i = 0; i < 16; i += 2)
			{
				int num6 = this.Fe32_0(num);
				int num7 = this.Fe32_3(num2);
				num3 ^= num6 + num7 + this.gSubKeys[num5++];
				num3 = (int)((uint)num3 >> 1 | (uint)((uint)num3 << 31));
				num4 = ((num4 << 1 | (int)((uint)num4 >> 31)) ^ num6 + 2 * num7 + this.gSubKeys[num5++]);
				num6 = this.Fe32_0(num3);
				num7 = this.Fe32_3(num4);
				num ^= num6 + num7 + this.gSubKeys[num5++];
				num = (int)((uint)num >> 1 | (uint)((uint)num << 31));
				num2 = ((num2 << 1 | (int)((uint)num2 >> 31)) ^ num6 + 2 * num7 + this.gSubKeys[num5++]);
			}
			this.Bits32ToBytes(num3 ^ this.gSubKeys[4], dst, dstIndex);
			this.Bits32ToBytes(num4 ^ this.gSubKeys[5], dst, dstIndex + 4);
			this.Bits32ToBytes(num ^ this.gSubKeys[6], dst, dstIndex + 8);
			this.Bits32ToBytes(num2 ^ this.gSubKeys[7], dst, dstIndex + 12);
		}

		// Token: 0x0600364B RID: 13899 RVA: 0x00150358 File Offset: 0x0014E558
		private void DecryptBlock(byte[] src, int srcIndex, byte[] dst, int dstIndex)
		{
			int num = this.BytesTo32Bits(src, srcIndex) ^ this.gSubKeys[4];
			int num2 = this.BytesTo32Bits(src, srcIndex + 4) ^ this.gSubKeys[5];
			int num3 = this.BytesTo32Bits(src, srcIndex + 8) ^ this.gSubKeys[6];
			int num4 = this.BytesTo32Bits(src, srcIndex + 12) ^ this.gSubKeys[7];
			int num5 = 39;
			for (int i = 0; i < 16; i += 2)
			{
				int num6 = this.Fe32_0(num);
				int num7 = this.Fe32_3(num2);
				num4 ^= num6 + 2 * num7 + this.gSubKeys[num5--];
				num3 = ((num3 << 1 | (int)((uint)num3 >> 31)) ^ num6 + num7 + this.gSubKeys[num5--]);
				num4 = (int)((uint)num4 >> 1 | (uint)((uint)num4 << 31));
				num6 = this.Fe32_0(num3);
				num7 = this.Fe32_3(num4);
				num2 ^= num6 + 2 * num7 + this.gSubKeys[num5--];
				num = ((num << 1 | (int)((uint)num >> 31)) ^ num6 + num7 + this.gSubKeys[num5--]);
				num2 = (int)((uint)num2 >> 1 | (uint)((uint)num2 << 31));
			}
			this.Bits32ToBytes(num3 ^ this.gSubKeys[0], dst, dstIndex);
			this.Bits32ToBytes(num4 ^ this.gSubKeys[1], dst, dstIndex + 4);
			this.Bits32ToBytes(num ^ this.gSubKeys[2], dst, dstIndex + 8);
			this.Bits32ToBytes(num2 ^ this.gSubKeys[3], dst, dstIndex + 12);
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x001504C8 File Offset: 0x0014E6C8
		private int F32(int x, int[] k32)
		{
			int num = this.M_b0(x);
			int num2 = this.M_b1(x);
			int num3 = this.M_b2(x);
			int num4 = this.M_b3(x);
			int x2 = k32[0];
			int x3 = k32[1];
			int x4 = k32[2];
			int x5 = k32[3];
			int result = 0;
			switch (this.k64Cnt & 3)
			{
			case 0:
				num = ((int)(TwofishEngine.P[1, num] & byte.MaxValue) ^ this.M_b0(x5));
				num2 = ((int)(TwofishEngine.P[0, num2] & byte.MaxValue) ^ this.M_b1(x5));
				num3 = ((int)(TwofishEngine.P[0, num3] & byte.MaxValue) ^ this.M_b2(x5));
				num4 = ((int)(TwofishEngine.P[1, num4] & byte.MaxValue) ^ this.M_b3(x5));
				break;
			case 1:
				return this.gMDS0[(int)(TwofishEngine.P[0, num] & byte.MaxValue) ^ this.M_b0(x2)] ^ this.gMDS1[(int)(TwofishEngine.P[0, num2] & byte.MaxValue) ^ this.M_b1(x2)] ^ this.gMDS2[(int)(TwofishEngine.P[1, num3] & byte.MaxValue) ^ this.M_b2(x2)] ^ this.gMDS3[(int)(TwofishEngine.P[1, num4] & byte.MaxValue) ^ this.M_b3(x2)];
			case 2:
				goto IL_1CF;
			case 3:
				break;
			default:
				return result;
			}
			num = ((int)(TwofishEngine.P[1, num] & byte.MaxValue) ^ this.M_b0(x4));
			num2 = ((int)(TwofishEngine.P[1, num2] & byte.MaxValue) ^ this.M_b1(x4));
			num3 = ((int)(TwofishEngine.P[0, num3] & byte.MaxValue) ^ this.M_b2(x4));
			num4 = ((int)(TwofishEngine.P[0, num4] & byte.MaxValue) ^ this.M_b3(x4));
			IL_1CF:
			result = (this.gMDS0[(int)(TwofishEngine.P[0, (int)(TwofishEngine.P[0, num] & byte.MaxValue) ^ this.M_b0(x3)] & byte.MaxValue) ^ this.M_b0(x2)] ^ this.gMDS1[(int)(TwofishEngine.P[0, (int)(TwofishEngine.P[1, num2] & byte.MaxValue) ^ this.M_b1(x3)] & byte.MaxValue) ^ this.M_b1(x2)] ^ this.gMDS2[(int)(TwofishEngine.P[1, (int)(TwofishEngine.P[0, num3] & byte.MaxValue) ^ this.M_b2(x3)] & byte.MaxValue) ^ this.M_b2(x2)] ^ this.gMDS3[(int)(TwofishEngine.P[1, (int)(TwofishEngine.P[1, num4] & byte.MaxValue) ^ this.M_b3(x3)] & byte.MaxValue) ^ this.M_b3(x2)]);
			return result;
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x0015079C File Offset: 0x0014E99C
		private int RS_MDS_Encode(int k0, int k1)
		{
			int num = k1;
			for (int i = 0; i < 4; i++)
			{
				num = this.RS_rem(num);
			}
			num ^= k0;
			for (int j = 0; j < 4; j++)
			{
				num = this.RS_rem(num);
			}
			return num;
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x001507D8 File Offset: 0x0014E9D8
		private int RS_rem(int x)
		{
			int num = (int)((uint)x >> 24 & 255U);
			int num2 = (num << 1 ^ (((num & 128) != 0) ? 333 : 0)) & 255;
			int num3 = (int)((uint)num >> 1 ^ (((num & 1) != 0) ? 166U : 0U) ^ (uint)num2);
			return x << 8 ^ num3 << 24 ^ num2 << 16 ^ num3 << 8 ^ num;
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x00150833 File Offset: 0x0014EA33
		private int LFSR1(int x)
		{
			return x >> 1 ^ (((x & 1) != 0) ? 180 : 0);
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x00150846 File Offset: 0x0014EA46
		private int LFSR2(int x)
		{
			return x >> 2 ^ (((x & 2) != 0) ? 180 : 0) ^ (((x & 1) != 0) ? 90 : 0);
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x00150864 File Offset: 0x0014EA64
		private int Mx_X(int x)
		{
			return x ^ this.LFSR2(x);
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x0015086F File Offset: 0x0014EA6F
		private int Mx_Y(int x)
		{
			return x ^ this.LFSR1(x) ^ this.LFSR2(x);
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x00150882 File Offset: 0x0014EA82
		private int M_b0(int x)
		{
			return x & 255;
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x0015088B File Offset: 0x0014EA8B
		private int M_b1(int x)
		{
			return (int)((uint)x >> 8 & 255U);
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x00150896 File Offset: 0x0014EA96
		private int M_b2(int x)
		{
			return (int)((uint)x >> 16 & 255U);
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x001508A2 File Offset: 0x0014EAA2
		private int M_b3(int x)
		{
			return (int)((uint)x >> 24 & 255U);
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x001508B0 File Offset: 0x0014EAB0
		private int Fe32_0(int x)
		{
			return this.gSBox[2 * (x & 255)] ^ this.gSBox[(int)(1U + 2U * ((uint)x >> 8 & 255U))] ^ this.gSBox[(int)(512U + 2U * ((uint)x >> 16 & 255U))] ^ this.gSBox[(int)(513U + 2U * ((uint)x >> 24 & 255U))];
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x00150918 File Offset: 0x0014EB18
		private int Fe32_3(int x)
		{
			return this.gSBox[(int)(2U * ((uint)x >> 24 & 255U))] ^ this.gSBox[1 + 2 * (x & 255)] ^ this.gSBox[(int)(512U + 2U * ((uint)x >> 8 & 255U))] ^ this.gSBox[(int)(513U + 2U * ((uint)x >> 16 & 255U))];
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x001462BD File Offset: 0x001444BD
		private int BytesTo32Bits(byte[] b, int p)
		{
			return (int)(b[p] & byte.MaxValue) | (int)(b[p + 1] & byte.MaxValue) << 8 | (int)(b[p + 2] & byte.MaxValue) << 16 | (int)(b[p + 3] & byte.MaxValue) << 24;
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x001462F4 File Offset: 0x001444F4
		private void Bits32ToBytes(int inData, byte[] b, int offset)
		{
			b[offset] = (byte)inData;
			b[offset + 1] = (byte)(inData >> 8);
			b[offset + 2] = (byte)(inData >> 16);
			b[offset + 3] = (byte)(inData >> 24);
		}

		// Token: 0x04002364 RID: 9060
		private static readonly byte[,] P = new byte[,]
		{
			{
				169,
				103,
				179,
				232,
				4,
				253,
				163,
				118,
				154,
				146,
				128,
				120,
				228,
				221,
				209,
				56,
				13,
				198,
				53,
				152,
				24,
				247,
				236,
				108,
				67,
				117,
				55,
				38,
				250,
				19,
				148,
				72,
				242,
				208,
				139,
				48,
				132,
				84,
				223,
				35,
				25,
				91,
				61,
				89,
				243,
				174,
				162,
				130,
				99,
				1,
				131,
				46,
				217,
				81,
				155,
				124,
				166,
				235,
				165,
				190,
				22,
				12,
				227,
				97,
				192,
				140,
				58,
				245,
				115,
				44,
				37,
				11,
				187,
				78,
				137,
				107,
				83,
				106,
				180,
				241,
				225,
				230,
				189,
				69,
				226,
				244,
				182,
				102,
				204,
				149,
				3,
				86,
				212,
				28,
				30,
				215,
				251,
				195,
				142,
				181,
				233,
				207,
				191,
				186,
				234,
				119,
				57,
				175,
				51,
				201,
				98,
				113,
				129,
				121,
				9,
				173,
				36,
				205,
				249,
				216,
				229,
				197,
				185,
				77,
				68,
				8,
				134,
				231,
				161,
				29,
				170,
				237,
				6,
				112,
				178,
				210,
				65,
				123,
				160,
				17,
				49,
				194,
				39,
				144,
				32,
				246,
				96,
				byte.MaxValue,
				150,
				92,
				177,
				171,
				158,
				156,
				82,
				27,
				95,
				147,
				10,
				239,
				145,
				133,
				73,
				238,
				45,
				79,
				143,
				59,
				71,
				135,
				109,
				70,
				214,
				62,
				105,
				100,
				42,
				206,
				203,
				47,
				252,
				151,
				5,
				122,
				172,
				127,
				213,
				26,
				75,
				14,
				167,
				90,
				40,
				20,
				63,
				41,
				136,
				60,
				76,
				2,
				184,
				218,
				176,
				23,
				85,
				31,
				138,
				125,
				87,
				199,
				141,
				116,
				183,
				196,
				159,
				114,
				126,
				21,
				34,
				18,
				88,
				7,
				153,
				52,
				110,
				80,
				222,
				104,
				101,
				188,
				219,
				248,
				200,
				168,
				43,
				64,
				220,
				254,
				50,
				164,
				202,
				16,
				33,
				240,
				211,
				93,
				15,
				0,
				111,
				157,
				54,
				66,
				74,
				94,
				193,
				224
			},
			{
				117,
				243,
				198,
				244,
				219,
				123,
				251,
				200,
				74,
				211,
				230,
				107,
				69,
				125,
				232,
				75,
				214,
				50,
				216,
				253,
				55,
				113,
				241,
				225,
				48,
				15,
				248,
				27,
				135,
				250,
				6,
				63,
				94,
				186,
				174,
				91,
				138,
				0,
				188,
				157,
				109,
				193,
				177,
				14,
				128,
				93,
				210,
				213,
				160,
				132,
				7,
				20,
				181,
				144,
				44,
				163,
				178,
				115,
				76,
				84,
				146,
				116,
				54,
				81,
				56,
				176,
				189,
				90,
				252,
				96,
				98,
				150,
				108,
				66,
				247,
				16,
				124,
				40,
				39,
				140,
				19,
				149,
				156,
				199,
				36,
				70,
				59,
				112,
				202,
				227,
				133,
				203,
				17,
				208,
				147,
				184,
				166,
				131,
				32,
				byte.MaxValue,
				159,
				119,
				195,
				204,
				3,
				111,
				8,
				191,
				64,
				231,
				43,
				226,
				121,
				12,
				170,
				130,
				65,
				58,
				234,
				185,
				228,
				154,
				164,
				151,
				126,
				218,
				122,
				23,
				102,
				148,
				161,
				29,
				61,
				240,
				222,
				179,
				11,
				114,
				167,
				28,
				239,
				209,
				83,
				62,
				143,
				51,
				38,
				95,
				236,
				118,
				42,
				73,
				129,
				136,
				238,
				33,
				196,
				26,
				235,
				217,
				197,
				57,
				153,
				205,
				173,
				49,
				139,
				1,
				24,
				35,
				221,
				31,
				78,
				45,
				249,
				72,
				79,
				242,
				101,
				142,
				120,
				92,
				88,
				25,
				141,
				229,
				152,
				87,
				103,
				127,
				5,
				100,
				175,
				99,
				182,
				254,
				245,
				183,
				60,
				165,
				206,
				233,
				104,
				68,
				224,
				77,
				67,
				105,
				41,
				46,
				172,
				21,
				89,
				168,
				10,
				158,
				110,
				71,
				223,
				52,
				53,
				106,
				207,
				220,
				34,
				201,
				192,
				155,
				137,
				212,
				237,
				171,
				18,
				162,
				13,
				82,
				187,
				2,
				47,
				169,
				215,
				97,
				30,
				180,
				80,
				4,
				246,
				194,
				22,
				37,
				134,
				86,
				85,
				9,
				190,
				145
			}
		};

		// Token: 0x04002365 RID: 9061
		private const int P_00 = 1;

		// Token: 0x04002366 RID: 9062
		private const int P_01 = 0;

		// Token: 0x04002367 RID: 9063
		private const int P_02 = 0;

		// Token: 0x04002368 RID: 9064
		private const int P_03 = 1;

		// Token: 0x04002369 RID: 9065
		private const int P_04 = 1;

		// Token: 0x0400236A RID: 9066
		private const int P_10 = 0;

		// Token: 0x0400236B RID: 9067
		private const int P_11 = 0;

		// Token: 0x0400236C RID: 9068
		private const int P_12 = 1;

		// Token: 0x0400236D RID: 9069
		private const int P_13 = 1;

		// Token: 0x0400236E RID: 9070
		private const int P_14 = 0;

		// Token: 0x0400236F RID: 9071
		private const int P_20 = 1;

		// Token: 0x04002370 RID: 9072
		private const int P_21 = 1;

		// Token: 0x04002371 RID: 9073
		private const int P_22 = 0;

		// Token: 0x04002372 RID: 9074
		private const int P_23 = 0;

		// Token: 0x04002373 RID: 9075
		private const int P_24 = 0;

		// Token: 0x04002374 RID: 9076
		private const int P_30 = 0;

		// Token: 0x04002375 RID: 9077
		private const int P_31 = 1;

		// Token: 0x04002376 RID: 9078
		private const int P_32 = 1;

		// Token: 0x04002377 RID: 9079
		private const int P_33 = 0;

		// Token: 0x04002378 RID: 9080
		private const int P_34 = 1;

		// Token: 0x04002379 RID: 9081
		private const int GF256_FDBK = 361;

		// Token: 0x0400237A RID: 9082
		private const int GF256_FDBK_2 = 180;

		// Token: 0x0400237B RID: 9083
		private const int GF256_FDBK_4 = 90;

		// Token: 0x0400237C RID: 9084
		private const int RS_GF_FDBK = 333;

		// Token: 0x0400237D RID: 9085
		private const int ROUNDS = 16;

		// Token: 0x0400237E RID: 9086
		private const int MAX_ROUNDS = 16;

		// Token: 0x0400237F RID: 9087
		private const int BLOCK_SIZE = 16;

		// Token: 0x04002380 RID: 9088
		private const int MAX_KEY_BITS = 256;

		// Token: 0x04002381 RID: 9089
		private const int INPUT_WHITEN = 0;

		// Token: 0x04002382 RID: 9090
		private const int OUTPUT_WHITEN = 4;

		// Token: 0x04002383 RID: 9091
		private const int ROUND_SUBKEYS = 8;

		// Token: 0x04002384 RID: 9092
		private const int TOTAL_SUBKEYS = 40;

		// Token: 0x04002385 RID: 9093
		private const int SK_STEP = 33686018;

		// Token: 0x04002386 RID: 9094
		private const int SK_BUMP = 16843009;

		// Token: 0x04002387 RID: 9095
		private const int SK_ROTL = 9;

		// Token: 0x04002388 RID: 9096
		private bool encrypting;

		// Token: 0x04002389 RID: 9097
		private int[] gMDS0 = new int[256];

		// Token: 0x0400238A RID: 9098
		private int[] gMDS1 = new int[256];

		// Token: 0x0400238B RID: 9099
		private int[] gMDS2 = new int[256];

		// Token: 0x0400238C RID: 9100
		private int[] gMDS3 = new int[256];

		// Token: 0x0400238D RID: 9101
		private int[] gSubKeys;

		// Token: 0x0400238E RID: 9102
		private int[] gSBox;

		// Token: 0x0400238F RID: 9103
		private int k64Cnt;

		// Token: 0x04002390 RID: 9104
		private byte[] workingKey;
	}
}
