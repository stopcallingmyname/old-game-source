using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.EdEC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x0200023E RID: 574
	public sealed class SubjectPublicKeyInfoFactory
	{
		// Token: 0x060014AD RID: 5293 RVA: 0x00022F1F File Offset: 0x0002111F
		private SubjectPublicKeyInfoFactory()
		{
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x000AB0F8 File Offset: 0x000A92F8
		public static SubjectPublicKeyInfo CreateSubjectPublicKeyInfo(AsymmetricKeyParameter publicKey)
		{
			if (publicKey == null)
			{
				throw new ArgumentNullException("publicKey");
			}
			if (publicKey.IsPrivate)
			{
				throw new ArgumentException("Private key passed - public key expected.", "publicKey");
			}
			if (publicKey is ElGamalPublicKeyParameters)
			{
				ElGamalPublicKeyParameters elGamalPublicKeyParameters = (ElGamalPublicKeyParameters)publicKey;
				ElGamalParameters parameters = elGamalPublicKeyParameters.Parameters;
				return new SubjectPublicKeyInfo(new AlgorithmIdentifier(OiwObjectIdentifiers.ElGamalAlgorithm, new ElGamalParameter(parameters.P, parameters.G).ToAsn1Object()), new DerInteger(elGamalPublicKeyParameters.Y));
			}
			if (publicKey is DsaPublicKeyParameters)
			{
				DsaPublicKeyParameters dsaPublicKeyParameters = (DsaPublicKeyParameters)publicKey;
				DsaParameters parameters2 = dsaPublicKeyParameters.Parameters;
				Asn1Encodable parameters3 = (parameters2 == null) ? null : new DsaParameter(parameters2.P, parameters2.Q, parameters2.G).ToAsn1Object();
				return new SubjectPublicKeyInfo(new AlgorithmIdentifier(X9ObjectIdentifiers.IdDsa, parameters3), new DerInteger(dsaPublicKeyParameters.Y));
			}
			if (publicKey is DHPublicKeyParameters)
			{
				DHPublicKeyParameters dhpublicKeyParameters = (DHPublicKeyParameters)publicKey;
				DHParameters parameters4 = dhpublicKeyParameters.Parameters;
				return new SubjectPublicKeyInfo(new AlgorithmIdentifier(dhpublicKeyParameters.AlgorithmOid, new DHParameter(parameters4.P, parameters4.G, parameters4.L).ToAsn1Object()), new DerInteger(dhpublicKeyParameters.Y));
			}
			if (publicKey is RsaKeyParameters)
			{
				RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)publicKey;
				return new SubjectPublicKeyInfo(new AlgorithmIdentifier(PkcsObjectIdentifiers.RsaEncryption, DerNull.Instance), new RsaPublicKeyStructure(rsaKeyParameters.Modulus, rsaKeyParameters.Exponent).ToAsn1Object());
			}
			if (publicKey is ECPublicKeyParameters)
			{
				ECPublicKeyParameters ecpublicKeyParameters = (ECPublicKeyParameters)publicKey;
				if (!(ecpublicKeyParameters.AlgorithmName == "ECGOST3410"))
				{
					X962Parameters x962Parameters;
					if (ecpublicKeyParameters.PublicKeyParamSet == null)
					{
						ECDomainParameters parameters5 = ecpublicKeyParameters.Parameters;
						x962Parameters = new X962Parameters(new X9ECParameters(parameters5.Curve, parameters5.G, parameters5.N, parameters5.H, parameters5.GetSeed()));
					}
					else
					{
						x962Parameters = new X962Parameters(ecpublicKeyParameters.PublicKeyParamSet);
					}
					byte[] encoded = ecpublicKeyParameters.Q.GetEncoded(false);
					return new SubjectPublicKeyInfo(new AlgorithmIdentifier(X9ObjectIdentifiers.IdECPublicKey, x962Parameters.ToAsn1Object()), encoded);
				}
				if (ecpublicKeyParameters.PublicKeyParamSet == null)
				{
					throw Platform.CreateNotImplementedException("Not a CryptoPro parameter set");
				}
				ECPoint ecpoint = ecpublicKeyParameters.Q.Normalize();
				BigInteger bI = ecpoint.AffineXCoord.ToBigInteger();
				BigInteger bI2 = ecpoint.AffineYCoord.ToBigInteger();
				byte[] array = new byte[64];
				SubjectPublicKeyInfoFactory.ExtractBytes(array, 0, bI);
				SubjectPublicKeyInfoFactory.ExtractBytes(array, 32, bI2);
				Gost3410PublicKeyAlgParameters gost3410PublicKeyAlgParameters = new Gost3410PublicKeyAlgParameters(ecpublicKeyParameters.PublicKeyParamSet, CryptoProObjectIdentifiers.GostR3411x94CryptoProParamSet);
				return new SubjectPublicKeyInfo(new AlgorithmIdentifier(CryptoProObjectIdentifiers.GostR3410x2001, gost3410PublicKeyAlgParameters.ToAsn1Object()), new DerOctetString(array));
			}
			else if (publicKey is Gost3410PublicKeyParameters)
			{
				Gost3410PublicKeyParameters gost3410PublicKeyParameters = (Gost3410PublicKeyParameters)publicKey;
				if (gost3410PublicKeyParameters.PublicKeyParamSet == null)
				{
					throw Platform.CreateNotImplementedException("Not a CryptoPro parameter set");
				}
				byte[] array2 = gost3410PublicKeyParameters.Y.ToByteArrayUnsigned();
				byte[] array3 = new byte[array2.Length];
				for (int num = 0; num != array3.Length; num++)
				{
					array3[num] = array2[array2.Length - 1 - num];
				}
				Gost3410PublicKeyAlgParameters gost3410PublicKeyAlgParameters2 = new Gost3410PublicKeyAlgParameters(gost3410PublicKeyParameters.PublicKeyParamSet, CryptoProObjectIdentifiers.GostR3411x94CryptoProParamSet);
				return new SubjectPublicKeyInfo(new AlgorithmIdentifier(CryptoProObjectIdentifiers.GostR3410x94, gost3410PublicKeyAlgParameters2.ToAsn1Object()), new DerOctetString(array3));
			}
			else
			{
				if (publicKey is X448PublicKeyParameters)
				{
					X448PublicKeyParameters x448PublicKeyParameters = (X448PublicKeyParameters)publicKey;
					return new SubjectPublicKeyInfo(new AlgorithmIdentifier(EdECObjectIdentifiers.id_X448), x448PublicKeyParameters.GetEncoded());
				}
				if (publicKey is X25519PublicKeyParameters)
				{
					X25519PublicKeyParameters x25519PublicKeyParameters = (X25519PublicKeyParameters)publicKey;
					return new SubjectPublicKeyInfo(new AlgorithmIdentifier(EdECObjectIdentifiers.id_X25519), x25519PublicKeyParameters.GetEncoded());
				}
				if (publicKey is Ed448PublicKeyParameters)
				{
					Ed448PublicKeyParameters ed448PublicKeyParameters = (Ed448PublicKeyParameters)publicKey;
					return new SubjectPublicKeyInfo(new AlgorithmIdentifier(EdECObjectIdentifiers.id_Ed448), ed448PublicKeyParameters.GetEncoded());
				}
				if (publicKey is Ed25519PublicKeyParameters)
				{
					Ed25519PublicKeyParameters ed25519PublicKeyParameters = (Ed25519PublicKeyParameters)publicKey;
					return new SubjectPublicKeyInfo(new AlgorithmIdentifier(EdECObjectIdentifiers.id_Ed25519), ed25519PublicKeyParameters.GetEncoded());
				}
				throw new ArgumentException("Class provided no convertible: " + Platform.GetTypeName(publicKey));
			}
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x000AB4CC File Offset: 0x000A96CC
		private static void ExtractBytes(byte[] encKey, int offset, BigInteger bI)
		{
			byte[] array = bI.ToByteArray();
			int num = (bI.BitLength + 7) / 8;
			for (int i = 0; i < num; i++)
			{
				encKey[offset + i] = array[array.Length - 1 - i];
			}
		}
	}
}
