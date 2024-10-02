using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.Logger;

namespace BestHTTP.SocketIO.Transports
{
	// Token: 0x020001D0 RID: 464
	internal sealed class PollingTransport : ITransport
	{
		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x0600115F RID: 4447 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public TransportTypes Type
		{
			get
			{
				return TransportTypes.Polling;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x000A1139 File Offset: 0x0009F339
		// (set) Token: 0x06001161 RID: 4449 RVA: 0x000A1141 File Offset: 0x0009F341
		public TransportStates State { get; private set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06001162 RID: 4450 RVA: 0x000A114A File Offset: 0x0009F34A
		// (set) Token: 0x06001163 RID: 4451 RVA: 0x000A1152 File Offset: 0x0009F352
		public SocketManager Manager { get; private set; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06001164 RID: 4452 RVA: 0x000A115B File Offset: 0x0009F35B
		public bool IsRequestInProgress
		{
			get
			{
				return this.LastRequest != null;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06001165 RID: 4453 RVA: 0x000A1166 File Offset: 0x0009F366
		public bool IsPollingInProgress
		{
			get
			{
				return this.PollRequest != null;
			}
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x000A1171 File Offset: 0x0009F371
		public PollingTransport(SocketManager manager)
		{
			this.Manager = manager;
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x000A118C File Offset: 0x0009F38C
		public void Open()
		{
			string text = "{0}?EIO={1}&transport=polling&t={2}-{3}{5}";
			if (this.Manager.Handshake != null)
			{
				text += "&sid={4}";
			}
			bool flag = !this.Manager.Options.QueryParamsOnlyForHandshake || (this.Manager.Options.QueryParamsOnlyForHandshake && this.Manager.Handshake == null);
			string format = text;
			object[] array = new object[6];
			array[0] = this.Manager.Uri.ToString();
			array[1] = 4;
			array[2] = this.Manager.Timestamp.ToString();
			int num = 3;
			SocketManager manager = this.Manager;
			ulong requestCounter = manager.RequestCounter;
			manager.RequestCounter = requestCounter + 1UL;
			array[num] = requestCounter.ToString();
			array[4] = ((this.Manager.Handshake != null) ? this.Manager.Handshake.Sid : string.Empty);
			array[5] = (flag ? this.Manager.Options.BuildQueryParams() : string.Empty);
			new HTTPRequest(new Uri(string.Format(format, array)), new OnRequestFinishedDelegate(this.OnRequestFinished))
			{
				DisableCache = true,
				DisableRetry = true
			}.Send();
			this.State = TransportStates.Opening;
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x000A12C4 File Offset: 0x0009F4C4
		public void Close()
		{
			if (this.State == TransportStates.Closed)
			{
				return;
			}
			this.State = TransportStates.Closed;
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x000A12D8 File Offset: 0x0009F4D8
		public void Send(Packet packet)
		{
			try
			{
				this.lonelyPacketList.Add(packet);
				this.Send(this.lonelyPacketList);
			}
			finally
			{
				this.lonelyPacketList.Clear();
			}
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x000A131C File Offset: 0x0009F51C
		public void Send(List<Packet> packets)
		{
			if (this.State != TransportStates.Opening && this.State != TransportStates.Open)
			{
				return;
			}
			if (this.IsRequestInProgress)
			{
				throw new Exception("Sending packets are still in progress!");
			}
			byte[] array = null;
			try
			{
				array = packets[0].EncodeBinary();
				for (int i = 1; i < packets.Count; i++)
				{
					byte[] array2 = packets[i].EncodeBinary();
					Array.Resize<byte>(ref array, array.Length + array2.Length);
					Array.Copy(array2, 0, array, array.Length - array2.Length, array2.Length);
				}
				packets.Clear();
			}
			catch (Exception ex)
			{
				((IManager)this.Manager).EmitError(SocketIOErrors.Internal, ex.Message + " " + ex.StackTrace);
				return;
			}
			string format = "{0}?EIO={1}&transport=polling&t={2}-{3}&sid={4}{5}";
			object[] array3 = new object[6];
			array3[0] = this.Manager.Uri.ToString();
			array3[1] = 4;
			array3[2] = this.Manager.Timestamp.ToString();
			int num = 3;
			SocketManager manager = this.Manager;
			ulong requestCounter = manager.RequestCounter;
			manager.RequestCounter = requestCounter + 1UL;
			array3[num] = requestCounter.ToString();
			array3[4] = this.Manager.Handshake.Sid;
			array3[5] = ((!this.Manager.Options.QueryParamsOnlyForHandshake) ? this.Manager.Options.BuildQueryParams() : string.Empty);
			this.LastRequest = new HTTPRequest(new Uri(string.Format(format, array3)), HTTPMethods.Post, new OnRequestFinishedDelegate(this.OnRequestFinished));
			this.LastRequest.DisableCache = true;
			this.LastRequest.SetHeader("Content-Type", "application/octet-stream");
			this.LastRequest.RawData = array;
			this.LastRequest.Send();
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x000A14D8 File Offset: 0x0009F6D8
		private void OnRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			this.LastRequest = null;
			if (this.State == TransportStates.Closed)
			{
				return;
			}
			string text = null;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (HTTPManager.Logger.Level <= Loglevels.All)
				{
					HTTPManager.Logger.Verbose("PollingTransport", "OnRequestFinished: " + resp.DataAsText);
				}
				if (resp.IsSuccess)
				{
					if (req.MethodType != HTTPMethods.Post)
					{
						this.ParseResponse(resp);
					}
				}
				else
				{
					text = string.Format("Polling - Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2} Uri: {3}", new object[]
					{
						resp.StatusCode,
						resp.Message,
						resp.DataAsText,
						req.CurrentUri
					});
				}
				break;
			case HTTPRequestStates.Error:
				text = ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.Aborted:
				text = string.Format("Polling - Request({0}) Aborted!", req.CurrentUri);
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = string.Format("Polling - Connection Timed Out! Uri: {0}", req.CurrentUri);
				break;
			case HTTPRequestStates.TimedOut:
				text = string.Format("Polling - Processing the request({0}) Timed Out!", req.CurrentUri);
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				((IManager)this.Manager).OnTransportError(this, text);
			}
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x000A1628 File Offset: 0x0009F828
		public void Poll()
		{
			if (this.PollRequest != null || this.State == TransportStates.Paused)
			{
				return;
			}
			string format = "{0}?EIO={1}&transport=polling&t={2}-{3}&sid={4}{5}";
			object[] array = new object[6];
			array[0] = this.Manager.Uri.ToString();
			array[1] = 4;
			array[2] = this.Manager.Timestamp.ToString();
			int num = 3;
			SocketManager manager = this.Manager;
			ulong requestCounter = manager.RequestCounter;
			manager.RequestCounter = requestCounter + 1UL;
			array[num] = requestCounter.ToString();
			array[4] = this.Manager.Handshake.Sid;
			array[5] = ((!this.Manager.Options.QueryParamsOnlyForHandshake) ? this.Manager.Options.BuildQueryParams() : string.Empty);
			this.PollRequest = new HTTPRequest(new Uri(string.Format(format, array)), HTTPMethods.Get, new OnRequestFinishedDelegate(this.OnPollRequestFinished));
			this.PollRequest.DisableCache = true;
			this.PollRequest.DisableRetry = true;
			this.PollRequest.Send();
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x000A172C File Offset: 0x0009F92C
		private void OnPollRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			this.PollRequest = null;
			if (this.State == TransportStates.Closed)
			{
				return;
			}
			string text = null;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (HTTPManager.Logger.Level <= Loglevels.All)
				{
					HTTPManager.Logger.Verbose("PollingTransport", "OnPollRequestFinished: " + resp.DataAsText);
				}
				if (resp.IsSuccess)
				{
					this.ParseResponse(resp);
				}
				else
				{
					text = string.Format("Polling - Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2} Uri: {3}", new object[]
					{
						resp.StatusCode,
						resp.Message,
						resp.DataAsText,
						req.CurrentUri
					});
				}
				break;
			case HTTPRequestStates.Error:
				text = ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.Aborted:
				text = string.Format("Polling - Request({0}) Aborted!", req.CurrentUri);
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = string.Format("Polling - Connection Timed Out! Uri: {0}", req.CurrentUri);
				break;
			case HTTPRequestStates.TimedOut:
				text = string.Format("Polling - Processing the request({0}) Timed Out!", req.CurrentUri);
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				((IManager)this.Manager).OnTransportError(this, text);
			}
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x000A1870 File Offset: 0x0009FA70
		private void OnPacket(Packet packet)
		{
			if (packet.AttachmentCount != 0 && !packet.HasAllAttachment)
			{
				this.PacketWithAttachment = packet;
				return;
			}
			TransportEventTypes transportEvent = packet.TransportEvent;
			if (transportEvent != TransportEventTypes.Open)
			{
				if (transportEvent == TransportEventTypes.Message)
				{
					if (packet.SocketIOEvent == SocketIOEventTypes.Connect)
					{
						this.State = TransportStates.Open;
					}
				}
			}
			else if (this.State != TransportStates.Opening)
			{
				HTTPManager.Logger.Warning("PollingTransport", "Received 'Open' packet while state is '" + this.State.ToString() + "'");
			}
			else
			{
				this.State = TransportStates.Open;
			}
			((IManager)this.Manager).OnPacket(packet);
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x000A1908 File Offset: 0x0009FB08
		private void ParseResponse(HTTPResponse resp)
		{
			try
			{
				if (resp != null && resp.Data != null && resp.Data.Length >= 1)
				{
					int num;
					for (int i = 0; i < resp.Data.Length; i += num)
					{
						PollingTransport.PayloadTypes payloadTypes = PollingTransport.PayloadTypes.Text;
						num = 0;
						if (resp.Data[i] < 48)
						{
							payloadTypes = (PollingTransport.PayloadTypes)resp.Data[i++];
							for (byte b = resp.Data[i++]; b != 255; b = resp.Data[i++])
							{
								num = num * 10 + (int)b;
							}
						}
						else
						{
							for (byte b2 = resp.Data[i++]; b2 != 58; b2 = resp.Data[i++])
							{
								num = num * 10 + (int)(b2 - 48);
							}
						}
						Packet packet = null;
						if (payloadTypes != PollingTransport.PayloadTypes.Text)
						{
							if (payloadTypes == PollingTransport.PayloadTypes.Binary)
							{
								if (this.PacketWithAttachment != null)
								{
									i++;
									num--;
									byte[] array = new byte[num];
									Array.Copy(resp.Data, i, array, 0, num);
									this.PacketWithAttachment.AddAttachmentFromServer(array, true);
									if (this.PacketWithAttachment.HasAllAttachment)
									{
										packet = this.PacketWithAttachment;
										this.PacketWithAttachment = null;
									}
								}
							}
						}
						else
						{
							packet = new Packet(Encoding.UTF8.GetString(resp.Data, i, num));
						}
						if (packet != null)
						{
							try
							{
								this.OnPacket(packet);
							}
							catch (Exception ex)
							{
								HTTPManager.Logger.Exception("PollingTransport", "ParseResponse - OnPacket", ex);
								((IManager)this.Manager).EmitError(SocketIOErrors.Internal, ex.Message + " " + ex.StackTrace);
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				((IManager)this.Manager).EmitError(SocketIOErrors.Internal, ex2.Message + " " + ex2.StackTrace);
				HTTPManager.Logger.Exception("PollingTransport", "ParseResponse", ex2);
			}
		}

		// Token: 0x040014C7 RID: 5319
		private HTTPRequest LastRequest;

		// Token: 0x040014C8 RID: 5320
		private HTTPRequest PollRequest;

		// Token: 0x040014C9 RID: 5321
		private Packet PacketWithAttachment;

		// Token: 0x040014CA RID: 5322
		private List<Packet> lonelyPacketList = new List<Packet>(1);

		// Token: 0x020008ED RID: 2285
		private enum PayloadTypes : byte
		{
			// Token: 0x0400348D RID: 13453
			Text,
			// Token: 0x0400348E RID: 13454
			Binary
		}
	}
}
