﻿using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x02000276 RID: 630
	internal sealed class ZTree
	{
		// Token: 0x06001744 RID: 5956 RVA: 0x000B74FD File Offset: 0x000B56FD
		internal static int d_code(int dist)
		{
			if (dist >= 256)
			{
				return (int)ZTree._dist_code[256 + (dist >> 7)];
			}
			return (int)ZTree._dist_code[dist];
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x000B7520 File Offset: 0x000B5720
		internal void gen_bitlen(Deflate s)
		{
			short[] array = this.dyn_tree;
			short[] static_tree = this.stat_desc.static_tree;
			int[] extra_bits = this.stat_desc.extra_bits;
			int extra_base = this.stat_desc.extra_base;
			int max_length = this.stat_desc.max_length;
			int num = 0;
			for (int i = 0; i <= 15; i++)
			{
				s.bl_count[i] = 0;
			}
			array[s.heap[s.heap_max] * 2 + 1] = 0;
			int j;
			for (j = s.heap_max + 1; j < 573; j++)
			{
				int num2 = s.heap[j];
				int i = (int)(array[(int)(array[num2 * 2 + 1] * 2 + 1)] + 1);
				if (i > max_length)
				{
					i = max_length;
					num++;
				}
				array[num2 * 2 + 1] = (short)i;
				if (num2 <= this.max_code)
				{
					short[] bl_count = s.bl_count;
					int num3 = i;
					bl_count[num3] += 1;
					int num4 = 0;
					if (num2 >= extra_base)
					{
						num4 = extra_bits[num2 - extra_base];
					}
					short num5 = array[num2 * 2];
					s.opt_len += (int)num5 * (i + num4);
					if (static_tree != null)
					{
						s.static_len += (int)num5 * ((int)static_tree[num2 * 2 + 1] + num4);
					}
				}
			}
			if (num == 0)
			{
				return;
			}
			do
			{
				int i = max_length - 1;
				while (s.bl_count[i] == 0)
				{
					i--;
				}
				short[] bl_count2 = s.bl_count;
				int num6 = i;
				bl_count2[num6] -= 1;
				short[] bl_count3 = s.bl_count;
				int num7 = i + 1;
				bl_count3[num7] += 2;
				short[] bl_count4 = s.bl_count;
				int num8 = max_length;
				bl_count4[num8] -= 1;
				num -= 2;
			}
			while (num > 0);
			for (int i = max_length; i != 0; i--)
			{
				int num2 = (int)s.bl_count[i];
				while (num2 != 0)
				{
					int num9 = s.heap[--j];
					if (num9 <= this.max_code)
					{
						if ((int)array[num9 * 2 + 1] != i)
						{
							s.opt_len += (int)(((long)i - (long)array[num9 * 2 + 1]) * (long)array[num9 * 2]);
							array[num9 * 2 + 1] = (short)i;
						}
						num2--;
					}
				}
			}
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x000B7738 File Offset: 0x000B5938
		internal void build_tree(Deflate s)
		{
			short[] array = this.dyn_tree;
			short[] static_tree = this.stat_desc.static_tree;
			int elems = this.stat_desc.elems;
			int num = -1;
			s.heap_len = 0;
			s.heap_max = 573;
			int num2;
			for (int i = 0; i < elems; i++)
			{
				if (array[i * 2] != 0)
				{
					int[] heap = s.heap;
					num2 = s.heap_len + 1;
					s.heap_len = num2;
					num = (heap[num2] = i);
					s.depth[i] = 0;
				}
				else
				{
					array[i * 2 + 1] = 0;
				}
			}
			int num3;
			while (s.heap_len < 2)
			{
				int[] heap2 = s.heap;
				num2 = s.heap_len + 1;
				s.heap_len = num2;
				num3 = (heap2[num2] = ((num < 2) ? (++num) : 0));
				array[num3 * 2] = 1;
				s.depth[num3] = 0;
				s.opt_len--;
				if (static_tree != null)
				{
					s.static_len -= (int)static_tree[num3 * 2 + 1];
				}
			}
			this.max_code = num;
			for (int i = s.heap_len / 2; i >= 1; i--)
			{
				s.pqdownheap(array, i);
			}
			num3 = elems;
			do
			{
				int i = s.heap[1];
				int[] heap3 = s.heap;
				int num4 = 1;
				int[] heap4 = s.heap;
				num2 = s.heap_len;
				s.heap_len = num2 - 1;
				heap3[num4] = heap4[num2];
				s.pqdownheap(array, 1);
				int num5 = s.heap[1];
				int[] heap5 = s.heap;
				num2 = s.heap_max - 1;
				s.heap_max = num2;
				heap5[num2] = i;
				int[] heap6 = s.heap;
				num2 = s.heap_max - 1;
				s.heap_max = num2;
				heap6[num2] = num5;
				array[num3 * 2] = array[i * 2] + array[num5 * 2];
				s.depth[num3] = Math.Max(s.depth[i], s.depth[num5]) + 1;
				array[i * 2 + 1] = (array[num5 * 2 + 1] = (short)num3);
				s.heap[1] = num3++;
				s.pqdownheap(array, 1);
			}
			while (s.heap_len >= 2);
			int[] heap7 = s.heap;
			num2 = s.heap_max - 1;
			s.heap_max = num2;
			heap7[num2] = s.heap[1];
			this.gen_bitlen(s);
			ZTree.gen_codes(array, num, s.bl_count);
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x000B7978 File Offset: 0x000B5B78
		internal static void gen_codes(short[] tree, int max_code, short[] bl_count)
		{
			short[] array = new short[16];
			short num = 0;
			for (int i = 1; i <= 15; i++)
			{
				num = (array[i] = (short)(num + bl_count[i - 1] << 1));
			}
			for (int j = 0; j <= max_code; j++)
			{
				int num2 = (int)tree[j * 2 + 1];
				if (num2 != 0)
				{
					int num3 = j * 2;
					short[] array2 = array;
					int num4 = num2;
					short num5 = array2[num4];
					array2[num4] = num5 + 1;
					tree[num3] = (short)ZTree.bi_reverse((int)num5, num2);
				}
			}
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x000B79E8 File Offset: 0x000B5BE8
		internal static int bi_reverse(int code, int len)
		{
			int num = 0;
			do
			{
				num |= (code & 1);
				code >>= 1;
				num <<= 1;
			}
			while (--len > 0);
			return num >> 1;
		}

		// Token: 0x040017EC RID: 6124
		private const int MAX_BITS = 15;

		// Token: 0x040017ED RID: 6125
		private const int BL_CODES = 19;

		// Token: 0x040017EE RID: 6126
		private const int D_CODES = 30;

		// Token: 0x040017EF RID: 6127
		private const int LITERALS = 256;

		// Token: 0x040017F0 RID: 6128
		private const int LENGTH_CODES = 29;

		// Token: 0x040017F1 RID: 6129
		private const int L_CODES = 286;

		// Token: 0x040017F2 RID: 6130
		private const int HEAP_SIZE = 573;

		// Token: 0x040017F3 RID: 6131
		internal const int MAX_BL_BITS = 7;

		// Token: 0x040017F4 RID: 6132
		internal const int END_BLOCK = 256;

		// Token: 0x040017F5 RID: 6133
		internal const int REP_3_6 = 16;

		// Token: 0x040017F6 RID: 6134
		internal const int REPZ_3_10 = 17;

		// Token: 0x040017F7 RID: 6135
		internal const int REPZ_11_138 = 18;

		// Token: 0x040017F8 RID: 6136
		internal static readonly int[] extra_lbits = new int[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			1,
			1,
			1,
			1,
			2,
			2,
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
			5,
			5,
			5,
			5,
			0
		};

		// Token: 0x040017F9 RID: 6137
		internal static readonly int[] extra_dbits = new int[]
		{
			0,
			0,
			0,
			0,
			1,
			1,
			2,
			2,
			3,
			3,
			4,
			4,
			5,
			5,
			6,
			6,
			7,
			7,
			8,
			8,
			9,
			9,
			10,
			10,
			11,
			11,
			12,
			12,
			13,
			13
		};

		// Token: 0x040017FA RID: 6138
		internal static readonly int[] extra_blbits = new int[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			2,
			3,
			7
		};

		// Token: 0x040017FB RID: 6139
		internal static readonly byte[] bl_order = new byte[]
		{
			16,
			17,
			18,
			0,
			8,
			7,
			9,
			6,
			10,
			5,
			11,
			4,
			12,
			3,
			13,
			2,
			14,
			1,
			15
		};

		// Token: 0x040017FC RID: 6140
		internal const int Buf_size = 16;

		// Token: 0x040017FD RID: 6141
		internal const int DIST_CODE_LEN = 512;

		// Token: 0x040017FE RID: 6142
		internal static readonly byte[] _dist_code = new byte[]
		{
			0,
			1,
			2,
			3,
			4,
			4,
			5,
			5,
			6,
			6,
			6,
			6,
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
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			9,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			10,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			11,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			12,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			13,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			14,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			15,
			0,
			0,
			16,
			17,
			18,
			18,
			19,
			19,
			20,
			20,
			20,
			20,
			21,
			21,
			21,
			21,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			28,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29,
			29
		};

		// Token: 0x040017FF RID: 6143
		internal static readonly byte[] _length_code = new byte[]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			8,
			9,
			9,
			10,
			10,
			11,
			11,
			12,
			12,
			12,
			12,
			13,
			13,
			13,
			13,
			14,
			14,
			14,
			14,
			15,
			15,
			15,
			15,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			16,
			17,
			17,
			17,
			17,
			17,
			17,
			17,
			17,
			18,
			18,
			18,
			18,
			18,
			18,
			18,
			18,
			19,
			19,
			19,
			19,
			19,
			19,
			19,
			19,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			20,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			21,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			22,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			23,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			24,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			25,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			26,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			27,
			28
		};

		// Token: 0x04001800 RID: 6144
		internal static readonly int[] base_length = new int[]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			10,
			12,
			14,
			16,
			20,
			24,
			28,
			32,
			40,
			48,
			56,
			64,
			80,
			96,
			112,
			128,
			160,
			192,
			224,
			0
		};

		// Token: 0x04001801 RID: 6145
		internal static readonly int[] base_dist = new int[]
		{
			0,
			1,
			2,
			3,
			4,
			6,
			8,
			12,
			16,
			24,
			32,
			48,
			64,
			96,
			128,
			192,
			256,
			384,
			512,
			768,
			1024,
			1536,
			2048,
			3072,
			4096,
			6144,
			8192,
			12288,
			16384,
			24576
		};

		// Token: 0x04001802 RID: 6146
		internal short[] dyn_tree;

		// Token: 0x04001803 RID: 6147
		internal int max_code;

		// Token: 0x04001804 RID: 6148
		internal StaticTree stat_desc;
	}
}
