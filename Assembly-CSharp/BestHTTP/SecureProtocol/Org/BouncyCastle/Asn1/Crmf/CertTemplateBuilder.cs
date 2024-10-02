using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x0200076A RID: 1898
	public class CertTemplateBuilder
	{
		// Token: 0x06004432 RID: 17458 RVA: 0x0018D237 File Offset: 0x0018B437
		public virtual CertTemplateBuilder SetVersion(int ver)
		{
			this.version = new DerInteger(ver);
			return this;
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x0018D246 File Offset: 0x0018B446
		public virtual CertTemplateBuilder SetSerialNumber(DerInteger ser)
		{
			this.serialNumber = ser;
			return this;
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x0018D250 File Offset: 0x0018B450
		public virtual CertTemplateBuilder SetSigningAlg(AlgorithmIdentifier aid)
		{
			this.signingAlg = aid;
			return this;
		}

		// Token: 0x06004435 RID: 17461 RVA: 0x0018D25A File Offset: 0x0018B45A
		public virtual CertTemplateBuilder SetIssuer(X509Name name)
		{
			this.issuer = name;
			return this;
		}

		// Token: 0x06004436 RID: 17462 RVA: 0x0018D264 File Offset: 0x0018B464
		public virtual CertTemplateBuilder SetValidity(OptionalValidity v)
		{
			this.validity = v;
			return this;
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x0018D26E File Offset: 0x0018B46E
		public virtual CertTemplateBuilder SetSubject(X509Name name)
		{
			this.subject = name;
			return this;
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x0018D278 File Offset: 0x0018B478
		public virtual CertTemplateBuilder SetPublicKey(SubjectPublicKeyInfo spki)
		{
			this.publicKey = spki;
			return this;
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x0018D282 File Offset: 0x0018B482
		public virtual CertTemplateBuilder SetIssuerUID(DerBitString uid)
		{
			this.issuerUID = uid;
			return this;
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x0018D28C File Offset: 0x0018B48C
		public virtual CertTemplateBuilder SetSubjectUID(DerBitString uid)
		{
			this.subjectUID = uid;
			return this;
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x0018D296 File Offset: 0x0018B496
		public virtual CertTemplateBuilder SetExtensions(X509Extensions extens)
		{
			this.extensions = extens;
			return this;
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x0018D2A0 File Offset: 0x0018B4A0
		public virtual CertTemplate Build()
		{
			Asn1EncodableVector v = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			this.AddOptional(v, 0, false, this.version);
			this.AddOptional(v, 1, false, this.serialNumber);
			this.AddOptional(v, 2, false, this.signingAlg);
			this.AddOptional(v, 3, true, this.issuer);
			this.AddOptional(v, 4, false, this.validity);
			this.AddOptional(v, 5, true, this.subject);
			this.AddOptional(v, 6, false, this.publicKey);
			this.AddOptional(v, 7, false, this.issuerUID);
			this.AddOptional(v, 8, false, this.subjectUID);
			this.AddOptional(v, 9, false, this.extensions);
			return CertTemplate.GetInstance(new DerSequence(v));
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x0018D35A File Offset: 0x0018B55A
		private void AddOptional(Asn1EncodableVector v, int tagNo, bool isExplicit, Asn1Encodable obj)
		{
			if (obj != null)
			{
				v.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(isExplicit, tagNo, obj)
				});
			}
		}

		// Token: 0x04002CBD RID: 11453
		private DerInteger version;

		// Token: 0x04002CBE RID: 11454
		private DerInteger serialNumber;

		// Token: 0x04002CBF RID: 11455
		private AlgorithmIdentifier signingAlg;

		// Token: 0x04002CC0 RID: 11456
		private X509Name issuer;

		// Token: 0x04002CC1 RID: 11457
		private OptionalValidity validity;

		// Token: 0x04002CC2 RID: 11458
		private X509Name subject;

		// Token: 0x04002CC3 RID: 11459
		private SubjectPublicKeyInfo publicKey;

		// Token: 0x04002CC4 RID: 11460
		private DerBitString issuerUID;

		// Token: 0x04002CC5 RID: 11461
		private DerBitString subjectUID;

		// Token: 0x04002CC6 RID: 11462
		private X509Extensions extensions;
	}
}
