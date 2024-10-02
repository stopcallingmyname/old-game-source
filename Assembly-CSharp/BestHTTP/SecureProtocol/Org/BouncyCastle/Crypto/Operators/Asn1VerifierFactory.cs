using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000513 RID: 1299
	public class Asn1VerifierFactory : IVerifierFactory
	{
		// Token: 0x06003123 RID: 12579 RVA: 0x001293A8 File Offset: 0x001275A8
		public Asn1VerifierFactory(string algorithm, AsymmetricKeyParameter publicKey)
		{
			if (algorithm == null)
			{
				throw new ArgumentNullException("algorithm");
			}
			if (publicKey == null)
			{
				throw new ArgumentNullException("publicKey");
			}
			if (publicKey.IsPrivate)
			{
				throw new ArgumentException("Key for verifying must be public", "publicKey");
			}
			DerObjectIdentifier algorithmOid = X509Utilities.GetAlgorithmOid(algorithm);
			this.publicKey = publicKey;
			this.algID = X509Utilities.GetSigAlgID(algorithmOid, algorithm);
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x0012940A File Offset: 0x0012760A
		public Asn1VerifierFactory(AlgorithmIdentifier algorithm, AsymmetricKeyParameter publicKey)
		{
			this.publicKey = publicKey;
			this.algID = algorithm;
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06003125 RID: 12581 RVA: 0x00129420 File Offset: 0x00127620
		public object AlgorithmDetails
		{
			get
			{
				return this.algID;
			}
		}

		// Token: 0x06003126 RID: 12582 RVA: 0x00129428 File Offset: 0x00127628
		public IStreamCalculator CreateCalculator()
		{
			return new DefaultVerifierCalculator(SignerUtilities.InitSigner(X509Utilities.GetSignatureName(this.algID), false, this.publicKey, null));
		}

		// Token: 0x04002059 RID: 8281
		private readonly AlgorithmIdentifier algID;

		// Token: 0x0400205A RID: 8282
		private readonly AsymmetricKeyParameter publicKey;
	}
}
