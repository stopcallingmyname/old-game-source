using System;
using System.Collections.Generic;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001E2 RID: 482
	public sealed class StreamItemContainer<T>
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060011CA RID: 4554 RVA: 0x000A2AD2 File Offset: 0x000A0CD2
		// (set) Token: 0x060011CB RID: 4555 RVA: 0x000A2ADA File Offset: 0x000A0CDA
		public List<T> Items { get; private set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x000A2AE3 File Offset: 0x000A0CE3
		// (set) Token: 0x060011CD RID: 4557 RVA: 0x000A2AEB File Offset: 0x000A0CEB
		public T LastAdded { get; private set; }

		// Token: 0x060011CE RID: 4558 RVA: 0x000A2AF4 File Offset: 0x000A0CF4
		public StreamItemContainer(long _id)
		{
			this.id = _id;
			this.Items = new List<T>();
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x000A2B0E File Offset: 0x000A0D0E
		public void AddItem(T item)
		{
			if (this.Items == null)
			{
				this.Items = new List<T>();
			}
			this.Items.Add(item);
			this.LastAdded = item;
		}

		// Token: 0x040014F4 RID: 5364
		public readonly long id;

		// Token: 0x040014F7 RID: 5367
		public bool IsCanceled;
	}
}
