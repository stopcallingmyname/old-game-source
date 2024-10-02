using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.JSON;
using BestHTTP.SocketIO.JsonEncoders;

namespace BestHTTP.SocketIO
{
	// Token: 0x020001C9 RID: 457
	public sealed class Packet
	{
		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060010C4 RID: 4292 RVA: 0x0009F16E File Offset: 0x0009D36E
		// (set) Token: 0x060010C5 RID: 4293 RVA: 0x0009F176 File Offset: 0x0009D376
		public TransportEventTypes TransportEvent { get; private set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060010C6 RID: 4294 RVA: 0x0009F17F File Offset: 0x0009D37F
		// (set) Token: 0x060010C7 RID: 4295 RVA: 0x0009F187 File Offset: 0x0009D387
		public SocketIOEventTypes SocketIOEvent { get; private set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x0009F190 File Offset: 0x0009D390
		// (set) Token: 0x060010C9 RID: 4297 RVA: 0x0009F198 File Offset: 0x0009D398
		public int AttachmentCount { get; private set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060010CA RID: 4298 RVA: 0x0009F1A1 File Offset: 0x0009D3A1
		// (set) Token: 0x060010CB RID: 4299 RVA: 0x0009F1A9 File Offset: 0x0009D3A9
		public int Id { get; private set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060010CC RID: 4300 RVA: 0x0009F1B2 File Offset: 0x0009D3B2
		// (set) Token: 0x060010CD RID: 4301 RVA: 0x0009F1BA File Offset: 0x0009D3BA
		public string Namespace { get; private set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060010CE RID: 4302 RVA: 0x0009F1C3 File Offset: 0x0009D3C3
		// (set) Token: 0x060010CF RID: 4303 RVA: 0x0009F1CB File Offset: 0x0009D3CB
		public string Payload { get; private set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060010D0 RID: 4304 RVA: 0x0009F1D4 File Offset: 0x0009D3D4
		// (set) Token: 0x060010D1 RID: 4305 RVA: 0x0009F1DC File Offset: 0x0009D3DC
		public string EventName { get; private set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060010D2 RID: 4306 RVA: 0x0009F1E5 File Offset: 0x0009D3E5
		// (set) Token: 0x060010D3 RID: 4307 RVA: 0x0009F1ED File Offset: 0x0009D3ED
		public List<byte[]> Attachments
		{
			get
			{
				return this.attachments;
			}
			set
			{
				this.attachments = value;
				this.AttachmentCount = ((this.attachments != null) ? this.attachments.Count : 0);
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060010D4 RID: 4308 RVA: 0x0009F212 File Offset: 0x0009D412
		public bool HasAllAttachment
		{
			get
			{
				return this.Attachments != null && this.Attachments.Count == this.AttachmentCount;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060010D5 RID: 4309 RVA: 0x0009F231 File Offset: 0x0009D431
		// (set) Token: 0x060010D6 RID: 4310 RVA: 0x0009F239 File Offset: 0x0009D439
		public bool IsDecoded { get; private set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x0009F242 File Offset: 0x0009D442
		// (set) Token: 0x060010D8 RID: 4312 RVA: 0x0009F24A File Offset: 0x0009D44A
		public object[] DecodedArgs { get; private set; }

		// Token: 0x060010D9 RID: 4313 RVA: 0x0009F253 File Offset: 0x0009D453
		internal Packet()
		{
			this.TransportEvent = TransportEventTypes.Unknown;
			this.SocketIOEvent = SocketIOEventTypes.Unknown;
			this.Payload = string.Empty;
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x0009F274 File Offset: 0x0009D474
		internal Packet(string from)
		{
			this.Parse(from);
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0009F283 File Offset: 0x0009D483
		public Packet(TransportEventTypes transportEvent, SocketIOEventTypes packetType, string nsp, string payload, int attachment = 0, int id = 0)
		{
			this.TransportEvent = transportEvent;
			this.SocketIOEvent = packetType;
			this.Namespace = nsp;
			this.Payload = payload;
			this.AttachmentCount = attachment;
			this.Id = id;
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0009F2B8 File Offset: 0x0009D4B8
		public object[] Decode(IJsonEncoder encoder)
		{
			if (this.IsDecoded || encoder == null)
			{
				return this.DecodedArgs;
			}
			this.IsDecoded = true;
			if (string.IsNullOrEmpty(this.Payload))
			{
				return this.DecodedArgs;
			}
			List<object> list = encoder.Decode(this.Payload);
			if (list != null && list.Count > 0)
			{
				if (this.SocketIOEvent == SocketIOEventTypes.Ack || this.SocketIOEvent == SocketIOEventTypes.BinaryAck)
				{
					this.DecodedArgs = list.ToArray();
				}
				else
				{
					list.RemoveAt(0);
					this.DecodedArgs = list.ToArray();
				}
			}
			return this.DecodedArgs;
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0009F344 File Offset: 0x0009D544
		public string DecodeEventName()
		{
			if (!string.IsNullOrEmpty(this.EventName))
			{
				return this.EventName;
			}
			if (string.IsNullOrEmpty(this.Payload))
			{
				return string.Empty;
			}
			if (this.Payload[0] != '[')
			{
				return string.Empty;
			}
			int num = 1;
			while (this.Payload.Length > num && this.Payload[num] != '"' && this.Payload[num] != '\'')
			{
				num++;
			}
			if (this.Payload.Length <= num)
			{
				return string.Empty;
			}
			int num2;
			num = (num2 = num + 1);
			while (this.Payload.Length > num && this.Payload[num] != '"' && this.Payload[num] != '\'')
			{
				num++;
			}
			if (this.Payload.Length <= num)
			{
				return string.Empty;
			}
			return this.EventName = this.Payload.Substring(num2, num - num2);
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x0009F440 File Offset: 0x0009D640
		public string RemoveEventName(bool removeArrayMarks)
		{
			if (string.IsNullOrEmpty(this.Payload))
			{
				return string.Empty;
			}
			if (this.Payload[0] != '[')
			{
				return string.Empty;
			}
			int num = 1;
			while (this.Payload.Length > num && this.Payload[num] != '"' && this.Payload[num] != '\'')
			{
				num++;
			}
			if (this.Payload.Length <= num)
			{
				return string.Empty;
			}
			int num2 = num;
			while (this.Payload.Length > num && this.Payload[num] != ',' && this.Payload[num] != ']')
			{
				num++;
			}
			if (this.Payload.Length <= ++num)
			{
				return string.Empty;
			}
			string text = this.Payload.Remove(num2, num - num2);
			if (removeArrayMarks)
			{
				text = text.Substring(1, text.Length - 2);
			}
			return text;
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0009F532 File Offset: 0x0009D732
		public bool ReconstructAttachmentAsIndex()
		{
			return this.PlaceholderReplacer(delegate(string json, Dictionary<string, object> obj)
			{
				int num = Convert.ToInt32(obj["num"]);
				this.Payload = this.Payload.Replace(json, num.ToString());
				this.IsDecoded = false;
			});
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0009F546 File Offset: 0x0009D746
		public bool ReconstructAttachmentAsBase64()
		{
			return this.HasAllAttachment && this.PlaceholderReplacer(delegate(string json, Dictionary<string, object> obj)
			{
				int index = Convert.ToInt32(obj["num"]);
				this.Payload = this.Payload.Replace(json, string.Format("\"{0}\"", Convert.ToBase64String(this.Attachments[index])));
				this.IsDecoded = false;
			});
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0009F564 File Offset: 0x0009D764
		internal void Parse(string from)
		{
			int num = 0;
			this.TransportEvent = (TransportEventTypes)this.ToInt(from[num++]);
			if (from.Length > num && this.ToInt(from[num]) >= 0)
			{
				this.SocketIOEvent = (SocketIOEventTypes)this.ToInt(from[num++]);
			}
			else
			{
				this.SocketIOEvent = SocketIOEventTypes.Unknown;
			}
			if (this.SocketIOEvent == SocketIOEventTypes.BinaryEvent || this.SocketIOEvent == SocketIOEventTypes.BinaryAck)
			{
				int num2 = from.IndexOf('-', num);
				if (num2 == -1)
				{
					num2 = from.Length;
				}
				int attachmentCount = 0;
				int.TryParse(from.Substring(num, num2 - num), out attachmentCount);
				this.AttachmentCount = attachmentCount;
				num = num2 + 1;
			}
			if (from.Length > num && from[num] == '/')
			{
				int num3 = from.IndexOf(',', num);
				if (num3 == -1)
				{
					num3 = from.Length;
				}
				this.Namespace = from.Substring(num, num3 - num);
				num = num3 + 1;
			}
			else
			{
				this.Namespace = "/";
			}
			if (from.Length > num && this.ToInt(from[num]) >= 0)
			{
				int num4 = num++;
				while (from.Length > num && this.ToInt(from[num]) >= 0)
				{
					num++;
				}
				int id = 0;
				int.TryParse(from.Substring(num4, num - num4), out id);
				this.Id = id;
			}
			if (from.Length > num)
			{
				this.Payload = from.Substring(num);
				return;
			}
			this.Payload = string.Empty;
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x0009F6D4 File Offset: 0x0009D8D4
		private int ToInt(char ch)
		{
			int num = Convert.ToInt32(ch) - 48;
			if (num < 0 || num > 9)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0009F6F8 File Offset: 0x0009D8F8
		internal string Encode()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.TransportEvent == TransportEventTypes.Unknown && this.AttachmentCount > 0)
			{
				this.TransportEvent = TransportEventTypes.Message;
			}
			if (this.TransportEvent != TransportEventTypes.Unknown)
			{
				stringBuilder.Append(((int)this.TransportEvent).ToString());
			}
			if (this.SocketIOEvent == SocketIOEventTypes.Unknown && this.AttachmentCount > 0)
			{
				this.SocketIOEvent = SocketIOEventTypes.BinaryEvent;
			}
			if (this.SocketIOEvent != SocketIOEventTypes.Unknown)
			{
				stringBuilder.Append(((int)this.SocketIOEvent).ToString());
			}
			if (this.SocketIOEvent == SocketIOEventTypes.BinaryEvent || this.SocketIOEvent == SocketIOEventTypes.BinaryAck)
			{
				stringBuilder.Append(this.AttachmentCount.ToString());
				stringBuilder.Append("-");
			}
			bool flag = false;
			if (this.Namespace != "/")
			{
				stringBuilder.Append(this.Namespace);
				flag = true;
			}
			if (this.Id != 0)
			{
				if (flag)
				{
					stringBuilder.Append(",");
					flag = false;
				}
				stringBuilder.Append(this.Id.ToString());
			}
			if (!string.IsNullOrEmpty(this.Payload))
			{
				if (flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(this.Payload);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x0009F830 File Offset: 0x0009DA30
		internal byte[] EncodeBinary()
		{
			if (this.AttachmentCount != 0 || (this.Attachments != null && this.Attachments.Count != 0))
			{
				if (this.Attachments == null)
				{
					throw new ArgumentException("packet.Attachments are null!");
				}
				if (this.AttachmentCount != this.Attachments.Count)
				{
					throw new ArgumentException("packet.AttachmentCount != packet.Attachments.Count. Use the packet.AddAttachment function to add data to a packet!");
				}
			}
			string s = this.Encode();
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			byte[] array = this.EncodeData(bytes, Packet.PayloadTypes.Textual, null);
			if (this.AttachmentCount != 0)
			{
				int num = array.Length;
				List<byte[]> list = new List<byte[]>(this.AttachmentCount);
				int num2 = 0;
				for (int i = 0; i < this.AttachmentCount; i++)
				{
					byte[] array2 = this.EncodeData(this.Attachments[i], Packet.PayloadTypes.Binary, new byte[]
					{
						4
					});
					list.Add(array2);
					num2 += array2.Length;
				}
				Array.Resize<byte>(ref array, array.Length + num2);
				for (int j = 0; j < this.AttachmentCount; j++)
				{
					byte[] array3 = list[j];
					Array.Copy(array3, 0, array, num, array3.Length);
					num += array3.Length;
				}
			}
			return array;
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x0009F954 File Offset: 0x0009DB54
		internal void AddAttachmentFromServer(byte[] data, bool copyFull)
		{
			if (data == null || data.Length == 0)
			{
				return;
			}
			if (this.attachments == null)
			{
				this.attachments = new List<byte[]>(this.AttachmentCount);
			}
			if (copyFull)
			{
				this.Attachments.Add(data);
				return;
			}
			byte[] array = new byte[data.Length - 1];
			Array.Copy(data, 1, array, 0, data.Length - 1);
			this.Attachments.Add(array);
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x0009F9B8 File Offset: 0x0009DBB8
		private byte[] EncodeData(byte[] data, Packet.PayloadTypes type, byte[] afterHeaderData)
		{
			int num = (afterHeaderData != null) ? afterHeaderData.Length : 0;
			string text = (data.Length + num).ToString();
			byte[] array = new byte[text.Length];
			for (int i = 0; i < text.Length; i++)
			{
				array[i] = (byte)char.GetNumericValue(text[i]);
			}
			byte[] array2 = new byte[data.Length + array.Length + 2 + num];
			array2[0] = (byte)type;
			for (int j = 0; j < array.Length; j++)
			{
				array2[1 + j] = array[j];
			}
			int num2 = 1 + array.Length;
			array2[num2++] = byte.MaxValue;
			if (afterHeaderData != null && afterHeaderData.Length != 0)
			{
				Array.Copy(afterHeaderData, 0, array2, num2, afterHeaderData.Length);
				num2 += afterHeaderData.Length;
			}
			Array.Copy(data, 0, array2, num2, data.Length);
			return array2;
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0009FA84 File Offset: 0x0009DC84
		private bool PlaceholderReplacer(Action<string, Dictionary<string, object>> onFound)
		{
			if (string.IsNullOrEmpty(this.Payload))
			{
				return false;
			}
			for (int i = this.Payload.IndexOf("_placeholder"); i >= 0; i = this.Payload.IndexOf("_placeholder"))
			{
				int num = i;
				while (this.Payload[num] != '{')
				{
					num--;
				}
				int num2 = i;
				while (this.Payload.Length > num2 && this.Payload[num2] != '}')
				{
					num2++;
				}
				if (this.Payload.Length <= num2)
				{
					return false;
				}
				string text = this.Payload.Substring(num, num2 - num + 1);
				bool flag = false;
				Dictionary<string, object> dictionary = Json.Decode(text, ref flag) as Dictionary<string, object>;
				if (!flag)
				{
					return false;
				}
				object obj;
				if (!dictionary.TryGetValue("_placeholder", out obj) || !(bool)obj)
				{
					return false;
				}
				if (!dictionary.TryGetValue("num", out obj))
				{
					return false;
				}
				onFound(text, dictionary);
			}
			return true;
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x0009FB7D File Offset: 0x0009DD7D
		public override string ToString()
		{
			return this.Payload;
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0009FB88 File Offset: 0x0009DD88
		internal Packet Clone()
		{
			return new Packet(this.TransportEvent, this.SocketIOEvent, this.Namespace, this.Payload, 0, this.Id)
			{
				EventName = this.EventName,
				AttachmentCount = this.AttachmentCount,
				attachments = this.attachments
			};
		}

		// Token: 0x04001489 RID: 5257
		public const string Placeholder = "_placeholder";

		// Token: 0x04001491 RID: 5265
		private List<byte[]> attachments;

		// Token: 0x020008EB RID: 2283
		private enum PayloadTypes : byte
		{
			// Token: 0x04003483 RID: 13443
			Textual,
			// Token: 0x04003484 RID: 13444
			Binary
		}
	}
}
