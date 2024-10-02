using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000087 RID: 135
public class PauseGUI : MonoBehaviour
{
	// Token: 0x060003C9 RID: 969 RVA: 0x00049E78 File Offset: 0x00048078
	private void Awake()
	{
		this.gui_style = new GUIStyle();
		this.gui_style.font = FontManager.font[2];
		this.gui_style.fontSize = 16;
		this.gui_style.normal.textColor = new Color(255f, 255f, 255f, 255f);
		this.gui_style.alignment = TextAnchor.MiddleCenter;
		this.gamevolume = Config.gamevolume;
		this.local_sens = Config.Sensitivity;
		AudioListener.volume = this.gamevolume;
	}

	// Token: 0x060003CA RID: 970 RVA: 0x00049F08 File Offset: 0x00048108
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.BackQuote))
		{
			this.active = !this.active;
			base.gameObject.GetComponent<AdminGUI>().SetActive(false);
			if (this.ws == null)
			{
				this.ws = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
			}
			if (this.tc == null)
			{
				this.tc = (TankController)Object.FindObjectOfType(typeof(TankController));
			}
			if (this.cc == null)
			{
				this.cc = (CarController)Object.FindObjectOfType(typeof(CarController));
			}
			if (this.td == null)
			{
				this.td = (TransportDetect)Object.FindObjectOfType(typeof(TransportDetect));
			}
			if (this.te == null)
			{
				this.te = (TransportExit)Object.FindObjectOfType(typeof(TransportExit));
			}
			MainGUI.ForceCursor = this.active;
			if (this.tc != null)
			{
				this.tc.activeControl = !this.active;
			}
			if (this.cc != null)
			{
				this.cc.activeControl = !this.active;
			}
			if (this.td != null)
			{
				this.td.activeControl = !this.active;
			}
			if (this.te != null)
			{
				this.te.activeControl = !this.active;
			}
		}
	}

	// Token: 0x060003CB RID: 971 RVA: 0x0004A0A8 File Offset: 0x000482A8
	private void OnGUI()
	{
		if (!this.active)
		{
			return;
		}
		GUI.DrawTexture(new Rect(0f, 0f, 200f, (float)Screen.height), GUIManager.tex_half_black);
		GUI.DrawTexture(new Rect(4f, GUIManager.XRES(128f) + 16f, 192f, 32f), GUIManager.tex_button);
		if (GUI.Button(new Rect(4f, GUIManager.XRES(128f) + 16f, 192f, 32f), Lang.GetLabel(139), this.gui_style))
		{
			Config.ChangeMode();
			this.active = false;
		}
		if (PlayerControl.isPrivateAdmin() > 0)
		{
			GUI.DrawTexture(new Rect(4f, GUIManager.XRES(128f) + 16f + 34f, 192f, 32f), GUIManager.tex_button_hover);
			if (GUI.Button(new Rect(4f, GUIManager.XRES(128f) + 16f + 34f, 192f, 32f), Lang.GetLabel(116), this.gui_style))
			{
				this.active = false;
				base.gameObject.GetComponent<AdminGUI>().SetActive(true);
			}
			if (CONST.GetGameMode() == MODE.BUILD)
			{
				GUI.DrawTexture(new Rect(4f, GUIManager.XRES(128f) + 16f + 68f, 192f, 32f), GUIManager.tex_button_hover);
				if (GUI.Button(new Rect(4f, GUIManager.XRES(128f) + 16f + 68f, 192f, 32f), Lang.GetLabel(129), this.gui_style))
				{
					this.active = false;
					base.gameObject.GetComponent<SaveGUI>().SetActive(true);
				}
			}
		}
		if (this.cschat == null)
		{
			this.cschat = base.gameObject.GetComponent<Chat>();
		}
		if (this.cschat)
		{
			string label;
			if (this.cschat.show)
			{
				label = Lang.GetLabel(140);
			}
			else
			{
				label = Lang.GetLabel(141);
			}
			GUI.DrawTexture(new Rect(4f, (float)(Screen.height - 220), 192f, 32f), GUIManager.tex_button);
			if (GUI.Button(new Rect(4f, (float)(Screen.height - 220), 192f, 32f), label, this.gui_style))
			{
				this.cschat.show = !this.cschat.show;
			}
		}
		GUI.DrawTexture(new Rect(4f, (float)(Screen.height - 180), 192f, 32f), GUIManager.tex_soundbar);
		float num = GUI.HorizontalSlider(new Rect(44f, (float)(Screen.height - 170), 144f, 24f), this.gamevolume, 0f, 1f);
		if (this.gamevolume != num)
		{
			this.gamevolume = num;
			AudioListener.volume = this.gamevolume;
			Config.gamevolume = this.gamevolume;
			PlayerPrefs.SetFloat(CONST.MD5("GameVolume"), this.gamevolume);
			PlayerPrefs.Save();
		}
		GUI.DrawTexture(new Rect(4f, (float)(Screen.height - 140), 192f, 32f), GUIManager.tex_sensbar);
		float num2 = GUI.HorizontalSlider(new Rect(44f, (float)(Screen.height - 130), 144f, 24f), this.local_sens, 0.1f, 20f);
		num2 = (float)((int)(num2 * 10f)) / 10f;
		if (this.local_sens != num2)
		{
			this.local_sens = num2;
			Config.Sensitivity = this.local_sens;
			PlayerPrefs.SetFloat(CONST.MD5("Sensitivity"), this.local_sens);
			PlayerPrefs.Save();
			this.ws.SetMouseSensitivity(this.local_sens);
		}
		GUI.DrawTexture(new Rect(4f, (float)(Screen.height - 60), 192f, 32f), GUIManager.tex_button);
		if (GUI.Button(new Rect(4f, (float)(Screen.height - 60), 192f, 32f), Lang.GetLabel(142), this.gui_style))
		{
			GameObject gameObject = GameObject.Find("Player");
			if (gameObject)
			{
				gameObject.GetComponent<Client>().send_disconnect();
			}
			GameController.STATE = GAME_STATES.MAINMENU;
			SceneManager.LoadScene(0);
		}
	}

	// Token: 0x0400083D RID: 2109
	private bool active;

	// Token: 0x0400083E RID: 2110
	private GUIStyle gui_style;

	// Token: 0x0400083F RID: 2111
	private PlayerControl pc;

	// Token: 0x04000840 RID: 2112
	private WeaponSystem ws;

	// Token: 0x04000841 RID: 2113
	private float gamevolume = 1f;

	// Token: 0x04000842 RID: 2114
	private Chat cschat;

	// Token: 0x04000843 RID: 2115
	private float local_sens = 3f;

	// Token: 0x04000844 RID: 2116
	private TankController tc;

	// Token: 0x04000845 RID: 2117
	private CarController cc;

	// Token: 0x04000846 RID: 2118
	private TransportDetect td;

	// Token: 0x04000847 RID: 2119
	private TransportExit te;

	// Token: 0x04000848 RID: 2120
	private bool _chat_filter;
}
