using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Facebook.Unity.Example
{
	// Token: 0x02000146 RID: 326
	internal class AppRequests : MenuBase
	{
		// Token: 0x06000B28 RID: 2856 RVA: 0x0008A37C File Offset: 0x0008857C
		protected override void GetGui()
		{
			if (base.Button("Select - Filter None"))
			{
				FB.AppRequest("Test Message", null, null, null, null, "", "", new FacebookDelegate<IAppRequestResult>(base.HandleResult));
			}
			if (base.Button("Select - Filter app_users"))
			{
				List<object> filters = new List<object>
				{
					"app_users"
				};
				FB.AppRequest("Test Message", null, filters, null, new int?(0), string.Empty, string.Empty, new FacebookDelegate<IAppRequestResult>(base.HandleResult));
			}
			if (base.Button("Select - Filter app_non_users"))
			{
				List<object> filters2 = new List<object>
				{
					"app_non_users"
				};
				FB.AppRequest("Test Message", null, filters2, null, new int?(0), string.Empty, string.Empty, new FacebookDelegate<IAppRequestResult>(base.HandleResult));
			}
			base.LabelAndTextField("Message: ", ref this.requestMessage);
			base.LabelAndTextField("To (optional): ", ref this.requestTo);
			base.LabelAndTextField("Filter (optional): ", ref this.requestFilter);
			base.LabelAndTextField("Exclude Ids (optional): ", ref this.requestExcludes);
			base.LabelAndTextField("Filters: ", ref this.requestExcludes);
			base.LabelAndTextField("Max Recipients (optional): ", ref this.requestMax);
			base.LabelAndTextField("Data (optional): ", ref this.requestData);
			base.LabelAndTextField("Title (optional): ", ref this.requestTitle);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Request Action (optional): ", base.LabelStyle, new GUILayoutOption[]
			{
				GUILayout.MaxWidth(200f * base.ScaleFactor)
			});
			this.selectedAction = GUILayout.Toolbar(this.selectedAction, this.actionTypeStrings, base.ButtonStyle, new GUILayoutOption[]
			{
				GUILayout.MinHeight((float)ConsoleBase.ButtonHeight * base.ScaleFactor),
				GUILayout.MaxWidth((float)(ConsoleBase.MainWindowWidth - 150))
			});
			GUILayout.EndHorizontal();
			base.LabelAndTextField("Request Object ID (optional): ", ref this.requestObjectID);
			if (base.Button("Custom App Request"))
			{
				OGActionType? selectedOGActionType = this.GetSelectedOGActionType();
				if (selectedOGActionType != null)
				{
					FB.AppRequest(this.requestMessage, selectedOGActionType.Value, this.requestObjectID, string.IsNullOrEmpty(this.requestTo) ? null : this.requestTo.Split(new char[]
					{
						','
					}), this.requestData, this.requestTitle, new FacebookDelegate<IAppRequestResult>(base.HandleResult));
					return;
				}
				FB.AppRequest(this.requestMessage, string.IsNullOrEmpty(this.requestTo) ? null : this.requestTo.Split(new char[]
				{
					','
				}), string.IsNullOrEmpty(this.requestFilter) ? null : this.requestFilter.Split(new char[]
				{
					','
				}).OfType<object>().ToList<object>(), string.IsNullOrEmpty(this.requestExcludes) ? null : this.requestExcludes.Split(new char[]
				{
					','
				}), new int?(string.IsNullOrEmpty(this.requestMax) ? 0 : int.Parse(this.requestMax)), this.requestData, this.requestTitle, new FacebookDelegate<IAppRequestResult>(base.HandleResult));
			}
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0008A6A4 File Offset: 0x000888A4
		private OGActionType? GetSelectedOGActionType()
		{
			string a = this.actionTypeStrings[this.selectedAction];
			if (a == OGActionType.SEND.ToString())
			{
				return new OGActionType?(OGActionType.SEND);
			}
			if (a == OGActionType.ASKFOR.ToString())
			{
				return new OGActionType?(OGActionType.ASKFOR);
			}
			if (a == OGActionType.TURN.ToString())
			{
				return new OGActionType?(OGActionType.TURN);
			}
			return null;
		}

		// Token: 0x040011A2 RID: 4514
		private string requestMessage = string.Empty;

		// Token: 0x040011A3 RID: 4515
		private string requestTo = string.Empty;

		// Token: 0x040011A4 RID: 4516
		private string requestFilter = string.Empty;

		// Token: 0x040011A5 RID: 4517
		private string requestExcludes = string.Empty;

		// Token: 0x040011A6 RID: 4518
		private string requestMax = string.Empty;

		// Token: 0x040011A7 RID: 4519
		private string requestData = string.Empty;

		// Token: 0x040011A8 RID: 4520
		private string requestTitle = string.Empty;

		// Token: 0x040011A9 RID: 4521
		private string requestObjectID = string.Empty;

		// Token: 0x040011AA RID: 4522
		private int selectedAction;

		// Token: 0x040011AB RID: 4523
		private string[] actionTypeStrings = new string[]
		{
			"NONE",
			OGActionType.SEND.ToString(),
			OGActionType.ASKFOR.ToString(),
			OGActionType.TURN.ToString()
		};
	}
}
