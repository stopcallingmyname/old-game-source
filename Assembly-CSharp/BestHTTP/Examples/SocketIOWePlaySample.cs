using System;
using System.Collections.Generic;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Events;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x020001AD RID: 429
	public sealed class SocketIOWePlaySample : MonoBehaviour
	{
		// Token: 0x06000FE2 RID: 4066 RVA: 0x0009C010 File Offset: 0x0009A210
		private void Start()
		{
			SocketOptions socketOptions = new SocketOptions();
			socketOptions.AutoConnect = false;
			SocketManager socketManager = new SocketManager(new Uri("http://io.weplay.io/socket.io/"), socketOptions);
			this.Socket = socketManager.Socket;
			this.Socket.On(SocketIOEventTypes.Connect, new SocketIOCallback(this.OnConnected));
			this.Socket.On("joined", new SocketIOCallback(this.OnJoined));
			this.Socket.On("connections", new SocketIOCallback(this.OnConnections));
			this.Socket.On("join", new SocketIOCallback(this.OnJoin));
			this.Socket.On("move", new SocketIOCallback(this.OnMove));
			this.Socket.On("message", new SocketIOCallback(this.OnMessage));
			this.Socket.On("reload", new SocketIOCallback(this.OnReload));
			this.Socket.On("frame", new SocketIOCallback(this.OnFrame), false);
			this.Socket.On(SocketIOEventTypes.Error, new SocketIOCallback(this.OnError));
			socketManager.Open();
			this.State = SocketIOWePlaySample.States.Connecting;
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0009C149 File Offset: 0x0009A349
		private void OnDestroy()
		{
			this.Socket.Manager.Close();
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0009C15B File Offset: 0x0009A35B
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				SampleSelector.SelectedSample.DestroyUnityObject();
			}
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0009C170 File Offset: 0x0009A370
		private void OnGUI()
		{
			switch (this.State)
			{
			case SocketIOWePlaySample.States.Connecting:
				GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
				{
					GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
					GUILayout.FlexibleSpace();
					GUIHelper.DrawCenteredText("Connecting to the server...");
					GUILayout.FlexibleSpace();
					GUILayout.EndVertical();
				});
				return;
			case SocketIOWePlaySample.States.WaitForNick:
				GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
				{
					this.DrawLoginScreen();
				});
				return;
			case SocketIOWePlaySample.States.Joined:
				GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
				{
					if (this.FrameTexture != null)
					{
						GUILayout.Box(this.FrameTexture, Array.Empty<GUILayoutOption>());
					}
					this.DrawControls();
					this.DrawChat(true);
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0009C1F4 File Offset: 0x0009A3F4
		private void DrawLoginScreen()
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			GUIHelper.DrawCenteredText("What's your nickname?");
			this.Nick = GUILayout.TextField(this.Nick, Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Join", Array.Empty<GUILayoutOption>()))
			{
				this.Join();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0009C254 File Offset: 0x0009A454
		private void DrawControls()
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Controls:", Array.Empty<GUILayoutOption>());
			for (int i = 0; i < this.controls.Length; i++)
			{
				if (GUILayout.Button(this.controls[i], Array.Empty<GUILayoutOption>()))
				{
					this.Socket.Emit("move", new object[]
					{
						this.controls[i]
					});
				}
			}
			GUILayout.Label(" Connections: " + this.connections, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0009C2E8 File Offset: 0x0009A4E8
		private void DrawChat(bool withInput = true)
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, Array.Empty<GUILayoutOption>());
			for (int i = 0; i < this.messages.Count; i++)
			{
				GUILayout.Label(this.messages[i], new GUILayoutOption[]
				{
					GUILayout.MinWidth((float)Screen.width)
				});
			}
			GUILayout.EndScrollView();
			if (withInput)
			{
				GUILayout.Label("Your message: ", Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				this.messageToSend = GUILayout.TextField(this.messageToSend, Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Send", new GUILayoutOption[]
				{
					GUILayout.MaxWidth(100f)
				}))
				{
					this.SendMessage();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0009C3B8 File Offset: 0x0009A5B8
		private void AddMessage(string msg)
		{
			this.messages.Insert(0, msg);
			if (this.messages.Count > this.MaxMessages)
			{
				this.messages.RemoveRange(this.MaxMessages, this.messages.Count - this.MaxMessages);
			}
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0009C408 File Offset: 0x0009A608
		private void SendMessage()
		{
			if (string.IsNullOrEmpty(this.messageToSend))
			{
				return;
			}
			this.Socket.Emit("message", new object[]
			{
				this.messageToSend
			});
			this.AddMessage(string.Format("{0}: {1}", this.Nick, this.messageToSend));
			this.messageToSend = string.Empty;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0009C46A File Offset: 0x0009A66A
		private void Join()
		{
			PlayerPrefs.SetString("Nick", this.Nick);
			this.Socket.Emit("join", new object[]
			{
				this.Nick
			});
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0009C49C File Offset: 0x0009A69C
		private void Reload()
		{
			this.FrameTexture = null;
			if (this.Socket != null)
			{
				this.Socket.Manager.Close();
				this.Socket = null;
				this.Start();
			}
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0009C4CA File Offset: 0x0009A6CA
		private void OnConnected(Socket socket, Packet packet, params object[] args)
		{
			if (PlayerPrefs.HasKey("Nick"))
			{
				this.Nick = PlayerPrefs.GetString("Nick", "NickName");
				this.Join();
			}
			else
			{
				this.State = SocketIOWePlaySample.States.WaitForNick;
			}
			this.AddMessage("connected");
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0009C507 File Offset: 0x0009A707
		private void OnJoined(Socket socket, Packet packet, params object[] args)
		{
			this.State = SocketIOWePlaySample.States.Joined;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0009C510 File Offset: 0x0009A710
		private void OnReload(Socket socket, Packet packet, params object[] args)
		{
			this.Reload();
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0009C518 File Offset: 0x0009A718
		private void OnMessage(Socket socket, Packet packet, params object[] args)
		{
			if (args.Length == 1)
			{
				this.AddMessage(args[0] as string);
				return;
			}
			this.AddMessage(string.Format("{0}: {1}", args[1], args[0]));
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0009C545 File Offset: 0x0009A745
		private void OnMove(Socket socket, Packet packet, params object[] args)
		{
			this.AddMessage(string.Format("{0} pressed {1}", args[1], args[0]));
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0009C560 File Offset: 0x0009A760
		private void OnJoin(Socket socket, Packet packet, params object[] args)
		{
			string arg = (args.Length > 1) ? string.Format("({0})", args[1]) : string.Empty;
			this.AddMessage(string.Format("{0} joined {1}", args[0], arg));
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0009C59C File Offset: 0x0009A79C
		private void OnConnections(Socket socket, Packet packet, params object[] args)
		{
			this.connections = Convert.ToInt32(args[0]);
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0009C5AC File Offset: 0x0009A7AC
		private void OnFrame(Socket socket, Packet packet, params object[] args)
		{
			if (this.State != SocketIOWePlaySample.States.Joined)
			{
				return;
			}
			if (this.FrameTexture == null)
			{
				this.FrameTexture = new Texture2D(0, 0, TextureFormat.RGBA32, false);
				this.FrameTexture.filterMode = FilterMode.Point;
			}
			byte[] data = packet.Attachments[0];
			this.FrameTexture.LoadImage(data);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0009C606 File Offset: 0x0009A806
		private void OnError(Socket socket, Packet packet, params object[] args)
		{
			this.AddMessage(string.Format("--ERROR - {0}", args[0].ToString()));
		}

		// Token: 0x040013EE RID: 5102
		private string[] controls = new string[]
		{
			"left",
			"right",
			"a",
			"b",
			"up",
			"down",
			"select",
			"start"
		};

		// Token: 0x040013EF RID: 5103
		private const float ratio = 1.5f;

		// Token: 0x040013F0 RID: 5104
		private int MaxMessages = 50;

		// Token: 0x040013F1 RID: 5105
		private SocketIOWePlaySample.States State;

		// Token: 0x040013F2 RID: 5106
		private Socket Socket;

		// Token: 0x040013F3 RID: 5107
		private string Nick = string.Empty;

		// Token: 0x040013F4 RID: 5108
		private string messageToSend = string.Empty;

		// Token: 0x040013F5 RID: 5109
		private int connections;

		// Token: 0x040013F6 RID: 5110
		private List<string> messages = new List<string>();

		// Token: 0x040013F7 RID: 5111
		private Vector2 scrollPos;

		// Token: 0x040013F8 RID: 5112
		private Texture2D FrameTexture;

		// Token: 0x020008E9 RID: 2281
		private enum States
		{
			// Token: 0x0400347D RID: 13437
			Connecting,
			// Token: 0x0400347E RID: 13438
			WaitForNick,
			// Token: 0x0400347F RID: 13439
			Joined
		}
	}
}
