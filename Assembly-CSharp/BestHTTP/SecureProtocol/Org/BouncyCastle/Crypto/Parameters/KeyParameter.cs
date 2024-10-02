using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004EC RID: 1260
	public class KeyParameter : ICipherParameters
	{
		// Token: 0x06003068 RID: 12392 RVA: 0x001276F4 File Offset: 0x001258F4
		public KeyParameter(byte[] key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.key = (byte[])key.Clone();
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x0012771C File Offset: 0x0012591C
		public KeyParameter(byte[] key, int keyOff, int keyLen)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (keyOff < 0 || keyOff > key.Length)
			{
				throw new ArgumentOutOfRangeException("keyOff");
			}
			if (keyLen < 0 || keyLen > key.Length - keyOff)
			{
				throw new ArgumentOutOfRangeException("keyLen");
			}
			this.key = new byte[keyLen];
			Array.Copy(key, keyOff, this.key, 0, keyLen);
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x00127784 File Offset: 0x00125984
		public byte[] GetKey()
		{
			return (byte[])this.key.Clone();
		}

		// Token: 0x04002009 RID: 8201
		private readonly byte[] key;
	}
}
