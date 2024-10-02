using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x0200033D RID: 829
	public class FixedPointPreCompInfo : PreCompInfo
	{
		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06002042 RID: 8258 RVA: 0x000EF62F File Offset: 0x000ED82F
		// (set) Token: 0x06002043 RID: 8259 RVA: 0x000EF637 File Offset: 0x000ED837
		public virtual ECLookupTable LookupTable
		{
			get
			{
				return this.m_lookupTable;
			}
			set
			{
				this.m_lookupTable = value;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06002044 RID: 8260 RVA: 0x000EF640 File Offset: 0x000ED840
		// (set) Token: 0x06002045 RID: 8261 RVA: 0x000EF648 File Offset: 0x000ED848
		public virtual ECPoint Offset
		{
			get
			{
				return this.m_offset;
			}
			set
			{
				this.m_offset = value;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06002046 RID: 8262 RVA: 0x000EF651 File Offset: 0x000ED851
		// (set) Token: 0x06002047 RID: 8263 RVA: 0x000EF659 File Offset: 0x000ED859
		public virtual int Width
		{
			get
			{
				return this.m_width;
			}
			set
			{
				this.m_width = value;
			}
		}

		// Token: 0x040019DB RID: 6619
		protected ECPoint m_offset;

		// Token: 0x040019DC RID: 6620
		protected ECLookupTable m_lookupTable;

		// Token: 0x040019DD RID: 6621
		protected int m_width = -1;
	}
}
