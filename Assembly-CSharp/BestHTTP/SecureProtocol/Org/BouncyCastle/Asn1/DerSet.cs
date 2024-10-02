using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200065F RID: 1631
	public class DerSet : Asn1Set
	{
		// Token: 0x06003CF2 RID: 15602 RVA: 0x00172CEC File Offset: 0x00170EEC
		public static DerSet FromVector(Asn1EncodableVector v)
		{
			if (v.Count >= 1)
			{
				return new DerSet(v);
			}
			return DerSet.Empty;
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x00172D03 File Offset: 0x00170F03
		internal static DerSet FromVector(Asn1EncodableVector v, bool needsSorting)
		{
			if (v.Count >= 1)
			{
				return new DerSet(v, needsSorting);
			}
			return DerSet.Empty;
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x00172D1B File Offset: 0x00170F1B
		public DerSet() : base(0)
		{
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x00172D24 File Offset: 0x00170F24
		public DerSet(Asn1Encodable obj) : base(1)
		{
			base.AddObject(obj);
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x00172D34 File Offset: 0x00170F34
		public DerSet(params Asn1Encodable[] v) : base(v.Length)
		{
			foreach (Asn1Encodable obj in v)
			{
				base.AddObject(obj);
			}
			base.Sort();
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x00172D6B File Offset: 0x00170F6B
		public DerSet(Asn1EncodableVector v) : this(v, true)
		{
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x00172D78 File Offset: 0x00170F78
		internal DerSet(Asn1EncodableVector v, bool needsSorting) : base(v.Count)
		{
			foreach (object obj in v)
			{
				Asn1Encodable obj2 = (Asn1Encodable)obj;
				base.AddObject(obj2);
			}
			if (needsSorting)
			{
				base.Sort();
			}
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x00172DE4 File Offset: 0x00170FE4
		internal override void Encode(DerOutputStream derOut)
		{
			MemoryStream memoryStream = new MemoryStream();
			DerOutputStream derOutputStream = new DerOutputStream(memoryStream);
			foreach (object obj in this)
			{
				Asn1Encodable obj2 = (Asn1Encodable)obj;
				derOutputStream.WriteObject(obj2);
			}
			Platform.Dispose(derOutputStream);
			byte[] bytes = memoryStream.ToArray();
			derOut.WriteEncoded(49, bytes);
		}

		// Token: 0x040026FD RID: 9981
		public static readonly DerSet Empty = new DerSet();
	}
}
