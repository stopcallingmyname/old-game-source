using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Anssi
{
	// Token: 0x020007D3 RID: 2003
	public class AnssiNamedCurves
	{
		// Token: 0x06004729 RID: 18217 RVA: 0x000947CE File Offset: 0x000929CE
		private static ECCurve ConfigureCurve(ECCurve curve)
		{
			return curve;
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x001168E2 File Offset: 0x00114AE2
		private static BigInteger FromHex(string hex)
		{
			return new BigInteger(1, Hex.Decode(hex));
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x001956D4 File Offset: 0x001938D4
		private static void DefineCurve(string name, DerObjectIdentifier oid, X9ECParametersHolder holder)
		{
			AnssiNamedCurves.objIds.Add(Platform.ToUpperInvariant(name), oid);
			AnssiNamedCurves.names.Add(oid, name);
			AnssiNamedCurves.curves.Add(oid, holder);
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x001956FF File Offset: 0x001938FF
		static AnssiNamedCurves()
		{
			AnssiNamedCurves.DefineCurve("FRP256v1", AnssiObjectIdentifiers.FRP256v1, AnssiNamedCurves.Frp256v1Holder.Instance);
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x00195734 File Offset: 0x00193934
		public static X9ECParameters GetByName(string name)
		{
			DerObjectIdentifier oid = AnssiNamedCurves.GetOid(name);
			if (oid != null)
			{
				return AnssiNamedCurves.GetByOid(oid);
			}
			return null;
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x00195754 File Offset: 0x00193954
		public static X9ECParameters GetByOid(DerObjectIdentifier oid)
		{
			X9ECParametersHolder x9ECParametersHolder = (X9ECParametersHolder)AnssiNamedCurves.curves[oid];
			if (x9ECParametersHolder != null)
			{
				return x9ECParametersHolder.Parameters;
			}
			return null;
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x0019577D File Offset: 0x0019397D
		public static DerObjectIdentifier GetOid(string name)
		{
			return (DerObjectIdentifier)AnssiNamedCurves.objIds[Platform.ToUpperInvariant(name)];
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x00195794 File Offset: 0x00193994
		public static string GetName(DerObjectIdentifier oid)
		{
			return (string)AnssiNamedCurves.names[oid];
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06004731 RID: 18225 RVA: 0x001957A6 File Offset: 0x001939A6
		public static IEnumerable Names
		{
			get
			{
				return new EnumerableProxy(AnssiNamedCurves.names.Values);
			}
		}

		// Token: 0x04002E9F RID: 11935
		private static readonly IDictionary objIds = Platform.CreateHashtable();

		// Token: 0x04002EA0 RID: 11936
		private static readonly IDictionary curves = Platform.CreateHashtable();

		// Token: 0x04002EA1 RID: 11937
		private static readonly IDictionary names = Platform.CreateHashtable();

		// Token: 0x020009D9 RID: 2521
		internal class Frp256v1Holder : X9ECParametersHolder
		{
			// Token: 0x060050B0 RID: 20656 RVA: 0x001B5BFD File Offset: 0x001B3DFD
			private Frp256v1Holder()
			{
			}

			// Token: 0x060050B1 RID: 20657 RVA: 0x001BA250 File Offset: 0x001B8450
			protected override X9ECParameters CreateParameters()
			{
				BigInteger q = AnssiNamedCurves.FromHex("F1FD178C0B3AD58F10126DE8CE42435B3961ADBCABC8CA6DE8FCF353D86E9C03");
				BigInteger a = AnssiNamedCurves.FromHex("F1FD178C0B3AD58F10126DE8CE42435B3961ADBCABC8CA6DE8FCF353D86E9C00");
				BigInteger b = AnssiNamedCurves.FromHex("EE353FCA5428A9300D4ABA754A44C00FDFEC0C9AE4B1A1803075ED967B7BB73F");
				byte[] seed = null;
				BigInteger bigInteger = AnssiNamedCurves.FromHex("F1FD178C0B3AD58F10126DE8CE42435B53DC67E140D2BF941FFDD459C6D655E1");
				BigInteger one = BigInteger.One;
				ECCurve eccurve = AnssiNamedCurves.ConfigureCurve(new FpCurve(q, a, b, bigInteger, one));
				X9ECPoint g = new X9ECPoint(eccurve, Hex.Decode("04B6B3D4C356C139EB31183D4749D423958C27D2DCAF98B70164C97A2DD98F5CFF6142E0F7C8B204911F9271F0F3ECEF8C2701C307E8E4C9E183115A1554062CFB"));
				return new X9ECParameters(eccurve, g, bigInteger, one, seed);
			}

			// Token: 0x0400379B RID: 14235
			internal static readonly X9ECParametersHolder Instance = new AnssiNamedCurves.Frp256v1Holder();
		}
	}
}
