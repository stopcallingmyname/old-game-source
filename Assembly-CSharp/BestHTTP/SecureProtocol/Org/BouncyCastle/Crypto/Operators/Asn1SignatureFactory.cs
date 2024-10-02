using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000512 RID: 1298
	public class Asn1SignatureFactory : ISignatureFactory
	{
		// Token: 0x0600311E RID: 12574 RVA: 0x001292FD File Offset: 0x001274FD
		public Asn1SignatureFactory(string algorithm, AsymmetricKeyParameter privateKey) : this(algorithm, privateKey, null)
		{
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x00129308 File Offset: 0x00127508
		public Asn1SignatureFactory(string algorithm, AsymmetricKeyParameter privateKey, SecureRandom random)
		{
			if (algorithm == null)
			{
				throw new ArgumentNullException("algorithm");
			}
			if (privateKey == null)
			{
				throw new ArgumentNullException("privateKey");
			}
			if (!privateKey.IsPrivate)
			{
				throw new ArgumentException("Key for signing must be private", "privateKey");
			}
			DerObjectIdentifier algorithmOid = X509Utilities.GetAlgorithmOid(algorithm);
			this.algorithm = algorithm;
			this.privateKey = privateKey;
			this.random = random;
			this.algID = X509Utilities.GetSigAlgID(algorithmOid, algorithm);
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06003120 RID: 12576 RVA: 0x00129378 File Offset: 0x00127578
		public object AlgorithmDetails
		{
			get
			{
				return this.algID;
			}
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x00129380 File Offset: 0x00127580
		public IStreamCalculator CreateCalculator()
		{
			return new DefaultSignatureCalculator(SignerUtilities.InitSigner(this.algorithm, true, this.privateKey, this.random));
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06003122 RID: 12578 RVA: 0x0012939F File Offset: 0x0012759F
		public static IEnumerable SignatureAlgNames
		{
			get
			{
				return X509Utilities.GetAlgNames();
			}
		}

		// Token: 0x04002055 RID: 8277
		private readonly AlgorithmIdentifier algID;

		// Token: 0x04002056 RID: 8278
		private readonly string algorithm;

		// Token: 0x04002057 RID: 8279
		private readonly AsymmetricKeyParameter privateKey;

		// Token: 0x04002058 RID: 8280
		private readonly SecureRandom random;
	}
}
