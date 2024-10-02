using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.GM;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Rosstandart;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.TeleTrust;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tsp
{
	// Token: 0x020002AD RID: 685
	public class TspUtil
	{
		// Token: 0x060018F4 RID: 6388 RVA: 0x000BB2FC File Offset: 0x000B94FC
		static TspUtil()
		{
			TspUtil.digestLengths.Add(PkcsObjectIdentifiers.MD5.Id, 16);
			TspUtil.digestLengths.Add(OiwObjectIdentifiers.IdSha1.Id, 20);
			TspUtil.digestLengths.Add(NistObjectIdentifiers.IdSha224.Id, 28);
			TspUtil.digestLengths.Add(NistObjectIdentifiers.IdSha256.Id, 32);
			TspUtil.digestLengths.Add(NistObjectIdentifiers.IdSha384.Id, 48);
			TspUtil.digestLengths.Add(NistObjectIdentifiers.IdSha512.Id, 64);
			TspUtil.digestLengths.Add(TeleTrusTObjectIdentifiers.RipeMD128.Id, 16);
			TspUtil.digestLengths.Add(TeleTrusTObjectIdentifiers.RipeMD160.Id, 20);
			TspUtil.digestLengths.Add(TeleTrusTObjectIdentifiers.RipeMD256.Id, 32);
			TspUtil.digestLengths.Add(CryptoProObjectIdentifiers.GostR3411.Id, 32);
			TspUtil.digestLengths.Add(RosstandartObjectIdentifiers.id_tc26_gost_3411_12_256.Id, 32);
			TspUtil.digestLengths.Add(RosstandartObjectIdentifiers.id_tc26_gost_3411_12_512.Id, 64);
			TspUtil.digestLengths.Add(GMObjectIdentifiers.sm3.Id, 32);
			TspUtil.digestNames.Add(PkcsObjectIdentifiers.MD5.Id, "MD5");
			TspUtil.digestNames.Add(OiwObjectIdentifiers.IdSha1.Id, "SHA1");
			TspUtil.digestNames.Add(NistObjectIdentifiers.IdSha224.Id, "SHA224");
			TspUtil.digestNames.Add(NistObjectIdentifiers.IdSha256.Id, "SHA256");
			TspUtil.digestNames.Add(NistObjectIdentifiers.IdSha384.Id, "SHA384");
			TspUtil.digestNames.Add(NistObjectIdentifiers.IdSha512.Id, "SHA512");
			TspUtil.digestNames.Add(PkcsObjectIdentifiers.MD5WithRsaEncryption.Id, "MD5");
			TspUtil.digestNames.Add(PkcsObjectIdentifiers.Sha1WithRsaEncryption.Id, "SHA1");
			TspUtil.digestNames.Add(PkcsObjectIdentifiers.Sha224WithRsaEncryption.Id, "SHA224");
			TspUtil.digestNames.Add(PkcsObjectIdentifiers.Sha256WithRsaEncryption.Id, "SHA256");
			TspUtil.digestNames.Add(PkcsObjectIdentifiers.Sha384WithRsaEncryption.Id, "SHA384");
			TspUtil.digestNames.Add(PkcsObjectIdentifiers.Sha512WithRsaEncryption.Id, "SHA512");
			TspUtil.digestNames.Add(TeleTrusTObjectIdentifiers.RipeMD128.Id, "RIPEMD128");
			TspUtil.digestNames.Add(TeleTrusTObjectIdentifiers.RipeMD160.Id, "RIPEMD160");
			TspUtil.digestNames.Add(TeleTrusTObjectIdentifiers.RipeMD256.Id, "RIPEMD256");
			TspUtil.digestNames.Add(CryptoProObjectIdentifiers.GostR3411.Id, "GOST3411");
			TspUtil.digestNames.Add(OiwObjectIdentifiers.DsaWithSha1.Id, "SHA1");
			TspUtil.digestNames.Add(OiwObjectIdentifiers.Sha1WithRsa.Id, "SHA1");
			TspUtil.digestNames.Add(OiwObjectIdentifiers.MD5WithRsa.Id, "MD5");
			TspUtil.digestNames.Add(RosstandartObjectIdentifiers.id_tc26_gost_3411_12_256.Id, "GOST3411-2012-256");
			TspUtil.digestNames.Add(RosstandartObjectIdentifiers.id_tc26_gost_3411_12_512.Id, "GOST3411-2012-512");
			TspUtil.digestNames.Add(GMObjectIdentifiers.sm3.Id, "SM3");
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x000BB6C0 File Offset: 0x000B98C0
		public static ICollection GetSignatureTimestamps(SignerInformation signerInfo)
		{
			IList list = Platform.CreateArrayList();
			BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttributes = signerInfo.UnsignedAttributes;
			if (unsignedAttributes != null)
			{
				foreach (object obj in unsignedAttributes.GetAll(PkcsObjectIdentifiers.IdAASignatureTimeStampToken))
				{
					foreach (object obj2 in ((Attribute)obj).AttrValues)
					{
						Asn1Encodable asn1Encodable = (Asn1Encodable)obj2;
						try
						{
							TimeStampToken timeStampToken = new TimeStampToken(BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.ContentInfo.GetInstance(asn1Encodable.ToAsn1Object()));
							TimeStampTokenInfo timeStampInfo = timeStampToken.TimeStampInfo;
							if (!Arrays.ConstantTimeAreEqual(DigestUtilities.CalculateDigest(TspUtil.GetDigestAlgName(timeStampInfo.MessageImprintAlgOid), signerInfo.GetSignature()), timeStampInfo.GetMessageImprintDigest()))
							{
								throw new TspValidationException("Incorrect digest in message imprint");
							}
							list.Add(timeStampToken);
						}
						catch (SecurityUtilityException)
						{
							throw new TspValidationException("Unknown hash algorithm specified in timestamp");
						}
						catch (Exception)
						{
							throw new TspValidationException("Timestamp could not be parsed");
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x000BB804 File Offset: 0x000B9A04
		public static void ValidateCertificate(X509Certificate cert)
		{
			if (cert.Version != 3)
			{
				throw new ArgumentException("Certificate must have an ExtendedKeyUsage extension.");
			}
			Asn1OctetString extensionValue = cert.GetExtensionValue(X509Extensions.ExtendedKeyUsage);
			if (extensionValue == null)
			{
				throw new TspValidationException("Certificate must have an ExtendedKeyUsage extension.");
			}
			if (!cert.GetCriticalExtensionOids().Contains(X509Extensions.ExtendedKeyUsage.Id))
			{
				throw new TspValidationException("Certificate must have an ExtendedKeyUsage extension marked as critical.");
			}
			try
			{
				ExtendedKeyUsage instance = ExtendedKeyUsage.GetInstance(Asn1Object.FromByteArray(extensionValue.GetOctets()));
				if (!instance.HasKeyPurposeId(KeyPurposeID.IdKPTimeStamping) || instance.Count != 1)
				{
					throw new TspValidationException("ExtendedKeyUsage not solely time stamping.");
				}
			}
			catch (IOException)
			{
				throw new TspValidationException("cannot process ExtendedKeyUsage extension");
			}
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x000BB8B4 File Offset: 0x000B9AB4
		internal static string GetDigestAlgName(string digestAlgOID)
		{
			string text = (string)TspUtil.digestNames[digestAlgOID];
			if (text == null)
			{
				return digestAlgOID;
			}
			return text;
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x000BB8D8 File Offset: 0x000B9AD8
		internal static int GetDigestLength(string digestAlgOID)
		{
			if (!TspUtil.digestLengths.Contains(digestAlgOID))
			{
				throw new TspException("digest algorithm cannot be found.");
			}
			return (int)TspUtil.digestLengths[digestAlgOID];
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x000BB902 File Offset: 0x000B9B02
		internal static IDigest CreateDigestInstance(string digestAlgOID)
		{
			return DigestUtilities.GetDigest(TspUtil.GetDigestAlgName(digestAlgOID));
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x000BB90F File Offset: 0x000B9B0F
		internal static ISet GetCriticalExtensionOids(X509Extensions extensions)
		{
			if (extensions == null)
			{
				return TspUtil.EmptySet;
			}
			return CollectionUtilities.ReadOnly(new HashSet(extensions.GetCriticalExtensionOids()));
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x000BB92A File Offset: 0x000B9B2A
		internal static ISet GetNonCriticalExtensionOids(X509Extensions extensions)
		{
			if (extensions == null)
			{
				return TspUtil.EmptySet;
			}
			return CollectionUtilities.ReadOnly(new HashSet(extensions.GetNonCriticalExtensionOids()));
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x000BB945 File Offset: 0x000B9B45
		internal static IList GetExtensionOids(X509Extensions extensions)
		{
			if (extensions == null)
			{
				return TspUtil.EmptyList;
			}
			return CollectionUtilities.ReadOnly(Platform.CreateArrayList(extensions.GetExtensionOids()));
		}

		// Token: 0x04001866 RID: 6246
		private static ISet EmptySet = CollectionUtilities.ReadOnly(new HashSet());

		// Token: 0x04001867 RID: 6247
		private static IList EmptyList = CollectionUtilities.ReadOnly(Platform.CreateArrayList());

		// Token: 0x04001868 RID: 6248
		private static readonly IDictionary digestLengths = Platform.CreateHashtable();

		// Token: 0x04001869 RID: 6249
		private static readonly IDictionary digestNames = Platform.CreateHashtable();
	}
}
