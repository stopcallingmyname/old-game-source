using System;
using BestHTTP.Futures;
using BestHTTP.SignalRCore.Messages;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001EC RID: 492
	public sealed class UploadItemController<TResult> : IFuture<TResult>, IDisposable
	{
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06001239 RID: 4665 RVA: 0x000A43D9 File Offset: 0x000A25D9
		public FutureState state
		{
			get
			{
				return this.future.state;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600123A RID: 4666 RVA: 0x000A43E6 File Offset: 0x000A25E6
		public TResult value
		{
			get
			{
				return this.future.value;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600123B RID: 4667 RVA: 0x000A43F3 File Offset: 0x000A25F3
		public Exception error
		{
			get
			{
				return this.future.error;
			}
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x000A4400 File Offset: 0x000A2600
		public UploadItemController(HubConnection hub, long iId, int[] sIds, IFuture<TResult> future)
		{
			this.hubConnection = hub;
			this.invocationId = iId;
			this.streamingIds = sIds;
			this.streams = new object[this.streamingIds.Length];
			this.future = future;
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x000A4438 File Offset: 0x000A2638
		public UploadChannel<TResult, T> GetUploadChannel<T>(int paramIdx)
		{
			UploadChannel<TResult, T> uploadChannel = this.streams[paramIdx] as UploadChannel<TResult, T>;
			if (uploadChannel == null)
			{
				uploadChannel = (this.streams[paramIdx] = new UploadChannel<TResult, T>(this, paramIdx));
			}
			return uploadChannel;
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x000A4468 File Offset: 0x000A2668
		public void UploadParam<T>(int streamId, T item)
		{
			if (streamId == 0)
			{
				return;
			}
			Message message = new Message
			{
				type = MessageTypes.StreamItem,
				invocationId = streamId.ToString(),
				item = item
			};
			this.hubConnection.SendMessage(message);
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x000A44B4 File Offset: 0x000A26B4
		public void Finish()
		{
			if (!this.isFinished)
			{
				this.isFinished = true;
				for (int i = 0; i < this.streamingIds.Length; i++)
				{
					if (this.streamingIds[i] > 0)
					{
						Message message = new Message
						{
							type = MessageTypes.Completion,
							invocationId = this.streamingIds[i].ToString()
						};
						this.hubConnection.SendMessage(message);
					}
				}
			}
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x000A4524 File Offset: 0x000A2724
		public void Cancel()
		{
			if (!this.isFinished)
			{
				this.isFinished = true;
				Message message = new Message
				{
					type = MessageTypes.CancelInvocation,
					invocationId = this.invocationId.ToString()
				};
				this.hubConnection.SendMessage(message);
				Array.Clear(this.streamingIds, 0, this.streamingIds.Length);
				StreamItemContainer<TResult> streamItemContainer = this.future.value as StreamItemContainer<TResult>;
				if (streamItemContainer != null)
				{
					streamItemContainer.IsCanceled = true;
				}
			}
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x000A45A7 File Offset: 0x000A27A7
		void IDisposable.Dispose()
		{
			this.Finish();
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x000A45AF File Offset: 0x000A27AF
		public IFuture<TResult> OnItem(FutureValueCallback<TResult> callback)
		{
			return this.future.OnItem(callback);
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x000A45BD File Offset: 0x000A27BD
		public IFuture<TResult> OnSuccess(FutureValueCallback<TResult> callback)
		{
			return this.future.OnSuccess(callback);
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x000A45CB File Offset: 0x000A27CB
		public IFuture<TResult> OnError(FutureErrorCallback callback)
		{
			return this.future.OnError(callback);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x000A45D9 File Offset: 0x000A27D9
		public IFuture<TResult> OnComplete(FutureCallback<TResult> callback)
		{
			return this.future.OnComplete(callback);
		}

		// Token: 0x04001515 RID: 5397
		public readonly long invocationId;

		// Token: 0x04001516 RID: 5398
		public readonly int[] streamingIds;

		// Token: 0x04001517 RID: 5399
		public readonly HubConnection hubConnection;

		// Token: 0x04001518 RID: 5400
		public readonly IFuture<TResult> future;

		// Token: 0x04001519 RID: 5401
		private object[] streams;

		// Token: 0x0400151A RID: 5402
		private bool isFinished;
	}
}
