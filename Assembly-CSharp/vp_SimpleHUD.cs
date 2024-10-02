using System;
using UnityEngine;

// Token: 0x020000EE RID: 238
[RequireComponent(typeof(AudioSource))]
public class vp_SimpleHUD : MonoBehaviour
{
	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000892 RID: 2194 RVA: 0x0007BB5C File Offset: 0x00079D5C
	public static GUIStyle MessageStyle
	{
		get
		{
			if (vp_SimpleHUD.m_MessageStyle == null)
			{
				vp_SimpleHUD.m_MessageStyle = new GUIStyle("Label");
				vp_SimpleHUD.m_MessageStyle.alignment = TextAnchor.MiddleCenter;
			}
			return vp_SimpleHUD.m_MessageStyle;
		}
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0007BB89 File Offset: 0x00079D89
	private void Awake()
	{
		this.m_Player = base.transform.GetComponent<vp_FPPlayerEventHandler>();
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x0007BB9C File Offset: 0x00079D9C
	protected virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x0007BBB8 File Offset: 0x00079DB8
	protected virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x0007BBD4 File Offset: 0x00079DD4
	protected virtual void OnGUI()
	{
		if (!this.ShowHUD)
		{
			return;
		}
		GUI.Box(new Rect(10f, (float)(Screen.height - 30), 100f, 22f), "Health: " + (int)(this.m_Player.Health.Get() * 100f) + "%");
		GUI.Box(new Rect((float)(Screen.width - 220), (float)(Screen.height - 30), 100f, 22f), "Clips: " + this.m_Player.GetItemCount.Send("AmmoClip"));
		if (!string.IsNullOrEmpty(this.m_PickupMessage) && this.m_MessageColor.a > 0.01f)
		{
			this.m_MessageColor = Color.Lerp(this.m_MessageColor, this.m_InvisibleColor, Time.deltaTime * 0.4f);
			GUI.color = this.m_MessageColor;
			GUI.Box(new Rect(200f, 150f, (float)(Screen.width - 400), (float)(Screen.height - 400)), this.m_PickupMessage, vp_SimpleHUD.MessageStyle);
			GUI.color = Color.white;
		}
		if (this.DamageFlashTexture != null && this.m_DamageFlashColor.a > 0.01f)
		{
			this.m_DamageFlashColor = Color.Lerp(this.m_DamageFlashColor, this.m_DamageFlashInvisibleColor, Time.deltaTime * 0.4f);
			GUI.color = this.m_DamageFlashColor;
			GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.DamageFlashTexture);
			GUI.color = Color.white;
		}
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0007BD9B File Offset: 0x00079F9B
	protected virtual void OnMessage_HUDText(string message)
	{
		this.m_MessageColor = Color.white;
		this.m_PickupMessage = message;
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x0007BDAF File Offset: 0x00079FAF
	protected virtual void OnMessage_HUDDamageFlash(float intensity)
	{
		if (this.DamageFlashTexture == null)
		{
			return;
		}
		if (intensity == 0f)
		{
			this.m_DamageFlashColor.a = 0f;
			return;
		}
		this.m_DamageFlashColor.a = this.m_DamageFlashColor.a + intensity;
	}

	// Token: 0x04000F15 RID: 3861
	public Texture DamageFlashTexture;

	// Token: 0x04000F16 RID: 3862
	public bool ShowHUD = true;

	// Token: 0x04000F17 RID: 3863
	private Color m_MessageColor = new Color(2f, 2f, 0f, 2f);

	// Token: 0x04000F18 RID: 3864
	private Color m_InvisibleColor = new Color(1f, 1f, 0f, 0f);

	// Token: 0x04000F19 RID: 3865
	private Color m_DamageFlashColor = new Color(0.8f, 0f, 0f, 0f);

	// Token: 0x04000F1A RID: 3866
	private Color m_DamageFlashInvisibleColor = new Color(1f, 0f, 0f, 0f);

	// Token: 0x04000F1B RID: 3867
	private string m_PickupMessage = "";

	// Token: 0x04000F1C RID: 3868
	protected static GUIStyle m_MessageStyle;

	// Token: 0x04000F1D RID: 3869
	private vp_FPPlayerEventHandler m_Player;
}
