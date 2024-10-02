using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x020000C2 RID: 194
public class vp_EventDump
{
	// Token: 0x06000677 RID: 1655 RVA: 0x0006BDA8 File Offset: 0x00069FA8
	public static string Dump(vp_EventHandler handler, string[] eventTypes)
	{
		string text = "";
		foreach (string a in eventTypes)
		{
			if (!(a == "vp_Message"))
			{
				if (!(a == "vp_Attempt"))
				{
					if (!(a == "vp_Value"))
					{
						if (a == "vp_Activity")
						{
							text += vp_EventDump.DumpEventsOfType("vp_Activity", (eventTypes.Length > 1) ? "ACTIVITIES:\n\n" : "", handler);
						}
					}
					else
					{
						text += vp_EventDump.DumpEventsOfType("vp_Value", (eventTypes.Length > 1) ? "VALUES:\n\n" : "", handler);
					}
				}
				else
				{
					text += vp_EventDump.DumpEventsOfType("vp_Attempt", (eventTypes.Length > 1) ? "ATTEMPTS:\n\n" : "", handler);
				}
			}
			else
			{
				text += vp_EventDump.DumpEventsOfType("vp_Message", (eventTypes.Length > 1) ? "MESSAGES:\n\n" : "", handler);
			}
		}
		return text;
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x0006BEA8 File Offset: 0x0006A0A8
	private static string DumpEventsOfType(string type, string caption, vp_EventHandler handler)
	{
		string text = caption.ToUpper();
		foreach (FieldInfo fieldInfo in handler.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
		{
			string text2 = null;
			if (!(type == "vp_Message"))
			{
				if (!(type == "vp_Attempt"))
				{
					if (!(type == "vp_Value"))
					{
						if (type == "vp_Activity")
						{
							if (fieldInfo.FieldType.ToString().Contains("vp_Activity"))
							{
								text2 = vp_EventDump.DumpEventListeners((vp_Event)fieldInfo.GetValue(handler), new string[]
								{
									"StartConditions",
									"StopConditions",
									"StartCallbacks",
									"StopCallbacks"
								});
							}
						}
					}
					else if (fieldInfo.FieldType.ToString().Contains("vp_Value"))
					{
						text2 = vp_EventDump.DumpEventListeners((vp_Event)fieldInfo.GetValue(handler), new string[]
						{
							"Get",
							"Set"
						});
					}
				}
				else if (fieldInfo.FieldType.ToString().Contains("vp_Attempt"))
				{
					text2 = vp_EventDump.DumpEventListeners((vp_Event)fieldInfo.GetValue(handler), new string[]
					{
						"Try"
					});
				}
			}
			else if (fieldInfo.FieldType.ToString().Contains("vp_Message"))
			{
				text2 = vp_EventDump.DumpEventListeners((vp_Message)fieldInfo.GetValue(handler), new string[]
				{
					"Send"
				});
			}
			if (!string.IsNullOrEmpty(text2))
			{
				text = string.Concat(new string[]
				{
					text,
					"\t\t",
					fieldInfo.Name,
					"\n",
					text2,
					"\n"
				});
			}
		}
		return text;
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0006C078 File Offset: 0x0006A278
	private static string DumpEventListeners(object e, string[] invokers)
	{
		Type type = e.GetType();
		string text = "";
		foreach (string text2 in invokers)
		{
			FieldInfo field = type.GetField(text2);
			if (field == null)
			{
				return "";
			}
			string[] methodNames = vp_EventDump.GetMethodNames(((Delegate)field.GetValue(e)).GetInvocationList());
			text += "\t\t\t\t";
			if (type.ToString().Contains("vp_Value"))
			{
				if (!(text2 == "Get"))
				{
					if (!(text2 == "Set"))
					{
						text += "Unsupported listener: ";
					}
					else
					{
						text += "Set";
					}
				}
				else
				{
					text += "Get";
				}
			}
			else if (type.ToString().Contains("vp_Attempt"))
			{
				text += "Try";
			}
			else if (type.ToString().Contains("vp_Message"))
			{
				text += "Send";
			}
			else if (type.ToString().Contains("vp_Activity"))
			{
				if (!(text2 == "StartConditions"))
				{
					if (!(text2 == "StopConditions"))
					{
						if (!(text2 == "StartCallbacks"))
						{
							if (!(text2 == "StopCallbacks"))
							{
								text += "Unsupported listener: ";
							}
							else
							{
								text += "Stop";
							}
						}
						else
						{
							text += "Start";
						}
					}
					else
					{
						text += "TryStop";
					}
				}
				else
				{
					text += "TryStart";
				}
			}
			else
			{
				text += "Unsupported listener";
			}
			if (methodNames.Length > 2)
			{
				text += ":\n";
			}
			else
			{
				text += ": ";
			}
			text += vp_EventDump.DumpDelegateNames(methodNames);
		}
		return text;
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0006C268 File Offset: 0x0006A468
	private static string[] GetMethodNames(Delegate[] list)
	{
		list = vp_EventDump.RemoveDelegatesFromList(list);
		string[] array = new string[list.Length];
		if (list.Length == 1)
		{
			array[0] = ((list[0].Target == null) ? "" : ("(" + list[0].Target + ") ")) + list[0].Method.Name;
		}
		else
		{
			for (int i = 1; i < list.Length; i++)
			{
				array[i] = ((list[i].Target == null) ? "" : ("(" + list[i].Target + ") ")) + list[i].Method.Name;
			}
		}
		return array;
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x0006C318 File Offset: 0x0006A518
	private static Delegate[] RemoveDelegatesFromList(Delegate[] list)
	{
		List<Delegate> list2 = new List<Delegate>(list);
		for (int i = list2.Count - 1; i > -1; i--)
		{
			if (list2[i] != null && list2[i].Method.Name.Contains("m_"))
			{
				list2.RemoveAt(i);
			}
		}
		return list2.ToArray();
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x0006C374 File Offset: 0x0006A574
	private static string DumpDelegateNames(string[] array)
	{
		string text = "";
		foreach (string text2 in array)
		{
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + ((array.Length > 2) ? "\t\t\t\t\t\t\t" : "") + text2 + "\n";
			}
		}
		return text;
	}
}
