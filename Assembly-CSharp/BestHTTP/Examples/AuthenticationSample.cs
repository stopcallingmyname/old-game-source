using System;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Authentication;
using BestHTTP.SignalR.Hubs;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x0200019C RID: 412
	public class AuthenticationSample : MonoBehaviour
	{
		// Token: 0x06000EF6 RID: 3830 RVA: 0x0009860C File Offset: 0x0009680C
		private void Start()
		{
			this.signalRConnection = new Connection(this.URI, new Hub[]
			{
				new BaseHub("noauthhub", "Messages"),
				new BaseHub("invokeauthhub", "Messages Invoked By Admin or Invoker"),
				new BaseHub("authhub", "Messages Requiring Authentication to Send or Receive"),
				new BaseHub("inheritauthhub", "Messages Requiring Authentication to Send or Receive Because of Inheritance"),
				new BaseHub("incomingauthhub", "Messages Requiring Authentication to Send"),
				new BaseHub("adminauthhub", "Messages Requiring Admin Membership to Send or Receive"),
				new BaseHub("userandroleauthhub", "Messages Requiring Name to be \"User\" and Role to be \"Admin\" to Send or Receive")
			});
			if (!string.IsNullOrEmpty(this.userName) && !string.IsNullOrEmpty(this.role))
			{
				this.signalRConnection.AuthenticationProvider = new HeaderAuthenticator(this.userName, this.role);
			}
			this.signalRConnection.OnConnected += this.signalRConnection_OnConnected;
			this.signalRConnection.Open();
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00098706 File Offset: 0x00096906
		private void OnDestroy()
		{
			this.signalRConnection.Close();
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00098713 File Offset: 0x00096913
		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, Array.Empty<GUILayoutOption>());
				GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
				if (this.signalRConnection.AuthenticationProvider == null)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Username (Enter 'User'):", Array.Empty<GUILayoutOption>());
					this.userName = GUILayout.TextField(this.userName, new GUILayoutOption[]
					{
						GUILayout.MinWidth(100f)
					});
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Roles (Enter 'Invoker' or 'Admin'):", Array.Empty<GUILayoutOption>());
					this.role = GUILayout.TextField(this.role, new GUILayoutOption[]
					{
						GUILayout.MinWidth(100f)
					});
					GUILayout.EndHorizontal();
					if (GUILayout.Button("Log in", Array.Empty<GUILayoutOption>()))
					{
						this.Restart();
					}
				}
				for (int i = 0; i < this.signalRConnection.Hubs.Length; i++)
				{
					(this.signalRConnection.Hubs[i] as BaseHub).Draw();
				}
				GUILayout.EndVertical();
				GUILayout.EndScrollView();
			});
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0009872C File Offset: 0x0009692C
		private void signalRConnection_OnConnected(Connection manager)
		{
			for (int i = 0; i < this.signalRConnection.Hubs.Length; i++)
			{
				(this.signalRConnection.Hubs[i] as BaseHub).InvokedFromClient();
			}
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00098768 File Offset: 0x00096968
		private void Restart()
		{
			this.signalRConnection.OnConnected -= this.signalRConnection_OnConnected;
			this.signalRConnection.Close();
			this.signalRConnection = null;
			this.Start();
		}

		// Token: 0x04001395 RID: 5013
		private readonly Uri URI = new Uri(GUIHelper.BaseURL + "/signalr");

		// Token: 0x04001396 RID: 5014
		private Connection signalRConnection;

		// Token: 0x04001397 RID: 5015
		private string userName = string.Empty;

		// Token: 0x04001398 RID: 5016
		private string role = string.Empty;

		// Token: 0x04001399 RID: 5017
		private Vector2 scrollPos;
	}
}
