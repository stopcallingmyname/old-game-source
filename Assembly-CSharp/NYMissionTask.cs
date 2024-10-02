using System;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class NYMissionTask
{
	// Token: 0x06000519 RID: 1305 RVA: 0x0005FF50 File Offset: 0x0005E150
	public NYMissionTask(int id, string _task, int _target_value)
	{
		this.taskID = id;
		this.Task = _task;
		this.target_value = _target_value;
		this.offset_value = 0;
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x0005FF7C File Offset: 0x0005E17C
	public void DrawTask(Rect _r)
	{
		this.CheckBoxRect = new Rect(_r.x - 5f, _r.y - 2f, 20f, 20f);
		this.TaskRect = new Rect(_r.x + 15f, _r.y, _r.width - 50f, 20f);
		GUI.DrawTexture(this.CheckBoxRect, this.Background);
		if (this.current_value + this.offset_value >= this.target_value)
		{
			this.TS = PERFORMANCE_STATUS.COMPLITED;
		}
		if (this.TS == PERFORMANCE_STATUS.COMPLITED)
		{
			GUI.DrawTexture(this.CheckBoxRect, this.Check);
		}
		int fontSize = GUIManager.gs_style1.fontSize;
		TextAnchor alignment = GUIManager.gs_style1.alignment;
		GUIManager.gs_style1.alignment = TextAnchor.UpperLeft;
		GUIManager.gs_style1.fontSize = 14;
		GUIManager.gs_style1.richText = true;
		GUI.Label(this.TaskRect, string.Format(this.Task, this.target_value), GUIManager.gs_style1);
		GUIManager.gs_style1.richText = false;
		GUIManager.gs_style1.alignment = TextAnchor.UpperRight;
		if (this.target_value > 1)
		{
			Color color = GUI.color;
			GUI.color = Color.yellow;
			if (this.TS == PERFORMANCE_STATUS.COMPLITED)
			{
				GUI.color = Color.green;
				GUI.Label(this.TaskRect, this.target_value.ToString() + "/" + this.target_value.ToString(), GUIManager.gs_style1);
			}
			else
			{
				GUI.Label(this.TaskRect, (this.current_value + this.offset_value).ToString() + "/" + this.target_value.ToString(), GUIManager.gs_style1);
			}
			GUI.color = color;
		}
		GUIManager.gs_style1.fontSize = fontSize;
		GUIManager.gs_style1.alignment = alignment;
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x00060159 File Offset: 0x0005E359
	public void SetOffsetValue(int _value)
	{
		this.offset_value = _value;
		if (this.current_value + this.offset_value > this.target_value)
		{
			this.offset_value = this.target_value - this.current_value;
		}
	}

	// Token: 0x0400096C RID: 2412
	public PERFORMANCE_STATUS TS = PERFORMANCE_STATUS.PENDING;

	// Token: 0x0400096D RID: 2413
	private int taskID;

	// Token: 0x0400096E RID: 2414
	private string Task;

	// Token: 0x0400096F RID: 2415
	private Texture2D Background;

	// Token: 0x04000970 RID: 2416
	private Texture2D Check;

	// Token: 0x04000971 RID: 2417
	public int target_value;

	// Token: 0x04000972 RID: 2418
	public int current_value;

	// Token: 0x04000973 RID: 2419
	public int offset_value;

	// Token: 0x04000974 RID: 2420
	private Rect CheckBoxRect;

	// Token: 0x04000975 RID: 2421
	private Rect TaskRect;
}
