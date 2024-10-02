using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000641 RID: 1601
	public class BerSet : DerSet
	{
		// Token: 0x06003BF1 RID: 15345 RVA: 0x0016FED2 File Offset: 0x0016E0D2
		public new static BerSet FromVector(Asn1EncodableVector v)
		{
			if (v.Count >= 1)
			{
				return new BerSet(v);
			}
			return BerSet.Empty;
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x0016FEE9 File Offset: 0x0016E0E9
		internal new static BerSet FromVector(Asn1EncodableVector v, bool needsSorting)
		{
			if (v.Count >= 1)
			{
				return new BerSet(v, needsSorting);
			}
			return BerSet.Empty;
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x0016FF01 File Offset: 0x0016E101
		public BerSet()
		{
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x0016FF09 File Offset: 0x0016E109
		public BerSet(Asn1Encodable obj) : base(obj)
		{
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x0016FF12 File Offset: 0x0016E112
		public BerSet(Asn1EncodableVector v) : base(v, false)
		{
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x0016FF1C File Offset: 0x0016E11C
		internal BerSet(Asn1EncodableVector v, bool needsSorting) : base(v, needsSorting)
		{
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x0016FF28 File Offset: 0x0016E128
		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteByte(49);
				derOut.WriteByte(128);
				foreach (object obj in this)
				{
					Asn1Encodable obj2 = (Asn1Encodable)obj;
					derOut.WriteObject(obj2);
				}
				derOut.WriteByte(0);
				derOut.WriteByte(0);
				return;
			}
			base.Encode(derOut);
		}

		// Token: 0x040026CB RID: 9931
		public new static readonly BerSet Empty = new BerSet();
	}
}
