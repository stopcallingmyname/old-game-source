using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000785 RID: 1925
	public class ContentInfo : Asn1Encodable
	{
		// Token: 0x060044F8 RID: 17656 RVA: 0x0018F574 File Offset: 0x0018D774
		public static ContentInfo GetInstance(object obj)
		{
			if (obj == null || obj is ContentInfo)
			{
				return (ContentInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new ContentInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj));
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x0018F5B1 File Offset: 0x0018D7B1
		public static ContentInfo GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return ContentInfo.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x0018F5C0 File Offset: 0x0018D7C0
		private ContentInfo(Asn1Sequence seq)
		{
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.contentType = (DerObjectIdentifier)seq[0];
			if (seq.Count > 1)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[1];
				if (!asn1TaggedObject.IsExplicit() || asn1TaggedObject.TagNo != 0)
				{
					throw new ArgumentException("Bad tag for 'content'", "seq");
				}
				this.content = asn1TaggedObject.GetObject();
			}
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x0018F659 File Offset: 0x0018D859
		public ContentInfo(DerObjectIdentifier contentType, Asn1Encodable content)
		{
			this.contentType = contentType;
			this.content = content;
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x060044FC RID: 17660 RVA: 0x0018F66F File Offset: 0x0018D86F
		public DerObjectIdentifier ContentType
		{
			get
			{
				return this.contentType;
			}
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x060044FD RID: 17661 RVA: 0x0018F677 File Offset: 0x0018D877
		public Asn1Encodable Content
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x0018F680 File Offset: 0x0018D880
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

		// Token: 0x04002D2F RID: 11567
		private readonly DerObjectIdentifier contentType;

		// Token: 0x04002D30 RID: 11568
		private readonly Asn1Encodable content;
	}
}
