using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000070 RID: 112
public class SmokeGrenade : MonoBehaviour
{
	// Token: 0x06000337 RID: 823 RVA: 0x00039E20 File Offset: 0x00038020
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
		this.s = (Sound)Object.FindObjectOfType(typeof(Sound));
		this.AS = base.gameObject.GetComponent<AudioSource>();
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00039E72 File Offset: 0x00038072
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(2.5f);
		this.Smoke();
		yield return new WaitForSeconds(20f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00039E84 File Offset: 0x00038084
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
			MonoBehaviour.print("Sended");
		}
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00039F04 File Offset: 0x00038104
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
		if (base.gameObject == null)
		{
			return;
		}
		EntManager.DeleteEnt(this.entid);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00039F70 File Offset: 0x00038170
	private void Smoke()
	{
		ParticleSystem componentInChildren = base.gameObject.GetComponentInChildren<ParticleSystem>();
		if (componentInChildren == null)
		{
			this.KillSelf();
			return;
		}
		this.s.PlaySound_SmokeGrenade(this.AS);
		componentInChildren.Play();
	}

	// Token: 0x040005DE RID: 1502
	public int id;

	// Token: 0x040005DF RID: 1503
	public int uid;

	// Token: 0x040005E0 RID: 1504
	public int entid;

	// Token: 0x040005E1 RID: 1505
	private Client cscl;

	// Token: 0x040005E2 RID: 1506
	private EntManager entmanager;

	// Token: 0x040005E3 RID: 1507
	private Sound s;

	// Token: 0x040005E4 RID: 1508
	private AudioSource AS;
}
