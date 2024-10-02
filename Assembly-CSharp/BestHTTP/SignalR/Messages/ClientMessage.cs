using System;
using BestHTTP.SignalR.Hubs;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x02000215 RID: 533
	public struct ClientMessage
	{
		// Token: 0x06001377 RID: 4983 RVA: 0x000A864D File Offset: 0x000A684D
		public ClientMessage(Hub hub, string method, object[] args, ulong callIdx, OnMethodResultDelegate resultCallback, OnMethodFailedDelegate resultErrorCallback, OnMethodProgressDelegate progressCallback)
		{
			this.Hub = hub;
			this.Method = method;
			this.Args = args;
			this.CallIdx = callIdx;
			this.ResultCallback = resultCallback;
			this.ResultErrorCallback = resultErrorCallback;
			this.ProgressCallback = progressCallback;
		}

		// Token: 0x040015C4 RID: 5572
		public readonly Hub Hub;

		// Token: 0x040015C5 RID: 5573
		public readonly string Method;

		// Token: 0x040015C6 RID: 5574
		public readonly object[] Args;

		// Token: 0x040015C7 RID: 5575
		public readonly ulong CallIdx;

		// Token: 0x040015C8 RID: 5576
		public readonly OnMethodResultDelegate ResultCallback;

		// Token: 0x040015C9 RID: 5577
		public readonly OnMethodFailedDelegate ResultErrorCallback;

		// Token: 0x040015CA RID: 5578
		public readonly OnMethodProgressDelegate ProgressCallback;
	}
}
