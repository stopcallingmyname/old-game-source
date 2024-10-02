﻿using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x0200032F RID: 815
	internal class LongArray
	{
		// Token: 0x06001F2E RID: 7982 RVA: 0x000E66BB File Offset: 0x000E48BB
		public LongArray(int intLen)
		{
			this.m_ints = new long[intLen];
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x000E66CF File Offset: 0x000E48CF
		public LongArray(long[] ints)
		{
			this.m_ints = ints;
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x000E66DE File Offset: 0x000E48DE
		public LongArray(long[] ints, int off, int len)
		{
			if (off == 0 && len == ints.Length)
			{
				this.m_ints = ints;
				return;
			}
			this.m_ints = new long[len];
			Array.Copy(ints, off, this.m_ints, 0, len);
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x000E6714 File Offset: 0x000E4914
		public LongArray(BigInteger bigInt)
		{
			if (bigInt == null || bigInt.SignValue < 0)
			{
				throw new ArgumentException("invalid F2m field value", "bigInt");
			}
			if (bigInt.SignValue == 0)
			{
				this.m_ints = new long[1];
				return;
			}
			byte[] array = bigInt.ToByteArray();
			int num = array.Length;
			int num2 = 0;
			if (array[0] == 0)
			{
				num--;
				num2 = 1;
			}
			int num3 = (num + 7) / 8;
			this.m_ints = new long[num3];
			int i = num3 - 1;
			int num4 = num % 8 + num2;
			long num5 = 0L;
			int j = num2;
			if (num2 < num4)
			{
				while (j < num4)
				{
					num5 <<= 8;
					uint num6 = (uint)array[j];
					num5 |= (long)((ulong)num6);
					j++;
				}
				this.m_ints[i--] = num5;
			}
			while (i >= 0)
			{
				num5 = 0L;
				for (int k = 0; k < 8; k++)
				{
					num5 <<= 8;
					uint num7 = (uint)array[j++];
					num5 |= (long)((ulong)num7);
				}
				this.m_ints[i] = num5;
				i--;
			}
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x000E6811 File Offset: 0x000E4A11
		internal void CopyTo(long[] z, int zOff)
		{
			Array.Copy(this.m_ints, 0, z, zOff, this.m_ints.Length);
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x000E682C File Offset: 0x000E4A2C
		public bool IsOne()
		{
			long[] ints = this.m_ints;
			if (ints[0] != 1L)
			{
				return false;
			}
			for (int i = 1; i < ints.Length; i++)
			{
				if (ints[i] != 0L)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x000E6860 File Offset: 0x000E4A60
		public bool IsZero()
		{
			long[] ints = this.m_ints;
			for (int i = 0; i < ints.Length; i++)
			{
				if (ints[i] != 0L)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x000E688A File Offset: 0x000E4A8A
		public int GetUsedLength()
		{
			return this.GetUsedLengthFrom(this.m_ints.Length);
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x000E689C File Offset: 0x000E4A9C
		public int GetUsedLengthFrom(int from)
		{
			long[] ints = this.m_ints;
			from = Math.Min(from, ints.Length);
			if (from < 1)
			{
				return 0;
			}
			if (ints[0] != 0L)
			{
				while (ints[--from] == 0L)
				{
				}
				return from + 1;
			}
			while (ints[--from] == 0L)
			{
				if (from <= 0)
				{
					return 0;
				}
			}
			return from + 1;
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x000E68E8 File Offset: 0x000E4AE8
		public int Degree()
		{
			int num = this.m_ints.Length;
			while (num != 0)
			{
				long num2 = this.m_ints[--num];
				if (num2 != 0L)
				{
					return (num << 6) + LongArray.BitLength(num2);
				}
			}
			return 0;
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x000E6920 File Offset: 0x000E4B20
		private int DegreeFrom(int limit)
		{
			int num = (int)((uint)(limit + 62) >> 6);
			while (num != 0)
			{
				long num2 = this.m_ints[--num];
				if (num2 != 0L)
				{
					return (num << 6) + LongArray.BitLength(num2);
				}
			}
			return 0;
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x000E6954 File Offset: 0x000E4B54
		private static int BitLength(long w)
		{
			int num = (int)((ulong)w >> 32);
			int num2;
			if (num == 0)
			{
				num = (int)w;
				num2 = 0;
			}
			else
			{
				num2 = 32;
			}
			int num3 = (int)((uint)num >> 16);
			int num4;
			if (num3 == 0)
			{
				num3 = (int)((uint)num >> 8);
				num4 = (int)((num3 == 0) ? LongArray.BitLengths[num] : (8 + LongArray.BitLengths[num3]));
			}
			else
			{
				int num5 = (int)((uint)num3 >> 8);
				num4 = (int)((num5 == 0) ? (16 + LongArray.BitLengths[num3]) : (24 + LongArray.BitLengths[num5]));
			}
			return num2 + num4;
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x000E69BC File Offset: 0x000E4BBC
		private long[] ResizedInts(int newLen)
		{
			long[] array = new long[newLen];
			Array.Copy(this.m_ints, 0, array, 0, Math.Min(this.m_ints.Length, newLen));
			return array;
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x000E69F0 File Offset: 0x000E4BF0
		public BigInteger ToBigInteger()
		{
			int usedLength = this.GetUsedLength();
			if (usedLength == 0)
			{
				return BigInteger.Zero;
			}
			long num = this.m_ints[usedLength - 1];
			byte[] array = new byte[8];
			int num2 = 0;
			bool flag = false;
			for (int i = 7; i >= 0; i--)
			{
				byte b = (byte)((ulong)num >> 8 * i);
				if (flag || b != 0)
				{
					flag = true;
					array[num2++] = b;
				}
			}
			byte[] array2 = new byte[8 * (usedLength - 1) + num2];
			for (int j = 0; j < num2; j++)
			{
				array2[j] = array[j];
			}
			for (int k = usedLength - 2; k >= 0; k--)
			{
				long num3 = this.m_ints[k];
				for (int l = 7; l >= 0; l--)
				{
					array2[num2++] = (byte)((ulong)num3 >> 8 * l);
				}
			}
			return new BigInteger(1, array2);
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x000E6AC4 File Offset: 0x000E4CC4
		private static long ShiftUp(long[] x, int xOff, int count, int shift)
		{
			int num = 64 - shift;
			long num2 = 0L;
			for (int i = 0; i < count; i++)
			{
				long num3 = x[xOff + i];
				x[xOff + i] = (num3 << shift | num2);
				num2 = (long)((ulong)num3 >> num);
			}
			return num2;
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x000E6B00 File Offset: 0x000E4D00
		private static long ShiftUp(long[] x, int xOff, long[] z, int zOff, int count, int shift)
		{
			int num = 64 - shift;
			long num2 = 0L;
			for (int i = 0; i < count; i++)
			{
				long num3 = x[xOff + i];
				z[zOff + i] = (num3 << shift | num2);
				num2 = (long)((ulong)num3 >> num);
			}
			return num2;
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x000E6B40 File Offset: 0x000E4D40
		public LongArray AddOne()
		{
			if (this.m_ints.Length == 0)
			{
				return new LongArray(new long[]
				{
					1L
				});
			}
			int newLen = Math.Max(1, this.GetUsedLength());
			long[] array = this.ResizedInts(newLen);
			array[0] ^= 1L;
			return new LongArray(array);
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x000E6B90 File Offset: 0x000E4D90
		private void AddShiftedByBitsSafe(LongArray other, int otherDegree, int bits)
		{
			int num = (int)((uint)(otherDegree + 63) >> 6);
			int num2 = (int)((uint)bits >> 6);
			int num3 = bits & 63;
			if (num3 == 0)
			{
				LongArray.Add(this.m_ints, num2, other.m_ints, 0, num);
				return;
			}
			long num4 = LongArray.AddShiftedUp(this.m_ints, num2, other.m_ints, 0, num, num3);
			if (num4 != 0L)
			{
				this.m_ints[num + num2] ^= num4;
			}
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x000E6BF4 File Offset: 0x000E4DF4
		private static long AddShiftedUp(long[] x, int xOff, long[] y, int yOff, int count, int shift)
		{
			int num = 64 - shift;
			long num2 = 0L;
			for (int i = 0; i < count; i++)
			{
				long num3 = y[yOff + i];
				x[xOff + i] ^= (num3 << shift | num2);
				num2 = (long)((ulong)num3 >> num);
			}
			return num2;
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x000E6C3C File Offset: 0x000E4E3C
		private static long AddShiftedDown(long[] x, int xOff, long[] y, int yOff, int count, int shift)
		{
			int num = 64 - shift;
			long num2 = 0L;
			int num3 = count;
			while (--num3 >= 0)
			{
				long num4 = y[yOff + num3];
				x[xOff + num3] ^= (long)((ulong)num4 >> shift | (ulong)num2);
				num2 = num4 << num;
			}
			return num2;
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x000E6C84 File Offset: 0x000E4E84
		public void AddShiftedByWords(LongArray other, int words)
		{
			int usedLength = other.GetUsedLength();
			if (usedLength == 0)
			{
				return;
			}
			int num = usedLength + words;
			if (num > this.m_ints.Length)
			{
				this.m_ints = this.ResizedInts(num);
			}
			LongArray.Add(this.m_ints, words, other.m_ints, 0, usedLength);
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x000E6CCC File Offset: 0x000E4ECC
		private static void Add(long[] x, int xOff, long[] y, int yOff, int count)
		{
			for (int i = 0; i < count; i++)
			{
				x[xOff + i] ^= y[yOff + i];
			}
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x000E6CF8 File Offset: 0x000E4EF8
		private static void Add(long[] x, int xOff, long[] y, int yOff, long[] z, int zOff, int count)
		{
			for (int i = 0; i < count; i++)
			{
				z[zOff + i] = (x[xOff + i] ^ y[yOff + i]);
			}
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x000E6D24 File Offset: 0x000E4F24
		private static void AddBoth(long[] x, int xOff, long[] y1, int y1Off, long[] y2, int y2Off, int count)
		{
			for (int i = 0; i < count; i++)
			{
				x[xOff + i] ^= (y1[y1Off + i] ^ y2[y2Off + i]);
			}
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x000E6D58 File Offset: 0x000E4F58
		private static void Distribute(long[] x, int src, int dst1, int dst2, int count)
		{
			for (int i = 0; i < count; i++)
			{
				long num = x[src + i];
				x[dst1 + i] ^= num;
				x[dst2 + i] ^= num;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001F47 RID: 8007 RVA: 0x000E6D94 File Offset: 0x000E4F94
		public int Length
		{
			get
			{
				return this.m_ints.Length;
			}
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x000E6DA0 File Offset: 0x000E4FA0
		private static void FlipWord(long[] buf, int off, int bit, long word)
		{
			int num = off + (int)((uint)bit >> 6);
			int num2 = bit & 63;
			if (num2 == 0)
			{
				buf[num] ^= word;
				return;
			}
			buf[num] ^= word << num2;
			word = (long)((ulong)word >> 64 - num2);
			if (word != 0L)
			{
				buf[num + 1] ^= word;
			}
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x000E6DF7 File Offset: 0x000E4FF7
		public bool TestBitZero()
		{
			return this.m_ints.Length != 0 && (this.m_ints[0] & 1L) != 0L;
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x000E6E14 File Offset: 0x000E5014
		private static bool TestBit(long[] buf, int off, int n)
		{
			int num = (int)((uint)n >> 6);
			int num2 = n & 63;
			long num3 = 1L << num2;
			return (buf[off + num] & num3) != 0L;
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x000E6E40 File Offset: 0x000E5040
		private static void FlipBit(long[] buf, int off, int n)
		{
			int num = (int)((uint)n >> 6);
			int num2 = n & 63;
			long num3 = 1L << num2;
			buf[off + num] ^= num3;
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x000E6E6C File Offset: 0x000E506C
		private static void MultiplyWord(long a, long[] b, int bLen, long[] c, int cOff)
		{
			if ((a & 1L) != 0L)
			{
				LongArray.Add(c, cOff, b, 0, bLen);
			}
			int num = 1;
			while ((a = (long)((ulong)a >> 1)) != 0L)
			{
				if ((a & 1L) != 0L)
				{
					long num2 = LongArray.AddShiftedUp(c, cOff, b, 0, bLen, num);
					if (num2 != 0L)
					{
						c[cOff + bLen] ^= num2;
					}
				}
				num++;
			}
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x000E6EC0 File Offset: 0x000E50C0
		public LongArray ModMultiplyLD(LongArray other, int m, int[] ks)
		{
			int num = this.Degree();
			if (num == 0)
			{
				return this;
			}
			int num2 = other.Degree();
			if (num2 == 0)
			{
				return other;
			}
			LongArray longArray = this;
			LongArray longArray2 = other;
			if (num > num2)
			{
				longArray = other;
				longArray2 = this;
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			int num4 = (int)((uint)(num + 63) >> 6);
			int num5 = (int)((uint)(num2 + 63) >> 6);
			int num6 = (int)((uint)(num + num2 + 62) >> 6);
			if (num4 != 1)
			{
				int num7 = (int)((uint)(num2 + 7 + 63) >> 6);
				int[] array = new int[16];
				long[] array2 = new long[num7 << 4];
				int num8 = num7;
				array[1] = num8;
				Array.Copy(longArray2.m_ints, 0, array2, num8, num5);
				for (int i = 2; i < 16; i++)
				{
					num8 = (array[i] = num8 + num7);
					if ((i & 1) == 0)
					{
						LongArray.ShiftUp(array2, (int)((uint)num8 >> 1), array2, num8, num7, 1);
					}
					else
					{
						LongArray.Add(array2, num7, array2, num8 - num7, array2, num8, num7);
					}
				}
				long[] array3 = new long[array2.Length];
				LongArray.ShiftUp(array2, 0, array3, 0, array2.Length, 4);
				long[] ints = longArray.m_ints;
				long[] array4 = new long[num6];
				int num9 = 15;
				for (int j = 56; j >= 0; j -= 8)
				{
					for (int k = 1; k < num4; k += 2)
					{
						int num10 = (int)((ulong)ints[k] >> j);
						int num11 = num10 & num9;
						int num12 = (int)((uint)num10 >> 4 & (uint)num9);
						LongArray.AddBoth(array4, k - 1, array2, array[num11], array3, array[num12], num7);
					}
					LongArray.ShiftUp(array4, 0, num6, 8);
				}
				for (int l = 56; l >= 0; l -= 8)
				{
					for (int n = 0; n < num4; n += 2)
					{
						int num13 = (int)((ulong)ints[n] >> l);
						int num14 = num13 & num9;
						int num15 = (int)((uint)num13 >> 4 & (uint)num9);
						LongArray.AddBoth(array4, n, array2, array[num14], array3, array[num15], num7);
					}
					if (l > 0)
					{
						LongArray.ShiftUp(array4, 0, num6, 8);
					}
				}
				return LongArray.ReduceResult(array4, 0, num6, m, ks);
			}
			long num16 = longArray.m_ints[0];
			if (num16 == 1L)
			{
				return longArray2;
			}
			long[] array5 = new long[num6];
			LongArray.MultiplyWord(num16, longArray2.m_ints, num5, array5, 0);
			return LongArray.ReduceResult(array5, 0, num6, m, ks);
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x000E70DC File Offset: 0x000E52DC
		public LongArray ModMultiply(LongArray other, int m, int[] ks)
		{
			int num = this.Degree();
			if (num == 0)
			{
				return this;
			}
			int num2 = other.Degree();
			if (num2 == 0)
			{
				return other;
			}
			LongArray longArray = this;
			LongArray longArray2 = other;
			if (num > num2)
			{
				longArray = other;
				longArray2 = this;
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			int num4 = (int)((uint)(num + 63) >> 6);
			int num5 = (int)((uint)(num2 + 63) >> 6);
			int num6 = (int)((uint)(num + num2 + 62) >> 6);
			if (num4 != 1)
			{
				int num7 = (int)((uint)(num2 + 7 + 63) >> 6);
				int[] array = new int[16];
				long[] array2 = new long[num7 << 4];
				int num8 = num7;
				array[1] = num8;
				Array.Copy(longArray2.m_ints, 0, array2, num8, num5);
				for (int i = 2; i < 16; i++)
				{
					num8 = (array[i] = num8 + num7);
					if ((i & 1) == 0)
					{
						LongArray.ShiftUp(array2, (int)((uint)num8 >> 1), array2, num8, num7, 1);
					}
					else
					{
						LongArray.Add(array2, num7, array2, num8 - num7, array2, num8, num7);
					}
				}
				long[] array3 = new long[array2.Length];
				LongArray.ShiftUp(array2, 0, array3, 0, array2.Length, 4);
				long[] ints = longArray.m_ints;
				long[] array4 = new long[num6 << 3];
				int num9 = 15;
				for (int j = 0; j < num4; j++)
				{
					long num10 = ints[j];
					int num11 = j;
					for (;;)
					{
						int num12 = (int)num10 & num9;
						num10 = (long)((ulong)num10 >> 4);
						int num13 = (int)num10 & num9;
						LongArray.AddBoth(array4, num11, array2, array[num12], array3, array[num13], num7);
						num10 = (long)((ulong)num10 >> 4);
						if (num10 == 0L)
						{
							break;
						}
						num11 += num6;
					}
				}
				int num14 = array4.Length;
				while ((num14 -= num6) != 0)
				{
					LongArray.AddShiftedUp(array4, num14 - num6, array4, num14, num6, 8);
				}
				return LongArray.ReduceResult(array4, 0, num6, m, ks);
			}
			long num15 = longArray.m_ints[0];
			if (num15 == 1L)
			{
				return longArray2;
			}
			long[] array5 = new long[num6];
			LongArray.MultiplyWord(num15, longArray2.m_ints, num5, array5, 0);
			return LongArray.ReduceResult(array5, 0, num6, m, ks);
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x000E72B8 File Offset: 0x000E54B8
		public LongArray ModMultiplyAlt(LongArray other, int m, int[] ks)
		{
			int num = this.Degree();
			if (num == 0)
			{
				return this;
			}
			int num2 = other.Degree();
			if (num2 == 0)
			{
				return other;
			}
			LongArray longArray = this;
			LongArray longArray2 = other;
			if (num > num2)
			{
				longArray = other;
				longArray2 = this;
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			int num4 = (int)((uint)(num + 63) >> 6);
			int num5 = (int)((uint)(num2 + 63) >> 6);
			int num6 = (int)((uint)(num + num2 + 62) >> 6);
			if (num4 != 1)
			{
				int num7 = 4;
				int num8 = 16;
				int num9 = 64;
				int num10 = 8;
				int num11 = (num9 < 64) ? num8 : (num8 - 1);
				int num12 = (int)((uint)(num2 + num11 + 63) >> 6);
				int num13 = num12 * num10;
				int num14 = num7 * num10;
				int[] array = new int[1 << num7];
				int num15 = num4;
				array[0] = num15;
				num15 += num13;
				array[1] = num15;
				for (int i = 2; i < array.Length; i++)
				{
					num15 += num6;
					array[i] = num15;
				}
				num15 += num6;
				num15++;
				long[] array2 = new long[num15];
				LongArray.Interleave(longArray.m_ints, 0, array2, 0, num4, num7);
				int num16 = num4;
				Array.Copy(longArray2.m_ints, 0, array2, num16, num5);
				for (int j = 1; j < num10; j++)
				{
					LongArray.ShiftUp(array2, num4, array2, num16 += num12, num12, j);
				}
				int num17 = (1 << num7) - 1;
				int num18 = 0;
				for (;;)
				{
					int num19 = 0;
					do
					{
						long num20 = (long)((ulong)array2[num19] >> num18);
						int num21 = 0;
						int num22 = num4;
						for (;;)
						{
							int num23 = (int)num20 & num17;
							if (num23 != 0)
							{
								LongArray.Add(array2, num19 + array[num23], array2, num22, num12);
							}
							if (++num21 == num10)
							{
								break;
							}
							num22 += num12;
							num20 = (long)((ulong)num20 >> num7);
						}
					}
					while (++num19 < num4);
					if ((num18 += num14) >= num9)
					{
						if (num18 >= 64)
						{
							break;
						}
						num18 = 64 - num7;
						num17 &= num17 << num9 - num18;
					}
					LongArray.ShiftUp(array2, num4, num13, num10);
				}
				int num24 = array.Length;
				while (--num24 > 1)
				{
					if (((long)num24 & 1L) == 0L)
					{
						LongArray.AddShiftedUp(array2, array[(int)((uint)num24 >> 1)], array2, array[num24], num6, num8);
					}
					else
					{
						LongArray.Distribute(array2, array[num24], array[num24 - 1], array[1], num6);
					}
				}
				return LongArray.ReduceResult(array2, array[1], num6, m, ks);
			}
			long num25 = longArray.m_ints[0];
			if (num25 == 1L)
			{
				return longArray2;
			}
			long[] array3 = new long[num6];
			LongArray.MultiplyWord(num25, longArray2.m_ints, num5, array3, 0);
			return LongArray.ReduceResult(array3, 0, num6, m, ks);
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x000E7534 File Offset: 0x000E5734
		public LongArray ModReduce(int m, int[] ks)
		{
			long[] array = Arrays.Clone(this.m_ints);
			int len = LongArray.ReduceInPlace(array, 0, array.Length, m, ks);
			return new LongArray(array, 0, len);
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x000E7564 File Offset: 0x000E5764
		public LongArray Multiply(LongArray other, int m, int[] ks)
		{
			int num = this.Degree();
			if (num == 0)
			{
				return this;
			}
			int num2 = other.Degree();
			if (num2 == 0)
			{
				return other;
			}
			LongArray longArray = this;
			LongArray longArray2 = other;
			if (num > num2)
			{
				longArray = other;
				longArray2 = this;
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			int num4 = (int)((uint)(num + 63) >> 6);
			int num5 = (int)((uint)(num2 + 63) >> 6);
			int num6 = (int)((uint)(num + num2 + 62) >> 6);
			if (num4 != 1)
			{
				int num7 = (int)((uint)(num2 + 7 + 63) >> 6);
				int[] array = new int[16];
				long[] array2 = new long[num7 << 4];
				int num8 = num7;
				array[1] = num8;
				Array.Copy(longArray2.m_ints, 0, array2, num8, num5);
				for (int i = 2; i < 16; i++)
				{
					num8 = (array[i] = num8 + num7);
					if ((i & 1) == 0)
					{
						LongArray.ShiftUp(array2, (int)((uint)num8 >> 1), array2, num8, num7, 1);
					}
					else
					{
						LongArray.Add(array2, num7, array2, num8 - num7, array2, num8, num7);
					}
				}
				long[] array3 = new long[array2.Length];
				LongArray.ShiftUp(array2, 0, array3, 0, array2.Length, 4);
				long[] ints = longArray.m_ints;
				long[] array4 = new long[num6 << 3];
				int num9 = 15;
				for (int j = 0; j < num4; j++)
				{
					long num10 = ints[j];
					int num11 = j;
					for (;;)
					{
						int num12 = (int)num10 & num9;
						num10 = (long)((ulong)num10 >> 4);
						int num13 = (int)num10 & num9;
						LongArray.AddBoth(array4, num11, array2, array[num12], array3, array[num13], num7);
						num10 = (long)((ulong)num10 >> 4);
						if (num10 == 0L)
						{
							break;
						}
						num11 += num6;
					}
				}
				int num14 = array4.Length;
				while ((num14 -= num6) != 0)
				{
					LongArray.AddShiftedUp(array4, num14 - num6, array4, num14, num6, 8);
				}
				return new LongArray(array4, 0, num6);
			}
			long num15 = longArray.m_ints[0];
			if (num15 == 1L)
			{
				return longArray2;
			}
			long[] array5 = new long[num6];
			LongArray.MultiplyWord(num15, longArray2.m_ints, num5, array5, 0);
			return new LongArray(array5, 0, num6);
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x000E773C File Offset: 0x000E593C
		public void Reduce(int m, int[] ks)
		{
			long[] ints = this.m_ints;
			int num = LongArray.ReduceInPlace(ints, 0, ints.Length, m, ks);
			if (num < ints.Length)
			{
				this.m_ints = new long[num];
				Array.Copy(ints, 0, this.m_ints, 0, num);
			}
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x000E7780 File Offset: 0x000E5980
		private static LongArray ReduceResult(long[] buf, int off, int len, int m, int[] ks)
		{
			int len2 = LongArray.ReduceInPlace(buf, off, len, m, ks);
			return new LongArray(buf, off, len2);
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x000E77A4 File Offset: 0x000E59A4
		private static int ReduceInPlace(long[] buf, int off, int len, int m, int[] ks)
		{
			int num = m + 63 >> 6;
			if (len < num)
			{
				return len;
			}
			int num2 = Math.Min(len << 6, (m << 1) - 1);
			int i;
			for (i = (len << 6) - num2; i >= 64; i -= 64)
			{
				len--;
			}
			int num3 = ks.Length;
			int num4 = ks[num3 - 1];
			int num5 = (num3 > 1) ? ks[num3 - 2] : 0;
			int num6 = Math.Max(m, num4 + 64);
			int num7 = i + Math.Min(num2 - num6, m - num5) >> 6;
			if (num7 > 1)
			{
				int num8 = len - num7;
				LongArray.ReduceVectorWise(buf, off, len, num8, m, ks);
				while (len > num8)
				{
					buf[off + --len] = 0L;
				}
				num2 = num8 << 6;
			}
			if (num2 > num6)
			{
				LongArray.ReduceWordWise(buf, off, len, num6, m, ks);
				num2 = num6;
			}
			if (num2 > m)
			{
				LongArray.ReduceBitWise(buf, off, num2, m, ks);
			}
			return num;
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x000E7874 File Offset: 0x000E5A74
		private static void ReduceBitWise(long[] buf, int off, int BitLength, int m, int[] ks)
		{
			while (--BitLength >= m)
			{
				if (LongArray.TestBit(buf, off, BitLength))
				{
					LongArray.ReduceBit(buf, off, BitLength, m, ks);
				}
			}
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x000E7898 File Offset: 0x000E5A98
		private static void ReduceBit(long[] buf, int off, int bit, int m, int[] ks)
		{
			LongArray.FlipBit(buf, off, bit);
			int num = bit - m;
			int num2 = ks.Length;
			while (--num2 >= 0)
			{
				LongArray.FlipBit(buf, off, ks[num2] + num);
			}
			LongArray.FlipBit(buf, off, num);
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x000E78D8 File Offset: 0x000E5AD8
		private static void ReduceWordWise(long[] buf, int off, int len, int toBit, int m, int[] ks)
		{
			int num = (int)((uint)toBit >> 6);
			while (--len > num)
			{
				long num2 = buf[off + len];
				if (num2 != 0L)
				{
					buf[off + len] = 0L;
					LongArray.ReduceWord(buf, off, len << 6, num2, m, ks);
				}
			}
			int num3 = toBit & 63;
			long num4 = (long)((ulong)buf[off + num] >> num3);
			if (num4 != 0L)
			{
				buf[off + num] ^= num4 << num3;
				LongArray.ReduceWord(buf, off, toBit, num4, m, ks);
			}
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x000E7948 File Offset: 0x000E5B48
		private static void ReduceWord(long[] buf, int off, int bit, long word, int m, int[] ks)
		{
			int num = bit - m;
			int num2 = ks.Length;
			while (--num2 >= 0)
			{
				LongArray.FlipWord(buf, off, num + ks[num2], word);
			}
			LongArray.FlipWord(buf, off, num, word);
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x000E7980 File Offset: 0x000E5B80
		private static void ReduceVectorWise(long[] buf, int off, int len, int words, int m, int[] ks)
		{
			int num = (words << 6) - m;
			int num2 = ks.Length;
			while (--num2 >= 0)
			{
				LongArray.FlipVector(buf, off, buf, off + words, len - words, num + ks[num2]);
			}
			LongArray.FlipVector(buf, off, buf, off + words, len - words, num);
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x000E79C8 File Offset: 0x000E5BC8
		private static void FlipVector(long[] x, int xOff, long[] y, int yOff, int yLen, int bits)
		{
			xOff += (int)((uint)bits >> 6);
			bits &= 63;
			if (bits == 0)
			{
				LongArray.Add(x, xOff, y, yOff, yLen);
				return;
			}
			long num = LongArray.AddShiftedDown(x, xOff + 1, y, yOff, yLen, 64 - bits);
			x[xOff] ^= num;
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x000E7A14 File Offset: 0x000E5C14
		public LongArray ModSquare(int m, int[] ks)
		{
			int usedLength = this.GetUsedLength();
			if (usedLength == 0)
			{
				return this;
			}
			int num = usedLength << 1;
			long[] array = new long[num];
			int i = 0;
			while (i < num)
			{
				long num2 = this.m_ints[(int)((uint)i >> 1)];
				array[i++] = LongArray.Interleave2_32to64((int)num2);
				array[i++] = LongArray.Interleave2_32to64((int)((ulong)num2 >> 32));
			}
			return new LongArray(array, 0, LongArray.ReduceInPlace(array, 0, array.Length, m, ks));
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x000E7A80 File Offset: 0x000E5C80
		public LongArray ModSquareN(int n, int m, int[] ks)
		{
			int num = this.GetUsedLength();
			if (num == 0)
			{
				return this;
			}
			long[] array = new long[m + 63 >> 6 << 1];
			Array.Copy(this.m_ints, 0, array, 0, num);
			while (--n >= 0)
			{
				LongArray.SquareInPlace(array, num, m, ks);
				num = LongArray.ReduceInPlace(array, 0, array.Length, m, ks);
			}
			return new LongArray(array, 0, num);
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x000E7AE0 File Offset: 0x000E5CE0
		public LongArray Square(int m, int[] ks)
		{
			int usedLength = this.GetUsedLength();
			if (usedLength == 0)
			{
				return this;
			}
			int num = usedLength << 1;
			long[] array = new long[num];
			int i = 0;
			while (i < num)
			{
				long num2 = this.m_ints[(int)((uint)i >> 1)];
				array[i++] = LongArray.Interleave2_32to64((int)num2);
				array[i++] = LongArray.Interleave2_32to64((int)((ulong)num2 >> 32));
			}
			return new LongArray(array, 0, array.Length);
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x000E7B44 File Offset: 0x000E5D44
		private static void SquareInPlace(long[] x, int xLen, int m, int[] ks)
		{
			int num = xLen << 1;
			while (--xLen >= 0)
			{
				long num2 = x[xLen];
				x[--num] = LongArray.Interleave2_32to64((int)((ulong)num2 >> 32));
				x[--num] = LongArray.Interleave2_32to64((int)num2);
			}
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x000E7B84 File Offset: 0x000E5D84
		private static void Interleave(long[] x, int xOff, long[] z, int zOff, int count, int width)
		{
			switch (width)
			{
			case 3:
				LongArray.Interleave3(x, xOff, z, zOff, count);
				return;
			case 5:
				LongArray.Interleave5(x, xOff, z, zOff, count);
				return;
			case 7:
				LongArray.Interleave7(x, xOff, z, zOff, count);
				return;
			}
			LongArray.Interleave2_n(x, xOff, z, zOff, count, (int)(LongArray.BitLengths[width] - 1));
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x000E7BEC File Offset: 0x000E5DEC
		private static void Interleave3(long[] x, int xOff, long[] z, int zOff, int count)
		{
			for (int i = 0; i < count; i++)
			{
				z[zOff + i] = LongArray.Interleave3(x[xOff + i]);
			}
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x000E7C18 File Offset: 0x000E5E18
		private static long Interleave3(long x)
		{
			return (x & long.MinValue) | LongArray.Interleave3_21to63((int)x & 2097151) | LongArray.Interleave3_21to63((int)((ulong)x >> 21) & 2097151) << 1 | LongArray.Interleave3_21to63((int)((ulong)x >> 42) & 2097151) << 2;
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x000E7C64 File Offset: 0x000E5E64
		private static long Interleave3_21to63(int x)
		{
			int num = LongArray.INTERLEAVE3_TABLE[x & 127];
			int num2 = LongArray.INTERLEAVE3_TABLE[(int)((uint)x >> 7 & 127U)];
			return ((long)LongArray.INTERLEAVE3_TABLE[(int)((uint)x >> 14)] & (long)((ulong)-1)) << 42 | ((long)num2 & (long)((ulong)-1)) << 21 | ((long)num & (long)((ulong)-1));
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x000E7CAC File Offset: 0x000E5EAC
		private static void Interleave5(long[] x, int xOff, long[] z, int zOff, int count)
		{
			for (int i = 0; i < count; i++)
			{
				z[zOff + i] = LongArray.Interleave5(x[xOff + i]);
			}
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x000E7CD8 File Offset: 0x000E5ED8
		private static long Interleave5(long x)
		{
			return LongArray.Interleave3_13to65((int)x & 8191) | LongArray.Interleave3_13to65((int)((ulong)x >> 13) & 8191) << 1 | LongArray.Interleave3_13to65((int)((ulong)x >> 26) & 8191) << 2 | LongArray.Interleave3_13to65((int)((ulong)x >> 39) & 8191) << 3 | LongArray.Interleave3_13to65((int)((ulong)x >> 52) & 8191) << 4;
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x000E7D40 File Offset: 0x000E5F40
		private static long Interleave3_13to65(int x)
		{
			int num = LongArray.INTERLEAVE5_TABLE[x & 127];
			return ((long)LongArray.INTERLEAVE5_TABLE[(int)((uint)x >> 7)] & (long)((ulong)-1)) << 35 | ((long)num & (long)((ulong)-1));
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x000E7D70 File Offset: 0x000E5F70
		private static void Interleave7(long[] x, int xOff, long[] z, int zOff, int count)
		{
			for (int i = 0; i < count; i++)
			{
				z[zOff + i] = LongArray.Interleave7(x[xOff + i]);
			}
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x000E7D9C File Offset: 0x000E5F9C
		private static long Interleave7(long x)
		{
			return (x & long.MinValue) | LongArray.INTERLEAVE7_TABLE[(int)x & 511] | LongArray.INTERLEAVE7_TABLE[(int)((ulong)x >> 9) & 511] << 1 | LongArray.INTERLEAVE7_TABLE[(int)((ulong)x >> 18) & 511] << 2 | LongArray.INTERLEAVE7_TABLE[(int)((ulong)x >> 27) & 511] << 3 | LongArray.INTERLEAVE7_TABLE[(int)((ulong)x >> 36) & 511] << 4 | LongArray.INTERLEAVE7_TABLE[(int)((ulong)x >> 45) & 511] << 5 | LongArray.INTERLEAVE7_TABLE[(int)((ulong)x >> 54) & 511] << 6;
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x000E7E3C File Offset: 0x000E603C
		private static void Interleave2_n(long[] x, int xOff, long[] z, int zOff, int count, int rounds)
		{
			for (int i = 0; i < count; i++)
			{
				z[zOff + i] = LongArray.Interleave2_n(x[xOff + i], rounds);
			}
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x000E7E68 File Offset: 0x000E6068
		private static long Interleave2_n(long x, int rounds)
		{
			while (rounds > 1)
			{
				rounds -= 2;
				x = (LongArray.Interleave4_16to64((int)x & 65535) | LongArray.Interleave4_16to64((int)((ulong)x >> 16) & 65535) << 1 | LongArray.Interleave4_16to64((int)((ulong)x >> 32) & 65535) << 2 | LongArray.Interleave4_16to64((int)((ulong)x >> 48) & 65535) << 3);
			}
			if (rounds > 0)
			{
				x = (LongArray.Interleave2_32to64((int)x) | LongArray.Interleave2_32to64((int)((ulong)x >> 32)) << 1);
			}
			return x;
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x000E7EE4 File Offset: 0x000E60E4
		private static long Interleave4_16to64(int x)
		{
			int num = LongArray.INTERLEAVE4_TABLE[x & 255];
			return ((long)LongArray.INTERLEAVE4_TABLE[(int)((uint)x >> 8)] & (long)((ulong)-1)) << 32 | ((long)num & (long)((ulong)-1));
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x000E7F18 File Offset: 0x000E6118
		private static long Interleave2_32to64(int x)
		{
			int num = (int)LongArray.INTERLEAVE2_TABLE[x & 255] | (int)LongArray.INTERLEAVE2_TABLE[(int)((uint)x >> 8 & 255U)] << 16;
			return ((long)((int)LongArray.INTERLEAVE2_TABLE[(int)((uint)x >> 16 & 255U)] | (int)LongArray.INTERLEAVE2_TABLE[(int)((uint)x >> 24)] << 16) & (long)((ulong)-1)) << 32 | ((long)num & (long)((ulong)-1));
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x000E7F74 File Offset: 0x000E6174
		public LongArray ModInverse(int m, int[] ks)
		{
			int num = this.Degree();
			if (num == 0)
			{
				throw new InvalidOperationException();
			}
			if (num == 1)
			{
				return this;
			}
			LongArray longArray = this.Copy();
			int intLen = m + 63 >> 6;
			LongArray longArray2 = new LongArray(intLen);
			LongArray.ReduceBit(longArray2.m_ints, 0, m, m, ks);
			LongArray longArray3 = new LongArray(intLen);
			longArray3.m_ints[0] = 1L;
			LongArray longArray4 = new LongArray(intLen);
			int[] array = new int[]
			{
				num,
				m + 1
			};
			LongArray[] array2 = new LongArray[]
			{
				longArray,
				longArray2
			};
			int[] array3 = new int[2];
			array3[0] = 1;
			int[] array4 = array3;
			LongArray[] array5 = new LongArray[]
			{
				longArray3,
				longArray4
			};
			int num2 = 1;
			int num3 = array[num2];
			int num4 = array4[num2];
			int num5 = num3 - array[1 - num2];
			for (;;)
			{
				if (num5 < 0)
				{
					num5 = -num5;
					array[num2] = num3;
					array4[num2] = num4;
					num2 = 1 - num2;
					num3 = array[num2];
					num4 = array4[num2];
				}
				array2[num2].AddShiftedByBitsSafe(array2[1 - num2], array[1 - num2], num5);
				int num6 = array2[num2].DegreeFrom(num3);
				if (num6 == 0)
				{
					break;
				}
				int num7 = array4[1 - num2];
				array5[num2].AddShiftedByBitsSafe(array5[1 - num2], num7, num5);
				num7 += num5;
				if (num7 > num4)
				{
					num4 = num7;
				}
				else if (num7 == num4)
				{
					num4 = array5[num2].DegreeFrom(num4);
				}
				num5 += num6 - num3;
				num3 = num6;
			}
			return array5[1 - num2];
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x000E80E5 File Offset: 0x000E62E5
		public override bool Equals(object obj)
		{
			return this.Equals(obj as LongArray);
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x000E80F4 File Offset: 0x000E62F4
		public virtual bool Equals(LongArray other)
		{
			if (this == other)
			{
				return true;
			}
			if (other == null)
			{
				return false;
			}
			int usedLength = this.GetUsedLength();
			if (other.GetUsedLength() != usedLength)
			{
				return false;
			}
			for (int i = 0; i < usedLength; i++)
			{
				if (this.m_ints[i] != other.m_ints[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x000E8140 File Offset: 0x000E6340
		public override int GetHashCode()
		{
			int usedLength = this.GetUsedLength();
			int num = 1;
			for (int i = 0; i < usedLength; i++)
			{
				long num2 = this.m_ints[i];
				num *= 31;
				num ^= (int)num2;
				num *= 31;
				num ^= (int)((ulong)num2 >> 32);
			}
			return num;
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x000E8183 File Offset: 0x000E6383
		public LongArray Copy()
		{
			return new LongArray(Arrays.Clone(this.m_ints));
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x000E8198 File Offset: 0x000E6398
		public override string ToString()
		{
			int num = this.GetUsedLength();
			if (num == 0)
			{
				return "0";
			}
			StringBuilder stringBuilder = new StringBuilder(Convert.ToString(this.m_ints[--num], 2));
			while (--num >= 0)
			{
				string text = Convert.ToString(this.m_ints[num], 2);
				int length = text.Length;
				if (length < 64)
				{
					stringBuilder.Append("0000000000000000000000000000000000000000000000000000000000000000".Substring(length));
				}
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001979 RID: 6521
		private static readonly ushort[] INTERLEAVE2_TABLE = new ushort[]
		{
			0,
			1,
			4,
			5,
			16,
			17,
			20,
			21,
			64,
			65,
			68,
			69,
			80,
			81,
			84,
			85,
			256,
			257,
			260,
			261,
			272,
			273,
			276,
			277,
			320,
			321,
			324,
			325,
			336,
			337,
			340,
			341,
			1024,
			1025,
			1028,
			1029,
			1040,
			1041,
			1044,
			1045,
			1088,
			1089,
			1092,
			1093,
			1104,
			1105,
			1108,
			1109,
			1280,
			1281,
			1284,
			1285,
			1296,
			1297,
			1300,
			1301,
			1344,
			1345,
			1348,
			1349,
			1360,
			1361,
			1364,
			1365,
			4096,
			4097,
			4100,
			4101,
			4112,
			4113,
			4116,
			4117,
			4160,
			4161,
			4164,
			4165,
			4176,
			4177,
			4180,
			4181,
			4352,
			4353,
			4356,
			4357,
			4368,
			4369,
			4372,
			4373,
			4416,
			4417,
			4420,
			4421,
			4432,
			4433,
			4436,
			4437,
			5120,
			5121,
			5124,
			5125,
			5136,
			5137,
			5140,
			5141,
			5184,
			5185,
			5188,
			5189,
			5200,
			5201,
			5204,
			5205,
			5376,
			5377,
			5380,
			5381,
			5392,
			5393,
			5396,
			5397,
			5440,
			5441,
			5444,
			5445,
			5456,
			5457,
			5460,
			5461,
			16384,
			16385,
			16388,
			16389,
			16400,
			16401,
			16404,
			16405,
			16448,
			16449,
			16452,
			16453,
			16464,
			16465,
			16468,
			16469,
			16640,
			16641,
			16644,
			16645,
			16656,
			16657,
			16660,
			16661,
			16704,
			16705,
			16708,
			16709,
			16720,
			16721,
			16724,
			16725,
			17408,
			17409,
			17412,
			17413,
			17424,
			17425,
			17428,
			17429,
			17472,
			17473,
			17476,
			17477,
			17488,
			17489,
			17492,
			17493,
			17664,
			17665,
			17668,
			17669,
			17680,
			17681,
			17684,
			17685,
			17728,
			17729,
			17732,
			17733,
			17744,
			17745,
			17748,
			17749,
			20480,
			20481,
			20484,
			20485,
			20496,
			20497,
			20500,
			20501,
			20544,
			20545,
			20548,
			20549,
			20560,
			20561,
			20564,
			20565,
			20736,
			20737,
			20740,
			20741,
			20752,
			20753,
			20756,
			20757,
			20800,
			20801,
			20804,
			20805,
			20816,
			20817,
			20820,
			20821,
			21504,
			21505,
			21508,
			21509,
			21520,
			21521,
			21524,
			21525,
			21568,
			21569,
			21572,
			21573,
			21584,
			21585,
			21588,
			21589,
			21760,
			21761,
			21764,
			21765,
			21776,
			21777,
			21780,
			21781,
			21824,
			21825,
			21828,
			21829,
			21840,
			21841,
			21844,
			21845
		};

		// Token: 0x0400197A RID: 6522
		private static readonly int[] INTERLEAVE3_TABLE = new int[]
		{
			0,
			1,
			8,
			9,
			64,
			65,
			72,
			73,
			512,
			513,
			520,
			521,
			576,
			577,
			584,
			585,
			4096,
			4097,
			4104,
			4105,
			4160,
			4161,
			4168,
			4169,
			4608,
			4609,
			4616,
			4617,
			4672,
			4673,
			4680,
			4681,
			32768,
			32769,
			32776,
			32777,
			32832,
			32833,
			32840,
			32841,
			33280,
			33281,
			33288,
			33289,
			33344,
			33345,
			33352,
			33353,
			36864,
			36865,
			36872,
			36873,
			36928,
			36929,
			36936,
			36937,
			37376,
			37377,
			37384,
			37385,
			37440,
			37441,
			37448,
			37449,
			262144,
			262145,
			262152,
			262153,
			262208,
			262209,
			262216,
			262217,
			262656,
			262657,
			262664,
			262665,
			262720,
			262721,
			262728,
			262729,
			266240,
			266241,
			266248,
			266249,
			266304,
			266305,
			266312,
			266313,
			266752,
			266753,
			266760,
			266761,
			266816,
			266817,
			266824,
			266825,
			294912,
			294913,
			294920,
			294921,
			294976,
			294977,
			294984,
			294985,
			295424,
			295425,
			295432,
			295433,
			295488,
			295489,
			295496,
			295497,
			299008,
			299009,
			299016,
			299017,
			299072,
			299073,
			299080,
			299081,
			299520,
			299521,
			299528,
			299529,
			299584,
			299585,
			299592,
			299593
		};

		// Token: 0x0400197B RID: 6523
		private static readonly int[] INTERLEAVE4_TABLE = new int[]
		{
			0,
			1,
			16,
			17,
			256,
			257,
			272,
			273,
			4096,
			4097,
			4112,
			4113,
			4352,
			4353,
			4368,
			4369,
			65536,
			65537,
			65552,
			65553,
			65792,
			65793,
			65808,
			65809,
			69632,
			69633,
			69648,
			69649,
			69888,
			69889,
			69904,
			69905,
			1048576,
			1048577,
			1048592,
			1048593,
			1048832,
			1048833,
			1048848,
			1048849,
			1052672,
			1052673,
			1052688,
			1052689,
			1052928,
			1052929,
			1052944,
			1052945,
			1114112,
			1114113,
			1114128,
			1114129,
			1114368,
			1114369,
			1114384,
			1114385,
			1118208,
			1118209,
			1118224,
			1118225,
			1118464,
			1118465,
			1118480,
			1118481,
			16777216,
			16777217,
			16777232,
			16777233,
			16777472,
			16777473,
			16777488,
			16777489,
			16781312,
			16781313,
			16781328,
			16781329,
			16781568,
			16781569,
			16781584,
			16781585,
			16842752,
			16842753,
			16842768,
			16842769,
			16843008,
			16843009,
			16843024,
			16843025,
			16846848,
			16846849,
			16846864,
			16846865,
			16847104,
			16847105,
			16847120,
			16847121,
			17825792,
			17825793,
			17825808,
			17825809,
			17826048,
			17826049,
			17826064,
			17826065,
			17829888,
			17829889,
			17829904,
			17829905,
			17830144,
			17830145,
			17830160,
			17830161,
			17891328,
			17891329,
			17891344,
			17891345,
			17891584,
			17891585,
			17891600,
			17891601,
			17895424,
			17895425,
			17895440,
			17895441,
			17895680,
			17895681,
			17895696,
			17895697,
			268435456,
			268435457,
			268435472,
			268435473,
			268435712,
			268435713,
			268435728,
			268435729,
			268439552,
			268439553,
			268439568,
			268439569,
			268439808,
			268439809,
			268439824,
			268439825,
			268500992,
			268500993,
			268501008,
			268501009,
			268501248,
			268501249,
			268501264,
			268501265,
			268505088,
			268505089,
			268505104,
			268505105,
			268505344,
			268505345,
			268505360,
			268505361,
			269484032,
			269484033,
			269484048,
			269484049,
			269484288,
			269484289,
			269484304,
			269484305,
			269488128,
			269488129,
			269488144,
			269488145,
			269488384,
			269488385,
			269488400,
			269488401,
			269549568,
			269549569,
			269549584,
			269549585,
			269549824,
			269549825,
			269549840,
			269549841,
			269553664,
			269553665,
			269553680,
			269553681,
			269553920,
			269553921,
			269553936,
			269553937,
			285212672,
			285212673,
			285212688,
			285212689,
			285212928,
			285212929,
			285212944,
			285212945,
			285216768,
			285216769,
			285216784,
			285216785,
			285217024,
			285217025,
			285217040,
			285217041,
			285278208,
			285278209,
			285278224,
			285278225,
			285278464,
			285278465,
			285278480,
			285278481,
			285282304,
			285282305,
			285282320,
			285282321,
			285282560,
			285282561,
			285282576,
			285282577,
			286261248,
			286261249,
			286261264,
			286261265,
			286261504,
			286261505,
			286261520,
			286261521,
			286265344,
			286265345,
			286265360,
			286265361,
			286265600,
			286265601,
			286265616,
			286265617,
			286326784,
			286326785,
			286326800,
			286326801,
			286327040,
			286327041,
			286327056,
			286327057,
			286330880,
			286330881,
			286330896,
			286330897,
			286331136,
			286331137,
			286331152,
			286331153
		};

		// Token: 0x0400197C RID: 6524
		private static readonly int[] INTERLEAVE5_TABLE = new int[]
		{
			0,
			1,
			32,
			33,
			1024,
			1025,
			1056,
			1057,
			32768,
			32769,
			32800,
			32801,
			33792,
			33793,
			33824,
			33825,
			1048576,
			1048577,
			1048608,
			1048609,
			1049600,
			1049601,
			1049632,
			1049633,
			1081344,
			1081345,
			1081376,
			1081377,
			1082368,
			1082369,
			1082400,
			1082401,
			33554432,
			33554433,
			33554464,
			33554465,
			33555456,
			33555457,
			33555488,
			33555489,
			33587200,
			33587201,
			33587232,
			33587233,
			33588224,
			33588225,
			33588256,
			33588257,
			34603008,
			34603009,
			34603040,
			34603041,
			34604032,
			34604033,
			34604064,
			34604065,
			34635776,
			34635777,
			34635808,
			34635809,
			34636800,
			34636801,
			34636832,
			34636833,
			1073741824,
			1073741825,
			1073741856,
			1073741857,
			1073742848,
			1073742849,
			1073742880,
			1073742881,
			1073774592,
			1073774593,
			1073774624,
			1073774625,
			1073775616,
			1073775617,
			1073775648,
			1073775649,
			1074790400,
			1074790401,
			1074790432,
			1074790433,
			1074791424,
			1074791425,
			1074791456,
			1074791457,
			1074823168,
			1074823169,
			1074823200,
			1074823201,
			1074824192,
			1074824193,
			1074824224,
			1074824225,
			1107296256,
			1107296257,
			1107296288,
			1107296289,
			1107297280,
			1107297281,
			1107297312,
			1107297313,
			1107329024,
			1107329025,
			1107329056,
			1107329057,
			1107330048,
			1107330049,
			1107330080,
			1107330081,
			1108344832,
			1108344833,
			1108344864,
			1108344865,
			1108345856,
			1108345857,
			1108345888,
			1108345889,
			1108377600,
			1108377601,
			1108377632,
			1108377633,
			1108378624,
			1108378625,
			1108378656,
			1108378657
		};

		// Token: 0x0400197D RID: 6525
		private static readonly long[] INTERLEAVE7_TABLE = new long[]
		{
			0L,
			1L,
			128L,
			129L,
			16384L,
			16385L,
			16512L,
			16513L,
			2097152L,
			2097153L,
			2097280L,
			2097281L,
			2113536L,
			2113537L,
			2113664L,
			2113665L,
			268435456L,
			268435457L,
			268435584L,
			268435585L,
			268451840L,
			268451841L,
			268451968L,
			268451969L,
			270532608L,
			270532609L,
			270532736L,
			270532737L,
			270548992L,
			270548993L,
			270549120L,
			270549121L,
			34359738368L,
			34359738369L,
			34359738496L,
			34359738497L,
			34359754752L,
			34359754753L,
			34359754880L,
			34359754881L,
			34361835520L,
			34361835521L,
			34361835648L,
			34361835649L,
			34361851904L,
			34361851905L,
			34361852032L,
			34361852033L,
			34628173824L,
			34628173825L,
			34628173952L,
			34628173953L,
			34628190208L,
			34628190209L,
			34628190336L,
			34628190337L,
			34630270976L,
			34630270977L,
			34630271104L,
			34630271105L,
			34630287360L,
			34630287361L,
			34630287488L,
			34630287489L,
			4398046511104L,
			4398046511105L,
			4398046511232L,
			4398046511233L,
			4398046527488L,
			4398046527489L,
			4398046527616L,
			4398046527617L,
			4398048608256L,
			4398048608257L,
			4398048608384L,
			4398048608385L,
			4398048624640L,
			4398048624641L,
			4398048624768L,
			4398048624769L,
			4398314946560L,
			4398314946561L,
			4398314946688L,
			4398314946689L,
			4398314962944L,
			4398314962945L,
			4398314963072L,
			4398314963073L,
			4398317043712L,
			4398317043713L,
			4398317043840L,
			4398317043841L,
			4398317060096L,
			4398317060097L,
			4398317060224L,
			4398317060225L,
			4432406249472L,
			4432406249473L,
			4432406249600L,
			4432406249601L,
			4432406265856L,
			4432406265857L,
			4432406265984L,
			4432406265985L,
			4432408346624L,
			4432408346625L,
			4432408346752L,
			4432408346753L,
			4432408363008L,
			4432408363009L,
			4432408363136L,
			4432408363137L,
			4432674684928L,
			4432674684929L,
			4432674685056L,
			4432674685057L,
			4432674701312L,
			4432674701313L,
			4432674701440L,
			4432674701441L,
			4432676782080L,
			4432676782081L,
			4432676782208L,
			4432676782209L,
			4432676798464L,
			4432676798465L,
			4432676798592L,
			4432676798593L,
			562949953421312L,
			562949953421313L,
			562949953421440L,
			562949953421441L,
			562949953437696L,
			562949953437697L,
			562949953437824L,
			562949953437825L,
			562949955518464L,
			562949955518465L,
			562949955518592L,
			562949955518593L,
			562949955534848L,
			562949955534849L,
			562949955534976L,
			562949955534977L,
			562950221856768L,
			562950221856769L,
			562950221856896L,
			562950221856897L,
			562950221873152L,
			562950221873153L,
			562950221873280L,
			562950221873281L,
			562950223953920L,
			562950223953921L,
			562950223954048L,
			562950223954049L,
			562950223970304L,
			562950223970305L,
			562950223970432L,
			562950223970433L,
			562984313159680L,
			562984313159681L,
			562984313159808L,
			562984313159809L,
			562984313176064L,
			562984313176065L,
			562984313176192L,
			562984313176193L,
			562984315256832L,
			562984315256833L,
			562984315256960L,
			562984315256961L,
			562984315273216L,
			562984315273217L,
			562984315273344L,
			562984315273345L,
			562984581595136L,
			562984581595137L,
			562984581595264L,
			562984581595265L,
			562984581611520L,
			562984581611521L,
			562984581611648L,
			562984581611649L,
			562984583692288L,
			562984583692289L,
			562984583692416L,
			562984583692417L,
			562984583708672L,
			562984583708673L,
			562984583708800L,
			562984583708801L,
			567347999932416L,
			567347999932417L,
			567347999932544L,
			567347999932545L,
			567347999948800L,
			567347999948801L,
			567347999948928L,
			567347999948929L,
			567348002029568L,
			567348002029569L,
			567348002029696L,
			567348002029697L,
			567348002045952L,
			567348002045953L,
			567348002046080L,
			567348002046081L,
			567348268367872L,
			567348268367873L,
			567348268368000L,
			567348268368001L,
			567348268384256L,
			567348268384257L,
			567348268384384L,
			567348268384385L,
			567348270465024L,
			567348270465025L,
			567348270465152L,
			567348270465153L,
			567348270481408L,
			567348270481409L,
			567348270481536L,
			567348270481537L,
			567382359670784L,
			567382359670785L,
			567382359670912L,
			567382359670913L,
			567382359687168L,
			567382359687169L,
			567382359687296L,
			567382359687297L,
			567382361767936L,
			567382361767937L,
			567382361768064L,
			567382361768065L,
			567382361784320L,
			567382361784321L,
			567382361784448L,
			567382361784449L,
			567382628106240L,
			567382628106241L,
			567382628106368L,
			567382628106369L,
			567382628122624L,
			567382628122625L,
			567382628122752L,
			567382628122753L,
			567382630203392L,
			567382630203393L,
			567382630203520L,
			567382630203521L,
			567382630219776L,
			567382630219777L,
			567382630219904L,
			567382630219905L,
			72057594037927936L,
			72057594037927937L,
			72057594037928064L,
			72057594037928065L,
			72057594037944320L,
			72057594037944321L,
			72057594037944448L,
			72057594037944449L,
			72057594040025088L,
			72057594040025089L,
			72057594040025216L,
			72057594040025217L,
			72057594040041472L,
			72057594040041473L,
			72057594040041600L,
			72057594040041601L,
			72057594306363392L,
			72057594306363393L,
			72057594306363520L,
			72057594306363521L,
			72057594306379776L,
			72057594306379777L,
			72057594306379904L,
			72057594306379905L,
			72057594308460544L,
			72057594308460545L,
			72057594308460672L,
			72057594308460673L,
			72057594308476928L,
			72057594308476929L,
			72057594308477056L,
			72057594308477057L,
			72057628397666304L,
			72057628397666305L,
			72057628397666432L,
			72057628397666433L,
			72057628397682688L,
			72057628397682689L,
			72057628397682816L,
			72057628397682817L,
			72057628399763456L,
			72057628399763457L,
			72057628399763584L,
			72057628399763585L,
			72057628399779840L,
			72057628399779841L,
			72057628399779968L,
			72057628399779969L,
			72057628666101760L,
			72057628666101761L,
			72057628666101888L,
			72057628666101889L,
			72057628666118144L,
			72057628666118145L,
			72057628666118272L,
			72057628666118273L,
			72057628668198912L,
			72057628668198913L,
			72057628668199040L,
			72057628668199041L,
			72057628668215296L,
			72057628668215297L,
			72057628668215424L,
			72057628668215425L,
			72061992084439040L,
			72061992084439041L,
			72061992084439168L,
			72061992084439169L,
			72061992084455424L,
			72061992084455425L,
			72061992084455552L,
			72061992084455553L,
			72061992086536192L,
			72061992086536193L,
			72061992086536320L,
			72061992086536321L,
			72061992086552576L,
			72061992086552577L,
			72061992086552704L,
			72061992086552705L,
			72061992352874496L,
			72061992352874497L,
			72061992352874624L,
			72061992352874625L,
			72061992352890880L,
			72061992352890881L,
			72061992352891008L,
			72061992352891009L,
			72061992354971648L,
			72061992354971649L,
			72061992354971776L,
			72061992354971777L,
			72061992354988032L,
			72061992354988033L,
			72061992354988160L,
			72061992354988161L,
			72062026444177408L,
			72062026444177409L,
			72062026444177536L,
			72062026444177537L,
			72062026444193792L,
			72062026444193793L,
			72062026444193920L,
			72062026444193921L,
			72062026446274560L,
			72062026446274561L,
			72062026446274688L,
			72062026446274689L,
			72062026446290944L,
			72062026446290945L,
			72062026446291072L,
			72062026446291073L,
			72062026712612864L,
			72062026712612865L,
			72062026712612992L,
			72062026712612993L,
			72062026712629248L,
			72062026712629249L,
			72062026712629376L,
			72062026712629377L,
			72062026714710016L,
			72062026714710017L,
			72062026714710144L,
			72062026714710145L,
			72062026714726400L,
			72062026714726401L,
			72062026714726528L,
			72062026714726529L,
			72620543991349248L,
			72620543991349249L,
			72620543991349376L,
			72620543991349377L,
			72620543991365632L,
			72620543991365633L,
			72620543991365760L,
			72620543991365761L,
			72620543993446400L,
			72620543993446401L,
			72620543993446528L,
			72620543993446529L,
			72620543993462784L,
			72620543993462785L,
			72620543993462912L,
			72620543993462913L,
			72620544259784704L,
			72620544259784705L,
			72620544259784832L,
			72620544259784833L,
			72620544259801088L,
			72620544259801089L,
			72620544259801216L,
			72620544259801217L,
			72620544261881856L,
			72620544261881857L,
			72620544261881984L,
			72620544261881985L,
			72620544261898240L,
			72620544261898241L,
			72620544261898368L,
			72620544261898369L,
			72620578351087616L,
			72620578351087617L,
			72620578351087744L,
			72620578351087745L,
			72620578351104000L,
			72620578351104001L,
			72620578351104128L,
			72620578351104129L,
			72620578353184768L,
			72620578353184769L,
			72620578353184896L,
			72620578353184897L,
			72620578353201152L,
			72620578353201153L,
			72620578353201280L,
			72620578353201281L,
			72620578619523072L,
			72620578619523073L,
			72620578619523200L,
			72620578619523201L,
			72620578619539456L,
			72620578619539457L,
			72620578619539584L,
			72620578619539585L,
			72620578621620224L,
			72620578621620225L,
			72620578621620352L,
			72620578621620353L,
			72620578621636608L,
			72620578621636609L,
			72620578621636736L,
			72620578621636737L,
			72624942037860352L,
			72624942037860353L,
			72624942037860480L,
			72624942037860481L,
			72624942037876736L,
			72624942037876737L,
			72624942037876864L,
			72624942037876865L,
			72624942039957504L,
			72624942039957505L,
			72624942039957632L,
			72624942039957633L,
			72624942039973888L,
			72624942039973889L,
			72624942039974016L,
			72624942039974017L,
			72624942306295808L,
			72624942306295809L,
			72624942306295936L,
			72624942306295937L,
			72624942306312192L,
			72624942306312193L,
			72624942306312320L,
			72624942306312321L,
			72624942308392960L,
			72624942308392961L,
			72624942308393088L,
			72624942308393089L,
			72624942308409344L,
			72624942308409345L,
			72624942308409472L,
			72624942308409473L,
			72624976397598720L,
			72624976397598721L,
			72624976397598848L,
			72624976397598849L,
			72624976397615104L,
			72624976397615105L,
			72624976397615232L,
			72624976397615233L,
			72624976399695872L,
			72624976399695873L,
			72624976399696000L,
			72624976399696001L,
			72624976399712256L,
			72624976399712257L,
			72624976399712384L,
			72624976399712385L,
			72624976666034176L,
			72624976666034177L,
			72624976666034304L,
			72624976666034305L,
			72624976666050560L,
			72624976666050561L,
			72624976666050688L,
			72624976666050689L,
			72624976668131328L,
			72624976668131329L,
			72624976668131456L,
			72624976668131457L,
			72624976668147712L,
			72624976668147713L,
			72624976668147840L,
			72624976668147841L
		};

		// Token: 0x0400197E RID: 6526
		private const string ZEROES = "0000000000000000000000000000000000000000000000000000000000000000";

		// Token: 0x0400197F RID: 6527
		internal static readonly byte[] BitLengths = new byte[]
		{
			0,
			1,
			2,
			2,
			3,
			3,
			3,
			3,
			4,
			4,
			4,
			4,
			4,
			4,
			4,
			4,
			5,
			5,
			5,
			5,
			5,
			5,
			5,
			5,
			5,
			5,
			5,
			5,
			5,
			5,
			5,
			5,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			6,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			7,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8,
			8
		};

		// Token: 0x04001980 RID: 6528
		private long[] m_ints;
	}
}
