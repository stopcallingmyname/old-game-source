using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.SignalRCore.Messages;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001EB RID: 491
	public sealed class JsonProtocol : IProtocol
	{
		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600122D RID: 4653 RVA: 0x0006AE98 File Offset: 0x00069098
		public TransferModes Type
		{
			get
			{
				return TransferModes.Text;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600122E RID: 4654 RVA: 0x000A3FA1 File Offset: 0x000A21A1
		// (set) Token: 0x0600122F RID: 4655 RVA: 0x000A3FA9 File Offset: 0x000A21A9
		public IEncoder Encoder { get; private set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06001230 RID: 4656 RVA: 0x000A3FB2 File Offset: 0x000A21B2
		// (set) Token: 0x06001231 RID: 4657 RVA: 0x000A3FBA File Offset: 0x000A21BA
		public HubConnection Connection { get; set; }

		// Token: 0x06001232 RID: 4658 RVA: 0x000A3FC3 File Offset: 0x000A21C3
		public JsonProtocol(IEncoder encoder)
		{
			if (encoder == null)
			{
				throw new ArgumentNullException("encoder");
			}
			if (encoder.Name != "json")
			{
				throw new ArgumentException("Encoder must be a json encoder!");
			}
			this.Encoder = encoder;
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x000A4000 File Offset: 0x000A2200
		public void ParseMessages(string data, ref List<Message> messages)
		{
			int num = 0;
			int num2 = data.IndexOf('\u001e');
			if (num2 == -1)
			{
				throw new Exception("Missing separator!");
			}
			while (num2 != -1)
			{
				string text = data.Substring(num, num2 - num);
				Message item = this.Encoder.DecodeAs<Message>(text);
				messages.Add(item);
				num = num2 + 1;
				num2 = data.IndexOf('\u001e', num);
			}
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x0000248C File Offset: 0x0000068C
		public void ParseMessages(byte[] data, ref List<Message> messages)
		{
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x000A405C File Offset: 0x000A225C
		public byte[] EncodeMessage(Message message)
		{
			string text = null;
			switch (message.type)
			{
			case MessageTypes.Invocation:
			case MessageTypes.StreamInvocation:
				if (message.streamIds != null)
				{
					text = this.Encoder.EncodeAsText<UploadInvocationMessage>(new UploadInvocationMessage
					{
						type = message.type,
						invocationId = message.invocationId,
						nonblocking = message.nonblocking,
						target = message.target,
						arguments = message.arguments,
						streamIds = message.streamIds
					});
				}
				else
				{
					text = this.Encoder.EncodeAsText<InvocationMessage>(new InvocationMessage
					{
						type = message.type,
						invocationId = message.invocationId,
						nonblocking = message.nonblocking,
						target = message.target,
						arguments = message.arguments
					});
				}
				break;
			case MessageTypes.StreamItem:
				text = this.Encoder.EncodeAsText<StreamItemMessage>(new StreamItemMessage
				{
					type = message.type,
					invocationId = message.invocationId,
					item = message.item
				});
				break;
			case MessageTypes.Completion:
				if (!string.IsNullOrEmpty(message.error))
				{
					text = this.Encoder.EncodeAsText<CompletionWithError>(new CompletionWithError
					{
						type = MessageTypes.Completion,
						invocationId = message.invocationId,
						error = message.error
					});
				}
				else if (message.result != null)
				{
					text = this.Encoder.EncodeAsText<CompletionWithResult>(new CompletionWithResult
					{
						type = MessageTypes.Completion,
						invocationId = message.invocationId,
						result = message.result
					});
				}
				else
				{
					text = this.Encoder.EncodeAsText<Completion>(new Completion
					{
						type = MessageTypes.Completion,
						invocationId = message.invocationId
					});
				}
				break;
			case MessageTypes.CancelInvocation:
				text = this.Encoder.EncodeAsText<CancelInvocationMessage>(new CancelInvocationMessage
				{
					invocationId = message.invocationId
				});
				break;
			case MessageTypes.Ping:
				text = this.Encoder.EncodeAsText<PingMessage>(default(PingMessage));
				break;
			}
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return JsonProtocol.WithSeparator(text);
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x000A42AC File Offset: 0x000A24AC
		public object[] GetRealArguments(Type[] argTypes, object[] arguments)
		{
			if (arguments == null || arguments.Length == 0)
			{
				return null;
			}
			if (argTypes.Length > arguments.Length)
			{
				throw new Exception(string.Format("argType.Length({0}) < arguments.length({1})", argTypes.Length, arguments.Length));
			}
			object[] array = new object[arguments.Length];
			for (int i = 0; i < arguments.Length; i++)
			{
				array[i] = this.ConvertTo(argTypes[i], arguments[i]);
			}
			return array;
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x000A4314 File Offset: 0x000A2514
		public object ConvertTo(Type toType, object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (toType.IsEnum)
			{
				return Enum.Parse(toType, obj.ToString(), true);
			}
			if (toType.IsPrimitive)
			{
				return Convert.ChangeType(obj, toType);
			}
			if (toType == typeof(string))
			{
				return obj.ToString();
			}
			if (toType.IsGenericType && toType.Name == "Nullable`1")
			{
				return Convert.ChangeType(obj, toType.GetGenericArguments()[0]);
			}
			return this.Encoder.ConvertTo(toType, obj);
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x000A439C File Offset: 0x000A259C
		public static byte[] WithSeparator(string str)
		{
			int byteCount = Encoding.UTF8.GetByteCount(str);
			byte[] array = new byte[byteCount + 1];
			Encoding.UTF8.GetBytes(str, 0, str.Length, array, 0);
			array[byteCount] = 30;
			return array;
		}

		// Token: 0x04001512 RID: 5394
		public const char Separator = '\u001e';
	}
}
