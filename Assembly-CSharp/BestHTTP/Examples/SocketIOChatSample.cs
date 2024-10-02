using System;
using System.Collections.Generic;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Events;
using BestHTTP.SocketIO.Transports;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x020001AC RID: 428
	public sealed class SocketIOChatSample : MonoBehaviour
	{
		// Token: 0x06000FCC RID: 4044 RVA: 0x0009B8E8 File Offset: 0x00099AE8
		private void Start()
		{
			this.State = SocketIOChatSample.ChatStates.Login;
			SocketOptions socketOptions = new SocketOptions();
			socketOptions.AutoConnect = false;
			socketOptions.ConnectWith = TransportTypes.WebSocket;
			this.Manager = new SocketManager(new Uri("https://socket-io-chat.now.sh/socket.io/"), socketOptions);
			this.Manager.Socket.On("login", new SocketIOCallback(this.OnLogin));
			this.Manager.Socket.On("new message", new SocketIOCallback(this.OnNewMessage));
			this.Manager.Socket.On("user joined", new SocketIOCallback(this.OnUserJoined));
			this.Manager.Socket.On("user left", new SocketIOCallback(this.OnUserLeft));
			this.Manager.Socket.On("typing", new SocketIOCallback(this.OnTyping));
			this.Manager.Socket.On("stop typing", new SocketIOCallback(this.OnStopTyping));
			this.Manager.Socket.On(SocketIOEventTypes.Error, delegate(Socket socket, Packet packet, object[] args)
			{
				Debug.LogError(string.Format("Error: {0}", args[0].ToString()));
			});
			this.Manager.Open();
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0009BA27 File Offset: 0x00099C27
		private void OnDestroy()
		{
			this.Manager.Close();
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0009BA34 File Offset: 0x00099C34
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				SampleSelector.SelectedSample.DestroyUnityObject();
			}
			if (this.typing && DateTime.UtcNow - this.lastTypingTime >= this.TYPING_TIMER_LENGTH)
			{
				this.Manager.Socket.Emit("stop typing", Array.Empty<object>());
				this.typing = false;
			}
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x0009BA9C File Offset: 0x00099C9C
		private void OnGUI()
		{
			SocketIOChatSample.ChatStates state = this.State;
			if (state == SocketIOChatSample.ChatStates.Login)
			{
				this.DrawLoginScreen();
				return;
			}
			if (state != SocketIOChatSample.ChatStates.Chat)
			{
				return;
			}
			this.DrawChatScreen();
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0009BAC5 File Offset: 0x00099CC5
		private void DrawLoginScreen()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
				GUILayout.FlexibleSpace();
				GUIHelper.DrawCenteredText("What's your nickname?");
				this.userName = GUILayout.TextField(this.userName, Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Join", Array.Empty<GUILayoutOption>()))
				{
					this.SetUserName();
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();
			});
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0009BADE File Offset: 0x00099CDE
		private void DrawChatScreen()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
				this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, Array.Empty<GUILayoutOption>());
				GUILayout.Label(this.chatLog, new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(true),
					GUILayout.ExpandHeight(true)
				});
				GUILayout.EndScrollView();
				string text = string.Empty;
				if (this.typingUsers.Count > 0)
				{
					text += string.Format("{0}", this.typingUsers[0]);
					for (int i = 1; i < this.typingUsers.Count; i++)
					{
						text += string.Format(", {0}", this.typingUsers[i]);
					}
					if (this.typingUsers.Count == 1)
					{
						text += " is typing!";
					}
					else
					{
						text += " are typing!";
					}
				}
				GUILayout.Label(text, Array.Empty<GUILayoutOption>());
				GUILayout.Label("Type here:", Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				this.message = GUILayout.TextField(this.message, Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Send", new GUILayoutOption[]
				{
					GUILayout.MaxWidth(100f)
				}))
				{
					this.SendMessage();
				}
				GUILayout.EndHorizontal();
				if (GUI.changed)
				{
					this.UpdateTyping();
				}
				GUILayout.EndVertical();
			});
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0009BAF7 File Offset: 0x00099CF7
		private void SetUserName()
		{
			if (string.IsNullOrEmpty(this.userName))
			{
				return;
			}
			this.State = SocketIOChatSample.ChatStates.Chat;
			this.Manager.Socket.Emit("add user", new object[]
			{
				this.userName
			});
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0009BB34 File Offset: 0x00099D34
		private void SendMessage()
		{
			if (string.IsNullOrEmpty(this.message))
			{
				return;
			}
			this.Manager.Socket.Emit("new message", new object[]
			{
				this.message
			});
			this.chatLog += string.Format("{0}: {1}\n", this.userName, this.message);
			this.message = string.Empty;
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0009BBA6 File Offset: 0x00099DA6
		private void UpdateTyping()
		{
			if (!this.typing)
			{
				this.typing = true;
				this.Manager.Socket.Emit("typing", Array.Empty<object>());
			}
			this.lastTypingTime = DateTime.UtcNow;
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0009BBE0 File Offset: 0x00099DE0
		private void addParticipantsMessage(Dictionary<string, object> data)
		{
			int num = Convert.ToInt32(data["numUsers"]);
			if (num == 1)
			{
				this.chatLog += "there's 1 participant\n";
				return;
			}
			this.chatLog = string.Concat(new object[]
			{
				this.chatLog,
				"there are ",
				num,
				" participants\n"
			});
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0009BC4C File Offset: 0x00099E4C
		private void addChatMessage(Dictionary<string, object> data)
		{
			string arg = data["username"] as string;
			string arg2 = data["message"] as string;
			this.chatLog += string.Format("{0}: {1}\n", arg, arg2);
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0009BC98 File Offset: 0x00099E98
		private void AddChatTyping(Dictionary<string, object> data)
		{
			string item = data["username"] as string;
			this.typingUsers.Add(item);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0009BCC4 File Offset: 0x00099EC4
		private void RemoveChatTyping(Dictionary<string, object> data)
		{
			string username = data["username"] as string;
			int num = this.typingUsers.FindIndex((string name) => name.Equals(username));
			if (num != -1)
			{
				this.typingUsers.RemoveAt(num);
			}
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0009BD15 File Offset: 0x00099F15
		private void OnLogin(Socket socket, Packet packet, params object[] args)
		{
			this.chatLog = "Welcome to Socket.IO Chat — \n";
			this.addParticipantsMessage(args[0] as Dictionary<string, object>);
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0009BD30 File Offset: 0x00099F30
		private void OnNewMessage(Socket socket, Packet packet, params object[] args)
		{
			this.addChatMessage(args[0] as Dictionary<string, object>);
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0009BD40 File Offset: 0x00099F40
		private void OnUserJoined(Socket socket, Packet packet, params object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			string arg = dictionary["username"] as string;
			this.chatLog += string.Format("{0} joined\n", arg);
			this.addParticipantsMessage(dictionary);
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0009BD8C File Offset: 0x00099F8C
		private void OnUserLeft(Socket socket, Packet packet, params object[] args)
		{
			Dictionary<string, object> dictionary = args[0] as Dictionary<string, object>;
			string arg = dictionary["username"] as string;
			this.chatLog += string.Format("{0} left\n", arg);
			this.addParticipantsMessage(dictionary);
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0009BDD6 File Offset: 0x00099FD6
		private void OnTyping(Socket socket, Packet packet, params object[] args)
		{
			this.AddChatTyping(args[0] as Dictionary<string, object>);
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0009BDE6 File Offset: 0x00099FE6
		private void OnStopTyping(Socket socket, Packet packet, params object[] args)
		{
			this.RemoveChatTyping(args[0] as Dictionary<string, object>);
		}

		// Token: 0x040013E4 RID: 5092
		private readonly TimeSpan TYPING_TIMER_LENGTH = TimeSpan.FromMilliseconds(700.0);

		// Token: 0x040013E5 RID: 5093
		private SocketManager Manager;

		// Token: 0x040013E6 RID: 5094
		private SocketIOChatSample.ChatStates State;

		// Token: 0x040013E7 RID: 5095
		private string userName = string.Empty;

		// Token: 0x040013E8 RID: 5096
		private string message = string.Empty;

		// Token: 0x040013E9 RID: 5097
		private string chatLog = string.Empty;

		// Token: 0x040013EA RID: 5098
		private Vector2 scrollPos;

		// Token: 0x040013EB RID: 5099
		private bool typing;

		// Token: 0x040013EC RID: 5100
		private DateTime lastTypingTime = DateTime.MinValue;

		// Token: 0x040013ED RID: 5101
		private List<string> typingUsers = new List<string>();

		// Token: 0x020008E6 RID: 2278
		private enum ChatStates
		{
			// Token: 0x04003477 RID: 13431
			Login,
			// Token: 0x04003478 RID: 13432
			Chat
		}
	}
}
