using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000096 RID: 150
public class PlayerAnimation : MonoBehaviour
{
	// Token: 0x06000508 RID: 1288 RVA: 0x0005FCD4 File Offset: 0x0005DED4
	private void Awake()
	{
		this.anim = base.gameObject.GetComponent<Animator>();
		if (this.anim.layerCount == 0)
		{
			base.StartCoroutine(this.layerupdate());
			return;
		}
		this.anim.SetLayerWeight(1, 1f);
		this.anim.SetLayerWeight(2, 1f);
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x0005FD2F File Offset: 0x0005DF2F
	private IEnumerator layerupdate()
	{
		yield return null;
		if (this.anim.layerCount == 0)
		{
			base.StartCoroutine(this.layerupdate());
		}
		else
		{
			this.anim.SetLayerWeight(1, 1f);
			this.anim.SetLayerWeight(2, 1f);
		}
		yield break;
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x0005FD40 File Offset: 0x0005DF40
	private void FixedUpdate()
	{
		if (this.anim == null)
		{
			return;
		}
		if (this.anim.layerCount == 0)
		{
			return;
		}
		AnimatorStateInfo currentAnimatorStateInfo = this.anim.GetCurrentAnimatorStateInfo(1);
		if (currentAnimatorStateInfo.nameHash == PlayerAnimation.Reload_state)
		{
			this.anim.SetBool("inReload", false);
		}
		if (currentAnimatorStateInfo.nameHash == PlayerAnimation.Draw_state)
		{
			this.anim.SetBool("inDraw", false);
		}
		if (currentAnimatorStateInfo.nameHash == PlayerAnimation.Shot_state)
		{
			this.anim.SetBool("inShot", false);
		}
		if (base.GetComponent<AudioSource>().clip == null)
		{
			base.GetComponent<AudioSource>().clip = ContentLoader.LoadSound("walk");
		}
		this.xval_lerp = Mathf.Lerp(this.xval_lerp, this.xval, Time.deltaTime * 5f);
		this.anim.SetFloat("Angle", this.xval_lerp);
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x0000248C File Offset: 0x0000068C
	private void UpdateAim()
	{
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x0005FE34 File Offset: 0x0005E034
	public void SetSpeed(float fval)
	{
		this.anim.SetFloat("Speed", fval);
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x0005FE47 File Offset: 0x0005E047
	public void SetWeaponClass()
	{
		this.anim.SetInteger("WeaponClass", 0);
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x0005FE5A File Offset: 0x0005E05A
	public void SetReload()
	{
		this.anim.SetBool("inReload", true);
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x0005FE6D File Offset: 0x0005E06D
	public void SetDraw()
	{
		this.anim.SetBool("inDraw", true);
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x0005FE80 File Offset: 0x0005E080
	public void SetShot()
	{
		this.anim.SetBool("inShot", true);
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x0005FE93 File Offset: 0x0005E093
	public void SetCrouch(bool bval)
	{
		this.anim.SetBool("Crouch", bval);
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x0005FEA6 File Offset: 0x0005E0A6
	public void SetJeepGunner(bool bval)
	{
		this.anim.SetBool("inJeepGunner", bval);
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x0005FEB9 File Offset: 0x0005E0B9
	public void SetRotation(float x)
	{
		this.xval = (x - 90f) * -1f - 15f;
		if (this.xval < -90f)
		{
			this.xval = -90f;
		}
	}

	// Token: 0x04000960 RID: 2400
	public Transform[] upperBodyChain;

	// Token: 0x04000961 RID: 2401
	protected Animator anim;

	// Token: 0x04000962 RID: 2402
	protected float sliderval;

	// Token: 0x04000963 RID: 2403
	protected float sliderval_rotation_y;

	// Token: 0x04000964 RID: 2404
	protected bool crouch;

	// Token: 0x04000965 RID: 2405
	private static int Reload_state = Animator.StringToHash("UpperBody.Reload");

	// Token: 0x04000966 RID: 2406
	private static int Draw_state = Animator.StringToHash("UpperBody.Draw");

	// Token: 0x04000967 RID: 2407
	private static int Shot_state = Animator.StringToHash("UpperBody.Shot");

	// Token: 0x04000968 RID: 2408
	private static int JeepGunner_state = Animator.StringToHash("UpperBody.JeepGunner");

	// Token: 0x04000969 RID: 2409
	private float xval;

	// Token: 0x0400096A RID: 2410
	private float xval_lerp;
}
