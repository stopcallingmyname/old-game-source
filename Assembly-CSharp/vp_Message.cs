using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x020000C4 RID: 196
public class vp_Message : vp_Event
{
	// Token: 0x06000687 RID: 1671 RVA: 0x0000248C File Offset: 0x0000068C
	protected static void Empty()
	{
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x0006B559 File Offset: 0x00069759
	public vp_Message(string name) : base(name)
	{
		this.InitFields();
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x0006CB6C File Offset: 0x0006AD6C
	protected override void InitFields()
	{
		this.m_Fields = new FieldInfo[]
		{
			base.GetType().GetField("Send")
		};
		base.StoreInvokerFieldNames();
		this.m_DelegateTypes = new Type[]
		{
			typeof(vp_Message.Sender)
		};
		this.Prefixes = new Dictionary<string, int>
		{
			{
				"OnMessage_",
				0
			}
		};
		this.m_DefaultMethods = new MethodInfo[]
		{
			base.Type.GetMethod("Empty")
		};
		this.Send = new vp_Message.Sender(vp_Message.Empty);
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x0006CBFF File Offset: 0x0006ADFF
	public override void Register(object t, string m, int v)
	{
		this.Send = (vp_Message.Sender)Delegate.Combine(this.Send, (vp_Message.Sender)Delegate.CreateDelegate(this.m_DelegateTypes[v], t, m));
		base.Refresh();
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x0006CC31 File Offset: 0x0006AE31
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04000C92 RID: 3218
	public vp_Message.Sender Send;

	// Token: 0x020008A1 RID: 2209
	// (Invoke) Token: 0x06004CD0 RID: 19664
	public delegate void Sender();
}
