using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ess
{
	// Token: 0x02000739 RID: 1849
	public class ContentHints : Asn1Encodable
	{
		// Token: 0x060042EC RID: 17132 RVA: 0x001886C0 File Offset: 0x001868C0
		public static ContentHints GetInstance(object o)
		{
			if (o == null || o is ContentHints)
			{
				return (ContentHints)o;
			}
			if (o is Asn1Sequence)
			{
				return new ContentHints((Asn1Sequence)o);
			}
			throw new ArgumentException("unknown object in 'ContentHints' factory : " + Platform.GetTypeName(o) + ".");
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x00188710 File Offset: 0x00186910
		private ContentHints(Asn1Sequence seq)
		{
			IAsn1Convertible asn1Convertible = seq[0];
			if (asn1Convertible.ToAsn1Object() is DerUtf8String)
			{
				this.contentDescription = DerUtf8String.GetInstance(asn1Convertible);
				this.contentType = DerObjectIdentifier.GetInstance(seq[1]);
				return;
			}
			this.contentType = DerObjectIdentifier.GetInstance(seq[0]);
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x00188769 File Offset: 0x00186969
		public ContentHints(DerObjectIdentifier contentType)
		{
			this.contentType = contentType;
			this.contentDescription = null;
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x0018877F File Offset: 0x0018697F
		public ContentHints(DerObjectIdentifier contentType, DerUtf8String contentDescription)
		{
			this.contentType = contentType;
			this.contentDescription = contentDescription;
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060042F0 RID: 17136 RVA: 0x00188795 File Offset: 0x00186995
		public DerObjectIdentifier ContentType
		{
			get
			{
				return this.contentType;
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060042F1 RID: 17137 RVA: 0x0018879D File Offset: 0x0018699D
		public DerUtf8String ContentDescription
		{
			get
			{
				return this.contentDescription;
			}
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x001887A8 File Offset: 0x001869A8
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.contentDescription != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.contentDescription
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.contentType
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002C0A RID: 11274
		private readonly DerUtf8String contentDescription;

		// Token: 0x04002C0B RID: 11275
		private readonly DerObjectIdentifier contentType;
	}
}
