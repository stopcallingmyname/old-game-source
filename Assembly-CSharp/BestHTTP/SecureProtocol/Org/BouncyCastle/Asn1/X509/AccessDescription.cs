using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000684 RID: 1668
	public class AccessDescription : Asn1Encodable
	{
		// Token: 0x06003DD0 RID: 15824 RVA: 0x0017552C File Offset: 0x0017372C
		public static AccessDescription GetInstance(object obj)
		{
			if (obj is AccessDescription)
			{
				return (AccessDescription)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new AccessDescription((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x0017556B File Offset: 0x0017376B
		private AccessDescription(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("wrong number of elements in sequence");
			}
			this.accessMethod = DerObjectIdentifier.GetInstance(seq[0]);
			this.accessLocation = GeneralName.GetInstance(seq[1]);
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x001755AB File Offset: 0x001737AB
		public AccessDescription(DerObjectIdentifier oid, GeneralName location)
		{
			this.accessMethod = oid;
			this.accessLocation = location;
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06003DD3 RID: 15827 RVA: 0x001755C1 File Offset: 0x001737C1
		public DerObjectIdentifier AccessMethod
		{
			get
			{
				return this.accessMethod;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06003DD4 RID: 15828 RVA: 0x001755C9 File Offset: 0x001737C9
		public GeneralName AccessLocation
		{
			get
			{
				return this.accessLocation;
			}
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x001755D1 File Offset: 0x001737D1
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.accessMethod,
				this.accessLocation
			});
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x001755F0 File Offset: 0x001737F0
		public override string ToString()
		{
			return "AccessDescription: Oid(" + this.accessMethod.Id + ")";
		}

		// Token: 0x04002772 RID: 10098
		public static readonly DerObjectIdentifier IdADCAIssuers = new DerObjectIdentifier("1.3.6.1.5.5.7.48.2");

		// Token: 0x04002773 RID: 10099
		public static readonly DerObjectIdentifier IdADOcsp = new DerObjectIdentifier("1.3.6.1.5.5.7.48.1");

		// Token: 0x04002774 RID: 10100
		private readonly DerObjectIdentifier accessMethod;

		// Token: 0x04002775 RID: 10101
		private readonly GeneralName accessLocation;
	}
}
