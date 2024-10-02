using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x0200026D RID: 621
	internal sealed class Inflate
	{
		// Token: 0x060016C9 RID: 5833 RVA: 0x000B54BC File Offset: 0x000B36BC
		internal int inflateReset(ZStream z)
		{
			if (z == null || z.istate == null)
			{
				return -2;
			}
			z.total_in = (z.total_out = 0L);
			z.msg = null;
			z.istate.mode = ((z.istate.nowrap != 0) ? 7 : 0);
			z.istate.blocks.reset(z, null);
			return 0;
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x000B551E File Offset: 0x000B371E
		internal int inflateEnd(ZStream z)
		{
			if (this.blocks != null)
			{
				this.blocks.free(z);
			}
			this.blocks = null;
			return 0;
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x000B553C File Offset: 0x000B373C
		internal int inflateInit(ZStream z, int w)
		{
			z.msg = null;
			this.blocks = null;
			this.nowrap = 0;
			if (w < 0)
			{
				w = -w;
				this.nowrap = 1;
			}
			if (w < 8 || w > 15)
			{
				this.inflateEnd(z);
				return -2;
			}
			this.wbits = w;
			z.istate.blocks = new InfBlocks(z, (z.istate.nowrap != 0) ? null : this, 1 << w);
			this.inflateReset(z);
			return 0;
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x000B55BC File Offset: 0x000B37BC
		internal int inflate(ZStream z, int f)
		{
			if (z == null || z.istate == null || z.next_in == null)
			{
				return -2;
			}
			f = ((f == 4) ? -5 : 0);
			int num = -5;
			int next_in_index;
			for (;;)
			{
				switch (z.istate.mode)
				{
				case 0:
				{
					if (z.avail_in == 0)
					{
						return num;
					}
					num = f;
					z.avail_in--;
					z.total_in += 1L;
					Inflate istate = z.istate;
					byte[] next_in = z.next_in;
					next_in_index = z.next_in_index;
					z.next_in_index = next_in_index + 1;
					if (((istate.method = next_in[next_in_index]) & 15) != 8)
					{
						z.istate.mode = 13;
						z.msg = "unknown compression method";
						z.istate.marker = 5;
						continue;
					}
					if ((z.istate.method >> 4) + 8 > z.istate.wbits)
					{
						z.istate.mode = 13;
						z.msg = "invalid window size";
						z.istate.marker = 5;
						continue;
					}
					z.istate.mode = 1;
					goto IL_142;
				}
				case 1:
					goto IL_142;
				case 2:
					goto IL_1EA;
				case 3:
					goto IL_253;
				case 4:
					goto IL_2C3;
				case 5:
					goto IL_332;
				case 6:
					goto IL_3AC;
				case 7:
					num = z.istate.blocks.proc(z, num);
					if (num == -3)
					{
						z.istate.mode = 13;
						z.istate.marker = 0;
						continue;
					}
					if (num == 0)
					{
						num = f;
					}
					if (num != 1)
					{
						return num;
					}
					num = f;
					z.istate.blocks.reset(z, z.istate.was);
					if (z.istate.nowrap != 0)
					{
						z.istate.mode = 12;
						continue;
					}
					z.istate.mode = 8;
					goto IL_45D;
				case 8:
					goto IL_45D;
				case 9:
					goto IL_4C7;
				case 10:
					goto IL_538;
				case 11:
					goto IL_5A8;
				case 12:
					return 1;
				case 13:
					return -3;
				}
				break;
				IL_142:
				if (z.avail_in == 0)
				{
					return num;
				}
				num = f;
				z.avail_in--;
				z.total_in += 1L;
				byte[] next_in2 = z.next_in;
				next_in_index = z.next_in_index;
				z.next_in_index = next_in_index + 1;
				int num2 = next_in2[next_in_index] & 255;
				if (((z.istate.method << 8) + num2) % 31 != 0)
				{
					z.istate.mode = 13;
					z.msg = "incorrect header check";
					z.istate.marker = 5;
					continue;
				}
				if ((num2 & 32) == 0)
				{
					z.istate.mode = 7;
					continue;
				}
				goto IL_1DE;
				IL_5A8:
				if (z.avail_in == 0)
				{
					return num;
				}
				num = f;
				z.avail_in--;
				z.total_in += 1L;
				Inflate istate2 = z.istate;
				long num3 = istate2.need;
				byte[] next_in3 = z.next_in;
				next_in_index = z.next_in_index;
				z.next_in_index = next_in_index + 1;
				istate2.need = num3 + (long)(next_in3[next_in_index] & 255UL);
				if ((int)z.istate.was[0] != (int)z.istate.need)
				{
					z.istate.mode = 13;
					z.msg = "incorrect data check";
					z.istate.marker = 5;
					continue;
				}
				goto IL_648;
				IL_538:
				if (z.avail_in == 0)
				{
					return num;
				}
				num = f;
				z.avail_in--;
				z.total_in += 1L;
				Inflate istate3 = z.istate;
				long num4 = istate3.need;
				byte[] next_in4 = z.next_in;
				next_in_index = z.next_in_index;
				z.next_in_index = next_in_index + 1;
				istate3.need = num4 + ((next_in4[next_in_index] & 255L) << 8 & 65280L);
				z.istate.mode = 11;
				goto IL_5A8;
				IL_4C7:
				if (z.avail_in == 0)
				{
					return num;
				}
				num = f;
				z.avail_in--;
				z.total_in += 1L;
				Inflate istate4 = z.istate;
				long num5 = istate4.need;
				byte[] next_in5 = z.next_in;
				next_in_index = z.next_in_index;
				z.next_in_index = next_in_index + 1;
				istate4.need = num5 + ((next_in5[next_in_index] & 255L) << 16 & 16711680L);
				z.istate.mode = 10;
				goto IL_538;
				IL_45D:
				if (z.avail_in == 0)
				{
					return num;
				}
				num = f;
				z.avail_in--;
				z.total_in += 1L;
				Inflate istate5 = z.istate;
				byte[] next_in6 = z.next_in;
				next_in_index = z.next_in_index;
				z.next_in_index = next_in_index + 1;
				istate5.need = ((next_in6[next_in_index] & 255L) << 24 & (long)((ulong)-16777216));
				z.istate.mode = 9;
				goto IL_4C7;
			}
			return -2;
			IL_1DE:
			z.istate.mode = 2;
			IL_1EA:
			if (z.avail_in == 0)
			{
				return num;
			}
			num = f;
			z.avail_in--;
			z.total_in += 1L;
			Inflate istate6 = z.istate;
			byte[] next_in7 = z.next_in;
			next_in_index = z.next_in_index;
			z.next_in_index = next_in_index + 1;
			istate6.need = ((next_in7[next_in_index] & 255L) << 24 & (long)((ulong)-16777216));
			z.istate.mode = 3;
			IL_253:
			if (z.avail_in == 0)
			{
				return num;
			}
			num = f;
			z.avail_in--;
			z.total_in += 1L;
			Inflate istate7 = z.istate;
			long num6 = istate7.need;
			byte[] next_in8 = z.next_in;
			next_in_index = z.next_in_index;
			z.next_in_index = next_in_index + 1;
			istate7.need = num6 + ((next_in8[next_in_index] & 255L) << 16 & 16711680L);
			z.istate.mode = 4;
			IL_2C3:
			if (z.avail_in == 0)
			{
				return num;
			}
			num = f;
			z.avail_in--;
			z.total_in += 1L;
			Inflate istate8 = z.istate;
			long num7 = istate8.need;
			byte[] next_in9 = z.next_in;
			next_in_index = z.next_in_index;
			z.next_in_index = next_in_index + 1;
			istate8.need = num7 + ((next_in9[next_in_index] & 255L) << 8 & 65280L);
			z.istate.mode = 5;
			IL_332:
			if (z.avail_in == 0)
			{
				return num;
			}
			z.avail_in--;
			z.total_in += 1L;
			Inflate istate9 = z.istate;
			long num8 = istate9.need;
			byte[] next_in10 = z.next_in;
			next_in_index = z.next_in_index;
			z.next_in_index = next_in_index + 1;
			istate9.need = num8 + (long)(next_in10[next_in_index] & 255UL);
			z.adler = z.istate.need;
			z.istate.mode = 6;
			return 2;
			IL_3AC:
			z.istate.mode = 13;
			z.msg = "need dictionary";
			z.istate.marker = 0;
			return -2;
			IL_648:
			z.istate.mode = 12;
			return 1;
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x000B5C28 File Offset: 0x000B3E28
		internal int inflateSetDictionary(ZStream z, byte[] dictionary, int dictLength)
		{
			int start = 0;
			int num = dictLength;
			if (z == null || z.istate == null || z.istate.mode != 6)
			{
				return -2;
			}
			if (z._adler.adler32(1L, dictionary, 0, dictLength) != z.adler)
			{
				return -3;
			}
			z.adler = z._adler.adler32(0L, null, 0, 0);
			if (num >= 1 << z.istate.wbits)
			{
				num = (1 << z.istate.wbits) - 1;
				start = dictLength - num;
			}
			z.istate.blocks.set_dictionary(dictionary, start, num);
			z.istate.mode = 7;
			return 0;
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x000B5CD0 File Offset: 0x000B3ED0
		internal int inflateSync(ZStream z)
		{
			if (z == null || z.istate == null)
			{
				return -2;
			}
			if (z.istate.mode != 13)
			{
				z.istate.mode = 13;
				z.istate.marker = 0;
			}
			int num;
			if ((num = z.avail_in) == 0)
			{
				return -5;
			}
			int num2 = z.next_in_index;
			int num3 = z.istate.marker;
			while (num != 0 && num3 < 4)
			{
				if (z.next_in[num2] == Inflate.mark[num3])
				{
					num3++;
				}
				else if (z.next_in[num2] != 0)
				{
					num3 = 0;
				}
				else
				{
					num3 = 4 - num3;
				}
				num2++;
				num--;
			}
			z.total_in += (long)(num2 - z.next_in_index);
			z.next_in_index = num2;
			z.avail_in = num;
			z.istate.marker = num3;
			if (num3 != 4)
			{
				return -3;
			}
			long total_in = z.total_in;
			long total_out = z.total_out;
			this.inflateReset(z);
			z.total_in = total_in;
			z.total_out = total_out;
			z.istate.mode = 7;
			return 0;
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x000B5DD7 File Offset: 0x000B3FD7
		internal int inflateSyncPoint(ZStream z)
		{
			if (z == null || z.istate == null || z.istate.blocks == null)
			{
				return -2;
			}
			return z.istate.blocks.sync_point();
		}

		// Token: 0x04001747 RID: 5959
		private const int MAX_WBITS = 15;

		// Token: 0x04001748 RID: 5960
		private const int PRESET_DICT = 32;

		// Token: 0x04001749 RID: 5961
		internal const int Z_NO_FLUSH = 0;

		// Token: 0x0400174A RID: 5962
		internal const int Z_PARTIAL_FLUSH = 1;

		// Token: 0x0400174B RID: 5963
		internal const int Z_SYNC_FLUSH = 2;

		// Token: 0x0400174C RID: 5964
		internal const int Z_FULL_FLUSH = 3;

		// Token: 0x0400174D RID: 5965
		internal const int Z_FINISH = 4;

		// Token: 0x0400174E RID: 5966
		private const int Z_DEFLATED = 8;

		// Token: 0x0400174F RID: 5967
		private const int Z_OK = 0;

		// Token: 0x04001750 RID: 5968
		private const int Z_STREAM_END = 1;

		// Token: 0x04001751 RID: 5969
		private const int Z_NEED_DICT = 2;

		// Token: 0x04001752 RID: 5970
		private const int Z_ERRNO = -1;

		// Token: 0x04001753 RID: 5971
		private const int Z_STREAM_ERROR = -2;

		// Token: 0x04001754 RID: 5972
		private const int Z_DATA_ERROR = -3;

		// Token: 0x04001755 RID: 5973
		private const int Z_MEM_ERROR = -4;

		// Token: 0x04001756 RID: 5974
		private const int Z_BUF_ERROR = -5;

		// Token: 0x04001757 RID: 5975
		private const int Z_VERSION_ERROR = -6;

		// Token: 0x04001758 RID: 5976
		private const int METHOD = 0;

		// Token: 0x04001759 RID: 5977
		private const int FLAG = 1;

		// Token: 0x0400175A RID: 5978
		private const int DICT4 = 2;

		// Token: 0x0400175B RID: 5979
		private const int DICT3 = 3;

		// Token: 0x0400175C RID: 5980
		private const int DICT2 = 4;

		// Token: 0x0400175D RID: 5981
		private const int DICT1 = 5;

		// Token: 0x0400175E RID: 5982
		private const int DICT0 = 6;

		// Token: 0x0400175F RID: 5983
		private const int BLOCKS = 7;

		// Token: 0x04001760 RID: 5984
		private const int CHECK4 = 8;

		// Token: 0x04001761 RID: 5985
		private const int CHECK3 = 9;

		// Token: 0x04001762 RID: 5986
		private const int CHECK2 = 10;

		// Token: 0x04001763 RID: 5987
		private const int CHECK1 = 11;

		// Token: 0x04001764 RID: 5988
		private const int DONE = 12;

		// Token: 0x04001765 RID: 5989
		private const int BAD = 13;

		// Token: 0x04001766 RID: 5990
		internal int mode;

		// Token: 0x04001767 RID: 5991
		internal int method;

		// Token: 0x04001768 RID: 5992
		internal long[] was = new long[1];

		// Token: 0x04001769 RID: 5993
		internal long need;

		// Token: 0x0400176A RID: 5994
		internal int marker;

		// Token: 0x0400176B RID: 5995
		internal int nowrap;

		// Token: 0x0400176C RID: 5996
		internal int wbits;

		// Token: 0x0400176D RID: 5997
		internal InfBlocks blocks;

		// Token: 0x0400176E RID: 5998
		private static readonly byte[] mark = new byte[]
		{
			0,
			0,
			byte.MaxValue,
			byte.MaxValue
		};
	}
}
