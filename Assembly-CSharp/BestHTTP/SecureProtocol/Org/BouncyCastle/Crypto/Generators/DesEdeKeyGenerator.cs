using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000546 RID: 1350
	public class DesEdeKeyGenerator : DesKeyGenerator
	{
		// Token: 0x0600332C RID: 13100 RVA: 0x00133546 File Offset: 0x00131746
		public DesEdeKeyGenerator()
		{
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x0013354E File Offset: 0x0013174E
		internal DesEdeKeyGenerator(int defaultStrength) : base(defaultStrength)
		{
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x00133558 File Offset: 0x00131758
		protected override void engineInit(KeyGenerationParameters parameters)
		{
			this.random = parameters.Random;
			this.strength = (parameters.Strength + 7) / 8;
			if (this.strength == 0 || this.strength == 21)
			{
				this.strength = 24;
				return;
			}
			if (this.strength == 14)
			{
				this.strength = 16;
				return;
			}
			if (this.strength != 24 && this.strength != 16)
			{
				throw new ArgumentException(string.Concat(new object[]
				{
					"DESede key must be ",
					192,
					" or ",
					128,
					" bits long."
				}));
			}
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x00133608 File Offset: 0x00131808
		protected override byte[] engineGenerateKey()
		{
			byte[] array = new byte[this.strength];
			do
			{
				this.random.NextBytes(array);
				DesParameters.SetOddParity(array);
			}
			while (DesEdeParameters.IsWeakKey(array, 0, array.Length) || !DesEdeParameters.IsRealEdeKey(array, 0));
			return array;
		}
	}
}
