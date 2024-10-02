using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x020000C8 RID: 200
public class vp_Value<V> : vp_Event
{
	// Token: 0x060006A1 RID: 1697 RVA: 0x0006D23C File Offset: 0x0006B43C
	protected static T Empty<T>()
	{
		return default(T);
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x0000248C File Offset: 0x0000068C
	protected static void Empty<T>(T value)
	{
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0006D252 File Offset: 0x0006B452
	private FieldInfo[] Fields
	{
		get
		{
			return this.m_Fields;
		}
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x0006B559 File Offset: 0x00069759
	public vp_Value(string name) : base(name)
	{
		this.InitFields();
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x0006D25C File Offset: 0x0006B45C
	protected override void InitFields()
	{
		this.m_Fields = new FieldInfo[]
		{
			base.GetType().GetField("Get"),
			base.GetType().GetField("Set")
		};
		base.StoreInvokerFieldNames();
		this.m_DelegateTypes = new Type[]
		{
			typeof(vp_Value<>.Getter<>),
			typeof(vp_Value<>.Setter<>)
		};
		this.m_DefaultMethods = new MethodInfo[]
		{
			base.GetStaticGenericMethod(base.Type, "Empty", typeof(void), this.m_ArgumentType),
			base.GetStaticGenericMethod(base.Type, "Empty", this.m_ArgumentType, typeof(void))
		};
		this.Prefixes = new Dictionary<string, int>
		{
			{
				"get_OnValue_",
				0
			},
			{
				"set_OnValue_",
				1
			}
		};
		if (this.m_DefaultMethods != null && this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
		if (this.m_DefaultMethods != null && this.m_DefaultMethods[1] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[1], this.m_DefaultMethods[1], base.MakeGenericType(this.m_DelegateTypes[1]));
		}
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x0006D3B6 File Offset: 0x0006B5B6
	public override void Register(object t, string m, int v)
	{
		if (m == null)
		{
			return;
		}
		base.SetFieldToExternalMethod(t, this.m_Fields[v], m, base.MakeGenericType(this.m_DelegateTypes[v]));
		base.Refresh();
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x0006D3E0 File Offset: 0x0006B5E0
	public override void Unregister(object t)
	{
		if (this.m_DefaultMethods != null && this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
		if (this.m_DefaultMethods != null && this.m_DefaultMethods[1] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[1], this.m_DefaultMethods[1], base.MakeGenericType(this.m_DelegateTypes[1]));
		}
		base.Refresh();
	}

	// Token: 0x04000C98 RID: 3224
	public vp_Value<V>.Getter<V> Get;

	// Token: 0x04000C99 RID: 3225
	public vp_Value<V>.Setter<V> Set;

	// Token: 0x020008A6 RID: 2214
	// (Invoke) Token: 0x06004CE0 RID: 19680
	public delegate T Getter<T>();

	// Token: 0x020008A7 RID: 2215
	// (Invoke) Token: 0x06004CE4 RID: 19684
	public delegate void Setter<T>(T o);
}
