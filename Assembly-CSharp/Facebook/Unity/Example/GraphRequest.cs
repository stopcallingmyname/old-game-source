using System;
using System.Collections;
using UnityEngine;

namespace Facebook.Unity.Example
{
	// Token: 0x02000148 RID: 328
	internal class GraphRequest : MenuBase
	{
		// Token: 0x06000B2E RID: 2862 RVA: 0x0008AACC File Offset: 0x00088CCC
		protected override void GetGui()
		{
			bool enabled = GUI.enabled;
			GUI.enabled = (enabled && FB.IsLoggedIn);
			if (base.Button("Basic Request - Me"))
			{
				FB.API("/me", HttpMethod.GET, new FacebookDelegate<IGraphResult>(base.HandleResult), null);
			}
			if (base.Button("Retrieve Profile Photo"))
			{
				FB.API("/me/picture", HttpMethod.GET, new FacebookDelegate<IGraphResult>(this.ProfilePhotoCallback), null);
			}
			if (base.Button("Take and Upload screenshot"))
			{
				base.StartCoroutine(this.TakeScreenshot());
			}
			base.LabelAndTextField("Request", ref this.apiQuery);
			if (base.Button("Custom Request"))
			{
				FB.API(this.apiQuery, HttpMethod.GET, new FacebookDelegate<IGraphResult>(base.HandleResult), null);
			}
			if (this.profilePic != null)
			{
				GUILayout.Box(this.profilePic, Array.Empty<GUILayoutOption>());
			}
			GUI.enabled = enabled;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0008ABAC File Offset: 0x00088DAC
		private void ProfilePhotoCallback(IGraphResult result)
		{
			if (string.IsNullOrEmpty(result.Error) && result.Texture != null)
			{
				this.profilePic = result.Texture;
			}
			base.HandleResult(result);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0008ABDC File Offset: 0x00088DDC
		private IEnumerator TakeScreenshot()
		{
			yield return new WaitForEndOfFrame();
			int width = Screen.width;
			int height = Screen.height;
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, false);
			texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
			texture2D.Apply();
			byte[] contents = texture2D.EncodeToPNG();
			WWWForm wwwform = new WWWForm();
			wwwform.AddBinaryData("image", contents, "InteractiveConsole.png");
			wwwform.AddField("message", "herp derp.  I did a thing!  Did I do this right?");
			FB.API("me/photos", HttpMethod.POST, new FacebookDelegate<IGraphResult>(base.HandleResult), wwwform);
			yield break;
		}

		// Token: 0x040011B7 RID: 4535
		private string apiQuery = string.Empty;

		// Token: 0x040011B8 RID: 4536
		private Texture2D profilePic;
	}
}
