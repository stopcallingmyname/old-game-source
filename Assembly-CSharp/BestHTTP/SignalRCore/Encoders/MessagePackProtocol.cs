using System;
using System.Collections.Generic;
using BestHTTP.SignalRCore.Messages;

namespace BestHTTP.SignalRCore.Encoders
{
	// Token: 0x020001F0 RID: 496
	public sealed class MessagePackProtocol : IProtocol
	{
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public TransferModes Type
		{
			get
			{
				return TransferModes.Binary;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x000A483A File Offset: 0x000A2A3A
		// (set) Token: 0x06001262 RID: 4706 RVA: 0x000A4842 File Offset: 0x000A2A42
		public IEncoder Encoder { get; private set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06001263 RID: 4707 RVA: 0x000A484B File Offset: 0x000A2A4B
		// (set) Token: 0x06001264 RID: 4708 RVA: 0x000A4853 File Offset: 0x000A2A53
		public HubConnection Connection { get; set; }

		// Token: 0x06001265 RID: 4709 RVA: 0x000A485C File Offset: 0x000A2A5C
		public MessagePackProtocol()
		{
			this.Encoder = new MessagePackEncoder();
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x000947C7 File Offset: 0x000929C7
		public object ConvertTo(Type toType, object obj)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x000947C7 File Offset: 0x000929C7
		public byte[] EncodeMessage(Message message)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x000947C7 File Offset: 0x000929C7
		public object[] GetRealArguments(Type[] argTypes, object[] arguments)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x000947C7 File Offset: 0x000929C7
		public void ParseMessages(string data, ref List<Message> messages)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x000947C7 File Offset: 0x000929C7
		public void ParseMessages(byte[] data, ref List<Message> messages)
		{
			throw new NotImplementedException();
		}
	}
}
