using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp
{
	// Token: 0x020002FF RID: 767
	public class Req : X509ExtensionBase
	{
		// Token: 0x06001BD8 RID: 7128 RVA: 0x000D184D File Offset: 0x000CFA4D
		public Req(Request req)
		{
			this.req = req;
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x000D185C File Offset: 0x000CFA5C
		public CertificateID GetCertID()
		{
			return new CertificateID(this.req.ReqCert);
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06001BDA RID: 7130 RVA: 0x000D186E File Offset: 0x000CFA6E
		public X509Extensions SingleRequestExtensions
		{
			get
			{
				return this.req.SingleRequestExtensions;
			}
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x000D187B File Offset: 0x000CFA7B
		protected override X509Extensions GetX509Extensions()
		{
			return this.SingleRequestExtensions;
		}

		// Token: 0x04001911 RID: 6417
		private Request req;
	}
}
