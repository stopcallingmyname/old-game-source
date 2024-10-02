using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.GM;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Sec;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Djb;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.GM;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Endo;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.EC
{
	// Token: 0x020005A1 RID: 1441
	public sealed class CustomNamedCurves
	{
		// Token: 0x0600369F RID: 13983 RVA: 0x00022F1F File Offset: 0x0002111F
		private CustomNamedCurves()
		{
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x001168E2 File Offset: 0x00114AE2
		private static BigInteger FromHex(string hex)
		{
			return new BigInteger(1, Hex.Decode(hex));
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x000947CE File Offset: 0x000929CE
		private static ECCurve ConfigureCurve(ECCurve curve)
		{
			return curve;
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x001521F2 File Offset: 0x001503F2
		private static ECCurve ConfigureCurveGlv(ECCurve c, GlvTypeBParameters p)
		{
			return c.Configure().SetEndomorphism(new GlvTypeBEndomorphism(c, p)).Create();
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x0015220B File Offset: 0x0015040B
		private static void DefineCurve(string name, X9ECParametersHolder holder)
		{
			CustomNamedCurves.names.Add(name);
			name = Platform.ToUpperInvariant(name);
			CustomNamedCurves.nameToCurve.Add(name, holder);
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x00152230 File Offset: 0x00150430
		private static void DefineCurveWithOid(string name, DerObjectIdentifier oid, X9ECParametersHolder holder)
		{
			CustomNamedCurves.names.Add(name);
			CustomNamedCurves.oidToName.Add(oid, name);
			CustomNamedCurves.oidToCurve.Add(oid, holder);
			name = Platform.ToUpperInvariant(name);
			CustomNamedCurves.nameToOid.Add(name, oid);
			CustomNamedCurves.nameToCurve.Add(name, holder);
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x00152284 File Offset: 0x00150484
		private static void DefineCurveAlias(string name, DerObjectIdentifier oid)
		{
			object obj = CustomNamedCurves.oidToCurve[oid];
			if (obj == null)
			{
				throw new InvalidOperationException();
			}
			name = Platform.ToUpperInvariant(name);
			CustomNamedCurves.nameToOid.Add(name, oid);
			CustomNamedCurves.nameToCurve.Add(name, obj);
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x001522C8 File Offset: 0x001504C8
		static CustomNamedCurves()
		{
			CustomNamedCurves.DefineCurve("curve25519", CustomNamedCurves.Curve25519Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("secp128r1", SecObjectIdentifiers.SecP128r1, CustomNamedCurves.SecP128R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("secp160k1", SecObjectIdentifiers.SecP160k1, CustomNamedCurves.SecP160K1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("secp160r1", SecObjectIdentifiers.SecP160r1, CustomNamedCurves.SecP160R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("secp160r2", SecObjectIdentifiers.SecP160r2, CustomNamedCurves.SecP160R2Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("secp192k1", SecObjectIdentifiers.SecP192k1, CustomNamedCurves.SecP192K1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("secp192r1", SecObjectIdentifiers.SecP192r1, CustomNamedCurves.SecP192R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("secp224k1", SecObjectIdentifiers.SecP224k1, CustomNamedCurves.SecP224K1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("secp224r1", SecObjectIdentifiers.SecP224r1, CustomNamedCurves.SecP224R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("secp256k1", SecObjectIdentifiers.SecP256k1, CustomNamedCurves.SecP256K1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("secp256r1", SecObjectIdentifiers.SecP256r1, CustomNamedCurves.SecP256R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("secp384r1", SecObjectIdentifiers.SecP384r1, CustomNamedCurves.SecP384R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("secp521r1", SecObjectIdentifiers.SecP521r1, CustomNamedCurves.SecP521R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect113r1", SecObjectIdentifiers.SecT113r1, CustomNamedCurves.SecT113R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect113r2", SecObjectIdentifiers.SecT113r2, CustomNamedCurves.SecT113R2Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect131r1", SecObjectIdentifiers.SecT131r1, CustomNamedCurves.SecT131R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect131r2", SecObjectIdentifiers.SecT131r2, CustomNamedCurves.SecT131R2Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect163k1", SecObjectIdentifiers.SecT163k1, CustomNamedCurves.SecT163K1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect163r1", SecObjectIdentifiers.SecT163r1, CustomNamedCurves.SecT163R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect163r2", SecObjectIdentifiers.SecT163r2, CustomNamedCurves.SecT163R2Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect193r1", SecObjectIdentifiers.SecT193r1, CustomNamedCurves.SecT193R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect193r2", SecObjectIdentifiers.SecT193r2, CustomNamedCurves.SecT193R2Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect233k1", SecObjectIdentifiers.SecT233k1, CustomNamedCurves.SecT233K1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect233r1", SecObjectIdentifiers.SecT233r1, CustomNamedCurves.SecT233R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect239k1", SecObjectIdentifiers.SecT239k1, CustomNamedCurves.SecT239K1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect283k1", SecObjectIdentifiers.SecT283k1, CustomNamedCurves.SecT283K1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect283r1", SecObjectIdentifiers.SecT283r1, CustomNamedCurves.SecT283R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect409k1", SecObjectIdentifiers.SecT409k1, CustomNamedCurves.SecT409K1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect409r1", SecObjectIdentifiers.SecT409r1, CustomNamedCurves.SecT409R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect571k1", SecObjectIdentifiers.SecT571k1, CustomNamedCurves.SecT571K1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sect571r1", SecObjectIdentifiers.SecT571r1, CustomNamedCurves.SecT571R1Holder.Instance);
			CustomNamedCurves.DefineCurveWithOid("sm2p256v1", GMObjectIdentifiers.sm2p256v1, CustomNamedCurves.SM2P256V1Holder.Instance);
			CustomNamedCurves.DefineCurveAlias("B-163", SecObjectIdentifiers.SecT163r2);
			CustomNamedCurves.DefineCurveAlias("B-233", SecObjectIdentifiers.SecT233r1);
			CustomNamedCurves.DefineCurveAlias("B-283", SecObjectIdentifiers.SecT283r1);
			CustomNamedCurves.DefineCurveAlias("B-409", SecObjectIdentifiers.SecT409r1);
			CustomNamedCurves.DefineCurveAlias("B-571", SecObjectIdentifiers.SecT571r1);
			CustomNamedCurves.DefineCurveAlias("K-163", SecObjectIdentifiers.SecT163k1);
			CustomNamedCurves.DefineCurveAlias("K-233", SecObjectIdentifiers.SecT233k1);
			CustomNamedCurves.DefineCurveAlias("K-283", SecObjectIdentifiers.SecT283k1);
			CustomNamedCurves.DefineCurveAlias("K-409", SecObjectIdentifiers.SecT409k1);
			CustomNamedCurves.DefineCurveAlias("K-571", SecObjectIdentifiers.SecT571k1);
			CustomNamedCurves.DefineCurveAlias("P-192", SecObjectIdentifiers.SecP192r1);
			CustomNamedCurves.DefineCurveAlias("P-224", SecObjectIdentifiers.SecP224r1);
			CustomNamedCurves.DefineCurveAlias("P-256", SecObjectIdentifiers.SecP256r1);
			CustomNamedCurves.DefineCurveAlias("P-384", SecObjectIdentifiers.SecP384r1);
			CustomNamedCurves.DefineCurveAlias("P-521", SecObjectIdentifiers.SecP521r1);
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x00152664 File Offset: 0x00150864
		public static X9ECParameters GetByName(string name)
		{
			X9ECParametersHolder x9ECParametersHolder = (X9ECParametersHolder)CustomNamedCurves.nameToCurve[Platform.ToUpperInvariant(name)];
			if (x9ECParametersHolder != null)
			{
				return x9ECParametersHolder.Parameters;
			}
			return null;
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x00152694 File Offset: 0x00150894
		public static X9ECParameters GetByOid(DerObjectIdentifier oid)
		{
			X9ECParametersHolder x9ECParametersHolder = (X9ECParametersHolder)CustomNamedCurves.oidToCurve[oid];
			if (x9ECParametersHolder != null)
			{
				return x9ECParametersHolder.Parameters;
			}
			return null;
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x001526BD File Offset: 0x001508BD
		public static DerObjectIdentifier GetOid(string name)
		{
			return (DerObjectIdentifier)CustomNamedCurves.nameToOid[Platform.ToUpperInvariant(name)];
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x001526D4 File Offset: 0x001508D4
		public static string GetName(DerObjectIdentifier oid)
		{
			return (string)CustomNamedCurves.oidToName[oid];
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x060036AB RID: 13995 RVA: 0x001526E6 File Offset: 0x001508E6
		public static IEnumerable Names
		{
			get
			{
				return new EnumerableProxy(CustomNamedCurves.names);
			}
		}

		// Token: 0x040023B7 RID: 9143
		private static readonly IDictionary nameToCurve = Platform.CreateHashtable();

		// Token: 0x040023B8 RID: 9144
		private static readonly IDictionary nameToOid = Platform.CreateHashtable();

		// Token: 0x040023B9 RID: 9145
		private static readonly IDictionary oidToCurve = Platform.CreateHashtable();

		// Token: 0x040023BA RID: 9146
		private static readonly IDictionary oidToName = Platform.CreateHashtable();

		// Token: 0x040023BB RID: 9147
		private static readonly IList names = Platform.CreateArrayList();

		// Token: 0x0200095A RID: 2394
		internal class Curve25519Holder : X9ECParametersHolder
		{
			// Token: 0x06004F28 RID: 20264 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Curve25519Holder()
			{
			}

			// Token: 0x06004F29 RID: 20265 RVA: 0x001B5C08 File Offset: 0x001B3E08
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new Curve25519());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("042AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD245A20AE19A1B8A086B4E01EDD2C7748D14C923D4D7E6D7C61B229E9C5A27ECED3D9"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036AD RID: 13997
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.Curve25519Holder();
		}

		// Token: 0x0200095B RID: 2395
		internal class SecP128R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F2B RID: 20267 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecP128R1Holder()
			{
			}

			// Token: 0x06004F2C RID: 20268 RVA: 0x001B5C54 File Offset: 0x001B3E54
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("000E0D4D696E6768756151750CC03A4473D03679");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecP128R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04161FF7528B899B2D0C28607CA52C5B86CF5AC8395BAFEB13C02DA292DDED7A83"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036AE RID: 13998
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecP128R1Holder();
		}

		// Token: 0x0200095C RID: 2396
		internal class SecP160K1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F2E RID: 20270 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecP160K1Holder()
			{
			}

			// Token: 0x06004F2F RID: 20271 RVA: 0x001B5CA8 File Offset: 0x001B3EA8
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				GlvTypeBParameters p = new GlvTypeBParameters(new BigInteger("9ba48cba5ebcb9b6bd33b92830b2a2e0e192f10a", 16), new BigInteger("c39c6c3b3a36d7701b9c71a1f5804ae5d0003f4", 16), new BigInteger[]
				{
					new BigInteger("9162fbe73984472a0a9e", 16),
					new BigInteger("-96341f1138933bc2f505", 16)
				}, new BigInteger[]
				{
					new BigInteger("127971af8721782ecffa3", 16),
					new BigInteger("9162fbe73984472a0a9e", 16)
				}, new BigInteger("9162fbe73984472a0a9d0590", 16), new BigInteger("96341f1138933bc2f503fd44", 16), 176);
				ECCurve eccurve = CustomNamedCurves.ConfigureCurveGlv(new SecP160K1Curve(), p);
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("043B4C382CE37AA192A4019E763036F4F5DD4D7EBB938CF935318FDCED6BC28286531733C3F03C4FEE"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036AF RID: 13999
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecP160K1Holder();
		}

		// Token: 0x0200095D RID: 2397
		internal class SecP160R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F31 RID: 20273 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecP160R1Holder()
			{
			}

			// Token: 0x06004F32 RID: 20274 RVA: 0x001B5D78 File Offset: 0x001B3F78
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("1053CDE42C14D696E67687561517533BF3F83345");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecP160R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("044A96B5688EF573284664698968C38BB913CBFC8223A628553168947D59DCC912042351377AC5FB32"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036B0 RID: 14000
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecP160R1Holder();
		}

		// Token: 0x0200095E RID: 2398
		internal class SecP160R2Holder : X9ECParametersHolder
		{
			// Token: 0x06004F34 RID: 20276 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecP160R2Holder()
			{
			}

			// Token: 0x06004F35 RID: 20277 RVA: 0x001B5DCC File Offset: 0x001B3FCC
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("B99B99B099B323E02709A4D696E6768756151751");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecP160R2Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0452DCB034293A117E1F4FF11B30F7199D3144CE6DFEAFFEF2E331F296E071FA0DF9982CFEA7D43F2E"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036B1 RID: 14001
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecP160R2Holder();
		}

		// Token: 0x0200095F RID: 2399
		internal class SecP192K1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F37 RID: 20279 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecP192K1Holder()
			{
			}

			// Token: 0x06004F38 RID: 20280 RVA: 0x001B5E20 File Offset: 0x001B4020
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				GlvTypeBParameters p = new GlvTypeBParameters(new BigInteger("bb85691939b869c1d087f601554b96b80cb4f55b35f433c2", 16), new BigInteger("3d84f26c12238d7b4f3d516613c1759033b1a5800175d0b1", 16), new BigInteger[]
				{
					new BigInteger("71169be7330b3038edb025f1", 16),
					new BigInteger("-b3fb3400dec5c4adceb8655c", 16)
				}, new BigInteger[]
				{
					new BigInteger("12511cfe811d0f4e6bc688b4d", 16),
					new BigInteger("71169be7330b3038edb025f1", 16)
				}, new BigInteger("71169be7330b3038edb025f1d0f9", 16), new BigInteger("b3fb3400dec5c4adceb8655d4c94", 16), 208);
				ECCurve eccurve = CustomNamedCurves.ConfigureCurveGlv(new SecP192K1Curve(), p);
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04DB4FF10EC057E9AE26B07D0280B7F4341DA5D1B1EAE06C7D9B2F2F6D9C5628A7844163D015BE86344082AA88D95E2F9D"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036B2 RID: 14002
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecP192K1Holder();
		}

		// Token: 0x02000960 RID: 2400
		internal class SecP192R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F3A RID: 20282 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecP192R1Holder()
			{
			}

			// Token: 0x06004F3B RID: 20283 RVA: 0x001B5EF0 File Offset: 0x001B40F0
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("3045AE6FC8422F64ED579528D38120EAE12196D5");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecP192R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04188DA80EB03090F67CBF20EB43A18800F4FF0AFD82FF101207192B95FFC8DA78631011ED6B24CDD573F977A11E794811"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036B3 RID: 14003
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecP192R1Holder();
		}

		// Token: 0x02000961 RID: 2401
		internal class SecP224K1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F3D RID: 20285 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecP224K1Holder()
			{
			}

			// Token: 0x06004F3E RID: 20286 RVA: 0x001B5F44 File Offset: 0x001B4144
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				GlvTypeBParameters p = new GlvTypeBParameters(new BigInteger("fe0e87005b4e83761908c5131d552a850b3f58b749c37cf5b84d6768", 16), new BigInteger("60dcd2104c4cbc0be6eeefc2bdd610739ec34e317f9b33046c9e4788", 16), new BigInteger[]
				{
					new BigInteger("6b8cf07d4ca75c88957d9d670591", 16),
					new BigInteger("-b8adf1378a6eb73409fa6c9c637d", 16)
				}, new BigInteger[]
				{
					new BigInteger("1243ae1b4d71613bc9f780a03690e", 16),
					new BigInteger("6b8cf07d4ca75c88957d9d670591", 16)
				}, new BigInteger("6b8cf07d4ca75c88957d9d67059037a4", 16), new BigInteger("b8adf1378a6eb73409fa6c9c637ba7f5", 16), 240);
				ECCurve eccurve = CustomNamedCurves.ConfigureCurveGlv(new SecP224K1Curve(), p);
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04A1455B334DF099DF30FC28A169A467E9E47075A90F7E650EB6B7A45C7E089FED7FBA344282CAFBD6F7E319F7C0B0BD59E2CA4BDB556D61A5"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036B4 RID: 14004
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecP224K1Holder();
		}

		// Token: 0x02000962 RID: 2402
		internal class SecP224R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F40 RID: 20288 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecP224R1Holder()
			{
			}

			// Token: 0x06004F41 RID: 20289 RVA: 0x001B6014 File Offset: 0x001B4214
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("BD71344799D5C7FCDC45B59FA3B9AB8F6A948BC5");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecP224R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04B70E0CBD6BB4BF7F321390B94A03C1D356C21122343280D6115C1D21BD376388B5F723FB4C22DFE6CD4375A05A07476444D5819985007E34"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036B5 RID: 14005
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecP224R1Holder();
		}

		// Token: 0x02000963 RID: 2403
		internal class SecP256K1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F43 RID: 20291 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecP256K1Holder()
			{
			}

			// Token: 0x06004F44 RID: 20292 RVA: 0x001B6068 File Offset: 0x001B4268
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				GlvTypeBParameters p = new GlvTypeBParameters(new BigInteger("7ae96a2b657c07106e64479eac3434e99cf0497512f58995c1396c28719501ee", 16), new BigInteger("5363ad4cc05c30e0a5261c028812645a122e22ea20816678df02967c1b23bd72", 16), new BigInteger[]
				{
					new BigInteger("3086d221a7d46bcde86c90e49284eb15", 16),
					new BigInteger("-e4437ed6010e88286f547fa90abfe4c3", 16)
				}, new BigInteger[]
				{
					new BigInteger("114ca50f7a8e2f3f657c1108d9d44cfd8", 16),
					new BigInteger("3086d221a7d46bcde86c90e49284eb15", 16)
				}, new BigInteger("3086d221a7d46bcde86c90e49284eb153dab", 16), new BigInteger("e4437ed6010e88286f547fa90abfe4c42212", 16), 272);
				ECCurve eccurve = CustomNamedCurves.ConfigureCurveGlv(new SecP256K1Curve(), p);
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0479BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036B6 RID: 14006
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecP256K1Holder();
		}

		// Token: 0x02000964 RID: 2404
		internal class SecP256R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F46 RID: 20294 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecP256R1Holder()
			{
			}

			// Token: 0x06004F47 RID: 20295 RVA: 0x001B6138 File Offset: 0x001B4338
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("C49D360886E704936A6678E1139D26B7819F7E90");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecP256R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("046B17D1F2E12C4247F8BCE6E563A440F277037D812DEB33A0F4A13945D898C2964FE342E2FE1A7F9B8EE7EB4A7C0F9E162BCE33576B315ECECBB6406837BF51F5"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036B7 RID: 14007
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecP256R1Holder();
		}

		// Token: 0x02000965 RID: 2405
		internal class SecP384R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F49 RID: 20297 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecP384R1Holder()
			{
			}

			// Token: 0x06004F4A RID: 20298 RVA: 0x001B618C File Offset: 0x001B438C
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("A335926AA319A27A1D00896A6773A4827ACDAC73");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecP384R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04AA87CA22BE8B05378EB1C71EF320AD746E1D3B628BA79B9859F741E082542A385502F25DBF55296C3A545E3872760AB73617DE4A96262C6F5D9E98BF9292DC29F8F41DBD289A147CE9DA3113B5F0B8C00A60B1CE1D7E819D7A431D7C90EA0E5F"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036B8 RID: 14008
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecP384R1Holder();
		}

		// Token: 0x02000966 RID: 2406
		internal class SecP521R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F4C RID: 20300 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecP521R1Holder()
			{
			}

			// Token: 0x06004F4D RID: 20301 RVA: 0x001B61E0 File Offset: 0x001B43E0
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("D09E8800291CB85396CC6717393284AAA0DA64BA");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecP521R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0400C6858E06B70404E9CD9E3ECB662395B4429C648139053FB521F828AF606B4D3DBAA14B5E77EFE75928FE1DC127A2FFA8DE3348B3C1856A429BF97E7E31C2E5BD66011839296A789A3BC0045C8A5FB42C7D1BD998F54449579B446817AFBD17273E662C97EE72995EF42640C550B9013FAD0761353C7086A272C24088BE94769FD16650"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036B9 RID: 14009
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecP521R1Holder();
		}

		// Token: 0x02000967 RID: 2407
		internal class SecT113R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F4F RID: 20303 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT113R1Holder()
			{
			}

			// Token: 0x06004F50 RID: 20304 RVA: 0x001B6234 File Offset: 0x001B4434
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("10E723AB14D696E6768756151756FEBF8FCB49A9");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT113R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04009D73616F35F4AB1407D73562C10F00A52830277958EE84D1315ED31886"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036BA RID: 14010
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT113R1Holder();
		}

		// Token: 0x02000968 RID: 2408
		internal class SecT113R2Holder : X9ECParametersHolder
		{
			// Token: 0x06004F52 RID: 20306 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT113R2Holder()
			{
			}

			// Token: 0x06004F53 RID: 20307 RVA: 0x001B6288 File Offset: 0x001B4488
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("10C0FB15760860DEF1EEF4D696E676875615175D");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT113R2Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0401A57A6A7B26CA5EF52FCDB816479700B3ADC94ED1FE674C06E695BABA1D"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036BB RID: 14011
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT113R2Holder();
		}

		// Token: 0x02000969 RID: 2409
		internal class SecT131R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F55 RID: 20309 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT131R1Holder()
			{
			}

			// Token: 0x06004F56 RID: 20310 RVA: 0x001B62DC File Offset: 0x001B44DC
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("4D696E676875615175985BD3ADBADA21B43A97E2");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT131R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("040081BAF91FDF9833C40F9C181343638399078C6E7EA38C001F73C8134B1B4EF9E150"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036BC RID: 14012
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT131R1Holder();
		}

		// Token: 0x0200096A RID: 2410
		internal class SecT131R2Holder : X9ECParametersHolder
		{
			// Token: 0x06004F58 RID: 20312 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT131R2Holder()
			{
			}

			// Token: 0x06004F59 RID: 20313 RVA: 0x001B6330 File Offset: 0x001B4530
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("985BD3ADBAD4D696E676875615175A21B43A97E3");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT131R2Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("040356DCD8F2F95031AD652D23951BB366A80648F06D867940A5366D9E265DE9EB240F"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036BD RID: 14013
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT131R2Holder();
		}

		// Token: 0x0200096B RID: 2411
		internal class SecT163K1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F5B RID: 20315 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT163K1Holder()
			{
			}

			// Token: 0x06004F5C RID: 20316 RVA: 0x001B6384 File Offset: 0x001B4584
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT163K1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0402FE13C0537BBC11ACAA07D793DE4E6D5E5C94EEE80289070FB05D38FF58321F2E800536D538CCDAA3D9"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036BE RID: 14014
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT163K1Holder();
		}

		// Token: 0x0200096C RID: 2412
		internal class SecT163R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F5E RID: 20318 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT163R1Holder()
			{
			}

			// Token: 0x06004F5F RID: 20319 RVA: 0x001B63D0 File Offset: 0x001B45D0
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("24B7B137C8A14D696E6768756151756FD0DA2E5C");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT163R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("040369979697AB43897789566789567F787A7876A65400435EDB42EFAFB2989D51FEFCE3C80988F41FF883"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036BF RID: 14015
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT163R1Holder();
		}

		// Token: 0x0200096D RID: 2413
		internal class SecT163R2Holder : X9ECParametersHolder
		{
			// Token: 0x06004F61 RID: 20321 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT163R2Holder()
			{
			}

			// Token: 0x06004F62 RID: 20322 RVA: 0x001B6424 File Offset: 0x001B4624
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("85E25BFE5C86226CDB12016F7553F9D0E693A268");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT163R2Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0403F0EBA16286A2D57EA0991168D4994637E8343E3600D51FBC6C71A0094FA2CDD545B11C5C0C797324F1"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036C0 RID: 14016
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT163R2Holder();
		}

		// Token: 0x0200096E RID: 2414
		internal class SecT193R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F64 RID: 20324 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT193R1Holder()
			{
			}

			// Token: 0x06004F65 RID: 20325 RVA: 0x001B6478 File Offset: 0x001B4678
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("103FAEC74D696E676875615175777FC5B191EF30");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT193R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0401F481BC5F0FF84A74AD6CDF6FDEF4BF6179625372D8C0C5E10025E399F2903712CCF3EA9E3A1AD17FB0B3201B6AF7CE1B05"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036C1 RID: 14017
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT193R1Holder();
		}

		// Token: 0x0200096F RID: 2415
		internal class SecT193R2Holder : X9ECParametersHolder
		{
			// Token: 0x06004F67 RID: 20327 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT193R2Holder()
			{
			}

			// Token: 0x06004F68 RID: 20328 RVA: 0x001B64CC File Offset: 0x001B46CC
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("10B7B4D696E676875615175137C8A16FD0DA2211");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT193R2Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0400D9B67D192E0367C803F39E1A7E82CA14A651350AAE617E8F01CE94335607C304AC29E7DEFBD9CA01F596F927224CDECF6C"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036C2 RID: 14018
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT193R2Holder();
		}

		// Token: 0x02000970 RID: 2416
		internal class SecT233K1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F6A RID: 20330 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT233K1Holder()
			{
			}

			// Token: 0x06004F6B RID: 20331 RVA: 0x001B6520 File Offset: 0x001B4720
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT233K1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04017232BA853A7E731AF129F22FF4149563A419C26BF50A4C9D6EEFAD612601DB537DECE819B7F70F555A67C427A8CD9BF18AEB9B56E0C11056FAE6A3"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036C3 RID: 14019
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT233K1Holder();
		}

		// Token: 0x02000971 RID: 2417
		internal class SecT233R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F6D RID: 20333 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT233R1Holder()
			{
			}

			// Token: 0x06004F6E RID: 20334 RVA: 0x001B656C File Offset: 0x001B476C
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("74D59FF07F6B413D0EA14B344B20A2DB049B50C3");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT233R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0400FAC9DFCBAC8313BB2139F1BB755FEF65BC391F8B36F8F8EB7371FD558B01006A08A41903350678E58528BEBF8A0BEFF867A7CA36716F7E01F81052"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036C4 RID: 14020
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT233R1Holder();
		}

		// Token: 0x02000972 RID: 2418
		internal class SecT239K1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F70 RID: 20336 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT239K1Holder()
			{
			}

			// Token: 0x06004F71 RID: 20337 RVA: 0x001B65C0 File Offset: 0x001B47C0
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT239K1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0429A0B6A887A983E9730988A68727A8B2D126C44CC2CC7B2A6555193035DC76310804F12E549BDB011C103089E73510ACB275FC312A5DC6B76553F0CA"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036C5 RID: 14021
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT239K1Holder();
		}

		// Token: 0x02000973 RID: 2419
		internal class SecT283K1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F73 RID: 20339 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT283K1Holder()
			{
			}

			// Token: 0x06004F74 RID: 20340 RVA: 0x001B660C File Offset: 0x001B480C
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT283K1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("040503213F78CA44883F1A3B8162F188E553CD265F23C1567A16876913B0C2AC245849283601CCDA380F1C9E318D90F95D07E5426FE87E45C0E8184698E45962364E34116177DD2259"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036C6 RID: 14022
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT283K1Holder();
		}

		// Token: 0x02000974 RID: 2420
		internal class SecT283R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F76 RID: 20342 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT283R1Holder()
			{
			}

			// Token: 0x06004F77 RID: 20343 RVA: 0x001B6658 File Offset: 0x001B4858
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("77E2B07370EB0F832A6DD5B62DFC88CD06BB84BE");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT283R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0405F939258DB7DD90E1934F8C70B0DFEC2EED25B8557EAC9C80E2E198F8CDBECD86B1205303676854FE24141CB98FE6D4B20D02B4516FF702350EDDB0826779C813F0DF45BE8112F4"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036C7 RID: 14023
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT283R1Holder();
		}

		// Token: 0x02000975 RID: 2421
		internal class SecT409K1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F79 RID: 20345 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT409K1Holder()
			{
			}

			// Token: 0x06004F7A RID: 20346 RVA: 0x001B66AC File Offset: 0x001B48AC
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT409K1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("040060F05F658F49C1AD3AB1890F7184210EFD0987E307C84C27ACCFB8F9F67CC2C460189EB5AAAA62EE222EB1B35540CFE902374601E369050B7C4E42ACBA1DACBF04299C3460782F918EA427E6325165E9EA10E3DA5F6C42E9C55215AA9CA27A5863EC48D8E0286B"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036C8 RID: 14024
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT409K1Holder();
		}

		// Token: 0x02000976 RID: 2422
		internal class SecT409R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F7C RID: 20348 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT409R1Holder()
			{
			}

			// Token: 0x06004F7D RID: 20349 RVA: 0x001B66F8 File Offset: 0x001B48F8
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("4099B5A457F9D69F79213D094C4BCD4D4262210B");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT409R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04015D4860D088DDB3496B0C6064756260441CDE4AF1771D4DB01FFE5B34E59703DC255A868A1180515603AEAB60794E54BB7996A70061B1CFAB6BE5F32BBFA78324ED106A7636B9C5A7BD198D0158AA4F5488D08F38514F1FDF4B4F40D2181B3681C364BA0273C706"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036C9 RID: 14025
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT409R1Holder();
		}

		// Token: 0x02000977 RID: 2423
		internal class SecT571K1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F7F RID: 20351 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT571K1Holder()
			{
			}

			// Token: 0x06004F80 RID: 20352 RVA: 0x001B674C File Offset: 0x001B494C
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT571K1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04026EB7A859923FBC82189631F8103FE4AC9CA2970012D5D46024804801841CA44370958493B205E647DA304DB4CEB08CBBD1BA39494776FB988B47174DCA88C7E2945283A01C89720349DC807F4FBF374F4AEADE3BCA95314DD58CEC9F307A54FFC61EFC006D8A2C9D4979C0AC44AEA74FBEBBB9F772AEDCB620B01A7BA7AF1B320430C8591984F601CD4C143EF1C7A3"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036CA RID: 14026
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT571K1Holder();
		}

		// Token: 0x02000978 RID: 2424
		internal class SecT571R1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F82 RID: 20354 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SecT571R1Holder()
			{
			}

			// Token: 0x06004F83 RID: 20355 RVA: 0x001B6798 File Offset: 0x001B4998
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("2AA058F73A0E33AB486B0F610410C53A7F132310");
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SecT571R1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("040303001D34B856296C16C0D40D3CD7750A93D1D2955FA80AA5F40FC8DB7B2ABDBDE53950F4C0D293CDD711A35B67FB1499AE60038614F1394ABFA3B4C850D927E1E7769C8EEC2D19037BF27342DA639B6DCCFFFEB73D69D78C6C27A6009CBBCA1980F8533921E8A684423E43BAB08A576291AF8F461BB2A8B3531D2F0485C19B16E2F1516E23DD3C1A4827AF1B8AC15B"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036CB RID: 14027
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SecT571R1Holder();
		}

		// Token: 0x02000979 RID: 2425
		internal class SM2P256V1Holder : X9ECParametersHolder
		{
			// Token: 0x06004F85 RID: 20357 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SM2P256V1Holder()
			{
			}

			// Token: 0x06004F86 RID: 20358 RVA: 0x001B67EC File Offset: 0x001B49EC
			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				ECCurve eccurve = CustomNamedCurves.ConfigureCurve(new SM2P256V1Curve());
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0432C4AE2C1F1981195F9904466A39C9948FE30BBFF2660BE1715A4589334C74C7BC3736A2F4F6779C59BDCEE36B692153D0A9877CC62A474002DF32E52139F0A0"));
				return new X9ECParameters(eccurve, g, eccurve.Order, eccurve.Cofactor, seed);
			}

			// Token: 0x040036CC RID: 14028
			internal static readonly X9ECParametersHolder Instance = new CustomNamedCurves.SM2P256V1Holder();
		}
	}
}
