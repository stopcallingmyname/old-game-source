using System;

namespace BestHTTP
{
	// Token: 0x02000188 RID: 392
	public interface IProtocol
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000E44 RID: 3652
		bool IsClosed { get; }

		// Token: 0x06000E45 RID: 3653
		void HandleEvents();
	}
}
