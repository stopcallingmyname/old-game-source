using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C7 RID: 199
public abstract class vp_StateEventHandler : vp_EventHandler
{
	// Token: 0x06000696 RID: 1686 RVA: 0x0006CE6C File Offset: 0x0006B06C
	protected override void Awake()
	{
		base.Awake();
		this.StoreStateTargets();
		this.StoreActivities();
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x0006CE80 File Offset: 0x0006B080
	protected void BindStateToActivity(vp_Activity a)
	{
		this.BindStateToActivityOnStart(a);
		this.BindStateToActivityOnStop(a);
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x0006CE90 File Offset: 0x0006B090
	protected void BindStateToActivityOnStart(vp_Activity a)
	{
		if (!this.ActivityInitialized(a))
		{
			return;
		}
		string s = a.EventName;
		a.StartCallbacks = (vp_Activity.Callback)Delegate.Combine(a.StartCallbacks, new vp_Activity.Callback(delegate()
		{
			foreach (vp_Component vp_Component in this.m_StateTargets)
			{
				if (vp_Component.gameObject.activeSelf)
				{
					vp_Component.SetState(s, true, true, false);
				}
			}
		}));
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x0006CEE4 File Offset: 0x0006B0E4
	protected void BindStateToActivityOnStop(vp_Activity a)
	{
		if (!this.ActivityInitialized(a))
		{
			return;
		}
		string s = a.EventName;
		a.StopCallbacks = (vp_Activity.Callback)Delegate.Combine(a.StopCallbacks, new vp_Activity.Callback(delegate()
		{
			foreach (vp_Component vp_Component in this.m_StateTargets)
			{
				if (vp_Component.gameObject.activeSelf)
				{
					vp_Component.SetState(s, false, true, false);
				}
			}
		}));
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x0006CF38 File Offset: 0x0006B138
	protected void StoreStateTargets()
	{
		foreach (vp_Component vp_Component in base.transform.root.GetComponentsInChildren<vp_Component>())
		{
			if (vp_Component.Parent == null || vp_Component.Parent.GetComponent<vp_Component>() == null)
			{
				this.m_StateTargets.Add(vp_Component);
			}
		}
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x0006CF98 File Offset: 0x0006B198
	protected void StoreActivities()
	{
		for (int i = 0; i < this.m_Events.Count; i++)
		{
			if (this.m_Events[i] is vp_Activity || this.m_Events[i].Type.BaseType == typeof(vp_Activity))
			{
				this.m_Activities.Add(this.m_Events[i] as vp_Activity);
			}
		}
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x0006D014 File Offset: 0x0006B214
	public void RefreshActivityStates()
	{
		foreach (vp_Activity vp_Activity in this.m_Activities)
		{
			if (vp_Activity != null)
			{
				this.alreadyRecursedTargets.Clear();
				foreach (vp_Component vp_Component in this.m_StateTargets)
				{
					if (vp_Component.gameObject.activeSelf)
					{
						bool flag = this.alreadyRecursedTargets.Contains(vp_Component.Transform);
						if (!flag)
						{
							vp_Component.SetState(vp_Activity.EventName, vp_Activity.Active, !flag, false);
							this.alreadyRecursedTargets.Add(vp_Component.Transform);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x0006D100 File Offset: 0x0006B300
	public void ResetActivityStates()
	{
		foreach (vp_Component vp_Component in this.m_StateTargets)
		{
			if (vp_Component.gameObject.activeSelf)
			{
				vp_Component.ResetState();
			}
		}
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x0006D160 File Offset: 0x0006B360
	public void SetState(string state, bool setActive = true, bool recursive = true, bool includeDisabled = false)
	{
		foreach (vp_Component vp_Component in this.m_StateTargets)
		{
			if (vp_Component.gameObject.activeSelf)
			{
				vp_Component.SetState(state, setActive, recursive, includeDisabled);
			}
		}
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x0006D1C4 File Offset: 0x0006B3C4
	private bool ActivityInitialized(vp_Activity a)
	{
		if (a == null)
		{
			Debug.LogError("Error: (" + this + ") Activity is null.");
			return false;
		}
		if (string.IsNullOrEmpty(a.EventName))
		{
			Debug.LogError("Error: (" + this + ") Activity not initialized. Make sure the event handler has run its Awake call before binding layers.");
			return false;
		}
		return true;
	}

	// Token: 0x04000C95 RID: 3221
	private List<vp_Component> m_StateTargets = new List<vp_Component>();

	// Token: 0x04000C96 RID: 3222
	private List<vp_Activity> m_Activities = new List<vp_Activity>();

	// Token: 0x04000C97 RID: 3223
	private List<Transform> alreadyRecursedTargets = new List<Transform>();
}
