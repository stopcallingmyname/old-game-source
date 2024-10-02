using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007B2 RID: 1970
	public class CmpCertificate : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x0600464E RID: 17998 RVA: 0x00192F89 File Offset: 0x00191189
		public CmpCertificate(AttributeCertificate x509v2AttrCert)
		{
			this.x509v2AttrCert = x509v2AttrCert;
		}

		// Token: 0x0600464F RID: 17999 RVA: 0x00192F98 File Offset: 0x00191198
		public CmpCertificate(X509CertificateStructure x509v3PKCert)
		{
			if (x509v3PKCert.Version != 3)
			{
				throw new ArgumentException("only version 3 certificates allowed", "x509v3PKCert");
			}
			this.x509v3PKCert = x509v3PKCert;
		}

		// Token: 0x06004650 RID: 18000 RVA: 0x00192FC0 File Offset: 0x001911C0
		public static CmpCertificate GetInstance(object obj)
		{
			if (obj is CmpCertificate)
			{
				return (CmpCertificate)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CmpCertificate(X509CertificateStructure.GetInstance(obj));
			}
			if (obj is Asn1TaggedObject)
			{
				return new CmpCertificate(AttributeCertificate.GetInstance(((Asn1TaggedObject)obj).GetObject()));
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x06004651 RID: 18001 RVA: 0x00193028 File Offset: 0x00191228
		public virtual bool IsX509v3PKCert
		{
			get
			{
				return this.x509v3PKCert != null;
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06004652 RID: 18002 RVA: 0x00193033 File Offset: 0x00191233
		public virtual X509CertificateStructure X509v3PKCert
		{
			get
			{
				return this.x509v3PKCert;
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06004653 RID: 18003 RVA: 0x0019303B File Offset: 0x0019123B
		public virtual AttributeCertificate X509v2AttrCert
		{
			get
			{
				return this.x509v2AttrCert;
			}
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x00193043 File Offset: 0x00191243
		public override Asn1Object ToAsn1Object()
		{
			if (this.x509v2AttrCert != null)
			{
				return new DerTaggedObject(true, 1, this.x509v2AttrCert);
			}
			return this.x509v3PKCert.ToAsn1Object();
		}

		// Token: 0x04002DB5 RID: 11701
		private readonly X509CertificateStructure x509v3PKCert;

		// Token: 0x04002DB6 RID: 11702
		private readonly AttributeCertificate x509v2AttrCert;
	}
}
