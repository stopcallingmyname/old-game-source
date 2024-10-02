using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using BestHTTP.Extensions;
using BestHTTP.PlatformSupport.Threading;
using BestHTTP.WebSocket.Frames;

namespace BestHTTP.WebSocket
{
	// Token: 0x020001B8 RID: 440
	public sealed class WebSocketResponse : HTTPResponse, IHeartbeat, IProtocol
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x0009D390 File Offset: 0x0009B590
		// (set) Token: 0x0600103F RID: 4159 RVA: 0x0009D398 File Offset: 0x0009B598
		public WebSocket WebSocket { get; internal set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x0009D3A1 File Offset: 0x0009B5A1
		public bool IsClosed
		{
			get
			{
				return this.closed;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x0009D3AB File Offset: 0x0009B5AB
		// (set) Token: 0x06001042 RID: 4162 RVA: 0x0009D3B3 File Offset: 0x0009B5B3
		public TimeSpan PingFrequnecy { get; private set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06001043 RID: 4163 RVA: 0x0009D3BC File Offset: 0x0009B5BC
		// (set) Token: 0x06001044 RID: 4164 RVA: 0x0009D3C4 File Offset: 0x0009B5C4
		public ushort MaxFragmentSize { get; private set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x0009D3CD File Offset: 0x0009B5CD
		public int BufferedAmount
		{
			get
			{
				return this._bufferedAmount;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x0009D3D5 File Offset: 0x0009B5D5
		// (set) Token: 0x06001047 RID: 4167 RVA: 0x0009D3DD File Offset: 0x0009B5DD
		public int Latency { get; private set; }

		// Token: 0x06001048 RID: 4168 RVA: 0x0009D3E8 File Offset: 0x0009B5E8
		internal WebSocketResponse(HTTPRequest request, Stream stream, bool isStreamed, bool isFromCache) : base(request, stream, isStreamed, isFromCache)
		{
			base.IsClosedManually = true;
			this.closed = false;
			this.MaxFragmentSize = 32767;
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0009D47B File Offset: 0x0009B67B
		internal void StartReceive()
		{
			if (base.IsUpgraded)
			{
				ThreadedRunner.RunLongLiving(new Action(this.ReceiveThreadFunc));
			}
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0009D498 File Offset: 0x0009B698
		internal void CloseStream()
		{
			ConnectionBase connectionWith = HTTPManager.GetConnectionWith(this.baseRequest);
			if (connectionWith != null)
			{
				connectionWith.Abort(HTTPConnectionStates.Closed);
			}
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0009D4BC File Offset: 0x0009B6BC
		public void Send(string message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message must not be null!");
			}
			int byteCount = Encoding.UTF8.GetByteCount(message);
			byte[] array = VariableSizedBufferPool.Get((long)byteCount, true);
			Encoding.UTF8.GetBytes(message, 0, message.Length, array, 0);
			WebSocketFrame webSocketFrame = new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Text, array, 0UL, (ulong)((long)byteCount), true, true);
			if (webSocketFrame.Data != null && webSocketFrame.Data.Length > (int)this.MaxFragmentSize)
			{
				WebSocketFrame[] array2 = webSocketFrame.Fragment(this.MaxFragmentSize);
				object sendLock = this.SendLock;
				lock (sendLock)
				{
					this.Send(webSocketFrame);
					if (array2 != null)
					{
						for (int i = 0; i < array2.Length; i++)
						{
							this.Send(array2[i]);
						}
					}
					goto IL_C0;
				}
			}
			this.Send(webSocketFrame);
			IL_C0:
			VariableSizedBufferPool.Release(array);
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0009D5A0 File Offset: 0x0009B7A0
		public void Send(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data must not be null!");
			}
			WebSocketFrame webSocketFrame = new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Binary, data);
			if (webSocketFrame.Data != null && webSocketFrame.Data.Length > (int)this.MaxFragmentSize)
			{
				WebSocketFrame[] array = webSocketFrame.Fragment(this.MaxFragmentSize);
				object sendLock = this.SendLock;
				lock (sendLock)
				{
					this.Send(webSocketFrame);
					if (array != null)
					{
						for (int i = 0; i < array.Length; i++)
						{
							this.Send(array[i]);
						}
					}
					return;
				}
			}
			this.Send(webSocketFrame);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0009D648 File Offset: 0x0009B848
		public void Send(byte[] data, ulong offset, ulong count)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data must not be null!");
			}
			if (offset + count > (ulong)((long)data.Length))
			{
				throw new ArgumentOutOfRangeException("offset + count >= data.Length");
			}
			WebSocketFrame webSocketFrame = new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Binary, data, offset, count, true, true);
			if (webSocketFrame.Data != null && webSocketFrame.Data.Length > (int)this.MaxFragmentSize)
			{
				WebSocketFrame[] array = webSocketFrame.Fragment(this.MaxFragmentSize);
				object sendLock = this.SendLock;
				lock (sendLock)
				{
					this.Send(webSocketFrame);
					if (array != null)
					{
						for (int i = 0; i < array.Length; i++)
						{
							this.Send(array[i]);
						}
					}
					return;
				}
			}
			this.Send(webSocketFrame);
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0009D708 File Offset: 0x0009B908
		public void Send(WebSocketFrame frame)
		{
			if (frame == null)
			{
				throw new ArgumentNullException("frame is null!");
			}
			if (this.closed || this.closeSent)
			{
				return;
			}
			this.unsentFrames.Enqueue(frame);
			if (!this.sendThreadCreated)
			{
				HTTPManager.Logger.Information("WebSocketResponse", "Send - Creating thread");
				ThreadedRunner.RunLongLiving(new Action(this.SendThreadFunc));
				this.sendThreadCreated = true;
			}
			Interlocked.Add(ref this._bufferedAmount, (frame.Data != null) ? frame.DataLength : 0);
			this.newFrameSignal.Set();
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0009D7A8 File Offset: 0x0009B9A8
		public void Insert(WebSocketFrame frame)
		{
			if (frame == null)
			{
				throw new ArgumentNullException("frame is null!");
			}
			if (this.closed || this.closeSent)
			{
				return;
			}
			object sendLock = this.SendLock;
			lock (sendLock)
			{
				this.unsentFrames.Enqueue(frame);
				if (!this.sendThreadCreated)
				{
					HTTPManager.Logger.Information("WebSocketResponse", "Insert - Creating thread");
					ThreadedRunner.RunLongLiving(new Action(this.SendThreadFunc));
					this.sendThreadCreated = true;
				}
			}
			Interlocked.Add(ref this._bufferedAmount, (frame.Data != null) ? frame.DataLength : 0);
			this.newFrameSignal.Set();
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0009D874 File Offset: 0x0009BA74
		public void SendNow(WebSocketFrame frame)
		{
			if (frame == null)
			{
				throw new ArgumentNullException("frame is null!");
			}
			if (this.closed || this.closeSent)
			{
				return;
			}
			using (RawFrameData rawFrameData = frame.Get())
			{
				this.Stream.Write(rawFrameData.Data, 0, rawFrameData.Length);
				this.Stream.Flush();
			}
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0009D8F0 File Offset: 0x0009BAF0
		public void Close()
		{
			this.Close(1000, "Bye!");
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0009D904 File Offset: 0x0009BB04
		public void Close(ushort code, string msg)
		{
			if (this.closed)
			{
				return;
			}
			WebSocketFrame webSocketFrame;
			while (this.unsentFrames.TryDequeue(out webSocketFrame))
			{
			}
			Interlocked.Exchange(ref this._bufferedAmount, 0);
			this.Send(new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.ConnectionClose, WebSocket.EncodeCloseData(code, msg)));
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0009D954 File Offset: 0x0009BB54
		public void StartPinging(int frequency)
		{
			if (frequency < 100)
			{
				throw new ArgumentException("frequency must be at least 100 milliseconds!");
			}
			this.PingFrequnecy = TimeSpan.FromMilliseconds((double)frequency);
			this.lastMessage = DateTime.UtcNow;
			this.SendPing();
			HTTPManager.Heartbeats.Subscribe(this);
			HTTPUpdateDelegator.OnApplicationForegroundStateChanged = (Action<bool>)Delegate.Combine(HTTPUpdateDelegator.OnApplicationForegroundStateChanged, new Action<bool>(this.OnApplicationForegroundStateChanged));
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0009D9BC File Offset: 0x0009BBBC
		private void SendThreadFunc()
		{
			try
			{
				using (new WriteOnlyBufferedStream(this.Stream, 16384))
				{
					while (!this.closed && !this.closeSent)
					{
						this.newFrameSignal.WaitOne();
						try
						{
							WebSocketFrame webSocketFrame;
							while (this.unsentFrames.TryDequeue(out webSocketFrame))
							{
								if (!this.closeSent)
								{
									using (RawFrameData rawFrameData = webSocketFrame.Get())
									{
										this.Stream.Write(rawFrameData.Data, 0, rawFrameData.Length);
									}
									VariableSizedBufferPool.Release(webSocketFrame.Data);
									if (webSocketFrame.Type == WebSocketFrameTypes.ConnectionClose)
									{
										this.closeSent = true;
									}
								}
								Interlocked.Add(ref this._bufferedAmount, -webSocketFrame.DataLength);
							}
							this.Stream.Flush();
						}
						catch (Exception exception)
						{
							if (HTTPUpdateDelegator.IsCreated)
							{
								this.baseRequest.Exception = exception;
								this.baseRequest.State = HTTPRequestStates.Error;
							}
							else
							{
								this.baseRequest.State = HTTPRequestStates.Aborted;
							}
							this.closed = true;
						}
					}
				}
			}
			finally
			{
				this.sendThreadCreated = false;
				((IDisposable)this.newFrameSignal).Dispose();
				this.newFrameSignal = null;
				HTTPManager.Logger.Information("WebSocketResponse", "SendThread - Closed!");
			}
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x0009DB68 File Offset: 0x0009BD68
		private void ReceiveThreadFunc()
		{
			try
			{
				using (ReadOnlyBufferedStream readOnlyBufferedStream = new ReadOnlyBufferedStream(this.Stream))
				{
					while (!this.closed)
					{
						try
						{
							WebSocketFrameReader webSocketFrameReader = default(WebSocketFrameReader);
							webSocketFrameReader.Read(readOnlyBufferedStream);
							this.lastMessage = DateTime.UtcNow;
							if (webSocketFrameReader.HasMask)
							{
								this.Close(1002, "Protocol Error: masked frame received from server!");
							}
							else if (!webSocketFrameReader.IsFinal)
							{
								if (this.OnIncompleteFrame == null)
								{
									this.IncompleteFrames.Add(webSocketFrameReader);
								}
								else
								{
									this.CompletedFrames.Enqueue(webSocketFrameReader);
								}
							}
							else
							{
								switch (webSocketFrameReader.Type)
								{
								case WebSocketFrameTypes.Continuation:
									if (this.OnIncompleteFrame != null)
									{
										this.CompletedFrames.Enqueue(webSocketFrameReader);
										continue;
									}
									webSocketFrameReader.Assemble(this.IncompleteFrames);
									this.IncompleteFrames.Clear();
									break;
								case WebSocketFrameTypes.Text:
								case WebSocketFrameTypes.Binary:
									break;
								case (WebSocketFrameTypes)3:
								case (WebSocketFrameTypes)4:
								case (WebSocketFrameTypes)5:
								case (WebSocketFrameTypes)6:
								case (WebSocketFrameTypes)7:
									continue;
								case WebSocketFrameTypes.ConnectionClose:
									goto IL_188;
								case WebSocketFrameTypes.Ping:
									if (!this.closeSent && !this.closed)
									{
										this.Send(new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Pong, webSocketFrameReader.Data));
										continue;
									}
									continue;
								case WebSocketFrameTypes.Pong:
									try
									{
										long num = BitConverter.ToInt64(webSocketFrameReader.Data, 0);
										TimeSpan timeSpan = TimeSpan.FromTicks(this.lastMessage.Ticks - num);
										this.rtts.Add((int)timeSpan.TotalMilliseconds);
										this.Latency = this.CalculateLatency();
										continue;
									}
									catch
									{
										continue;
									}
									goto IL_188;
								default:
									continue;
								}
								webSocketFrameReader.DecodeWithExtensions(this.WebSocket);
								this.CompletedFrames.Enqueue(webSocketFrameReader);
								continue;
								IL_188:
								this.CloseFrame = webSocketFrameReader;
								if (!this.closeSent)
								{
									this.Send(new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.ConnectionClose, null));
								}
								this.closed = true;
							}
						}
						catch (ThreadAbortException)
						{
							this.IncompleteFrames.Clear();
							this.baseRequest.State = HTTPRequestStates.Aborted;
							this.closed = true;
							this.newFrameSignal.Set();
						}
						catch (Exception exception)
						{
							if (HTTPUpdateDelegator.IsCreated)
							{
								this.baseRequest.Exception = exception;
								this.baseRequest.State = HTTPRequestStates.Error;
							}
							else
							{
								this.baseRequest.State = HTTPRequestStates.Aborted;
							}
							this.closed = true;
							this.newFrameSignal.Set();
						}
					}
				}
			}
			finally
			{
				HTTPManager.Heartbeats.Unsubscribe(this);
				HTTPUpdateDelegator.OnApplicationForegroundStateChanged = (Action<bool>)Delegate.Remove(HTTPUpdateDelegator.OnApplicationForegroundStateChanged, new Action<bool>(this.OnApplicationForegroundStateChanged));
				HTTPManager.Logger.Information("WebSocketResponse", "ReceiveThread - Closed!");
			}
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0009DE7C File Offset: 0x0009C07C
		void IProtocol.HandleEvents()
		{
			WebSocketFrameReader arg;
			while (this.CompletedFrames.TryDequeue(out arg))
			{
				try
				{
					switch (arg.Type)
					{
					case WebSocketFrameTypes.Continuation:
						break;
					case WebSocketFrameTypes.Text:
						if (arg.IsFinal)
						{
							if (this.OnText != null)
							{
								this.OnText(this, arg.DataAsText);
								continue;
							}
							continue;
						}
						break;
					case WebSocketFrameTypes.Binary:
						if (arg.IsFinal)
						{
							if (this.OnBinary != null)
							{
								this.OnBinary(this, arg.Data);
								continue;
							}
							continue;
						}
						break;
					default:
						continue;
					}
					if (this.OnIncompleteFrame != null)
					{
						this.OnIncompleteFrame(this, arg);
					}
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("WebSocketResponse", "HandleEvents", ex);
				}
			}
			if (this.IsClosed && this.OnClosed != null && this.baseRequest.State == HTTPRequestStates.Processing)
			{
				try
				{
					ushort arg2 = 0;
					string arg3 = string.Empty;
					if (this.CloseFrame.Data != null && this.CloseFrame.Data.Length >= 2)
					{
						if (BitConverter.IsLittleEndian)
						{
							Array.Reverse(this.CloseFrame.Data, 0, 2);
						}
						arg2 = BitConverter.ToUInt16(this.CloseFrame.Data, 0);
						if (this.CloseFrame.Data.Length > 2)
						{
							arg3 = Encoding.UTF8.GetString(this.CloseFrame.Data, 2, this.CloseFrame.Data.Length - 2);
						}
						VariableSizedBufferPool.Release(this.CloseFrame.Data);
					}
					this.OnClosed(this, arg2, arg3);
				}
				catch (Exception ex2)
				{
					HTTPManager.Logger.Exception("WebSocketResponse", "HandleEvents - OnClosed", ex2);
				}
			}
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0009E044 File Offset: 0x0009C244
		void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
		{
			DateTime utcNow = DateTime.UtcNow;
			if (utcNow - this.lastPing >= this.PingFrequnecy)
			{
				this.SendPing();
			}
			if (utcNow - (this.lastMessage + this.PingFrequnecy) > this.WebSocket.CloseAfterNoMesssage)
			{
				HTTPManager.Logger.Warning("WebSocketResponse", string.Format("No message received in the given time! Closing WebSocket. LastMessage: {0}, PingFrequency: {1}, Close After: {2}, Now: {3}", new object[]
				{
					this.lastMessage,
					this.PingFrequnecy,
					this.WebSocket.CloseAfterNoMesssage,
					utcNow
				}));
				this.CloseWithError("No message received in the given time!");
			}
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0009E101 File Offset: 0x0009C301
		private void OnApplicationForegroundStateChanged(bool isPaused)
		{
			if (!isPaused)
			{
				this.lastMessage = DateTime.UtcNow;
			}
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x0009E114 File Offset: 0x0009C314
		private void SendPing()
		{
			this.lastPing = DateTime.UtcNow;
			try
			{
				byte[] bytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
				WebSocketFrame frame = new WebSocketFrame(this.WebSocket, WebSocketFrameTypes.Ping, bytes);
				this.Insert(frame);
			}
			catch
			{
				HTTPManager.Logger.Information("WebSocketResponse", "Error while sending PING message! Closing WebSocket.");
				this.CloseWithError("Error while sending PING message!");
			}
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x0009E18C File Offset: 0x0009C38C
		private void CloseWithError(string message)
		{
			this.baseRequest.Exception = new Exception(message);
			this.baseRequest.State = HTTPRequestStates.Error;
			this.closed = true;
			HTTPManager.Heartbeats.Unsubscribe(this);
			HTTPUpdateDelegator.OnApplicationForegroundStateChanged = (Action<bool>)Delegate.Remove(HTTPUpdateDelegator.OnApplicationForegroundStateChanged, new Action<bool>(this.OnApplicationForegroundStateChanged));
			this.newFrameSignal.Set();
			this.CloseStream();
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x0009E200 File Offset: 0x0009C400
		private int CalculateLatency()
		{
			if (this.rtts.Count == 0)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.rtts.Count; i++)
			{
				num += this.rtts[i];
			}
			return num / this.rtts.Count;
		}

		// Token: 0x04001413 RID: 5139
		public static int RTTBufferCapacity = 5;

		// Token: 0x04001415 RID: 5141
		public Action<WebSocketResponse, string> OnText;

		// Token: 0x04001416 RID: 5142
		public Action<WebSocketResponse, byte[]> OnBinary;

		// Token: 0x04001417 RID: 5143
		public Action<WebSocketResponse, WebSocketFrameReader> OnIncompleteFrame;

		// Token: 0x04001418 RID: 5144
		public Action<WebSocketResponse, ushort, string> OnClosed;

		// Token: 0x0400141B RID: 5147
		private int _bufferedAmount;

		// Token: 0x0400141D RID: 5149
		public DateTime lastMessage = DateTime.MinValue;

		// Token: 0x0400141E RID: 5150
		private List<WebSocketFrameReader> IncompleteFrames = new List<WebSocketFrameReader>();

		// Token: 0x0400141F RID: 5151
		private ConcurrentQueue<WebSocketFrameReader> CompletedFrames = new ConcurrentQueue<WebSocketFrameReader>();

		// Token: 0x04001420 RID: 5152
		private WebSocketFrameReader CloseFrame;

		// Token: 0x04001421 RID: 5153
		private object SendLock = new object();

		// Token: 0x04001422 RID: 5154
		private ConcurrentQueue<WebSocketFrame> unsentFrames = new ConcurrentQueue<WebSocketFrame>();

		// Token: 0x04001423 RID: 5155
		private volatile AutoResetEvent newFrameSignal = new AutoResetEvent(false);

		// Token: 0x04001424 RID: 5156
		private volatile bool sendThreadCreated;

		// Token: 0x04001425 RID: 5157
		private volatile bool closeSent;

		// Token: 0x04001426 RID: 5158
		private volatile bool closed;

		// Token: 0x04001427 RID: 5159
		private DateTime lastPing = DateTime.MinValue;

		// Token: 0x04001428 RID: 5160
		private CircularBuffer<int> rtts = new CircularBuffer<int>(WebSocketResponse.RTTBufferCapacity);
	}
}
