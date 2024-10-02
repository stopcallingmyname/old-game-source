using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006BB RID: 1723
	public class V1TbsCertificateGenerator
	{
		// Token: 0x06003F7F RID: 16255 RVA: 0x0017A7DA File Offset: 0x001789DA
		public void SetSerialNumber(DerInteger serialNumber)
		{
			this.serialNumber = serialNumber;
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x0017A7E3 File Offset: 0x001789E3
		public void SetSignature(AlgorithmIdentifier signature)
		{
			this.signature = signature;
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x0017A7EC File Offset: 0x001789EC
		public void SetIssuer(X509Name issuer)
		{
			this.issuer = issuer;
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x0017A7F5 File Offset: 0x001789F5
		public void SetStartDate(Time startDate)
		{
			this.startDate = startDate;
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x0017A7FE File Offset: 0x001789FE
		public void SetStartDate(DerUtcTime startDate)
		{
			this.startDate = new Time(startDate);
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x0017A80C File Offset: 0x00178A0C
		public void SetEndDate(Time endDate)
		{
			this.endDate = endDate;
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x0017A815 File Offset: 0x00178A15
		public void SetEndDate(DerUtcTime endDate)
		{
			this.endDate = new Time(endDate);
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x0017A823 File Offset: 0x00178A23
		public void SetSubject(X509Name subject)
		{
			this.subject = subject;
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x0017A82C File Offset: 0x00178A2C
		public void SetSubjectPublicKeyInfo(SubjectPublicKeyInfo pubKeyInfo)
		{
			this.subjectPublicKeyInfo = pubKeyInfo;
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x0017A838 File Offset: 0x00178A38
		public TbsCertificateStructure GenerateTbsCertificate()
		{
			if (this.serialNumber == null || this.signature == null || this.issuer == null || this.startDate == null || this.endDate == null || this.subject == null || this.subjectPublicKeyInfo == null)
			{
				throw new InvalidOperationException("not all mandatory fields set in V1 TBScertificate generator");
			}
			return new TbsCertificateStructure(new DerSequence(new Asn1Encodable[]
			{
				this.serialNumber,
				this.signature,
				this.issuer,
				new DerSequence(new Asn1Encodable[]
				{
					this.startDate,
					this.endDate
				}),
				this.subject,
				this.subjectPublicKeyInfo
			}));
		}

		// Token: 0x04002836 RID: 10294
		internal DerTaggedObject version = new DerTaggedObject(0, new DerInteger(0));

		// Token: 0x04002837 RID: 10295
		internal DerInteger serialNumber;

		// Token: 0x04002838 RID: 10296
		internal AlgorithmIdentifier signature;

		// Token: 0x04002839 RID: 10297
		internal X509Name issuer;

		// Token: 0x0400283A RID: 10298
		internal Time startDate;

		// Token: 0x0400283B RID: 10299
		internal Time endDate;

		// Token: 0x0400283C RID: 10300
		internal X509Name subject;

		// Token: 0x0400283D RID: 10301
		internal SubjectPublicKeyInfo subjectPublicKeyInfo;
	}
}
