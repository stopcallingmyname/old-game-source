using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.EdEC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Sec;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs
{
	// Token: 0x020002CE RID: 718
	public sealed class PrivateKeyInfoFactory
	{
		// Token: 0x06001A6F RID: 6767 RVA: 0x00022F1F File Offset: 0x0002111F
		private PrivateKeyInfoFactory()
		{
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x000C66C8 File Offset: 0x000C48C8
		public static PrivateKeyInfo CreatePrivateKeyInfo(AsymmetricKeyParameter privateKey)
		{
			return PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey, null);
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x000C66D4 File Offset: 0x000C48D4
		public static PrivateKeyInfo CreatePrivateKeyInfo(AsymmetricKeyParameter privateKey, Asn1Set attributes)
		{
			if (privateKey == null)
			{
				throw new ArgumentNullException("privateKey");
			}
			if (!privateKey.IsPrivate)
			{
				throw new ArgumentException("Public key passed - private key expected", "privateKey");
			}
			if (privateKey is ElGamalPrivateKeyParameters)
			{
				ElGamalPrivateKeyParameters elGamalPrivateKeyParameters = (ElGamalPrivateKeyParameters)privateKey;
				ElGamalParameters parameters = elGamalPrivateKeyParameters.Parameters;
				return new PrivateKeyInfo(new AlgorithmIdentifier(OiwObjectIdentifiers.ElGamalAlgorithm, new ElGamalParameter(parameters.P, parameters.G).ToAsn1Object()), new DerInteger(elGamalPrivateKeyParameters.X), attributes);
			}
			if (privateKey is DsaPrivateKeyParameters)
			{
				DsaPrivateKeyParameters dsaPrivateKeyParameters = (DsaPrivateKeyParameters)privateKey;
				DsaParameters parameters2 = dsaPrivateKeyParameters.Parameters;
				return new PrivateKeyInfo(new AlgorithmIdentifier(X9ObjectIdentifiers.IdDsa, new DsaParameter(parameters2.P, parameters2.Q, parameters2.G).ToAsn1Object()), new DerInteger(dsaPrivateKeyParameters.X), attributes);
			}
			if (privateKey is DHPrivateKeyParameters)
			{
				DHPrivateKeyParameters dhprivateKeyParameters = (DHPrivateKeyParameters)privateKey;
				DHParameter dhparameter = new DHParameter(dhprivateKeyParameters.Parameters.P, dhprivateKeyParameters.Parameters.G, dhprivateKeyParameters.Parameters.L);
				return new PrivateKeyInfo(new AlgorithmIdentifier(dhprivateKeyParameters.AlgorithmOid, dhparameter.ToAsn1Object()), new DerInteger(dhprivateKeyParameters.X), attributes);
			}
			if (privateKey is RsaKeyParameters)
			{
				AlgorithmIdentifier privateKeyAlgorithm = new AlgorithmIdentifier(PkcsObjectIdentifiers.RsaEncryption, DerNull.Instance);
				RsaPrivateKeyStructure rsaPrivateKeyStructure;
				if (privateKey is RsaPrivateCrtKeyParameters)
				{
					RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters = (RsaPrivateCrtKeyParameters)privateKey;
					rsaPrivateKeyStructure = new RsaPrivateKeyStructure(rsaPrivateCrtKeyParameters.Modulus, rsaPrivateCrtKeyParameters.PublicExponent, rsaPrivateCrtKeyParameters.Exponent, rsaPrivateCrtKeyParameters.P, rsaPrivateCrtKeyParameters.Q, rsaPrivateCrtKeyParameters.DP, rsaPrivateCrtKeyParameters.DQ, rsaPrivateCrtKeyParameters.QInv);
				}
				else
				{
					RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)privateKey;
					rsaPrivateKeyStructure = new RsaPrivateKeyStructure(rsaKeyParameters.Modulus, BigInteger.Zero, rsaKeyParameters.Exponent, BigInteger.Zero, BigInteger.Zero, BigInteger.Zero, BigInteger.Zero, BigInteger.Zero);
				}
				return new PrivateKeyInfo(privateKeyAlgorithm, rsaPrivateKeyStructure.ToAsn1Object(), attributes);
			}
			if (privateKey is ECPrivateKeyParameters)
			{
				ECPrivateKeyParameters ecprivateKeyParameters = (ECPrivateKeyParameters)privateKey;
				DerBitString publicKey = new DerBitString(ECKeyPairGenerator.GetCorrespondingPublicKey(ecprivateKeyParameters).Q.GetEncoded(false));
				ECDomainParameters parameters3 = ecprivateKeyParameters.Parameters;
				int bitLength = parameters3.N.BitLength;
				AlgorithmIdentifier privateKeyAlgorithm2;
				ECPrivateKeyStructure privateKey2;
				if (ecprivateKeyParameters.AlgorithmName == "ECGOST3410")
				{
					if (ecprivateKeyParameters.PublicKeyParamSet == null)
					{
						throw Platform.CreateNotImplementedException("Not a CryptoPro parameter set");
					}
					Gost3410PublicKeyAlgParameters parameters4 = new Gost3410PublicKeyAlgParameters(ecprivateKeyParameters.PublicKeyParamSet, CryptoProObjectIdentifiers.GostR3411x94CryptoProParamSet);
					privateKeyAlgorithm2 = new AlgorithmIdentifier(CryptoProObjectIdentifiers.GostR3410x2001, parameters4);
					privateKey2 = new ECPrivateKeyStructure(bitLength, ecprivateKeyParameters.D, publicKey, null);
				}
				else
				{
					X962Parameters parameters5;
					if (ecprivateKeyParameters.PublicKeyParamSet == null)
					{
						parameters5 = new X962Parameters(new X9ECParameters(parameters3.Curve, parameters3.G, parameters3.N, parameters3.H, parameters3.GetSeed()));
					}
					else
					{
						parameters5 = new X962Parameters(ecprivateKeyParameters.PublicKeyParamSet);
					}
					privateKey2 = new ECPrivateKeyStructure(bitLength, ecprivateKeyParameters.D, publicKey, parameters5);
					privateKeyAlgorithm2 = new AlgorithmIdentifier(X9ObjectIdentifiers.IdECPublicKey, parameters5);
				}
				return new PrivateKeyInfo(privateKeyAlgorithm2, privateKey2, attributes);
			}
			if (privateKey is Gost3410PrivateKeyParameters)
			{
				Gost3410PrivateKeyParameters gost3410PrivateKeyParameters = (Gost3410PrivateKeyParameters)privateKey;
				if (gost3410PrivateKeyParameters.PublicKeyParamSet == null)
				{
					throw Platform.CreateNotImplementedException("Not a CryptoPro parameter set");
				}
				byte[] array = gost3410PrivateKeyParameters.X.ToByteArrayUnsigned();
				byte[] array2 = new byte[array.Length];
				for (int num = 0; num != array2.Length; num++)
				{
					array2[num] = array[array.Length - 1 - num];
				}
				Gost3410PublicKeyAlgParameters gost3410PublicKeyAlgParameters = new Gost3410PublicKeyAlgParameters(gost3410PrivateKeyParameters.PublicKeyParamSet, CryptoProObjectIdentifiers.GostR3411x94CryptoProParamSet, null);
				return new PrivateKeyInfo(new AlgorithmIdentifier(CryptoProObjectIdentifiers.GostR3410x94, gost3410PublicKeyAlgParameters.ToAsn1Object()), new DerOctetString(array2), attributes);
			}
			else
			{
				if (privateKey is X448PrivateKeyParameters)
				{
					X448PrivateKeyParameters x448PrivateKeyParameters = (X448PrivateKeyParameters)privateKey;
					return new PrivateKeyInfo(new AlgorithmIdentifier(EdECObjectIdentifiers.id_X448), new DerOctetString(x448PrivateKeyParameters.GetEncoded()), attributes, x448PrivateKeyParameters.GeneratePublicKey().GetEncoded());
				}
				if (privateKey is X25519PrivateKeyParameters)
				{
					X25519PrivateKeyParameters x25519PrivateKeyParameters = (X25519PrivateKeyParameters)privateKey;
					return new PrivateKeyInfo(new AlgorithmIdentifier(EdECObjectIdentifiers.id_X25519), new DerOctetString(x25519PrivateKeyParameters.GetEncoded()), attributes, x25519PrivateKeyParameters.GeneratePublicKey().GetEncoded());
				}
				if (privateKey is Ed448PrivateKeyParameters)
				{
					Ed448PrivateKeyParameters ed448PrivateKeyParameters = (Ed448PrivateKeyParameters)privateKey;
					return new PrivateKeyInfo(new AlgorithmIdentifier(EdECObjectIdentifiers.id_Ed448), new DerOctetString(ed448PrivateKeyParameters.GetEncoded()), attributes, ed448PrivateKeyParameters.GeneratePublicKey().GetEncoded());
				}
				if (privateKey is Ed25519PrivateKeyParameters)
				{
					Ed25519PrivateKeyParameters ed25519PrivateKeyParameters = (Ed25519PrivateKeyParameters)privateKey;
					return new PrivateKeyInfo(new AlgorithmIdentifier(EdECObjectIdentifiers.id_Ed25519), new DerOctetString(ed25519PrivateKeyParameters.GetEncoded()), attributes, ed25519PrivateKeyParameters.GeneratePublicKey().GetEncoded());
				}
				throw new ArgumentException("Class provided is not convertible: " + Platform.GetTypeName(privateKey));
			}
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x000C6B57 File Offset: 0x000C4D57
		public static PrivateKeyInfo CreatePrivateKeyInfo(char[] passPhrase, EncryptedPrivateKeyInfo encInfo)
		{
			return PrivateKeyInfoFactory.CreatePrivateKeyInfo(passPhrase, false, encInfo);
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x000C6B64 File Offset: 0x000C4D64
		public static PrivateKeyInfo CreatePrivateKeyInfo(char[] passPhrase, bool wrongPkcs12Zero, EncryptedPrivateKeyInfo encInfo)
		{
			AlgorithmIdentifier encryptionAlgorithm = encInfo.EncryptionAlgorithm;
			IBufferedCipher bufferedCipher = PbeUtilities.CreateEngine(encryptionAlgorithm) as IBufferedCipher;
			if (bufferedCipher == null)
			{
				throw new Exception("Unknown encryption algorithm: " + encryptionAlgorithm.Algorithm);
			}
			ICipherParameters parameters = PbeUtilities.GenerateCipherParameters(encryptionAlgorithm, passPhrase, wrongPkcs12Zero);
			bufferedCipher.Init(false, parameters);
			return PrivateKeyInfo.GetInstance(bufferedCipher.DoFinal(encInfo.GetEncryptedData()));
		}
	}
}
