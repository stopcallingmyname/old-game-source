using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.EdEC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.Kdf;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002D9 RID: 729
	public sealed class AgreementUtilities
	{
		// Token: 0x06001AA3 RID: 6819 RVA: 0x00022F1F File Offset: 0x0002111F
		private AgreementUtilities()
		{
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x000C7CE8 File Offset: 0x000C5EE8
		static AgreementUtilities()
		{
			AgreementUtilities.algorithms[X9ObjectIdentifiers.DHSinglePassCofactorDHSha1KdfScheme.Id] = "ECCDHWITHSHA1KDF";
			AgreementUtilities.algorithms[X9ObjectIdentifiers.DHSinglePassStdDHSha1KdfScheme.Id] = "ECDHWITHSHA1KDF";
			AgreementUtilities.algorithms[X9ObjectIdentifiers.MqvSinglePassSha1KdfScheme.Id] = "ECMQVWITHSHA1KDF";
			AgreementUtilities.algorithms[EdECObjectIdentifiers.id_X25519.Id] = "X25519";
			AgreementUtilities.algorithms[EdECObjectIdentifiers.id_X448.Id] = "X448";
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x000C7D7C File Offset: 0x000C5F7C
		public static IBasicAgreement GetBasicAgreement(DerObjectIdentifier oid)
		{
			return AgreementUtilities.GetBasicAgreement(oid.Id);
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x000C7D8C File Offset: 0x000C5F8C
		public static IBasicAgreement GetBasicAgreement(string algorithm)
		{
			string mechanism = AgreementUtilities.GetMechanism(algorithm);
			if (mechanism == "DH" || mechanism == "DIFFIEHELLMAN")
			{
				return new DHBasicAgreement();
			}
			if (mechanism == "ECDH")
			{
				return new ECDHBasicAgreement();
			}
			if (mechanism == "ECDHC" || mechanism == "ECCDH")
			{
				return new ECDHCBasicAgreement();
			}
			if (mechanism == "ECMQV")
			{
				return new ECMqvBasicAgreement();
			}
			throw new SecurityUtilityException("Basic Agreement " + algorithm + " not recognised.");
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x000C7E1B File Offset: 0x000C601B
		public static IBasicAgreement GetBasicAgreementWithKdf(DerObjectIdentifier oid, string wrapAlgorithm)
		{
			return AgreementUtilities.GetBasicAgreementWithKdf(oid.Id, wrapAlgorithm);
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x000C7E2C File Offset: 0x000C602C
		public static IBasicAgreement GetBasicAgreementWithKdf(string agreeAlgorithm, string wrapAlgorithm)
		{
			string mechanism = AgreementUtilities.GetMechanism(agreeAlgorithm);
			if (mechanism == "DHWITHSHA1KDF" || mechanism == "ECDHWITHSHA1KDF")
			{
				return new ECDHWithKdfBasicAgreement(wrapAlgorithm, new ECDHKekGenerator(new Sha1Digest()));
			}
			if (mechanism == "ECMQVWITHSHA1KDF")
			{
				return new ECMqvWithKdfBasicAgreement(wrapAlgorithm, new ECDHKekGenerator(new Sha1Digest()));
			}
			throw new SecurityUtilityException("Basic Agreement (with KDF) " + agreeAlgorithm + " not recognised.");
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x000C7E9E File Offset: 0x000C609E
		public static IRawAgreement GetRawAgreement(DerObjectIdentifier oid)
		{
			return AgreementUtilities.GetRawAgreement(oid.Id);
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x000C7EAC File Offset: 0x000C60AC
		public static IRawAgreement GetRawAgreement(string algorithm)
		{
			string mechanism = AgreementUtilities.GetMechanism(algorithm);
			if (mechanism == "X25519")
			{
				return new X25519Agreement();
			}
			if (mechanism == "X448")
			{
				return new X448Agreement();
			}
			throw new SecurityUtilityException("Raw Agreement " + algorithm + " not recognised.");
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x000C7EFB File Offset: 0x000C60FB
		public static string GetAlgorithmName(DerObjectIdentifier oid)
		{
			return (string)AgreementUtilities.algorithms[oid.Id];
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x000C7F14 File Offset: 0x000C6114
		private static string GetMechanism(string algorithm)
		{
			string text = Platform.ToUpperInvariant(algorithm);
			string text2 = (string)AgreementUtilities.algorithms[text];
			if (text2 != null)
			{
				return text2;
			}
			return text;
		}

		// Token: 0x040018DC RID: 6364
		private static readonly IDictionary algorithms = Platform.CreateHashtable();
	}
}
