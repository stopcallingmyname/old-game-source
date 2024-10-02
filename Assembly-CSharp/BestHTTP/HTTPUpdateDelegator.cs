using System;
using System.Threading;
using BestHTTP.Caching;
using BestHTTP.Cookies;
using UnityEngine;

namespace BestHTTP
{
	// Token: 0x0200018A RID: 394
	[ExecuteInEditMode]
	public sealed class HTTPUpdateDelegator : MonoBehaviour
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x00096204 File Offset: 0x00094404
		// (set) Token: 0x06000E85 RID: 3717 RVA: 0x0009620B File Offset: 0x0009440B
		public static HTTPUpdateDelegator Instance { get; private set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x00096213 File Offset: 0x00094413
		// (set) Token: 0x06000E87 RID: 3719 RVA: 0x0009621A File Offset: 0x0009441A
		public static bool IsCreated { get; private set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x00096222 File Offset: 0x00094422
		// (set) Token: 0x06000E89 RID: 3721 RVA: 0x00096229 File Offset: 0x00094429
		public static bool IsThreaded { get; set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x00096231 File Offset: 0x00094431
		// (set) Token: 0x06000E8B RID: 3723 RVA: 0x00096238 File Offset: 0x00094438
		public static bool IsThreadRunning { get; private set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x00096240 File Offset: 0x00094440
		// (set) Token: 0x06000E8D RID: 3725 RVA: 0x00096247 File Offset: 0x00094447
		public static int ThreadFrequencyInMS { get; set; } = 100;

		// Token: 0x06000E8F RID: 3727 RVA: 0x00096258 File Offset: 0x00094458
		public static void CheckInstance()
		{
			try
			{
				if (!HTTPUpdateDelegator.IsCreated)
				{
					GameObject gameObject = GameObject.Find("HTTP Update Delegator");
					if (gameObject != null)
					{
						HTTPUpdateDelegator.Instance = gameObject.GetComponent<HTTPUpdateDelegator>();
					}
					if (HTTPUpdateDelegator.Instance == null)
					{
						HTTPUpdateDelegator.Instance = new GameObject("HTTP Update Delegator")
						{
							hideFlags = HideFlags.DontSave
						}.AddComponent<HTTPUpdateDelegator>();
					}
					HTTPUpdateDelegator.IsCreated = true;
					HTTPManager.Logger.Information("HTTPUpdateDelegator", "Instance Created!");
				}
			}
			catch
			{
				HTTPManager.Logger.Error("HTTPUpdateDelegator", "Please call the BestHTTP.HTTPManager.Setup() from one of Unity's event(eg. awake, start) before you send any request!");
			}
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x000962FC File Offset: 0x000944FC
		private void Setup()
		{
			HTTPCacheService.SetupCacheFolder();
			CookieJar.SetupFolder();
			CookieJar.Load();
			if (HTTPUpdateDelegator.IsThreaded)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadFunc));
			}
			HTTPUpdateDelegator.IsSetupCalled = true;
			if (!Application.isEditor || Application.isPlaying)
			{
				Object.DontDestroyOnLoad(base.gameObject);
			}
			HTTPManager.Logger.Information("HTTPUpdateDelegator", "Setup done!");
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00096364 File Offset: 0x00094564
		private void ThreadFunc(object obj)
		{
			HTTPManager.Logger.Information("HTTPUpdateDelegator", "Update Thread Started");
			try
			{
				HTTPUpdateDelegator.IsThreadRunning = true;
				while (HTTPUpdateDelegator.IsThreadRunning)
				{
					HTTPManager.OnUpdate();
					Thread.Sleep(HTTPUpdateDelegator.ThreadFrequencyInMS);
				}
			}
			finally
			{
				HTTPManager.Logger.Information("HTTPUpdateDelegator", "Update Thread Ended");
			}
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x000963CC File Offset: 0x000945CC
		private void Update()
		{
			if (!HTTPUpdateDelegator.IsSetupCalled)
			{
				HTTPUpdateDelegator.IsSetupCalled = true;
				this.Setup();
			}
			if (!HTTPUpdateDelegator.IsThreaded)
			{
				HTTPManager.OnUpdate();
			}
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x000963ED File Offset: 0x000945ED
		private void OnDisable()
		{
			HTTPManager.Logger.Information("HTTPUpdateDelegator", "OnDisable Called!");
			this.OnApplicationQuit();
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00096409 File Offset: 0x00094609
		private void OnApplicationPause(bool isPaused)
		{
			if (HTTPUpdateDelegator.OnApplicationForegroundStateChanged != null)
			{
				HTTPUpdateDelegator.OnApplicationForegroundStateChanged(isPaused);
			}
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00096420 File Offset: 0x00094620
		private void OnApplicationQuit()
		{
			HTTPManager.Logger.Information("HTTPUpdateDelegator", "OnApplicationQuit Called!");
			if (HTTPUpdateDelegator.OnBeforeApplicationQuit != null)
			{
				try
				{
					if (!HTTPUpdateDelegator.OnBeforeApplicationQuit())
					{
						HTTPManager.Logger.Information("HTTPUpdateDelegator", "OnBeforeApplicationQuit call returned false, postponing plugin shutdown.");
						return;
					}
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("HTTPUpdateDelegator", string.Empty, ex);
				}
			}
			HTTPUpdateDelegator.IsThreadRunning = false;
			if (!HTTPUpdateDelegator.IsCreated)
			{
				return;
			}
			HTTPUpdateDelegator.IsCreated = false;
			HTTPManager.OnQuit();
		}

		// Token: 0x0400134D RID: 4941
		public static Func<bool> OnBeforeApplicationQuit;

		// Token: 0x0400134E RID: 4942
		public static Action<bool> OnApplicationForegroundStateChanged;

		// Token: 0x0400134F RID: 4943
		private static bool IsSetupCalled;
	}
}
