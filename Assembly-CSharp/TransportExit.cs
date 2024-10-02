using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
public class TransportExit : MonoBehaviour
{
	// Token: 0x060002A2 RID: 674 RVA: 0x0003737C File Offset: 0x0003557C
	private void Start()
	{
		this.cscr = (Crosshair)Object.FindObjectOfType(typeof(Crosshair));
		this.oc = (OrbitCam)Object.FindObjectOfType(typeof(OrbitCam));
		this.cscr.SetActive(false);
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x000373CC File Offset: 0x000355CC
	private void Update()
	{
		if (this.activeControl && Input.GetKeyDown(KeyCode.F))
		{
			if (this.cl == null)
			{
				this.cl = (Client)Object.FindObjectOfType(typeof(Client));
			}
			this.cscr.SetActive(true);
			this.oc.zoom = false;
			if (this.vehicleType == CONST.VEHICLES.TANKS)
			{
				this.cl.send_exit_the_ent(base.gameObject.GetComponentInChildren<Tank>().uid);
				return;
			}
			if (this.vehicleType == CONST.VEHICLES.JEEP)
			{
				Car car = base.gameObject.GetComponentInChildren<Car>();
				if (car == null)
				{
					car = base.gameObject.GetComponentInParent<Car>();
				}
				if (car == null)
				{
					return;
				}
				this.cl.send_exit_the_ent(car.uid);
			}
		}
	}

	// Token: 0x04000518 RID: 1304
	private Client cl;

	// Token: 0x04000519 RID: 1305
	public bool activeControl = true;

	// Token: 0x0400051A RID: 1306
	private Crosshair cscr;

	// Token: 0x0400051B RID: 1307
	private OrbitCam oc;

	// Token: 0x0400051C RID: 1308
	public int vehicleType;
}
