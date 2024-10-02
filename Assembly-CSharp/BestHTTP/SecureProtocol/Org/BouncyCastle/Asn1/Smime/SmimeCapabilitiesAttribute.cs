using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Smime
{
	// Token: 0x020006E2 RID: 1762
	public class SmimeCapabilitiesAttribute : AttributeX509
	{
		// Token: 0x060040C1 RID: 16577 RVA: 0x00180666 File Offset: 0x0017E866
		public SmimeCapabilitiesAttribute(SmimeCapabilityVector capabilities) : base(SmimeAttributes.SmimeCapabilities, new DerSet(new DerSequence(capabilities.ToAsn1EncodableVector())))
		{
		}
	}
}
