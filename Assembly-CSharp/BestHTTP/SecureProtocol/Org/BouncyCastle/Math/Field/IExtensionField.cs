using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field
{
	// Token: 0x02000317 RID: 791
	public interface IExtensionField : IFiniteField
	{
		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001DF8 RID: 7672
		IFiniteField Subfield { get; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06001DF9 RID: 7673
		int Degree { get; }
	}
}
