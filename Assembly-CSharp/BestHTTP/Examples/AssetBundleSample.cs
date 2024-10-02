using System;
using System.Collections;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x02000193 RID: 403
	public sealed class AssetBundleSample : MonoBehaviour
	{
		// Token: 0x06000EA8 RID: 3752 RVA: 0x00096C67 File Offset: 0x00094E67
		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				GUILayout.Label("Status: " + this.status, Array.Empty<GUILayoutOption>());
				if (this.texture != null)
				{
					GUILayout.Box(this.texture, new GUILayoutOption[]
					{
						GUILayout.MaxHeight(256f)
					});
				}
				if (!this.downloading && GUILayout.Button("Start Download", Array.Empty<GUILayoutOption>()))
				{
					this.UnloadBundle();
					base.StartCoroutine(this.DownloadAssetBundle());
				}
			});
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00096C80 File Offset: 0x00094E80
		private void OnDestroy()
		{
			this.UnloadBundle();
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00096C88 File Offset: 0x00094E88
		private IEnumerator DownloadAssetBundle()
		{
			this.downloading = true;
			HTTPRequest request = new HTTPRequest(this.URI).Send();
			this.status = "Download started";
			while (request.State < HTTPRequestStates.Finished)
			{
				yield return new WaitForSeconds(0.1f);
				this.status += ".";
			}
			switch (request.State)
			{
			case HTTPRequestStates.Finished:
				if (request.Response.IsSuccess)
				{
					this.status = string.Format("AssetBundle downloaded! Loaded from local cache: {0}", request.Response.IsFromCache.ToString());
					AssetBundleCreateRequest async = AssetBundle.LoadFromMemoryAsync(request.Response.Data);
					yield return async;
					yield return base.StartCoroutine(this.ProcessAssetBundle(async.assetBundle));
					async = null;
				}
				else
				{
					this.status = string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", request.Response.StatusCode, request.Response.Message, request.Response.DataAsText);
					Debug.LogWarning(this.status);
				}
				break;
			case HTTPRequestStates.Error:
				this.status = "Request Finished with Error! " + ((request.Exception != null) ? (request.Exception.Message + "\n" + request.Exception.StackTrace) : "No Exception");
				Debug.LogError(this.status);
				break;
			case HTTPRequestStates.Aborted:
				this.status = "Request Aborted!";
				Debug.LogWarning(this.status);
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				this.status = "Connection Timed Out!";
				Debug.LogError(this.status);
				break;
			case HTTPRequestStates.TimedOut:
				this.status = "Processing the request Timed Out!";
				Debug.LogError(this.status);
				break;
			}
			this.downloading = false;
			yield break;
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00096C97 File Offset: 0x00094E97
		private IEnumerator ProcessAssetBundle(AssetBundle bundle)
		{
			if (bundle == null)
			{
				yield break;
			}
			this.cachedBundle = bundle;
			AssetBundleRequest asyncAsset = this.cachedBundle.LoadAssetAsync("9443182_orig", typeof(Texture2D));
			yield return asyncAsset;
			this.texture = (asyncAsset.asset as Texture2D);
			yield break;
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00096CAD File Offset: 0x00094EAD
		private void UnloadBundle()
		{
			if (this.cachedBundle != null)
			{
				this.cachedBundle.Unload(true);
				this.cachedBundle = null;
			}
		}

		// Token: 0x0400136C RID: 4972
		private Uri URI = new Uri(GUIHelper.BaseURL + "/AssetBundles/WebGL/demobundle.assetbundle");

		// Token: 0x0400136D RID: 4973
		private string status = "Waiting for user interaction";

		// Token: 0x0400136E RID: 4974
		private AssetBundle cachedBundle;

		// Token: 0x0400136F RID: 4975
		private Texture2D texture;

		// Token: 0x04001370 RID: 4976
		private bool downloading;
	}
}
