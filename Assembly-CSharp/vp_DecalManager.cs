using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E1 RID: 225
public sealed class vp_DecalManager
{
	// Token: 0x17000055 RID: 85
	// (get) Token: 0x0600081F RID: 2079 RVA: 0x000786F7 File Offset: 0x000768F7
	// (set) Token: 0x06000820 RID: 2080 RVA: 0x000786FE File Offset: 0x000768FE
	public static float MaxDecals
	{
		get
		{
			return vp_DecalManager.m_MaxDecals;
		}
		set
		{
			vp_DecalManager.m_MaxDecals = value;
			vp_DecalManager.Refresh();
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x06000821 RID: 2081 RVA: 0x0007870B File Offset: 0x0007690B
	// (set) Token: 0x06000822 RID: 2082 RVA: 0x00078712 File Offset: 0x00076912
	public static float FadedDecals
	{
		get
		{
			return vp_DecalManager.m_FadedDecals;
		}
		set
		{
			if (value > vp_DecalManager.m_MaxDecals)
			{
				Debug.LogError("FadedDecals can't be larger than MaxDecals");
				return;
			}
			vp_DecalManager.m_FadedDecals = value;
			vp_DecalManager.Refresh();
		}
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x00078734 File Offset: 0x00076934
	static vp_DecalManager()
	{
		vp_DecalManager.Refresh();
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x00022F1F File Offset: 0x0002111F
	private vp_DecalManager()
	{
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x00078782 File Offset: 0x00076982
	public static void Add(GameObject decal)
	{
		vp_DecalManager.m_Decals.Add(decal);
		vp_DecalManager.FadeAndRemove();
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x00078794 File Offset: 0x00076994
	private static void FadeAndRemove()
	{
		if ((float)vp_DecalManager.m_Decals.Count > vp_DecalManager.m_NonFadedDecals)
		{
			int num = 0;
			while ((float)num < (float)vp_DecalManager.m_Decals.Count - vp_DecalManager.m_NonFadedDecals)
			{
				if (vp_DecalManager.m_Decals[num] != null)
				{
					Color color = vp_DecalManager.m_Decals[num].GetComponent<Renderer>().material.color;
					color.a -= vp_DecalManager.m_FadeAmount;
					vp_DecalManager.m_Decals[num].GetComponent<Renderer>().material.color = color;
				}
				num++;
			}
		}
		if (vp_DecalManager.m_Decals[0] != null)
		{
			if (vp_DecalManager.m_Decals[0].GetComponent<Renderer>().material.color.a <= 0f)
			{
				Object.Destroy(vp_DecalManager.m_Decals[0]);
				vp_DecalManager.m_Decals.Remove(vp_DecalManager.m_Decals[0]);
				return;
			}
		}
		else
		{
			vp_DecalManager.m_Decals.RemoveAt(0);
		}
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x00078894 File Offset: 0x00076A94
	private static void Refresh()
	{
		if (vp_DecalManager.m_MaxDecals < vp_DecalManager.m_FadedDecals)
		{
			vp_DecalManager.m_MaxDecals = vp_DecalManager.m_FadedDecals;
		}
		vp_DecalManager.m_FadeAmount = vp_DecalManager.m_MaxDecals / vp_DecalManager.m_FadedDecals / vp_DecalManager.m_MaxDecals;
		vp_DecalManager.m_NonFadedDecals = vp_DecalManager.m_MaxDecals - vp_DecalManager.m_FadedDecals;
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x000788D4 File Offset: 0x00076AD4
	private static void DebugOutput()
	{
		int num = 0;
		int num2 = 0;
		using (List<GameObject>.Enumerator enumerator = vp_DecalManager.m_Decals.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetComponent<Renderer>().material.color.a == 1f)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
	}

	// Token: 0x04000E6C RID: 3692
	public static readonly vp_DecalManager instance = new vp_DecalManager();

	// Token: 0x04000E6D RID: 3693
	private static List<GameObject> m_Decals = new List<GameObject>();

	// Token: 0x04000E6E RID: 3694
	private static float m_MaxDecals = 100f;

	// Token: 0x04000E6F RID: 3695
	private static float m_FadedDecals = 20f;

	// Token: 0x04000E70 RID: 3696
	private static float m_NonFadedDecals = 0f;

	// Token: 0x04000E71 RID: 3697
	private static float m_FadeAmount = 0f;
}
