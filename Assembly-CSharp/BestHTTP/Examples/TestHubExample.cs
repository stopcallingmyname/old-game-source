using System;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using BestHTTP.SignalRCore.Messages;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x020001A9 RID: 425
	public class TestHubExample : MonoBehaviour
	{
		// Token: 0x06000F91 RID: 3985 RVA: 0x0009ADF8 File Offset: 0x00098FF8
		private void Start()
		{
			HubOptions hubOptions = new HubOptions();
			hubOptions.SkipNegotiation = false;
			this.hub = new HubConnection(this.URI, new JsonProtocol(new LitJsonEncoder()), hubOptions);
			this.hub.OnConnected += this.Hub_OnConnected;
			this.hub.OnError += this.Hub_OnError;
			this.hub.OnClosed += this.Hub_OnClosed;
			this.hub.OnMessage += this.Hub_OnMessage;
			this.hub.On<string>("Send", delegate(string arg)
			{
				this.uiText += string.Format(" On Send: {0}\n", arg);
			});
			this.hub.On<TestHubExample.Person>("Person", delegate(TestHubExample.Person person)
			{
				this.uiText += string.Format(" On Person: {0}\n", person);
			});
			this.hub.On<TestHubExample.Person, TestHubExample.Person>("TwoPersons", delegate(TestHubExample.Person person1, TestHubExample.Person person2)
			{
				this.uiText += string.Format(" On TwoPersons: {0}, {1}\n", person1, person2);
			});
			this.hub.StartConnect();
			this.uiText = "StartConnect called\n";
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0009AEF4 File Offset: 0x000990F4
		private void OnDestroy()
		{
			if (this.hub != null)
			{
				this.hub.StartClose();
			}
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0009AF09 File Offset: 0x00099109
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

		// Token: 0x06000F94 RID: 3988 RVA: 0x0009AF24 File Offset: 0x00099124
		private void Hub_OnConnected(HubConnection hub)
		{
			this.uiText += "Hub Connected\n";
			hub.Send("Send", new object[]
			{
				"my message"
			});
			hub.Invoke<string>("NoParam", Array.Empty<object>()).OnSuccess(delegate(string ret)
			{
				this.uiText += string.Format(" 'NoParam' returned: {0}\n", ret);
			});
			hub.Invoke<int>("Add", new object[]
			{
				10,
				20
			}).OnSuccess(delegate(int result)
			{
				this.uiText += string.Format(" 'Add(10, 20)' returned: {0}\n", result);
			}).OnError(delegate(Exception error)
			{
				this.uiText += string.Format(" 'Add(10, 20)' error: {0}\n", error);
			});
			hub.Invoke<int?>("NullableTest", new object[]
			{
				10
			}).OnSuccess(delegate(int? result)
			{
				this.uiText += string.Format(" 'NullableTest(10)' returned: {0}\n", result);
			}).OnError(delegate(Exception error)
			{
				this.uiText += string.Format(" 'NullableTest(10)' error: {0}\n", error);
			});
			hub.Invoke<TestHubExample.Person>("GetPerson", new object[]
			{
				"Mr. Smith",
				26
			}).OnSuccess(delegate(TestHubExample.Person result)
			{
				this.uiText += string.Format(" 'GetPerson(\"Mr. Smith\", 26)' returned: {0}\n", result);
			}).OnError(delegate(Exception error)
			{
				this.uiText += string.Format(" 'GetPerson(\"Mr. Smith\", 26)' error: {0}\n", error);
			});
			hub.Invoke<int>("SingleResultFailure", new object[]
			{
				10,
				20
			}).OnSuccess(delegate(int result)
			{
				this.uiText += string.Format(" 'SingleResultFailure(10, 20)' returned: {0}\n", result);
			}).OnError(delegate(Exception error)
			{
				this.uiText += string.Format(" 'SingleResultFailure(10, 20)' error: {0}\n", error);
			});
			hub.Invoke<int[]>("Batched", new object[]
			{
				10
			}).OnSuccess(delegate(int[] result)
			{
				this.uiText += string.Format(" 'Batched(10)' returned items: {0}\n", result.Length);
			}).OnError(delegate(Exception error)
			{
				this.uiText += string.Format(" 'Batched(10)' error: {0}\n", error);
			});
			hub.Stream<int>("ObservableCounter", new object[]
			{
				10,
				1000
			}).OnItem(delegate(StreamItemContainer<int> result)
			{
				this.uiText += string.Format(" 'ObservableCounter(10, 1000)' OnItem: {0}\n", result.LastAdded);
			}).OnSuccess(delegate(StreamItemContainer<int> result)
			{
				this.uiText += string.Format(" 'ObservableCounter(10, 1000)' OnSuccess. Final count: {0}\n", result.Items.Count);
			}).OnError(delegate(Exception error)
			{
				this.uiText += string.Format(" 'ObservableCounter(10, 1000)' error: {0}\n", error);
			});
			StreamItemContainer<int> value = hub.Stream<int>("ChannelCounter", new object[]
			{
				10,
				1000
			}).OnItem(delegate(StreamItemContainer<int> result)
			{
				this.uiText += string.Format(" 'ChannelCounter(10, 1000)' OnItem: {0}\n", result.LastAdded);
			}).OnSuccess(delegate(StreamItemContainer<int> result)
			{
				this.uiText += string.Format(" 'ChannelCounter(10, 1000)' OnSuccess. Final count: {0}\n", result.Items.Count);
			}).OnError(delegate(Exception error)
			{
				this.uiText += string.Format(" 'ChannelCounter(10, 1000)' error: {0}\n", error);
			}).value;
			hub.CancelStream<int>(value);
			hub.Stream<TestHubExample.Person>("GetRandomPersons", new object[]
			{
				20,
				2000
			}).OnItem(delegate(StreamItemContainer<TestHubExample.Person> result)
			{
				this.uiText += string.Format(" 'GetRandomPersons(20, 1000)' OnItem: {0}\n", result.LastAdded);
			}).OnSuccess(delegate(StreamItemContainer<TestHubExample.Person> result)
			{
				this.uiText += string.Format(" 'GetRandomPersons(20, 1000)' OnSuccess. Final count: {0}\n", result.Items.Count);
			});
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0006AE98 File Offset: 0x00069098
		private bool Hub_OnMessage(HubConnection hub, Message message)
		{
			return true;
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0009B1E4 File Offset: 0x000993E4
		private void Hub_OnClosed(HubConnection hub)
		{
			this.uiText += "Hub Closed\n";
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0009B1FC File Offset: 0x000993FC
		private void Hub_OnError(HubConnection hub, string error)
		{
			this.uiText = this.uiText + "Hub Error: " + error + "\n";
		}

		// Token: 0x040013D9 RID: 5081
		private readonly Uri URI = new Uri(GUIHelper.BaseURL + "/TestHub");

		// Token: 0x040013DA RID: 5082
		private HubConnection hub;

		// Token: 0x040013DB RID: 5083
		private Vector2 scrollPos;

		// Token: 0x040013DC RID: 5084
		private string uiText;

		// Token: 0x020008E0 RID: 2272
		private sealed class Person
		{
			// Token: 0x17000C19 RID: 3097
			// (get) Token: 0x06004DC7 RID: 19911 RVA: 0x001AFC69 File Offset: 0x001ADE69
			// (set) Token: 0x06004DC8 RID: 19912 RVA: 0x001AFC71 File Offset: 0x001ADE71
			public string Name { get; set; }

			// Token: 0x17000C1A RID: 3098
			// (get) Token: 0x06004DC9 RID: 19913 RVA: 0x001AFC7A File Offset: 0x001ADE7A
			// (set) Token: 0x06004DCA RID: 19914 RVA: 0x001AFC82 File Offset: 0x001ADE82
			public long Age { get; set; }

			// Token: 0x06004DCB RID: 19915 RVA: 0x001AFC8C File Offset: 0x001ADE8C
			public override string ToString()
			{
				return string.Format("[Person Name: '{0}', Age: {1}]", this.Name, this.Age.ToString());
			}
		}
	}
}
