using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x0200029B RID: 667
	public class LinkedDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x0600182D RID: 6189 RVA: 0x000B9B7B File Offset: 0x000B7D7B
		public virtual void Add(object k, object v)
		{
			this.hash.Add(k, v);
			this.keys.Add(k);
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x000B9B97 File Offset: 0x000B7D97
		public virtual void Clear()
		{
			this.hash.Clear();
			this.keys.Clear();
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x000B9BAF File Offset: 0x000B7DAF
		public virtual bool Contains(object k)
		{
			return this.hash.Contains(k);
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x000B9BC0 File Offset: 0x000B7DC0
		public virtual void CopyTo(Array array, int index)
		{
			foreach (object key in this.keys)
			{
				array.SetValue(this.hash[key], index++);
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06001831 RID: 6193 RVA: 0x000B9C28 File Offset: 0x000B7E28
		public virtual int Count
		{
			get
			{
				return this.hash.Count;
			}
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x000B9C35 File Offset: 0x000B7E35
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x000B9C3D File Offset: 0x000B7E3D
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new LinkedDictionaryEnumerator(this);
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x000B9C45 File Offset: 0x000B7E45
		public virtual void Remove(object k)
		{
			this.hash.Remove(k);
			this.keys.Remove(k);
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001835 RID: 6197 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001836 RID: 6198 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06001837 RID: 6199 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001838 RID: 6200 RVA: 0x000B9C5F File Offset: 0x000B7E5F
		public virtual object SyncRoot
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06001839 RID: 6201 RVA: 0x000B9C67 File Offset: 0x000B7E67
		public virtual ICollection Keys
		{
			get
			{
				return Platform.CreateArrayList(this.keys);
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x0600183A RID: 6202 RVA: 0x000B9C74 File Offset: 0x000B7E74
		public virtual ICollection Values
		{
			get
			{
				IList list = Platform.CreateArrayList(this.keys.Count);
				foreach (object key in this.keys)
				{
					list.Add(this.hash[key]);
				}
				return list;
			}
		}

		// Token: 0x170002EF RID: 751
		public virtual object this[object k]
		{
			get
			{
				return this.hash[k];
			}
			set
			{
				if (!this.hash.Contains(k))
				{
					this.keys.Add(k);
				}
				this.hash[k] = value;
			}
		}

		// Token: 0x0400182E RID: 6190
		internal readonly IDictionary hash = Platform.CreateHashtable();

		// Token: 0x0400182F RID: 6191
		internal readonly IList keys = Platform.CreateArrayList();
	}
}
