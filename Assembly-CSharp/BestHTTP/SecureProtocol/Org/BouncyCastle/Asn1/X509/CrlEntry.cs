using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006B7 RID: 1719
	public class CrlEntry : Asn1Encodable
	{
		// Token: 0x06003F5D RID: 16221 RVA: 0x0017A160 File Offset: 0x00178360
		public CrlEntry(Asn1Sequence seq)
		{
			if (seq.Count < 2 || seq.Count > 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.seq = seq;
			this.userCertificate = DerInteger.GetInstance(seq[0]);
			this.revocationDate = Time.GetInstance(seq[1]);
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06003F5E RID: 16222 RVA: 0x0017A1CB File Offset: 0x001783CB
		public DerInteger UserCertificate
		{
			get
			{
				return this.userCertificate;
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06003F5F RID: 16223 RVA: 0x0017A1D3 File Offset: 0x001783D3
		public Time RevocationDate
		{
			get
			{
				return this.revocationDate;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06003F60 RID: 16224 RVA: 0x0017A1DB File Offset: 0x001783DB
		public X509Extensions Extensions
		{
			get
			{
				if (this.crlEntryExtensions == null && this.seq.Count == 3)
				{
					this.crlEntryExtensions = X509Extensions.GetInstance(this.seq[2]);
				}
				return this.crlEntryExtensions;
			}
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x0017A210 File Offset: 0x00178410
		public override Asn1Object ToAsn1Object()
		{
			return this.seq;
		}

		// Token: 0x04002827 RID: 10279
		internal Asn1Sequence seq;

		// Token: 0x04002828 RID: 10280
		internal DerInteger userCertificate;

		// Token: 0x04002829 RID: 10281
		internal Time revocationDate;

		// Token: 0x0400282A RID: 10282
		internal X509Extensions crlEntryExtensions;
	}
}
