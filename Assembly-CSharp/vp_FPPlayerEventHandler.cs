using System;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class vp_FPPlayerEventHandler : vp_StateEventHandler
{
	// Token: 0x06000793 RID: 1939 RVA: 0x000733EC File Offset: 0x000715EC
	protected override void Awake()
	{
		base.Awake();
		base.BindStateToActivity(this.Walk);
		base.BindStateToActivity(this.Jump);
		base.BindStateToActivity(this.Crouch);
		base.BindStateToActivity(this.Zoom);
		base.BindStateToActivity(this.Reload);
		base.BindStateToActivity(this.Dead);
		base.BindStateToActivityOnStart(this.Attack);
		this.SetWeapon.AutoDuration = 1f;
		this.Reload.AutoDuration = 1f;
		this.Zoom.MinDuration = 0.2f;
		this.Crouch.MinDuration = 0.5f;
		this.Jump.MinPause = 0f;
		this.SetWeapon.MinPause = 0.2f;
	}

	// Token: 0x04000D76 RID: 3446
	public vp_Message<float> HUDDamageFlash;

	// Token: 0x04000D77 RID: 3447
	public vp_Message<string> HUDText;

	// Token: 0x04000D78 RID: 3448
	public vp_Value<Vector2> InputMoveVector;

	// Token: 0x04000D79 RID: 3449
	public vp_Value<bool> AllowGameplayInput;

	// Token: 0x04000D7A RID: 3450
	public vp_Value<float> Health;

	// Token: 0x04000D7B RID: 3451
	public vp_Value<Vector3> Position;

	// Token: 0x04000D7C RID: 3452
	public vp_Value<Vector2> Rotation;

	// Token: 0x04000D7D RID: 3453
	public vp_Value<Vector3> Forward;

	// Token: 0x04000D7E RID: 3454
	public vp_Activity Dead;

	// Token: 0x04000D7F RID: 3455
	public vp_Activity Walk;

	// Token: 0x04000D80 RID: 3456
	public vp_Activity Jump;

	// Token: 0x04000D81 RID: 3457
	public vp_Activity Crouch;

	// Token: 0x04000D82 RID: 3458
	public vp_Activity Zoom;

	// Token: 0x04000D83 RID: 3459
	public vp_Activity Attack;

	// Token: 0x04000D84 RID: 3460
	public vp_Activity Reload;

	// Token: 0x04000D85 RID: 3461
	public vp_Activity<int> SetWeapon;

	// Token: 0x04000D86 RID: 3462
	public vp_Activity<Vector3> Earthquake;

	// Token: 0x04000D87 RID: 3463
	public vp_Attempt SetPrevWeapon;

	// Token: 0x04000D88 RID: 3464
	public vp_Attempt SetNextWeapon;

	// Token: 0x04000D89 RID: 3465
	public vp_Attempt<string> SetWeaponByName;

	// Token: 0x04000D8A RID: 3466
	public vp_Value<int> CurrentWeaponID;

	// Token: 0x04000D8B RID: 3467
	public vp_Value<string> CurrentWeaponName;

	// Token: 0x04000D8C RID: 3468
	public vp_Value<bool> CurrentWeaponWielded;

	// Token: 0x04000D8D RID: 3469
	public vp_Value<float> CurrentWeaponReloadDuration;

	// Token: 0x04000D8E RID: 3470
	public vp_Value<string> CurrentWeaponClipType;

	// Token: 0x04000D8F RID: 3471
	public vp_Message<string, int> GetItemCount;

	// Token: 0x04000D90 RID: 3472
	public vp_Attempt<object> AddItem;

	// Token: 0x04000D91 RID: 3473
	public vp_Attempt<object> RemoveItem;

	// Token: 0x04000D92 RID: 3474
	public vp_Attempt<object> AddAmmo;

	// Token: 0x04000D93 RID: 3475
	public vp_Attempt DepleteAmmo;

	// Token: 0x04000D94 RID: 3476
	public vp_Attempt RemoveClip;

	// Token: 0x04000D95 RID: 3477
	public vp_Message<float> FallImpact;

	// Token: 0x04000D96 RID: 3478
	public vp_Message<float> HeadImpact;

	// Token: 0x04000D97 RID: 3479
	public vp_Message<Vector3> ForceImpact;

	// Token: 0x04000D98 RID: 3480
	public vp_Message<float> GroundStomp;

	// Token: 0x04000D99 RID: 3481
	public vp_Message<float> BombShake;

	// Token: 0x04000D9A RID: 3482
	public vp_Value<Vector3> EarthQuakeForce;

	// Token: 0x04000D9B RID: 3483
	public vp_Message Stop;

	// Token: 0x04000D9C RID: 3484
	public vp_Value<Transform> Platform;

	// Token: 0x04000D9D RID: 3485
	public vp_Value<bool> Pause = new vp_Value<bool>("");
}
