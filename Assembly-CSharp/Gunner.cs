using System;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class Gunner : MonoBehaviour
{
	// Token: 0x06000276 RID: 630 RVA: 0x00030513 File Offset: 0x0002E713
	private void Start()
	{
		this.cross = (Resources.Load("GUI/humvee_dynamic") as Texture);
		this.crossStatic = (Resources.Load("GUI/humvee_static") as Texture);
		this.myTransform = base.transform;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x0003054C File Offset: 0x0002E74C
	private void Update()
	{
		if (this.cc == null)
		{
			this.cc = (CarController)Object.FindObjectOfType(typeof(CarController));
		}
		RaycastHit raycastHit;
		if (this.cc != null && this.cc.activeControl && this.cc.enabled && this.cc.myPosition == CONST.VEHICLES.POSITION_JEEP_GUNNER && Physics.Raycast(base.transform.position, base.transform.TransformDirection(Vector3.forward), out raycastHit))
		{
			this.pointCursor = Camera.main.WorldToScreenPoint(raycastHit.point);
		}
	}

	// Token: 0x06000278 RID: 632 RVA: 0x000305F8 File Offset: 0x0002E7F8
	private void OnGUI()
	{
		if (this.cc == null)
		{
			this.cc = (CarController)Object.FindObjectOfType(typeof(CarController));
		}
		if (this.cc != null && this.cc.activeControl && this.cc.enabled && this.cc.myPosition == CONST.VEHICLES.POSITION_JEEP_GUNNER)
		{
			Rect position = new Rect
			{
				x = this.pointCursor.x - 16f,
				y = (float)(Screen.height / 2 - 16),
				width = 32f,
				height = 32f
			};
			GUI.DrawTexture(position, this.cross);
			position.x = (float)(Screen.width / 2 - 16);
			GUI.DrawTexture(position, this.crossStatic);
		}
	}

	// Token: 0x040003D0 RID: 976
	public float rotSpeed;

	// Token: 0x040003D1 RID: 977
	private Vector3 pointCursor;

	// Token: 0x040003D2 RID: 978
	private Texture cross;

	// Token: 0x040003D3 RID: 979
	private Texture crossStatic;

	// Token: 0x040003D4 RID: 980
	private CarController cc;

	// Token: 0x040003D5 RID: 981
	private Transform myTransform;
}
