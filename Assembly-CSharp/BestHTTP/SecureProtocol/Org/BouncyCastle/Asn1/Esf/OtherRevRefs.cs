using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000752 RID: 1874
	public class OtherRevRefs : Asn1Encodable
	{
		// Token: 0x0600438E RID: 17294 RVA: 0x0018AA7C File Offset: 0x00188C7C
		public static OtherRevRefs GetInstance(object obj)
		{
			if (obj == null || obj is OtherRevRefs)
			{
				return (OtherRevRefs)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OtherRevRefs((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'OtherRevRefs' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x0018AACC File Offset: 0x00188CCC
		private OtherRevRefs(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.otherRevRefType = (DerObjectIdentifier)seq[0].ToAsn1Object();
			this.otherRevRefs = seq[1].ToAsn1Object();
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x0018AB3F File Offset: 0x00188D3F
		public OtherRevRefs(DerObjectIdentifier otherRevRefType, Asn1Encodable otherRevRefs)
		{
			if (otherRevRefType == null)
			{
				throw new ArgumentNullException("otherRevRefType");
			}
			if (otherRevRefs == null)
			{
				throw new ArgumentNullException("otherRevRefs");
			}
			this.otherRevRefType = otherRevRefType;
			this.otherRevRefs = otherRevRefs.ToAsn1Object();
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06004391 RID: 17297 RVA: 0x0018AB76 File Offset: 0x00188D76
		public DerObjectIdentifier OtherRevRefType
		{
			get
			{
				return this.otherRevRefType;
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06004392 RID: 17298 RVA: 0x0018AB7E File Offset: 0x00188D7E
		public Asn1Object OtherRevRefsObject
		{
			get
			{
				return this.otherRevRefs;
			}
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x0018AB86 File Offset: 0x00188D86
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.otherRevRefType,
				this.otherRevRefs
			});
		}

		// Token: 0x04002C4A RID: 11338
		private readonly DerObjectIdentifier otherRevRefType;

		// Token: 0x04002C4B RID: 11339
		private readonly Asn1Object otherRevRefs;
	}
}
