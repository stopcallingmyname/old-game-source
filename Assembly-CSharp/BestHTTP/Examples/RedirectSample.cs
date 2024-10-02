using System;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using BestHTTP.SignalRCore.Messages;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x020001A7 RID: 423
	public sealed class RedirectSample : MonoBehaviour
	{
		// Token: 0x06000F7D RID: 3965 RVA: 0x0009AA0C File Offset: 0x00098C0C
		private void Start()
		{
			this.hub = new HubConnection(this.URI, new JsonProtocol(new LitJsonEncoder()));
			this.hub.AuthenticationProvider = new RedirectLoggerAccessTokenAuthenticator(this.hub);
			this.hub.OnConnected += this.Hub_OnConnected;
			this.hub.OnError += this.Hub_OnError;
			this.hub.OnClosed += this.Hub_OnClosed;
			this.hub.OnMessage += this.Hub_OnMessage;
			this.hub.OnRedirected += this.Hub_Redirected;
			this.hub.StartConnect();
			this.uiText = "StartConnect called\n";
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0009AAD3 File Offset: 0x00098CD3
		private void OnDestroy()
		{
			if (this.hub != null)
			{
				this.hub.StartClose();
			}
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0009AAE8 File Offset: 0x00098CE8
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

		// Token: 0x06000F80 RID: 3968 RVA: 0x0009AB01 File Offset: 0x00098D01
		private void Hub_Redirected(HubConnection hub, Uri oldUri, Uri newUri)
		{
			this.uiText += string.Format("Hub connection redirected to '<color=green>{0}</color>'!\n", hub.Uri);
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0009AB24 File Offset: 0x00098D24
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

		// Token: 0x06000F82 RID: 3970 RVA: 0x0006AE98 File Offset: 0x00069098
		private bool Hub_OnMessage(HubConnection hub, Message message)
		{
			return true;
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0009AB72 File Offset: 0x00098D72
		private void Hub_OnClosed(HubConnection hub)
		{
			this.uiText += "Hub Closed\n";
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0009AB8A File Offset: 0x00098D8A
		private void Hub_OnError(HubConnection hub, string error)
		{
			this.uiText = this.uiText + "Hub Error: " + error + "\n";
		}

		// Token: 0x040013D2 RID: 5074
		private readonly Uri URI = new Uri(GUIHelper.BaseURL + "/redirect_sample");

		// Token: 0x040013D3 RID: 5075
		public HubConnection hub;

		// Token: 0x040013D4 RID: 5076
		private Vector2 scrollPos;

		// Token: 0x040013D5 RID: 5077
		public string uiText;
	}
}
