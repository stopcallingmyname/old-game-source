using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Smime
{
	// Token: 0x020006E5 RID: 1765
	public class SmimeEncryptionKeyPreferenceAttribute : AttributeX509
	{
		// Token: 0x060040CE RID: 16590 RVA: 0x00180855 File Offset: 0x0017EA55
		public SmimeEncryptionKeyPreferenceAttribute(IssuerAndSerialNumber issAndSer) : base(SmimeAttributes.EncrypKeyPref, new DerSet(new DerTaggedObject(false, 0, issAndSer)))
		{
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x0018086F File Offset: 0x0017EA6F
		public SmimeEncryptionKeyPreferenceAttribute(RecipientKeyIdentifier rKeyID) : base(SmimeAttributes.EncrypKeyPref, new DerSet(new DerTaggedObject(false, 1, rKeyID)))
		{
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x00180889 File Offset: 0x0017EA89
		public SmimeEncryptionKeyPreferenceAttribute(Asn1OctetString sKeyID) : base(SmimeAttributes.EncrypKeyPref, new DerSet(new DerTaggedObject(false, 2, sKeyID)))
		{
		}
	}
}
