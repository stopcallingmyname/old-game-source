using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using BestHTTP.Extensions;

namespace BestHTTP.ServerSentEvents
{
	// Token: 0x02000236 RID: 566
	public sealed class EventSourceResponse : HTTPResponse, IProtocol
	{
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x000AA1A4 File Offset: 0x000A83A4
		// (set) Token: 0x0600145F RID: 5215 RVA: 0x000AA1AC File Offset: 0x000A83AC
		public bool IsClosed { get; private set; }

		// Token: 0x06001460 RID: 5216 RVA: 0x000AA1B5 File Offset: 0x000A83B5
		public EventSourceResponse(HTTPRequest request, Stream stream, bool isStreamed, bool isFromCache) : base(request, stream, isStreamed, isFromCache)
		{
			base.IsClosedManually = true;
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x000AA1D4 File Offset: 0x000A83D4
		public override bool Receive(int forceReadRawContentLength = -1, bool readPayloadData = true)
		{
			bool flag = base.Receive(forceReadRawContentLength, false);
			string firstHeaderValue = base.GetFirstHeaderValue("content-type");
			base.IsUpgraded = (flag && base.StatusCode == 200 && !string.IsNullOrEmpty(firstHeaderValue) && firstHeaderValue.ToLower().StartsWith("text/event-stream"));
			if (!base.IsUpgraded)
			{
				base.ReadPayload(forceReadRawContentLength);
			}
			return flag;
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x000AA238 File Offset: 0x000A8438
		internal void StartReceive()
		{
			if (base.IsUpgraded)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReceiveThreadFunc));
			}
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x000AA254 File Offset: 0x000A8454
		private void ReceiveThreadFunc(object param)
		{
			try
			{
				if (base.HasHeaderWithValue("transfer-encoding", "chunked"))
				{
					this.ReadChunked(this.Stream);
				}
				else
				{
					this.ReadRaw(this.Stream, -1L);
				}
			}
			catch (ThreadAbortException)
			{
				this.baseRequest.State = HTTPRequestStates.Aborted;
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
			}
			finally
			{
				this.IsClosed = true;
			}
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x000AA300 File Offset: 0x000A8500
		private new void ReadChunked(Stream stream)
		{
			int num = base.ReadChunkLength(stream);
			byte[] array = VariableSizedBufferPool.Get((long)num, true);
			while (num != 0)
			{
				if (array.Length < num)
				{
					VariableSizedBufferPool.Resize(ref array, num, true);
				}
				int num2 = 0;
				do
				{
					int num3 = stream.Read(array, num2, num - num2);
					if (num3 == 0)
					{
						goto Block_2;
					}
					num2 += num3;
				}
				while (num2 < num);
				this.FeedData(array, num2);
				HTTPResponse.ReadTo(stream, 10);
				num = base.ReadChunkLength(stream);
				continue;
				Block_2:
				throw new Exception("The remote server closed the connection unexpectedly!");
			}
			VariableSizedBufferPool.Release(array);
			base.ReadHeaders(stream);
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x000AA380 File Offset: 0x000A8580
		private new void ReadRaw(Stream stream, long contentLength)
		{
			byte[] array = VariableSizedBufferPool.Get(1024L, true);
			int num;
			do
			{
				num = stream.Read(array, 0, array.Length);
				this.FeedData(array, num);
			}
			while (num > 0);
			VariableSizedBufferPool.Release(array);
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x000AA3B8 File Offset: 0x000A85B8
		public void FeedData(byte[] buffer, int count)
		{
			if (count == -1)
			{
				count = buffer.Length;
			}
			if (count == 0)
			{
				return;
			}
			if (this.LineBuffer == null)
			{
				this.LineBuffer = VariableSizedBufferPool.Get(1024L, true);
			}
			int num = 0;
			for (;;)
			{
				int num2 = -1;
				int num3 = 1;
				int num4 = num;
				while (num4 < count && num2 == -1)
				{
					if (buffer[num4] == 13)
					{
						if (num4 + 1 < count && buffer[num4 + 1] == 10)
						{
							num3 = 2;
						}
						num2 = num4;
					}
					else if (buffer[num4] == 10)
					{
						num2 = num4;
					}
					num4++;
				}
				int num5 = (num2 == -1) ? count : num2;
				if (this.LineBuffer.Length < this.LineBufferPos + (num5 - num))
				{
					int newSize = this.LineBufferPos + (num5 - num);
					VariableSizedBufferPool.Resize(ref this.LineBuffer, newSize, true);
				}
				Array.Copy(buffer, num, this.LineBuffer, this.LineBufferPos, num5 - num);
				this.LineBufferPos += num5 - num;
				if (num2 == -1)
				{
					break;
				}
				this.ParseLine(this.LineBuffer, this.LineBufferPos);
				this.LineBufferPos = 0;
				num = num2 + num3;
				if (num2 == -1 || num >= count)
				{
					return;
				}
			}
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x000AA4BC File Offset: 0x000A86BC
		private void ParseLine(byte[] buffer, int count)
		{
			if (count == 0)
			{
				if (this.CurrentMessage != null)
				{
					this.CompletedMessages.Enqueue(this.CurrentMessage);
					this.CurrentMessage = null;
				}
				return;
			}
			if (buffer[0] == 58)
			{
				return;
			}
			int num = -1;
			int num2 = 0;
			while (num2 < count && num == -1)
			{
				if (buffer[num2] == 58)
				{
					num = num2;
				}
				num2++;
			}
			string @string;
			string text;
			if (num != -1)
			{
				@string = Encoding.UTF8.GetString(buffer, 0, num);
				if (num + 1 < count && buffer[num + 1] == 32)
				{
					num++;
				}
				num++;
				if (num >= count)
				{
					return;
				}
				text = Encoding.UTF8.GetString(buffer, num, count - num);
			}
			else
			{
				@string = Encoding.UTF8.GetString(buffer, 0, count);
				text = string.Empty;
			}
			if (this.CurrentMessage == null)
			{
				this.CurrentMessage = new Message();
			}
			if (@string == "id")
			{
				this.CurrentMessage.Id = text;
				return;
			}
			if (@string == "event")
			{
				this.CurrentMessage.Event = text;
				return;
			}
			if (@string == "data")
			{
				if (this.CurrentMessage.Data != null)
				{
					Message currentMessage = this.CurrentMessage;
					currentMessage.Data += Environment.NewLine;
				}
				Message currentMessage2 = this.CurrentMessage;
				currentMessage2.Data += text;
				return;
			}
			if (!(@string == "retry"))
			{
				return;
			}
			int num3;
			if (int.TryParse(text, out num3))
			{
				this.CurrentMessage.Retry = TimeSpan.FromMilliseconds((double)num3);
			}
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x000AA624 File Offset: 0x000A8824
		void IProtocol.HandleEvents()
		{
			if (this.OnMessage != null)
			{
				Message arg;
				while (this.CompletedMessages.TryDequeue(out arg))
				{
					try
					{
						this.OnMessage(this, arg);
					}
					catch (Exception ex)
					{
						HTTPManager.Logger.Exception("EventSourceMessage", "HandleEvents - OnMessage", ex);
					}
				}
			}
			if (this.IsClosed && this.OnClosed != null)
			{
				try
				{
					this.OnClosed(this);
				}
				catch (Exception ex2)
				{
					HTTPManager.Logger.Exception("EventSourceMessage", "HandleEvents - OnClosed", ex2);
				}
				finally
				{
					this.OnClosed = null;
				}
			}
		}

		// Token: 0x0400160B RID: 5643
		public Action<EventSourceResponse, Message> OnMessage;

		// Token: 0x0400160C RID: 5644
		public Action<EventSourceResponse> OnClosed;

		// Token: 0x0400160D RID: 5645
		private byte[] LineBuffer;

		// Token: 0x0400160E RID: 5646
		private int LineBufferPos;

		// Token: 0x0400160F RID: 5647
		private Message CurrentMessage;

		// Token: 0x04001610 RID: 5648
		private ConcurrentQueue<Message> CompletedMessages = new ConcurrentQueue<Message>();
	}
}
