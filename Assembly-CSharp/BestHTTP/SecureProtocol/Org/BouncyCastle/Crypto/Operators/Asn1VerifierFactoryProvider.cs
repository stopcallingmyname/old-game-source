using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000514 RID: 1300
	public class Asn1VerifierFactoryProvider : IVerifierFactoryProvider
	{
		// Token: 0x06003127 RID: 12583 RVA: 0x00129447 File Offset: 0x00127647
		public Asn1VerifierFactoryProvider(AsymmetricKeyParameter publicKey)
		{
			this.publicKey = publicKey;
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x00129456 File Offset: 0x00127656
		public IVerifierFactory CreateVerifierFactory(object algorithmDetails)
		{
			return new Asn1VerifierFactory((AlgorithmIdentifier)algorithmDetails, this.publicKey);
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06003129 RID: 12585 RVA: 0x0012939F File Offset: 0x0012759F
		public IEnumerable SignatureAlgNames
		{
			get
			{
				return X509Utilities.GetAlgNames();
			}
		}

		// Token: 0x0400205B RID: 8283
		private readonly AsymmetricKeyParameter publicKey;
	}
}
