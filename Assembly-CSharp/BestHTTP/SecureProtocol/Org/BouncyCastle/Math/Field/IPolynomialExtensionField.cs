using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field
{
	// Token: 0x0200031A RID: 794
	public interface IPolynomialExtensionField : IExtensionField, IFiniteField
	{
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06001DFE RID: 7678
		IPolynomial MinimalPolynomial { get; }
	}
}
