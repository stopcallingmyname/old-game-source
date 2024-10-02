using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x02000708 RID: 1800
	public class CertID : Asn1Encodable
	{
		// Token: 0x060041C9 RID: 16841 RVA: 0x0018410A File Offset: 0x0018230A
		public static CertID GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return CertID.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x00184118 File Offset: 0x00182318
		public static CertID GetInstance(object obj)
		{
			if (obj == null || obj is CertID)
			{
				return (CertID)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CertID((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060041CB RID: 16843 RVA: 0x00184165 File Offset: 0x00182365
		public CertID(AlgorithmIdentifier hashAlgorithm, Asn1OctetString issuerNameHash, Asn1OctetString issuerKeyHash, DerInteger serialNumber)
		{
			this.hashAlgorithm = hashAlgorithm;
			this.issuerNameHash = issuerNameHash;
			this.issuerKeyHash = issuerKeyHash;
			this.serialNumber = serialNumber;
		}

		// Token: 0x060041CC RID: 16844 RVA: 0x0018418C File Offset: 0x0018238C
		private CertID(Asn1Sequence seq)
		{
			if (seq.Count != 4)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.hashAlgorithm = AlgorithmIdentifier.GetInstance(seq[0]);
			this.issuerNameHash = Asn1OctetString.GetInstance(seq[1]);
			this.issuerKeyHash = Asn1OctetString.GetInstance(seq[2]);
			this.serialNumber = DerInteger.GetInstance(seq[3]);
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x060041CD RID: 16845 RVA: 0x00184200 File Offset: 0x00182400
		public AlgorithmIdentifier HashAlgorithm
		{
			get
			{
				return this.hashAlgorithm;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x060041CE RID: 16846 RVA: 0x00184208 File Offset: 0x00182408
		public Asn1OctetString IssuerNameHash
		{
			get
			{
				return this.issuerNameHash;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x060041CF RID: 16847 RVA: 0x00184210 File Offset: 0x00182410
		public Asn1OctetString IssuerKeyHash
		{
			get
			{
				return this.issuerKeyHash;
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x060041D0 RID: 16848 RVA: 0x00184218 File Offset: 0x00182418
		public DerInteger SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
		}

		// Token: 0x060041D1 RID: 16849 RVA: 0x00184220 File Offset: 0x00182420
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.hashAlgorithm,
				this.issuerNameHash,
				this.issuerKeyHash,
				this.serialNumber
			});
		}

		// Token: 0x04002AA4 RID: 10916
		private readonly AlgorithmIdentifier hashAlgorithm;

		// Token: 0x04002AA5 RID: 10917
		private readonly Asn1OctetString issuerNameHash;

		// Token: 0x04002AA6 RID: 10918
		private readonly Asn1OctetString issuerKeyHash;

		// Token: 0x04002AA7 RID: 10919
		private readonly DerInteger serialNumber;
	}
}
