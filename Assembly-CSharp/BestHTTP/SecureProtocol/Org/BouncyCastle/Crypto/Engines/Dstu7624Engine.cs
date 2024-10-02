using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000575 RID: 1397
	public class Dstu7624Engine : IBlockCipher
	{
		// Token: 0x0600348B RID: 13451 RVA: 0x00140518 File Offset: 0x0013E718
		public Dstu7624Engine(int blockSizeBits)
		{
			if (blockSizeBits != 128 && blockSizeBits != 256 && blockSizeBits != 512)
			{
				throw new ArgumentException("unsupported block length: only 128/256/512 are allowed");
			}
			this.wordsInBlock = blockSizeBits / 64;
			this.internalState = new ulong[this.wordsInBlock];
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x0014056C File Offset: 0x0013E76C
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("Invalid parameter passed to Dstu7624Engine Init");
			}
			this.forEncryption = forEncryption;
			byte[] key = ((KeyParameter)parameters).GetKey();
			int num = key.Length << 3;
			int num2 = this.wordsInBlock << 6;
			if (num != 128 && num != 256 && num != 512)
			{
				throw new ArgumentException("unsupported key length: only 128/256/512 are allowed");
			}
			if (num != num2 && num != 2 * num2)
			{
				throw new ArgumentException("Unsupported key length");
			}
			if (num != 128)
			{
				if (num != 256)
				{
					if (num == 512)
					{
						this.roundsAmount = 18;
					}
				}
				else
				{
					this.roundsAmount = 14;
				}
			}
			else
			{
				this.roundsAmount = 10;
			}
			this.wordsInKey = num / 64;
			this.roundKeys = new ulong[this.roundsAmount + 1][];
			for (int i = 0; i < this.roundKeys.Length; i++)
			{
				this.roundKeys[i] = new ulong[this.wordsInBlock];
			}
			this.workingKey = new ulong[this.wordsInKey];
			if (key.Length != this.wordsInKey * 8)
			{
				throw new ArgumentException("Invalid key parameter passed to Dstu7624Engine Init");
			}
			Pack.LE_To_UInt64(key, 0, this.workingKey);
			ulong[] array = new ulong[this.wordsInBlock];
			this.WorkingKeyExpandKT(this.workingKey, array);
			this.WorkingKeyExpandEven(this.workingKey, array);
			this.WorkingKeyExpandOdd();
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x001406C8 File Offset: 0x0013E8C8
		private void WorkingKeyExpandKT(ulong[] workingKey, ulong[] tempKeys)
		{
			ulong[] array = new ulong[this.wordsInBlock];
			ulong[] array2 = new ulong[this.wordsInBlock];
			this.internalState = new ulong[this.wordsInBlock];
			this.internalState[0] += (ulong)((long)(this.wordsInBlock + this.wordsInKey + 1));
			if (this.wordsInBlock == this.wordsInKey)
			{
				Array.Copy(workingKey, 0, array, 0, array.Length);
				Array.Copy(workingKey, 0, array2, 0, array2.Length);
			}
			else
			{
				Array.Copy(workingKey, 0, array, 0, this.wordsInBlock);
				Array.Copy(workingKey, this.wordsInBlock, array2, 0, this.wordsInBlock);
			}
			for (int i = 0; i < this.internalState.Length; i++)
			{
				this.internalState[i] += array[i];
			}
			this.EncryptionRound();
			for (int j = 0; j < this.internalState.Length; j++)
			{
				this.internalState[j] ^= array2[j];
			}
			this.EncryptionRound();
			for (int k = 0; k < this.internalState.Length; k++)
			{
				this.internalState[k] += array[k];
			}
			this.EncryptionRound();
			Array.Copy(this.internalState, 0, tempKeys, 0, this.wordsInBlock);
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x00140808 File Offset: 0x0013EA08
		private void WorkingKeyExpandEven(ulong[] workingKey, ulong[] tempKey)
		{
			ulong[] array = new ulong[this.wordsInKey];
			ulong[] array2 = new ulong[this.wordsInBlock];
			int num = 0;
			Array.Copy(workingKey, 0, array, 0, this.wordsInKey);
			ulong num2 = 281479271743489UL;
			for (;;)
			{
				for (int i = 0; i < this.wordsInBlock; i++)
				{
					array2[i] = tempKey[i] + num2;
				}
				for (int j = 0; j < this.wordsInBlock; j++)
				{
					this.internalState[j] = array[j] + array2[j];
				}
				this.EncryptionRound();
				for (int k = 0; k < this.wordsInBlock; k++)
				{
					this.internalState[k] ^= array2[k];
				}
				this.EncryptionRound();
				for (int l = 0; l < this.wordsInBlock; l++)
				{
					this.internalState[l] += array2[l];
				}
				Array.Copy(this.internalState, 0, this.roundKeys[num], 0, this.wordsInBlock);
				if (this.roundsAmount == num)
				{
					break;
				}
				if (this.wordsInKey != this.wordsInBlock)
				{
					num += 2;
					num2 <<= 1;
					for (int m = 0; m < this.wordsInBlock; m++)
					{
						array2[m] = tempKey[m] + num2;
					}
					for (int n = 0; n < this.wordsInBlock; n++)
					{
						this.internalState[n] = array[this.wordsInBlock + n] + array2[n];
					}
					this.EncryptionRound();
					for (int num3 = 0; num3 < this.wordsInBlock; num3++)
					{
						this.internalState[num3] ^= array2[num3];
					}
					this.EncryptionRound();
					for (int num4 = 0; num4 < this.wordsInBlock; num4++)
					{
						this.internalState[num4] += array2[num4];
					}
					Array.Copy(this.internalState, 0, this.roundKeys[num], 0, this.wordsInBlock);
					if (this.roundsAmount == num)
					{
						break;
					}
				}
				num += 2;
				num2 <<= 1;
				ulong num5 = array[0];
				for (int num6 = 1; num6 < array.Length; num6++)
				{
					array[num6 - 1] = array[num6];
				}
				array[array.Length - 1] = num5;
			}
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x00140A38 File Offset: 0x0013EC38
		private void WorkingKeyExpandOdd()
		{
			for (int i = 1; i < this.roundsAmount; i += 2)
			{
				this.RotateLeft(this.roundKeys[i - 1], this.roundKeys[i]);
			}
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x00140A70 File Offset: 0x0013EC70
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (this.workingKey == null)
			{
				throw new InvalidOperationException("Dstu7624Engine not initialised");
			}
			Check.DataLength(input, inOff, this.GetBlockSize(), "input buffer too short");
			Check.OutputLength(output, outOff, this.GetBlockSize(), "output buffer too short");
			if (this.forEncryption)
			{
				int num = this.wordsInBlock;
				if (num == 2)
				{
					this.EncryptBlock_128(input, inOff, output, outOff);
				}
				else
				{
					Pack.LE_To_UInt64(input, inOff, this.internalState);
					this.AddRoundKey(0);
					int num2 = 0;
					for (;;)
					{
						this.EncryptionRound();
						if (++num2 == this.roundsAmount)
						{
							break;
						}
						this.XorRoundKey(num2);
					}
					this.AddRoundKey(this.roundsAmount);
					Pack.UInt64_To_LE(this.internalState, output, outOff);
				}
			}
			else
			{
				int num = this.wordsInBlock;
				if (num == 2)
				{
					this.DecryptBlock_128(input, inOff, output, outOff);
				}
				else
				{
					Pack.LE_To_UInt64(input, inOff, this.internalState);
					this.SubRoundKey(this.roundsAmount);
					int num3 = this.roundsAmount;
					for (;;)
					{
						this.DecryptionRound();
						if (--num3 == 0)
						{
							break;
						}
						this.XorRoundKey(num3);
					}
					this.SubRoundKey(0);
					Pack.UInt64_To_LE(this.internalState, output, outOff);
				}
			}
			return this.GetBlockSize();
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x00140B8F File Offset: 0x0013ED8F
		private void EncryptionRound()
		{
			this.SubBytes();
			this.ShiftRows();
			this.MixColumns();
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x00140BA3 File Offset: 0x0013EDA3
		private void DecryptionRound()
		{
			this.MixColumnsInv();
			this.InvShiftRows();
			this.InvSubBytes();
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x00140BB8 File Offset: 0x0013EDB8
		private void DecryptBlock_128(byte[] input, int inOff, byte[] output, int outOff)
		{
			ulong num = Pack.LE_To_UInt64(input, inOff);
			ulong num2 = Pack.LE_To_UInt64(input, inOff + 8);
			ulong[] array = this.roundKeys[this.roundsAmount];
			num -= array[0];
			num2 -= array[1];
			int num3 = this.roundsAmount;
			for (;;)
			{
				num = Dstu7624Engine.MixColumnInv(num);
				num2 = Dstu7624Engine.MixColumnInv(num2);
				uint num4 = (uint)num;
				uint num5 = (uint)(num >> 32);
				uint num6 = (uint)num2;
				uint num7 = (uint)(num2 >> 32);
				uint num8 = (uint)Dstu7624Engine.T0[(int)(num4 & 255U)];
				byte b = Dstu7624Engine.T1[(int)(num4 >> 8 & 255U)];
				byte b2 = Dstu7624Engine.T2[(int)(num4 >> 16 & 255U)];
				byte b3 = Dstu7624Engine.T3[(int)(num4 >> 24)];
				num4 = (num8 | (uint)((uint)b << 8) | (uint)((uint)b2 << 16) | (uint)((uint)b3 << 24));
				uint num9 = (uint)Dstu7624Engine.T0[(int)(num7 & 255U)];
				byte b4 = Dstu7624Engine.T1[(int)(num7 >> 8 & 255U)];
				byte b5 = Dstu7624Engine.T2[(int)(num7 >> 16 & 255U)];
				byte b6 = Dstu7624Engine.T3[(int)(num7 >> 24)];
				num7 = (num9 | (uint)((uint)b4 << 8) | (uint)((uint)b5 << 16) | (uint)((uint)b6 << 24));
				num = ((ulong)num4 | (ulong)num7 << 32);
				uint num10 = (uint)Dstu7624Engine.T0[(int)(num6 & 255U)];
				byte b7 = Dstu7624Engine.T1[(int)(num6 >> 8 & 255U)];
				byte b8 = Dstu7624Engine.T2[(int)(num6 >> 16 & 255U)];
				byte b9 = Dstu7624Engine.T3[(int)(num6 >> 24)];
				num6 = (num10 | (uint)((uint)b7 << 8) | (uint)((uint)b8 << 16) | (uint)((uint)b9 << 24));
				uint num11 = (uint)Dstu7624Engine.T0[(int)(num5 & 255U)];
				byte b10 = Dstu7624Engine.T1[(int)(num5 >> 8 & 255U)];
				byte b11 = Dstu7624Engine.T2[(int)(num5 >> 16 & 255U)];
				byte b12 = Dstu7624Engine.T3[(int)(num5 >> 24)];
				num5 = (num11 | (uint)((uint)b10 << 8) | (uint)((uint)b11 << 16) | (uint)((uint)b12 << 24));
				num2 = ((ulong)num6 | (ulong)num5 << 32);
				if (--num3 == 0)
				{
					break;
				}
				array = this.roundKeys[num3];
				num ^= array[0];
				num2 ^= array[1];
			}
			array = this.roundKeys[0];
			num -= array[0];
			num2 -= array[1];
			Pack.UInt64_To_LE(num, output, outOff);
			Pack.UInt64_To_LE(num2, output, outOff + 8);
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x00140DC8 File Offset: 0x0013EFC8
		private void EncryptBlock_128(byte[] input, int inOff, byte[] output, int outOff)
		{
			ulong num = Pack.LE_To_UInt64(input, inOff);
			ulong num2 = Pack.LE_To_UInt64(input, inOff + 8);
			ulong[] array = this.roundKeys[0];
			num += array[0];
			num2 += array[1];
			int num3 = 0;
			for (;;)
			{
				uint num4 = (uint)num;
				uint num5 = (uint)(num >> 32);
				uint num6 = (uint)num2;
				uint num7 = (uint)(num2 >> 32);
				uint num8 = (uint)Dstu7624Engine.S0[(int)(num4 & 255U)];
				byte b = Dstu7624Engine.S1[(int)(num4 >> 8 & 255U)];
				byte b2 = Dstu7624Engine.S2[(int)(num4 >> 16 & 255U)];
				byte b3 = Dstu7624Engine.S3[(int)(num4 >> 24)];
				num4 = (num8 | (uint)((uint)b << 8) | (uint)((uint)b2 << 16) | (uint)((uint)b3 << 24));
				uint num9 = (uint)Dstu7624Engine.S0[(int)(num7 & 255U)];
				byte b4 = Dstu7624Engine.S1[(int)(num7 >> 8 & 255U)];
				byte b5 = Dstu7624Engine.S2[(int)(num7 >> 16 & 255U)];
				byte b6 = Dstu7624Engine.S3[(int)(num7 >> 24)];
				num7 = (num9 | (uint)((uint)b4 << 8) | (uint)((uint)b5 << 16) | (uint)((uint)b6 << 24));
				num = ((ulong)num4 | (ulong)num7 << 32);
				uint num10 = (uint)Dstu7624Engine.S0[(int)(num6 & 255U)];
				byte b7 = Dstu7624Engine.S1[(int)(num6 >> 8 & 255U)];
				byte b8 = Dstu7624Engine.S2[(int)(num6 >> 16 & 255U)];
				byte b9 = Dstu7624Engine.S3[(int)(num6 >> 24)];
				num6 = (num10 | (uint)((uint)b7 << 8) | (uint)((uint)b8 << 16) | (uint)((uint)b9 << 24));
				uint num11 = (uint)Dstu7624Engine.S0[(int)(num5 & 255U)];
				byte b10 = Dstu7624Engine.S1[(int)(num5 >> 8 & 255U)];
				byte b11 = Dstu7624Engine.S2[(int)(num5 >> 16 & 255U)];
				byte b12 = Dstu7624Engine.S3[(int)(num5 >> 24)];
				num5 = (num11 | (uint)((uint)b10 << 8) | (uint)((uint)b11 << 16) | (uint)((uint)b12 << 24));
				num2 = ((ulong)num6 | (ulong)num5 << 32);
				num = Dstu7624Engine.MixColumn(num);
				num2 = Dstu7624Engine.MixColumn(num2);
				if (++num3 == this.roundsAmount)
				{
					break;
				}
				array = this.roundKeys[num3];
				num ^= array[0];
				num2 ^= array[1];
			}
			array = this.roundKeys[this.roundsAmount];
			num += array[0];
			num2 += array[1];
			Pack.UInt64_To_LE(num, output, outOff);
			Pack.UInt64_To_LE(num2, output, outOff + 8);
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x00140FDC File Offset: 0x0013F1DC
		private void SubBytes()
		{
			for (int i = 0; i < this.wordsInBlock; i++)
			{
				ulong num = this.internalState[i];
				uint num2 = (uint)num;
				uint num3 = (uint)(num >> 32);
				uint num4 = (uint)Dstu7624Engine.S0[(int)(num2 & 255U)];
				byte b = Dstu7624Engine.S1[(int)(num2 >> 8 & 255U)];
				byte b2 = Dstu7624Engine.S2[(int)(num2 >> 16 & 255U)];
				byte b3 = Dstu7624Engine.S3[(int)(num2 >> 24)];
				num2 = (num4 | (uint)((uint)b << 8) | (uint)((uint)b2 << 16) | (uint)((uint)b3 << 24));
				uint num5 = (uint)Dstu7624Engine.S0[(int)(num3 & 255U)];
				byte b4 = Dstu7624Engine.S1[(int)(num3 >> 8 & 255U)];
				byte b5 = Dstu7624Engine.S2[(int)(num3 >> 16 & 255U)];
				byte b6 = Dstu7624Engine.S3[(int)(num3 >> 24)];
				num3 = (num5 | (uint)((uint)b4 << 8) | (uint)((uint)b5 << 16) | (uint)((uint)b6 << 24));
				this.internalState[i] = ((ulong)num2 | (ulong)num3 << 32);
			}
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x001410BC File Offset: 0x0013F2BC
		private void InvSubBytes()
		{
			for (int i = 0; i < this.wordsInBlock; i++)
			{
				ulong num = this.internalState[i];
				uint num2 = (uint)num;
				uint num3 = (uint)(num >> 32);
				uint num4 = (uint)Dstu7624Engine.T0[(int)(num2 & 255U)];
				byte b = Dstu7624Engine.T1[(int)(num2 >> 8 & 255U)];
				byte b2 = Dstu7624Engine.T2[(int)(num2 >> 16 & 255U)];
				byte b3 = Dstu7624Engine.T3[(int)(num2 >> 24)];
				num2 = (num4 | (uint)((uint)b << 8) | (uint)((uint)b2 << 16) | (uint)((uint)b3 << 24));
				uint num5 = (uint)Dstu7624Engine.T0[(int)(num3 & 255U)];
				byte b4 = Dstu7624Engine.T1[(int)(num3 >> 8 & 255U)];
				byte b5 = Dstu7624Engine.T2[(int)(num3 >> 16 & 255U)];
				byte b6 = Dstu7624Engine.T3[(int)(num3 >> 24)];
				num3 = (num5 | (uint)((uint)b4 << 8) | (uint)((uint)b5 << 16) | (uint)((uint)b6 << 24));
				this.internalState[i] = ((ulong)num2 | (ulong)num3 << 32);
			}
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x0014119C File Offset: 0x0013F39C
		private void ShiftRows()
		{
			int num = this.wordsInBlock;
			if (num == 2)
			{
				ulong num2 = this.internalState[0];
				ulong num3 = this.internalState[1];
				ulong num4 = (num2 ^ num3) & 18446744069414584320UL;
				num2 ^= num4;
				num3 ^= num4;
				this.internalState[0] = num2;
				this.internalState[1] = num3;
				return;
			}
			if (num == 4)
			{
				ulong num5 = this.internalState[0];
				ulong num6 = this.internalState[1];
				ulong num7 = this.internalState[2];
				ulong num8 = this.internalState[3];
				ulong num9 = (num5 ^ num7) & 18446744069414584320UL;
				num5 ^= num9;
				num7 ^= num9;
				num9 = ((num6 ^ num8) & 281474976645120UL);
				num6 ^= num9;
				num8 ^= num9;
				num9 = ((num5 ^ num6) & 18446462603027742720UL);
				num5 ^= num9;
				num6 ^= num9;
				num9 = ((num7 ^ num8) & 18446462603027742720UL);
				num7 ^= num9;
				num8 ^= num9;
				this.internalState[0] = num5;
				this.internalState[1] = num6;
				this.internalState[2] = num7;
				this.internalState[3] = num8;
				return;
			}
			if (num != 8)
			{
				throw new InvalidOperationException("unsupported block length: only 128/256/512 are allowed");
			}
			ulong num10 = this.internalState[0];
			ulong num11 = this.internalState[1];
			ulong num12 = this.internalState[2];
			ulong num13 = this.internalState[3];
			ulong num14 = this.internalState[4];
			ulong num15 = this.internalState[5];
			ulong num16 = this.internalState[6];
			ulong num17 = this.internalState[7];
			ulong num18 = (num10 ^ num14) & 18446744069414584320UL;
			num10 ^= num18;
			num14 ^= num18;
			num18 = ((num11 ^ num15) & 72057594021150720UL);
			num11 ^= num18;
			num15 ^= num18;
			num18 = ((num12 ^ num16) & 281474976645120UL);
			num12 ^= num18;
			num16 ^= num18;
			num18 = ((num13 ^ num17) & 1099511627520UL);
			num13 ^= num18;
			num17 ^= num18;
			num18 = ((num10 ^ num12) & 18446462603027742720UL);
			num10 ^= num18;
			num12 ^= num18;
			num18 = ((num11 ^ num13) & 72056494543077120UL);
			num11 ^= num18;
			num13 ^= num18;
			num18 = ((num14 ^ num16) & 18446462603027742720UL);
			num14 ^= num18;
			num16 ^= num18;
			num18 = ((num15 ^ num17) & 72056494543077120UL);
			num15 ^= num18;
			num17 ^= num18;
			num18 = ((num10 ^ num11) & 18374966859414961920UL);
			num10 ^= num18;
			num11 ^= num18;
			num18 = ((num12 ^ num13) & 18374966859414961920UL);
			num12 ^= num18;
			num13 ^= num18;
			num18 = ((num14 ^ num15) & 18374966859414961920UL);
			num14 ^= num18;
			num15 ^= num18;
			num18 = ((num16 ^ num17) & 18374966859414961920UL);
			num16 ^= num18;
			num17 ^= num18;
			this.internalState[0] = num10;
			this.internalState[1] = num11;
			this.internalState[2] = num12;
			this.internalState[3] = num13;
			this.internalState[4] = num14;
			this.internalState[5] = num15;
			this.internalState[6] = num16;
			this.internalState[7] = num17;
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x001414EC File Offset: 0x0013F6EC
		private void InvShiftRows()
		{
			int num = this.wordsInBlock;
			if (num == 2)
			{
				ulong num2 = this.internalState[0];
				ulong num3 = this.internalState[1];
				ulong num4 = (num2 ^ num3) & 18446744069414584320UL;
				num2 ^= num4;
				num3 ^= num4;
				this.internalState[0] = num2;
				this.internalState[1] = num3;
				return;
			}
			if (num == 4)
			{
				ulong num5 = this.internalState[0];
				ulong num6 = this.internalState[1];
				ulong num7 = this.internalState[2];
				ulong num8 = this.internalState[3];
				ulong num9 = (num5 ^ num6) & 18446462603027742720UL;
				num5 ^= num9;
				num6 ^= num9;
				num9 = ((num7 ^ num8) & 18446462603027742720UL);
				num7 ^= num9;
				num8 ^= num9;
				num9 = ((num5 ^ num7) & 18446744069414584320UL);
				num5 ^= num9;
				num7 ^= num9;
				num9 = ((num6 ^ num8) & 281474976645120UL);
				num6 ^= num9;
				num8 ^= num9;
				this.internalState[0] = num5;
				this.internalState[1] = num6;
				this.internalState[2] = num7;
				this.internalState[3] = num8;
				return;
			}
			if (num != 8)
			{
				throw new InvalidOperationException("unsupported block length: only 128/256/512 are allowed");
			}
			ulong num10 = this.internalState[0];
			ulong num11 = this.internalState[1];
			ulong num12 = this.internalState[2];
			ulong num13 = this.internalState[3];
			ulong num14 = this.internalState[4];
			ulong num15 = this.internalState[5];
			ulong num16 = this.internalState[6];
			ulong num17 = this.internalState[7];
			ulong num18 = (num10 ^ num11) & 18374966859414961920UL;
			num10 ^= num18;
			num11 ^= num18;
			num18 = ((num12 ^ num13) & 18374966859414961920UL);
			num12 ^= num18;
			num13 ^= num18;
			num18 = ((num14 ^ num15) & 18374966859414961920UL);
			num14 ^= num18;
			num15 ^= num18;
			num18 = ((num16 ^ num17) & 18374966859414961920UL);
			num16 ^= num18;
			num17 ^= num18;
			num18 = ((num10 ^ num12) & 18446462603027742720UL);
			num10 ^= num18;
			num12 ^= num18;
			num18 = ((num11 ^ num13) & 72056494543077120UL);
			num11 ^= num18;
			num13 ^= num18;
			num18 = ((num14 ^ num16) & 18446462603027742720UL);
			num14 ^= num18;
			num16 ^= num18;
			num18 = ((num15 ^ num17) & 72056494543077120UL);
			num15 ^= num18;
			num17 ^= num18;
			num18 = ((num10 ^ num14) & 18446744069414584320UL);
			num10 ^= num18;
			num14 ^= num18;
			num18 = ((num11 ^ num15) & 72057594021150720UL);
			num11 ^= num18;
			num15 ^= num18;
			num18 = ((num12 ^ num16) & 281474976645120UL);
			num12 ^= num18;
			num16 ^= num18;
			num18 = ((num13 ^ num17) & 1099511627520UL);
			num13 ^= num18;
			num17 ^= num18;
			this.internalState[0] = num10;
			this.internalState[1] = num11;
			this.internalState[2] = num12;
			this.internalState[3] = num13;
			this.internalState[4] = num14;
			this.internalState[5] = num15;
			this.internalState[6] = num16;
			this.internalState[7] = num17;
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x0014183C File Offset: 0x0013FA3C
		private void AddRoundKey(int round)
		{
			ulong[] array = this.roundKeys[round];
			for (int i = 0; i < this.wordsInBlock; i++)
			{
				this.internalState[i] += array[i];
			}
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x00141878 File Offset: 0x0013FA78
		private void SubRoundKey(int round)
		{
			ulong[] array = this.roundKeys[round];
			for (int i = 0; i < this.wordsInBlock; i++)
			{
				this.internalState[i] -= array[i];
			}
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x001418B4 File Offset: 0x0013FAB4
		private void XorRoundKey(int round)
		{
			ulong[] array = this.roundKeys[round];
			for (int i = 0; i < this.wordsInBlock; i++)
			{
				this.internalState[i] ^= array[i];
			}
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x001418F0 File Offset: 0x0013FAF0
		private static ulong MixColumn(ulong c)
		{
			ulong num = Dstu7624Engine.MulX(c);
			ulong num2 = Dstu7624Engine.Rotate(8, c) ^ c;
			num2 ^= Dstu7624Engine.Rotate(16, num2);
			num2 ^= Dstu7624Engine.Rotate(48, c);
			ulong x = Dstu7624Engine.MulX2(num2 ^ c ^ num);
			return num2 ^ Dstu7624Engine.Rotate(32, x) ^ Dstu7624Engine.Rotate(40, num) ^ Dstu7624Engine.Rotate(48, num);
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x0014194C File Offset: 0x0013FB4C
		private void MixColumns()
		{
			for (int i = 0; i < this.wordsInBlock; i++)
			{
				this.internalState[i] = Dstu7624Engine.MixColumn(this.internalState[i]);
			}
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x00141980 File Offset: 0x0013FB80
		private static ulong MixColumnInv(ulong c)
		{
			ulong num = c ^ Dstu7624Engine.Rotate(8, c);
			num ^= Dstu7624Engine.Rotate(32, num);
			num ^= Dstu7624Engine.Rotate(48, c);
			ulong num2 = num ^ c;
			ulong num3 = Dstu7624Engine.Rotate(48, c);
			ulong num4 = Dstu7624Engine.Rotate(56, c);
			ulong n = num2 ^ num4;
			ulong num5 = Dstu7624Engine.Rotate(56, num2);
			num5 ^= Dstu7624Engine.MulX(n);
			ulong num6 = Dstu7624Engine.Rotate(16, num2) ^ c;
			num6 ^= Dstu7624Engine.Rotate(40, Dstu7624Engine.MulX(num5) ^ c);
			ulong num7 = num2 ^ num3;
			num7 ^= Dstu7624Engine.MulX(num6);
			ulong num8 = Dstu7624Engine.Rotate(16, num);
			num8 ^= Dstu7624Engine.MulX(num7);
			ulong num9 = num2 ^ Dstu7624Engine.Rotate(24, c) ^ num3 ^ num4;
			num9 ^= Dstu7624Engine.MulX(num8);
			ulong num10 = Dstu7624Engine.Rotate(32, num2) ^ c ^ num4;
			num10 ^= Dstu7624Engine.MulX(num9);
			return num ^ Dstu7624Engine.MulX(Dstu7624Engine.Rotate(40, num10));
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x00141A70 File Offset: 0x0013FC70
		private void MixColumnsInv()
		{
			for (int i = 0; i < this.wordsInBlock; i++)
			{
				this.internalState[i] = Dstu7624Engine.MixColumnInv(this.internalState[i]);
			}
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x00141AA3 File Offset: 0x0013FCA3
		private static ulong MulX(ulong n)
		{
			return (n & 9187201950435737471UL) << 1 ^ ((n & 9259542123273814144UL) >> 7) * 29UL;
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x00141AC4 File Offset: 0x0013FCC4
		private static ulong MulX2(ulong n)
		{
			return (n & 4557430888798830399UL) << 2 ^ ((n & 9259542123273814144UL) >> 6) * 29UL ^ ((n & 4629771061636907072UL) >> 6) * 29UL;
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x00141AF7 File Offset: 0x0013FCF7
		private static ulong Rotate(int n, ulong x)
		{
			return x >> n | x << -n;
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x00141B08 File Offset: 0x0013FD08
		private void RotateLeft(ulong[] x, ulong[] z)
		{
			int num = this.wordsInBlock;
			if (num == 2)
			{
				ulong num2 = x[0];
				ulong num3 = x[1];
				z[0] = (num2 >> 56 | num3 << 8);
				z[1] = (num3 >> 56 | num2 << 8);
				return;
			}
			if (num == 4)
			{
				ulong num4 = x[0];
				ulong num5 = x[1];
				ulong num6 = x[2];
				ulong num7 = x[3];
				z[0] = (num5 >> 24 | num6 << 40);
				z[1] = (num6 >> 24 | num7 << 40);
				z[2] = (num7 >> 24 | num4 << 40);
				z[3] = (num4 >> 24 | num5 << 40);
				return;
			}
			if (num != 8)
			{
				throw new InvalidOperationException("unsupported block length: only 128/256/512 are allowed");
			}
			ulong num8 = x[0];
			ulong num9 = x[1];
			ulong num10 = x[2];
			ulong num11 = x[3];
			ulong num12 = x[4];
			ulong num13 = x[5];
			ulong num14 = x[6];
			ulong num15 = x[7];
			z[0] = (num10 >> 24 | num11 << 40);
			z[1] = (num11 >> 24 | num12 << 40);
			z[2] = (num12 >> 24 | num13 << 40);
			z[3] = (num13 >> 24 | num14 << 40);
			z[4] = (num14 >> 24 | num15 << 40);
			z[5] = (num15 >> 24 | num8 << 40);
			z[6] = (num8 >> 24 | num9 << 40);
			z[7] = (num9 >> 24 | num10 << 40);
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x060034A4 RID: 13476 RVA: 0x00141C39 File Offset: 0x0013FE39
		public virtual string AlgorithmName
		{
			get
			{
				return "DSTU7624";
			}
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x00141C40 File Offset: 0x0013FE40
		public virtual int GetBlockSize()
		{
			return this.wordsInBlock << 3;
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x060034A6 RID: 13478 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x00141C4A File Offset: 0x0013FE4A
		public virtual void Reset()
		{
			Array.Clear(this.internalState, 0, this.internalState.Length);
		}

		// Token: 0x04002268 RID: 8808
		private ulong[] internalState;

		// Token: 0x04002269 RID: 8809
		private ulong[] workingKey;

		// Token: 0x0400226A RID: 8810
		private ulong[][] roundKeys;

		// Token: 0x0400226B RID: 8811
		private int wordsInBlock;

		// Token: 0x0400226C RID: 8812
		private int wordsInKey;

		// Token: 0x0400226D RID: 8813
		private const int ROUNDS_128 = 10;

		// Token: 0x0400226E RID: 8814
		private const int ROUNDS_256 = 14;

		// Token: 0x0400226F RID: 8815
		private const int ROUNDS_512 = 18;

		// Token: 0x04002270 RID: 8816
		private int roundsAmount;

		// Token: 0x04002271 RID: 8817
		private bool forEncryption;

		// Token: 0x04002272 RID: 8818
		private const ulong mdsMatrix = 290207332435296513UL;

		// Token: 0x04002273 RID: 8819
		private const ulong mdsInvMatrix = 14616231584692868525UL;

		// Token: 0x04002274 RID: 8820
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

		// Token: 0x04002275 RID: 8821
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

		// Token: 0x04002276 RID: 8822
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

		// Token: 0x04002277 RID: 8823
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

		// Token: 0x04002278 RID: 8824
		private static readonly byte[] T0 = new byte[]
		{
			164,
			162,
			169,
			197,
			78,
			201,
			3,
			217,
			126,
			15,
			210,
			173,
			231,
			211,
			39,
			91,
			227,
			161,
			232,
			230,
			124,
			42,
			85,
			12,
			134,
			57,
			215,
			141,
			184,
			18,
			111,
			40,
			205,
			138,
			112,
			86,
			114,
			249,
			191,
			79,
			115,
			233,
			247,
			87,
			22,
			172,
			80,
			192,
			157,
			183,
			71,
			113,
			96,
			196,
			116,
			67,
			108,
			31,
			147,
			119,
			220,
			206,
			32,
			140,
			153,
			95,
			68,
			1,
			245,
			30,
			135,
			94,
			97,
			44,
			75,
			29,
			129,
			21,
			244,
			35,
			214,
			234,
			225,
			103,
			241,
			127,
			254,
			218,
			60,
			7,
			83,
			106,
			132,
			156,
			203,
			2,
			131,
			51,
			221,
			53,
			226,
			89,
			90,
			152,
			165,
			146,
			100,
			4,
			6,
			16,
			77,
			28,
			151,
			8,
			49,
			238,
			171,
			5,
			175,
			121,
			160,
			24,
			70,
			109,
			252,
			137,
			212,
			199,
			byte.MaxValue,
			240,
			207,
			66,
			145,
			248,
			104,
			10,
			101,
			142,
			182,
			253,
			195,
			239,
			120,
			76,
			204,
			158,
			48,
			46,
			188,
			11,
			84,
			26,
			166,
			187,
			38,
			128,
			72,
			148,
			50,
			125,
			167,
			63,
			174,
			34,
			61,
			102,
			170,
			246,
			0,
			93,
			189,
			74,
			224,
			59,
			180,
			23,
			139,
			159,
			118,
			176,
			36,
			154,
			37,
			99,
			219,
			235,
			122,
			62,
			92,
			179,
			177,
			41,
			242,
			202,
			88,
			110,
			216,
			168,
			47,
			117,
			223,
			20,
			251,
			19,
			73,
			136,
			178,
			236,
			228,
			52,
			45,
			150,
			198,
			58,
			237,
			149,
			14,
			229,
			133,
			107,
			64,
			33,
			155,
			9,
			25,
			43,
			82,
			222,
			69,
			163,
			250,
			81,
			194,
			181,
			209,
			144,
			185,
			243,
			55,
			193,
			13,
			186,
			65,
			17,
			56,
			123,
			190,
			208,
			213,
			105,
			54,
			200,
			98,
			27,
			130,
			143
		};

		// Token: 0x04002279 RID: 8825
		private static readonly byte[] T1 = new byte[]
		{
			131,
			242,
			42,
			235,
			233,
			191,
			123,
			156,
			52,
			150,
			141,
			152,
			185,
			105,
			140,
			41,
			61,
			136,
			104,
			6,
			57,
			17,
			76,
			14,
			160,
			86,
			64,
			146,
			21,
			188,
			179,
			220,
			111,
			248,
			38,
			186,
			190,
			189,
			49,
			251,
			195,
			254,
			128,
			97,
			225,
			122,
			50,
			210,
			112,
			32,
			161,
			69,
			236,
			217,
			26,
			93,
			180,
			216,
			9,
			165,
			85,
			142,
			55,
			118,
			169,
			103,
			16,
			23,
			54,
			101,
			177,
			149,
			98,
			89,
			116,
			163,
			80,
			47,
			75,
			200,
			208,
			143,
			205,
			212,
			60,
			134,
			18,
			29,
			35,
			239,
			244,
			83,
			25,
			53,
			230,
			127,
			94,
			214,
			121,
			81,
			34,
			20,
			247,
			30,
			74,
			66,
			155,
			65,
			115,
			45,
			193,
			92,
			166,
			162,
			224,
			46,
			211,
			40,
			187,
			201,
			174,
			106,
			209,
			90,
			48,
			144,
			132,
			249,
			178,
			88,
			207,
			126,
			197,
			203,
			151,
			228,
			22,
			108,
			250,
			176,
			109,
			31,
			82,
			153,
			13,
			78,
			3,
			145,
			194,
			77,
			100,
			119,
			159,
			221,
			196,
			73,
			138,
			154,
			36,
			56,
			167,
			87,
			133,
			199,
			124,
			125,
			231,
			246,
			183,
			172,
			39,
			70,
			222,
			223,
			59,
			215,
			158,
			43,
			11,
			213,
			19,
			117,
			240,
			114,
			182,
			157,
			27,
			1,
			63,
			68,
			229,
			135,
			253,
			7,
			241,
			171,
			148,
			24,
			234,
			252,
			58,
			130,
			95,
			5,
			84,
			219,
			0,
			139,
			227,
			72,
			12,
			202,
			120,
			137,
			10,
			byte.MaxValue,
			62,
			91,
			129,
			238,
			113,
			226,
			218,
			44,
			184,
			181,
			204,
			110,
			168,
			107,
			173,
			96,
			198,
			8,
			4,
			2,
			232,
			245,
			79,
			164,
			243,
			192,
			206,
			67,
			37,
			28,
			33,
			51,
			15,
			175,
			71,
			237,
			102,
			99,
			147,
			170
		};

		// Token: 0x0400227A RID: 8826
		private static readonly byte[] T2 = new byte[]
		{
			69,
			212,
			11,
			67,
			241,
			114,
			237,
			164,
			194,
			56,
			230,
			113,
			253,
			182,
			58,
			149,
			80,
			68,
			75,
			226,
			116,
			107,
			30,
			17,
			90,
			198,
			180,
			216,
			165,
			138,
			112,
			163,
			168,
			250,
			5,
			217,
			151,
			64,
			201,
			144,
			152,
			143,
			220,
			18,
			49,
			44,
			71,
			106,
			153,
			174,
			200,
			127,
			249,
			79,
			93,
			150,
			111,
			244,
			179,
			57,
			33,
			218,
			156,
			133,
			158,
			59,
			240,
			191,
			239,
			6,
			238,
			229,
			95,
			32,
			16,
			204,
			60,
			84,
			74,
			82,
			148,
			14,
			192,
			40,
			246,
			86,
			96,
			162,
			227,
			15,
			236,
			157,
			36,
			131,
			126,
			213,
			124,
			235,
			24,
			215,
			205,
			221,
			120,
			byte.MaxValue,
			219,
			161,
			9,
			208,
			118,
			132,
			117,
			187,
			29,
			26,
			47,
			176,
			254,
			214,
			52,
			99,
			53,
			210,
			42,
			89,
			109,
			77,
			119,
			231,
			142,
			97,
			207,
			159,
			206,
			39,
			245,
			128,
			134,
			199,
			166,
			251,
			248,
			135,
			171,
			98,
			63,
			223,
			72,
			0,
			20,
			154,
			189,
			91,
			4,
			146,
			2,
			37,
			101,
			76,
			83,
			12,
			242,
			41,
			175,
			23,
			108,
			65,
			48,
			233,
			147,
			85,
			247,
			172,
			104,
			38,
			196,
			125,
			202,
			122,
			62,
			160,
			55,
			3,
			193,
			54,
			105,
			102,
			8,
			22,
			167,
			188,
			197,
			211,
			34,
			183,
			19,
			70,
			50,
			232,
			87,
			136,
			43,
			129,
			178,
			78,
			100,
			28,
			170,
			145,
			88,
			46,
			155,
			92,
			27,
			81,
			115,
			66,
			35,
			1,
			110,
			243,
			13,
			190,
			61,
			10,
			45,
			31,
			103,
			51,
			25,
			123,
			94,
			234,
			222,
			139,
			203,
			169,
			140,
			141,
			173,
			73,
			130,
			228,
			186,
			195,
			21,
			209,
			224,
			137,
			252,
			177,
			185,
			181,
			7,
			121,
			184,
			225
		};

		// Token: 0x0400227B RID: 8827
		private static readonly byte[] T3 = new byte[]
		{
			178,
			182,
			35,
			17,
			167,
			136,
			197,
			166,
			57,
			143,
			196,
			232,
			115,
			34,
			67,
			195,
			130,
			39,
			205,
			24,
			81,
			98,
			45,
			247,
			92,
			14,
			59,
			253,
			202,
			155,
			13,
			15,
			121,
			140,
			16,
			76,
			116,
			28,
			10,
			142,
			124,
			148,
			7,
			199,
			94,
			20,
			161,
			33,
			87,
			80,
			78,
			169,
			128,
			217,
			239,
			100,
			65,
			207,
			60,
			238,
			46,
			19,
			41,
			186,
			52,
			90,
			174,
			138,
			97,
			51,
			18,
			185,
			85,
			168,
			21,
			5,
			246,
			3,
			6,
			73,
			181,
			37,
			9,
			22,
			12,
			42,
			56,
			252,
			32,
			244,
			229,
			127,
			215,
			49,
			43,
			102,
			111,
			byte.MaxValue,
			114,
			134,
			240,
			163,
			47,
			120,
			0,
			188,
			204,
			226,
			176,
			241,
			66,
			180,
			48,
			95,
			96,
			4,
			236,
			165,
			227,
			139,
			231,
			29,
			191,
			132,
			123,
			230,
			129,
			248,
			222,
			216,
			210,
			23,
			206,
			75,
			71,
			214,
			105,
			108,
			25,
			153,
			154,
			1,
			179,
			133,
			177,
			249,
			89,
			194,
			55,
			233,
			200,
			160,
			237,
			79,
			137,
			104,
			109,
			213,
			38,
			145,
			135,
			88,
			189,
			201,
			152,
			220,
			117,
			192,
			118,
			245,
			103,
			107,
			126,
			235,
			82,
			203,
			209,
			91,
			159,
			11,
			219,
			64,
			146,
			26,
			250,
			172,
			228,
			225,
			113,
			31,
			101,
			141,
			151,
			158,
			149,
			144,
			93,
			183,
			193,
			175,
			84,
			251,
			2,
			224,
			53,
			187,
			58,
			77,
			173,
			44,
			61,
			86,
			8,
			27,
			74,
			147,
			106,
			171,
			184,
			122,
			242,
			125,
			218,
			63,
			254,
			62,
			190,
			234,
			170,
			68,
			198,
			208,
			54,
			72,
			112,
			150,
			119,
			36,
			83,
			223,
			243,
			131,
			40,
			50,
			69,
			30,
			164,
			211,
			162,
			70,
			110,
			156,
			221,
			99,
			212,
			157
		};
	}
}
