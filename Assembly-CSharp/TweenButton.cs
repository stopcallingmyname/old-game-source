using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class TweenButton
{
	// Token: 0x0600059C RID: 1436 RVA: 0x00067560 File Offset: 0x00065760
	public TweenButton(GUIGS _guigs, Vector2 _myOffset, OFFSET_TYPE _offset_type, Texture2D _primary, Texture2D _background, Texture2D _secondary, Texture2D _selected, AudioClip _hover_melody, AudioClip _press_melody, AudioSource _AS, float _hover_melody_prepause, float _tween_scale = 10f, int _rotate_speed = 10, float _org_width = 0f, float _org_height = 0f, float _force_angle = 0f)
	{
		this.myOffset = _myOffset;
		this.offset_type = _offset_type;
		this.primary = _primary;
		this.background = _background;
		this.secondary = _secondary;
		this.selected = _selected;
		this.tween_scale = _tween_scale;
		this.rotateSpeed = _rotate_speed;
		if (_org_width > 0f)
		{
			this.orgWidth = _org_width;
			this.myWidth = _org_width;
		}
		else
		{
			this.orgWidth = (float)this.primary.width;
			this.myWidth = (float)this.primary.width;
		}
		if (_org_height > 0f)
		{
			this.orgHeight = _org_height;
			this.myHeight = _org_height;
		}
		else
		{
			this.orgHeight = (float)this.primary.height;
			this.myHeight = (float)this.primary.height;
		}
		this.myGUIGS = _guigs;
		this.myAngle = _force_angle;
		this.hover_melody = _hover_melody;
		this.press_melody = _press_melody;
		this.hover_melody_prepause = _hover_melody_prepause;
		this.AS = _AS;
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x00067684 File Offset: 0x00065884
	public bool DrawButton(Vector2 mpos)
	{
		if (this.myGUIGS != GM.currGUIState && this.myGUIGS != GUIGS.NULL)
		{
			return false;
		}
		if (!this.draw)
		{
			return false;
		}
		this.Tween(this.buttonRect.Contains(mpos));
		Color color = GUI.color;
		Matrix4x4 matrix = GUI.matrix;
		GUIUtility.RotateAroundPivot(this.myAngle, this.buttonRect.center);
		GUI.color = new Color(color.r, color.g, color.b, this.a);
		if (this.background != null)
		{
			GUI.DrawTexture(this.buttonRect, this.background);
		}
		GUI.color = new Color(color.r, color.g, color.b, 1f);
		Color color2 = GUI.color;
		if (this.primary != null)
		{
			GUI.DrawTexture(this.buttonRect, this.primary);
		}
		GUI.matrix = matrix;
		GUI.color = Color.white;
		if (this.secondary != null)
		{
			GUI.DrawTexture(this.buttonRect, this.secondary);
		}
		if (this.isSelected && this.selected != null)
		{
			GUI.DrawTexture(this.buttonRect, this.selected);
		}
		GUI.color = color2;
		GUI.color = color;
		if (GUI.Button(this.buttonRect, "", GUIManager.gs_style1))
		{
			if (this.press_melody != null)
			{
				if (this.AS == null)
				{
					this.AS = Object.FindObjectOfType<MainCamera>().GetComponent<AudioSource>();
				}
				if (this.AS != null)
				{
					this.AS.PlayOneShot(this.press_melody);
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x00067834 File Offset: 0x00065A34
	private void Tween(bool hover)
	{
		if (this.tt.Count != 0)
		{
			foreach (TWEEN_TYPE tween_TYPE in this.tt)
			{
				if (tween_TYPE == TWEEN_TYPE.BIGGER)
				{
					this.BiggerTween(hover);
				}
				if (this.offset_type == OFFSET_TYPE.IS_CENTER)
				{
					this.buttonRect = new Rect(this.myOffset.x - this.myWidth / 2f, this.myOffset.y - this.myHeight / 2f, this.myWidth, this.myHeight);
				}
				else if (this.offset_type == OFFSET_TYPE.FROM_CENTER)
				{
					Vector2 vector = GUIUtility.ScreenToGUIPoint(new Vector2((float)(Screen.width / 2) + this.myOffset.x - this.myWidth / 2f, (float)(Screen.height / 2) + this.myOffset.y - this.myHeight / 2f));
					this.buttonRect = new Rect(vector.x, vector.y, this.myWidth, this.myHeight);
				}
				else if (this.offset_type == OFFSET_TYPE.SCREEN_OFFSET)
				{
					this.buttonRect = new Rect((float)Screen.width / 2f + this.myOffset.x - this.myWidth / 2f, this.myOffset.y - this.myHeight / 2f, this.myWidth, this.myHeight);
				}
				if (tween_TYPE == TWEEN_TYPE.ROTATE)
				{
					this.RotateTween(hover);
				}
				if (tween_TYPE == TWEEN_TYPE.GLOW)
				{
					this.GlowTween(hover);
				}
			}
			if (hover)
			{
				if (!this.played)
				{
					if (this.hover_melody == null)
					{
						return;
					}
					if (this.AS == null)
					{
						this.AS = Object.FindObjectOfType<MainCamera>().GetComponent<AudioSource>();
					}
					if (this.AS != null)
					{
						this.AS.PlayOneShot(this.hover_melody);
					}
					this.played = true;
					return;
				}
			}
			else
			{
				this.played = false;
			}
			return;
		}
		if (this.offset_type == OFFSET_TYPE.IS_CENTER)
		{
			this.buttonRect = new Rect(this.myOffset.x - this.myWidth / 2f, this.myOffset.y - this.myHeight / 2f, this.myWidth, this.myHeight);
			return;
		}
		if (this.offset_type == OFFSET_TYPE.FROM_CENTER)
		{
			Vector2 vector2 = GUIUtility.ScreenToGUIPoint(new Vector2((float)(Screen.width / 2) + this.myOffset.x - this.myWidth / 2f, (float)(Screen.height / 2) + this.myOffset.y - this.myHeight / 2f));
			this.buttonRect = new Rect(vector2.x, vector2.y, this.myWidth, this.myHeight);
			return;
		}
		if (this.offset_type == OFFSET_TYPE.SCREEN_OFFSET)
		{
			this.buttonRect = new Rect((float)Screen.width / 2f + this.myOffset.x - this.myWidth / 2f, this.myOffset.y - this.myHeight / 2f, this.myWidth, this.myHeight);
		}
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00067B7C File Offset: 0x00065D7C
	private void BiggerTween(bool hover)
	{
		if (hover || this.forceHover)
		{
			this.myWidth = Mathf.Lerp(this.myWidth, this.orgWidth * (1f + this.tween_scale / 100f), Time.deltaTime * 10f);
			this.myHeight = Mathf.Lerp(this.myHeight, this.orgHeight * (1f + this.tween_scale / 100f), Time.deltaTime * 10f);
			return;
		}
		this.myWidth = Mathf.Lerp(this.myWidth, this.orgWidth, Time.deltaTime * 10f);
		this.myHeight = Mathf.Lerp(this.myHeight, this.orgHeight, Time.deltaTime * 10f);
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x00067C43 File Offset: 0x00065E43
	private void RotateTween(bool hover)
	{
		this.myAngle += Time.deltaTime * (float)this.rotateSpeed;
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x00067C60 File Offset: 0x00065E60
	private void GlowTween(bool hover)
	{
		this.a += Time.deltaTime * (float)this.coeff;
		if (this.a >= 1f || this.a <= 0f)
		{
			this.coeff *= -1;
		}
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x00067CB0 File Offset: 0x00065EB0
	public void DropSize()
	{
		this.myWidth = 0f;
		this.myHeight = 0f;
		if (this.hover_melody_prepause > 0f)
		{
			this.mute_time = Time.time + this.hover_melody_prepause;
			this.draw = false;
			return;
		}
		this.draw = true;
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x00067D04 File Offset: 0x00065F04
	public void GO()
	{
		if (this.mute_time > Time.time)
		{
			return;
		}
		if (this.AS == null)
		{
			this.AS = Object.FindObjectOfType<MainCamera>().GetComponent<AudioSource>();
		}
		if (this.AS != null)
		{
			this.AS.PlayOneShot(this.hover_melody);
		}
		this.draw = true;
	}

	// Token: 0x04000C2A RID: 3114
	private GUIGS myGUIGS;

	// Token: 0x04000C2B RID: 3115
	private Vector2 myOffset;

	// Token: 0x04000C2C RID: 3116
	public List<TWEEN_TYPE> tt = new List<TWEEN_TYPE>();

	// Token: 0x04000C2D RID: 3117
	private float tween_scale;

	// Token: 0x04000C2E RID: 3118
	private Texture2D background;

	// Token: 0x04000C2F RID: 3119
	private Texture2D primary;

	// Token: 0x04000C30 RID: 3120
	private Texture2D secondary;

	// Token: 0x04000C31 RID: 3121
	private Texture2D selected;

	// Token: 0x04000C32 RID: 3122
	public bool isSelected;

	// Token: 0x04000C33 RID: 3123
	private AudioClip hover_melody;

	// Token: 0x04000C34 RID: 3124
	private AudioClip press_melody;

	// Token: 0x04000C35 RID: 3125
	private AudioSource AS;

	// Token: 0x04000C36 RID: 3126
	private float hover_melody_prepause;

	// Token: 0x04000C37 RID: 3127
	private float myWidth;

	// Token: 0x04000C38 RID: 3128
	private float myHeight;

	// Token: 0x04000C39 RID: 3129
	private float orgWidth;

	// Token: 0x04000C3A RID: 3130
	private float orgHeight;

	// Token: 0x04000C3B RID: 3131
	private float myAngle;

	// Token: 0x04000C3C RID: 3132
	private int rotateSpeed;

	// Token: 0x04000C3D RID: 3133
	private float a = 1f;

	// Token: 0x04000C3E RID: 3134
	public bool draw = true;

	// Token: 0x04000C3F RID: 3135
	private Rect buttonRect;

	// Token: 0x04000C40 RID: 3136
	private OFFSET_TYPE offset_type;

	// Token: 0x04000C41 RID: 3137
	private float mute_time;

	// Token: 0x04000C42 RID: 3138
	private bool played;

	// Token: 0x04000C43 RID: 3139
	public bool forceHover;

	// Token: 0x04000C44 RID: 3140
	private int coeff = -1;
}
