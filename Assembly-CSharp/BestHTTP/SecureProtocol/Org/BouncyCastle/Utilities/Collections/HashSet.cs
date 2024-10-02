using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x02000299 RID: 665
	public class HashSet : ISet, ICollection, IEnumerable
	{
		// Token: 0x06001813 RID: 6163 RVA: 0x000B9999 File Offset: 0x000B7B99
		public HashSet()
		{
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x000B99AC File Offset: 0x000B7BAC
		public HashSet(IEnumerable s)
		{
			foreach (object o in s)
			{
				this.Add(o);
			}
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x000B9A0C File Offset: 0x000B7C0C
		public virtual void Add(object o)
		{
			this.impl[o] = null;
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x000B9A1C File Offset: 0x000B7C1C
		public virtual void AddAll(IEnumerable e)
		{
			foreach (object o in e)
			{
				this.Add(o);
			}
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x000B9A6C File Offset: 0x000B7C6C
		public virtual void Clear()
		{
			this.impl.Clear();
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x000B9A79 File Offset: 0x000B7C79
		public virtual bool Contains(object o)
		{
			return this.impl.Contains(o);
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x000B9A87 File Offset: 0x000B7C87
		public virtual void CopyTo(Array array, int index)
		{
			this.impl.Keys.CopyTo(array, index);
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x0600181A RID: 6170 RVA: 0x000B9A9B File Offset: 0x000B7C9B
		public virtual int Count
		{
			get
			{
				return this.impl.Count;
			}
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x000B9AA8 File Offset: 0x000B7CA8
		public virtual IEnumerator GetEnumerator()
		{
			return this.impl.Keys.GetEnumerator();
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x0600181C RID: 6172 RVA: 0x000B9ABA File Offset: 0x000B7CBA
		public virtual bool IsEmpty
		{
			get
			{
				return this.impl.Count == 0;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x0600181D RID: 6173 RVA: 0x000B9ACA File Offset: 0x000B7CCA
		public virtual bool IsFixedSize
		{
			get
			{
				return this.impl.IsFixedSize;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x0600181E RID: 6174 RVA: 0x000B9AD7 File Offset: 0x000B7CD7
		public virtual bool IsReadOnly
		{
			get
			{
				return this.impl.IsReadOnly;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x0600181F RID: 6175 RVA: 0x000B9AE4 File Offset: 0x000B7CE4
		public virtual bool IsSynchronized
		{
			get
			{
				return this.impl.IsSynchronized;
			}
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x000B9AF1 File Offset: 0x000B7CF1
		public virtual void Remove(object o)
		{
			this.impl.Remove(o);
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x000B9B00 File Offset: 0x000B7D00
		public virtual void RemoveAll(IEnumerable e)
		{
			foreach (object o in e)
			{
				this.Remove(o);
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06001822 RID: 6178 RVA: 0x000B9B50 File Offset: 0x000B7D50
		public virtual object SyncRoot
		{
			get
			{
				return this.impl.SyncRoot;
			}
		}

		// Token: 0x0400182D RID: 6189
		private readonly IDictionary impl = Platform.CreateHashtable();
	}
}
