using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006EF RID: 1775
	public class ContentInfo : Asn1Encodable
	{
		// Token: 0x0600410E RID: 16654 RVA: 0x001818A8 File Offset: 0x0017FAA8
		public static ContentInfo GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			ContentInfo contentInfo = obj as ContentInfo;
			if (contentInfo != null)
			{
				return contentInfo;
			}
			return new ContentInfo(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x001818D1 File Offset: 0x0017FAD1
		private ContentInfo(Asn1Sequence seq)
		{
			this.contentType = (DerObjectIdentifier)seq[0];
			if (seq.Count > 1)
			{
				this.content = ((Asn1TaggedObject)seq[1]).GetObject();
			}
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x0018190B File Offset: 0x0017FB0B
		public ContentInfo(DerObjectIdentifier contentType, Asn1Encodable content)
		{
			this.contentType = contentType;
			this.content = content;
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06004111 RID: 16657 RVA: 0x00181921 File Offset: 0x0017FB21
		public DerObjectIdentifier ContentType
		{
			get
			{
				return this.contentType;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06004112 RID: 16658 RVA: 0x00181929 File Offset: 0x0017FB29
		public Asn1Encodable Content
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x00181934 File Offset: 0x0017FB34
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.contentType
			});
			if (this.content != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new BerTaggedObject(0, this.content)
				});
			}
			return new BerSequence(asn1EncodableVector);
		}

		// Token: 0x040029BB RID: 10683
		private readonly DerObjectIdentifier contentType;

		// Token: 0x040029BC RID: 10684
		private readonly Asn1Encodable content;
	}
}
