using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Anssi;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.GM;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Sec;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.TeleTrust;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000677 RID: 1655
	public class ECNamedCurveTable
	{
		// Token: 0x06003D79 RID: 15737 RVA: 0x00173FA8 File Offset: 0x001721A8
		public static X9ECParameters GetByName(string name)
		{
			X9ECParameters x9ECParameters = X962NamedCurves.GetByName(name);
			if (x9ECParameters == null)
			{
				x9ECParameters = SecNamedCurves.GetByName(name);
			}
			if (x9ECParameters == null)
			{
				x9ECParameters = NistNamedCurves.GetByName(name);
			}
			if (x9ECParameters == null)
			{
				x9ECParameters = TeleTrusTNamedCurves.GetByName(name);
			}
			if (x9ECParameters == null)
			{
				x9ECParameters = AnssiNamedCurves.GetByName(name);
			}
			if (x9ECParameters == null)
			{
				x9ECParameters = ECNamedCurveTable.FromDomainParameters(ECGost3410NamedCurves.GetByName(name));
			}
			if (x9ECParameters == null)
			{
				x9ECParameters = GMNamedCurves.GetByName(name);
			}
			return x9ECParameters;
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x00174000 File Offset: 0x00172200
		public static string GetName(DerObjectIdentifier oid)
		{
			string name = X962NamedCurves.GetName(oid);
			if (name == null)
			{
				name = SecNamedCurves.GetName(oid);
			}
			if (name == null)
			{
				name = NistNamedCurves.GetName(oid);
			}
			if (name == null)
			{
				name = TeleTrusTNamedCurves.GetName(oid);
			}
			if (name == null)
			{
				name = AnssiNamedCurves.GetName(oid);
			}
			if (name == null)
			{
				name = ECGost3410NamedCurves.GetName(oid);
			}
			if (name == null)
			{
				name = GMNamedCurves.GetName(oid);
			}
			return name;
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x00174054 File Offset: 0x00172254
		public static DerObjectIdentifier GetOid(string name)
		{
			DerObjectIdentifier oid = X962NamedCurves.GetOid(name);
			if (oid == null)
			{
				oid = SecNamedCurves.GetOid(name);
			}
			if (oid == null)
			{
				oid = NistNamedCurves.GetOid(name);
			}
			if (oid == null)
			{
				oid = TeleTrusTNamedCurves.GetOid(name);
			}
			if (oid == null)
			{
				oid = AnssiNamedCurves.GetOid(name);
			}
			if (oid == null)
			{
				oid = ECGost3410NamedCurves.GetOid(name);
			}
			if (oid == null)
			{
				oid = GMNamedCurves.GetOid(name);
			}
			return oid;
		}

		// Token: 0x06003D7C RID: 15740 RVA: 0x001740A8 File Offset: 0x001722A8
		public static X9ECParameters GetByOid(DerObjectIdentifier oid)
		{
			X9ECParameters x9ECParameters = X962NamedCurves.GetByOid(oid);
			if (x9ECParameters == null)
			{
				x9ECParameters = SecNamedCurves.GetByOid(oid);
			}
			if (x9ECParameters == null)
			{
				x9ECParameters = TeleTrusTNamedCurves.GetByOid(oid);
			}
			if (x9ECParameters == null)
			{
				x9ECParameters = AnssiNamedCurves.GetByOid(oid);
			}
			if (x9ECParameters == null)
			{
				x9ECParameters = ECNamedCurveTable.FromDomainParameters(ECGost3410NamedCurves.GetByOid(oid));
			}
			if (x9ECParameters == null)
			{
				x9ECParameters = GMNamedCurves.GetByOid(oid);
			}
			return x9ECParameters;
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06003D7D RID: 15741 RVA: 0x001740F4 File Offset: 0x001722F4
		public static IEnumerable Names
		{
			get
			{
				IList list = Platform.CreateArrayList();
				CollectionUtilities.AddRange(list, X962NamedCurves.Names);
				CollectionUtilities.AddRange(list, SecNamedCurves.Names);
				CollectionUtilities.AddRange(list, NistNamedCurves.Names);
				CollectionUtilities.AddRange(list, TeleTrusTNamedCurves.Names);
				CollectionUtilities.AddRange(list, AnssiNamedCurves.Names);
				CollectionUtilities.AddRange(list, ECGost3410NamedCurves.Names);
				CollectionUtilities.AddRange(list, GMNamedCurves.Names);
				return list;
			}
		}

		// Token: 0x06003D7E RID: 15742 RVA: 0x00174153 File Offset: 0x00172353
		private static X9ECParameters FromDomainParameters(ECDomainParameters dp)
		{
			if (dp != null)
			{
				return new X9ECParameters(dp.Curve, dp.G, dp.N, dp.H, dp.GetSeed());
			}
			return null;
		}
	}
}
