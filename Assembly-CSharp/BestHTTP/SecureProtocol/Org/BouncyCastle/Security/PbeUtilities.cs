using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.BC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.TeleTrust;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002E5 RID: 741
	public sealed class PbeUtilities
	{
		// Token: 0x06001B10 RID: 6928 RVA: 0x00022F1F File Offset: 0x0002111F
		private PbeUtilities()
		{
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x000CBEB8 File Offset: 0x000CA0B8
		static PbeUtilities()
		{
			PbeUtilities.algorithms["PKCS5SCHEME1"] = "Pkcs5scheme1";
			PbeUtilities.algorithms["PKCS5SCHEME2"] = "Pkcs5scheme2";
			PbeUtilities.algorithms[PkcsObjectIdentifiers.IdPbeS2.Id] = "Pkcs5scheme2";
			PbeUtilities.algorithms["PBEWITHMD2ANDDES-CBC"] = "PBEwithMD2andDES-CBC";
			PbeUtilities.algorithms[PkcsObjectIdentifiers.PbeWithMD2AndDesCbc.Id] = "PBEwithMD2andDES-CBC";
			PbeUtilities.algorithms["PBEWITHMD2ANDRC2-CBC"] = "PBEwithMD2andRC2-CBC";
			PbeUtilities.algorithms[PkcsObjectIdentifiers.PbeWithMD2AndRC2Cbc.Id] = "PBEwithMD2andRC2-CBC";
			PbeUtilities.algorithms["PBEWITHMD5ANDDES-CBC"] = "PBEwithMD5andDES-CBC";
			PbeUtilities.algorithms[PkcsObjectIdentifiers.PbeWithMD5AndDesCbc.Id] = "PBEwithMD5andDES-CBC";
			PbeUtilities.algorithms["PBEWITHMD5ANDRC2-CBC"] = "PBEwithMD5andRC2-CBC";
			PbeUtilities.algorithms[PkcsObjectIdentifiers.PbeWithMD5AndRC2Cbc.Id] = "PBEwithMD5andRC2-CBC";
			PbeUtilities.algorithms["PBEWITHSHA1ANDDES"] = "PBEwithSHA-1andDES-CBC";
			PbeUtilities.algorithms["PBEWITHSHA-1ANDDES"] = "PBEwithSHA-1andDES-CBC";
			PbeUtilities.algorithms["PBEWITHSHA1ANDDES-CBC"] = "PBEwithSHA-1andDES-CBC";
			PbeUtilities.algorithms["PBEWITHSHA-1ANDDES-CBC"] = "PBEwithSHA-1andDES-CBC";
			PbeUtilities.algorithms[PkcsObjectIdentifiers.PbeWithSha1AndDesCbc.Id] = "PBEwithSHA-1andDES-CBC";
			PbeUtilities.algorithms["PBEWITHSHA1ANDRC2"] = "PBEwithSHA-1andRC2-CBC";
			PbeUtilities.algorithms["PBEWITHSHA-1ANDRC2"] = "PBEwithSHA-1andRC2-CBC";
			PbeUtilities.algorithms["PBEWITHSHA1ANDRC2-CBC"] = "PBEwithSHA-1andRC2-CBC";
			PbeUtilities.algorithms["PBEWITHSHA-1ANDRC2-CBC"] = "PBEwithSHA-1andRC2-CBC";
			PbeUtilities.algorithms[PkcsObjectIdentifiers.PbeWithSha1AndRC2Cbc.Id] = "PBEwithSHA-1andRC2-CBC";
			PbeUtilities.algorithms["PKCS12"] = "Pkcs12";
			PbeUtilities.algorithms[BCObjectIdentifiers.bc_pbe_sha1_pkcs12_aes128_cbc.Id] = "PBEwithSHA-1and128bitAES-CBC-BC";
			PbeUtilities.algorithms[BCObjectIdentifiers.bc_pbe_sha1_pkcs12_aes192_cbc.Id] = "PBEwithSHA-1and192bitAES-CBC-BC";
			PbeUtilities.algorithms[BCObjectIdentifiers.bc_pbe_sha1_pkcs12_aes256_cbc.Id] = "PBEwithSHA-1and256bitAES-CBC-BC";
			PbeUtilities.algorithms[BCObjectIdentifiers.bc_pbe_sha256_pkcs12_aes128_cbc.Id] = "PBEwithSHA-256and128bitAES-CBC-BC";
			PbeUtilities.algorithms[BCObjectIdentifiers.bc_pbe_sha256_pkcs12_aes192_cbc.Id] = "PBEwithSHA-256and192bitAES-CBC-BC";
			PbeUtilities.algorithms[BCObjectIdentifiers.bc_pbe_sha256_pkcs12_aes256_cbc.Id] = "PBEwithSHA-256and256bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHAAND128BITRC4"] = "PBEwithSHA-1and128bitRC4";
			PbeUtilities.algorithms["PBEWITHSHA1AND128BITRC4"] = "PBEwithSHA-1and128bitRC4";
			PbeUtilities.algorithms["PBEWITHSHA-1AND128BITRC4"] = "PBEwithSHA-1and128bitRC4";
			PbeUtilities.algorithms[PkcsObjectIdentifiers.PbeWithShaAnd128BitRC4.Id] = "PBEwithSHA-1and128bitRC4";
			PbeUtilities.algorithms["PBEWITHSHAAND40BITRC4"] = "PBEwithSHA-1and40bitRC4";
			PbeUtilities.algorithms["PBEWITHSHA1AND40BITRC4"] = "PBEwithSHA-1and40bitRC4";
			PbeUtilities.algorithms["PBEWITHSHA-1AND40BITRC4"] = "PBEwithSHA-1and40bitRC4";
			PbeUtilities.algorithms[PkcsObjectIdentifiers.PbeWithShaAnd40BitRC4.Id] = "PBEwithSHA-1and40bitRC4";
			PbeUtilities.algorithms["PBEWITHSHAAND3-KEYDESEDE-CBC"] = "PBEwithSHA-1and3-keyDESEDE-CBC";
			PbeUtilities.algorithms["PBEWITHSHAAND3-KEYTRIPLEDES-CBC"] = "PBEwithSHA-1and3-keyDESEDE-CBC";
			PbeUtilities.algorithms["PBEWITHSHA1AND3-KEYDESEDE-CBC"] = "PBEwithSHA-1and3-keyDESEDE-CBC";
			PbeUtilities.algorithms["PBEWITHSHA1AND3-KEYTRIPLEDES-CBC"] = "PBEwithSHA-1and3-keyDESEDE-CBC";
			PbeUtilities.algorithms["PBEWITHSHA-1AND3-KEYDESEDE-CBC"] = "PBEwithSHA-1and3-keyDESEDE-CBC";
			PbeUtilities.algorithms["PBEWITHSHA-1AND3-KEYTRIPLEDES-CBC"] = "PBEwithSHA-1and3-keyDESEDE-CBC";
			PbeUtilities.algorithms[PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc.Id] = "PBEwithSHA-1and3-keyDESEDE-CBC";
			PbeUtilities.algorithms["PBEWITHSHAAND2-KEYDESEDE-CBC"] = "PBEwithSHA-1and2-keyDESEDE-CBC";
			PbeUtilities.algorithms["PBEWITHSHAAND2-KEYTRIPLEDES-CBC"] = "PBEwithSHA-1and2-keyDESEDE-CBC";
			PbeUtilities.algorithms["PBEWITHSHA1AND2-KEYDESEDE-CBC"] = "PBEwithSHA-1and2-keyDESEDE-CBC";
			PbeUtilities.algorithms["PBEWITHSHA1AND2-KEYTRIPLEDES-CBC"] = "PBEwithSHA-1and2-keyDESEDE-CBC";
			PbeUtilities.algorithms["PBEWITHSHA-1AND2-KEYDESEDE-CBC"] = "PBEwithSHA-1and2-keyDESEDE-CBC";
			PbeUtilities.algorithms["PBEWITHSHA-1AND2-KEYTRIPLEDES-CBC"] = "PBEwithSHA-1and2-keyDESEDE-CBC";
			PbeUtilities.algorithms[PkcsObjectIdentifiers.PbeWithShaAnd2KeyTripleDesCbc.Id] = "PBEwithSHA-1and2-keyDESEDE-CBC";
			PbeUtilities.algorithms["PBEWITHSHAAND128BITRC2-CBC"] = "PBEwithSHA-1and128bitRC2-CBC";
			PbeUtilities.algorithms["PBEWITHSHA1AND128BITRC2-CBC"] = "PBEwithSHA-1and128bitRC2-CBC";
			PbeUtilities.algorithms["PBEWITHSHA-1AND128BITRC2-CBC"] = "PBEwithSHA-1and128bitRC2-CBC";
			PbeUtilities.algorithms[PkcsObjectIdentifiers.PbeWithShaAnd128BitRC2Cbc.Id] = "PBEwithSHA-1and128bitRC2-CBC";
			PbeUtilities.algorithms["PBEWITHSHAAND40BITRC2-CBC"] = "PBEwithSHA-1and40bitRC2-CBC";
			PbeUtilities.algorithms["PBEWITHSHA1AND40BITRC2-CBC"] = "PBEwithSHA-1and40bitRC2-CBC";
			PbeUtilities.algorithms["PBEWITHSHA-1AND40BITRC2-CBC"] = "PBEwithSHA-1and40bitRC2-CBC";
			PbeUtilities.algorithms[PkcsObjectIdentifiers.PbewithShaAnd40BitRC2Cbc.Id] = "PBEwithSHA-1and40bitRC2-CBC";
			PbeUtilities.algorithms["PBEWITHSHAAND128BITAES-CBC-BC"] = "PBEwithSHA-1and128bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHA1AND128BITAES-CBC-BC"] = "PBEwithSHA-1and128bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHA-1AND128BITAES-CBC-BC"] = "PBEwithSHA-1and128bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHAAND192BITAES-CBC-BC"] = "PBEwithSHA-1and192bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHA1AND192BITAES-CBC-BC"] = "PBEwithSHA-1and192bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHA-1AND192BITAES-CBC-BC"] = "PBEwithSHA-1and192bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHAAND256BITAES-CBC-BC"] = "PBEwithSHA-1and256bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHA1AND256BITAES-CBC-BC"] = "PBEwithSHA-1and256bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHA-1AND256BITAES-CBC-BC"] = "PBEwithSHA-1and256bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHA256AND128BITAES-CBC-BC"] = "PBEwithSHA-256and128bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHA-256AND128BITAES-CBC-BC"] = "PBEwithSHA-256and128bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHA256AND192BITAES-CBC-BC"] = "PBEwithSHA-256and192bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHA-256AND192BITAES-CBC-BC"] = "PBEwithSHA-256and192bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHA256AND256BITAES-CBC-BC"] = "PBEwithSHA-256and256bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHA-256AND256BITAES-CBC-BC"] = "PBEwithSHA-256and256bitAES-CBC-BC";
			PbeUtilities.algorithms["PBEWITHSHAANDIDEA"] = "PBEwithSHA-1andIDEA-CBC";
			PbeUtilities.algorithms["PBEWITHSHAANDIDEA-CBC"] = "PBEwithSHA-1andIDEA-CBC";
			PbeUtilities.algorithms["PBEWITHSHAANDTWOFISH"] = "PBEwithSHA-1andTWOFISH-CBC";
			PbeUtilities.algorithms["PBEWITHSHAANDTWOFISH-CBC"] = "PBEwithSHA-1andTWOFISH-CBC";
			PbeUtilities.algorithms["PBEWITHHMACSHA1"] = "PBEwithHmacSHA-1";
			PbeUtilities.algorithms["PBEWITHHMACSHA-1"] = "PBEwithHmacSHA-1";
			PbeUtilities.algorithms[OiwObjectIdentifiers.IdSha1.Id] = "PBEwithHmacSHA-1";
			PbeUtilities.algorithms["PBEWITHHMACSHA224"] = "PBEwithHmacSHA-224";
			PbeUtilities.algorithms["PBEWITHHMACSHA-224"] = "PBEwithHmacSHA-224";
			PbeUtilities.algorithms[NistObjectIdentifiers.IdSha224.Id] = "PBEwithHmacSHA-224";
			PbeUtilities.algorithms["PBEWITHHMACSHA256"] = "PBEwithHmacSHA-256";
			PbeUtilities.algorithms["PBEWITHHMACSHA-256"] = "PBEwithHmacSHA-256";
			PbeUtilities.algorithms[NistObjectIdentifiers.IdSha256.Id] = "PBEwithHmacSHA-256";
			PbeUtilities.algorithms["PBEWITHHMACRIPEMD128"] = "PBEwithHmacRipeMD128";
			PbeUtilities.algorithms[TeleTrusTObjectIdentifiers.RipeMD128.Id] = "PBEwithHmacRipeMD128";
			PbeUtilities.algorithms["PBEWITHHMACRIPEMD160"] = "PBEwithHmacRipeMD160";
			PbeUtilities.algorithms[TeleTrusTObjectIdentifiers.RipeMD160.Id] = "PBEwithHmacRipeMD160";
			PbeUtilities.algorithms["PBEWITHHMACRIPEMD256"] = "PBEwithHmacRipeMD256";
			PbeUtilities.algorithms[TeleTrusTObjectIdentifiers.RipeMD256.Id] = "PBEwithHmacRipeMD256";
			PbeUtilities.algorithms["PBEWITHHMACTIGER"] = "PBEwithHmacTiger";
			PbeUtilities.algorithms["PBEWITHMD5AND128BITAES-CBC-OPENSSL"] = "PBEwithMD5and128bitAES-CBC-OpenSSL";
			PbeUtilities.algorithms["PBEWITHMD5AND192BITAES-CBC-OPENSSL"] = "PBEwithMD5and192bitAES-CBC-OpenSSL";
			PbeUtilities.algorithms["PBEWITHMD5AND256BITAES-CBC-OPENSSL"] = "PBEwithMD5and256bitAES-CBC-OpenSSL";
			PbeUtilities.algorithmType["Pkcs5scheme1"] = "Pkcs5S1";
			PbeUtilities.algorithmType["Pkcs5scheme2"] = "Pkcs5S2";
			PbeUtilities.algorithmType["PBEwithMD2andDES-CBC"] = "Pkcs5S1";
			PbeUtilities.algorithmType["PBEwithMD2andRC2-CBC"] = "Pkcs5S1";
			PbeUtilities.algorithmType["PBEwithMD5andDES-CBC"] = "Pkcs5S1";
			PbeUtilities.algorithmType["PBEwithMD5andRC2-CBC"] = "Pkcs5S1";
			PbeUtilities.algorithmType["PBEwithSHA-1andDES-CBC"] = "Pkcs5S1";
			PbeUtilities.algorithmType["PBEwithSHA-1andRC2-CBC"] = "Pkcs5S1";
			PbeUtilities.algorithmType["Pkcs12"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithSHA-1and128bitRC4"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithSHA-1and40bitRC4"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithSHA-1and3-keyDESEDE-CBC"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithSHA-1and2-keyDESEDE-CBC"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithSHA-1and128bitRC2-CBC"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithSHA-1and40bitRC2-CBC"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithSHA-1and128bitAES-CBC-BC"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithSHA-1and192bitAES-CBC-BC"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithSHA-1and256bitAES-CBC-BC"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithSHA-256and128bitAES-CBC-BC"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithSHA-256and192bitAES-CBC-BC"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithSHA-256and256bitAES-CBC-BC"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithSHA-1andIDEA-CBC"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithSHA-1andTWOFISH-CBC"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithHmacSHA-1"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithHmacSHA-224"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithHmacSHA-256"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithHmacRipeMD128"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithHmacRipeMD160"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithHmacRipeMD256"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithHmacTiger"] = "Pkcs12";
			PbeUtilities.algorithmType["PBEwithMD5and128bitAES-CBC-OpenSSL"] = "OpenSsl";
			PbeUtilities.algorithmType["PBEwithMD5and192bitAES-CBC-OpenSSL"] = "OpenSsl";
			PbeUtilities.algorithmType["PBEwithMD5and256bitAES-CBC-OpenSSL"] = "OpenSsl";
			PbeUtilities.oids["PBEwithMD2andDES-CBC"] = PkcsObjectIdentifiers.PbeWithMD2AndDesCbc;
			PbeUtilities.oids["PBEwithMD2andRC2-CBC"] = PkcsObjectIdentifiers.PbeWithMD2AndRC2Cbc;
			PbeUtilities.oids["PBEwithMD5andDES-CBC"] = PkcsObjectIdentifiers.PbeWithMD5AndDesCbc;
			PbeUtilities.oids["PBEwithMD5andRC2-CBC"] = PkcsObjectIdentifiers.PbeWithMD5AndRC2Cbc;
			PbeUtilities.oids["PBEwithSHA-1andDES-CBC"] = PkcsObjectIdentifiers.PbeWithSha1AndDesCbc;
			PbeUtilities.oids["PBEwithSHA-1andRC2-CBC"] = PkcsObjectIdentifiers.PbeWithSha1AndRC2Cbc;
			PbeUtilities.oids["PBEwithSHA-1and128bitRC4"] = PkcsObjectIdentifiers.PbeWithShaAnd128BitRC4;
			PbeUtilities.oids["PBEwithSHA-1and40bitRC4"] = PkcsObjectIdentifiers.PbeWithShaAnd40BitRC4;
			PbeUtilities.oids["PBEwithSHA-1and3-keyDESEDE-CBC"] = PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc;
			PbeUtilities.oids["PBEwithSHA-1and2-keyDESEDE-CBC"] = PkcsObjectIdentifiers.PbeWithShaAnd2KeyTripleDesCbc;
			PbeUtilities.oids["PBEwithSHA-1and128bitRC2-CBC"] = PkcsObjectIdentifiers.PbeWithShaAnd128BitRC2Cbc;
			PbeUtilities.oids["PBEwithSHA-1and40bitRC2-CBC"] = PkcsObjectIdentifiers.PbewithShaAnd40BitRC2Cbc;
			PbeUtilities.oids["PBEwithHmacSHA-1"] = OiwObjectIdentifiers.IdSha1;
			PbeUtilities.oids["PBEwithHmacSHA-224"] = NistObjectIdentifiers.IdSha224;
			PbeUtilities.oids["PBEwithHmacSHA-256"] = NistObjectIdentifiers.IdSha256;
			PbeUtilities.oids["PBEwithHmacRipeMD128"] = TeleTrusTObjectIdentifiers.RipeMD128;
			PbeUtilities.oids["PBEwithHmacRipeMD160"] = TeleTrusTObjectIdentifiers.RipeMD160;
			PbeUtilities.oids["PBEwithHmacRipeMD256"] = TeleTrusTObjectIdentifiers.RipeMD256;
			PbeUtilities.oids["Pkcs5scheme2"] = PkcsObjectIdentifiers.IdPbeS2;
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x000CCAF0 File Offset: 0x000CACF0
		private static PbeParametersGenerator MakePbeGenerator(string type, IDigest digest, byte[] key, byte[] salt, int iterationCount)
		{
			PbeParametersGenerator pbeParametersGenerator;
			if (type.Equals("Pkcs5S1"))
			{
				pbeParametersGenerator = new Pkcs5S1ParametersGenerator(digest);
			}
			else if (type.Equals("Pkcs5S2"))
			{
				pbeParametersGenerator = new Pkcs5S2ParametersGenerator();
			}
			else if (type.Equals("Pkcs12"))
			{
				pbeParametersGenerator = new Pkcs12ParametersGenerator(digest);
			}
			else
			{
				if (!type.Equals("OpenSsl"))
				{
					throw new ArgumentException("Unknown PBE type: " + type, "type");
				}
				pbeParametersGenerator = new OpenSslPbeParametersGenerator();
			}
			pbeParametersGenerator.Init(key, salt, iterationCount);
			return pbeParametersGenerator;
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x000CCB74 File Offset: 0x000CAD74
		public static DerObjectIdentifier GetObjectIdentifier(string mechanism)
		{
			mechanism = (string)PbeUtilities.algorithms[Platform.ToUpperInvariant(mechanism)];
			if (mechanism != null)
			{
				return (DerObjectIdentifier)PbeUtilities.oids[mechanism];
			}
			return null;
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06001B14 RID: 6932 RVA: 0x000CCBA2 File Offset: 0x000CADA2
		public static ICollection Algorithms
		{
			get
			{
				return PbeUtilities.oids.Keys;
			}
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x000CCBB0 File Offset: 0x000CADB0
		public static bool IsPkcs12(string algorithm)
		{
			string text = (string)PbeUtilities.algorithms[Platform.ToUpperInvariant(algorithm)];
			return text != null && "Pkcs12".Equals(PbeUtilities.algorithmType[text]);
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x000CCBF0 File Offset: 0x000CADF0
		public static bool IsPkcs5Scheme1(string algorithm)
		{
			string text = (string)PbeUtilities.algorithms[Platform.ToUpperInvariant(algorithm)];
			return text != null && "Pkcs5S1".Equals(PbeUtilities.algorithmType[text]);
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x000CCC30 File Offset: 0x000CAE30
		public static bool IsPkcs5Scheme2(string algorithm)
		{
			string text = (string)PbeUtilities.algorithms[Platform.ToUpperInvariant(algorithm)];
			return text != null && "Pkcs5S2".Equals(PbeUtilities.algorithmType[text]);
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x000CCC70 File Offset: 0x000CAE70
		public static bool IsOpenSsl(string algorithm)
		{
			string text = (string)PbeUtilities.algorithms[Platform.ToUpperInvariant(algorithm)];
			return text != null && "OpenSsl".Equals(PbeUtilities.algorithmType[text]);
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x000CCCB0 File Offset: 0x000CAEB0
		public static bool IsPbeAlgorithm(string algorithm)
		{
			string text = (string)PbeUtilities.algorithms[Platform.ToUpperInvariant(algorithm)];
			return text != null && PbeUtilities.algorithmType[text] != null;
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x000CCCE6 File Offset: 0x000CAEE6
		public static Asn1Encodable GenerateAlgorithmParameters(DerObjectIdentifier algorithmOid, byte[] salt, int iterationCount)
		{
			return PbeUtilities.GenerateAlgorithmParameters(algorithmOid.Id, salt, iterationCount);
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x000CCCF5 File Offset: 0x000CAEF5
		public static Asn1Encodable GenerateAlgorithmParameters(string algorithm, byte[] salt, int iterationCount)
		{
			if (PbeUtilities.IsPkcs12(algorithm))
			{
				return new Pkcs12PbeParams(salt, iterationCount);
			}
			if (PbeUtilities.IsPkcs5Scheme2(algorithm))
			{
				return new Pbkdf2Params(salt, iterationCount);
			}
			return new PbeParameter(salt, iterationCount);
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x000CCD1E File Offset: 0x000CAF1E
		public static ICipherParameters GenerateCipherParameters(DerObjectIdentifier algorithmOid, char[] password, Asn1Encodable pbeParameters)
		{
			return PbeUtilities.GenerateCipherParameters(algorithmOid.Id, password, false, pbeParameters);
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x000CCD2E File Offset: 0x000CAF2E
		public static ICipherParameters GenerateCipherParameters(DerObjectIdentifier algorithmOid, char[] password, bool wrongPkcs12Zero, Asn1Encodable pbeParameters)
		{
			return PbeUtilities.GenerateCipherParameters(algorithmOid.Id, password, wrongPkcs12Zero, pbeParameters);
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x000CCD3E File Offset: 0x000CAF3E
		public static ICipherParameters GenerateCipherParameters(AlgorithmIdentifier algID, char[] password)
		{
			return PbeUtilities.GenerateCipherParameters(algID.Algorithm.Id, password, false, algID.Parameters);
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x000CCD58 File Offset: 0x000CAF58
		public static ICipherParameters GenerateCipherParameters(AlgorithmIdentifier algID, char[] password, bool wrongPkcs12Zero)
		{
			return PbeUtilities.GenerateCipherParameters(algID.Algorithm.Id, password, wrongPkcs12Zero, algID.Parameters);
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x000CCD72 File Offset: 0x000CAF72
		public static ICipherParameters GenerateCipherParameters(string algorithm, char[] password, Asn1Encodable pbeParameters)
		{
			return PbeUtilities.GenerateCipherParameters(algorithm, password, false, pbeParameters);
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x000CCD80 File Offset: 0x000CAF80
		public static ICipherParameters GenerateCipherParameters(string algorithm, char[] password, bool wrongPkcs12Zero, Asn1Encodable pbeParameters)
		{
			string text = (string)PbeUtilities.algorithms[Platform.ToUpperInvariant(algorithm)];
			byte[] array = null;
			byte[] salt = null;
			int iterationCount = 0;
			if (PbeUtilities.IsPkcs12(text))
			{
				Pkcs12PbeParams instance = Pkcs12PbeParams.GetInstance(pbeParameters);
				salt = instance.GetIV();
				iterationCount = instance.Iterations.IntValue;
				array = PbeParametersGenerator.Pkcs12PasswordToBytes(password, wrongPkcs12Zero);
			}
			else if (!PbeUtilities.IsPkcs5Scheme2(text))
			{
				PbeParameter instance2 = PbeParameter.GetInstance(pbeParameters);
				salt = instance2.GetSalt();
				iterationCount = instance2.IterationCount.IntValue;
				array = PbeParametersGenerator.Pkcs5PasswordToBytes(password);
			}
			ICipherParameters parameters = null;
			if (PbeUtilities.IsPkcs5Scheme2(text))
			{
				PbeS2Parameters instance3 = PbeS2Parameters.GetInstance(pbeParameters.ToAsn1Object());
				EncryptionScheme encryptionScheme = instance3.EncryptionScheme;
				DerObjectIdentifier algorithm2 = encryptionScheme.Algorithm;
				Asn1Object obj = encryptionScheme.Parameters.ToAsn1Object();
				Pbkdf2Params instance4 = Pbkdf2Params.GetInstance(instance3.KeyDerivationFunc.Parameters.ToAsn1Object());
				byte[] array2;
				if (algorithm2.Equals(PkcsObjectIdentifiers.RC2Cbc))
				{
					array2 = RC2CbcParameter.GetInstance(obj).GetIV();
				}
				else
				{
					array2 = Asn1OctetString.GetInstance(obj).GetOctets();
				}
				salt = instance4.GetSalt();
				iterationCount = instance4.IterationCount.IntValue;
				array = PbeParametersGenerator.Pkcs5PasswordToBytes(password);
				int keySize = (instance4.KeyLength != null) ? (instance4.KeyLength.IntValue * 8) : GeneratorUtilities.GetDefaultKeySize(algorithm2);
				parameters = PbeUtilities.MakePbeGenerator((string)PbeUtilities.algorithmType[text], null, array, salt, iterationCount).GenerateDerivedParameters(algorithm2.Id, keySize);
				if (array2 != null && !Arrays.AreEqual(array2, new byte[array2.Length]))
				{
					parameters = new ParametersWithIV(parameters, array2);
				}
			}
			else if (Platform.StartsWith(text, "PBEwithSHA-1"))
			{
				PbeParametersGenerator pbeParametersGenerator = PbeUtilities.MakePbeGenerator((string)PbeUtilities.algorithmType[text], new Sha1Digest(), array, salt, iterationCount);
				if (text.Equals("PBEwithSHA-1and128bitAES-CBC-BC"))
				{
					parameters = pbeParametersGenerator.GenerateDerivedParameters("AES", 128, 128);
				}
				else if (text.Equals("PBEwithSHA-1and192bitAES-CBC-BC"))
				{
					parameters = pbeParametersGenerator.GenerateDerivedParameters("AES", 192, 128);
				}
				else if (text.Equals("PBEwithSHA-1and256bitAES-CBC-BC"))
				{
					parameters = pbeParametersGenerator.GenerateDerivedParameters("AES", 256, 128);
				}
				else if (text.Equals("PBEwithSHA-1and128bitRC4"))
				{
					parameters = pbeParametersGenerator.GenerateDerivedParameters("RC4", 128);
				}
				else if (text.Equals("PBEwithSHA-1and40bitRC4"))
				{
					parameters = pbeParametersGenerator.GenerateDerivedParameters("RC4", 40);
				}
				else if (text.Equals("PBEwithSHA-1and3-keyDESEDE-CBC"))
				{
					parameters = pbeParametersGenerator.GenerateDerivedParameters("DESEDE", 192, 64);
				}
				else if (text.Equals("PBEwithSHA-1and2-keyDESEDE-CBC"))
				{
					parameters = pbeParametersGenerator.GenerateDerivedParameters("DESEDE", 128, 64);
				}
				else if (text.Equals("PBEwithSHA-1and128bitRC2-CBC"))
				{
					parameters = pbeParametersGenerator.GenerateDerivedParameters("RC2", 128, 64);
				}
				else if (text.Equals("PBEwithSHA-1and40bitRC2-CBC"))
				{
					parameters = pbeParametersGenerator.GenerateDerivedParameters("RC2", 40, 64);
				}
				else if (text.Equals("PBEwithSHA-1andDES-CBC"))
				{
					parameters = pbeParametersGenerator.GenerateDerivedParameters("DES", 64, 64);
				}
				else if (text.Equals("PBEwithSHA-1andRC2-CBC"))
				{
					parameters = pbeParametersGenerator.GenerateDerivedParameters("RC2", 64, 64);
				}
			}
			else if (Platform.StartsWith(text, "PBEwithSHA-256"))
			{
				PbeParametersGenerator pbeParametersGenerator2 = PbeUtilities.MakePbeGenerator((string)PbeUtilities.algorithmType[text], new Sha256Digest(), array, salt, iterationCount);
				if (text.Equals("PBEwithSHA-256and128bitAES-CBC-BC"))
				{
					parameters = pbeParametersGenerator2.GenerateDerivedParameters("AES", 128, 128);
				}
				else if (text.Equals("PBEwithSHA-256and192bitAES-CBC-BC"))
				{
					parameters = pbeParametersGenerator2.GenerateDerivedParameters("AES", 192, 128);
				}
				else if (text.Equals("PBEwithSHA-256and256bitAES-CBC-BC"))
				{
					parameters = pbeParametersGenerator2.GenerateDerivedParameters("AES", 256, 128);
				}
			}
			else if (Platform.StartsWith(text, "PBEwithMD5"))
			{
				PbeParametersGenerator pbeParametersGenerator3 = PbeUtilities.MakePbeGenerator((string)PbeUtilities.algorithmType[text], new MD5Digest(), array, salt, iterationCount);
				if (text.Equals("PBEwithMD5andDES-CBC"))
				{
					parameters = pbeParametersGenerator3.GenerateDerivedParameters("DES", 64, 64);
				}
				else if (text.Equals("PBEwithMD5andRC2-CBC"))
				{
					parameters = pbeParametersGenerator3.GenerateDerivedParameters("RC2", 64, 64);
				}
				else if (text.Equals("PBEwithMD5and128bitAES-CBC-OpenSSL"))
				{
					parameters = pbeParametersGenerator3.GenerateDerivedParameters("AES", 128, 128);
				}
				else if (text.Equals("PBEwithMD5and192bitAES-CBC-OpenSSL"))
				{
					parameters = pbeParametersGenerator3.GenerateDerivedParameters("AES", 192, 128);
				}
				else if (text.Equals("PBEwithMD5and256bitAES-CBC-OpenSSL"))
				{
					parameters = pbeParametersGenerator3.GenerateDerivedParameters("AES", 256, 128);
				}
			}
			else if (Platform.StartsWith(text, "PBEwithMD2"))
			{
				PbeParametersGenerator pbeParametersGenerator4 = PbeUtilities.MakePbeGenerator((string)PbeUtilities.algorithmType[text], new MD2Digest(), array, salt, iterationCount);
				if (text.Equals("PBEwithMD2andDES-CBC"))
				{
					parameters = pbeParametersGenerator4.GenerateDerivedParameters("DES", 64, 64);
				}
				else if (text.Equals("PBEwithMD2andRC2-CBC"))
				{
					parameters = pbeParametersGenerator4.GenerateDerivedParameters("RC2", 64, 64);
				}
			}
			else if (Platform.StartsWith(text, "PBEwithHmac"))
			{
				IDigest digest = DigestUtilities.GetDigest(text.Substring("PBEwithHmac".Length));
				PbeParametersGenerator pbeParametersGenerator5 = PbeUtilities.MakePbeGenerator((string)PbeUtilities.algorithmType[text], digest, array, salt, iterationCount);
				int keySize2 = digest.GetDigestSize() * 8;
				parameters = pbeParametersGenerator5.GenerateDerivedMacParameters(keySize2);
			}
			Array.Clear(array, 0, array.Length);
			return PbeUtilities.FixDesParity(text, parameters);
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x000CD354 File Offset: 0x000CB554
		public static object CreateEngine(DerObjectIdentifier algorithmOid)
		{
			return PbeUtilities.CreateEngine(algorithmOid.Id);
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x000CD364 File Offset: 0x000CB564
		public static object CreateEngine(AlgorithmIdentifier algID)
		{
			string id = algID.Algorithm.Id;
			if (PbeUtilities.IsPkcs5Scheme2(id))
			{
				return CipherUtilities.GetCipher(PbeS2Parameters.GetInstance(algID.Parameters.ToAsn1Object()).EncryptionScheme.Algorithm);
			}
			return PbeUtilities.CreateEngine(id);
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x000CD3AC File Offset: 0x000CB5AC
		public static object CreateEngine(string algorithm)
		{
			string text = (string)PbeUtilities.algorithms[Platform.ToUpperInvariant(algorithm)];
			if (Platform.StartsWith(text, "PBEwithHmac"))
			{
				string str = text.Substring("PBEwithHmac".Length);
				return MacUtilities.GetMac("HMAC/" + str);
			}
			if (Platform.StartsWith(text, "PBEwithMD2") || Platform.StartsWith(text, "PBEwithMD5") || Platform.StartsWith(text, "PBEwithSHA-1") || Platform.StartsWith(text, "PBEwithSHA-256"))
			{
				if (Platform.EndsWith(text, "AES-CBC-BC") || Platform.EndsWith(text, "AES-CBC-OPENSSL"))
				{
					return CipherUtilities.GetCipher("AES/CBC");
				}
				if (Platform.EndsWith(text, "DES-CBC"))
				{
					return CipherUtilities.GetCipher("DES/CBC");
				}
				if (Platform.EndsWith(text, "DESEDE-CBC"))
				{
					return CipherUtilities.GetCipher("DESEDE/CBC");
				}
				if (Platform.EndsWith(text, "RC2-CBC"))
				{
					return CipherUtilities.GetCipher("RC2/CBC");
				}
				if (Platform.EndsWith(text, "RC4"))
				{
					return CipherUtilities.GetCipher("RC4");
				}
			}
			return null;
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x000CD4BB File Offset: 0x000CB6BB
		public static string GetEncodingName(DerObjectIdentifier oid)
		{
			return (string)PbeUtilities.algorithms[oid.Id];
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x000CD4D4 File Offset: 0x000CB6D4
		private static ICipherParameters FixDesParity(string mechanism, ICipherParameters parameters)
		{
			if (!Platform.EndsWith(mechanism, "DES-CBC") && !Platform.EndsWith(mechanism, "DESEDE-CBC"))
			{
				return parameters;
			}
			if (parameters is ParametersWithIV)
			{
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				return new ParametersWithIV(PbeUtilities.FixDesParity(mechanism, parametersWithIV.Parameters), parametersWithIV.GetIV());
			}
			byte[] key = ((KeyParameter)parameters).GetKey();
			DesParameters.SetOddParity(key);
			return new KeyParameter(key);
		}

		// Token: 0x040018E7 RID: 6375
		private const string Pkcs5S1 = "Pkcs5S1";

		// Token: 0x040018E8 RID: 6376
		private const string Pkcs5S2 = "Pkcs5S2";

		// Token: 0x040018E9 RID: 6377
		private const string Pkcs12 = "Pkcs12";

		// Token: 0x040018EA RID: 6378
		private const string OpenSsl = "OpenSsl";

		// Token: 0x040018EB RID: 6379
		private static readonly IDictionary algorithms = Platform.CreateHashtable();

		// Token: 0x040018EC RID: 6380
		private static readonly IDictionary algorithmType = Platform.CreateHashtable();

		// Token: 0x040018ED RID: 6381
		private static readonly IDictionary oids = Platform.CreateHashtable();
	}
}
