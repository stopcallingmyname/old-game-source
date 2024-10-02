using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000662 RID: 1634
	public abstract class DerStringBase : Asn1Object, IAsn1String
	{
		// Token: 0x06003D04 RID: 15620
		public abstract string GetString();

		// Token: 0x06003D05 RID: 15621 RVA: 0x00172EF5 File Offset: 0x001710F5
		public override string ToString()
		{
			return this.GetString();
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x00172EFD File Offset: 0x001710FD
		protected override int Asn1GetHashCode()
		{
			return this.GetString().GetHashCode();
		}
	}
}
