using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs
{
	// Token: 0x020002CF RID: 719
	public class X509CertificateEntry : Pkcs12Entry
	{
		// Token: 0x06001A74 RID: 6772 RVA: 0x000C6BBD File Offset: 0x000C4DBD
		public X509CertificateEntry(X509Certificate cert) : base(Platform.CreateHashtable())
		{
			this.cert = cert;
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x000C6BD1 File Offset: 0x000C4DD1
		[Obsolete]
		public X509CertificateEntry(X509Certificate cert, Hashtable attributes) : base(attributes)
		{
			this.cert = cert;
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x000C6BD1 File Offset: 0x000C4DD1
		public X509CertificateEntry(X509Certificate cert, IDictionary attributes) : base(attributes)
		{
			this.cert = cert;
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06001A77 RID: 6775 RVA: 0x000C6BE1 File Offset: 0x000C4DE1
		public X509Certificate Certificate
		{
			get
			{
				return this.cert;
			}
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x000C6BEC File Offset: 0x000C4DEC
		public override bool Equals(object obj)
		{
			X509CertificateEntry x509CertificateEntry = obj as X509CertificateEntry;
			return x509CertificateEntry != null && this.cert.Equals(x509CertificateEntry.cert);
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x000C6C16 File Offset: 0x000C4E16
		public override int GetHashCode()
		{
			return ~this.cert.GetHashCode();
		}

		// Token: 0x040018CB RID: 6347
		private readonly X509Certificate cert;
	}
}
