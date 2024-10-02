using System;

namespace PlatformSupport.Collections.Specialized
{
	// Token: 0x0200016C RID: 364
	public interface INotifyCollectionChanged
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000CAF RID: 3247
		// (remove) Token: 0x06000CB0 RID: 3248
		event NotifyCollectionChangedEventHandler CollectionChanged;
	}
}
