using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004F2 RID: 1266
	public class NaccacheSternPrivateKeyParameters : NaccacheSternKeyParameters
	{
		// Token: 0x0600307F RID: 12415 RVA: 0x00127993 File Offset: 0x00125B93
		[Obsolete]
		public NaccacheSternPrivateKeyParameters(BigInteger g, BigInteger n, int lowerSigmaBound, ArrayList smallPrimes, BigInteger phiN) : base(true, g, n, lowerSigmaBound)
		{
			this.smallPrimes = smallPrimes;
			this.phiN = phiN;
		}

		// Token: 0x06003080 RID: 12416 RVA: 0x00127993 File Offset: 0x00125B93
		public NaccacheSternPrivateKeyParameters(BigInteger g, BigInteger n, int lowerSigmaBound, IList smallPrimes, BigInteger phiN) : base(true, g, n, lowerSigmaBound)
		{
			this.smallPrimes = smallPrimes;
			this.phiN = phiN;
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06003081 RID: 12417 RVA: 0x001279AF File Offset: 0x00125BAF
		public BigInteger PhiN
		{
			get
			{
				return this.phiN;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06003082 RID: 12418 RVA: 0x001279B7 File Offset: 0x00125BB7
		[Obsolete("Use 'SmallPrimesList' instead")]
		public ArrayList SmallPrimes
		{
			get
			{
				return new ArrayList(this.smallPrimes);
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06003083 RID: 12419 RVA: 0x001279C4 File Offset: 0x00125BC4
		public IList SmallPrimesList
		{
			get
			{
				return this.smallPrimes;
			}
		}

		// Token: 0x04002015 RID: 8213
		private readonly BigInteger phiN;

		// Token: 0x04002016 RID: 8214
		private readonly IList smallPrimes;
	}
}
