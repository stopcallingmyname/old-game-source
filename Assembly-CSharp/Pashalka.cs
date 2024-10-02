using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200006B RID: 107
public class Pashalka : MonoBehaviour
{
	// Token: 0x0600031C RID: 796 RVA: 0x00039916 File Offset: 0x00037B16
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
	}

	// Token: 0x0600031D RID: 797 RVA: 0x00039932 File Offset: 0x00037B32
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(10f);
		if (this.timer)
		{
			this.KillSelf();
		}
		yield break;
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00039941 File Offset: 0x00037B41
	public void KillSelf()
	{
		EntManager.DeleteEnt(this.entid);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600031F RID: 799 RVA: 0x0003995C File Offset: 0x00037B5C
	private void Update()
	{
		if (this.healthImage != null)
		{
			this.healthImage.transform.LookAt(Camera.main.transform, Vector3.up);
			this.healthImage.fillAmount = (float)this.health / 500000f;
		}
		if (this.mat != null && this.mat.color.a > 0f)
		{
			this.mat.color = new Color(this.mat.color.r, this.mat.color.g, this.mat.color.b, this.mat.color.a - 0.05f);
		}
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00039A29 File Offset: 0x00037C29
	public void SetHealth(int _health)
	{
		this.health = _health;
	}

	// Token: 0x040005C6 RID: 1478
	public int id;

	// Token: 0x040005C7 RID: 1479
	public int uid;

	// Token: 0x040005C8 RID: 1480
	public int entid;

	// Token: 0x040005C9 RID: 1481
	public bool timer = true;

	// Token: 0x040005CA RID: 1482
	private Client cscl;

	// Token: 0x040005CB RID: 1483
	private EntManager entmanager;

	// Token: 0x040005CC RID: 1484
	public Image healthImage;

	// Token: 0x040005CD RID: 1485
	public int health;

	// Token: 0x040005CE RID: 1486
	public Material mat;
}
