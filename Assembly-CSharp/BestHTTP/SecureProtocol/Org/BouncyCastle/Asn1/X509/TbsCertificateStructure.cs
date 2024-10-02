using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006B6 RID: 1718
	public class TbsCertificateStructure : Asn1Encodable
	{
		// Token: 0x06003F4D RID: 16205 RVA: 0x00179EB1 File Offset: 0x001780B1
		public static TbsCertificateStructure GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return TbsCertificateStructure.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x00179EBF File Offset: 0x001780BF
		public static TbsCertificateStructure GetInstance(object obj)
		{
			if (obj is TbsCertificateStructure)
			{
				return (TbsCertificateStructure)obj;
			}
			if (obj != null)
			{
				return new TbsCertificateStructure(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x00179EE0 File Offset: 0x001780E0
		internal TbsCertificateStructure(Asn1Sequence seq)
		{
			int num = 0;
			this.seq = seq;
			if (seq[0] is DerTaggedObject)
			{
				this.version = DerInteger.GetInstance((Asn1TaggedObject)seq[0], true);
			}
			else
			{
				num = -1;
				this.version = new DerInteger(0);
			}
			bool flag = false;
			bool flag2 = false;
			if (this.version.Value.Equals(BigInteger.Zero))
			{
				flag = true;
			}
			else if (this.version.Value.Equals(BigInteger.One))
			{
				flag2 = true;
			}
			else if (!this.version.Value.Equals(BigInteger.Two))
			{
				throw new ArgumentException("version number not recognised");
			}
			this.serialNumber = DerInteger.GetInstance(seq[num + 1]);
			this.signature = AlgorithmIdentifier.GetInstance(seq[num + 2]);
			this.issuer = X509Name.GetInstance(seq[num + 3]);
			Asn1Sequence asn1Sequence = (Asn1Sequence)seq[num + 4];
			this.startDate = Time.GetInstance(asn1Sequence[0]);
			this.endDate = Time.GetInstance(asn1Sequence[1]);
			this.subject = X509Name.GetInstance(seq[num + 5]);
			this.subjectPublicKeyInfo = SubjectPublicKeyInfo.GetInstance(seq[num + 6]);
			int i = seq.Count - (num + 6) - 1;
			if (i != 0 && flag)
			{
				throw new ArgumentException("version 1 certificate contains extra data");
			}
			while (i > 0)
			{
				DerTaggedObject derTaggedObject = (DerTaggedObject)seq[num + 6 + i];
				switch (derTaggedObject.TagNo)
				{
				case 1:
					this.issuerUniqueID = DerBitString.GetInstance(derTaggedObject, false);
					break;
				case 2:
					this.subjectUniqueID = DerBitString.GetInstance(derTaggedObject, false);
					break;
				case 3:
					if (flag2)
					{
						throw new ArgumentException("version 2 certificate cannot contain extensions");
					}
					this.extensions = X509Extensions.GetInstance(Asn1Sequence.GetInstance(derTaggedObject, true));
					break;
				default:
					throw new ArgumentException("Unknown tag encountered in structure: " + derTaggedObject.TagNo);
				}
				i--;
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06003F50 RID: 16208 RVA: 0x0017A0EA File Offset: 0x001782EA
		public int Version
		{
			get
			{
				return this.version.Value.IntValue + 1;
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06003F51 RID: 16209 RVA: 0x0017A0FE File Offset: 0x001782FE
		public DerInteger VersionNumber
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06003F52 RID: 16210 RVA: 0x0017A106 File Offset: 0x00178306
		public DerInteger SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06003F53 RID: 16211 RVA: 0x0017A10E File Offset: 0x0017830E
		public AlgorithmIdentifier Signature
		{
			get
			{
				return this.signature;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06003F54 RID: 16212 RVA: 0x0017A116 File Offset: 0x00178316
		public X509Name Issuer
		{
			get
			{
				return this.issuer;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06003F55 RID: 16213 RVA: 0x0017A11E File Offset: 0x0017831E
		public Time StartDate
		{
			get
			{
				return this.startDate;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06003F56 RID: 16214 RVA: 0x0017A126 File Offset: 0x00178326
		public Time EndDate
		{
			get
			{
				return this.endDate;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06003F57 RID: 16215 RVA: 0x0017A12E File Offset: 0x0017832E
		public X509Name Subject
		{
			get
			{
				return this.subject;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06003F58 RID: 16216 RVA: 0x0017A136 File Offset: 0x00178336
		public SubjectPublicKeyInfo SubjectPublicKeyInfo
		{
			get
			{
				return this.subjectPublicKeyInfo;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06003F59 RID: 16217 RVA: 0x0017A13E File Offset: 0x0017833E
		public DerBitString IssuerUniqueID
		{
			get
			{
				return this.issuerUniqueID;
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06003F5A RID: 16218 RVA: 0x0017A146 File Offset: 0x00178346
		public DerBitString SubjectUniqueID
		{
			get
			{
				return this.subjectUniqueID;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06003F5B RID: 16219 RVA: 0x0017A14E File Offset: 0x0017834E
		public X509Extensions Extensions
		{
			get
			{
				return this.extensions;
			}
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x0017A156 File Offset: 0x00178356
		public override Asn1Object ToAsn1Object()
		{
			return this.seq;
		}

		// Token: 0x0400281B RID: 10267
		internal Asn1Sequence seq;

		// Token: 0x0400281C RID: 10268
		internal DerInteger version;

		// Token: 0x0400281D RID: 10269
		internal DerInteger serialNumber;

		// Token: 0x0400281E RID: 10270
		internal AlgorithmIdentifier signature;

		// Token: 0x0400281F RID: 10271
		internal X509Name issuer;

		// Token: 0x04002820 RID: 10272
		internal Time startDate;

		// Token: 0x04002821 RID: 10273
		internal Time endDate;

		// Token: 0x04002822 RID: 10274
		internal X509Name subject;

		// Token: 0x04002823 RID: 10275
		internal SubjectPublicKeyInfo subjectPublicKeyInfo;

		// Token: 0x04002824 RID: 10276
		internal DerBitString issuerUniqueID;

		// Token: 0x04002825 RID: 10277
		internal DerBitString subjectUniqueID;

		// Token: 0x04002826 RID: 10278
		internal X509Extensions extensions;
	}
}
