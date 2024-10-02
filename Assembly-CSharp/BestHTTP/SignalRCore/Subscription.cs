using System;
using System.Collections.Generic;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001E4 RID: 484
	internal sealed class Subscription
	{
		// Token: 0x060011D1 RID: 4561 RVA: 0x000A2B48 File Offset: 0x000A0D48
		public void Add(Type[] paramTypes, Action<object[]> callback)
		{
			List<CallbackDescriptor> obj = this.callbacks;
			lock (obj)
			{
				this.callbacks.Add(new CallbackDescriptor(paramTypes, callback));
			}
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x000A2B94 File Offset: 0x000A0D94
		public void Remove(Action<object[]> callback)
		{
			List<CallbackDescriptor> obj = this.callbacks;
			lock (obj)
			{
				int num = -1;
				int num2 = 0;
				while (num2 < this.callbacks.Count && num == -1)
				{
					if (this.callbacks[num2].Callback == callback)
					{
						num = num2;
					}
					num2++;
				}
				if (num != -1)
				{
					this.callbacks.RemoveAt(num);
				}
			}
		}

		// Token: 0x040014FA RID: 5370
		public List<CallbackDescriptor> callbacks = new List<CallbackDescriptor>(1);
	}
}
