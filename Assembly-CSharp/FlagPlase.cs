using System;
using UnityEngine;

// Token: 0x0200005F RID: 95
public class FlagPlase : MonoBehaviour
{
	// Token: 0x060002D0 RID: 720 RVA: 0x00037FBC File Offset: 0x000361BC
	private void Awake()
	{
		this.timer = Time.time;
		this.obj = base.gameObject;
		this.goPlayer = GameObject.Find("Player");
		this.goMainCamera = this.goPlayer.GetComponentInChildren<Camera>();
		this.flag_place[0] = (Texture2D)Resources.Load("GUI/bflag");
		this.flag_place[1] = (Texture2D)Resources.Load("GUI/rflag");
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x0003802F File Offset: 0x0003622F
	public void MyGUI()
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

	// Token: 0x060002D2 RID: 722 RVA: 0x00038068 File Offset: 0x00036268
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

	// Token: 0x060002D3 RID: 723 RVA: 0x000380D0 File Offset: 0x000362D0
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
		p.y += 12f;
		Vector3 vector = this.goMainCamera.GetComponent<Camera>().WorldToScreenPoint(p);
		vector.y = (float)Screen.height - vector.y;
		if (Vector3.Angle(p - this.goPlayer.transform.position, this.goPlayer.transform.forward) < 90f)
		{
			float num = ((float)dist + -206.72f) / -3.23f;
			if (num < 24f)
			{
				num = 24f;
			}
			if (this.team == 0 || this.team == 1)
			{
				GUI.DrawTexture(new Rect(vector.x - num / 2f, vector.y, num, num), this.flag_place[this.team]);
			}
		}
	}

	// Token: 0x0400055D RID: 1373
	private GameObject obj;

	// Token: 0x0400055E RID: 1374
	private float timer;

	// Token: 0x0400055F RID: 1375
	private GameObject goPlayer;

	// Token: 0x04000560 RID: 1376
	private Camera goMainCamera;

	// Token: 0x04000561 RID: 1377
	private float dist;

	// Token: 0x04000562 RID: 1378
	public Texture2D[] flag_place = new Texture2D[2];

	// Token: 0x04000563 RID: 1379
	public int team = 1;

	// Token: 0x04000564 RID: 1380
	public bool accepted;
}
