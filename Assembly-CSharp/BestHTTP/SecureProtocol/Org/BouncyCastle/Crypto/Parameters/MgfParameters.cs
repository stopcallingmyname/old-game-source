using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004ED RID: 1261
	public class MgfParameters : IDerivationParameters
	{
		// Token: 0x0600306B RID: 12395 RVA: 0x00127796 File Offset: 0x00125996
		public MgfParameters(byte[] seed) : this(seed, 0, seed.Length)
		{
		}

		// Token: 0x0600306C RID: 12396 RVA: 0x001277A3 File Offset: 0x001259A3
		public MgfParameters(byte[] seed, int off, int len)
		{
			this.seed = new byte[len];
			Array.Copy(seed, off, this.seed, 0, len);
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x001277C6 File Offset: 0x001259C6
		public byte[] GetSeed()
		{
			return (byte[])this.seed.Clone();
		}

		// Token: 0x0400200A RID: 8202
		private readonly byte[] seed;
	}
}
