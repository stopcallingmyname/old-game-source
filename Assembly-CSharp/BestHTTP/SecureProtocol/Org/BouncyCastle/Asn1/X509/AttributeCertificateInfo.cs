using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200068A RID: 1674
	public class AttributeCertificateInfo : Asn1Encodable
	{
		// Token: 0x06003DFE RID: 15870 RVA: 0x00175B2E File Offset: 0x00173D2E
		public static AttributeCertificateInfo GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return AttributeCertificateInfo.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x00175B3C File Offset: 0x00173D3C
		public static AttributeCertificateInfo GetInstance(object obj)
		{
			if (obj is AttributeCertificateInfo)
			{
				return (AttributeCertificateInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new AttributeCertificateInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x00175B7C File Offset: 0x00173D7C
		private AttributeCertificateInfo(Asn1Sequence seq)
		{
			if (seq.Count < 7 || seq.Count > 9)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.version = DerInteger.GetInstance(seq[0]);
			this.holder = Holder.GetInstance(seq[1]);
			this.issuer = AttCertIssuer.GetInstance(seq[2]);
			this.signature = AlgorithmIdentifier.GetInstance(seq[3]);
			this.serialNumber = DerInteger.GetInstance(seq[4]);
			this.attrCertValidityPeriod = AttCertValidityPeriod.GetInstance(seq[5]);
			this.attributes = Asn1Sequence.GetInstance(seq[6]);
			for (int i = 7; i < seq.Count; i++)
			{
				Asn1Encodable asn1Encodable = seq[i];
				if (asn1Encodable is DerBitString)
				{
					this.issuerUniqueID = DerBitString.GetInstance(seq[i]);
				}
				else if (asn1Encodable is Asn1Sequence || asn1Encodable is X509Extensions)
				{
					this.extensions = X509Extensions.GetInstance(seq[i]);
				}
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06003E01 RID: 15873 RVA: 0x00175C92 File Offset: 0x00173E92
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06003E02 RID: 15874 RVA: 0x00175C9A File Offset: 0x00173E9A
		public Holder Holder
		{
			get
			{
				return this.holder;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06003E03 RID: 15875 RVA: 0x00175CA2 File Offset: 0x00173EA2
		public AttCertIssuer Issuer
		{
			get
			{
				return this.issuer;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06003E04 RID: 15876 RVA: 0x00175CAA File Offset: 0x00173EAA
		public AlgorithmIdentifier Signature
		{
			get
			{
				return this.signature;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06003E05 RID: 15877 RVA: 0x00175CB2 File Offset: 0x00173EB2
		public DerInteger SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06003E06 RID: 15878 RVA: 0x00175CBA File Offset: 0x00173EBA
		public AttCertValidityPeriod AttrCertValidityPeriod
		{
			get
			{
				return this.attrCertValidityPeriod;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06003E07 RID: 15879 RVA: 0x00175CC2 File Offset: 0x00173EC2
		public Asn1Sequence Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06003E08 RID: 15880 RVA: 0x00175CCA File Offset: 0x00173ECA
		public DerBitString IssuerUniqueID
		{
			get
			{
				return this.issuerUniqueID;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06003E09 RID: 15881 RVA: 0x00175CD2 File Offset: 0x00173ED2
		public X509Extensions Extensions
		{
			get
			{
				return this.extensions;
			}
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x00175CDC File Offset: 0x00173EDC
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				this.holder,
				this.issuer,
				this.signature,
				this.serialNumber,
				this.attrCertValidityPeriod,
				this.attributes
			});
			if (this.issuerUniqueID != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.issuerUniqueID
				});
			}
			if (this.extensions != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.extensions
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002781 RID: 10113
		internal readonly DerInteger version;

		// Token: 0x04002782 RID: 10114
		internal readonly Holder holder;

		// Token: 0x04002783 RID: 10115
		internal readonly AttCertIssuer issuer;

		// Token: 0x04002784 RID: 10116
		internal readonly AlgorithmIdentifier signature;

		// Token: 0x04002785 RID: 10117
		internal readonly DerInteger serialNumber;

		// Token: 0x04002786 RID: 10118
		internal readonly AttCertValidityPeriod attrCertValidityPeriod;

		// Token: 0x04002787 RID: 10119
		internal readonly Asn1Sequence attributes;

		// Token: 0x04002788 RID: 10120
		internal readonly DerBitString issuerUniqueID;

		// Token: 0x04002789 RID: 10121
		internal readonly X509Extensions extensions;
	}
}
