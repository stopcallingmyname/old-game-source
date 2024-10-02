using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class Minigun : MonoBehaviour
{
	// Token: 0x06000312 RID: 786 RVA: 0x000394C8 File Offset: 0x000376C8
	private void Update()
	{
		this.cannon.Rotate(Vector3.forward * (float)this.speed * Time.deltaTime);
		if (this.S == null)
		{
			this.S = GameObject.Find("Player").GetComponent<Sound>();
		}
		if (this.AS == null)
		{
			this.AS = GameObject.Find("Player/MainCamera/TMPAudio").GetComponent<AudioSource>();
		}
		if (this.speed > 0)
		{
			float num = (float)(this.speed - 50) / 800f;
			if (num < 0f)
			{
				num = 0f;
			}
			this.S.PlaySound_Pich(num, this.AS);
			if (this.speedUp)
			{
				this.S.PlaySound_MinigunMotor(this.AS);
			}
		}
		if (!this.speedUp && this.speed > 0)
		{
			this.speed -= this.speed / 100 + 1;
			if (this.speed <= 0)
			{
				this.speed = 0;
			}
			if (this.speed <= 100)
			{
				this.S.PlaySound_Stop(this.AS);
			}
		}
	}

	// Token: 0x040005B3 RID: 1459
	public Transform cannon;

	// Token: 0x040005B4 RID: 1460
	public int speed;

	// Token: 0x040005B5 RID: 1461
	public bool speedUp;

	// Token: 0x040005B6 RID: 1462
	private AudioSource AS;

	// Token: 0x040005B7 RID: 1463
	private Sound S;
}
