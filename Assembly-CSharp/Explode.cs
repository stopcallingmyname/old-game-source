using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class Explode : MonoBehaviour
{
	// Token: 0x060002CA RID: 714 RVA: 0x00037F49 File Offset: 0x00036149
	private void Awake()
	{
		this.explode = ContentLoader.LoadSound("grenade");
		this.fx = (Resources.Load("Prefab/explode") as GameObject);
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00037F70 File Offset: 0x00036170
	private IEnumerator Start()
	{
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		audioSource.maxDistance = 35f;
		audioSource.spatialBlend = 1f;
		audioSource.playOnAwake = false;
		audioSource.PlayOneShot(this.explode, AudioListener.volume);
		Object.Instantiate<GameObject>(this.fx).transform.position = base.gameObject.transform.position;
		yield return new WaitForSeconds(1f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00016B46 File Offset: 0x00014D46
	private void KillSelf()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000555 RID: 1365
	private AudioClip explode;

	// Token: 0x04000556 RID: 1366
	private GameObject fx;
}
