using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Smime
{
	// Token: 0x020006E1 RID: 1761
	public class SmimeCapabilities : Asn1Encodable
	{
		// Token: 0x060040BA RID: 16570 RVA: 0x00180468 File Offset: 0x0017E668
		public static SmimeCapabilities GetInstance(object obj)
		{
			if (obj == null || obj is SmimeCapabilities)
			{
				return (SmimeCapabilities)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new SmimeCapabilities((Asn1Sequence)obj);
			}
			if (obj is AttributeX509)
			{
				return new SmimeCapabilities((Asn1Sequence)((AttributeX509)obj).AttrValues[0]);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x001804D9 File Offset: 0x0017E6D9
		public SmimeCapabilities(Asn1Sequence seq)
		{
			this.capabilities = seq;
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x001804E8 File Offset: 0x0017E6E8
		[Obsolete("Use 'GetCapabilitiesForOid' instead")]
		public ArrayList GetCapabilities(DerObjectIdentifier capability)
		{
			ArrayList arrayList = new ArrayList();
			this.DoGetCapabilitiesForOid(capability, arrayList);
			return arrayList;
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x00180504 File Offset: 0x0017E704
		public IList GetCapabilitiesForOid(DerObjectIdentifier capability)
		{
			IList list = Platform.CreateArrayList();
			this.DoGetCapabilitiesForOid(capability, list);
			return list;
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x00180520 File Offset: 0x0017E720
		private void DoGetCapabilitiesForOid(DerObjectIdentifier capability, IList list)
		{
			if (capability == null)
			{
				using (IEnumerator enumerator = this.capabilities.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						SmimeCapability instance = SmimeCapability.GetInstance(obj);
						list.Add(instance);
					}
					return;
				}
			}
			foreach (object obj2 in this.capabilities)
			{
				SmimeCapability instance2 = SmimeCapability.GetInstance(obj2);
				if (capability.Equals(instance2.CapabilityID))
				{
					list.Add(instance2);
				}
			}
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x001805D4 File Offset: 0x0017E7D4
		public override Asn1Object ToAsn1Object()
		{
			return this.capabilities;
		}

		// Token: 0x04002960 RID: 10592
		public static readonly DerObjectIdentifier PreferSignedData = PkcsObjectIdentifiers.PreferSignedData;

		// Token: 0x04002961 RID: 10593
		public static readonly DerObjectIdentifier CannotDecryptAny = PkcsObjectIdentifiers.CannotDecryptAny;

		// Token: 0x04002962 RID: 10594
		public static readonly DerObjectIdentifier SmimeCapabilitesVersions = PkcsObjectIdentifiers.SmimeCapabilitiesVersions;

		// Token: 0x04002963 RID: 10595
		public static readonly DerObjectIdentifier Aes256Cbc = NistObjectIdentifiers.IdAes256Cbc;

		// Token: 0x04002964 RID: 10596
		public static readonly DerObjectIdentifier Aes192Cbc = NistObjectIdentifiers.IdAes192Cbc;

		// Token: 0x04002965 RID: 10597
		public static readonly DerObjectIdentifier Aes128Cbc = NistObjectIdentifiers.IdAes128Cbc;

		// Token: 0x04002966 RID: 10598
		public static readonly DerObjectIdentifier IdeaCbc = new DerObjectIdentifier("1.3.6.1.4.1.188.7.1.1.2");

		// Token: 0x04002967 RID: 10599
		public static readonly DerObjectIdentifier Cast5Cbc = new DerObjectIdentifier("1.2.840.113533.7.66.10");

		// Token: 0x04002968 RID: 10600
		public static readonly DerObjectIdentifier DesCbc = new DerObjectIdentifier("1.3.14.3.2.7");

		// Token: 0x04002969 RID: 10601
		public static readonly DerObjectIdentifier DesEde3Cbc = PkcsObjectIdentifiers.DesEde3Cbc;

		// Token: 0x0400296A RID: 10602
		public static readonly DerObjectIdentifier RC2Cbc = PkcsObjectIdentifiers.RC2Cbc;

		// Token: 0x0400296B RID: 10603
		private Asn1Sequence capabilities;
	}
}
