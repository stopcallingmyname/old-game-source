using System;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001DB RID: 475
	public static class UploadItemControllerExtensions
	{
		// Token: 0x060011B6 RID: 4534 RVA: 0x000A29D7 File Offset: 0x000A0BD7
		public static void Upload<TResult, P1>(this UploadItemController<TResult> controller, P1 item)
		{
			controller.UploadParam<P1>(controller.streamingIds[0], item);
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x000A29E8 File Offset: 0x000A0BE8
		public static void Upload<TResult, P1, P2>(this UploadItemController<TResult> controller, P1 param1, P2 param2)
		{
			controller.UploadParam<P1>(controller.streamingIds[0], param1);
			controller.UploadParam<P2>(controller.streamingIds[1], param2);
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x000A2A08 File Offset: 0x000A0C08
		public static void Upload<TResult, P1, P2, P3>(this UploadItemController<TResult> controller, P1 param1, P2 param2, P3 param3)
		{
			controller.UploadParam<P1>(controller.streamingIds[0], param1);
			controller.UploadParam<P2>(controller.streamingIds[1], param2);
			controller.UploadParam<P3>(controller.streamingIds[2], param3);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x000A2A37 File Offset: 0x000A0C37
		public static void Upload<TResult, P1, P2, P3, P4>(this UploadItemController<TResult> controller, P1 param1, P2 param2, P3 param3, P4 param4)
		{
			controller.UploadParam<P1>(controller.streamingIds[0], param1);
			controller.UploadParam<P2>(controller.streamingIds[1], param2);
			controller.UploadParam<P3>(controller.streamingIds[2], param3);
			controller.UploadParam<P4>(controller.streamingIds[3], param4);
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x000A2A78 File Offset: 0x000A0C78
		public static void Upload<TResult, P1, P2, P3, P4, P5>(this UploadItemController<TResult> controller, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5)
		{
			controller.UploadParam<P1>(controller.streamingIds[0], param1);
			controller.UploadParam<P2>(controller.streamingIds[1], param2);
			controller.UploadParam<P3>(controller.streamingIds[2], param3);
			controller.UploadParam<P4>(controller.streamingIds[3], param4);
			controller.UploadParam<P5>(controller.streamingIds[4], param5);
		}
	}
}
