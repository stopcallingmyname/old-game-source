using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist
{
	// Token: 0x0200071A RID: 1818
	public sealed class NistObjectIdentifiers
	{
		// Token: 0x0600424A RID: 16970 RVA: 0x00022F1F File Offset: 0x0002111F
		private NistObjectIdentifiers()
		{
		}

		// Token: 0x04002AE6 RID: 10982
		public static readonly DerObjectIdentifier NistAlgorithm = new DerObjectIdentifier("2.16.840.1.101.3.4");

		// Token: 0x04002AE7 RID: 10983
		public static readonly DerObjectIdentifier HashAlgs = NistObjectIdentifiers.NistAlgorithm.Branch("2");

		// Token: 0x04002AE8 RID: 10984
		public static readonly DerObjectIdentifier IdSha256 = NistObjectIdentifiers.HashAlgs.Branch("1");

		// Token: 0x04002AE9 RID: 10985
		public static readonly DerObjectIdentifier IdSha384 = NistObjectIdentifiers.HashAlgs.Branch("2");

		// Token: 0x04002AEA RID: 10986
		public static readonly DerObjectIdentifier IdSha512 = NistObjectIdentifiers.HashAlgs.Branch("3");

		// Token: 0x04002AEB RID: 10987
		public static readonly DerObjectIdentifier IdSha224 = NistObjectIdentifiers.HashAlgs.Branch("4");

		// Token: 0x04002AEC RID: 10988
		public static readonly DerObjectIdentifier IdSha512_224 = NistObjectIdentifiers.HashAlgs.Branch("5");

		// Token: 0x04002AED RID: 10989
		public static readonly DerObjectIdentifier IdSha512_256 = NistObjectIdentifiers.HashAlgs.Branch("6");

		// Token: 0x04002AEE RID: 10990
		public static readonly DerObjectIdentifier IdSha3_224 = NistObjectIdentifiers.HashAlgs.Branch("7");

		// Token: 0x04002AEF RID: 10991
		public static readonly DerObjectIdentifier IdSha3_256 = NistObjectIdentifiers.HashAlgs.Branch("8");

		// Token: 0x04002AF0 RID: 10992
		public static readonly DerObjectIdentifier IdSha3_384 = NistObjectIdentifiers.HashAlgs.Branch("9");

		// Token: 0x04002AF1 RID: 10993
		public static readonly DerObjectIdentifier IdSha3_512 = NistObjectIdentifiers.HashAlgs.Branch("10");

		// Token: 0x04002AF2 RID: 10994
		public static readonly DerObjectIdentifier IdShake128 = NistObjectIdentifiers.HashAlgs.Branch("11");

		// Token: 0x04002AF3 RID: 10995
		public static readonly DerObjectIdentifier IdShake256 = NistObjectIdentifiers.HashAlgs.Branch("12");

		// Token: 0x04002AF4 RID: 10996
		public static readonly DerObjectIdentifier IdHMacWithSha3_224 = NistObjectIdentifiers.HashAlgs.Branch("13");

		// Token: 0x04002AF5 RID: 10997
		public static readonly DerObjectIdentifier IdHMacWithSha3_256 = NistObjectIdentifiers.HashAlgs.Branch("14");

		// Token: 0x04002AF6 RID: 10998
		public static readonly DerObjectIdentifier IdHMacWithSha3_384 = NistObjectIdentifiers.HashAlgs.Branch("15");

		// Token: 0x04002AF7 RID: 10999
		public static readonly DerObjectIdentifier IdHMacWithSha3_512 = NistObjectIdentifiers.HashAlgs.Branch("16");

		// Token: 0x04002AF8 RID: 11000
		public static readonly DerObjectIdentifier Aes = new DerObjectIdentifier(NistObjectIdentifiers.NistAlgorithm + ".1");

		// Token: 0x04002AF9 RID: 11001
		public static readonly DerObjectIdentifier IdAes128Ecb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".1");

		// Token: 0x04002AFA RID: 11002
		public static readonly DerObjectIdentifier IdAes128Cbc = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".2");

		// Token: 0x04002AFB RID: 11003
		public static readonly DerObjectIdentifier IdAes128Ofb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".3");

		// Token: 0x04002AFC RID: 11004
		public static readonly DerObjectIdentifier IdAes128Cfb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".4");

		// Token: 0x04002AFD RID: 11005
		public static readonly DerObjectIdentifier IdAes128Wrap = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".5");

		// Token: 0x04002AFE RID: 11006
		public static readonly DerObjectIdentifier IdAes128Gcm = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".6");

		// Token: 0x04002AFF RID: 11007
		public static readonly DerObjectIdentifier IdAes128Ccm = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".7");

		// Token: 0x04002B00 RID: 11008
		public static readonly DerObjectIdentifier IdAes192Ecb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".21");

		// Token: 0x04002B01 RID: 11009
		public static readonly DerObjectIdentifier IdAes192Cbc = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".22");

		// Token: 0x04002B02 RID: 11010
		public static readonly DerObjectIdentifier IdAes192Ofb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".23");

		// Token: 0x04002B03 RID: 11011
		public static readonly DerObjectIdentifier IdAes192Cfb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".24");

		// Token: 0x04002B04 RID: 11012
		public static readonly DerObjectIdentifier IdAes192Wrap = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".25");

		// Token: 0x04002B05 RID: 11013
		public static readonly DerObjectIdentifier IdAes192Gcm = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".26");

		// Token: 0x04002B06 RID: 11014
		public static readonly DerObjectIdentifier IdAes192Ccm = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".27");

		// Token: 0x04002B07 RID: 11015
		public static readonly DerObjectIdentifier IdAes256Ecb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".41");

		// Token: 0x04002B08 RID: 11016
		public static readonly DerObjectIdentifier IdAes256Cbc = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".42");

		// Token: 0x04002B09 RID: 11017
		public static readonly DerObjectIdentifier IdAes256Ofb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".43");

		// Token: 0x04002B0A RID: 11018
		public static readonly DerObjectIdentifier IdAes256Cfb = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".44");

		// Token: 0x04002B0B RID: 11019
		public static readonly DerObjectIdentifier IdAes256Wrap = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".45");

		// Token: 0x04002B0C RID: 11020
		public static readonly DerObjectIdentifier IdAes256Gcm = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".46");

		// Token: 0x04002B0D RID: 11021
		public static readonly DerObjectIdentifier IdAes256Ccm = new DerObjectIdentifier(NistObjectIdentifiers.Aes + ".47");

		// Token: 0x04002B0E RID: 11022
		public static readonly DerObjectIdentifier IdDsaWithSha2 = new DerObjectIdentifier(NistObjectIdentifiers.NistAlgorithm + ".3");

		// Token: 0x04002B0F RID: 11023
		public static readonly DerObjectIdentifier DsaWithSha224 = new DerObjectIdentifier(NistObjectIdentifiers.IdDsaWithSha2 + ".1");

		// Token: 0x04002B10 RID: 11024
		public static readonly DerObjectIdentifier DsaWithSha256 = new DerObjectIdentifier(NistObjectIdentifiers.IdDsaWithSha2 + ".2");

		// Token: 0x04002B11 RID: 11025
		public static readonly DerObjectIdentifier DsaWithSha384 = new DerObjectIdentifier(NistObjectIdentifiers.IdDsaWithSha2 + ".3");

		// Token: 0x04002B12 RID: 11026
		public static readonly DerObjectIdentifier DsaWithSha512 = new DerObjectIdentifier(NistObjectIdentifiers.IdDsaWithSha2 + ".4");
	}
}
