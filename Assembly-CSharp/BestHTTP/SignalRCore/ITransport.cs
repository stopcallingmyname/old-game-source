using System;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001E0 RID: 480
	public interface ITransport
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060011BB RID: 4539
		TransferModes TransferMode { get; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060011BC RID: 4540
		TransportTypes TransportType { get; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060011BD RID: 4541
		TransportStates State { get; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060011BE RID: 4542
		string ErrorReason { get; }

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060011BF RID: 4543
		// (remove) Token: 0x060011C0 RID: 4544
		event Action<TransportStates, TransportStates> OnStateChanged;

		// Token: 0x060011C1 RID: 4545
		void StartConnect();

		// Token: 0x060011C2 RID: 4546
		void StartClose();

		// Token: 0x060011C3 RID: 4547
		void Send(byte[] msg);
	}
}
