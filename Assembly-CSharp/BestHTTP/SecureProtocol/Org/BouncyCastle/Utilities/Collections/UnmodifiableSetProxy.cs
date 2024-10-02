using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x020002A2 RID: 674
	public class UnmodifiableSetProxy : UnmodifiableSet
	{
		// Token: 0x0600188D RID: 6285 RVA: 0x000B9F5A File Offset: 0x000B815A
		public UnmodifiableSetProxy(ISet s)
		{
			this.s = s;
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x000B9F69 File Offset: 0x000B8169
		public override bool Contains(object o)
		{
			return this.s.Contains(o);
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x000B9F77 File Offset: 0x000B8177
		public override void CopyTo(Array array, int index)
		{
			this.s.CopyTo(array, index);
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06001890 RID: 6288 RVA: 0x000B9F86 File Offset: 0x000B8186
		public override int Count
		{
			get
			{
				return this.s.Count;
			}
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x000B9F93 File Offset: 0x000B8193
		public override IEnumerator GetEnumerator()
		{
			return this.s.GetEnumerator();
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06001892 RID: 6290 RVA: 0x000B9FA0 File Offset: 0x000B81A0
		public override bool IsEmpty
		{
			get
			{
				return this.s.IsEmpty;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06001893 RID: 6291 RVA: 0x000B9FAD File Offset: 0x000B81AD
		public override bool IsFixedSize
		{
			get
			{
				return this.s.IsFixedSize;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06001894 RID: 6292 RVA: 0x000B9FBA File Offset: 0x000B81BA
		public override bool IsSynchronized
		{
			get
			{
				return this.s.IsSynchronized;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06001895 RID: 6293 RVA: 0x000B9FC7 File Offset: 0x000B81C7
		public override object SyncRoot
		{
			get
			{
				return this.s.SyncRoot;
			}
		}

		// Token: 0x04001834 RID: 6196
		private readonly ISet s;
	}
}
