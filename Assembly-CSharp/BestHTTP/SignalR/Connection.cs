using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BestHTTP.Extensions;
using BestHTTP.JSON;
using BestHTTP.Logger;
using BestHTTP.SignalR.Authentication;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.JsonEncoders;
using BestHTTP.SignalR.Messages;
using BestHTTP.SignalR.Transports;
using PlatformSupport.Collections.ObjectModel;
using PlatformSupport.Collections.Specialized;

namespace BestHTTP.SignalR
{
	// Token: 0x02000208 RID: 520
	public sealed class Connection : IHeartbeat, IConnection
	{
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060012C8 RID: 4808 RVA: 0x000A54FC File Offset: 0x000A36FC
		// (set) Token: 0x060012C9 RID: 4809 RVA: 0x000A5504 File Offset: 0x000A3704
		public Uri Uri { get; private set; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060012CA RID: 4810 RVA: 0x000A550D File Offset: 0x000A370D
		// (set) Token: 0x060012CB RID: 4811 RVA: 0x000A5518 File Offset: 0x000A3718
		public ConnectionStates State
		{
			get
			{
				return this._state;
			}
			private set
			{
				ConnectionStates state = this._state;
				this._state = value;
				if (this.OnStateChanged != null)
				{
					this.OnStateChanged(this, state, this._state);
				}
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060012CC RID: 4812 RVA: 0x000A554E File Offset: 0x000A374E
		// (set) Token: 0x060012CD RID: 4813 RVA: 0x000A5556 File Offset: 0x000A3756
		public NegotiationData NegotiationResult { get; private set; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060012CE RID: 4814 RVA: 0x000A555F File Offset: 0x000A375F
		// (set) Token: 0x060012CF RID: 4815 RVA: 0x000A5567 File Offset: 0x000A3767
		public Hub[] Hubs { get; private set; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060012D0 RID: 4816 RVA: 0x000A5570 File Offset: 0x000A3770
		// (set) Token: 0x060012D1 RID: 4817 RVA: 0x000A5578 File Offset: 0x000A3778
		public TransportBase Transport { get; private set; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060012D2 RID: 4818 RVA: 0x000A5581 File Offset: 0x000A3781
		// (set) Token: 0x060012D3 RID: 4819 RVA: 0x000A5589 File Offset: 0x000A3789
		public ProtocolVersions Protocol { get; private set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060012D4 RID: 4820 RVA: 0x000A5592 File Offset: 0x000A3792
		// (set) Token: 0x060012D5 RID: 4821 RVA: 0x000A559C File Offset: 0x000A379C
		public ObservableDictionary<string, string> AdditionalQueryParams
		{
			get
			{
				return this.additionalQueryParams;
			}
			set
			{
				if (this.additionalQueryParams != null)
				{
					this.additionalQueryParams.CollectionChanged -= this.AdditionalQueryParams_CollectionChanged;
				}
				this.additionalQueryParams = value;
				this.BuiltQueryParams = null;
				if (value != null)
				{
					value.CollectionChanged += this.AdditionalQueryParams_CollectionChanged;
				}
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060012D6 RID: 4822 RVA: 0x000A55EB File Offset: 0x000A37EB
		// (set) Token: 0x060012D7 RID: 4823 RVA: 0x000A55F3 File Offset: 0x000A37F3
		public bool QueryParamsOnlyForHandshake { get; set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060012D8 RID: 4824 RVA: 0x000A55FC File Offset: 0x000A37FC
		// (set) Token: 0x060012D9 RID: 4825 RVA: 0x000A5604 File Offset: 0x000A3804
		public IJsonEncoder JsonEncoder { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060012DA RID: 4826 RVA: 0x000A560D File Offset: 0x000A380D
		// (set) Token: 0x060012DB RID: 4827 RVA: 0x000A5615 File Offset: 0x000A3815
		public IAuthenticationProvider AuthenticationProvider { get; set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x000A561E File Offset: 0x000A381E
		// (set) Token: 0x060012DD RID: 4829 RVA: 0x000A5626 File Offset: 0x000A3826
		public TimeSpan PingInterval { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x000A562F File Offset: 0x000A382F
		// (set) Token: 0x060012DF RID: 4831 RVA: 0x000A5637 File Offset: 0x000A3837
		public TimeSpan ReconnectDelay { get; set; }

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060012E0 RID: 4832 RVA: 0x000A5640 File Offset: 0x000A3840
		// (remove) Token: 0x060012E1 RID: 4833 RVA: 0x000A5678 File Offset: 0x000A3878
		public event OnConnectedDelegate OnConnected;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060012E2 RID: 4834 RVA: 0x000A56B0 File Offset: 0x000A38B0
		// (remove) Token: 0x060012E3 RID: 4835 RVA: 0x000A56E8 File Offset: 0x000A38E8
		public event OnClosedDelegate OnClosed;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060012E4 RID: 4836 RVA: 0x000A5720 File Offset: 0x000A3920
		// (remove) Token: 0x060012E5 RID: 4837 RVA: 0x000A5758 File Offset: 0x000A3958
		public event OnErrorDelegate OnError;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060012E6 RID: 4838 RVA: 0x000A5790 File Offset: 0x000A3990
		// (remove) Token: 0x060012E7 RID: 4839 RVA: 0x000A57C8 File Offset: 0x000A39C8
		public event OnConnectedDelegate OnReconnecting;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060012E8 RID: 4840 RVA: 0x000A5800 File Offset: 0x000A3A00
		// (remove) Token: 0x060012E9 RID: 4841 RVA: 0x000A5838 File Offset: 0x000A3A38
		public event OnConnectedDelegate OnReconnected;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060012EA RID: 4842 RVA: 0x000A5870 File Offset: 0x000A3A70
		// (remove) Token: 0x060012EB RID: 4843 RVA: 0x000A58A8 File Offset: 0x000A3AA8
		public event OnStateChanged OnStateChanged;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x060012EC RID: 4844 RVA: 0x000A58E0 File Offset: 0x000A3AE0
		// (remove) Token: 0x060012ED RID: 4845 RVA: 0x000A5918 File Offset: 0x000A3B18
		public event OnNonHubMessageDelegate OnNonHubMessage;

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x000A594D File Offset: 0x000A3B4D
		// (set) Token: 0x060012EF RID: 4847 RVA: 0x000A5955 File Offset: 0x000A3B55
		public OnPrepareRequestDelegate RequestPreparator { get; set; }

		// Token: 0x17000207 RID: 519
		public Hub this[int idx]
		{
			get
			{
				return this.Hubs[idx];
			}
		}

		// Token: 0x17000208 RID: 520
		public Hub this[string hubName]
		{
			get
			{
				for (int i = 0; i < this.Hubs.Length; i++)
				{
					Hub hub = this.Hubs[i];
					if (hub.Name.Equals(hubName, StringComparison.OrdinalIgnoreCase))
					{
						return hub;
					}
				}
				return null;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060012F2 RID: 4850 RVA: 0x000A59A4 File Offset: 0x000A3BA4
		private uint Timestamp
		{
			get
			{
				return (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).Ticks;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060012F3 RID: 4851 RVA: 0x000A59D4 File Offset: 0x000A3BD4
		private string ConnectionData
		{
			get
			{
				if (!string.IsNullOrEmpty(this.BuiltConnectionData))
				{
					return this.BuiltConnectionData;
				}
				StringBuilder stringBuilder = new StringBuilder("[", this.Hubs.Length * 4);
				if (this.Hubs != null)
				{
					for (int i = 0; i < this.Hubs.Length; i++)
					{
						stringBuilder.Append("{\"Name\":\"");
						stringBuilder.Append(this.Hubs[i].Name);
						stringBuilder.Append("\"}");
						if (i < this.Hubs.Length - 1)
						{
							stringBuilder.Append(",");
						}
					}
				}
				stringBuilder.Append("]");
				return this.BuiltConnectionData = Uri.EscapeUriString(stringBuilder.ToString());
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060012F4 RID: 4852 RVA: 0x000A5A8C File Offset: 0x000A3C8C
		private string QueryParams
		{
			get
			{
				if (this.AdditionalQueryParams == null || this.AdditionalQueryParams.Count == 0)
				{
					return string.Empty;
				}
				if (!string.IsNullOrEmpty(this.BuiltQueryParams))
				{
					return this.BuiltQueryParams;
				}
				StringBuilder stringBuilder = new StringBuilder(this.AdditionalQueryParams.Count * 4);
				foreach (KeyValuePair<string, string> keyValuePair in this.AdditionalQueryParams)
				{
					stringBuilder.Append("&");
					stringBuilder.Append(keyValuePair.Key);
					if (!string.IsNullOrEmpty(keyValuePair.Value))
					{
						stringBuilder.Append("=");
						stringBuilder.Append(Uri.EscapeDataString(keyValuePair.Value));
					}
				}
				return this.BuiltQueryParams = stringBuilder.ToString();
			}
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x000A5B6C File Offset: 0x000A3D6C
		public Connection(Uri uri, params string[] hubNames) : this(uri)
		{
			if (hubNames != null && hubNames.Length != 0)
			{
				this.Hubs = new Hub[hubNames.Length];
				for (int i = 0; i < hubNames.Length; i++)
				{
					this.Hubs[i] = new Hub(hubNames[i], this);
				}
			}
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x000A5BB4 File Offset: 0x000A3DB4
		public Connection(Uri uri, params Hub[] hubs) : this(uri)
		{
			this.Hubs = hubs;
			if (hubs != null)
			{
				for (int i = 0; i < hubs.Length; i++)
				{
					((IHub)hubs[i]).Connection = this;
				}
			}
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x000A5BEC File Offset: 0x000A3DEC
		public Connection(Uri uri)
		{
			this.State = ConnectionStates.Initial;
			this.Uri = uri;
			this.JsonEncoder = Connection.DefaultEncoder;
			this.PingInterval = TimeSpan.FromMinutes(5.0);
			this.Protocol = ProtocolVersions.Protocol_2_2;
			this.ReconnectDelay = TimeSpan.FromSeconds(5.0);
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x000A5C78 File Offset: 0x000A3E78
		public void Open()
		{
			if (this.State != ConnectionStates.Initial && this.State != ConnectionStates.Closed)
			{
				return;
			}
			if (this.AuthenticationProvider != null && this.AuthenticationProvider.IsPreAuthRequired)
			{
				this.State = ConnectionStates.Authenticating;
				this.AuthenticationProvider.OnAuthenticationSucceded += this.OnAuthenticationSucceded;
				this.AuthenticationProvider.OnAuthenticationFailed += this.OnAuthenticationFailed;
				this.AuthenticationProvider.StartAuthentication();
				return;
			}
			this.StartImpl();
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x000A5CF3 File Offset: 0x000A3EF3
		private void OnAuthenticationSucceded(IAuthenticationProvider provider)
		{
			provider.OnAuthenticationSucceded -= this.OnAuthenticationSucceded;
			provider.OnAuthenticationFailed -= this.OnAuthenticationFailed;
			this.StartImpl();
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x000A5D1F File Offset: 0x000A3F1F
		private void OnAuthenticationFailed(IAuthenticationProvider provider, string reason)
		{
			provider.OnAuthenticationSucceded -= this.OnAuthenticationSucceded;
			provider.OnAuthenticationFailed -= this.OnAuthenticationFailed;
			((IConnection)this).Error(reason);
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x000A5D4C File Offset: 0x000A3F4C
		private void StartImpl()
		{
			this.State = ConnectionStates.Negotiating;
			this.NegotiationResult = new NegotiationData(this);
			this.NegotiationResult.OnReceived = new Action<NegotiationData>(this.OnNegotiationDataReceived);
			this.NegotiationResult.OnError = new Action<NegotiationData, string>(this.OnNegotiationError);
			this.NegotiationResult.Start();
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x000A5DA8 File Offset: 0x000A3FA8
		private void OnNegotiationDataReceived(NegotiationData data)
		{
			int num = -1;
			int num2 = 0;
			while (num2 < this.ClientProtocols.Length && num == -1)
			{
				if (data.ProtocolVersion == this.ClientProtocols[num2])
				{
					num = num2;
				}
				num2++;
			}
			if (num == -1)
			{
				num = 2;
				HTTPManager.Logger.Warning("SignalR Connection", "Unknown protocol version: " + data.ProtocolVersion);
			}
			this.Protocol = (ProtocolVersions)num;
			if (data.TryWebSockets)
			{
				this.Transport = new WebSocketTransport(this);
				this.NextProtocolToTry = SupportedProtocols.ServerSentEvents;
			}
			else
			{
				this.Transport = new ServerSentEventsTransport(this);
				this.NextProtocolToTry = SupportedProtocols.HTTP;
			}
			this.State = ConnectionStates.Connecting;
			this.TransportConnectionStartedAt = new DateTime?(DateTime.UtcNow);
			this.Transport.Connect();
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x000A5E64 File Offset: 0x000A4064
		private void OnNegotiationError(NegotiationData data, string error)
		{
			((IConnection)this).Error(error);
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x000A5E70 File Offset: 0x000A4070
		public void Close()
		{
			if (this.State == ConnectionStates.Closed)
			{
				return;
			}
			this.State = ConnectionStates.Closed;
			this.ReconnectStarted = false;
			this.TransportConnectionStartedAt = null;
			if (this.Transport != null)
			{
				this.Transport.Abort();
				this.Transport = null;
			}
			this.NegotiationResult = null;
			HTTPManager.Heartbeats.Unsubscribe(this);
			this.LastReceivedMessage = null;
			if (this.Hubs != null)
			{
				for (int i = 0; i < this.Hubs.Length; i++)
				{
					((IHub)this.Hubs[i]).Close();
				}
			}
			if (this.BufferedMessages != null)
			{
				this.BufferedMessages.Clear();
				this.BufferedMessages = null;
			}
			if (this.OnClosed != null)
			{
				try
				{
					this.OnClosed(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("SignalR Connection", "OnClosed", ex);
				}
			}
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x000A5F54 File Offset: 0x000A4154
		public void Reconnect()
		{
			if (this.ReconnectStarted)
			{
				return;
			}
			this.ReconnectStarted = true;
			if (this.State != ConnectionStates.Reconnecting)
			{
				this.ReconnectStartedAt = DateTime.UtcNow;
			}
			this.State = ConnectionStates.Reconnecting;
			HTTPManager.Logger.Warning("SignalR Connection", "Reconnecting");
			this.Transport.Reconnect();
			if (this.PingRequest != null)
			{
				this.PingRequest.Abort();
			}
			if (this.OnReconnecting != null)
			{
				try
				{
					this.OnReconnecting(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("SignalR Connection", "OnReconnecting", ex);
				}
			}
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x000A5FFC File Offset: 0x000A41FC
		public bool Send(object arg)
		{
			if (arg == null)
			{
				throw new ArgumentNullException("arg");
			}
			if (this.State != ConnectionStates.Connected)
			{
				return false;
			}
			string text = this.JsonEncoder.Encode(arg);
			if (string.IsNullOrEmpty(text))
			{
				HTTPManager.Logger.Error("SignalR Connection", "Failed to JSon encode the given argument. Please try to use an advanced JSon encoder(check the documentation how you can do it).");
			}
			else
			{
				this.Transport.Send(text);
			}
			return true;
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x000A605A File Offset: 0x000A425A
		public bool SendJson(string json)
		{
			if (json == null)
			{
				throw new ArgumentNullException("json");
			}
			if (this.State != ConnectionStates.Connected)
			{
				return false;
			}
			this.Transport.Send(json);
			return true;
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x000A6084 File Offset: 0x000A4284
		void IConnection.OnMessage(IServerMessage msg)
		{
			if (this.State == ConnectionStates.Closed)
			{
				return;
			}
			if (this.State == ConnectionStates.Connecting)
			{
				if (this.BufferedMessages == null)
				{
					this.BufferedMessages = new List<IServerMessage>();
				}
				this.BufferedMessages.Add(msg);
				return;
			}
			this.LastMessageReceivedAt = DateTime.UtcNow;
			switch (msg.Type)
			{
			case MessageTypes.KeepAlive:
				break;
			case MessageTypes.Data:
				if (this.OnNonHubMessage != null)
				{
					this.OnNonHubMessage(this, (msg as DataMessage).Data);
					return;
				}
				break;
			case MessageTypes.Multiple:
				this.LastReceivedMessage = (msg as MultiMessage);
				if (this.LastReceivedMessage.IsInitialization)
				{
					HTTPManager.Logger.Information("SignalR Connection", "OnMessage - Init");
				}
				if (this.LastReceivedMessage.GroupsToken != null)
				{
					this.GroupsToken = this.LastReceivedMessage.GroupsToken;
				}
				if (this.LastReceivedMessage.ShouldReconnect)
				{
					HTTPManager.Logger.Information("SignalR Connection", "OnMessage - Should Reconnect");
					this.Reconnect();
				}
				if (this.LastReceivedMessage.Data != null)
				{
					for (int i = 0; i < this.LastReceivedMessage.Data.Count; i++)
					{
						((IConnection)this).OnMessage(this.LastReceivedMessage.Data[i]);
					}
					return;
				}
				break;
			case MessageTypes.Result:
			case MessageTypes.Failure:
			case MessageTypes.Progress:
			{
				ulong invocationId = (msg as IHubMessage).InvocationId;
				Hub hub = this.FindHub(invocationId);
				if (hub != null)
				{
					((IHub)hub).OnMessage(msg);
					return;
				}
				HTTPManager.Logger.Warning("SignalR Connection", string.Format("No Hub found for Progress message! Id: {0}", invocationId.ToString()));
				return;
			}
			case MessageTypes.MethodCall:
			{
				MethodCallMessage methodCallMessage = msg as MethodCallMessage;
				Hub hub = this[methodCallMessage.Hub];
				if (hub != null)
				{
					((IHub)hub).OnMethod(methodCallMessage);
					return;
				}
				HTTPManager.Logger.Warning("SignalR Connection", string.Format("Hub \"{0}\" not found!", methodCallMessage.Hub));
				return;
			}
			default:
				HTTPManager.Logger.Warning("SignalR Connection", "Unknown message type received: " + msg.Type.ToString());
				break;
			}
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x000A6284 File Offset: 0x000A4484
		void IConnection.TransportStarted()
		{
			if (this.State != ConnectionStates.Connecting)
			{
				return;
			}
			this.InitOnStart();
			if (this.OnConnected != null)
			{
				try
				{
					this.OnConnected(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("SignalR Connection", "OnOpened", ex);
				}
			}
			if (this.BufferedMessages != null)
			{
				for (int i = 0; i < this.BufferedMessages.Count; i++)
				{
					((IConnection)this).OnMessage(this.BufferedMessages[i]);
				}
				this.BufferedMessages.Clear();
				this.BufferedMessages = null;
			}
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x000A6324 File Offset: 0x000A4524
		void IConnection.TransportReconnected()
		{
			if (this.State != ConnectionStates.Reconnecting)
			{
				return;
			}
			HTTPManager.Logger.Information("SignalR Connection", "Transport Reconnected");
			this.InitOnStart();
			if (this.OnReconnected != null)
			{
				try
				{
					this.OnReconnected(this);
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("SignalR Connection", "OnReconnected", ex);
				}
			}
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x000A6394 File Offset: 0x000A4594
		void IConnection.TransportAborted()
		{
			this.Close();
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x000A639C File Offset: 0x000A459C
		void IConnection.Error(string reason)
		{
			if (this.State == ConnectionStates.Closed)
			{
				return;
			}
			if (HTTPManager.IsQuitting)
			{
				this.Close();
				return;
			}
			HTTPManager.Logger.Error("SignalR Connection", reason);
			this.ReconnectStarted = false;
			if (this.OnError != null)
			{
				this.OnError(this, reason);
			}
			if (this.State == ConnectionStates.Connected || this.State == ConnectionStates.Reconnecting)
			{
				this.ReconnectDelayStartedAt = DateTime.UtcNow;
				if (this.State != ConnectionStates.Reconnecting)
				{
					this.ReconnectStartedAt = DateTime.UtcNow;
					return;
				}
			}
			else if (this.State != ConnectionStates.Connecting || !this.TryFallbackTransport())
			{
				this.Close();
			}
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x000A6436 File Offset: 0x000A4636
		Uri IConnection.BuildUri(RequestTypes type)
		{
			return ((IConnection)this).BuildUri(type, null);
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x000A6440 File Offset: 0x000A4640
		Uri IConnection.BuildUri(RequestTypes type, TransportBase transport)
		{
			this.queryBuilder.Length = 0;
			UriBuilder uriBuilder = new UriBuilder(this.Uri);
			if (!uriBuilder.Path.EndsWith("/"))
			{
				UriBuilder uriBuilder2 = uriBuilder;
				uriBuilder2.Path += "/";
			}
			long requestCounter;
			long value;
			do
			{
				requestCounter = this.RequestCounter;
				value = requestCounter % long.MaxValue;
			}
			while (Interlocked.CompareExchange(ref this.RequestCounter, value, requestCounter) != requestCounter);
			switch (type)
			{
			case RequestTypes.Negotiate:
			{
				UriBuilder uriBuilder3 = uriBuilder;
				uriBuilder3.Path += "negotiate";
				break;
			}
			case RequestTypes.Connect:
			{
				if (transport != null && transport.Type == TransportTypes.WebSocket)
				{
					uriBuilder.Scheme = (HTTPProtocolFactory.IsSecureProtocol(this.Uri) ? "wss" : "ws");
				}
				UriBuilder uriBuilder4 = uriBuilder;
				uriBuilder4.Path += "connect";
				break;
			}
			case RequestTypes.Start:
			{
				UriBuilder uriBuilder5 = uriBuilder;
				uriBuilder5.Path += "start";
				break;
			}
			case RequestTypes.Poll:
			{
				UriBuilder uriBuilder6 = uriBuilder;
				uriBuilder6.Path += "poll";
				if (this.LastReceivedMessage != null)
				{
					this.queryBuilder.Append("messageId=");
					this.queryBuilder.Append(this.LastReceivedMessage.MessageId);
				}
				if (!string.IsNullOrEmpty(this.GroupsToken))
				{
					if (this.queryBuilder.Length > 0)
					{
						this.queryBuilder.Append("&");
					}
					this.queryBuilder.Append("groupsToken=");
					this.queryBuilder.Append(this.GroupsToken);
				}
				break;
			}
			case RequestTypes.Send:
			{
				UriBuilder uriBuilder7 = uriBuilder;
				uriBuilder7.Path += "send";
				break;
			}
			case RequestTypes.Reconnect:
			{
				if (transport != null && transport.Type == TransportTypes.WebSocket)
				{
					uriBuilder.Scheme = (HTTPProtocolFactory.IsSecureProtocol(this.Uri) ? "wss" : "ws");
				}
				UriBuilder uriBuilder8 = uriBuilder;
				uriBuilder8.Path += "reconnect";
				if (this.LastReceivedMessage != null)
				{
					this.queryBuilder.Append("messageId=");
					this.queryBuilder.Append(this.LastReceivedMessage.MessageId);
				}
				if (!string.IsNullOrEmpty(this.GroupsToken))
				{
					if (this.queryBuilder.Length > 0)
					{
						this.queryBuilder.Append("&");
					}
					this.queryBuilder.Append("groupsToken=");
					this.queryBuilder.Append(this.GroupsToken);
				}
				break;
			}
			case RequestTypes.Abort:
			{
				UriBuilder uriBuilder9 = uriBuilder;
				uriBuilder9.Path += "abort";
				break;
			}
			case RequestTypes.Ping:
			{
				UriBuilder uriBuilder10 = uriBuilder;
				uriBuilder10.Path += "ping";
				this.queryBuilder.Append("&tid=");
				this.queryBuilder.Append(Interlocked.Increment(ref this.RequestCounter).ToString());
				this.queryBuilder.Append("&_=");
				this.queryBuilder.Append(this.Timestamp.ToString());
				goto IL_458;
			}
			}
			if (this.queryBuilder.Length > 0)
			{
				this.queryBuilder.Append("&");
			}
			this.queryBuilder.Append("tid=");
			this.queryBuilder.Append(Interlocked.Increment(ref this.RequestCounter).ToString());
			this.queryBuilder.Append("&_=");
			this.queryBuilder.Append(this.Timestamp.ToString());
			if (transport != null)
			{
				this.queryBuilder.Append("&transport=");
				this.queryBuilder.Append(transport.Name);
			}
			this.queryBuilder.Append("&clientProtocol=");
			this.queryBuilder.Append(this.ClientProtocols[(int)this.Protocol]);
			if (this.NegotiationResult != null && !string.IsNullOrEmpty(this.NegotiationResult.ConnectionToken))
			{
				this.queryBuilder.Append("&connectionToken=");
				this.queryBuilder.Append(this.NegotiationResult.ConnectionToken);
			}
			if (this.Hubs != null && this.Hubs.Length != 0)
			{
				this.queryBuilder.Append("&connectionData=");
				this.queryBuilder.Append(this.ConnectionData);
			}
			IL_458:
			if (this.AdditionalQueryParams != null && this.AdditionalQueryParams.Count > 0)
			{
				this.queryBuilder.Append(this.QueryParams);
			}
			uriBuilder.Query = this.queryBuilder.ToString();
			this.queryBuilder.Length = 0;
			return uriBuilder.Uri;
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x000A68F0 File Offset: 0x000A4AF0
		HTTPRequest IConnection.PrepareRequest(HTTPRequest req, RequestTypes type)
		{
			if (req != null && this.AuthenticationProvider != null)
			{
				this.AuthenticationProvider.PrepareRequest(req, type);
			}
			if (this.RequestPreparator != null)
			{
				this.RequestPreparator(this, req, type);
			}
			return req;
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x000A6924 File Offset: 0x000A4B24
		string IConnection.ParseResponse(string responseStr)
		{
			Dictionary<string, object> dictionary = Json.Decode(responseStr) as Dictionary<string, object>;
			if (dictionary == null)
			{
				((IConnection)this).Error("Failed to parse Start response: " + responseStr);
				return string.Empty;
			}
			object obj;
			if (!dictionary.TryGetValue("Response", out obj) || obj == null)
			{
				((IConnection)this).Error("No 'Response' key found in response: " + responseStr);
				return string.Empty;
			}
			return obj.ToString();
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x000A6988 File Offset: 0x000A4B88
		void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
		{
			ConnectionStates state = this.State;
			if (state != ConnectionStates.Connected)
			{
				if (state != ConnectionStates.Reconnecting)
				{
					if (this.TransportConnectionStartedAt != null && DateTime.UtcNow - this.TransportConnectionStartedAt >= this.NegotiationResult.TransportConnectTimeout)
					{
						HTTPManager.Logger.Warning("SignalR Connection", "OnHeartbeatUpdate - Transport failed to connect in the given time!");
						((IConnection)this).Error("Transport failed to connect in the given time!");
					}
				}
				else
				{
					if (DateTime.UtcNow - this.ReconnectStartedAt >= this.NegotiationResult.DisconnectTimeout)
					{
						HTTPManager.Logger.Warning("SignalR Connection", "OnHeartbeatUpdate - Failed to reconnect in the given time!");
						this.Close();
						return;
					}
					if (DateTime.UtcNow - this.ReconnectDelayStartedAt >= this.ReconnectDelay)
					{
						if (HTTPManager.Logger.Level <= Loglevels.Warning)
						{
							HTTPManager.Logger.Warning("SignalR Connection", string.Concat(new string[]
							{
								this.ReconnectStarted.ToString(),
								" ",
								this.ReconnectStartedAt.ToString(),
								" ",
								this.NegotiationResult.DisconnectTimeout.ToString()
							}));
						}
						this.Reconnect();
						return;
					}
				}
			}
			else
			{
				if (this.Transport.SupportsKeepAlive && this.NegotiationResult.KeepAliveTimeout != null && DateTime.UtcNow - this.LastMessageReceivedAt >= this.NegotiationResult.KeepAliveTimeout)
				{
					this.Reconnect();
				}
				if (this.PingRequest == null && DateTime.UtcNow - this.LastPingSentAt >= this.PingInterval)
				{
					this.Ping();
					return;
				}
			}
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x000A6B9A File Offset: 0x000A4D9A
		private void InitOnStart()
		{
			this.State = ConnectionStates.Connected;
			this.ReconnectStarted = false;
			this.TransportConnectionStartedAt = null;
			this.LastPingSentAt = DateTime.UtcNow;
			this.LastMessageReceivedAt = DateTime.UtcNow;
			HTTPManager.Heartbeats.Subscribe(this);
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x000A6BD8 File Offset: 0x000A4DD8
		private Hub FindHub(ulong msgId)
		{
			if (this.Hubs != null)
			{
				for (int i = 0; i < this.Hubs.Length; i++)
				{
					if (((IHub)this.Hubs[i]).HasSentMessageId(msgId))
					{
						return this.Hubs[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x000A6C1C File Offset: 0x000A4E1C
		private bool TryFallbackTransport()
		{
			if (this.State == ConnectionStates.Connecting)
			{
				if (this.BufferedMessages != null)
				{
					this.BufferedMessages.Clear();
				}
				this.Transport.Stop();
				this.Transport = null;
				switch (this.NextProtocolToTry)
				{
				case SupportedProtocols.Unknown:
					return false;
				case SupportedProtocols.HTTP:
					this.Transport = new PollingTransport(this);
					this.NextProtocolToTry = SupportedProtocols.Unknown;
					break;
				case SupportedProtocols.WebSocket:
					this.Transport = new WebSocketTransport(this);
					break;
				case SupportedProtocols.ServerSentEvents:
					this.Transport = new ServerSentEventsTransport(this);
					this.NextProtocolToTry = SupportedProtocols.HTTP;
					break;
				}
				this.TransportConnectionStartedAt = new DateTime?(DateTime.UtcNow);
				this.Transport.Connect();
				if (this.PingRequest != null)
				{
					this.PingRequest.Abort();
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x000A6CE4 File Offset: 0x000A4EE4
		private void AdditionalQueryParams_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.BuiltQueryParams = null;
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x000A6CF0 File Offset: 0x000A4EF0
		private void Ping()
		{
			HTTPManager.Logger.Information("SignalR Connection", "Sending Ping request.");
			this.PingRequest = new HTTPRequest(((IConnection)this).BuildUri(RequestTypes.Ping), new OnRequestFinishedDelegate(this.OnPingRequestFinished));
			this.PingRequest.ConnectTimeout = this.PingInterval;
			((IConnection)this).PrepareRequest(this.PingRequest, RequestTypes.Ping);
			this.PingRequest.Send();
			this.LastPingSentAt = DateTime.UtcNow;
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x000A6D68 File Offset: 0x000A4F68
		private void OnPingRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			this.PingRequest = null;
			string text = string.Empty;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess)
				{
					string text2 = ((IConnection)this).ParseResponse(resp.DataAsText);
					if (text2 != "pong")
					{
						text = "Wrong answer for ping request: " + text2;
					}
					else
					{
						HTTPManager.Logger.Information("SignalR Connection", "Pong received.");
					}
				}
				else
				{
					text = string.Format("Ping - Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText);
				}
				break;
			case HTTPRequestStates.Error:
				text = "Ping - Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = "Ping - Connection Timed Out!";
				break;
			case HTTPRequestStates.TimedOut:
				text = "Ping - Processing the request Timed Out!";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				((IConnection)this).Error(text);
			}
		}

		// Token: 0x0400155E RID: 5470
		public static IJsonEncoder DefaultEncoder = new DefaultJsonEncoder();

		// Token: 0x04001560 RID: 5472
		private ConnectionStates _state;

		// Token: 0x04001565 RID: 5477
		private ObservableDictionary<string, string> additionalQueryParams;

		// Token: 0x04001573 RID: 5491
		internal long ClientMessageCounter;

		// Token: 0x04001574 RID: 5492
		private readonly string[] ClientProtocols = new string[]
		{
			"1.3",
			"1.4",
			"1.5"
		};

		// Token: 0x04001575 RID: 5493
		private long RequestCounter;

		// Token: 0x04001576 RID: 5494
		private MultiMessage LastReceivedMessage;

		// Token: 0x04001577 RID: 5495
		private string GroupsToken;

		// Token: 0x04001578 RID: 5496
		private List<IServerMessage> BufferedMessages;

		// Token: 0x04001579 RID: 5497
		private DateTime LastMessageReceivedAt;

		// Token: 0x0400157A RID: 5498
		private DateTime ReconnectStartedAt;

		// Token: 0x0400157B RID: 5499
		private DateTime ReconnectDelayStartedAt;

		// Token: 0x0400157C RID: 5500
		private bool ReconnectStarted;

		// Token: 0x0400157D RID: 5501
		private DateTime LastPingSentAt;

		// Token: 0x0400157E RID: 5502
		private HTTPRequest PingRequest;

		// Token: 0x0400157F RID: 5503
		private DateTime? TransportConnectionStartedAt;

		// Token: 0x04001580 RID: 5504
		private StringBuilder queryBuilder = new StringBuilder();

		// Token: 0x04001581 RID: 5505
		private string BuiltConnectionData;

		// Token: 0x04001582 RID: 5506
		private string BuiltQueryParams;

		// Token: 0x04001583 RID: 5507
		private SupportedProtocols NextProtocolToTry;
	}
}
