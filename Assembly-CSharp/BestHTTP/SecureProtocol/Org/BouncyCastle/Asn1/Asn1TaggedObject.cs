using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000632 RID: 1586
	public abstract class Asn1TaggedObject : Asn1Object, Asn1TaggedObjectParser, IAsn1Convertible
	{
		// Token: 0x06003BAA RID: 15274 RVA: 0x0016F660 File Offset: 0x0016D860
		internal static bool IsConstructed(bool isExplicit, Asn1Object obj)
		{
			if (isExplicit || obj is Asn1Sequence || obj is Asn1Set)
			{
				return true;
			}
			Asn1TaggedObject asn1TaggedObject = obj as Asn1TaggedObject;
			return asn1TaggedObject != null && Asn1TaggedObject.IsConstructed(asn1TaggedObject.IsExplicit(), asn1TaggedObject.GetObject());
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x0016F69F File Offset: 0x0016D89F
		public static Asn1TaggedObject GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			if (explicitly)
			{
				return (Asn1TaggedObject)obj.GetObject();
			}
			throw new ArgumentException("implicitly tagged tagged object");
		}

		// Token: 0x06003BAC RID: 15276 RVA: 0x0016F6BA File Offset: 0x0016D8BA
		public static Asn1TaggedObject GetInstance(object obj)
		{
			if (obj == null || obj is Asn1TaggedObject)
			{
				return (Asn1TaggedObject)obj;
			}
			throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003BAD RID: 15277 RVA: 0x0016F6E8 File Offset: 0x0016D8E8
		protected Asn1TaggedObject(int tagNo, Asn1Encodable obj)
		{
			this.explicitly = true;
			this.tagNo = tagNo;
			this.obj = obj;
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x0016F70C File Offset: 0x0016D90C
		protected Asn1TaggedObject(bool explicitly, int tagNo, Asn1Encodable obj)
		{
			this.explicitly = (explicitly || obj is IAsn1Choice);
			this.tagNo = tagNo;
			this.obj = obj;
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x0016F740 File Offset: 0x0016D940
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			Asn1TaggedObject asn1TaggedObject = asn1Object as Asn1TaggedObject;
			return asn1TaggedObject != null && (this.tagNo == asn1TaggedObject.tagNo && this.explicitly == asn1TaggedObject.explicitly) && object.Equals(this.GetObject(), asn1TaggedObject.GetObject());
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x0016F788 File Offset: 0x0016D988
		protected override int Asn1GetHashCode()
		{
			int num = this.tagNo.GetHashCode();
			if (this.obj != null)
			{
				num ^= this.obj.GetHashCode();
			}
			return num;
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06003BB1 RID: 15281 RVA: 0x0016F7B8 File Offset: 0x0016D9B8
		public int TagNo
		{
			get
			{
				return this.tagNo;
			}
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x0016F7C0 File Offset: 0x0016D9C0
		public bool IsExplicit()
		{
			return this.explicitly;
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsEmpty()
		{
			return false;
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x0016F7C8 File Offset: 0x0016D9C8
		public Asn1Object GetObject()
		{
			if (this.obj != null)
			{
				return this.obj.ToAsn1Object();
			}
			return null;
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x0016F7E0 File Offset: 0x0016D9E0
		public IAsn1Convertible GetObjectParser(int tag, bool isExplicit)
		{
			if (tag == 4)
			{
				return Asn1OctetString.GetInstance(this, isExplicit).Parser;
			}
			if (tag == 16)
			{
				return Asn1Sequence.GetInstance(this, isExplicit).Parser;
			}
			if (tag == 17)
			{
				return Asn1Set.GetInstance(this, isExplicit).Parser;
			}
			if (isExplicit)
			{
				return this.GetObject();
			}
			throw Platform.CreateNotImplementedException("implicit tagging for tag: " + tag);
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x0016F841 File Offset: 0x0016DA41
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"[",
				this.tagNo,
				"]",
				this.obj
			});
		}

		// Token: 0x040026A1 RID: 9889
		internal int tagNo;

		// Token: 0x040026A2 RID: 9890
		internal bool explicitly = true;

		// Token: 0x040026A3 RID: 9891
		internal Asn1Encodable obj;
	}
}
