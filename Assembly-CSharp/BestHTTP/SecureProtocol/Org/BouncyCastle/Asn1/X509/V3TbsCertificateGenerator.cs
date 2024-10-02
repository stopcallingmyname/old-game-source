using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006BF RID: 1727
	public class V3TbsCertificateGenerator
	{
		// Token: 0x06003FAF RID: 16303 RVA: 0x0017AF47 File Offset: 0x00179147
		public void SetSerialNumber(DerInteger serialNumber)
		{
			this.serialNumber = serialNumber;
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x0017AF50 File Offset: 0x00179150
		public void SetSignature(AlgorithmIdentifier signature)
		{
			this.signature = signature;
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x0017AF59 File Offset: 0x00179159
		public void SetIssuer(X509Name issuer)
		{
			this.issuer = issuer;
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x0017AF62 File Offset: 0x00179162
		public void SetStartDate(DerUtcTime startDate)
		{
			this.startDate = new Time(startDate);
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x0017AF70 File Offset: 0x00179170
		public void SetStartDate(Time startDate)
		{
			this.startDate = startDate;
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x0017AF79 File Offset: 0x00179179
		public void SetEndDate(DerUtcTime endDate)
		{
			this.endDate = new Time(endDate);
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x0017AF87 File Offset: 0x00179187
		public void SetEndDate(Time endDate)
		{
			this.endDate = endDate;
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x0017AF90 File Offset: 0x00179190
		public void SetSubject(X509Name subject)
		{
			this.subject = subject;
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x0017AF99 File Offset: 0x00179199
		public void SetIssuerUniqueID(DerBitString uniqueID)
		{
			this.issuerUniqueID = uniqueID;
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x0017AFA2 File Offset: 0x001791A2
		public void SetSubjectUniqueID(DerBitString uniqueID)
		{
			this.subjectUniqueID = uniqueID;
		}

		// Token: 0x06003FB9 RID: 16313 RVA: 0x0017AFAB File Offset: 0x001791AB
		public void SetSubjectPublicKeyInfo(SubjectPublicKeyInfo pubKeyInfo)
		{
			this.subjectPublicKeyInfo = pubKeyInfo;
		}

		// Token: 0x06003FBA RID: 16314 RVA: 0x0017AFB4 File Offset: 0x001791B4
		public void SetExtensions(X509Extensions extensions)
		{
			this.extensions = extensions;
			if (extensions != null)
			{
				X509Extension extension = extensions.GetExtension(X509Extensions.SubjectAlternativeName);
				if (extension != null && extension.IsCritical)
				{
					this.altNamePresentAndCritical = true;
				}
			}
		}

		// Token: 0x06003FBB RID: 16315 RVA: 0x0017AFEC File Offset: 0x001791EC
		public TbsCertificateStructure GenerateTbsCertificate()
		{
			if (this.serialNumber == null || this.signature == null || this.issuer == null || this.startDate == null || this.endDate == null || (this.subject == null && !this.altNamePresentAndCritical) || this.subjectPublicKeyInfo == null)
			{
				throw new InvalidOperationException("not all mandatory fields set in V3 TBScertificate generator");
			}
			DerSequence derSequence = new DerSequence(new Asn1Encodable[]
			{
				this.startDate,
				this.endDate
			});
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				this.serialNumber,
				this.signature,
				this.issuer,
				derSequence
			});
			if (this.subject != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.subject
				});
			}
			else
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					DerSequence.Empty
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.subjectPublicKeyInfo
			});
			if (this.issuerUniqueID != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.issuerUniqueID)
				});
			}
			if (this.subjectUniqueID != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 2, this.subjectUniqueID)
				});
			}
			if (this.extensions != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(3, this.extensions)
				});
			}
			return new TbsCertificateStructure(new DerSequence(asn1EncodableVector));
		}

		// Token: 0x04002852 RID: 10322
		internal DerTaggedObject version = new DerTaggedObject(0, new DerInteger(2));

		// Token: 0x04002853 RID: 10323
		internal DerInteger serialNumber;

		// Token: 0x04002854 RID: 10324
		internal AlgorithmIdentifier signature;

		// Token: 0x04002855 RID: 10325
		internal X509Name issuer;

		// Token: 0x04002856 RID: 10326
		internal Time startDate;

		// Token: 0x04002857 RID: 10327
		internal Time endDate;

		// Token: 0x04002858 RID: 10328
		internal X509Name subject;

		// Token: 0x04002859 RID: 10329
		internal SubjectPublicKeyInfo subjectPublicKeyInfo;

		// Token: 0x0400285A RID: 10330
		internal X509Extensions extensions;

		// Token: 0x0400285B RID: 10331
		private bool altNamePresentAndCritical;

		// Token: 0x0400285C RID: 10332
		private DerBitString issuerUniqueID;

		// Token: 0x0400285D RID: 10333
		private DerBitString subjectUniqueID;
	}
}
