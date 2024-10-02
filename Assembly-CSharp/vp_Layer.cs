using System;
using UnityEngine;

// Token: 0x020000CD RID: 205
public sealed class vp_Layer
{
	// Token: 0x060006D1 RID: 1745 RVA: 0x0006E684 File Offset: 0x0006C884
	static vp_Layer()
	{
		Physics.IgnoreLayerCollision(30, 29);
		Physics.IgnoreLayerCollision(29, 29);
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x00022F1F File Offset: 0x0002111F
	private vp_Layer()
	{
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x0006E6A4 File Offset: 0x0006C8A4
	public static void Set(GameObject obj, int layer, bool recursive = false)
	{
		if (layer < 0 || layer > 31)
		{
			Debug.LogError("vp_Layer: Attempted to set layer id out of range [0-31].");
			return;
		}
		obj.layer = layer;
		if (recursive)
		{
			foreach (object obj2 in obj.transform)
			{
				vp_Layer.Set(((Transform)obj2).gameObject, layer, true);
			}
		}
	}

	// Token: 0x04000CB8 RID: 3256
	public static readonly vp_Layer instance = new vp_Layer();

	// Token: 0x04000CB9 RID: 3257
	public const int Default = 0;

	// Token: 0x04000CBA RID: 3258
	public const int TransparentFX = 1;

	// Token: 0x04000CBB RID: 3259
	public const int IgnoreRaycast = 2;

	// Token: 0x04000CBC RID: 3260
	public const int Water = 4;

	// Token: 0x04000CBD RID: 3261
	public const int Enemy = 25;

	// Token: 0x04000CBE RID: 3262
	public const int Pickup = 26;

	// Token: 0x04000CBF RID: 3263
	public const int Trigger = 27;

	// Token: 0x04000CC0 RID: 3264
	public const int MovableObject = 28;

	// Token: 0x04000CC1 RID: 3265
	public const int Debris = 29;

	// Token: 0x04000CC2 RID: 3266
	public const int LocalPlayer = 30;

	// Token: 0x04000CC3 RID: 3267
	public const int Weapon = 31;

	// Token: 0x04000CC4 RID: 3268
	public const int Players = 10;

	// Token: 0x020008AA RID: 2218
	public static class Mask
	{
		// Token: 0x0400339D RID: 13213
		public const int BulletBlockers = -1811939349;

		// Token: 0x0400339E RID: 13214
		public const int ExternalBlockers = -1744831509;

		// Token: 0x0400339F RID: 13215
		public const int PhysicsBlockers = 1342177280;

		// Token: 0x040033A0 RID: 13216
		public const int IgnoreWalkThru = -738197525;
	}
}
