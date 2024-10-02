using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x020000BF RID: 191
public class vp_Attempt : vp_Event
{
	// Token: 0x06000659 RID: 1625 RVA: 0x0006AE98 File Offset: 0x00069098
	protected static bool AlwaysOK()
	{
		return true;
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x0006B559 File Offset: 0x00069759
	public vp_Attempt(string name) : base(name)
	{
		this.InitFields();
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x0006B568 File Offset: 0x00069768
	protected override void InitFields()
	{
		this.m_Fields = new FieldInfo[]
		{
			base.Type.GetField("Try")
		};
		base.StoreInvokerFieldNames();
		this.m_DefaultMethods = new MethodInfo[]
		{
			base.Type.GetMethod("AlwaysOK")
		};
		this.m_DelegateTypes = new Type[]
		{
			typeof(vp_Attempt.Tryer)
		};
		this.Prefixes = new Dictionary<string, int>
		{
			{
				"OnAttempt_",
				0
			}
		};
		this.Try = new vp_Attempt.Tryer(vp_Attempt.AlwaysOK);
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x0006B5FB File Offset: 0x000697FB
	public override void Register(object t, string m, int v)
	{
		this.Try = (vp_Attempt.Tryer)Delegate.CreateDelegate(this.m_DelegateTypes[v], t, m);
		base.Refresh();
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x0006B61D File Offset: 0x0006981D
	public override void Unregister(object t)
	{
		this.Try = new vp_Attempt.Tryer(vp_Attempt.AlwaysOK);
		base.Refresh();
	}

	// Token: 0x04000C81 RID: 3201
	public vp_Attempt.Tryer Try;

	// Token: 0x0200089E RID: 2206
	// (Invoke) Token: 0x06004CC6 RID: 19654
	public delegate bool Tryer();
}
