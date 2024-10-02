using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004C2 RID: 1218
	public class DesEdeParameters : DesParameters
	{
		// Token: 0x06002F68 RID: 12136 RVA: 0x00125758 File Offset: 0x00123958
		private static byte[] FixKey(byte[] key, int keyOff, int keyLen)
		{
			byte[] array = new byte[24];
			if (keyLen != 16)
			{
				if (keyLen != 24)
				{
					throw new ArgumentException("Bad length for DESede key: " + keyLen, "keyLen");
				}
				Array.Copy(key, keyOff, array, 0, 24);
			}
			else
			{
				Array.Copy(key, keyOff, array, 0, 16);
				Array.Copy(key, keyOff, array, 16, 8);
			}
			if (DesEdeParameters.IsWeakKey(array))
			{
				throw new ArgumentException("attempt to create weak DESede key");
			}
			return array;
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x001257CD File Offset: 0x001239CD
		public DesEdeParameters(byte[] key) : base(DesEdeParameters.FixKey(key, 0, key.Length))
		{
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x001257DF File Offset: 0x001239DF
		public DesEdeParameters(byte[] key, int keyOff, int keyLen) : base(DesEdeParameters.FixKey(key, keyOff, keyLen))
		{
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x001257F0 File Offset: 0x001239F0
		public static bool IsWeakKey(byte[] key, int offset, int length)
		{
			for (int i = offset; i < length; i += 8)
			{
				if (DesParameters.IsWeakKey(key, i))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x00125815 File Offset: 0x00123A15
		public new static bool IsWeakKey(byte[] key, int offset)
		{
			return DesEdeParameters.IsWeakKey(key, offset, key.Length - offset);
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x00125823 File Offset: 0x00123A23
		public new static bool IsWeakKey(byte[] key)
		{
			return DesEdeParameters.IsWeakKey(key, 0, key.Length);
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x0012582F File Offset: 0x00123A2F
		public static bool IsRealEdeKey(byte[] key, int offset)
		{
			if (key.Length != 16)
			{
				return DesEdeParameters.IsReal3Key(key, offset);
			}
			return DesEdeParameters.IsReal2Key(key, offset);
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x00125848 File Offset: 0x00123A48
		public static bool IsReal2Key(byte[] key, int offset)
		{
			bool flag = false;
			for (int num = offset; num != offset + 8; num++)
			{
				flag |= (key[num] != key[num + 8]);
			}
			return flag;
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x00125878 File Offset: 0x00123A78
		public static bool IsReal3Key(byte[] key, int offset)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int num = offset; num != offset + 8; num++)
			{
				flag |= (key[num] != key[num + 8]);
				flag2 |= (key[num] != key[num + 16]);
				flag3 |= (key[num + 8] != key[num + 16]);
			}
			return flag && flag2 && flag3;
		}

		// Token: 0x04001FAC RID: 8108
		public const int DesEdeKeyLength = 24;
	}
}
