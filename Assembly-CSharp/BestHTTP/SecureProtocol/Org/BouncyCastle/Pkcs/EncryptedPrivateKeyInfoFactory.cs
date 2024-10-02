using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs
{
	// Token: 0x020002C7 RID: 711
	public sealed class EncryptedPrivateKeyInfoFactory
	{
		// Token: 0x06001A28 RID: 6696 RVA: 0x00022F1F File Offset: 0x0002111F
		private EncryptedPrivateKeyInfoFactory()
		{
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x000C4060 File Offset: 0x000C2260
		public static EncryptedPrivateKeyInfo CreateEncryptedPrivateKeyInfo(DerObjectIdentifier algorithm, char[] passPhrase, byte[] salt, int iterationCount, AsymmetricKeyParameter key)
		{
			return EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(algorithm.Id, passPhrase, salt, iterationCount, PrivateKeyInfoFactory.CreatePrivateKeyInfo(key));
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x000C4077 File Offset: 0x000C2277
		public static EncryptedPrivateKeyInfo CreateEncryptedPrivateKeyInfo(string algorithm, char[] passPhrase, byte[] salt, int iterationCount, AsymmetricKeyParameter key)
		{
			return EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(algorithm, passPhrase, salt, iterationCount, PrivateKeyInfoFactory.CreatePrivateKeyInfo(key));
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x000C408C File Offset: 0x000C228C
		public static EncryptedPrivateKeyInfo CreateEncryptedPrivateKeyInfo(string algorithm, char[] passPhrase, byte[] salt, int iterationCount, PrivateKeyInfo keyInfo)
		{
			IBufferedCipher bufferedCipher = PbeUtilities.CreateEngine(algorithm) as IBufferedCipher;
			if (bufferedCipher == null)
			{
				throw new Exception("Unknown encryption algorithm: " + algorithm);
			}
			Asn1Encodable asn1Encodable = PbeUtilities.GenerateAlgorithmParameters(algorithm, salt, iterationCount);
			ICipherParameters parameters = PbeUtilities.GenerateCipherParameters(algorithm, passPhrase, asn1Encodable);
			bufferedCipher.Init(true, parameters);
			byte[] encoding = bufferedCipher.DoFinal(keyInfo.GetEncoded());
			return new EncryptedPrivateKeyInfo(new AlgorithmIdentifier(PbeUtilities.GetObjectIdentifier(algorithm), asn1Encodable), encoding);
		}
	}
}
