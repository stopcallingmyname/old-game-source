using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x0200061B RID: 1563
	public class SignerID : X509CertStoreSelector
	{
		// Token: 0x06003B09 RID: 15113 RVA: 0x0016D18C File Offset: 0x0016B38C
		public override int GetHashCode()
		{
			int num = Arrays.GetHashCode(base.SubjectKeyIdentifier);
			BigInteger serialNumber = base.SerialNumber;
			if (serialNumber != null)
			{
				num ^= serialNumber.GetHashCode();
			}
			X509Name issuer = base.Issuer;
			if (issuer != null)
			{
				num ^= issuer.GetHashCode();
			}
			return num;
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x0016D1CC File Offset: 0x0016B3CC
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return false;
			}
			SignerID signerID = obj as SignerID;
			return signerID != null && (Arrays.AreEqual(base.SubjectKeyIdentifier, signerID.SubjectKeyIdentifier) && object.Equals(base.SerialNumber, signerID.SerialNumber)) && X509CertStoreSelector.IssuersMatch(base.Issuer, signerID.Issuer);
		}
	}
}
