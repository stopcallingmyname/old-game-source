using System;
using BestHTTP.SignalR;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x020001A3 RID: 419
	public sealed class SimpleStreamingSample : MonoBehaviour
	{
		// Token: 0x06000F52 RID: 3922 RVA: 0x0009A188 File Offset: 0x00098388
		private void Start()
		{
			this.signalRConnection = new Connection(this.URI);
			this.signalRConnection.OnNonHubMessage += this.signalRConnection_OnNonHubMessage;
			this.signalRConnection.OnStateChanged += this.signalRConnection_OnStateChanged;
			this.signalRConnection.OnError += this.signalRConnection_OnError;
			this.signalRConnection.Open();
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0009A1F6 File Offset: 0x000983F6
		private void OnDestroy()
		{
			this.signalRConnection.Close();
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0009A203 File Offset: 0x00098403
		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				GUILayout.Label("Messages", Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Space(20f);
				this.messages.Draw((float)(Screen.width - 20), 0f);
				GUILayout.EndHorizontal();
			});
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0009A21C File Offset: 0x0009841C
		private void signalRConnection_OnNonHubMessage(Connection connection, object data)
		{
			this.messages.Add("[Server Message] " + data.ToString());
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0009A239 File Offset: 0x00098439
		private void signalRConnection_OnStateChanged(Connection connection, ConnectionStates oldState, ConnectionStates newState)
		{
			this.messages.Add(string.Format("[State Change] {0} => {1}", oldState, newState));
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0009A25C File Offset: 0x0009845C
		private void signalRConnection_OnError(Connection connection, string error)
		{
			this.messages.Add("[Error] " + error);
		}

		// Token: 0x040013C2 RID: 5058
		private readonly Uri URI = new Uri(GUIHelper.BaseURL + "/streaming-connection");

		// Token: 0x040013C3 RID: 5059
		private Connection signalRConnection;

		// Token: 0x040013C4 RID: 5060
		private GUIMessageList messages = new GUIMessageList();
	}
}
