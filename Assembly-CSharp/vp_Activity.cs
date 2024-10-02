using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class vp_Activity : vp_Event
{
	// Token: 0x06000641 RID: 1601 RVA: 0x0000248C File Offset: 0x0000068C
	protected static void Empty()
	{
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0006AE98 File Offset: 0x00069098
	protected static bool AlwaysOK()
	{
		return true;
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x0006AEB1 File Offset: 0x000690B1
	public vp_Activity(string name) : base(name)
	{
		this.InitFields();
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000644 RID: 1604 RVA: 0x0006AED6 File Offset: 0x000690D6
	// (set) Token: 0x06000645 RID: 1605 RVA: 0x0006AEDE File Offset: 0x000690DE
	public float MinPause
	{
		get
		{
			return this.m_MinPause;
		}
		set
		{
			this.m_MinPause = Mathf.Max(0f, value);
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000646 RID: 1606 RVA: 0x0006AEF1 File Offset: 0x000690F1
	// (set) Token: 0x06000647 RID: 1607 RVA: 0x0006AEFC File Offset: 0x000690FC
	public float MinDuration
	{
		get
		{
			return this.m_MinDuration;
		}
		set
		{
			this.m_MinDuration = Mathf.Max(0.001f, value);
			if (this.m_MaxDuration == -1f)
			{
				return;
			}
			if (this.m_MinDuration > this.m_MaxDuration)
			{
				this.m_MinDuration = this.m_MaxDuration;
				Debug.LogWarning("Warning: (vp_Activity) Tried to set MinDuration longer than MaxDuration for '" + base.EventName + "'. Capping at MaxDuration.");
			}
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000648 RID: 1608 RVA: 0x0006AF5C File Offset: 0x0006915C
	// (set) Token: 0x06000649 RID: 1609 RVA: 0x0006AF64 File Offset: 0x00069164
	public float AutoDuration
	{
		get
		{
			return this.m_MaxDuration;
		}
		set
		{
			if (value == -1f)
			{
				this.m_MaxDuration = value;
				return;
			}
			this.m_MaxDuration = Mathf.Max(0.001f, value);
			if (this.m_MaxDuration < this.m_MinDuration)
			{
				this.m_MaxDuration = this.m_MinDuration;
				Debug.LogWarning("Warning: (vp_Activity) Tried to set MaxDuration shorter than MinDuration for '" + base.EventName + "'. Capping at MinDuration.");
			}
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x0600064A RID: 1610 RVA: 0x0006AFC8 File Offset: 0x000691C8
	// (set) Token: 0x0600064B RID: 1611 RVA: 0x0006B020 File Offset: 0x00069220
	public object Argument
	{
		get
		{
			if (this.m_ArgumentType == null)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Error: (",
					this,
					") Tried to fetch argument from '",
					base.EventName,
					"' but this activity takes no parameters."
				}));
				return null;
			}
			return this.m_Argument;
		}
		set
		{
			if (this.m_ArgumentType == null)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Error: (",
					this,
					") Tried to set argument for '",
					base.EventName,
					"' but this activity takes no parameters."
				}));
				return;
			}
			this.m_Argument = value;
		}
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x0006B078 File Offset: 0x00069278
	protected override void InitFields()
	{
		this.m_DelegateTypes = new Type[]
		{
			typeof(vp_Activity.Callback),
			typeof(vp_Activity.Callback),
			typeof(vp_Activity.Condition),
			typeof(vp_Activity.Condition),
			typeof(vp_Activity.Callback),
			typeof(vp_Activity.Callback)
		};
		this.m_Fields = new FieldInfo[]
		{
			base.Type.GetField("StartCallbacks"),
			base.Type.GetField("StopCallbacks"),
			base.Type.GetField("StartConditions"),
			base.Type.GetField("StopConditions"),
			base.Type.GetField("FailStartCallbacks"),
			base.Type.GetField("FailStopCallbacks")
		};
		base.StoreInvokerFieldNames();
		this.m_DefaultMethods = new MethodInfo[]
		{
			base.Type.GetMethod("Empty"),
			base.Type.GetMethod("Empty"),
			base.Type.GetMethod("AlwaysOK"),
			base.Type.GetMethod("AlwaysOK"),
			base.Type.GetMethod("Empty"),
			base.Type.GetMethod("Empty")
		};
		this.Prefixes = new Dictionary<string, int>
		{
			{
				"OnStart_",
				0
			},
			{
				"OnStop_",
				1
			},
			{
				"CanStart_",
				2
			},
			{
				"CanStop_",
				3
			},
			{
				"OnFailStart_",
				4
			},
			{
				"OnFailStop_",
				5
			}
		};
		this.StartCallbacks = new vp_Activity.Callback(vp_Activity.Empty);
		this.StopCallbacks = new vp_Activity.Callback(vp_Activity.Empty);
		this.StartConditions = new vp_Activity.Condition(vp_Activity.AlwaysOK);
		this.StopConditions = new vp_Activity.Condition(vp_Activity.AlwaysOK);
		this.FailStartCallbacks = new vp_Activity.Callback(vp_Activity.Empty);
		this.FailStopCallbacks = new vp_Activity.Callback(vp_Activity.Empty);
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x0006B2A0 File Offset: 0x000694A0
	public override void Register(object t, string m, int v)
	{
		base.AddExternalMethodToField(t, this.m_Fields[v], m, this.m_DelegateTypes[v]);
		base.Refresh();
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x0006B2C0 File Offset: 0x000694C0
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.RemoveExternalMethodFromField(t, this.m_Fields[1]);
		base.RemoveExternalMethodFromField(t, this.m_Fields[2]);
		base.RemoveExternalMethodFromField(t, this.m_Fields[3]);
		base.RemoveExternalMethodFromField(t, this.m_Fields[4]);
		base.RemoveExternalMethodFromField(t, this.m_Fields[5]);
		base.Refresh();
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x0006B330 File Offset: 0x00069530
	public bool TryStart(bool startIfAllowed = true)
	{
		if (this.m_Active)
		{
			return false;
		}
		if (Time.time < this.NextAllowedStartTime)
		{
			this.m_Argument = null;
			return false;
		}
		Delegate[] invocationList = this.StartConditions.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			if (!((vp_Activity.Condition)invocationList[i])())
			{
				this.m_Argument = null;
				if (startIfAllowed && this.FailStartCallbacks != null)
				{
					this.FailStartCallbacks();
				}
				return false;
			}
		}
		if (startIfAllowed)
		{
			this.Active = true;
		}
		return true;
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x0006B3B0 File Offset: 0x000695B0
	public bool TryStop(bool stopIfAllowed = true)
	{
		if (!this.m_Active)
		{
			return false;
		}
		if (Time.time < this.NextAllowedStopTime)
		{
			return false;
		}
		Delegate[] invocationList = this.StopConditions.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			if (!((vp_Activity.Condition)invocationList[i])())
			{
				if (stopIfAllowed && this.FailStopCallbacks != null)
				{
					this.FailStopCallbacks();
				}
				return false;
			}
		}
		if (stopIfAllowed)
		{
			this.Active = false;
		}
		return true;
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000652 RID: 1618 RVA: 0x0006B4D1 File Offset: 0x000696D1
	// (set) Token: 0x06000651 RID: 1617 RVA: 0x0006B424 File Offset: 0x00069624
	public bool Active
	{
		get
		{
			return this.m_Active;
		}
		set
		{
			if (value && !this.m_Active)
			{
				this.m_Active = true;
				if (this.StartCallbacks != null)
				{
					this.StartCallbacks();
				}
				this.NextAllowedStopTime = Time.time + this.m_MinDuration;
				if (this.m_MaxDuration > 0f)
				{
					vp_Timer.In(this.m_MaxDuration, delegate()
					{
						this.Stop(0f);
					}, this.m_ForceStopTimer);
					return;
				}
			}
			else if (!value && this.m_Active)
			{
				this.m_Active = false;
				if (this.StopCallbacks != null)
				{
					this.StopCallbacks();
				}
				this.NextAllowedStartTime = Time.time + this.m_MinPause;
				this.m_Argument = null;
			}
		}
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x0006B4D9 File Offset: 0x000696D9
	public void Start(float forcedActiveDuration = 0f)
	{
		this.Active = true;
		if (forcedActiveDuration > 0f)
		{
			this.NextAllowedStopTime = Time.time + forcedActiveDuration;
		}
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x0006B4F7 File Offset: 0x000696F7
	public void Stop(float forcedPauseDuration = 0f)
	{
		this.Active = false;
		if (forcedPauseDuration > 0f)
		{
			this.NextAllowedStartTime = Time.time + forcedPauseDuration;
		}
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x0006B515 File Offset: 0x00069715
	public void Disallow(float duration)
	{
		this.NextAllowedStartTime = Time.time + duration;
	}

	// Token: 0x04000C73 RID: 3187
	public vp_Activity.Callback StartCallbacks;

	// Token: 0x04000C74 RID: 3188
	public vp_Activity.Callback StopCallbacks;

	// Token: 0x04000C75 RID: 3189
	public vp_Activity.Condition StartConditions;

	// Token: 0x04000C76 RID: 3190
	public vp_Activity.Condition StopConditions;

	// Token: 0x04000C77 RID: 3191
	public vp_Activity.Callback FailStartCallbacks;

	// Token: 0x04000C78 RID: 3192
	public vp_Activity.Callback FailStopCallbacks;

	// Token: 0x04000C79 RID: 3193
	protected vp_Timer.Handle m_ForceStopTimer = new vp_Timer.Handle();

	// Token: 0x04000C7A RID: 3194
	protected object m_Argument;

	// Token: 0x04000C7B RID: 3195
	protected bool m_Active;

	// Token: 0x04000C7C RID: 3196
	public float NextAllowedStartTime;

	// Token: 0x04000C7D RID: 3197
	public float NextAllowedStopTime;

	// Token: 0x04000C7E RID: 3198
	private float m_MinPause;

	// Token: 0x04000C7F RID: 3199
	private float m_MinDuration;

	// Token: 0x04000C80 RID: 3200
	private float m_MaxDuration = -1f;

	// Token: 0x0200089C RID: 2204
	// (Invoke) Token: 0x06004CBE RID: 19646
	public delegate void Callback();

	// Token: 0x0200089D RID: 2205
	// (Invoke) Token: 0x06004CC2 RID: 19650
	public delegate bool Condition();
}
