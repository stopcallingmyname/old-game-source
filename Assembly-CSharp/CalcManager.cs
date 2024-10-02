using System;

// Token: 0x0200003D RID: 61
internal class CalcManager
{
	// Token: 0x060001E7 RID: 487 RVA: 0x00024CEC File Offset: 0x00022EEC
	public static int CalcLevel(int exp)
	{
		int num = 1;
		while (exp >= (num * (num + 1) * (num + 2) + 15 * num) * 10)
		{
			num++;
		}
		return num;
	}
}
