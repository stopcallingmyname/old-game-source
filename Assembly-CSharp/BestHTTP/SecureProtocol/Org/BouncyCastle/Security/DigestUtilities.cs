using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.GM;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Misc;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Rosstandart;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.TeleTrust;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.UA;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002DB RID: 731
	public sealed class DigestUtilities
	{
		// Token: 0x06001AB6 RID: 6838 RVA: 0x00022F1F File Offset: 0x0002111F
		private DigestUtilities()
		{
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x000C90E4 File Offset: 0x000C72E4
		static DigestUtilities()
		{
			((DigestUtilities.DigestAlgorithm)Enums.GetArbitraryValue(typeof(DigestUtilities.DigestAlgorithm))).ToString();
			DigestUtilities.algorithms[PkcsObjectIdentifiers.MD2.Id] = "MD2";
			DigestUtilities.algorithms[PkcsObjectIdentifiers.MD4.Id] = "MD4";
			DigestUtilities.algorithms[PkcsObjectIdentifiers.MD5.Id] = "MD5";
			DigestUtilities.algorithms["SHA1"] = "SHA-1";
			DigestUtilities.algorithms[OiwObjectIdentifiers.IdSha1.Id] = "SHA-1";
			DigestUtilities.algorithms["SHA224"] = "SHA-224";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha224.Id] = "SHA-224";
			DigestUtilities.algorithms["SHA256"] = "SHA-256";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha256.Id] = "SHA-256";
			DigestUtilities.algorithms["SHA384"] = "SHA-384";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha384.Id] = "SHA-384";
			DigestUtilities.algorithms["SHA512"] = "SHA-512";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha512.Id] = "SHA-512";
			DigestUtilities.algorithms["SHA512/224"] = "SHA-512/224";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha512_224.Id] = "SHA-512/224";
			DigestUtilities.algorithms["SHA512/256"] = "SHA-512/256";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha512_256.Id] = "SHA-512/256";
			DigestUtilities.algorithms["RIPEMD-128"] = "RIPEMD128";
			DigestUtilities.algorithms[TeleTrusTObjectIdentifiers.RipeMD128.Id] = "RIPEMD128";
			DigestUtilities.algorithms["RIPEMD-160"] = "RIPEMD160";
			DigestUtilities.algorithms[TeleTrusTObjectIdentifiers.RipeMD160.Id] = "RIPEMD160";
			DigestUtilities.algorithms["RIPEMD-256"] = "RIPEMD256";
			DigestUtilities.algorithms[TeleTrusTObjectIdentifiers.RipeMD256.Id] = "RIPEMD256";
			DigestUtilities.algorithms["RIPEMD-320"] = "RIPEMD320";
			DigestUtilities.algorithms[CryptoProObjectIdentifiers.GostR3411.Id] = "GOST3411";
			DigestUtilities.algorithms["KECCAK224"] = "KECCAK-224";
			DigestUtilities.algorithms["KECCAK256"] = "KECCAK-256";
			DigestUtilities.algorithms["KECCAK288"] = "KECCAK-288";
			DigestUtilities.algorithms["KECCAK384"] = "KECCAK-384";
			DigestUtilities.algorithms["KECCAK512"] = "KECCAK-512";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha3_224.Id] = "SHA3-224";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha3_256.Id] = "SHA3-256";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha3_384.Id] = "SHA3-384";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdSha3_512.Id] = "SHA3-512";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdShake128.Id] = "SHAKE128";
			DigestUtilities.algorithms[NistObjectIdentifiers.IdShake256.Id] = "SHAKE256";
			DigestUtilities.algorithms[GMObjectIdentifiers.sm3.Id] = "SM3";
			DigestUtilities.algorithms[MiscObjectIdentifiers.id_blake2b160.Id] = "BLAKE2B-160";
			DigestUtilities.algorithms[MiscObjectIdentifiers.id_blake2b256.Id] = "BLAKE2B-256";
			DigestUtilities.algorithms[MiscObjectIdentifiers.id_blake2b384.Id] = "BLAKE2B-384";
			DigestUtilities.algorithms[MiscObjectIdentifiers.id_blake2b512.Id] = "BLAKE2B-512";
			DigestUtilities.algorithms[MiscObjectIdentifiers.id_blake2s128.Id] = "BLAKE2S-128";
			DigestUtilities.algorithms[MiscObjectIdentifiers.id_blake2s160.Id] = "BLAKE2S-160";
			DigestUtilities.algorithms[MiscObjectIdentifiers.id_blake2s224.Id] = "BLAKE2S-224";
			DigestUtilities.algorithms[MiscObjectIdentifiers.id_blake2s256.Id] = "BLAKE2S-256";
			DigestUtilities.algorithms[RosstandartObjectIdentifiers.id_tc26_gost_3411_12_256.Id] = "GOST3411-2012-256";
			DigestUtilities.algorithms[RosstandartObjectIdentifiers.id_tc26_gost_3411_12_512.Id] = "GOST3411-2012-512";
			DigestUtilities.algorithms[UAObjectIdentifiers.dstu7564digest_256.Id] = "DSTU7564-256";
			DigestUtilities.algorithms[UAObjectIdentifiers.dstu7564digest_384.Id] = "DSTU7564-384";
			DigestUtilities.algorithms[UAObjectIdentifiers.dstu7564digest_512.Id] = "DSTU7564-512";
			DigestUtilities.oids["MD2"] = PkcsObjectIdentifiers.MD2;
			DigestUtilities.oids["MD4"] = PkcsObjectIdentifiers.MD4;
			DigestUtilities.oids["MD5"] = PkcsObjectIdentifiers.MD5;
			DigestUtilities.oids["SHA-1"] = OiwObjectIdentifiers.IdSha1;
			DigestUtilities.oids["SHA-224"] = NistObjectIdentifiers.IdSha224;
			DigestUtilities.oids["SHA-256"] = NistObjectIdentifiers.IdSha256;
			DigestUtilities.oids["SHA-384"] = NistObjectIdentifiers.IdSha384;
			DigestUtilities.oids["SHA-512"] = NistObjectIdentifiers.IdSha512;
			DigestUtilities.oids["SHA-512/224"] = NistObjectIdentifiers.IdSha512_224;
			DigestUtilities.oids["SHA-512/256"] = NistObjectIdentifiers.IdSha512_256;
			DigestUtilities.oids["SHA3-224"] = NistObjectIdentifiers.IdSha3_224;
			DigestUtilities.oids["SHA3-256"] = NistObjectIdentifiers.IdSha3_256;
			DigestUtilities.oids["SHA3-384"] = NistObjectIdentifiers.IdSha3_384;
			DigestUtilities.oids["SHA3-512"] = NistObjectIdentifiers.IdSha3_512;
			DigestUtilities.oids["SHAKE128"] = NistObjectIdentifiers.IdShake128;
			DigestUtilities.oids["SHAKE256"] = NistObjectIdentifiers.IdShake256;
			DigestUtilities.oids["RIPEMD128"] = TeleTrusTObjectIdentifiers.RipeMD128;
			DigestUtilities.oids["RIPEMD160"] = TeleTrusTObjectIdentifiers.RipeMD160;
			DigestUtilities.oids["RIPEMD256"] = TeleTrusTObjectIdentifiers.RipeMD256;
			DigestUtilities.oids["GOST3411"] = CryptoProObjectIdentifiers.GostR3411;
			DigestUtilities.oids["SM3"] = GMObjectIdentifiers.sm3;
			DigestUtilities.oids["BLAKE2B-160"] = MiscObjectIdentifiers.id_blake2b160;
			DigestUtilities.oids["BLAKE2B-256"] = MiscObjectIdentifiers.id_blake2b256;
			DigestUtilities.oids["BLAKE2B-384"] = MiscObjectIdentifiers.id_blake2b384;
			DigestUtilities.oids["BLAKE2B-512"] = MiscObjectIdentifiers.id_blake2b512;
			DigestUtilities.oids["BLAKE2S-128"] = MiscObjectIdentifiers.id_blake2s128;
			DigestUtilities.oids["BLAKE2S-160"] = MiscObjectIdentifiers.id_blake2s160;
			DigestUtilities.oids["BLAKE2S-224"] = MiscObjectIdentifiers.id_blake2s224;
			DigestUtilities.oids["BLAKE2S-256"] = MiscObjectIdentifiers.id_blake2s256;
			DigestUtilities.oids["GOST3411-2012-256"] = RosstandartObjectIdentifiers.id_tc26_gost_3411_12_256;
			DigestUtilities.oids["GOST3411-2012-512"] = RosstandartObjectIdentifiers.id_tc26_gost_3411_12_512;
			DigestUtilities.oids["DSTU7564-256"] = UAObjectIdentifiers.dstu7564digest_256;
			DigestUtilities.oids["DSTU7564-384"] = UAObjectIdentifiers.dstu7564digest_384;
			DigestUtilities.oids["DSTU7564-512"] = UAObjectIdentifiers.dstu7564digest_512;
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x000C9864 File Offset: 0x000C7A64
		public static DerObjectIdentifier GetObjectIdentifier(string mechanism)
		{
			if (mechanism == null)
			{
				throw new ArgumentNullException("mechanism");
			}
			mechanism = Platform.ToUpperInvariant(mechanism);
			string text = (string)DigestUtilities.algorithms[mechanism];
			if (text != null)
			{
				mechanism = text;
			}
			return (DerObjectIdentifier)DigestUtilities.oids[mechanism];
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06001AB9 RID: 6841 RVA: 0x000C98AE File Offset: 0x000C7AAE
		public static ICollection Algorithms
		{
			get
			{
				return DigestUtilities.oids.Keys;
			}
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x000C98BA File Offset: 0x000C7ABA
		public static IDigest GetDigest(DerObjectIdentifier id)
		{
			return DigestUtilities.GetDigest(id.Id);
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x000C98C8 File Offset: 0x000C7AC8
		public static IDigest GetDigest(string algorithm)
		{
			string text = Platform.ToUpperInvariant(algorithm);
			string text2 = (string)DigestUtilities.algorithms[text];
			if (text2 == null)
			{
				text2 = text;
			}
			try
			{
				switch ((DigestUtilities.DigestAlgorithm)Enums.GetEnumValue(typeof(DigestUtilities.DigestAlgorithm), text2))
				{
				case DigestUtilities.DigestAlgorithm.BLAKE2B_160:
					return new Blake2bDigest(160);
				case DigestUtilities.DigestAlgorithm.BLAKE2B_256:
					return new Blake2bDigest(256);
				case DigestUtilities.DigestAlgorithm.BLAKE2B_384:
					return new Blake2bDigest(384);
				case DigestUtilities.DigestAlgorithm.BLAKE2B_512:
					return new Blake2bDigest(512);
				case DigestUtilities.DigestAlgorithm.BLAKE2S_128:
					return new Blake2sDigest(128);
				case DigestUtilities.DigestAlgorithm.BLAKE2S_160:
					return new Blake2sDigest(160);
				case DigestUtilities.DigestAlgorithm.BLAKE2S_224:
					return new Blake2sDigest(224);
				case DigestUtilities.DigestAlgorithm.BLAKE2S_256:
					return new Blake2sDigest(256);
				case DigestUtilities.DigestAlgorithm.DSTU7564_256:
					return new Dstu7564Digest(256);
				case DigestUtilities.DigestAlgorithm.DSTU7564_384:
					return new Dstu7564Digest(384);
				case DigestUtilities.DigestAlgorithm.DSTU7564_512:
					return new Dstu7564Digest(512);
				case DigestUtilities.DigestAlgorithm.GOST3411:
					return new Gost3411Digest();
				case DigestUtilities.DigestAlgorithm.GOST3411_2012_256:
					return new GOST3411_2012_256Digest();
				case DigestUtilities.DigestAlgorithm.GOST3411_2012_512:
					return new GOST3411_2012_512Digest();
				case DigestUtilities.DigestAlgorithm.KECCAK_224:
					return new KeccakDigest(224);
				case DigestUtilities.DigestAlgorithm.KECCAK_256:
					return new KeccakDigest(256);
				case DigestUtilities.DigestAlgorithm.KECCAK_288:
					return new KeccakDigest(288);
				case DigestUtilities.DigestAlgorithm.KECCAK_384:
					return new KeccakDigest(384);
				case DigestUtilities.DigestAlgorithm.KECCAK_512:
					return new KeccakDigest(512);
				case DigestUtilities.DigestAlgorithm.MD2:
					return new MD2Digest();
				case DigestUtilities.DigestAlgorithm.MD4:
					return new MD4Digest();
				case DigestUtilities.DigestAlgorithm.MD5:
					return new MD5Digest();
				case DigestUtilities.DigestAlgorithm.NONE:
					return new NullDigest();
				case DigestUtilities.DigestAlgorithm.RIPEMD128:
					return new RipeMD128Digest();
				case DigestUtilities.DigestAlgorithm.RIPEMD160:
					return new RipeMD160Digest();
				case DigestUtilities.DigestAlgorithm.RIPEMD256:
					return new RipeMD256Digest();
				case DigestUtilities.DigestAlgorithm.RIPEMD320:
					return new RipeMD320Digest();
				case DigestUtilities.DigestAlgorithm.SHA_1:
					return new Sha1Digest();
				case DigestUtilities.DigestAlgorithm.SHA_224:
					return new Sha224Digest();
				case DigestUtilities.DigestAlgorithm.SHA_256:
					return new Sha256Digest();
				case DigestUtilities.DigestAlgorithm.SHA_384:
					return new Sha384Digest();
				case DigestUtilities.DigestAlgorithm.SHA_512:
					return new Sha512Digest();
				case DigestUtilities.DigestAlgorithm.SHA_512_224:
					return new Sha512tDigest(224);
				case DigestUtilities.DigestAlgorithm.SHA_512_256:
					return new Sha512tDigest(256);
				case DigestUtilities.DigestAlgorithm.SHA3_224:
					return new Sha3Digest(224);
				case DigestUtilities.DigestAlgorithm.SHA3_256:
					return new Sha3Digest(256);
				case DigestUtilities.DigestAlgorithm.SHA3_384:
					return new Sha3Digest(384);
				case DigestUtilities.DigestAlgorithm.SHA3_512:
					return new Sha3Digest(512);
				case DigestUtilities.DigestAlgorithm.SHAKE128:
					return new ShakeDigest(128);
				case DigestUtilities.DigestAlgorithm.SHAKE256:
					return new ShakeDigest(256);
				case DigestUtilities.DigestAlgorithm.SM3:
					return new SM3Digest();
				case DigestUtilities.DigestAlgorithm.TIGER:
					return new TigerDigest();
				case DigestUtilities.DigestAlgorithm.WHIRLPOOL:
					return new WhirlpoolDigest();
				}
			}
			catch (ArgumentException)
			{
			}
			throw new SecurityUtilityException("Digest " + text2 + " not recognised.");
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x000C9C30 File Offset: 0x000C7E30
		public static string GetAlgorithmName(DerObjectIdentifier oid)
		{
			return (string)DigestUtilities.algorithms[oid.Id];
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x000C9C47 File Offset: 0x000C7E47
		public static byte[] CalculateDigest(string algorithm, byte[] input)
		{
			IDigest digest = DigestUtilities.GetDigest(algorithm);
			digest.BlockUpdate(input, 0, input.Length);
			return DigestUtilities.DoFinal(digest);
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x000C9C60 File Offset: 0x000C7E60
		public static byte[] DoFinal(IDigest digest)
		{
			byte[] array = new byte[digest.GetDigestSize()];
			digest.DoFinal(array, 0);
			return array;
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x000C9C83 File Offset: 0x000C7E83
		public static byte[] DoFinal(IDigest digest, byte[] input)
		{
			digest.BlockUpdate(input, 0, input.Length);
			return DigestUtilities.DoFinal(digest);
		}

		// Token: 0x040018DF RID: 6367
		private static readonly IDictionary algorithms = Platform.CreateHashtable();

		// Token: 0x040018E0 RID: 6368
		private static readonly IDictionary oids = Platform.CreateHashtable();

		// Token: 0x02000906 RID: 2310
		private enum DigestAlgorithm
		{
			// Token: 0x0400351A RID: 13594
			BLAKE2B_160,
			// Token: 0x0400351B RID: 13595
			BLAKE2B_256,
			// Token: 0x0400351C RID: 13596
			BLAKE2B_384,
			// Token: 0x0400351D RID: 13597
			BLAKE2B_512,
			// Token: 0x0400351E RID: 13598
			BLAKE2S_128,
			// Token: 0x0400351F RID: 13599
			BLAKE2S_160,
			// Token: 0x04003520 RID: 13600
			BLAKE2S_224,
			// Token: 0x04003521 RID: 13601
			BLAKE2S_256,
			// Token: 0x04003522 RID: 13602
			DSTU7564_256,
			// Token: 0x04003523 RID: 13603
			DSTU7564_384,
			// Token: 0x04003524 RID: 13604
			DSTU7564_512,
			// Token: 0x04003525 RID: 13605
			GOST3411,
			// Token: 0x04003526 RID: 13606
			GOST3411_2012_256,
			// Token: 0x04003527 RID: 13607
			GOST3411_2012_512,
			// Token: 0x04003528 RID: 13608
			KECCAK_224,
			// Token: 0x04003529 RID: 13609
			KECCAK_256,
			// Token: 0x0400352A RID: 13610
			KECCAK_288,
			// Token: 0x0400352B RID: 13611
			KECCAK_384,
			// Token: 0x0400352C RID: 13612
			KECCAK_512,
			// Token: 0x0400352D RID: 13613
			MD2,
			// Token: 0x0400352E RID: 13614
			MD4,
			// Token: 0x0400352F RID: 13615
			MD5,
			// Token: 0x04003530 RID: 13616
			NONE,
			// Token: 0x04003531 RID: 13617
			RIPEMD128,
			// Token: 0x04003532 RID: 13618
			RIPEMD160,
			// Token: 0x04003533 RID: 13619
			RIPEMD256,
			// Token: 0x04003534 RID: 13620
			RIPEMD320,
			// Token: 0x04003535 RID: 13621
			SHA_1,
			// Token: 0x04003536 RID: 13622
			SHA_224,
			// Token: 0x04003537 RID: 13623
			SHA_256,
			// Token: 0x04003538 RID: 13624
			SHA_384,
			// Token: 0x04003539 RID: 13625
			SHA_512,
			// Token: 0x0400353A RID: 13626
			SHA_512_224,
			// Token: 0x0400353B RID: 13627
			SHA_512_256,
			// Token: 0x0400353C RID: 13628
			SHA3_224,
			// Token: 0x0400353D RID: 13629
			SHA3_256,
			// Token: 0x0400353E RID: 13630
			SHA3_384,
			// Token: 0x0400353F RID: 13631
			SHA3_512,
			// Token: 0x04003540 RID: 13632
			SHAKE128,
			// Token: 0x04003541 RID: 13633
			SHAKE256,
			// Token: 0x04003542 RID: 13634
			SM3,
			// Token: 0x04003543 RID: 13635
			TIGER,
			// Token: 0x04003544 RID: 13636
			WHIRLPOOL
		}
	}
}
