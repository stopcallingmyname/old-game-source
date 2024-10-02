﻿using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes.Gcm
{
	// Token: 0x0200052D RID: 1325
	public class Tables64kGcmMultiplier : IGcmMultiplier
	{
		// Token: 0x06003231 RID: 12849 RVA: 0x0012E930 File Offset: 0x0012CB30
		public void Init(byte[] H)
		{
			if (this.M == null)
			{
				this.M = new uint[16][][];
			}
			else if (Arrays.AreEqual(this.H, H))
			{
				return;
			}
			this.H = Arrays.Clone(H);
			this.M[0] = new uint[256][];
			this.M[0][0] = new uint[4];
			this.M[0][128] = GcmUtilities.AsUints(H);
			for (int i = 64; i >= 1; i >>= 1)
			{
				uint[] array = (uint[])this.M[0][i + i].Clone();
				GcmUtilities.MultiplyP(array);
				this.M[0][i] = array;
			}
			int num = 0;
			for (;;)
			{
				for (int j = 2; j < 256; j += j)
				{
					for (int k = 1; k < j; k++)
					{
						uint[] array2 = (uint[])this.M[num][j].Clone();
						GcmUtilities.Xor(array2, this.M[num][k]);
						this.M[num][j + k] = array2;
					}
				}
				if (++num == 16)
				{
					break;
				}
				this.M[num] = new uint[256][];
				this.M[num][0] = new uint[4];
				for (int l = 128; l > 0; l >>= 1)
				{
					uint[] array3 = (uint[])this.M[num - 1][l].Clone();
					GcmUtilities.MultiplyP8(array3);
					this.M[num][l] = array3;
				}
			}
		}

		// Token: 0x06003232 RID: 12850 RVA: 0x0012EAA4 File Offset: 0x0012CCA4
		public void MultiplyH(byte[] x)
		{
			uint[] array = new uint[4];
			for (int num = 0; num != 16; num++)
			{
				uint[] array2 = this.M[num][(int)x[num]];
				array[0] ^= array2[0];
				array[1] ^= array2[1];
				array[2] ^= array2[2];
				array[3] ^= array2[3];
			}
			Pack.UInt32_To_BE(array, x, 0);
		}

		// Token: 0x040020EE RID: 8430
		private byte[] H;

		// Token: 0x040020EF RID: 8431
		private uint[][][] M;
	}
}
