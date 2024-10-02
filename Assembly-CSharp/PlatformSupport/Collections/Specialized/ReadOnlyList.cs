using System;
using System.Collections;

namespace PlatformSupport.Collections.Specialized
{
	// Token: 0x0200016F RID: 367
	internal sealed class ReadOnlyList : IList, ICollection, IEnumerable
	{
		// Token: 0x06000CC6 RID: 3270 RVA: 0x0008FED6 File Offset: 0x0008E0D6
		internal ReadOnlyList(IList list)
		{
			this._list = list;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0008FEE5 File Offset: 0x0008E0E5
		public int Count
		{
			get
			{
				return this._list.Count;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000CC8 RID: 3272 RVA: 0x0006AE98 File Offset: 0x00069098
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x0006AE98 File Offset: 0x00069098
		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x0008FEF2 File Offset: 0x0008E0F2
		public bool IsSynchronized
		{
			get
			{
				return this._list.IsSynchronized;
			}
		}

		// Token: 0x170000CF RID: 207
		public object this[int index]
		{
			get
			{
				return this._list[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x0008FF14 File Offset: 0x0008E114
		public object SyncRoot
		{
			get
			{
				return this._list.SyncRoot;
			}
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public int Add(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0008FF21 File Offset: 0x0008E121
		public bool Contains(object value)
		{
			return this._list.Contains(value);
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x0008FF2F File Offset: 0x0008E12F
		public void CopyTo(Array array, int index)
		{
			this._list.CopyTo(array, index);
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0008FF3E File Offset: 0x0008E13E
		public IEnumerator GetEnumerator()
		{
			return this._list.GetEnumerator();
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0008FF4B File Offset: 0x0008E14B
		public int IndexOf(object value)
		{
			return this._list.IndexOf(value);
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public void Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public void Remove(object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04001289 RID: 4745
		private readonly IList _list;
	}
}
