using System;
using BestHTTP.WebSocket;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x020001AE RID: 430
	public class WebSocketSample : MonoBehaviour
	{
		// Token: 0x06000FF9 RID: 4089 RVA: 0x0009C6DD File Offset: 0x0009A8DD
		private void OnDestroy()
		{
			if (this.webSocket != null)
			{
				this.webSocket.Close();
			}
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0009C6F2 File Offset: 0x0009A8F2
		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, Array.Empty<GUILayoutOption>());
				GUILayout.Label(this.Text, Array.Empty<GUILayoutOption>());
				GUILayout.EndScrollView();
				GUILayout.Space(5f);
				GUILayout.FlexibleSpace();
				this.address = GUILayout.TextField(this.address, Array.Empty<GUILayoutOption>());
				if (this.webSocket == null && GUILayout.Button("Open Web Socket", Array.Empty<GUILayoutOption>()))
				{
					this.webSocket = new WebSocket(new Uri(this.address));
					this.webSocket.StartPingThread = true;
					if (HTTPManager.Proxy != null)
					{
						this.webSocket.InternalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false);
					}
					WebSocket webSocket = this.webSocket;
					webSocket.OnOpen = (OnWebSocketOpenDelegate)Delegate.Combine(webSocket.OnOpen, new OnWebSocketOpenDelegate(this.OnOpen));
					WebSocket webSocket2 = this.webSocket;
					webSocket2.OnMessage = (OnWebSocketMessageDelegate)Delegate.Combine(webSocket2.OnMessage, new OnWebSocketMessageDelegate(this.OnMessageReceived));
					WebSocket webSocket3 = this.webSocket;
					webSocket3.OnClosed = (OnWebSocketClosedDelegate)Delegate.Combine(webSocket3.OnClosed, new OnWebSocketClosedDelegate(this.OnClosed));
					WebSocket webSocket4 = this.webSocket;
					webSocket4.OnError = (OnWebSocketErrorDelegate)Delegate.Combine(webSocket4.OnError, new OnWebSocketErrorDelegate(this.OnError));
					this.webSocket.Open();
					this.Text += "Opening Web Socket...\n";
				}
				if (this.webSocket != null && this.webSocket.IsOpen)
				{
					GUILayout.Space(10f);
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					this.msgToSend = GUILayout.TextField(this.msgToSend, Array.Empty<GUILayoutOption>());
					GUILayout.EndHorizontal();
					if (GUILayout.Button("Send", new GUILayoutOption[]
					{
						GUILayout.MaxWidth(70f)
					}))
					{
						this.Text += "Sending message...\n";
						this.webSocket.Send(this.msgToSend);
					}
					GUILayout.Space(10f);
					if (GUILayout.Button("Close", Array.Empty<GUILayoutOption>()))
					{
						this.webSocket.Close(1000, "Bye!");
					}
				}
			});
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0009C70B File Offset: 0x0009A90B
		private void OnOpen(WebSocket ws)
		{
			this.Text += string.Format("-WebSocket Open!\n", Array.Empty<object>());
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0009C72D File Offset: 0x0009A92D
		private void OnMessageReceived(WebSocket ws, string message)
		{
			this.Text += string.Format("-Message received: {0}\n", message);
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0009C74B File Offset: 0x0009A94B
		private void OnClosed(WebSocket ws, ushort code, string message)
		{
			this.Text += string.Format("-WebSocket closed! Code: {0} Message: {1}\n", code, message);
			this.webSocket = null;
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0009C778 File Offset: 0x0009A978
		private void OnError(WebSocket ws, Exception ex)
		{
			string str = string.Empty;
			if (ws.InternalRequest.Response != null)
			{
				str = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
			}
			this.Text += string.Format("-An error occured: {0}\n", (ex != null) ? ex.Message : ("Unknown Error " + str));
			this.webSocket = null;
		}

		// Token: 0x040013F9 RID: 5113
		private string address = "wss://echo.websocket.org";

		// Token: 0x040013FA RID: 5114
		private string msgToSend = "Hello World!";

		// Token: 0x040013FB RID: 5115
		private string Text = string.Empty;

		// Token: 0x040013FC RID: 5116
		private WebSocket webSocket;

		// Token: 0x040013FD RID: 5117
		private Vector2 scrollPos;
	}
}
