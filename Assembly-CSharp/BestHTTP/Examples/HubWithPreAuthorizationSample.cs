using System;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using BestHTTP.SignalRCore.Messages;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x020001A5 RID: 421
	public sealed class HubWithPreAuthorizationSample : MonoBehaviour
	{
		// Token: 0x06000F64 RID: 3940 RVA: 0x0009A4C0 File Offset: 0x000986C0
		private void Start()
		{
			this.hub = new HubConnection(this.URI, new JsonProtocol(new LitJsonEncoder()));
			this.hub.AuthenticationProvider = new PreAuthAccessTokenAuthenticator(this.AuthURI);
			this.hub.AuthenticationProvider.OnAuthenticationSucceded += this.AuthenticationProvider_OnAuthenticationSucceded;
			this.hub.AuthenticationProvider.OnAuthenticationFailed += this.AuthenticationProvider_OnAuthenticationFailed;
			this.hub.OnConnected += this.Hub_OnConnected;
			this.hub.OnError += this.Hub_OnError;
			this.hub.OnClosed += this.Hub_OnClosed;
			this.hub.OnMessage += this.Hub_OnMessage;
			this.hub.StartConnect();
			this.uiText = "StartConnect called\n";
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0009A5A8 File Offset: 0x000987A8
		private void AuthenticationProvider_OnAuthenticationSucceded(IAuthenticationProvider provider)
		{
			string text = string.Format("Pre-Authentication Succeded! Token: '{0}' \n", (this.hub.AuthenticationProvider as PreAuthAccessTokenAuthenticator).Token);
			Debug.Log(text);
			this.uiText += text;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0009A5ED File Offset: 0x000987ED
		private void AuthenticationProvider_OnAuthenticationFailed(IAuthenticationProvider provider, string reason)
		{
			this.uiText += string.Format("Authentication Failed! Reason: '{0}'\n", reason);
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0009A60B File Offset: 0x0009880B
		private void OnDestroy()
		{
			if (this.hub != null)
			{
				this.hub.StartClose();
			}
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0009A620 File Offset: 0x00098820
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

		// Token: 0x06000F69 RID: 3945 RVA: 0x0009A63C File Offset: 0x0009883C
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

		// Token: 0x06000F6A RID: 3946 RVA: 0x0006AE98 File Offset: 0x00069098
		private bool Hub_OnMessage(HubConnection hub, Message message)
		{
			return true;
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0009A68A File Offset: 0x0009888A
		private void Hub_OnClosed(HubConnection hub)
		{
			this.uiText += "Hub Closed\n";
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0009A6A2 File Offset: 0x000988A2
		private void Hub_OnError(HubConnection hub, string error)
		{
			this.uiText = this.uiText + "Hub Error: " + error + "\n";
		}

		// Token: 0x040013C9 RID: 5065
		private readonly Uri URI = new Uri(GUIHelper.BaseURL + "/HubWithAuthorization");

		// Token: 0x040013CA RID: 5066
		private readonly Uri AuthURI = new Uri(GUIHelper.BaseURL + "/generateJwtToken");

		// Token: 0x040013CB RID: 5067
		private HubConnection hub;

		// Token: 0x040013CC RID: 5068
		private Vector2 scrollPos;

		// Token: 0x040013CD RID: 5069
		private string uiText;
	}
}
