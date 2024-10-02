using System;
using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.JsonEncoders;
using BestHTTP.SignalR.Messages;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x020001A0 RID: 416
	public sealed class DemoHubSample : MonoBehaviour
	{
		// Token: 0x06000F1E RID: 3870 RVA: 0x00099320 File Offset: 0x00097520
		private void Start()
		{
			this.demoHub = new DemoHub();
			this.typedDemoHub = new TypedDemoHub();
			this.vbDemoHub = new Hub("vbdemo");
			this.signalRConnection = new Connection(this.URI, new Hub[]
			{
				this.demoHub,
				this.typedDemoHub,
				this.vbDemoHub
			});
			this.signalRConnection.JsonEncoder = new LitJsonEncoder();
			this.signalRConnection.OnConnected += delegate(Connection connection)
			{
				var <>f__AnonymousType = new
				{
					Name = "Foo",
					Age = 20,
					Address = new
					{
						Street = "One Microsoft Way",
						Zip = "98052"
					}
				};
				this.demoHub.AddToGroups();
				this.demoHub.GetValue();
				this.demoHub.TaskWithException();
				this.demoHub.GenericTaskWithException();
				this.demoHub.SynchronousException();
				this.demoHub.DynamicTask();
				this.demoHub.PassingDynamicComplex(<>f__AnonymousType);
				this.demoHub.SimpleArray(new int[]
				{
					5,
					5,
					6
				});
				this.demoHub.ComplexType(<>f__AnonymousType);
				this.demoHub.ComplexArray(new object[]
				{
					<>f__AnonymousType,
					<>f__AnonymousType,
					<>f__AnonymousType
				});
				this.demoHub.ReportProgress("Long running job!");
				this.demoHub.Overload();
				this.demoHub.State["name"] = "Testing state!";
				this.demoHub.ReadStateValue();
				this.demoHub.PlainTask();
				this.demoHub.GenericTaskWithContinueWith();
				this.typedDemoHub.Echo("Typed echo callback");
				this.vbDemoHub.Call("readStateValue", delegate(Hub hub, ClientMessage msg, ResultMessage result)
				{
					this.vbReadStateResult = string.Format("Read some state from VB.NET! => {0}", (result.ReturnValue == null) ? "undefined" : result.ReturnValue.ToString());
				}, Array.Empty<object>());
			};
			this.signalRConnection.Open();
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x000993B7 File Offset: 0x000975B7
		private void OnDestroy()
		{
			this.signalRConnection.Close();
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x000993C4 File Offset: 0x000975C4
		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, Array.Empty<GUILayoutOption>());
				GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
				this.demoHub.Draw();
				this.typedDemoHub.Draw();
				GUILayout.Label("Read State Value", Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Space(20f);
				GUILayout.Label(this.vbReadStateResult, Array.Empty<GUILayoutOption>());
				GUILayout.EndHorizontal();
				GUILayout.Space(10f);
				GUILayout.EndVertical();
				GUILayout.EndScrollView();
			});
		}

		// Token: 0x040013A6 RID: 5030
		private readonly Uri URI = new Uri(GUIHelper.BaseURL + "/signalr");

		// Token: 0x040013A7 RID: 5031
		private Connection signalRConnection;

		// Token: 0x040013A8 RID: 5032
		private DemoHub demoHub;

		// Token: 0x040013A9 RID: 5033
		private TypedDemoHub typedDemoHub;

		// Token: 0x040013AA RID: 5034
		private Hub vbDemoHub;

		// Token: 0x040013AB RID: 5035
		private string vbReadStateResult = string.Empty;

		// Token: 0x040013AC RID: 5036
		private Vector2 scrollPos;
	}
}
