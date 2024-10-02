using System;

namespace BestHTTP.SignalRCore.Encoders
{
	// Token: 0x020001EF RID: 495
	public sealed class MessagePackEncoder : IEncoder
	{
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06001259 RID: 4697 RVA: 0x000A4833 File Offset: 0x000A2A33
		public string Name
		{
			get
			{
				return "messagepack";
			}
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x000947C7 File Offset: 0x000929C7
		public object ConvertTo(Type toType, object obj)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x000947C7 File Offset: 0x000929C7
		public T DecodeAs<T>(string text)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x000947C7 File Offset: 0x000929C7
		public T DecodeAs<T>(byte[] data)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x000947C7 File Offset: 0x000929C7
		public byte[] EncodeAsBinary<T>(T value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x000947C7 File Offset: 0x000929C7
		public string EncodeAsText<T>(T value)
		{
			throw new NotImplementedException();
		}
	}
}
