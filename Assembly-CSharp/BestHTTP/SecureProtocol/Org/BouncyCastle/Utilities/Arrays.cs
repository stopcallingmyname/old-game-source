using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities
{
	// Token: 0x02000260 RID: 608
	public abstract class Arrays
	{
		// Token: 0x06001622 RID: 5666 RVA: 0x000AFFE4 File Offset: 0x000AE1E4
		public static bool AreAllZeroes(byte[] buf, int off, int len)
		{
			uint num = 0U;
			for (int i = 0; i < len; i++)
			{
				num |= (uint)buf[off + i];
			}
			return num == 0U;
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x000B000B File Offset: 0x000AE20B
		public static bool AreEqual(bool[] a, bool[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x000B0022 File Offset: 0x000AE222
		public static bool AreEqual(char[] a, char[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x000B0039 File Offset: 0x000AE239
		public static bool AreEqual(byte[] a, byte[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x000B0050 File Offset: 0x000AE250
		[Obsolete("Use 'AreEqual' method instead")]
		public static bool AreSame(byte[] a, byte[] b)
		{
			return Arrays.AreEqual(a, b);
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x000B005C File Offset: 0x000AE25C
		public static bool ConstantTimeAreEqual(byte[] a, byte[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			int num2 = 0;
			while (num != 0)
			{
				num--;
				num2 |= (int)(a[num] ^ b[num]);
			}
			return num2 == 0;
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x000B008E File Offset: 0x000AE28E
		public static bool AreEqual(int[] a, int[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x000B00A5 File Offset: 0x000AE2A5
		[CLSCompliant(false)]
		public static bool AreEqual(uint[] a, uint[] b)
		{
			return a == b || (a != null && b != null && Arrays.HaveSameContents(a, b));
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x000B00BC File Offset: 0x000AE2BC
		private static bool HaveSameContents(bool[] a, bool[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x000B00E8 File Offset: 0x000AE2E8
		private static bool HaveSameContents(char[] a, char[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x000B0114 File Offset: 0x000AE314
		private static bool HaveSameContents(byte[] a, byte[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x000B0140 File Offset: 0x000AE340
		private static bool HaveSameContents(int[] a, int[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x000B016C File Offset: 0x000AE36C
		private static bool HaveSameContents(uint[] a, uint[] b)
		{
			int num = a.Length;
			if (num != b.Length)
			{
				return false;
			}
			while (num != 0)
			{
				num--;
				if (a[num] != b[num])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x000B0198 File Offset: 0x000AE398
		public static string ToString(object[] a)
		{
			StringBuilder stringBuilder = new StringBuilder(91);
			if (a.Length != 0)
			{
				stringBuilder.Append(a[0]);
				for (int i = 1; i < a.Length; i++)
				{
					stringBuilder.Append(", ").Append(a[i]);
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x000B01EC File Offset: 0x000AE3EC
		public static int GetHashCode(byte[] data)
		{
			if (data == null)
			{
				return 0;
			}
			int num = data.Length;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= (int)data[num];
			}
			return num2;
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x000B0220 File Offset: 0x000AE420
		public static int GetHashCode(byte[] data, int off, int len)
		{
			if (data == null)
			{
				return 0;
			}
			int num = len;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= (int)data[off + num];
			}
			return num2;
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x000B0254 File Offset: 0x000AE454
		public static int GetHashCode(int[] data)
		{
			if (data == null)
			{
				return 0;
			}
			int num = data.Length;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= data[num];
			}
			return num2;
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x000B0288 File Offset: 0x000AE488
		public static int GetHashCode(int[] data, int off, int len)
		{
			if (data == null)
			{
				return 0;
			}
			int num = len;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= data[off + num];
			}
			return num2;
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x000B02BC File Offset: 0x000AE4BC
		[CLSCompliant(false)]
		public static int GetHashCode(uint[] data)
		{
			if (data == null)
			{
				return 0;
			}
			int num = data.Length;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= (int)data[num];
			}
			return num2;
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x000B02F0 File Offset: 0x000AE4F0
		[CLSCompliant(false)]
		public static int GetHashCode(uint[] data, int off, int len)
		{
			if (data == null)
			{
				return 0;
			}
			int num = len;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= (int)data[off + num];
			}
			return num2;
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x000B0324 File Offset: 0x000AE524
		[CLSCompliant(false)]
		public static int GetHashCode(ulong[] data)
		{
			if (data == null)
			{
				return 0;
			}
			int num = data.Length;
			int num2 = num + 1;
			while (--num >= 0)
			{
				ulong num3 = data[num];
				num2 *= 257;
				num2 ^= (int)num3;
				num2 *= 257;
				num2 ^= (int)(num3 >> 32);
			}
			return num2;
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x000B036C File Offset: 0x000AE56C
		[CLSCompliant(false)]
		public static int GetHashCode(ulong[] data, int off, int len)
		{
			if (data == null)
			{
				return 0;
			}
			int num = len;
			int num2 = num + 1;
			while (--num >= 0)
			{
				ulong num3 = data[off + num];
				num2 *= 257;
				num2 ^= (int)num3;
				num2 *= 257;
				num2 ^= (int)(num3 >> 32);
			}
			return num2;
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x000B03B2 File Offset: 0x000AE5B2
		public static byte[] Clone(byte[] data)
		{
			if (data != null)
			{
				return (byte[])data.Clone();
			}
			return null;
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x000B03C4 File Offset: 0x000AE5C4
		public static byte[] Clone(byte[] data, byte[] existing)
		{
			if (data == null)
			{
				return null;
			}
			if (existing == null || existing.Length != data.Length)
			{
				return Arrays.Clone(data);
			}
			Array.Copy(data, 0, existing, 0, existing.Length);
			return existing;
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x000B03EA File Offset: 0x000AE5EA
		public static int[] Clone(int[] data)
		{
			if (data != null)
			{
				return (int[])data.Clone();
			}
			return null;
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x000B03FC File Offset: 0x000AE5FC
		internal static uint[] Clone(uint[] data)
		{
			if (data != null)
			{
				return (uint[])data.Clone();
			}
			return null;
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x000B040E File Offset: 0x000AE60E
		public static long[] Clone(long[] data)
		{
			if (data != null)
			{
				return (long[])data.Clone();
			}
			return null;
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x000B0420 File Offset: 0x000AE620
		[CLSCompliant(false)]
		public static ulong[] Clone(ulong[] data)
		{
			if (data != null)
			{
				return (ulong[])data.Clone();
			}
			return null;
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x000B0432 File Offset: 0x000AE632
		[CLSCompliant(false)]
		public static ulong[] Clone(ulong[] data, ulong[] existing)
		{
			if (data == null)
			{
				return null;
			}
			if (existing == null || existing.Length != data.Length)
			{
				return Arrays.Clone(data);
			}
			Array.Copy(data, 0, existing, 0, existing.Length);
			return existing;
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x000B0458 File Offset: 0x000AE658
		public static bool Contains(byte[] a, byte n)
		{
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] == n)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x000B047C File Offset: 0x000AE67C
		public static bool Contains(short[] a, short n)
		{
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] == n)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x000B04A0 File Offset: 0x000AE6A0
		public static bool Contains(int[] a, int n)
		{
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] == n)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x000B04C4 File Offset: 0x000AE6C4
		public static void Fill(byte[] buf, byte b)
		{
			int i = buf.Length;
			while (i > 0)
			{
				buf[--i] = b;
			}
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x000B04E4 File Offset: 0x000AE6E4
		public static void Fill(byte[] buf, int from, int to, byte b)
		{
			for (int i = from; i < to; i++)
			{
				buf[i] = b;
			}
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x000B0504 File Offset: 0x000AE704
		public static byte[] CopyOf(byte[] data, int newLength)
		{
			byte[] array = new byte[newLength];
			Array.Copy(data, 0, array, 0, Math.Min(newLength, data.Length));
			return array;
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x000B052C File Offset: 0x000AE72C
		public static char[] CopyOf(char[] data, int newLength)
		{
			char[] array = new char[newLength];
			Array.Copy(data, 0, array, 0, Math.Min(newLength, data.Length));
			return array;
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x000B0554 File Offset: 0x000AE754
		public static int[] CopyOf(int[] data, int newLength)
		{
			int[] array = new int[newLength];
			Array.Copy(data, 0, array, 0, Math.Min(newLength, data.Length));
			return array;
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x000B057C File Offset: 0x000AE77C
		public static long[] CopyOf(long[] data, int newLength)
		{
			long[] array = new long[newLength];
			Array.Copy(data, 0, array, 0, Math.Min(newLength, data.Length));
			return array;
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x000B05A4 File Offset: 0x000AE7A4
		public static BigInteger[] CopyOf(BigInteger[] data, int newLength)
		{
			BigInteger[] array = new BigInteger[newLength];
			Array.Copy(data, 0, array, 0, Math.Min(newLength, data.Length));
			return array;
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x000B05CC File Offset: 0x000AE7CC
		public static byte[] CopyOfRange(byte[] data, int from, int to)
		{
			int length = Arrays.GetLength(from, to);
			byte[] array = new byte[length];
			Array.Copy(data, from, array, 0, Math.Min(length, data.Length - from));
			return array;
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x000B0600 File Offset: 0x000AE800
		public static int[] CopyOfRange(int[] data, int from, int to)
		{
			int length = Arrays.GetLength(from, to);
			int[] array = new int[length];
			Array.Copy(data, from, array, 0, Math.Min(length, data.Length - from));
			return array;
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x000B0634 File Offset: 0x000AE834
		public static long[] CopyOfRange(long[] data, int from, int to)
		{
			int length = Arrays.GetLength(from, to);
			long[] array = new long[length];
			Array.Copy(data, from, array, 0, Math.Min(length, data.Length - from));
			return array;
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x000B0668 File Offset: 0x000AE868
		public static BigInteger[] CopyOfRange(BigInteger[] data, int from, int to)
		{
			int length = Arrays.GetLength(from, to);
			BigInteger[] array = new BigInteger[length];
			Array.Copy(data, from, array, 0, Math.Min(length, data.Length - from));
			return array;
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x000B0699 File Offset: 0x000AE899
		private static int GetLength(int from, int to)
		{
			int num = to - from;
			if (num < 0)
			{
				throw new ArgumentException(from + " > " + to);
			}
			return num;
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x000B06C0 File Offset: 0x000AE8C0
		public static byte[] Append(byte[] a, byte b)
		{
			if (a == null)
			{
				return new byte[]
				{
					b
				};
			}
			int num = a.Length;
			byte[] array = new byte[num + 1];
			Array.Copy(a, 0, array, 0, num);
			array[num] = b;
			return array;
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x000B06F8 File Offset: 0x000AE8F8
		public static short[] Append(short[] a, short b)
		{
			if (a == null)
			{
				return new short[]
				{
					b
				};
			}
			int num = a.Length;
			short[] array = new short[num + 1];
			Array.Copy(a, 0, array, 0, num);
			array[num] = b;
			return array;
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x000B0730 File Offset: 0x000AE930
		public static int[] Append(int[] a, int b)
		{
			if (a == null)
			{
				return new int[]
				{
					b
				};
			}
			int num = a.Length;
			int[] array = new int[num + 1];
			Array.Copy(a, 0, array, 0, num);
			array[num] = b;
			return array;
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x000B0768 File Offset: 0x000AE968
		public static byte[] Concatenate(byte[] a, byte[] b)
		{
			if (a == null)
			{
				return Arrays.Clone(b);
			}
			if (b == null)
			{
				return Arrays.Clone(a);
			}
			byte[] array = new byte[a.Length + b.Length];
			Array.Copy(a, 0, array, 0, a.Length);
			Array.Copy(b, 0, array, a.Length, b.Length);
			return array;
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x000B07B4 File Offset: 0x000AE9B4
		public static byte[] ConcatenateAll(params byte[][] vs)
		{
			byte[][] array = new byte[vs.Length][];
			int num = 0;
			int num2 = 0;
			foreach (byte[] array2 in vs)
			{
				if (array2 != null)
				{
					array[num++] = array2;
					num2 += array2.Length;
				}
			}
			byte[] array3 = new byte[num2];
			int num3 = 0;
			for (int j = 0; j < num; j++)
			{
				byte[] array4 = array[j];
				Array.Copy(array4, 0, array3, num3, array4.Length);
				num3 += array4.Length;
			}
			return array3;
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x000B0834 File Offset: 0x000AEA34
		public static int[] Concatenate(int[] a, int[] b)
		{
			if (a == null)
			{
				return Arrays.Clone(b);
			}
			if (b == null)
			{
				return Arrays.Clone(a);
			}
			int[] array = new int[a.Length + b.Length];
			Array.Copy(a, 0, array, 0, a.Length);
			Array.Copy(b, 0, array, a.Length, b.Length);
			return array;
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x000B0880 File Offset: 0x000AEA80
		public static byte[] Prepend(byte[] a, byte b)
		{
			if (a == null)
			{
				return new byte[]
				{
					b
				};
			}
			int num = a.Length;
			byte[] array = new byte[num + 1];
			Array.Copy(a, 0, array, 1, num);
			array[0] = b;
			return array;
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x000B08B8 File Offset: 0x000AEAB8
		public static short[] Prepend(short[] a, short b)
		{
			if (a == null)
			{
				return new short[]
				{
					b
				};
			}
			int num = a.Length;
			short[] array = new short[num + 1];
			Array.Copy(a, 0, array, 1, num);
			array[0] = b;
			return array;
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x000B08F0 File Offset: 0x000AEAF0
		public static int[] Prepend(int[] a, int b)
		{
			if (a == null)
			{
				return new int[]
				{
					b
				};
			}
			int num = a.Length;
			int[] array = new int[num + 1];
			Array.Copy(a, 0, array, 1, num);
			array[0] = b;
			return array;
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x000B0928 File Offset: 0x000AEB28
		public static byte[] Reverse(byte[] a)
		{
			if (a == null)
			{
				return null;
			}
			int num = 0;
			int num2 = a.Length;
			byte[] array = new byte[num2];
			while (--num2 >= 0)
			{
				array[num2] = a[num++];
			}
			return array;
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x000B095C File Offset: 0x000AEB5C
		public static int[] Reverse(int[] a)
		{
			if (a == null)
			{
				return null;
			}
			int num = 0;
			int num2 = a.Length;
			int[] array = new int[num2];
			while (--num2 >= 0)
			{
				array[num2] = a[num++];
			}
			return array;
		}

		// Token: 0x04001683 RID: 5763
		public static readonly byte[] EmptyBytes = new byte[0];

		// Token: 0x04001684 RID: 5764
		public static readonly int[] EmptyInts = new int[0];
	}
}
