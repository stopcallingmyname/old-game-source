using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs
{
	// Token: 0x020002CD RID: 717
	public class Pkcs12Utilities
	{
		// Token: 0x06001A6C RID: 6764 RVA: 0x000C65A1 File Offset: 0x000C47A1
		public static byte[] ConvertToDefiniteLength(byte[] berPkcs12File)
		{
			return new Pfx(Asn1Sequence.GetInstance(Asn1Object.FromByteArray(berPkcs12File))).GetEncoded("DER");
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x000C65C0 File Offset: 0x000C47C0
		public static byte[] ConvertToDefiniteLength(byte[] berPkcs12File, char[] passwd)
		{
			Pfx pfx = new Pfx(Asn1Sequence.GetInstance(Asn1Object.FromByteArray(berPkcs12File)));
			ContentInfo contentInfo = pfx.AuthSafe;
			Asn1Object asn1Object = Asn1Object.FromByteArray(Asn1OctetString.GetInstance(contentInfo.Content).GetOctets());
			contentInfo = new ContentInfo(contentInfo.ContentType, new DerOctetString(asn1Object.GetEncoded("DER")));
			MacData macData = pfx.MacData;
			try
			{
				int intValue = macData.IterationCount.IntValue;
				byte[] octets = Asn1OctetString.GetInstance(contentInfo.Content).GetOctets();
				byte[] digest = Pkcs12Store.CalculatePbeMac(macData.Mac.AlgorithmID.Algorithm, macData.GetSalt(), intValue, passwd, false, octets);
				macData = new MacData(new DigestInfo(new AlgorithmIdentifier(macData.Mac.AlgorithmID.Algorithm, DerNull.Instance), digest), macData.GetSalt(), intValue);
			}
			catch (Exception ex)
			{
				throw new IOException("error constructing MAC: " + ex.ToString());
			}
			return new Pfx(contentInfo, macData).GetEncoded("DER");
		}
	}
}
