using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BB RID: 187
[Serializable]
public class vp_State
{
	// Token: 0x06000630 RID: 1584 RVA: 0x0006A98F File Offset: 0x00068B8F
	public vp_State(string typeName, string name = "Untitled", string path = null, TextAsset asset = null)
	{
		this.TypeName = typeName;
		this.Name = name;
		this.TextAsset = asset;
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000631 RID: 1585 RVA: 0x0006A9AD File Offset: 0x00068BAD
	// (set) Token: 0x06000632 RID: 1586 RVA: 0x0006A9B5 File Offset: 0x00068BB5
	public bool Enabled
	{
		get
		{
			return this.m_Enabled;
		}
		set
		{
			this.m_Enabled = value;
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.StateManager == null)
			{
				return;
			}
			if (this.m_Enabled)
			{
				this.StateManager.ImposeBlockingList(this);
				return;
			}
			this.StateManager.RelaxBlockingList(this);
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000633 RID: 1587 RVA: 0x0006A9F0 File Offset: 0x00068BF0
	public bool Blocked
	{
		get
		{
			return this.CurrentlyBlockedBy.Count > 0;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000634 RID: 1588 RVA: 0x0006AA00 File Offset: 0x00068C00
	public int BlockCount
	{
		get
		{
			return this.CurrentlyBlockedBy.Count;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000635 RID: 1589 RVA: 0x0006AA0D File Offset: 0x00068C0D
	protected List<vp_State> CurrentlyBlockedBy
	{
		get
		{
			if (this.m_CurrentlyBlockedBy == null)
			{
				this.m_CurrentlyBlockedBy = new List<vp_State>();
			}
			return this.m_CurrentlyBlockedBy;
		}
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x0006AA28 File Offset: 0x00068C28
	public void AddBlocker(vp_State blocker)
	{
		if (!this.CurrentlyBlockedBy.Contains(blocker))
		{
			this.CurrentlyBlockedBy.Add(blocker);
		}
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x0006AA44 File Offset: 0x00068C44
	public void RemoveBlocker(vp_State blocker)
	{
		if (this.CurrentlyBlockedBy.Contains(blocker))
		{
			this.CurrentlyBlockedBy.Remove(blocker);
		}
	}

	// Token: 0x04000C64 RID: 3172
	public vp_StateManager StateManager;

	// Token: 0x04000C65 RID: 3173
	public string TypeName;

	// Token: 0x04000C66 RID: 3174
	public string Name;

	// Token: 0x04000C67 RID: 3175
	public TextAsset TextAsset;

	// Token: 0x04000C68 RID: 3176
	public vp_ComponentPreset Preset;

	// Token: 0x04000C69 RID: 3177
	public List<int> StatesToBlock;

	// Token: 0x04000C6A RID: 3178
	[NonSerialized]
	protected bool m_Enabled;

	// Token: 0x04000C6B RID: 3179
	[NonSerialized]
	protected List<vp_State> m_CurrentlyBlockedBy;
}
