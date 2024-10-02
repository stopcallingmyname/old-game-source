using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.EdEC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002E7 RID: 743
	public sealed class PublicKeyFactory
	{
		// Token: 0x06001B32 RID: 6962 RVA: 0x00022F1F File Offset: 0x0002111F
		private PublicKeyFactory()
		{
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x000CD9D0 File Offset: 0x000CBBD0
		public static AsymmetricKeyParameter CreateKey(byte[] keyInfoData)
		{
			return PublicKeyFactory.CreateKey(SubjectPublicKeyInfo.GetInstance(Asn1Object.FromByteArray(keyInfoData)));
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x000CD9E2 File Offset: 0x000CBBE2
		public static AsymmetricKeyParameter CreateKey(Stream inStr)
		{
			return PublicKeyFactory.CreateKey(SubjectPublicKeyInfo.GetInstance(Asn1Object.FromStream(inStr)));
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x000CD9F4 File Offset: 0x000CBBF4
		public static AsymmetricKeyParameter CreateKey(SubjectPublicKeyInfo keyInfo)
		{
			AlgorithmIdentifier algorithmID = keyInfo.AlgorithmID;
			DerObjectIdentifier algorithm = algorithmID.Algorithm;
			if (algorithm.Equals(PkcsObjectIdentifiers.RsaEncryption) || algorithm.Equals(X509ObjectIdentifiers.IdEARsa) || algorithm.Equals(PkcsObjectIdentifiers.IdRsassaPss) || algorithm.Equals(PkcsObjectIdentifiers.IdRsaesOaep))
			{
				RsaPublicKeyStructure instance = RsaPublicKeyStructure.GetInstance(keyInfo.GetPublicKey());
				return new RsaKeyParameters(false, instance.Modulus, instance.PublicExponent);
			}
			if (algorithm.Equals(X9ObjectIdentifiers.DHPublicNumber))
			{
				Asn1Sequence instance2 = Asn1Sequence.GetInstance(algorithmID.Parameters.ToAsn1Object());
				BigInteger value = DHPublicKey.GetInstance(keyInfo.GetPublicKey()).Y.Value;
				if (PublicKeyFactory.IsPkcsDHParam(instance2))
				{
					return PublicKeyFactory.ReadPkcsDHParam(algorithm, value, instance2);
				}
				DHDomainParameters instance3 = DHDomainParameters.GetInstance(instance2);
				BigInteger value2 = instance3.P.Value;
				BigInteger value3 = instance3.G.Value;
				BigInteger value4 = instance3.Q.Value;
				BigInteger j = null;
				if (instance3.J != null)
				{
					j = instance3.J.Value;
				}
				DHValidationParameters validation = null;
				DHValidationParms validationParms = instance3.ValidationParms;
				if (validationParms != null)
				{
					byte[] bytes = validationParms.Seed.GetBytes();
					BigInteger value5 = validationParms.PgenCounter.Value;
					validation = new DHValidationParameters(bytes, value5.IntValue);
				}
				return new DHPublicKeyParameters(value, new DHParameters(value2, value3, value4, j, validation));
			}
			else
			{
				if (algorithm.Equals(PkcsObjectIdentifiers.DhKeyAgreement))
				{
					Asn1Sequence instance4 = Asn1Sequence.GetInstance(algorithmID.Parameters.ToAsn1Object());
					DerInteger derInteger = (DerInteger)keyInfo.GetPublicKey();
					return PublicKeyFactory.ReadPkcsDHParam(algorithm, derInteger.Value, instance4);
				}
				if (algorithm.Equals(OiwObjectIdentifiers.ElGamalAlgorithm))
				{
					ElGamalParameter elGamalParameter = new ElGamalParameter(Asn1Sequence.GetInstance(algorithmID.Parameters.ToAsn1Object()));
					return new ElGamalPublicKeyParameters(((DerInteger)keyInfo.GetPublicKey()).Value, new ElGamalParameters(elGamalParameter.P, elGamalParameter.G));
				}
				if (algorithm.Equals(X9ObjectIdentifiers.IdDsa) || algorithm.Equals(OiwObjectIdentifiers.DsaWithSha1))
				{
					DerInteger derInteger2 = (DerInteger)keyInfo.GetPublicKey();
					Asn1Encodable parameters = algorithmID.Parameters;
					DsaParameters parameters2 = null;
					if (parameters != null)
					{
						DsaParameter instance5 = DsaParameter.GetInstance(parameters.ToAsn1Object());
						parameters2 = new DsaParameters(instance5.P, instance5.Q, instance5.G);
					}
					return new DsaPublicKeyParameters(derInteger2.Value, parameters2);
				}
				if (algorithm.Equals(X9ObjectIdentifiers.IdECPublicKey))
				{
					X962Parameters x962Parameters = new X962Parameters(algorithmID.Parameters.ToAsn1Object());
					X9ECParameters x9ECParameters;
					if (x962Parameters.IsNamedCurve)
					{
						x9ECParameters = ECKeyPairGenerator.FindECCurveByOid((DerObjectIdentifier)x962Parameters.Parameters);
					}
					else
					{
						x9ECParameters = new X9ECParameters((Asn1Sequence)x962Parameters.Parameters);
					}
					Asn1OctetString s = new DerOctetString(keyInfo.PublicKeyData.GetBytes());
					ECPoint point = new X9ECPoint(x9ECParameters.Curve, s).Point;
					if (x962Parameters.IsNamedCurve)
					{
						return new ECPublicKeyParameters("EC", point, (DerObjectIdentifier)x962Parameters.Parameters);
					}
					ECDomainParameters parameters3 = new ECDomainParameters(x9ECParameters.Curve, x9ECParameters.G, x9ECParameters.N, x9ECParameters.H, x9ECParameters.GetSeed());
					return new ECPublicKeyParameters(point, parameters3);
				}
				else if (algorithm.Equals(CryptoProObjectIdentifiers.GostR3410x2001))
				{
					Gost3410PublicKeyAlgParameters gost3410PublicKeyAlgParameters = new Gost3410PublicKeyAlgParameters((Asn1Sequence)algorithmID.Parameters);
					Asn1OctetString asn1OctetString;
					try
					{
						asn1OctetString = (Asn1OctetString)keyInfo.GetPublicKey();
					}
					catch (IOException)
					{
						throw new ArgumentException("invalid info structure in GOST3410 public key");
					}
					byte[] octets = asn1OctetString.GetOctets();
					byte[] array = new byte[32];
					byte[] array2 = new byte[32];
					for (int num = 0; num != array2.Length; num++)
					{
						array[num] = octets[31 - num];
					}
					for (int num2 = 0; num2 != array.Length; num2++)
					{
						array2[num2] = octets[63 - num2];
					}
					ECDomainParameters byOid = ECGost3410NamedCurves.GetByOid(gost3410PublicKeyAlgParameters.PublicKeyParamSet);
					if (byOid == null)
					{
						return null;
					}
					ECPoint q = byOid.Curve.CreatePoint(new BigInteger(1, array), new BigInteger(1, array2));
					return new ECPublicKeyParameters("ECGOST3410", q, gost3410PublicKeyAlgParameters.PublicKeyParamSet);
				}
				else
				{
					if (algorithm.Equals(CryptoProObjectIdentifiers.GostR3410x94))
					{
						Gost3410PublicKeyAlgParameters gost3410PublicKeyAlgParameters2 = new Gost3410PublicKeyAlgParameters((Asn1Sequence)algorithmID.Parameters);
						DerOctetString derOctetString;
						try
						{
							derOctetString = (DerOctetString)keyInfo.GetPublicKey();
						}
						catch (IOException)
						{
							throw new ArgumentException("invalid info structure in GOST3410 public key");
						}
						byte[] octets2 = derOctetString.GetOctets();
						byte[] array3 = new byte[octets2.Length];
						for (int num3 = 0; num3 != octets2.Length; num3++)
						{
							array3[num3] = octets2[octets2.Length - 1 - num3];
						}
						return new Gost3410PublicKeyParameters(new BigInteger(1, array3), gost3410PublicKeyAlgParameters2.PublicKeyParamSet);
					}
					if (algorithm.Equals(EdECObjectIdentifiers.id_X25519))
					{
						return new X25519PublicKeyParameters(PublicKeyFactory.GetRawKey(keyInfo, X25519PublicKeyParameters.KeySize), 0);
					}
					if (algorithm.Equals(EdECObjectIdentifiers.id_X448))
					{
						return new X448PublicKeyParameters(PublicKeyFactory.GetRawKey(keyInfo, X448PublicKeyParameters.KeySize), 0);
					}
					if (algorithm.Equals(EdECObjectIdentifiers.id_Ed25519))
					{
						return new Ed25519PublicKeyParameters(PublicKeyFactory.GetRawKey(keyInfo, Ed25519PublicKeyParameters.KeySize), 0);
					}
					if (algorithm.Equals(EdECObjectIdentifiers.id_Ed448))
					{
						return new Ed448PublicKeyParameters(PublicKeyFactory.GetRawKey(keyInfo, Ed448PublicKeyParameters.KeySize), 0);
					}
					throw new SecurityUtilityException("algorithm identifier in public key not recognised: " + algorithm);
				}
			}
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x000CDF1C File Offset: 0x000CC11C
		private static byte[] GetRawKey(SubjectPublicKeyInfo keyInfo, int expectedSize)
		{
			byte[] octets = keyInfo.PublicKeyData.GetOctets();
			if (expectedSize != octets.Length)
			{
				throw new SecurityUtilityException("public key encoding has incorrect length");
			}
			return octets;
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x000CDF48 File Offset: 0x000CC148
		private static bool IsPkcsDHParam(Asn1Sequence seq)
		{
			if (seq.Count == 2)
			{
				return true;
			}
			if (seq.Count > 3)
			{
				return false;
			}
			DerInteger instance = DerInteger.GetInstance(seq[2]);
			DerInteger instance2 = DerInteger.GetInstance(seq[0]);
			return instance.Value.CompareTo(BigInteger.ValueOf((long)instance2.Value.BitLength)) <= 0;
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x000CDFA8 File Offset: 0x000CC1A8
		private static DHPublicKeyParameters ReadPkcsDHParam(DerObjectIdentifier algOid, BigInteger y, Asn1Sequence seq)
		{
			DHParameter dhparameter = new DHParameter(seq);
			BigInteger l = dhparameter.L;
			int l2 = (l == null) ? 0 : l.IntValue;
			DHParameters parameters = new DHParameters(dhparameter.P, dhparameter.G, null, l2);
			return new DHPublicKeyParameters(y, parameters, algOid);
		}
	}
}
