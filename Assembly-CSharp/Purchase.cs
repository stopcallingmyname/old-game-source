using System;

// Token: 0x02000014 RID: 20
public class Purchase
{
	// Token: 0x0600005B RID: 91 RVA: 0x00004742 File Offset: 0x00002942
	public Purchase(int _PID, int _PTime, int _PMoney, int _PCost, int _PItem, int _PStatus)
	{
		this.PID = _PID;
		this.PTime = _PTime;
		this.PMoney = _PMoney;
		this.PCost = _PCost;
		this.PItem = _PItem;
		this.PStatus = _PStatus;
		this.PUpdating = false;
	}

	// Token: 0x0400004E RID: 78
	public int PID;

	// Token: 0x0400004F RID: 79
	public int PTime;

	// Token: 0x04000050 RID: 80
	public int PMoney;

	// Token: 0x04000051 RID: 81
	public int PCost;

	// Token: 0x04000052 RID: 82
	public int PItem;

	// Token: 0x04000053 RID: 83
	public int PStatus;

	// Token: 0x04000054 RID: 84
	public bool PUpdating;
}
