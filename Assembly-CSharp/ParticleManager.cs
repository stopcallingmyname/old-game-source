using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class ParticleManager : MonoBehaviour
{
	// Token: 0x060001F5 RID: 501 RVA: 0x00024ED7 File Offset: 0x000230D7
	private void Awake()
	{
		ParticleManager.THIS = this;
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00024EDF File Offset: 0x000230DF
	public void CreateParticle(float x, float y, float z, float r, float g, float b, float a)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.pgoBrick);
		gameObject.transform.position = new Vector3(x, y, z);
		gameObject.GetComponent<Renderer>().material.color = new Color(r, g, b, a);
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x00024F1C File Offset: 0x0002311C
	public void CreateHit(Transform _player, int hitbox, float x, float y, float z)
	{
		GameObject gameObject;
		if (hitbox == 1)
		{
			gameObject = Object.Instantiate<GameObject>(this.pgoHitHead);
		}
		else
		{
			gameObject = Object.Instantiate<GameObject>(this.pgoHitBody);
		}
		gameObject.transform.position = new Vector3(x, y, z);
		gameObject.transform.LookAt(_player.position + new Vector3(0f, 1.75f, 0f));
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x00024F86 File Offset: 0x00023186
	public void CreateFlame(Vector3 pos, Transform parent)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.pgoFlame);
		gameObject.transform.position = pos;
		gameObject.transform.parent = parent;
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0000248C File Offset: 0x0000068C
	public void CreateGhostDeath(Vector3 pos)
	{
	}

	// Token: 0x060001FA RID: 506 RVA: 0x0000248C File Offset: 0x0000068C
	private void Update()
	{
	}

	// Token: 0x040001FD RID: 509
	public static ParticleManager THIS;

	// Token: 0x040001FE RID: 510
	public GameObject pgoBrick;

	// Token: 0x040001FF RID: 511
	public GameObject pgoHitHead;

	// Token: 0x04000200 RID: 512
	public GameObject pgoHitBody;

	// Token: 0x04000201 RID: 513
	public GameObject pgoFlame;

	// Token: 0x04000202 RID: 514
	public GameObject pgoGhostDeath;

	// Token: 0x04000203 RID: 515
	public GameObject FireWork1;

	// Token: 0x04000204 RID: 516
	public GameObject FireWork2;

	// Token: 0x04000205 RID: 517
	private bool Fire;
}
