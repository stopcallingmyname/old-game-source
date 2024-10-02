using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200079D RID: 1949
	public class RecipientInfo : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x060045AD RID: 17837 RVA: 0x00191041 File Offset: 0x0018F241
		public RecipientInfo(KeyTransRecipientInfo info)
		{
			this.info = info;
		}

		// Token: 0x060045AE RID: 17838 RVA: 0x00191050 File Offset: 0x0018F250
		public RecipientInfo(KeyAgreeRecipientInfo info)
		{
			this.info = new DerTaggedObject(false, 1, info);
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x00191066 File Offset: 0x0018F266
		public RecipientInfo(KekRecipientInfo info)
		{
			this.info = new DerTaggedObject(false, 2, info);
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x0019107C File Offset: 0x0018F27C
		public RecipientInfo(PasswordRecipientInfo info)
		{
			this.info = new DerTaggedObject(false, 3, info);
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x00191092 File Offset: 0x0018F292
		public RecipientInfo(OtherRecipientInfo info)
		{
			this.info = new DerTaggedObject(false, 4, info);
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x00191041 File Offset: 0x0018F241
		public RecipientInfo(Asn1Object info)
		{
			this.info = info;
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x001910A8 File Offset: 0x0018F2A8
		public static RecipientInfo GetInstance(object o)
		{
			if (o == null || o is RecipientInfo)
			{
				return (RecipientInfo)o;
			}
			if (o is Asn1Sequence)
			{
				return new RecipientInfo((Asn1Sequence)o);
			}
			if (o is Asn1TaggedObject)
			{
				return new RecipientInfo((Asn1TaggedObject)o);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(o));
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x060045B4 RID: 17844 RVA: 0x00191104 File Offset: 0x0018F304
		public DerInteger Version
		{
			get
			{
				if (!(this.info is Asn1TaggedObject))
				{
					return KeyTransRecipientInfo.GetInstance(this.info).Version;
				}
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)this.info;
				switch (asn1TaggedObject.TagNo)
				{
				case 1:
					return KeyAgreeRecipientInfo.GetInstance(asn1TaggedObject, false).Version;
				case 2:
					return this.GetKekInfo(asn1TaggedObject).Version;
				case 3:
					return PasswordRecipientInfo.GetInstance(asn1TaggedObject, false).Version;
				case 4:
					return new DerInteger(0);
				default:
					throw new InvalidOperationException("unknown tag");
				}
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x060045B5 RID: 17845 RVA: 0x00191194 File Offset: 0x0018F394
		public bool IsTagged
		{
			get
			{
				return this.info is Asn1TaggedObject;
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x060045B6 RID: 17846 RVA: 0x001911A4 File Offset: 0x0018F3A4
		public Asn1Encodable Info
		{
			get
			{
				if (!(this.info is Asn1TaggedObject))
				{
					return KeyTransRecipientInfo.GetInstance(this.info);
				}
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)this.info;
				switch (asn1TaggedObject.TagNo)
				{
				case 1:
					return KeyAgreeRecipientInfo.GetInstance(asn1TaggedObject, false);
				case 2:
					return this.GetKekInfo(asn1TaggedObject);
				case 3:
					return PasswordRecipientInfo.GetInstance(asn1TaggedObject, false);
				case 4:
					return OtherRecipientInfo.GetInstance(asn1TaggedObject, false);
				default:
					throw new InvalidOperationException("unknown tag");
				}
			}
		}

		// Token: 0x060045B7 RID: 17847 RVA: 0x00191221 File Offset: 0x0018F421
		private KekRecipientInfo GetKekInfo(Asn1TaggedObject o)
		{
			return KekRecipientInfo.GetInstance(o, o.IsExplicit());
		}

		// Token: 0x060045B8 RID: 17848 RVA: 0x0019122F File Offset: 0x0018F42F
		public override Asn1Object ToAsn1Object()
		{
			return this.info.ToAsn1Object();
		}

		// Token: 0x04002D70 RID: 11632
		internal Asn1Encodable info;
	}
}
