using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Kisa;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ntt;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Encodings;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Paddings;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002DA RID: 730
	public sealed class CipherUtilities
	{
		// Token: 0x06001AAD RID: 6829 RVA: 0x000C7F40 File Offset: 0x000C6140
		static CipherUtilities()
		{
			((CipherUtilities.CipherAlgorithm)Enums.GetArbitraryValue(typeof(CipherUtilities.CipherAlgorithm))).ToString();
			((CipherUtilities.CipherMode)Enums.GetArbitraryValue(typeof(CipherUtilities.CipherMode))).ToString();
			((CipherUtilities.CipherPadding)Enums.GetArbitraryValue(typeof(CipherUtilities.CipherPadding))).ToString();
			CipherUtilities.algorithms[NistObjectIdentifiers.IdAes128Ecb.Id] = "AES/ECB/PKCS7PADDING";
			CipherUtilities.algorithms[NistObjectIdentifiers.IdAes192Ecb.Id] = "AES/ECB/PKCS7PADDING";
			CipherUtilities.algorithms[NistObjectIdentifiers.IdAes256Ecb.Id] = "AES/ECB/PKCS7PADDING";
			CipherUtilities.algorithms["AES//PKCS7"] = "AES/ECB/PKCS7PADDING";
			CipherUtilities.algorithms["AES//PKCS7PADDING"] = "AES/ECB/PKCS7PADDING";
			CipherUtilities.algorithms["AES//PKCS5"] = "AES/ECB/PKCS7PADDING";
			CipherUtilities.algorithms["AES//PKCS5PADDING"] = "AES/ECB/PKCS7PADDING";
			CipherUtilities.algorithms[NistObjectIdentifiers.IdAes128Cbc.Id] = "AES/CBC/PKCS7PADDING";
			CipherUtilities.algorithms[NistObjectIdentifiers.IdAes192Cbc.Id] = "AES/CBC/PKCS7PADDING";
			CipherUtilities.algorithms[NistObjectIdentifiers.IdAes256Cbc.Id] = "AES/CBC/PKCS7PADDING";
			CipherUtilities.algorithms[NistObjectIdentifiers.IdAes128Ofb.Id] = "AES/OFB/NOPADDING";
			CipherUtilities.algorithms[NistObjectIdentifiers.IdAes192Ofb.Id] = "AES/OFB/NOPADDING";
			CipherUtilities.algorithms[NistObjectIdentifiers.IdAes256Ofb.Id] = "AES/OFB/NOPADDING";
			CipherUtilities.algorithms[NistObjectIdentifiers.IdAes128Cfb.Id] = "AES/CFB/NOPADDING";
			CipherUtilities.algorithms[NistObjectIdentifiers.IdAes192Cfb.Id] = "AES/CFB/NOPADDING";
			CipherUtilities.algorithms[NistObjectIdentifiers.IdAes256Cfb.Id] = "AES/CFB/NOPADDING";
			CipherUtilities.algorithms["RSA/ECB/PKCS1"] = "RSA//PKCS1PADDING";
			CipherUtilities.algorithms["RSA/ECB/PKCS1PADDING"] = "RSA//PKCS1PADDING";
			CipherUtilities.algorithms[PkcsObjectIdentifiers.RsaEncryption.Id] = "RSA//PKCS1PADDING";
			CipherUtilities.algorithms[PkcsObjectIdentifiers.IdRsaesOaep.Id] = "RSA//OAEPPADDING";
			CipherUtilities.algorithms[OiwObjectIdentifiers.DesCbc.Id] = "DES/CBC";
			CipherUtilities.algorithms[OiwObjectIdentifiers.DesCfb.Id] = "DES/CFB";
			CipherUtilities.algorithms[OiwObjectIdentifiers.DesEcb.Id] = "DES/ECB";
			CipherUtilities.algorithms[OiwObjectIdentifiers.DesOfb.Id] = "DES/OFB";
			CipherUtilities.algorithms[OiwObjectIdentifiers.DesEde.Id] = "DESEDE";
			CipherUtilities.algorithms["TDEA"] = "DESEDE";
			CipherUtilities.algorithms[PkcsObjectIdentifiers.DesEde3Cbc.Id] = "DESEDE/CBC";
			CipherUtilities.algorithms[PkcsObjectIdentifiers.RC2Cbc.Id] = "RC2/CBC";
			CipherUtilities.algorithms["1.3.6.1.4.1.188.7.1.1.2"] = "IDEA/CBC";
			CipherUtilities.algorithms["1.2.840.113533.7.66.10"] = "CAST5/CBC";
			CipherUtilities.algorithms["RC4"] = "ARC4";
			CipherUtilities.algorithms["ARCFOUR"] = "ARC4";
			CipherUtilities.algorithms["1.2.840.113549.3.4"] = "ARC4";
			CipherUtilities.algorithms["PBEWITHSHA1AND128BITRC4"] = "PBEWITHSHAAND128BITRC4";
			CipherUtilities.algorithms[PkcsObjectIdentifiers.PbeWithShaAnd128BitRC4.Id] = "PBEWITHSHAAND128BITRC4";
			CipherUtilities.algorithms["PBEWITHSHA1AND40BITRC4"] = "PBEWITHSHAAND40BITRC4";
			CipherUtilities.algorithms[PkcsObjectIdentifiers.PbeWithShaAnd40BitRC4.Id] = "PBEWITHSHAAND40BITRC4";
			CipherUtilities.algorithms["PBEWITHSHA1ANDDES"] = "PBEWITHSHA1ANDDES-CBC";
			CipherUtilities.algorithms[PkcsObjectIdentifiers.PbeWithSha1AndDesCbc.Id] = "PBEWITHSHA1ANDDES-CBC";
			CipherUtilities.algorithms["PBEWITHSHA1ANDRC2"] = "PBEWITHSHA1ANDRC2-CBC";
			CipherUtilities.algorithms[PkcsObjectIdentifiers.PbeWithSha1AndRC2Cbc.Id] = "PBEWITHSHA1ANDRC2-CBC";
			CipherUtilities.algorithms["PBEWITHSHA1AND3-KEYTRIPLEDES-CBC"] = "PBEWITHSHAAND3-KEYTRIPLEDES-CBC";
			CipherUtilities.algorithms["PBEWITHSHAAND3KEYTRIPLEDES"] = "PBEWITHSHAAND3-KEYTRIPLEDES-CBC";
			CipherUtilities.algorithms[PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc.Id] = "PBEWITHSHAAND3-KEYTRIPLEDES-CBC";
			CipherUtilities.algorithms["PBEWITHSHA1ANDDESEDE"] = "PBEWITHSHAAND3-KEYTRIPLEDES-CBC";
			CipherUtilities.algorithms["PBEWITHSHA1AND2-KEYTRIPLEDES-CBC"] = "PBEWITHSHAAND2-KEYTRIPLEDES-CBC";
			CipherUtilities.algorithms[PkcsObjectIdentifiers.PbeWithShaAnd2KeyTripleDesCbc.Id] = "PBEWITHSHAAND2-KEYTRIPLEDES-CBC";
			CipherUtilities.algorithms["PBEWITHSHA1AND128BITRC2-CBC"] = "PBEWITHSHAAND128BITRC2-CBC";
			CipherUtilities.algorithms[PkcsObjectIdentifiers.PbeWithShaAnd128BitRC2Cbc.Id] = "PBEWITHSHAAND128BITRC2-CBC";
			CipherUtilities.algorithms["PBEWITHSHA1AND40BITRC2-CBC"] = "PBEWITHSHAAND40BITRC2-CBC";
			CipherUtilities.algorithms[PkcsObjectIdentifiers.PbewithShaAnd40BitRC2Cbc.Id] = "PBEWITHSHAAND40BITRC2-CBC";
			CipherUtilities.algorithms["PBEWITHSHA1AND128BITAES-CBC-BC"] = "PBEWITHSHAAND128BITAES-CBC-BC";
			CipherUtilities.algorithms["PBEWITHSHA-1AND128BITAES-CBC-BC"] = "PBEWITHSHAAND128BITAES-CBC-BC";
			CipherUtilities.algorithms["PBEWITHSHA1AND192BITAES-CBC-BC"] = "PBEWITHSHAAND192BITAES-CBC-BC";
			CipherUtilities.algorithms["PBEWITHSHA-1AND192BITAES-CBC-BC"] = "PBEWITHSHAAND192BITAES-CBC-BC";
			CipherUtilities.algorithms["PBEWITHSHA1AND256BITAES-CBC-BC"] = "PBEWITHSHAAND256BITAES-CBC-BC";
			CipherUtilities.algorithms["PBEWITHSHA-1AND256BITAES-CBC-BC"] = "PBEWITHSHAAND256BITAES-CBC-BC";
			CipherUtilities.algorithms["PBEWITHSHA-256AND128BITAES-CBC-BC"] = "PBEWITHSHA256AND128BITAES-CBC-BC";
			CipherUtilities.algorithms["PBEWITHSHA-256AND192BITAES-CBC-BC"] = "PBEWITHSHA256AND192BITAES-CBC-BC";
			CipherUtilities.algorithms["PBEWITHSHA-256AND256BITAES-CBC-BC"] = "PBEWITHSHA256AND256BITAES-CBC-BC";
			CipherUtilities.algorithms["GOST"] = "GOST28147";
			CipherUtilities.algorithms["GOST-28147"] = "GOST28147";
			CipherUtilities.algorithms[CryptoProObjectIdentifiers.GostR28147Cbc.Id] = "GOST28147/CBC/PKCS7PADDING";
			CipherUtilities.algorithms["RC5-32"] = "RC5";
			CipherUtilities.algorithms[NttObjectIdentifiers.IdCamellia128Cbc.Id] = "CAMELLIA/CBC/PKCS7PADDING";
			CipherUtilities.algorithms[NttObjectIdentifiers.IdCamellia192Cbc.Id] = "CAMELLIA/CBC/PKCS7PADDING";
			CipherUtilities.algorithms[NttObjectIdentifiers.IdCamellia256Cbc.Id] = "CAMELLIA/CBC/PKCS7PADDING";
			CipherUtilities.algorithms[KisaObjectIdentifiers.IdSeedCbc.Id] = "SEED/CBC/PKCS7PADDING";
			CipherUtilities.algorithms["1.3.6.1.4.1.3029.1.2"] = "BLOWFISH/CBC";
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x00022F1F File Offset: 0x0002111F
		private CipherUtilities()
		{
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x000C85D8 File Offset: 0x000C67D8
		public static DerObjectIdentifier GetObjectIdentifier(string mechanism)
		{
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			mechanism = Platform.ToUpperInvariant(mechanism);
			string text = (string)CipherUtilities.algorithms[mechanism];
			if (text != null)
			{
				mechanism = text;
			}
			return (DerObjectIdentifier)CipherUtilities.oids[mechanism];
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06001AB0 RID: 6832 RVA: 0x000C8622 File Offset: 0x000C6822
		public static ICollection Algorithms
		{
			get
			{
				return CipherUtilities.oids.Keys;
			}
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x000C862E File Offset: 0x000C682E
		public static IBufferedCipher GetCipher(DerObjectIdentifier oid)
		{
			return CipherUtilities.GetCipher(oid.Id);
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x000C863C File Offset: 0x000C683C
		public static IBufferedCipher GetCipher(string algorithm)
		{
			if (algorithm == null)
			{
				throw new ArgumentNullException("algorithm");
			}
			algorithm = Platform.ToUpperInvariant(algorithm);
			string text = (string)CipherUtilities.algorithms[algorithm];
			if (text != null)
			{
				algorithm = text;
			}
			IBasicAgreement basicAgreement = null;
			if (algorithm == "IES")
			{
				basicAgreement = new DHBasicAgreement();
			}
			else if (algorithm == "ECIES")
			{
				basicAgreement = new ECDHBasicAgreement();
			}
			if (basicAgreement != null)
			{
				return new BufferedIesCipher(new IesEngine(basicAgreement, new Kdf2BytesGenerator(new Sha1Digest()), new HMac(new Sha1Digest())));
			}
			if (Platform.StartsWith(algorithm, "PBE"))
			{
				if (Platform.EndsWith(algorithm, "-CBC"))
				{
					if (algorithm == "PBEWITHSHA1ANDDES-CBC")
					{
						return new PaddedBufferedBlockCipher(new CbcBlockCipher(new DesEngine()));
					}
					if (algorithm == "PBEWITHSHA1ANDRC2-CBC")
					{
						return new PaddedBufferedBlockCipher(new CbcBlockCipher(new RC2Engine()));
					}
					if (Strings.IsOneOf(algorithm, new string[]
					{
						"PBEWITHSHAAND2-KEYTRIPLEDES-CBC",
						"PBEWITHSHAAND3-KEYTRIPLEDES-CBC"
					}))
					{
						return new PaddedBufferedBlockCipher(new CbcBlockCipher(new DesEdeEngine()));
					}
					if (Strings.IsOneOf(algorithm, new string[]
					{
						"PBEWITHSHAAND128BITRC2-CBC",
						"PBEWITHSHAAND40BITRC2-CBC"
					}))
					{
						return new PaddedBufferedBlockCipher(new CbcBlockCipher(new RC2Engine()));
					}
				}
				else if ((Platform.EndsWith(algorithm, "-BC") || Platform.EndsWith(algorithm, "-OPENSSL")) && Strings.IsOneOf(algorithm, new string[]
				{
					"PBEWITHSHAAND128BITAES-CBC-BC",
					"PBEWITHSHAAND192BITAES-CBC-BC",
					"PBEWITHSHAAND256BITAES-CBC-BC",
					"PBEWITHSHA256AND128BITAES-CBC-BC",
					"PBEWITHSHA256AND192BITAES-CBC-BC",
					"PBEWITHSHA256AND256BITAES-CBC-BC",
					"PBEWITHMD5AND128BITAES-CBC-OPENSSL",
					"PBEWITHMD5AND192BITAES-CBC-OPENSSL",
					"PBEWITHMD5AND256BITAES-CBC-OPENSSL"
				}))
				{
					return new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()));
				}
			}
			string[] array = algorithm.Split(new char[]
			{
				'/'
			});
			IBlockCipher blockCipher = null;
			IAsymmetricBlockCipher asymmetricBlockCipher = null;
			IStreamCipher streamCipher = null;
			string text2 = array[0];
			string text3 = (string)CipherUtilities.algorithms[text2];
			if (text3 != null)
			{
				text2 = text3;
			}
			CipherUtilities.CipherAlgorithm cipherAlgorithm;
			try
			{
				cipherAlgorithm = (CipherUtilities.CipherAlgorithm)Enums.GetEnumValue(typeof(CipherUtilities.CipherAlgorithm), text2);
			}
			catch (ArgumentException)
			{
				throw new SecurityUtilityException("Cipher " + algorithm + " not recognised.");
			}
			switch (cipherAlgorithm)
			{
			case CipherUtilities.CipherAlgorithm.AES:
				blockCipher = new AesEngine();
				break;
			case CipherUtilities.CipherAlgorithm.ARC4:
				streamCipher = new RC4Engine();
				break;
			case CipherUtilities.CipherAlgorithm.BLOWFISH:
				blockCipher = new BlowfishEngine();
				break;
			case CipherUtilities.CipherAlgorithm.CAMELLIA:
				blockCipher = new CamelliaEngine();
				break;
			case CipherUtilities.CipherAlgorithm.CAST5:
				blockCipher = new Cast5Engine();
				break;
			case CipherUtilities.CipherAlgorithm.CAST6:
				blockCipher = new Cast6Engine();
				break;
			case CipherUtilities.CipherAlgorithm.DES:
				blockCipher = new DesEngine();
				break;
			case CipherUtilities.CipherAlgorithm.DESEDE:
				blockCipher = new DesEdeEngine();
				break;
			case CipherUtilities.CipherAlgorithm.ELGAMAL:
				asymmetricBlockCipher = new ElGamalEngine();
				break;
			case CipherUtilities.CipherAlgorithm.GOST28147:
				blockCipher = new Gost28147Engine();
				break;
			case CipherUtilities.CipherAlgorithm.HC128:
				streamCipher = new HC128Engine();
				break;
			case CipherUtilities.CipherAlgorithm.HC256:
				streamCipher = new HC256Engine();
				break;
			case CipherUtilities.CipherAlgorithm.IDEA:
				blockCipher = new IdeaEngine();
				break;
			case CipherUtilities.CipherAlgorithm.NOEKEON:
				blockCipher = new NoekeonEngine();
				break;
			case CipherUtilities.CipherAlgorithm.PBEWITHSHAAND128BITRC4:
			case CipherUtilities.CipherAlgorithm.PBEWITHSHAAND40BITRC4:
				streamCipher = new RC4Engine();
				break;
			case CipherUtilities.CipherAlgorithm.RC2:
				blockCipher = new RC2Engine();
				break;
			case CipherUtilities.CipherAlgorithm.RC5:
				blockCipher = new RC532Engine();
				break;
			case CipherUtilities.CipherAlgorithm.RC5_64:
				blockCipher = new RC564Engine();
				break;
			case CipherUtilities.CipherAlgorithm.RC6:
				blockCipher = new RC6Engine();
				break;
			case CipherUtilities.CipherAlgorithm.RIJNDAEL:
				blockCipher = new RijndaelEngine();
				break;
			case CipherUtilities.CipherAlgorithm.RSA:
				asymmetricBlockCipher = new RsaBlindedEngine();
				break;
			case CipherUtilities.CipherAlgorithm.SALSA20:
				streamCipher = new Salsa20Engine();
				break;
			case CipherUtilities.CipherAlgorithm.SEED:
				blockCipher = new SeedEngine();
				break;
			case CipherUtilities.CipherAlgorithm.SERPENT:
				blockCipher = new SerpentEngine();
				break;
			case CipherUtilities.CipherAlgorithm.SKIPJACK:
				blockCipher = new SkipjackEngine();
				break;
			case CipherUtilities.CipherAlgorithm.SM4:
				blockCipher = new SM4Engine();
				break;
			case CipherUtilities.CipherAlgorithm.TEA:
				blockCipher = new TeaEngine();
				break;
			case CipherUtilities.CipherAlgorithm.THREEFISH_256:
				blockCipher = new ThreefishEngine(256);
				break;
			case CipherUtilities.CipherAlgorithm.THREEFISH_512:
				blockCipher = new ThreefishEngine(512);
				break;
			case CipherUtilities.CipherAlgorithm.THREEFISH_1024:
				blockCipher = new ThreefishEngine(1024);
				break;
			case CipherUtilities.CipherAlgorithm.TNEPRES:
				blockCipher = new TnepresEngine();
				break;
			case CipherUtilities.CipherAlgorithm.TWOFISH:
				blockCipher = new TwofishEngine();
				break;
			case CipherUtilities.CipherAlgorithm.VMPC:
				streamCipher = new VmpcEngine();
				break;
			case CipherUtilities.CipherAlgorithm.VMPC_KSA3:
				streamCipher = new VmpcKsa3Engine();
				break;
			case CipherUtilities.CipherAlgorithm.XTEA:
				blockCipher = new XteaEngine();
				break;
			default:
				throw new SecurityUtilityException("Cipher " + algorithm + " not recognised.");
			}
			if (streamCipher != null)
			{
				if (array.Length > 1)
				{
					throw new ArgumentException("Modes and paddings not used for stream ciphers");
				}
				return new BufferedStreamCipher(streamCipher);
			}
			else
			{
				bool flag = false;
				bool flag2 = true;
				IBlockCipherPadding blockCipherPadding = null;
				IAeadBlockCipher aeadBlockCipher = null;
				if (array.Length > 2)
				{
					if (streamCipher != null)
					{
						throw new ArgumentException("Paddings not used for stream ciphers");
					}
					string text4 = array[2];
					CipherUtilities.CipherPadding cipherPadding;
					if (text4 == "")
					{
						cipherPadding = CipherUtilities.CipherPadding.RAW;
					}
					else if (text4 == "X9.23PADDING")
					{
						cipherPadding = CipherUtilities.CipherPadding.X923PADDING;
					}
					else
					{
						try
						{
							cipherPadding = (CipherUtilities.CipherPadding)Enums.GetEnumValue(typeof(CipherUtilities.CipherPadding), text4);
						}
						catch (ArgumentException)
						{
							throw new SecurityUtilityException("Cipher " + algorithm + " not recognised.");
						}
					}
					switch (cipherPadding)
					{
					case CipherUtilities.CipherPadding.NOPADDING:
						flag2 = false;
						break;
					case CipherUtilities.CipherPadding.RAW:
						break;
					case CipherUtilities.CipherPadding.ISO10126PADDING:
					case CipherUtilities.CipherPadding.ISO10126D2PADDING:
					case CipherUtilities.CipherPadding.ISO10126_2PADDING:
						blockCipherPadding = new ISO10126d2Padding();
						break;
					case CipherUtilities.CipherPadding.ISO7816_4PADDING:
					case CipherUtilities.CipherPadding.ISO9797_1PADDING:
						blockCipherPadding = new ISO7816d4Padding();
						break;
					case CipherUtilities.CipherPadding.ISO9796_1:
					case CipherUtilities.CipherPadding.ISO9796_1PADDING:
						asymmetricBlockCipher = new ISO9796d1Encoding(asymmetricBlockCipher);
						break;
					case CipherUtilities.CipherPadding.OAEP:
					case CipherUtilities.CipherPadding.OAEPPADDING:
						asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher);
						break;
					case CipherUtilities.CipherPadding.OAEPWITHMD5ANDMGF1PADDING:
						asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher, new MD5Digest());
						break;
					case CipherUtilities.CipherPadding.OAEPWITHSHA1ANDMGF1PADDING:
					case CipherUtilities.CipherPadding.OAEPWITHSHA_1ANDMGF1PADDING:
						asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher, new Sha1Digest());
						break;
					case CipherUtilities.CipherPadding.OAEPWITHSHA224ANDMGF1PADDING:
					case CipherUtilities.CipherPadding.OAEPWITHSHA_224ANDMGF1PADDING:
						asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher, new Sha224Digest());
						break;
					case CipherUtilities.CipherPadding.OAEPWITHSHA256ANDMGF1PADDING:
					case CipherUtilities.CipherPadding.OAEPWITHSHA_256ANDMGF1PADDING:
						asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher, new Sha256Digest());
						break;
					case CipherUtilities.CipherPadding.OAEPWITHSHA384ANDMGF1PADDING:
					case CipherUtilities.CipherPadding.OAEPWITHSHA_384ANDMGF1PADDING:
						asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher, new Sha384Digest());
						break;
					case CipherUtilities.CipherPadding.OAEPWITHSHA512ANDMGF1PADDING:
					case CipherUtilities.CipherPadding.OAEPWITHSHA_512ANDMGF1PADDING:
						asymmetricBlockCipher = new OaepEncoding(asymmetricBlockCipher, new Sha512Digest());
						break;
					case CipherUtilities.CipherPadding.PKCS1:
					case CipherUtilities.CipherPadding.PKCS1PADDING:
						asymmetricBlockCipher = new Pkcs1Encoding(asymmetricBlockCipher);
						break;
					case CipherUtilities.CipherPadding.PKCS5:
					case CipherUtilities.CipherPadding.PKCS5PADDING:
					case CipherUtilities.CipherPadding.PKCS7:
					case CipherUtilities.CipherPadding.PKCS7PADDING:
						blockCipherPadding = new Pkcs7Padding();
						break;
					case CipherUtilities.CipherPadding.TBCPADDING:
						blockCipherPadding = new TbcPadding();
						break;
					case CipherUtilities.CipherPadding.WITHCTS:
						flag = true;
						break;
					case CipherUtilities.CipherPadding.X923PADDING:
						blockCipherPadding = new X923Padding();
						break;
					case CipherUtilities.CipherPadding.ZEROBYTEPADDING:
						blockCipherPadding = new ZeroBytePadding();
						break;
					default:
						throw new SecurityUtilityException("Cipher " + algorithm + " not recognised.");
					}
				}
				if (array.Length > 1)
				{
					string text5 = array[1];
					int digitIndex = CipherUtilities.GetDigitIndex(text5);
					string text6 = (digitIndex >= 0) ? text5.Substring(0, digitIndex) : text5;
					try
					{
						switch ((text6 == "") ? CipherUtilities.CipherMode.NONE : ((CipherUtilities.CipherMode)Enums.GetEnumValue(typeof(CipherUtilities.CipherMode), text6)))
						{
						case CipherUtilities.CipherMode.ECB:
						case CipherUtilities.CipherMode.NONE:
							break;
						case CipherUtilities.CipherMode.CBC:
							blockCipher = new CbcBlockCipher(blockCipher);
							break;
						case CipherUtilities.CipherMode.CCM:
							aeadBlockCipher = new CcmBlockCipher(blockCipher);
							break;
						case CipherUtilities.CipherMode.CFB:
						{
							int bitBlockSize = (digitIndex < 0) ? (8 * blockCipher.GetBlockSize()) : int.Parse(text5.Substring(digitIndex));
							blockCipher = new CfbBlockCipher(blockCipher, bitBlockSize);
							break;
						}
						case CipherUtilities.CipherMode.CTR:
							blockCipher = new SicBlockCipher(blockCipher);
							break;
						case CipherUtilities.CipherMode.CTS:
							flag = true;
							blockCipher = new CbcBlockCipher(blockCipher);
							break;
						case CipherUtilities.CipherMode.EAX:
							aeadBlockCipher = new EaxBlockCipher(blockCipher);
							break;
						case CipherUtilities.CipherMode.GCM:
							aeadBlockCipher = new GcmBlockCipher(blockCipher);
							break;
						case CipherUtilities.CipherMode.GOFB:
							blockCipher = new GOfbBlockCipher(blockCipher);
							break;
						case CipherUtilities.CipherMode.OCB:
							aeadBlockCipher = new OcbBlockCipher(blockCipher, CipherUtilities.CreateBlockCipher(cipherAlgorithm));
							break;
						case CipherUtilities.CipherMode.OFB:
						{
							int blockSize = (digitIndex < 0) ? (8 * blockCipher.GetBlockSize()) : int.Parse(text5.Substring(digitIndex));
							blockCipher = new OfbBlockCipher(blockCipher, blockSize);
							break;
						}
						case CipherUtilities.CipherMode.OPENPGPCFB:
							blockCipher = new OpenPgpCfbBlockCipher(blockCipher);
							break;
						case CipherUtilities.CipherMode.SIC:
							if (blockCipher.GetBlockSize() < 16)
							{
								throw new ArgumentException("Warning: SIC-Mode can become a twotime-pad if the blocksize of the cipher is too small. Use a cipher with a block size of at least 128 bits (e.g. AES)");
							}
							blockCipher = new SicBlockCipher(blockCipher);
							break;
						default:
							throw new SecurityUtilityException("Cipher " + algorithm + " not recognised.");
						}
					}
					catch (ArgumentException)
					{
						throw new SecurityUtilityException("Cipher " + algorithm + " not recognised.");
					}
				}
				if (aeadBlockCipher != null)
				{
					if (flag)
					{
						throw new SecurityUtilityException("CTS mode not valid for AEAD ciphers.");
					}
					if (flag2 && array.Length > 2 && array[2] != "")
					{
						throw new SecurityUtilityException("Bad padding specified for AEAD cipher.");
					}
					return new BufferedAeadBlockCipher(aeadBlockCipher);
				}
				else if (blockCipher != null)
				{
					if (flag)
					{
						return new CtsBlockCipher(blockCipher);
					}
					if (blockCipherPadding != null)
					{
						return new PaddedBufferedBlockCipher(blockCipher, blockCipherPadding);
					}
					if (!flag2 || blockCipher.IsPartialBlockOkay)
					{
						return new BufferedBlockCipher(blockCipher);
					}
					return new PaddedBufferedBlockCipher(blockCipher);
				}
				else
				{
					if (asymmetricBlockCipher != null)
					{
						return new BufferedAsymmetricBlockCipher(asymmetricBlockCipher);
					}
					throw new SecurityUtilityException("Cipher " + algorithm + " not recognised.");
				}
			}
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x000C8F2C File Offset: 0x000C712C
		public static string GetAlgorithmName(DerObjectIdentifier oid)
		{
			return (string)CipherUtilities.algorithms[oid.Id];
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x000C8F44 File Offset: 0x000C7144
		private static int GetDigitIndex(string s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (char.IsDigit(s[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x000C8F74 File Offset: 0x000C7174
		private static IBlockCipher CreateBlockCipher(CipherUtilities.CipherAlgorithm cipherAlgorithm)
		{
			switch (cipherAlgorithm)
			{
			case CipherUtilities.CipherAlgorithm.AES:
				return new AesEngine();
			case CipherUtilities.CipherAlgorithm.BLOWFISH:
				return new BlowfishEngine();
			case CipherUtilities.CipherAlgorithm.CAMELLIA:
				return new CamelliaEngine();
			case CipherUtilities.CipherAlgorithm.CAST5:
				return new Cast5Engine();
			case CipherUtilities.CipherAlgorithm.CAST6:
				return new Cast6Engine();
			case CipherUtilities.CipherAlgorithm.DES:
				return new DesEngine();
			case CipherUtilities.CipherAlgorithm.DESEDE:
				return new DesEdeEngine();
			case CipherUtilities.CipherAlgorithm.GOST28147:
				return new Gost28147Engine();
			case CipherUtilities.CipherAlgorithm.IDEA:
				return new IdeaEngine();
			case CipherUtilities.CipherAlgorithm.NOEKEON:
				return new NoekeonEngine();
			case CipherUtilities.CipherAlgorithm.RC2:
				return new RC2Engine();
			case CipherUtilities.CipherAlgorithm.RC5:
				return new RC532Engine();
			case CipherUtilities.CipherAlgorithm.RC5_64:
				return new RC564Engine();
			case CipherUtilities.CipherAlgorithm.RC6:
				return new RC6Engine();
			case CipherUtilities.CipherAlgorithm.RIJNDAEL:
				return new RijndaelEngine();
			case CipherUtilities.CipherAlgorithm.SEED:
				return new SeedEngine();
			case CipherUtilities.CipherAlgorithm.SERPENT:
				return new SerpentEngine();
			case CipherUtilities.CipherAlgorithm.SKIPJACK:
				return new SkipjackEngine();
			case CipherUtilities.CipherAlgorithm.SM4:
				return new SM4Engine();
			case CipherUtilities.CipherAlgorithm.TEA:
				return new TeaEngine();
			case CipherUtilities.CipherAlgorithm.THREEFISH_256:
				return new ThreefishEngine(256);
			case CipherUtilities.CipherAlgorithm.THREEFISH_512:
				return new ThreefishEngine(512);
			case CipherUtilities.CipherAlgorithm.THREEFISH_1024:
				return new ThreefishEngine(1024);
			case CipherUtilities.CipherAlgorithm.TNEPRES:
				return new TnepresEngine();
			case CipherUtilities.CipherAlgorithm.TWOFISH:
				return new TwofishEngine();
			case CipherUtilities.CipherAlgorithm.XTEA:
				return new XteaEngine();
			}
			throw new SecurityUtilityException("Cipher " + cipherAlgorithm + " not recognised or not a block cipher");
		}

		// Token: 0x040018DD RID: 6365
		private static readonly IDictionary algorithms = Platform.CreateHashtable();

		// Token: 0x040018DE RID: 6366
		private static readonly IDictionary oids = Platform.CreateHashtable();

		// Token: 0x02000903 RID: 2307
		private enum CipherAlgorithm
		{
			// Token: 0x040034C5 RID: 13509
			AES,
			// Token: 0x040034C6 RID: 13510
			ARC4,
			// Token: 0x040034C7 RID: 13511
			BLOWFISH,
			// Token: 0x040034C8 RID: 13512
			CAMELLIA,
			// Token: 0x040034C9 RID: 13513
			CAST5,
			// Token: 0x040034CA RID: 13514
			CAST6,
			// Token: 0x040034CB RID: 13515
			DES,
			// Token: 0x040034CC RID: 13516
			DESEDE,
			// Token: 0x040034CD RID: 13517
			ELGAMAL,
			// Token: 0x040034CE RID: 13518
			GOST28147,
			// Token: 0x040034CF RID: 13519
			HC128,
			// Token: 0x040034D0 RID: 13520
			HC256,
			// Token: 0x040034D1 RID: 13521
			IDEA,
			// Token: 0x040034D2 RID: 13522
			NOEKEON,
			// Token: 0x040034D3 RID: 13523
			PBEWITHSHAAND128BITRC4,
			// Token: 0x040034D4 RID: 13524
			PBEWITHSHAAND40BITRC4,
			// Token: 0x040034D5 RID: 13525
			RC2,
			// Token: 0x040034D6 RID: 13526
			RC5,
			// Token: 0x040034D7 RID: 13527
			RC5_64,
			// Token: 0x040034D8 RID: 13528
			RC6,
			// Token: 0x040034D9 RID: 13529
			RIJNDAEL,
			// Token: 0x040034DA RID: 13530
			RSA,
			// Token: 0x040034DB RID: 13531
			SALSA20,
			// Token: 0x040034DC RID: 13532
			SEED,
			// Token: 0x040034DD RID: 13533
			SERPENT,
			// Token: 0x040034DE RID: 13534
			SKIPJACK,
			// Token: 0x040034DF RID: 13535
			SM4,
			// Token: 0x040034E0 RID: 13536
			TEA,
			// Token: 0x040034E1 RID: 13537
			THREEFISH_256,
			// Token: 0x040034E2 RID: 13538
			THREEFISH_512,
			// Token: 0x040034E3 RID: 13539
			THREEFISH_1024,
			// Token: 0x040034E4 RID: 13540
			TNEPRES,
			// Token: 0x040034E5 RID: 13541
			TWOFISH,
			// Token: 0x040034E6 RID: 13542
			VMPC,
			// Token: 0x040034E7 RID: 13543
			VMPC_KSA3,
			// Token: 0x040034E8 RID: 13544
			XTEA
		}

		// Token: 0x02000904 RID: 2308
		private enum CipherMode
		{
			// Token: 0x040034EA RID: 13546
			ECB,
			// Token: 0x040034EB RID: 13547
			NONE,
			// Token: 0x040034EC RID: 13548
			CBC,
			// Token: 0x040034ED RID: 13549
			CCM,
			// Token: 0x040034EE RID: 13550
			CFB,
			// Token: 0x040034EF RID: 13551
			CTR,
			// Token: 0x040034F0 RID: 13552
			CTS,
			// Token: 0x040034F1 RID: 13553
			EAX,
			// Token: 0x040034F2 RID: 13554
			GCM,
			// Token: 0x040034F3 RID: 13555
			GOFB,
			// Token: 0x040034F4 RID: 13556
			OCB,
			// Token: 0x040034F5 RID: 13557
			OFB,
			// Token: 0x040034F6 RID: 13558
			OPENPGPCFB,
			// Token: 0x040034F7 RID: 13559
			SIC
		}

		// Token: 0x02000905 RID: 2309
		private enum CipherPadding
		{
			// Token: 0x040034F9 RID: 13561
			NOPADDING,
			// Token: 0x040034FA RID: 13562
			RAW,
			// Token: 0x040034FB RID: 13563
			ISO10126PADDING,
			// Token: 0x040034FC RID: 13564
			ISO10126D2PADDING,
			// Token: 0x040034FD RID: 13565
			ISO10126_2PADDING,
			// Token: 0x040034FE RID: 13566
			ISO7816_4PADDING,
			// Token: 0x040034FF RID: 13567
			ISO9797_1PADDING,
			// Token: 0x04003500 RID: 13568
			ISO9796_1,
			// Token: 0x04003501 RID: 13569
			ISO9796_1PADDING,
			// Token: 0x04003502 RID: 13570
			OAEP,
			// Token: 0x04003503 RID: 13571
			OAEPPADDING,
			// Token: 0x04003504 RID: 13572
			OAEPWITHMD5ANDMGF1PADDING,
			// Token: 0x04003505 RID: 13573
			OAEPWITHSHA1ANDMGF1PADDING,
			// Token: 0x04003506 RID: 13574
			OAEPWITHSHA_1ANDMGF1PADDING,
			// Token: 0x04003507 RID: 13575
			OAEPWITHSHA224ANDMGF1PADDING,
			// Token: 0x04003508 RID: 13576
			OAEPWITHSHA_224ANDMGF1PADDING,
			// Token: 0x04003509 RID: 13577
			OAEPWITHSHA256ANDMGF1PADDING,
			// Token: 0x0400350A RID: 13578
			OAEPWITHSHA_256ANDMGF1PADDING,
			// Token: 0x0400350B RID: 13579
			OAEPWITHSHA384ANDMGF1PADDING,
			// Token: 0x0400350C RID: 13580
			OAEPWITHSHA_384ANDMGF1PADDING,
			// Token: 0x0400350D RID: 13581
			OAEPWITHSHA512ANDMGF1PADDING,
			// Token: 0x0400350E RID: 13582
			OAEPWITHSHA_512ANDMGF1PADDING,
			// Token: 0x0400350F RID: 13583
			PKCS1,
			// Token: 0x04003510 RID: 13584
			PKCS1PADDING,
			// Token: 0x04003511 RID: 13585
			PKCS5,
			// Token: 0x04003512 RID: 13586
			PKCS5PADDING,
			// Token: 0x04003513 RID: 13587
			PKCS7,
			// Token: 0x04003514 RID: 13588
			PKCS7PADDING,
			// Token: 0x04003515 RID: 13589
			TBCPADDING,
			// Token: 0x04003516 RID: 13590
			WITHCTS,
			// Token: 0x04003517 RID: 13591
			X923PADDING,
			// Token: 0x04003518 RID: 13592
			ZEROBYTEPADDING
		}
	}
}
