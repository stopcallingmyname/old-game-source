using System;
using UnityEngine;

// Token: 0x0200012C RID: 300
public class HeadBob : MonoBehaviour
{
	// Token: 0x06000A6F RID: 2671 RVA: 0x00085628 File Offset: 0x00083828
	private void Update()
	{
		if (!this.Active)
		{
			return;
		}
		int tickCount = Environment.TickCount;
		if (tickCount < this.oldtime + 10)
		{
			return;
		}
		float num = (float)(tickCount - this.oldtime);
		this.oldtime = tickCount;
		float num2 = 0f;
		float axis = Input.GetAxis("Horizontal");
		float axis2 = Input.GetAxis("Vertical");
		if (Mathf.Abs(axis) == 0f && Mathf.Abs(axis2) == 0f)
		{
			this.timer = 0f;
		}
		else
		{
			num2 = Mathf.Sin(this.timer);
			this.timer += this.bobbingSpeed * num * 0.1f;
			if (this.timer > 6.2831855f)
			{
				this.timer -= 6.2831855f;
			}
		}
		if (num2 != 0f)
		{
			float num3 = num2 * this.bobbingAmount;
			num3 = Mathf.Clamp(Mathf.Abs(axis) + Mathf.Abs(axis2), 0f, 1f) * num3;
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, this.midpoint + num3, base.transform.localPosition.z);
			return;
		}
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, this.midpoint, base.transform.localPosition.z);
	}

	// Token: 0x04001080 RID: 4224
	public bool Active;

	// Token: 0x04001081 RID: 4225
	private float timer;

	// Token: 0x04001082 RID: 4226
	private float bobbingSpeed = 0.075f;

	// Token: 0x04001083 RID: 4227
	private float bobbingAmount = 0.07f;

	// Token: 0x04001084 RID: 4228
	private float midpoint = 1.75f;

	// Token: 0x04001085 RID: 4229
	private int oldtime;
}
