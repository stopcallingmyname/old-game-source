using System;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using BestHTTP.SignalRCore.Messages;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x020001A4 RID: 420
	public sealed class HubWithAuthorizationSample : MonoBehaviour
	{
		// Token: 0x06000F5A RID: 3930 RVA: 0x0009A2F4 File Offset: 0x000984F4
		private void Start()
		{
			this.hub = new HubConnection(this.URI, new JsonProtocol(new LitJsonEncoder()));
			this.hub.OnConnected += this.Hub_OnConnected;
			this.hub.OnError += this.Hub_OnError;
			this.hub.OnClosed += this.Hub_OnClosed;
			this.hub.OnMessage += this.Hub_OnMessage;
			this.hub.StartConnect();
			this.uiText = "StartConnect called\n";
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0009A38E File Offset: 0x0009858E
		private void OnDestroy()
		{
			if (this.hub != null)
			{
				this.hub.StartClose();
			}
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0009A3A3 File Offset: 0x000985A3
		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, Array.Empty<GUILayoutOption>());
				GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
				GUILayout.Label(this.uiText, Array.Empty<GUILayoutOption>());
				GUILayout.EndVertical();
				GUILayout.EndScrollView();
			});
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0009A3BC File Offset: 0x000985BC
		private void Hub_OnConnected(HubConnection hub)
		{
			this.uiText += "Hub Connected\n";
			hub.Invoke<string>("Echo", new object[]
			{
				"Message from the client"
			}).OnSuccess(delegate(string ret)
			{
				this.uiText += string.Format(" 'Echo' returned: '{0}'\n", ret);
			});
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0006AE98 File Offset: 0x00069098
		private bool Hub_OnMessage(HubConnection hub, Message message)
		{
			return true;
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x0009A40A File Offset: 0x0009860A
		private void Hub_OnClosed(HubConnection hub)
		{
			this.uiText += "Hub Closed\n";
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0009A422 File Offset: 0x00098622
		private void Hub_OnError(HubConnection hub, string error)
		{
			this.uiText = this.uiText + "Hub Error: " + error + "\n";
		}

		// Token: 0x040013C5 RID: 5061
		private readonly Uri URI = new Uri(GUIHelper.BaseURL + "/redirect");

		// Token: 0x040013C6 RID: 5062
		private HubConnection hub;

		// Token: 0x040013C7 RID: 5063
		private Vector2 scrollPos;

		// Token: 0x040013C8 RID: 5064
		private string uiText;
	}
}
