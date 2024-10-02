using System;
using System.Collections;
using AssemblyCSharp;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class NpcLerp : MonoBehaviour
{
	// Token: 0x060001A6 RID: 422 RVA: 0x00023163 File Offset: 0x00021363
	private void Awake()
	{
		this.anim = base.gameObject.GetComponent<Animator>();
		this.roar_sound = Resources.Load<AudioClip>("boss_ghost_roar");
		this.sound = base.gameObject.AddComponent<AudioSource>();
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00023198 File Offset: 0x00021398
	public void PlayAnimation(int move)
	{
		switch (move)
		{
		case 0:
			this.sound.PlayOneShot(this.roar_sound);
			base.StartCoroutine(this.Roar());
			return;
		case 1:
			base.StartCoroutine(this.Hit1());
			return;
		case 2:
			base.StartCoroutine(this.Hit2());
			return;
		default:
			return;
		}
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x000231F2 File Offset: 0x000213F2
	private IEnumerator Roar()
	{
		this.anim.SetBool("roar", true);
		yield return new WaitForSeconds(2.2f);
		if (base.gameObject == null)
		{
			yield break;
		}
		this.anim.SetBool("roar", false);
		yield break;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00023201 File Offset: 0x00021401
	private IEnumerator Hit1()
	{
		this.anim.SetInteger("Hit", 1);
		yield return new WaitForSeconds(4f);
		if (base.gameObject == null)
		{
			yield break;
		}
		this.anim.SetInteger("Hit", 0);
		yield break;
	}

	// Token: 0x060001AA RID: 426 RVA: 0x00023210 File Offset: 0x00021410
	private IEnumerator Hit2()
	{
		this.anim.SetInteger("Hit", 2);
		yield return new WaitForSeconds(1.84f);
		if (base.gameObject == null)
		{
			yield break;
		}
		this.anim.SetInteger("Hit", 0);
		yield break;
	}

	// Token: 0x060001AB RID: 427 RVA: 0x00023220 File Offset: 0x00021420
	private void FixedUpdate()
	{
		if (this.ent.go.name == "Boss")
		{
			Vector3 vector = this.ent.go.transform.position - this.ent.position;
			if ((double)vector.magnitude > 0.05)
			{
				this.anim.SetFloat("Speed", vector.magnitude);
			}
		}
		this.ent.go.transform.position = Vector3.Lerp(this.ent.go.transform.position, this.ent.position, Time.deltaTime * 2.5f);
		float num = this.ent.go.transform.eulerAngles.y;
		float num2 = this.ent.rotation.y - num;
		if (num2 > 180f)
		{
			num += 360f;
		}
		if (num2 < -180f)
		{
			num -= 360f;
		}
		num = Mathf.Lerp(num, this.ent.rotation.y, Time.deltaTime * 5f);
		this.ent.go.transform.eulerAngles = new Vector3(0f, num, 0f);
	}

	// Token: 0x04000173 RID: 371
	public CEnt ent;

	// Token: 0x04000174 RID: 372
	private Animator anim;

	// Token: 0x04000175 RID: 373
	private bool roar;

	// Token: 0x04000176 RID: 374
	private AudioSource sound;

	// Token: 0x04000177 RID: 375
	private AudioClip roar_sound;
}
