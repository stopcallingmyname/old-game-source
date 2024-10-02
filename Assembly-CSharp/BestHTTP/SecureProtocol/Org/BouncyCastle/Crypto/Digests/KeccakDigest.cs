using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005AA RID: 1450
	public class KeccakDigest : IDigest, IMemoable
	{
		// Token: 0x0600372F RID: 14127 RVA: 0x001568C1 File Offset: 0x00154AC1
		public KeccakDigest() : this(288)
		{
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x001568CE File Offset: 0x00154ACE
		public KeccakDigest(int bitLength)
		{
			this.state = new ulong[25];
			this.dataQueue = new byte[192];
			base..ctor();
			this.Init(bitLength);
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x001568FA File Offset: 0x00154AFA
		public KeccakDigest(KeccakDigest source)
		{
			this.state = new ulong[25];
			this.dataQueue = new byte[192];
			base..ctor();
			this.CopyIn(source);
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x00156928 File Offset: 0x00154B28
		private void CopyIn(KeccakDigest source)
		{
			Array.Copy(source.state, 0, this.state, 0, source.state.Length);
			Array.Copy(source.dataQueue, 0, this.dataQueue, 0, source.dataQueue.Length);
			this.rate = source.rate;
			this.bitsInQueue = source.bitsInQueue;
			this.fixedOutputLength = source.fixedOutputLength;
			this.squeezing = source.squeezing;
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06003733 RID: 14131 RVA: 0x0015699B File Offset: 0x00154B9B
		public virtual string AlgorithmName
		{
			get
			{
				return "Keccak-" + this.fixedOutputLength;
			}
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x001569B2 File Offset: 0x00154BB2
		public virtual int GetDigestSize()
		{
			return this.fixedOutputLength >> 3;
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x001569BC File Offset: 0x00154BBC
		public virtual void Update(byte input)
		{
			this.Absorb(new byte[]
			{
				input
			}, 0, 1);
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x001569D0 File Offset: 0x00154BD0
		public virtual void BlockUpdate(byte[] input, int inOff, int len)
		{
			this.Absorb(input, inOff, len);
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x001569DB File Offset: 0x00154BDB
		public virtual int DoFinal(byte[] output, int outOff)
		{
			this.Squeeze(output, outOff, (long)this.fixedOutputLength);
			this.Reset();
			return this.GetDigestSize();
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x001569F8 File Offset: 0x00154BF8
		protected virtual int DoFinal(byte[] output, int outOff, byte partialByte, int partialBits)
		{
			if (partialBits > 0)
			{
				this.AbsorbBits((int)partialByte, partialBits);
			}
			this.Squeeze(output, outOff, (long)this.fixedOutputLength);
			this.Reset();
			return this.GetDigestSize();
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x00156A23 File Offset: 0x00154C23
		public virtual void Reset()
		{
			this.Init(this.fixedOutputLength);
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x00156A31 File Offset: 0x00154C31
		public virtual int GetByteLength()
		{
			return this.rate >> 3;
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x00156A3C File Offset: 0x00154C3C
		private void Init(int bitLength)
		{
			if (bitLength <= 256)
			{
				if (bitLength != 128 && bitLength != 224 && bitLength != 256)
				{
					goto IL_4A;
				}
			}
			else if (bitLength != 288 && bitLength != 384 && bitLength != 512)
			{
				goto IL_4A;
			}
			this.InitSponge(1600 - (bitLength << 1));
			return;
			IL_4A:
			throw new ArgumentException("must be one of 128, 224, 256, 288, 384, or 512.", "bitLength");
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x00156AA4 File Offset: 0x00154CA4
		private void InitSponge(int rate)
		{
			if (rate <= 0 || rate >= 1600 || (rate & 63) != 0)
			{
				throw new InvalidOperationException("invalid rate value");
			}
			this.rate = rate;
			Array.Clear(this.state, 0, this.state.Length);
			Arrays.Fill(this.dataQueue, 0);
			this.bitsInQueue = 0;
			this.squeezing = false;
			this.fixedOutputLength = 1600 - rate >> 1;
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x00156B14 File Offset: 0x00154D14
		protected void Absorb(byte[] data, int off, int len)
		{
			if ((this.bitsInQueue & 7) != 0)
			{
				throw new InvalidOperationException("attempt to absorb with odd length queue");
			}
			if (this.squeezing)
			{
				throw new InvalidOperationException("attempt to absorb while squeezing");
			}
			int num = this.bitsInQueue >> 3;
			int num2 = this.rate >> 3;
			int i = 0;
			while (i < len)
			{
				if (num == 0 && i <= len - num2)
				{
					do
					{
						this.KeccakAbsorb(data, off + i);
						i += num2;
					}
					while (i <= len - num2);
				}
				else
				{
					int num3 = Math.Min(num2 - num, len - i);
					Array.Copy(data, off + i, this.dataQueue, num, num3);
					num += num3;
					i += num3;
					if (num == num2)
					{
						this.KeccakAbsorb(this.dataQueue, 0);
						num = 0;
					}
				}
			}
			this.bitsInQueue = num << 3;
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x00156BC4 File Offset: 0x00154DC4
		protected void AbsorbBits(int data, int bits)
		{
			if (bits < 1 || bits > 7)
			{
				throw new ArgumentException("must be in the range 1 to 7", "bits");
			}
			if ((this.bitsInQueue & 7) != 0)
			{
				throw new InvalidOperationException("attempt to absorb with odd length queue");
			}
			if (this.squeezing)
			{
				throw new InvalidOperationException("attempt to absorb while squeezing");
			}
			int num = (1 << bits) - 1;
			this.dataQueue[this.bitsInQueue >> 3] = (byte)(data & num);
			this.bitsInQueue += bits;
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x00156C3C File Offset: 0x00154E3C
		private void PadAndSwitchToSqueezingPhase()
		{
			byte[] array = this.dataQueue;
			int num = this.bitsInQueue >> 3;
			array[num] |= (byte)(1 << (this.bitsInQueue & 7));
			int num2 = this.bitsInQueue + 1;
			this.bitsInQueue = num2;
			if (num2 == this.rate)
			{
				this.KeccakAbsorb(this.dataQueue, 0);
				this.bitsInQueue = 0;
			}
			int num3 = this.bitsInQueue >> 6;
			int num4 = this.bitsInQueue & 63;
			int num5 = 0;
			for (int i = 0; i < num3; i++)
			{
				this.state[i] ^= Pack.LE_To_UInt64(this.dataQueue, num5);
				num5 += 8;
			}
			if (num4 > 0)
			{
				ulong num6 = (1UL << num4) - 1UL;
				this.state[num3] ^= (Pack.LE_To_UInt64(this.dataQueue, num5) & num6);
			}
			this.state[this.rate - 1 >> 6] ^= 9223372036854775808UL;
			this.KeccakPermutation();
			this.KeccakExtract();
			this.bitsInQueue = this.rate;
			this.squeezing = true;
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x00156D54 File Offset: 0x00154F54
		protected void Squeeze(byte[] output, int offset, long outputLength)
		{
			if (!this.squeezing)
			{
				this.PadAndSwitchToSqueezingPhase();
			}
			if ((outputLength & 7L) != 0L)
			{
				throw new InvalidOperationException("outputLength not a multiple of 8");
			}
			int num2;
			for (long num = 0L; num < outputLength; num += (long)num2)
			{
				if (this.bitsInQueue == 0)
				{
					this.KeccakPermutation();
					this.KeccakExtract();
					this.bitsInQueue = this.rate;
				}
				num2 = (int)Math.Min((long)this.bitsInQueue, outputLength - num);
				Array.Copy(this.dataQueue, this.rate - this.bitsInQueue >> 3, output, offset + (int)(num >> 3), num2 >> 3);
				this.bitsInQueue -= num2;
			}
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x00156DF4 File Offset: 0x00154FF4
		private void KeccakAbsorb(byte[] data, int off)
		{
			int num = this.rate >> 6;
			for (int i = 0; i < num; i++)
			{
				this.state[i] ^= Pack.LE_To_UInt64(data, off);
				off += 8;
			}
			this.KeccakPermutation();
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x00156E38 File Offset: 0x00155038
		private void KeccakExtract()
		{
			Pack.UInt64_To_LE(this.state, 0, this.rate >> 6, this.dataQueue, 0);
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x00156E58 File Offset: 0x00155058
		private void KeccakPermutation()
		{
			ulong[] array = this.state;
			ulong num = array[0];
			ulong num2 = array[1];
			ulong num3 = array[2];
			ulong num4 = array[3];
			ulong num5 = array[4];
			ulong num6 = array[5];
			ulong num7 = array[6];
			ulong num8 = array[7];
			ulong num9 = array[8];
			ulong num10 = array[9];
			ulong num11 = array[10];
			ulong num12 = array[11];
			ulong num13 = array[12];
			ulong num14 = array[13];
			ulong num15 = array[14];
			ulong num16 = array[15];
			ulong num17 = array[16];
			ulong num18 = array[17];
			ulong num19 = array[18];
			ulong num20 = array[19];
			ulong num21 = array[20];
			ulong num22 = array[21];
			ulong num23 = array[22];
			ulong num24 = array[23];
			ulong num25 = array[24];
			for (int i = 0; i < 24; i++)
			{
				ulong num26 = num ^ num6 ^ num11 ^ num16 ^ num21;
				ulong num27 = num2 ^ num7 ^ num12 ^ num17 ^ num22;
				ulong num28 = num3 ^ num8 ^ num13 ^ num18 ^ num23;
				ulong num29 = num4 ^ num9 ^ num14 ^ num19 ^ num24;
				ulong num30 = num5 ^ num10 ^ num15 ^ num20 ^ num25;
				ulong num31 = (num27 << 1 | num27 >> 63) ^ num30;
				ulong num32 = (num28 << 1 | num28 >> 63) ^ num26;
				ulong num33 = (num29 << 1 | num29 >> 63) ^ num27;
				ulong num34 = (num30 << 1 | num30 >> 63) ^ num28;
				ulong num35 = (num26 << 1 | num26 >> 63) ^ num29;
				num ^= num31;
				num6 ^= num31;
				num11 ^= num31;
				num16 ^= num31;
				num21 ^= num31;
				num2 ^= num32;
				num7 ^= num32;
				num12 ^= num32;
				num17 ^= num32;
				num22 ^= num32;
				num3 ^= num33;
				num8 ^= num33;
				num13 ^= num33;
				num18 ^= num33;
				num23 ^= num33;
				num4 ^= num34;
				num9 ^= num34;
				num14 ^= num34;
				num19 ^= num34;
				num24 ^= num34;
				num5 ^= num35;
				num10 ^= num35;
				num15 ^= num35;
				num20 ^= num35;
				num25 ^= num35;
				num27 = (num2 << 1 | num2 >> 63);
				num2 = (num7 << 44 | num7 >> 20);
				num7 = (num10 << 20 | num10 >> 44);
				num10 = (num23 << 61 | num23 >> 3);
				num23 = (num15 << 39 | num15 >> 25);
				num15 = (num21 << 18 | num21 >> 46);
				num21 = (num3 << 62 | num3 >> 2);
				num3 = (num13 << 43 | num13 >> 21);
				num13 = (num14 << 25 | num14 >> 39);
				num14 = (num20 << 8 | num20 >> 56);
				num20 = (num24 << 56 | num24 >> 8);
				num24 = (num16 << 41 | num16 >> 23);
				num16 = (num5 << 27 | num5 >> 37);
				num5 = (num25 << 14 | num25 >> 50);
				num25 = (num22 << 2 | num22 >> 62);
				num22 = (num9 << 55 | num9 >> 9);
				num9 = (num17 << 45 | num17 >> 19);
				num17 = (num6 << 36 | num6 >> 28);
				num6 = (num4 << 28 | num4 >> 36);
				num4 = (num19 << 21 | num19 >> 43);
				num19 = (num18 << 15 | num18 >> 49);
				num18 = (num12 << 10 | num12 >> 54);
				num12 = (num8 << 6 | num8 >> 58);
				num8 = (num11 << 3 | num11 >> 61);
				num11 = num27;
				num26 = (num ^ (~num2 & num3));
				num27 = (num2 ^ (~num3 & num4));
				num3 ^= (~num4 & num5);
				num4 ^= (~num5 & num);
				num5 ^= (~num & num2);
				num = num26;
				num2 = num27;
				num26 = (num6 ^ (~num7 & num8));
				num27 = (num7 ^ (~num8 & num9));
				num8 ^= (~num9 & num10);
				num9 ^= (~num10 & num6);
				num10 ^= (~num6 & num7);
				num6 = num26;
				num7 = num27;
				num26 = (num11 ^ (~num12 & num13));
				num27 = (num12 ^ (~num13 & num14));
				num13 ^= (~num14 & num15);
				num14 ^= (~num15 & num11);
				num15 ^= (~num11 & num12);
				num11 = num26;
				num12 = num27;
				num26 = (num16 ^ (~num17 & num18));
				num27 = (num17 ^ (~num18 & num19));
				num18 ^= (~num19 & num20);
				num19 ^= (~num20 & num16);
				num20 ^= (~num16 & num17);
				num16 = num26;
				num17 = num27;
				num26 = (num21 ^ (~num22 & num23));
				num27 = (num22 ^ (~num23 & num24));
				num23 ^= (~num24 & num25);
				num24 ^= (~num25 & num21);
				num25 ^= (~num21 & num22);
				num21 = num26;
				num22 = num27;
				num ^= KeccakDigest.KeccakRoundConstants[i];
			}
			array[0] = num;
			array[1] = num2;
			array[2] = num3;
			array[3] = num4;
			array[4] = num5;
			array[5] = num6;
			array[6] = num7;
			array[7] = num8;
			array[8] = num9;
			array[9] = num10;
			array[10] = num11;
			array[11] = num12;
			array[12] = num13;
			array[13] = num14;
			array[14] = num15;
			array[15] = num16;
			array[16] = num17;
			array[17] = num18;
			array[18] = num19;
			array[19] = num20;
			array[20] = num21;
			array[21] = num22;
			array[22] = num23;
			array[23] = num24;
			array[24] = num25;
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x00157340 File Offset: 0x00155540
		public virtual IMemoable Copy()
		{
			return new KeccakDigest(this);
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x00157348 File Offset: 0x00155548
		public virtual void Reset(IMemoable other)
		{
			this.CopyIn((KeccakDigest)other);
		}

		// Token: 0x04002414 RID: 9236
		private static readonly ulong[] KeccakRoundConstants = new ulong[]
		{
			1UL,
			32898UL,
			9223372036854808714UL,
			9223372039002292224UL,
			32907UL,
			2147483649UL,
			9223372039002292353UL,
			9223372036854808585UL,
			138UL,
			136UL,
			2147516425UL,
			2147483658UL,
			2147516555UL,
			9223372036854775947UL,
			9223372036854808713UL,
			9223372036854808579UL,
			9223372036854808578UL,
			9223372036854775936UL,
			32778UL,
			9223372039002259466UL,
			9223372039002292353UL,
			9223372036854808704UL,
			2147483649UL,
			9223372039002292232UL
		};

		// Token: 0x04002415 RID: 9237
		private ulong[] state;

		// Token: 0x04002416 RID: 9238
		protected byte[] dataQueue;

		// Token: 0x04002417 RID: 9239
		protected int rate;

		// Token: 0x04002418 RID: 9240
		protected int bitsInQueue;

		// Token: 0x04002419 RID: 9241
		protected int fixedOutputLength;

		// Token: 0x0400241A RID: 9242
		protected bool squeezing;
	}
}
