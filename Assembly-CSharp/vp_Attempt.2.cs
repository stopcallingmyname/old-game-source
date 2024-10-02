using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class vp_Attempt<V> : vp_Attempt
{
	// Token: 0x0600065E RID: 1630 RVA: 0x0006AE98 File Offset: 0x00069098
	protected static bool AlwaysOK<T>(T value)
	{
		return true;
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x0006B637 File Offset: 0x00069837
	public vp_Attempt(string name) : base(name)
	{
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x0006B640 File Offset: 0x00069840
	protected override void InitFields()
	{
		this.m_Fields = new FieldInfo[]
		{
			base.Type.GetField("Try")
		};
		base.StoreInvokerFieldNames();
		this.m_DefaultMethods = new MethodInfo[]
		{
			base.GetStaticGenericMethod(base.Type, "AlwaysOK", this.m_ArgumentType, typeof(bool))
		};
		this.m_DelegateTypes = new Type[]
		{
			typeof(vp_Attempt<>.Tryer<>)
		};
		this.Prefixes = new Dictionary<string, int>
		{
			{
				"OnAttempt_",
				0
			}
		};
		if (this.m_DefaultMethods != null && this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x0006B710 File Offset: 0x00069910
	public override void Register(object t, string m, int v)
	{
		if (((Delegate)this.m_Fields[v].GetValue(this)).Method.Name != this.m_DefaultMethods[v].Name)
		{
			Debug.LogWarning("Warning: Event '" + base.EventName + "' of type (vp_Attempt) targets multiple methods. Events of this type must reference a single method (only the last reference will be functional).");
		}
		if (m != null)
		{
			base.SetFieldToExternalMethod(t, this.m_Fields[0], m, base.MakeGenericType(this.m_DelegateTypes[v]));
		}
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x0006B789 File Offset: 0x00069989
	public override void Unregister(object t)
	{
		if (this.m_DefaultMethods != null && this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
	}

	// Token: 0x04000C82 RID: 3202
	public new vp_Attempt<V>.Tryer<V> Try;

	// Token: 0x0200089F RID: 2207
	// (Invoke) Token: 0x06004CCA RID: 19658
	public delegate bool Tryer<T>(T value);
}
