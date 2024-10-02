using System;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class TNTPlace : MonoBehaviour
{
	// Token: 0x0600034B RID: 843 RVA: 0x0003A554 File Offset: 0x00038754
	private void Awake()
	{
		this.timer = Time.time;
		this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		this.obj = base.gameObject;
		this.goPlayer = GameObject.Find("Player");
		this.goMainCamera = this.goPlayer.GetComponentInChildren<Camera>();
		this.TNT_place[0] = (Texture2D)Resources.Load("GUI/tnt_arrow");
		this.TNT_place[1] = (Texture2D)Resources.Load("GUI/defend_arrow");
	}

	// Token: 0x0600034C RID: 844 RVA: 0x0003A5E1 File Offset: 0x000387E1
	private void OnGUI()
	{
		if (this.goMainCamera == null)
		{
			return;
		}
		this.DrawTNTPlace(this.obj.transform.position, (int)this.dist);
	}

	// Token: 0x0600034D RID: 845 RVA: 0x0003A610 File Offset: 0x00038810
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

	// Token: 0x0600034E RID: 846 RVA: 0x0003A678 File Offset: 0x00038878
	private void DrawTNTPlace(Vector3 p, int dist)
	{
		if (dist <= 1)
		{
			dist = 1;
		}
		if (dist >= 200)
		{
			dist = 200;
		}
		p.y += 1f;
		Vector3 vector = this.goMainCamera.GetComponent<Camera>().WorldToScreenPoint(p);
		vector.y = (float)Screen.height - vector.y;
		if (Vector3.Angle(p - this.goPlayer.transform.position, this.goPlayer.transform.forward) < 90f)
		{
			float num = ((float)dist + -206.72f) / -3.23f;
			if (num < 24f)
			{
				num = 24f;
			}
			int team = this.cscl.cspc.GetTeam();
			if (team == 0 || team == 1)
			{
				GUI.DrawTexture(new Rect(vector.x - num / 2f, vector.y, num, num), this.TNT_place[team]);
			}
		}
	}

	// Token: 0x040005FE RID: 1534
	public int id;

	// Token: 0x040005FF RID: 1535
	public int uid;

	// Token: 0x04000600 RID: 1536
	public int entid;

	// Token: 0x04000601 RID: 1537
	private GameObject obj;

	// Token: 0x04000602 RID: 1538
	private float timer;

	// Token: 0x04000603 RID: 1539
	private GameObject goPlayer;

	// Token: 0x04000604 RID: 1540
	private Camera goMainCamera;

	// Token: 0x04000605 RID: 1541
	private float dist;

	// Token: 0x04000606 RID: 1542
	public Texture2D[] TNT_place = new Texture2D[2];

	// Token: 0x04000607 RID: 1543
	private Client cscl;
}
