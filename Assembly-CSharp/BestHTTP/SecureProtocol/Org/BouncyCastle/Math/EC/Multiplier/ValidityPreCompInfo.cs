using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x02000347 RID: 839
	internal class ValidityPreCompInfo : PreCompInfo
	{
		// Token: 0x0600205D RID: 8285 RVA: 0x000EF9E7 File Offset: 0x000EDBE7
		internal bool HasFailed()
		{
			return this.failed;
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x000EF9EF File Offset: 0x000EDBEF
		internal void ReportFailed()
		{
			this.failed = true;
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x000EF9F8 File Offset: 0x000EDBF8
		internal bool HasCurveEquationPassed()
		{
			return this.curveEquationPassed;
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x000EFA00 File Offset: 0x000EDC00
		internal void ReportCurveEquationPassed()
		{
			this.curveEquationPassed = true;
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x000EFA09 File Offset: 0x000EDC09
		internal bool HasOrderPassed()
		{
			return this.orderPassed;
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x000EFA11 File Offset: 0x000EDC11
		internal void ReportOrderPassed()
		{
			this.orderPassed = true;
		}

		// Token: 0x040019E3 RID: 6627
		internal static readonly string PRECOMP_NAME = "bc_validity";

		// Token: 0x040019E4 RID: 6628
		private bool failed;

		// Token: 0x040019E5 RID: 6629
		private bool curveEquationPassed;

		// Token: 0x040019E6 RID: 6630
		private bool orderPassed;
	}
}
