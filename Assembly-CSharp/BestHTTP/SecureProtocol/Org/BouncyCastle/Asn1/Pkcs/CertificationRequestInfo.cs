using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006EE RID: 1774
	public class CertificationRequestInfo : Asn1Encodable
	{
		// Token: 0x06004105 RID: 16645 RVA: 0x0018167F File Offset: 0x0017F87F
		public static CertificationRequestInfo GetInstance(object obj)
		{
			if (obj is CertificationRequestInfo)
			{
				return (CertificationRequestInfo)obj;
			}
			if (obj != null)
			{
				return new CertificationRequestInfo(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x001816A0 File Offset: 0x0017F8A0
		public CertificationRequestInfo(X509Name subject, SubjectPublicKeyInfo pkInfo, Asn1Set attributes)
		{
			this.subject = subject;
			this.subjectPKInfo = pkInfo;
			this.attributes = attributes;
			CertificationRequestInfo.ValidateAttributes(attributes);
			if (subject == null || this.version == null || this.subjectPKInfo == null)
			{
				throw new ArgumentException("Not all mandatory fields set in CertificationRequestInfo generator.");
			}
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x001816F8 File Offset: 0x0017F8F8
		private CertificationRequestInfo(Asn1Sequence seq)
		{
			this.version = (DerInteger)seq[0];
			this.subject = X509Name.GetInstance(seq[1]);
			this.subjectPKInfo = SubjectPublicKeyInfo.GetInstance(seq[2]);
			if (seq.Count > 3)
			{
				DerTaggedObject obj = (DerTaggedObject)seq[3];
				this.attributes = Asn1Set.GetInstance(obj, false);
			}
			CertificationRequestInfo.ValidateAttributes(this.attributes);
			if (this.subject == null || this.version == null || this.subjectPKInfo == null)
			{
				throw new ArgumentException("Not all mandatory fields set in CertificationRequestInfo generator.");
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06004108 RID: 16648 RVA: 0x0018179E File Offset: 0x0017F99E
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06004109 RID: 16649 RVA: 0x001817A6 File Offset: 0x0017F9A6
		public X509Name Subject
		{
			get
			{
				return this.subject;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x0600410A RID: 16650 RVA: 0x001817AE File Offset: 0x0017F9AE
		public SubjectPublicKeyInfo SubjectPublicKeyInfo
		{
			get
			{
				return this.subjectPKInfo;
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x0600410B RID: 16651 RVA: 0x001817B6 File Offset: 0x0017F9B6
		public Asn1Set Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		// Token: 0x0600410C RID: 16652 RVA: 0x001817C0 File Offset: 0x0017F9C0
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				this.subject,
				this.subjectPKInfo
			});
			if (this.attributes != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.attributes)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x00181820 File Offset: 0x0017FA20
		private static void ValidateAttributes(Asn1Set attributes)
		{
			if (attributes == null)
			{
				return;
			}
			foreach (object obj in attributes)
			{
				AttributePkcs instance = AttributePkcs.GetInstance(((Asn1Encodable)obj).ToAsn1Object());
				if (instance.AttrType.Equals(PkcsObjectIdentifiers.Pkcs9AtChallengePassword) && instance.AttrValues.Count != 1)
				{
					throw new ArgumentException("challengePassword attribute must have one value");
				}
			}
		}

		// Token: 0x040029B7 RID: 10679
		internal DerInteger version = new DerInteger(0);

		// Token: 0x040029B8 RID: 10680
		internal X509Name subject;

		// Token: 0x040029B9 RID: 10681
		internal SubjectPublicKeyInfo subjectPKInfo;

		// Token: 0x040029BA RID: 10682
		internal Asn1Set attributes;
	}
}
