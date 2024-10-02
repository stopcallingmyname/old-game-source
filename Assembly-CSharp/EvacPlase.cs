using System;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class EvacPlase : MonoBehaviour
{
	// Token: 0x060002C5 RID: 709 RVA: 0x00037D7C File Offset: 0x00035F7C
	private void Awake()
	{
		this.timer = Time.time;
		this.obj = base.gameObject;
		this.goPlayer = GameObject.Find("Player");
		this.goMainCamera = this.goPlayer.GetComponentInChildren<Camera>();
		this.evac_place = (Texture2D)Resources.Load("GUI/evacuate");
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x00037DD6 File Offset: 0x00035FD6
	private void OnGUI()
	{
		if (this.goMainCamera == null)
		{
			return;
		}
		if (!this.accepted)
		{
			return;
		}
		this.DrawFlagPlace(this.obj.transform.position, (int)this.dist);
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x00037E10 File Offset: 0x00036010
	private void Update()
	{
		if (this.goMainCamera == null)
		{
			return;
		}
		if (Time.time < this.timer + 1f)
		{
			return;
		}
		this.dist = Vector3.Distance(this.goPlayer.transform.position, this.obj.transform.position);
		this.timer = Time.time;
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x00037E78 File Offset: 0x00036078
	private void DrawFlagPlace(Vector3 p, int dist)
	{
		if (dist <= 1)
		{
			dist = 1;
		}
		if (dist >= 200)
		{
			dist = 200;
		}
		p.y += 6f;
		Vector3 vector = this.goMainCamera.GetComponent<Camera>().WorldToScreenPoint(p);
		vector.y = (float)Screen.height - vector.y;
		if (Vector3.Angle(p - this.goPlayer.transform.position, this.goPlayer.transform.forward) < 90f)
		{
			float num = ((float)dist + -206.72f) / -3.23f;
			if (num < 24f)
			{
				num = 24f;
			}
			GUI.DrawTexture(new Rect(vector.x - num / 2f, vector.y, num, num), this.evac_place);
		}
	}

	// Token: 0x0400054E RID: 1358
	private GameObject obj;

	// Token: 0x0400054F RID: 1359
	private float timer;

	// Token: 0x04000550 RID: 1360
	private GameObject goPlayer;

	// Token: 0x04000551 RID: 1361
	private Camera goMainCamera;

	// Token: 0x04000552 RID: 1362
	private float dist;

	// Token: 0x04000553 RID: 1363
	public Texture2D evac_place;

	// Token: 0x04000554 RID: 1364
	public bool accepted;
}
