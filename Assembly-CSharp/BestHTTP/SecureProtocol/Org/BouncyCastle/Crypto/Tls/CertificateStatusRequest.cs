using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000405 RID: 1029
	public class CertificateStatusRequest
	{
		// Token: 0x0600298B RID: 10635 RVA: 0x0010E91D File Offset: 0x0010CB1D
		public CertificateStatusRequest(byte statusType, object request)
		{
			if (!CertificateStatusRequest.IsCorrectType(statusType, request))
			{
				throw new ArgumentException("not an instance of the correct type", "request");
			}
			this.mStatusType = statusType;
			this.mRequest = request;
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600298C RID: 10636 RVA: 0x0010E94C File Offset: 0x0010CB4C
		public virtual byte StatusType
		{
			get
			{
				return this.mStatusType;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x0600298D RID: 10637 RVA: 0x0010E954 File Offset: 0x0010CB54
		public virtual object Request
		{
			get
			{
				return this.mRequest;
			}
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x0010E95C File Offset: 0x0010CB5C
		public virtual OcspStatusRequest GetOcspStatusRequest()
		{
			if (!CertificateStatusRequest.IsCorrectType(1, this.mRequest))
			{
				throw new InvalidOperationException("'request' is not an OCSPStatusRequest");
			}
			return (OcspStatusRequest)this.mRequest;
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x0010E984 File Offset: 0x0010CB84
		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint8(this.mStatusType, output);
			byte b = this.mStatusType;
			if (b == 1)
			{
				((OcspStatusRequest)this.mRequest).Encode(output);
				return;
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x0010E9C4 File Offset: 0x0010CBC4
		public static CertificateStatusRequest Parse(Stream input)
		{
			byte b = TlsUtilities.ReadUint8(input);
			if (b == 1)
			{
				object request = OcspStatusRequest.Parse(input);
				return new CertificateStatusRequest(b, request);
			}
			throw new TlsFatalAlert(50);
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x0010E9F4 File Offset: 0x0010CBF4
		protected static bool IsCorrectType(byte statusType, object request)
		{
			if (statusType == 1)
			{
				return request is OcspStatusRequest;
			}
			throw new ArgumentException("unsupported CertificateStatusType", "statusType");
		}

		// Token: 0x04001B6E RID: 7022
		protected readonly byte mStatusType;

		// Token: 0x04001B6F RID: 7023
		protected readonly object mRequest;
	}
}
