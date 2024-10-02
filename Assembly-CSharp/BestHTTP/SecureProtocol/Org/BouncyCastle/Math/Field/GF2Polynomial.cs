using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field
{
	// Token: 0x02000316 RID: 790
	internal class GF2Polynomial : IPolynomial
	{
		// Token: 0x06001DF3 RID: 7667 RVA: 0x000E1149 File Offset: 0x000DF349
		internal GF2Polynomial(int[] exponents)
		{
			this.exponents = Arrays.Clone(exponents);
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001DF4 RID: 7668 RVA: 0x000E115D File Offset: 0x000DF35D
		public virtual int Degree
		{
			get
			{
				return this.exponents[this.exponents.Length - 1];
			}
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x000E1170 File Offset: 0x000DF370
		public virtual int[] GetExponentsPresent()
		{
			return Arrays.Clone(this.exponents);
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x000E1180 File Offset: 0x000DF380
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			GF2Polynomial gf2Polynomial = obj as GF2Polynomial;
			return gf2Polynomial != null && Arrays.AreEqual(this.exponents, gf2Polynomial.exponents);
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x000E11B0 File Offset: 0x000DF3B0
		public override int GetHashCode()
		{
			return Arrays.GetHashCode(this.exponents);
		}

		// Token: 0x0400194B RID: 6475
		protected readonly int[] exponents;
	}
}
