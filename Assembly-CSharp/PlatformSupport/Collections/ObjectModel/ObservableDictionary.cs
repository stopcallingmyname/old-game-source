using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PlatformSupport.Collections.Specialized;

namespace PlatformSupport.Collections.ObjectModel
{
	// Token: 0x02000170 RID: 368
	public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x0008FF59 File Offset: 0x0008E159
		protected IDictionary<TKey, TValue> Dictionary
		{
			get
			{
				return this._Dictionary;
			}
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0008FF61 File Offset: 0x0008E161
		public ObservableDictionary()
		{
			this._Dictionary = new Dictionary<TKey, TValue>();
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0008FF74 File Offset: 0x0008E174
		public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
		{
			this._Dictionary = new Dictionary<TKey, TValue>(dictionary);
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0008FF88 File Offset: 0x0008E188
		public ObservableDictionary(IEqualityComparer<TKey> comparer)
		{
			this._Dictionary = new Dictionary<TKey, TValue>(comparer);
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0008FF9C File Offset: 0x0008E19C
		public ObservableDictionary(int capacity)
		{
			this._Dictionary = new Dictionary<TKey, TValue>(capacity);
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0008FFB0 File Offset: 0x0008E1B0
		public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		{
			this._Dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0008FFC5 File Offset: 0x0008E1C5
		public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			this._Dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0008FFDA File Offset: 0x0008E1DA
		public void Add(TKey key, TValue value)
		{
			this.Insert(key, value, true);
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0008FFE5 File Offset: 0x0008E1E5
		public bool ContainsKey(TKey key)
		{
			return this.Dictionary.ContainsKey(key);
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x0008FFF3 File Offset: 0x0008E1F3
		public ICollection<TKey> Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00090000 File Offset: 0x0008E200
		public bool Remove(TKey key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			TValue tvalue;
			this.Dictionary.TryGetValue(key, out tvalue);
			bool flag = this.Dictionary.Remove(key);
			if (flag)
			{
				this.OnCollectionChanged();
			}
			return flag;
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00090044 File Offset: 0x0008E244
		public bool TryGetValue(TKey key, out TValue value)
		{
			return this.Dictionary.TryGetValue(key, out value);
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x00090053 File Offset: 0x0008E253
		public ICollection<TValue> Values
		{
			get
			{
				return this.Dictionary.Values;
			}
		}

		// Token: 0x170000D4 RID: 212
		public TValue this[TKey key]
		{
			get
			{
				return this.Dictionary[key];
			}
			set
			{
				this.Insert(key, value, false);
			}
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00090079 File Offset: 0x0008E279
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			this.Insert(item.Key, item.Value, true);
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00090090 File Offset: 0x0008E290
		public void Clear()
		{
			if (this.Dictionary.Count > 0)
			{
				this.Dictionary.Clear();
				this.OnCollectionChanged();
			}
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x000900B1 File Offset: 0x0008E2B1
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return this.Dictionary.Contains(item);
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x000900BF File Offset: 0x0008E2BF
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.Dictionary.CopyTo(array, arrayIndex);
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x000900CE File Offset: 0x0008E2CE
		public int Count
		{
			get
			{
				return this.Dictionary.Count;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x000900DB File Offset: 0x0008E2DB
		public bool IsReadOnly
		{
			get
			{
				return this.Dictionary.IsReadOnly;
			}
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x000900E8 File Offset: 0x0008E2E8
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return this.Remove(item.Key);
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x000900F7 File Offset: 0x0008E2F7
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this.Dictionary.GetEnumerator();
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00090104 File Offset: 0x0008E304
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Dictionary.GetEnumerator();
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000CEF RID: 3311 RVA: 0x00090114 File Offset: 0x0008E314
		// (remove) Token: 0x06000CF0 RID: 3312 RVA: 0x0009014C File Offset: 0x0008E34C
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000CF1 RID: 3313 RVA: 0x00090184 File Offset: 0x0008E384
		// (remove) Token: 0x06000CF2 RID: 3314 RVA: 0x000901BC File Offset: 0x0008E3BC
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06000CF3 RID: 3315 RVA: 0x000901F4 File Offset: 0x0008E3F4
		public void AddRange(IDictionary<TKey, TValue> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			if (items.Count > 0)
			{
				if (this.Dictionary.Count > 0)
				{
					if (items.Keys.Any((TKey k) => this.Dictionary.ContainsKey(k)))
					{
						throw new ArgumentException("An item with the same key has already been added.");
					}
					using (IEnumerator<KeyValuePair<TKey, TValue>> enumerator = items.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<TKey, TValue> item = enumerator.Current;
							this.Dictionary.Add(item);
						}
						goto IL_85;
					}
				}
				this._Dictionary = new Dictionary<TKey, TValue>(items);
				IL_85:
				this.OnCollectionChanged(NotifyCollectionChangedAction.Add, items.ToArray<KeyValuePair<TKey, TValue>>());
			}
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x000902A4 File Offset: 0x0008E4A4
		private void Insert(TKey key, TValue value, bool add)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			TValue tvalue;
			if (!this.Dictionary.TryGetValue(key, out tvalue))
			{
				this.Dictionary[key] = value;
				this.OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
				return;
			}
			if (add)
			{
				throw new ArgumentException("An item with the same key has already been added.");
			}
			if (object.Equals(tvalue, value))
			{
				return;
			}
			this.Dictionary[key] = value;
			this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, tvalue));
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00090334 File Offset: 0x0008E534
		private void OnPropertyChanged()
		{
			this.OnPropertyChanged("Count");
			this.OnPropertyChanged("Item[]");
			this.OnPropertyChanged("Keys");
			this.OnPropertyChanged("Values");
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00090362 File Offset: 0x0008E562
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0009037E File Offset: 0x0008E57E
		private void OnCollectionChanged()
		{
			this.OnPropertyChanged();
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x000903A0 File Offset: 0x0008E5A0
		private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> changedItem)
		{
			this.OnPropertyChanged();
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, changedItem));
			}
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x000903C8 File Offset: 0x0008E5C8
		private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem)
		{
			this.OnPropertyChanged();
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
			}
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x000903F6 File Offset: 0x0008E5F6
		private void OnCollectionChanged(NotifyCollectionChangedAction action, IList newItems)
		{
			this.OnPropertyChanged();
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItems));
			}
		}

		// Token: 0x0400128A RID: 4746
		private const string CountString = "Count";

		// Token: 0x0400128B RID: 4747
		private const string IndexerName = "Item[]";

		// Token: 0x0400128C RID: 4748
		private const string KeysName = "Keys";

		// Token: 0x0400128D RID: 4749
		private const string ValuesName = "Values";

		// Token: 0x0400128E RID: 4750
		private IDictionary<TKey, TValue> _Dictionary;
	}
}
