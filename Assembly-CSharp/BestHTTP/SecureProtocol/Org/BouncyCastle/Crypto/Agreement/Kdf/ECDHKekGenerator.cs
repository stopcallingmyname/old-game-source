using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.Kdf
{
	// Token: 0x020005D6 RID: 1494
	public class ECDHKekGenerator : IDerivationFunction
	{
		// Token: 0x06003932 RID: 14642 RVA: 0x00165EEF File Offset: 0x001640EF
		public ECDHKekGenerator(IDigest digest)
		{
			this.kdf = new Kdf2BytesGenerator(digest);
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x00165F04 File Offset: 0x00164104
		public virtual void Init(IDerivationParameters param)
		{
			DHKdfParameters dhkdfParameters = (DHKdfParameters)param;
			this.algorithm = dhkdfParameters.Algorithm;
			this.keySize = dhkdfParameters.KeySize;
			this.z = dhkdfParameters.GetZ();
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06003934 RID: 14644 RVA: 0x00165F3C File Offset: 0x0016413C
		public virtual IDigest Digest
		{
			get
			{
				return this.kdf.Digest;
			}
		}

		// Token: 0x06003935 RID: 14645 RVA: 0x00165F4C File Offset: 0x0016414C
		public virtual int GenerateBytes(byte[] outBytes, int outOff, int len)
		{
			DerSequence derSequence = new DerSequence(new Asn1Encodable[]
			{
				new AlgorithmIdentifier(this.algorithm, DerNull.Instance),
				new DerTaggedObject(true, 2, new DerOctetString(Pack.UInt32_To_BE((uint)this.keySize)))
			});
			this.kdf.Init(new KdfParameters(this.z, derSequence.GetDerEncoded()));
			return this.kdf.GenerateBytes(outBytes, outOff, len);
		}

		// Token: 0x04002584 RID: 9604
		private readonly IDerivationFunction kdf;

		// Token: 0x04002585 RID: 9605
		private DerObjectIdentifier algorithm;

		// Token: 0x04002586 RID: 9606
		private int keySize;

		// Token: 0x04002587 RID: 9607
		private byte[] z;
	}
}
