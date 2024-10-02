using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x0200023A RID: 570
	public interface IX509AttributeCertificate : IX509Extension
	{
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06001492 RID: 5266
		int Version { get; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06001493 RID: 5267
		BigInteger SerialNumber { get; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06001494 RID: 5268
		DateTime NotBefore { get; }

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06001495 RID: 5269
		DateTime NotAfter { get; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06001496 RID: 5270
		AttributeCertificateHolder Holder { get; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06001497 RID: 5271
		AttributeCertificateIssuer Issuer { get; }

		// Token: 0x06001498 RID: 5272
		X509Attribute[] GetAttributes();

		// Token: 0x06001499 RID: 5273
		X509Attribute[] GetAttributes(string oid);

		// Token: 0x0600149A RID: 5274
		bool[] GetIssuerUniqueID();

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x0600149B RID: 5275
		bool IsValidNow { get; }

		// Token: 0x0600149C RID: 5276
		bool IsValid(DateTime date);

		// Token: 0x0600149D RID: 5277
		void CheckValidity();

		// Token: 0x0600149E RID: 5278
		void CheckValidity(DateTime date);

		// Token: 0x0600149F RID: 5279
		byte[] GetSignature();

		// Token: 0x060014A0 RID: 5280
		void Verify(AsymmetricKeyParameter publicKey);

		// Token: 0x060014A1 RID: 5281
		byte[] GetEncoded();
	}
}
