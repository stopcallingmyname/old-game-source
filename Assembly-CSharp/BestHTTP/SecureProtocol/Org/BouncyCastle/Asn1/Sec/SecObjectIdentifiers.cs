using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Sec
{
	// Token: 0x020006E8 RID: 1768
	public abstract class SecObjectIdentifiers
	{
		// Token: 0x04002979 RID: 10617
		public static readonly DerObjectIdentifier EllipticCurve = new DerObjectIdentifier("1.3.132.0");

		// Token: 0x0400297A RID: 10618
		public static readonly DerObjectIdentifier SecT163k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".1");

		// Token: 0x0400297B RID: 10619
		public static readonly DerObjectIdentifier SecT163r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".2");

		// Token: 0x0400297C RID: 10620
		public static readonly DerObjectIdentifier SecT239k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".3");

		// Token: 0x0400297D RID: 10621
		public static readonly DerObjectIdentifier SecT113r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".4");

		// Token: 0x0400297E RID: 10622
		public static readonly DerObjectIdentifier SecT113r2 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".5");

		// Token: 0x0400297F RID: 10623
		public static readonly DerObjectIdentifier SecP112r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".6");

		// Token: 0x04002980 RID: 10624
		public static readonly DerObjectIdentifier SecP112r2 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".7");

		// Token: 0x04002981 RID: 10625
		public static readonly DerObjectIdentifier SecP160r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".8");

		// Token: 0x04002982 RID: 10626
		public static readonly DerObjectIdentifier SecP160k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".9");

		// Token: 0x04002983 RID: 10627
		public static readonly DerObjectIdentifier SecP256k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".10");

		// Token: 0x04002984 RID: 10628
		public static readonly DerObjectIdentifier SecT163r2 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".15");

		// Token: 0x04002985 RID: 10629
		public static readonly DerObjectIdentifier SecT283k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".16");

		// Token: 0x04002986 RID: 10630
		public static readonly DerObjectIdentifier SecT283r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".17");

		// Token: 0x04002987 RID: 10631
		public static readonly DerObjectIdentifier SecT131r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".22");

		// Token: 0x04002988 RID: 10632
		public static readonly DerObjectIdentifier SecT131r2 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".23");

		// Token: 0x04002989 RID: 10633
		public static readonly DerObjectIdentifier SecT193r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".24");

		// Token: 0x0400298A RID: 10634
		public static readonly DerObjectIdentifier SecT193r2 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".25");

		// Token: 0x0400298B RID: 10635
		public static readonly DerObjectIdentifier SecT233k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".26");

		// Token: 0x0400298C RID: 10636
		public static readonly DerObjectIdentifier SecT233r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".27");

		// Token: 0x0400298D RID: 10637
		public static readonly DerObjectIdentifier SecP128r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".28");

		// Token: 0x0400298E RID: 10638
		public static readonly DerObjectIdentifier SecP128r2 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".29");

		// Token: 0x0400298F RID: 10639
		public static readonly DerObjectIdentifier SecP160r2 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".30");

		// Token: 0x04002990 RID: 10640
		public static readonly DerObjectIdentifier SecP192k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".31");

		// Token: 0x04002991 RID: 10641
		public static readonly DerObjectIdentifier SecP224k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".32");

		// Token: 0x04002992 RID: 10642
		public static readonly DerObjectIdentifier SecP224r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".33");

		// Token: 0x04002993 RID: 10643
		public static readonly DerObjectIdentifier SecP384r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".34");

		// Token: 0x04002994 RID: 10644
		public static readonly DerObjectIdentifier SecP521r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".35");

		// Token: 0x04002995 RID: 10645
		public static readonly DerObjectIdentifier SecT409k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".36");

		// Token: 0x04002996 RID: 10646
		public static readonly DerObjectIdentifier SecT409r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".37");

		// Token: 0x04002997 RID: 10647
		public static readonly DerObjectIdentifier SecT571k1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".38");

		// Token: 0x04002998 RID: 10648
		public static readonly DerObjectIdentifier SecT571r1 = new DerObjectIdentifier(SecObjectIdentifiers.EllipticCurve + ".39");

		// Token: 0x04002999 RID: 10649
		public static readonly DerObjectIdentifier SecP192r1 = X9ObjectIdentifiers.Prime192v1;

		// Token: 0x0400299A RID: 10650
		public static readonly DerObjectIdentifier SecP256r1 = X9ObjectIdentifiers.Prime256v1;
	}
}
