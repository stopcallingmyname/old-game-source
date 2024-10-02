using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO.Pem;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.OpenSsl
{
	// Token: 0x020002D1 RID: 721
	public class MiscPemGenerator : PemObjectGenerator
	{
		// Token: 0x06001A7B RID: 6779 RVA: 0x000C6C24 File Offset: 0x000C4E24
		public MiscPemGenerator(object obj)
		{
			this.obj = obj;
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x000C6C33 File Offset: 0x000C4E33
		public MiscPemGenerator(object obj, string algorithm, char[] password, SecureRandom random)
		{
			this.obj = obj;
			this.algorithm = algorithm;
			this.password = password;
			this.random = random;
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x000C6C58 File Offset: 0x000C4E58
		private static PemObject CreatePemObject(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (obj is AsymmetricCipherKeyPair)
			{
				return MiscPemGenerator.CreatePemObject(((AsymmetricCipherKeyPair)obj).Private);
			}
			if (obj is PemObject)
			{
				return (PemObject)obj;
			}
			if (obj is PemObjectGenerator)
			{
				return ((PemObjectGenerator)obj).Generate();
			}
			string type;
			byte[] content;
			if (obj is X509Certificate)
			{
				type = "CERTIFICATE";
				try
				{
					content = ((X509Certificate)obj).GetEncoded();
					goto IL_167;
				}
				catch (CertificateEncodingException ex)
				{
					throw new IOException("Cannot Encode object: " + ex.ToString());
				}
			}
			if (obj is X509Crl)
			{
				type = "X509 CRL";
				try
				{
					content = ((X509Crl)obj).GetEncoded();
					goto IL_167;
				}
				catch (CrlException ex2)
				{
					throw new IOException("Cannot Encode object: " + ex2.ToString());
				}
			}
			if (obj is AsymmetricKeyParameter)
			{
				AsymmetricKeyParameter asymmetricKeyParameter = (AsymmetricKeyParameter)obj;
				if (asymmetricKeyParameter.IsPrivate)
				{
					string str;
					content = MiscPemGenerator.EncodePrivateKey(asymmetricKeyParameter, out str);
					type = str + " PRIVATE KEY";
				}
				else
				{
					type = "PUBLIC KEY";
					content = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(asymmetricKeyParameter).GetDerEncoded();
				}
			}
			else if (obj is IX509AttributeCertificate)
			{
				type = "ATTRIBUTE CERTIFICATE";
				content = ((X509V2AttributeCertificate)obj).GetEncoded();
			}
			else if (obj is Pkcs10CertificationRequest)
			{
				type = "CERTIFICATE REQUEST";
				content = ((Pkcs10CertificationRequest)obj).GetEncoded();
			}
			else
			{
				if (!(obj is BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.ContentInfo))
				{
					throw new PemGenerationException("Object type not supported: " + Platform.GetTypeName(obj));
				}
				type = "PKCS7";
				content = ((BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.ContentInfo)obj).GetEncoded();
			}
			IL_167:
			return new PemObject(type, content);
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x000C6DF0 File Offset: 0x000C4FF0
		private static PemObject CreatePemObject(object obj, string algorithm, char[] password, SecureRandom random)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (algorithm == null)
			{
				throw new ArgumentNullException("algorithm");
			}
			if (password == null)
			{
				throw new ArgumentNullException("password");
			}
			if (random == null)
			{
				throw new ArgumentNullException("random");
			}
			if (obj is AsymmetricCipherKeyPair)
			{
				return MiscPemGenerator.CreatePemObject(((AsymmetricCipherKeyPair)obj).Private, algorithm, password, random);
			}
			string text = null;
			byte[] array = null;
			if (obj is AsymmetricKeyParameter)
			{
				AsymmetricKeyParameter asymmetricKeyParameter = (AsymmetricKeyParameter)obj;
				if (asymmetricKeyParameter.IsPrivate)
				{
					string str;
					array = MiscPemGenerator.EncodePrivateKey(asymmetricKeyParameter, out str);
					text = str + " PRIVATE KEY";
				}
			}
			if (text == null || array == null)
			{
				throw new PemGenerationException("Object type not supported: " + Platform.GetTypeName(obj));
			}
			string text2 = Platform.ToUpperInvariant(algorithm);
			if (text2 == "DESEDE")
			{
				text2 = "DES-EDE3-CBC";
			}
			byte[] array2 = new byte[Platform.StartsWith(text2, "AES-") ? 16 : 8];
			random.NextBytes(array2);
			byte[] content = PemUtilities.Crypt(true, array, password, text2, array2);
			IList list = Platform.CreateArrayList(2);
			list.Add(new PemHeader("Proc-Type", "4,ENCRYPTED"));
			list.Add(new PemHeader("DEK-Info", text2 + "," + Hex.ToHexString(array2)));
			return new PemObject(text, list, content);
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x000C6F34 File Offset: 0x000C5134
		private static byte[] EncodePrivateKey(AsymmetricKeyParameter akp, out string keyType)
		{
			PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(akp);
			AlgorithmIdentifier privateKeyAlgorithm = privateKeyInfo.PrivateKeyAlgorithm;
			DerObjectIdentifier derObjectIdentifier = privateKeyAlgorithm.Algorithm;
			if (derObjectIdentifier.Equals(X9ObjectIdentifiers.IdDsa))
			{
				keyType = "DSA";
				DsaParameter instance = DsaParameter.GetInstance(privateKeyAlgorithm.Parameters);
				BigInteger x = ((DsaPrivateKeyParameters)akp).X;
				BigInteger value = instance.G.ModPow(x, instance.P);
				return new DerSequence(new Asn1Encodable[]
				{
					new DerInteger(0),
					new DerInteger(instance.P),
					new DerInteger(instance.Q),
					new DerInteger(instance.G),
					new DerInteger(value),
					new DerInteger(x)
				}).GetEncoded();
			}
			if (derObjectIdentifier.Equals(PkcsObjectIdentifiers.RsaEncryption))
			{
				keyType = "RSA";
			}
			else
			{
				if (!derObjectIdentifier.Equals(CryptoProObjectIdentifiers.GostR3410x2001) && !derObjectIdentifier.Equals(X9ObjectIdentifiers.IdECPublicKey))
				{
					throw new ArgumentException("Cannot handle private key of type: " + Platform.GetTypeName(akp), "akp");
				}
				keyType = "EC";
			}
			return privateKeyInfo.ParsePrivateKey().GetEncoded();
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x000C7054 File Offset: 0x000C5254
		public PemObject Generate()
		{
			PemObject result;
			try
			{
				if (this.algorithm != null)
				{
					result = MiscPemGenerator.CreatePemObject(this.obj, this.algorithm, this.password, this.random);
				}
				else
				{
					result = MiscPemGenerator.CreatePemObject(this.obj);
				}
			}
			catch (IOException exception)
			{
				throw new PemGenerationException("encoding exception", exception);
			}
			return result;
		}

		// Token: 0x040018CC RID: 6348
		private object obj;

		// Token: 0x040018CD RID: 6349
		private string algorithm;

		// Token: 0x040018CE RID: 6350
		private char[] password;

		// Token: 0x040018CF RID: 6351
		private SecureRandom random;
	}
}
