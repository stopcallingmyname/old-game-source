using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class FlameCaster : MonoBehaviour
{
	// Token: 0x0600002C RID: 44 RVA: 0x00002851 File Offset: 0x00000A51
	private void Start()
	{
		this.myTransform = base.transform;
		this.myPS = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x0600002D RID: 45 RVA: 0x0000286C File Offset: 0x00000A6C
	private void Update()
	{
		if (!this.myPS.isPlaying)
		{
			return;
		}
		if (this.timer > Time.time)
		{
			return;
		}
		this.timer = Time.time + 0.1f;
		Ray ray = new Ray(this.myTransform.position, this.myTransform.forward);
		ray.direction += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
		RaycastHit raycastHit;
		if (!Physics.Raycast(ray, out raycastHit, 20f))
		{
			return;
		}
		if (ParticleManager.THIS == null)
		{
			return;
		}
		ParticleManager.THIS.CreateFlame(raycastHit.point, raycastHit.transform);
	}

	// Token: 0x0400001A RID: 26
	private ParticleSystem myPS;

	// Token: 0x0400001B RID: 27
	private Transform myTransform;

	// Token: 0x0400001C RID: 28
	private float timer;
}
