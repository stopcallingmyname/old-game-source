using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Extension
{
	// Token: 0x0200025E RID: 606
	public class SubjectKeyIdentifierStructure : SubjectKeyIdentifier
	{
		// Token: 0x0600161A RID: 5658 RVA: 0x000AFDB4 File Offset: 0x000ADFB4
		public SubjectKeyIdentifierStructure(Asn1OctetString encodedValue) : base((Asn1OctetString)X509ExtensionUtilities.FromExtensionValue(encodedValue))
		{
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x000AFDC8 File Offset: 0x000ADFC8
		private static Asn1OctetString FromPublicKey(AsymmetricKeyParameter pubKey)
		{
			Asn1OctetString result;
			try
			{
				result = (Asn1OctetString)new SubjectKeyIdentifier(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pubKey)).ToAsn1Object();
			}
			catch (Exception ex)
			{
				throw new CertificateParsingException("Exception extracting certificate details: " + ex.ToString());
			}
			return result;
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x000AFE18 File Offset: 0x000AE018
		public SubjectKeyIdentifierStructure(AsymmetricKeyParameter pubKey) : base(SubjectKeyIdentifierStructure.FromPublicKey(pubKey))
		{
		}
	}
}
