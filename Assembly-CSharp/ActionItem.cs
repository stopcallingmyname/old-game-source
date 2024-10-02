using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class ActionItem
{
	// Token: 0x0600005C RID: 92 RVA: 0x0000477E File Offset: 0x0000297E
	public ActionItem(int _ItemID, int _ActionID)
	{
		this.ItemID = _ItemID;
		this.ActionID = _ActionID;
		this.ActionText = ContentLoader.LoadTexture("ActionOffer" + this.ActionID.ToString());
	}

	// Token: 0x04000055 RID: 85
	public int ItemID;

	// Token: 0x04000056 RID: 86
	public int ActionID;

	// Token: 0x04000057 RID: 87
	public Texture2D ActionText;
}
