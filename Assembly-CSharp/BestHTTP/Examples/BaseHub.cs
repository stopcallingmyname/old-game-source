using System;
using System.Collections.Generic;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x0200019D RID: 413
	internal class BaseHub : Hub
	{
		// Token: 0x06000EFD RID: 3837 RVA: 0x000988E8 File Offset: 0x00096AE8
		public BaseHub(string name, string title) : base(name)
		{
			this.Title = title;
			base.On("joined", new OnMethodCallCallbackDelegate(this.Joined));
			base.On("rejoined", new OnMethodCallCallbackDelegate(this.Rejoined));
			base.On("left", new OnMethodCallCallbackDelegate(this.Left));
			base.On("invoked", new OnMethodCallCallbackDelegate(this.Invoked));
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0009896C File Offset: 0x00096B6C
		private void Joined(Hub hub, MethodCallMessage methodCall)
		{
			Dictionary<string, object> dictionary = methodCall.Arguments[2] as Dictionary<string, object>;
			this.messages.Add(string.Format("{0} joined at {1}\n\tIsAuthenticated: {2} IsAdmin: {3} UserName: {4}", new object[]
			{
				methodCall.Arguments[0],
				methodCall.Arguments[1],
				dictionary["IsAuthenticated"],
				dictionary["IsAdmin"],
				dictionary["UserName"]
			}));
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x000989E2 File Offset: 0x00096BE2
		private void Rejoined(Hub hub, MethodCallMessage methodCall)
		{
			this.messages.Add(string.Format("{0} reconnected at {1}", methodCall.Arguments[0], methodCall.Arguments[1]));
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00098A09 File Offset: 0x00096C09
		private void Left(Hub hub, MethodCallMessage methodCall)
		{
			this.messages.Add(string.Format("{0} left at {1}", methodCall.Arguments[0], methodCall.Arguments[1]));
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00098A30 File Offset: 0x00096C30
		private void Invoked(Hub hub, MethodCallMessage methodCall)
		{
			this.messages.Add(string.Format("{0} invoked hub method at {1}", methodCall.Arguments[0], methodCall.Arguments[1]));
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00098A57 File Offset: 0x00096C57
		public void InvokedFromClient()
		{
			base.Call("invokedFromClient", new OnMethodResultDelegate(this.OnInvoked), new OnMethodFailedDelegate(this.OnInvokeFailed), Array.Empty<object>());
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00098A82 File Offset: 0x00096C82
		private void OnInvoked(Hub hub, ClientMessage originalMessage, ResultMessage result)
		{
			Debug.Log(hub.Name + " invokedFromClient success!");
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00098A99 File Offset: 0x00096C99
		private void OnInvokeFailed(Hub hub, ClientMessage originalMessage, FailureMessage result)
		{
			Debug.LogWarning(hub.Name + " " + result.ErrorMessage);
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00098AB8 File Offset: 0x00096CB8
		public void Draw()
		{
			GUILayout.Label(this.Title, Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			this.messages.Draw((float)(Screen.width - 20), 100f);
			GUILayout.EndHorizontal();
		}

		// Token: 0x0400139A RID: 5018
		private string Title;

		// Token: 0x0400139B RID: 5019
		private GUIMessageList messages = new GUIMessageList();
	}
}
