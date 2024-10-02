using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002BA RID: 698
	[Serializable]
	public class PkixCertPathValidatorException : GeneralSecurityException
	{
		// Token: 0x06001937 RID: 6455 RVA: 0x000BD034 File Offset: 0x000BB234
		public PkixCertPathValidatorException()
		{
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x000BD043 File Offset: 0x000BB243
		public PkixCertPathValidatorException(string message) : base(message)
		{
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x000BD053 File Offset: 0x000BB253
		public PkixCertPathValidatorException(string message, Exception cause) : base(message)
		{
			this.cause = cause;
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x000BD06C File Offset: 0x000BB26C
		public PkixCertPathValidatorException(string message, Exception cause, PkixCertPath certPath, int index) : base(message)
		{
			if (certPath == null && index != -1)
			{
				throw new ArgumentNullException("certPath = null and index != -1");
			}
			if (index < -1 || (certPath != null && index >= certPath.Certificates.Count))
			{
				throw new IndexOutOfRangeException(" index < -1 or out of bound of certPath.getCertificates()");
			}
			this.cause = cause;
			this.certPath = certPath;
			this.index = index;
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x0600193B RID: 6459 RVA: 0x000BD0D4 File Offset: 0x000BB2D4
		public override string Message
		{
			get
			{
				string message = base.Message;
				if (message != null)
				{
					return message;
				}
				if (this.cause != null)
				{
					return this.cause.Message;
				}
				return null;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600193C RID: 6460 RVA: 0x000BD102 File Offset: 0x000BB302
		public PkixCertPath CertPath
		{
			get
			{
				return this.certPath;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x0600193D RID: 6461 RVA: 0x000BD10A File Offset: 0x000BB30A
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x04001876 RID: 6262
		private Exception cause;

		// Token: 0x04001877 RID: 6263
		private PkixCertPath certPath;

		// Token: 0x04001878 RID: 6264
		private int index = -1;
	}
}
