using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.JSON;
using BestHTTP.SignalRCore.Messages;
using BestHTTP.WebSocket;

namespace BestHTTP.SignalRCore.Transports
{
	// Token: 0x020001F1 RID: 497
	internal sealed class WebSocketTransport : ITransport
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600126B RID: 4715 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public TransportTypes TransportType
		{
			get
			{
				return TransportTypes.WebSocket;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public TransferModes TransferMode
		{
			get
			{
				return TransferModes.Binary;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600126D RID: 4717 RVA: 0x000A486F File Offset: 0x000A2A6F
		// (set) Token: 0x0600126E RID: 4718 RVA: 0x000A4878 File Offset: 0x000A2A78
		public TransportStates State
		{
			get
			{
				return this._state;
			}
			private set
			{
				if (this._state != value)
				{
					TransportStates state = this._state;
					this._state = value;
					if (this.OnStateChanged != null)
					{
						this.OnStateChanged(state, this._state);
					}
				}
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600126F RID: 4719 RVA: 0x000A48B6 File Offset: 0x000A2AB6
		// (set) Token: 0x06001270 RID: 4720 RVA: 0x000A48BE File Offset: 0x000A2ABE
		public string ErrorReason { get; private set; }

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06001271 RID: 4721 RVA: 0x000A48C8 File Offset: 0x000A2AC8
		// (remove) Token: 0x06001272 RID: 4722 RVA: 0x000A4900 File Offset: 0x000A2B00
		public event Action<TransportStates, TransportStates> OnStateChanged;

		// Token: 0x06001273 RID: 4723 RVA: 0x000A4935 File Offset: 0x000A2B35
		public WebSocketTransport(HubConnection con)
		{
			this.connection = con;
			this.State = TransportStates.Initial;
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x000A4958 File Offset: 0x000A2B58
		void ITransport.StartConnect()
		{
			HTTPManager.Logger.Verbose("WebSocketTransport", "StartConnect");
			if (this.webSocket == null)
			{
				Uri uri = this.BuildUri(this.connection.Uri);
				if (this.connection.AuthenticationProvider != null)
				{
					uri = (this.connection.AuthenticationProvider.PrepareUri(uri) ?? uri);
				}
				HTTPManager.Logger.Verbose("WebSocketTransport", "StartConnect connecting to Uri: " + uri.ToString());
				this.webSocket = new WebSocket(uri);
			}
			if (this.connection.AuthenticationProvider != null)
			{
				this.connection.AuthenticationProvider.PrepareRequest(this.webSocket.InternalRequest);
			}
			WebSocket webSocket = this.webSocket;
			webSocket.OnOpen = (OnWebSocketOpenDelegate)Delegate.Combine(webSocket.OnOpen, new OnWebSocketOpenDelegate(this.OnOpen));
			WebSocket webSocket2 = this.webSocket;
			webSocket2.OnMessage = (OnWebSocketMessageDelegate)Delegate.Combine(webSocket2.OnMessage, new OnWebSocketMessageDelegate(this.OnMessage));
			WebSocket webSocket3 = this.webSocket;
			webSocket3.OnBinary = (OnWebSocketBinaryDelegate)Delegate.Combine(webSocket3.OnBinary, new OnWebSocketBinaryDelegate(this.OnBinary));
			WebSocket webSocket4 = this.webSocket;
			webSocket4.OnErrorDesc = (OnWebSocketErrorDescriptionDelegate)Delegate.Combine(webSocket4.OnErrorDesc, new OnWebSocketErrorDescriptionDelegate(this.OnError));
			WebSocket webSocket5 = this.webSocket;
			webSocket5.OnClosed = (OnWebSocketClosedDelegate)Delegate.Combine(webSocket5.OnClosed, new OnWebSocketClosedDelegate(this.OnClosed));
			this.webSocket.Open();
			this.State = TransportStates.Connecting;
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x000A4ADF File Offset: 0x000A2CDF
		void ITransport.Send(byte[] msg)
		{
			if (this.webSocket == null)
			{
				return;
			}
			this.webSocket.Send(msg);
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x000A4AF8 File Offset: 0x000A2CF8
		private void OnOpen(WebSocket webSocket)
		{
			HTTPManager.Logger.Verbose("WebSocketTransport", "OnOpen");
			byte[] msg = JsonProtocol.WithSeparator(string.Format("{{'protocol':'{0}', 'version': 1}}", this.connection.Protocol.Encoder.Name));
			((ITransport)this).Send(msg);
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x000A4B48 File Offset: 0x000A2D48
		private string ParseHandshakeResponse(string data)
		{
			Dictionary<string, object> dictionary = Json.Decode(data) as Dictionary<string, object>;
			if (dictionary == null)
			{
				return "Couldn't parse json data: " + data;
			}
			object obj;
			if (dictionary.TryGetValue("error", out obj))
			{
				return obj.ToString();
			}
			return null;
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x000A4B87 File Offset: 0x000A2D87
		private void HandleHandshakeResponse(string data)
		{
			this.ErrorReason = this.ParseHandshakeResponse(data);
			this.State = (string.IsNullOrEmpty(this.ErrorReason) ? TransportStates.Connected : TransportStates.Failed);
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x000A4BB0 File Offset: 0x000A2DB0
		private void OnMessage(WebSocket webSocket, string data)
		{
			if (this.State == TransportStates.Connecting)
			{
				this.HandleHandshakeResponse(data);
				return;
			}
			this.messages.Clear();
			try
			{
				this.connection.Protocol.ParseMessages(data, ref this.messages);
				this.connection.OnMessages(this.messages);
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("WebSocketTransport", "OnMessage(string)", ex);
			}
			finally
			{
				this.messages.Clear();
			}
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x000A4C44 File Offset: 0x000A2E44
		private void OnBinary(WebSocket webSocket, byte[] data)
		{
			if (this.State == TransportStates.Connecting)
			{
				this.HandleHandshakeResponse(Encoding.UTF8.GetString(data, 0, data.Length));
				return;
			}
			this.messages.Clear();
			try
			{
				this.connection.Protocol.ParseMessages(data, ref this.messages);
				this.connection.OnMessages(this.messages);
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("WebSocketTransport", "OnMessage(byte[])", ex);
			}
			finally
			{
				this.messages.Clear();
			}
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x000A4CE8 File Offset: 0x000A2EE8
		private void OnError(WebSocket webSocket, string reason)
		{
			HTTPManager.Logger.Verbose("WebSocketTransport", "OnError: " + reason);
			this.ErrorReason = reason;
			this.State = TransportStates.Failed;
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x000A4D14 File Offset: 0x000A2F14
		private void OnClosed(WebSocket webSocket, ushort code, string message)
		{
			HTTPManager.Logger.Verbose("WebSocketTransport", string.Concat(new object[]
			{
				"OnClosed: ",
				code,
				" ",
				message
			}));
			this.webSocket = null;
			this.State = TransportStates.Closed;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x000A4D66 File Offset: 0x000A2F66
		void ITransport.StartClose()
		{
			HTTPManager.Logger.Verbose("WebSocketTransport", "StartClose");
			if (this.webSocket != null)
			{
				this.State = TransportStates.Closing;
				this.webSocket.Close();
				return;
			}
			this.State = TransportStates.Closed;
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x000A4DA0 File Offset: 0x000A2FA0
		private Uri BuildUri(Uri baseUri)
		{
			if (this.connection.NegotiationResult == null)
			{
				return baseUri;
			}
			UriBuilder uriBuilder = new UriBuilder(baseUri);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(baseUri.Query);
			if (!string.IsNullOrEmpty(this.connection.NegotiationResult.ConnectionId))
			{
				stringBuilder.Append("&id=").Append(this.connection.NegotiationResult.ConnectionId);
			}
			uriBuilder.Query = stringBuilder.ToString();
			return uriBuilder.Uri;
		}

		// Token: 0x0400151F RID: 5407
		private TransportStates _state;

		// Token: 0x04001522 RID: 5410
		private WebSocket webSocket;

		// Token: 0x04001523 RID: 5411
		private HubConnection connection;

		// Token: 0x04001524 RID: 5412
		private List<Message> messages = new List<Message>();
	}
}
