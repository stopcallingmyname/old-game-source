using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509.Qualified
{
	// Token: 0x020006CD RID: 1741
	public class BiometricData : Asn1Encodable
	{
		// Token: 0x06004030 RID: 16432 RVA: 0x0017DBCC File Offset: 0x0017BDCC
		public static BiometricData GetInstance(object obj)
		{
			if (obj == null || obj is BiometricData)
			{
				return (BiometricData)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new BiometricData(Asn1Sequence.GetInstance(obj));
			}
			throw new ArgumentException("unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x0017DC1C File Offset: 0x0017BE1C
		private BiometricData(Asn1Sequence seq)
		{
			this.typeOfBiometricData = TypeOfBiometricData.GetInstance(seq[0]);
			this.hashAlgorithm = AlgorithmIdentifier.GetInstance(seq[1]);
			this.biometricDataHash = Asn1OctetString.GetInstance(seq[2]);
			if (seq.Count > 3)
			{
				this.sourceDataUri = DerIA5String.GetInstance(seq[3]);
			}
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x0017DC80 File Offset: 0x0017BE80
		public BiometricData(TypeOfBiometricData typeOfBiometricData, AlgorithmIdentifier hashAlgorithm, Asn1OctetString biometricDataHash, DerIA5String sourceDataUri)
		{
			this.typeOfBiometricData = typeOfBiometricData;
			this.hashAlgorithm = hashAlgorithm;
			this.biometricDataHash = biometricDataHash;
			this.sourceDataUri = sourceDataUri;
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x0017DCA5 File Offset: 0x0017BEA5
		public BiometricData(TypeOfBiometricData typeOfBiometricData, AlgorithmIdentifier hashAlgorithm, Asn1OctetString biometricDataHash)
		{
			this.typeOfBiometricData = typeOfBiometricData;
			this.hashAlgorithm = hashAlgorithm;
			this.biometricDataHash = biometricDataHash;
			this.sourceDataUri = null;
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06004034 RID: 16436 RVA: 0x0017DCC9 File Offset: 0x0017BEC9
		public TypeOfBiometricData TypeOfBiometricData
		{
			get
			{
				return this.typeOfBiometricData;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06004035 RID: 16437 RVA: 0x0017DCD1 File Offset: 0x0017BED1
		public AlgorithmIdentifier HashAlgorithm
		{
			get
			{
				return this.hashAlgorithm;
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06004036 RID: 16438 RVA: 0x0017DCD9 File Offset: 0x0017BED9
		public Asn1OctetString BiometricDataHash
		{
			get
			{
				return this.biometricDataHash;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06004037 RID: 16439 RVA: 0x0017DCE1 File Offset: 0x0017BEE1
		public DerIA5String SourceDataUri
		{
			get
			{
				return this.sourceDataUri;
			}
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x0017DCEC File Offset: 0x0017BEEC
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.typeOfBiometricData,
				this.hashAlgorithm,
				this.biometricDataHash
			});
			if (this.sourceDataUri != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.sourceDataUri
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x040028DE RID: 10462
		private readonly TypeOfBiometricData typeOfBiometricData;

		// Token: 0x040028DF RID: 10463
		private readonly AlgorithmIdentifier hashAlgorithm;

		// Token: 0x040028E0 RID: 10464
		private readonly Asn1OctetString biometricDataHash;

		// Token: 0x040028E1 RID: 10465
		private readonly DerIA5String sourceDataUri;
	}
}
