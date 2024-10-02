using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002DC RID: 732
	public sealed class DotNetUtilities
	{
		// Token: 0x06001AC0 RID: 6848 RVA: 0x00022F1F File Offset: 0x0002111F
		private DotNetUtilities()
		{
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x000C9C96 File Offset: 0x000C7E96
		public static X509Certificate ToX509Certificate(X509CertificateStructure x509Struct)
		{
			return new X509Certificate(x509Struct.GetDerEncoded());
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x000C9CA3 File Offset: 0x000C7EA3
		public static X509Certificate ToX509Certificate(X509Certificate x509Cert)
		{
			return new X509Certificate(x509Cert.GetEncoded());
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x000C9CB0 File Offset: 0x000C7EB0
		public static X509Certificate FromX509Certificate(X509Certificate x509Cert)
		{
			return new X509CertificateParser().ReadCertificate(x509Cert.GetRawCertData());
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x000C9CC2 File Offset: 0x000C7EC2
		public static AsymmetricCipherKeyPair GetDsaKeyPair(DSA dsa)
		{
			return DotNetUtilities.GetDsaKeyPair(dsa.ExportParameters(true));
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x000C9CD0 File Offset: 0x000C7ED0
		public static AsymmetricCipherKeyPair GetDsaKeyPair(DSAParameters dp)
		{
			DsaValidationParameters parameters = (dp.Seed != null) ? new DsaValidationParameters(dp.Seed, dp.Counter) : null;
			DsaParameters parameters2 = new DsaParameters(new BigInteger(1, dp.P), new BigInteger(1, dp.Q), new BigInteger(1, dp.G), parameters);
			AsymmetricKeyParameter publicParameter = new DsaPublicKeyParameters(new BigInteger(1, dp.Y), parameters2);
			DsaPrivateKeyParameters privateParameter = new DsaPrivateKeyParameters(new BigInteger(1, dp.X), parameters2);
			return new AsymmetricCipherKeyPair(publicParameter, privateParameter);
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x000C9D50 File Offset: 0x000C7F50
		public static DsaPublicKeyParameters GetDsaPublicKey(DSA dsa)
		{
			return DotNetUtilities.GetDsaPublicKey(dsa.ExportParameters(false));
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x000C9D60 File Offset: 0x000C7F60
		public static DsaPublicKeyParameters GetDsaPublicKey(DSAParameters dp)
		{
			DsaValidationParameters parameters = (dp.Seed != null) ? new DsaValidationParameters(dp.Seed, dp.Counter) : null;
			DsaParameters parameters2 = new DsaParameters(new BigInteger(1, dp.P), new BigInteger(1, dp.Q), new BigInteger(1, dp.G), parameters);
			return new DsaPublicKeyParameters(new BigInteger(1, dp.Y), parameters2);
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x000C9DC7 File Offset: 0x000C7FC7
		public static AsymmetricCipherKeyPair GetRsaKeyPair(RSA rsa)
		{
			return DotNetUtilities.GetRsaKeyPair(rsa.ExportParameters(true));
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x000C9DD8 File Offset: 0x000C7FD8
		public static AsymmetricCipherKeyPair GetRsaKeyPair(RSAParameters rp)
		{
			BigInteger modulus = new BigInteger(1, rp.Modulus);
			BigInteger bigInteger = new BigInteger(1, rp.Exponent);
			AsymmetricKeyParameter publicParameter = new RsaKeyParameters(false, modulus, bigInteger);
			RsaPrivateCrtKeyParameters privateParameter = new RsaPrivateCrtKeyParameters(modulus, bigInteger, new BigInteger(1, rp.D), new BigInteger(1, rp.P), new BigInteger(1, rp.Q), new BigInteger(1, rp.DP), new BigInteger(1, rp.DQ), new BigInteger(1, rp.InverseQ));
			return new AsymmetricCipherKeyPair(publicParameter, privateParameter);
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x000C9E5D File Offset: 0x000C805D
		public static RsaKeyParameters GetRsaPublicKey(RSA rsa)
		{
			return DotNetUtilities.GetRsaPublicKey(rsa.ExportParameters(false));
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x000C9E6B File Offset: 0x000C806B
		public static RsaKeyParameters GetRsaPublicKey(RSAParameters rp)
		{
			return new RsaKeyParameters(false, new BigInteger(1, rp.Modulus), new BigInteger(1, rp.Exponent));
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x000C9E8B File Offset: 0x000C808B
		public static AsymmetricCipherKeyPair GetKeyPair(AsymmetricAlgorithm privateKey)
		{
			if (privateKey is DSA)
			{
				return DotNetUtilities.GetDsaKeyPair((DSA)privateKey);
			}
			if (privateKey is RSA)
			{
				return DotNetUtilities.GetRsaKeyPair((RSA)privateKey);
			}
			throw new ArgumentException("Unsupported algorithm specified", "privateKey");
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x000C9EC4 File Offset: 0x000C80C4
		public static RSA ToRSA(RsaKeyParameters rsaKey)
		{
			return DotNetUtilities.CreateRSAProvider(DotNetUtilities.ToRSAParameters(rsaKey));
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x000C9ED1 File Offset: 0x000C80D1
		public static RSA ToRSA(RsaKeyParameters rsaKey, CspParameters csp)
		{
			return DotNetUtilities.CreateRSAProvider(DotNetUtilities.ToRSAParameters(rsaKey), csp);
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x000C9EDF File Offset: 0x000C80DF
		public static RSA ToRSA(RsaPrivateCrtKeyParameters privKey)
		{
			return DotNetUtilities.CreateRSAProvider(DotNetUtilities.ToRSAParameters(privKey));
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x000C9EEC File Offset: 0x000C80EC
		public static RSA ToRSA(RsaPrivateCrtKeyParameters privKey, CspParameters csp)
		{
			return DotNetUtilities.CreateRSAProvider(DotNetUtilities.ToRSAParameters(privKey), csp);
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x000C9EFA File Offset: 0x000C80FA
		public static RSA ToRSA(RsaPrivateKeyStructure privKey)
		{
			return DotNetUtilities.CreateRSAProvider(DotNetUtilities.ToRSAParameters(privKey));
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x000C9F07 File Offset: 0x000C8107
		public static RSA ToRSA(RsaPrivateKeyStructure privKey, CspParameters csp)
		{
			return DotNetUtilities.CreateRSAProvider(DotNetUtilities.ToRSAParameters(privKey), csp);
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x000C9F18 File Offset: 0x000C8118
		public static RSAParameters ToRSAParameters(RsaKeyParameters rsaKey)
		{
			RSAParameters rsaparameters = default(RSAParameters);
			rsaparameters.Modulus = rsaKey.Modulus.ToByteArrayUnsigned();
			if (rsaKey.IsPrivate)
			{
				rsaparameters.D = DotNetUtilities.ConvertRSAParametersField(rsaKey.Exponent, rsaparameters.Modulus.Length);
			}
			else
			{
				rsaparameters.Exponent = rsaKey.Exponent.ToByteArrayUnsigned();
			}
			return rsaparameters;
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x000C9F78 File Offset: 0x000C8178
		public static RSAParameters ToRSAParameters(RsaPrivateCrtKeyParameters privKey)
		{
			RSAParameters rsaparameters = default(RSAParameters);
			rsaparameters.Modulus = privKey.Modulus.ToByteArrayUnsigned();
			rsaparameters.Exponent = privKey.PublicExponent.ToByteArrayUnsigned();
			rsaparameters.P = privKey.P.ToByteArrayUnsigned();
			rsaparameters.Q = privKey.Q.ToByteArrayUnsigned();
			rsaparameters.D = DotNetUtilities.ConvertRSAParametersField(privKey.Exponent, rsaparameters.Modulus.Length);
			rsaparameters.DP = DotNetUtilities.ConvertRSAParametersField(privKey.DP, rsaparameters.P.Length);
			rsaparameters.DQ = DotNetUtilities.ConvertRSAParametersField(privKey.DQ, rsaparameters.Q.Length);
			rsaparameters.InverseQ = DotNetUtilities.ConvertRSAParametersField(privKey.QInv, rsaparameters.Q.Length);
			return rsaparameters;
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x000CA040 File Offset: 0x000C8240
		public static RSAParameters ToRSAParameters(RsaPrivateKeyStructure privKey)
		{
			RSAParameters rsaparameters = default(RSAParameters);
			rsaparameters.Modulus = privKey.Modulus.ToByteArrayUnsigned();
			rsaparameters.Exponent = privKey.PublicExponent.ToByteArrayUnsigned();
			rsaparameters.P = privKey.Prime1.ToByteArrayUnsigned();
			rsaparameters.Q = privKey.Prime2.ToByteArrayUnsigned();
			rsaparameters.D = DotNetUtilities.ConvertRSAParametersField(privKey.PrivateExponent, rsaparameters.Modulus.Length);
			rsaparameters.DP = DotNetUtilities.ConvertRSAParametersField(privKey.Exponent1, rsaparameters.P.Length);
			rsaparameters.DQ = DotNetUtilities.ConvertRSAParametersField(privKey.Exponent2, rsaparameters.Q.Length);
			rsaparameters.InverseQ = DotNetUtilities.ConvertRSAParametersField(privKey.Coefficient, rsaparameters.Q.Length);
			return rsaparameters;
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x000CA108 File Offset: 0x000C8308
		private static byte[] ConvertRSAParametersField(BigInteger n, int size)
		{
			byte[] array = n.ToByteArrayUnsigned();
			if (array.Length == size)
			{
				return array;
			}
			if (array.Length > size)
			{
				throw new ArgumentException("Specified size too small", "size");
			}
			byte[] array2 = new byte[size];
			Array.Copy(array, 0, array2, size - array.Length, array.Length);
			return array2;
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x000CA152 File Offset: 0x000C8352
		private static RSA CreateRSAProvider(RSAParameters rp)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider(new CspParameters
			{
				KeyContainerName = string.Format("BouncyCastle-{0}", Guid.NewGuid())
			});
			rsacryptoServiceProvider.ImportParameters(rp);
			return rsacryptoServiceProvider;
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x000CA17F File Offset: 0x000C837F
		private static RSA CreateRSAProvider(RSAParameters rp, CspParameters csp)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider(csp);
			rsacryptoServiceProvider.ImportParameters(rp);
			return rsacryptoServiceProvider;
		}
	}
}
