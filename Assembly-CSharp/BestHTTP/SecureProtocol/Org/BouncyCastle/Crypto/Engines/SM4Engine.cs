using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000595 RID: 1429
	public class SM4Engine : IBlockCipher
	{
		// Token: 0x06003615 RID: 13845 RVA: 0x0014C9DC File Offset: 0x0014ABDC
		private static uint tau(uint A)
		{
			uint num = (uint)SM4Engine.Sbox[(int)(A >> 24)];
			uint num2 = (uint)SM4Engine.Sbox[(int)(A >> 16 & 255U)];
			uint num3 = (uint)SM4Engine.Sbox[(int)(A >> 8 & 255U)];
			uint num4 = (uint)SM4Engine.Sbox[(int)(A & 255U)];
			return num << 24 | num2 << 16 | num3 << 8 | num4;
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x0014CA30 File Offset: 0x0014AC30
		private static uint L_ap(uint B)
		{
			return B ^ Integers.RotateLeft(B, 13) ^ Integers.RotateLeft(B, 23);
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x0014CA45 File Offset: 0x0014AC45
		private uint T_ap(uint Z)
		{
			return SM4Engine.L_ap(SM4Engine.tau(Z));
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x0014CA54 File Offset: 0x0014AC54
		private void ExpandKey(bool forEncryption, byte[] key)
		{
			uint num = Pack.BE_To_UInt32(key, 0) ^ SM4Engine.FK[0];
			uint num2 = Pack.BE_To_UInt32(key, 4) ^ SM4Engine.FK[1];
			uint num3 = Pack.BE_To_UInt32(key, 8) ^ SM4Engine.FK[2];
			uint num4 = Pack.BE_To_UInt32(key, 12) ^ SM4Engine.FK[3];
			if (forEncryption)
			{
				this.rk[0] = (num ^ this.T_ap(num2 ^ num3 ^ num4 ^ SM4Engine.CK[0]));
				this.rk[1] = (num2 ^ this.T_ap(num3 ^ num4 ^ this.rk[0] ^ SM4Engine.CK[1]));
				this.rk[2] = (num3 ^ this.T_ap(num4 ^ this.rk[0] ^ this.rk[1] ^ SM4Engine.CK[2]));
				this.rk[3] = (num4 ^ this.T_ap(this.rk[0] ^ this.rk[1] ^ this.rk[2] ^ SM4Engine.CK[3]));
				for (int i = 4; i < 32; i++)
				{
					this.rk[i] = (this.rk[i - 4] ^ this.T_ap(this.rk[i - 3] ^ this.rk[i - 2] ^ this.rk[i - 1] ^ SM4Engine.CK[i]));
				}
				return;
			}
			this.rk[31] = (num ^ this.T_ap(num2 ^ num3 ^ num4 ^ SM4Engine.CK[0]));
			this.rk[30] = (num2 ^ this.T_ap(num3 ^ num4 ^ this.rk[31] ^ SM4Engine.CK[1]));
			this.rk[29] = (num3 ^ this.T_ap(num4 ^ this.rk[31] ^ this.rk[30] ^ SM4Engine.CK[2]));
			this.rk[28] = (num4 ^ this.T_ap(this.rk[31] ^ this.rk[30] ^ this.rk[29] ^ SM4Engine.CK[3]));
			for (int j = 27; j >= 0; j--)
			{
				this.rk[j] = (this.rk[j + 4] ^ this.T_ap(this.rk[j + 3] ^ this.rk[j + 2] ^ this.rk[j + 1] ^ SM4Engine.CK[31 - j]));
			}
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x0014CCA2 File Offset: 0x0014AEA2
		private static uint L(uint B)
		{
			return B ^ Integers.RotateLeft(B, 2) ^ Integers.RotateLeft(B, 10) ^ Integers.RotateLeft(B, 18) ^ Integers.RotateLeft(B, 24);
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x0014CCC8 File Offset: 0x0014AEC8
		private static uint T(uint Z)
		{
			return SM4Engine.L(SM4Engine.tau(Z));
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x0014CCD8 File Offset: 0x0014AED8
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			KeyParameter keyParameter = parameters as KeyParameter;
			if (keyParameter == null)
			{
				throw new ArgumentException("invalid parameter passed to SM4 init - " + Platform.GetTypeName(parameters), "parameters");
			}
			byte[] key = keyParameter.GetKey();
			if (key.Length != 16)
			{
				throw new ArgumentException("SM4 requires a 128 bit key", "parameters");
			}
			if (this.rk == null)
			{
				this.rk = new uint[32];
			}
			this.ExpandKey(forEncryption, key);
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x0600361C RID: 13852 RVA: 0x0014CD45 File Offset: 0x0014AF45
		public virtual string AlgorithmName
		{
			get
			{
				return "SM4";
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x0600361D RID: 13853 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x0012AD29 File Offset: 0x00128F29
		public virtual int GetBlockSize()
		{
			return 16;
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x0014CD4C File Offset: 0x0014AF4C
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (this.rk == null)
			{
				throw new InvalidOperationException("SM4 not initialised");
			}
			Check.DataLength(input, inOff, 16, "input buffer too short");
			Check.OutputLength(output, outOff, 16, "output buffer too short");
			uint num = Pack.BE_To_UInt32(input, inOff);
			uint num2 = Pack.BE_To_UInt32(input, inOff + 4);
			uint num3 = Pack.BE_To_UInt32(input, inOff + 8);
			uint num4 = Pack.BE_To_UInt32(input, inOff + 12);
			for (int i = 0; i < 32; i += 4)
			{
				num ^= SM4Engine.T(num2 ^ num3 ^ num4 ^ this.rk[i]);
				num2 ^= SM4Engine.T(num3 ^ num4 ^ num ^ this.rk[i + 1]);
				num3 ^= SM4Engine.T(num4 ^ num ^ num2 ^ this.rk[i + 2]);
				num4 ^= SM4Engine.T(num ^ num2 ^ num3 ^ this.rk[i + 3]);
			}
			Pack.UInt32_To_BE(num4, output, outOff);
			Pack.UInt32_To_BE(num3, output, outOff + 4);
			Pack.UInt32_To_BE(num2, output, outOff + 8);
			Pack.UInt32_To_BE(num, output, outOff + 12);
			return 16;
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void Reset()
		{
		}

		// Token: 0x04002340 RID: 9024
		private const int BlockSize = 16;

		// Token: 0x04002341 RID: 9025
		private static readonly byte[] Sbox = new byte[]
		{
			214,
			144,
			233,
			254,
			204,
			225,
			61,
			183,
			22,
			182,
			20,
			194,
			40,
			251,
			44,
			5,
			43,
			103,
			154,
			118,
			42,
			190,
			4,
			195,
			170,
			68,
			19,
			38,
			73,
			134,
			6,
			153,
			156,
			66,
			80,
			244,
			145,
			239,
			152,
			122,
			51,
			84,
			11,
			67,
			237,
			207,
			172,
			98,
			228,
			179,
			28,
			169,
			201,
			8,
			232,
			149,
			128,
			223,
			148,
			250,
			117,
			143,
			63,
			166,
			71,
			7,
			167,
			252,
			243,
			115,
			23,
			186,
			131,
			89,
			60,
			25,
			230,
			133,
			79,
			168,
			104,
			107,
			129,
			178,
			113,
			100,
			218,
			139,
			248,
			235,
			15,
			75,
			112,
			86,
			157,
			53,
			30,
			36,
			14,
			94,
			99,
			88,
			209,
			162,
			37,
			34,
			124,
			59,
			1,
			33,
			120,
			135,
			212,
			0,
			70,
			87,
			159,
			211,
			39,
			82,
			76,
			54,
			2,
			231,
			160,
			196,
			200,
			158,
			234,
			191,
			138,
			210,
			64,
			199,
			56,
			181,
			163,
			247,
			242,
			206,
			249,
			97,
			21,
			161,
			224,
			174,
			93,
			164,
			155,
			52,
			26,
			85,
			173,
			147,
			50,
			48,
			245,
			140,
			177,
			227,
			29,
			246,
			226,
			46,
			130,
			102,
			202,
			96,
			192,
			41,
			35,
			171,
			13,
			83,
			78,
			111,
			213,
			219,
			55,
			69,
			222,
			253,
			142,
			47,
			3,
			byte.MaxValue,
			106,
			114,
			109,
			108,
			91,
			81,
			141,
			27,
			175,
			146,
			187,
			221,
			188,
			127,
			17,
			217,
			92,
			65,
			31,
			16,
			90,
			216,
			10,
			193,
			49,
			136,
			165,
			205,
			123,
			189,
			45,
			116,
			208,
			18,
			184,
			229,
			180,
			176,
			137,
			105,
			151,
			74,
			12,
			150,
			119,
			126,
			101,
			185,
			241,
			9,
			197,
			110,
			198,
			132,
			24,
			240,
			125,
			236,
			58,
			220,
			77,
			32,
			121,
			238,
			95,
			62,
			215,
			203,
			57,
			72
		};

		// Token: 0x04002342 RID: 9026
		private static readonly uint[] CK = new uint[]
		{
			462357U,
			472066609U,
			943670861U,
			1415275113U,
			1886879365U,
			2358483617U,
			2830087869U,
			3301692121U,
			3773296373U,
			4228057617U,
			404694573U,
			876298825U,
			1347903077U,
			1819507329U,
			2291111581U,
			2762715833U,
			3234320085U,
			3705924337U,
			4177462797U,
			337322537U,
			808926789U,
			1280531041U,
			1752135293U,
			2223739545U,
			2695343797U,
			3166948049U,
			3638552301U,
			4110090761U,
			269950501U,
			741554753U,
			1213159005U,
			1684763257U
		};

		// Token: 0x04002343 RID: 9027
		private static readonly uint[] FK = new uint[]
		{
			2746333894U,
			1453994832U,
			1736282519U,
			2993693404U
		};

		// Token: 0x04002344 RID: 9028
		private uint[] rk;
	}
}
