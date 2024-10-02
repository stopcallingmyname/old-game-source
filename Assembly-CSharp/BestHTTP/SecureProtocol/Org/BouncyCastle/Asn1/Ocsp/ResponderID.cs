using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x02000710 RID: 1808
	public class ResponderID : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x060041F8 RID: 16888 RVA: 0x001848E8 File Offset: 0x00182AE8
		public static ResponderID GetInstance(object obj)
		{
			if (obj == null || obj is ResponderID)
			{
				return (ResponderID)obj;
			}
			if (obj is DerOctetString)
			{
				return new ResponderID((DerOctetString)obj);
			}
			if (!(obj is Asn1TaggedObject))
			{
				return new ResponderID(X509Name.GetInstance(obj));
			}
			Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
			if (asn1TaggedObject.TagNo == 1)
			{
				return new ResponderID(X509Name.GetInstance(asn1TaggedObject, true));
			}
			return new ResponderID(Asn1OctetString.GetInstance(asn1TaggedObject, true));
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x00184958 File Offset: 0x00182B58
		public ResponderID(Asn1OctetString id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			this.id = id;
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x00184958 File Offset: 0x00182B58
		public ResponderID(X509Name id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			this.id = id;
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x00184975 File Offset: 0x00182B75
		public static ResponderID GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return ResponderID.GetInstance(obj.GetObject());
		}

		// Token: 0x060041FC RID: 16892 RVA: 0x00184982 File Offset: 0x00182B82
		public virtual byte[] GetKeyHash()
		{
			if (this.id is Asn1OctetString)
			{
				return ((Asn1OctetString)this.id).GetOctets();
			}
			return null;
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x060041FD RID: 16893 RVA: 0x001849A3 File Offset: 0x00182BA3
		public virtual X509Name Name
		{
			get
			{
				if (this.id is Asn1OctetString)
				{
					return null;
				}
				return X509Name.GetInstance(this.id);
			}
		}

		// Token: 0x060041FE RID: 16894 RVA: 0x001849BF File Offset: 0x00182BBF
		public override Asn1Object ToAsn1Object()
		{
			if (this.id is Asn1OctetString)
			{
				return new DerTaggedObject(true, 2, this.id);
			}
			return new DerTaggedObject(true, 1, this.id);
		}

		// Token: 0x04002AC2 RID: 10946
		private readonly Asn1Encodable id;
	}
}
