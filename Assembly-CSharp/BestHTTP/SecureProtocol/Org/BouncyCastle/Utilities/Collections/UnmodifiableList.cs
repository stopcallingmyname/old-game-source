using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x0200029F RID: 671
	public abstract class UnmodifiableList : IList, ICollection, IEnumerable
	{
		// Token: 0x06001863 RID: 6243 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public virtual int Add(object o)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public virtual void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001865 RID: 6245
		public abstract bool Contains(object o);

		// Token: 0x06001866 RID: 6246
		public abstract void CopyTo(Array array, int index);

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06001867 RID: 6247
		public abstract int Count { get; }

		// Token: 0x06001868 RID: 6248
		public abstract IEnumerator GetEnumerator();

		// Token: 0x06001869 RID: 6249
		public abstract int IndexOf(object o);

		// Token: 0x0600186A RID: 6250 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public virtual void Insert(int i, object o)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x0600186B RID: 6251
		public abstract bool IsFixedSize { get; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x0600186C RID: 6252 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x0600186D RID: 6253
		public abstract bool IsSynchronized { get; }

		// Token: 0x0600186E RID: 6254 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public virtual void Remove(object o)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public virtual void RemoveAt(int i)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06001870 RID: 6256
		public abstract object SyncRoot { get; }

		// Token: 0x17000308 RID: 776
		public virtual object this[int i]
		{
			get
			{
				return this.GetValue(i);
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06001873 RID: 6259
		protected abstract object GetValue(int i);
	}
}
