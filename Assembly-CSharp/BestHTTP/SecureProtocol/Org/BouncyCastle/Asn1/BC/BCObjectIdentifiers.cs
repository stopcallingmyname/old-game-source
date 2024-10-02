using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.BC
{
	// Token: 0x020007D2 RID: 2002
	public abstract class BCObjectIdentifiers
	{
		// Token: 0x04002E7D RID: 11901
		public static readonly DerObjectIdentifier bc = new DerObjectIdentifier("1.3.6.1.4.1.22554");

		// Token: 0x04002E7E RID: 11902
		public static readonly DerObjectIdentifier bc_pbe = BCObjectIdentifiers.bc.Branch("1");

		// Token: 0x04002E7F RID: 11903
		public static readonly DerObjectIdentifier bc_pbe_sha1 = BCObjectIdentifiers.bc_pbe.Branch("1");

		// Token: 0x04002E80 RID: 11904
		public static readonly DerObjectIdentifier bc_pbe_sha256 = BCObjectIdentifiers.bc_pbe.Branch("2.1");

		// Token: 0x04002E81 RID: 11905
		public static readonly DerObjectIdentifier bc_pbe_sha384 = BCObjectIdentifiers.bc_pbe.Branch("2.2");

		// Token: 0x04002E82 RID: 11906
		public static readonly DerObjectIdentifier bc_pbe_sha512 = BCObjectIdentifiers.bc_pbe.Branch("2.3");

		// Token: 0x04002E83 RID: 11907
		public static readonly DerObjectIdentifier bc_pbe_sha224 = BCObjectIdentifiers.bc_pbe.Branch("2.4");

		// Token: 0x04002E84 RID: 11908
		public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs5 = BCObjectIdentifiers.bc_pbe_sha1.Branch("1");

		// Token: 0x04002E85 RID: 11909
		public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs12 = BCObjectIdentifiers.bc_pbe_sha1.Branch("2");

		// Token: 0x04002E86 RID: 11910
		public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs5 = BCObjectIdentifiers.bc_pbe_sha256.Branch("1");

		// Token: 0x04002E87 RID: 11911
		public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs12 = BCObjectIdentifiers.bc_pbe_sha256.Branch("2");

		// Token: 0x04002E88 RID: 11912
		public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs12_aes128_cbc = BCObjectIdentifiers.bc_pbe_sha1_pkcs12.Branch("1.2");

		// Token: 0x04002E89 RID: 11913
		public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs12_aes192_cbc = BCObjectIdentifiers.bc_pbe_sha1_pkcs12.Branch("1.22");

		// Token: 0x04002E8A RID: 11914
		public static readonly DerObjectIdentifier bc_pbe_sha1_pkcs12_aes256_cbc = BCObjectIdentifiers.bc_pbe_sha1_pkcs12.Branch("1.42");

		// Token: 0x04002E8B RID: 11915
		public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs12_aes128_cbc = BCObjectIdentifiers.bc_pbe_sha256_pkcs12.Branch("1.2");

		// Token: 0x04002E8C RID: 11916
		public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs12_aes192_cbc = BCObjectIdentifiers.bc_pbe_sha256_pkcs12.Branch("1.22");

		// Token: 0x04002E8D RID: 11917
		public static readonly DerObjectIdentifier bc_pbe_sha256_pkcs12_aes256_cbc = BCObjectIdentifiers.bc_pbe_sha256_pkcs12.Branch("1.42");

		// Token: 0x04002E8E RID: 11918
		public static readonly DerObjectIdentifier bc_sig = BCObjectIdentifiers.bc.Branch("2");

		// Token: 0x04002E8F RID: 11919
		public static readonly DerObjectIdentifier sphincs256 = BCObjectIdentifiers.bc_sig.Branch("1");

		// Token: 0x04002E90 RID: 11920
		public static readonly DerObjectIdentifier sphincs256_with_BLAKE512 = BCObjectIdentifiers.sphincs256.Branch("1");

		// Token: 0x04002E91 RID: 11921
		public static readonly DerObjectIdentifier sphincs256_with_SHA512 = BCObjectIdentifiers.sphincs256.Branch("2");

		// Token: 0x04002E92 RID: 11922
		public static readonly DerObjectIdentifier sphincs256_with_SHA3_512 = BCObjectIdentifiers.sphincs256.Branch("3");

		// Token: 0x04002E93 RID: 11923
		public static readonly DerObjectIdentifier xmss = BCObjectIdentifiers.bc_sig.Branch("2");

		// Token: 0x04002E94 RID: 11924
		public static readonly DerObjectIdentifier xmss_with_SHA256 = BCObjectIdentifiers.xmss.Branch("1");

		// Token: 0x04002E95 RID: 11925
		public static readonly DerObjectIdentifier xmss_with_SHA512 = BCObjectIdentifiers.xmss.Branch("2");

		// Token: 0x04002E96 RID: 11926
		public static readonly DerObjectIdentifier xmss_with_SHAKE128 = BCObjectIdentifiers.xmss.Branch("3");

		// Token: 0x04002E97 RID: 11927
		public static readonly DerObjectIdentifier xmss_with_SHAKE256 = BCObjectIdentifiers.xmss.Branch("4");

		// Token: 0x04002E98 RID: 11928
		public static readonly DerObjectIdentifier xmss_mt = BCObjectIdentifiers.bc_sig.Branch("3");

		// Token: 0x04002E99 RID: 11929
		public static readonly DerObjectIdentifier xmss_mt_with_SHA256 = BCObjectIdentifiers.xmss_mt.Branch("1");

		// Token: 0x04002E9A RID: 11930
		public static readonly DerObjectIdentifier xmss_mt_with_SHA512 = BCObjectIdentifiers.xmss_mt.Branch("2");

		// Token: 0x04002E9B RID: 11931
		public static readonly DerObjectIdentifier xmss_mt_with_SHAKE128 = BCObjectIdentifiers.xmss_mt.Branch("3");

		// Token: 0x04002E9C RID: 11932
		public static readonly DerObjectIdentifier xmss_mt_with_SHAKE256 = BCObjectIdentifiers.xmss_mt.Branch("4");

		// Token: 0x04002E9D RID: 11933
		public static readonly DerObjectIdentifier bc_exch = BCObjectIdentifiers.bc.Branch("3");

		// Token: 0x04002E9E RID: 11934
		public static readonly DerObjectIdentifier newHope = BCObjectIdentifiers.bc_exch.Branch("1");
	}
}
