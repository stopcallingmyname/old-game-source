using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x020000C6 RID: 198
public class vp_Message<V, VResult> : vp_Message
{
	// Token: 0x06000691 RID: 1681 RVA: 0x0006CD60 File Offset: 0x0006AF60
	protected static TResult Empty<T, TResult>(T value)
	{
		return default(TResult);
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x0006CC48 File Offset: 0x0006AE48
	public vp_Message(string name) : base(name)
	{
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x0006CD78 File Offset: 0x0006AF78
	protected override void InitFields()
	{
		this.m_Fields = new FieldInfo[]
		{
			base.Type.GetField("Send")
		};
		base.StoreInvokerFieldNames();
		this.m_DefaultMethods = new MethodInfo[]
		{
			base.GetStaticGenericMethod(base.Type, "Empty", this.m_ArgumentType, this.m_ReturnType)
		};
		this.m_DelegateTypes = new Type[]
		{
			typeof(vp_Message<, >.Sender<, >)
		};
		this.Prefixes = new Dictionary<string, int>
		{
			{
				"OnMessage_",
				0
			}
		};
		if (this.m_DefaultMethods != null && this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x0006CE42 File Offset: 0x0006B042
	public override void Register(object t, string m, int v)
	{
		if (m == null)
		{
			return;
		}
		base.AddExternalMethodToField(t, this.m_Fields[0], m, base.MakeGenericType(this.m_DelegateTypes[0]));
		base.Refresh();
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0006CC31 File Offset: 0x0006AE31
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04000C94 RID: 3220
	public new vp_Message<V, VResult>.Sender<V, VResult> Send;

	// Token: 0x020008A3 RID: 2211
	// (Invoke) Token: 0x06004CD8 RID: 19672
	public delegate TResult Sender<T, TResult>(T value);
}
