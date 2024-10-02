using System;
using AssemblyCSharp;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class EntManager : MonoBehaviour
{
	// Token: 0x060001E9 RID: 489 RVA: 0x00024D18 File Offset: 0x00022F18
	public static void Clear()
	{
		for (int i = 0; i < 512; i++)
		{
			if (EntManager.Ent[i] != null)
			{
				Object.Destroy(EntManager.Ent[i].go);
				EntManager.Ent[i] = null;
			}
		}
	}

	// Token: 0x060001EA RID: 490 RVA: 0x00024D58 File Offset: 0x00022F58
	public static CEnt CreateEnt(GameObject pgo, Vector3 position, Vector3 rotation)
	{
		CEnt freeEnt = EntManager.GetFreeEnt();
		if (freeEnt == null)
		{
			return null;
		}
		freeEnt.go = Object.Instantiate<GameObject>(pgo);
		freeEnt.go.transform.position = position;
		freeEnt.go.transform.eulerAngles = rotation;
		return freeEnt;
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00024D9F File Offset: 0x00022F9F
	public static void DeleteEnt(int entid)
	{
		EntManager.Ent[entid] = null;
	}

	// Token: 0x060001EC RID: 492 RVA: 0x00024DAC File Offset: 0x00022FAC
	public static CEnt GetEnt(int uid)
	{
		for (int i = 0; i < 512; i++)
		{
			if (EntManager.Ent[i] != null && EntManager.Ent[i].uid == uid)
			{
				return EntManager.Ent[i];
			}
		}
		return null;
	}

	// Token: 0x060001ED RID: 493 RVA: 0x00024DEC File Offset: 0x00022FEC
	private static CEnt GetFreeEnt()
	{
		for (int i = 0; i < 512; i++)
		{
			if (EntManager.Ent[i] == null)
			{
				EntManager.Ent[i] = new CEnt();
				EntManager.Ent[i].index = i;
				return EntManager.Ent[i];
			}
		}
		return null;
	}

	// Token: 0x040001AC RID: 428
	public const int MAX_ENT = 512;

	// Token: 0x040001AD RID: 429
	public static CEnt[] Ent = new CEnt[512];
}
