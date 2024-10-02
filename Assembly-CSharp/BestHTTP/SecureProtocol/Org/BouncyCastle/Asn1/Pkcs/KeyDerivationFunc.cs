using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006F5 RID: 1781
	public class KeyDerivationFunc : AlgorithmIdentifier
	{
		// Token: 0x06004134 RID: 16692 RVA: 0x00181E3C File Offset: 0x0018003C
		internal KeyDerivationFunc(Asn1Sequence seq) : base(seq)
		{
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x00181CBB File Offset: 0x0017FEBB
		public KeyDerivationFunc(DerObjectIdentifier id, Asn1Encodable parameters) : base(id, parameters)
		{
		}
	}
}
