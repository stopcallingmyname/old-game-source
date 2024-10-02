using System;

// Token: 0x020000BE RID: 190
public class vp_Activity<V> : vp_Activity
{
	// Token: 0x06000657 RID: 1623 RVA: 0x0006B531 File Offset: 0x00069731
	public vp_Activity(string name) : base(name)
	{
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x0006B53A File Offset: 0x0006973A
	public bool TryStart<T>(T argument)
	{
		if (this.m_Active)
		{
			return false;
		}
		this.m_Argument = argument;
		return base.TryStart(true);
	}
}
