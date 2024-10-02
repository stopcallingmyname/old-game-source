using System;
using System.Text;
using BestHTTP.Decompression.Zlib;
using BestHTTP.Extensions;
using BestHTTP.WebSocket.Extensions;
using BestHTTP.WebSocket.Frames;

namespace BestHTTP.WebSocket
{
	// Token: 0x020001B7 RID: 439
	public sealed class WebSocket
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600101D RID: 4125 RVA: 0x0009CA78 File Offset: 0x0009AC78
		// (set) Token: 0x0600101E RID: 4126 RVA: 0x0009CA80 File Offset: 0x0009AC80
		public WebSocketStates State { get; private set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600101F RID: 4127 RVA: 0x0009CA89 File Offset: 0x0009AC89
		public bool IsOpen
		{
			get
			{
				return this.webSocket != null && !this.webSocket.IsClosed;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06001020 RID: 4128 RVA: 0x0009CAA3 File Offset: 0x0009ACA3
		public int BufferedAmount
		{
			get
			{
				return this.webSocket.BufferedAmount;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06001021 RID: 4129 RVA: 0x0009CAB0 File Offset: 0x0009ACB0
		// (set) Token: 0x06001022 RID: 4130 RVA: 0x0009CAB8 File Offset: 0x0009ACB8
		public bool StartPingThread { get; set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06001023 RID: 4131 RVA: 0x0009CAC1 File Offset: 0x0009ACC1
		// (set) Token: 0x06001024 RID: 4132 RVA: 0x0009CAC9 File Offset: 0x0009ACC9
		public int PingFrequency { get; set; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x0009CAD2 File Offset: 0x0009ACD2
		// (set) Token: 0x06001026 RID: 4134 RVA: 0x0009CADA File Offset: 0x0009ACDA
		public TimeSpan CloseAfterNoMesssage { get; set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06001027 RID: 4135 RVA: 0x0009CAE3 File Offset: 0x0009ACE3
		// (set) Token: 0x06001028 RID: 4136 RVA: 0x0009CAEB File Offset: 0x0009ACEB
		public HTTPRequest InternalRequest { get; private set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06001029 RID: 4137 RVA: 0x0009CAF4 File Offset: 0x0009ACF4
		// (set) Token: 0x0600102A RID: 4138 RVA: 0x0009CAFC File Offset: 0x0009ACFC
		public IExtension[] Extensions { get; private set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600102B RID: 4139 RVA: 0x0009CB05 File Offset: 0x0009AD05
		public int Latency
		{
			get
			{
				return this.webSocket.Latency;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600102C RID: 4140 RVA: 0x0009CB12 File Offset: 0x0009AD12
		public DateTime LastMessageReceived
		{
			get
			{
				return this.webSocket.lastMessage;
			}
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0009CB20 File Offset: 0x0009AD20
		public WebSocket(Uri uri) : this(uri, string.Empty, string.Empty, Array.Empty<IExtension>())
		{
			this.Extensions = new IExtension[]
			{
				new PerMessageCompression(CompressionLevel.Default, false, false, 15, 15, 256)
			};
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0009CB64 File Offset: 0x0009AD64
		public WebSocket(Uri uri, string origin, string protocol, params IExtension[] extensions)
		{
			string text = HTTPProtocolFactory.IsSecureProtocol(uri) ? "wss" : "ws";
			int num = (uri.Port != -1) ? uri.Port : (text.Equals("wss", StringComparison.OrdinalIgnoreCase) ? 443 : 80);
			uri = new Uri(string.Concat(new object[]
			{
				text,
				"://",
				uri.Host,
				":",
				num,
				uri.GetRequestPathAndQueryURL()
			}));
			this.PingFrequency = 1000;
			this.CloseAfterNoMesssage = TimeSpan.FromSeconds(10.0);
			this.InternalRequest = new HTTPRequest(uri, new OnRequestFinishedDelegate(this.OnInternalRequestCallback));
			this.InternalRequest.OnUpgraded = new OnRequestFinishedDelegate(this.OnInternalRequestUpgraded);
			this.InternalRequest.SetHeader("Upgrade", "websocket");
			this.InternalRequest.SetHeader("Connection", "Upgrade");
			this.InternalRequest.SetHeader("Sec-WebSocket-Key", this.GetSecKey(new object[]
			{
				this,
				this.InternalRequest,
				uri,
				new object()
			}));
			if (!string.IsNullOrEmpty(origin))
			{
				this.InternalRequest.SetHeader("Origin", origin);
			}
			this.InternalRequest.SetHeader("Sec-WebSocket-Version", "13");
			if (!string.IsNullOrEmpty(protocol))
			{
				this.InternalRequest.SetHeader("Sec-WebSocket-Protocol", protocol);
			}
			this.InternalRequest.SetHeader("Cache-Control", "no-cache");
			this.InternalRequest.SetHeader("Pragma", "no-cache");
			this.Extensions = extensions;
			this.InternalRequest.DisableCache = true;
			this.InternalRequest.DisableRetry = true;
			this.InternalRequest.TryToMinimizeTCPLatency = true;
			HTTPProxy httpproxy = HTTPManager.Proxy as HTTPProxy;
			if (httpproxy != null)
			{
				this.InternalRequest.Proxy = new HTTPProxy(httpproxy.Address, httpproxy.Credentials, false, false, httpproxy.NonTransparentForHTTPS);
			}
			HTTPManager.Setup();
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0009CD78 File Offset: 0x0009AF78
		private void OnInternalRequestCallback(HTTPRequest req, HTTPResponse resp)
		{
			string text = string.Empty;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				HTTPManager.Logger.Information("WebSocket", string.Format("Request finished. Status Code: {0} Message: {1}", resp.StatusCode.ToString(), resp.Message));
				if (resp.StatusCode == 101)
				{
					return;
				}
				text = string.Format("Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText);
				break;
			case HTTPRequestStates.Error:
				text = "Request Finished with Error! " + ((req.Exception != null) ? ("Exception: " + req.Exception.Message + req.Exception.StackTrace) : string.Empty);
				break;
			case HTTPRequestStates.Aborted:
				text = "Request Aborted!";
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = "Connection Timed Out!";
				break;
			case HTTPRequestStates.TimedOut:
				text = "Processing the request Timed Out!";
				break;
			default:
				return;
			}
			if (this.State != WebSocketStates.Connecting || !string.IsNullOrEmpty(text))
			{
				if (this.OnError != null)
				{
					this.OnError(this, req.Exception);
				}
				if (this.OnErrorDesc != null)
				{
					this.OnErrorDesc(this, text);
				}
				if (this.OnError == null && this.OnErrorDesc == null)
				{
					HTTPManager.Logger.Error("WebSocket", text);
				}
			}
			else if (this.OnClosed != null)
			{
				this.OnClosed(this, 1000, "Closed while opening");
			}
			if (!req.IsKeepAlive && resp != null && resp is WebSocketResponse)
			{
				(resp as WebSocketResponse).CloseStream();
			}
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0009CEFC File Offset: 0x0009B0FC
		private void OnInternalRequestUpgraded(HTTPRequest req, HTTPResponse resp)
		{
			this.webSocket = (resp as WebSocketResponse);
			if (this.webSocket == null)
			{
				if (this.OnError != null)
				{
					this.OnError(this, req.Exception);
				}
				if (this.OnErrorDesc != null)
				{
					string reason = string.Empty;
					if (req.Exception != null)
					{
						reason = req.Exception.Message + " " + req.Exception.StackTrace;
					}
					this.OnErrorDesc(this, reason);
				}
				this.State = WebSocketStates.Closed;
				return;
			}
			if (this.State == WebSocketStates.Closed)
			{
				this.webSocket.CloseStream();
				return;
			}
			this.webSocket.WebSocket = this;
			if (this.Extensions != null)
			{
				for (int i = 0; i < this.Extensions.Length; i++)
				{
					IExtension extension = this.Extensions[i];
					try
					{
						if (extension != null && !extension.ParseNegotiation(this.webSocket))
						{
							this.Extensions[i] = null;
						}
					}
					catch (Exception ex)
					{
						HTTPManager.Logger.Exception("WebSocket", "ParseNegotiation", ex);
						this.Extensions[i] = null;
					}
				}
			}
			this.State = WebSocketStates.Open;
			if (this.OnOpen != null)
			{
				try
				{
					this.OnOpen(this);
				}
				catch (Exception ex2)
				{
					HTTPManager.Logger.Exception("WebSocket", "OnOpen", ex2);
				}
			}
			this.webSocket.OnText = delegate(WebSocketResponse ws, string msg)
			{
				if (this.OnMessage != null)
				{
					this.OnMessage(this, msg);
				}
			};
			this.webSocket.OnBinary = delegate(WebSocketResponse ws, byte[] bin)
			{
				if (this.OnBinary != null)
				{
					this.OnBinary(this, bin);
				}
			};
			this.webSocket.OnClosed = delegate(WebSocketResponse ws, ushort code, string msg)
			{
				this.State = WebSocketStates.Closed;
				if (this.OnClosed != null)
				{
					this.OnClosed(this, code, msg);
				}
			};
			if (this.OnIncompleteFrame != null)
			{
				this.webSocket.OnIncompleteFrame = delegate(WebSocketResponse ws, WebSocketFrameReader frame)
				{
					if (this.OnIncompleteFrame != null)
					{
						this.OnIncompleteFrame(this, frame);
					}
				};
			}
			if (this.StartPingThread)
			{
				this.webSocket.StartPinging(Math.Max(this.PingFrequency, 100));
			}
			this.webSocket.StartReceive();
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0009D0E8 File Offset: 0x0009B2E8
		public void Open()
		{
			if (this.requestSent)
			{
				throw new InvalidOperationException("Open already called! You can't reuse this WebSocket instance!");
			}
			if (this.Extensions != null)
			{
				try
				{
					for (int i = 0; i < this.Extensions.Length; i++)
					{
						IExtension extension = this.Extensions[i];
						if (extension != null)
						{
							extension.AddNegotiation(this.InternalRequest);
						}
					}
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("WebSocket", "Open", ex);
				}
			}
			this.InternalRequest.Send();
			this.requestSent = true;
			this.State = WebSocketStates.Connecting;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0009D180 File Offset: 0x0009B380
		public void Send(string message)
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.webSocket.Send(message);
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0009D197 File Offset: 0x0009B397
		public void Send(byte[] buffer)
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.webSocket.Send(buffer);
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0009D1AE File Offset: 0x0009B3AE
		public void Send(byte[] buffer, ulong offset, ulong count)
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.webSocket.Send(buffer, offset, count);
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0009D1C7 File Offset: 0x0009B3C7
		public void Send(WebSocketFrame frame)
		{
			if (this.IsOpen)
			{
				this.webSocket.Send(frame);
			}
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0009D1E0 File Offset: 0x0009B3E0
		public void Close()
		{
			if (this.State >= WebSocketStates.Closing)
			{
				return;
			}
			if (this.State == WebSocketStates.Connecting)
			{
				this.State = WebSocketStates.Closed;
				if (this.OnClosed != null)
				{
					this.OnClosed(this, 1005, string.Empty);
					return;
				}
			}
			else
			{
				this.State = WebSocketStates.Closing;
				this.webSocket.Close();
			}
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0009D237 File Offset: 0x0009B437
		public void Close(ushort code, string message)
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.webSocket.Close(code, message);
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0009D250 File Offset: 0x0009B450
		public static byte[] EncodeCloseData(ushort code, string message)
		{
			int byteCount = Encoding.UTF8.GetByteCount(message);
			byte[] result;
			using (BufferPoolMemoryStream bufferPoolMemoryStream = new BufferPoolMemoryStream(2 + byteCount))
			{
				byte[] bytes = BitConverter.GetBytes(code);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(bytes, 0, bytes.Length);
				}
				bufferPoolMemoryStream.Write(bytes, 0, bytes.Length);
				bytes = Encoding.UTF8.GetBytes(message);
				bufferPoolMemoryStream.Write(bytes, 0, bytes.Length);
				result = bufferPoolMemoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0009D2D0 File Offset: 0x0009B4D0
		private string GetSecKey(object[] from)
		{
			byte[] array = new byte[16];
			int num = 0;
			for (int i = 0; i < from.Length; i++)
			{
				byte[] bytes = BitConverter.GetBytes(from[i].GetHashCode());
				int num2 = 0;
				while (num2 < bytes.Length && num < array.Length)
				{
					array[num++] = bytes[num2];
					num2++;
				}
			}
			return Convert.ToBase64String(array);
		}

		// Token: 0x0400140A RID: 5130
		public OnWebSocketOpenDelegate OnOpen;

		// Token: 0x0400140B RID: 5131
		public OnWebSocketMessageDelegate OnMessage;

		// Token: 0x0400140C RID: 5132
		public OnWebSocketBinaryDelegate OnBinary;

		// Token: 0x0400140D RID: 5133
		public OnWebSocketClosedDelegate OnClosed;

		// Token: 0x0400140E RID: 5134
		public OnWebSocketErrorDelegate OnError;

		// Token: 0x0400140F RID: 5135
		public OnWebSocketErrorDescriptionDelegate OnErrorDesc;

		// Token: 0x04001410 RID: 5136
		public OnWebSocketIncompleteFrameDelegate OnIncompleteFrame;

		// Token: 0x04001411 RID: 5137
		private bool requestSent;

		// Token: 0x04001412 RID: 5138
		private WebSocketResponse webSocket;
	}
}
