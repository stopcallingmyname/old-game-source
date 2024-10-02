using System;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class Cannon : MonoBehaviour
{
	// Token: 0x06000250 RID: 592 RVA: 0x0002B9B5 File Offset: 0x00029BB5
	private void Start()
	{
		this.preZoom = ContentLoader.LoadTexture("preZoom");
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0002B9C8 File Offset: 0x00029BC8
	private void Update()
	{
		if (this.tc == null)
		{
			this.tc = (TankController)Object.FindObjectOfType(typeof(TankController));
		}
		RaycastHit raycastHit;
		if (base.transform.parent.parent.parent.parent != null && base.transform.parent.parent.parent.parent.transform.name == "Player" && this.tc.activeControl && this.tc.enabled && Physics.Raycast(base.transform.position, base.transform.TransformDirection(Vector3.forward), out raycastHit))
		{
			Debug.DrawLine(base.transform.position, raycastHit.point, Color.red);
			this.pointCursor = Camera.main.WorldToScreenPoint(raycastHit.point);
		}
	}

	// Token: 0x06000252 RID: 594 RVA: 0x0002BAC4 File Offset: 0x00029CC4
	private void OnGUI()
	{
		if (this.tc == null)
		{
			this.tc = (TankController)Object.FindObjectOfType(typeof(TankController));
		}
		if (base.transform.parent.parent.parent.parent != null && base.transform.parent.parent.parent.parent.transform.name == "Player" && this.tc.activeControl && this.tc.enabled)
		{
			GUI.DrawTexture(new Rect
			{
				x = this.pointCursor.x - 14f,
				y = (float)Screen.height - this.pointCursor.y - 12f,
				width = 28f,
				height = 24f
			}, this.preZoom);
		}
	}

	// Token: 0x040002F8 RID: 760
	public float rotSpeed;

	// Token: 0x040002F9 RID: 761
	private Vector3 pointCursor;

	// Token: 0x040002FA RID: 762
	private Texture preZoom;

	// Token: 0x040002FB RID: 763
	private TankController tc;
}
