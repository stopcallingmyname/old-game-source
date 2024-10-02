using System;
using BestHTTP.ServerSentEvents;
using LitJson;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x0200019A RID: 410
	public class SimpleTest : MonoBehaviour
	{
		// Token: 0x06000EEA RID: 3818 RVA: 0x0009833B File Offset: 0x0009653B
		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, Array.Empty<GUILayoutOption>());
				GUILayout.Label(this.Text, Array.Empty<GUILayoutOption>());
				GUILayout.EndScrollView();
				GUILayout.Space(5f);
				GUILayout.FlexibleSpace();
				this.address = GUILayout.TextField(this.address, Array.Empty<GUILayoutOption>());
				if (this.eventSource == null && GUILayout.Button("Open Server-Sent Events", Array.Empty<GUILayoutOption>()))
				{
					this.eventSource = new EventSource(new Uri(this.address));
					this.eventSource.OnOpen += this.OnOpen;
					this.eventSource.OnClosed += this.OnClosed;
					this.eventSource.OnError += this.OnError;
					this.eventSource.OnStateChanged += this.OnStateChanged;
					this.eventSource.OnMessage += this.OnMessage;
					this.eventSource.On("datetime", new OnEventDelegate(this.OnDateTime));
					this.eventSource.Open();
					this.Text += "Opening Server-Sent Events...\n";
				}
				if (this.eventSource != null && this.eventSource.State == States.Open)
				{
					GUILayout.Space(10f);
					if (GUILayout.Button("Close", Array.Empty<GUILayoutOption>()))
					{
						this.eventSource.Close();
					}
				}
			});
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00098354 File Offset: 0x00096554
		private void OnOpen(EventSource eventSource)
		{
			this.Text += "Open\n";
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0009836C File Offset: 0x0009656C
		private void OnClosed(EventSource eventSource)
		{
			this.Text += "Closed\n";
			this.eventSource = null;
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0009838B File Offset: 0x0009658B
		private void OnError(EventSource eventSource, string error)
		{
			this.Text = this.Text + "Error: " + error + "\n";
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x000983A9 File Offset: 0x000965A9
		private void OnStateChanged(EventSource eventSource, States oldState, States newState)
		{
			this.Text += string.Format("State Changed {0} => {1}\n", oldState, newState);
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x000983D2 File Offset: 0x000965D2
		private void OnMessage(EventSource eventSource, Message message)
		{
			this.Text += string.Format("Message: {0}\n", message);
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x000983F0 File Offset: 0x000965F0
		private void OnDateTime(EventSource eventSource, Message message)
		{
			DateTimeData dateTimeData = JsonMapper.ToObject<DateTimeData>(message.Data);
			this.Text += string.Format("OnDateTime: {0}\n", dateTimeData.ToString());
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0009842A File Offset: 0x0009662A
		private void OnDestroy()
		{
			if (this.eventSource != null)
			{
				this.eventSource.Close();
				this.eventSource = null;
			}
		}

		// Token: 0x0400138F RID: 5007
		private string address = GUIHelper.BaseURL + "/sse";

		// Token: 0x04001390 RID: 5008
		private EventSource eventSource;

		// Token: 0x04001391 RID: 5009
		private string Text = string.Empty;

		// Token: 0x04001392 RID: 5010
		private Vector2 scrollPos;
	}
}
