using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200062F RID: 1583
	public abstract class Asn1Set : Asn1Object, IEnumerable
	{
		// Token: 0x06003B8F RID: 15247 RVA: 0x0016EE8C File Offset: 0x0016D08C
		public static Asn1Set GetInstance(object obj)
		{
			if (obj == null || obj is Asn1Set)
			{
				return (Asn1Set)obj;
			}
			if (obj is Asn1SetParser)
			{
				return Asn1Set.GetInstance(((Asn1SetParser)obj).ToAsn1Object());
			}
			if (obj is byte[])
			{
				try
				{
					return Asn1Set.GetInstance(Asn1Object.FromByteArray((byte[])obj));
				}
				catch (IOException ex)
				{
					throw new ArgumentException("failed to construct set from byte[]: " + ex.Message);
				}
			}
			if (obj is Asn1Encodable)
			{
				Asn1Object asn1Object = ((Asn1Encodable)obj).ToAsn1Object();
				if (asn1Object is Asn1Set)
				{
					return (Asn1Set)asn1Object;
				}
			}
			throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x0016EF48 File Offset: 0x0016D148
		public static Asn1Set GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			Asn1Object @object = obj.GetObject();
			if (explicitly)
			{
				if (!obj.IsExplicit())
				{
					throw new ArgumentException("object implicit - explicit expected.");
				}
				return (Asn1Set)@object;
			}
			else
			{
				if (obj.IsExplicit())
				{
					return new DerSet(@object);
				}
				if (@object is Asn1Set)
				{
					return (Asn1Set)@object;
				}
				if (@object is Asn1Sequence)
				{
					Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
					foreach (object obj2 in ((Asn1Sequence)@object))
					{
						Asn1Encodable asn1Encodable = (Asn1Encodable)obj2;
						asn1EncodableVector.Add(new Asn1Encodable[]
						{
							asn1Encodable
						});
					}
					return new DerSet(asn1EncodableVector, false);
				}
				throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
			}
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x0016F024 File Offset: 0x0016D224
		protected internal Asn1Set(int capacity)
		{
			this._set = Platform.CreateArrayList(capacity);
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x0016F038 File Offset: 0x0016D238
		public virtual IEnumerator GetEnumerator()
		{
			return this._set.GetEnumerator();
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x0016F045 File Offset: 0x0016D245
		[Obsolete("Use GetEnumerator() instead")]
		public IEnumerator GetObjects()
		{
			return this.GetEnumerator();
		}

		// Token: 0x170007B9 RID: 1977
		public virtual Asn1Encodable this[int index]
		{
			get
			{
				return (Asn1Encodable)this._set[index];
			}
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x0016F060 File Offset: 0x0016D260
		[Obsolete("Use 'object[index]' syntax instead")]
		public Asn1Encodable GetObjectAt(int index)
		{
			return this[index];
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06003B96 RID: 15254 RVA: 0x0016F069 File Offset: 0x0016D269
		[Obsolete("Use 'Count' property instead")]
		public int Size
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06003B97 RID: 15255 RVA: 0x0016F071 File Offset: 0x0016D271
		public virtual int Count
		{
			get
			{
				return this._set.Count;
			}
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x0016F080 File Offset: 0x0016D280
		public virtual Asn1Encodable[] ToArray()
		{
			Asn1Encodable[] array = new Asn1Encodable[this.Count];
			for (int i = 0; i < this.Count; i++)
			{
				array[i] = this[i];
			}
			return array;
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06003B99 RID: 15257 RVA: 0x0016F0B5 File Offset: 0x0016D2B5
		public Asn1SetParser Parser
		{
			get
			{
				return new Asn1Set.Asn1SetParserImpl(this);
			}
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x0016F0C0 File Offset: 0x0016D2C0
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

		// Token: 0x06003B9B RID: 15259 RVA: 0x0016F130 File Offset: 0x0016D330
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			Asn1Set asn1Set = asn1Object as Asn1Set;
			if (asn1Set == null)
			{
				return false;
			}
			if (this.Count != asn1Set.Count)
			{
				return false;
			}
			IEnumerator enumerator = this.GetEnumerator();
			IEnumerator enumerator2 = asn1Set.GetEnumerator();
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

		// Token: 0x06003B9C RID: 15260 RVA: 0x0016F1A0 File Offset: 0x0016D3A0
		private Asn1Encodable GetCurrent(IEnumerator e)
		{
			Asn1Encodable asn1Encodable = (Asn1Encodable)e.Current;
			if (asn1Encodable == null)
			{
				return DerNull.Instance;
			}
			return asn1Encodable;
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x0016F1C4 File Offset: 0x0016D3C4
		protected internal void Sort()
		{
			if (this._set.Count < 2)
			{
				return;
			}
			Asn1Encodable[] array = new Asn1Encodable[this._set.Count];
			byte[][] array2 = new byte[this._set.Count][];
			for (int i = 0; i < this._set.Count; i++)
			{
				Asn1Encodable asn1Encodable = (Asn1Encodable)this._set[i];
				array[i] = asn1Encodable;
				array2[i] = asn1Encodable.GetEncoded("DER");
			}
			Array.Sort(array2, array, new Asn1Set.DerComparer());
			for (int j = 0; j < this._set.Count; j++)
			{
				this._set[j] = array[j];
			}
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x0016F273 File Offset: 0x0016D473
		protected internal void AddObject(Asn1Encodable obj)
		{
			this._set.Add(obj);
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x0016F282 File Offset: 0x0016D482
		public override string ToString()
		{
			return CollectionUtilities.ToString(this._set);
		}

		// Token: 0x0400269D RID: 9885
		private readonly IList _set;

		// Token: 0x0200098A RID: 2442
		private class Asn1SetParserImpl : Asn1SetParser, IAsn1Convertible
		{
			// Token: 0x06004FCC RID: 20428 RVA: 0x001B7BAF File Offset: 0x001B5DAF
			public Asn1SetParserImpl(Asn1Set outer)
			{
				this.outer = outer;
				this.max = outer.Count;
			}

			// Token: 0x06004FCD RID: 20429 RVA: 0x001B7BCC File Offset: 0x001B5DCC
			public IAsn1Convertible ReadObject()
			{
				if (this.index == this.max)
				{
					return null;
				}
				Asn1Set asn1Set = this.outer;
				int num = this.index;
				this.index = num + 1;
				Asn1Encodable asn1Encodable = asn1Set[num];
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

			// Token: 0x06004FCE RID: 20430 RVA: 0x001B7C2F File Offset: 0x001B5E2F
			public virtual Asn1Object ToAsn1Object()
			{
				return this.outer;
			}

			// Token: 0x0400370A RID: 14090
			private readonly Asn1Set outer;

			// Token: 0x0400370B RID: 14091
			private readonly int max;

			// Token: 0x0400370C RID: 14092
			private int index;
		}

		// Token: 0x0200098B RID: 2443
		private class DerComparer : IComparer
		{
			// Token: 0x06004FCF RID: 20431 RVA: 0x001B7C38 File Offset: 0x001B5E38
			public int Compare(object x, object y)
			{
				byte[] array = (byte[])x;
				byte[] array2 = (byte[])y;
				int num = Math.Min(array.Length, array2.Length);
				int num2 = 0;
				while (num2 != num)
				{
					byte b = array[num2];
					byte b2 = array2[num2];
					if (b != b2)
					{
						if (b >= b2)
						{
							return 1;
						}
						return -1;
					}
					else
					{
						num2++;
					}
				}
				if (array.Length > array2.Length)
				{
					if (!this.AllZeroesFrom(array, num))
					{
						return 1;
					}
					return 0;
				}
				else
				{
					if (array.Length >= array2.Length)
					{
						return 0;
					}
					if (!this.AllZeroesFrom(array2, num))
					{
						return -1;
					}
					return 0;
				}
			}

			// Token: 0x06004FD0 RID: 20432 RVA: 0x001B7CB2 File Offset: 0x001B5EB2
			private bool AllZeroesFrom(byte[] bs, int pos)
			{
				while (pos < bs.Length)
				{
					if (bs[pos++] != 0)
					{
						return false;
					}
				}
				return true;
			}
		}
	}
}
