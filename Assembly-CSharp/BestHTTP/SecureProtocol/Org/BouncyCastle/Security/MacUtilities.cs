using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Iana;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Rosstandart;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Paddings;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002E2 RID: 738
	public sealed class MacUtilities
	{
		// Token: 0x06001AF4 RID: 6900 RVA: 0x00022F1F File Offset: 0x0002111F
		private MacUtilities()
		{
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x000CAFAC File Offset: 0x000C91AC
		static MacUtilities()
		{
			MacUtilities.algorithms[IanaObjectIdentifiers.HmacMD5.Id] = "HMAC-MD5";
			MacUtilities.algorithms[IanaObjectIdentifiers.HmacRipeMD160.Id] = "HMAC-RIPEMD160";
			MacUtilities.algorithms[IanaObjectIdentifiers.HmacSha1.Id] = "HMAC-SHA1";
			MacUtilities.algorithms[IanaObjectIdentifiers.HmacTiger.Id] = "HMAC-TIGER";
			MacUtilities.algorithms[PkcsObjectIdentifiers.IdHmacWithSha1.Id] = "HMAC-SHA1";
			MacUtilities.algorithms[PkcsObjectIdentifiers.IdHmacWithSha224.Id] = "HMAC-SHA224";
			MacUtilities.algorithms[PkcsObjectIdentifiers.IdHmacWithSha256.Id] = "HMAC-SHA256";
			MacUtilities.algorithms[PkcsObjectIdentifiers.IdHmacWithSha384.Id] = "HMAC-SHA384";
			MacUtilities.algorithms[PkcsObjectIdentifiers.IdHmacWithSha512.Id] = "HMAC-SHA512";
			MacUtilities.algorithms[NistObjectIdentifiers.IdHMacWithSha3_224.Id] = "HMAC-SHA3-224";
			MacUtilities.algorithms[NistObjectIdentifiers.IdHMacWithSha3_256.Id] = "HMAC-SHA3-256";
			MacUtilities.algorithms[NistObjectIdentifiers.IdHMacWithSha3_384.Id] = "HMAC-SHA3-384";
			MacUtilities.algorithms[NistObjectIdentifiers.IdHMacWithSha3_512.Id] = "HMAC-SHA3-512";
			MacUtilities.algorithms[RosstandartObjectIdentifiers.id_tc26_hmac_gost_3411_12_256.Id] = "HMAC-GOST3411-2012-256";
			MacUtilities.algorithms[RosstandartObjectIdentifiers.id_tc26_hmac_gost_3411_12_512.Id] = "HMAC-GOST3411-2012-512";
			MacUtilities.algorithms["DES"] = "DESMAC";
			MacUtilities.algorithms["DES/CFB8"] = "DESMAC/CFB8";
			MacUtilities.algorithms["DES64"] = "DESMAC64";
			MacUtilities.algorithms["DESEDE"] = "DESEDEMAC";
			MacUtilities.algorithms[PkcsObjectIdentifiers.DesEde3Cbc.Id] = "DESEDEMAC";
			MacUtilities.algorithms["DESEDE/CFB8"] = "DESEDEMAC/CFB8";
			MacUtilities.algorithms["DESISO9797MAC"] = "DESWITHISO9797";
			MacUtilities.algorithms["DESEDE64"] = "DESEDEMAC64";
			MacUtilities.algorithms["DESEDE64WITHISO7816-4PADDING"] = "DESEDEMAC64WITHISO7816-4PADDING";
			MacUtilities.algorithms["DESEDEISO9797ALG1MACWITHISO7816-4PADDING"] = "DESEDEMAC64WITHISO7816-4PADDING";
			MacUtilities.algorithms["DESEDEISO9797ALG1WITHISO7816-4PADDING"] = "DESEDEMAC64WITHISO7816-4PADDING";
			MacUtilities.algorithms["ISO9797ALG3"] = "ISO9797ALG3MAC";
			MacUtilities.algorithms["ISO9797ALG3MACWITHISO7816-4PADDING"] = "ISO9797ALG3WITHISO7816-4PADDING";
			MacUtilities.algorithms["SKIPJACK"] = "SKIPJACKMAC";
			MacUtilities.algorithms["SKIPJACK/CFB8"] = "SKIPJACKMAC/CFB8";
			MacUtilities.algorithms["IDEA"] = "IDEAMAC";
			MacUtilities.algorithms["IDEA/CFB8"] = "IDEAMAC/CFB8";
			MacUtilities.algorithms["RC2"] = "RC2MAC";
			MacUtilities.algorithms["RC2/CFB8"] = "RC2MAC/CFB8";
			MacUtilities.algorithms["RC5"] = "RC5MAC";
			MacUtilities.algorithms["RC5/CFB8"] = "RC5MAC/CFB8";
			MacUtilities.algorithms["GOST28147"] = "GOST28147MAC";
			MacUtilities.algorithms["VMPC"] = "VMPCMAC";
			MacUtilities.algorithms["VMPC-MAC"] = "VMPCMAC";
			MacUtilities.algorithms["SIPHASH"] = "SIPHASH-2-4";
			MacUtilities.algorithms["PBEWITHHMACSHA"] = "PBEWITHHMACSHA1";
			MacUtilities.algorithms["1.3.14.3.2.26"] = "PBEWITHHMACSHA1";
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x000CB35B File Offset: 0x000C955B
		public static IMac GetMac(DerObjectIdentifier id)
		{
			return MacUtilities.GetMac(id.Id);
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x000CB368 File Offset: 0x000C9568
		public static IMac GetMac(string algorithm)
		{
			string text = Platform.ToUpperInvariant(algorithm);
			string text2 = (string)MacUtilities.algorithms[text];
			if (text2 == null)
			{
				text2 = text;
			}
			if (Platform.StartsWith(text2, "PBEWITH"))
			{
				text2 = text2.Substring("PBEWITH".Length);
			}
			if (Platform.StartsWith(text2, "HMAC"))
			{
				string algorithm2;
				if (Platform.StartsWith(text2, "HMAC-") || Platform.StartsWith(text2, "HMAC/"))
				{
					algorithm2 = text2.Substring(5);
				}
				else
				{
					algorithm2 = text2.Substring(4);
				}
				return new HMac(DigestUtilities.GetDigest(algorithm2));
			}
			if (text2 == "AESCMAC")
			{
				return new CMac(new AesEngine());
			}
			if (text2 == "DESMAC")
			{
				return new CbcBlockCipherMac(new DesEngine());
			}
			if (text2 == "DESMAC/CFB8")
			{
				return new CfbBlockCipherMac(new DesEngine());
			}
			if (text2 == "DESMAC64")
			{
				return new CbcBlockCipherMac(new DesEngine(), 64);
			}
			if (text2 == "DESEDECMAC")
			{
				return new CMac(new DesEdeEngine());
			}
			if (text2 == "DESEDEMAC")
			{
				return new CbcBlockCipherMac(new DesEdeEngine());
			}
			if (text2 == "DESEDEMAC/CFB8")
			{
				return new CfbBlockCipherMac(new DesEdeEngine());
			}
			if (text2 == "DESEDEMAC64")
			{
				return new CbcBlockCipherMac(new DesEdeEngine(), 64);
			}
			if (text2 == "DESEDEMAC64WITHISO7816-4PADDING")
			{
				return new CbcBlockCipherMac(new DesEdeEngine(), 64, new ISO7816d4Padding());
			}
			if (text2 == "DESWITHISO9797" || text2 == "ISO9797ALG3MAC")
			{
				return new ISO9797Alg3Mac(new DesEngine());
			}
			if (text2 == "ISO9797ALG3WITHISO7816-4PADDING")
			{
				return new ISO9797Alg3Mac(new DesEngine(), new ISO7816d4Padding());
			}
			if (text2 == "SKIPJACKMAC")
			{
				return new CbcBlockCipherMac(new SkipjackEngine());
			}
			if (text2 == "SKIPJACKMAC/CFB8")
			{
				return new CfbBlockCipherMac(new SkipjackEngine());
			}
			if (text2 == "IDEAMAC")
			{
				return new CbcBlockCipherMac(new IdeaEngine());
			}
			if (text2 == "IDEAMAC/CFB8")
			{
				return new CfbBlockCipherMac(new IdeaEngine());
			}
			if (text2 == "RC2MAC")
			{
				return new CbcBlockCipherMac(new RC2Engine());
			}
			if (text2 == "RC2MAC/CFB8")
			{
				return new CfbBlockCipherMac(new RC2Engine());
			}
			if (text2 == "RC5MAC")
			{
				return new CbcBlockCipherMac(new RC532Engine());
			}
			if (text2 == "RC5MAC/CFB8")
			{
				return new CfbBlockCipherMac(new RC532Engine());
			}
			if (text2 == "GOST28147MAC")
			{
				return new Gost28147Mac();
			}
			if (text2 == "VMPCMAC")
			{
				return new VmpcMac();
			}
			if (text2 == "SIPHASH-2-4")
			{
				return new SipHash();
			}
			throw new SecurityUtilityException("Mac " + text2 + " not recognised.");
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x000CB628 File Offset: 0x000C9828
		public static string GetAlgorithmName(DerObjectIdentifier oid)
		{
			return (string)MacUtilities.algorithms[oid.Id];
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x000CB63F File Offset: 0x000C983F
		public static byte[] CalculateMac(string algorithm, ICipherParameters cp, byte[] input)
		{
			IMac mac = MacUtilities.GetMac(algorithm);
			mac.Init(cp);
			mac.BlockUpdate(input, 0, input.Length);
			return MacUtilities.DoFinal(mac);
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x000CB660 File Offset: 0x000C9860
		public static byte[] DoFinal(IMac mac)
		{
			byte[] array = new byte[mac.GetMacSize()];
			mac.DoFinal(array, 0);
			return array;
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x000CB683 File Offset: 0x000C9883
		public static byte[] DoFinal(IMac mac, byte[] input)
		{
			mac.BlockUpdate(input, 0, input.Length);
			return MacUtilities.DoFinal(mac);
		}

		// Token: 0x040018E4 RID: 6372
		private static readonly IDictionary algorithms = Platform.CreateHashtable();
	}
}
