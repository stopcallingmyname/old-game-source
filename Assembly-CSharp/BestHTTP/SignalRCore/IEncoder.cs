using System;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001E1 RID: 481
	public interface IEncoder
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060011C4 RID: 4548
		string Name { get; }

		// Token: 0x060011C5 RID: 4549
		string EncodeAsText<T>(T value);

		// Token: 0x060011C6 RID: 4550
		T DecodeAs<T>(string text);

		// Token: 0x060011C7 RID: 4551
		byte[] EncodeAsBinary<T>(T value);

		// Token: 0x060011C8 RID: 4552
		T DecodeAs<T>(byte[] data);

		// Token: 0x060011C9 RID: 4553
		object ConvertTo(Type toType, object obj);
	}
}
