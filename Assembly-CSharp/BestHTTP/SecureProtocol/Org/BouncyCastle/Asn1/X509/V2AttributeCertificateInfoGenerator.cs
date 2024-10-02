using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006BC RID: 1724
	public class V2AttributeCertificateInfoGenerator
	{
		// Token: 0x06003F89 RID: 16265 RVA: 0x0017A8E5 File Offset: 0x00178AE5
		public V2AttributeCertificateInfoGenerator()
		{
			this.version = new DerInteger(1);
			this.attributes = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x0017A909 File Offset: 0x00178B09
		public void SetHolder(Holder holder)
		{
			this.holder = holder;
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x0017A912 File Offset: 0x00178B12
		public void AddAttribute(string oid, Asn1Encodable value)
		{
			this.attributes.Add(new Asn1Encodable[]
			{
				new AttributeX509(new DerObjectIdentifier(oid), new DerSet(value))
			});
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x0017A939 File Offset: 0x00178B39
		public void AddAttribute(AttributeX509 attribute)
		{
			this.attributes.Add(new Asn1Encodable[]
			{
				attribute
			});
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x0017A950 File Offset: 0x00178B50
		public void SetSerialNumber(DerInteger serialNumber)
		{
			this.serialNumber = serialNumber;
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x0017A959 File Offset: 0x00178B59
		public void SetSignature(AlgorithmIdentifier signature)
		{
			this.signature = signature;
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x0017A962 File Offset: 0x00178B62
		public void SetIssuer(AttCertIssuer issuer)
		{
			this.issuer = issuer;
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x0017A96B File Offset: 0x00178B6B
		public void SetStartDate(DerGeneralizedTime startDate)
		{
			this.startDate = startDate;
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x0017A974 File Offset: 0x00178B74
		public void SetEndDate(DerGeneralizedTime endDate)
		{
			this.endDate = endDate;
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x0017A97D File Offset: 0x00178B7D
		public void SetIssuerUniqueID(DerBitString issuerUniqueID)
		{
			this.issuerUniqueID = issuerUniqueID;
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x0017A986 File Offset: 0x00178B86
		public void SetExtensions(X509Extensions extensions)
		{
			this.extensions = extensions;
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x0017A990 File Offset: 0x00178B90
		public AttributeCertificateInfo GenerateAttributeCertificateInfo()
		{
			if (this.serialNumber == null || this.signature == null || this.issuer == null || this.startDate == null || this.endDate == null || this.holder == null || this.attributes == null)
			{
				throw new InvalidOperationException("not all mandatory fields set in V2 AttributeCertificateInfo generator");
			}
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				this.holder,
				this.issuer,
				this.signature,
				this.serialNumber
			});
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				new AttCertValidityPeriod(this.startDate, this.endDate)
			});
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				new DerSequence(this.attributes)
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
			return AttributeCertificateInfo.GetInstance(new DerSequence(asn1EncodableVector));
		}

		// Token: 0x0400283E RID: 10302
		internal DerInteger version;

		// Token: 0x0400283F RID: 10303
		internal Holder holder;

		// Token: 0x04002840 RID: 10304
		internal AttCertIssuer issuer;

		// Token: 0x04002841 RID: 10305
		internal AlgorithmIdentifier signature;

		// Token: 0x04002842 RID: 10306
		internal DerInteger serialNumber;

		// Token: 0x04002843 RID: 10307
		internal Asn1EncodableVector attributes;

		// Token: 0x04002844 RID: 10308
		internal DerBitString issuerUniqueID;

		// Token: 0x04002845 RID: 10309
		internal X509Extensions extensions;

		// Token: 0x04002846 RID: 10310
		internal DerGeneralizedTime startDate;

		// Token: 0x04002847 RID: 10311
		internal DerGeneralizedTime endDate;
	}
}
