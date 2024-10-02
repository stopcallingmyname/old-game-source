using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000794 RID: 1940
	public class OriginatorIdentifierOrKey : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06004565 RID: 17765 RVA: 0x00190704 File Offset: 0x0018E904
		public OriginatorIdentifierOrKey(IssuerAndSerialNumber id)
		{
			this.id = id;
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x00190713 File Offset: 0x0018E913
		[Obsolete("Use version taking a 'SubjectKeyIdentifier'")]
		public OriginatorIdentifierOrKey(Asn1OctetString id) : this(new SubjectKeyIdentifier(id))
		{
		}

		// Token: 0x06004567 RID: 17767 RVA: 0x00190721 File Offset: 0x0018E921
		public OriginatorIdentifierOrKey(SubjectKeyIdentifier id)
		{
			this.id = new DerTaggedObject(false, 0, id);
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x00190737 File Offset: 0x0018E937
		public OriginatorIdentifierOrKey(OriginatorPublicKey id)
		{
			this.id = new DerTaggedObject(false, 1, id);
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x00190704 File Offset: 0x0018E904
		[Obsolete("Use more specific version")]
		public OriginatorIdentifierOrKey(Asn1Object id)
		{
			this.id = id;
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x00190704 File Offset: 0x0018E904
		private OriginatorIdentifierOrKey(Asn1TaggedObject id)
		{
			this.id = id;
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x0019074D File Offset: 0x0018E94D
		public static OriginatorIdentifierOrKey GetInstance(Asn1TaggedObject o, bool explicitly)
		{
			if (!explicitly)
			{
				throw new ArgumentException("Can't implicitly tag OriginatorIdentifierOrKey");
			}
			return OriginatorIdentifierOrKey.GetInstance(o.GetObject());
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x00190768 File Offset: 0x0018E968
		public static OriginatorIdentifierOrKey GetInstance(object o)
		{
			if (o == null || o is OriginatorIdentifierOrKey)
			{
				return (OriginatorIdentifierOrKey)o;
			}
			if (o is IssuerAndSerialNumber)
			{
				return new OriginatorIdentifierOrKey((IssuerAndSerialNumber)o);
			}
			if (o is SubjectKeyIdentifier)
			{
				return new OriginatorIdentifierOrKey((SubjectKeyIdentifier)o);
			}
			if (o is OriginatorPublicKey)
			{
				return new OriginatorIdentifierOrKey((OriginatorPublicKey)o);
			}
			if (o is Asn1TaggedObject)
			{
				return new OriginatorIdentifierOrKey((Asn1TaggedObject)o);
			}
			throw new ArgumentException("Invalid OriginatorIdentifierOrKey: " + Platform.GetTypeName(o));
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x0600456D RID: 17773 RVA: 0x001907EC File Offset: 0x0018E9EC
		public Asn1Encodable ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x0600456E RID: 17774 RVA: 0x001907F4 File Offset: 0x0018E9F4
		public IssuerAndSerialNumber IssuerAndSerialNumber
		{
			get
			{
				if (this.id is IssuerAndSerialNumber)
				{
					return (IssuerAndSerialNumber)this.id;
				}
				return null;
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x0600456F RID: 17775 RVA: 0x00190810 File Offset: 0x0018EA10
		public SubjectKeyIdentifier SubjectKeyIdentifier
		{
			get
			{
				if (this.id is Asn1TaggedObject && ((Asn1TaggedObject)this.id).TagNo == 0)
				{
					return SubjectKeyIdentifier.GetInstance((Asn1TaggedObject)this.id, false);
				}
				return null;
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x06004570 RID: 17776 RVA: 0x00190844 File Offset: 0x0018EA44
		[Obsolete("Use 'OriginatorPublicKey' property")]
		public OriginatorPublicKey OriginatorKey
		{
			get
			{
				return this.OriginatorPublicKey;
			}
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06004571 RID: 17777 RVA: 0x0019084C File Offset: 0x0018EA4C
		public OriginatorPublicKey OriginatorPublicKey
		{
			get
			{
				if (this.id is Asn1TaggedObject && ((Asn1TaggedObject)this.id).TagNo == 1)
				{
					return OriginatorPublicKey.GetInstance((Asn1TaggedObject)this.id, false);
				}
				return null;
			}
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x00190881 File Offset: 0x0018EA81
		public override Asn1Object ToAsn1Object()
		{
			return this.id.ToAsn1Object();
		}

		// Token: 0x04002D5E RID: 11614
		private Asn1Encodable id;
	}
}
