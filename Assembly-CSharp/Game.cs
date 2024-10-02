using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
public class Game : MonoBehaviour
{
	// Token: 0x06000198 RID: 408 RVA: 0x00023050 File Offset: 0x00021250
	public static bool isStandAlone()
	{
		return !Application.isEditor;
	}

	// Token: 0x0400016F RID: 367
	public static string username = "...";

	// Token: 0x04000170 RID: 368
	public static bool SteamIsActiv;
}
