using System;
using UnityEngine;

namespace Facebook.Unity.Example
{
	// Token: 0x02000147 RID: 327
	internal class DialogShare : MenuBase
	{
		// Token: 0x06000B2B RID: 2859 RVA: 0x0007D96F File Offset: 0x0007BB6F
		protected override bool ShowDialogModeSelector()
		{
			return false;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0008A7DC File Offset: 0x000889DC
		protected override void GetGui()
		{
			bool enabled = GUI.enabled;
			if (base.Button("Share - Link"))
			{
				FB.ShareLink(new Uri("https://developers.facebook.com/"), "", "", null, new FacebookDelegate<IShareResult>(base.HandleResult));
			}
			if (base.Button("Share - Link Photo"))
			{
				FB.ShareLink(new Uri("https://developers.facebook.com/"), "Link Share", "Look I'm sharing a link", new Uri("http://i.imgur.com/j4M7vCO.jpg"), new FacebookDelegate<IShareResult>(base.HandleResult));
			}
			base.LabelAndTextField("Link", ref this.shareLink);
			base.LabelAndTextField("Title", ref this.shareTitle);
			base.LabelAndTextField("Description", ref this.shareDescription);
			base.LabelAndTextField("Image", ref this.shareImage);
			if (base.Button("Share - Custom"))
			{
				FB.ShareLink(new Uri(this.shareLink), this.shareTitle, this.shareDescription, new Uri(this.shareImage), new FacebookDelegate<IShareResult>(base.HandleResult));
			}
			GUI.enabled = (enabled && (!Constants.IsEditor || (Constants.IsEditor && FB.IsLoggedIn)));
			if (base.Button("Feed Share - No To"))
			{
				FB.FeedShare(string.Empty, new Uri("https://developers.facebook.com/"), "Test Title", "Test caption", "Test Description", new Uri("http://i.imgur.com/zkYlB.jpg"), string.Empty, new FacebookDelegate<IShareResult>(base.HandleResult));
			}
			base.LabelAndTextField("To", ref this.feedTo);
			base.LabelAndTextField("Link", ref this.feedLink);
			base.LabelAndTextField("Title", ref this.feedTitle);
			base.LabelAndTextField("Caption", ref this.feedCaption);
			base.LabelAndTextField("Description", ref this.feedDescription);
			base.LabelAndTextField("Image", ref this.feedImage);
			base.LabelAndTextField("Media Source", ref this.feedMediaSource);
			if (base.Button("Feed Share - Custom"))
			{
				FB.FeedShare(this.feedTo, string.IsNullOrEmpty(this.feedLink) ? null : new Uri(this.feedLink), this.feedTitle, this.feedCaption, this.feedDescription, string.IsNullOrEmpty(this.feedImage) ? null : new Uri(this.feedImage), this.feedMediaSource, new FacebookDelegate<IShareResult>(base.HandleResult));
			}
			GUI.enabled = enabled;
		}

		// Token: 0x040011AC RID: 4524
		private string shareLink = "https://developers.facebook.com/";

		// Token: 0x040011AD RID: 4525
		private string shareTitle = "Link Title";

		// Token: 0x040011AE RID: 4526
		private string shareDescription = "Link Description";

		// Token: 0x040011AF RID: 4527
		private string shareImage = "http://i.imgur.com/j4M7vCO.jpg";

		// Token: 0x040011B0 RID: 4528
		private string feedTo = string.Empty;

		// Token: 0x040011B1 RID: 4529
		private string feedLink = "https://developers.facebook.com/";

		// Token: 0x040011B2 RID: 4530
		private string feedTitle = "Test Title";

		// Token: 0x040011B3 RID: 4531
		private string feedCaption = "Test Caption";

		// Token: 0x040011B4 RID: 4532
		private string feedDescription = "Test Description";

		// Token: 0x040011B5 RID: 4533
		private string feedImage = "http://i.imgur.com/zkYlB.jpg";

		// Token: 0x040011B6 RID: 4534
		private string feedMediaSource = string.Empty;
	}
}
