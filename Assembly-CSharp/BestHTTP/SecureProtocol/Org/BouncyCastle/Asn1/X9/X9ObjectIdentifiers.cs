using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000683 RID: 1667
	public abstract class X9ObjectIdentifiers
	{
		// Token: 0x04002730 RID: 10032
		internal const string AnsiX962 = "1.2.840.10045";

		// Token: 0x04002731 RID: 10033
		public static readonly DerObjectIdentifier ansi_X9_62 = new DerObjectIdentifier("1.2.840.10045");

		// Token: 0x04002732 RID: 10034
		public static readonly DerObjectIdentifier IdFieldType = X9ObjectIdentifiers.ansi_X9_62.Branch("1");

		// Token: 0x04002733 RID: 10035
		public static readonly DerObjectIdentifier PrimeField = X9ObjectIdentifiers.IdFieldType.Branch("1");

		// Token: 0x04002734 RID: 10036
		public static readonly DerObjectIdentifier CharacteristicTwoField = X9ObjectIdentifiers.IdFieldType.Branch("2");

		// Token: 0x04002735 RID: 10037
		public static readonly DerObjectIdentifier GNBasis = X9ObjectIdentifiers.CharacteristicTwoField.Branch("3.1");

		// Token: 0x04002736 RID: 10038
		public static readonly DerObjectIdentifier TPBasis = X9ObjectIdentifiers.CharacteristicTwoField.Branch("3.2");

		// Token: 0x04002737 RID: 10039
		public static readonly DerObjectIdentifier PPBasis = X9ObjectIdentifiers.CharacteristicTwoField.Branch("3.3");

		// Token: 0x04002738 RID: 10040
		[Obsolete("Use 'id_ecSigType' instead")]
		public const string IdECSigType = "1.2.840.10045.4";

		// Token: 0x04002739 RID: 10041
		public static readonly DerObjectIdentifier id_ecSigType = X9ObjectIdentifiers.ansi_X9_62.Branch("4");

		// Token: 0x0400273A RID: 10042
		public static readonly DerObjectIdentifier ECDsaWithSha1 = X9ObjectIdentifiers.id_ecSigType.Branch("1");

		// Token: 0x0400273B RID: 10043
		[Obsolete("Use 'id_publicKeyType' instead")]
		public const string IdPublicKeyType = "1.2.840.10045.2";

		// Token: 0x0400273C RID: 10044
		public static readonly DerObjectIdentifier id_publicKeyType = X9ObjectIdentifiers.ansi_X9_62.Branch("2");

		// Token: 0x0400273D RID: 10045
		public static readonly DerObjectIdentifier IdECPublicKey = X9ObjectIdentifiers.id_publicKeyType.Branch("1");

		// Token: 0x0400273E RID: 10046
		public static readonly DerObjectIdentifier ECDsaWithSha2 = X9ObjectIdentifiers.id_ecSigType.Branch("3");

		// Token: 0x0400273F RID: 10047
		public static readonly DerObjectIdentifier ECDsaWithSha224 = X9ObjectIdentifiers.ECDsaWithSha2.Branch("1");

		// Token: 0x04002740 RID: 10048
		public static readonly DerObjectIdentifier ECDsaWithSha256 = X9ObjectIdentifiers.ECDsaWithSha2.Branch("2");

		// Token: 0x04002741 RID: 10049
		public static readonly DerObjectIdentifier ECDsaWithSha384 = X9ObjectIdentifiers.ECDsaWithSha2.Branch("3");

		// Token: 0x04002742 RID: 10050
		public static readonly DerObjectIdentifier ECDsaWithSha512 = X9ObjectIdentifiers.ECDsaWithSha2.Branch("4");

		// Token: 0x04002743 RID: 10051
		public static readonly DerObjectIdentifier EllipticCurve = X9ObjectIdentifiers.ansi_X9_62.Branch("3");

		// Token: 0x04002744 RID: 10052
		public static readonly DerObjectIdentifier CTwoCurve = X9ObjectIdentifiers.EllipticCurve.Branch("0");

		// Token: 0x04002745 RID: 10053
		public static readonly DerObjectIdentifier C2Pnb163v1 = X9ObjectIdentifiers.CTwoCurve.Branch("1");

		// Token: 0x04002746 RID: 10054
		public static readonly DerObjectIdentifier C2Pnb163v2 = X9ObjectIdentifiers.CTwoCurve.Branch("2");

		// Token: 0x04002747 RID: 10055
		public static readonly DerObjectIdentifier C2Pnb163v3 = X9ObjectIdentifiers.CTwoCurve.Branch("3");

		// Token: 0x04002748 RID: 10056
		public static readonly DerObjectIdentifier C2Pnb176w1 = X9ObjectIdentifiers.CTwoCurve.Branch("4");

		// Token: 0x04002749 RID: 10057
		public static readonly DerObjectIdentifier C2Tnb191v1 = X9ObjectIdentifiers.CTwoCurve.Branch("5");

		// Token: 0x0400274A RID: 10058
		public static readonly DerObjectIdentifier C2Tnb191v2 = X9ObjectIdentifiers.CTwoCurve.Branch("6");

		// Token: 0x0400274B RID: 10059
		public static readonly DerObjectIdentifier C2Tnb191v3 = X9ObjectIdentifiers.CTwoCurve.Branch("7");

		// Token: 0x0400274C RID: 10060
		public static readonly DerObjectIdentifier C2Onb191v4 = X9ObjectIdentifiers.CTwoCurve.Branch("8");

		// Token: 0x0400274D RID: 10061
		public static readonly DerObjectIdentifier C2Onb191v5 = X9ObjectIdentifiers.CTwoCurve.Branch("9");

		// Token: 0x0400274E RID: 10062
		public static readonly DerObjectIdentifier C2Pnb208w1 = X9ObjectIdentifiers.CTwoCurve.Branch("10");

		// Token: 0x0400274F RID: 10063
		public static readonly DerObjectIdentifier C2Tnb239v1 = X9ObjectIdentifiers.CTwoCurve.Branch("11");

		// Token: 0x04002750 RID: 10064
		public static readonly DerObjectIdentifier C2Tnb239v2 = X9ObjectIdentifiers.CTwoCurve.Branch("12");

		// Token: 0x04002751 RID: 10065
		public static readonly DerObjectIdentifier C2Tnb239v3 = X9ObjectIdentifiers.CTwoCurve.Branch("13");

		// Token: 0x04002752 RID: 10066
		public static readonly DerObjectIdentifier C2Onb239v4 = X9ObjectIdentifiers.CTwoCurve.Branch("14");

		// Token: 0x04002753 RID: 10067
		public static readonly DerObjectIdentifier C2Onb239v5 = X9ObjectIdentifiers.CTwoCurve.Branch("15");

		// Token: 0x04002754 RID: 10068
		public static readonly DerObjectIdentifier C2Pnb272w1 = X9ObjectIdentifiers.CTwoCurve.Branch("16");

		// Token: 0x04002755 RID: 10069
		public static readonly DerObjectIdentifier C2Pnb304w1 = X9ObjectIdentifiers.CTwoCurve.Branch("17");

		// Token: 0x04002756 RID: 10070
		public static readonly DerObjectIdentifier C2Tnb359v1 = X9ObjectIdentifiers.CTwoCurve.Branch("18");

		// Token: 0x04002757 RID: 10071
		public static readonly DerObjectIdentifier C2Pnb368w1 = X9ObjectIdentifiers.CTwoCurve.Branch("19");

		// Token: 0x04002758 RID: 10072
		public static readonly DerObjectIdentifier C2Tnb431r1 = X9ObjectIdentifiers.CTwoCurve.Branch("20");

		// Token: 0x04002759 RID: 10073
		public static readonly DerObjectIdentifier PrimeCurve = X9ObjectIdentifiers.EllipticCurve.Branch("1");

		// Token: 0x0400275A RID: 10074
		public static readonly DerObjectIdentifier Prime192v1 = X9ObjectIdentifiers.PrimeCurve.Branch("1");

		// Token: 0x0400275B RID: 10075
		public static readonly DerObjectIdentifier Prime192v2 = X9ObjectIdentifiers.PrimeCurve.Branch("2");

		// Token: 0x0400275C RID: 10076
		public static readonly DerObjectIdentifier Prime192v3 = X9ObjectIdentifiers.PrimeCurve.Branch("3");

		// Token: 0x0400275D RID: 10077
		public static readonly DerObjectIdentifier Prime239v1 = X9ObjectIdentifiers.PrimeCurve.Branch("4");

		// Token: 0x0400275E RID: 10078
		public static readonly DerObjectIdentifier Prime239v2 = X9ObjectIdentifiers.PrimeCurve.Branch("5");

		// Token: 0x0400275F RID: 10079
		public static readonly DerObjectIdentifier Prime239v3 = X9ObjectIdentifiers.PrimeCurve.Branch("6");

		// Token: 0x04002760 RID: 10080
		public static readonly DerObjectIdentifier Prime256v1 = X9ObjectIdentifiers.PrimeCurve.Branch("7");

		// Token: 0x04002761 RID: 10081
		public static readonly DerObjectIdentifier IdDsa = new DerObjectIdentifier("1.2.840.10040.4.1");

		// Token: 0x04002762 RID: 10082
		public static readonly DerObjectIdentifier IdDsaWithSha1 = new DerObjectIdentifier("1.2.840.10040.4.3");

		// Token: 0x04002763 RID: 10083
		public static readonly DerObjectIdentifier X9x63Scheme = new DerObjectIdentifier("1.3.133.16.840.63.0");

		// Token: 0x04002764 RID: 10084
		public static readonly DerObjectIdentifier DHSinglePassStdDHSha1KdfScheme = X9ObjectIdentifiers.X9x63Scheme.Branch("2");

		// Token: 0x04002765 RID: 10085
		public static readonly DerObjectIdentifier DHSinglePassCofactorDHSha1KdfScheme = X9ObjectIdentifiers.X9x63Scheme.Branch("3");

		// Token: 0x04002766 RID: 10086
		public static readonly DerObjectIdentifier MqvSinglePassSha1KdfScheme = X9ObjectIdentifiers.X9x63Scheme.Branch("16");

		// Token: 0x04002767 RID: 10087
		public static readonly DerObjectIdentifier ansi_x9_42 = new DerObjectIdentifier("1.2.840.10046");

		// Token: 0x04002768 RID: 10088
		public static readonly DerObjectIdentifier DHPublicNumber = X9ObjectIdentifiers.ansi_x9_42.Branch("2.1");

		// Token: 0x04002769 RID: 10089
		public static readonly DerObjectIdentifier X9x42Schemes = X9ObjectIdentifiers.ansi_x9_42.Branch("2.3");

		// Token: 0x0400276A RID: 10090
		public static readonly DerObjectIdentifier DHStatic = X9ObjectIdentifiers.X9x42Schemes.Branch("1");

		// Token: 0x0400276B RID: 10091
		public static readonly DerObjectIdentifier DHEphem = X9ObjectIdentifiers.X9x42Schemes.Branch("2");

		// Token: 0x0400276C RID: 10092
		public static readonly DerObjectIdentifier DHOneFlow = X9ObjectIdentifiers.X9x42Schemes.Branch("3");

		// Token: 0x0400276D RID: 10093
		public static readonly DerObjectIdentifier DHHybrid1 = X9ObjectIdentifiers.X9x42Schemes.Branch("4");

		// Token: 0x0400276E RID: 10094
		public static readonly DerObjectIdentifier DHHybrid2 = X9ObjectIdentifiers.X9x42Schemes.Branch("5");

		// Token: 0x0400276F RID: 10095
		public static readonly DerObjectIdentifier DHHybridOneFlow = X9ObjectIdentifiers.X9x42Schemes.Branch("6");

		// Token: 0x04002770 RID: 10096
		public static readonly DerObjectIdentifier Mqv2 = X9ObjectIdentifiers.X9x42Schemes.Branch("7");

		// Token: 0x04002771 RID: 10097
		public static readonly DerObjectIdentifier Mqv1 = X9ObjectIdentifiers.X9x42Schemes.Branch("8");
	}
}
