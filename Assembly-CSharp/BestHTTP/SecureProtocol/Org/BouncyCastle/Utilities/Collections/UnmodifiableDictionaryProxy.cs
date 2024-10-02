using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x0200029E RID: 670
	public class UnmodifiableDictionaryProxy : UnmodifiableDictionary
	{
		// Token: 0x06001857 RID: 6231 RVA: 0x000B9E33 File Offset: 0x000B8033
		public UnmodifiableDictionaryProxy(IDictionary d)
		{
			this.d = d;
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x000B9E42 File Offset: 0x000B8042
		public override bool Contains(object k)
		{
			return this.d.Contains(k);
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x000B9E50 File Offset: 0x000B8050
		public override void CopyTo(Array array, int index)
		{
			this.d.CopyTo(array, index);
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x0600185A RID: 6234 RVA: 0x000B9E5F File Offset: 0x000B805F
		public override int Count
		{
			get
			{
				return this.d.Count;
			}
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x000B9E6C File Offset: 0x000B806C
		public override IDictionaryEnumerator GetEnumerator()
		{
			return this.d.GetEnumerator();
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x0600185C RID: 6236 RVA: 0x000B9E79 File Offset: 0x000B8079
		public override bool IsFixedSize
		{
			get
			{
				return this.d.IsFixedSize;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x0600185D RID: 6237 RVA: 0x000B9E86 File Offset: 0x000B8086
		public override bool IsSynchronized
		{
			get
			{
				return this.d.IsSynchronized;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x0600185E RID: 6238 RVA: 0x000B9E93 File Offset: 0x000B8093
		public override object SyncRoot
		{
			get
			{
				return this.d.SyncRoot;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x0600185F RID: 6239 RVA: 0x000B9EA0 File Offset: 0x000B80A0
		public override ICollection Keys
		{
			get
			{
				return this.d.Keys;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06001860 RID: 6240 RVA: 0x000B9EAD File Offset: 0x000B80AD
		public override ICollection Values
		{
			get
			{
				return this.d.Values;
			}
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x000B9EBA File Offset: 0x000B80BA
		protected override object GetValue(object k)
		{
			return this.d[k];
		}

		// Token: 0x04001832 RID: 6194
		private readonly IDictionary d;
	}
}
