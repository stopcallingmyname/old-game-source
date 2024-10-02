using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.EdEC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Iana;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Kisa;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ntt;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Rosstandart;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002DE RID: 734
	public sealed class GeneratorUtilities
	{
		// Token: 0x06001ADC RID: 6876 RVA: 0x00022F1F File Offset: 0x0002111F
		private GeneratorUtilities()
		{
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x000CA190 File Offset: 0x000C8390
		static GeneratorUtilities()
		{
			GeneratorUtilities.AddKgAlgorithm("AES", new object[]
			{
				"AESWRAP"
			});
			GeneratorUtilities.AddKgAlgorithm("AES128", new object[]
			{
				"2.16.840.1.101.3.4.2",
				NistObjectIdentifiers.IdAes128Cbc,
				NistObjectIdentifiers.IdAes128Cfb,
				NistObjectIdentifiers.IdAes128Ecb,
				NistObjectIdentifiers.IdAes128Ofb,
				NistObjectIdentifiers.IdAes128Wrap
			});
			GeneratorUtilities.AddKgAlgorithm("AES192", new object[]
			{
				"2.16.840.1.101.3.4.22",
				NistObjectIdentifiers.IdAes192Cbc,
				NistObjectIdentifiers.IdAes192Cfb,
				NistObjectIdentifiers.IdAes192Ecb,
				NistObjectIdentifiers.IdAes192Ofb,
				NistObjectIdentifiers.IdAes192Wrap
			});
			GeneratorUtilities.AddKgAlgorithm("AES256", new object[]
			{
				"2.16.840.1.101.3.4.42",
				NistObjectIdentifiers.IdAes256Cbc,
				NistObjectIdentifiers.IdAes256Cfb,
				NistObjectIdentifiers.IdAes256Ecb,
				NistObjectIdentifiers.IdAes256Ofb,
				NistObjectIdentifiers.IdAes256Wrap
			});
			GeneratorUtilities.AddKgAlgorithm("BLOWFISH", new object[]
			{
				"1.3.6.1.4.1.3029.1.2"
			});
			GeneratorUtilities.AddKgAlgorithm("CAMELLIA", new object[]
			{
				"CAMELLIAWRAP"
			});
			GeneratorUtilities.AddKgAlgorithm("CAMELLIA128", new object[]
			{
				NttObjectIdentifiers.IdCamellia128Cbc,
				NttObjectIdentifiers.IdCamellia128Wrap
			});
			GeneratorUtilities.AddKgAlgorithm("CAMELLIA192", new object[]
			{
				NttObjectIdentifiers.IdCamellia192Cbc,
				NttObjectIdentifiers.IdCamellia192Wrap
			});
			GeneratorUtilities.AddKgAlgorithm("CAMELLIA256", new object[]
			{
				NttObjectIdentifiers.IdCamellia256Cbc,
				NttObjectIdentifiers.IdCamellia256Wrap
			});
			GeneratorUtilities.AddKgAlgorithm("CAST5", new object[]
			{
				"1.2.840.113533.7.66.10"
			});
			GeneratorUtilities.AddKgAlgorithm("CAST6", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("DES", new object[]
			{
				OiwObjectIdentifiers.DesCbc,
				OiwObjectIdentifiers.DesCfb,
				OiwObjectIdentifiers.DesEcb,
				OiwObjectIdentifiers.DesOfb
			});
			GeneratorUtilities.AddKgAlgorithm("DESEDE", new object[]
			{
				"DESEDEWRAP",
				"TDEA",
				OiwObjectIdentifiers.DesEde
			});
			GeneratorUtilities.AddKgAlgorithm("DESEDE3", new object[]
			{
				PkcsObjectIdentifiers.DesEde3Cbc,
				PkcsObjectIdentifiers.IdAlgCms3DesWrap
			});
			GeneratorUtilities.AddKgAlgorithm("GOST28147", new object[]
			{
				"GOST",
				"GOST-28147",
				CryptoProObjectIdentifiers.GostR28147Cbc
			});
			GeneratorUtilities.AddKgAlgorithm("HC128", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("HC256", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("IDEA", new object[]
			{
				"1.3.6.1.4.1.188.7.1.1.2"
			});
			GeneratorUtilities.AddKgAlgorithm("NOEKEON", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("RC2", new object[]
			{
				PkcsObjectIdentifiers.RC2Cbc,
				PkcsObjectIdentifiers.IdAlgCmsRC2Wrap
			});
			GeneratorUtilities.AddKgAlgorithm("RC4", new object[]
			{
				"ARC4",
				"1.2.840.113549.3.4"
			});
			GeneratorUtilities.AddKgAlgorithm("RC5", new object[]
			{
				"RC5-32"
			});
			GeneratorUtilities.AddKgAlgorithm("RC5-64", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("RC6", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("RIJNDAEL", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("SALSA20", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("SEED", new object[]
			{
				KisaObjectIdentifiers.IdNpkiAppCmsSeedWrap,
				KisaObjectIdentifiers.IdSeedCbc
			});
			GeneratorUtilities.AddKgAlgorithm("SERPENT", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("SKIPJACK", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("SM4", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("TEA", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("THREEFISH-256", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("THREEFISH-512", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("THREEFISH-1024", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("TNEPRES", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("TWOFISH", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("VMPC", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("VMPC-KSA3", Array.Empty<object>());
			GeneratorUtilities.AddKgAlgorithm("XTEA", Array.Empty<object>());
			GeneratorUtilities.AddHMacKeyGenerator("MD2", Array.Empty<object>());
			GeneratorUtilities.AddHMacKeyGenerator("MD4", Array.Empty<object>());
			GeneratorUtilities.AddHMacKeyGenerator("MD5", new object[]
			{
				IanaObjectIdentifiers.HmacMD5
			});
			GeneratorUtilities.AddHMacKeyGenerator("SHA1", new object[]
			{
				PkcsObjectIdentifiers.IdHmacWithSha1,
				IanaObjectIdentifiers.HmacSha1
			});
			GeneratorUtilities.AddHMacKeyGenerator("SHA224", new object[]
			{
				PkcsObjectIdentifiers.IdHmacWithSha224
			});
			GeneratorUtilities.AddHMacKeyGenerator("SHA256", new object[]
			{
				PkcsObjectIdentifiers.IdHmacWithSha256
			});
			GeneratorUtilities.AddHMacKeyGenerator("SHA384", new object[]
			{
				PkcsObjectIdentifiers.IdHmacWithSha384
			});
			GeneratorUtilities.AddHMacKeyGenerator("SHA512", new object[]
			{
				PkcsObjectIdentifiers.IdHmacWithSha512
			});
			GeneratorUtilities.AddHMacKeyGenerator("SHA512/224", Array.Empty<object>());
			GeneratorUtilities.AddHMacKeyGenerator("SHA512/256", Array.Empty<object>());
			GeneratorUtilities.AddHMacKeyGenerator("KECCAK224", Array.Empty<object>());
			GeneratorUtilities.AddHMacKeyGenerator("KECCAK256", Array.Empty<object>());
			GeneratorUtilities.AddHMacKeyGenerator("KECCAK288", Array.Empty<object>());
			GeneratorUtilities.AddHMacKeyGenerator("KECCAK384", Array.Empty<object>());
			GeneratorUtilities.AddHMacKeyGenerator("KECCAK512", Array.Empty<object>());
			GeneratorUtilities.AddHMacKeyGenerator("SHA3-224", new object[]
			{
				NistObjectIdentifiers.IdHMacWithSha3_224
			});
			GeneratorUtilities.AddHMacKeyGenerator("SHA3-256", new object[]
			{
				NistObjectIdentifiers.IdHMacWithSha3_256
			});
			GeneratorUtilities.AddHMacKeyGenerator("SHA3-384", new object[]
			{
				NistObjectIdentifiers.IdHMacWithSha3_384
			});
			GeneratorUtilities.AddHMacKeyGenerator("SHA3-512", new object[]
			{
				NistObjectIdentifiers.IdHMacWithSha3_512
			});
			GeneratorUtilities.AddHMacKeyGenerator("RIPEMD128", Array.Empty<object>());
			GeneratorUtilities.AddHMacKeyGenerator("RIPEMD160", new object[]
			{
				IanaObjectIdentifiers.HmacRipeMD160
			});
			GeneratorUtilities.AddHMacKeyGenerator("TIGER", new object[]
			{
				IanaObjectIdentifiers.HmacTiger
			});
			GeneratorUtilities.AddHMacKeyGenerator("GOST3411-2012-256", new object[]
			{
				RosstandartObjectIdentifiers.id_tc26_hmac_gost_3411_12_256
			});
			GeneratorUtilities.AddHMacKeyGenerator("GOST3411-2012-512", new object[]
			{
				RosstandartObjectIdentifiers.id_tc26_hmac_gost_3411_12_512
			});
			GeneratorUtilities.AddKpgAlgorithm("DH", new object[]
			{
				"DIFFIEHELLMAN"
			});
			GeneratorUtilities.AddKpgAlgorithm("DSA", Array.Empty<object>());
			GeneratorUtilities.AddKpgAlgorithm("EC", new object[]
			{
				X9ObjectIdentifiers.DHSinglePassStdDHSha1KdfScheme
			});
			GeneratorUtilities.AddKpgAlgorithm("ECDH", new object[]
			{
				"ECIES"
			});
			GeneratorUtilities.AddKpgAlgorithm("ECDHC", Array.Empty<object>());
			GeneratorUtilities.AddKpgAlgorithm("ECMQV", new object[]
			{
				X9ObjectIdentifiers.MqvSinglePassSha1KdfScheme
			});
			GeneratorUtilities.AddKpgAlgorithm("ECDSA", Array.Empty<object>());
			GeneratorUtilities.AddKpgAlgorithm("ECGOST3410", new object[]
			{
				"ECGOST-3410",
				"GOST-3410-2001"
			});
			GeneratorUtilities.AddKpgAlgorithm("Ed25519", new object[]
			{
				"Ed25519ctx",
				"Ed25519ph",
				EdECObjectIdentifiers.id_Ed25519
			});
			GeneratorUtilities.AddKpgAlgorithm("Ed448", new object[]
			{
				"Ed448ph",
				EdECObjectIdentifiers.id_Ed448
			});
			GeneratorUtilities.AddKpgAlgorithm("ELGAMAL", Array.Empty<object>());
			GeneratorUtilities.AddKpgAlgorithm("GOST3410", new object[]
			{
				"GOST-3410",
				"GOST-3410-94"
			});
			GeneratorUtilities.AddKpgAlgorithm("RSA", new object[]
			{
				"1.2.840.113549.1.1.1"
			});
			GeneratorUtilities.AddKpgAlgorithm("X25519", new object[]
			{
				EdECObjectIdentifiers.id_X25519
			});
			GeneratorUtilities.AddKpgAlgorithm("X448", new object[]
			{
				EdECObjectIdentifiers.id_X448
			});
			GeneratorUtilities.AddDefaultKeySizeEntries(64, new string[]
			{
				"DES"
			});
			GeneratorUtilities.AddDefaultKeySizeEntries(80, new string[]
			{
				"SKIPJACK"
			});
			GeneratorUtilities.AddDefaultKeySizeEntries(128, new string[]
			{
				"AES128",
				"BLOWFISH",
				"CAMELLIA128",
				"CAST5",
				"DESEDE",
				"HC128",
				"HMACMD2",
				"HMACMD4",
				"HMACMD5",
				"HMACRIPEMD128",
				"IDEA",
				"NOEKEON",
				"RC2",
				"RC4",
				"RC5",
				"SALSA20",
				"SEED",
				"SM4",
				"TEA",
				"XTEA",
				"VMPC",
				"VMPC-KSA3"
			});
			GeneratorUtilities.AddDefaultKeySizeEntries(160, new string[]
			{
				"HMACRIPEMD160",
				"HMACSHA1"
			});
			GeneratorUtilities.AddDefaultKeySizeEntries(192, new string[]
			{
				"AES",
				"AES192",
				"CAMELLIA192",
				"DESEDE3",
				"HMACTIGER",
				"RIJNDAEL",
				"SERPENT",
				"TNEPRES"
			});
			GeneratorUtilities.AddDefaultKeySizeEntries(224, new string[]
			{
				"HMACSHA3-224",
				"HMACKECCAK224",
				"HMACSHA224",
				"HMACSHA512/224"
			});
			GeneratorUtilities.AddDefaultKeySizeEntries(256, new string[]
			{
				"AES256",
				"CAMELLIA",
				"CAMELLIA256",
				"CAST6",
				"GOST28147",
				"HC256",
				"HMACGOST3411-2012-256",
				"HMACSHA3-256",
				"HMACKECCAK256",
				"HMACSHA256",
				"HMACSHA512/256",
				"RC5-64",
				"RC6",
				"THREEFISH-256",
				"TWOFISH"
			});
			GeneratorUtilities.AddDefaultKeySizeEntries(288, new string[]
			{
				"HMACKECCAK288"
			});
			GeneratorUtilities.AddDefaultKeySizeEntries(384, new string[]
			{
				"HMACSHA3-384",
				"HMACKECCAK384",
				"HMACSHA384"
			});
			GeneratorUtilities.AddDefaultKeySizeEntries(512, new string[]
			{
				"HMACGOST3411-2012-512",
				"HMACSHA3-512",
				"HMACKECCAK512",
				"HMACSHA512",
				"THREEFISH-512"
			});
			GeneratorUtilities.AddDefaultKeySizeEntries(1024, new string[]
			{
				"THREEFISH-1024"
			});
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x000CABA8 File Offset: 0x000C8DA8
		private static void AddDefaultKeySizeEntries(int size, params string[] algorithms)
		{
			foreach (string key in algorithms)
			{
				GeneratorUtilities.defaultKeySizes.Add(key, size);
			}
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x000CABDC File Offset: 0x000C8DDC
		private static void AddKgAlgorithm(string canonicalName, params object[] aliases)
		{
			GeneratorUtilities.kgAlgorithms[Platform.ToUpperInvariant(canonicalName)] = canonicalName;
			foreach (object obj in aliases)
			{
				GeneratorUtilities.kgAlgorithms[Platform.ToUpperInvariant(obj.ToString())] = canonicalName;
			}
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x000CAC24 File Offset: 0x000C8E24
		private static void AddKpgAlgorithm(string canonicalName, params object[] aliases)
		{
			GeneratorUtilities.kpgAlgorithms[Platform.ToUpperInvariant(canonicalName)] = canonicalName;
			foreach (object obj in aliases)
			{
				GeneratorUtilities.kpgAlgorithms[Platform.ToUpperInvariant(obj.ToString())] = canonicalName;
			}
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x000CAC6C File Offset: 0x000C8E6C
		private static void AddHMacKeyGenerator(string algorithm, params object[] aliases)
		{
			string text = "HMAC" + algorithm;
			GeneratorUtilities.kgAlgorithms[text] = text;
			GeneratorUtilities.kgAlgorithms["HMAC-" + algorithm] = text;
			GeneratorUtilities.kgAlgorithms["HMAC/" + algorithm] = text;
			foreach (object obj in aliases)
			{
				GeneratorUtilities.kgAlgorithms[Platform.ToUpperInvariant(obj.ToString())] = text;
			}
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x000CACE7 File Offset: 0x000C8EE7
		internal static string GetCanonicalKeyGeneratorAlgorithm(string algorithm)
		{
			return (string)GeneratorUtilities.kgAlgorithms[Platform.ToUpperInvariant(algorithm)];
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x000CACFE File Offset: 0x000C8EFE
		internal static string GetCanonicalKeyPairGeneratorAlgorithm(string algorithm)
		{
			return (string)GeneratorUtilities.kpgAlgorithms[Platform.ToUpperInvariant(algorithm)];
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x000CAD15 File Offset: 0x000C8F15
		public static CipherKeyGenerator GetKeyGenerator(DerObjectIdentifier oid)
		{
			return GeneratorUtilities.GetKeyGenerator(oid.Id);
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x000CAD24 File Offset: 0x000C8F24
		public static CipherKeyGenerator GetKeyGenerator(string algorithm)
		{
			string canonicalKeyGeneratorAlgorithm = GeneratorUtilities.GetCanonicalKeyGeneratorAlgorithm(algorithm);
			if (canonicalKeyGeneratorAlgorithm == null)
			{
				throw new SecurityUtilityException("KeyGenerator " + algorithm + " not recognised.");
			}
			int num = GeneratorUtilities.FindDefaultKeySize(canonicalKeyGeneratorAlgorithm);
			if (num == -1)
			{
				throw new SecurityUtilityException(string.Concat(new string[]
				{
					"KeyGenerator ",
					algorithm,
					" (",
					canonicalKeyGeneratorAlgorithm,
					") not supported."
				}));
			}
			if (canonicalKeyGeneratorAlgorithm == "DES")
			{
				return new DesKeyGenerator(num);
			}
			if (canonicalKeyGeneratorAlgorithm == "DESEDE" || canonicalKeyGeneratorAlgorithm == "DESEDE3")
			{
				return new DesEdeKeyGenerator(num);
			}
			return new CipherKeyGenerator(num);
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x000CADC8 File Offset: 0x000C8FC8
		public static IAsymmetricCipherKeyPairGenerator GetKeyPairGenerator(DerObjectIdentifier oid)
		{
			return GeneratorUtilities.GetKeyPairGenerator(oid.Id);
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x000CADD8 File Offset: 0x000C8FD8
		public static IAsymmetricCipherKeyPairGenerator GetKeyPairGenerator(string algorithm)
		{
			string canonicalKeyPairGeneratorAlgorithm = GeneratorUtilities.GetCanonicalKeyPairGeneratorAlgorithm(algorithm);
			if (canonicalKeyPairGeneratorAlgorithm == null)
			{
				throw new SecurityUtilityException("KeyPairGenerator " + algorithm + " not recognised.");
			}
			if (canonicalKeyPairGeneratorAlgorithm == "DH")
			{
				return new DHKeyPairGenerator();
			}
			if (canonicalKeyPairGeneratorAlgorithm == "DSA")
			{
				return new DsaKeyPairGenerator();
			}
			if (Platform.StartsWith(canonicalKeyPairGeneratorAlgorithm, "EC"))
			{
				return new ECKeyPairGenerator(canonicalKeyPairGeneratorAlgorithm);
			}
			if (canonicalKeyPairGeneratorAlgorithm == "Ed25519")
			{
				return new Ed25519KeyPairGenerator();
			}
			if (canonicalKeyPairGeneratorAlgorithm == "Ed448")
			{
				return new Ed448KeyPairGenerator();
			}
			if (canonicalKeyPairGeneratorAlgorithm == "ELGAMAL")
			{
				return new ElGamalKeyPairGenerator();
			}
			if (canonicalKeyPairGeneratorAlgorithm == "GOST3410")
			{
				return new Gost3410KeyPairGenerator();
			}
			if (canonicalKeyPairGeneratorAlgorithm == "RSA")
			{
				return new RsaKeyPairGenerator();
			}
			if (canonicalKeyPairGeneratorAlgorithm == "X25519")
			{
				return new X25519KeyPairGenerator();
			}
			if (canonicalKeyPairGeneratorAlgorithm == "X448")
			{
				return new X448KeyPairGenerator();
			}
			throw new SecurityUtilityException(string.Concat(new string[]
			{
				"KeyPairGenerator ",
				algorithm,
				" (",
				canonicalKeyPairGeneratorAlgorithm,
				") not supported."
			}));
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x000CAEF4 File Offset: 0x000C90F4
		internal static int GetDefaultKeySize(DerObjectIdentifier oid)
		{
			return GeneratorUtilities.GetDefaultKeySize(oid.Id);
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x000CAF04 File Offset: 0x000C9104
		internal static int GetDefaultKeySize(string algorithm)
		{
			string canonicalKeyGeneratorAlgorithm = GeneratorUtilities.GetCanonicalKeyGeneratorAlgorithm(algorithm);
			if (canonicalKeyGeneratorAlgorithm == null)
			{
				throw new SecurityUtilityException("KeyGenerator " + algorithm + " not recognised.");
			}
			int num = GeneratorUtilities.FindDefaultKeySize(canonicalKeyGeneratorAlgorithm);
			if (num == -1)
			{
				throw new SecurityUtilityException(string.Concat(new string[]
				{
					"KeyGenerator ",
					algorithm,
					" (",
					canonicalKeyGeneratorAlgorithm,
					") not supported."
				}));
			}
			return num;
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x000CAF6E File Offset: 0x000C916E
		private static int FindDefaultKeySize(string canonicalName)
		{
			if (!GeneratorUtilities.defaultKeySizes.Contains(canonicalName))
			{
				return -1;
			}
			return (int)GeneratorUtilities.defaultKeySizes[canonicalName];
		}

		// Token: 0x040018E1 RID: 6369
		private static readonly IDictionary kgAlgorithms = Platform.CreateHashtable();

		// Token: 0x040018E2 RID: 6370
		private static readonly IDictionary kpgAlgorithms = Platform.CreateHashtable();

		// Token: 0x040018E3 RID: 6371
		private static readonly IDictionary defaultKeySizes = Platform.CreateHashtable();
	}
}
