using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Kisa;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ntt;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005F2 RID: 1522
	public class CmsEnvelopedGenerator
	{
		// Token: 0x060039D2 RID: 14802 RVA: 0x00168313 File Offset: 0x00166513
		public CmsEnvelopedGenerator() : this(new SecureRandom())
		{
		}

		// Token: 0x060039D3 RID: 14803 RVA: 0x00168320 File Offset: 0x00166520
		public CmsEnvelopedGenerator(SecureRandom rand)
		{
			this.rand = rand;
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x060039D4 RID: 14804 RVA: 0x0016833A File Offset: 0x0016653A
		// (set) Token: 0x060039D5 RID: 14805 RVA: 0x00168342 File Offset: 0x00166542
		public CmsAttributeTableGenerator UnprotectedAttributeGenerator
		{
			get
			{
				return this.unprotectedAttributeGenerator;
			}
			set
			{
				this.unprotectedAttributeGenerator = value;
			}
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x0016834C File Offset: 0x0016654C
		public void AddKeyTransRecipient(X509Certificate cert)
		{
			KeyTransRecipientInfoGenerator keyTransRecipientInfoGenerator = new KeyTransRecipientInfoGenerator();
			keyTransRecipientInfoGenerator.RecipientCert = cert;
			this.recipientInfoGenerators.Add(keyTransRecipientInfoGenerator);
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x00168374 File Offset: 0x00166574
		public void AddKeyTransRecipient(AsymmetricKeyParameter pubKey, byte[] subKeyId)
		{
			KeyTransRecipientInfoGenerator keyTransRecipientInfoGenerator = new KeyTransRecipientInfoGenerator();
			keyTransRecipientInfoGenerator.RecipientPublicKey = pubKey;
			keyTransRecipientInfoGenerator.SubjectKeyIdentifier = new DerOctetString(subKeyId);
			this.recipientInfoGenerators.Add(keyTransRecipientInfoGenerator);
		}

		// Token: 0x060039D8 RID: 14808 RVA: 0x001683A7 File Offset: 0x001665A7
		public void AddKekRecipient(string keyAlgorithm, KeyParameter key, byte[] keyIdentifier)
		{
			this.AddKekRecipient(keyAlgorithm, key, new KekIdentifier(keyIdentifier, null, null));
		}

		// Token: 0x060039D9 RID: 14809 RVA: 0x001683BC File Offset: 0x001665BC
		public void AddKekRecipient(string keyAlgorithm, KeyParameter key, KekIdentifier kekIdentifier)
		{
			KekRecipientInfoGenerator kekRecipientInfoGenerator = new KekRecipientInfoGenerator();
			kekRecipientInfoGenerator.KekIdentifier = kekIdentifier;
			kekRecipientInfoGenerator.KeyEncryptionKeyOID = keyAlgorithm;
			kekRecipientInfoGenerator.KeyEncryptionKey = key;
			this.recipientInfoGenerators.Add(kekRecipientInfoGenerator);
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x001683F4 File Offset: 0x001665F4
		public void AddPasswordRecipient(CmsPbeKey pbeKey, string kekAlgorithmOid)
		{
			Pbkdf2Params parameters = new Pbkdf2Params(pbeKey.Salt, pbeKey.IterationCount);
			PasswordRecipientInfoGenerator passwordRecipientInfoGenerator = new PasswordRecipientInfoGenerator();
			passwordRecipientInfoGenerator.KeyDerivationAlgorithm = new AlgorithmIdentifier(PkcsObjectIdentifiers.IdPbkdf2, parameters);
			passwordRecipientInfoGenerator.KeyEncryptionKeyOID = kekAlgorithmOid;
			passwordRecipientInfoGenerator.KeyEncryptionKey = pbeKey.GetEncoded(kekAlgorithmOid);
			this.recipientInfoGenerators.Add(passwordRecipientInfoGenerator);
		}

		// Token: 0x060039DB RID: 14811 RVA: 0x0016844C File Offset: 0x0016664C
		public void AddKeyAgreementRecipient(string agreementAlgorithm, AsymmetricKeyParameter senderPrivateKey, AsymmetricKeyParameter senderPublicKey, X509Certificate recipientCert, string cekWrapAlgorithm)
		{
			IList list = Platform.CreateArrayList(1);
			list.Add(recipientCert);
			this.AddKeyAgreementRecipients(agreementAlgorithm, senderPrivateKey, senderPublicKey, list, cekWrapAlgorithm);
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x00168478 File Offset: 0x00166678
		public void AddKeyAgreementRecipients(string agreementAlgorithm, AsymmetricKeyParameter senderPrivateKey, AsymmetricKeyParameter senderPublicKey, ICollection recipientCerts, string cekWrapAlgorithm)
		{
			if (!senderPrivateKey.IsPrivate)
			{
				throw new ArgumentException("Expected private key", "senderPrivateKey");
			}
			if (senderPublicKey.IsPrivate)
			{
				throw new ArgumentException("Expected public key", "senderPublicKey");
			}
			KeyAgreeRecipientInfoGenerator keyAgreeRecipientInfoGenerator = new KeyAgreeRecipientInfoGenerator();
			keyAgreeRecipientInfoGenerator.KeyAgreementOID = new DerObjectIdentifier(agreementAlgorithm);
			keyAgreeRecipientInfoGenerator.KeyEncryptionOID = new DerObjectIdentifier(cekWrapAlgorithm);
			keyAgreeRecipientInfoGenerator.RecipientCerts = recipientCerts;
			keyAgreeRecipientInfoGenerator.SenderKeyPair = new AsymmetricCipherKeyPair(senderPublicKey, senderPrivateKey);
			this.recipientInfoGenerators.Add(keyAgreeRecipientInfoGenerator);
		}

		// Token: 0x060039DD RID: 14813 RVA: 0x001684F8 File Offset: 0x001666F8
		protected internal virtual AlgorithmIdentifier GetAlgorithmIdentifier(string encryptionOid, KeyParameter encKey, Asn1Encodable asn1Params, out ICipherParameters cipherParameters)
		{
			Asn1Object asn1Object;
			if (asn1Params != null)
			{
				asn1Object = asn1Params.ToAsn1Object();
				cipherParameters = ParameterUtilities.GetCipherParameters(encryptionOid, encKey, asn1Object);
			}
			else
			{
				asn1Object = DerNull.Instance;
				cipherParameters = encKey;
			}
			return new AlgorithmIdentifier(new DerObjectIdentifier(encryptionOid), asn1Object);
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x00168534 File Offset: 0x00166734
		protected internal virtual Asn1Encodable GenerateAsn1Parameters(string encryptionOid, byte[] encKeyBytes)
		{
			Asn1Encodable result = null;
			try
			{
				if (encryptionOid.Equals(CmsEnvelopedGenerator.RC2Cbc))
				{
					byte[] array = new byte[8];
					this.rand.NextBytes(array);
					int num = encKeyBytes.Length * 8;
					int parameterVersion;
					if (num < 256)
					{
						parameterVersion = (int)CmsEnvelopedGenerator.rc2Table[num];
					}
					else
					{
						parameterVersion = num;
					}
					result = new RC2CbcParameter(parameterVersion, array);
				}
				else
				{
					result = ParameterUtilities.GenerateParameters(encryptionOid, this.rand);
				}
			}
			catch (SecurityUtilityException)
			{
			}
			return result;
		}

		// Token: 0x040025E8 RID: 9704
		internal static readonly short[] rc2Table = new short[]
		{
			189,
			86,
			234,
			242,
			162,
			241,
			172,
			42,
			176,
			147,
			209,
			156,
			27,
			51,
			253,
			208,
			48,
			4,
			182,
			220,
			125,
			223,
			50,
			75,
			247,
			203,
			69,
			155,
			49,
			187,
			33,
			90,
			65,
			159,
			225,
			217,
			74,
			77,
			158,
			218,
			160,
			104,
			44,
			195,
			39,
			95,
			128,
			54,
			62,
			238,
			251,
			149,
			26,
			254,
			206,
			168,
			52,
			169,
			19,
			240,
			166,
			63,
			216,
			12,
			120,
			36,
			175,
			35,
			82,
			193,
			103,
			23,
			245,
			102,
			144,
			231,
			232,
			7,
			184,
			96,
			72,
			230,
			30,
			83,
			243,
			146,
			164,
			114,
			140,
			8,
			21,
			110,
			134,
			0,
			132,
			250,
			244,
			127,
			138,
			66,
			25,
			246,
			219,
			205,
			20,
			141,
			80,
			18,
			186,
			60,
			6,
			78,
			236,
			179,
			53,
			17,
			161,
			136,
			142,
			43,
			148,
			153,
			183,
			113,
			116,
			211,
			228,
			191,
			58,
			222,
			150,
			14,
			188,
			10,
			237,
			119,
			252,
			55,
			107,
			3,
			121,
			137,
			98,
			198,
			215,
			192,
			210,
			124,
			106,
			139,
			34,
			163,
			91,
			5,
			93,
			2,
			117,
			213,
			97,
			227,
			24,
			143,
			85,
			81,
			173,
			31,
			11,
			94,
			133,
			229,
			194,
			87,
			99,
			202,
			61,
			108,
			180,
			197,
			204,
			112,
			178,
			145,
			89,
			13,
			71,
			32,
			200,
			79,
			88,
			224,
			1,
			226,
			22,
			56,
			196,
			111,
			59,
			15,
			101,
			70,
			190,
			126,
			45,
			123,
			130,
			249,
			64,
			181,
			29,
			115,
			248,
			235,
			38,
			199,
			135,
			151,
			37,
			84,
			177,
			40,
			170,
			152,
			157,
			165,
			100,
			109,
			122,
			212,
			16,
			129,
			68,
			239,
			73,
			214,
			174,
			46,
			221,
			118,
			92,
			47,
			167,
			28,
			201,
			9,
			105,
			154,
			131,
			207,
			41,
			57,
			185,
			233,
			76,
			255,
			67,
			171
		};

		// Token: 0x040025E9 RID: 9705
		public static readonly string DesEde3Cbc = PkcsObjectIdentifiers.DesEde3Cbc.Id;

		// Token: 0x040025EA RID: 9706
		public static readonly string RC2Cbc = PkcsObjectIdentifiers.RC2Cbc.Id;

		// Token: 0x040025EB RID: 9707
		public const string IdeaCbc = "1.3.6.1.4.1.188.7.1.1.2";

		// Token: 0x040025EC RID: 9708
		public const string Cast5Cbc = "1.2.840.113533.7.66.10";

		// Token: 0x040025ED RID: 9709
		public static readonly string Aes128Cbc = NistObjectIdentifiers.IdAes128Cbc.Id;

		// Token: 0x040025EE RID: 9710
		public static readonly string Aes192Cbc = NistObjectIdentifiers.IdAes192Cbc.Id;

		// Token: 0x040025EF RID: 9711
		public static readonly string Aes256Cbc = NistObjectIdentifiers.IdAes256Cbc.Id;

		// Token: 0x040025F0 RID: 9712
		public static readonly string Camellia128Cbc = NttObjectIdentifiers.IdCamellia128Cbc.Id;

		// Token: 0x040025F1 RID: 9713
		public static readonly string Camellia192Cbc = NttObjectIdentifiers.IdCamellia192Cbc.Id;

		// Token: 0x040025F2 RID: 9714
		public static readonly string Camellia256Cbc = NttObjectIdentifiers.IdCamellia256Cbc.Id;

		// Token: 0x040025F3 RID: 9715
		public static readonly string SeedCbc = KisaObjectIdentifiers.IdSeedCbc.Id;

		// Token: 0x040025F4 RID: 9716
		public static readonly string DesEde3Wrap = PkcsObjectIdentifiers.IdAlgCms3DesWrap.Id;

		// Token: 0x040025F5 RID: 9717
		public static readonly string Aes128Wrap = NistObjectIdentifiers.IdAes128Wrap.Id;

		// Token: 0x040025F6 RID: 9718
		public static readonly string Aes192Wrap = NistObjectIdentifiers.IdAes192Wrap.Id;

		// Token: 0x040025F7 RID: 9719
		public static readonly string Aes256Wrap = NistObjectIdentifiers.IdAes256Wrap.Id;

		// Token: 0x040025F8 RID: 9720
		public static readonly string Camellia128Wrap = NttObjectIdentifiers.IdCamellia128Wrap.Id;

		// Token: 0x040025F9 RID: 9721
		public static readonly string Camellia192Wrap = NttObjectIdentifiers.IdCamellia192Wrap.Id;

		// Token: 0x040025FA RID: 9722
		public static readonly string Camellia256Wrap = NttObjectIdentifiers.IdCamellia256Wrap.Id;

		// Token: 0x040025FB RID: 9723
		public static readonly string SeedWrap = KisaObjectIdentifiers.IdNpkiAppCmsSeedWrap.Id;

		// Token: 0x040025FC RID: 9724
		public static readonly string ECDHSha1Kdf = X9ObjectIdentifiers.DHSinglePassStdDHSha1KdfScheme.Id;

		// Token: 0x040025FD RID: 9725
		public static readonly string ECMqvSha1Kdf = X9ObjectIdentifiers.MqvSinglePassSha1KdfScheme.Id;

		// Token: 0x040025FE RID: 9726
		internal readonly IList recipientInfoGenerators = Platform.CreateArrayList();

		// Token: 0x040025FF RID: 9727
		internal readonly SecureRandom rand;

		// Token: 0x04002600 RID: 9728
		internal CmsAttributeTableGenerator unprotectedAttributeGenerator;
	}
}
