using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x0200034C RID: 844
	public class WTauNafPreCompInfo : PreCompInfo
	{
		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06002085 RID: 8325 RVA: 0x000F0339 File Offset: 0x000EE539
		// (set) Token: 0x06002086 RID: 8326 RVA: 0x000F0341 File Offset: 0x000EE541
		public virtual AbstractF2mPoint[] PreComp
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

		// Token: 0x040019EE RID: 6638
		protected AbstractF2mPoint[] m_preComp;
	}
}
