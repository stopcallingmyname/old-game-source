using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005A4 RID: 1444
	public class Dstu7564Digest : IDigest, IMemoable
	{
		// Token: 0x060036D4 RID: 14036 RVA: 0x00153D91 File Offset: 0x00151F91
		public Dstu7564Digest(Dstu7564Digest digest)
		{
			this.CopyIn(digest);
		}

		// Token: 0x060036D5 RID: 14037 RVA: 0x00153DA0 File Offset: 0x00151FA0
		private void CopyIn(Dstu7564Digest digest)
		{
			this.hashSize = digest.hashSize;
			this.blockSize = digest.blockSize;
			this.rounds = digest.rounds;
			if (this.columns > 0 && this.columns == digest.columns)
			{
				Array.Copy(digest.state, 0, this.state, 0, this.columns);
				Array.Copy(digest.buf, 0, this.buf, 0, this.blockSize);
			}
			else
			{
				this.columns = digest.columns;
				this.state = Arrays.Clone(digest.state);
				this.tempState1 = new ulong[this.columns];
				this.tempState2 = new ulong[this.columns];
				this.buf = Arrays.Clone(digest.buf);
			}
			this.inputBlocks = digest.inputBlocks;
			this.bufOff = digest.bufOff;
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x00153E84 File Offset: 0x00152084
		public Dstu7564Digest(int hashSizeBits)
		{
			if (hashSizeBits == 256 || hashSizeBits == 384 || hashSizeBits == 512)
			{
				this.hashSize = hashSizeBits / 8;
				if (hashSizeBits > 256)
				{
					this.columns = 16;
					this.rounds = 14;
				}
				else
				{
					this.columns = 8;
					this.rounds = 10;
				}
				this.blockSize = this.columns << 3;
				this.state = new ulong[this.columns];
				this.state[0] = (ulong)((long)this.blockSize);
				this.tempState1 = new ulong[this.columns];
				this.tempState2 = new ulong[this.columns];
				this.buf = new byte[this.blockSize];
				return;
			}
			throw new ArgumentException("Hash size is not recommended. Use 256/384/512 instead");
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060036D7 RID: 14039 RVA: 0x00153F4F File Offset: 0x0015214F
		public virtual string AlgorithmName
		{
			get
			{
				return "DSTU7564";
			}
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x00153F56 File Offset: 0x00152156
		public virtual int GetDigestSize()
		{
			return this.hashSize;
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x00153F5E File Offset: 0x0015215E
		public virtual int GetByteLength()
		{
			return this.blockSize;
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x00153F68 File Offset: 0x00152168
		public virtual void Update(byte input)
		{
			byte[] array = this.buf;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array[num] = input;
			if (this.bufOff == this.blockSize)
			{
				this.ProcessBlock(this.buf, 0);
				this.bufOff = 0;
				this.inputBlocks += 1UL;
			}
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x00153FC0 File Offset: 0x001521C0
		public virtual void BlockUpdate(byte[] input, int inOff, int length)
		{
			while (this.bufOff != 0 && length > 0)
			{
				this.Update(input[inOff++]);
				length--;
			}
			if (length > 0)
			{
				while (length >= this.blockSize)
				{
					this.ProcessBlock(input, inOff);
					inOff += this.blockSize;
					length -= this.blockSize;
					this.inputBlocks += 1UL;
				}
				while (length > 0)
				{
					this.Update(input[inOff++]);
					length--;
				}
			}
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x00154044 File Offset: 0x00152244
		public virtual int DoFinal(byte[] output, int outOff)
		{
			int num = this.bufOff;
			byte[] array = this.buf;
			int num2 = this.bufOff;
			this.bufOff = num2 + 1;
			array[num2] = 128;
			int num3 = this.blockSize - 12;
			if (this.bufOff > num3)
			{
				while (this.bufOff < this.blockSize)
				{
					byte[] array2 = this.buf;
					num2 = this.bufOff;
					this.bufOff = num2 + 1;
					array2[num2] = 0;
				}
				this.bufOff = 0;
				this.ProcessBlock(this.buf, 0);
			}
			while (this.bufOff < num3)
			{
				byte[] array3 = this.buf;
				num2 = this.bufOff;
				this.bufOff = num2 + 1;
				array3[num2] = 0;
			}
			ulong num4 = (this.inputBlocks & (ulong)-1) * (ulong)((long)this.blockSize) + (ulong)num << 3;
			Pack.UInt32_To_LE((uint)num4, this.buf, this.bufOff);
			this.bufOff += 4;
			Pack.UInt64_To_LE((num4 >> 32) + ((this.inputBlocks >> 32) * (ulong)((long)this.blockSize) << 3), this.buf, this.bufOff);
			this.ProcessBlock(this.buf, 0);
			Array.Copy(this.state, 0, this.tempState1, 0, this.columns);
			this.P(this.tempState1);
			for (int i = 0; i < this.columns; i++)
			{
				this.state[i] ^= this.tempState1[i];
			}
			int num5 = this.hashSize / 8;
			for (int j = this.columns - num5; j < this.columns; j++)
			{
				Pack.UInt64_To_LE(this.state[j], output, outOff);
				outOff += 8;
			}
			this.Reset();
			return this.hashSize;
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x001541EF File Offset: 0x001523EF
		public virtual void Reset()
		{
			Array.Clear(this.state, 0, this.state.Length);
			this.state[0] = (ulong)((long)this.blockSize);
			this.inputBlocks = 0UL;
			this.bufOff = 0;
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x00154224 File Offset: 0x00152424
		protected virtual void ProcessBlock(byte[] input, int inOff)
		{
			int num = inOff;
			for (int i = 0; i < this.columns; i++)
			{
				ulong num2 = Pack.LE_To_UInt64(input, num);
				num += 8;
				this.tempState1[i] = (this.state[i] ^ num2);
				this.tempState2[i] = num2;
			}
			this.P(this.tempState1);
			this.Q(this.tempState2);
			for (int j = 0; j < this.columns; j++)
			{
				this.state[j] ^= (this.tempState1[j] ^ this.tempState2[j]);
			}
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x001542B8 File Offset: 0x001524B8
		private void P(ulong[] s)
		{
			for (int i = 0; i < this.rounds; i++)
			{
				ulong num = (ulong)((long)i);
				for (int j = 0; j < this.columns; j++)
				{
					s[j] ^= num;
					num += 16UL;
				}
				this.ShiftRows(s);
				this.SubBytes(s);
				this.MixColumns(s);
			}
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x00154314 File Offset: 0x00152514
		private void Q(ulong[] s)
		{
			for (int i = 0; i < this.rounds; i++)
			{
				ulong num = (ulong)((long)(this.columns - 1 << 4 ^ i) << 56 | 67818912035696883L);
				for (int j = 0; j < this.columns; j++)
				{
					s[j] += num;
					num -= 1152921504606846976UL;
				}
				this.ShiftRows(s);
				this.SubBytes(s);
				this.MixColumns(s);
			}
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x0015438C File Offset: 0x0015258C
		private static ulong MixColumn(ulong c)
		{
			ulong num = (c & 9187201950435737471UL) << 1 ^ ((c & 9259542123273814144UL) >> 7) * 29UL;
			ulong num2 = Dstu7564Digest.Rotate(8, c) ^ c;
			num2 ^= Dstu7564Digest.Rotate(16, num2);
			num2 ^= Dstu7564Digest.Rotate(48, c);
			ulong num3 = num2 ^ c ^ num;
			num3 = ((num3 & 4557430888798830399UL) << 2 ^ ((num3 & 9259542123273814144UL) >> 6) * 29UL ^ ((num3 & 4629771061636907072UL) >> 6) * 29UL);
			return num2 ^ Dstu7564Digest.Rotate(32, num3) ^ Dstu7564Digest.Rotate(40, num) ^ Dstu7564Digest.Rotate(48, num);
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x00154430 File Offset: 0x00152630
		private void MixColumns(ulong[] s)
		{
			for (int i = 0; i < this.columns; i++)
			{
				s[i] = Dstu7564Digest.MixColumn(s[i]);
			}
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x00141AF7 File Offset: 0x0013FCF7
		private static ulong Rotate(int n, ulong x)
		{
			return x >> n | x << -n;
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x0015445C File Offset: 0x0015265C
		private void ShiftRows(ulong[] s)
		{
			int num = this.columns;
			if (num == 8)
			{
				ulong num2 = s[0];
				ulong num3 = s[1];
				ulong num4 = s[2];
				ulong num5 = s[3];
				ulong num6 = s[4];
				ulong num7 = s[5];
				ulong num8 = s[6];
				ulong num9 = s[7];
				ulong num10 = (num2 ^ num6) & 18446744069414584320UL;
				num2 ^= num10;
				num6 ^= num10;
				num10 = ((num3 ^ num7) & 72057594021150720UL);
				num3 ^= num10;
				num7 ^= num10;
				num10 = ((num4 ^ num8) & 281474976645120UL);
				num4 ^= num10;
				num8 ^= num10;
				num10 = ((num5 ^ num9) & 1099511627520UL);
				num5 ^= num10;
				num9 ^= num10;
				num10 = ((num2 ^ num4) & 18446462603027742720UL);
				num2 ^= num10;
				num4 ^= num10;
				num10 = ((num3 ^ num5) & 72056494543077120UL);
				num3 ^= num10;
				num5 ^= num10;
				num10 = ((num6 ^ num8) & 18446462603027742720UL);
				num6 ^= num10;
				num8 ^= num10;
				num10 = ((num7 ^ num9) & 72056494543077120UL);
				num7 ^= num10;
				num9 ^= num10;
				num10 = ((num2 ^ num3) & 18374966859414961920UL);
				num2 ^= num10;
				num3 ^= num10;
				num10 = ((num4 ^ num5) & 18374966859414961920UL);
				num4 ^= num10;
				num5 ^= num10;
				num10 = ((num6 ^ num7) & 18374966859414961920UL);
				num6 ^= num10;
				num7 ^= num10;
				num10 = ((num8 ^ num9) & 18374966859414961920UL);
				num8 ^= num10;
				num9 ^= num10;
				s[0] = num2;
				s[1] = num3;
				s[2] = num4;
				s[3] = num5;
				s[4] = num6;
				s[5] = num7;
				s[6] = num8;
				s[7] = num9;
				return;
			}
			if (num != 16)
			{
				throw new InvalidOperationException("unsupported state size: only 512/1024 are allowed");
			}
			ulong num11 = s[0];
			ulong num12 = s[1];
			ulong num13 = s[2];
			ulong num14 = s[3];
			ulong num15 = s[4];
			ulong num16 = s[5];
			ulong num17 = s[6];
			ulong num18 = s[7];
			ulong num19 = s[8];
			ulong num20 = s[9];
			ulong num21 = s[10];
			ulong num22 = s[11];
			ulong num23 = s[12];
			ulong num24 = s[13];
			ulong num25 = s[14];
			ulong num26 = s[15];
			ulong num27 = (num11 ^ num19) & 18374686479671623680UL;
			num11 ^= num27;
			num19 ^= num27;
			num27 = ((num12 ^ num20) & 18374686479671623680UL);
			num12 ^= num27;
			num20 ^= num27;
			num27 = ((num13 ^ num21) & 18446462598732840960UL);
			num13 ^= num27;
			num21 ^= num27;
			num27 = ((num14 ^ num22) & 18446742974197923840UL);
			num14 ^= num27;
			num22 ^= num27;
			num27 = ((num15 ^ num23) & 18446744069414584320UL);
			num15 ^= num27;
			num23 ^= num27;
			num27 = ((num16 ^ num24) & 72057594021150720UL);
			num16 ^= num27;
			num24 ^= num27;
			num27 = ((num17 ^ num25) & 72057594037862400UL);
			num17 ^= num27;
			num25 ^= num27;
			num27 = ((num18 ^ num26) & 72057594037927680UL);
			num18 ^= num27;
			num26 ^= num27;
			num27 = ((num11 ^ num15) & 72057589742960640UL);
			num11 ^= num27;
			num15 ^= num27;
			num27 = ((num12 ^ num16) & 18446744073692774400UL);
			num12 ^= num27;
			num16 ^= num27;
			num27 = ((num13 ^ num17) & 18374967954648268800UL);
			num13 ^= num27;
			num17 ^= num27;
			num27 = ((num14 ^ num18) & 18374687579183251200UL);
			num14 ^= num27;
			num18 ^= num27;
			num27 = ((num19 ^ num23) & 72057589742960640UL);
			num19 ^= num27;
			num23 ^= num27;
			num27 = ((num20 ^ num24) & 18446744073692774400UL);
			num20 ^= num27;
			num24 ^= num27;
			num27 = ((num21 ^ num25) & 18374967954648268800UL);
			num21 ^= num27;
			num25 ^= num27;
			num27 = ((num22 ^ num26) & 18374687579183251200UL);
			num22 ^= num27;
			num26 ^= num27;
			num27 = ((num11 ^ num13) & 18446462603027742720UL);
			num11 ^= num27;
			num13 ^= num27;
			num27 = ((num12 ^ num14) & 72056494543077120UL);
			num12 ^= num27;
			num14 ^= num27;
			num27 = ((num15 ^ num17) & 18446462603027742720UL);
			num15 ^= num27;
			num17 ^= num27;
			num27 = ((num16 ^ num18) & 72056494543077120UL);
			num16 ^= num27;
			num18 ^= num27;
			num27 = ((num19 ^ num21) & 18446462603027742720UL);
			num19 ^= num27;
			num21 ^= num27;
			num27 = ((num20 ^ num22) & 72056494543077120UL);
			num20 ^= num27;
			num22 ^= num27;
			num27 = ((num23 ^ num25) & 18446462603027742720UL);
			num23 ^= num27;
			num25 ^= num27;
			num27 = ((num24 ^ num26) & 72056494543077120UL);
			num24 ^= num27;
			num26 ^= num27;
			num27 = ((num11 ^ num12) & 18374966859414961920UL);
			num11 ^= num27;
			num12 ^= num27;
			num27 = ((num13 ^ num14) & 18374966859414961920UL);
			num13 ^= num27;
			num14 ^= num27;
			num27 = ((num15 ^ num16) & 18374966859414961920UL);
			num15 ^= num27;
			num16 ^= num27;
			num27 = ((num17 ^ num18) & 18374966859414961920UL);
			num17 ^= num27;
			num18 ^= num27;
			num27 = ((num19 ^ num20) & 18374966859414961920UL);
			num19 ^= num27;
			num20 ^= num27;
			num27 = ((num21 ^ num22) & 18374966859414961920UL);
			num21 ^= num27;
			num22 ^= num27;
			num27 = ((num23 ^ num24) & 18374966859414961920UL);
			num23 ^= num27;
			num24 ^= num27;
			num27 = ((num25 ^ num26) & 18374966859414961920UL);
			num25 ^= num27;
			num26 ^= num27;
			s[0] = num11;
			s[1] = num12;
			s[2] = num13;
			s[3] = num14;
			s[4] = num15;
			s[5] = num16;
			s[6] = num17;
			s[7] = num18;
			s[8] = num19;
			s[9] = num20;
			s[10] = num21;
			s[11] = num22;
			s[12] = num23;
			s[13] = num24;
			s[14] = num25;
			s[15] = num26;
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x00154AB8 File Offset: 0x00152CB8
		private void SubBytes(ulong[] s)
		{
			for (int i = 0; i < this.columns; i++)
			{
				ulong num = s[i];
				uint num2 = (uint)num;
				uint num3 = (uint)(num >> 32);
				uint num4 = (uint)Dstu7564Digest.S0[(int)(num2 & 255U)];
				byte b = Dstu7564Digest.S1[(int)(num2 >> 8 & 255U)];
				byte b2 = Dstu7564Digest.S2[(int)(num2 >> 16 & 255U)];
				byte b3 = Dstu7564Digest.S3[(int)(num2 >> 24)];
				num2 = (num4 | (uint)((uint)b << 8) | (uint)((uint)b2 << 16) | (uint)((uint)b3 << 24));
				uint num5 = (uint)Dstu7564Digest.S0[(int)(num3 & 255U)];
				byte b4 = Dstu7564Digest.S1[(int)(num3 >> 8 & 255U)];
				byte b5 = Dstu7564Digest.S2[(int)(num3 >> 16 & 255U)];
				byte b6 = Dstu7564Digest.S3[(int)(num3 >> 24)];
				num3 = (num5 | (uint)((uint)b4 << 8) | (uint)((uint)b5 << 16) | (uint)((uint)b6 << 24));
				s[i] = ((ulong)num2 | (ulong)num3 << 32);
			}
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x00154B8C File Offset: 0x00152D8C
		public virtual IMemoable Copy()
		{
			return new Dstu7564Digest(this);
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x00154B94 File Offset: 0x00152D94
		public virtual void Reset(IMemoable other)
		{
			Dstu7564Digest digest = (Dstu7564Digest)other;
			this.CopyIn(digest);
		}

		// Token: 0x040023DC RID: 9180
		private const int NB_512 = 8;

		// Token: 0x040023DD RID: 9181
		private const int NB_1024 = 16;

		// Token: 0x040023DE RID: 9182
		private const int NR_512 = 10;

		// Token: 0x040023DF RID: 9183
		private const int NR_1024 = 14;

		// Token: 0x040023E0 RID: 9184
		private int hashSize;

		// Token: 0x040023E1 RID: 9185
		private int blockSize;

		// Token: 0x040023E2 RID: 9186
		private int columns;

		// Token: 0x040023E3 RID: 9187
		private int rounds;

		// Token: 0x040023E4 RID: 9188
		private ulong[] state;

		// Token: 0x040023E5 RID: 9189
		private ulong[] tempState1;

		// Token: 0x040023E6 RID: 9190
		private ulong[] tempState2;

		// Token: 0x040023E7 RID: 9191
		private ulong inputBlocks;

		// Token: 0x040023E8 RID: 9192
		private int bufOff;

		// Token: 0x040023E9 RID: 9193
		private byte[] buf;

		// Token: 0x040023EA RID: 9194
		private static readonly byte[] S0 = new byte[]
		{
			168,
			67,
			95,
			6,
			107,
			117,
			108,
			89,
			113,
			223,
			135,
			149,
			23,
			240,
			216,
			9,
			109,
			243,
			29,
			203,
			201,
			77,
			44,
			175,
			121,
			224,
			151,
			253,
			111,
			75,
			69,
			57,
			62,
			221,
			163,
			79,
			180,
			182,
			154,
			14,
			31,
			191,
			21,
			225,
			73,
			210,
			147,
			198,
			146,
			114,
			158,
			97,
			209,
			99,
			250,
			238,
			244,
			25,
			213,
			173,
			88,
			164,
			187,
			161,
			220,
			242,
			131,
			55,
			66,
			228,
			122,
			50,
			156,
			204,
			171,
			74,
			143,
			110,
			4,
			39,
			46,
			231,
			226,
			90,
			150,
			22,
			35,
			43,
			194,
			101,
			102,
			15,
			188,
			169,
			71,
			65,
			52,
			72,
			252,
			183,
			106,
			136,
			165,
			83,
			134,
			249,
			91,
			219,
			56,
			123,
			195,
			30,
			34,
			51,
			36,
			40,
			54,
			199,
			178,
			59,
			142,
			119,
			186,
			245,
			20,
			159,
			8,
			85,
			155,
			76,
			254,
			96,
			92,
			218,
			24,
			70,
			205,
			125,
			33,
			176,
			63,
			27,
			137,
			byte.MaxValue,
			235,
			132,
			105,
			58,
			157,
			215,
			211,
			112,
			103,
			64,
			181,
			222,
			93,
			48,
			145,
			177,
			120,
			17,
			1,
			229,
			0,
			104,
			152,
			160,
			197,
			2,
			166,
			116,
			45,
			11,
			162,
			118,
			179,
			190,
			206,
			189,
			174,
			233,
			138,
			49,
			28,
			236,
			241,
			153,
			148,
			170,
			246,
			38,
			47,
			239,
			232,
			140,
			53,
			3,
			212,
			127,
			251,
			5,
			193,
			94,
			144,
			32,
			61,
			130,
			247,
			234,
			10,
			13,
			126,
			248,
			80,
			26,
			196,
			7,
			87,
			184,
			60,
			98,
			227,
			200,
			172,
			82,
			100,
			16,
			208,
			217,
			19,
			12,
			18,
			41,
			81,
			185,
			207,
			214,
			115,
			141,
			129,
			84,
			192,
			237,
			78,
			68,
			167,
			42,
			133,
			37,
			230,
			202,
			124,
			139,
			86,
			128
		};

		// Token: 0x040023EB RID: 9195
		private static readonly byte[] S1 = new byte[]
		{
			206,
			187,
			235,
			146,
			234,
			203,
			19,
			193,
			233,
			58,
			214,
			178,
			210,
			144,
			23,
			248,
			66,
			21,
			86,
			180,
			101,
			28,
			136,
			67,
			197,
			92,
			54,
			186,
			245,
			87,
			103,
			141,
			49,
			246,
			100,
			88,
			158,
			244,
			34,
			170,
			117,
			15,
			2,
			177,
			223,
			109,
			115,
			77,
			124,
			38,
			46,
			247,
			8,
			93,
			68,
			62,
			159,
			20,
			200,
			174,
			84,
			16,
			216,
			188,
			26,
			107,
			105,
			243,
			189,
			51,
			171,
			250,
			209,
			155,
			104,
			78,
			22,
			149,
			145,
			238,
			76,
			99,
			142,
			91,
			204,
			60,
			25,
			161,
			129,
			73,
			123,
			217,
			111,
			55,
			96,
			202,
			231,
			43,
			72,
			253,
			150,
			69,
			252,
			65,
			18,
			13,
			121,
			229,
			137,
			140,
			227,
			32,
			48,
			220,
			183,
			108,
			74,
			181,
			63,
			151,
			212,
			98,
			45,
			6,
			164,
			165,
			131,
			95,
			42,
			218,
			201,
			0,
			126,
			162,
			85,
			191,
			17,
			213,
			156,
			207,
			14,
			10,
			61,
			81,
			125,
			147,
			27,
			254,
			196,
			71,
			9,
			134,
			11,
			143,
			157,
			106,
			7,
			185,
			176,
			152,
			24,
			50,
			113,
			75,
			239,
			59,
			112,
			160,
			228,
			64,
			byte.MaxValue,
			195,
			169,
			230,
			120,
			249,
			139,
			70,
			128,
			30,
			56,
			225,
			184,
			168,
			224,
			12,
			35,
			118,
			29,
			37,
			36,
			5,
			241,
			110,
			148,
			40,
			154,
			132,
			232,
			163,
			79,
			119,
			211,
			133,
			226,
			82,
			242,
			130,
			80,
			122,
			47,
			116,
			83,
			179,
			97,
			175,
			57,
			53,
			222,
			205,
			31,
			153,
			172,
			173,
			114,
			44,
			221,
			208,
			135,
			190,
			94,
			166,
			236,
			4,
			198,
			3,
			52,
			251,
			219,
			89,
			182,
			194,
			1,
			240,
			90,
			237,
			167,
			102,
			33,
			127,
			138,
			39,
			199,
			192,
			41,
			215
		};

		// Token: 0x040023EC RID: 9196
		private static readonly byte[] S2 = new byte[]
		{
			147,
			217,
			154,
			181,
			152,
			34,
			69,
			252,
			186,
			106,
			223,
			2,
			159,
			220,
			81,
			89,
			74,
			23,
			43,
			194,
			148,
			244,
			187,
			163,
			98,
			228,
			113,
			212,
			205,
			112,
			22,
			225,
			73,
			60,
			192,
			216,
			92,
			155,
			173,
			133,
			83,
			161,
			122,
			200,
			45,
			224,
			209,
			114,
			166,
			44,
			196,
			227,
			118,
			120,
			183,
			180,
			9,
			59,
			14,
			65,
			76,
			222,
			178,
			144,
			37,
			165,
			215,
			3,
			17,
			0,
			195,
			46,
			146,
			239,
			78,
			18,
			157,
			125,
			203,
			53,
			16,
			213,
			79,
			158,
			77,
			169,
			85,
			198,
			208,
			123,
			24,
			151,
			211,
			54,
			230,
			72,
			86,
			129,
			143,
			119,
			204,
			156,
			185,
			226,
			172,
			184,
			47,
			21,
			164,
			124,
			218,
			56,
			30,
			11,
			5,
			214,
			20,
			110,
			108,
			126,
			102,
			253,
			177,
			229,
			96,
			175,
			94,
			51,
			135,
			201,
			240,
			93,
			109,
			63,
			136,
			141,
			199,
			247,
			29,
			233,
			236,
			237,
			128,
			41,
			39,
			207,
			153,
			168,
			80,
			15,
			55,
			36,
			40,
			48,
			149,
			210,
			62,
			91,
			64,
			131,
			179,
			105,
			87,
			31,
			7,
			28,
			138,
			188,
			32,
			235,
			206,
			142,
			171,
			238,
			49,
			162,
			115,
			249,
			202,
			58,
			26,
			251,
			13,
			193,
			254,
			250,
			242,
			111,
			189,
			150,
			221,
			67,
			82,
			182,
			8,
			243,
			174,
			190,
			25,
			137,
			50,
			38,
			176,
			234,
			75,
			100,
			132,
			130,
			107,
			245,
			121,
			191,
			1,
			95,
			117,
			99,
			27,
			35,
			61,
			104,
			42,
			101,
			232,
			145,
			246,
			byte.MaxValue,
			19,
			88,
			241,
			71,
			10,
			127,
			197,
			167,
			231,
			97,
			90,
			6,
			70,
			68,
			66,
			4,
			160,
			219,
			57,
			134,
			84,
			170,
			140,
			52,
			33,
			139,
			248,
			12,
			116,
			103
		};

		// Token: 0x040023ED RID: 9197
		private static readonly byte[] S3 = new byte[]
		{
			104,
			141,
			202,
			77,
			115,
			75,
			78,
			42,
			212,
			82,
			38,
			179,
			84,
			30,
			25,
			31,
			34,
			3,
			70,
			61,
			45,
			74,
			83,
			131,
			19,
			138,
			183,
			213,
			37,
			121,
			245,
			189,
			88,
			47,
			13,
			2,
			237,
			81,
			158,
			17,
			242,
			62,
			85,
			94,
			209,
			22,
			60,
			102,
			112,
			93,
			243,
			69,
			64,
			204,
			232,
			148,
			86,
			8,
			206,
			26,
			58,
			210,
			225,
			223,
			181,
			56,
			110,
			14,
			229,
			244,
			249,
			134,
			233,
			79,
			214,
			133,
			35,
			207,
			50,
			153,
			49,
			20,
			174,
			238,
			200,
			72,
			211,
			48,
			161,
			146,
			65,
			177,
			24,
			196,
			44,
			113,
			114,
			68,
			21,
			253,
			55,
			190,
			95,
			170,
			155,
			136,
			216,
			171,
			137,
			156,
			250,
			96,
			234,
			188,
			98,
			12,
			36,
			166,
			168,
			236,
			103,
			32,
			219,
			124,
			40,
			221,
			172,
			91,
			52,
			126,
			16,
			241,
			123,
			143,
			99,
			160,
			5,
			154,
			67,
			119,
			33,
			191,
			39,
			9,
			195,
			159,
			182,
			215,
			41,
			194,
			235,
			192,
			164,
			139,
			140,
			29,
			251,
			byte.MaxValue,
			193,
			178,
			151,
			46,
			248,
			101,
			246,
			117,
			7,
			4,
			73,
			51,
			228,
			217,
			185,
			208,
			66,
			199,
			108,
			144,
			0,
			142,
			111,
			80,
			1,
			197,
			218,
			71,
			63,
			205,
			105,
			162,
			226,
			122,
			167,
			198,
			147,
			15,
			10,
			6,
			230,
			43,
			150,
			163,
			28,
			175,
			106,
			18,
			132,
			57,
			231,
			176,
			130,
			247,
			254,
			157,
			135,
			92,
			129,
			53,
			222,
			180,
			165,
			252,
			128,
			239,
			203,
			187,
			107,
			118,
			186,
			90,
			125,
			120,
			11,
			149,
			227,
			173,
			116,
			152,
			59,
			54,
			100,
			109,
			220,
			240,
			89,
			169,
			76,
			23,
			127,
			145,
			184,
			201,
			87,
			27,
			224,
			97
		};
	}
}
