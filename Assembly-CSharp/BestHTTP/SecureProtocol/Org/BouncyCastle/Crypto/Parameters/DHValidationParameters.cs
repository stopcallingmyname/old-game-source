using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004C9 RID: 1225
	public class DHValidationParameters
	{
		// Token: 0x06002FA1 RID: 12193 RVA: 0x00125F0C File Offset: 0x0012410C
		public DHValidationParameters(byte[] seed, int counter)
		{
			if (seed == null)
			{
				throw new ArgumentNullException("seed");
			}
			this.seed = (byte[])seed.Clone();
			this.counter = counter;
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x00125F3A File Offset: 0x0012413A
		public byte[] GetSeed()
		{
			return (byte[])this.seed.Clone();
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06002FA3 RID: 12195 RVA: 0x00125F4C File Offset: 0x0012414C
		public int Counter
		{
			get
			{
				return this.counter;
			}
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x00125F54 File Offset: 0x00124154
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DHValidationParameters dhvalidationParameters = obj as DHValidationParameters;
			return dhvalidationParameters != null && this.Equals(dhvalidationParameters);
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x00125F7A File Offset: 0x0012417A
		protected bool Equals(DHValidationParameters other)
		{
			return this.counter == other.counter && Arrays.AreEqual(this.seed, other.seed);
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x00125FA0 File Offset: 0x001241A0
		public override int GetHashCode()
		{
			return this.counter.GetHashCode() ^ Arrays.GetHashCode(this.seed);
		}

		// Token: 0x04001FBD RID: 8125
		private readonly byte[] seed;

		// Token: 0x04001FBE RID: 8126
		private readonly int counter;
	}
}
