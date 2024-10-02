using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x0200029D RID: 669
	public abstract class UnmodifiableDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06001846 RID: 6214 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public virtual void Add(object k, object v)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public virtual void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001848 RID: 6216
		public abstract bool Contains(object k);

		// Token: 0x06001849 RID: 6217
		public abstract void CopyTo(Array array, int index);

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x0600184A RID: 6218
		public abstract int Count { get; }

		// Token: 0x0600184B RID: 6219 RVA: 0x000B9E22 File Offset: 0x000B8022
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600184C RID: 6220
		public abstract IDictionaryEnumerator GetEnumerator();

		// Token: 0x0600184D RID: 6221 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public virtual void Remove(object k)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x0600184E RID: 6222
		public abstract bool IsFixedSize { get; }

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x0600184F RID: 6223 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06001850 RID: 6224
		public abstract bool IsSynchronized { get; }

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06001851 RID: 6225
		public abstract object SyncRoot { get; }

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06001852 RID: 6226
		public abstract ICollection Keys { get; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06001853 RID: 6227
		public abstract ICollection Values { get; }

		// Token: 0x170002FC RID: 764
		public virtual object this[object k]
		{
			get
			{
				return this.GetValue(k);
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06001856 RID: 6230
		protected abstract object GetValue(object k);
	}
}
