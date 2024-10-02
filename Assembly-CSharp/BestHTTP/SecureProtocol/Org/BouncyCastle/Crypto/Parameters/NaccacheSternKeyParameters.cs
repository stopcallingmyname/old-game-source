using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004F1 RID: 1265
	public class NaccacheSternKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x0600307B RID: 12411 RVA: 0x0012795C File Offset: 0x00125B5C
		public NaccacheSternKeyParameters(bool privateKey, BigInteger g, BigInteger n, int lowerSigmaBound) : base(privateKey)
		{
			this.g = g;
			this.n = n;
			this.lowerSigmaBound = lowerSigmaBound;
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x0600307C RID: 12412 RVA: 0x0012797B File Offset: 0x00125B7B
		public BigInteger G
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x0600307D RID: 12413 RVA: 0x00127983 File Offset: 0x00125B83
		public int LowerSigmaBound
		{
			get
			{
				return this.lowerSigmaBound;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x0600307E RID: 12414 RVA: 0x0012798B File Offset: 0x00125B8B
		public BigInteger Modulus
		{
			get
			{
				return this.n;
			}
		}

		// Token: 0x04002012 RID: 8210
		private readonly BigInteger g;

		// Token: 0x04002013 RID: 8211
		private readonly BigInteger n;

		// Token: 0x04002014 RID: 8212
		private readonly int lowerSigmaBound;
	}
}
