using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000797 RID: 1943
	public class OtherKeyAttribute : Asn1Encodable
	{
		// Token: 0x06004581 RID: 17793 RVA: 0x00190AE8 File Offset: 0x0018ECE8
		public static OtherKeyAttribute GetInstance(object obj)
		{
			if (obj == null || obj is OtherKeyAttribute)
			{
				return (OtherKeyAttribute)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OtherKeyAttribute((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x00190B35 File Offset: 0x0018ED35
		public OtherKeyAttribute(Asn1Sequence seq)
		{
			this.keyAttrId = (DerObjectIdentifier)seq[0];
			this.keyAttr = seq[1];
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x00190B5C File Offset: 0x0018ED5C
		public OtherKeyAttribute(DerObjectIdentifier keyAttrId, Asn1Encodable keyAttr)
		{
			this.keyAttrId = keyAttrId;
			this.keyAttr = keyAttr;
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x06004584 RID: 17796 RVA: 0x00190B72 File Offset: 0x0018ED72
		public DerObjectIdentifier KeyAttrId
		{
			get
			{
				return this.keyAttrId;
			}
		}

		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x06004585 RID: 17797 RVA: 0x00190B7A File Offset: 0x0018ED7A
		public Asn1Encodable KeyAttr
		{
			get
			{
				return this.keyAttr;
			}
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x00190B82 File Offset: 0x0018ED82
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.keyAttrId,
				this.keyAttr
			});
		}

		// Token: 0x04002D63 RID: 11619
		private DerObjectIdentifier keyAttrId;

		// Token: 0x04002D64 RID: 11620
		private Asn1Encodable keyAttr;
	}
}
