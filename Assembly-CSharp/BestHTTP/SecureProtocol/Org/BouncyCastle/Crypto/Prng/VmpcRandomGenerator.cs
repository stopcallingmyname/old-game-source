using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004B7 RID: 1207
	public class VmpcRandomGenerator : IRandomGenerator
	{
		// Token: 0x06002F1E RID: 12062 RVA: 0x00123CD0 File Offset: 0x00121ED0
		public virtual void AddSeedMaterial(byte[] seed)
		{
			for (int i = 0; i < seed.Length; i++)
			{
				this.s = this.P[(int)(this.s + this.P[(int)(this.n & byte.MaxValue)] + seed[i] & byte.MaxValue)];
				byte b = this.P[(int)(this.n & byte.MaxValue)];
				this.P[(int)(this.n & byte.MaxValue)] = this.P[(int)(this.s & byte.MaxValue)];
				this.P[(int)(this.s & byte.MaxValue)] = b;
				this.n = (this.n + 1 & byte.MaxValue);
			}
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x00123D85 File Offset: 0x00121F85
		public virtual void AddSeedMaterial(long seed)
		{
			this.AddSeedMaterial(Pack.UInt64_To_BE((ulong)seed));
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x00123D93 File Offset: 0x00121F93
		public virtual void NextBytes(byte[] bytes)
		{
			this.NextBytes(bytes, 0, bytes.Length);
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x00123DA0 File Offset: 0x00121FA0
		public virtual void NextBytes(byte[] bytes, int start, int len)
		{
			byte[] p = this.P;
			lock (p)
			{
				int num = start + len;
				for (int num2 = start; num2 != num; num2++)
				{
					this.s = this.P[(int)(this.s + this.P[(int)(this.n & byte.MaxValue)] & byte.MaxValue)];
					bytes[num2] = this.P[(int)(this.P[(int)(this.P[(int)(this.s & byte.MaxValue)] & byte.MaxValue)] + 1 & byte.MaxValue)];
					byte b = this.P[(int)(this.n & byte.MaxValue)];
					this.P[(int)(this.n & byte.MaxValue)] = this.P[(int)(this.s & byte.MaxValue)];
					this.P[(int)(this.s & byte.MaxValue)] = b;
					this.n = (this.n + 1 & byte.MaxValue);
				}
			}
		}

		// Token: 0x04001F72 RID: 8050
		private byte n;

		// Token: 0x04001F73 RID: 8051
		private byte[] P = new byte[]
		{
			187,
			44,
			98,
			127,
			181,
			170,
			212,
			13,
			129,
			254,
			178,
			130,
			203,
			160,
			161,
			8,
			24,
			113,
			86,
			232,
			73,
			2,
			16,
			196,
			222,
			53,
			165,
			236,
			128,
			18,
			184,
			105,
			218,
			47,
			117,
			204,
			162,
			9,
			54,
			3,
			97,
			45,
			253,
			224,
			221,
			5,
			67,
			144,
			173,
			200,
			225,
			175,
			87,
			155,
			76,
			216,
			81,
			174,
			80,
			133,
			60,
			10,
			228,
			243,
			156,
			38,
			35,
			83,
			201,
			131,
			151,
			70,
			177,
			153,
			100,
			49,
			119,
			213,
			29,
			214,
			120,
			189,
			94,
			176,
			138,
			34,
			56,
			248,
			104,
			43,
			42,
			197,
			211,
			247,
			188,
			111,
			223,
			4,
			229,
			149,
			62,
			37,
			134,
			166,
			11,
			143,
			241,
			36,
			14,
			215,
			64,
			179,
			207,
			126,
			6,
			21,
			154,
			77,
			28,
			163,
			219,
			50,
			146,
			88,
			17,
			39,
			244,
			89,
			208,
			78,
			106,
			23,
			91,
			172,
			byte.MaxValue,
			7,
			192,
			101,
			121,
			252,
			199,
			205,
			118,
			66,
			93,
			231,
			58,
			52,
			122,
			48,
			40,
			15,
			115,
			1,
			249,
			209,
			210,
			25,
			233,
			145,
			185,
			90,
			237,
			65,
			109,
			180,
			195,
			158,
			191,
			99,
			250,
			31,
			51,
			96,
			71,
			137,
			240,
			150,
			26,
			95,
			147,
			61,
			55,
			75,
			217,
			168,
			193,
			27,
			246,
			57,
			139,
			183,
			12,
			32,
			206,
			136,
			110,
			182,
			116,
			142,
			141,
			22,
			41,
			242,
			135,
			245,
			235,
			112,
			227,
			251,
			85,
			159,
			198,
			68,
			74,
			69,
			125,
			226,
			107,
			92,
			108,
			102,
			169,
			140,
			238,
			132,
			19,
			167,
			30,
			157,
			220,
			103,
			72,
			186,
			46,
			230,
			164,
			171,
			124,
			148,
			0,
			33,
			239,
			234,
			190,
			202,
			114,
			79,
			82,
			152,
			63,
			194,
			20,
			123,
			59,
			84
		};

		// Token: 0x04001F74 RID: 8052
		private byte s = 190;
	}
}
