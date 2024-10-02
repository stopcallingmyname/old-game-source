using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x020007A2 RID: 1954
	public class SignerIdentifier : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x060045DF RID: 17887 RVA: 0x00191B44 File Offset: 0x0018FD44
		public SignerIdentifier(IssuerAndSerialNumber id)
		{
			this.id = id;
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x00191B53 File Offset: 0x0018FD53
		public SignerIdentifier(Asn1OctetString id)
		{
			this.id = new DerTaggedObject(false, 0, id);
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x00191B44 File Offset: 0x0018FD44
		public SignerIdentifier(Asn1Object id)
		{
			this.id = id;
		}

		// Token: 0x060045E2 RID: 17890 RVA: 0x00191B6C File Offset: 0x0018FD6C
		public static SignerIdentifier GetInstance(object o)
		{
			if (o == null || o is SignerIdentifier)
			{
				return (SignerIdentifier)o;
			}
			if (o is IssuerAndSerialNumber)
			{
				return new SignerIdentifier((IssuerAndSerialNumber)o);
			}
			if (o is Asn1OctetString)
			{
				return new SignerIdentifier((Asn1OctetString)o);
			}
			if (o is Asn1Object)
			{
				return new SignerIdentifier((Asn1Object)o);
			}
			throw new ArgumentException("Illegal object in SignerIdentifier: " + Platform.GetTypeName(o));
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x060045E3 RID: 17891 RVA: 0x00191BDC File Offset: 0x0018FDDC
		public bool IsTagged
		{
			get
			{
				return this.id is Asn1TaggedObject;
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x060045E4 RID: 17892 RVA: 0x00191BEC File Offset: 0x0018FDEC
		public Asn1Encodable ID
		{
			get
			{
				if (this.id is Asn1TaggedObject)
				{
					return Asn1OctetString.GetInstance((Asn1TaggedObject)this.id, false);
				}
				return this.id;
			}
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x00191C13 File Offset: 0x0018FE13
		public override Asn1Object ToAsn1Object()
		{
			return this.id.ToAsn1Object();
		}

		// Token: 0x04002D87 RID: 11655
		private Asn1Encodable id;
	}
}
