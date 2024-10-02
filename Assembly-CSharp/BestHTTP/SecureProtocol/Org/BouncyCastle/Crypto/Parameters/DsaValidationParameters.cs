using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004D0 RID: 1232
	public class DsaValidationParameters
	{
		// Token: 0x06002FC9 RID: 12233 RVA: 0x00126350 File Offset: 0x00124550
		public DsaValidationParameters(byte[] seed, int counter) : this(seed, counter, -1)
		{
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x0012635B File Offset: 0x0012455B
		public DsaValidationParameters(byte[] seed, int counter, int usageIndex)
		{
			if (seed == null)
			{
				throw new ArgumentNullException("seed");
			}
			this.seed = (byte[])seed.Clone();
			this.counter = counter;
			this.usageIndex = usageIndex;
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x00126390 File Offset: 0x00124590
		public virtual byte[] GetSeed()
		{
			return (byte[])this.seed.Clone();
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06002FCC RID: 12236 RVA: 0x001263A2 File Offset: 0x001245A2
		public virtual int Counter
		{
			get
			{
				return this.counter;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06002FCD RID: 12237 RVA: 0x001263AA File Offset: 0x001245AA
		public virtual int UsageIndex
		{
			get
			{
				return this.usageIndex;
			}
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x001263B4 File Offset: 0x001245B4
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DsaValidationParameters dsaValidationParameters = obj as DsaValidationParameters;
			return dsaValidationParameters != null && this.Equals(dsaValidationParameters);
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x001263DA File Offset: 0x001245DA
		protected virtual bool Equals(DsaValidationParameters other)
		{
			return this.counter == other.counter && Arrays.AreEqual(this.seed, other.seed);
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x00126400 File Offset: 0x00124600
		public override int GetHashCode()
		{
			return this.counter.GetHashCode() ^ Arrays.GetHashCode(this.seed);
		}

		// Token: 0x04001FCE RID: 8142
		private readonly byte[] seed;

		// Token: 0x04001FCF RID: 8143
		private readonly int counter;

		// Token: 0x04001FD0 RID: 8144
		private readonly int usageIndex;
	}
}
