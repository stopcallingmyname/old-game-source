using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000770 RID: 1904
	public class OptionalValidity : Asn1Encodable
	{
		// Token: 0x06004461 RID: 17505 RVA: 0x0018D8A0 File Offset: 0x0018BAA0
		private OptionalValidity(Asn1Sequence seq)
		{
			foreach (object obj in seq)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
				if (asn1TaggedObject.TagNo == 0)
				{
					this.notBefore = Time.GetInstance(asn1TaggedObject, true);
				}
				else
				{
					this.notAfter = Time.GetInstance(asn1TaggedObject, true);
				}
			}
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x0018D918 File Offset: 0x0018BB18
		public static OptionalValidity GetInstance(object obj)
		{
			if (obj == null || obj is OptionalValidity)
			{
				return (OptionalValidity)obj;
			}
			return new OptionalValidity(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06004463 RID: 17507 RVA: 0x0018D937 File Offset: 0x0018BB37
		public virtual Time NotBefore
		{
			get
			{
				return this.notBefore;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06004464 RID: 17508 RVA: 0x0018D93F File Offset: 0x0018BB3F
		public virtual Time NotAfter
		{
			get
			{
				return this.notAfter;
			}
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x0018D948 File Offset: 0x0018BB48
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.notBefore != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.notBefore)
				});
			}
			if (this.notAfter != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, this.notAfter)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002CDA RID: 11482
		private readonly Time notBefore;

		// Token: 0x04002CDB RID: 11483
		private readonly Time notAfter;
	}
}
