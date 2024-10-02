using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Endo;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Sec
{
	// Token: 0x020006E7 RID: 1767
	public sealed class SecNamedCurves
	{
		// Token: 0x060040DE RID: 16606 RVA: 0x00022F1F File Offset: 0x0002111F
		private SecNamedCurves()
		{
		}

		// Token: 0x060040DF RID: 16607 RVA: 0x000947CE File Offset: 0x000929CE
		private static ECCurve ConfigureCurve(ECCurve curve)
		{
			return curve;
		}

		// Token: 0x060040E0 RID: 16608 RVA: 0x001521F2 File Offset: 0x001503F2
		private static ECCurve ConfigureCurveGlv(ECCurve c, GlvTypeBParameters p)
		{
			return c.Configure().SetEndomorphism(new GlvTypeBEndomorphism(c, p)).Create();
		}

		// Token: 0x060040E1 RID: 16609 RVA: 0x001168E2 File Offset: 0x00114AE2
		private static BigInteger FromHex(string hex)
		{
			return new BigInteger(1, Hex.Decode(hex));
		}

		// Token: 0x060040E2 RID: 16610 RVA: 0x00180B3C File Offset: 0x0017ED3C
		private static void DefineCurve(string name, DerObjectIdentifier oid, X9ECParametersHolder holder)
		{
			SecNamedCurves.objIds.Add(Platform.ToUpperInvariant(name), oid);
			SecNamedCurves.names.Add(oid, name);
			SecNamedCurves.curves.Add(oid, holder);
		}

		// Token: 0x060040E3 RID: 16611 RVA: 0x00180B68 File Offset: 0x0017ED68
		static SecNamedCurves()
		{
			SecNamedCurves.DefineCurve("secp112r1", SecObjectIdentifiers.SecP112r1, SecNamedCurves.Secp112r1Holder.Instance);
			SecNamedCurves.DefineCurve("secp112r2", SecObjectIdentifiers.SecP112r2, SecNamedCurves.Secp112r2Holder.Instance);
			SecNamedCurves.DefineCurve("secp128r1", SecObjectIdentifiers.SecP128r1, SecNamedCurves.Secp128r1Holder.Instance);
			SecNamedCurves.DefineCurve("secp128r2", SecObjectIdentifiers.SecP128r2, SecNamedCurves.Secp128r2Holder.Instance);
			SecNamedCurves.DefineCurve("secp160k1", SecObjectIdentifiers.SecP160k1, SecNamedCurves.Secp160k1Holder.Instance);
			SecNamedCurves.DefineCurve("secp160r1", SecObjectIdentifiers.SecP160r1, SecNamedCurves.Secp160r1Holder.Instance);
			SecNamedCurves.DefineCurve("secp160r2", SecObjectIdentifiers.SecP160r2, SecNamedCurves.Secp160r2Holder.Instance);
			SecNamedCurves.DefineCurve("secp192k1", SecObjectIdentifiers.SecP192k1, SecNamedCurves.Secp192k1Holder.Instance);
			SecNamedCurves.DefineCurve("secp192r1", SecObjectIdentifiers.SecP192r1, SecNamedCurves.Secp192r1Holder.Instance);
			SecNamedCurves.DefineCurve("secp224k1", SecObjectIdentifiers.SecP224k1, SecNamedCurves.Secp224k1Holder.Instance);
			SecNamedCurves.DefineCurve("secp224r1", SecObjectIdentifiers.SecP224r1, SecNamedCurves.Secp224r1Holder.Instance);
			SecNamedCurves.DefineCurve("secp256k1", SecObjectIdentifiers.SecP256k1, SecNamedCurves.Secp256k1Holder.Instance);
			SecNamedCurves.DefineCurve("secp256r1", SecObjectIdentifiers.SecP256r1, SecNamedCurves.Secp256r1Holder.Instance);
			SecNamedCurves.DefineCurve("secp384r1", SecObjectIdentifiers.SecP384r1, SecNamedCurves.Secp384r1Holder.Instance);
			SecNamedCurves.DefineCurve("secp521r1", SecObjectIdentifiers.SecP521r1, SecNamedCurves.Secp521r1Holder.Instance);
			SecNamedCurves.DefineCurve("sect113r1", SecObjectIdentifiers.SecT113r1, SecNamedCurves.Sect113r1Holder.Instance);
			SecNamedCurves.DefineCurve("sect113r2", SecObjectIdentifiers.SecT113r2, SecNamedCurves.Sect113r2Holder.Instance);
			SecNamedCurves.DefineCurve("sect131r1", SecObjectIdentifiers.SecT131r1, SecNamedCurves.Sect131r1Holder.Instance);
			SecNamedCurves.DefineCurve("sect131r2", SecObjectIdentifiers.SecT131r2, SecNamedCurves.Sect131r2Holder.Instance);
			SecNamedCurves.DefineCurve("sect163k1", SecObjectIdentifiers.SecT163k1, SecNamedCurves.Sect163k1Holder.Instance);
			SecNamedCurves.DefineCurve("sect163r1", SecObjectIdentifiers.SecT163r1, SecNamedCurves.Sect163r1Holder.Instance);
			SecNamedCurves.DefineCurve("sect163r2", SecObjectIdentifiers.SecT163r2, SecNamedCurves.Sect163r2Holder.Instance);
			SecNamedCurves.DefineCurve("sect193r1", SecObjectIdentifiers.SecT193r1, SecNamedCurves.Sect193r1Holder.Instance);
			SecNamedCurves.DefineCurve("sect193r2", SecObjectIdentifiers.SecT193r2, SecNamedCurves.Sect193r2Holder.Instance);
			SecNamedCurves.DefineCurve("sect233k1", SecObjectIdentifiers.SecT233k1, SecNamedCurves.Sect233k1Holder.Instance);
			SecNamedCurves.DefineCurve("sect233r1", SecObjectIdentifiers.SecT233r1, SecNamedCurves.Sect233r1Holder.Instance);
			SecNamedCurves.DefineCurve("sect239k1", SecObjectIdentifiers.SecT239k1, SecNamedCurves.Sect239k1Holder.Instance);
			SecNamedCurves.DefineCurve("sect283k1", SecObjectIdentifiers.SecT283k1, SecNamedCurves.Sect283k1Holder.Instance);
			SecNamedCurves.DefineCurve("sect283r1", SecObjectIdentifiers.SecT283r1, SecNamedCurves.Sect283r1Holder.Instance);
			SecNamedCurves.DefineCurve("sect409k1", SecObjectIdentifiers.SecT409k1, SecNamedCurves.Sect409k1Holder.Instance);
			SecNamedCurves.DefineCurve("sect409r1", SecObjectIdentifiers.SecT409r1, SecNamedCurves.Sect409r1Holder.Instance);
			SecNamedCurves.DefineCurve("sect571k1", SecObjectIdentifiers.SecT571k1, SecNamedCurves.Sect571k1Holder.Instance);
			SecNamedCurves.DefineCurve("sect571r1", SecObjectIdentifiers.SecT571r1, SecNamedCurves.Sect571r1Holder.Instance);
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x00180E28 File Offset: 0x0017F028
		public static X9ECParameters GetByName(string name)
		{
			DerObjectIdentifier oid = SecNamedCurves.GetOid(name);
			if (oid != null)
			{
				return SecNamedCurves.GetByOid(oid);
			}
			return null;
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x00180E48 File Offset: 0x0017F048
		public static X9ECParameters GetByOid(DerObjectIdentifier oid)
		{
			X9ECParametersHolder x9ECParametersHolder = (X9ECParametersHolder)SecNamedCurves.curves[oid];
			if (x9ECParametersHolder != null)
			{
				return x9ECParametersHolder.Parameters;
			}
			return null;
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x00180E71 File Offset: 0x0017F071
		public static DerObjectIdentifier GetOid(string name)
		{
			return (DerObjectIdentifier)SecNamedCurves.objIds[Platform.ToUpperInvariant(name)];
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x00180E88 File Offset: 0x0017F088
		public static string GetName(DerObjectIdentifier oid)
		{
			return (string)SecNamedCurves.names[oid];
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x060040E8 RID: 16616 RVA: 0x00180E9A File Offset: 0x0017F09A
		public static IEnumerable Names
		{
			get
			{
				return new EnumerableProxy(SecNamedCurves.names.Values);
			}
		}

		// Token: 0x04002976 RID: 10614
		private static readonly IDictionary objIds = Platform.CreateHashtable();

		// Token: 0x04002977 RID: 10615
		private static readonly IDictionary curves = Platform.CreateHashtable();

		// Token: 0x04002978 RID: 10616
		private static readonly IDictionary names = Platform.CreateHashtable();

		// Token: 0x020009B4 RID: 2484
		internal class Secp112r1Holder : X9ECParametersHolder
		{
			// Token: 0x06005047 RID: 20551 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp112r1Holder()
			{
			}

			// Token: 0x06005048 RID: 20552 RVA: 0x001B8F64 File Offset: 0x001B7164
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("DB7C2ABF62E35E668076BEAD208B");
				BigInteger a = SecNamedCurves.FromHex("DB7C2ABF62E35E668076BEAD2088");
				BigInteger b = SecNamedCurves.FromHex("659EF8BA043916EEDE8911702B22");
				byte[] seed = Hex.Decode("00F50B028E4D696E676875615175290472783FB1");
				BigInteger bigInteger = SecNamedCurves.FromHex("DB7C2ABF62E35E7628DFAC6561C5");
				BigInteger one = BigInteger.One;
				ECCurve eccurve = SecNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, one));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0409487239995A5EE76B55F9C2F098A89CE5AF8724C0A23E0E0FF77500"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x0400373A RID: 14138
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp112r1Holder();
		}

		// Token: 0x020009B5 RID: 2485
		internal class Secp112r2Holder : X9ECParametersHolder
		{
			// Token: 0x0600504A RID: 20554 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp112r2Holder()
			{
			}

			// Token: 0x0600504B RID: 20555 RVA: 0x001B8FE8 File Offset: 0x001B71E8
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("DB7C2ABF62E35E668076BEAD208B");
				BigInteger a = SecNamedCurves.FromHex("6127C24C05F38A0AAAF65C0EF02C");
				BigInteger b = SecNamedCurves.FromHex("51DEF1815DB5ED74FCC34C85D709");
				byte[] seed = Hex.Decode("002757A1114D696E6768756151755316C05E0BD4");
				BigInteger bigInteger = SecNamedCurves.FromHex("36DF0AAFD8B8D7597CA10520D04B");
				BigInteger bigInteger2 = BigInteger.ValueOf(4L);
				ECCurve eccurve = SecNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, bigInteger2));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("044BA30AB5E892B4E1649DD0928643ADCD46F5882E3747DEF36E956E97"));
				return new X9ECParameters(eccurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x0400373B RID: 14139
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp112r2Holder();
		}

		// Token: 0x020009B6 RID: 2486
		internal class Secp128r1Holder : X9ECParametersHolder
		{
			// Token: 0x0600504D RID: 20557 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp128r1Holder()
			{
			}

			// Token: 0x0600504E RID: 20558 RVA: 0x001B906C File Offset: 0x001B726C
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("FFFFFFFDFFFFFFFFFFFFFFFFFFFFFFFF");
				BigInteger a = SecNamedCurves.FromHex("FFFFFFFDFFFFFFFFFFFFFFFFFFFFFFFC");
				BigInteger b = SecNamedCurves.FromHex("E87579C11079F43DD824993C2CEE5ED3");
				byte[] seed = Hex.Decode("000E0D4D696E6768756151750CC03A4473D03679");
				BigInteger bigInteger = SecNamedCurves.FromHex("FFFFFFFE0000000075A30D1B9038A115");
				BigInteger one = BigInteger.One;
				ECCurve eccurve = SecNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, one));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04161FF7528B899B2D0C28607CA52C5B86CF5AC8395BAFEB13C02DA292DDED7A83"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x0400373C RID: 14140
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp128r1Holder();
		}

		// Token: 0x020009B7 RID: 2487
		internal class Secp128r2Holder : X9ECParametersHolder
		{
			// Token: 0x06005050 RID: 20560 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp128r2Holder()
			{
			}

			// Token: 0x06005051 RID: 20561 RVA: 0x001B90F0 File Offset: 0x001B72F0
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("FFFFFFFDFFFFFFFFFFFFFFFFFFFFFFFF");
				BigInteger a = SecNamedCurves.FromHex("D6031998D1B3BBFEBF59CC9BBFF9AEE1");
				BigInteger b = SecNamedCurves.FromHex("5EEEFCA380D02919DC2C6558BB6D8A5D");
				byte[] seed = Hex.Decode("004D696E67687561517512D8F03431FCE63B88F4");
				BigInteger bigInteger = SecNamedCurves.FromHex("3FFFFFFF7FFFFFFFBE0024720613B5A3");
				BigInteger bigInteger2 = BigInteger.ValueOf(4L);
				ECCurve eccurve = SecNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, bigInteger2));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("047B6AA5D85E572983E6FB32A7CDEBC14027B6916A894D3AEE7106FE805FC34B44"));
				return new X9ECParameters(eccurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x0400373D RID: 14141
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp128r2Holder();
		}

		// Token: 0x020009B8 RID: 2488
		internal class Secp160k1Holder : X9ECParametersHolder
		{
			// Token: 0x06005053 RID: 20563 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp160k1Holder()
			{
			}

			// Token: 0x06005054 RID: 20564 RVA: 0x001B9174 File Offset: 0x001B7374
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFAC73");
				BigInteger zero = BigInteger.Zero;
				BigInteger b = BigInteger.ValueOf(7L);
				byte[] seed = null;
				BigInteger bigInteger = SecNamedCurves.FromHex("0100000000000000000001B8FA16DFAB9ACA16B6B3");
				BigInteger one = BigInteger.One;
				GlvTypeBParameters p = new GlvTypeBParameters(new BigInteger("9ba48cba5ebcb9b6bd33b92830b2a2e0e192f10a", 16), new BigInteger("c39c6c3b3a36d7701b9c71a1f5804ae5d0003f4", 16), new BigInteger[]
				{
					new BigInteger("9162fbe73984472a0a9e", 16),
					new BigInteger("-96341f1138933bc2f505", 16)
				}, new BigInteger[]
				{
					new BigInteger("127971af8721782ecffa3", 16),
					new BigInteger("9162fbe73984472a0a9e", 16)
				}, new BigInteger("9162fbe73984472a0a9d0590", 16), new BigInteger("96341f1138933bc2f503fd44", 16), 176);
				ECCurve eccurve = SecNamedCurves.ConfigureCurveGlv(new FpCurve(q, zero, b, bigInteger, one), p);
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("043B4C382CE37AA192A4019E763036F4F5DD4D7EBB938CF935318FDCED6BC28286531733C3F03C4FEE"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x0400373E RID: 14142
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp160k1Holder();
		}

		// Token: 0x020009B9 RID: 2489
		internal class Secp160r1Holder : X9ECParametersHolder
		{
			// Token: 0x06005056 RID: 20566 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp160r1Holder()
			{
			}

			// Token: 0x06005057 RID: 20567 RVA: 0x001B9270 File Offset: 0x001B7470
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF7FFFFFFF");
				BigInteger a = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF7FFFFFFC");
				BigInteger b = SecNamedCurves.FromHex("1C97BEFC54BD7A8B65ACF89F81D4D4ADC565FA45");
				byte[] seed = Hex.Decode("1053CDE42C14D696E67687561517533BF3F83345");
				BigInteger bigInteger = SecNamedCurves.FromHex("0100000000000000000001F4C8F927AED3CA752257");
				BigInteger one = BigInteger.One;
				ECCurve eccurve = SecNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, one));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("044A96B5688EF573284664698968C38BB913CBFC8223A628553168947D59DCC912042351377AC5FB32"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x0400373F RID: 14143
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp160r1Holder();
		}

		// Token: 0x020009BA RID: 2490
		internal class Secp160r2Holder : X9ECParametersHolder
		{
			// Token: 0x06005059 RID: 20569 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp160r2Holder()
			{
			}

			// Token: 0x0600505A RID: 20570 RVA: 0x001B92F4 File Offset: 0x001B74F4
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFAC73");
				BigInteger a = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFAC70");
				BigInteger b = SecNamedCurves.FromHex("B4E134D3FB59EB8BAB57274904664D5AF50388BA");
				byte[] seed = Hex.Decode("B99B99B099B323E02709A4D696E6768756151751");
				BigInteger bigInteger = SecNamedCurves.FromHex("0100000000000000000000351EE786A818F3A1A16B");
				BigInteger one = BigInteger.One;
				ECCurve eccurve = SecNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, one));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0452DCB034293A117E1F4FF11B30F7199D3144CE6DFEAFFEF2E331F296E071FA0DF9982CFEA7D43F2E"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x04003740 RID: 14144
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp160r2Holder();
		}

		// Token: 0x020009BB RID: 2491
		internal class Secp192k1Holder : X9ECParametersHolder
		{
			// Token: 0x0600505C RID: 20572 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp192k1Holder()
			{
			}

			// Token: 0x0600505D RID: 20573 RVA: 0x001B9378 File Offset: 0x001B7578
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFEE37");
				BigInteger zero = BigInteger.Zero;
				BigInteger b = BigInteger.ValueOf(3L);
				byte[] seed = null;
				BigInteger bigInteger = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFE26F2FC170F69466A74DEFD8D");
				BigInteger one = BigInteger.One;
				GlvTypeBParameters p = new GlvTypeBParameters(new BigInteger("bb85691939b869c1d087f601554b96b80cb4f55b35f433c2", 16), new BigInteger("3d84f26c12238d7b4f3d516613c1759033b1a5800175d0b1", 16), new BigInteger[]
				{
					new BigInteger("71169be7330b3038edb025f1", 16),
					new BigInteger("-b3fb3400dec5c4adceb8655c", 16)
				}, new BigInteger[]
				{
					new BigInteger("12511cfe811d0f4e6bc688b4d", 16),
					new BigInteger("71169be7330b3038edb025f1", 16)
				}, new BigInteger("71169be7330b3038edb025f1d0f9", 16), new BigInteger("b3fb3400dec5c4adceb8655d4c94", 16), 208);
				ECCurve eccurve = SecNamedCurves.ConfigureCurveGlv(new FpCurve(q, zero, b, bigInteger, one), p);
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04DB4FF10EC057E9AE26B07D0280B7F4341DA5D1B1EAE06C7D9B2F2F6D9C5628A7844163D015BE86344082AA88D95E2F9D"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x04003741 RID: 14145
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp192k1Holder();
		}

		// Token: 0x020009BC RID: 2492
		internal class Secp192r1Holder : X9ECParametersHolder
		{
			// Token: 0x0600505F RID: 20575 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp192r1Holder()
			{
			}

			// Token: 0x06005060 RID: 20576 RVA: 0x001B9474 File Offset: 0x001B7674
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFFF");
				BigInteger a = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFFC");
				BigInteger b = SecNamedCurves.FromHex("64210519E59C80E70FA7E9AB72243049FEB8DEECC146B9B1");
				byte[] seed = Hex.Decode("3045AE6FC8422F64ED579528D38120EAE12196D5");
				BigInteger bigInteger = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFF99DEF836146BC9B1B4D22831");
				BigInteger one = BigInteger.One;
				ECCurve eccurve = SecNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, one));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04188DA80EB03090F67CBF20EB43A18800F4FF0AFD82FF101207192B95FFC8DA78631011ED6B24CDD573F977A11E794811"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x04003742 RID: 14146
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp192r1Holder();
		}

		// Token: 0x020009BD RID: 2493
		internal class Secp224k1Holder : X9ECParametersHolder
		{
			// Token: 0x06005062 RID: 20578 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp224k1Holder()
			{
			}

			// Token: 0x06005063 RID: 20579 RVA: 0x001B94F8 File Offset: 0x001B76F8
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFE56D");
				BigInteger zero = BigInteger.Zero;
				BigInteger b = BigInteger.ValueOf(5L);
				byte[] seed = null;
				BigInteger bigInteger = SecNamedCurves.FromHex("010000000000000000000000000001DCE8D2EC6184CAF0A971769FB1F7");
				BigInteger one = BigInteger.One;
				GlvTypeBParameters p = new GlvTypeBParameters(new BigInteger("fe0e87005b4e83761908c5131d552a850b3f58b749c37cf5b84d6768", 16), new BigInteger("60dcd2104c4cbc0be6eeefc2bdd610739ec34e317f9b33046c9e4788", 16), new BigInteger[]
				{
					new BigInteger("6b8cf07d4ca75c88957d9d670591", 16),
					new BigInteger("-b8adf1378a6eb73409fa6c9c637d", 16)
				}, new BigInteger[]
				{
					new BigInteger("1243ae1b4d71613bc9f780a03690e", 16),
					new BigInteger("6b8cf07d4ca75c88957d9d670591", 16)
				}, new BigInteger("6b8cf07d4ca75c88957d9d67059037a4", 16), new BigInteger("b8adf1378a6eb73409fa6c9c637ba7f5", 16), 240);
				ECCurve eccurve = SecNamedCurves.ConfigureCurveGlv(new FpCurve(q, zero, b, bigInteger, one), p);
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04A1455B334DF099DF30FC28A169A467E9E47075A90F7E650EB6B7A45C7E089FED7FBA344282CAFBD6F7E319F7C0B0BD59E2CA4BDB556D61A5"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x04003743 RID: 14147
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp224k1Holder();
		}

		// Token: 0x020009BE RID: 2494
		internal class Secp224r1Holder : X9ECParametersHolder
		{
			// Token: 0x06005065 RID: 20581 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp224r1Holder()
			{
			}

			// Token: 0x06005066 RID: 20582 RVA: 0x001B95F4 File Offset: 0x001B77F4
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000001");
				BigInteger a = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFE");
				BigInteger b = SecNamedCurves.FromHex("B4050A850C04B3ABF54132565044B0B7D7BFD8BA270B39432355FFB4");
				byte[] seed = Hex.Decode("BD71344799D5C7FCDC45B59FA3B9AB8F6A948BC5");
				BigInteger bigInteger = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFF16A2E0B8F03E13DD29455C5C2A3D");
				BigInteger one = BigInteger.One;
				ECCurve eccurve = SecNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, one));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04B70E0CBD6BB4BF7F321390B94A03C1D356C21122343280D6115C1D21BD376388B5F723FB4C22DFE6CD4375A05A07476444D5819985007E34"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x04003744 RID: 14148
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp224r1Holder();
		}

		// Token: 0x020009BF RID: 2495
		internal class Secp256k1Holder : X9ECParametersHolder
		{
			// Token: 0x06005068 RID: 20584 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp256k1Holder()
			{
			}

			// Token: 0x06005069 RID: 20585 RVA: 0x001B9678 File Offset: 0x001B7878
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F");
				BigInteger zero = BigInteger.Zero;
				BigInteger b = BigInteger.ValueOf(7L);
				byte[] seed = null;
				BigInteger bigInteger = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141");
				BigInteger one = BigInteger.One;
				GlvTypeBParameters p = new GlvTypeBParameters(new BigInteger("7ae96a2b657c07106e64479eac3434e99cf0497512f58995c1396c28719501ee", 16), new BigInteger("5363ad4cc05c30e0a5261c028812645a122e22ea20816678df02967c1b23bd72", 16), new BigInteger[]
				{
					new BigInteger("3086d221a7d46bcde86c90e49284eb15", 16),
					new BigInteger("-e4437ed6010e88286f547fa90abfe4c3", 16)
				}, new BigInteger[]
				{
					new BigInteger("114ca50f7a8e2f3f657c1108d9d44cfd8", 16),
					new BigInteger("3086d221a7d46bcde86c90e49284eb15", 16)
				}, new BigInteger("3086d221a7d46bcde86c90e49284eb153dab", 16), new BigInteger("e4437ed6010e88286f547fa90abfe4c42212", 16), 272);
				ECCurve eccurve = SecNamedCurves.ConfigureCurveGlv(new FpCurve(q, zero, b, bigInteger, one), p);
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0479BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x04003745 RID: 14149
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp256k1Holder();
		}

		// Token: 0x020009C0 RID: 2496
		internal class Secp256r1Holder : X9ECParametersHolder
		{
			// Token: 0x0600506B RID: 20587 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp256r1Holder()
			{
			}

			// Token: 0x0600506C RID: 20588 RVA: 0x001B9774 File Offset: 0x001B7974
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFF");
				BigInteger a = SecNamedCurves.FromHex("FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFC");
				BigInteger b = SecNamedCurves.FromHex("5AC635D8AA3A93E7B3EBBD55769886BC651D06B0CC53B0F63BCE3C3E27D2604B");
				byte[] seed = Hex.Decode("C49D360886E704936A6678E1139D26B7819F7E90");
				BigInteger bigInteger = SecNamedCurves.FromHex("FFFFFFFF00000000FFFFFFFFFFFFFFFFBCE6FAADA7179E84F3B9CAC2FC632551");
				BigInteger one = BigInteger.One;
				ECCurve eccurve = SecNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, one));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("046B17D1F2E12C4247F8BCE6E563A440F277037D812DEB33A0F4A13945D898C2964FE342E2FE1A7F9B8EE7EB4A7C0F9E162BCE33576B315ECECBB6406837BF51F5"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x04003746 RID: 14150
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp256r1Holder();
		}

		// Token: 0x020009C1 RID: 2497
		internal class Secp384r1Holder : X9ECParametersHolder
		{
			// Token: 0x0600506E RID: 20590 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp384r1Holder()
			{
			}

			// Token: 0x0600506F RID: 20591 RVA: 0x001B97F8 File Offset: 0x001B79F8
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFF0000000000000000FFFFFFFF");
				BigInteger a = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFF0000000000000000FFFFFFFC");
				BigInteger b = SecNamedCurves.FromHex("B3312FA7E23EE7E4988E056BE3F82D19181D9C6EFE8141120314088F5013875AC656398D8A2ED19D2A85C8EDD3EC2AEF");
				byte[] seed = Hex.Decode("A335926AA319A27A1D00896A6773A4827ACDAC73");
				BigInteger bigInteger = SecNamedCurves.FromHex("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC7634D81F4372DDF581A0DB248B0A77AECEC196ACCC52973");
				BigInteger one = BigInteger.One;
				ECCurve eccurve = SecNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, one));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04AA87CA22BE8B05378EB1C71EF320AD746E1D3B628BA79B9859F741E082542A385502F25DBF55296C3A545E3872760AB73617DE4A96262C6F5D9E98BF9292DC29F8F41DBD289A147CE9DA3113B5F0B8C00A60B1CE1D7E819D7A431D7C90EA0E5F"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x04003747 RID: 14151
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp384r1Holder();
		}

		// Token: 0x020009C2 RID: 2498
		internal class Secp521r1Holder : X9ECParametersHolder
		{
			// Token: 0x06005071 RID: 20593 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Secp521r1Holder()
			{
			}

			// Token: 0x06005072 RID: 20594 RVA: 0x001B987C File Offset: 0x001B7A7C
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = SecNamedCurves.FromHex("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
				BigInteger a = SecNamedCurves.FromHex("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC");
				BigInteger b = SecNamedCurves.FromHex("0051953EB9618E1C9A1F929A21A0B68540EEA2DA725B99B315F3B8B489918EF109E156193951EC7E937B1652C0BD3BB1BF073573DF883D2C34F1EF451FD46B503F00");
				byte[] seed = Hex.Decode("D09E8800291CB85396CC6717393284AAA0DA64BA");
				BigInteger bigInteger = SecNamedCurves.FromHex("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFA51868783BF2F966B7FCC0148F709A5D03BB5C9B8899C47AEBB6FB71E91386409");
				BigInteger one = BigInteger.One;
				ECCurve eccurve = SecNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, one));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0400C6858E06B70404E9CD9E3ECB662395B4429C648139053FB521F828AF606B4D3DBAA14B5E77EFE75928FE1DC127A2FFA8DE3348B3C1856A429BF97E7E31C2E5BD66011839296A789A3BC0045C8A5FB42C7D1BD998F54449579B446817AFBD17273E662C97EE72995EF42640C550B9013FAD0761353C7086A272C24088BE94769FD16650"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x04003748 RID: 14152
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Secp521r1Holder();
		}

		// Token: 0x020009C3 RID: 2499
		internal class Sect113r1Holder : X9ECParametersHolder
		{
			// Token: 0x06005074 RID: 20596 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect113r1Holder()
			{
			}

			// Token: 0x06005075 RID: 20597 RVA: 0x001B9900 File Offset: 0x001B7B00
			protected override X9ECParameters CreateParameters()
			{
				BigInteger a = SecNamedCurves.FromHex("003088250CA6E7C7FE649CE85820F7");
				BigInteger b = SecNamedCurves.FromHex("00E8BEE4D3E2260744188BE0E9C723");
				byte[] seed = Hex.Decode("10E723AB14D696E6768756151756FEBF8FCB49A9");
				BigInteger bigInteger = SecNamedCurves.FromHex("0100000000000000D9CCEC8A39E56F");
				BigInteger bigInteger2 = BigInteger.ValueOf(2L);
				F2mCurve f2mCurve = new F2mCurve(113, 9, a, b, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("04009D73616F35F4AB1407D73562C10F00A52830277958EE84D1315ED31886"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x04003749 RID: 14153
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect113r1Holder();

			// Token: 0x0400374A RID: 14154
			private const int m = 113;

			// Token: 0x0400374B RID: 14155
			private const int k = 9;
		}

		// Token: 0x020009C4 RID: 2500
		internal class Sect113r2Holder : X9ECParametersHolder
		{
			// Token: 0x06005077 RID: 20599 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect113r2Holder()
			{
			}

			// Token: 0x06005078 RID: 20600 RVA: 0x001B997C File Offset: 0x001B7B7C
			protected override X9ECParameters CreateParameters()
			{
				BigInteger a = SecNamedCurves.FromHex("00689918DBEC7E5A0DD6DFC0AA55C7");
				BigInteger b = SecNamedCurves.FromHex("0095E9A9EC9B297BD4BF36E059184F");
				byte[] seed = Hex.Decode("10C0FB15760860DEF1EEF4D696E676875615175D");
				BigInteger bigInteger = SecNamedCurves.FromHex("010000000000000108789B2496AF93");
				BigInteger bigInteger2 = BigInteger.ValueOf(2L);
				F2mCurve f2mCurve = new F2mCurve(113, 9, a, b, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("0401A57A6A7B26CA5EF52FCDB816479700B3ADC94ED1FE674C06E695BABA1D"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x0400374C RID: 14156
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect113r2Holder();

			// Token: 0x0400374D RID: 14157
			private const int m = 113;

			// Token: 0x0400374E RID: 14158
			private const int k = 9;
		}

		// Token: 0x020009C5 RID: 2501
		internal class Sect131r1Holder : X9ECParametersHolder
		{
			// Token: 0x0600507A RID: 20602 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect131r1Holder()
			{
			}

			// Token: 0x0600507B RID: 20603 RVA: 0x001B99F8 File Offset: 0x001B7BF8
			protected override X9ECParameters CreateParameters()
			{
				BigInteger a = SecNamedCurves.FromHex("07A11B09A76B562144418FF3FF8C2570B8");
				BigInteger b = SecNamedCurves.FromHex("0217C05610884B63B9C6C7291678F9D341");
				byte[] seed = Hex.Decode("4D696E676875615175985BD3ADBADA21B43A97E2");
				BigInteger bigInteger = SecNamedCurves.FromHex("0400000000000000023123953A9464B54D");
				BigInteger bigInteger2 = BigInteger.ValueOf(2L);
				F2mCurve f2mCurve = new F2mCurve(131, 2, 3, 8, a, b, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("040081BAF91FDF9833C40F9C181343638399078C6E7EA38C001F73C8134B1B4EF9E150"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x0400374F RID: 14159
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect131r1Holder();

			// Token: 0x04003750 RID: 14160
			private const int m = 131;

			// Token: 0x04003751 RID: 14161
			private const int k1 = 2;

			// Token: 0x04003752 RID: 14162
			private const int k2 = 3;

			// Token: 0x04003753 RID: 14163
			private const int k3 = 8;
		}

		// Token: 0x020009C6 RID: 2502
		internal class Sect131r2Holder : X9ECParametersHolder
		{
			// Token: 0x0600507D RID: 20605 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect131r2Holder()
			{
			}

			// Token: 0x0600507E RID: 20606 RVA: 0x001B9A78 File Offset: 0x001B7C78
			protected override X9ECParameters CreateParameters()
			{
				BigInteger a = SecNamedCurves.FromHex("03E5A88919D7CAFCBF415F07C2176573B2");
				BigInteger b = SecNamedCurves.FromHex("04B8266A46C55657AC734CE38F018F2192");
				byte[] seed = Hex.Decode("985BD3ADBAD4D696E676875615175A21B43A97E3");
				BigInteger bigInteger = SecNamedCurves.FromHex("0400000000000000016954A233049BA98F");
				BigInteger bigInteger2 = BigInteger.ValueOf(2L);
				F2mCurve f2mCurve = new F2mCurve(131, 2, 3, 8, a, b, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("040356DCD8F2F95031AD652D23951BB366A80648F06D867940A5366D9E265DE9EB240F"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x04003754 RID: 14164
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect131r2Holder();

			// Token: 0x04003755 RID: 14165
			private const int m = 131;

			// Token: 0x04003756 RID: 14166
			private const int k1 = 2;

			// Token: 0x04003757 RID: 14167
			private const int k2 = 3;

			// Token: 0x04003758 RID: 14168
			private const int k3 = 8;
		}

		// Token: 0x020009C7 RID: 2503
		internal class Sect163k1Holder : X9ECParametersHolder
		{
			// Token: 0x06005080 RID: 20608 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect163k1Holder()
			{
			}

			// Token: 0x06005081 RID: 20609 RVA: 0x001B9AF8 File Offset: 0x001B7CF8
			protected override X9ECParameters CreateParameters()
			{
				BigInteger one = BigInteger.One;
				BigInteger one2 = BigInteger.One;
				byte[] seed = null;
				BigInteger bigInteger = SecNamedCurves.FromHex("04000000000000000000020108A2E0CC0D99F8A5EF");
				BigInteger bigInteger2 = BigInteger.ValueOf(2L);
				F2mCurve f2mCurve = new F2mCurve(163, 3, 6, 7, one, one2, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("0402FE13C0537BBC11ACAA07D793DE4E6D5E5C94EEE80289070FB05D38FF58321F2E800536D538CCDAA3D9"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x04003759 RID: 14169
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect163k1Holder();

			// Token: 0x0400375A RID: 14170
			private const int m = 163;

			// Token: 0x0400375B RID: 14171
			private const int k1 = 3;

			// Token: 0x0400375C RID: 14172
			private const int k2 = 6;

			// Token: 0x0400375D RID: 14173
			private const int k3 = 7;
		}

		// Token: 0x020009C8 RID: 2504
		internal class Sect163r1Holder : X9ECParametersHolder
		{
			// Token: 0x06005083 RID: 20611 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect163r1Holder()
			{
			}

			// Token: 0x06005084 RID: 20612 RVA: 0x001B9B64 File Offset: 0x001B7D64
			protected override X9ECParameters CreateParameters()
			{
				BigInteger a = SecNamedCurves.FromHex("07B6882CAAEFA84F9554FF8428BD88E246D2782AE2");
				BigInteger b = SecNamedCurves.FromHex("0713612DCDDCB40AAB946BDA29CA91F73AF958AFD9");
				byte[] seed = Hex.Decode("24B7B137C8A14D696E6768756151756FD0DA2E5C");
				BigInteger bigInteger = SecNamedCurves.FromHex("03FFFFFFFFFFFFFFFFFFFF48AAB689C29CA710279B");
				BigInteger bigInteger2 = BigInteger.ValueOf(2L);
				F2mCurve f2mCurve = new F2mCurve(163, 3, 6, 7, a, b, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("040369979697AB43897789566789567F787A7876A65400435EDB42EFAFB2989D51FEFCE3C80988F41FF883"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x0400375E RID: 14174
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect163r1Holder();

			// Token: 0x0400375F RID: 14175
			private const int m = 163;

			// Token: 0x04003760 RID: 14176
			private const int k1 = 3;

			// Token: 0x04003761 RID: 14177
			private const int k2 = 6;

			// Token: 0x04003762 RID: 14178
			private const int k3 = 7;
		}

		// Token: 0x020009C9 RID: 2505
		internal class Sect163r2Holder : X9ECParametersHolder
		{
			// Token: 0x06005086 RID: 20614 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect163r2Holder()
			{
			}

			// Token: 0x06005087 RID: 20615 RVA: 0x001B9BE4 File Offset: 0x001B7DE4
			protected override X9ECParameters CreateParameters()
			{
				BigInteger one = BigInteger.One;
				BigInteger b = SecNamedCurves.FromHex("020A601907B8C953CA1481EB10512F78744A3205FD");
				byte[] seed = Hex.Decode("85E25BFE5C86226CDB12016F7553F9D0E693A268");
				BigInteger bigInteger = SecNamedCurves.FromHex("040000000000000000000292FE77E70C12A4234C33");
				BigInteger bigInteger2 = BigInteger.ValueOf(2L);
				F2mCurve f2mCurve = new F2mCurve(163, 3, 6, 7, one, b, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("0403F0EBA16286A2D57EA0991168D4994637E8343E3600D51FBC6C71A0094FA2CDD545B11C5C0C797324F1"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x04003763 RID: 14179
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect163r2Holder();

			// Token: 0x04003764 RID: 14180
			private const int m = 163;

			// Token: 0x04003765 RID: 14181
			private const int k1 = 3;

			// Token: 0x04003766 RID: 14182
			private const int k2 = 6;

			// Token: 0x04003767 RID: 14183
			private const int k3 = 7;
		}

		// Token: 0x020009CA RID: 2506
		internal class Sect193r1Holder : X9ECParametersHolder
		{
			// Token: 0x06005089 RID: 20617 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect193r1Holder()
			{
			}

			// Token: 0x0600508A RID: 20618 RVA: 0x001B9C5C File Offset: 0x001B7E5C
			protected override X9ECParameters CreateParameters()
			{
				BigInteger a = SecNamedCurves.FromHex("0017858FEB7A98975169E171F77B4087DE098AC8A911DF7B01");
				BigInteger b = SecNamedCurves.FromHex("00FDFB49BFE6C3A89FACADAA7A1E5BBC7CC1C2E5D831478814");
				byte[] seed = Hex.Decode("103FAEC74D696E676875615175777FC5B191EF30");
				BigInteger bigInteger = SecNamedCurves.FromHex("01000000000000000000000000C7F34A778F443ACC920EBA49");
				BigInteger bigInteger2 = BigInteger.ValueOf(2L);
				F2mCurve f2mCurve = new F2mCurve(193, 15, a, b, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("0401F481BC5F0FF84A74AD6CDF6FDEF4BF6179625372D8C0C5E10025E399F2903712CCF3EA9E3A1AD17FB0B3201B6AF7CE1B05"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x04003768 RID: 14184
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect193r1Holder();

			// Token: 0x04003769 RID: 14185
			private const int m = 193;

			// Token: 0x0400376A RID: 14186
			private const int k = 15;
		}

		// Token: 0x020009CB RID: 2507
		internal class Sect193r2Holder : X9ECParametersHolder
		{
			// Token: 0x0600508C RID: 20620 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect193r2Holder()
			{
			}

			// Token: 0x0600508D RID: 20621 RVA: 0x001B9CD8 File Offset: 0x001B7ED8
			protected override X9ECParameters CreateParameters()
			{
				BigInteger a = SecNamedCurves.FromHex("0163F35A5137C2CE3EA6ED8667190B0BC43ECD69977702709B");
				BigInteger b = SecNamedCurves.FromHex("00C9BB9E8927D4D64C377E2AB2856A5B16E3EFB7F61D4316AE");
				byte[] seed = Hex.Decode("10B7B4D696E676875615175137C8A16FD0DA2211");
				BigInteger bigInteger = SecNamedCurves.FromHex("010000000000000000000000015AAB561B005413CCD4EE99D5");
				BigInteger bigInteger2 = BigInteger.ValueOf(2L);
				F2mCurve f2mCurve = new F2mCurve(193, 15, a, b, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("0400D9B67D192E0367C803F39E1A7E82CA14A651350AAE617E8F01CE94335607C304AC29E7DEFBD9CA01F596F927224CDECF6C"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x0400376B RID: 14187
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect193r2Holder();

			// Token: 0x0400376C RID: 14188
			private const int m = 193;

			// Token: 0x0400376D RID: 14189
			private const int k = 15;
		}

		// Token: 0x020009CC RID: 2508
		internal class Sect233k1Holder : X9ECParametersHolder
		{
			// Token: 0x0600508F RID: 20623 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect233k1Holder()
			{
			}

			// Token: 0x06005090 RID: 20624 RVA: 0x001B9D54 File Offset: 0x001B7F54
			protected override X9ECParameters CreateParameters()
			{
				BigInteger zero = BigInteger.Zero;
				BigInteger one = BigInteger.One;
				byte[] seed = null;
				BigInteger bigInteger = SecNamedCurves.FromHex("8000000000000000000000000000069D5BB915BCD46EFB1AD5F173ABDF");
				BigInteger bigInteger2 = BigInteger.ValueOf(4L);
				F2mCurve f2mCurve = new F2mCurve(233, 74, zero, one, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("04017232BA853A7E731AF129F22FF4149563A419C26BF50A4C9D6EEFAD612601DB537DECE819B7F70F555A67C427A8CD9BF18AEB9B56E0C11056FAE6A3"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x0400376E RID: 14190
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect233k1Holder();

			// Token: 0x0400376F RID: 14191
			private const int m = 233;

			// Token: 0x04003770 RID: 14192
			private const int k = 74;
		}

		// Token: 0x020009CD RID: 2509
		internal class Sect233r1Holder : X9ECParametersHolder
		{
			// Token: 0x06005092 RID: 20626 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect233r1Holder()
			{
			}

			// Token: 0x06005093 RID: 20627 RVA: 0x001B9DC0 File Offset: 0x001B7FC0
			protected override X9ECParameters CreateParameters()
			{
				BigInteger one = BigInteger.One;
				BigInteger b = SecNamedCurves.FromHex("0066647EDE6C332C7F8C0923BB58213B333B20E9CE4281FE115F7D8F90AD");
				byte[] seed = Hex.Decode("74D59FF07F6B413D0EA14B344B20A2DB049B50C3");
				BigInteger bigInteger = SecNamedCurves.FromHex("01000000000000000000000000000013E974E72F8A6922031D2603CFE0D7");
				BigInteger bigInteger2 = BigInteger.ValueOf(2L);
				F2mCurve f2mCurve = new F2mCurve(233, 74, one, b, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("0400FAC9DFCBAC8313BB2139F1BB755FEF65BC391F8B36F8F8EB7371FD558B01006A08A41903350678E58528BEBF8A0BEFF867A7CA36716F7E01F81052"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x04003771 RID: 14193
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect233r1Holder();

			// Token: 0x04003772 RID: 14194
			private const int m = 233;

			// Token: 0x04003773 RID: 14195
			private const int k = 74;
		}

		// Token: 0x020009CE RID: 2510
		internal class Sect239k1Holder : X9ECParametersHolder
		{
			// Token: 0x06005095 RID: 20629 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect239k1Holder()
			{
			}

			// Token: 0x06005096 RID: 20630 RVA: 0x001B9E38 File Offset: 0x001B8038
			protected override X9ECParameters CreateParameters()
			{
				BigInteger zero = BigInteger.Zero;
				BigInteger one = BigInteger.One;
				byte[] seed = null;
				BigInteger bigInteger = SecNamedCurves.FromHex("2000000000000000000000000000005A79FEC67CB6E91F1C1DA800E478A5");
				BigInteger bigInteger2 = BigInteger.ValueOf(4L);
				F2mCurve f2mCurve = new F2mCurve(239, 158, zero, one, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("0429A0B6A887A983E9730988A68727A8B2D126C44CC2CC7B2A6555193035DC76310804F12E549BDB011C103089E73510ACB275FC312A5DC6B76553F0CA"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x04003774 RID: 14196
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect239k1Holder();

			// Token: 0x04003775 RID: 14197
			private const int m = 239;

			// Token: 0x04003776 RID: 14198
			private const int k = 158;
		}

		// Token: 0x020009CF RID: 2511
		internal class Sect283k1Holder : X9ECParametersHolder
		{
			// Token: 0x06005098 RID: 20632 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect283k1Holder()
			{
			}

			// Token: 0x06005099 RID: 20633 RVA: 0x001B9EA4 File Offset: 0x001B80A4
			protected override X9ECParameters CreateParameters()
			{
				BigInteger zero = BigInteger.Zero;
				BigInteger one = BigInteger.One;
				byte[] seed = null;
				BigInteger bigInteger = SecNamedCurves.FromHex("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE9AE2ED07577265DFF7F94451E061E163C61");
				BigInteger bigInteger2 = BigInteger.ValueOf(4L);
				F2mCurve f2mCurve = new F2mCurve(283, 5, 7, 12, zero, one, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("040503213F78CA44883F1A3B8162F188E553CD265F23C1567A16876913B0C2AC245849283601CCDA380F1C9E318D90F95D07E5426FE87E45C0E8184698E45962364E34116177DD2259"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x04003777 RID: 14199
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect283k1Holder();

			// Token: 0x04003778 RID: 14200
			private const int m = 283;

			// Token: 0x04003779 RID: 14201
			private const int k1 = 5;

			// Token: 0x0400377A RID: 14202
			private const int k2 = 7;

			// Token: 0x0400377B RID: 14203
			private const int k3 = 12;
		}

		// Token: 0x020009D0 RID: 2512
		internal class Sect283r1Holder : X9ECParametersHolder
		{
			// Token: 0x0600509B RID: 20635 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect283r1Holder()
			{
			}

			// Token: 0x0600509C RID: 20636 RVA: 0x001B9F10 File Offset: 0x001B8110
			protected override X9ECParameters CreateParameters()
			{
				BigInteger one = BigInteger.One;
				BigInteger b = SecNamedCurves.FromHex("027B680AC8B8596DA5A4AF8A19A0303FCA97FD7645309FA2A581485AF6263E313B79A2F5");
				byte[] seed = Hex.Decode("77E2B07370EB0F832A6DD5B62DFC88CD06BB84BE");
				BigInteger bigInteger = SecNamedCurves.FromHex("03FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEF90399660FC938A90165B042A7CEFADB307");
				BigInteger bigInteger2 = BigInteger.ValueOf(2L);
				F2mCurve f2mCurve = new F2mCurve(283, 5, 7, 12, one, b, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("0405F939258DB7DD90E1934F8C70B0DFEC2EED25B8557EAC9C80E2E198F8CDBECD86B1205303676854FE24141CB98FE6D4B20D02B4516FF702350EDDB0826779C813F0DF45BE8112F4"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x0400377C RID: 14204
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect283r1Holder();

			// Token: 0x0400377D RID: 14205
			private const int m = 283;

			// Token: 0x0400377E RID: 14206
			private const int k1 = 5;

			// Token: 0x0400377F RID: 14207
			private const int k2 = 7;

			// Token: 0x04003780 RID: 14208
			private const int k3 = 12;
		}

		// Token: 0x020009D1 RID: 2513
		internal class Sect409k1Holder : X9ECParametersHolder
		{
			// Token: 0x0600509E RID: 20638 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect409k1Holder()
			{
			}

			// Token: 0x0600509F RID: 20639 RVA: 0x001B9F8C File Offset: 0x001B818C
			protected override X9ECParameters CreateParameters()
			{
				BigInteger zero = BigInteger.Zero;
				BigInteger one = BigInteger.One;
				byte[] seed = null;
				BigInteger bigInteger = SecNamedCurves.FromHex("7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE5F83B2D4EA20400EC4557D5ED3E3E7CA5B4B5C83B8E01E5FCF");
				BigInteger bigInteger2 = BigInteger.ValueOf(4L);
				F2mCurve f2mCurve = new F2mCurve(409, 87, zero, one, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("040060F05F658F49C1AD3AB1890F7184210EFD0987E307C84C27ACCFB8F9F67CC2C460189EB5AAAA62EE222EB1B35540CFE902374601E369050B7C4E42ACBA1DACBF04299C3460782F918EA427E6325165E9EA10E3DA5F6C42E9C55215AA9CA27A5863EC48D8E0286B"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x04003781 RID: 14209
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect409k1Holder();

			// Token: 0x04003782 RID: 14210
			private const int m = 409;

			// Token: 0x04003783 RID: 14211
			private const int k = 87;
		}

		// Token: 0x020009D2 RID: 2514
		internal class Sect409r1Holder : X9ECParametersHolder
		{
			// Token: 0x060050A1 RID: 20641 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect409r1Holder()
			{
			}

			// Token: 0x060050A2 RID: 20642 RVA: 0x001B9FF8 File Offset: 0x001B81F8
			protected override X9ECParameters CreateParameters()
			{
				BigInteger one = BigInteger.One;
				BigInteger b = SecNamedCurves.FromHex("0021A5C2C8EE9FEB5C4B9A753B7B476B7FD6422EF1F3DD674761FA99D6AC27C8A9A197B272822F6CD57A55AA4F50AE317B13545F");
				byte[] seed = Hex.Decode("4099B5A457F9D69F79213D094C4BCD4D4262210B");
				BigInteger bigInteger = SecNamedCurves.FromHex("010000000000000000000000000000000000000000000000000001E2AAD6A612F33307BE5FA47C3C9E052F838164CD37D9A21173");
				BigInteger bigInteger2 = BigInteger.ValueOf(2L);
				F2mCurve f2mCurve = new F2mCurve(409, 87, one, b, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("04015D4860D088DDB3496B0C6064756260441CDE4AF1771D4DB01FFE5B34E59703DC255A868A1180515603AEAB60794E54BB7996A70061B1CFAB6BE5F32BBFA78324ED106A7636B9C5A7BD198D0158AA4F5488D08F38514F1FDF4B4F40D2181B3681C364BA0273C706"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x04003784 RID: 14212
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect409r1Holder();

			// Token: 0x04003785 RID: 14213
			private const int m = 409;

			// Token: 0x04003786 RID: 14214
			private const int k = 87;
		}

		// Token: 0x020009D3 RID: 2515
		internal class Sect571k1Holder : X9ECParametersHolder
		{
			// Token: 0x060050A4 RID: 20644 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect571k1Holder()
			{
			}

			// Token: 0x060050A5 RID: 20645 RVA: 0x001BA070 File Offset: 0x001B8270
			protected override X9ECParameters CreateParameters()
			{
				BigInteger zero = BigInteger.Zero;
				BigInteger one = BigInteger.One;
				byte[] seed = null;
				BigInteger bigInteger = SecNamedCurves.FromHex("020000000000000000000000000000000000000000000000000000000000000000000000131850E1F19A63E4B391A8DB917F4138B630D84BE5D639381E91DEB45CFE778F637C1001");
				BigInteger bigInteger2 = BigInteger.ValueOf(4L);
				F2mCurve f2mCurve = new F2mCurve(571, 2, 5, 10, zero, one, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("04026EB7A859923FBC82189631F8103FE4AC9CA2970012D5D46024804801841CA44370958493B205E647DA304DB4CEB08CBBD1BA39494776FB988B47174DCA88C7E2945283A01C89720349DC807F4FBF374F4AEADE3BCA95314DD58CEC9F307A54FFC61EFC006D8A2C9D4979C0AC44AEA74FBEBBB9F772AEDCB620B01A7BA7AF1B320430C8591984F601CD4C143EF1C7A3"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x04003787 RID: 14215
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect571k1Holder();

			// Token: 0x04003788 RID: 14216
			private const int m = 571;

			// Token: 0x04003789 RID: 14217
			private const int k1 = 2;

			// Token: 0x0400378A RID: 14218
			private const int k2 = 5;

			// Token: 0x0400378B RID: 14219
			private const int k3 = 10;
		}

		// Token: 0x020009D4 RID: 2516
		internal class Sect571r1Holder : X9ECParametersHolder
		{
			// Token: 0x060050A7 RID: 20647 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Sect571r1Holder()
			{
			}

			// Token: 0x060050A8 RID: 20648 RVA: 0x001BA0DC File Offset: 0x001B82DC
			protected override X9ECParameters CreateParameters()
			{
				BigInteger one = BigInteger.One;
				BigInteger b = SecNamedCurves.FromHex("02F40E7E2221F295DE297117B7F3D62F5C6A97FFCB8CEFF1CD6BA8CE4A9A18AD84FFABBD8EFA59332BE7AD6756A66E294AFD185A78FF12AA520E4DE739BACA0C7FFEFF7F2955727A");
				byte[] seed = Hex.Decode("2AA058F73A0E33AB486B0F610410C53A7F132310");
				BigInteger bigInteger = SecNamedCurves.FromHex("03FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE661CE18FF55987308059B186823851EC7DD9CA1161DE93D5174D66E8382E9BB2FE84E47");
				BigInteger bigInteger2 = BigInteger.ValueOf(2L);
				F2mCurve f2mCurve = new F2mCurve(571, 2, 5, 10, one, b, bigInteger, bigInteger2);
				X9ECPoint g = new X9ECPoint(f2mCurve, Hex.Decode("040303001D34B856296C16C0D40D3CD7750A93D1D2955FA80AA5F40FC8DB7B2ABDBDE53950F4C0D293CDD711A35B67FB1499AE60038614F1394ABFA3B4C850D927E1E7769C8EEC2D19037BF27342DA639B6DCCFFFEB73D69D78C6C27A6009CBBCA1980F8533921E8A684423E43BAB08A576291AF8F461BB2A8B3531D2F0485C19B16E2F1516E23DD3C1A4827AF1B8AC15B"));
				return new X9ECParameters(f2mCurve, g, bigInteger, bigInteger2, seed);
			}

			// Token: 0x0400378C RID: 14220
			internal static readonly X9ECParametersHolder Instance = new SecNamedCurves.Sect571r1Holder();

			// Token: 0x0400378D RID: 14221
			private const int m = 571;

			// Token: 0x0400378E RID: 14222
			private const int k1 = 2;

			// Token: 0x0400378F RID: 14223
			private const int k2 = 5;

			// Token: 0x04003790 RID: 14224
			private const int k3 = 10;
		}
	}
}
