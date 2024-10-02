using System;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001E3 RID: 483
	internal struct CallbackDescriptor
	{
		// Token: 0x060011D0 RID: 4560 RVA: 0x000A2B36 File Offset: 0x000A0D36
		public CallbackDescriptor(Type[] paramTypes, Action<object[]> callback)
		{
			this.ParamTypes = paramTypes;
			this.Callback = callback;
		}

		// Token: 0x040014F8 RID: 5368
		public readonly Type[] ParamTypes;

		// Token: 0x040014F9 RID: 5369
		public readonly Action<object[]> Callback;
	}
}
