using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x020000C5 RID: 197
public class vp_Message<V> : vp_Message
{
	// Token: 0x0600068C RID: 1676 RVA: 0x0000248C File Offset: 0x0000068C
	protected static void Empty<T>(T value)
	{
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x0006CC48 File Offset: 0x0006AE48
	public vp_Message(string name) : base(name)
	{
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x0006CC54 File Offset: 0x0006AE54
	protected override void InitFields()
	{
		this.m_Fields = new FieldInfo[]
		{
			base.Type.GetField("Send")
		};
		base.StoreInvokerFieldNames();
		this.m_DefaultMethods = new MethodInfo[]
		{
			base.GetStaticGenericMethod(base.Type, "Empty", this.m_ArgumentType, typeof(void))
		};
		this.m_DelegateTypes = new Type[]
		{
			typeof(vp_Message<>.Sender<>)
		};
		this.Prefixes = new Dictionary<string, int>
		{
			{
				"OnMessage_",
				0
			}
		};
		this.Send = new vp_Message<V>.Sender<V>(vp_Message<V>.Empty<V>);
		if (this.m_DefaultMethods != null && this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x0006CD34 File Offset: 0x0006AF34
	public override void Register(object t, string m, int v)
	{
		if (m == null)
		{
			return;
		}
		base.AddExternalMethodToField(t, this.m_Fields[v], m, base.MakeGenericType(this.m_DelegateTypes[v]));
		base.Refresh();
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x0006CC31 File Offset: 0x0006AE31
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04000C93 RID: 3219
	public new vp_Message<V>.Sender<V> Send;

	// Token: 0x020008A2 RID: 2210
	// (Invoke) Token: 0x06004CD4 RID: 19668
	public delegate void Sender<T>(T value);
}
