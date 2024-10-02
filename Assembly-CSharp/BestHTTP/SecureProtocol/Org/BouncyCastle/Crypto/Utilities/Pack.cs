using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities
{
	// Token: 0x020003EF RID: 1007
	internal sealed class Pack
	{
		// Token: 0x060028A0 RID: 10400 RVA: 0x00022F1F File Offset: 0x0002111F
		private Pack()
		{
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x0010CD45 File Offset: 0x0010AF45
		internal static void UInt16_To_BE(ushort n, byte[] bs)
		{
			bs[0] = (byte)(n >> 8);
			bs[1] = (byte)n;
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x0010CD53 File Offset: 0x0010AF53
		internal static void UInt16_To_BE(ushort n, byte[] bs, int off)
		{
			bs[off] = (byte)(n >> 8);
			bs[off + 1] = (byte)n;
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x0010CD63 File Offset: 0x0010AF63
		internal static ushort BE_To_UInt16(byte[] bs)
		{
			return (ushort)((int)bs[0] << 8 | (int)bs[1]);
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x0010CD6F File Offset: 0x0010AF6F
		internal static ushort BE_To_UInt16(byte[] bs, int off)
		{
			return (ushort)((int)bs[off] << 8 | (int)bs[off + 1]);
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x0010CD80 File Offset: 0x0010AF80
		internal static byte[] UInt32_To_BE(uint n)
		{
			byte[] array = new byte[4];
			Pack.UInt32_To_BE(n, array, 0);
			return array;
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x0010CD9D File Offset: 0x0010AF9D
		internal static void UInt32_To_BE(uint n, byte[] bs)
		{
			bs[0] = (byte)(n >> 24);
			bs[1] = (byte)(n >> 16);
			bs[2] = (byte)(n >> 8);
			bs[3] = (byte)n;
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x0010CDBB File Offset: 0x0010AFBB
		internal static void UInt32_To_BE(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)(n >> 24);
			bs[off + 1] = (byte)(n >> 16);
			bs[off + 2] = (byte)(n >> 8);
			bs[off + 3] = (byte)n;
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x0010CDE0 File Offset: 0x0010AFE0
		internal static byte[] UInt32_To_BE(uint[] ns)
		{
			byte[] array = new byte[4 * ns.Length];
			Pack.UInt32_To_BE(ns, array, 0);
			return array;
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x0010CE04 File Offset: 0x0010B004
		internal static void UInt32_To_BE(uint[] ns, byte[] bs, int off)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				Pack.UInt32_To_BE(ns[i], bs, off);
				off += 4;
			}
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x0010CE2E File Offset: 0x0010B02E
		internal static uint BE_To_UInt32(byte[] bs)
		{
			return (uint)((int)bs[0] << 24 | (int)bs[1] << 16 | (int)bs[2] << 8 | (int)bs[3]);
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x0010CE47 File Offset: 0x0010B047
		internal static uint BE_To_UInt32(byte[] bs, int off)
		{
			return (uint)((int)bs[off] << 24 | (int)bs[off + 1] << 16 | (int)bs[off + 2] << 8 | (int)bs[off + 3]);
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x0010CE68 File Offset: 0x0010B068
		internal static void BE_To_UInt32(byte[] bs, int off, uint[] ns)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				ns[i] = Pack.BE_To_UInt32(bs, off);
				off += 4;
			}
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x0010CE94 File Offset: 0x0010B094
		internal static byte[] UInt64_To_BE(ulong n)
		{
			byte[] array = new byte[8];
			Pack.UInt64_To_BE(n, array, 0);
			return array;
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x0010CEB1 File Offset: 0x0010B0B1
		internal static void UInt64_To_BE(ulong n, byte[] bs)
		{
			Pack.UInt32_To_BE((uint)(n >> 32), bs);
			Pack.UInt32_To_BE((uint)n, bs, 4);
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x0010CEC7 File Offset: 0x0010B0C7
		internal static void UInt64_To_BE(ulong n, byte[] bs, int off)
		{
			Pack.UInt32_To_BE((uint)(n >> 32), bs, off);
			Pack.UInt32_To_BE((uint)n, bs, off + 4);
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x0010CEE0 File Offset: 0x0010B0E0
		internal static byte[] UInt64_To_BE(ulong[] ns)
		{
			byte[] array = new byte[8 * ns.Length];
			Pack.UInt64_To_BE(ns, array, 0);
			return array;
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x0010CF04 File Offset: 0x0010B104
		internal static void UInt64_To_BE(ulong[] ns, byte[] bs, int off)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				Pack.UInt64_To_BE(ns[i], bs, off);
				off += 8;
			}
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x0010CF30 File Offset: 0x0010B130
		internal static ulong BE_To_UInt64(byte[] bs)
		{
			ulong num = (ulong)Pack.BE_To_UInt32(bs);
			uint num2 = Pack.BE_To_UInt32(bs, 4);
			return num << 32 | (ulong)num2;
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x0010CF54 File Offset: 0x0010B154
		internal static ulong BE_To_UInt64(byte[] bs, int off)
		{
			ulong num = (ulong)Pack.BE_To_UInt32(bs, off);
			uint num2 = Pack.BE_To_UInt32(bs, off + 4);
			return num << 32 | (ulong)num2;
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x0010CF7C File Offset: 0x0010B17C
		internal static void BE_To_UInt64(byte[] bs, int off, ulong[] ns)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				ns[i] = Pack.BE_To_UInt64(bs, off);
				off += 8;
			}
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x0010CFA6 File Offset: 0x0010B1A6
		internal static void UInt16_To_LE(ushort n, byte[] bs)
		{
			bs[0] = (byte)n;
			bs[1] = (byte)(n >> 8);
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x0010CFB4 File Offset: 0x0010B1B4
		internal static void UInt16_To_LE(ushort n, byte[] bs, int off)
		{
			bs[off] = (byte)n;
			bs[off + 1] = (byte)(n >> 8);
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x0010CFC4 File Offset: 0x0010B1C4
		internal static ushort LE_To_UInt16(byte[] bs)
		{
			return (ushort)((int)bs[0] | (int)bs[1] << 8);
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x0010CFD0 File Offset: 0x0010B1D0
		internal static ushort LE_To_UInt16(byte[] bs, int off)
		{
			return (ushort)((int)bs[off] | (int)bs[off + 1] << 8);
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x0010CFE0 File Offset: 0x0010B1E0
		internal static byte[] UInt32_To_LE(uint n)
		{
			byte[] array = new byte[4];
			Pack.UInt32_To_LE(n, array, 0);
			return array;
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x0010CFFD File Offset: 0x0010B1FD
		internal static void UInt32_To_LE(uint n, byte[] bs)
		{
			bs[0] = (byte)n;
			bs[1] = (byte)(n >> 8);
			bs[2] = (byte)(n >> 16);
			bs[3] = (byte)(n >> 24);
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x0010D01B File Offset: 0x0010B21B
		internal static void UInt32_To_LE(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)n;
			bs[off + 1] = (byte)(n >> 8);
			bs[off + 2] = (byte)(n >> 16);
			bs[off + 3] = (byte)(n >> 24);
		}

		// Token: 0x060028BC RID: 10428 RVA: 0x0010D040 File Offset: 0x0010B240
		internal static byte[] UInt32_To_LE(uint[] ns)
		{
			byte[] array = new byte[4 * ns.Length];
			Pack.UInt32_To_LE(ns, array, 0);
			return array;
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x0010D064 File Offset: 0x0010B264
		internal static void UInt32_To_LE(uint[] ns, byte[] bs, int off)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				Pack.UInt32_To_LE(ns[i], bs, off);
				off += 4;
			}
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x0010D08E File Offset: 0x0010B28E
		internal static uint LE_To_UInt32(byte[] bs)
		{
			return (uint)((int)bs[0] | (int)bs[1] << 8 | (int)bs[2] << 16 | (int)bs[3] << 24);
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x0010D0A7 File Offset: 0x0010B2A7
		internal static uint LE_To_UInt32(byte[] bs, int off)
		{
			return (uint)((int)bs[off] | (int)bs[off + 1] << 8 | (int)bs[off + 2] << 16 | (int)bs[off + 3] << 24);
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x0010D0C8 File Offset: 0x0010B2C8
		internal static void LE_To_UInt32(byte[] bs, int off, uint[] ns)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				ns[i] = Pack.LE_To_UInt32(bs, off);
				off += 4;
			}
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x0010D0F4 File Offset: 0x0010B2F4
		internal static void LE_To_UInt32(byte[] bs, int bOff, uint[] ns, int nOff, int count)
		{
			for (int i = 0; i < count; i++)
			{
				ns[nOff + i] = Pack.LE_To_UInt32(bs, bOff);
				bOff += 4;
			}
		}

		// Token: 0x060028C2 RID: 10434 RVA: 0x0010D120 File Offset: 0x0010B320
		internal static uint[] LE_To_UInt32(byte[] bs, int off, int count)
		{
			uint[] array = new uint[count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Pack.LE_To_UInt32(bs, off);
				off += 4;
			}
			return array;
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x0010D154 File Offset: 0x0010B354
		internal static byte[] UInt64_To_LE(ulong n)
		{
			byte[] array = new byte[8];
			Pack.UInt64_To_LE(n, array, 0);
			return array;
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x0010D171 File Offset: 0x0010B371
		internal static void UInt64_To_LE(ulong n, byte[] bs)
		{
			Pack.UInt32_To_LE((uint)n, bs);
			Pack.UInt32_To_LE((uint)(n >> 32), bs, 4);
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x0010D187 File Offset: 0x0010B387
		internal static void UInt64_To_LE(ulong n, byte[] bs, int off)
		{
			Pack.UInt32_To_LE((uint)n, bs, off);
			Pack.UInt32_To_LE((uint)(n >> 32), bs, off + 4);
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x0010D1A0 File Offset: 0x0010B3A0
		internal static byte[] UInt64_To_LE(ulong[] ns)
		{
			byte[] array = new byte[8 * ns.Length];
			Pack.UInt64_To_LE(ns, array, 0);
			return array;
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x0010D1C4 File Offset: 0x0010B3C4
		internal static void UInt64_To_LE(ulong[] ns, byte[] bs, int off)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				Pack.UInt64_To_LE(ns[i], bs, off);
				off += 8;
			}
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x0010D1F0 File Offset: 0x0010B3F0
		internal static void UInt64_To_LE(ulong[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
		{
			for (int i = 0; i < nsLen; i++)
			{
				Pack.UInt64_To_LE(ns[nsOff + i], bs, bsOff);
				bsOff += 8;
			}
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x0010D21C File Offset: 0x0010B41C
		internal static ulong LE_To_UInt64(byte[] bs)
		{
			uint num = Pack.LE_To_UInt32(bs);
			return (ulong)Pack.LE_To_UInt32(bs, 4) << 32 | (ulong)num;
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x0010D240 File Offset: 0x0010B440
		internal static ulong LE_To_UInt64(byte[] bs, int off)
		{
			uint num = Pack.LE_To_UInt32(bs, off);
			return (ulong)Pack.LE_To_UInt32(bs, off + 4) << 32 | (ulong)num;
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x0010D268 File Offset: 0x0010B468
		internal static void LE_To_UInt64(byte[] bs, int off, ulong[] ns)
		{
			for (int i = 0; i < ns.Length; i++)
			{
				ns[i] = Pack.LE_To_UInt64(bs, off);
				off += 8;
			}
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x0010D294 File Offset: 0x0010B494
		internal static void LE_To_UInt64(byte[] bs, int bsOff, ulong[] ns, int nsOff, int nsLen)
		{
			for (int i = 0; i < nsLen; i++)
			{
				ns[nsOff + i] = Pack.LE_To_UInt64(bs, bsOff);
				bsOff += 8;
			}
		}
	}
}
