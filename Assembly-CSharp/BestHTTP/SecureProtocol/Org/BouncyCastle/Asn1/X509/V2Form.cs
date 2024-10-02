using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006BD RID: 1725
	public class V2Form : Asn1Encodable
	{
		// Token: 0x06003F95 RID: 16277 RVA: 0x0017AA98 File Offset: 0x00178C98
		public static V2Form GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return V2Form.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003F96 RID: 16278 RVA: 0x0017AAA6 File Offset: 0x00178CA6
		public static V2Form GetInstance(object obj)
		{
			if (obj is V2Form)
			{
				return (V2Form)obj;
			}
			if (obj != null)
			{
				return new V2Form(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x0017AAC7 File Offset: 0x00178CC7
		public V2Form(GeneralNames issuerName) : this(issuerName, null, null)
		{
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x0017AAD2 File Offset: 0x00178CD2
		public V2Form(GeneralNames issuerName, IssuerSerial baseCertificateID) : this(issuerName, baseCertificateID, null)
		{
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x0017AADD File Offset: 0x00178CDD
		public V2Form(GeneralNames issuerName, ObjectDigestInfo objectDigestInfo) : this(issuerName, null, objectDigestInfo)
		{
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x0017AAE8 File Offset: 0x00178CE8
		public V2Form(GeneralNames issuerName, IssuerSerial baseCertificateID, ObjectDigestInfo objectDigestInfo)
		{
			this.issuerName = issuerName;
			this.baseCertificateID = baseCertificateID;
			this.objectDigestInfo = objectDigestInfo;
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x0017AB08 File Offset: 0x00178D08
		private V2Form(Asn1Sequence seq)
		{
			if (seq.Count > 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			int num = 0;
			if (!(seq[0] is Asn1TaggedObject))
			{
				num++;
				this.issuerName = GeneralNames.GetInstance(seq[0]);
			}
			for (int num2 = num; num2 != seq.Count; num2++)
			{
				Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(seq[num2]);
				if (instance.TagNo == 0)
				{
					this.baseCertificateID = IssuerSerial.GetInstance(instance, false);
				}
				else
				{
					if (instance.TagNo != 1)
					{
						throw new ArgumentException("Bad tag number: " + instance.TagNo);
					}
					this.objectDigestInfo = ObjectDigestInfo.GetInstance(instance, false);
				}
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06003F9C RID: 16284 RVA: 0x0017ABCD File Offset: 0x00178DCD
		public GeneralNames IssuerName
		{
			get
			{
				return this.issuerName;
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06003F9D RID: 16285 RVA: 0x0017ABD5 File Offset: 0x00178DD5
		public IssuerSerial BaseCertificateID
		{
			get
			{
				return this.baseCertificateID;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06003F9E RID: 16286 RVA: 0x0017ABDD File Offset: 0x00178DDD
		public ObjectDigestInfo ObjectDigestInfo
		{
			get
			{
				return this.objectDigestInfo;
			}
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x0017ABE8 File Offset: 0x00178DE8
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.issuerName != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.issuerName
				});
			}
			if (this.baseCertificateID != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.baseCertificateID)
				});
			}
			if (this.objectDigestInfo != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.objectDigestInfo)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002848 RID: 10312
		internal GeneralNames issuerName;

		// Token: 0x04002849 RID: 10313
		internal IssuerSerial baseCertificateID;

		// Token: 0x0400284A RID: 10314
		internal ObjectDigestInfo objectDigestInfo;
	}
}
