using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class SnowFlake : MonoBehaviour
{
	// Token: 0x06000585 RID: 1413 RVA: 0x00066C24 File Offset: 0x00064E24
	private void Awake()
	{
		this.active = true;
		this.sc_width = (float)Screen.width;
		this.sc_height = (float)Screen.height;
		for (int i = 0; i < 6; i++)
		{
			this.arr_tex[i] = (Resources.Load("GUI/ValentinesDay/heart" + Random.Range(1, 3)) as Texture2D);
		}
		this.num = (float)Random.Range(20, 60);
		int num = 0;
		while ((float)num < this.num)
		{
			int num2 = Random.Range(8, 32);
			this.Snow13.Add(new flakesnow(this.arr_tex[Random.Range(0, 6)], new Rect(Random.Range(0f, this.sc_width), Random.Range(0f, this.sc_height), (float)num2, (float)num2)));
			num++;
		}
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x00066CF6 File Offset: 0x00064EF6
	private void OnResize()
	{
		this.sc_width = (float)Screen.width;
		this.sc_height = (float)Screen.height;
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x00066D10 File Offset: 0x00064F10
	private void FixedUpdate()
	{
		for (int i = 0; i < this.Snow13.Count; i++)
		{
			Vector2 vector = new Vector2(this.Snow13[i].rec_flake.x, this.Snow13[i].rec_flake.y);
			Vector2 b = new Vector2(this.Snow13[i].rec_flake.x + Random.Range(-5f, 5f), this.Snow13[i].rec_flake.y + Random.Range(10f, 10f));
			vector = Vector2.Lerp(vector, b, 5f * Time.deltaTime);
			this.Snow13[i].rec_flake.x = vector.x;
			this.Snow13[i].rec_flake.y = vector.y;
			if (this.Snow13[i].rec_flake.y > this.sc_height - 50f || this.Snow13[i].rec_flake.x > this.sc_width - 50f || this.Snow13[i].rec_flake.x < 50f)
			{
				this.Snow13[i].tex_flake = this.arr_tex[Random.Range(0, 6)];
				this.Snow13[i].rec_flake = new Rect(Random.Range(0f, this.sc_width), 2f, 32f, 32f);
			}
		}
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x00066EC4 File Offset: 0x000650C4
	public void OnGUI()
	{
		if (!this.active)
		{
			return;
		}
		for (int i = 0; i < this.Snow13.Count; i++)
		{
			GUI.DrawTexture(this.Snow13[i].rec_flake, this.Snow13[i].tex_flake);
		}
	}

	// Token: 0x04000A05 RID: 2565
	public List<flakesnow> Snow13 = new List<flakesnow>();

	// Token: 0x04000A06 RID: 2566
	public bool active;

	// Token: 0x04000A07 RID: 2567
	private float sc_width;

	// Token: 0x04000A08 RID: 2568
	private float sc_height;

	// Token: 0x04000A09 RID: 2569
	private float num;

	// Token: 0x04000A0A RID: 2570
	private Texture2D[] arr_tex = new Texture2D[6];
}
