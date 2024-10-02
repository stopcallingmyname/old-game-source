using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.IsisMtt.Ocsp
{
	// Token: 0x0200072E RID: 1838
	public class CertHash : Asn1Encodable
	{
		// Token: 0x060042AF RID: 17071 RVA: 0x001875C0 File Offset: 0x001857C0
		public static CertHash GetInstance(object obj)
		{
			if (obj == null || obj is CertHash)
			{
				return (CertHash)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CertHash((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060042B0 RID: 17072 RVA: 0x00187610 File Offset: 0x00185810
		private CertHash(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.hashAlgorithm = AlgorithmIdentifier.GetInstance(seq[0]);
			this.certificateHash = Asn1OctetString.GetInstance(seq[1]).GetOctets();
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x00187670 File Offset: 0x00185870
		public CertHash(AlgorithmIdentifier hashAlgorithm, byte[] certificateHash)
		{
			if (hashAlgorithm == null)
			{
				throw new ArgumentNullException("hashAlgorithm");
			}
			if (certificateHash == null)
			{
				throw new ArgumentNullException("certificateHash");
			}
			this.hashAlgorithm = hashAlgorithm;
			this.certificateHash = (byte[])certificateHash.Clone();
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x060042B2 RID: 17074 RVA: 0x001876AC File Offset: 0x001858AC
		public AlgorithmIdentifier HashAlgorithm
		{
			get
			{
				return this.hashAlgorithm;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x060042B3 RID: 17075 RVA: 0x001876B4 File Offset: 0x001858B4
		public byte[] CertificateHash
		{
			get
			{
				return (byte[])this.certificateHash.Clone();
			}
		}

		// Token: 0x060042B4 RID: 17076 RVA: 0x001876C6 File Offset: 0x001858C6
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.hashAlgorithm,
				new DerOctetString(this.certificateHash)
			});
		}

		// Token: 0x04002B8E RID: 11150
		private readonly AlgorithmIdentifier hashAlgorithm;

		// Token: 0x04002B8F RID: 11151
		private readonly byte[] certificateHash;
	}
}
