using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X500;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.IsisMtt.X509
{
	// Token: 0x0200072D RID: 1837
	public class Restriction : Asn1Encodable
	{
		// Token: 0x060042AA RID: 17066 RVA: 0x00187548 File Offset: 0x00185748
		public static Restriction GetInstance(object obj)
		{
			if (obj is Restriction)
			{
				return (Restriction)obj;
			}
			if (obj is IAsn1String)
			{
				return new Restriction(DirectoryString.GetInstance(obj));
			}
			throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x00187587 File Offset: 0x00185787
		private Restriction(DirectoryString restriction)
		{
			this.restriction = restriction;
		}

		// Token: 0x060042AC RID: 17068 RVA: 0x00187596 File Offset: 0x00185796
		public Restriction(string restriction)
		{
			this.restriction = new DirectoryString(restriction);
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x060042AD RID: 17069 RVA: 0x001875AA File Offset: 0x001857AA
		public virtual DirectoryString RestrictionString
		{
			get
			{
				return this.restriction;
			}
		}

		// Token: 0x060042AE RID: 17070 RVA: 0x001875B2 File Offset: 0x001857B2
		public override Asn1Object ToAsn1Object()
		{
			return this.restriction.ToAsn1Object();
		}

		// Token: 0x04002B8D RID: 11149
		private readonly DirectoryString restriction;
	}
}
