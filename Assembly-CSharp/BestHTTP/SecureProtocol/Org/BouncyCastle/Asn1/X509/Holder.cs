using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200069F RID: 1695
	public class Holder : Asn1Encodable
	{
		// Token: 0x06003EAF RID: 16047 RVA: 0x00177F20 File Offset: 0x00176120
		public static Holder GetInstance(object obj)
		{
			if (obj is Holder)
			{
				return (Holder)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Holder((Asn1Sequence)obj);
			}
			if (obj is Asn1TaggedObject)
			{
				return new Holder((Asn1TaggedObject)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003EB0 RID: 16048 RVA: 0x00177F80 File Offset: 0x00176180
		public Holder(Asn1TaggedObject tagObj)
		{
			int tagNo = tagObj.TagNo;
			if (tagNo != 0)
			{
				if (tagNo != 1)
				{
					throw new ArgumentException("unknown tag in Holder");
				}
				this.entityName = GeneralNames.GetInstance(tagObj, false);
			}
			else
			{
				this.baseCertificateID = IssuerSerial.GetInstance(tagObj, false);
			}
			this.version = 0;
		}

		// Token: 0x06003EB1 RID: 16049 RVA: 0x00177FD4 File Offset: 0x001761D4
		private Holder(Asn1Sequence seq)
		{
			if (seq.Count > 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			for (int num = 0; num != seq.Count; num++)
			{
				Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(seq[num]);
				switch (instance.TagNo)
				{
				case 0:
					this.baseCertificateID = IssuerSerial.GetInstance(instance, false);
					break;
				case 1:
					this.entityName = GeneralNames.GetInstance(instance, false);
					break;
				case 2:
					this.objectDigestInfo = ObjectDigestInfo.GetInstance(instance, false);
					break;
				default:
					throw new ArgumentException("unknown tag in Holder");
				}
			}
			this.version = 1;
		}

		// Token: 0x06003EB2 RID: 16050 RVA: 0x00178083 File Offset: 0x00176283
		public Holder(IssuerSerial baseCertificateID) : this(baseCertificateID, 1)
		{
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x0017808D File Offset: 0x0017628D
		public Holder(IssuerSerial baseCertificateID, int version)
		{
			this.baseCertificateID = baseCertificateID;
			this.version = version;
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06003EB4 RID: 16052 RVA: 0x001780A3 File Offset: 0x001762A3
		public int Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x001780AB File Offset: 0x001762AB
		public Holder(GeneralNames entityName) : this(entityName, 1)
		{
		}

		// Token: 0x06003EB6 RID: 16054 RVA: 0x001780B5 File Offset: 0x001762B5
		public Holder(GeneralNames entityName, int version)
		{
			this.entityName = entityName;
			this.version = version;
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x001780CB File Offset: 0x001762CB
		public Holder(ObjectDigestInfo objectDigestInfo)
		{
			this.objectDigestInfo = objectDigestInfo;
			this.version = 1;
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06003EB8 RID: 16056 RVA: 0x001780E1 File Offset: 0x001762E1
		public IssuerSerial BaseCertificateID
		{
			get
			{
				return this.baseCertificateID;
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06003EB9 RID: 16057 RVA: 0x001780E9 File Offset: 0x001762E9
		public GeneralNames EntityName
		{
			get
			{
				return this.entityName;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06003EBA RID: 16058 RVA: 0x001780F1 File Offset: 0x001762F1
		public ObjectDigestInfo ObjectDigestInfo
		{
			get
			{
				return this.objectDigestInfo;
			}
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x001780FC File Offset: 0x001762FC
		public override Asn1Object ToAsn1Object()
		{
			if (this.version == 1)
			{
				Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
				if (this.baseCertificateID != null)
				{
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						new DerTaggedObject(false, 0, this.baseCertificateID)
					});
				}
				if (this.entityName != null)
				{
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						new DerTaggedObject(false, 1, this.entityName)
					});
				}
				if (this.objectDigestInfo != null)
				{
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						new DerTaggedObject(false, 2, this.objectDigestInfo)
					});
				}
				return new DerSequence(asn1EncodableVector);
			}
			if (this.entityName != null)
			{
				return new DerTaggedObject(false, 1, this.entityName);
			}
			return new DerTaggedObject(false, 0, this.baseCertificateID);
		}

		// Token: 0x040027C7 RID: 10183
		internal readonly IssuerSerial baseCertificateID;

		// Token: 0x040027C8 RID: 10184
		internal readonly GeneralNames entityName;

		// Token: 0x040027C9 RID: 10185
		internal readonly ObjectDigestInfo objectDigestInfo;

		// Token: 0x040027CA RID: 10186
		private readonly int version;
	}
}
