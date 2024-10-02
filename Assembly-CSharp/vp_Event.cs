using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x020000C1 RID: 193
public abstract class vp_Event
{
	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000663 RID: 1635 RVA: 0x0006B7C7 File Offset: 0x000699C7
	public string EventName
	{
		get
		{
			return this.m_Name;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000664 RID: 1636 RVA: 0x0006B7CF File Offset: 0x000699CF
	public Type Type
	{
		get
		{
			return this.m_Type;
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000665 RID: 1637 RVA: 0x0006B7D7 File Offset: 0x000699D7
	public Type ArgumentType
	{
		get
		{
			return this.m_ArgumentType;
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000666 RID: 1638 RVA: 0x0006B7DF File Offset: 0x000699DF
	public Type ReturnType
	{
		get
		{
			return this.m_ReturnType;
		}
	}

	// Token: 0x06000667 RID: 1639
	public abstract void Register(object target, string method, int variant);

	// Token: 0x06000668 RID: 1640
	public abstract void Unregister(object target);

	// Token: 0x06000669 RID: 1641
	protected abstract void InitFields();

	// Token: 0x0600066A RID: 1642 RVA: 0x0006B7E7 File Offset: 0x000699E7
	public vp_Event(string name = "")
	{
		this.m_Type = base.GetType();
		this.m_ArgumentType = this.GetArgumentType;
		this.m_ReturnType = this.GetGenericReturnType;
		this.m_Name = name;
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x0006B81C File Offset: 0x00069A1C
	protected void StoreInvokerFieldNames()
	{
		this.InvokerFieldNames = new string[this.m_Fields.Length];
		for (int i = 0; i < this.m_Fields.Length; i++)
		{
			this.InvokerFieldNames[i] = this.m_Fields[i].Name;
		}
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x0006B864 File Offset: 0x00069A64
	protected Type MakeGenericType(Type type)
	{
		if (this.m_ReturnType == typeof(void))
		{
			return type.MakeGenericType(new Type[]
			{
				this.m_ArgumentType,
				this.m_ArgumentType
			});
		}
		return type.MakeGenericType(new Type[]
		{
			this.m_ArgumentType,
			this.m_ReturnType,
			this.m_ArgumentType,
			this.m_ReturnType
		});
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x0006B8D8 File Offset: 0x00069AD8
	protected void SetFieldToExternalMethod(object target, FieldInfo field, string method, Type type)
	{
		Delegate @delegate = Delegate.CreateDelegate(type, target, method, false, false);
		if (@delegate == null)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Error (",
				this,
				") Failed to bind: ",
				target,
				" -> ",
				method,
				"."
			}));
			return;
		}
		field.SetValue(this, @delegate);
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x0006B93C File Offset: 0x00069B3C
	protected void AddExternalMethodToField(object target, FieldInfo field, string method, Type type)
	{
		Delegate @delegate = Delegate.Combine((Delegate)field.GetValue(this), Delegate.CreateDelegate(type, target, method, false, false));
		if (@delegate == null)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Error (",
				this,
				") Failed to bind: ",
				target,
				" -> ",
				method,
				"."
			}));
			return;
		}
		field.SetValue(this, @delegate);
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x0006B9B0 File Offset: 0x00069BB0
	protected void SetFieldToLocalMethod(FieldInfo field, MethodInfo method, Type type)
	{
		Delegate @delegate = Delegate.CreateDelegate(type, method);
		if (@delegate == null)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Error (",
				this,
				") Failed to bind: ",
				method,
				"."
			}));
			return;
		}
		field.SetValue(this, @delegate);
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x0006BA04 File Offset: 0x00069C04
	protected void RemoveExternalMethodFromField(object target, FieldInfo field)
	{
		List<Delegate> list = new List<Delegate>(((Delegate)field.GetValue(this)).GetInvocationList());
		if (list == null)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Error (",
				this,
				") Failed to remove: ",
				target,
				" -> ",
				field.Name,
				"."
			}));
			return;
		}
		for (int i = list.Count - 1; i > -1; i--)
		{
			if (list[i].Target == target)
			{
				list.Remove(list[i]);
			}
		}
		if (list != null)
		{
			field.SetValue(this, Delegate.Combine(list.ToArray()));
		}
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x0006BAB4 File Offset: 0x00069CB4
	protected MethodInfo GetStaticGenericMethod(Type e, string name, Type parameterType, Type returnType)
	{
		foreach (MethodInfo methodInfo in e.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			if (!(methodInfo.Name != name))
			{
				MethodInfo methodInfo2;
				if (this.GetGenericReturnType == typeof(void))
				{
					methodInfo2 = methodInfo.MakeGenericMethod(new Type[]
					{
						this.m_ArgumentType
					});
				}
				else
				{
					methodInfo2 = methodInfo.MakeGenericMethod(new Type[]
					{
						this.m_ArgumentType,
						this.m_ReturnType
					});
				}
				if (methodInfo2.GetParameters().Length <= 1 && (methodInfo2.GetParameters().Length != 1 || !(parameterType == typeof(void))) && (methodInfo2.GetParameters().Length != 0 || !(parameterType != typeof(void))) && (methodInfo2.GetParameters().Length != 1 || !(methodInfo2.GetParameters()[0].ParameterType != parameterType)) && !(returnType != methodInfo2.ReturnType))
				{
					return methodInfo2;
				}
			}
		}
		return null;
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000672 RID: 1650 RVA: 0x0006BBB6 File Offset: 0x00069DB6
	private Type GetArgumentType
	{
		get
		{
			if (!this.Type.IsGenericType)
			{
				return typeof(void);
			}
			return base.GetType().GetGenericArguments()[0];
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x06000673 RID: 1651 RVA: 0x0006BBE0 File Offset: 0x00069DE0
	private Type GetGenericReturnType
	{
		get
		{
			if (!this.Type.IsGenericType)
			{
				return typeof(void);
			}
			if (this.Type.GetGenericArguments().Length != 2)
			{
				return typeof(void);
			}
			return this.Type.GetGenericArguments()[1];
		}
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0006BC30 File Offset: 0x00069E30
	public Type GetParameterType(int index)
	{
		if (!this.Type.IsGenericType)
		{
			return typeof(void);
		}
		if (index > this.m_Fields.Length - 1)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Error: (",
				this,
				") Event '",
				this.EventName,
				"' only supports ",
				this.m_Fields.Length,
				" indices. 'GetParameterType' referenced index ",
				index,
				"."
			}));
		}
		if (this.m_DelegateTypes[index].GetMethod("Invoke").GetParameters().Length == 0)
		{
			return typeof(void);
		}
		return this.m_ArgumentType;
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x0006BCEC File Offset: 0x00069EEC
	public Type GetReturnType(int index)
	{
		if (index > this.m_Fields.Length - 1)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Error: (",
				this,
				") Event '",
				this.EventName,
				"' only supports ",
				this.m_Fields.Length,
				" indices. 'GetReturnType' referenced index ",
				index,
				"."
			}));
			return null;
		}
		if (this.Type.GetGenericArguments().Length > 1)
		{
			return this.GetGenericReturnType;
		}
		Type returnType = this.m_DelegateTypes[index].GetMethod("Invoke").ReturnType;
		if (returnType.IsGenericParameter)
		{
			return this.m_ArgumentType;
		}
		return returnType;
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x0000248C File Offset: 0x0000068C
	protected void Refresh()
	{
	}

	// Token: 0x04000C83 RID: 3203
	protected string m_Name;

	// Token: 0x04000C84 RID: 3204
	protected Type m_Type;

	// Token: 0x04000C85 RID: 3205
	protected Type m_ArgumentType;

	// Token: 0x04000C86 RID: 3206
	protected Type m_ReturnType;

	// Token: 0x04000C87 RID: 3207
	protected FieldInfo[] m_Fields;

	// Token: 0x04000C88 RID: 3208
	protected Type[] m_DelegateTypes;

	// Token: 0x04000C89 RID: 3209
	protected MethodInfo[] m_DefaultMethods;

	// Token: 0x04000C8A RID: 3210
	public string[] InvokerFieldNames;

	// Token: 0x04000C8B RID: 3211
	public Dictionary<string, int> Prefixes;
}
