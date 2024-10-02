using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002C2 RID: 706
	internal class ReasonsMask
	{
		// Token: 0x060019E0 RID: 6624 RVA: 0x000C0D80 File Offset: 0x000BEF80
		internal ReasonsMask(int reasons)
		{
			this._reasons = reasons;
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x000C0D8F File Offset: 0x000BEF8F
		internal ReasonsMask() : this(0)
		{
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x000C0D98 File Offset: 0x000BEF98
		internal void AddReasons(ReasonsMask mask)
		{
			this._reasons |= mask.Reasons.IntValue;
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060019E3 RID: 6627 RVA: 0x000C0DB2 File Offset: 0x000BEFB2
		internal bool IsAllReasons
		{
			get
			{
				return this._reasons == ReasonsMask.AllReasons._reasons;
			}
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x000C0DC6 File Offset: 0x000BEFC6
		internal ReasonsMask Intersect(ReasonsMask mask)
		{
			ReasonsMask reasonsMask = new ReasonsMask();
			reasonsMask.AddReasons(new ReasonsMask(this._reasons & mask.Reasons.IntValue));
			return reasonsMask;
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x000C0DEA File Offset: 0x000BEFEA
		internal bool HasNewReasons(ReasonsMask mask)
		{
			return (this._reasons | (mask.Reasons.IntValue ^ this._reasons)) != 0;
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060019E6 RID: 6630 RVA: 0x000C0E08 File Offset: 0x000BF008
		public ReasonFlags Reasons
		{
			get
			{
				return new ReasonFlags(this._reasons);
			}
		}

		// Token: 0x040018A9 RID: 6313
		private int _reasons;

		// Token: 0x040018AA RID: 6314
		internal static readonly ReasonsMask AllReasons = new ReasonsMask(33023);
	}
}
