using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000065 RID: 101
public class M61 : MonoBehaviour
{
	// Token: 0x060002F6 RID: 758 RVA: 0x00038CEA File Offset: 0x00036EEA
	private void Awake()
	{
		this.entmanager = (EntManager)Object.FindObjectOfType(typeof(EntManager));
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x00038D06 File Offset: 0x00036F06
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(2.5f);
		this.KillSelf();
		yield break;
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x00038D15 File Offset: 0x00036F15
	private void OnTriggerEnter(Collider col)
	{
		if (base.gameObject == null)
		{
			return;
		}
		if (col.GetComponent<Data>() != null && col.GetComponent<Data>().isGost)
		{
			this.KillSelf();
		}
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00038D48 File Offset: 0x00036F48
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
		GameObject gameObject = GameObject.Find("/" + base.gameObject.name + "/m61");
		if (gameObject == null)
		{
			gameObject = GameObject.Find("/" + base.gameObject.name);
			if (gameObject == null)
			{
				return;
			}
		}
		if (this.id == this.cscl.myindex)
		{
			this.cscl.send_detonateent(this.uid, gameObject.transform.position);
		}
		EntManager.DeleteEnt(this.entid);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000590 RID: 1424
	public int id;

	// Token: 0x04000591 RID: 1425
	public int uid;

	// Token: 0x04000592 RID: 1426
	public int entid;

	// Token: 0x04000593 RID: 1427
	private Client cscl;

	// Token: 0x04000594 RID: 1428
	private EntManager entmanager;
}
