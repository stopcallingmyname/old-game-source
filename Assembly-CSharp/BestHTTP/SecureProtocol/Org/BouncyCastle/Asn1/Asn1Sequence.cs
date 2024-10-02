using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200062D RID: 1581
	public abstract class Asn1Sequence : Asn1Object, IEnumerable
	{
		// Token: 0x06003B7F RID: 15231 RVA: 0x0016EBD4 File Offset: 0x0016CDD4
		public static Asn1Sequence GetInstance(object obj)
		{
			if (obj == null || obj is Asn1Sequence)
			{
				return (Asn1Sequence)obj;
			}
			if (obj is Asn1SequenceParser)
			{
				return Asn1Sequence.GetInstance(((Asn1SequenceParser)obj).ToAsn1Object());
			}
			if (obj is byte[])
			{
				try
				{
					return Asn1Sequence.GetInstance(Asn1Object.FromByteArray((byte[])obj));
				}
				catch (IOException ex)
				{
					throw new ArgumentException("failed to construct sequence from byte[]: " + ex.Message);
				}
			}
			if (obj is Asn1Encodable)
			{
				Asn1Object asn1Object = ((Asn1Encodable)obj).ToAsn1Object();
				if (asn1Object is Asn1Sequence)
				{
					return (Asn1Sequence)asn1Object;
				}
			}
			throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x0016EC90 File Offset: 0x0016CE90
		public static Asn1Sequence GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			Asn1Object @object = obj.GetObject();
			if (explicitly)
			{
				if (!obj.IsExplicit())
				{
					throw new ArgumentException("object implicit - explicit expected.");
				}
				return (Asn1Sequence)@object;
			}
			else if (obj.IsExplicit())
			{
				if (obj is BerTaggedObject)
				{
					return new BerSequence(@object);
				}
				return new DerSequence(@object);
			}
			else
			{
				if (@object is Asn1Sequence)
				{
					return (Asn1Sequence)@object;
				}
				throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
			}
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x0016ED08 File Offset: 0x0016CF08
		protected internal Asn1Sequence(int capacity)
		{
			this.seq = Platform.CreateArrayList(capacity);
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x0016ED1C File Offset: 0x0016CF1C
		public virtual IEnumerator GetEnumerator()
		{
			return this.seq.GetEnumerator();
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x0016ED29 File Offset: 0x0016CF29
		[Obsolete("Use GetEnumerator() instead")]
		public IEnumerator GetObjects()
		{
			return this.GetEnumerator();
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06003B84 RID: 15236 RVA: 0x0016ED31 File Offset: 0x0016CF31
		public virtual Asn1SequenceParser Parser
		{
			get
			{
				return new Asn1Sequence.Asn1SequenceParserImpl(this);
			}
		}

		// Token: 0x170007B6 RID: 1974
		public virtual Asn1Encodable this[int index]
		{
			get
			{
				return (Asn1Encodable)this.seq[index];
			}
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x0016ED4C File Offset: 0x0016CF4C
		[Obsolete("Use 'object[index]' syntax instead")]
		public Asn1Encodable GetObjectAt(int index)
		{
			return this[index];
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06003B87 RID: 15239 RVA: 0x0016ED55 File Offset: 0x0016CF55
		[Obsolete("Use 'Count' property instead")]
		public int Size
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06003B88 RID: 15240 RVA: 0x0016ED5D File Offset: 0x0016CF5D
		public virtual int Count
		{
			get
			{
				return this.seq.Count;
			}
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x0016ED6C File Offset: 0x0016CF6C
		protected override int Asn1GetHashCode()
		{
			int num = this.Count;
			foreach (object obj in this)
			{
				num *= 17;
				if (obj == null)
				{
					num ^= DerNull.Instance.GetHashCode();
				}
				else
				{
					num ^= obj.GetHashCode();
				}
			}
			return num;
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x0016EDDC File Offset: 0x0016CFDC
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			Asn1Sequence asn1Sequence = asn1Object as Asn1Sequence;
			if (asn1Sequence == null)
			{
				return false;
			}
			if (this.Count != asn1Sequence.Count)
			{
				return false;
			}
			IEnumerator enumerator = this.GetEnumerator();
			IEnumerator enumerator2 = asn1Sequence.GetEnumerator();
			while (enumerator.MoveNext() && enumerator2.MoveNext())
			{
				object obj = this.GetCurrent(enumerator).ToAsn1Object();
				Asn1Object obj2 = this.GetCurrent(enumerator2).ToAsn1Object();
				if (!obj.Equals(obj2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x0016EE4C File Offset: 0x0016D04C
		private Asn1Encodable GetCurrent(IEnumerator e)
		{
			Asn1Encodable asn1Encodable = (Asn1Encodable)e.Current;
			if (asn1Encodable == null)
			{
				return DerNull.Instance;
			}
			return asn1Encodable;
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x0016EE6F File Offset: 0x0016D06F
		protected internal void AddObject(Asn1Encodable obj)
		{
			this.seq.Add(obj);
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x0016EE7E File Offset: 0x0016D07E
		public override string ToString()
		{
			return CollectionUtilities.ToString(this.seq);
		}

		// Token: 0x0400269C RID: 9884
		private readonly IList seq;

		// Token: 0x02000989 RID: 2441
		private class Asn1SequenceParserImpl : Asn1SequenceParser, IAsn1Convertible
		{
			// Token: 0x06004FC9 RID: 20425 RVA: 0x001B7B28 File Offset: 0x001B5D28
			public Asn1SequenceParserImpl(Asn1Sequence outer)
			{
				this.outer = outer;
				this.max = outer.Count;
			}

			// Token: 0x06004FCA RID: 20426 RVA: 0x001B7B44 File Offset: 0x001B5D44
			public IAsn1Convertible ReadObject()
			{
				if (this.index == this.max)
				{
					return null;
				}
				Asn1Sequence asn1Sequence = this.outer;
				int num = this.index;
				this.index = num + 1;
				Asn1Encodable asn1Encodable = asn1Sequence[num];
				if (asn1Encodable is Asn1Sequence)
				{
					return ((Asn1Sequence)asn1Encodable).Parser;
				}
				if (asn1Encodable is Asn1Set)
				{
					return ((Asn1Set)asn1Encodable).Parser;
				}
				return asn1Encodable;
			}

			// Token: 0x06004FCB RID: 20427 RVA: 0x001B7BA7 File Offset: 0x001B5DA7
			public Asn1Object ToAsn1Object()
			{
				return this.outer;
			}

			// Token: 0x04003707 RID: 14087
			private readonly Asn1Sequence outer;

			// Token: 0x04003708 RID: 14088
			private readonly int max;

			// Token: 0x04003709 RID: 14089
			private int index;
		}
	}
}
