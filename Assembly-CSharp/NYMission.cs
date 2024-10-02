using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class NYMission
{
	// Token: 0x0600051C RID: 1308 RVA: 0x0006018A File Offset: 0x0005E38A
	public NYMission(int id, Vector2 _center, Color _mainColor, float _size, AudioSource _AS, bool _finish = false, bool _force_color = false)
	{
		this.mainColor = _mainColor;
		this.force_color = _force_color;
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x000601AC File Offset: 0x0005E3AC
	public bool DrawMissionPoint(Vector2 mpos, bool selected)
	{
		bool result = false;
		if (this.MS == PERFORMANCE_STATUS.INACTIVE)
		{
			this.inactive.DrawButton(mpos);
		}
		else if (this.MS == PERFORMANCE_STATUS.PENDING)
		{
			if (this.force_color)
			{
				this.c = GUI.color;
				GUI.color = this.mainColor;
			}
			if (!this.inactiveCurrent.draw)
			{
				this.inactiveCurrent.GO();
			}
			else
			{
				this.inactiveCurrent.isSelected = selected;
				result = this.inactiveCurrent.DrawButton(mpos);
			}
			if (this.force_color)
			{
				GUI.color = this.c;
			}
		}
		else
		{
			this.c = GUI.color;
			GUI.color = this.mainColor;
			if (!this.complited.draw)
			{
				this.complited.GO();
			}
			else
			{
				this.complited.isSelected = selected;
				result = this.complited.DrawButton(mpos);
			}
			GUI.color = this.c;
		}
		return result;
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x0000248C File Offset: 0x0000068C
	public void DrawMissionText(Rect _r)
	{
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x0006029C File Offset: 0x0005E49C
	public void SetStats(string _stats)
	{
		string[] array = _stats.Split(new char[]
		{
			'|'
		});
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				int.TryParse(array[i], out this.TasksList[i + 1].current_value);
				this.TasksList[i + 1].offset_value = 0;
			}
		}
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x0000248C File Offset: 0x0000068C
	public void DropButtonsSize()
	{
	}

	// Token: 0x04000976 RID: 2422
	public PERFORMANCE_STATUS MS;

	// Token: 0x04000977 RID: 2423
	private int missionID;

	// Token: 0x04000978 RID: 2424
	private string Header;

	// Token: 0x04000979 RID: 2425
	private TweenButton inactive;

	// Token: 0x0400097A RID: 2426
	private TweenButton inactiveCurrent;

	// Token: 0x0400097B RID: 2427
	private TweenButton complited;

	// Token: 0x0400097C RID: 2428
	private Color mainColor;

	// Token: 0x0400097D RID: 2429
	private bool force_color;

	// Token: 0x0400097E RID: 2430
	public Dictionary<int, NYMissionTask> TasksList = new Dictionary<int, NYMissionTask>();

	// Token: 0x0400097F RID: 2431
	private Color c;
}
