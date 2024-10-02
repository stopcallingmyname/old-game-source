using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class VehicleSmoke : MonoBehaviour
{
	// Token: 0x06000355 RID: 853 RVA: 0x0003A7DA File Offset: 0x000389DA
	private void Awake()
	{
		this.s = (Sound)Object.FindObjectOfType(typeof(Sound));
		this.AS = base.gameObject.GetComponent<AudioSource>();
	}

	// Token: 0x06000356 RID: 854 RVA: 0x0003A807 File Offset: 0x00038A07
	private IEnumerator Start()
	{
		this.Smoke();
		yield return new WaitForSeconds(20f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x06000357 RID: 855 RVA: 0x0003A816 File Offset: 0x00038A16
	private void KillSelf()
	{
		EntManager.DeleteEnt(this.entid);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000358 RID: 856 RVA: 0x0003A82E File Offset: 0x00038A2E
	private void Smoke()
	{
		this.s.PlaySound_SmokeGrenade(this.AS);
		this.Smoke1.Play();
		this.Smoke2.Play();
		this.Smoke3.Play();
		this.Smoke4.Play();
	}

	// Token: 0x04000611 RID: 1553
	public int id;

	// Token: 0x04000612 RID: 1554
	public int uid;

	// Token: 0x04000613 RID: 1555
	public int entid;

	// Token: 0x04000614 RID: 1556
	private Client cscl;

	// Token: 0x04000615 RID: 1557
	private EntManager entmanager;

	// Token: 0x04000616 RID: 1558
	public ParticleSystem Smoke1;

	// Token: 0x04000617 RID: 1559
	public ParticleSystem Smoke2;

	// Token: 0x04000618 RID: 1560
	public ParticleSystem Smoke3;

	// Token: 0x04000619 RID: 1561
	public ParticleSystem Smoke4;

	// Token: 0x0400061A RID: 1562
	private Sound s;

	// Token: 0x0400061B RID: 1563
	private AudioSource AS;
}
