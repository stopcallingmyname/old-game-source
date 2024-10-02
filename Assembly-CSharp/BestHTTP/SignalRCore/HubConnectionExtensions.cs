using System;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001DA RID: 474
	public static class HubConnectionExtensions
	{
		// Token: 0x060011AC RID: 4524 RVA: 0x000A2905 File Offset: 0x000A0B05
		public static UploadItemController<StreamItemContainer<TResult>> UploadStreamWithDownStream<TResult, T1>(this HubConnection hub, string target)
		{
			if (hub.State != ConnectionStates.Connected)
			{
				return null;
			}
			return hub.UploadStreamWithDownStream<TResult>(target, 1);
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x000A291A File Offset: 0x000A0B1A
		public static UploadItemController<StreamItemContainer<TResult>> UploadStreamWithDownStream<TResult, T1, T2>(this HubConnection hub, string target)
		{
			if (hub.State != ConnectionStates.Connected)
			{
				return null;
			}
			return hub.UploadStreamWithDownStream<TResult>(target, 2);
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x000A292F File Offset: 0x000A0B2F
		public static UploadItemController<StreamItemContainer<TResult>> UploadStreamWithDownStream<TResult, T1, T2, T3>(this HubConnection hub, string target)
		{
			if (hub.State != ConnectionStates.Connected)
			{
				return null;
			}
			return hub.UploadStreamWithDownStream<TResult>(target, 3);
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x000A2944 File Offset: 0x000A0B44
		public static UploadItemController<StreamItemContainer<TResult>> UploadStreamWithDownStream<TResult, T1, T2, T3, T4>(this HubConnection hub, string target)
		{
			if (hub.State != ConnectionStates.Connected)
			{
				return null;
			}
			return hub.UploadStreamWithDownStream<TResult>(target, 4);
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x000A2959 File Offset: 0x000A0B59
		public static UploadItemController<StreamItemContainer<TResult>> UploadStreamWithDownStream<TResult, T1, T2, T3, T4, T5>(this HubConnection hub, string target)
		{
			if (hub.State != ConnectionStates.Connected)
			{
				return null;
			}
			return hub.UploadStreamWithDownStream<TResult>(target, 5);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x000A296E File Offset: 0x000A0B6E
		public static UploadItemController<TResult> Upload<TResult, T1>(this HubConnection hub, string target)
		{
			if (hub.State != ConnectionStates.Connected)
			{
				return null;
			}
			return hub.Upload<TResult>(target, 1);
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x000A2983 File Offset: 0x000A0B83
		public static UploadItemController<TResult> UploadStream<TResult, T1, T2>(this HubConnection hub, string target)
		{
			if (hub.State != ConnectionStates.Connected)
			{
				return null;
			}
			return hub.Upload<TResult>(target, 2);
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x000A2998 File Offset: 0x000A0B98
		public static UploadItemController<TResult> Upload<TResult, T1, T2, T3>(this HubConnection hub, string target)
		{
			if (hub.State != ConnectionStates.Connected)
			{
				return null;
			}
			return hub.Upload<TResult>(target, 3);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x000A29AD File Offset: 0x000A0BAD
		public static UploadItemController<TResult> Upload<TResult, T1, T2, T3, T4>(this HubConnection hub, string target)
		{
			if (hub.State != ConnectionStates.Connected)
			{
				return null;
			}
			return hub.Upload<TResult>(target, 4);
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x000A29C2 File Offset: 0x000A0BC2
		public static UploadItemController<TResult> Upload<TResult, T1, T2, T3, T4, T5>(this HubConnection hub, string target)
		{
			if (hub.State != ConnectionStates.Connected)
			{
				return null;
			}
			return hub.Upload<TResult>(target, 5);
		}
	}
}
