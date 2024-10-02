using System;
using BestHTTP.SignalRCore.Messages;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001ED RID: 493
	public sealed class UploadChannel<TResult, T> : IDisposable
	{
		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06001246 RID: 4678 RVA: 0x000A45E7 File Offset: 0x000A27E7
		// (set) Token: 0x06001247 RID: 4679 RVA: 0x000A45EF File Offset: 0x000A27EF
		public UploadItemController<TResult> Controller { get; private set; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06001248 RID: 4680 RVA: 0x000A45F8 File Offset: 0x000A27F8
		// (set) Token: 0x06001249 RID: 4681 RVA: 0x000A4600 File Offset: 0x000A2800
		public int ParamIdx { get; private set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600124A RID: 4682 RVA: 0x000A4609 File Offset: 0x000A2809
		// (set) Token: 0x0600124B RID: 4683 RVA: 0x000A4620 File Offset: 0x000A2820
		public bool IsFinished
		{
			get
			{
				return this.Controller.streamingIds[this.ParamIdx] == 0;
			}
			private set
			{
				if (value)
				{
					this.Controller.streamingIds[this.ParamIdx] = 0;
				}
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600124C RID: 4684 RVA: 0x000A4638 File Offset: 0x000A2838
		public int StreamingId
		{
			get
			{
				return this.Controller.streamingIds[this.ParamIdx];
			}
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x000A464C File Offset: 0x000A284C
		internal UploadChannel(UploadItemController<TResult> ctrl, int paramIdx)
		{
			this.Controller = ctrl;
			this.ParamIdx = paramIdx;
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x000A4664 File Offset: 0x000A2864
		public void Upload(T item)
		{
			int streamingId = this.StreamingId;
			if (streamingId > 0)
			{
				this.Controller.UploadParam<T>(streamingId, item);
			}
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x000A4689 File Offset: 0x000A2889
		public void Cancel()
		{
			if (!this.IsFinished)
			{
				this.Controller.Cancel();
			}
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x000A46A0 File Offset: 0x000A28A0
		public void Finish()
		{
			if (!this.IsFinished)
			{
				int streamingId = this.StreamingId;
				if (streamingId > 0)
				{
					this.IsFinished = true;
					Message message = new Message
					{
						type = MessageTypes.Completion,
						invocationId = streamingId.ToString()
					};
					this.Controller.hubConnection.SendMessage(message);
				}
			}
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x000A46F8 File Offset: 0x000A28F8
		void IDisposable.Dispose()
		{
			if (!this.IsFinished)
			{
				this.Finish();
			}
			GC.SuppressFinalize(this);
		}
	}
}
