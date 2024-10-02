using System;
using UnityEngine;

// Token: 0x02000097 RID: 151
public class PreviewAnimation : MonoBehaviour
{
	// Token: 0x06000516 RID: 1302 RVA: 0x0005FF2A File Offset: 0x0005E12A
	private void Awake()
	{
		this.anim = base.gameObject.GetComponent<Animator>();
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x0005FF3D File Offset: 0x0005E13D
	public void SetState(int val)
	{
		this.anim.SetInteger("anim", val);
	}

	// Token: 0x0400096B RID: 2411
	protected Animator anim;
}
