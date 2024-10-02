using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.JPake
{
	// Token: 0x020005D8 RID: 1496
	public class JPakePrimeOrderGroup
	{
		// Token: 0x06003942 RID: 14658 RVA: 0x001666DB File Offset: 0x001648DB
		public JPakePrimeOrderGroup(BigInteger p, BigInteger q, BigInteger g) : this(p, q, g, false)
		{
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x001666E8 File Offset: 0x001648E8
		public JPakePrimeOrderGroup(BigInteger p, BigInteger q, BigInteger g, bool skipChecks)
		{
			JPakeUtilities.ValidateNotNull(p, "p");
			JPakeUtilities.ValidateNotNull(q, "q");
			JPakeUtilities.ValidateNotNull(g, "g");
			if (!skipChecks)
			{
				if (!p.Subtract(JPakeUtilities.One).Mod(q).Equals(JPakeUtilities.Zero))
				{
					throw new ArgumentException("p-1 must be evenly divisible by q");
				}
				if (g.CompareTo(BigInteger.Two) == -1 || g.CompareTo(p.Subtract(JPakeUtilities.One)) == 1)
				{
					throw new ArgumentException("g must be in [2, p-1]");
				}
				if (!g.ModPow(q, p).Equals(JPakeUtilities.One))
				{
					throw new ArgumentException("g^q mod p must equal 1");
				}
				if (!p.IsProbablePrime(20))
				{
					throw new ArgumentException("p must be prime");
				}
				if (!q.IsProbablePrime(20))
				{
					throw new ArgumentException("q must be prime");
				}
			}
			this.p = p;
			this.q = q;
			this.g = g;
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06003944 RID: 14660 RVA: 0x001667D6 File Offset: 0x001649D6
		public virtual BigInteger P
		{
			get
			{
				return this.p;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06003945 RID: 14661 RVA: 0x001667DE File Offset: 0x001649DE
		public virtual BigInteger Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06003946 RID: 14662 RVA: 0x001667E6 File Offset: 0x001649E6
		public virtual BigInteger G
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x040025A0 RID: 9632
		private readonly BigInteger p;

		// Token: 0x040025A1 RID: 9633
		private readonly BigInteger q;

		// Token: 0x040025A2 RID: 9634
		private readonly BigInteger g;
	}
}
