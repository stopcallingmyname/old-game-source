using System;

namespace BestHTTP.SocketIO.Events
{
	// Token: 0x020001D8 RID: 472
	public static class EventNames
	{
		// Token: 0x0600119D RID: 4509 RVA: 0x000A250C File Offset: 0x000A070C
		public static string GetNameFor(SocketIOEventTypes type)
		{
			return EventNames.SocketIONames[(int)(type + 1)];
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x000A2517 File Offset: 0x000A0717
		public static string GetNameFor(TransportEventTypes transEvent)
		{
			return EventNames.TransportNames[(int)(transEvent + 1)];
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x000A2524 File Offset: 0x000A0724
		public static bool IsBlacklisted(string eventName)
		{
			for (int i = 0; i < EventNames.BlacklistedEvents.Length; i++)
			{
				if (string.Compare(EventNames.BlacklistedEvents[i], eventName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040014D4 RID: 5332
		public const string Connect = "connect";

		// Token: 0x040014D5 RID: 5333
		public const string Disconnect = "disconnect";

		// Token: 0x040014D6 RID: 5334
		public const string Event = "event";

		// Token: 0x040014D7 RID: 5335
		public const string Ack = "ack";

		// Token: 0x040014D8 RID: 5336
		public const string Error = "error";

		// Token: 0x040014D9 RID: 5337
		public const string BinaryEvent = "binaryevent";

		// Token: 0x040014DA RID: 5338
		public const string BinaryAck = "binaryack";

		// Token: 0x040014DB RID: 5339
		private static string[] SocketIONames = new string[]
		{
			"unknown",
			"connect",
			"disconnect",
			"event",
			"ack",
			"error",
			"binaryevent",
			"binaryack"
		};

		// Token: 0x040014DC RID: 5340
		private static string[] TransportNames = new string[]
		{
			"unknown",
			"open",
			"close",
			"ping",
			"pong",
			"message",
			"upgrade",
			"noop"
		};

		// Token: 0x040014DD RID: 5341
		private static string[] BlacklistedEvents = new string[]
		{
			"connect",
			"connect_error",
			"connect_timeout",
			"disconnect",
			"error",
			"reconnect",
			"reconnect_attempt",
			"reconnect_failed",
			"reconnect_error",
			"reconnecting"
		};
	}
}
