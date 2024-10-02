using System;
using UnityEngine;

// Token: 0x02000035 RID: 53
public class LogCatch : MonoBehaviour
{
	// Token: 0x060001A1 RID: 417 RVA: 0x0002312D File Offset: 0x0002132D
	private void OnEnable()
	{
		Application.RegisterLogCallback(new Application.LogCallback(this.HandleLog));
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00016B66 File Offset: 0x00014D66
	private void OnDisable()
	{
		Application.RegisterLogCallback(null);
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00023140 File Offset: 0x00021340
	private void Awake()
	{
		this.OnEnable();
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00023148 File Offset: 0x00021348
	private void HandleLog(string logString, string stackTrace, LogType t)
	{
		if (logString == "NullReferenceException: Object reference not set to an instance of an object")
		{
			stackTrace == "vp_FPController.FixedMove ()\nvp_FPController.FixedUpdate ()\n";
		}
	}
}
