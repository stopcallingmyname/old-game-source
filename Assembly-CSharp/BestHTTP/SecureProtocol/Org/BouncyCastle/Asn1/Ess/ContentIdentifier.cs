using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ess
{
	// Token: 0x0200073A RID: 1850
	public class ContentIdentifier : Asn1Encodable
	{
		// Token: 0x060042F3 RID: 17139 RVA: 0x001887F8 File Offset: 0x001869F8
		public static ContentIdentifier GetInstance(object o)
		{
			if (o == null || o is ContentIdentifier)
			{
				return (ContentIdentifier)o;
			}
			if (o is Asn1OctetString)
			{
				return new ContentIdentifier((Asn1OctetString)o);
			}
			throw new ArgumentException("unknown object in 'ContentIdentifier' factory : " + Platform.GetTypeName(o) + ".");
		}

		// Token: 0x060042F4 RID: 17140 RVA: 0x00188845 File Offset: 0x00186A45
		public ContentIdentifier(Asn1OctetString value)
		{
			this.value = value;
		}

		// Token: 0x060042F5 RID: 17141 RVA: 0x00188854 File Offset: 0x00186A54
		public ContentIdentifier(byte[] value) : this(new DerOctetString(value))
		{
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060042F6 RID: 17142 RVA: 0x00188862 File Offset: 0x00186A62
		public Asn1OctetString Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x00188862 File Offset: 0x00186A62
		public override Asn1Object ToAsn1Object()
		{
			return this.value;
		}

		// Token: 0x04002C0C RID: 11276
		private Asn1OctetString value;
	}
}
