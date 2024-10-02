using System;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x020001A1 RID: 417
	internal class TypedDemoHub : Hub
	{
		// Token: 0x06000F25 RID: 3877 RVA: 0x0009960D File Offset: 0x0009780D
		public TypedDemoHub() : base("typeddemohub")
		{
			base.On("Echo", new OnMethodCallCallbackDelegate(this.Echo));
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00099647 File Offset: 0x00097847
		private void Echo(Hub hub, MethodCallMessage methodCall)
		{
			this.typedEchoClientResult = string.Format("{0} #{1} triggered!", methodCall.Arguments[0], methodCall.Arguments[1]);
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x00099669 File Offset: 0x00097869
		public void Echo(string msg)
		{
			base.Call("echo", new OnMethodResultDelegate(this.OnEcho_Done), new object[]
			{
				msg
			});
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0009968D File Offset: 0x0009788D
		private void OnEcho_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
		{
			this.typedEchoResult = "TypedDemoHub.Echo(string message) invoked!";
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0009969C File Offset: 0x0009789C
		public void Draw()
		{
			GUILayout.Label("Typed callback", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Label(this.typedEchoResult, Array.Empty<GUILayoutOption>());
			GUILayout.Label(this.typedEchoClientResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
		}

		// Token: 0x040013AD RID: 5037
		private string typedEchoResult = string.Empty;

		// Token: 0x040013AE RID: 5038
		private string typedEchoClientResult = string.Empty;
	}
}
