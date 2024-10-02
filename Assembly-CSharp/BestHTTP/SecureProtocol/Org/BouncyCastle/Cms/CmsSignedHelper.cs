using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Eac;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.TeleTrust;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000602 RID: 1538
	internal class CmsSignedHelper
	{
		// Token: 0x06003A7E RID: 14974 RVA: 0x0016AA50 File Offset: 0x00168C50
		private static void AddEntries(DerObjectIdentifier oid, string digest, string encryption)
		{
			string id = oid.Id;
			CmsSignedHelper.digestAlgs.Add(id, digest);
			CmsSignedHelper.encryptionAlgs.Add(id, encryption);
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x0016AA7C File Offset: 0x00168C7C
		static CmsSignedHelper()
		{
			CmsSignedHelper.AddEntries(NistObjectIdentifiers.DsaWithSha224, "SHA224", "DSA");
			CmsSignedHelper.AddEntries(NistObjectIdentifiers.DsaWithSha256, "SHA256", "DSA");
			CmsSignedHelper.AddEntries(NistObjectIdentifiers.DsaWithSha384, "SHA384", "DSA");
			CmsSignedHelper.AddEntries(NistObjectIdentifiers.DsaWithSha512, "SHA512", "DSA");
			CmsSignedHelper.AddEntries(OiwObjectIdentifiers.DsaWithSha1, "SHA1", "DSA");
			CmsSignedHelper.AddEntries(OiwObjectIdentifiers.MD4WithRsa, "MD4", "RSA");
			CmsSignedHelper.AddEntries(OiwObjectIdentifiers.MD4WithRsaEncryption, "MD4", "RSA");
			CmsSignedHelper.AddEntries(OiwObjectIdentifiers.MD5WithRsa, "MD5", "RSA");
			CmsSignedHelper.AddEntries(OiwObjectIdentifiers.Sha1WithRsa, "SHA1", "RSA");
			CmsSignedHelper.AddEntries(PkcsObjectIdentifiers.MD2WithRsaEncryption, "MD2", "RSA");
			CmsSignedHelper.AddEntries(PkcsObjectIdentifiers.MD4WithRsaEncryption, "MD4", "RSA");
			CmsSignedHelper.AddEntries(PkcsObjectIdentifiers.MD5WithRsaEncryption, "MD5", "RSA");
			CmsSignedHelper.AddEntries(PkcsObjectIdentifiers.Sha1WithRsaEncryption, "SHA1", "RSA");
			CmsSignedHelper.AddEntries(PkcsObjectIdentifiers.Sha224WithRsaEncryption, "SHA224", "RSA");
			CmsSignedHelper.AddEntries(PkcsObjectIdentifiers.Sha256WithRsaEncryption, "SHA256", "RSA");
			CmsSignedHelper.AddEntries(PkcsObjectIdentifiers.Sha384WithRsaEncryption, "SHA384", "RSA");
			CmsSignedHelper.AddEntries(PkcsObjectIdentifiers.Sha512WithRsaEncryption, "SHA512", "RSA");
			CmsSignedHelper.AddEntries(X9ObjectIdentifiers.ECDsaWithSha1, "SHA1", "ECDSA");
			CmsSignedHelper.AddEntries(X9ObjectIdentifiers.ECDsaWithSha224, "SHA224", "ECDSA");
			CmsSignedHelper.AddEntries(X9ObjectIdentifiers.ECDsaWithSha256, "SHA256", "ECDSA");
			CmsSignedHelper.AddEntries(X9ObjectIdentifiers.ECDsaWithSha384, "SHA384", "ECDSA");
			CmsSignedHelper.AddEntries(X9ObjectIdentifiers.ECDsaWithSha512, "SHA512", "ECDSA");
			CmsSignedHelper.AddEntries(X9ObjectIdentifiers.IdDsaWithSha1, "SHA1", "DSA");
			CmsSignedHelper.AddEntries(EacObjectIdentifiers.id_TA_ECDSA_SHA_1, "SHA1", "ECDSA");
			CmsSignedHelper.AddEntries(EacObjectIdentifiers.id_TA_ECDSA_SHA_224, "SHA224", "ECDSA");
			CmsSignedHelper.AddEntries(EacObjectIdentifiers.id_TA_ECDSA_SHA_256, "SHA256", "ECDSA");
			CmsSignedHelper.AddEntries(EacObjectIdentifiers.id_TA_ECDSA_SHA_384, "SHA384", "ECDSA");
			CmsSignedHelper.AddEntries(EacObjectIdentifiers.id_TA_ECDSA_SHA_512, "SHA512", "ECDSA");
			CmsSignedHelper.AddEntries(EacObjectIdentifiers.id_TA_RSA_v1_5_SHA_1, "SHA1", "RSA");
			CmsSignedHelper.AddEntries(EacObjectIdentifiers.id_TA_RSA_v1_5_SHA_256, "SHA256", "RSA");
			CmsSignedHelper.AddEntries(EacObjectIdentifiers.id_TA_RSA_PSS_SHA_1, "SHA1", "RSAandMGF1");
			CmsSignedHelper.AddEntries(EacObjectIdentifiers.id_TA_RSA_PSS_SHA_256, "SHA256", "RSAandMGF1");
			CmsSignedHelper.encryptionAlgs.Add(X9ObjectIdentifiers.IdDsa.Id, "DSA");
			CmsSignedHelper.encryptionAlgs.Add(PkcsObjectIdentifiers.RsaEncryption.Id, "RSA");
			CmsSignedHelper.encryptionAlgs.Add(TeleTrusTObjectIdentifiers.TeleTrusTRsaSignatureAlgorithm, "RSA");
			CmsSignedHelper.encryptionAlgs.Add(X509ObjectIdentifiers.IdEARsa.Id, "RSA");
			CmsSignedHelper.encryptionAlgs.Add(CmsSignedGenerator.EncryptionRsaPss, "RSAandMGF1");
			CmsSignedHelper.encryptionAlgs.Add(CryptoProObjectIdentifiers.GostR3410x94.Id, "GOST3410");
			CmsSignedHelper.encryptionAlgs.Add(CryptoProObjectIdentifiers.GostR3410x2001.Id, "ECGOST3410");
			CmsSignedHelper.encryptionAlgs.Add("1.3.6.1.4.1.5849.1.6.2", "ECGOST3410");
			CmsSignedHelper.encryptionAlgs.Add("1.3.6.1.4.1.5849.1.1.5", "GOST3410");
			CmsSignedHelper.digestAlgs.Add(PkcsObjectIdentifiers.MD2.Id, "MD2");
			CmsSignedHelper.digestAlgs.Add(PkcsObjectIdentifiers.MD4.Id, "MD4");
			CmsSignedHelper.digestAlgs.Add(PkcsObjectIdentifiers.MD5.Id, "MD5");
			CmsSignedHelper.digestAlgs.Add(OiwObjectIdentifiers.IdSha1.Id, "SHA1");
			CmsSignedHelper.digestAlgs.Add(NistObjectIdentifiers.IdSha224.Id, "SHA224");
			CmsSignedHelper.digestAlgs.Add(NistObjectIdentifiers.IdSha256.Id, "SHA256");
			CmsSignedHelper.digestAlgs.Add(NistObjectIdentifiers.IdSha384.Id, "SHA384");
			CmsSignedHelper.digestAlgs.Add(NistObjectIdentifiers.IdSha512.Id, "SHA512");
			CmsSignedHelper.digestAlgs.Add(TeleTrusTObjectIdentifiers.RipeMD128.Id, "RIPEMD128");
			CmsSignedHelper.digestAlgs.Add(TeleTrusTObjectIdentifiers.RipeMD160.Id, "RIPEMD160");
			CmsSignedHelper.digestAlgs.Add(TeleTrusTObjectIdentifiers.RipeMD256.Id, "RIPEMD256");
			CmsSignedHelper.digestAlgs.Add(CryptoProObjectIdentifiers.GostR3411.Id, "GOST3411");
			CmsSignedHelper.digestAlgs.Add("1.3.6.1.4.1.5849.1.2.1", "GOST3411");
			CmsSignedHelper.digestAliases.Add("SHA1", new string[]
			{
				"SHA-1"
			});
			CmsSignedHelper.digestAliases.Add("SHA224", new string[]
			{
				"SHA-224"
			});
			CmsSignedHelper.digestAliases.Add("SHA256", new string[]
			{
				"SHA-256"
			});
			CmsSignedHelper.digestAliases.Add("SHA384", new string[]
			{
				"SHA-384"
			});
			CmsSignedHelper.digestAliases.Add("SHA512", new string[]
			{
				"SHA-512"
			});
			CmsSignedHelper.noParams.Add(CmsSignedGenerator.EncryptionDsa);
			CmsSignedHelper.noParams.Add(CmsSignedHelper.EncryptionECDsaWithSha1);
			CmsSignedHelper.noParams.Add(CmsSignedHelper.EncryptionECDsaWithSha224);
			CmsSignedHelper.noParams.Add(CmsSignedHelper.EncryptionECDsaWithSha256);
			CmsSignedHelper.noParams.Add(CmsSignedHelper.EncryptionECDsaWithSha384);
			CmsSignedHelper.noParams.Add(CmsSignedHelper.EncryptionECDsaWithSha512);
			CmsSignedHelper.ecAlgorithms.Add(CmsSignedGenerator.DigestSha1, CmsSignedHelper.EncryptionECDsaWithSha1);
			CmsSignedHelper.ecAlgorithms.Add(CmsSignedGenerator.DigestSha224, CmsSignedHelper.EncryptionECDsaWithSha224);
			CmsSignedHelper.ecAlgorithms.Add(CmsSignedGenerator.DigestSha256, CmsSignedHelper.EncryptionECDsaWithSha256);
			CmsSignedHelper.ecAlgorithms.Add(CmsSignedGenerator.DigestSha384, CmsSignedHelper.EncryptionECDsaWithSha384);
			CmsSignedHelper.ecAlgorithms.Add(CmsSignedGenerator.DigestSha512, CmsSignedHelper.EncryptionECDsaWithSha512);
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x0016B0EC File Offset: 0x001692EC
		internal string GetDigestAlgName(string digestAlgOid)
		{
			string text = (string)CmsSignedHelper.digestAlgs[digestAlgOid];
			if (text != null)
			{
				return text;
			}
			return digestAlgOid;
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x0016B110 File Offset: 0x00169310
		internal AlgorithmIdentifier GetEncAlgorithmIdentifier(DerObjectIdentifier encOid, Asn1Encodable sigX509Parameters)
		{
			if (CmsSignedHelper.noParams.Contains(encOid.Id))
			{
				return new AlgorithmIdentifier(encOid);
			}
			return new AlgorithmIdentifier(encOid, sigX509Parameters);
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x0016B134 File Offset: 0x00169334
		internal string[] GetDigestAliases(string algName)
		{
			string[] array = (string[])CmsSignedHelper.digestAliases[algName];
			if (array != null)
			{
				return (string[])array.Clone();
			}
			return new string[0];
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x0016B168 File Offset: 0x00169368
		internal string GetEncryptionAlgName(string encryptionAlgOid)
		{
			string text = (string)CmsSignedHelper.encryptionAlgs[encryptionAlgOid];
			if (text != null)
			{
				return text;
			}
			return encryptionAlgOid;
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x0016B18C File Offset: 0x0016938C
		internal IDigest GetDigestInstance(string algorithm)
		{
			IDigest digest;
			try
			{
				digest = DigestUtilities.GetDigest(algorithm);
			}
			catch (SecurityUtilityException ex)
			{
				foreach (string algorithm2 in this.GetDigestAliases(algorithm))
				{
					try
					{
						return DigestUtilities.GetDigest(algorithm2);
					}
					catch (SecurityUtilityException)
					{
					}
				}
				throw ex;
			}
			return digest;
		}

		// Token: 0x06003A85 RID: 14981 RVA: 0x0016B1EC File Offset: 0x001693EC
		internal ISigner GetSignatureInstance(string algorithm)
		{
			return SignerUtilities.GetSigner(algorithm);
		}

		// Token: 0x06003A86 RID: 14982 RVA: 0x0016B1F4 File Offset: 0x001693F4
		internal IX509Store CreateAttributeStore(string type, Asn1Set certSet)
		{
			IList list = Platform.CreateArrayList();
			if (certSet != null)
			{
				foreach (object obj in certSet)
				{
					Asn1Encodable asn1Encodable = (Asn1Encodable)obj;
					try
					{
						Asn1Object asn1Object = asn1Encodable.ToAsn1Object();
						if (asn1Object is Asn1TaggedObject)
						{
							Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)asn1Object;
							if (asn1TaggedObject.TagNo == 2)
							{
								list.Add(new X509V2AttributeCertificate(Asn1Sequence.GetInstance(asn1TaggedObject, false).GetEncoded()));
							}
						}
					}
					catch (Exception e)
					{
						throw new CmsException("can't re-encode attribute certificate!", e);
					}
				}
			}
			IX509Store result;
			try
			{
				result = X509StoreFactory.Create("AttributeCertificate/" + type, new X509CollectionStoreParameters(list));
			}
			catch (ArgumentException e2)
			{
				throw new CmsException("can't setup the X509Store", e2);
			}
			return result;
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x0016B2DC File Offset: 0x001694DC
		internal IX509Store CreateCertificateStore(string type, Asn1Set certSet)
		{
			IList list = Platform.CreateArrayList();
			if (certSet != null)
			{
				this.AddCertsFromSet(list, certSet);
			}
			IX509Store result;
			try
			{
				result = X509StoreFactory.Create("Certificate/" + type, new X509CollectionStoreParameters(list));
			}
			catch (ArgumentException e)
			{
				throw new CmsException("can't setup the X509Store", e);
			}
			return result;
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x0016B334 File Offset: 0x00169534
		internal IX509Store CreateCrlStore(string type, Asn1Set crlSet)
		{
			IList list = Platform.CreateArrayList();
			if (crlSet != null)
			{
				this.AddCrlsFromSet(list, crlSet);
			}
			IX509Store result;
			try
			{
				result = X509StoreFactory.Create("CRL/" + type, new X509CollectionStoreParameters(list));
			}
			catch (ArgumentException e)
			{
				throw new CmsException("can't setup the X509Store", e);
			}
			return result;
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x0016B38C File Offset: 0x0016958C
		private void AddCertsFromSet(IList certs, Asn1Set certSet)
		{
			X509CertificateParser x509CertificateParser = new X509CertificateParser();
			foreach (object obj in certSet)
			{
				Asn1Encodable asn1Encodable = (Asn1Encodable)obj;
				try
				{
					Asn1Object asn1Object = asn1Encodable.ToAsn1Object();
					if (asn1Object is Asn1Sequence)
					{
						certs.Add(x509CertificateParser.ReadCertificate(asn1Object.GetEncoded()));
					}
				}
				catch (Exception e)
				{
					throw new CmsException("can't re-encode certificate!", e);
				}
			}
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x0016B424 File Offset: 0x00169624
		private void AddCrlsFromSet(IList crls, Asn1Set crlSet)
		{
			X509CrlParser x509CrlParser = new X509CrlParser();
			foreach (object obj in crlSet)
			{
				Asn1Encodable asn1Encodable = (Asn1Encodable)obj;
				try
				{
					crls.Add(x509CrlParser.ReadCrl(asn1Encodable.GetEncoded()));
				}
				catch (Exception e)
				{
					throw new CmsException("can't re-encode CRL!", e);
				}
			}
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x0016B4A8 File Offset: 0x001696A8
		internal AlgorithmIdentifier FixAlgID(AlgorithmIdentifier algId)
		{
			if (algId.Parameters == null)
			{
				return new AlgorithmIdentifier(algId.Algorithm, DerNull.Instance);
			}
			return algId;
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x0016B4C4 File Offset: 0x001696C4
		internal string GetEncOid(AsymmetricKeyParameter key, string digestOID)
		{
			string text;
			if (key is RsaKeyParameters)
			{
				if (!((RsaKeyParameters)key).IsPrivate)
				{
					throw new ArgumentException("Expected RSA private key");
				}
				text = CmsSignedGenerator.EncryptionRsa;
			}
			else if (key is DsaPrivateKeyParameters)
			{
				if (digestOID.Equals(CmsSignedGenerator.DigestSha1))
				{
					text = CmsSignedGenerator.EncryptionDsa;
				}
				else if (digestOID.Equals(CmsSignedGenerator.DigestSha224))
				{
					text = NistObjectIdentifiers.DsaWithSha224.Id;
				}
				else if (digestOID.Equals(CmsSignedGenerator.DigestSha256))
				{
					text = NistObjectIdentifiers.DsaWithSha256.Id;
				}
				else if (digestOID.Equals(CmsSignedGenerator.DigestSha384))
				{
					text = NistObjectIdentifiers.DsaWithSha384.Id;
				}
				else
				{
					if (!digestOID.Equals(CmsSignedGenerator.DigestSha512))
					{
						throw new ArgumentException("can't mix DSA with anything but SHA1/SHA2");
					}
					text = NistObjectIdentifiers.DsaWithSha512.Id;
				}
			}
			else if (key is ECPrivateKeyParameters)
			{
				if (((ECPrivateKeyParameters)key).AlgorithmName == "ECGOST3410")
				{
					text = CmsSignedGenerator.EncryptionECGost3410;
				}
				else
				{
					text = (string)CmsSignedHelper.ecAlgorithms[digestOID];
					if (text == null)
					{
						throw new ArgumentException("can't mix ECDSA with anything but SHA family digests");
					}
				}
			}
			else
			{
				if (!(key is Gost3410PrivateKeyParameters))
				{
					throw new ArgumentException("Unknown algorithm in CmsSignedGenerator.GetEncOid");
				}
				text = CmsSignedGenerator.EncryptionGost3410;
			}
			return text;
		}

		// Token: 0x04002646 RID: 9798
		internal static readonly CmsSignedHelper Instance = new CmsSignedHelper();

		// Token: 0x04002647 RID: 9799
		private static readonly string EncryptionECDsaWithSha1 = X9ObjectIdentifiers.ECDsaWithSha1.Id;

		// Token: 0x04002648 RID: 9800
		private static readonly string EncryptionECDsaWithSha224 = X9ObjectIdentifiers.ECDsaWithSha224.Id;

		// Token: 0x04002649 RID: 9801
		private static readonly string EncryptionECDsaWithSha256 = X9ObjectIdentifiers.ECDsaWithSha256.Id;

		// Token: 0x0400264A RID: 9802
		private static readonly string EncryptionECDsaWithSha384 = X9ObjectIdentifiers.ECDsaWithSha384.Id;

		// Token: 0x0400264B RID: 9803
		private static readonly string EncryptionECDsaWithSha512 = X9ObjectIdentifiers.ECDsaWithSha512.Id;

		// Token: 0x0400264C RID: 9804
		private static readonly IDictionary encryptionAlgs = Platform.CreateHashtable();

		// Token: 0x0400264D RID: 9805
		private static readonly IDictionary digestAlgs = Platform.CreateHashtable();

		// Token: 0x0400264E RID: 9806
		private static readonly IDictionary digestAliases = Platform.CreateHashtable();

		// Token: 0x0400264F RID: 9807
		private static readonly ISet noParams = new HashSet();

		// Token: 0x04002650 RID: 9808
		private static readonly IDictionary ecAlgorithms = Platform.CreateHashtable();
	}
}
