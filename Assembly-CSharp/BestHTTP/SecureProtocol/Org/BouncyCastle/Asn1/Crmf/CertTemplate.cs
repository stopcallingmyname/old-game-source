using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000769 RID: 1897
	public class CertTemplate : Asn1Encodable
	{
		// Token: 0x06004425 RID: 17445 RVA: 0x0018D048 File Offset: 0x0018B248
		private CertTemplate(Asn1Sequence seq)
		{
			this.seq = seq;
			foreach (object obj in seq)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
				switch (asn1TaggedObject.TagNo)
				{
				case 0:
					this.version = DerInteger.GetInstance(asn1TaggedObject, false);
					break;
				case 1:
					this.serialNumber = DerInteger.GetInstance(asn1TaggedObject, false);
					break;
				case 2:
					this.signingAlg = AlgorithmIdentifier.GetInstance(asn1TaggedObject, false);
					break;
				case 3:
					this.issuer = X509Name.GetInstance(asn1TaggedObject, true);
					break;
				case 4:
					this.validity = OptionalValidity.GetInstance(Asn1Sequence.GetInstance(asn1TaggedObject, false));
					break;
				case 5:
					this.subject = X509Name.GetInstance(asn1TaggedObject, true);
					break;
				case 6:
					this.publicKey = SubjectPublicKeyInfo.GetInstance(asn1TaggedObject, false);
					break;
				case 7:
					this.issuerUID = DerBitString.GetInstance(asn1TaggedObject, false);
					break;
				case 8:
					this.subjectUID = DerBitString.GetInstance(asn1TaggedObject, false);
					break;
				case 9:
					this.extensions = X509Extensions.GetInstance(asn1TaggedObject, false);
					break;
				default:
					throw new ArgumentException("unknown tag: " + asn1TaggedObject.TagNo, "seq");
				}
			}
		}

		// Token: 0x06004426 RID: 17446 RVA: 0x0018D1B4 File Offset: 0x0018B3B4
		public static CertTemplate GetInstance(object obj)
		{
			if (obj is CertTemplate)
			{
				return (CertTemplate)obj;
			}
			if (obj != null)
			{
				return new CertTemplate(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06004427 RID: 17447 RVA: 0x0018D1D5 File Offset: 0x0018B3D5
		public virtual int Version
		{
			get
			{
				return this.version.Value.IntValue;
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06004428 RID: 17448 RVA: 0x0018D1E7 File Offset: 0x0018B3E7
		public virtual DerInteger SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x06004429 RID: 17449 RVA: 0x0018D1EF File Offset: 0x0018B3EF
		public virtual AlgorithmIdentifier SigningAlg
		{
			get
			{
				return this.signingAlg;
			}
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x0600442A RID: 17450 RVA: 0x0018D1F7 File Offset: 0x0018B3F7
		public virtual X509Name Issuer
		{
			get
			{
				return this.issuer;
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x0600442B RID: 17451 RVA: 0x0018D1FF File Offset: 0x0018B3FF
		public virtual OptionalValidity Validity
		{
			get
			{
				return this.validity;
			}
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x0600442C RID: 17452 RVA: 0x0018D207 File Offset: 0x0018B407
		public virtual X509Name Subject
		{
			get
			{
				return this.subject;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x0600442D RID: 17453 RVA: 0x0018D20F File Offset: 0x0018B40F
		public virtual SubjectPublicKeyInfo PublicKey
		{
			get
			{
				return this.publicKey;
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x0600442E RID: 17454 RVA: 0x0018D217 File Offset: 0x0018B417
		public virtual DerBitString IssuerUID
		{
			get
			{
				return this.issuerUID;
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x0600442F RID: 17455 RVA: 0x0018D21F File Offset: 0x0018B41F
		public virtual DerBitString SubjectUID
		{
			get
			{
				return this.subjectUID;
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06004430 RID: 17456 RVA: 0x0018D227 File Offset: 0x0018B427
		public virtual X509Extensions Extensions
		{
			get
			{
				return this.extensions;
			}
		}

		// Token: 0x06004431 RID: 17457 RVA: 0x0018D22F File Offset: 0x0018B42F
		public override Asn1Object ToAsn1Object()
		{
			return this.seq;
		}

		// Token: 0x04002CB2 RID: 11442
		private readonly Asn1Sequence seq;

		// Token: 0x04002CB3 RID: 11443
		private readonly DerInteger version;

		// Token: 0x04002CB4 RID: 11444
		private readonly DerInteger serialNumber;

		// Token: 0x04002CB5 RID: 11445
		private readonly AlgorithmIdentifier signingAlg;

		// Token: 0x04002CB6 RID: 11446
		private readonly X509Name issuer;

		// Token: 0x04002CB7 RID: 11447
		private readonly OptionalValidity validity;

		// Token: 0x04002CB8 RID: 11448
		private readonly X509Name subject;

		// Token: 0x04002CB9 RID: 11449
		private readonly SubjectPublicKeyInfo publicKey;

		// Token: 0x04002CBA RID: 11450
		private readonly DerBitString issuerUID;

		// Token: 0x04002CBB RID: 11451
		private readonly DerBitString subjectUID;

		// Token: 0x04002CBC RID: 11452
		private readonly X509Extensions extensions;
	}
}
