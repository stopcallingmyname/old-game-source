using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Hubs
{
	// Token: 0x02000227 RID: 551
	public class Hub : IHub
	{
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060013D8 RID: 5080 RVA: 0x000A8C39 File Offset: 0x000A6E39
		// (set) Token: 0x060013D9 RID: 5081 RVA: 0x000A8C41 File Offset: 0x000A6E41
		public string Name { get; private set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x000A8C4A File Offset: 0x000A6E4A
		public Dictionary<string, object> State
		{
			get
			{
				if (this.state == null)
				{
					this.state = new Dictionary<string, object>();
				}
				return this.state;
			}
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060013DB RID: 5083 RVA: 0x000A8C68 File Offset: 0x000A6E68
		// (remove) Token: 0x060013DC RID: 5084 RVA: 0x000A8CA0 File Offset: 0x000A6EA0
		public event OnMethodCallDelegate OnMethodCall;

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060013DD RID: 5085 RVA: 0x000A8CD5 File Offset: 0x000A6ED5
		// (set) Token: 0x060013DE RID: 5086 RVA: 0x000A8CDD File Offset: 0x000A6EDD
		Connection IHub.Connection { get; set; }

		// Token: 0x060013DF RID: 5087 RVA: 0x000A8CE6 File Offset: 0x000A6EE6
		public Hub(string name) : this(name, null)
		{
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x000A8CF0 File Offset: 0x000A6EF0
		public Hub(string name, Connection manager)
		{
			this.Name = name;
			((IHub)this).Connection = manager;
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x000A8D27 File Offset: 0x000A6F27
		public void On(string method, OnMethodCallCallbackDelegate callback)
		{
			this.MethodTable[method] = callback;
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x000A8D36 File Offset: 0x000A6F36
		public void Off(string method)
		{
			this.MethodTable[method] = null;
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x000A8D45 File Offset: 0x000A6F45
		public bool Call(string method, params object[] args)
		{
			return this.Call(method, null, null, null, args);
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x000A8D52 File Offset: 0x000A6F52
		public bool Call(string method, OnMethodResultDelegate onResult, params object[] args)
		{
			return this.Call(method, onResult, null, null, args);
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x000A8D5F File Offset: 0x000A6F5F
		public bool Call(string method, OnMethodResultDelegate onResult, OnMethodFailedDelegate onResultError, params object[] args)
		{
			return this.Call(method, onResult, onResultError, null, args);
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x000A8D6D File Offset: 0x000A6F6D
		public bool Call(string method, OnMethodResultDelegate onResult, OnMethodProgressDelegate onProgress, params object[] args)
		{
			return this.Call(method, onResult, null, onProgress, args);
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x000A8D7C File Offset: 0x000A6F7C
		public bool Call(string method, OnMethodResultDelegate onResult, OnMethodFailedDelegate onResultError, OnMethodProgressDelegate onProgress, params object[] args)
		{
			long clientMessageCounter;
			long value;
			do
			{
				clientMessageCounter = ((IHub)this).Connection.ClientMessageCounter;
				value = clientMessageCounter % long.MaxValue + 1L;
			}
			while (Interlocked.CompareExchange(ref ((IHub)this).Connection.ClientMessageCounter, value, clientMessageCounter) != clientMessageCounter);
			return ((IHub)this).Call(new ClientMessage(this, method, args, (ulong)((IHub)this).Connection.ClientMessageCounter, onResult, onResultError, onProgress));
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x000A8DD9 File Offset: 0x000A6FD9
		bool IHub.Call(ClientMessage msg)
		{
			if (!((IHub)this).Connection.SendJson(this.BuildMessage(msg)))
			{
				return false;
			}
			this.SentMessages.Add(msg.CallIdx, msg);
			return true;
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x000A8E04 File Offset: 0x000A7004
		bool IHub.HasSentMessageId(ulong id)
		{
			return this.SentMessages.ContainsKey(id);
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x000A8E12 File Offset: 0x000A7012
		void IHub.Close()
		{
			this.SentMessages.Clear();
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x000A8E20 File Offset: 0x000A7020
		void IHub.OnMethod(MethodCallMessage msg)
		{
			this.MergeState(msg.State);
			if (this.OnMethodCall != null)
			{
				try
				{
					this.OnMethodCall(this, msg.Method, msg.Arguments);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("Hub - " + this.Name, "IHub.OnMethod - OnMethodCall", ex);
				}
			}
			OnMethodCallCallbackDelegate onMethodCallCallbackDelegate;
			if (this.MethodTable.TryGetValue(msg.Method, out onMethodCallCallbackDelegate) && onMethodCallCallbackDelegate != null)
			{
				try
				{
					onMethodCallCallbackDelegate(this, msg);
					return;
				}
				catch (Exception ex2)
				{
					HTTPManager.Logger.Exception("Hub - " + this.Name, "IHub.OnMethod - callback", ex2);
					return;
				}
			}
			if (this.OnMethodCall == null)
			{
				HTTPManager.Logger.Warning("Hub - " + this.Name, string.Format("[Client] {0}.{1} (args: {2})", this.Name, msg.Method, msg.Arguments.Length));
			}
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x000A8F24 File Offset: 0x000A7124
		void IHub.OnMessage(IServerMessage msg)
		{
			ulong invocationId = (msg as IHubMessage).InvocationId;
			ClientMessage clientMessage;
			if (!this.SentMessages.TryGetValue(invocationId, out clientMessage))
			{
				HTTPManager.Logger.Warning("Hub - " + this.Name, "OnMessage - Sent message not found with id: " + invocationId.ToString());
				return;
			}
			switch (msg.Type)
			{
			case MessageTypes.Result:
			{
				ResultMessage resultMessage = msg as ResultMessage;
				this.MergeState(resultMessage.State);
				if (clientMessage.ResultCallback != null)
				{
					try
					{
						clientMessage.ResultCallback(this, clientMessage, resultMessage);
					}
					catch (Exception ex)
					{
						HTTPManager.Logger.Exception("Hub " + this.Name, "IHub.OnMessage - ResultCallback", ex);
					}
				}
				this.SentMessages.Remove(invocationId);
				return;
			}
			case MessageTypes.Failure:
			{
				FailureMessage failureMessage = msg as FailureMessage;
				this.MergeState(failureMessage.State);
				if (clientMessage.ResultErrorCallback != null)
				{
					try
					{
						clientMessage.ResultErrorCallback(this, clientMessage, failureMessage);
					}
					catch (Exception ex2)
					{
						HTTPManager.Logger.Exception("Hub " + this.Name, "IHub.OnMessage - ResultErrorCallback", ex2);
					}
				}
				this.SentMessages.Remove(invocationId);
				return;
			}
			case MessageTypes.MethodCall:
				break;
			case MessageTypes.Progress:
				if (clientMessage.ProgressCallback != null)
				{
					try
					{
						clientMessage.ProgressCallback(this, clientMessage, msg as ProgressMessage);
					}
					catch (Exception ex3)
					{
						HTTPManager.Logger.Exception("Hub " + this.Name, "IHub.OnMessage - ProgressCallback", ex3);
					}
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x000A90C4 File Offset: 0x000A72C4
		private void MergeState(IDictionary<string, object> state)
		{
			if (state != null && state.Count > 0)
			{
				foreach (KeyValuePair<string, object> keyValuePair in state)
				{
					this.State[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x000A912C File Offset: 0x000A732C
		private string BuildMessage(ClientMessage msg)
		{
			string result;
			try
			{
				this.builder.Append("{\"H\":\"");
				this.builder.Append(this.Name);
				this.builder.Append("\",\"M\":\"");
				this.builder.Append(msg.Method);
				this.builder.Append("\",\"A\":");
				string value = string.Empty;
				if (msg.Args != null && msg.Args.Length != 0)
				{
					value = ((IHub)this).Connection.JsonEncoder.Encode(msg.Args);
				}
				else
				{
					value = "[]";
				}
				this.builder.Append(value);
				this.builder.Append(",\"I\":\"");
				this.builder.Append(msg.CallIdx.ToString());
				this.builder.Append("\"");
				if (msg.Hub.state != null && msg.Hub.state.Count > 0)
				{
					this.builder.Append(",\"S\":");
					value = ((IHub)this).Connection.JsonEncoder.Encode(msg.Hub.state);
					this.builder.Append(value);
				}
				this.builder.Append("}");
				result = this.builder.ToString();
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("Hub - " + this.Name, "Send", ex);
				result = null;
			}
			finally
			{
				this.builder.Length = 0;
			}
			return result;
		}

		// Token: 0x040015E2 RID: 5602
		private Dictionary<string, object> state;

		// Token: 0x040015E4 RID: 5604
		private Dictionary<ulong, ClientMessage> SentMessages = new Dictionary<ulong, ClientMessage>();

		// Token: 0x040015E5 RID: 5605
		private Dictionary<string, OnMethodCallCallbackDelegate> MethodTable = new Dictionary<string, OnMethodCallCallbackDelegate>();

		// Token: 0x040015E6 RID: 5606
		private StringBuilder builder = new StringBuilder();
	}
}
