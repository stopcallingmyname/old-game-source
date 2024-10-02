using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public abstract class vp_EventHandler : MonoBehaviour
{
	// Token: 0x0600067E RID: 1662 RVA: 0x0006C3C4 File Offset: 0x0006A5C4
	protected virtual void Awake()
	{
		this.StoreHandlerEvents();
		this.m_Initialized = true;
		for (int i = this.m_PendingRegistrants.Count - 1; i > -1; i--)
		{
			this.Register(this.m_PendingRegistrants[i]);
			this.m_PendingRegistrants.Remove(this.m_PendingRegistrants[i]);
		}
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x0006C420 File Offset: 0x0006A620
	protected void StoreHandlerEvents()
	{
		object obj = null;
		List<FieldInfo> fields = this.GetFields();
		if (fields == null || fields.Count == 0)
		{
			return;
		}
		foreach (FieldInfo fieldInfo in fields)
		{
			try
			{
				obj = Activator.CreateInstance(fieldInfo.FieldType, new object[]
				{
					fieldInfo.Name
				});
			}
			catch
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Error: (",
					this,
					") does not support the type of '",
					fieldInfo.Name,
					"' in '",
					fieldInfo.DeclaringType,
					"'."
				}));
				continue;
			}
			if (obj != null)
			{
				fieldInfo.SetValue(this, obj);
				if (!this.m_Events.Contains((vp_Event)obj))
				{
					this.m_Events.Add((vp_Event)obj);
				}
				foreach (string str in ((vp_Event)obj).Prefixes.Keys)
				{
					this.m_EventsByCallback.Add(str + fieldInfo.Name, (vp_Event)obj);
				}
			}
		}
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x0006C5B4 File Offset: 0x0006A7B4
	public List<FieldInfo> GetFields()
	{
		List<FieldInfo> list = new List<FieldInfo>();
		Type type = base.GetType();
		Type type2 = null;
		do
		{
			if (type2 != null)
			{
				type = type2;
			}
			list.AddRange(type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
			if (type.BaseType != typeof(vp_StateEventHandler) && type.BaseType != typeof(vp_EventHandler))
			{
				type2 = type.BaseType;
			}
		}
		while (type.BaseType != typeof(vp_StateEventHandler) && type.BaseType != typeof(vp_EventHandler) && type != null);
		if (list == null || list.Count == 0)
		{
			Debug.LogWarning("Warning: (" + this + ") Found no fields to store as events.");
		}
		return list;
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x0006C67C File Offset: 0x0006A87C
	public void Register(object target)
	{
		if (target == null)
		{
			Debug.LogError("Error: (" + this + ") Target object was null.");
			return;
		}
		if (!this.m_Initialized)
		{
			this.m_PendingRegistrants.Add(target);
			return;
		}
		vp_EventHandler.ScriptMethods scriptMethods = this.GetScriptMethods(target);
		if (scriptMethods == null)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Error: (",
				this,
				") could not get script methods for '",
				target,
				"'."
			}));
			return;
		}
		foreach (MethodInfo methodInfo in scriptMethods.Events)
		{
			vp_Event vp_Event;
			if (this.m_EventsByCallback.TryGetValue(methodInfo.Name, out vp_Event))
			{
				int num;
				vp_Event.Prefixes.TryGetValue(methodInfo.Name.Substring(0, methodInfo.Name.IndexOf('_', 4) + 1), out num);
				if (this.CompareMethodSignatures(methodInfo, vp_Event.GetParameterType(num), vp_Event.GetReturnType(num)))
				{
					vp_Event.Register(target, methodInfo.Name, num);
				}
			}
		}
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x0006C79C File Offset: 0x0006A99C
	public void Unregister(object target)
	{
		if (target == null)
		{
			Debug.LogError("Error: (" + this + ") Target object was null.");
			return;
		}
		foreach (vp_Event vp_Event in this.m_Events)
		{
			if (vp_Event != null)
			{
				foreach (string name in vp_Event.InvokerFieldNames)
				{
					FieldInfo field = vp_Event.Type.GetField(name);
					if (!(field == null))
					{
						object value = field.GetValue(vp_Event);
						if (value != null)
						{
							Delegate @delegate = (Delegate)value;
							if (@delegate != null)
							{
								Delegate[] invocationList = @delegate.GetInvocationList();
								for (int j = 0; j < invocationList.Length; j++)
								{
									if (invocationList[j].Target == target)
									{
										vp_Event.Unregister(target);
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x0006C890 File Offset: 0x0006AA90
	protected bool CompareMethodSignatures(MethodInfo scriptMethod, Type handlerParameterType, Type handlerReturnType)
	{
		if (scriptMethod.ReturnType != handlerReturnType)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Error: (",
				scriptMethod.DeclaringType,
				") Return type (",
				vp_Utility.GetTypeAlias(scriptMethod.ReturnType),
				") is not valid for '",
				scriptMethod.Name,
				"'. Return type declared in event handler was: (",
				vp_Utility.GetTypeAlias(handlerReturnType),
				")."
			}));
			return false;
		}
		if (scriptMethod.GetParameters().Length == 1)
		{
			if (((ParameterInfo)scriptMethod.GetParameters().GetValue(0)).ParameterType != handlerParameterType)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Error: (",
					scriptMethod.DeclaringType,
					") Parameter type (",
					vp_Utility.GetTypeAlias(((ParameterInfo)scriptMethod.GetParameters().GetValue(0)).ParameterType),
					") is not valid for '",
					scriptMethod.Name,
					"'. Parameter type declared in event handler was: (",
					vp_Utility.GetTypeAlias(handlerParameterType),
					")."
				}));
				return false;
			}
		}
		else if (scriptMethod.GetParameters().Length == 0)
		{
			if (handlerParameterType != typeof(void))
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Error: (",
					scriptMethod.DeclaringType,
					") Can't register method '",
					scriptMethod.Name,
					"' with 0 parameters. Expected: 1 parameter of type (",
					vp_Utility.GetTypeAlias(handlerParameterType),
					")."
				}));
				return false;
			}
		}
		else if (scriptMethod.GetParameters().Length > 1)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Error: (",
				scriptMethod.DeclaringType,
				") Can't register method '",
				scriptMethod.Name,
				"' with ",
				scriptMethod.GetParameters().Length,
				" parameters. Max parameter count: 1 of type (",
				vp_Utility.GetTypeAlias(handlerParameterType),
				")."
			}));
			return false;
		}
		return true;
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x0006CA90 File Offset: 0x0006AC90
	protected vp_EventHandler.ScriptMethods GetScriptMethods(object target)
	{
		vp_EventHandler.ScriptMethods scriptMethods;
		if (!vp_EventHandler.m_StoredScriptTypes.TryGetValue(target.GetType(), out scriptMethods))
		{
			scriptMethods = new vp_EventHandler.ScriptMethods(target.GetType());
			vp_EventHandler.m_StoredScriptTypes.Add(target.GetType(), scriptMethods);
		}
		return scriptMethods;
	}

	// Token: 0x04000C8C RID: 3212
	protected bool m_Initialized;

	// Token: 0x04000C8D RID: 3213
	protected Dictionary<string, vp_Event> m_EventsByCallback = new Dictionary<string, vp_Event>();

	// Token: 0x04000C8E RID: 3214
	protected List<vp_Event> m_Events = new List<vp_Event>();

	// Token: 0x04000C8F RID: 3215
	protected List<object> m_PendingRegistrants = new List<object>();

	// Token: 0x04000C90 RID: 3216
	protected static Dictionary<Type, vp_EventHandler.ScriptMethods> m_StoredScriptTypes = new Dictionary<Type, vp_EventHandler.ScriptMethods>();

	// Token: 0x04000C91 RID: 3217
	protected static string[] m_SupportedPrefixes = new string[]
	{
		"OnMessage_",
		"CanStart_",
		"CanStop_",
		"OnStart_",
		"OnStop_",
		"OnAttempt_",
		"get_OnValue_",
		"set_OnValue_",
		"OnFailStart_",
		"OnFailStop_"
	};

	// Token: 0x020008A0 RID: 2208
	protected class ScriptMethods
	{
		// Token: 0x06004CCD RID: 19661 RVA: 0x001AD0AB File Offset: 0x001AB2AB
		public ScriptMethods(Type type)
		{
			this.Events = vp_EventHandler.ScriptMethods.GetMethods(type);
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x001AD0CC File Offset: 0x001AB2CC
		protected static List<MethodInfo> GetMethods(Type type)
		{
			List<MethodInfo> list = new List<MethodInfo>();
			List<string> list2 = new List<string>();
			MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			while (type != null)
			{
				foreach (MethodInfo methodInfo in methods)
				{
					if (!methodInfo.Name.Contains(">m__") && !list2.Contains(methodInfo.Name))
					{
						foreach (string value in vp_EventHandler.m_SupportedPrefixes)
						{
							if (methodInfo.Name.Contains(value))
							{
								list.Add(methodInfo);
								list2.Add(methodInfo.Name);
								break;
							}
						}
					}
				}
				type = type.BaseType;
			}
			return list;
		}

		// Token: 0x04003391 RID: 13201
		public List<MethodInfo> Events = new List<MethodInfo>();
	}
}
