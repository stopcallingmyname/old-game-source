using System;
using System.Collections.Generic;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x02000194 RID: 404
	public sealed class LargeFileDownloadSample : MonoBehaviour
	{
		// Token: 0x06000EAF RID: 3759 RVA: 0x00096D7F File Offset: 0x00094F7F
		private void Awake()
		{
			if (PlayerPrefs.HasKey("DownloadLength"))
			{
				this.progress = (float)PlayerPrefs.GetInt("DownloadProgress") / (float)PlayerPrefs.GetInt("DownloadLength");
			}
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00096DAA File Offset: 0x00094FAA
		private void OnDestroy()
		{
			if (this.request != null && this.request.State < HTTPRequestStates.Finished)
			{
				this.request.OnProgress = null;
				this.request.Callback = null;
				this.request.Abort();
			}
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00096DE5 File Offset: 0x00094FE5
		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				GUILayout.Label("Request status: " + this.status, Array.Empty<GUILayoutOption>());
				GUILayout.Space(5f);
				GUILayout.Label(string.Format("Progress: {0:P2} of {1:N0}Mb", this.progress, PlayerPrefs.GetInt("DownloadLength") / 1048576), Array.Empty<GUILayoutOption>());
				GUILayout.HorizontalSlider(this.progress, 0f, 1f, Array.Empty<GUILayoutOption>());
				GUILayout.Space(50f);
				if (this.request == null)
				{
					GUILayout.Label(string.Format("Desired Fragment Size: {0:N} KBytes", (float)this.fragmentSize / 1024f), Array.Empty<GUILayoutOption>());
					this.fragmentSize = (int)GUILayout.HorizontalSlider((float)this.fragmentSize, 4096f, 10485760f, Array.Empty<GUILayoutOption>());
					GUILayout.Space(5f);
					if (GUILayout.Button(PlayerPrefs.HasKey("DownloadProgress") ? "Continue Download" : "Start Download", Array.Empty<GUILayoutOption>()))
					{
						this.StreamLargeFileTest();
						return;
					}
				}
				else if (this.request.State == HTTPRequestStates.Processing && GUILayout.Button("Abort Download", Array.Empty<GUILayoutOption>()))
				{
					this.request.Abort();
				}
			});
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00096E00 File Offset: 0x00095000
		private void StreamLargeFileTest()
		{
			this.request = new HTTPRequest(new Uri("https://uk3.testmy.net/dl-102400"), delegate(HTTPRequest req, HTTPResponse resp)
			{
				switch (req.State)
				{
				case HTTPRequestStates.Processing:
					if (!PlayerPrefs.HasKey("DownloadLength"))
					{
						string firstHeaderValue = resp.GetFirstHeaderValue("content-length");
						if (!string.IsNullOrEmpty(firstHeaderValue))
						{
							PlayerPrefs.SetInt("DownloadLength", int.Parse(firstHeaderValue));
						}
					}
					this.ProcessFragments(resp.GetStreamedFragments());
					this.status = "Processing";
					return;
				case HTTPRequestStates.Finished:
					if (!resp.IsSuccess)
					{
						this.status = string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText);
						Debug.LogWarning(this.status);
						this.request = null;
						return;
					}
					this.ProcessFragments(resp.GetStreamedFragments());
					if (resp.IsStreamingFinished)
					{
						this.status = "Streaming finished!";
						PlayerPrefs.DeleteKey("DownloadProgress");
						PlayerPrefs.Save();
						this.request = null;
						return;
					}
					this.status = "Processing";
					return;
				case HTTPRequestStates.Error:
					this.status = "Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
					Debug.LogError(this.status);
					this.request = null;
					return;
				case HTTPRequestStates.Aborted:
					this.status = "Request Aborted!";
					Debug.LogWarning(this.status);
					this.request = null;
					return;
				case HTTPRequestStates.ConnectionTimedOut:
					this.status = "Connection Timed Out!";
					Debug.LogError(this.status);
					this.request = null;
					return;
				case HTTPRequestStates.TimedOut:
					this.status = "Processing the request Timed Out!";
					Debug.LogError(this.status);
					this.request = null;
					return;
				default:
					return;
				}
			});
			if (PlayerPrefs.HasKey("DownloadProgress"))
			{
				this.request.SetRangeHeader(PlayerPrefs.GetInt("DownloadProgress"));
			}
			else
			{
				PlayerPrefs.SetInt("DownloadProgress", 0);
			}
			this.request.DisableCache = true;
			this.request.UseStreaming = true;
			this.request.StreamFragmentSize = this.fragmentSize;
			this.request.Send();
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00096E94 File Offset: 0x00095094
		private void ProcessFragments(List<byte[]> fragments)
		{
			if (fragments != null && fragments.Count > 0)
			{
				for (int i = 0; i < fragments.Count; i++)
				{
					int value = PlayerPrefs.GetInt("DownloadProgress") + fragments[i].Length;
					PlayerPrefs.SetInt("DownloadProgress", value);
				}
				PlayerPrefs.Save();
				this.progress = (float)PlayerPrefs.GetInt("DownloadProgress") / (float)PlayerPrefs.GetInt("DownloadLength");
			}
		}

		// Token: 0x04001371 RID: 4977
		private const string URL = "https://uk3.testmy.net/dl-102400";

		// Token: 0x04001372 RID: 4978
		private HTTPRequest request;

		// Token: 0x04001373 RID: 4979
		private string status = string.Empty;

		// Token: 0x04001374 RID: 4980
		private float progress;

		// Token: 0x04001375 RID: 4981
		private int fragmentSize = 4096;
	}
}
