using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000547 RID: 1351
	public class DesKeyGenerator : CipherKeyGenerator
	{
		// Token: 0x06003330 RID: 13104 RVA: 0x00133649 File Offset: 0x00131849
		public DesKeyGenerator()
		{
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x00133651 File Offset: 0x00131851
		internal DesKeyGenerator(int defaultStrength) : base(defaultStrength)
		{
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x0013365C File Offset: 0x0013185C
		protected override void engineInit(KeyGenerationParameters parameters)
		{
			base.engineInit(parameters);
			if (this.strength == 0 || this.strength == 7)
			{
				this.strength = 8;
				return;
			}
			if (this.strength != 8)
			{
				throw new ArgumentException("DES key must be " + 64 + " bits long.");
			}
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x001336B0 File Offset: 0x001318B0
		protected override byte[] engineGenerateKey()
		{
			byte[] array = new byte[8];
			do
			{
				this.random.NextBytes(array);
				DesParameters.SetOddParity(array);
			}
			while (DesParameters.IsWeakKey(array, 0));
			return array;
		}
	}
}
