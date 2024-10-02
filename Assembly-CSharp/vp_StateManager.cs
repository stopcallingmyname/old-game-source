using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BC RID: 188
public class vp_StateManager
{
	// Token: 0x06000638 RID: 1592 RVA: 0x0006AA64 File Offset: 0x00068C64
	public vp_StateManager(vp_Component component, List<vp_State> states)
	{
		this.m_States = states;
		this.m_Component = component;
		this.m_Component.RefreshDefaultState();
		this.m_StateIds = new Dictionary<string, int>(StringComparer.CurrentCulture);
		foreach (vp_State vp_State in this.m_States)
		{
			vp_State.StateManager = this;
			if (!this.m_StateIds.ContainsKey(vp_State.Name))
			{
				this.m_StateIds.Add(vp_State.Name, this.m_States.IndexOf(vp_State));
			}
			else
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"Warning: ",
					this.m_Component.GetType(),
					" on '",
					this.m_Component.name,
					"' has more than one state named: '",
					vp_State.Name,
					"'. Only the topmost one will be used."
				}));
				this.m_States[this.m_DefaultId].StatesToBlock.Add(this.m_States.IndexOf(vp_State));
			}
			if (vp_State.Preset == null)
			{
				vp_State.Preset = new vp_ComponentPreset();
			}
			if (vp_State.TextAsset != null)
			{
				vp_State.Preset.LoadFromTextAsset(vp_State.TextAsset);
			}
		}
		this.m_DefaultId = this.m_States.Count - 1;
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x0006ABEC File Offset: 0x00068DEC
	public void ImposeBlockingList(vp_State blocker)
	{
		foreach (int index in blocker.StatesToBlock)
		{
			this.m_States[index].AddBlocker(blocker);
		}
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x0006AC4C File Offset: 0x00068E4C
	public void RelaxBlockingList(vp_State blocker)
	{
		foreach (int index in blocker.StatesToBlock)
		{
			this.m_States[index].RemoveBlocker(blocker);
		}
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0006ACAC File Offset: 0x00068EAC
	public void SetState(string state, bool setEnabled = true)
	{
		if (!vp_StateManager.AppPlaying())
		{
			return;
		}
		if (!this.m_StateIds.TryGetValue(state, out this.m_TargetId))
		{
			return;
		}
		if (this.m_TargetId == this.m_DefaultId && !setEnabled)
		{
			Debug.LogWarning(vp_StateManager.m_DefaultStateNoDisableMessage);
			return;
		}
		this.m_States[this.m_TargetId].Enabled = setEnabled;
		this.CombineStates();
		this.m_Component.Refresh();
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0006AD1C File Offset: 0x00068F1C
	public void Reset()
	{
		if (!vp_StateManager.AppPlaying())
		{
			return;
		}
		foreach (vp_State vp_State in this.m_States)
		{
			vp_State.Enabled = false;
		}
		this.m_States[this.m_DefaultId].Enabled = true;
		this.m_TargetId = this.m_DefaultId;
		this.CombineStates();
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x0006ADA0 File Offset: 0x00068FA0
	public void CombineStates()
	{
		for (int i = this.m_States.Count - 1; i > -1; i--)
		{
			if ((i == this.m_DefaultId || (this.m_States[i].Enabled && !this.m_States[i].Blocked && !(this.m_States[i].TextAsset == null))) && this.m_States[i].Preset != null && !(this.m_States[i].Preset.ComponentType == null))
			{
				vp_ComponentPreset.Apply(this.m_Component, this.m_States[i].Preset);
			}
		}
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x0006AE61 File Offset: 0x00069061
	public bool IsEnabled(string state)
	{
		return vp_StateManager.AppPlaying() && this.m_StateIds.TryGetValue(state, out this.m_TargetId) && this.m_States[this.m_TargetId].Enabled;
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x0006AE98 File Offset: 0x00069098
	private static bool AppPlaying()
	{
		return true;
	}

	// Token: 0x04000C6C RID: 3180
	private vp_Component m_Component;

	// Token: 0x04000C6D RID: 3181
	[NonSerialized]
	private List<vp_State> m_States;

	// Token: 0x04000C6E RID: 3182
	private Dictionary<string, int> m_StateIds;

	// Token: 0x04000C6F RID: 3183
	private static string m_AppNotPlayingMessage = "Error: StateManager can only be accessed while application is playing.";

	// Token: 0x04000C70 RID: 3184
	private static string m_DefaultStateNoDisableMessage = "Warning: The 'Default' state cannot be disabled.";

	// Token: 0x04000C71 RID: 3185
	private int m_DefaultId;

	// Token: 0x04000C72 RID: 3186
	private int m_TargetId;
}
