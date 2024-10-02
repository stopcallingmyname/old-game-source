using System;

namespace BestHTTP.Futures
{
	// Token: 0x020007E5 RID: 2021
	public interface IFuture<T>
	{
		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x060047D3 RID: 18387
		FutureState state { get; }

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x060047D4 RID: 18388
		T value { get; }

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x060047D5 RID: 18389
		Exception error { get; }

		// Token: 0x060047D6 RID: 18390
		IFuture<T> OnItem(FutureValueCallback<T> callback);

		// Token: 0x060047D7 RID: 18391
		IFuture<T> OnSuccess(FutureValueCallback<T> callback);

		// Token: 0x060047D8 RID: 18392
		IFuture<T> OnError(FutureErrorCallback callback);

		// Token: 0x060047D9 RID: 18393
		IFuture<T> OnComplete(FutureCallback<T> callback);
	}
}
