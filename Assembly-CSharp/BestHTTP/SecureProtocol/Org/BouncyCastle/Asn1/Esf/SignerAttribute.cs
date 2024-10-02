using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000758 RID: 1880
	public class SignerAttribute : Asn1Encodable
	{
		// Token: 0x060043B9 RID: 17337 RVA: 0x0018B676 File Offset: 0x00189876
		public static SignerAttribute GetInstance(object obj)
		{
			if (obj == null || obj is SignerAttribute)
			{
				return (SignerAttribute)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new SignerAttribute(obj);
			}
			throw new ArgumentException("Unknown object in 'SignerAttribute' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x0018B6B4 File Offset: 0x001898B4
		private SignerAttribute(object obj)
		{
			DerTaggedObject derTaggedObject = (DerTaggedObject)((Asn1Sequence)obj)[0];
			if (derTaggedObject.TagNo == 0)
			{
				this.claimedAttributes = Asn1Sequence.GetInstance(derTaggedObject, true);
				return;
			}
			if (derTaggedObject.TagNo == 1)
			{
				this.certifiedAttributes = AttributeCertificate.GetInstance(derTaggedObject);
				return;
			}
			throw new ArgumentException("illegal tag.", "obj");
		}

		// Token: 0x060043BB RID: 17339 RVA: 0x0018B714 File Offset: 0x00189914
		public SignerAttribute(Asn1Sequence claimedAttributes)
		{
			this.claimedAttributes = claimedAttributes;
		}

		// Token: 0x060043BC RID: 17340 RVA: 0x0018B723 File Offset: 0x00189923
		public SignerAttribute(AttributeCertificate certifiedAttributes)
		{
			this.certifiedAttributes = certifiedAttributes;
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x060043BD RID: 17341 RVA: 0x0018B732 File Offset: 0x00189932
		public virtual Asn1Sequence ClaimedAttributes
		{
			get
			{
				return this.claimedAttributes;
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x060043BE RID: 17342 RVA: 0x0018B73A File Offset: 0x0018993A
		public virtual AttributeCertificate CertifiedAttributes
		{
			get
			{
				return this.certifiedAttributes;
			}
		}

		// Token: 0x060043BF RID: 17343 RVA: 0x0018B744 File Offset: 0x00189944
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.claimedAttributes != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(0, this.claimedAttributes)
				});
			}
			else
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(1, this.certifiedAttributes)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002C57 RID: 11351
		private Asn1Sequence claimedAttributes;

		// Token: 0x04002C58 RID: 11352
		private AttributeCertificate certifiedAttributes;
	}
}
