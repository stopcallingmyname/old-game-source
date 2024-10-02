using System;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x020001A2 RID: 418
	internal class DemoHub : Hub
	{
		// Token: 0x06000F2A RID: 3882 RVA: 0x0009970C File Offset: 0x0009790C
		public DemoHub() : base("demo")
		{
			base.On("invoke", new OnMethodCallCallbackDelegate(this.Invoke));
			base.On("signal", new OnMethodCallCallbackDelegate(this.Signal));
			base.On("groupAdded", new OnMethodCallCallbackDelegate(this.GroupAdded));
			base.On("fromArbitraryCode", new OnMethodCallCallbackDelegate(this.FromArbitraryCode));
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00099848 File Offset: 0x00097A48
		public void ReportProgress(string arg)
		{
			base.Call("reportProgress", new OnMethodResultDelegate(this.OnLongRunningJob_Done), null, new OnMethodProgressDelegate(this.OnLongRunningJob_Progress), new object[]
			{
				arg
			});
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00099884 File Offset: 0x00097A84
		public void OnLongRunningJob_Progress(Hub hub, ClientMessage originialMessage, ProgressMessage progress)
		{
			this.longRunningJobProgress = (float)progress.Progress;
			this.longRunningJobStatus = progress.Progress.ToString() + "%";
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x000998BC File Offset: 0x00097ABC
		public void OnLongRunningJob_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
		{
			this.longRunningJobStatus = result.ReturnValue.ToString();
			this.MultipleCalls();
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x000998D5 File Offset: 0x00097AD5
		public void MultipleCalls()
		{
			base.Call("multipleCalls", Array.Empty<object>());
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x000998E8 File Offset: 0x00097AE8
		public void DynamicTask()
		{
			base.Call("dynamicTask", new OnMethodResultDelegate(this.OnDynamicTask_Done), new OnMethodFailedDelegate(this.OnDynamicTask_Failed), Array.Empty<object>());
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00099913 File Offset: 0x00097B13
		private void OnDynamicTask_Failed(Hub hub, ClientMessage originalMessage, FailureMessage result)
		{
			this.dynamicTaskResult = string.Format("The dynamic task failed :( {0}", result.ErrorMessage);
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0009992B File Offset: 0x00097B2B
		private void OnDynamicTask_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
		{
			this.dynamicTaskResult = string.Format("The dynamic task! {0}", result.ReturnValue);
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00099943 File Offset: 0x00097B43
		public void AddToGroups()
		{
			base.Call("addToGroups", Array.Empty<object>());
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00099956 File Offset: 0x00097B56
		public void GetValue()
		{
			base.Call("getValue", delegate(Hub hub, ClientMessage msg, ResultMessage result)
			{
				this.genericTaskResult = string.Format("The value is {0} after 5 seconds", result.ReturnValue);
			}, Array.Empty<object>());
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00099975 File Offset: 0x00097B75
		public void TaskWithException()
		{
			base.Call("taskWithException", null, delegate(Hub hub, ClientMessage msg, FailureMessage error)
			{
				this.taskWithExceptionResult = string.Format("Error: {0}", error.ErrorMessage);
			}, Array.Empty<object>());
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00099995 File Offset: 0x00097B95
		public void GenericTaskWithException()
		{
			base.Call("genericTaskWithException", null, delegate(Hub hub, ClientMessage msg, FailureMessage error)
			{
				this.genericTaskWithExceptionResult = string.Format("Error: {0}", error.ErrorMessage);
			}, Array.Empty<object>());
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x000999B5 File Offset: 0x00097BB5
		public void SynchronousException()
		{
			base.Call("synchronousException", null, delegate(Hub hub, ClientMessage msg, FailureMessage error)
			{
				this.synchronousExceptionResult = string.Format("Error: {0}", error.ErrorMessage);
			}, Array.Empty<object>());
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x000999D5 File Offset: 0x00097BD5
		public void PassingDynamicComplex(object person)
		{
			base.Call("passingDynamicComplex", delegate(Hub hub, ClientMessage msg, ResultMessage result)
			{
				this.invokingHubMethodWithDynamicResult = string.Format("The person's age is {0}", result.ReturnValue);
			}, new object[]
			{
				person
			});
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x000999F9 File Offset: 0x00097BF9
		public void SimpleArray(int[] array)
		{
			base.Call("simpleArray", delegate(Hub hub, ClientMessage msg, ResultMessage result)
			{
				this.simpleArrayResult = "Simple array works!";
			}, new object[]
			{
				array
			});
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00099A1D File Offset: 0x00097C1D
		public void ComplexType(object person)
		{
			base.Call("complexType", delegate(Hub hub, ClientMessage msg, ResultMessage result)
			{
				this.complexTypeResult = string.Format("Complex Type -> {0}", ((IHub)this).Connection.JsonEncoder.Encode(base.State["person"]));
			}, new object[]
			{
				person
			});
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x00099A41 File Offset: 0x00097C41
		public void ComplexArray(object[] complexArray)
		{
			base.Call("ComplexArray", delegate(Hub hub, ClientMessage msg, ResultMessage result)
			{
				this.complexArrayResult = "Complex Array Works!";
			}, new object[]
			{
				complexArray
			});
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00099A65 File Offset: 0x00097C65
		public void Overload()
		{
			base.Call("Overload", new OnMethodResultDelegate(this.OnVoidOverload_Done), Array.Empty<object>());
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x00099A84 File Offset: 0x00097C84
		private void OnVoidOverload_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
		{
			this.voidOverloadResult = "Void Overload called";
			this.Overload(101);
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00099A99 File Offset: 0x00097C99
		public void Overload(int number)
		{
			base.Call("Overload", new OnMethodResultDelegate(this.OnIntOverload_Done), new object[]
			{
				number
			});
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x00099AC2 File Offset: 0x00097CC2
		private void OnIntOverload_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
		{
			this.intOverloadResult = string.Format("Overload with return value called => {0}", result.ReturnValue.ToString());
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00099ADF File Offset: 0x00097CDF
		public void ReadStateValue()
		{
			base.Call("readStateValue", delegate(Hub hub, ClientMessage msg, ResultMessage result)
			{
				this.readStateResult = string.Format("Read some state! => {0}", result.ReturnValue);
			}, Array.Empty<object>());
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x00099AFE File Offset: 0x00097CFE
		public void PlainTask()
		{
			base.Call("plainTask", delegate(Hub hub, ClientMessage msg, ResultMessage result)
			{
				this.plainTaskResult = "Plain Task Result";
			}, Array.Empty<object>());
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00099B1D File Offset: 0x00097D1D
		public void GenericTaskWithContinueWith()
		{
			base.Call("genericTaskWithContinueWith", delegate(Hub hub, ClientMessage msg, ResultMessage result)
			{
				this.genericTaskWithContinueWithResult = result.ReturnValue.ToString();
			}, Array.Empty<object>());
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x00099B3C File Offset: 0x00097D3C
		private void FromArbitraryCode(Hub hub, MethodCallMessage methodCall)
		{
			this.fromArbitraryCodeResult = (methodCall.Arguments[0] as string);
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x00099B51 File Offset: 0x00097D51
		private void GroupAdded(Hub hub, MethodCallMessage methodCall)
		{
			if (!string.IsNullOrEmpty(this.groupAddedResult))
			{
				this.groupAddedResult = "Group Already Added!";
				return;
			}
			this.groupAddedResult = "Group Added!";
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x00099B77 File Offset: 0x00097D77
		private void Signal(Hub hub, MethodCallMessage methodCall)
		{
			this.dynamicTaskResult = string.Format("The dynamic task! {0}", methodCall.Arguments[0]);
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x00099B91 File Offset: 0x00097D91
		private void Invoke(Hub hub, MethodCallMessage methodCall)
		{
			this.invokeResults.Add(string.Format("{0} client state index -> {1}", methodCall.Arguments[0], base.State["index"]));
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x00099BC0 File Offset: 0x00097DC0
		public void Draw()
		{
			GUILayout.Label("Arbitrary Code", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(string.Format("Sending {0} from arbitrary code without the hub itself!", this.fromArbitraryCodeResult), Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Group Added", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(this.groupAddedResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Dynamic Task", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(this.dynamicTaskResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Report Progress", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Label(this.longRunningJobStatus, Array.Empty<GUILayoutOption>());
			GUILayout.HorizontalSlider(this.longRunningJobProgress, 0f, 100f, Array.Empty<GUILayoutOption>());
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Generic Task", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(this.genericTaskResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Task With Exception", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(this.taskWithExceptionResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Generic Task With Exception", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(this.genericTaskWithExceptionResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Synchronous Exception", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(this.synchronousExceptionResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Invoking hub method with dynamic", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(this.invokingHubMethodWithDynamicResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Simple Array", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(this.simpleArrayResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Complex Type", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(this.complexTypeResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Complex Array", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(this.complexArrayResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Overloads", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Label(this.voidOverloadResult, Array.Empty<GUILayoutOption>());
			GUILayout.Label(this.intOverloadResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Read State Value", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(this.readStateResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Plain Task", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(this.plainTaskResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Generic Task With ContinueWith", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label(this.genericTaskWithContinueWithResult, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.Label("Message Pump", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			this.invokeResults.Draw((float)(Screen.width - 40), 270f);
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
		}

		// Token: 0x040013AF RID: 5039
		private float longRunningJobProgress;

		// Token: 0x040013B0 RID: 5040
		private string longRunningJobStatus = "Not Started!";

		// Token: 0x040013B1 RID: 5041
		private string fromArbitraryCodeResult = string.Empty;

		// Token: 0x040013B2 RID: 5042
		private string groupAddedResult = string.Empty;

		// Token: 0x040013B3 RID: 5043
		private string dynamicTaskResult = string.Empty;

		// Token: 0x040013B4 RID: 5044
		private string genericTaskResult = string.Empty;

		// Token: 0x040013B5 RID: 5045
		private string taskWithExceptionResult = string.Empty;

		// Token: 0x040013B6 RID: 5046
		private string genericTaskWithExceptionResult = string.Empty;

		// Token: 0x040013B7 RID: 5047
		private string synchronousExceptionResult = string.Empty;

		// Token: 0x040013B8 RID: 5048
		private string invokingHubMethodWithDynamicResult = string.Empty;

		// Token: 0x040013B9 RID: 5049
		private string simpleArrayResult = string.Empty;

		// Token: 0x040013BA RID: 5050
		private string complexTypeResult = string.Empty;

		// Token: 0x040013BB RID: 5051
		private string complexArrayResult = string.Empty;

		// Token: 0x040013BC RID: 5052
		private string voidOverloadResult = string.Empty;

		// Token: 0x040013BD RID: 5053
		private string intOverloadResult = string.Empty;

		// Token: 0x040013BE RID: 5054
		private string readStateResult = string.Empty;

		// Token: 0x040013BF RID: 5055
		private string plainTaskResult = string.Empty;

		// Token: 0x040013C0 RID: 5056
		private string genericTaskWithContinueWithResult = string.Empty;

		// Token: 0x040013C1 RID: 5057
		private GUIMessageList invokeResults = new GUIMessageList();
	}
}
