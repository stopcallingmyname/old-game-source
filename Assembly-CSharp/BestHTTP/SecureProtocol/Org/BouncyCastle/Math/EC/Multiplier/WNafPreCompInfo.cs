using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x02000349 RID: 841
	public class WNafPreCompInfo : PreCompInfo
	{
		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x000EFB78 File Offset: 0x000EDD78
		// (set) Token: 0x06002069 RID: 8297 RVA: 0x000EFB80 File Offset: 0x000EDD80
		public virtual ECPoint[] PreComp
		{
			get
			{
				return this.m_preComp;
			}
			set
			{
				this.m_preComp = value;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x0600206A RID: 8298 RVA: 0x000EFB89 File Offset: 0x000EDD89
		// (set) Token: 0x0600206B RID: 8299 RVA: 0x000EFB91 File Offset: 0x000EDD91
		public virtual ECPoint[] PreCompNeg
		{
			get
			{
				return this.m_preCompNeg;
			}
			set
			{
				this.m_preCompNeg = value;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x0600206C RID: 8300 RVA: 0x000EFB9A File Offset: 0x000EDD9A
		// (set) Token: 0x0600206D RID: 8301 RVA: 0x000EFBA2 File Offset: 0x000EDDA2
		public virtual ECPoint Twice
		{
			get
			{
				return this.m_twice;
			}
			set
			{
				this.m_twice = value;
			}
		}

		// Token: 0x040019E7 RID: 6631
		protected ECPoint[] m_preComp;

		// Token: 0x040019E8 RID: 6632
		protected ECPoint[] m_preCompNeg;

		// Token: 0x040019E9 RID: 6633
		protected ECPoint m_twice;
	}
}
