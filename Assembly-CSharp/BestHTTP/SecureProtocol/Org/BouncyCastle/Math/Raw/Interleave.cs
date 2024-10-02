using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw
{
	// Token: 0x02000307 RID: 775
	internal abstract class Interleave
	{
		// Token: 0x06001C72 RID: 7282 RVA: 0x000D6224 File Offset: 0x000D4424
		internal static uint Expand8to16(uint x)
		{
			x &= 255U;
			x = ((x | x << 4) & 3855U);
			x = ((x | x << 2) & 13107U);
			x = ((x | x << 1) & 21845U);
			return x;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000D6257 File Offset: 0x000D4457
		internal static uint Expand16to32(uint x)
		{
			x &= 65535U;
			x = ((x | x << 8) & 16711935U);
			x = ((x | x << 4) & 252645135U);
			x = ((x | x << 2) & 858993459U);
			x = ((x | x << 1) & 1431655765U);
			return x;
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x000D6298 File Offset: 0x000D4498
		internal static ulong Expand32to64(uint x)
		{
			uint num = (x ^ x >> 8) & 65280U;
			x ^= (num ^ num << 8);
			num = ((x ^ x >> 4) & 15728880U);
			x ^= (num ^ num << 4);
			num = ((x ^ x >> 2) & 202116108U);
			x ^= (num ^ num << 2);
			num = ((x ^ x >> 1) & 572662306U);
			x ^= (num ^ num << 1);
			return ((ulong)(x >> 1) & 1431655765UL) << 32 | ((ulong)x & 1431655765UL);
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x000D6314 File Offset: 0x000D4514
		internal static void Expand64To128(ulong x, ulong[] z, int zOff)
		{
			ulong num = (x ^ x >> 16) & (ulong)-65536;
			x ^= (num ^ num << 16);
			num = ((x ^ x >> 8) & 280375465148160UL);
			x ^= (num ^ num << 8);
			num = ((x ^ x >> 4) & 67555025218437360UL);
			x ^= (num ^ num << 4);
			num = ((x ^ x >> 2) & 868082074056920076UL);
			x ^= (num ^ num << 2);
			num = ((x ^ x >> 1) & 2459565876494606882UL);
			x ^= (num ^ num << 1);
			z[zOff] = (x & 6148914691236517205UL);
			z[zOff + 1] = (x >> 1 & 6148914691236517205UL);
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x000D63C0 File Offset: 0x000D45C0
		internal static void Expand64To128Rev(ulong x, ulong[] z, int zOff)
		{
			ulong num = (x ^ x >> 16) & (ulong)-65536;
			x ^= (num ^ num << 16);
			num = ((x ^ x >> 8) & 280375465148160UL);
			x ^= (num ^ num << 8);
			num = ((x ^ x >> 4) & 67555025218437360UL);
			x ^= (num ^ num << 4);
			num = ((x ^ x >> 2) & 868082074056920076UL);
			x ^= (num ^ num << 2);
			num = ((x ^ x >> 1) & 2459565876494606882UL);
			x ^= (num ^ num << 1);
			z[zOff] = (x & 12297829382473034410UL);
			z[zOff + 1] = (x << 1 & 12297829382473034410UL);
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x000D646C File Offset: 0x000D466C
		internal static uint Shuffle(uint x)
		{
			uint num = (x ^ x >> 8) & 65280U;
			x ^= (num ^ num << 8);
			num = ((x ^ x >> 4) & 15728880U);
			x ^= (num ^ num << 4);
			num = ((x ^ x >> 2) & 202116108U);
			x ^= (num ^ num << 2);
			num = ((x ^ x >> 1) & 572662306U);
			x ^= (num ^ num << 1);
			return x;
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x000D64D0 File Offset: 0x000D46D0
		internal static ulong Shuffle(ulong x)
		{
			ulong num = (x ^ x >> 16) & (ulong)-65536;
			x ^= (num ^ num << 16);
			num = ((x ^ x >> 8) & 280375465148160UL);
			x ^= (num ^ num << 8);
			num = ((x ^ x >> 4) & 67555025218437360UL);
			x ^= (num ^ num << 4);
			num = ((x ^ x >> 2) & 868082074056920076UL);
			x ^= (num ^ num << 2);
			num = ((x ^ x >> 1) & 2459565876494606882UL);
			x ^= (num ^ num << 1);
			return x;
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x000D655C File Offset: 0x000D475C
		internal static uint Shuffle2(uint x)
		{
			uint num = (x ^ x >> 7) & 11141290U;
			x ^= (num ^ num << 7);
			num = ((x ^ x >> 14) & 52428U);
			x ^= (num ^ num << 14);
			num = ((x ^ x >> 4) & 15728880U);
			x ^= (num ^ num << 4);
			num = ((x ^ x >> 8) & 65280U);
			x ^= (num ^ num << 8);
			return x;
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000D65C0 File Offset: 0x000D47C0
		internal static uint Unshuffle(uint x)
		{
			uint num = (x ^ x >> 1) & 572662306U;
			x ^= (num ^ num << 1);
			num = ((x ^ x >> 2) & 202116108U);
			x ^= (num ^ num << 2);
			num = ((x ^ x >> 4) & 15728880U);
			x ^= (num ^ num << 4);
			num = ((x ^ x >> 8) & 65280U);
			x ^= (num ^ num << 8);
			return x;
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x000D6624 File Offset: 0x000D4824
		internal static ulong Unshuffle(ulong x)
		{
			ulong num = (x ^ x >> 1) & 2459565876494606882UL;
			x ^= (num ^ num << 1);
			num = ((x ^ x >> 2) & 868082074056920076UL);
			x ^= (num ^ num << 2);
			num = ((x ^ x >> 4) & 67555025218437360UL);
			x ^= (num ^ num << 4);
			num = ((x ^ x >> 8) & 280375465148160UL);
			x ^= (num ^ num << 8);
			num = ((x ^ x >> 16) & (ulong)-65536);
			x ^= (num ^ num << 16);
			return x;
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x000D66B0 File Offset: 0x000D48B0
		internal static uint Unshuffle2(uint x)
		{
			uint num = (x ^ x >> 8) & 65280U;
			x ^= (num ^ num << 8);
			num = ((x ^ x >> 4) & 15728880U);
			x ^= (num ^ num << 4);
			num = ((x ^ x >> 14) & 52428U);
			x ^= (num ^ num << 14);
			num = ((x ^ x >> 7) & 11141290U);
			x ^= (num ^ num << 7);
			return x;
		}

		// Token: 0x0400193D RID: 6461
		private const ulong M32 = 1431655765UL;

		// Token: 0x0400193E RID: 6462
		private const ulong M64 = 6148914691236517205UL;

		// Token: 0x0400193F RID: 6463
		private const ulong M64R = 12297829382473034410UL;
	}
}
