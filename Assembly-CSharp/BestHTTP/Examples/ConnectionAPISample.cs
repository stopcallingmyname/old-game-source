using System;
using BestHTTP.Cookies;
using BestHTTP.JSON;
using BestHTTP.SignalR;
using BestHTTP.SignalR.JsonEncoders;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x0200019E RID: 414
	public sealed class ConnectionAPISample : MonoBehaviour
	{
		// Token: 0x06000F06 RID: 3846 RVA: 0x00098B08 File Offset: 0x00096D08
		private void Start()
		{
			if (PlayerPrefs.HasKey("userName"))
			{
				CookieJar.Set(this.URI, new Cookie("user", PlayerPrefs.GetString("userName")));
			}
			this.signalRConnection = new Connection(this.URI);
			this.signalRConnection.JsonEncoder = new LitJsonEncoder();
			this.signalRConnection.OnStateChanged += this.signalRConnection_OnStateChanged;
			this.signalRConnection.OnNonHubMessage += this.signalRConnection_OnGeneralMessage;
			this.signalRConnection.Open();
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00098B9A File Offset: 0x00096D9A
		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
				GUILayout.Label("To Everybody", Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				this.ToEveryBodyText = GUILayout.TextField(this.ToEveryBodyText, new GUILayoutOption[]
				{
					GUILayout.MinWidth(100f)
				});
				if (GUILayout.Button("Broadcast", Array.Empty<GUILayoutOption>()))
				{
					this.Broadcast(this.ToEveryBodyText);
				}
				if (GUILayout.Button("Broadcast (All Except Me)", Array.Empty<GUILayoutOption>()))
				{
					this.BroadcastExceptMe(this.ToEveryBodyText);
				}
				if (GUILayout.Button("Enter Name", Array.Empty<GUILayoutOption>()))
				{
					this.EnterName(this.ToEveryBodyText);
				}
				if (GUILayout.Button("Join Group", Array.Empty<GUILayoutOption>()))
				{
					this.JoinGroup(this.ToEveryBodyText);
				}
				if (GUILayout.Button("Leave Group", Array.Empty<GUILayoutOption>()))
				{
					this.LeaveGroup(this.ToEveryBodyText);
				}
				GUILayout.EndHorizontal();
				GUILayout.Label("To Me", Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				this.ToMeText = GUILayout.TextField(this.ToMeText, new GUILayoutOption[]
				{
					GUILayout.MinWidth(100f)
				});
				if (GUILayout.Button("Send to me", Array.Empty<GUILayoutOption>()))
				{
					this.SendToMe(this.ToMeText);
				}
				GUILayout.EndHorizontal();
				GUILayout.Label("Private Message", Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Message:", Array.Empty<GUILayoutOption>());
				this.PrivateMessageText = GUILayout.TextField(this.PrivateMessageText, new GUILayoutOption[]
				{
					GUILayout.MinWidth(100f)
				});
				GUILayout.Label("User or Group name:", Array.Empty<GUILayoutOption>());
				this.PrivateMessageUserOrGroupName = GUILayout.TextField(this.PrivateMessageUserOrGroupName, new GUILayoutOption[]
				{
					GUILayout.MinWidth(100f)
				});
				if (GUILayout.Button("Send to user", Array.Empty<GUILayoutOption>()))
				{
					this.SendToUser(this.PrivateMessageUserOrGroupName, this.PrivateMessageText);
				}
				if (GUILayout.Button("Send to group", Array.Empty<GUILayoutOption>()))
				{
					this.SendToGroup(this.PrivateMessageUserOrGroupName, this.PrivateMessageText);
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(20f);
				if (this.signalRConnection.State == ConnectionStates.Closed)
				{
					if (GUILayout.Button("Start Connection", Array.Empty<GUILayoutOption>()))
					{
						this.signalRConnection.Open();
					}
				}
				else if (GUILayout.Button("Stop Connection", Array.Empty<GUILayoutOption>()))
				{
					this.signalRConnection.Close();
				}
				GUILayout.Space(20f);
				GUILayout.Label("Messages", Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Space(20f);
				this.messages.Draw((float)(Screen.width - 20), 0f);
				GUILayout.EndHorizontal();
				GUILayout.EndVertical();
			});
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00098BB3 File Offset: 0x00096DB3
		private void OnDestroy()
		{
			this.signalRConnection.Close();
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x00098BC0 File Offset: 0x00096DC0
		private void signalRConnection_OnGeneralMessage(Connection manager, object data)
		{
			string str = Json.Encode(data);
			this.messages.Add("[Server Message] " + str);
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x00098BEA File Offset: 0x00096DEA
		private void signalRConnection_OnStateChanged(Connection manager, ConnectionStates oldState, ConnectionStates newState)
		{
			this.messages.Add(string.Format("[State Change] {0} => {1}", oldState.ToString(), newState.ToString()));
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x00098C1B File Offset: 0x00096E1B
		private void Broadcast(string text)
		{
			this.signalRConnection.Send(new
			{
				Type = ConnectionAPISample.MessageTypes.Broadcast,
				Value = text
			});
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x00098C30 File Offset: 0x00096E30
		private void BroadcastExceptMe(string text)
		{
			this.signalRConnection.Send(new
			{
				Type = ConnectionAPISample.MessageTypes.BroadcastExceptMe,
				Value = text
			});
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x00098C45 File Offset: 0x00096E45
		private void EnterName(string name)
		{
			this.signalRConnection.Send(new
			{
				Type = ConnectionAPISample.MessageTypes.Join,
				Value = name
			});
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x00098C5A File Offset: 0x00096E5A
		private void JoinGroup(string groupName)
		{
			this.signalRConnection.Send(new
			{
				Type = ConnectionAPISample.MessageTypes.AddToGroup,
				Value = groupName
			});
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x00098C6F File Offset: 0x00096E6F
		private void LeaveGroup(string groupName)
		{
			this.signalRConnection.Send(new
			{
				Type = ConnectionAPISample.MessageTypes.RemoveFromGroup,
				Value = groupName
			});
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x00098C84 File Offset: 0x00096E84
		private void SendToMe(string text)
		{
			this.signalRConnection.Send(new
			{
				Type = ConnectionAPISample.MessageTypes.Send,
				Value = text
			});
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00098C99 File Offset: 0x00096E99
		private void SendToUser(string userOrGroupName, string text)
		{
			this.signalRConnection.Send(new
			{
				Type = ConnectionAPISample.MessageTypes.PrivateMessage,
				Value = string.Format("{0}|{1}", userOrGroupName, text)
			});
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00098CB9 File Offset: 0x00096EB9
		private void SendToGroup(string userOrGroupName, string text)
		{
			this.signalRConnection.Send(new
			{
				Type = ConnectionAPISample.MessageTypes.SendToGroup,
				Value = string.Format("{0}|{1}", userOrGroupName, text)
			});
		}

		// Token: 0x0400139C RID: 5020
		private readonly Uri URI = new Uri(GUIHelper.BaseURL + "/raw-connection/");

		// Token: 0x0400139D RID: 5021
		private Connection signalRConnection;

		// Token: 0x0400139E RID: 5022
		private string ToEveryBodyText = string.Empty;

		// Token: 0x0400139F RID: 5023
		private string ToMeText = string.Empty;

		// Token: 0x040013A0 RID: 5024
		private string PrivateMessageText = string.Empty;

		// Token: 0x040013A1 RID: 5025
		private string PrivateMessageUserOrGroupName = string.Empty;

		// Token: 0x040013A2 RID: 5026
		private GUIMessageList messages = new GUIMessageList();

		// Token: 0x020008DF RID: 2271
		private enum MessageTypes
		{
			// Token: 0x04003453 RID: 13395
			Send,
			// Token: 0x04003454 RID: 13396
			Broadcast,
			// Token: 0x04003455 RID: 13397
			Join,
			// Token: 0x04003456 RID: 13398
			PrivateMessage,
			// Token: 0x04003457 RID: 13399
			AddToGroup,
			// Token: 0x04003458 RID: 13400
			RemoveFromGroup,
			// Token: 0x04003459 RID: 13401
			SendToGroup,
			// Token: 0x0400345A RID: 13402
			BroadcastExceptMe
		}
	}
}
