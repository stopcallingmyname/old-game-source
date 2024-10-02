using System;
using System.Collections;
using BestHTTP.Futures;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using BestHTTP.SignalRCore.Messages;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x020001AB RID: 427
	public sealed class UploadHubSample : MonoBehaviour
	{
		// Token: 0x06000FB6 RID: 4022 RVA: 0x0009B5BC File Offset: 0x000997BC
		private void Start()
		{
			HubOptions hubOptions = new HubOptions();
			hubOptions.SkipNegotiation = true;
			this.hub = new HubConnection(this.URI, new JsonProtocol(new LitJsonEncoder()), hubOptions);
			this.hub.OnConnected += this.Hub_OnConnected;
			this.hub.OnError += this.Hub_OnError;
			this.hub.OnClosed += this.Hub_OnClosed;
			this.hub.OnMessage += this.Hub_OnMessage;
			this.hub.OnRedirected += this.Hub_Redirected;
			this.hub.StartConnect();
			this.uiText = "StartConnect called\n";
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0009B67B File Offset: 0x0009987B
		private void OnDestroy()
		{
			if (this.hub != null)
			{
				this.hub.StartClose();
			}
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0009B690 File Offset: 0x00099890
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

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0009B6A9 File Offset: 0x000998A9
		private void Hub_Redirected(HubConnection hub, Uri oldUri, Uri newUri)
		{
			this.uiText += string.Format("Hub connection redirected to '<color=green>{0}</color>'!\n", hub.Uri);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0009B6CC File Offset: 0x000998CC
		private void Hub_OnConnected(HubConnection hub)
		{
			this.uiText += "Hub Connected\n";
			base.StartCoroutine(this.UploadWord());
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0009B6F1 File Offset: 0x000998F1
		private IEnumerator UploadWord()
		{
			this.uiText += "\n<color=green>UploadWord</color>:\n";
			UploadItemController<string> controller = this.hub.Upload("UploadWord");
			controller.OnComplete(delegate(IFuture<string> result)
			{
				this.uiText += string.Format("-UploadWord completed, result: '<color=yellow>{0}</color>'\n", result.value);
				base.StartCoroutine(this.ScoreTracker());
			});
			yield return new WaitForSeconds(0.1f);
			controller.Upload("Hello ");
			this.uiText += "-'<color=green>Hello </color>' uploaded!\n";
			yield return new WaitForSeconds(0.1f);
			controller.Upload("World");
			this.uiText += "-'<color=green>World</color>' uploaded!\n";
			yield return new WaitForSeconds(0.1f);
			controller.Upload("!!");
			this.uiText += "-'<color=green>!!</color>' uploaded!\n";
			yield return new WaitForSeconds(0.1f);
			controller.Finish();
			this.uiText += "-Sent upload finished message.\n";
			yield return new WaitForSeconds(0.1f);
			yield break;
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0009B700 File Offset: 0x00099900
		private IEnumerator ScoreTracker()
		{
			this.uiText += "\n<color=green>ScoreTracker</color>:\n";
			UploadItemController<string> controller = this.hub.UploadStream("ScoreTracker");
			controller.OnComplete(delegate(IFuture<string> result)
			{
				this.uiText += string.Format("-ScoreTracker completed, result: '<color=yellow>{0}</color>'\n", result.value);
				base.StartCoroutine(this.ScoreTrackerWithParameterChannels());
			});
			int num3;
			for (int i = 0; i < 5; i = num3 + 1)
			{
				yield return new WaitForSeconds(0.1f);
				int num = Random.Range(0, 10);
				int num2 = Random.Range(0, 10);
				controller.Upload(num, num2);
				this.uiText += string.Format("-Score({0}/{1}) uploaded! p1's score: <color=green>{2}</color> p2's score: <color=green>{3}</color>\n", new object[]
				{
					i + 1,
					5,
					num,
					num2
				});
				num3 = i;
			}
			yield return new WaitForSeconds(0.1f);
			controller.Finish();
			this.uiText += "-Sent upload finished message.\n";
			yield return new WaitForSeconds(0.1f);
			yield break;
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0009B70F File Offset: 0x0009990F
		private IEnumerator ScoreTrackerWithParameterChannels()
		{
			this.uiText += "\n<color=green>ScoreTracker using upload channels</color>:\n";
			using (UploadItemController<string> controller = this.hub.UploadStream("ScoreTracker"))
			{
				controller.OnComplete(delegate(IFuture<string> result)
				{
					this.uiText += string.Format("-ScoreTracker completed, result: '<color=yellow>{0}</color>'\n", result.value);
					base.StartCoroutine(this.StreamEcho());
				});
				using (UploadChannel<string, int> player1param = controller.GetUploadChannel<int>(0))
				{
					int num2;
					for (int i = 0; i < 5; i = num2 + 1)
					{
						yield return new WaitForSeconds(0.1f);
						int num = Random.Range(0, 10);
						player1param.Upload(num);
						this.uiText += string.Format("-Player 1's score({0}/{1}) uploaded! Score: <color=green>{2}</color>\n", i + 1, 5, num);
						num2 = i;
					}
				}
				UploadChannel<string, int> player1param = null;
				this.uiText += "\n";
				using (UploadChannel<string, int> player1param = controller.GetUploadChannel<int>(1))
				{
					int num2;
					for (int i = 0; i < 5; i = num2 + 1)
					{
						yield return new WaitForSeconds(0.1f);
						int num3 = Random.Range(0, 10);
						player1param.Upload(num3);
						this.uiText += string.Format("-Player 2's score({0}/{1}) uploaded! Score: <color=green>{2}</color>\n", i + 1, 5, num3);
						num2 = i;
					}
				}
				player1param = null;
				this.uiText += "\n-All scores uploaded!\n";
			}
			UploadItemController<string> controller = null;
			yield return new WaitForSeconds(0.1f);
			yield break;
			yield break;
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0009B71E File Offset: 0x0009991E
		private IEnumerator StreamEcho()
		{
			this.uiText += "\n<color=green>StreamEcho</color>:\n";
			using (UploadItemController<StreamItemContainer<string>> controller = this.hub.UploadStreamWithDownStream("StreamEcho"))
			{
				controller.OnComplete(delegate(IFuture<StreamItemContainer<string>> result)
				{
					this.uiText += "-StreamEcho completed!\n";
					base.StartCoroutine(this.PersonEcho());
				});
				controller.OnItem(delegate(StreamItemContainer<string> item)
				{
					this.uiText += string.Format("-Received from server: '<color=yellow>{0}</color>'\n", item.LastAdded);
				});
				int num;
				for (int i = 0; i < 5; i = num + 1)
				{
					yield return new WaitForSeconds(0.1f);
					string text = string.Format("Message from client {0}/{1}", i + 1, 5);
					controller.Upload(text);
					this.uiText += string.Format("-Sent message to the server: <color=green>{0}</color>\n", text);
					num = i;
				}
				yield return new WaitForSeconds(0.1f);
			}
			UploadItemController<StreamItemContainer<string>> controller = null;
			this.uiText += "-Upload finished!\n";
			yield return new WaitForSeconds(0.1f);
			yield break;
			yield break;
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0009B72D File Offset: 0x0009992D
		private IEnumerator PersonEcho()
		{
			this.uiText += "\n<color=green>PersonEcho</color>:\n";
			using (UploadItemController<StreamItemContainer<Person>> controller = this.hub.UploadStreamWithDownStream("PersonEcho"))
			{
				controller.OnComplete(delegate(IFuture<StreamItemContainer<Person>> result)
				{
					this.uiText += "-PersonEcho completed!\n";
				});
				controller.OnItem(delegate(StreamItemContainer<Person> item)
				{
					this.uiText += string.Format("-Received from server: '<color=yellow>{0}</color>'\n", item.LastAdded);
				});
				int num;
				for (int i = 0; i < 5; i = num + 1)
				{
					yield return new WaitForSeconds(0.1f);
					Person person = new Person
					{
						Name = "Mr. Smith",
						Age = (long)(20 + i * 2)
					};
					controller.Upload(person);
					this.uiText += string.Format("-Sent person to the server: <color=green>{0}</color>\n", person);
					num = i;
				}
				yield return new WaitForSeconds(0.1f);
			}
			UploadItemController<StreamItemContainer<Person>> controller = null;
			this.uiText += "-Upload finished!\n";
			yield return new WaitForSeconds(0.1f);
			yield break;
			yield break;
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0006AE98 File Offset: 0x00069098
		private bool Hub_OnMessage(HubConnection hub, Message message)
		{
			return true;
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0009B73C File Offset: 0x0009993C
		private void Hub_OnClosed(HubConnection hub)
		{
			this.uiText += "Hub Closed\n";
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0009B754 File Offset: 0x00099954
		private void Hub_OnError(HubConnection hub, string error)
		{
			this.uiText = this.uiText + "Hub Error: " + error + "\n";
		}

		// Token: 0x040013DF RID: 5087
		private readonly Uri URI = new Uri(GUIHelper.BaseURL + "/uploading");

		// Token: 0x040013E0 RID: 5088
		public HubConnection hub;

		// Token: 0x040013E1 RID: 5089
		private Vector2 scrollPos;

		// Token: 0x040013E2 RID: 5090
		public string uiText;

		// Token: 0x040013E3 RID: 5091
		private const float YieldWaitTime = 0.1f;
	}
}
