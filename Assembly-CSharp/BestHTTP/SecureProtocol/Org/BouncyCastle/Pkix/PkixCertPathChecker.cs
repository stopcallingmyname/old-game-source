using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002B8 RID: 696
	public abstract class PkixCertPathChecker
	{
		// Token: 0x0600192F RID: 6447
		public abstract void Init(bool forward);

		// Token: 0x06001930 RID: 6448
		public abstract bool IsForwardCheckingSupported();

		// Token: 0x06001931 RID: 6449
		public abstract ISet GetSupportedExtensions();

		// Token: 0x06001932 RID: 6450
		public abstract void Check(X509Certificate cert, ISet unresolvedCritExts);

		// Token: 0x06001933 RID: 6451 RVA: 0x000BC9DB File Offset: 0x000BABDB
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}
	}
}
