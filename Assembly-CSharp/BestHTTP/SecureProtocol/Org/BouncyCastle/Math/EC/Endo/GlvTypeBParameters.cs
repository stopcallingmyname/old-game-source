using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Endo
{
	// Token: 0x02000352 RID: 850
	public class GlvTypeBParameters
	{
		// Token: 0x06002094 RID: 8340 RVA: 0x000F053C File Offset: 0x000EE73C
		public GlvTypeBParameters(BigInteger beta, BigInteger lambda, BigInteger[] v1, BigInteger[] v2, BigInteger g1, BigInteger g2, int bits)
		{
			this.m_beta = beta;
			this.m_lambda = lambda;
			this.m_v1 = v1;
			this.m_v2 = v2;
			this.m_g1 = g1;
			this.m_g2 = g2;
			this.m_bits = bits;
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06002095 RID: 8341 RVA: 0x000F0579 File Offset: 0x000EE779
		public virtual BigInteger Beta
		{
			get
			{
				return this.m_beta;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06002096 RID: 8342 RVA: 0x000F0581 File Offset: 0x000EE781
		public virtual BigInteger Lambda
		{
			get
			{
				return this.m_lambda;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06002097 RID: 8343 RVA: 0x000F0589 File Offset: 0x000EE789
		public virtual BigInteger[] V1
		{
			get
			{
				return this.m_v1;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06002098 RID: 8344 RVA: 0x000F0591 File Offset: 0x000EE791
		public virtual BigInteger[] V2
		{
			get
			{
				return this.m_v2;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06002099 RID: 8345 RVA: 0x000F0599 File Offset: 0x000EE799
		public virtual BigInteger G1
		{
			get
			{
				return this.m_g1;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x0600209A RID: 8346 RVA: 0x000F05A1 File Offset: 0x000EE7A1
		public virtual BigInteger G2
		{
			get
			{
				return this.m_g2;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x0600209B RID: 8347 RVA: 0x000F05A9 File Offset: 0x000EE7A9
		public virtual int Bits
		{
			get
			{
				return this.m_bits;
			}
		}

		// Token: 0x040019F2 RID: 6642
		protected readonly BigInteger m_beta;

		// Token: 0x040019F3 RID: 6643
		protected readonly BigInteger m_lambda;

		// Token: 0x040019F4 RID: 6644
		protected readonly BigInteger[] m_v1;

		// Token: 0x040019F5 RID: 6645
		protected readonly BigInteger[] m_v2;

		// Token: 0x040019F6 RID: 6646
		protected readonly BigInteger m_g1;

		// Token: 0x040019F7 RID: 6647
		protected readonly BigInteger m_g2;

		// Token: 0x040019F8 RID: 6648
		protected readonly int m_bits;
	}
}
