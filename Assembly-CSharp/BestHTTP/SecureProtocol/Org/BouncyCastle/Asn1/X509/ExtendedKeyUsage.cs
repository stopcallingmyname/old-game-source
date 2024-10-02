using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200069B RID: 1691
	public class ExtendedKeyUsage : Asn1Encodable
	{
		// Token: 0x06003E83 RID: 16003 RVA: 0x001772DD File Offset: 0x001754DD
		public static ExtendedKeyUsage GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return ExtendedKeyUsage.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x001772EC File Offset: 0x001754EC
		public static ExtendedKeyUsage GetInstance(object obj)
		{
			if (obj is ExtendedKeyUsage)
			{
				return (ExtendedKeyUsage)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new ExtendedKeyUsage((Asn1Sequence)obj);
			}
			if (obj is X509Extension)
			{
				return ExtendedKeyUsage.GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));
			}
			throw new ArgumentException("Invalid ExtendedKeyUsage: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x0017734C File Offset: 0x0017554C
		private ExtendedKeyUsage(Asn1Sequence seq)
		{
			this.seq = seq;
			foreach (object obj in seq)
			{
				if (!(obj is DerObjectIdentifier))
				{
					throw new ArgumentException("Only DerObjectIdentifier instances allowed in ExtendedKeyUsage.");
				}
				this.usageTable[obj] = obj;
			}
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x001773CC File Offset: 0x001755CC
		public ExtendedKeyUsage(params KeyPurposeID[] usages)
		{
			this.seq = new DerSequence(usages);
			foreach (KeyPurposeID keyPurposeID in usages)
			{
				this.usageTable[keyPurposeID] = keyPurposeID;
			}
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x00177419 File Offset: 0x00175619
		[Obsolete]
		public ExtendedKeyUsage(ArrayList usages) : this(usages)
		{
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x00177424 File Offset: 0x00175624
		public ExtendedKeyUsage(IEnumerable usages)
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in usages)
			{
				Asn1Encodable instance = DerObjectIdentifier.GetInstance(obj);
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					instance
				});
				this.usageTable[instance] = instance;
			}
			this.seq = new DerSequence(asn1EncodableVector);
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x001774B8 File Offset: 0x001756B8
		public bool HasKeyPurposeId(KeyPurposeID keyPurposeId)
		{
			return this.usageTable.Contains(keyPurposeId);
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x001774C6 File Offset: 0x001756C6
		[Obsolete("Use 'GetAllUsages'")]
		public ArrayList GetUsages()
		{
			return new ArrayList(this.usageTable.Values);
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x001774D8 File Offset: 0x001756D8
		public IList GetAllUsages()
		{
			return Platform.CreateArrayList(this.usageTable.Values);
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06003E8C RID: 16012 RVA: 0x001774EA File Offset: 0x001756EA
		public int Count
		{
			get
			{
				return this.usageTable.Count;
			}
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x001774F7 File Offset: 0x001756F7
		public override Asn1Object ToAsn1Object()
		{
			return this.seq;
		}

		// Token: 0x040027B6 RID: 10166
		internal readonly IDictionary usageTable = Platform.CreateHashtable();

		// Token: 0x040027B7 RID: 10167
		internal readonly Asn1Sequence seq;
	}
}
