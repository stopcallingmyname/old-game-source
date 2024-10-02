using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.OpenSsl
{
	// Token: 0x020002D4 RID: 724
	internal sealed class PemUtilities
	{
		// Token: 0x06001A90 RID: 6800 RVA: 0x000C7800 File Offset: 0x000C5A00
		static PemUtilities()
		{
			((PemUtilities.PemBaseAlg)Enums.GetArbitraryValue(typeof(PemUtilities.PemBaseAlg))).ToString();
			((PemUtilities.PemMode)Enums.GetArbitraryValue(typeof(PemUtilities.PemMode))).ToString();
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x000C7854 File Offset: 0x000C5A54
		private static void ParseDekAlgName(string dekAlgName, out PemUtilities.PemBaseAlg baseAlg, out PemUtilities.PemMode mode)
		{
			try
			{
				mode = PemUtilities.PemMode.ECB;
				if (dekAlgName == "DES-EDE" || dekAlgName == "DES-EDE3")
				{
					baseAlg = (PemUtilities.PemBaseAlg)Enums.GetEnumValue(typeof(PemUtilities.PemBaseAlg), dekAlgName);
					return;
				}
				int num = dekAlgName.LastIndexOf('-');
				if (num >= 0)
				{
					baseAlg = (PemUtilities.PemBaseAlg)Enums.GetEnumValue(typeof(PemUtilities.PemBaseAlg), dekAlgName.Substring(0, num));
					mode = (PemUtilities.PemMode)Enums.GetEnumValue(typeof(PemUtilities.PemMode), dekAlgName.Substring(num + 1));
					return;
				}
			}
			catch (ArgumentException)
			{
			}
			throw new EncryptionException("Unknown DEK algorithm: " + dekAlgName);
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x000C790C File Offset: 0x000C5B0C
		internal static byte[] Crypt(bool encrypt, byte[] bytes, char[] password, string dekAlgName, byte[] iv)
		{
			PemUtilities.PemBaseAlg baseAlg;
			PemUtilities.PemMode pemMode;
			PemUtilities.ParseDekAlgName(dekAlgName, out baseAlg, out pemMode);
			string text;
			switch (pemMode)
			{
			case PemUtilities.PemMode.CBC:
			case PemUtilities.PemMode.ECB:
				text = "PKCS5Padding";
				break;
			case PemUtilities.PemMode.CFB:
			case PemUtilities.PemMode.OFB:
				text = "NoPadding";
				break;
			default:
				throw new EncryptionException("Unknown DEK algorithm: " + dekAlgName);
			}
			byte[] array = iv;
			string text2;
			switch (baseAlg)
			{
			case PemUtilities.PemBaseAlg.AES_128:
			case PemUtilities.PemBaseAlg.AES_192:
			case PemUtilities.PemBaseAlg.AES_256:
				text2 = "AES";
				if (array.Length > 8)
				{
					array = new byte[8];
					Array.Copy(iv, 0, array, 0, array.Length);
				}
				break;
			case PemUtilities.PemBaseAlg.BF:
				text2 = "BLOWFISH";
				break;
			case PemUtilities.PemBaseAlg.DES:
				text2 = "DES";
				break;
			case PemUtilities.PemBaseAlg.DES_EDE:
			case PemUtilities.PemBaseAlg.DES_EDE3:
				text2 = "DESede";
				break;
			case PemUtilities.PemBaseAlg.RC2:
			case PemUtilities.PemBaseAlg.RC2_40:
			case PemUtilities.PemBaseAlg.RC2_64:
				text2 = "RC2";
				break;
			default:
				throw new EncryptionException("Unknown DEK algorithm: " + dekAlgName);
			}
			IBufferedCipher cipher = CipherUtilities.GetCipher(string.Concat(new object[]
			{
				text2,
				"/",
				pemMode,
				"/",
				text
			}));
			ICipherParameters parameters = PemUtilities.GetCipherParameters(password, baseAlg, array);
			if (pemMode != PemUtilities.PemMode.ECB)
			{
				parameters = new ParametersWithIV(parameters, iv);
			}
			cipher.Init(encrypt, parameters);
			return cipher.DoFinal(bytes);
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x000C7A44 File Offset: 0x000C5C44
		private static ICipherParameters GetCipherParameters(char[] password, PemUtilities.PemBaseAlg baseAlg, byte[] salt)
		{
			int keySize;
			string algorithm;
			switch (baseAlg)
			{
			case PemUtilities.PemBaseAlg.AES_128:
				keySize = 128;
				algorithm = "AES128";
				break;
			case PemUtilities.PemBaseAlg.AES_192:
				keySize = 192;
				algorithm = "AES192";
				break;
			case PemUtilities.PemBaseAlg.AES_256:
				keySize = 256;
				algorithm = "AES256";
				break;
			case PemUtilities.PemBaseAlg.BF:
				keySize = 128;
				algorithm = "BLOWFISH";
				break;
			case PemUtilities.PemBaseAlg.DES:
				keySize = 64;
				algorithm = "DES";
				break;
			case PemUtilities.PemBaseAlg.DES_EDE:
				keySize = 128;
				algorithm = "DESEDE";
				break;
			case PemUtilities.PemBaseAlg.DES_EDE3:
				keySize = 192;
				algorithm = "DESEDE3";
				break;
			case PemUtilities.PemBaseAlg.RC2:
				keySize = 128;
				algorithm = "RC2";
				break;
			case PemUtilities.PemBaseAlg.RC2_40:
				keySize = 40;
				algorithm = "RC2";
				break;
			case PemUtilities.PemBaseAlg.RC2_64:
				keySize = 64;
				algorithm = "RC2";
				break;
			default:
				return null;
			}
			OpenSslPbeParametersGenerator openSslPbeParametersGenerator = new OpenSslPbeParametersGenerator();
			openSslPbeParametersGenerator.Init(PbeParametersGenerator.Pkcs5PasswordToBytes(password), salt);
			return openSslPbeParametersGenerator.GenerateDerivedParameters(algorithm, keySize);
		}

		// Token: 0x02000901 RID: 2305
		private enum PemBaseAlg
		{
			// Token: 0x040034B5 RID: 13493
			AES_128,
			// Token: 0x040034B6 RID: 13494
			AES_192,
			// Token: 0x040034B7 RID: 13495
			AES_256,
			// Token: 0x040034B8 RID: 13496
			BF,
			// Token: 0x040034B9 RID: 13497
			DES,
			// Token: 0x040034BA RID: 13498
			DES_EDE,
			// Token: 0x040034BB RID: 13499
			DES_EDE3,
			// Token: 0x040034BC RID: 13500
			RC2,
			// Token: 0x040034BD RID: 13501
			RC2_40,
			// Token: 0x040034BE RID: 13502
			RC2_64
		}

		// Token: 0x02000902 RID: 2306
		private enum PemMode
		{
			// Token: 0x040034C0 RID: 13504
			CBC,
			// Token: 0x040034C1 RID: 13505
			CFB,
			// Token: 0x040034C2 RID: 13506
			ECB,
			// Token: 0x040034C3 RID: 13507
			OFB
		}
	}
}
