using System;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x02000195 RID: 405
	public sealed class TextureDownloadSample : MonoBehaviour
	{
		// Token: 0x06000EB7 RID: 3767 RVA: 0x00097208 File Offset: 0x00095408
		private void Awake()
		{
			HTTPManager.MaxConnectionPerServer = 1;
			for (int i = 0; i < this.Images.Length; i++)
			{
				this.Textures[i] = new Texture2D(100, 150);
			}
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00097242 File Offset: 0x00095442
		private void OnDestroy()
		{
			HTTPManager.MaxConnectionPerServer = 4;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0009724A File Offset: 0x0009544A
		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, Array.Empty<GUILayoutOption>());
				int selected = 0;
				Texture[] textures = this.Textures;
				GUILayout.SelectionGrid(selected, textures, 3, Array.Empty<GUILayoutOption>());
				if (this.finishedCount == this.Images.Length && this.allDownloadedFromLocalCache)
				{
					GUIHelper.DrawCenteredText("All images loaded from the local cache!");
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Connection/Server: ", new GUILayoutOption[]
				{
					GUILayout.Width(150f)
				});
				GUILayout.Label(HTTPManager.MaxConnectionPerServer.ToString(), new GUILayoutOption[]
				{
					GUILayout.Width(20f)
				});
				HTTPManager.MaxConnectionPerServer = (byte)GUILayout.HorizontalSlider((float)HTTPManager.MaxConnectionPerServer, 1f, 10f, Array.Empty<GUILayoutOption>());
				GUILayout.EndHorizontal();
				if (GUILayout.Button("Start Download", Array.Empty<GUILayoutOption>()))
				{
					this.DownloadImages();
				}
				GUILayout.EndScrollView();
			});
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x00097264 File Offset: 0x00095464
		private void DownloadImages()
		{
			this.allDownloadedFromLocalCache = true;
			this.finishedCount = 0;
			for (int i = 0; i < this.Images.Length; i++)
			{
				this.Textures[i] = new Texture2D(100, 150);
				new HTTPRequest(new Uri(this.BaseURL + this.Images[i]), new OnRequestFinishedDelegate(this.ImageDownloaded))
				{
					Tag = this.Textures[i]
				}.Send();
			}
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x000972E4 File Offset: 0x000954E4
		private void ImageDownloaded(HTTPRequest req, HTTPResponse resp)
		{
			this.finishedCount++;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess)
				{
					(req.Tag as Texture2D).LoadImage(resp.Data);
					this.allDownloadedFromLocalCache = (this.allDownloadedFromLocalCache && resp.IsFromCache);
					return;
				}
				Debug.LogWarning(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText));
				return;
			case HTTPRequestStates.Error:
				Debug.LogError("Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
				return;
			case HTTPRequestStates.Aborted:
				Debug.LogWarning("Request Aborted!");
				return;
			case HTTPRequestStates.ConnectionTimedOut:
				Debug.LogError("Connection Timed Out!");
				return;
			case HTTPRequestStates.TimedOut:
				Debug.LogError("Processing the request Timed Out!");
				return;
			default:
				return;
			}
		}

		// Token: 0x04001376 RID: 4982
		private string BaseURL = GUIHelper.BaseURL + "/images/Demo/";

		// Token: 0x04001377 RID: 4983
		private string[] Images = new string[]
		{
			"One.png",
			"Two.png",
			"Three.png",
			"Four.png",
			"Five.png",
			"Six.png",
			"Seven.png",
			"Eight.png",
			"Nine.png"
		};

		// Token: 0x04001378 RID: 4984
		private Texture2D[] Textures = new Texture2D[9];

		// Token: 0x04001379 RID: 4985
		private bool allDownloadedFromLocalCache;

		// Token: 0x0400137A RID: 4986
		private int finishedCount;

		// Token: 0x0400137B RID: 4987
		private Vector2 scrollPos;
	}
}
