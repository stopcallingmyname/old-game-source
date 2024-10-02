using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000617 RID: 1559
	public class RecipientID : X509CertStoreSelector
	{
		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x0016CE33 File Offset: 0x0016B033
		// (set) Token: 0x06003AF4 RID: 15092 RVA: 0x0016CE40 File Offset: 0x0016B040
		public byte[] KeyIdentifier
		{
			get
			{
				return Arrays.Clone(this.keyIdentifier);
			}
			set
			{
				this.keyIdentifier = Arrays.Clone(value);
			}
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x0016CE50 File Offset: 0x0016B050
		public override int GetHashCode()
		{
			int num = Arrays.GetHashCode(this.keyIdentifier) ^ Arrays.GetHashCode(base.SubjectKeyIdentifier);
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

		// Token: 0x06003AF6 RID: 15094 RVA: 0x0016CE9C File Offset: 0x0016B09C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			RecipientID recipientID = obj as RecipientID;
			return recipientID != null && (Arrays.AreEqual(this.keyIdentifier, recipientID.keyIdentifier) && Arrays.AreEqual(base.SubjectKeyIdentifier, recipientID.SubjectKeyIdentifier) && object.Equals(base.SerialNumber, recipientID.SerialNumber)) && X509CertStoreSelector.IssuersMatch(base.Issuer, recipientID.Issuer);
		}

		// Token: 0x04002673 RID: 9843
		private byte[] keyIdentifier;
	}
}
