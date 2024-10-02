﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D3 RID: 211
[RequireComponent(typeof(vp_FPPlayerEventHandler))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(vp_FPInput))]
public class vp_FPController : vp_Component
{
	// Token: 0x06000735 RID: 1845 RVA: 0x0007092F File Offset: 0x0006EB2F
	public void ClearPos(float x, float y, float z)
	{
		this.m_PrevPosition.Clear();
		this.m_PrevPos = new Vector3(x, y, z);
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000736 RID: 1846 RVA: 0x0007094A File Offset: 0x0006EB4A
	public bool Grounded
	{
		get
		{
			return this.m_Grounded;
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000737 RID: 1847 RVA: 0x00070952 File Offset: 0x0006EB52
	public bool HeadContact
	{
		get
		{
			return this.m_HeadContact;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000738 RID: 1848 RVA: 0x0007095A File Offset: 0x0006EB5A
	public Vector3 GroundNormal
	{
		get
		{
			return this.m_GroundHit.normal;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000739 RID: 1849 RVA: 0x00070967 File Offset: 0x0006EB67
	public float GroundAngle
	{
		get
		{
			return Vector3.Angle(this.m_GroundHit.normal, Vector3.up);
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x0600073A RID: 1850 RVA: 0x0007097E File Offset: 0x0006EB7E
	public Transform GroundTransform
	{
		get
		{
			return this.m_GroundHit.transform;
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x0600073B RID: 1851 RVA: 0x0007098B File Offset: 0x0006EB8B
	public Vector3 SmoothPosition
	{
		get
		{
			return this.m_SmoothPosition;
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x0600073C RID: 1852 RVA: 0x00070993 File Offset: 0x0006EB93
	public Vector3 Velocity
	{
		get
		{
			return this.m_CharacterController.velocity;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x0600073D RID: 1853 RVA: 0x000709A0 File Offset: 0x0006EBA0
	public CharacterController CharacterController
	{
		get
		{
			return this.m_CharacterController;
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x0600073E RID: 1854 RVA: 0x000709A8 File Offset: 0x0006EBA8
	private vp_FPPlayerEventHandler Player
	{
		get
		{
			if (this.m_Player == null && this.EventHandler != null)
			{
				this.m_Player = (vp_FPPlayerEventHandler)this.EventHandler;
			}
			return this.m_Player;
		}
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x000709E0 File Offset: 0x0006EBE0
	protected override void Awake()
	{
		base.Awake();
		this.m_FPSCamera = base.transform.root.GetComponentInChildren<vp_FPCamera>();
		this.m_Map = (Map)Object.FindObjectOfType(typeof(Map));
		this.m_CharacterController = base.gameObject.GetComponent<CharacterController>();
		this.m_NormalHeight = this.CharacterController.height;
		this.CharacterController.center = (this.m_NormalCenter = new Vector3(0f, this.m_NormalHeight * 0.5f, 0f));
		this.CharacterController.radius = this.m_NormalHeight * 0.25f;
		this.m_CrouchHeight = this.m_NormalHeight * 0.5f;
		this.m_CrouchCenter = this.m_NormalCenter * 0.5f;
		this.m_CrouchHeight = 1.036f;
		this.m_CrouchCenter = new Vector3(0f, 0.518f, 0f);
		this.myTransform = base.transform;
		this.vecDataPos = Random.Range(0, 16);
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x00070AF4 File Offset: 0x0006ECF4
	protected override void Start()
	{
		base.Start();
		this.SetPosition(base.Transform.position);
		if (this.PhysicsHasCollisionTrigger)
		{
			this.m_Trigger = new GameObject("Trigger");
			this.m_Trigger.transform.parent = this.m_Transform;
			CapsuleCollider capsuleCollider = this.m_Trigger.AddComponent<CapsuleCollider>();
			capsuleCollider.isTrigger = true;
			capsuleCollider.radius = this.CharacterController.radius + this.m_SkinWidth;
			capsuleCollider.height = this.CharacterController.height + this.m_SkinWidth * 2f;
			capsuleCollider.center = this.CharacterController.center;
			this.m_Trigger.layer = 30;
			this.m_Trigger.transform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x00070BC3 File Offset: 0x0006EDC3
	protected override void Update()
	{
		base.Update();
		this.SmoothMove();
		this.UpdateMove();
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x00070BD8 File Offset: 0x0006EDD8
	private void UpdateMove()
	{
		bool flag = false;
		Vector3 position = base.gameObject.transform.position;
		if (base.gameObject.transform.position.x > this.m_Map.mlx.y)
		{
			position.x = this.m_Map.mlx.y;
			flag = true;
		}
		if (base.gameObject.transform.position.y > this.m_Map.mly.y)
		{
			position.y = this.m_Map.mly.y;
			flag = true;
		}
		if (base.gameObject.transform.position.z > this.m_Map.mlz.y)
		{
			position.z = this.m_Map.mlz.y;
			flag = true;
		}
		if (base.gameObject.transform.position.x < this.m_Map.mlx.x)
		{
			position.x = this.m_Map.mlx.x;
			flag = true;
		}
		if (base.gameObject.transform.position.y < this.m_Map.mly.x)
		{
			position.y = this.m_Map.mly.x;
			flag = true;
		}
		if (base.gameObject.transform.position.z < this.m_Map.mlz.x)
		{
			position.z = this.m_Map.mlz.x;
			flag = true;
		}
		if (flag)
		{
			base.gameObject.transform.position = position;
			this.m_SmoothPosition = position;
		}
		this.b = this.m_Map.GetBlock((int)this.myTransform.position.x, (int)this.myTransform.position.y, (int)this.myTransform.position.z).block;
		this.bUp = this.m_Map.GetBlock((int)this.myTransform.position.x, (int)this.myTransform.position.y + 1, (int)this.myTransform.position.z).block;
		if (this.bUp != null && this.bUp.GetName() == "!Water")
		{
			this.b = this.bUp;
		}
		int layerMask = 1;
		RaycastHit raycastHit;
		if (!Physics.Raycast(new Ray(base.gameObject.transform.position, -Vector3.up), out raycastHit, 64f, layerMask) && Physics.Raycast(new Ray(base.gameObject.transform.position + 1.65f * Vector3.up, -Vector3.up), out raycastHit, 64f, layerMask))
		{
			base.gameObject.transform.position = raycastHit.point;
		}
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x00070EDC File Offset: 0x0006F0DC
	protected override void FixedUpdate()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		this.UpdateMotor();
		this.UpdateJump();
		this.UpdateForces();
		this.UpdateSliding();
		this.FixedMove();
		this.UpdateCollisions();
		this.UpdatePlatformMove();
		this.m_PrevPos = base.Transform.position;
		if (this.Velocity.magnitude > 4f && this.m_Grounded)
		{
			if (this.b != null)
			{
				if (ZipLoader.GetBlockType(this.b) != this.currBlockType)
				{
					base.GetComponent<Sound>().PlaySound_Stop(null);
					this.currBlockType = ZipLoader.GetBlockType(this.b);
				}
				base.GetComponent<Sound>().PlaySound_Walk(this.currBlockType);
			}
			else
			{
				if (1 != this.currBlockType)
				{
					base.GetComponent<Sound>().PlaySound_Stop(null);
					this.currBlockType = 1;
				}
				base.GetComponent<Sound>().PlaySound_Walk(this.currBlockType);
			}
		}
		else
		{
			base.GetComponent<Sound>().PlaySound_Stop(null);
		}
		this.CheckValue();
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x00070FDD File Offset: 0x0006F1DD
	protected virtual void UpdateMotor()
	{
		if (!this.MotorFreeFly)
		{
			this.UpdateThrottleWalk();
		}
		else
		{
			this.UpdateThrottleFree();
		}
		this.m_MotorThrottle = vp_Utility.SnapToZero(this.m_MotorThrottle, 0.0001f);
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x0007100C File Offset: 0x0006F20C
	protected virtual void UpdateThrottleWalk()
	{
		this.UpdateSlopeFactor();
		this.m_MotorAirSpeedModifier = (this.m_Grounded ? 1f : this.MotorAirSpeed);
		this.m_MotorThrottle += this.m_MoveVector.x * base.Transform.TransformDirection(Vector3.right * this.m_MotorAirSpeedModifier * (this.MotorAcceleration * 0.1f)) * this.m_SlopeFactor;
		this.vecData[this.vecDataPos] = this.m_MoveVector.y * base.Transform.TransformDirection(Vector3.forward * this.m_MotorAirSpeedModifier * (this.MotorAcceleration * 0.1f)) * this.m_SlopeFactor;
		this.m_MotorThrottle += this.vecData[this.vecDataPos];
		this.m_MotorThrottle.x = this.m_MotorThrottle.x / (1f + this.MotorDamping * this.m_MotorAirSpeedModifier * Time.timeScale);
		this.m_MotorThrottle.z = this.m_MotorThrottle.z / (1f + this.MotorDamping * this.m_MotorAirSpeedModifier * Time.timeScale);
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x0007115C File Offset: 0x0006F35C
	protected virtual void UpdateThrottleFree()
	{
		this.m_MotorThrottle += this.m_MoveVector.y * base.Transform.TransformDirection(base.Transform.InverseTransformDirection(this.Player.Forward.Get()) * (this.MotorAcceleration * 0.1f));
		this.m_MotorThrottle += this.m_MoveVector.x * base.Transform.TransformDirection(Vector3.right * (this.MotorAcceleration * 0.1f));
		this.m_MotorThrottle.x = this.m_MotorThrottle.x / (1f + this.MotorDamping * Time.timeScale);
		this.m_MotorThrottle.z = this.m_MotorThrottle.z / (1f + this.MotorDamping * Time.timeScale);
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x0007124C File Offset: 0x0006F44C
	protected virtual void UpdateJump()
	{
		if (!this.MotorFreeFly)
		{
			this.UpdateJumpForceWalk();
		}
		else
		{
			this.UpdateJumpForceFree();
		}
		this.m_MotorThrottle.y = this.m_MotorThrottle.y + this.m_MotorJumpForceAcc * Time.timeScale;
		this.m_MotorJumpForceAcc /= 1f + this.MotorJumpForceHoldDamping * Time.timeScale;
		this.m_MotorThrottle.y = this.m_MotorThrottle.y / (1f + this.MotorJumpForceDamping * Time.timeScale);
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x000712CC File Offset: 0x0006F4CC
	protected virtual void UpdateJumpForceWalk()
	{
		if (this.Player.Jump.Active && !this.m_Grounded)
		{
			if (this.m_MotorJumpForceHoldSkipFrames > 2)
			{
				if (this.m_CharacterController.velocity.y >= 0f)
				{
					this.m_MotorJumpForceAcc += this.MotorJumpForceHold;
					return;
				}
			}
			else
			{
				this.m_MotorJumpForceHoldSkipFrames++;
			}
		}
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x00071338 File Offset: 0x0006F538
	protected virtual void UpdateJumpForceFree()
	{
		if (this.Player.Jump.Active && this.Player.Crouch.Active)
		{
			return;
		}
		if (this.Player.Jump.Active)
		{
			this.m_MotorJumpForceAcc += this.MotorJumpForceHold;
			return;
		}
		if (this.Player.Crouch.Active)
		{
			this.m_MotorJumpForceAcc -= this.MotorJumpForceHold;
			if (this.Grounded && this.CharacterController.height == this.m_NormalHeight)
			{
				this.CharacterController.height = this.m_CrouchHeight;
				this.CharacterController.center = this.m_CrouchCenter;
			}
		}
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x000713F4 File Offset: 0x0006F5F4
	protected virtual void UpdateForces()
	{
		if (this.m_Grounded && this.m_FallSpeed <= 0f)
		{
			this.m_FallSpeed = Physics.gravity.y * (this.PhysicsGravityModifier * 0.002f);
		}
		else
		{
			this.m_FallSpeed += Physics.gravity.y * (this.PhysicsGravityModifier * 0.002f);
		}
		if (this.m_FallSpeed < this.m_LastFallSpeed)
		{
			this.m_HighestFallSpeed = this.m_FallSpeed;
		}
		this.m_LastFallSpeed = this.m_FallSpeed;
		if (this.m_SmoothForceFrame[0] != Vector3.zero)
		{
			this.AddForceInternal(this.m_SmoothForceFrame[0]);
			for (int i = 0; i < 120; i++)
			{
				this.m_SmoothForceFrame[i] = ((i < 119) ? this.m_SmoothForceFrame[i + 1] : Vector3.zero);
				if (this.m_SmoothForceFrame[i] == Vector3.zero)
				{
					break;
				}
			}
		}
		this.m_ExternalForce /= 1f + this.PhysicsForceDamping * Time.timeScale;
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x00071518 File Offset: 0x0006F718
	protected virtual void UpdateSliding()
	{
		bool slideFast = this.m_SlideFast;
		bool slide = this.m_Slide;
		this.m_Slide = false;
		if (!this.m_Grounded)
		{
			this.m_OnSteepGroundSince = 0f;
			this.m_SlideFast = false;
		}
		else if (this.GroundAngle > this.PhysicsSlopeSlideLimit)
		{
			this.m_Slide = true;
			if (this.GroundAngle <= this.m_CharacterController.slopeLimit)
			{
				this.m_SlopeSlideSpeed = Mathf.Max(this.m_SlopeSlideSpeed, this.PhysicsSlopeSlidiness * 0.01f);
				this.m_OnSteepGroundSince = 0f;
				this.m_SlideFast = false;
				this.m_SlopeSlideSpeed = ((Mathf.Abs(this.m_SlopeSlideSpeed) < 0.0001f) ? 0f : (this.m_SlopeSlideSpeed / (1f + 0.05f * Time.timeScale)));
			}
			else
			{
				if (this.m_SlopeSlideSpeed > 0.01f)
				{
					this.m_SlideFast = true;
				}
				if (this.m_OnSteepGroundSince == 0f)
				{
					this.m_OnSteepGroundSince = Time.time;
				}
				this.m_SlopeSlideSpeed += this.PhysicsSlopeSlidiness * 0.01f * ((Time.time - this.m_OnSteepGroundSince) * 0.125f) * Time.timeScale;
				this.m_SlopeSlideSpeed = Mathf.Max(this.PhysicsSlopeSlidiness * 0.01f, this.m_SlopeSlideSpeed);
			}
			this.AddForce(Vector3.Cross(Vector3.Cross(this.GroundNormal, Vector3.down), this.GroundNormal) * this.m_SlopeSlideSpeed * Time.timeScale);
		}
		else
		{
			this.m_OnSteepGroundSince = 0f;
			this.m_SlideFast = false;
			this.m_SlopeSlideSpeed = 0f;
		}
		if (this.m_MotorThrottle != Vector3.zero)
		{
			this.m_Slide = false;
		}
		if (this.m_SlideFast)
		{
			this.m_SlideFallSpeed = base.Transform.position.y;
		}
		else if (slideFast && !this.Grounded)
		{
			this.m_FallSpeed = base.Transform.position.y - this.m_SlideFallSpeed;
		}
		if (slide != this.m_Slide)
		{
			this.Player.SetState("Slide", this.m_Slide, true, false);
		}
		if (slideFast != this.m_SlideFast)
		{
			this.Player.SetState("SlideFast", this.m_SlideFast, true, false);
		}
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x00071760 File Offset: 0x0006F960
	protected virtual void FixedMove()
	{
		this.m_MoveDirection = Vector3.zero;
		this.m_MoveDirection += this.m_ExternalForce;
		this.m_MoveDirection += this.m_MotorThrottle;
		this.m_MoveDirection.y = this.m_MoveDirection.y + this.m_FallSpeed;
		this.m_CurrentAntiBumpOffset = 0f;
		if (this.m_Grounded && this.m_MotorThrottle.y <= 0.001f)
		{
			this.m_CurrentAntiBumpOffset = Mathf.Max(this.m_CharacterController.stepOffset, Vector3.Scale(this.m_MoveDirection, Vector3.one - Vector3.up).magnitude);
			this.m_MoveDirection += this.m_CurrentAntiBumpOffset * Vector3.down;
		}
		this.m_PredictedPos = base.Transform.position + vp_Utility.NaNSafeVector3(this.m_MoveDirection * base.Delta * Time.timeScale, default(Vector3));
		if (this.m_Platform != null && this.m_PositionOnPlatform != Vector3.zero)
		{
			this.m_CharacterController.Move(vp_Utility.NaNSafeVector3(this.m_Platform.TransformPoint(this.m_PositionOnPlatform) - this.m_Transform.position, default(Vector3)));
		}
		this.m_CharacterController.Move(vp_Utility.NaNSafeVector3(this.m_MoveDirection * base.Delta * Time.timeScale, default(Vector3)));
		if (this.Player.Dead.Active)
		{
			this.m_MoveVector = Vector2.zero;
			return;
		}
		Physics.SphereCast(new Ray(base.Transform.position + Vector3.up * this.m_CharacterController.radius, Vector3.down), this.m_CharacterController.radius, out this.m_GroundHit, this.m_SkinWidth + 0.001f, -1744831509);
		this.m_Grounded = (this.m_GroundHit.collider != null);
		this.check_m_Grounded = (this.m_GroundHit.collider != null);
		if (this.m_GroundHit.transform == null && this.m_LastGroundHit.transform != null)
		{
			if (this.m_Platform != null && this.m_PositionOnPlatform != Vector3.zero)
			{
				this.AddForce(this.m_Platform.position - this.m_LastPlatformPos);
				this.m_Platform = null;
			}
			if (this.m_CurrentAntiBumpOffset != 0f)
			{
				this.m_CharacterController.Move(vp_Utility.NaNSafeVector3(this.m_CurrentAntiBumpOffset * Vector3.up, default(Vector3)) * base.Delta * Time.timeScale);
				this.m_PredictedPos += vp_Utility.NaNSafeVector3(this.m_CurrentAntiBumpOffset * Vector3.up, default(Vector3)) * base.Delta * Time.timeScale;
				this.m_MoveDirection += this.m_CurrentAntiBumpOffset * Vector3.up;
			}
		}
		if (this.m_Grounded != this.m_NETGrounded)
		{
			this.m_NETGrounded = this.m_Grounded;
			if (this.m_NETGrounded)
			{
				this.client.send_position(4);
				return;
			}
			this.client.send_position(3);
		}
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x00071B0C File Offset: 0x0006FD0C
	protected virtual void SmoothMove()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		this.m_FixedPosition = base.Transform.position;
		base.Transform.position = this.m_SmoothPosition;
		this.m_CharacterController.Move(vp_Utility.NaNSafeVector3(this.m_MoveDirection * base.Delta * Time.timeScale, default(Vector3)));
		this.m_SmoothPosition = base.Transform.position;
		base.Transform.position = this.m_FixedPosition;
		if (Vector3.Distance(base.Transform.position, this.m_SmoothPosition) > this.m_CharacterController.radius)
		{
			this.m_SmoothPosition = base.Transform.position;
		}
		if (this.m_Platform != null && (this.m_LastPlatformPos.y < this.m_Platform.position.y || this.m_LastPlatformPos.y > this.m_Platform.position.y))
		{
			this.m_SmoothPosition.y = base.Transform.position.y;
		}
		this.m_SmoothPosition = Vector3.Lerp(this.m_SmoothPosition, base.Transform.position, Time.deltaTime);
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x00071C58 File Offset: 0x0006FE58
	protected virtual void UpdateCollisions()
	{
		if (this.m_GroundHit.transform != null && this.m_GroundHit.transform != this.m_LastGroundHit.transform)
		{
			this.m_SmoothPosition.y = base.Transform.position.y;
			if (!this.MotorFreeFly)
			{
				this.m_FallImpact = -this.m_HighestFallSpeed * Time.timeScale;
			}
			else
			{
				this.m_FallImpact = -(this.CharacterController.velocity.y * 0.01f) * Time.timeScale;
			}
			this.DeflectDownForce();
			this.m_HighestFallSpeed = 0f;
			if (this.Player != null && this.Player.FallImpact != null)
			{
				vp_Message<float>.Sender<float> send = this.Player.FallImpact.Send;
				if (send != null)
				{
					send(this.m_FallImpact);
				}
			}
			this.m_MotorThrottle.y = 0f;
			this.m_MotorJumpForceAcc = 0f;
			this.m_MotorJumpForceHoldSkipFrames = 0;
			this.m_Platform = null;
		}
		else
		{
			this.m_FallImpact = 0f;
		}
		this.m_LastGroundHit = this.m_GroundHit;
		if (this.m_PredictedPos.y > base.Transform.position.y && (this.m_ExternalForce.y > 0f || this.m_MotorThrottle.y > 0f))
		{
			this.DeflectUpForce();
		}
		if (this.m_PredictedPos.x != base.Transform.position.x || (this.m_PredictedPos.z != base.Transform.position.z && this.m_ExternalForce != Vector3.zero))
		{
			this.DeflectHorizontalForce();
		}
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x00071E20 File Offset: 0x00070020
	private void UpdatePlatformMove()
	{
		if (this.m_Platform == null)
		{
			return;
		}
		this.m_PositionOnPlatform = this.m_Platform.InverseTransformPoint(this.m_Transform.position);
		this.m_Player.Rotation.Set(new Vector2(this.m_Player.Rotation.Get().x, this.m_Player.Rotation.Get().y - Mathf.DeltaAngle(this.m_Platform.eulerAngles.y, this.m_LastPlatformAngle)));
		this.m_LastPlatformAngle = this.m_Platform.eulerAngles.y;
		this.m_LastPlatformPos = this.m_Platform.position;
		this.m_SmoothPosition = base.Transform.position;
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x00071EFC File Offset: 0x000700FC
	protected virtual void UpdateSlopeFactor()
	{
		if (!this.m_Grounded)
		{
			this.m_SlopeFactor = 1f;
			return;
		}
		this.m_SlopeFactor = 1f + (1f - Vector3.Angle(this.m_GroundHit.normal, this.m_MotorThrottle) / 90f);
		if (Mathf.Abs(1f - this.m_SlopeFactor) < 0.01f)
		{
			this.m_SlopeFactor = 1f;
			return;
		}
		if (this.m_SlopeFactor <= 1f)
		{
			if (this.MotorSlopeSpeedUp == 1f)
			{
				this.m_SlopeFactor *= 1.2f;
			}
			else
			{
				this.m_SlopeFactor *= this.MotorSlopeSpeedUp;
			}
			this.m_SlopeFactor = ((this.GroundAngle > this.m_CharacterController.slopeLimit) ? 0f : this.m_SlopeFactor);
			return;
		}
		if (this.MotorSlopeSpeedDown == 1f)
		{
			this.m_SlopeFactor = 1f / this.m_SlopeFactor;
			this.m_SlopeFactor *= 1.2f;
			return;
		}
		this.m_SlopeFactor *= this.MotorSlopeSpeedDown;
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x0007201C File Offset: 0x0007021C
	public virtual void SetPosition(Vector3 position)
	{
		base.Transform.position = position;
		this.m_SmoothPosition = position;
		this.m_PrevPos = position;
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x00072038 File Offset: 0x00070238
	protected virtual void AddForceInternal(Vector3 force)
	{
		this.m_ExternalForce += force;
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x0007204C File Offset: 0x0007024C
	public virtual void AddForce(float x, float y, float z)
	{
		this.AddForce(new Vector3(x, y, z));
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x0007205C File Offset: 0x0007025C
	public virtual void AddForce(Vector3 force)
	{
		if (Time.timeScale >= 1f)
		{
			this.AddForceInternal(force);
			return;
		}
		this.AddSoftForce(force, 1f);
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x00072080 File Offset: 0x00070280
	public virtual void AddSoftForce(Vector3 force, float frames)
	{
		force /= Time.timeScale;
		frames = Mathf.Clamp(frames, 1f, 120f);
		this.AddForceInternal(force / frames);
		for (int i = 0; i < Mathf.RoundToInt(frames) - 1; i++)
		{
			this.m_SmoothForceFrame[i] += force / frames;
		}
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x000720F0 File Offset: 0x000702F0
	public virtual void StopSoftForce()
	{
		int num = 0;
		while (num < 120 && !(this.m_SmoothForceFrame[num] == Vector3.zero))
		{
			this.m_SmoothForceFrame[num] = Vector3.zero;
			num++;
		}
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00072134 File Offset: 0x00070334
	public virtual void Stop()
	{
		this.m_CharacterController.Move(Vector3.zero);
		this.m_MotorThrottle = Vector3.zero;
		this.m_ExternalForce = Vector3.zero;
		this.StopSoftForce();
		this.m_MoveVector = Vector2.zero;
		this.m_FallSpeed = 0f;
		this.m_SmoothPosition = base.Transform.position;
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x00072198 File Offset: 0x00070398
	public virtual void DeflectDownForce()
	{
		if (this.GroundAngle > this.PhysicsSlopeSlideLimit)
		{
			this.m_SlopeSlideSpeed = this.m_FallImpact * (0.25f * Time.timeScale);
		}
		if (this.GroundAngle > 85f)
		{
			this.m_MotorThrottle += vp_Utility.HorizontalVector(this.GroundNormal * this.m_FallImpact);
			this.m_Grounded = false;
			this.check_m_Grounded = false;
		}
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x00072210 File Offset: 0x00070410
	protected virtual void DeflectUpForce()
	{
		if (!this.m_HeadContact)
		{
			return;
		}
		this.m_NewDir = Vector3.Cross(Vector3.Cross(this.m_CeilingHit.normal, Vector3.up), this.m_CeilingHit.normal);
		this.m_ForceImpact = this.m_MotorThrottle.y + this.m_ExternalForce.y;
		Vector3 a = this.m_NewDir * (this.m_MotorThrottle.y + this.m_ExternalForce.y) * (1f - this.PhysicsWallFriction);
		this.m_ForceImpact -= a.magnitude;
		this.AddForce(a * Time.timeScale);
		this.m_MotorThrottle.y = 0f;
		this.m_ExternalForce.y = 0f;
		this.m_FallSpeed = 0f;
		this.m_NewDir.x = base.Transform.InverseTransformDirection(this.m_NewDir).x;
		this.Player.HeadImpact.Send((this.m_NewDir.x < 0f || (this.m_NewDir.x == 0f && Random.value < 0.5f)) ? (-this.m_ForceImpact) : this.m_ForceImpact);
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00072368 File Offset: 0x00070568
	protected virtual void DeflectHorizontalForce()
	{
		this.m_PredictedPos.y = base.Transform.position.y;
		this.m_PrevPos.y = base.Transform.position.y;
		this.m_PrevDir = (this.m_PredictedPos - this.m_PrevPos).normalized;
		this.CapsuleBottom = this.m_PrevPos + Vector3.up * this.m_CharacterController.radius;
		this.CapsuleTop = this.CapsuleBottom + Vector3.up * (this.m_CharacterController.height - this.m_CharacterController.radius * 2f);
		if (!Physics.CapsuleCast(this.CapsuleBottom, this.CapsuleTop, this.m_CharacterController.radius, this.m_PrevDir, out this.m_WallHit, Vector3.Distance(this.m_PrevPos, this.m_PredictedPos), -1744831509))
		{
			return;
		}
		this.m_NewDir = Vector3.Cross(this.m_WallHit.normal, Vector3.up).normalized;
		if (Vector3.Dot(Vector3.Cross(this.m_WallHit.point - base.Transform.position, this.m_PrevPos - base.Transform.position), Vector3.up) > 0f)
		{
			this.m_NewDir = -this.m_NewDir;
		}
		this.m_ForceMultiplier = Mathf.Abs(Vector3.Dot(this.m_PrevDir, this.m_NewDir)) * (1f - this.PhysicsWallFriction);
		if (this.PhysicsWallBounce > 0f)
		{
			this.m_NewDir = Vector3.Lerp(this.m_NewDir, Vector3.Reflect(this.m_PrevDir, this.m_WallHit.normal), this.PhysicsWallBounce);
			this.m_ForceMultiplier = Mathf.Lerp(this.m_ForceMultiplier, 1f, this.PhysicsWallBounce * (1f - this.PhysicsWallFriction));
		}
		this.m_ForceImpact = 0f;
		float y = this.m_ExternalForce.y;
		this.m_ExternalForce.y = 0f;
		this.m_ForceImpact = this.m_ExternalForce.magnitude;
		this.m_ExternalForce = this.m_NewDir * this.m_ExternalForce.magnitude * this.m_ForceMultiplier;
		this.m_ForceImpact -= this.m_ExternalForce.magnitude;
		int num = 0;
		while (num < 120 && !(this.m_SmoothForceFrame[num] == Vector3.zero))
		{
			this.m_SmoothForceFrame[num] = this.m_SmoothForceFrame[num].magnitude * this.m_NewDir * this.m_ForceMultiplier;
			num++;
		}
		this.m_ExternalForce.y = y;
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x0007264C File Offset: 0x0007084C
	protected virtual void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
		if (attachedRigidbody == null || attachedRigidbody.isKinematic)
		{
			return;
		}
		if (hit.moveDirection.y < -0.3f)
		{
			return;
		}
		Vector3 a = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
		attachedRigidbody.velocity = a * (this.PhysicsPushForce / attachedRigidbody.mass);
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x000726C5 File Offset: 0x000708C5
	protected virtual bool CanStart_Jump()
	{
		return this.MotorFreeFly || (this.m_Grounded && this.m_MotorJumpDone && this.GroundAngle <= this.m_CharacterController.slopeLimit);
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x000726FB File Offset: 0x000708FB
	protected virtual bool CanStart_Walk()
	{
		return !this.Player.Crouch.Active;
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x00072714 File Offset: 0x00070914
	protected virtual void OnStart_Jump()
	{
		this.m_MotorJumpDone = false;
		this.m_SmoothPosition.y = base.Transform.position.y;
		if (this.MotorFreeFly && !this.Grounded)
		{
			return;
		}
		this.m_MotorThrottle.y = this.MotorJumpForce / Time.timeScale;
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x0007276B File Offset: 0x0007096B
	protected virtual void OnStop_Jump()
	{
		this.m_MotorJumpDone = true;
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x00072774 File Offset: 0x00070974
	protected virtual bool CanStop_Crouch()
	{
		if (Physics.SphereCast(new Ray(base.Transform.position, Vector3.up), this.m_CharacterController.radius, this.m_NormalHeight - this.m_CharacterController.radius + 0.01f, -1744831509))
		{
			this.Player.Crouch.NextAllowedStopTime = Time.time + 1f;
			return false;
		}
		return true;
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x000727E3 File Offset: 0x000709E3
	protected virtual void OnStart_Crouch()
	{
		if (this.MotorFreeFly && !this.Grounded)
		{
			return;
		}
		this.CharacterController.height = this.m_CrouchHeight;
		this.CharacterController.center = this.m_CrouchCenter;
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x00072818 File Offset: 0x00070A18
	protected virtual void OnStop_Crouch()
	{
		this.CharacterController.height = this.m_NormalHeight;
		this.CharacterController.center = this.m_NormalCenter;
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x0007283C File Offset: 0x00070A3C
	protected virtual void OnMessage_ForceImpact(Vector3 force)
	{
		this.AddForce(force);
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x00072845 File Offset: 0x00070A45
	protected virtual void OnMessage_Stop()
	{
		this.Stop();
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06000765 RID: 1893 RVA: 0x0007284D File Offset: 0x00070A4D
	// (set) Token: 0x06000766 RID: 1894 RVA: 0x0007285A File Offset: 0x00070A5A
	protected virtual Vector3 OnValue_Position
	{
		get
		{
			return base.Transform.position;
		}
		set
		{
			this.SetPosition(value);
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06000767 RID: 1895 RVA: 0x00072863 File Offset: 0x00070A63
	protected virtual Transform OnValue_Platform
	{
		get
		{
			return this.m_Platform;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000768 RID: 1896 RVA: 0x0007286B File Offset: 0x00070A6B
	// (set) Token: 0x06000769 RID: 1897 RVA: 0x00072873 File Offset: 0x00070A73
	protected virtual Vector2 OnValue_InputMoveVector
	{
		get
		{
			return this.m_MoveVector;
		}
		set
		{
			this.m_MoveVector = value.normalized;
		}
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x00072882 File Offset: 0x00070A82
	private void CheckValue()
	{
		if (Time.time < this.lastcheck + 5f)
		{
			return;
		}
		this.lastcheck = Time.time;
	}

	// Token: 0x04000D0E RID: 3342
	protected vp_FPCamera m_FPSCamera;

	// Token: 0x04000D0F RID: 3343
	protected Map m_Map;

	// Token: 0x04000D10 RID: 3344
	private List<Vector3> m_PrevPosition = new List<Vector3>();

	// Token: 0x04000D11 RID: 3345
	protected CharacterController m_CharacterController;

	// Token: 0x04000D12 RID: 3346
	protected Vector3 m_FixedPosition = Vector3.zero;

	// Token: 0x04000D13 RID: 3347
	protected Vector3 m_SmoothPosition = Vector3.zero;

	// Token: 0x04000D14 RID: 3348
	public Client client;

	// Token: 0x04000D15 RID: 3349
	protected bool fake1;

	// Token: 0x04000D16 RID: 3350
	protected bool fake2;

	// Token: 0x04000D17 RID: 3351
	protected bool m_HeadContact;

	// Token: 0x04000D18 RID: 3352
	protected bool fake3;

	// Token: 0x04000D19 RID: 3353
	protected bool fake4;

	// Token: 0x04000D1A RID: 3354
	protected bool m_Grounded;

	// Token: 0x04000D1B RID: 3355
	protected bool m_NETGrounded;

	// Token: 0x04000D1C RID: 3356
	protected bool fake5;

	// Token: 0x04000D1D RID: 3357
	protected RaycastHit m_GroundHit;

	// Token: 0x04000D1E RID: 3358
	protected RaycastHit m_LastGroundHit;

	// Token: 0x04000D1F RID: 3359
	protected RaycastHit m_CeilingHit;

	// Token: 0x04000D20 RID: 3360
	protected RaycastHit m_WallHit;

	// Token: 0x04000D21 RID: 3361
	protected float m_FallImpact;

	// Token: 0x04000D22 RID: 3362
	protected float m_MotorAirSpeedModifier = 1f;

	// Token: 0x04000D23 RID: 3363
	protected float m_CurrentAntiBumpOffset;

	// Token: 0x04000D24 RID: 3364
	protected float m_SlopeFactor = 1f;

	// Token: 0x04000D25 RID: 3365
	protected Vector3 m_MoveDirection = Vector3.zero;

	// Token: 0x04000D26 RID: 3366
	protected Vector2 m_MoveVector = Vector2.zero;

	// Token: 0x04000D27 RID: 3367
	protected Vector3 m_MotorThrottle = Vector3.zero;

	// Token: 0x04000D28 RID: 3368
	public float MotorAcceleration = 0.18f;

	// Token: 0x04000D29 RID: 3369
	public float MotorDamping = 0.17f;

	// Token: 0x04000D2A RID: 3370
	public float MotorAirSpeed = 0.35f;

	// Token: 0x04000D2B RID: 3371
	public float MotorSlopeSpeedUp = 1f;

	// Token: 0x04000D2C RID: 3372
	public float MotorSlopeSpeedDown = 1f;

	// Token: 0x04000D2D RID: 3373
	public bool MotorFreeFly;

	// Token: 0x04000D2E RID: 3374
	public float MotorJumpForce = 0.2f;

	// Token: 0x04000D2F RID: 3375
	public float MotorJumpForceDamping = 0.1f;

	// Token: 0x04000D30 RID: 3376
	public float MotorJumpForceHold;

	// Token: 0x04000D31 RID: 3377
	public float MotorJumpForceHoldDamping;

	// Token: 0x04000D32 RID: 3378
	protected int m_MotorJumpForceHoldSkipFrames;

	// Token: 0x04000D33 RID: 3379
	protected float m_MotorJumpForceAcc;

	// Token: 0x04000D34 RID: 3380
	private bool m_MotorJumpDone = true;

	// Token: 0x04000D35 RID: 3381
	protected float m_FallSpeed;

	// Token: 0x04000D36 RID: 3382
	protected float m_LastFallSpeed;

	// Token: 0x04000D37 RID: 3383
	protected float m_HighestFallSpeed;

	// Token: 0x04000D38 RID: 3384
	public float PhysicsForceDamping = 0.05f;

	// Token: 0x04000D39 RID: 3385
	public float PhysicsPushForce = 5f;

	// Token: 0x04000D3A RID: 3386
	public float PhysicsGravityModifier = 0.2f;

	// Token: 0x04000D3B RID: 3387
	public float PhysicsSlopeSlideLimit = 30f;

	// Token: 0x04000D3C RID: 3388
	public float PhysicsSlopeSlidiness = 0.15f;

	// Token: 0x04000D3D RID: 3389
	public float PhysicsWallBounce;

	// Token: 0x04000D3E RID: 3390
	public float PhysicsWallFriction;

	// Token: 0x04000D3F RID: 3391
	public bool PhysicsHasCollisionTrigger = true;

	// Token: 0x04000D40 RID: 3392
	protected GameObject m_Trigger;

	// Token: 0x04000D41 RID: 3393
	protected Vector3 m_ExternalForce = Vector3.zero;

	// Token: 0x04000D42 RID: 3394
	protected Vector3[] m_SmoothForceFrame = new Vector3[120];

	// Token: 0x04000D43 RID: 3395
	protected bool m_Slide;

	// Token: 0x04000D44 RID: 3396
	protected bool m_SlideFast;

	// Token: 0x04000D45 RID: 3397
	protected float m_SlideFallSpeed;

	// Token: 0x04000D46 RID: 3398
	protected float m_OnSteepGroundSince;

	// Token: 0x04000D47 RID: 3399
	protected float m_SlopeSlideSpeed;

	// Token: 0x04000D48 RID: 3400
	protected Vector3 m_PredictedPos = Vector3.zero;

	// Token: 0x04000D49 RID: 3401
	protected Vector3 m_PrevPos = Vector3.zero;

	// Token: 0x04000D4A RID: 3402
	protected Vector3 m_PrevDir = Vector3.zero;

	// Token: 0x04000D4B RID: 3403
	protected Vector3 m_NewDir = Vector3.zero;

	// Token: 0x04000D4C RID: 3404
	protected float m_ForceImpact;

	// Token: 0x04000D4D RID: 3405
	protected float m_ForceMultiplier;

	// Token: 0x04000D4E RID: 3406
	protected Vector3 CapsuleBottom = Vector3.zero;

	// Token: 0x04000D4F RID: 3407
	protected Vector3 CapsuleTop = Vector3.zero;

	// Token: 0x04000D50 RID: 3408
	protected float m_SkinWidth = 0.08f;

	// Token: 0x04000D51 RID: 3409
	protected Transform m_Platform;

	// Token: 0x04000D52 RID: 3410
	protected Vector3 m_PositionOnPlatform = Vector3.zero;

	// Token: 0x04000D53 RID: 3411
	protected float m_LastPlatformAngle;

	// Token: 0x04000D54 RID: 3412
	protected Vector3 m_LastPlatformPos = Vector3.zero;

	// Token: 0x04000D55 RID: 3413
	protected float m_NormalHeight;

	// Token: 0x04000D56 RID: 3414
	protected Vector3 m_NormalCenter = Vector3.zero;

	// Token: 0x04000D57 RID: 3415
	protected float m_CrouchHeight;

	// Token: 0x04000D58 RID: 3416
	protected Vector3 m_CrouchCenter = Vector3.zero;

	// Token: 0x04000D59 RID: 3417
	private Block b;

	// Token: 0x04000D5A RID: 3418
	private Block bUp;

	// Token: 0x04000D5B RID: 3419
	private int currBlockType = 1;

	// Token: 0x04000D5C RID: 3420
	private Transform myTransform;

	// Token: 0x04000D5D RID: 3421
	private bool check_m_Grounded;

	// Token: 0x04000D5E RID: 3422
	protected Vector3[] vecData = new Vector3[16];

	// Token: 0x04000D5F RID: 3423
	protected int vecDataPos;

	// Token: 0x04000D60 RID: 3424
	private vp_FPPlayerEventHandler m_Player;

	// Token: 0x04000D61 RID: 3425
	private float maxspeed;

	// Token: 0x04000D62 RID: 3426
	private Vector3 m_PrevPos_;

	// Token: 0x04000D63 RID: 3427
	private int m_PosError;

	// Token: 0x04000D64 RID: 3428
	private Vector3 oldPos = Vector3.zero;

	// Token: 0x04000D65 RID: 3429
	private float lastcheck;
}
