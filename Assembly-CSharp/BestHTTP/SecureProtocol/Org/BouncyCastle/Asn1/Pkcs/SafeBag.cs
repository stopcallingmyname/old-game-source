using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x02000702 RID: 1794
	public class SafeBag : Asn1Encodable
	{
		// Token: 0x06004198 RID: 16792 RVA: 0x001838D4 File Offset: 0x00181AD4
		public SafeBag(DerObjectIdentifier oid, Asn1Object obj)
		{
			this.bagID = oid;
			this.bagValue = obj;
			this.bagAttributes = null;
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x001838F1 File Offset: 0x00181AF1
		public SafeBag(DerObjectIdentifier oid, Asn1Object obj, Asn1Set bagAttributes)
		{
			this.bagID = oid;
			this.bagValue = obj;
			this.bagAttributes = bagAttributes;
		}

		// Token: 0x0600419A RID: 16794 RVA: 0x00183910 File Offset: 0x00181B10
		public SafeBag(Asn1Sequence seq)
		{
			this.bagID = (DerObjectIdentifier)seq[0];
			this.bagValue = ((DerTaggedObject)seq[1]).GetObject();
			if (seq.Count == 3)
			{
				this.bagAttributes = (Asn1Set)seq[2];
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x0600419B RID: 16795 RVA: 0x00183967 File Offset: 0x00181B67
		public DerObjectIdentifier BagID
		{
			get
			{
				return this.bagID;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x0600419C RID: 16796 RVA: 0x0018396F File Offset: 0x00181B6F
		public Asn1Object BagValue
		{
			get
			{
				return this.bagValue;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x0600419D RID: 16797 RVA: 0x00183977 File Offset: 0x00181B77
		public Asn1Set BagAttributes
		{
			get
			{
				return this.bagAttributes;
			}
		}

		// Token: 0x0600419E RID: 16798 RVA: 0x00183980 File Offset: 0x00181B80
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.bagID,
				new DerTaggedObject(0, this.bagValue)
			});
			if (this.bagAttributes != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.bagAttributes
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002A82 RID: 10882
		private readonly DerObjectIdentifier bagID;

		// Token: 0x04002A83 RID: 10883
		private readonly Asn1Object bagValue;

		// Token: 0x04002A84 RID: 10884
		private readonly Asn1Set bagAttributes;
	}
}
