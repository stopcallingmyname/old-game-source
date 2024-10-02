using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x020002A1 RID: 673
	public abstract class UnmodifiableSet : ISet, ICollection, IEnumerable
	{
		// Token: 0x0600187F RID: 6271 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public virtual void Add(object o)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public virtual void AddAll(IEnumerable e)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public virtual void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001882 RID: 6274
		public abstract bool Contains(object o);

		// Token: 0x06001883 RID: 6275
		public abstract void CopyTo(Array array, int index);

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06001884 RID: 6276
		public abstract int Count { get; }

		// Token: 0x06001885 RID: 6277
		public abstract IEnumerator GetEnumerator();

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06001886 RID: 6278
		public abstract bool IsEmpty { get; }

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06001887 RID: 6279
		public abstract bool IsFixedSize { get; }

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06001888 RID: 6280 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06001889 RID: 6281
		public abstract bool IsSynchronized { get; }

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x0600188A RID: 6282
		public abstract object SyncRoot { get; }

		// Token: 0x0600188B RID: 6283 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public virtual void Remove(object o)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public virtual void RemoveAll(IEnumerable e)
		{
			throw new NotSupportedException();
		}
	}
}
