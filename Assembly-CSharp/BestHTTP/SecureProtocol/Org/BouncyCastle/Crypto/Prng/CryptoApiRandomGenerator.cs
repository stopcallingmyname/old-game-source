using System;
using System.Security.Cryptography;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004AE RID: 1198
	public class CryptoApiRandomGenerator : IRandomGenerator
	{
		// Token: 0x06002EEE RID: 12014 RVA: 0x001234EC File Offset: 0x001216EC
		public CryptoApiRandomGenerator() : this(RandomNumberGenerator.Create())
		{
		}

		// Token: 0x06002EEF RID: 12015 RVA: 0x001234F9 File Offset: 0x001216F9
		public CryptoApiRandomGenerator(RandomNumberGenerator rng)
		{
			this.rndProv = rng;
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void AddSeedMaterial(byte[] seed)
		{
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void AddSeedMaterial(long seed)
		{
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x00123508 File Offset: 0x00121708
		public virtual void NextBytes(byte[] bytes)
		{
			this.rndProv.GetBytes(bytes);
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x00123518 File Offset: 0x00121718
		public virtual void NextBytes(byte[] bytes, int start, int len)
		{
			if (start < 0)
			{
				throw new ArgumentException("Start offset cannot be negative", "start");
			}
			if (bytes.Length < start + len)
			{
				throw new ArgumentException("Byte array too small for requested offset and length");
			}
			if (bytes.Length == len && start == 0)
			{
				this.NextBytes(bytes);
				return;
			}
			byte[] array = new byte[len];
			this.NextBytes(array);
			Array.Copy(array, 0, bytes, start, len);
		}

		// Token: 0x04001F5E RID: 8030
		private readonly RandomNumberGenerator rndProv;
	}
}
