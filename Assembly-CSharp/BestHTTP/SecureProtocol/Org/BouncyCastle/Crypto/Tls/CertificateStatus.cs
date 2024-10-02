using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000404 RID: 1028
	public class CertificateStatus
	{
		// Token: 0x06002984 RID: 10628 RVA: 0x0010E815 File Offset: 0x0010CA15
		public CertificateStatus(byte statusType, object response)
		{
			if (!CertificateStatus.IsCorrectType(statusType, response))
			{
				throw new ArgumentException("not an instance of the correct type", "response");
			}
			this.mStatusType = statusType;
			this.mResponse = response;
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06002985 RID: 10629 RVA: 0x0010E844 File Offset: 0x0010CA44
		public virtual byte StatusType
		{
			get
			{
				return this.mStatusType;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06002986 RID: 10630 RVA: 0x0010E84C File Offset: 0x0010CA4C
		public virtual object Response
		{
			get
			{
				return this.mResponse;
			}
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x0010E854 File Offset: 0x0010CA54
		public virtual OcspResponse GetOcspResponse()
		{
			if (!CertificateStatus.IsCorrectType(1, this.mResponse))
			{
				throw new InvalidOperationException("'response' is not an OcspResponse");
			}
			return (OcspResponse)this.mResponse;
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x0010E87C File Offset: 0x0010CA7C
		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint8(this.mStatusType, output);
			byte b = this.mStatusType;
			if (b == 1)
			{
				TlsUtilities.WriteOpaque24(((OcspResponse)this.mResponse).GetEncoded("DER"), output);
				return;
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x0010E8C4 File Offset: 0x0010CAC4
		public static CertificateStatus Parse(Stream input)
		{
			byte b = TlsUtilities.ReadUint8(input);
			if (b == 1)
			{
				object instance = OcspResponse.GetInstance(TlsUtilities.ReadDerObject(TlsUtilities.ReadOpaque24(input)));
				return new CertificateStatus(b, instance);
			}
			throw new TlsFatalAlert(50);
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x0010E8FE File Offset: 0x0010CAFE
		protected static bool IsCorrectType(byte statusType, object response)
		{
			if (statusType == 1)
			{
				return response is OcspResponse;
			}
			throw new ArgumentException("unsupported CertificateStatusType", "statusType");
		}

		// Token: 0x04001B6C RID: 7020
		protected readonly byte mStatusType;

		// Token: 0x04001B6D RID: 7021
		protected readonly object mResponse;
	}
}
