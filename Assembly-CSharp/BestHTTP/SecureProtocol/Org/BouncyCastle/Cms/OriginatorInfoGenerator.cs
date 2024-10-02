using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000611 RID: 1553
	public class OriginatorInfoGenerator
	{
		// Token: 0x06003AD8 RID: 15064 RVA: 0x0016C958 File Offset: 0x0016AB58
		public OriginatorInfoGenerator(X509Certificate origCert)
		{
			this.origCerts = Platform.CreateArrayList(1);
			this.origCrls = null;
			this.origCerts.Add(origCert.CertificateStructure);
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x0016C985 File Offset: 0x0016AB85
		public OriginatorInfoGenerator(IX509Store origCerts) : this(origCerts, null)
		{
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x0016C98F File Offset: 0x0016AB8F
		public OriginatorInfoGenerator(IX509Store origCerts, IX509Store origCrls)
		{
			this.origCerts = CmsUtilities.GetCertificatesFromStore(origCerts);
			this.origCrls = ((origCrls == null) ? null : CmsUtilities.GetCrlsFromStore(origCrls));
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x0016C9B8 File Offset: 0x0016ABB8
		public virtual OriginatorInfo Generate()
		{
			Asn1Set certs = CmsUtilities.CreateDerSetFromList(this.origCerts);
			Asn1Set crls = (this.origCrls == null) ? null : CmsUtilities.CreateDerSetFromList(this.origCrls);
			return new OriginatorInfo(certs, crls);
		}

		// Token: 0x0400266B RID: 9835
		private readonly IList origCerts;

		// Token: 0x0400266C RID: 9836
		private readonly IList origCrls;
	}
}
