using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x020000B9 RID: 185
public class vp_Component : MonoBehaviour
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x060005E8 RID: 1512 RVA: 0x00068BC1 File Offset: 0x00066DC1
	public vp_StateManager StateManager
	{
		get
		{
			return this.m_StateManager;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x060005E9 RID: 1513 RVA: 0x00068BC9 File Offset: 0x00066DC9
	public vp_State DefaultState
	{
		get
		{
			return this.m_DefaultState;
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x060005EA RID: 1514 RVA: 0x00068BD1 File Offset: 0x00066DD1
	public float Delta
	{
		get
		{
			return Time.deltaTime * 60f;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x060005EB RID: 1515 RVA: 0x00068BDE File Offset: 0x00066DDE
	public float SDelta
	{
		get
		{
			return Time.smoothDeltaTime * 60f;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060005EC RID: 1516 RVA: 0x00068BEB File Offset: 0x00066DEB
	public Transform Transform
	{
		get
		{
			return this.m_Transform;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x060005ED RID: 1517 RVA: 0x00068BF3 File Offset: 0x00066DF3
	public Transform Parent
	{
		get
		{
			return this.m_Parent;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060005EE RID: 1518 RVA: 0x00068BFB File Offset: 0x00066DFB
	public Transform Root
	{
		get
		{
			return this.m_Root;
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060005EF RID: 1519 RVA: 0x00068C03 File Offset: 0x00066E03
	public AudioSource Audio
	{
		get
		{
			return this.m_Audio;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x060005F0 RID: 1520 RVA: 0x00068C0B File Offset: 0x00066E0B
	// (set) Token: 0x060005F1 RID: 1521 RVA: 0x00068C30 File Offset: 0x00066E30
	public bool Rendering
	{
		get
		{
			return this.Renderers.Count > 0 && this.Renderers[0].enabled;
		}
		set
		{
			foreach (Renderer renderer in this.Renderers)
			{
				if (!(renderer == null))
				{
					renderer.enabled = value;
				}
			}
		}
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x00068C8C File Offset: 0x00066E8C
	protected virtual void Awake()
	{
		this.m_Transform = base.transform;
		this.m_Parent = base.transform.parent;
		this.m_Root = base.transform.root;
		this.m_Audio = base.GetComponent<AudioSource>();
		this.EventHandler = (vp_EventHandler)this.m_Transform.root.GetComponentInChildren(typeof(vp_EventHandler));
		this.CacheChildren();
		this.CacheSiblings();
		this.CacheFamily();
		this.CacheRenderers();
		this.CacheAudioSources();
		this.m_StateManager = new vp_StateManager(this, this.States);
		this.StateManager.SetState("Default", base.enabled);
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x00068D3E File Offset: 0x00066F3E
	protected virtual void Start()
	{
		this.ResetState();
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0000248C File Offset: 0x0000068C
	protected virtual void Init()
	{
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x00068D46 File Offset: 0x00066F46
	protected virtual void OnEnable()
	{
		if (this.EventHandler != null)
		{
			this.EventHandler.Register(this);
		}
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x00068D62 File Offset: 0x00066F62
	protected virtual void OnDisable()
	{
		if (this.EventHandler != null)
		{
			this.EventHandler.Unregister(this);
		}
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x00068D7E File Offset: 0x00066F7E
	protected virtual void Update()
	{
		if (!this.m_Initialized)
		{
			this.Init();
			this.m_Initialized = true;
		}
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0000248C File Offset: 0x0000068C
	protected virtual void FixedUpdate()
	{
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0000248C File Offset: 0x0000068C
	protected virtual void LateUpdate()
	{
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x00068D98 File Offset: 0x00066F98
	public void SetState(string state, bool enabled = true, bool recursive = false, bool includeDisabled = false)
	{
		this.m_StateManager.SetState(state, enabled);
		if (recursive)
		{
			foreach (vp_Component vp_Component in this.Children)
			{
				if (includeDisabled || (vp_Utility.IsActive(vp_Component.gameObject) && vp_Component.enabled))
				{
					vp_Component.SetState(state, enabled, true, includeDisabled);
				}
			}
		}
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x00068E18 File Offset: 0x00067018
	public void ActivateGameObject(bool setActive = true)
	{
		if (setActive)
		{
			this.Activate();
			using (List<vp_Component>.Enumerator enumerator = this.Siblings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					vp_Component vp_Component = enumerator.Current;
					vp_Component.Activate();
				}
				return;
			}
		}
		this.DeactivateWhenSilent();
		foreach (vp_Component vp_Component2 in this.Siblings)
		{
			vp_Component2.DeactivateWhenSilent();
		}
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x00068EB8 File Offset: 0x000670B8
	public void ResetState()
	{
		this.m_StateManager.Reset();
		this.Refresh();
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x00068ECB File Offset: 0x000670CB
	public bool StateEnabled(string stateName)
	{
		return this.m_StateManager.IsEnabled(stateName);
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x00068EDC File Offset: 0x000670DC
	public void RefreshDefaultState()
	{
		vp_State vp_State = null;
		if (this.States.Count == 0)
		{
			vp_State = new vp_State(Assembly.Load("Assembly-CSharp").GetType().Name, "Default", null, null);
			this.States.Add(vp_State);
		}
		else
		{
			for (int i = this.States.Count - 1; i > -1; i--)
			{
				if (this.States[i].Name == "Default")
				{
					vp_State = this.States[i];
					this.States.Remove(vp_State);
					this.States.Add(vp_State);
				}
			}
			if (vp_State == null)
			{
				vp_State = new vp_State(Assembly.Load("Assembly-CSharp").GetType().Name, "Default", null, null);
				this.States.Add(vp_State);
			}
		}
		if (vp_State.Preset == null || vp_State.Preset.ComponentType == null)
		{
			vp_State.Preset = new vp_ComponentPreset();
		}
		if (vp_State.TextAsset == null)
		{
			vp_State.Preset.InitFromComponent(this);
		}
		vp_State.Enabled = true;
		this.m_DefaultState = vp_State;
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x00069003 File Offset: 0x00067203
	public void ApplyPreset(vp_ComponentPreset preset)
	{
		vp_ComponentPreset.Apply(this, preset);
		this.RefreshDefaultState();
		this.Refresh();
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x00069019 File Offset: 0x00067219
	public vp_ComponentPreset Load(string path)
	{
		vp_ComponentPreset result = vp_ComponentPreset.LoadFromResources(this, path);
		this.RefreshDefaultState();
		this.Refresh();
		return result;
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0006902E File Offset: 0x0006722E
	public vp_ComponentPreset Load(TextAsset asset)
	{
		vp_ComponentPreset result = vp_ComponentPreset.LoadFromTextAsset(this, asset);
		this.RefreshDefaultState();
		this.Refresh();
		return result;
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x00069044 File Offset: 0x00067244
	public void CacheChildren()
	{
		this.Children.Clear();
		foreach (vp_Component vp_Component in base.GetComponentsInChildren<vp_Component>(true))
		{
			if (vp_Component.transform.parent == base.transform)
			{
				this.Children.Add(vp_Component);
			}
		}
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0006909C File Offset: 0x0006729C
	public void CacheSiblings()
	{
		this.Siblings.Clear();
		foreach (vp_Component vp_Component in base.GetComponents<vp_Component>())
		{
			if (vp_Component != this)
			{
				this.Siblings.Add(vp_Component);
			}
		}
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x000690E4 File Offset: 0x000672E4
	public void CacheFamily()
	{
		this.Family.Clear();
		foreach (vp_Component vp_Component in base.transform.root.GetComponentsInChildren<vp_Component>(true))
		{
			if (vp_Component != this)
			{
				this.Family.Add(vp_Component);
			}
		}
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x00069138 File Offset: 0x00067338
	public void CacheRenderers()
	{
		this.Renderers.Clear();
		foreach (Renderer item in base.GetComponentsInChildren<Renderer>(true))
		{
			this.Renderers.Add(item);
		}
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x00069178 File Offset: 0x00067378
	public void CacheAudioSources()
	{
		this.AudioSources.Clear();
		foreach (AudioSource item in base.GetComponentsInChildren<AudioSource>(true))
		{
			this.AudioSources.Add(item);
		}
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x000691B6 File Offset: 0x000673B6
	public virtual void Activate()
	{
		this.m_DeactivationTimer.Cancel();
		vp_Utility.Activate(base.gameObject, true);
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x000691CF File Offset: 0x000673CF
	public virtual void Deactivate()
	{
		vp_Utility.Activate(base.gameObject, false);
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x000691E0 File Offset: 0x000673E0
	public void DeactivateWhenSilent()
	{
		if (this == null)
		{
			return;
		}
		if (vp_Utility.IsActive(base.gameObject))
		{
			foreach (AudioSource audioSource in this.AudioSources)
			{
				if (audioSource.isPlaying && !audioSource.loop)
				{
					this.Rendering = false;
					vp_Timer.In(0.1f, delegate()
					{
						this.DeactivateWhenSilent();
					}, this.m_DeactivationTimer);
					return;
				}
			}
		}
		this.Deactivate();
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x0000248C File Offset: 0x0000068C
	public virtual void Refresh()
	{
	}

	// Token: 0x04000C4D RID: 3149
	public bool Persist;

	// Token: 0x04000C4E RID: 3150
	protected vp_StateManager m_StateManager;

	// Token: 0x04000C4F RID: 3151
	public vp_EventHandler EventHandler;

	// Token: 0x04000C50 RID: 3152
	[NonSerialized]
	protected vp_State m_DefaultState;

	// Token: 0x04000C51 RID: 3153
	protected bool m_Initialized;

	// Token: 0x04000C52 RID: 3154
	protected Transform m_Transform;

	// Token: 0x04000C53 RID: 3155
	protected Transform m_Parent;

	// Token: 0x04000C54 RID: 3156
	protected Transform m_Root;

	// Token: 0x04000C55 RID: 3157
	public List<vp_State> States = new List<vp_State>();

	// Token: 0x04000C56 RID: 3158
	public List<vp_Component> Children = new List<vp_Component>();

	// Token: 0x04000C57 RID: 3159
	public List<vp_Component> Siblings = new List<vp_Component>();

	// Token: 0x04000C58 RID: 3160
	public List<vp_Component> Family = new List<vp_Component>();

	// Token: 0x04000C59 RID: 3161
	public List<Renderer> Renderers = new List<Renderer>();

	// Token: 0x04000C5A RID: 3162
	public List<AudioSource> AudioSources = new List<AudioSource>();

	// Token: 0x04000C5B RID: 3163
	protected AudioSource m_Audio;

	// Token: 0x04000C5C RID: 3164
	protected vp_Timer.Handle m_DeactivationTimer = new vp_Timer.Handle();
}
