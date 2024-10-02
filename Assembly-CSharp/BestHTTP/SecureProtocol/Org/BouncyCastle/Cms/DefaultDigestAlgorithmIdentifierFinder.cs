using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.TeleTrust;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000600 RID: 1536
	public class DefaultDigestAlgorithmIdentifierFinder
	{
		// Token: 0x06003A69 RID: 14953 RVA: 0x0016A2A8 File Offset: 0x001684A8
		static DefaultDigestAlgorithmIdentifierFinder()
		{
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(OiwObjectIdentifiers.MD4WithRsaEncryption, PkcsObjectIdentifiers.MD4);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(OiwObjectIdentifiers.MD4WithRsa, PkcsObjectIdentifiers.MD4);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(OiwObjectIdentifiers.MD5WithRsa, PkcsObjectIdentifiers.MD5);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(OiwObjectIdentifiers.Sha1WithRsa, OiwObjectIdentifiers.IdSha1);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(OiwObjectIdentifiers.DsaWithSha1, OiwObjectIdentifiers.IdSha1);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(PkcsObjectIdentifiers.Sha224WithRsaEncryption, NistObjectIdentifiers.IdSha224);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(PkcsObjectIdentifiers.Sha256WithRsaEncryption, NistObjectIdentifiers.IdSha256);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(PkcsObjectIdentifiers.Sha384WithRsaEncryption, NistObjectIdentifiers.IdSha384);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(PkcsObjectIdentifiers.Sha512WithRsaEncryption, NistObjectIdentifiers.IdSha512);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(PkcsObjectIdentifiers.MD2WithRsaEncryption, PkcsObjectIdentifiers.MD2);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(PkcsObjectIdentifiers.MD4WithRsaEncryption, PkcsObjectIdentifiers.MD4);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(PkcsObjectIdentifiers.MD5WithRsaEncryption, PkcsObjectIdentifiers.MD5);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(PkcsObjectIdentifiers.Sha1WithRsaEncryption, OiwObjectIdentifiers.IdSha1);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(X9ObjectIdentifiers.ECDsaWithSha1, OiwObjectIdentifiers.IdSha1);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(X9ObjectIdentifiers.ECDsaWithSha224, NistObjectIdentifiers.IdSha224);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(X9ObjectIdentifiers.ECDsaWithSha256, NistObjectIdentifiers.IdSha256);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(X9ObjectIdentifiers.ECDsaWithSha384, NistObjectIdentifiers.IdSha384);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(X9ObjectIdentifiers.ECDsaWithSha512, NistObjectIdentifiers.IdSha512);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(X9ObjectIdentifiers.IdDsaWithSha1, OiwObjectIdentifiers.IdSha1);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(NistObjectIdentifiers.DsaWithSha224, NistObjectIdentifiers.IdSha224);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(NistObjectIdentifiers.DsaWithSha256, NistObjectIdentifiers.IdSha256);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(NistObjectIdentifiers.DsaWithSha384, NistObjectIdentifiers.IdSha384);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(NistObjectIdentifiers.DsaWithSha512, NistObjectIdentifiers.IdSha512);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD128, TeleTrusTObjectIdentifiers.RipeMD128);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD160, TeleTrusTObjectIdentifiers.RipeMD160);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(TeleTrusTObjectIdentifiers.RsaSignatureWithRipeMD256, TeleTrusTObjectIdentifiers.RipeMD256);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x94, CryptoProObjectIdentifiers.GostR3411);
			DefaultDigestAlgorithmIdentifierFinder.digestOids.Add(CryptoProObjectIdentifiers.GostR3411x94WithGostR3410x2001, CryptoProObjectIdentifiers.GostR3411);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHA-1", OiwObjectIdentifiers.IdSha1);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHA-224", NistObjectIdentifiers.IdSha224);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHA-256", NistObjectIdentifiers.IdSha256);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHA-384", NistObjectIdentifiers.IdSha384);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHA-512", NistObjectIdentifiers.IdSha512);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHA1", OiwObjectIdentifiers.IdSha1);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHA224", NistObjectIdentifiers.IdSha224);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHA256", NistObjectIdentifiers.IdSha256);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHA384", NistObjectIdentifiers.IdSha384);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHA512", NistObjectIdentifiers.IdSha512);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHA3-224", NistObjectIdentifiers.IdSha3_224);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHA3-256", NistObjectIdentifiers.IdSha3_256);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHA3-384", NistObjectIdentifiers.IdSha3_384);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHA3-512", NistObjectIdentifiers.IdSha3_512);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHAKE-128", NistObjectIdentifiers.IdShake128);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("SHAKE-256", NistObjectIdentifiers.IdShake256);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("GOST3411", CryptoProObjectIdentifiers.GostR3411);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("MD2", PkcsObjectIdentifiers.MD2);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("MD4", PkcsObjectIdentifiers.MD4);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("MD5", PkcsObjectIdentifiers.MD5);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("RIPEMD128", TeleTrusTObjectIdentifiers.RipeMD128);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("RIPEMD160", TeleTrusTObjectIdentifiers.RipeMD160);
			DefaultDigestAlgorithmIdentifierFinder.digestNameToOids.Add("RIPEMD256", TeleTrusTObjectIdentifiers.RipeMD256);
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x0016A6C8 File Offset: 0x001688C8
		public AlgorithmIdentifier find(AlgorithmIdentifier sigAlgId)
		{
			AlgorithmIdentifier result;
			if (sigAlgId.Algorithm.Equals(PkcsObjectIdentifiers.IdRsassaPss))
			{
				result = RsassaPssParameters.GetInstance(sigAlgId.Parameters).HashAlgorithm;
			}
			else
			{
				result = new AlgorithmIdentifier((DerObjectIdentifier)DefaultDigestAlgorithmIdentifierFinder.digestOids[sigAlgId.Algorithm], DerNull.Instance);
			}
			return result;
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x0016A71B File Offset: 0x0016891B
		public AlgorithmIdentifier find(string digAlgName)
		{
			return new AlgorithmIdentifier((DerObjectIdentifier)DefaultDigestAlgorithmIdentifierFinder.digestNameToOids[digAlgName], DerNull.Instance);
		}

		// Token: 0x0400262C RID: 9772
		private static readonly IDictionary digestOids = Platform.CreateHashtable();

		// Token: 0x0400262D RID: 9773
		private static readonly IDictionary digestNameToOids = Platform.CreateHashtable();
	}
}
