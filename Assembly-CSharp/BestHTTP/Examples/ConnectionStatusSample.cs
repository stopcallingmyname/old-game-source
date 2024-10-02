using System;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x0200019F RID: 415
	public sealed class ConnectionStatusSample : MonoBehaviour
	{
		// Token: 0x06000F15 RID: 3861 RVA: 0x00098FF8 File Offset: 0x000971F8
		private void Start()
		{
			this.signalRConnection = new Connection(this.URI, new string[]
			{
				"StatusHub"
			});
			this.signalRConnection.OnNonHubMessage += this.signalRConnection_OnNonHubMessage;
			this.signalRConnection.OnError += this.signalRConnection_OnError;
			this.signalRConnection.OnStateChanged += this.signalRConnection_OnStateChanged;
			this.signalRConnection["StatusHub"].OnMethodCall += this.statusHub_OnMethodCall;
			this.signalRConnection.Open();
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x00099095 File Offset: 0x00097295
		private void OnDestroy()
		{
			this.signalRConnection.Close();
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x000990A2 File Offset: 0x000972A2
		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("START", Array.Empty<GUILayoutOption>()) && this.signalRConnection.State != ConnectionStates.Connected)
				{
					this.signalRConnection.Open();
				}
				if (GUILayout.Button("STOP", Array.Empty<GUILayoutOption>()) && this.signalRConnection.State == ConnectionStates.Connected)
				{
					this.signalRConnection.Close();
					this.messages.Clear();
				}
				if (GUILayout.Button("PING", Array.Empty<GUILayoutOption>()) && this.signalRConnection.State == ConnectionStates.Connected)
				{
					this.signalRConnection["StatusHub"].Call("Ping", Array.Empty<object>());
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(20f);
				GUILayout.Label("Connection Status Messages", Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Space(20f);
				this.messages.Draw((float)(Screen.width - 20), 0f);
				GUILayout.EndHorizontal();
			});
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x000990BB File Offset: 0x000972BB
		private void signalRConnection_OnNonHubMessage(Connection manager, object data)
		{
			this.messages.Add("[Server Message] " + data.ToString());
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x000990D8 File Offset: 0x000972D8
		private void signalRConnection_OnStateChanged(Connection manager, ConnectionStates oldState, ConnectionStates newState)
		{
			this.messages.Add(string.Format("[State Change] {0} => {1}", oldState, newState));
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x000990FB File Offset: 0x000972FB
		private void signalRConnection_OnError(Connection manager, string error)
		{
			this.messages.Add("[Error] " + error);
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x00099114 File Offset: 0x00097314
		private void statusHub_OnMethodCall(Hub hub, string method, params object[] args)
		{
			string arg = (args.Length != 0) ? (args[0] as string) : string.Empty;
			string arg2 = (args.Length > 1) ? args[1].ToString() : string.Empty;
			if (method == "joined")
			{
				this.messages.Add(string.Format("[{0}] {1} joined at {2}", hub.Name, arg, arg2));
				return;
			}
			if (method == "rejoined")
			{
				this.messages.Add(string.Format("[{0}] {1} reconnected at {2}", hub.Name, arg, arg2));
				return;
			}
			if (!(method == "leave"))
			{
				this.messages.Add(string.Format("[{0}] {1}", hub.Name, method));
				return;
			}
			this.messages.Add(string.Format("[{0}] {1} leaved at {2}", hub.Name, arg, arg2));
		}

		// Token: 0x040013A3 RID: 5027
		private readonly Uri URI = new Uri(GUIHelper.BaseURL + "/signalr");

		// Token: 0x040013A4 RID: 5028
		private Connection signalRConnection;

		// Token: 0x040013A5 RID: 5029
		private GUIMessageList messages = new GUIMessageList();
	}
}
