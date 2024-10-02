using System;
using UnityEngine;

// Token: 0x0200009B RID: 155
public class VDMissionTask
{
	// Token: 0x0600052D RID: 1325 RVA: 0x00061004 File Offset: 0x0005F204
	public VDMissionTask(int id, string _task, int _target_value)
	{
		this.taskID = id;
		this.Task = _task;
		this.target_value = _target_value;
		this.offset_value = 0;
		this.Background = Resources.Load<Texture2D>("VDQuest/Ramka");
		this.Check = Resources.Load<Texture2D>("VDQuest/Check");
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x0006105C File Offset: 0x0005F25C
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

	// Token: 0x0600052F RID: 1327 RVA: 0x00061239 File Offset: 0x0005F439
	public void SetOffsetValue(int _value)
	{
		this.offset_value = _value;
		if (this.current_value + this.offset_value > this.target_value)
		{
			this.offset_value = this.target_value - this.current_value;
		}
	}

	// Token: 0x04000994 RID: 2452
	public PERFORMANCE_STATUS TS = PERFORMANCE_STATUS.PENDING;

	// Token: 0x04000995 RID: 2453
	private int taskID;

	// Token: 0x04000996 RID: 2454
	private string Task;

	// Token: 0x04000997 RID: 2455
	private Texture2D Background;

	// Token: 0x04000998 RID: 2456
	private Texture2D Check;

	// Token: 0x04000999 RID: 2457
	public int target_value;

	// Token: 0x0400099A RID: 2458
	public int current_value;

	// Token: 0x0400099B RID: 2459
	public int offset_value;

	// Token: 0x0400099C RID: 2460
	private Rect CheckBoxRect;

	// Token: 0x0400099D RID: 2461
	private Rect TaskRect;
}
