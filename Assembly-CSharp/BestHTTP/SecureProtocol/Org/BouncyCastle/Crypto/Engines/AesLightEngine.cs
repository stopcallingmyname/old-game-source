using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000568 RID: 1384
	public class AesLightEngine : IBlockCipher
	{
		// Token: 0x060033F7 RID: 13303 RVA: 0x00137301 File Offset: 0x00135501
		private static uint Shift(uint r, int shift)
		{
			return r >> shift | r << 32 - shift;
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x00137313 File Offset: 0x00135513
		private static uint FFmulX(uint x)
		{
			return (x & 2139062143U) << 1 ^ ((x & 2155905152U) >> 7) * 27U;
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x001398FC File Offset: 0x00137AFC
		private static uint FFmulX2(uint x)
		{
			uint num = (x & 1061109567U) << 2;
			uint num2 = x & 3233857728U;
			num2 ^= num2 >> 1;
			return num ^ num2 >> 2 ^ num2 >> 5;
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x00139928 File Offset: 0x00137B28
		private static uint Mcol(uint x)
		{
			uint num = AesLightEngine.Shift(x, 8);
			uint num2 = x ^ num;
			return AesLightEngine.Shift(num2, 16) ^ num ^ AesLightEngine.FFmulX(num2);
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x00139954 File Offset: 0x00137B54
		private static uint Inv_Mcol(uint x)
		{
			uint num = x ^ AesLightEngine.Shift(x, 8);
			uint num2 = x ^ AesLightEngine.FFmulX(num);
			num ^= AesLightEngine.FFmulX2(num2);
			return num2 ^ (num ^ AesLightEngine.Shift(num, 16));
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x00139990 File Offset: 0x00137B90
		private static uint SubWord(uint x)
		{
			return (uint)((int)AesLightEngine.S[(int)(x & 255U)] | (int)AesLightEngine.S[(int)(x >> 8 & 255U)] << 8 | (int)AesLightEngine.S[(int)(x >> 16 & 255U)] << 16 | (int)AesLightEngine.S[(int)(x >> 24 & 255U)] << 24);
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x001399E4 File Offset: 0x00137BE4
		private uint[][] GenerateWorkingKey(byte[] key, bool forEncryption)
		{
			int num = key.Length;
			if (num < 16 || num > 32 || (num & 7) != 0)
			{
				throw new ArgumentException("Key length not 128/192/256 bits.");
			}
			int num2 = num >> 2;
			this.ROUNDS = num2 + 6;
			uint[][] array = new uint[this.ROUNDS + 1][];
			for (int i = 0; i <= this.ROUNDS; i++)
			{
				array[i] = new uint[4];
			}
			switch (num2)
			{
			case 4:
			{
				uint num3 = Pack.LE_To_UInt32(key, 0);
				array[0][0] = num3;
				uint num4 = Pack.LE_To_UInt32(key, 4);
				array[0][1] = num4;
				uint num5 = Pack.LE_To_UInt32(key, 8);
				array[0][2] = num5;
				uint num6 = Pack.LE_To_UInt32(key, 12);
				array[0][3] = num6;
				for (int j = 1; j <= 10; j++)
				{
					uint num7 = AesLightEngine.SubWord(AesLightEngine.Shift(num6, 8)) ^ (uint)AesLightEngine.rcon[j - 1];
					num3 ^= num7;
					array[j][0] = num3;
					num4 ^= num3;
					array[j][1] = num4;
					num5 ^= num4;
					array[j][2] = num5;
					num6 ^= num5;
					array[j][3] = num6;
				}
				goto IL_4EC;
			}
			case 6:
			{
				uint num8 = Pack.LE_To_UInt32(key, 0);
				array[0][0] = num8;
				uint num9 = Pack.LE_To_UInt32(key, 4);
				array[0][1] = num9;
				uint num10 = Pack.LE_To_UInt32(key, 8);
				array[0][2] = num10;
				uint num11 = Pack.LE_To_UInt32(key, 12);
				array[0][3] = num11;
				uint num12 = Pack.LE_To_UInt32(key, 16);
				array[1][0] = num12;
				uint num13 = Pack.LE_To_UInt32(key, 20);
				array[1][1] = num13;
				uint num14 = 1U;
				uint num15 = AesLightEngine.SubWord(AesLightEngine.Shift(num13, 8)) ^ num14;
				num14 <<= 1;
				num8 ^= num15;
				array[1][2] = num8;
				num9 ^= num8;
				array[1][3] = num9;
				num10 ^= num9;
				array[2][0] = num10;
				num11 ^= num10;
				array[2][1] = num11;
				num12 ^= num11;
				array[2][2] = num12;
				num13 ^= num12;
				array[2][3] = num13;
				for (int k = 3; k < 12; k += 3)
				{
					num15 = (AesLightEngine.SubWord(AesLightEngine.Shift(num13, 8)) ^ num14);
					num14 <<= 1;
					num8 ^= num15;
					array[k][0] = num8;
					num9 ^= num8;
					array[k][1] = num9;
					num10 ^= num9;
					array[k][2] = num10;
					num11 ^= num10;
					array[k][3] = num11;
					num12 ^= num11;
					array[k + 1][0] = num12;
					num13 ^= num12;
					array[k + 1][1] = num13;
					num15 = (AesLightEngine.SubWord(AesLightEngine.Shift(num13, 8)) ^ num14);
					num14 <<= 1;
					num8 ^= num15;
					array[k + 1][2] = num8;
					num9 ^= num8;
					array[k + 1][3] = num9;
					num10 ^= num9;
					array[k + 2][0] = num10;
					num11 ^= num10;
					array[k + 2][1] = num11;
					num12 ^= num11;
					array[k + 2][2] = num12;
					num13 ^= num12;
					array[k + 2][3] = num13;
				}
				num15 = (AesLightEngine.SubWord(AesLightEngine.Shift(num13, 8)) ^ num14);
				num8 ^= num15;
				array[12][0] = num8;
				num9 ^= num8;
				array[12][1] = num9;
				num10 ^= num9;
				array[12][2] = num10;
				num11 ^= num10;
				array[12][3] = num11;
				goto IL_4EC;
			}
			case 8:
			{
				uint num16 = Pack.LE_To_UInt32(key, 0);
				array[0][0] = num16;
				uint num17 = Pack.LE_To_UInt32(key, 4);
				array[0][1] = num17;
				uint num18 = Pack.LE_To_UInt32(key, 8);
				array[0][2] = num18;
				uint num19 = Pack.LE_To_UInt32(key, 12);
				array[0][3] = num19;
				uint num20 = Pack.LE_To_UInt32(key, 16);
				array[1][0] = num20;
				uint num21 = Pack.LE_To_UInt32(key, 20);
				array[1][1] = num21;
				uint num22 = Pack.LE_To_UInt32(key, 24);
				array[1][2] = num22;
				uint num23 = Pack.LE_To_UInt32(key, 28);
				array[1][3] = num23;
				uint num24 = 1U;
				uint num25;
				for (int l = 2; l < 14; l += 2)
				{
					num25 = (AesLightEngine.SubWord(AesLightEngine.Shift(num23, 8)) ^ num24);
					num24 <<= 1;
					num16 ^= num25;
					array[l][0] = num16;
					num17 ^= num16;
					array[l][1] = num17;
					num18 ^= num17;
					array[l][2] = num18;
					num19 ^= num18;
					array[l][3] = num19;
					num25 = AesLightEngine.SubWord(num19);
					num20 ^= num25;
					array[l + 1][0] = num20;
					num21 ^= num20;
					array[l + 1][1] = num21;
					num22 ^= num21;
					array[l + 1][2] = num22;
					num23 ^= num22;
					array[l + 1][3] = num23;
				}
				num25 = (AesLightEngine.SubWord(AesLightEngine.Shift(num23, 8)) ^ num24);
				num16 ^= num25;
				array[14][0] = num16;
				num17 ^= num16;
				array[14][1] = num17;
				num18 ^= num17;
				array[14][2] = num18;
				num19 ^= num18;
				array[14][3] = num19;
				goto IL_4EC;
			}
			}
			throw new InvalidOperationException("Should never get here");
			IL_4EC:
			if (!forEncryption)
			{
				for (int m = 1; m < this.ROUNDS; m++)
				{
					uint[] array2 = array[m];
					for (int n = 0; n < 4; n++)
					{
						array2[n] = AesLightEngine.Inv_Mcol(array2[n]);
					}
				}
			}
			return array;
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x00139F1C File Offset: 0x0013811C
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			KeyParameter keyParameter = parameters as KeyParameter;
			if (keyParameter == null)
			{
				throw new ArgumentException("invalid parameter passed to AES init - " + Platform.GetTypeName(parameters));
			}
			this.WorkingKey = this.GenerateWorkingKey(keyParameter.GetKey(), forEncryption);
			this.forEncryption = forEncryption;
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06003400 RID: 13312 RVA: 0x00137981 File Offset: 0x00135B81
		public virtual string AlgorithmName
		{
			get
			{
				return "AES";
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06003401 RID: 13313 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x0012AD29 File Offset: 0x00128F29
		public virtual int GetBlockSize()
		{
			return 16;
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x00139F64 File Offset: 0x00138164
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (this.WorkingKey == null)
			{
				throw new InvalidOperationException("AES engine not initialised");
			}
			Check.DataLength(input, inOff, 16, "input buffer too short");
			Check.OutputLength(output, outOff, 16, "output buffer too short");
			this.UnPackBlock(input, inOff);
			if (this.forEncryption)
			{
				this.EncryptBlock(this.WorkingKey);
			}
			else
			{
				this.DecryptBlock(this.WorkingKey);
			}
			this.PackBlock(output, outOff);
			return 16;
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void Reset()
		{
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x00139FD6 File Offset: 0x001381D6
		private void UnPackBlock(byte[] bytes, int off)
		{
			this.C0 = Pack.LE_To_UInt32(bytes, off);
			this.C1 = Pack.LE_To_UInt32(bytes, off + 4);
			this.C2 = Pack.LE_To_UInt32(bytes, off + 8);
			this.C3 = Pack.LE_To_UInt32(bytes, off + 12);
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x0013A013 File Offset: 0x00138213
		private void PackBlock(byte[] bytes, int off)
		{
			Pack.UInt32_To_LE(this.C0, bytes, off);
			Pack.UInt32_To_LE(this.C1, bytes, off + 4);
			Pack.UInt32_To_LE(this.C2, bytes, off + 8);
			Pack.UInt32_To_LE(this.C3, bytes, off + 12);
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x0013A050 File Offset: 0x00138250
		private void EncryptBlock(uint[][] KW)
		{
			uint[] array = KW[0];
			uint num = this.C0 ^ array[0];
			uint num2 = this.C1 ^ array[1];
			uint num3 = this.C2 ^ array[2];
			uint num4 = this.C3 ^ array[3];
			int i = 1;
			uint num5;
			uint num6;
			uint num7;
			while (i < this.ROUNDS - 1)
			{
				array = KW[i++];
				num5 = (AesLightEngine.Mcol((uint)((int)AesLightEngine.S[(int)(num & 255U)] ^ (int)AesLightEngine.S[(int)(num2 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num3 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num4 >> 24 & 255U)] << 24)) ^ array[0]);
				num6 = (AesLightEngine.Mcol((uint)((int)AesLightEngine.S[(int)(num2 & 255U)] ^ (int)AesLightEngine.S[(int)(num3 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num4 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num >> 24 & 255U)] << 24)) ^ array[1]);
				num7 = (AesLightEngine.Mcol((uint)((int)AesLightEngine.S[(int)(num3 & 255U)] ^ (int)AesLightEngine.S[(int)(num4 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num2 >> 24 & 255U)] << 24)) ^ array[2]);
				num4 = (AesLightEngine.Mcol((uint)((int)AesLightEngine.S[(int)(num4 & 255U)] ^ (int)AesLightEngine.S[(int)(num >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num2 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num3 >> 24 & 255U)] << 24)) ^ array[3]);
				array = KW[i++];
				num = (AesLightEngine.Mcol((uint)((int)AesLightEngine.S[(int)(num5 & 255U)] ^ (int)AesLightEngine.S[(int)(num6 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num7 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num4 >> 24 & 255U)] << 24)) ^ array[0]);
				num2 = (AesLightEngine.Mcol((uint)((int)AesLightEngine.S[(int)(num6 & 255U)] ^ (int)AesLightEngine.S[(int)(num7 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num4 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num5 >> 24 & 255U)] << 24)) ^ array[1]);
				num3 = (AesLightEngine.Mcol((uint)((int)AesLightEngine.S[(int)(num7 & 255U)] ^ (int)AesLightEngine.S[(int)(num4 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num5 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num6 >> 24 & 255U)] << 24)) ^ array[2]);
				num4 = (AesLightEngine.Mcol((uint)((int)AesLightEngine.S[(int)(num4 & 255U)] ^ (int)AesLightEngine.S[(int)(num5 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num6 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num7 >> 24 & 255U)] << 24)) ^ array[3]);
			}
			array = KW[i++];
			num5 = (AesLightEngine.Mcol((uint)((int)AesLightEngine.S[(int)(num & 255U)] ^ (int)AesLightEngine.S[(int)(num2 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num3 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num4 >> 24 & 255U)] << 24)) ^ array[0]);
			num6 = (AesLightEngine.Mcol((uint)((int)AesLightEngine.S[(int)(num2 & 255U)] ^ (int)AesLightEngine.S[(int)(num3 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num4 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num >> 24 & 255U)] << 24)) ^ array[1]);
			num7 = (AesLightEngine.Mcol((uint)((int)AesLightEngine.S[(int)(num3 & 255U)] ^ (int)AesLightEngine.S[(int)(num4 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num2 >> 24 & 255U)] << 24)) ^ array[2]);
			num4 = (AesLightEngine.Mcol((uint)((int)AesLightEngine.S[(int)(num4 & 255U)] ^ (int)AesLightEngine.S[(int)(num >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num2 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num3 >> 24 & 255U)] << 24)) ^ array[3]);
			array = KW[i];
			this.C0 = (uint)((int)AesLightEngine.S[(int)(num5 & 255U)] ^ (int)AesLightEngine.S[(int)(num6 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num7 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num4 >> 24 & 255U)] << 24 ^ (int)array[0]);
			this.C1 = (uint)((int)AesLightEngine.S[(int)(num6 & 255U)] ^ (int)AesLightEngine.S[(int)(num7 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num4 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num5 >> 24 & 255U)] << 24 ^ (int)array[1]);
			this.C2 = (uint)((int)AesLightEngine.S[(int)(num7 & 255U)] ^ (int)AesLightEngine.S[(int)(num4 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num5 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num6 >> 24 & 255U)] << 24 ^ (int)array[2]);
			this.C3 = (uint)((int)AesLightEngine.S[(int)(num4 & 255U)] ^ (int)AesLightEngine.S[(int)(num5 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.S[(int)(num6 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.S[(int)(num7 >> 24 & 255U)] << 24 ^ (int)array[3]);
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x0013A60C File Offset: 0x0013880C
		private void DecryptBlock(uint[][] KW)
		{
			uint[] array = KW[this.ROUNDS];
			uint num = this.C0 ^ array[0];
			uint num2 = this.C1 ^ array[1];
			uint num3 = this.C2 ^ array[2];
			uint num4 = this.C3 ^ array[3];
			int i = this.ROUNDS - 1;
			uint num5;
			uint num6;
			uint num7;
			while (i > 1)
			{
				array = KW[i--];
				num5 = (AesLightEngine.Inv_Mcol((uint)((int)AesLightEngine.Si[(int)(num & 255U)] ^ (int)AesLightEngine.Si[(int)(num4 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num3 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num2 >> 24 & 255U)] << 24)) ^ array[0]);
				num6 = (AesLightEngine.Inv_Mcol((uint)((int)AesLightEngine.Si[(int)(num2 & 255U)] ^ (int)AesLightEngine.Si[(int)(num >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num4 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num3 >> 24 & 255U)] << 24)) ^ array[1]);
				num7 = (AesLightEngine.Inv_Mcol((uint)((int)AesLightEngine.Si[(int)(num3 & 255U)] ^ (int)AesLightEngine.Si[(int)(num2 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num4 >> 24 & 255U)] << 24)) ^ array[2]);
				num4 = (AesLightEngine.Inv_Mcol((uint)((int)AesLightEngine.Si[(int)(num4 & 255U)] ^ (int)AesLightEngine.Si[(int)(num3 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num2 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num >> 24 & 255U)] << 24)) ^ array[3]);
				array = KW[i--];
				num = (AesLightEngine.Inv_Mcol((uint)((int)AesLightEngine.Si[(int)(num5 & 255U)] ^ (int)AesLightEngine.Si[(int)(num4 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num7 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num6 >> 24 & 255U)] << 24)) ^ array[0]);
				num2 = (AesLightEngine.Inv_Mcol((uint)((int)AesLightEngine.Si[(int)(num6 & 255U)] ^ (int)AesLightEngine.Si[(int)(num5 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num4 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num7 >> 24 & 255U)] << 24)) ^ array[1]);
				num3 = (AesLightEngine.Inv_Mcol((uint)((int)AesLightEngine.Si[(int)(num7 & 255U)] ^ (int)AesLightEngine.Si[(int)(num6 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num5 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num4 >> 24 & 255U)] << 24)) ^ array[2]);
				num4 = (AesLightEngine.Inv_Mcol((uint)((int)AesLightEngine.Si[(int)(num4 & 255U)] ^ (int)AesLightEngine.Si[(int)(num7 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num6 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num5 >> 24 & 255U)] << 24)) ^ array[3]);
			}
			array = KW[1];
			num5 = (AesLightEngine.Inv_Mcol((uint)((int)AesLightEngine.Si[(int)(num & 255U)] ^ (int)AesLightEngine.Si[(int)(num4 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num3 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num2 >> 24 & 255U)] << 24)) ^ array[0]);
			num6 = (AesLightEngine.Inv_Mcol((uint)((int)AesLightEngine.Si[(int)(num2 & 255U)] ^ (int)AesLightEngine.Si[(int)(num >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num4 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num3 >> 24 & 255U)] << 24)) ^ array[1]);
			num7 = (AesLightEngine.Inv_Mcol((uint)((int)AesLightEngine.Si[(int)(num3 & 255U)] ^ (int)AesLightEngine.Si[(int)(num2 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num4 >> 24 & 255U)] << 24)) ^ array[2]);
			num4 = (AesLightEngine.Inv_Mcol((uint)((int)AesLightEngine.Si[(int)(num4 & 255U)] ^ (int)AesLightEngine.Si[(int)(num3 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num2 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num >> 24 & 255U)] << 24)) ^ array[3]);
			array = KW[0];
			this.C0 = (uint)((int)AesLightEngine.Si[(int)(num5 & 255U)] ^ (int)AesLightEngine.Si[(int)(num4 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num7 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num6 >> 24 & 255U)] << 24 ^ (int)array[0]);
			this.C1 = (uint)((int)AesLightEngine.Si[(int)(num6 & 255U)] ^ (int)AesLightEngine.Si[(int)(num5 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num4 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num7 >> 24 & 255U)] << 24 ^ (int)array[1]);
			this.C2 = (uint)((int)AesLightEngine.Si[(int)(num7 & 255U)] ^ (int)AesLightEngine.Si[(int)(num6 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num5 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num4 >> 24 & 255U)] << 24 ^ (int)array[2]);
			this.C3 = (uint)((int)AesLightEngine.Si[(int)(num4 & 255U)] ^ (int)AesLightEngine.Si[(int)(num7 >> 8 & 255U)] << 8 ^ (int)AesLightEngine.Si[(int)(num6 >> 16 & 255U)] << 16 ^ (int)AesLightEngine.Si[(int)(num5 >> 24 & 255U)] << 24 ^ (int)array[3]);
		}

		// Token: 0x04002201 RID: 8705
		private static readonly byte[] S = new byte[]
		{
			99,
			124,
			119,
			123,
			242,
			107,
			111,
			197,
			48,
			1,
			103,
			43,
			254,
			215,
			171,
			118,
			202,
			130,
			201,
			125,
			250,
			89,
			71,
			240,
			173,
			212,
			162,
			175,
			156,
			164,
			114,
			192,
			183,
			253,
			147,
			38,
			54,
			63,
			247,
			204,
			52,
			165,
			229,
			241,
			113,
			216,
			49,
			21,
			4,
			199,
			35,
			195,
			24,
			150,
			5,
			154,
			7,
			18,
			128,
			226,
			235,
			39,
			178,
			117,
			9,
			131,
			44,
			26,
			27,
			110,
			90,
			160,
			82,
			59,
			214,
			179,
			41,
			227,
			47,
			132,
			83,
			209,
			0,
			237,
			32,
			252,
			177,
			91,
			106,
			203,
			190,
			57,
			74,
			76,
			88,
			207,
			208,
			239,
			170,
			251,
			67,
			77,
			51,
			133,
			69,
			249,
			2,
			127,
			80,
			60,
			159,
			168,
			81,
			163,
			64,
			143,
			146,
			157,
			56,
			245,
			188,
			182,
			218,
			33,
			16,
			byte.MaxValue,
			243,
			210,
			205,
			12,
			19,
			236,
			95,
			151,
			68,
			23,
			196,
			167,
			126,
			61,
			100,
			93,
			25,
			115,
			96,
			129,
			79,
			220,
			34,
			42,
			144,
			136,
			70,
			238,
			184,
			20,
			222,
			94,
			11,
			219,
			224,
			50,
			58,
			10,
			73,
			6,
			36,
			92,
			194,
			211,
			172,
			98,
			145,
			149,
			228,
			121,
			231,
			200,
			55,
			109,
			141,
			213,
			78,
			169,
			108,
			86,
			244,
			234,
			101,
			122,
			174,
			8,
			186,
			120,
			37,
			46,
			28,
			166,
			180,
			198,
			232,
			221,
			116,
			31,
			75,
			189,
			139,
			138,
			112,
			62,
			181,
			102,
			72,
			3,
			246,
			14,
			97,
			53,
			87,
			185,
			134,
			193,
			29,
			158,
			225,
			248,
			152,
			17,
			105,
			217,
			142,
			148,
			155,
			30,
			135,
			233,
			206,
			85,
			40,
			223,
			140,
			161,
			137,
			13,
			191,
			230,
			66,
			104,
			65,
			153,
			45,
			15,
			176,
			84,
			187,
			22
		};

		// Token: 0x04002202 RID: 8706
		private static readonly byte[] Si = new byte[]
		{
			82,
			9,
			106,
			213,
			48,
			54,
			165,
			56,
			191,
			64,
			163,
			158,
			129,
			243,
			215,
			251,
			124,
			227,
			57,
			130,
			155,
			47,
			byte.MaxValue,
			135,
			52,
			142,
			67,
			68,
			196,
			222,
			233,
			203,
			84,
			123,
			148,
			50,
			166,
			194,
			35,
			61,
			238,
			76,
			149,
			11,
			66,
			250,
			195,
			78,
			8,
			46,
			161,
			102,
			40,
			217,
			36,
			178,
			118,
			91,
			162,
			73,
			109,
			139,
			209,
			37,
			114,
			248,
			246,
			100,
			134,
			104,
			152,
			22,
			212,
			164,
			92,
			204,
			93,
			101,
			182,
			146,
			108,
			112,
			72,
			80,
			253,
			237,
			185,
			218,
			94,
			21,
			70,
			87,
			167,
			141,
			157,
			132,
			144,
			216,
			171,
			0,
			140,
			188,
			211,
			10,
			247,
			228,
			88,
			5,
			184,
			179,
			69,
			6,
			208,
			44,
			30,
			143,
			202,
			63,
			15,
			2,
			193,
			175,
			189,
			3,
			1,
			19,
			138,
			107,
			58,
			145,
			17,
			65,
			79,
			103,
			220,
			234,
			151,
			242,
			207,
			206,
			240,
			180,
			230,
			115,
			150,
			172,
			116,
			34,
			231,
			173,
			53,
			133,
			226,
			249,
			55,
			232,
			28,
			117,
			223,
			110,
			71,
			241,
			26,
			113,
			29,
			41,
			197,
			137,
			111,
			183,
			98,
			14,
			170,
			24,
			190,
			27,
			252,
			86,
			62,
			75,
			198,
			210,
			121,
			32,
			154,
			219,
			192,
			254,
			120,
			205,
			90,
			244,
			31,
			221,
			168,
			51,
			136,
			7,
			199,
			49,
			177,
			18,
			16,
			89,
			39,
			128,
			236,
			95,
			96,
			81,
			127,
			169,
			25,
			181,
			74,
			13,
			45,
			229,
			122,
			159,
			147,
			201,
			156,
			239,
			160,
			224,
			59,
			77,
			174,
			42,
			245,
			176,
			200,
			235,
			187,
			60,
			131,
			83,
			153,
			97,
			23,
			43,
			4,
			126,
			186,
			119,
			214,
			38,
			225,
			105,
			20,
			99,
			85,
			33,
			12,
			125
		};

		// Token: 0x04002203 RID: 8707
		private static readonly byte[] rcon = new byte[]
		{
			1,
			2,
			4,
			8,
			16,
			32,
			64,
			128,
			27,
			54,
			108,
			216,
			171,
			77,
			154,
			47,
			94,
			188,
			99,
			198,
			151,
			53,
			106,
			212,
			179,
			125,
			250,
			239,
			197,
			145
		};

		// Token: 0x04002204 RID: 8708
		private const uint m1 = 2155905152U;

		// Token: 0x04002205 RID: 8709
		private const uint m2 = 2139062143U;

		// Token: 0x04002206 RID: 8710
		private const uint m3 = 27U;

		// Token: 0x04002207 RID: 8711
		private const uint m4 = 3233857728U;

		// Token: 0x04002208 RID: 8712
		private const uint m5 = 1061109567U;

		// Token: 0x04002209 RID: 8713
		private int ROUNDS;

		// Token: 0x0400220A RID: 8714
		private uint[][] WorkingKey;

		// Token: 0x0400220B RID: 8715
		private uint C0;

		// Token: 0x0400220C RID: 8716
		private uint C1;

		// Token: 0x0400220D RID: 8717
		private uint C2;

		// Token: 0x0400220E RID: 8718
		private uint C3;

		// Token: 0x0400220F RID: 8719
		private bool forEncryption;

		// Token: 0x04002210 RID: 8720
		private const int BLOCK_SIZE = 16;
	}
}
