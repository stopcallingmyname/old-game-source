using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp
{
	// Token: 0x02000303 RID: 771
	public class SingleResp : X509ExtensionBase
	{
		// Token: 0x06001BEE RID: 7150 RVA: 0x000D1A7C File Offset: 0x000CFC7C
		public SingleResp(SingleResponse resp)
		{
			this.resp = resp;
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x000D1A8B File Offset: 0x000CFC8B
		public CertificateID GetCertID()
		{
			return new CertificateID(this.resp.CertId);
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x000D1AA0 File Offset: 0x000CFCA0
		public object GetCertStatus()
		{
			CertStatus certStatus = this.resp.CertStatus;
			if (certStatus.TagNo == 0)
			{
				return null;
			}
			if (certStatus.TagNo == 1)
			{
				return new RevokedStatus(RevokedInfo.GetInstance(certStatus.Status));
			}
			return new UnknownStatus();
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001BF1 RID: 7153 RVA: 0x000D1AE2 File Offset: 0x000CFCE2
		public DateTime ThisUpdate
		{
			get
			{
				return this.resp.ThisUpdate.ToDateTime();
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06001BF2 RID: 7154 RVA: 0x000D1AF4 File Offset: 0x000CFCF4
		public DateTimeObject NextUpdate
		{
			get
			{
				if (this.resp.NextUpdate != null)
				{
					return new DateTimeObject(this.resp.NextUpdate.ToDateTime());
				}
				return null;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06001BF3 RID: 7155 RVA: 0x000D1B1A File Offset: 0x000CFD1A
		public X509Extensions SingleExtensions
		{
			get
			{
				return this.resp.SingleExtensions;
			}
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x000D1B27 File Offset: 0x000CFD27
		protected override X509Extensions GetX509Extensions()
		{
			return this.SingleExtensions;
		}

		// Token: 0x04001915 RID: 6421
		internal readonly SingleResponse resp;
	}
}
