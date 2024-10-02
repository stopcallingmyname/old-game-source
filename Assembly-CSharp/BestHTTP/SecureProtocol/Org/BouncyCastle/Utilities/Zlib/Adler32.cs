﻿using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x02000269 RID: 617
	internal sealed class Adler32
	{
		// Token: 0x06001691 RID: 5777 RVA: 0x000B0E6C File Offset: 0x000AF06C
		internal long adler32(long adler, byte[] buf, int index, int len)
		{
			if (buf == null)
			{
				return 1L;
			}
			long num = adler & 65535L;
			long num2 = adler >> 16 & 65535L;
			while (len > 0)
			{
				int i = (len < 5552) ? len : 5552;
				len -= i;
				while (i >= 16)
				{
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					num += (long)(buf[index++] & byte.MaxValue);
					num2 += num;
					i -= 16;
				}
				if (i != 0)
				{
					do
					{
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
					}
					while (--i != 0);
				}
				num %= 65521L;
				num2 %= 65521L;
			}
			return num2 << 16 | num;
		}

		// Token: 0x04001689 RID: 5769
		private const int BASE = 65521;

		// Token: 0x0400168A RID: 5770
		private const int NMAX = 5552;
	}
}
