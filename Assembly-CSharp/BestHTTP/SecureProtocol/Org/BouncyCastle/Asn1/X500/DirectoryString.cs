using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X500
{
	// Token: 0x020006D5 RID: 1749
	public class DirectoryString : Asn1Encodable, IAsn1Choice, IAsn1String
	{
		// Token: 0x06004061 RID: 16481 RVA: 0x0017E41C File Offset: 0x0017C61C
		public static DirectoryString GetInstance(object obj)
		{
			if (obj == null || obj is DirectoryString)
			{
				return (DirectoryString)obj;
			}
			if (obj is DerStringBase && (obj is DerT61String || obj is DerPrintableString || obj is DerUniversalString || obj is DerUtf8String || obj is DerBmpString))
			{
				return new DirectoryString((DerStringBase)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x0017E491 File Offset: 0x0017C691
		public static DirectoryString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			if (!isExplicit)
			{
				throw new ArgumentException("choice item must be explicitly tagged");
			}
			return DirectoryString.GetInstance(obj.GetObject());
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x0017E4AC File Offset: 0x0017C6AC
		private DirectoryString(DerStringBase str)
		{
			this.str = str;
		}

		// Token: 0x06004064 RID: 16484 RVA: 0x0017E4BB File Offset: 0x0017C6BB
		public DirectoryString(string str)
		{
			this.str = new DerUtf8String(str);
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x0017E4CF File Offset: 0x0017C6CF
		public string GetString()
		{
			return this.str.GetString();
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x0017E4DC File Offset: 0x0017C6DC
		public override Asn1Object ToAsn1Object()
		{
			return this.str.ToAsn1Object();
		}

		// Token: 0x040028F8 RID: 10488
		private readonly DerStringBase str;
	}
}
