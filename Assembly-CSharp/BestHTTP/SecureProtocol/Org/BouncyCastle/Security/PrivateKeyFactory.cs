﻿using System;
using System.IO;
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
using BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002E6 RID: 742
	public sealed class PrivateKeyFactory
	{
		// Token: 0x06001B27 RID: 6951 RVA: 0x00022F1F File Offset: 0x0002111F
		private PrivateKeyFactory()
		{
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x000CD53A File Offset: 0x000CB73A
		public static AsymmetricKeyParameter CreateKey(byte[] privateKeyInfoData)
		{
			return PrivateKeyFactory.CreateKey(PrivateKeyInfo.GetInstance(Asn1Object.FromByteArray(privateKeyInfoData)));
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x000CD54C File Offset: 0x000CB74C
		public static AsymmetricKeyParameter CreateKey(Stream inStr)
		{
			return PrivateKeyFactory.CreateKey(PrivateKeyInfo.GetInstance(Asn1Object.FromStream(inStr)));
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x000CD560 File Offset: 0x000CB760
		public static AsymmetricKeyParameter CreateKey(PrivateKeyInfo keyInfo)
		{
			AlgorithmIdentifier privateKeyAlgorithm = keyInfo.PrivateKeyAlgorithm;
			DerObjectIdentifier algorithm = privateKeyAlgorithm.Algorithm;
			if (algorithm.Equals(PkcsObjectIdentifiers.RsaEncryption) || algorithm.Equals(X509ObjectIdentifiers.IdEARsa) || algorithm.Equals(PkcsObjectIdentifiers.IdRsassaPss) || algorithm.Equals(PkcsObjectIdentifiers.IdRsaesOaep))
			{
				RsaPrivateKeyStructure instance = RsaPrivateKeyStructure.GetInstance(keyInfo.ParsePrivateKey());
				return new RsaPrivateCrtKeyParameters(instance.Modulus, instance.PublicExponent, instance.PrivateExponent, instance.Prime1, instance.Prime2, instance.Exponent1, instance.Exponent2, instance.Coefficient);
			}
			if (algorithm.Equals(PkcsObjectIdentifiers.DhKeyAgreement))
			{
				DHParameter dhparameter = new DHParameter(Asn1Sequence.GetInstance(privateKeyAlgorithm.Parameters.ToAsn1Object()));
				DerInteger derInteger = (DerInteger)keyInfo.ParsePrivateKey();
				BigInteger l = dhparameter.L;
				int l2 = (l == null) ? 0 : l.IntValue;
				DHParameters parameters = new DHParameters(dhparameter.P, dhparameter.G, null, l2);
				return new DHPrivateKeyParameters(derInteger.Value, parameters, algorithm);
			}
			if (algorithm.Equals(OiwObjectIdentifiers.ElGamalAlgorithm))
			{
				ElGamalParameter elGamalParameter = new ElGamalParameter(Asn1Sequence.GetInstance(privateKeyAlgorithm.Parameters.ToAsn1Object()));
				return new ElGamalPrivateKeyParameters(((DerInteger)keyInfo.ParsePrivateKey()).Value, new ElGamalParameters(elGamalParameter.P, elGamalParameter.G));
			}
			if (algorithm.Equals(X9ObjectIdentifiers.IdDsa))
			{
				DerInteger derInteger2 = (DerInteger)keyInfo.ParsePrivateKey();
				Asn1Encodable parameters2 = privateKeyAlgorithm.Parameters;
				DsaParameters parameters3 = null;
				if (parameters2 != null)
				{
					DsaParameter instance2 = DsaParameter.GetInstance(parameters2.ToAsn1Object());
					parameters3 = new DsaParameters(instance2.P, instance2.Q, instance2.G);
				}
				return new DsaPrivateKeyParameters(derInteger2.Value, parameters3);
			}
			if (algorithm.Equals(X9ObjectIdentifiers.IdECPublicKey))
			{
				X962Parameters x962Parameters = new X962Parameters(privateKeyAlgorithm.Parameters.ToAsn1Object());
				X9ECParameters x9ECParameters;
				if (x962Parameters.IsNamedCurve)
				{
					x9ECParameters = ECKeyPairGenerator.FindECCurveByOid((DerObjectIdentifier)x962Parameters.Parameters);
				}
				else
				{
					x9ECParameters = new X9ECParameters((Asn1Sequence)x962Parameters.Parameters);
				}
				BigInteger key = ECPrivateKeyStructure.GetInstance(keyInfo.ParsePrivateKey()).GetKey();
				if (x962Parameters.IsNamedCurve)
				{
					return new ECPrivateKeyParameters("EC", key, (DerObjectIdentifier)x962Parameters.Parameters);
				}
				ECDomainParameters parameters4 = new ECDomainParameters(x9ECParameters.Curve, x9ECParameters.G, x9ECParameters.N, x9ECParameters.H, x9ECParameters.GetSeed());
				return new ECPrivateKeyParameters(key, parameters4);
			}
			else if (algorithm.Equals(CryptoProObjectIdentifiers.GostR3410x2001))
			{
				Gost3410PublicKeyAlgParameters gost3410PublicKeyAlgParameters = new Gost3410PublicKeyAlgParameters(Asn1Sequence.GetInstance(privateKeyAlgorithm.Parameters.ToAsn1Object()));
				ECDomainParameters byOid = ECGost3410NamedCurves.GetByOid(gost3410PublicKeyAlgParameters.PublicKeyParamSet);
				if (byOid == null)
				{
					throw new ArgumentException("Unrecognized curve OID for GostR3410x2001 private key");
				}
				Asn1Object asn1Object = keyInfo.ParsePrivateKey();
				ECPrivateKeyStructure ecprivateKeyStructure;
				if (asn1Object is DerInteger)
				{
					ecprivateKeyStructure = new ECPrivateKeyStructure(byOid.N.BitLength, ((DerInteger)asn1Object).PositiveValue);
				}
				else
				{
					ecprivateKeyStructure = ECPrivateKeyStructure.GetInstance(asn1Object);
				}
				return new ECPrivateKeyParameters("ECGOST3410", ecprivateKeyStructure.GetKey(), gost3410PublicKeyAlgParameters.PublicKeyParamSet);
			}
			else
			{
				if (algorithm.Equals(CryptoProObjectIdentifiers.GostR3410x94))
				{
					Gost3410PublicKeyAlgParameters instance3 = Gost3410PublicKeyAlgParameters.GetInstance(privateKeyAlgorithm.Parameters);
					Asn1Object asn1Object2 = keyInfo.ParsePrivateKey();
					BigInteger x;
					if (asn1Object2 is DerInteger)
					{
						x = DerInteger.GetInstance(asn1Object2).PositiveValue;
					}
					else
					{
						x = new BigInteger(1, Arrays.Reverse(Asn1OctetString.GetInstance(asn1Object2).GetOctets()));
					}
					return new Gost3410PrivateKeyParameters(x, instance3.PublicKeyParamSet);
				}
				if (algorithm.Equals(EdECObjectIdentifiers.id_X25519))
				{
					return new X25519PrivateKeyParameters(PrivateKeyFactory.GetRawKey(keyInfo, X25519PrivateKeyParameters.KeySize), 0);
				}
				if (algorithm.Equals(EdECObjectIdentifiers.id_X448))
				{
					return new X448PrivateKeyParameters(PrivateKeyFactory.GetRawKey(keyInfo, X448PrivateKeyParameters.KeySize), 0);
				}
				if (algorithm.Equals(EdECObjectIdentifiers.id_Ed25519))
				{
					return new Ed25519PrivateKeyParameters(PrivateKeyFactory.GetRawKey(keyInfo, Ed25519PrivateKeyParameters.KeySize), 0);
				}
				if (algorithm.Equals(EdECObjectIdentifiers.id_Ed448))
				{
					return new Ed448PrivateKeyParameters(PrivateKeyFactory.GetRawKey(keyInfo, Ed448PrivateKeyParameters.KeySize), 0);
				}
				throw new SecurityUtilityException("algorithm identifier in private key not recognised");
			}
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x000CD944 File Offset: 0x000CBB44
		private static byte[] GetRawKey(PrivateKeyInfo keyInfo, int expectedSize)
		{
			byte[] octets = Asn1OctetString.GetInstance(keyInfo.ParsePrivateKey()).GetOctets();
			if (expectedSize != octets.Length)
			{
				throw new SecurityUtilityException("private key encoding has incorrect length");
			}
			return octets;
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x000CD974 File Offset: 0x000CBB74
		public static AsymmetricKeyParameter DecryptKey(char[] passPhrase, EncryptedPrivateKeyInfo encInfo)
		{
			return PrivateKeyFactory.CreateKey(PrivateKeyInfoFactory.CreatePrivateKeyInfo(passPhrase, encInfo));
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x000CD982 File Offset: 0x000CBB82
		public static AsymmetricKeyParameter DecryptKey(char[] passPhrase, byte[] encryptedPrivateKeyInfoData)
		{
			return PrivateKeyFactory.DecryptKey(passPhrase, Asn1Object.FromByteArray(encryptedPrivateKeyInfoData));
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x000CD990 File Offset: 0x000CBB90
		public static AsymmetricKeyParameter DecryptKey(char[] passPhrase, Stream encryptedPrivateKeyInfoStream)
		{
			return PrivateKeyFactory.DecryptKey(passPhrase, Asn1Object.FromStream(encryptedPrivateKeyInfoStream));
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x000CD99E File Offset: 0x000CBB9E
		private static AsymmetricKeyParameter DecryptKey(char[] passPhrase, Asn1Object asn1Object)
		{
			return PrivateKeyFactory.DecryptKey(passPhrase, EncryptedPrivateKeyInfo.GetInstance(asn1Object));
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x000CD9AC File Offset: 0x000CBBAC
		public static byte[] EncryptKey(DerObjectIdentifier algorithm, char[] passPhrase, byte[] salt, int iterationCount, AsymmetricKeyParameter key)
		{
			return EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(algorithm, passPhrase, salt, iterationCount, key).GetEncoded();
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x000CD9BE File Offset: 0x000CBBBE
		public static byte[] EncryptKey(string algorithm, char[] passPhrase, byte[] salt, int iterationCount, AsymmetricKeyParameter key)
		{
			return EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(algorithm, passPhrase, salt, iterationCount, key).GetEncoded();
		}
	}
}
