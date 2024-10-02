using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200079F RID: 1951
	public class ScvpReqRes : Asn1Encodable
	{
		// Token: 0x060045C3 RID: 17859 RVA: 0x001913DA File Offset: 0x0018F5DA
		public static ScvpReqRes GetInstance(object obj)
		{
			if (obj is ScvpReqRes)
			{
				return (ScvpReqRes)obj;
			}
			if (obj != null)
			{
				return new ScvpReqRes(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x001913FC File Offset: 0x0018F5FC
		private ScvpReqRes(Asn1Sequence seq)
		{
			if (seq[0] is Asn1TaggedObject)
			{
				this.request = ContentInfo.GetInstance(Asn1TaggedObject.GetInstance(seq[0]), true);
				this.response = ContentInfo.GetInstance(seq[1]);
				return;
			}
			this.request = null;
			this.response = ContentInfo.GetInstance(seq[0]);
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x00191461 File Offset: 0x0018F661
		public ScvpReqRes(ContentInfo response) : this(null, response)
		{
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x0019146B File Offset: 0x0018F66B
		public ScvpReqRes(ContentInfo request, ContentInfo response)
		{
			this.request = request;
			this.response = response;
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x060045C7 RID: 17863 RVA: 0x00191481 File Offset: 0x0018F681
		public virtual ContentInfo Request
		{
			get
			{
				return this.request;
			}
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x060045C8 RID: 17864 RVA: 0x00191489 File Offset: 0x0018F689
		public virtual ContentInfo Response
		{
			get
			{
				return this.response;
			}
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x00191494 File Offset: 0x0018F694
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.request != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.request)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.response
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002D74 RID: 11636
		private readonly ContentInfo request;

		// Token: 0x04002D75 RID: 11637
		private readonly ContentInfo response;
	}
}
