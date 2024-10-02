using System;
using UnityEngine;

// Token: 0x0200012F RID: 303
public class DetonatorSprayHelper : MonoBehaviour
{
	// Token: 0x06000A84 RID: 2692 RVA: 0x000859C0 File Offset: 0x00083BC0
	private void Start()
	{
		this.startTime = Random.value * (this.startTimeMax - this.startTimeMin) + this.startTimeMin + Time.time;
		this.stopTime = Random.value * (this.stopTimeMax - this.stopTimeMin) + this.stopTimeMin + Time.time;
		base.GetComponent<Renderer>().material = ((Random.value > 0.5f) ? this.firstMaterial : this.secondMaterial);
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x00085A3E File Offset: 0x00083C3E
	private void FixedUpdate()
	{
		float time = Time.time;
		float num = this.startTime;
		float time2 = Time.time;
		float num2 = this.stopTime;
	}

	// Token: 0x04001094 RID: 4244
	public float startTimeMin;

	// Token: 0x04001095 RID: 4245
	public float startTimeMax;

	// Token: 0x04001096 RID: 4246
	public float stopTimeMin = 10f;

	// Token: 0x04001097 RID: 4247
	public float stopTimeMax = 10f;

	// Token: 0x04001098 RID: 4248
	public Material firstMaterial;

	// Token: 0x04001099 RID: 4249
	public Material secondMaterial;

	// Token: 0x0400109A RID: 4250
	private float startTime;

	// Token: 0x0400109B RID: 4251
	private float stopTime;

	// Token: 0x0400109C RID: 4252
	private bool isReallyOn;
}
