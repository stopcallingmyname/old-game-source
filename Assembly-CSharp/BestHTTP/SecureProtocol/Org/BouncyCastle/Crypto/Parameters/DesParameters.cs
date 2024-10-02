using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004C3 RID: 1219
	public class DesParameters : KeyParameter
	{
		// Token: 0x06002F71 RID: 12145 RVA: 0x001258D2 File Offset: 0x00123AD2
		public DesParameters(byte[] key) : base(key)
		{
			if (DesParameters.IsWeakKey(key))
			{
				throw new ArgumentException("attempt to create weak DES key");
			}
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x001258EE File Offset: 0x00123AEE
		public DesParameters(byte[] key, int keyOff, int keyLen) : base(key, keyOff, keyLen)
		{
			if (DesParameters.IsWeakKey(key, keyOff))
			{
				throw new ArgumentException("attempt to create weak DES key");
			}
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x00125910 File Offset: 0x00123B10
		public static bool IsWeakKey(byte[] key, int offset)
		{
			if (key.Length - offset < 8)
			{
				throw new ArgumentException("key material too short.");
			}
			for (int i = 0; i < 16; i++)
			{
				bool flag = false;
				for (int j = 0; j < 8; j++)
				{
					if (key[j + offset] != DesParameters.DES_weak_keys[i * 8 + j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x00125967 File Offset: 0x00123B67
		public static bool IsWeakKey(byte[] key)
		{
			return DesParameters.IsWeakKey(key, 0);
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x00125970 File Offset: 0x00123B70
		public static byte SetOddParity(byte b)
		{
			uint num = (uint)(b ^ 1);
			num ^= num >> 4;
			num ^= num >> 2;
			num ^= num >> 1;
			num &= 1U;
			return (byte)((uint)b ^ num);
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x0012599C File Offset: 0x00123B9C
		public static void SetOddParity(byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i++)
			{
				bytes[i] = DesParameters.SetOddParity(bytes[i]);
			}
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x001259C4 File Offset: 0x00123BC4
		public static void SetOddParity(byte[] bytes, int off, int len)
		{
			for (int i = 0; i < len; i++)
			{
				bytes[off + i] = DesParameters.SetOddParity(bytes[off + i]);
			}
		}

		// Token: 0x04001FAD RID: 8109
		public const int DesKeyLength = 8;

		// Token: 0x04001FAE RID: 8110
		private const int N_DES_WEAK_KEYS = 16;

		// Token: 0x04001FAF RID: 8111
		private static readonly byte[] DES_weak_keys = new byte[]
		{
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			31,
			31,
			31,
			31,
			14,
			14,
			14,
			14,
			224,
			224,
			224,
			224,
			241,
			241,
			241,
			241,
			254,
			254,
			254,
			254,
			254,
			254,
			254,
			254,
			1,
			254,
			1,
			254,
			1,
			254,
			1,
			254,
			31,
			224,
			31,
			224,
			14,
			241,
			14,
			241,
			1,
			224,
			1,
			224,
			1,
			241,
			1,
			241,
			31,
			254,
			31,
			254,
			14,
			254,
			14,
			254,
			1,
			31,
			1,
			31,
			1,
			14,
			1,
			14,
			224,
			254,
			224,
			254,
			241,
			254,
			241,
			254,
			254,
			1,
			254,
			1,
			254,
			1,
			254,
			1,
			224,
			31,
			224,
			31,
			241,
			14,
			241,
			14,
			224,
			1,
			224,
			1,
			241,
			1,
			241,
			1,
			254,
			31,
			254,
			31,
			254,
			14,
			254,
			14,
			31,
			1,
			31,
			1,
			14,
			1,
			14,
			1,
			254,
			224,
			254,
			224,
			254,
			241,
			254,
			241
		};
	}
}
