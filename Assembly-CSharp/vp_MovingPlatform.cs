using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E5 RID: 229
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class vp_MovingPlatform : MonoBehaviour
{
	// Token: 0x0600083A RID: 2106 RVA: 0x00079224 File Offset: 0x00077424
	private void Start()
	{
		this.m_Transform = base.transform;
		this.m_Collider = base.GetComponentInChildren<Collider>();
		this.m_RigidBody = base.GetComponent<Rigidbody>();
		this.m_RigidBody.useGravity = false;
		this.m_RigidBody.isKinematic = true;
		this.m_NextWaypoint = 0;
		this.m_Audio = base.GetComponent<AudioSource>();
		this.m_Audio.loop = true;
		this.m_Audio.clip = this.SoundMove;
		if (this.PathWaypoints == null)
		{
			return;
		}
		base.gameObject.layer = 28;
		foreach (object obj in this.PathWaypoints.transform)
		{
			Transform transform = (Transform)obj;
			if (vp_Utility.IsActive(transform.gameObject))
			{
				this.m_Waypoints.Add(transform);
				transform.gameObject.layer = 28;
			}
			if (transform.GetComponent<Renderer>() != null)
			{
				transform.GetComponent<Renderer>().enabled = false;
			}
			if (transform.GetComponent<Collider>() != null)
			{
				transform.GetComponent<Collider>().enabled = false;
			}
		}
		IComparer @object = new vp_MovingPlatform.WaypointComparer();
		this.m_Waypoints.Sort(new Comparison<Transform>(@object.Compare));
		if (this.m_Waypoints.Count > 0)
		{
			this.m_CurrentTargetPosition = this.m_Waypoints[this.m_NextWaypoint].position;
			this.m_CurrentTargetAngle = this.m_Waypoints[this.m_NextWaypoint].eulerAngles;
			this.m_Transform.position = this.m_CurrentTargetPosition;
			this.m_Transform.eulerAngles = this.m_CurrentTargetAngle;
			if (this.MoveAutoStartTarget > this.m_Waypoints.Count - 1)
			{
				this.MoveAutoStartTarget = this.m_Waypoints.Count - 1;
			}
		}
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x00079410 File Offset: 0x00077610
	private void FixedUpdate()
	{
		this.UpdatePath();
		this.UpdateMovement();
		this.UpdateRotation();
		this.UpdateVelocity();
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x0007942C File Offset: 0x0007762C
	private void UpdatePath()
	{
		if (this.m_Waypoints.Count < 2)
		{
			return;
		}
		if (this.GetDistanceLeft() < 0.01f && Time.time >= this.m_NextAllowedMoveTime)
		{
			switch (this.PathType)
			{
			case vp_MovingPlatform.PathMoveType.PingPong:
				if (this.PathDirection == vp_MovingPlatform.Direction.Backwards)
				{
					if (this.m_NextWaypoint == 0)
					{
						this.PathDirection = vp_MovingPlatform.Direction.Forward;
					}
				}
				else if (this.m_NextWaypoint == this.m_Waypoints.Count - 1)
				{
					this.PathDirection = vp_MovingPlatform.Direction.Backwards;
				}
				this.OnArriveAtWaypoint();
				this.GoToNextWaypoint();
				break;
			case vp_MovingPlatform.PathMoveType.Loop:
				this.OnArriveAtWaypoint();
				this.GoToNextWaypoint();
				return;
			case vp_MovingPlatform.PathMoveType.Target:
				if (this.m_NextWaypoint != this.m_TargetedWayPoint)
				{
					if (this.m_Moving)
					{
						if (this.m_PhysicsCurrentMoveVelocity == 0f)
						{
							this.OnStart();
						}
						else
						{
							this.OnArriveAtWaypoint();
						}
					}
					this.GoToNextWaypoint();
					return;
				}
				if (this.m_Moving)
				{
					this.OnStop();
					return;
				}
				if (this.m_NextWaypoint != 0)
				{
					this.OnArriveAtDestination();
					return;
				}
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x0007952A File Offset: 0x0007772A
	private void OnStart()
	{
		if (this.SoundStart != null)
		{
			this.m_Audio.PlayOneShot(this.SoundStart, AudioListener.volume);
		}
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x00079550 File Offset: 0x00077750
	private void OnArriveAtWaypoint()
	{
		if (this.SoundWaypoint != null)
		{
			this.m_Audio.PlayOneShot(this.SoundWaypoint, AudioListener.volume);
		}
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x00079576 File Offset: 0x00077776
	private void OnArriveAtDestination()
	{
		if (this.MoveReturnDelay > 0f && !this.m_ReturnDelayTimer.Active)
		{
			vp_Timer.In(this.MoveReturnDelay, delegate()
			{
				this.GoTo(0);
			}, this.m_ReturnDelayTimer);
		}
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x000795B0 File Offset: 0x000777B0
	private void OnStop()
	{
		this.m_Audio.Stop();
		if (this.SoundStop != null)
		{
			this.m_Audio.PlayOneShot(this.SoundStop, AudioListener.volume);
		}
		this.m_Transform.position = this.m_CurrentTargetPosition;
		this.m_Transform.eulerAngles = this.m_CurrentTargetAngle;
		this.m_Moving = false;
		if (this.m_NextWaypoint == 0)
		{
			this.m_NextAllowedMoveTime = Time.time + this.MoveCooldown;
		}
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x00079630 File Offset: 0x00077830
	private void UpdateMovement()
	{
		if (this.m_Waypoints.Count < 2)
		{
			return;
		}
		switch (this.MoveInterpolationMode)
		{
		case vp_MovingPlatform.MovementInterpolationMode.EaseInOut:
			this.m_Transform.position = vp_Utility.NaNSafeVector3(Vector3.Lerp(this.m_Transform.position, this.m_CurrentTargetPosition, this.m_EaseInOutCurve.Evaluate(this.m_MoveTime)), default(Vector3));
			return;
		case vp_MovingPlatform.MovementInterpolationMode.EaseIn:
			this.m_Transform.position = vp_Utility.NaNSafeVector3(Vector3.MoveTowards(this.m_Transform.position, this.m_CurrentTargetPosition, this.m_MoveTime), default(Vector3));
			return;
		case vp_MovingPlatform.MovementInterpolationMode.EaseOut:
			this.m_Transform.position = vp_Utility.NaNSafeVector3(Vector3.Lerp(this.m_Transform.position, this.m_CurrentTargetPosition, this.m_LinearCurve.Evaluate(this.m_MoveTime)), default(Vector3));
			return;
		case vp_MovingPlatform.MovementInterpolationMode.EaseOut2:
			this.m_Transform.position = vp_Utility.NaNSafeVector3(Vector3.Lerp(this.m_Transform.position, this.m_CurrentTargetPosition, this.MoveSpeed * 0.25f), default(Vector3));
			return;
		case vp_MovingPlatform.MovementInterpolationMode.Slerp:
			this.m_Transform.position = vp_Utility.NaNSafeVector3(Vector3.Slerp(this.m_Transform.position, this.m_CurrentTargetPosition, this.m_LinearCurve.Evaluate(this.m_MoveTime)), default(Vector3));
			return;
		case vp_MovingPlatform.MovementInterpolationMode.Lerp:
			this.m_Transform.position = vp_Utility.NaNSafeVector3(Vector3.MoveTowards(this.m_Transform.position, this.m_CurrentTargetPosition, this.MoveSpeed), default(Vector3));
			return;
		default:
			return;
		}
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x000797DC File Offset: 0x000779DC
	private void UpdateRotation()
	{
		switch (this.RotationInterpolationMode)
		{
		case vp_MovingPlatform.RotateInterpolationMode.SyncToMovement:
			if (this.m_Moving)
			{
				this.m_Transform.eulerAngles = vp_Utility.NaNSafeVector3(new Vector3(Mathf.LerpAngle(this.m_OriginalAngle.x, this.m_CurrentTargetAngle.x, 1f - this.GetDistanceLeft() / this.m_TravelDistance), Mathf.LerpAngle(this.m_OriginalAngle.y, this.m_CurrentTargetAngle.y, 1f - this.GetDistanceLeft() / this.m_TravelDistance), Mathf.LerpAngle(this.m_OriginalAngle.z, this.m_CurrentTargetAngle.z, 1f - this.GetDistanceLeft() / this.m_TravelDistance)), default(Vector3));
				return;
			}
			break;
		case vp_MovingPlatform.RotateInterpolationMode.EaseOut:
			this.m_Transform.eulerAngles = vp_Utility.NaNSafeVector3(new Vector3(Mathf.LerpAngle(this.m_Transform.eulerAngles.x, this.m_CurrentTargetAngle.x, this.m_LinearCurve.Evaluate(this.m_MoveTime)), Mathf.LerpAngle(this.m_Transform.eulerAngles.y, this.m_CurrentTargetAngle.y, this.m_LinearCurve.Evaluate(this.m_MoveTime)), Mathf.LerpAngle(this.m_Transform.eulerAngles.z, this.m_CurrentTargetAngle.z, this.m_LinearCurve.Evaluate(this.m_MoveTime))), default(Vector3));
			return;
		case vp_MovingPlatform.RotateInterpolationMode.CustomEaseOut:
			this.m_Transform.eulerAngles = vp_Utility.NaNSafeVector3(new Vector3(Mathf.LerpAngle(this.m_Transform.eulerAngles.x, this.m_CurrentTargetAngle.x, this.RotationEaseAmount), Mathf.LerpAngle(this.m_Transform.eulerAngles.y, this.m_CurrentTargetAngle.y, this.RotationEaseAmount), Mathf.LerpAngle(this.m_Transform.eulerAngles.z, this.m_CurrentTargetAngle.z, this.RotationEaseAmount)), default(Vector3));
			return;
		case vp_MovingPlatform.RotateInterpolationMode.CustomRotate:
			this.m_Transform.Rotate(this.RotationSpeed);
			break;
		default:
			return;
		}
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x00079A10 File Offset: 0x00077C10
	private void UpdateVelocity()
	{
		this.m_MoveTime += this.MoveSpeed * 0.01f;
		this.m_PhysicsCurrentMoveVelocity = (this.m_Transform.position - this.m_PrevPos).magnitude;
		this.m_PhysicsCurrentRotationVelocity = (this.m_Transform.eulerAngles - this.m_PrevAngle).magnitude;
		this.m_PrevPos = this.m_Transform.position;
		this.m_PrevAngle = this.m_Transform.eulerAngles;
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x00079AA0 File Offset: 0x00077CA0
	public void GoTo(int targetWayPoint)
	{
		if (Time.time < this.m_NextAllowedMoveTime)
		{
			return;
		}
		if (this.PathType != vp_MovingPlatform.PathMoveType.Target)
		{
			return;
		}
		this.m_TargetedWayPoint = this.GetValidWaypoint(targetWayPoint);
		if (targetWayPoint > this.m_NextWaypoint)
		{
			if (this.PathDirection != vp_MovingPlatform.Direction.Direct)
			{
				this.PathDirection = vp_MovingPlatform.Direction.Forward;
			}
		}
		else if (this.PathDirection != vp_MovingPlatform.Direction.Direct)
		{
			this.PathDirection = vp_MovingPlatform.Direction.Backwards;
		}
		this.m_Moving = true;
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x00079B04 File Offset: 0x00077D04
	protected float GetDistanceLeft()
	{
		if (this.m_Waypoints.Count < 2)
		{
			return 0f;
		}
		return Vector3.Distance(this.m_Transform.position, this.m_Waypoints[this.m_NextWaypoint].position);
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x00079B40 File Offset: 0x00077D40
	protected void GoToNextWaypoint()
	{
		if (this.m_Waypoints.Count < 2)
		{
			return;
		}
		this.m_MoveTime = 0f;
		if (!this.m_Audio.isPlaying)
		{
			this.m_Audio.Play();
		}
		this.m_CurrentWaypoint = this.m_NextWaypoint;
		switch (this.PathDirection)
		{
		case vp_MovingPlatform.Direction.Forward:
			this.m_NextWaypoint = this.GetValidWaypoint(this.m_NextWaypoint + 1);
			break;
		case vp_MovingPlatform.Direction.Backwards:
			this.m_NextWaypoint = this.GetValidWaypoint(this.m_NextWaypoint - 1);
			break;
		case vp_MovingPlatform.Direction.Direct:
			this.m_NextWaypoint = this.m_TargetedWayPoint;
			break;
		}
		this.m_OriginalAngle = this.m_CurrentTargetAngle;
		this.m_CurrentTargetPosition = this.m_Waypoints[this.m_NextWaypoint].position;
		this.m_CurrentTargetAngle = this.m_Waypoints[this.m_NextWaypoint].eulerAngles;
		this.m_TravelDistance = this.GetDistanceLeft();
		this.m_Moving = true;
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x00079C35 File Offset: 0x00077E35
	protected int GetValidWaypoint(int wayPoint)
	{
		if (wayPoint < 0)
		{
			return this.m_Waypoints.Count - 1;
		}
		if (wayPoint > this.m_Waypoints.Count - 1)
		{
			return 0;
		}
		return wayPoint;
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x00079C5C File Offset: 0x00077E5C
	private void OnTriggerEnter(Collider col)
	{
		if (!this.GetPlayer(col))
		{
			return;
		}
		this.TryPushPlayer();
		this.TryAutoStart();
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x00079C74 File Offset: 0x00077E74
	private void OnTriggerStay(Collider col)
	{
		if (!this.PhysicsSnapPlayerToTopOnIntersect)
		{
			return;
		}
		if (!this.GetPlayer(col))
		{
			return;
		}
		this.TrySnapPlayerToTop();
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x00079C90 File Offset: 0x00077E90
	private bool GetPlayer(Collider col)
	{
		if (!this.m_KnownPlayers.ContainsKey(col))
		{
			if (col.gameObject.layer != 30)
			{
				return false;
			}
			vp_FPPlayerEventHandler component = col.transform.root.GetComponent<vp_FPPlayerEventHandler>();
			if (component == null)
			{
				return false;
			}
			this.m_KnownPlayers.Add(col, component);
		}
		if (!this.m_KnownPlayers.TryGetValue(col, out this.m_PlayerToPush))
		{
			return false;
		}
		this.m_PlayerCollider = col;
		return true;
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x00079D04 File Offset: 0x00077F04
	private void TryPushPlayer()
	{
		if (this.m_PlayerToPush == null || this.m_PlayerToPush.Platform == null)
		{
			return;
		}
		if (this.m_PlayerToPush.Position.Get().y > this.m_Collider.bounds.max.y)
		{
			return;
		}
		if (this.m_PlayerToPush.Platform.Get() == this.m_Transform)
		{
			return;
		}
		float num = this.m_PhysicsCurrentMoveVelocity;
		if (num == 0f)
		{
			num = this.m_PhysicsCurrentRotationVelocity * 0.1f;
		}
		if (num > 0f)
		{
			this.m_PlayerToPush.ForceImpact.Send(vp_Utility.HorizontalVector(-(this.m_Transform.position - this.m_PlayerCollider.bounds.center).normalized * num * this.m_PhysicsPushForce));
		}
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x00079E04 File Offset: 0x00078004
	private void TrySnapPlayerToTop()
	{
		if (this.m_PlayerToPush == null || this.m_PlayerToPush.Platform == null)
		{
			return;
		}
		if (this.m_PlayerToPush.Position.Get().y > this.m_Collider.bounds.max.y)
		{
			return;
		}
		if (this.m_PlayerToPush.Platform.Get() == this.m_Transform)
		{
			return;
		}
		if (this.RotationSpeed.x != 0f || this.RotationSpeed.z != 0f || this.m_CurrentTargetAngle.x != 0f || this.m_CurrentTargetAngle.z != 0f)
		{
			return;
		}
		if (this.m_Collider.bounds.max.x < this.m_PlayerCollider.bounds.max.x || this.m_Collider.bounds.max.z < this.m_PlayerCollider.bounds.max.z || this.m_Collider.bounds.min.x > this.m_PlayerCollider.bounds.min.x || this.m_Collider.bounds.min.z > this.m_PlayerCollider.bounds.min.z)
		{
			return;
		}
		Vector3 o = this.m_PlayerToPush.Position.Get();
		o.y = this.m_Collider.bounds.max.y - 0.1f;
		this.m_PlayerToPush.Position.Set(o);
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x00079FEB File Offset: 0x000781EB
	private void TryAutoStart()
	{
		if (this.MoveAutoStartTarget == 0)
		{
			return;
		}
		if (this.m_PhysicsCurrentMoveVelocity > 0f || this.m_Moving)
		{
			return;
		}
		this.GoTo(this.MoveAutoStartTarget);
	}

	// Token: 0x04000E90 RID: 3728
	protected Transform m_Transform;

	// Token: 0x04000E91 RID: 3729
	public vp_MovingPlatform.PathMoveType PathType;

	// Token: 0x04000E92 RID: 3730
	public GameObject PathWaypoints;

	// Token: 0x04000E93 RID: 3731
	public vp_MovingPlatform.Direction PathDirection;

	// Token: 0x04000E94 RID: 3732
	protected List<Transform> m_Waypoints = new List<Transform>();

	// Token: 0x04000E95 RID: 3733
	protected int m_NextWaypoint;

	// Token: 0x04000E96 RID: 3734
	protected Vector3 m_CurrentTargetPosition = Vector3.zero;

	// Token: 0x04000E97 RID: 3735
	protected Vector3 m_CurrentTargetAngle = Vector3.zero;

	// Token: 0x04000E98 RID: 3736
	protected int m_TargetedWayPoint;

	// Token: 0x04000E99 RID: 3737
	protected float m_TravelDistance;

	// Token: 0x04000E9A RID: 3738
	protected Vector3 m_OriginalAngle = Vector3.zero;

	// Token: 0x04000E9B RID: 3739
	protected int m_CurrentWaypoint;

	// Token: 0x04000E9C RID: 3740
	public int MoveAutoStartTarget = 1000;

	// Token: 0x04000E9D RID: 3741
	public float MoveSpeed = 0.1f;

	// Token: 0x04000E9E RID: 3742
	public float MoveReturnDelay;

	// Token: 0x04000E9F RID: 3743
	public float MoveCooldown;

	// Token: 0x04000EA0 RID: 3744
	protected bool m_Moving;

	// Token: 0x04000EA1 RID: 3745
	protected float m_NextAllowedMoveTime;

	// Token: 0x04000EA2 RID: 3746
	protected float m_MoveTime;

	// Token: 0x04000EA3 RID: 3747
	protected vp_Timer.Handle m_ReturnDelayTimer = new vp_Timer.Handle();

	// Token: 0x04000EA4 RID: 3748
	protected Vector3 m_PrevPos = Vector3.zero;

	// Token: 0x04000EA5 RID: 3749
	public vp_MovingPlatform.MovementInterpolationMode MoveInterpolationMode;

	// Token: 0x04000EA6 RID: 3750
	protected AnimationCurve m_EaseInOutCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	// Token: 0x04000EA7 RID: 3751
	protected AnimationCurve m_LinearCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000EA8 RID: 3752
	public float RotationEaseAmount = 0.1f;

	// Token: 0x04000EA9 RID: 3753
	public Vector3 RotationSpeed = Vector3.zero;

	// Token: 0x04000EAA RID: 3754
	protected Vector3 m_PrevAngle = Vector3.zero;

	// Token: 0x04000EAB RID: 3755
	public vp_MovingPlatform.RotateInterpolationMode RotationInterpolationMode;

	// Token: 0x04000EAC RID: 3756
	public AudioClip SoundStart;

	// Token: 0x04000EAD RID: 3757
	public AudioClip SoundStop;

	// Token: 0x04000EAE RID: 3758
	public AudioClip SoundMove;

	// Token: 0x04000EAF RID: 3759
	public AudioClip SoundWaypoint;

	// Token: 0x04000EB0 RID: 3760
	protected AudioSource m_Audio;

	// Token: 0x04000EB1 RID: 3761
	public bool PhysicsSnapPlayerToTopOnIntersect = true;

	// Token: 0x04000EB2 RID: 3762
	protected Rigidbody m_RigidBody;

	// Token: 0x04000EB3 RID: 3763
	protected Collider m_Collider;

	// Token: 0x04000EB4 RID: 3764
	protected Collider m_PlayerCollider;

	// Token: 0x04000EB5 RID: 3765
	protected vp_FPPlayerEventHandler m_PlayerToPush;

	// Token: 0x04000EB6 RID: 3766
	public float m_PhysicsPushForce = 2f;

	// Token: 0x04000EB7 RID: 3767
	protected float m_PhysicsCurrentMoveVelocity;

	// Token: 0x04000EB8 RID: 3768
	protected float m_PhysicsCurrentRotationVelocity;

	// Token: 0x04000EB9 RID: 3769
	protected Dictionary<Collider, vp_FPPlayerEventHandler> m_KnownPlayers = new Dictionary<Collider, vp_FPPlayerEventHandler>();

	// Token: 0x020008B6 RID: 2230
	public enum PathMoveType
	{
		// Token: 0x040033BD RID: 13245
		PingPong,
		// Token: 0x040033BE RID: 13246
		Loop,
		// Token: 0x040033BF RID: 13247
		Target
	}

	// Token: 0x020008B7 RID: 2231
	public enum Direction
	{
		// Token: 0x040033C1 RID: 13249
		Forward,
		// Token: 0x040033C2 RID: 13250
		Backwards,
		// Token: 0x040033C3 RID: 13251
		Direct
	}

	// Token: 0x020008B8 RID: 2232
	protected class WaypointComparer : IComparer
	{
		// Token: 0x06004D29 RID: 19753 RVA: 0x001AD9EF File Offset: 0x001ABBEF
		int IComparer.Compare(object x, object y)
		{
			return new CaseInsensitiveComparer().Compare(((Transform)x).name, ((Transform)y).name);
		}
	}

	// Token: 0x020008B9 RID: 2233
	public enum MovementInterpolationMode
	{
		// Token: 0x040033C5 RID: 13253
		EaseInOut,
		// Token: 0x040033C6 RID: 13254
		EaseIn,
		// Token: 0x040033C7 RID: 13255
		EaseOut,
		// Token: 0x040033C8 RID: 13256
		EaseOut2,
		// Token: 0x040033C9 RID: 13257
		Slerp,
		// Token: 0x040033CA RID: 13258
		Lerp
	}

	// Token: 0x020008BA RID: 2234
	public enum RotateInterpolationMode
	{
		// Token: 0x040033CC RID: 13260
		SyncToMovement,
		// Token: 0x040033CD RID: 13261
		EaseOut,
		// Token: 0x040033CE RID: 13262
		CustomEaseOut,
		// Token: 0x040033CF RID: 13263
		CustomRotate
	}
}
