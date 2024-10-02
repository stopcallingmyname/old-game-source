using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000610 RID: 1552
	public class OriginatorID : X509CertStoreSelector
	{
		// Token: 0x06003AD5 RID: 15061 RVA: 0x0016C8B8 File Offset: 0x0016AAB8
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

		// Token: 0x06003AD6 RID: 15062 RVA: 0x0016C8F8 File Offset: 0x0016AAF8
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return false;
			}
			OriginatorID originatorID = obj as OriginatorID;
			return originatorID != null && (Arrays.AreEqual(base.SubjectKeyIdentifier, originatorID.SubjectKeyIdentifier) && object.Equals(base.SerialNumber, originatorID.SerialNumber)) && X509CertStoreSelector.IssuersMatch(base.Issuer, originatorID.Issuer);
		}
	}
}
