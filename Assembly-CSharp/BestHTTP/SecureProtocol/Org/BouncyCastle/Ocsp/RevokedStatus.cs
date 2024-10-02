using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp
{
	// Token: 0x02000302 RID: 770
	public class RevokedStatus : CertificateStatus
	{
		// Token: 0x06001BE9 RID: 7145 RVA: 0x000D19FD File Offset: 0x000CFBFD
		public RevokedStatus(RevokedInfo info)
		{
			this.info = info;
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x000D1A0C File Offset: 0x000CFC0C
		public RevokedStatus(DateTime revocationDate, int reason)
		{
			this.info = new RevokedInfo(new DerGeneralizedTime(revocationDate), new CrlReason(reason));
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001BEB RID: 7147 RVA: 0x000D1A2B File Offset: 0x000CFC2B
		public DateTime RevocationTime
		{
			get
			{
				return this.info.RevocationTime.ToDateTime();
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001BEC RID: 7148 RVA: 0x000D1A3D File Offset: 0x000CFC3D
		public bool HasRevocationReason
		{
			get
			{
				return this.info.RevocationReason != null;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001BED RID: 7149 RVA: 0x000D1A4D File Offset: 0x000CFC4D
		public int RevocationReason
		{
			get
			{
				if (this.info.RevocationReason == null)
				{
					throw new InvalidOperationException("attempt to get a reason where none is available");
				}
				return this.info.RevocationReason.Value.IntValue;
			}
		}

		// Token: 0x04001914 RID: 6420
		internal readonly RevokedInfo info;
	}
}
