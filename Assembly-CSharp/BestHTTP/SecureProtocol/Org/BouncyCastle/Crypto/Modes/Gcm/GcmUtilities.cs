using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes.Gcm
{
	// Token: 0x02000529 RID: 1321
	internal abstract class GcmUtilities
	{
		// Token: 0x06003206 RID: 12806 RVA: 0x0012E138 File Offset: 0x0012C338
		private static uint[] GenerateLookup()
		{
			uint[] array = new uint[256];
			for (int i = 0; i < 256; i++)
			{
				uint num = 0U;
				for (int j = 7; j >= 0; j--)
				{
					if ((i & 1 << j) != 0)
					{
						num ^= 3774873600U >> 7 - j;
					}
				}
				array[i] = num;
			}
			return array;
		}

		// Token: 0x06003207 RID: 12807 RVA: 0x0012E18C File Offset: 0x0012C38C
		internal static byte[] OneAsBytes()
		{
			byte[] array = new byte[16];
			array[0] = 128;
			return array;
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x0012E19D File Offset: 0x0012C39D
		internal static uint[] OneAsUints()
		{
			uint[] array = new uint[4];
			array[0] = 2147483648U;
			return array;
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x0012E1AD File Offset: 0x0012C3AD
		internal static ulong[] OneAsUlongs()
		{
			ulong[] array = new ulong[2];
			array[0] = 9223372036854775808UL;
			return array;
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x0012E1C1 File Offset: 0x0012C3C1
		internal static byte[] AsBytes(uint[] x)
		{
			return Pack.UInt32_To_BE(x);
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x0012E1C9 File Offset: 0x0012C3C9
		internal static void AsBytes(uint[] x, byte[] z)
		{
			Pack.UInt32_To_BE(x, z, 0);
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x0012E1D4 File Offset: 0x0012C3D4
		internal static byte[] AsBytes(ulong[] x)
		{
			byte[] array = new byte[16];
			Pack.UInt64_To_BE(x, array, 0);
			return array;
		}

		// Token: 0x0600320D RID: 12813 RVA: 0x0012E1F2 File Offset: 0x0012C3F2
		internal static void AsBytes(ulong[] x, byte[] z)
		{
			Pack.UInt64_To_BE(x, z, 0);
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x0012E1FC File Offset: 0x0012C3FC
		internal static uint[] AsUints(byte[] bs)
		{
			uint[] array = new uint[4];
			Pack.BE_To_UInt32(bs, 0, array);
			return array;
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x0012E219 File Offset: 0x0012C419
		internal static void AsUints(byte[] bs, uint[] output)
		{
			Pack.BE_To_UInt32(bs, 0, output);
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x0012E224 File Offset: 0x0012C424
		internal static ulong[] AsUlongs(byte[] x)
		{
			ulong[] array = new ulong[2];
			Pack.BE_To_UInt64(x, 0, array);
			return array;
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x0012E241 File Offset: 0x0012C441
		public static void AsUlongs(byte[] x, ulong[] z)
		{
			Pack.BE_To_UInt64(x, 0, z);
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x0012E24C File Offset: 0x0012C44C
		internal static void Multiply(byte[] x, byte[] y)
		{
			uint[] x2 = GcmUtilities.AsUints(x);
			uint[] y2 = GcmUtilities.AsUints(y);
			GcmUtilities.Multiply(x2, y2);
			GcmUtilities.AsBytes(x2, x);
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x0012E274 File Offset: 0x0012C474
		internal static void Multiply(uint[] x, uint[] y)
		{
			uint num = x[0];
			uint num2 = x[1];
			uint num3 = x[2];
			uint num4 = x[3];
			uint num5 = 0U;
			uint num6 = 0U;
			uint num7 = 0U;
			uint num8 = 0U;
			for (int i = 0; i < 4; i++)
			{
				int num9 = (int)y[i];
				for (int j = 0; j < 32; j++)
				{
					uint num10 = (uint)(num9 >> 31);
					num9 <<= 1;
					num5 ^= (num & num10);
					num6 ^= (num2 & num10);
					num7 ^= (num3 & num10);
					num8 ^= (num4 & num10);
					uint num11 = (uint)((int)((int)num4 << 31) >> 8);
					num4 = (num4 >> 1 | num3 << 31);
					num3 = (num3 >> 1 | num2 << 31);
					num2 = (num2 >> 1 | num << 31);
					num = (num >> 1 ^ (num11 & 3774873600U));
				}
			}
			x[0] = num5;
			x[1] = num6;
			x[2] = num7;
			x[3] = num8;
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x0012E33C File Offset: 0x0012C53C
		internal static void Multiply(ulong[] x, ulong[] y)
		{
			ulong num = x[0];
			ulong num2 = x[1];
			ulong num3 = 0UL;
			ulong num4 = 0UL;
			for (int i = 0; i < 2; i++)
			{
				long num5 = (long)y[i];
				for (int j = 0; j < 64; j++)
				{
					ulong num6 = (ulong)(num5 >> 63);
					num5 <<= 1;
					num3 ^= (num & num6);
					num4 ^= (num2 & num6);
					ulong num7 = num2 << 63 >> 8;
					num2 = (num2 >> 1 | num << 63);
					num = (num >> 1 ^ (num7 & 16212958658533785600UL));
				}
			}
			x[0] = num3;
			x[1] = num4;
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x0012E3C4 File Offset: 0x0012C5C4
		internal static void MultiplyP(uint[] x)
		{
			uint num = (uint)((int)GcmUtilities.ShiftRight(x) >> 8);
			x[0] ^= (num & 3774873600U);
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x0012E3EC File Offset: 0x0012C5EC
		internal static void MultiplyP(uint[] x, uint[] z)
		{
			uint num = (uint)((int)GcmUtilities.ShiftRight(x, z) >> 8);
			z[0] ^= (num & 3774873600U);
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x0012E418 File Offset: 0x0012C618
		internal static void MultiplyP8(uint[] x)
		{
			uint num = GcmUtilities.ShiftRightN(x, 8);
			x[0] ^= GcmUtilities.LOOKUP[(int)(num >> 24)];
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x0012E444 File Offset: 0x0012C644
		internal static void MultiplyP8(uint[] x, uint[] y)
		{
			uint num = GcmUtilities.ShiftRightN(x, 8, y);
			y[0] ^= GcmUtilities.LOOKUP[(int)(num >> 24)];
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x0012E470 File Offset: 0x0012C670
		internal static uint ShiftRight(uint[] x)
		{
			uint num = x[0];
			x[0] = num >> 1;
			uint num2 = num << 31;
			num = x[1];
			x[1] = (num >> 1 | num2);
			num2 = num << 31;
			num = x[2];
			x[2] = (num >> 1 | num2);
			num2 = num << 31;
			num = x[3];
			x[3] = (num >> 1 | num2);
			return num << 31;
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x0012E4C0 File Offset: 0x0012C6C0
		internal static uint ShiftRight(uint[] x, uint[] z)
		{
			uint num = x[0];
			z[0] = num >> 1;
			uint num2 = num << 31;
			num = x[1];
			z[1] = (num >> 1 | num2);
			num2 = num << 31;
			num = x[2];
			z[2] = (num >> 1 | num2);
			num2 = num << 31;
			num = x[3];
			z[3] = (num >> 1 | num2);
			return num << 31;
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x0012E510 File Offset: 0x0012C710
		internal static uint ShiftRightN(uint[] x, int n)
		{
			uint num = x[0];
			int num2 = 32 - n;
			x[0] = num >> n;
			uint num3 = num << num2;
			num = x[1];
			x[1] = (num >> n | num3);
			num3 = num << num2;
			num = x[2];
			x[2] = (num >> n | num3);
			num3 = num << num2;
			num = x[3];
			x[3] = (num >> n | num3);
			return num << num2;
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x0012E578 File Offset: 0x0012C778
		internal static uint ShiftRightN(uint[] x, int n, uint[] z)
		{
			uint num = x[0];
			int num2 = 32 - n;
			z[0] = num >> n;
			uint num3 = num << num2;
			num = x[1];
			z[1] = (num >> n | num3);
			num3 = num << num2;
			num = x[2];
			z[2] = (num >> n | num3);
			num3 = num << num2;
			num = x[3];
			z[3] = (num >> n | num3);
			return num << num2;
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x0012E5E0 File Offset: 0x0012C7E0
		internal static void Xor(byte[] x, byte[] y)
		{
			int num = 0;
			do
			{
				int num2 = num;
				x[num2] ^= y[num];
				num++;
				int num3 = num;
				x[num3] ^= y[num];
				num++;
				int num4 = num;
				x[num4] ^= y[num];
				num++;
				int num5 = num;
				x[num5] ^= y[num];
				num++;
			}
			while (num < 16);
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x0012E640 File Offset: 0x0012C840
		internal static void Xor(byte[] x, byte[] y, int yOff)
		{
			int num = 0;
			do
			{
				int num2 = num;
				x[num2] ^= y[yOff + num];
				num++;
				int num3 = num;
				x[num3] ^= y[yOff + num];
				num++;
				int num4 = num;
				x[num4] ^= y[yOff + num];
				num++;
				int num5 = num;
				x[num5] ^= y[yOff + num];
				num++;
			}
			while (num < 16);
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x0012E6A8 File Offset: 0x0012C8A8
		internal static void Xor(byte[] x, int xOff, byte[] y, int yOff, byte[] z, int zOff)
		{
			int num = 0;
			do
			{
				z[zOff + num] = (x[xOff + num] ^ y[yOff + num]);
				num++;
				z[zOff + num] = (x[xOff + num] ^ y[yOff + num]);
				num++;
				z[zOff + num] = (x[xOff + num] ^ y[yOff + num]);
				num++;
				z[zOff + num] = (x[xOff + num] ^ y[yOff + num]);
				num++;
			}
			while (num < 16);
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x0012E718 File Offset: 0x0012C918
		internal static void Xor(byte[] x, byte[] y, int yOff, int yLen)
		{
			while (--yLen >= 0)
			{
				int num = yLen;
				x[num] ^= y[yOff + yLen];
			}
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x0012E736 File Offset: 0x0012C936
		internal static void Xor(byte[] x, int xOff, byte[] y, int yOff, int len)
		{
			while (--len >= 0)
			{
				int num = xOff + len;
				x[num] ^= y[yOff + len];
			}
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x0012E75C File Offset: 0x0012C95C
		internal static void Xor(byte[] x, byte[] y, byte[] z)
		{
			int num = 0;
			do
			{
				z[num] = (x[num] ^ y[num]);
				num++;
				z[num] = (x[num] ^ y[num]);
				num++;
				z[num] = (x[num] ^ y[num]);
				num++;
				z[num] = (x[num] ^ y[num]);
				num++;
			}
			while (num < 16);
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x0012E7AC File Offset: 0x0012C9AC
		internal static void Xor(uint[] x, uint[] y)
		{
			x[0] ^= y[0];
			x[1] ^= y[1];
			x[2] ^= y[2];
			x[3] ^= y[3];
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x0012E7E6 File Offset: 0x0012C9E6
		internal static void Xor(uint[] x, uint[] y, uint[] z)
		{
			z[0] = (x[0] ^ y[0]);
			z[1] = (x[1] ^ y[1]);
			z[2] = (x[2] ^ y[2]);
			z[3] = (x[3] ^ y[3]);
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x0012E810 File Offset: 0x0012CA10
		internal static void Xor(ulong[] x, ulong[] y)
		{
			x[0] ^= y[0];
			x[1] ^= y[1];
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x000FAE14 File Offset: 0x000F9014
		internal static void Xor(ulong[] x, ulong[] y, ulong[] z)
		{
			z[0] = (x[0] ^ y[0]);
			z[1] = (x[1] ^ y[1]);
		}

		// Token: 0x040020EA RID: 8426
		private const uint E1 = 3774873600U;

		// Token: 0x040020EB RID: 8427
		private const ulong E1L = 16212958658533785600UL;

		// Token: 0x040020EC RID: 8428
		private static readonly uint[] LOOKUP = GcmUtilities.GenerateLookup();
	}
}
