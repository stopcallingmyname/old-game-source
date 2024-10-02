using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class GazGrenade : MonoBehaviour
{
	// Token: 0x060002D5 RID: 725 RVA: 0x000381D4 File Offset: 0x000363D4
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.s = (Sound)Object.FindObjectOfType(typeof(Sound));
		this.AS = base.gameObject.GetComponent<AudioSource>();
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x00038226 File Offset: 0x00036426
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(2.5f);
		this.Smoke();
		yield return new WaitForSeconds(20f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00038238 File Offset: 0x00036438
	private void KillSelf()
	{
		if (this.cscl == null)
		{
			this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		if (this.cscl == null)
		{
			return;
		}
		if (GameObject.Find("/" + base.gameObject.name) == null)
		{
			return;
		}
		EntManager.DeleteEnt(this.entid);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x000382B8 File Offset: 0x000364B8
	private void Smoke()
	{
		ParticleSystem componentInChildren = base.gameObject.GetComponentInChildren<ParticleSystem>();
		if (componentInChildren == null)
		{
			this.KillSelf();
			return;
		}
		if (this.cscl == null)
		{
			this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		if (this.cscl != null && this.id == this.cscl.myindex)
		{
			this.cscl.send_new_ent_pos(this.uid, base.transform.position);
		}
		this.s.PlaySound_SmokeGrenade(this.AS);
		componentInChildren.Play();
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00038360 File Offset: 0x00036560
	private void OnCollisionEnter(Collision collision)
	{
		if (this.cscl == null)
		{
			this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		if (this.cscl == null)
		{
			return;
		}
		if (this.id == this.cscl.myindex)
		{
			this.cscl.send_new_ent_pos(this.uid, base.transform.position);
		}
	}

	// Token: 0x04000565 RID: 1381
	public int id;

	// Token: 0x04000566 RID: 1382
	public int uid;

	// Token: 0x04000567 RID: 1383
	public int entid;

	// Token: 0x04000568 RID: 1384
	private Client cscl;

	// Token: 0x04000569 RID: 1385
	private EntManager entmanager;

	// Token: 0x0400056A RID: 1386
	private Sound s;

	// Token: 0x0400056B RID: 1387
	private AudioSource AS;
}
