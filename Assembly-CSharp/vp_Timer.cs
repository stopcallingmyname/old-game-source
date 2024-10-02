using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000CE RID: 206
public class vp_Timer : MonoBehaviour
{
	// Token: 0x17000027 RID: 39
	// (get) Token: 0x060006D4 RID: 1748 RVA: 0x0006E720 File Offset: 0x0006C920
	public bool WasAddedCorrectly
	{
		get
		{
			return Application.isPlaying && !(base.gameObject != vp_Timer.m_MainObject);
		}
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x0006E740 File Offset: 0x0006C940
	private void Awake()
	{
		if (!this.WasAddedCorrectly)
		{
			Object.Destroy(this);
			return;
		}
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x0006E751 File Offset: 0x0006C951
	private void OnEnable()
	{
		SceneManager.sceneLoaded += this.OnLevelLoad;
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x0006E764 File Offset: 0x0006C964
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= this.OnLevelLoad;
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x0006E778 File Offset: 0x0006C978
	private void Update()
	{
		vp_Timer.m_EventBatch = 0;
		while (vp_Timer.m_Active.Count > 0 && vp_Timer.m_EventBatch < vp_Timer.MaxEventsPerFrame)
		{
			if (vp_Timer.m_EventIterator < 0)
			{
				vp_Timer.m_EventIterator = vp_Timer.m_Active.Count - 1;
				return;
			}
			if (vp_Timer.m_EventIterator > vp_Timer.m_Active.Count - 1)
			{
				vp_Timer.m_EventIterator = vp_Timer.m_Active.Count - 1;
			}
			if (Time.time >= vp_Timer.m_Active[vp_Timer.m_EventIterator].DueTime || vp_Timer.m_Active[vp_Timer.m_EventIterator].Id == 0)
			{
				vp_Timer.m_Active[vp_Timer.m_EventIterator].Execute();
			}
			else if (vp_Timer.m_Active[vp_Timer.m_EventIterator].Paused)
			{
				vp_Timer.m_Active[vp_Timer.m_EventIterator].DueTime += Time.deltaTime;
			}
			else
			{
				vp_Timer.m_Active[vp_Timer.m_EventIterator].LifeTime += Time.deltaTime;
			}
			vp_Timer.m_EventIterator--;
			vp_Timer.m_EventBatch++;
		}
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x0006E8A1 File Offset: 0x0006CAA1
	public static void In(float delay, vp_Timer.Callback callback, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, 1, -1f);
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x0006E8B3 File Offset: 0x0006CAB3
	public static void In(float delay, vp_Timer.Callback callback, int iterations, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, iterations, -1f);
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x0006E8C5 File Offset: 0x0006CAC5
	public static void In(float delay, vp_Timer.Callback callback, int iterations, float interval, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, callback, null, null, timerHandle, iterations, interval);
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x0006E8D4 File Offset: 0x0006CAD4
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, 1, -1f);
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x0006E8E6 File Offset: 0x0006CAE6
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, int iterations, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, iterations, -1f);
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x0006E8F9 File Offset: 0x0006CAF9
	public static void In(float delay, vp_Timer.ArgCallback callback, object arguments, int iterations, float interval, vp_Timer.Handle timerHandle = null)
	{
		vp_Timer.Schedule(delay, null, callback, arguments, timerHandle, iterations, interval);
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x0006E909 File Offset: 0x0006CB09
	public static void Start(vp_Timer.Handle timerHandle)
	{
		vp_Timer.Schedule(315360000f, delegate
		{
		}, null, null, timerHandle, 1, -1f);
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x0006E940 File Offset: 0x0006CB40
	private static void Schedule(float time, vp_Timer.Callback func, vp_Timer.ArgCallback argFunc, object args, vp_Timer.Handle timerHandle, int iterations, float interval)
	{
		if (func == null && argFunc == null)
		{
			Debug.LogError("Error: (vp_Timer) Aborted event because function is null.");
			return;
		}
		if (vp_Timer.m_MainObject == null)
		{
			if (vp_Timer.m_AppQuitting)
			{
				return;
			}
			vp_Timer.m_MainObject = new GameObject("Timers");
			vp_Timer.m_MainObject.AddComponent<vp_Timer>();
			Object.DontDestroyOnLoad(vp_Timer.m_MainObject);
		}
		time = Mathf.Max(0f, time);
		iterations = Mathf.Max(0, iterations);
		interval = ((interval == -1f) ? time : Mathf.Max(0f, interval));
		vp_Timer.m_NewEvent = null;
		if (vp_Timer.m_Pool.Count > 0)
		{
			vp_Timer.m_NewEvent = vp_Timer.m_Pool[0];
			vp_Timer.m_Pool.Remove(vp_Timer.m_NewEvent);
		}
		else
		{
			vp_Timer.m_NewEvent = new vp_Timer.Event();
		}
		vp_Timer.m_EventCount++;
		vp_Timer.m_NewEvent.Id = vp_Timer.m_EventCount;
		if (func != null)
		{
			vp_Timer.m_NewEvent.Function = func;
		}
		else if (argFunc != null)
		{
			vp_Timer.m_NewEvent.ArgFunction = argFunc;
			vp_Timer.m_NewEvent.Arguments = args;
		}
		vp_Timer.m_NewEvent.StartTime = Time.time;
		vp_Timer.m_NewEvent.DueTime = Time.time + time;
		vp_Timer.m_NewEvent.Iterations = iterations;
		vp_Timer.m_NewEvent.Interval = interval;
		vp_Timer.m_NewEvent.LifeTime = 0f;
		vp_Timer.m_NewEvent.Paused = false;
		vp_Timer.m_Active.Add(vp_Timer.m_NewEvent);
		if (timerHandle != null)
		{
			if (timerHandle.Active)
			{
				timerHandle.Cancel();
			}
			timerHandle.Id = vp_Timer.m_NewEvent.Id;
		}
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x0006EAD3 File Offset: 0x0006CCD3
	private static void Cancel(vp_Timer.Handle handle)
	{
		if (handle == null)
		{
			return;
		}
		if (handle.Active)
		{
			handle.Id = 0;
			return;
		}
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x0006EAEC File Offset: 0x0006CCEC
	public static void CancelAll()
	{
		for (int i = vp_Timer.m_Active.Count - 1; i > -1; i--)
		{
			vp_Timer.m_Active[i].Id = 0;
		}
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x0006EB24 File Offset: 0x0006CD24
	public static void CancelAll(string methodName)
	{
		for (int i = vp_Timer.m_Active.Count - 1; i > -1; i--)
		{
			if (vp_Timer.m_Active[i].MethodName == methodName)
			{
				vp_Timer.m_Active[i].Id = 0;
			}
		}
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x0006EB71 File Offset: 0x0006CD71
	public static void DestroyAll()
	{
		vp_Timer.m_Active.Clear();
		vp_Timer.m_Pool.Clear();
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x0006EB88 File Offset: 0x0006CD88
	private void OnLevelLoad(Scene scene, LoadSceneMode mode)
	{
		for (int i = vp_Timer.m_Active.Count - 1; i > -1; i--)
		{
			if (vp_Timer.m_Active[i].CancelOnLoad)
			{
				vp_Timer.m_Active[i].Id = 0;
			}
		}
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x0006EBD0 File Offset: 0x0006CDD0
	public static vp_Timer.Stats EditorGetStats()
	{
		vp_Timer.Stats result;
		result.Created = vp_Timer.m_Active.Count + vp_Timer.m_Pool.Count;
		result.Inactive = vp_Timer.m_Pool.Count;
		result.Active = vp_Timer.m_Active.Count;
		return result;
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x0006EC1C File Offset: 0x0006CE1C
	public static string EditorGetMethodInfo(int eventIndex)
	{
		if (eventIndex < 0 || eventIndex > vp_Timer.m_Active.Count - 1)
		{
			return "Argument out of range.";
		}
		return vp_Timer.m_Active[eventIndex].MethodInfo;
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x0006EC47 File Offset: 0x0006CE47
	public static int EditorGetMethodId(int eventIndex)
	{
		if (eventIndex < 0 || eventIndex > vp_Timer.m_Active.Count - 1)
		{
			return 0;
		}
		return vp_Timer.m_Active[eventIndex].Id;
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x0006EC6E File Offset: 0x0006CE6E
	private void OnApplicationQuit()
	{
		vp_Timer.m_AppQuitting = true;
	}

	// Token: 0x04000CC5 RID: 3269
	private static GameObject m_MainObject = null;

	// Token: 0x04000CC6 RID: 3270
	private static List<vp_Timer.Event> m_Active = new List<vp_Timer.Event>();

	// Token: 0x04000CC7 RID: 3271
	private static List<vp_Timer.Event> m_Pool = new List<vp_Timer.Event>();

	// Token: 0x04000CC8 RID: 3272
	private static vp_Timer.Event m_NewEvent = null;

	// Token: 0x04000CC9 RID: 3273
	private static int m_EventCount = 0;

	// Token: 0x04000CCA RID: 3274
	private static int m_EventBatch = 0;

	// Token: 0x04000CCB RID: 3275
	private static int m_EventIterator = 0;

	// Token: 0x04000CCC RID: 3276
	private static bool m_AppQuitting = false;

	// Token: 0x04000CCD RID: 3277
	public static int MaxEventsPerFrame = 500;

	// Token: 0x020008AB RID: 2219
	// (Invoke) Token: 0x06004CEC RID: 19692
	public delegate void Callback();

	// Token: 0x020008AC RID: 2220
	// (Invoke) Token: 0x06004CF0 RID: 19696
	public delegate void ArgCallback(object args);

	// Token: 0x020008AD RID: 2221
	public struct Stats
	{
		// Token: 0x040033A1 RID: 13217
		public int Created;

		// Token: 0x040033A2 RID: 13218
		public int Inactive;

		// Token: 0x040033A3 RID: 13219
		public int Active;
	}

	// Token: 0x020008AE RID: 2222
	private class Event
	{
		// Token: 0x06004CF3 RID: 19699 RVA: 0x001AD26C File Offset: 0x001AB46C
		public void Execute()
		{
			if (this.Id == 0 || this.DueTime == 0f)
			{
				this.Recycle();
				return;
			}
			if (this.Function != null)
			{
				this.Function();
			}
			else
			{
				if (this.ArgFunction == null)
				{
					this.Error("Aborted event because function is null.");
					this.Recycle();
					return;
				}
				this.ArgFunction(this.Arguments);
			}
			if (this.Iterations > 0)
			{
				this.Iterations--;
				if (this.Iterations < 1)
				{
					this.Recycle();
					return;
				}
			}
			this.DueTime = Time.time + this.Interval;
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x001AD310 File Offset: 0x001AB510
		private void Recycle()
		{
			this.Id = 0;
			this.DueTime = 0f;
			this.StartTime = 0f;
			this.CancelOnLoad = true;
			this.Function = null;
			this.ArgFunction = null;
			this.Arguments = null;
			if (vp_Timer.m_Active.Remove(this))
			{
				vp_Timer.m_Pool.Add(this);
			}
		}

		// Token: 0x06004CF5 RID: 19701 RVA: 0x001AD36E File Offset: 0x001AB56E
		private void Destroy()
		{
			vp_Timer.m_Active.Remove(this);
			vp_Timer.m_Pool.Remove(this);
		}

		// Token: 0x06004CF6 RID: 19702 RVA: 0x001AD388 File Offset: 0x001AB588
		private void Error(string message)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Error: (",
				this,
				") ",
				message
			}));
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06004CF7 RID: 19703 RVA: 0x001AD3B4 File Offset: 0x001AB5B4
		public string MethodName
		{
			get
			{
				if (this.Function != null)
				{
					if (this.Function.Method != null)
					{
						if (this.Function.Method.Name[0] == '<')
						{
							return "delegate";
						}
						return this.Function.Method.Name;
					}
				}
				else if (this.ArgFunction != null && this.ArgFunction.Method != null)
				{
					if (this.ArgFunction.Method.Name[0] == '<')
					{
						return "delegate";
					}
					return this.ArgFunction.Method.Name;
				}
				return null;
			}
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x06004CF8 RID: 19704 RVA: 0x001AD45C File Offset: 0x001AB65C
		public string MethodInfo
		{
			get
			{
				string text = this.MethodName;
				if (!string.IsNullOrEmpty(text))
				{
					text += "(";
					if (this.Arguments != null)
					{
						if (this.Arguments.GetType().IsArray)
						{
							object[] array = (object[])this.Arguments;
							foreach (object obj in array)
							{
								text += obj.ToString();
								if (Array.IndexOf<object>(array, obj) < array.Length - 1)
								{
									text += ", ";
								}
							}
						}
						else
						{
							text += this.Arguments;
						}
					}
					text += ")";
				}
				else
				{
					text = "(function = null)";
				}
				return text;
			}
		}

		// Token: 0x040033A4 RID: 13220
		public int Id;

		// Token: 0x040033A5 RID: 13221
		public vp_Timer.Callback Function;

		// Token: 0x040033A6 RID: 13222
		public vp_Timer.ArgCallback ArgFunction;

		// Token: 0x040033A7 RID: 13223
		public object Arguments;

		// Token: 0x040033A8 RID: 13224
		public int Iterations = 1;

		// Token: 0x040033A9 RID: 13225
		public float Interval = -1f;

		// Token: 0x040033AA RID: 13226
		public float DueTime;

		// Token: 0x040033AB RID: 13227
		public float StartTime;

		// Token: 0x040033AC RID: 13228
		public float LifeTime;

		// Token: 0x040033AD RID: 13229
		public bool Paused;

		// Token: 0x040033AE RID: 13230
		public bool CancelOnLoad = true;
	}

	// Token: 0x020008AF RID: 2223
	public class Handle
	{
		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x06004CFA RID: 19706 RVA: 0x001AD530 File Offset: 0x001AB730
		// (set) Token: 0x06004CFB RID: 19707 RVA: 0x001AD547 File Offset: 0x001AB747
		public bool Paused
		{
			get
			{
				return this.Active && this.m_Event.Paused;
			}
			set
			{
				if (this.Active)
				{
					this.m_Event.Paused = value;
				}
			}
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x06004CFC RID: 19708 RVA: 0x001AD55D File Offset: 0x001AB75D
		public float TimeOfInitiation
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.StartTime;
				}
				return 0f;
			}
		}

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x06004CFD RID: 19709 RVA: 0x001AD578 File Offset: 0x001AB778
		public float TimeOfFirstIteration
		{
			get
			{
				if (this.Active)
				{
					return this.m_FirstDueTime;
				}
				return 0f;
			}
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x06004CFE RID: 19710 RVA: 0x001AD58E File Offset: 0x001AB78E
		public float TimeOfNextIteration
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.DueTime;
				}
				return 0f;
			}
		}

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x06004CFF RID: 19711 RVA: 0x001AD5A9 File Offset: 0x001AB7A9
		public float TimeOfLastIteration
		{
			get
			{
				if (this.Active)
				{
					return Time.time + this.DurationLeft;
				}
				return 0f;
			}
		}

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x06004D00 RID: 19712 RVA: 0x001AD5C5 File Offset: 0x001AB7C5
		public float Delay
		{
			get
			{
				return Mathf.Round((this.m_FirstDueTime - this.TimeOfInitiation) * 1000f) / 1000f;
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06004D01 RID: 19713 RVA: 0x001AD5E5 File Offset: 0x001AB7E5
		public float Interval
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.Interval;
				}
				return 0f;
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06004D02 RID: 19714 RVA: 0x001AD600 File Offset: 0x001AB800
		public float TimeUntilNextIteration
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.DueTime - Time.time;
				}
				return 0f;
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x06004D03 RID: 19715 RVA: 0x001AD621 File Offset: 0x001AB821
		public float DurationLeft
		{
			get
			{
				if (this.Active)
				{
					return this.TimeUntilNextIteration + (float)(this.m_Event.Iterations - 1) * this.m_Event.Interval;
				}
				return 0f;
			}
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x06004D04 RID: 19716 RVA: 0x001AD652 File Offset: 0x001AB852
		public float DurationTotal
		{
			get
			{
				if (this.Active)
				{
					return this.Delay + (float)this.m_StartIterations * ((this.m_StartIterations > 1) ? this.Interval : 0f);
				}
				return 0f;
			}
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x06004D05 RID: 19717 RVA: 0x001AD687 File Offset: 0x001AB887
		public float Duration
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.LifeTime;
				}
				return 0f;
			}
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06004D06 RID: 19718 RVA: 0x001AD6A2 File Offset: 0x001AB8A2
		public int IterationsTotal
		{
			get
			{
				return this.m_StartIterations;
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06004D07 RID: 19719 RVA: 0x001AD6AA File Offset: 0x001AB8AA
		public int IterationsLeft
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.Iterations;
				}
				return 0;
			}
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06004D08 RID: 19720 RVA: 0x001AD6C1 File Offset: 0x001AB8C1
		// (set) Token: 0x06004D09 RID: 19721 RVA: 0x001AD6CC File Offset: 0x001AB8CC
		public int Id
		{
			get
			{
				return this.m_Id;
			}
			set
			{
				this.m_Id = value;
				if (this.m_Id == 0)
				{
					this.m_Event.DueTime = 0f;
					return;
				}
				this.m_Event = null;
				for (int i = vp_Timer.m_Active.Count - 1; i > -1; i--)
				{
					if (vp_Timer.m_Active[i].Id == this.m_Id)
					{
						this.m_Event = vp_Timer.m_Active[i];
						break;
					}
				}
				if (this.m_Event == null)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Error: (",
						this,
						") Failed to assign event with Id '",
						this.m_Id,
						"'."
					}));
				}
				this.m_StartIterations = this.m_Event.Iterations;
				this.m_FirstDueTime = this.m_Event.DueTime;
			}
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06004D0A RID: 19722 RVA: 0x001AD7A6 File Offset: 0x001AB9A6
		public bool Active
		{
			get
			{
				return this.m_Event != null && this.Id != 0 && this.m_Event.Id != 0 && this.m_Event.Id == this.Id;
			}
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06004D0B RID: 19723 RVA: 0x001AD7DA File Offset: 0x001AB9DA
		public string MethodName
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.MethodName;
				}
				return "";
			}
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06004D0C RID: 19724 RVA: 0x001AD7F5 File Offset: 0x001AB9F5
		public string MethodInfo
		{
			get
			{
				if (this.Active)
				{
					return this.m_Event.MethodInfo;
				}
				return "";
			}
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06004D0D RID: 19725 RVA: 0x001AD810 File Offset: 0x001ABA10
		// (set) Token: 0x06004D0E RID: 19726 RVA: 0x001AD827 File Offset: 0x001ABA27
		public bool CancelOnLoad
		{
			get
			{
				return !this.Active || this.m_Event.CancelOnLoad;
			}
			set
			{
				if (this.Active)
				{
					this.m_Event.CancelOnLoad = value;
					return;
				}
				Debug.LogWarning("Warning: (" + this + ") Tried to set CancelOnLoad on inactive timer handle.");
			}
		}

		// Token: 0x06004D0F RID: 19727 RVA: 0x001AD853 File Offset: 0x001ABA53
		public void Cancel()
		{
			vp_Timer.Cancel(this);
		}

		// Token: 0x06004D10 RID: 19728 RVA: 0x001AD85B File Offset: 0x001ABA5B
		public void Execute()
		{
			this.m_Event.DueTime = Time.time;
		}

		// Token: 0x040033AF RID: 13231
		private vp_Timer.Event m_Event;

		// Token: 0x040033B0 RID: 13232
		private int m_Id;

		// Token: 0x040033B1 RID: 13233
		private int m_StartIterations = 1;

		// Token: 0x040033B2 RID: 13234
		private float m_FirstDueTime;
	}
}
