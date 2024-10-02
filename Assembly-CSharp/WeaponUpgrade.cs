using System;

// Token: 0x0200003A RID: 58
public class WeaponUpgrade
{
	// Token: 0x060001E1 RID: 481 RVA: 0x000246EB File Offset: 0x000228EB
	public WeaponUpgrade(int _Val, int _Cost)
	{
		this.Val = _Val;
		if (this.Val <= 0)
		{
			this.Val = 1;
		}
		this.Cost = _Cost;
		this.is_active = true;
	}

	// Token: 0x04000190 RID: 400
	public bool is_active;

	// Token: 0x04000191 RID: 401
	public int Val;

	// Token: 0x04000192 RID: 402
	public int Cost;
}
