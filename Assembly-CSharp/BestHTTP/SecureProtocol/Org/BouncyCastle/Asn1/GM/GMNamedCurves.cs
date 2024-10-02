using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.GM
{
	// Token: 0x02000737 RID: 1847
	public sealed class GMNamedCurves
	{
		// Token: 0x060042E0 RID: 17120 RVA: 0x00022F1F File Offset: 0x0002111F
		private GMNamedCurves()
		{
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x000947CE File Offset: 0x000929CE
		private static ECCurve ConfigureCurve(ECCurve curve)
		{
			return curve;
		}

		// Token: 0x060042E2 RID: 17122 RVA: 0x001168E2 File Offset: 0x00114AE2
		private static BigInteger FromHex(string hex)
		{
			return new BigInteger(1, Hex.Decode(hex));
		}

		// Token: 0x060042E3 RID: 17123 RVA: 0x001880A1 File Offset: 0x001862A1
		private static void DefineCurve(string name, DerObjectIdentifier oid, X9ECParametersHolder holder)
		{
			GMNamedCurves.objIds.Add(Platform.ToUpperInvariant(name), oid);
			GMNamedCurves.names.Add(oid, name);
			GMNamedCurves.curves.Add(oid, holder);
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x001880CC File Offset: 0x001862CC
		static GMNamedCurves()
		{
			GMNamedCurves.DefineCurve("wapip192v1", GMObjectIdentifiers.wapip192v1, GMNamedCurves.WapiP192V1Holder.Instance);
			GMNamedCurves.DefineCurve("sm2p256v1", GMObjectIdentifiers.sm2p256v1, GMNamedCurves.SM2P256V1Holder.Instance);
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x00188120 File Offset: 0x00186320
		public static X9ECParameters GetByName(string name)
		{
			DerObjectIdentifier oid = GMNamedCurves.GetOid(name);
			if (oid != null)
			{
				return GMNamedCurves.GetByOid(oid);
			}
			return null;
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x00188140 File Offset: 0x00186340
		public static X9ECParameters GetByOid(DerObjectIdentifier oid)
		{
			X9ECParametersHolder x9ECParametersHolder = (X9ECParametersHolder)GMNamedCurves.curves[oid];
			if (x9ECParametersHolder != null)
			{
				return x9ECParametersHolder.Parameters;
			}
			return null;
		}

		// Token: 0x060042E7 RID: 17127 RVA: 0x00188169 File Offset: 0x00186369
		public static DerObjectIdentifier GetOid(string name)
		{
			return (DerObjectIdentifier)GMNamedCurves.objIds[Platform.ToUpperInvariant(name)];
		}

		// Token: 0x060042E8 RID: 17128 RVA: 0x00188180 File Offset: 0x00186380
		public static string GetName(DerObjectIdentifier oid)
		{
			return (string)GMNamedCurves.names[oid];
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060042E9 RID: 17129 RVA: 0x00188192 File Offset: 0x00186392
		public static IEnumerable Names
		{
			get
			{
				return new EnumerableProxy(GMNamedCurves.names.Values);
			}
		}

		// Token: 0x04002BC6 RID: 11206
		private static readonly IDictionary objIds = Platform.CreateHashtable();

		// Token: 0x04002BC7 RID: 11207
		private static readonly IDictionary curves = Platform.CreateHashtable();

		// Token: 0x04002BC8 RID: 11208
		private static readonly IDictionary names = Platform.CreateHashtable();

		// Token: 0x020009D7 RID: 2519
		internal class SM2P256V1Holder : X9ECParametersHolder
		{
			// Token: 0x060050AA RID: 20650 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private SM2P256V1Holder()
			{
			}

			// Token: 0x060050AB RID: 20651 RVA: 0x001BA158 File Offset: 0x001B8358
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = GMNamedCurves.FromHex("FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFF");
				BigInteger a = GMNamedCurves.FromHex("FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFC");
				BigInteger b = GMNamedCurves.FromHex("28E9FA9E9D9F5E344D5A9E4BCF6509A7F39789F515AB8F92DDBCBD414D940E93");
				byte[] seed = null;
				BigInteger bigInteger = GMNamedCurves.FromHex("FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFF7203DF6B21C6052B53BBF40939D54123");
				BigInteger one = BigInteger.One;
				ECCurve eccurve = GMNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, one));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("0432C4AE2C1F1981195F9904466A39C9948FE30BBFF2660BE1715A4589334C74C7BC3736A2F4F6779C59BDCEE36B692153D0A9877CC62A474002DF32E52139F0A0"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x04003799 RID: 14233
			internal static readonly X9ECParametersHolder Instance = new GMNamedCurves.SM2P256V1Holder();
		}

		// Token: 0x020009D8 RID: 2520
		internal class WapiP192V1Holder : X9ECParametersHolder
		{
			// Token: 0x060050AD RID: 20653 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private WapiP192V1Holder()
			{
			}

			// Token: 0x060050AE RID: 20654 RVA: 0x001BA1D4 File Offset: 0x001B83D4
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = GMNamedCurves.FromHex("BDB6F4FE3E8B1D9E0DA8C0D46F4C318CEFE4AFE3B6B8551F");
				BigInteger a = GMNamedCurves.FromHex("BB8E5E8FBC115E139FE6A814FE48AAA6F0ADA1AA5DF91985");
				BigInteger b = GMNamedCurves.FromHex("1854BEBDC31B21B7AEFC80AB0ECD10D5B1B3308E6DBF11C1");
				byte[] seed = null;
				BigInteger bigInteger = GMNamedCurves.FromHex("BDB6F4FE3E8B1D9E0DA8C0D40FC962195DFAE76F56564677");
				BigInteger one = BigInteger.One;
				ECCurve eccurve = GMNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, one));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("044AD5F7048DE709AD51236DE65E4D4B482C836DC6E410664002BB3A02D4AAADACAE24817A4CA3A1B014B5270432DB27D2"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x0400379A RID: 14234
			internal static readonly X9ECParametersHolder Instance = new GMNamedCurves.WapiP192V1Holder();
		}
	}
}
