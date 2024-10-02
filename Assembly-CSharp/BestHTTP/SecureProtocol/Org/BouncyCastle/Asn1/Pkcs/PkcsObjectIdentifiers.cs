using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006FC RID: 1788
	public abstract class PkcsObjectIdentifiers
	{
		// Token: 0x040029D5 RID: 10709
		public const string Pkcs1 = "1.2.840.113549.1.1";

		// Token: 0x040029D6 RID: 10710
		internal static readonly DerObjectIdentifier Pkcs1Oid = new DerObjectIdentifier("1.2.840.113549.1.1");

		// Token: 0x040029D7 RID: 10711
		public static readonly DerObjectIdentifier RsaEncryption = PkcsObjectIdentifiers.Pkcs1Oid.Branch("1");

		// Token: 0x040029D8 RID: 10712
		public static readonly DerObjectIdentifier MD2WithRsaEncryption = PkcsObjectIdentifiers.Pkcs1Oid.Branch("2");

		// Token: 0x040029D9 RID: 10713
		public static readonly DerObjectIdentifier MD4WithRsaEncryption = PkcsObjectIdentifiers.Pkcs1Oid.Branch("3");

		// Token: 0x040029DA RID: 10714
		public static readonly DerObjectIdentifier MD5WithRsaEncryption = PkcsObjectIdentifiers.Pkcs1Oid.Branch("4");

		// Token: 0x040029DB RID: 10715
		public static readonly DerObjectIdentifier Sha1WithRsaEncryption = PkcsObjectIdentifiers.Pkcs1Oid.Branch("5");

		// Token: 0x040029DC RID: 10716
		public static readonly DerObjectIdentifier SrsaOaepEncryptionSet = PkcsObjectIdentifiers.Pkcs1Oid.Branch("6");

		// Token: 0x040029DD RID: 10717
		public static readonly DerObjectIdentifier IdRsaesOaep = PkcsObjectIdentifiers.Pkcs1Oid.Branch("7");

		// Token: 0x040029DE RID: 10718
		public static readonly DerObjectIdentifier IdMgf1 = PkcsObjectIdentifiers.Pkcs1Oid.Branch("8");

		// Token: 0x040029DF RID: 10719
		public static readonly DerObjectIdentifier IdPSpecified = PkcsObjectIdentifiers.Pkcs1Oid.Branch("9");

		// Token: 0x040029E0 RID: 10720
		public static readonly DerObjectIdentifier IdRsassaPss = PkcsObjectIdentifiers.Pkcs1Oid.Branch("10");

		// Token: 0x040029E1 RID: 10721
		public static readonly DerObjectIdentifier Sha256WithRsaEncryption = PkcsObjectIdentifiers.Pkcs1Oid.Branch("11");

		// Token: 0x040029E2 RID: 10722
		public static readonly DerObjectIdentifier Sha384WithRsaEncryption = PkcsObjectIdentifiers.Pkcs1Oid.Branch("12");

		// Token: 0x040029E3 RID: 10723
		public static readonly DerObjectIdentifier Sha512WithRsaEncryption = PkcsObjectIdentifiers.Pkcs1Oid.Branch("13");

		// Token: 0x040029E4 RID: 10724
		public static readonly DerObjectIdentifier Sha224WithRsaEncryption = PkcsObjectIdentifiers.Pkcs1Oid.Branch("14");

		// Token: 0x040029E5 RID: 10725
		public static readonly DerObjectIdentifier Sha512_224WithRSAEncryption = PkcsObjectIdentifiers.Pkcs1Oid.Branch("15");

		// Token: 0x040029E6 RID: 10726
		public static readonly DerObjectIdentifier Sha512_256WithRSAEncryption = PkcsObjectIdentifiers.Pkcs1Oid.Branch("16");

		// Token: 0x040029E7 RID: 10727
		public const string Pkcs3 = "1.2.840.113549.1.3";

		// Token: 0x040029E8 RID: 10728
		public static readonly DerObjectIdentifier DhKeyAgreement = new DerObjectIdentifier("1.2.840.113549.1.3.1");

		// Token: 0x040029E9 RID: 10729
		public const string Pkcs5 = "1.2.840.113549.1.5";

		// Token: 0x040029EA RID: 10730
		public static readonly DerObjectIdentifier PbeWithMD2AndDesCbc = new DerObjectIdentifier("1.2.840.113549.1.5.1");

		// Token: 0x040029EB RID: 10731
		public static readonly DerObjectIdentifier PbeWithMD2AndRC2Cbc = new DerObjectIdentifier("1.2.840.113549.1.5.4");

		// Token: 0x040029EC RID: 10732
		public static readonly DerObjectIdentifier PbeWithMD5AndDesCbc = new DerObjectIdentifier("1.2.840.113549.1.5.3");

		// Token: 0x040029ED RID: 10733
		public static readonly DerObjectIdentifier PbeWithMD5AndRC2Cbc = new DerObjectIdentifier("1.2.840.113549.1.5.6");

		// Token: 0x040029EE RID: 10734
		public static readonly DerObjectIdentifier PbeWithSha1AndDesCbc = new DerObjectIdentifier("1.2.840.113549.1.5.10");

		// Token: 0x040029EF RID: 10735
		public static readonly DerObjectIdentifier PbeWithSha1AndRC2Cbc = new DerObjectIdentifier("1.2.840.113549.1.5.11");

		// Token: 0x040029F0 RID: 10736
		public static readonly DerObjectIdentifier IdPbeS2 = new DerObjectIdentifier("1.2.840.113549.1.5.13");

		// Token: 0x040029F1 RID: 10737
		public static readonly DerObjectIdentifier IdPbkdf2 = new DerObjectIdentifier("1.2.840.113549.1.5.12");

		// Token: 0x040029F2 RID: 10738
		public const string EncryptionAlgorithm = "1.2.840.113549.3";

		// Token: 0x040029F3 RID: 10739
		public static readonly DerObjectIdentifier DesEde3Cbc = new DerObjectIdentifier("1.2.840.113549.3.7");

		// Token: 0x040029F4 RID: 10740
		public static readonly DerObjectIdentifier RC2Cbc = new DerObjectIdentifier("1.2.840.113549.3.2");

		// Token: 0x040029F5 RID: 10741
		public const string DigestAlgorithm = "1.2.840.113549.2";

		// Token: 0x040029F6 RID: 10742
		public static readonly DerObjectIdentifier MD2 = new DerObjectIdentifier("1.2.840.113549.2.2");

		// Token: 0x040029F7 RID: 10743
		public static readonly DerObjectIdentifier MD4 = new DerObjectIdentifier("1.2.840.113549.2.4");

		// Token: 0x040029F8 RID: 10744
		public static readonly DerObjectIdentifier MD5 = new DerObjectIdentifier("1.2.840.113549.2.5");

		// Token: 0x040029F9 RID: 10745
		public static readonly DerObjectIdentifier IdHmacWithSha1 = new DerObjectIdentifier("1.2.840.113549.2.7");

		// Token: 0x040029FA RID: 10746
		public static readonly DerObjectIdentifier IdHmacWithSha224 = new DerObjectIdentifier("1.2.840.113549.2.8");

		// Token: 0x040029FB RID: 10747
		public static readonly DerObjectIdentifier IdHmacWithSha256 = new DerObjectIdentifier("1.2.840.113549.2.9");

		// Token: 0x040029FC RID: 10748
		public static readonly DerObjectIdentifier IdHmacWithSha384 = new DerObjectIdentifier("1.2.840.113549.2.10");

		// Token: 0x040029FD RID: 10749
		public static readonly DerObjectIdentifier IdHmacWithSha512 = new DerObjectIdentifier("1.2.840.113549.2.11");

		// Token: 0x040029FE RID: 10750
		public const string Pkcs7 = "1.2.840.113549.1.7";

		// Token: 0x040029FF RID: 10751
		public static readonly DerObjectIdentifier Data = new DerObjectIdentifier("1.2.840.113549.1.7.1");

		// Token: 0x04002A00 RID: 10752
		public static readonly DerObjectIdentifier SignedData = new DerObjectIdentifier("1.2.840.113549.1.7.2");

		// Token: 0x04002A01 RID: 10753
		public static readonly DerObjectIdentifier EnvelopedData = new DerObjectIdentifier("1.2.840.113549.1.7.3");

		// Token: 0x04002A02 RID: 10754
		public static readonly DerObjectIdentifier SignedAndEnvelopedData = new DerObjectIdentifier("1.2.840.113549.1.7.4");

		// Token: 0x04002A03 RID: 10755
		public static readonly DerObjectIdentifier DigestedData = new DerObjectIdentifier("1.2.840.113549.1.7.5");

		// Token: 0x04002A04 RID: 10756
		public static readonly DerObjectIdentifier EncryptedData = new DerObjectIdentifier("1.2.840.113549.1.7.6");

		// Token: 0x04002A05 RID: 10757
		public const string Pkcs9 = "1.2.840.113549.1.9";

		// Token: 0x04002A06 RID: 10758
		public static readonly DerObjectIdentifier Pkcs9AtEmailAddress = new DerObjectIdentifier("1.2.840.113549.1.9.1");

		// Token: 0x04002A07 RID: 10759
		public static readonly DerObjectIdentifier Pkcs9AtUnstructuredName = new DerObjectIdentifier("1.2.840.113549.1.9.2");

		// Token: 0x04002A08 RID: 10760
		public static readonly DerObjectIdentifier Pkcs9AtContentType = new DerObjectIdentifier("1.2.840.113549.1.9.3");

		// Token: 0x04002A09 RID: 10761
		public static readonly DerObjectIdentifier Pkcs9AtMessageDigest = new DerObjectIdentifier("1.2.840.113549.1.9.4");

		// Token: 0x04002A0A RID: 10762
		public static readonly DerObjectIdentifier Pkcs9AtSigningTime = new DerObjectIdentifier("1.2.840.113549.1.9.5");

		// Token: 0x04002A0B RID: 10763
		public static readonly DerObjectIdentifier Pkcs9AtCounterSignature = new DerObjectIdentifier("1.2.840.113549.1.9.6");

		// Token: 0x04002A0C RID: 10764
		public static readonly DerObjectIdentifier Pkcs9AtChallengePassword = new DerObjectIdentifier("1.2.840.113549.1.9.7");

		// Token: 0x04002A0D RID: 10765
		public static readonly DerObjectIdentifier Pkcs9AtUnstructuredAddress = new DerObjectIdentifier("1.2.840.113549.1.9.8");

		// Token: 0x04002A0E RID: 10766
		public static readonly DerObjectIdentifier Pkcs9AtExtendedCertificateAttributes = new DerObjectIdentifier("1.2.840.113549.1.9.9");

		// Token: 0x04002A0F RID: 10767
		public static readonly DerObjectIdentifier Pkcs9AtSigningDescription = new DerObjectIdentifier("1.2.840.113549.1.9.13");

		// Token: 0x04002A10 RID: 10768
		public static readonly DerObjectIdentifier Pkcs9AtExtensionRequest = new DerObjectIdentifier("1.2.840.113549.1.9.14");

		// Token: 0x04002A11 RID: 10769
		public static readonly DerObjectIdentifier Pkcs9AtSmimeCapabilities = new DerObjectIdentifier("1.2.840.113549.1.9.15");

		// Token: 0x04002A12 RID: 10770
		public static readonly DerObjectIdentifier IdSmime = new DerObjectIdentifier("1.2.840.113549.1.9.16");

		// Token: 0x04002A13 RID: 10771
		public static readonly DerObjectIdentifier Pkcs9AtFriendlyName = new DerObjectIdentifier("1.2.840.113549.1.9.20");

		// Token: 0x04002A14 RID: 10772
		public static readonly DerObjectIdentifier Pkcs9AtLocalKeyID = new DerObjectIdentifier("1.2.840.113549.1.9.21");

		// Token: 0x04002A15 RID: 10773
		[Obsolete("Use X509Certificate instead")]
		public static readonly DerObjectIdentifier X509CertType = new DerObjectIdentifier("1.2.840.113549.1.9.22.1");

		// Token: 0x04002A16 RID: 10774
		public const string CertTypes = "1.2.840.113549.1.9.22";

		// Token: 0x04002A17 RID: 10775
		public static readonly DerObjectIdentifier X509Certificate = new DerObjectIdentifier("1.2.840.113549.1.9.22.1");

		// Token: 0x04002A18 RID: 10776
		public static readonly DerObjectIdentifier SdsiCertificate = new DerObjectIdentifier("1.2.840.113549.1.9.22.2");

		// Token: 0x04002A19 RID: 10777
		public const string CrlTypes = "1.2.840.113549.1.9.23";

		// Token: 0x04002A1A RID: 10778
		public static readonly DerObjectIdentifier X509Crl = new DerObjectIdentifier("1.2.840.113549.1.9.23.1");

		// Token: 0x04002A1B RID: 10779
		public static readonly DerObjectIdentifier IdAlg = PkcsObjectIdentifiers.IdSmime.Branch("3");

		// Token: 0x04002A1C RID: 10780
		public static readonly DerObjectIdentifier IdAlgEsdh = PkcsObjectIdentifiers.IdAlg.Branch("5");

		// Token: 0x04002A1D RID: 10781
		public static readonly DerObjectIdentifier IdAlgCms3DesWrap = PkcsObjectIdentifiers.IdAlg.Branch("6");

		// Token: 0x04002A1E RID: 10782
		public static readonly DerObjectIdentifier IdAlgCmsRC2Wrap = PkcsObjectIdentifiers.IdAlg.Branch("7");

		// Token: 0x04002A1F RID: 10783
		public static readonly DerObjectIdentifier IdAlgPwriKek = PkcsObjectIdentifiers.IdAlg.Branch("9");

		// Token: 0x04002A20 RID: 10784
		public static readonly DerObjectIdentifier IdAlgSsdh = PkcsObjectIdentifiers.IdAlg.Branch("10");

		// Token: 0x04002A21 RID: 10785
		public static readonly DerObjectIdentifier IdRsaKem = PkcsObjectIdentifiers.IdAlg.Branch("14");

		// Token: 0x04002A22 RID: 10786
		public static readonly DerObjectIdentifier PreferSignedData = PkcsObjectIdentifiers.Pkcs9AtSmimeCapabilities.Branch("1");

		// Token: 0x04002A23 RID: 10787
		public static readonly DerObjectIdentifier CannotDecryptAny = PkcsObjectIdentifiers.Pkcs9AtSmimeCapabilities.Branch("2");

		// Token: 0x04002A24 RID: 10788
		public static readonly DerObjectIdentifier SmimeCapabilitiesVersions = PkcsObjectIdentifiers.Pkcs9AtSmimeCapabilities.Branch("3");

		// Token: 0x04002A25 RID: 10789
		public static readonly DerObjectIdentifier IdAAReceiptRequest = PkcsObjectIdentifiers.IdSmime.Branch("2.1");

		// Token: 0x04002A26 RID: 10790
		public const string IdCT = "1.2.840.113549.1.9.16.1";

		// Token: 0x04002A27 RID: 10791
		public static readonly DerObjectIdentifier IdCTAuthData = new DerObjectIdentifier("1.2.840.113549.1.9.16.1.2");

		// Token: 0x04002A28 RID: 10792
		public static readonly DerObjectIdentifier IdCTTstInfo = new DerObjectIdentifier("1.2.840.113549.1.9.16.1.4");

		// Token: 0x04002A29 RID: 10793
		public static readonly DerObjectIdentifier IdCTCompressedData = new DerObjectIdentifier("1.2.840.113549.1.9.16.1.9");

		// Token: 0x04002A2A RID: 10794
		public static readonly DerObjectIdentifier IdCTAuthEnvelopedData = new DerObjectIdentifier("1.2.840.113549.1.9.16.1.23");

		// Token: 0x04002A2B RID: 10795
		public static readonly DerObjectIdentifier IdCTTimestampedData = new DerObjectIdentifier("1.2.840.113549.1.9.16.1.31");

		// Token: 0x04002A2C RID: 10796
		public const string IdCti = "1.2.840.113549.1.9.16.6";

		// Token: 0x04002A2D RID: 10797
		public static readonly DerObjectIdentifier IdCtiEtsProofOfOrigin = new DerObjectIdentifier("1.2.840.113549.1.9.16.6.1");

		// Token: 0x04002A2E RID: 10798
		public static readonly DerObjectIdentifier IdCtiEtsProofOfReceipt = new DerObjectIdentifier("1.2.840.113549.1.9.16.6.2");

		// Token: 0x04002A2F RID: 10799
		public static readonly DerObjectIdentifier IdCtiEtsProofOfDelivery = new DerObjectIdentifier("1.2.840.113549.1.9.16.6.3");

		// Token: 0x04002A30 RID: 10800
		public static readonly DerObjectIdentifier IdCtiEtsProofOfSender = new DerObjectIdentifier("1.2.840.113549.1.9.16.6.4");

		// Token: 0x04002A31 RID: 10801
		public static readonly DerObjectIdentifier IdCtiEtsProofOfApproval = new DerObjectIdentifier("1.2.840.113549.1.9.16.6.5");

		// Token: 0x04002A32 RID: 10802
		public static readonly DerObjectIdentifier IdCtiEtsProofOfCreation = new DerObjectIdentifier("1.2.840.113549.1.9.16.6.6");

		// Token: 0x04002A33 RID: 10803
		public const string IdAA = "1.2.840.113549.1.9.16.2";

		// Token: 0x04002A34 RID: 10804
		public static readonly DerObjectIdentifier IdAAOid = new DerObjectIdentifier("1.2.840.113549.1.9.16.2");

		// Token: 0x04002A35 RID: 10805
		public static readonly DerObjectIdentifier IdAAContentHint = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.4");

		// Token: 0x04002A36 RID: 10806
		public static readonly DerObjectIdentifier IdAAMsgSigDigest = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.5");

		// Token: 0x04002A37 RID: 10807
		public static readonly DerObjectIdentifier IdAAContentReference = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.10");

		// Token: 0x04002A38 RID: 10808
		public static readonly DerObjectIdentifier IdAAEncrypKeyPref = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.11");

		// Token: 0x04002A39 RID: 10809
		public static readonly DerObjectIdentifier IdAASigningCertificate = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.12");

		// Token: 0x04002A3A RID: 10810
		public static readonly DerObjectIdentifier IdAASigningCertificateV2 = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.47");

		// Token: 0x04002A3B RID: 10811
		public static readonly DerObjectIdentifier IdAAContentIdentifier = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.7");

		// Token: 0x04002A3C RID: 10812
		public static readonly DerObjectIdentifier IdAASignatureTimeStampToken = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.14");

		// Token: 0x04002A3D RID: 10813
		public static readonly DerObjectIdentifier IdAAEtsSigPolicyID = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.15");

		// Token: 0x04002A3E RID: 10814
		public static readonly DerObjectIdentifier IdAAEtsCommitmentType = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.16");

		// Token: 0x04002A3F RID: 10815
		public static readonly DerObjectIdentifier IdAAEtsSignerLocation = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.17");

		// Token: 0x04002A40 RID: 10816
		public static readonly DerObjectIdentifier IdAAEtsSignerAttr = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.18");

		// Token: 0x04002A41 RID: 10817
		public static readonly DerObjectIdentifier IdAAEtsOtherSigCert = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.19");

		// Token: 0x04002A42 RID: 10818
		public static readonly DerObjectIdentifier IdAAEtsContentTimestamp = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.20");

		// Token: 0x04002A43 RID: 10819
		public static readonly DerObjectIdentifier IdAAEtsCertificateRefs = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.21");

		// Token: 0x04002A44 RID: 10820
		public static readonly DerObjectIdentifier IdAAEtsRevocationRefs = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.22");

		// Token: 0x04002A45 RID: 10821
		public static readonly DerObjectIdentifier IdAAEtsCertValues = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.23");

		// Token: 0x04002A46 RID: 10822
		public static readonly DerObjectIdentifier IdAAEtsRevocationValues = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.24");

		// Token: 0x04002A47 RID: 10823
		public static readonly DerObjectIdentifier IdAAEtsEscTimeStamp = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.25");

		// Token: 0x04002A48 RID: 10824
		public static readonly DerObjectIdentifier IdAAEtsCertCrlTimestamp = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.26");

		// Token: 0x04002A49 RID: 10825
		public static readonly DerObjectIdentifier IdAAEtsArchiveTimestamp = new DerObjectIdentifier("1.2.840.113549.1.9.16.2.27");

		// Token: 0x04002A4A RID: 10826
		public static readonly DerObjectIdentifier IdAADecryptKeyID = PkcsObjectIdentifiers.IdAAOid.Branch("37");

		// Token: 0x04002A4B RID: 10827
		public static readonly DerObjectIdentifier IdAAImplCryptoAlgs = PkcsObjectIdentifiers.IdAAOid.Branch("38");

		// Token: 0x04002A4C RID: 10828
		public static readonly DerObjectIdentifier IdAAAsymmDecryptKeyID = PkcsObjectIdentifiers.IdAAOid.Branch("54");

		// Token: 0x04002A4D RID: 10829
		public static readonly DerObjectIdentifier IdAAImplCompressAlgs = PkcsObjectIdentifiers.IdAAOid.Branch("43");

		// Token: 0x04002A4E RID: 10830
		public static readonly DerObjectIdentifier IdAACommunityIdentifiers = PkcsObjectIdentifiers.IdAAOid.Branch("40");

		// Token: 0x04002A4F RID: 10831
		[Obsolete("Use 'IdAAEtsSigPolicyID' instead")]
		public static readonly DerObjectIdentifier IdAASigPolicyID = PkcsObjectIdentifiers.IdAAEtsSigPolicyID;

		// Token: 0x04002A50 RID: 10832
		[Obsolete("Use 'IdAAEtsCommitmentType' instead")]
		public static readonly DerObjectIdentifier IdAACommitmentType = PkcsObjectIdentifiers.IdAAEtsCommitmentType;

		// Token: 0x04002A51 RID: 10833
		[Obsolete("Use 'IdAAEtsSignerLocation' instead")]
		public static readonly DerObjectIdentifier IdAASignerLocation = PkcsObjectIdentifiers.IdAAEtsSignerLocation;

		// Token: 0x04002A52 RID: 10834
		[Obsolete("Use 'IdAAEtsOtherSigCert' instead")]
		public static readonly DerObjectIdentifier IdAAOtherSigCert = PkcsObjectIdentifiers.IdAAEtsOtherSigCert;

		// Token: 0x04002A53 RID: 10835
		public const string IdSpq = "1.2.840.113549.1.9.16.5";

		// Token: 0x04002A54 RID: 10836
		public static readonly DerObjectIdentifier IdSpqEtsUri = new DerObjectIdentifier("1.2.840.113549.1.9.16.5.1");

		// Token: 0x04002A55 RID: 10837
		public static readonly DerObjectIdentifier IdSpqEtsUNotice = new DerObjectIdentifier("1.2.840.113549.1.9.16.5.2");

		// Token: 0x04002A56 RID: 10838
		public const string Pkcs12 = "1.2.840.113549.1.12";

		// Token: 0x04002A57 RID: 10839
		public const string BagTypes = "1.2.840.113549.1.12.10.1";

		// Token: 0x04002A58 RID: 10840
		public static readonly DerObjectIdentifier KeyBag = new DerObjectIdentifier("1.2.840.113549.1.12.10.1.1");

		// Token: 0x04002A59 RID: 10841
		public static readonly DerObjectIdentifier Pkcs8ShroudedKeyBag = new DerObjectIdentifier("1.2.840.113549.1.12.10.1.2");

		// Token: 0x04002A5A RID: 10842
		public static readonly DerObjectIdentifier CertBag = new DerObjectIdentifier("1.2.840.113549.1.12.10.1.3");

		// Token: 0x04002A5B RID: 10843
		public static readonly DerObjectIdentifier CrlBag = new DerObjectIdentifier("1.2.840.113549.1.12.10.1.4");

		// Token: 0x04002A5C RID: 10844
		public static readonly DerObjectIdentifier SecretBag = new DerObjectIdentifier("1.2.840.113549.1.12.10.1.5");

		// Token: 0x04002A5D RID: 10845
		public static readonly DerObjectIdentifier SafeContentsBag = new DerObjectIdentifier("1.2.840.113549.1.12.10.1.6");

		// Token: 0x04002A5E RID: 10846
		public const string Pkcs12PbeIds = "1.2.840.113549.1.12.1";

		// Token: 0x04002A5F RID: 10847
		public static readonly DerObjectIdentifier PbeWithShaAnd128BitRC4 = new DerObjectIdentifier("1.2.840.113549.1.12.1.1");

		// Token: 0x04002A60 RID: 10848
		public static readonly DerObjectIdentifier PbeWithShaAnd40BitRC4 = new DerObjectIdentifier("1.2.840.113549.1.12.1.2");

		// Token: 0x04002A61 RID: 10849
		public static readonly DerObjectIdentifier PbeWithShaAnd3KeyTripleDesCbc = new DerObjectIdentifier("1.2.840.113549.1.12.1.3");

		// Token: 0x04002A62 RID: 10850
		public static readonly DerObjectIdentifier PbeWithShaAnd2KeyTripleDesCbc = new DerObjectIdentifier("1.2.840.113549.1.12.1.4");

		// Token: 0x04002A63 RID: 10851
		public static readonly DerObjectIdentifier PbeWithShaAnd128BitRC2Cbc = new DerObjectIdentifier("1.2.840.113549.1.12.1.5");

		// Token: 0x04002A64 RID: 10852
		public static readonly DerObjectIdentifier PbewithShaAnd40BitRC2Cbc = new DerObjectIdentifier("1.2.840.113549.1.12.1.6");
	}
}
