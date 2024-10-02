using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006A3 RID: 1699
	public sealed class KeyPurposeID : DerObjectIdentifier
	{
		// Token: 0x06003ED6 RID: 16086 RVA: 0x00176905 File Offset: 0x00174B05
		private KeyPurposeID(string id) : base(id)
		{
		}

		// Token: 0x040027DB RID: 10203
		private const string IdKP = "1.3.6.1.5.5.7.3";

		// Token: 0x040027DC RID: 10204
		public static readonly KeyPurposeID AnyExtendedKeyUsage = new KeyPurposeID(X509Extensions.ExtendedKeyUsage.Id + ".0");

		// Token: 0x040027DD RID: 10205
		public static readonly KeyPurposeID IdKPServerAuth = new KeyPurposeID("1.3.6.1.5.5.7.3.1");

		// Token: 0x040027DE RID: 10206
		public static readonly KeyPurposeID IdKPClientAuth = new KeyPurposeID("1.3.6.1.5.5.7.3.2");

		// Token: 0x040027DF RID: 10207
		public static readonly KeyPurposeID IdKPCodeSigning = new KeyPurposeID("1.3.6.1.5.5.7.3.3");

		// Token: 0x040027E0 RID: 10208
		public static readonly KeyPurposeID IdKPEmailProtection = new KeyPurposeID("1.3.6.1.5.5.7.3.4");

		// Token: 0x040027E1 RID: 10209
		public static readonly KeyPurposeID IdKPIpsecEndSystem = new KeyPurposeID("1.3.6.1.5.5.7.3.5");

		// Token: 0x040027E2 RID: 10210
		public static readonly KeyPurposeID IdKPIpsecTunnel = new KeyPurposeID("1.3.6.1.5.5.7.3.6");

		// Token: 0x040027E3 RID: 10211
		public static readonly KeyPurposeID IdKPIpsecUser = new KeyPurposeID("1.3.6.1.5.5.7.3.7");

		// Token: 0x040027E4 RID: 10212
		public static readonly KeyPurposeID IdKPTimeStamping = new KeyPurposeID("1.3.6.1.5.5.7.3.8");

		// Token: 0x040027E5 RID: 10213
		public static readonly KeyPurposeID IdKPOcspSigning = new KeyPurposeID("1.3.6.1.5.5.7.3.9");

		// Token: 0x040027E6 RID: 10214
		public static readonly KeyPurposeID IdKPSmartCardLogon = new KeyPurposeID("1.3.6.1.4.1.311.20.2.2");

		// Token: 0x040027E7 RID: 10215
		public static readonly KeyPurposeID IdKPMacAddress = new KeyPurposeID("1.3.6.1.1.1.1.22");
	}
}
