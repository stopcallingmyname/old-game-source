using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x0200023D RID: 573
	public class PrincipalUtilities
	{
		// Token: 0x060014A9 RID: 5289 RVA: 0x000AB02C File Offset: 0x000A922C
		public static X509Name GetIssuerX509Principal(X509Certificate cert)
		{
			X509Name issuer;
			try
			{
				issuer = TbsCertificateStructure.GetInstance(Asn1Object.FromByteArray(cert.GetTbsCertificate())).Issuer;
			}
			catch (Exception e)
			{
				throw new CertificateEncodingException("Could not extract issuer", e);
			}
			return issuer;
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x000AB070 File Offset: 0x000A9270
		public static X509Name GetSubjectX509Principal(X509Certificate cert)
		{
			X509Name subject;
			try
			{
				subject = TbsCertificateStructure.GetInstance(Asn1Object.FromByteArray(cert.GetTbsCertificate())).Subject;
			}
			catch (Exception e)
			{
				throw new CertificateEncodingException("Could not extract subject", e);
			}
			return subject;
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x000AB0B4 File Offset: 0x000A92B4
		public static X509Name GetIssuerX509Principal(X509Crl crl)
		{
			X509Name issuer;
			try
			{
				issuer = TbsCertificateList.GetInstance(Asn1Object.FromByteArray(crl.GetTbsCertList())).Issuer;
			}
			catch (Exception e)
			{
				throw new CrlException("Could not extract issuer", e);
			}
			return issuer;
		}
	}
}
