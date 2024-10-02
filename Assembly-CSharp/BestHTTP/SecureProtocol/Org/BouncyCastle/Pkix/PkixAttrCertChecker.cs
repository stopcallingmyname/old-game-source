using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002B0 RID: 688
	public abstract class PkixAttrCertChecker
	{
		// Token: 0x06001906 RID: 6406
		public abstract ISet GetSupportedExtensions();

		// Token: 0x06001907 RID: 6407
		public abstract void Check(IX509AttributeCertificate attrCert, PkixCertPath certPath, PkixCertPath holderCertPath, ICollection unresolvedCritExts);

		// Token: 0x06001908 RID: 6408
		public abstract PkixAttrCertChecker Clone();
	}
}
