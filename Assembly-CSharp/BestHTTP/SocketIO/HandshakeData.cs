using System;
using System.Collections.Generic;
using BestHTTP.JSON;

namespace BestHTTP.SocketIO
{
	// Token: 0x020001C6 RID: 454
	public sealed class HandshakeData
	{
		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x0009EFED File Offset: 0x0009D1ED
		// (set) Token: 0x060010A5 RID: 4261 RVA: 0x0009EFF5 File Offset: 0x0009D1F5
		public string Sid { get; private set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x0009EFFE File Offset: 0x0009D1FE
		// (set) Token: 0x060010A7 RID: 4263 RVA: 0x0009F006 File Offset: 0x0009D206
		public List<string> Upgrades { get; private set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060010A8 RID: 4264 RVA: 0x0009F00F File Offset: 0x0009D20F
		// (set) Token: 0x060010A9 RID: 4265 RVA: 0x0009F017 File Offset: 0x0009D217
		public TimeSpan PingInterval { get; private set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060010AA RID: 4266 RVA: 0x0009F020 File Offset: 0x0009D220
		// (set) Token: 0x060010AB RID: 4267 RVA: 0x0009F028 File Offset: 0x0009D228
		public TimeSpan PingTimeout { get; private set; }

		// Token: 0x060010AC RID: 4268 RVA: 0x0009F034 File Offset: 0x0009D234
		public bool Parse(string str)
		{
			bool flag = false;
			Dictionary<string, object> from = Json.Decode(str, ref flag) as Dictionary<string, object>;
			if (!flag)
			{
				return false;
			}
			try
			{
				this.Sid = HandshakeData.GetString(from, "sid");
				this.Upgrades = HandshakeData.GetStringList(from, "upgrades");
				this.PingInterval = TimeSpan.FromMilliseconds((double)HandshakeData.GetInt(from, "pingInterval"));
				this.PingTimeout = TimeSpan.FromMilliseconds((double)HandshakeData.GetInt(from, "pingTimeout"));
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("HandshakeData", "Parse", ex);
				return false;
			}
			return true;
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x0009F0D8 File Offset: 0x0009D2D8
		private static object Get(Dictionary<string, object> from, string key)
		{
			object result;
			if (!from.TryGetValue(key, out result))
			{
				throw new Exception(string.Format("Can't get {0} from Handshake data!", key));
			}
			return result;
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0009F102 File Offset: 0x0009D302
		private static string GetString(Dictionary<string, object> from, string key)
		{
			return HandshakeData.Get(from, key) as string;
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x0009F110 File Offset: 0x0009D310
		private static List<string> GetStringList(Dictionary<string, object> from, string key)
		{
			List<object> list = HandshakeData.Get(from, key) as List<object>;
			List<string> list2 = new List<string>(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i] as string;
				if (text != null)
				{
					list2.Add(text);
				}
			}
			return list2;
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x0009F15F File Offset: 0x0009D35F
		private static int GetInt(Dictionary<string, object> from, string key)
		{
			return (int)((double)HandshakeData.Get(from, key));
		}
	}
}
