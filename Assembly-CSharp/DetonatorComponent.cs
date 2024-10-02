using System;
using UnityEngine;

// Token: 0x02000133 RID: 307
public abstract class DetonatorComponent : MonoBehaviour
{
	// Token: 0x06000AA7 RID: 2727
	public abstract void Explode();

	// Token: 0x06000AA8 RID: 2728
	public abstract void Init();

	// Token: 0x06000AA9 RID: 2729 RVA: 0x00086EC4 File Offset: 0x000850C4
	public void SetStartValues()
	{
		this.startSize = this.size;
		this.startForce = this.force;
		this.startVelocity = this.velocity;
		this.startDuration = this.duration;
		this.startDetail = this.detail;
		this.startColor = this.color;
		this.startLocalPosition = this.localPosition;
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x00086F25 File Offset: 0x00085125
	public Detonator MyDetonator()
	{
		return base.GetComponent("Detonator") as Detonator;
	}

	// Token: 0x040010FC RID: 4348
	public bool on = true;

	// Token: 0x040010FD RID: 4349
	public bool detonatorControlled = true;

	// Token: 0x040010FE RID: 4350
	[HideInInspector]
	public float startSize = 1f;

	// Token: 0x040010FF RID: 4351
	public float size = 1f;

	// Token: 0x04001100 RID: 4352
	public float explodeDelayMin;

	// Token: 0x04001101 RID: 4353
	public float explodeDelayMax;

	// Token: 0x04001102 RID: 4354
	[HideInInspector]
	public float startDuration = 2f;

	// Token: 0x04001103 RID: 4355
	public float duration = 2f;

	// Token: 0x04001104 RID: 4356
	[HideInInspector]
	public float timeScale = 1f;

	// Token: 0x04001105 RID: 4357
	[HideInInspector]
	public float startDetail = 1f;

	// Token: 0x04001106 RID: 4358
	public float detail = 1f;

	// Token: 0x04001107 RID: 4359
	[HideInInspector]
	public Color startColor = Color.white;

	// Token: 0x04001108 RID: 4360
	public Color color = Color.white;

	// Token: 0x04001109 RID: 4361
	[HideInInspector]
	public Vector3 startLocalPosition = Vector3.zero;

	// Token: 0x0400110A RID: 4362
	public Vector3 localPosition = Vector3.zero;

	// Token: 0x0400110B RID: 4363
	[HideInInspector]
	public Vector3 startForce = Vector3.zero;

	// Token: 0x0400110C RID: 4364
	public Vector3 force = Vector3.zero;

	// Token: 0x0400110D RID: 4365
	[HideInInspector]
	public Vector3 startVelocity = Vector3.zero;

	// Token: 0x0400110E RID: 4366
	public Vector3 velocity = Vector3.zero;

	// Token: 0x0400110F RID: 4367
	public float detailThreshold;
}
