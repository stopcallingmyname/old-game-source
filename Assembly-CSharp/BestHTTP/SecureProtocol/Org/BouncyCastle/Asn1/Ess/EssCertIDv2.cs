using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ess
{
	// Token: 0x0200073C RID: 1852
	public class EssCertIDv2 : Asn1Encodable
	{
		// Token: 0x060042FF RID: 17151 RVA: 0x001889B8 File Offset: 0x00186BB8
		public static EssCertIDv2 GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			EssCertIDv2 essCertIDv = obj as EssCertIDv2;
			if (essCertIDv != null)
			{
				return essCertIDv;
			}
			return new EssCertIDv2(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x06004300 RID: 17152 RVA: 0x001889E4 File Offset: 0x00186BE4
		private EssCertIDv2(Asn1Sequence seq)
		{
			if (seq.Count > 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			int num = 0;
			if (seq[0] is Asn1OctetString)
			{
				this.hashAlgorithm = EssCertIDv2.DefaultAlgID;
			}
			else
			{
				this.hashAlgorithm = AlgorithmIdentifier.GetInstance(seq[num++].ToAsn1Object());
			}
			this.certHash = Asn1OctetString.GetInstance(seq[num++].ToAsn1Object()).GetOctets();
			if (seq.Count > num)
			{
				this.issuerSerial = IssuerSerial.GetInstance(Asn1Sequence.GetInstance(seq[num].ToAsn1Object()));
			}
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x00188A9D File Offset: 0x00186C9D
		public EssCertIDv2(byte[] certHash) : this(null, certHash, null)
		{
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x00188AA8 File Offset: 0x00186CA8
		public EssCertIDv2(AlgorithmIdentifier algId, byte[] certHash) : this(algId, certHash, null)
		{
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x00188AB3 File Offset: 0x00186CB3
		public EssCertIDv2(byte[] certHash, IssuerSerial issuerSerial) : this(null, certHash, issuerSerial)
		{
		}

		// Token: 0x06004304 RID: 17156 RVA: 0x00188ABE File Offset: 0x00186CBE
		public EssCertIDv2(AlgorithmIdentifier algId, byte[] certHash, IssuerSerial issuerSerial)
		{
			if (algId == null)
			{
				this.hashAlgorithm = EssCertIDv2.DefaultAlgID;
			}
			else
			{
				this.hashAlgorithm = algId;
			}
			this.certHash = certHash;
			this.issuerSerial = issuerSerial;
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06004305 RID: 17157 RVA: 0x00188AEB File Offset: 0x00186CEB
		public AlgorithmIdentifier HashAlgorithm
		{
			get
			{
				return this.hashAlgorithm;
			}
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x00188AF3 File Offset: 0x00186CF3
		public byte[] GetCertHash()
		{
			return Arrays.Clone(this.certHash);
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06004307 RID: 17159 RVA: 0x00188B00 File Offset: 0x00186D00
		public IssuerSerial IssuerSerial
		{
			get
			{
				return this.issuerSerial;
			}
		}

		// Token: 0x06004308 RID: 17160 RVA: 0x00188B08 File Offset: 0x00186D08
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (!this.hashAlgorithm.Equals(EssCertIDv2.DefaultAlgID))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.hashAlgorithm
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				new DerOctetString(this.certHash).ToAsn1Object()
			});
			if (this.issuerSerial != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.issuerSerial
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002C0F RID: 11279
		private readonly AlgorithmIdentifier hashAlgorithm;

		// Token: 0x04002C10 RID: 11280
		private readonly byte[] certHash;

		// Token: 0x04002C11 RID: 11281
		private readonly IssuerSerial issuerSerial;

		// Token: 0x04002C12 RID: 11282
		private static readonly AlgorithmIdentifier DefaultAlgID = new AlgorithmIdentifier(NistObjectIdentifiers.IdSha256);
	}
}
