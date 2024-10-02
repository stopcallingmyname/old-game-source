using System;

namespace BestHTTP.SignalRCore.Messages
{
	// Token: 0x020001FB RID: 507
	public struct Message
	{
		// Token: 0x06001281 RID: 4737 RVA: 0x000A4E24 File Offset: 0x000A3024
		public override string ToString()
		{
			switch (this.type)
			{
			case MessageTypes.Invocation:
				return string.Format("[Invocation Id: {0}, Target: '{1}', Argument count: {2}, Stream Ids: {3}]", new object[]
				{
					this.invocationId,
					this.target,
					(this.arguments != null) ? this.arguments.Length : 0,
					(this.streamIds != null) ? this.streamIds.Length : 0
				});
			case MessageTypes.StreamItem:
				return string.Format("[StreamItem Id: {0}, Item: {1}]", this.invocationId, this.item.ToString());
			case MessageTypes.Completion:
				return string.Format("[Completion Id: {0}, Result: {1}, Error: '{2}']", this.invocationId, this.result, this.error);
			case MessageTypes.StreamInvocation:
				return string.Format("[StreamInvocation Id: {0}, Target: '{1}', Argument count: {2}]", this.invocationId, this.target, (this.arguments != null) ? this.arguments.Length : 0);
			case MessageTypes.CancelInvocation:
				return string.Format("[CancelInvocation Id: {0}]", this.invocationId);
			case MessageTypes.Ping:
				return "[Ping]";
			case MessageTypes.Close:
				if (!string.IsNullOrEmpty(this.error))
				{
					return string.Format("[Close {0}]", this.error);
				}
				return "[Close]";
			default:
				return "Unknown message! Type: " + this.type;
			}
		}

		// Token: 0x04001545 RID: 5445
		public MessageTypes type;

		// Token: 0x04001546 RID: 5446
		public string invocationId;

		// Token: 0x04001547 RID: 5447
		public bool nonblocking;

		// Token: 0x04001548 RID: 5448
		public string target;

		// Token: 0x04001549 RID: 5449
		public object[] arguments;

		// Token: 0x0400154A RID: 5450
		public int[] streamIds;

		// Token: 0x0400154B RID: 5451
		public object item;

		// Token: 0x0400154C RID: 5452
		public object result;

		// Token: 0x0400154D RID: 5453
		public string error;
	}
}
