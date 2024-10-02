using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x0200029C RID: 668
	internal class LinkedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x0600183D RID: 6205 RVA: 0x000B9D20 File Offset: 0x000B7F20
		internal LinkedDictionaryEnumerator(LinkedDictionary parent)
		{
			this.parent = parent;
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x0600183E RID: 6206 RVA: 0x000B9D36 File Offset: 0x000B7F36
		public virtual object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x0600183F RID: 6207 RVA: 0x000B9D44 File Offset: 0x000B7F44
		public virtual DictionaryEntry Entry
		{
			get
			{
				object currentKey = this.CurrentKey;
				return new DictionaryEntry(currentKey, this.parent.hash[currentKey]);
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001840 RID: 6208 RVA: 0x000B9D6F File Offset: 0x000B7F6F
		public virtual object Key
		{
			get
			{
				return this.CurrentKey;
			}
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x000B9D78 File Offset: 0x000B7F78
		public virtual bool MoveNext()
		{
			if (this.pos >= this.parent.keys.Count)
			{
				return false;
			}
			int num = this.pos + 1;
			this.pos = num;
			return num < this.parent.keys.Count;
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x000B9DC2 File Offset: 0x000B7FC2
		public virtual void Reset()
		{
			this.pos = -1;
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001843 RID: 6211 RVA: 0x000B9DCB File Offset: 0x000B7FCB
		public virtual object Value
		{
			get
			{
				return this.parent.hash[this.CurrentKey];
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06001844 RID: 6212 RVA: 0x000B9DE3 File Offset: 0x000B7FE3
		private object CurrentKey
		{
			get
			{
				if (this.pos < 0 || this.pos >= this.parent.keys.Count)
				{
					throw new InvalidOperationException();
				}
				return this.parent.keys[this.pos];
			}
		}

		// Token: 0x04001830 RID: 6192
		private readonly LinkedDictionary parent;

		// Token: 0x04001831 RID: 6193
		private int pos = -1;
	}
}
