using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Sec;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist
{
	// Token: 0x02000719 RID: 1817
	public sealed class NistNamedCurves
	{
		// Token: 0x06004242 RID: 16962 RVA: 0x00022F1F File Offset: 0x0002111F
		private NistNamedCurves()
		{
		}

		// Token: 0x06004243 RID: 16963 RVA: 0x00185577 File Offset: 0x00183777
		private static void DefineCurveAlias(string name, DerObjectIdentifier oid)
		{
			NistNamedCurves.objIds.Add(Platform.ToUpperInvariant(name), oid);
			NistNamedCurves.names.Add(oid, name);
		}

		// Token: 0x06004244 RID: 16964 RVA: 0x00185598 File Offset: 0x00183798
		static NistNamedCurves()
		{
			NistNamedCurves.DefineCurveAlias("B-163", SecObjectIdentifiers.SecT163r2);
			NistNamedCurves.DefineCurveAlias("B-233", SecObjectIdentifiers.SecT233r1);
			NistNamedCurves.DefineCurveAlias("B-283", SecObjectIdentifiers.SecT283r1);
			NistNamedCurves.DefineCurveAlias("B-409", SecObjectIdentifiers.SecT409r1);
			NistNamedCurves.DefineCurveAlias("B-571", SecObjectIdentifiers.SecT571r1);
			NistNamedCurves.DefineCurveAlias("K-163", SecObjectIdentifiers.SecT163k1);
			NistNamedCurves.DefineCurveAlias("K-233", SecObjectIdentifiers.SecT233k1);
			NistNamedCurves.DefineCurveAlias("K-283", SecObjectIdentifiers.SecT283k1);
			NistNamedCurves.DefineCurveAlias("K-409", SecObjectIdentifiers.SecT409k1);
			NistNamedCurves.DefineCurveAlias("K-571", SecObjectIdentifiers.SecT571k1);
			NistNamedCurves.DefineCurveAlias("P-192", SecObjectIdentifiers.SecP192r1);
			NistNamedCurves.DefineCurveAlias("P-224", SecObjectIdentifiers.SecP224r1);
			NistNamedCurves.DefineCurveAlias("P-256", SecObjectIdentifiers.SecP256r1);
			NistNamedCurves.DefineCurveAlias("P-384", SecObjectIdentifiers.SecP384r1);
			NistNamedCurves.DefineCurveAlias("P-521", SecObjectIdentifiers.SecP521r1);
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x0018569C File Offset: 0x0018389C
		public static X9ECParameters GetByName(string name)
		{
			DerObjectIdentifier oid = NistNamedCurves.GetOid(name);
			if (oid != null)
			{
				return NistNamedCurves.GetByOid(oid);
			}
			return null;
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x001856BB File Offset: 0x001838BB
		public static X9ECParameters GetByOid(DerObjectIdentifier oid)
		{
			return SecNamedCurves.GetByOid(oid);
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x001856C3 File Offset: 0x001838C3
		public static DerObjectIdentifier GetOid(string name)
		{
			return (DerObjectIdentifier)NistNamedCurves.objIds[Platform.ToUpperInvariant(name)];
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x001856DA File Offset: 0x001838DA
		public static string GetName(DerObjectIdentifier oid)
		{
			return (string)NistNamedCurves.names[oid];
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06004249 RID: 16969 RVA: 0x001856EC File Offset: 0x001838EC
		public static IEnumerable Names
		{
			get
			{
				return new EnumerableProxy(NistNamedCurves.names.Values);
			}
		}

		// Token: 0x04002AE4 RID: 10980
		private static readonly IDictionary objIds = Platform.CreateHashtable();

		// Token: 0x04002AE5 RID: 10981
		private static readonly IDictionary names = Platform.CreateHashtable();
	}
}
