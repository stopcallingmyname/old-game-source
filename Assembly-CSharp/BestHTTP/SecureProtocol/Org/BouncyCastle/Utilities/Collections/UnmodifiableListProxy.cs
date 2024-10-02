using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x020002A0 RID: 672
	public class UnmodifiableListProxy : UnmodifiableList
	{
		// Token: 0x06001874 RID: 6260 RVA: 0x000B9ED1 File Offset: 0x000B80D1
		public UnmodifiableListProxy(IList l)
		{
			this.l = l;
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x000B9EE0 File Offset: 0x000B80E0
		public override bool Contains(object o)
		{
			return this.l.Contains(o);
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x000B9EEE File Offset: 0x000B80EE
		public override void CopyTo(Array array, int index)
		{
			this.l.CopyTo(array, index);
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06001877 RID: 6263 RVA: 0x000B9EFD File Offset: 0x000B80FD
		public override int Count
		{
			get
			{
				return this.l.Count;
			}
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x000B9F0A File Offset: 0x000B810A
		public override IEnumerator GetEnumerator()
		{
			return this.l.GetEnumerator();
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x000B9F17 File Offset: 0x000B8117
		public override int IndexOf(object o)
		{
			return this.l.IndexOf(o);
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x0600187A RID: 6266 RVA: 0x000B9F25 File Offset: 0x000B8125
		public override bool IsFixedSize
		{
			get
			{
				return this.l.IsFixedSize;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x0600187B RID: 6267 RVA: 0x000B9F32 File Offset: 0x000B8132
		public override bool IsSynchronized
		{
			get
			{
				return this.l.IsSynchronized;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x0600187C RID: 6268 RVA: 0x000B9F3F File Offset: 0x000B813F
		public override object SyncRoot
		{
			get
			{
				return this.l.SyncRoot;
			}
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x000B9F4C File Offset: 0x000B814C
		protected override object GetValue(int i)
		{
			return this.l[i];
		}

		// Token: 0x04001833 RID: 6195
		private readonly IList l;
	}
}
