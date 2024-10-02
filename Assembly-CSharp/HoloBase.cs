using System;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class HoloBase : MonoBehaviour
{
	// Token: 0x0600019F RID: 415 RVA: 0x000230AC File Offset: 0x000212AC
	public static GameObject Create(Vector3 pos, int _mode)
	{
		if (_mode == 0)
		{
			HoloBase.goCenter = (GameObject)Object.Instantiate(Resources.Load("Prefab/Center"));
		}
		else if (_mode == 9)
		{
			HoloBase.goCenter = (GameObject)Object.Instantiate(Resources.Load("Prefab/evakPoint"));
		}
		else if (_mode == 777)
		{
			HoloBase.goCenter = (GameObject)Object.Instantiate(Resources.Load("Prefab/flagZone"));
		}
		HoloBase.goCenter.transform.position = pos;
		return HoloBase.goCenter;
	}

	// Token: 0x04000172 RID: 370
	private static GameObject goCenter;
}
