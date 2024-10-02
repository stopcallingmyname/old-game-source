using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003CB RID: 971
	public class CipherKeyGenerator
	{
		// Token: 0x0600280A RID: 10250 RVA: 0x0010C9F1 File Offset: 0x0010ABF1
		public CipherKeyGenerator()
		{
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x0010CA00 File Offset: 0x0010AC00
		internal CipherKeyGenerator(int defaultStrength)
		{
			if (defaultStrength < 1)
			{
				throw new ArgumentException("strength must be a positive value", "defaultStrength");
			}
			this.defaultStrength = defaultStrength;
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x0600280C RID: 10252 RVA: 0x0010CA2A File Offset: 0x0010AC2A
		public int DefaultStrength
		{
			get
			{
				return this.defaultStrength;
			}
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x0010CA32 File Offset: 0x0010AC32
		public void Init(KeyGenerationParameters parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			this.uninitialised = false;
			this.engineInit(parameters);
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x0010CA50 File Offset: 0x0010AC50
		protected virtual void engineInit(KeyGenerationParameters parameters)
		{
			this.random = parameters.Random;
			this.strength = (parameters.Strength + 7) / 8;
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x0010CA70 File Offset: 0x0010AC70
		public byte[] GenerateKey()
		{
			if (this.uninitialised)
			{
				if (this.defaultStrength < 1)
				{
					throw new InvalidOperationException("Generator has not been initialised");
				}
				this.uninitialised = false;
				this.engineInit(new KeyGenerationParameters(new SecureRandom(), this.defaultStrength));
			}
			return this.engineGenerateKey();
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x0010CABC File Offset: 0x0010ACBC
		protected virtual byte[] engineGenerateKey()
		{
			return SecureRandom.GetNextBytes(this.random, this.strength);
		}

		// Token: 0x04001B02 RID: 6914
		protected internal SecureRandom random;

		// Token: 0x04001B03 RID: 6915
		protected internal int strength;

		// Token: 0x04001B04 RID: 6916
		private bool uninitialised = true;

		// Token: 0x04001B05 RID: 6917
		private int defaultStrength;
	}
}
