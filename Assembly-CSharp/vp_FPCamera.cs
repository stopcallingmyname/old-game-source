using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D2 RID: 210
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AudioListener))]
public class vp_FPCamera : vp_Component
{
	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000701 RID: 1793 RVA: 0x0006F3E0 File Offset: 0x0006D5E0
	// (set) Token: 0x06000702 RID: 1794 RVA: 0x0006F3E8 File Offset: 0x0006D5E8
	public bool DrawCameraCollisionDebugLine
	{
		get
		{
			return this.m_DrawCameraCollisionDebugLine;
		}
		set
		{
			this.m_DrawCameraCollisionDebugLine = value;
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000703 RID: 1795 RVA: 0x0006F3F1 File Offset: 0x0006D5F1
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

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000704 RID: 1796 RVA: 0x0006F426 File Offset: 0x0006D626
	// (set) Token: 0x06000705 RID: 1797 RVA: 0x0006F439 File Offset: 0x0006D639
	public Vector2 Angle
	{
		get
		{
			return new Vector2(this.m_Pitch, this.m_Yaw);
		}
		set
		{
			this.Pitch = value.x;
			this.Yaw = value.y;
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000706 RID: 1798 RVA: 0x0006F453 File Offset: 0x0006D653
	public Vector3 Forward
	{
		get
		{
			return this.m_Transform.forward;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000707 RID: 1799 RVA: 0x0006F460 File Offset: 0x0006D660
	// (set) Token: 0x06000708 RID: 1800 RVA: 0x0006F468 File Offset: 0x0006D668
	public float Pitch
	{
		get
		{
			return this.m_Pitch;
		}
		set
		{
			if (value > 90f)
			{
				value -= 360f;
			}
			this.m_Pitch = value;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000709 RID: 1801 RVA: 0x0006F482 File Offset: 0x0006D682
	// (set) Token: 0x0600070A RID: 1802 RVA: 0x0006F48A File Offset: 0x0006D68A
	public float Yaw
	{
		get
		{
			return this.m_Yaw;
		}
		set
		{
			this.m_Yaw = value;
		}
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x0006F494 File Offset: 0x0006D694
	protected override void Awake()
	{
		base.Awake();
		this.FPController = base.Root.GetComponent<vp_FPController>();
		this.m_InitialRotation = new Vector2(base.Transform.eulerAngles.y, base.Transform.eulerAngles.x);
		base.Parent.gameObject.layer = 30;
		foreach (object obj in base.Parent)
		{
			Transform transform = (Transform)obj;
			if (!(transform.gameObject.name == "Snow"))
			{
				transform.gameObject.layer = 30;
			}
		}
		base.GetComponent<Camera>().cullingMask &= 1073741823;
		base.GetComponent<Camera>().depth = 0f;
		foreach (object obj2 in base.Transform)
		{
			Camera camera = (Camera)((Transform)obj2).GetComponent(typeof(Camera));
			if (camera != null)
			{
				camera.transform.localPosition = Vector3.zero;
				camera.transform.localEulerAngles = Vector3.zero;
				camera.clearFlags = CameraClearFlags.Depth;
				camera.cullingMask = int.MinValue;
				camera.depth = 1f;
				camera.farClipPlane = 100f;
				camera.nearClipPlane = 0.01f;
				camera.fieldOfView = 65f;
				break;
			}
		}
		this.m_PositionSpring = new vp_Spring(base.Transform, vp_Spring.UpdateMode.Position, false);
		this.m_PositionSpring.MinVelocity = 1E-05f;
		this.m_PositionSpring.RestState = this.PositionOffset;
		this.m_PositionSpring2 = new vp_Spring(base.Transform, vp_Spring.UpdateMode.PositionAdditive, false);
		this.m_PositionSpring2.MinVelocity = 1E-05f;
		this.m_RotationSpring = new vp_Spring(base.Transform, vp_Spring.UpdateMode.RotationAdditive, false);
		this.m_RotationSpring.MinVelocity = 1E-05f;
		float sensitivity = Config.Sensitivity;
		this.MouseSensitivityRestore = new Vector2(sensitivity, sensitivity);
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x0006F6E0 File Offset: 0x0006D8E0
	protected override void Start()
	{
		base.Start();
		this.Refresh();
		this.SnapSprings();
		this.SnapZoom();
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x0006F6FA File Offset: 0x0006D8FA
	protected override void Init()
	{
		base.Init();
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x0006F702 File Offset: 0x0006D902
	protected override void Update()
	{
		base.Update();
		if (Time.timeScale == 0f)
		{
			return;
		}
		this.UpdateMouseLook();
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x0006F71D File Offset: 0x0006D91D
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (Time.timeScale == 0f)
		{
			return;
		}
		this.UpdateZoom();
		this.UpdateSwaying();
		this.UpdateBob();
		this.UpdateEarthQuake();
		this.UpdateShakes();
		this.UpdateSprings();
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0006F758 File Offset: 0x0006D958
	protected override void LateUpdate()
	{
		base.LateUpdate();
		if (Time.timeScale == 0f)
		{
			return;
		}
		this.m_Transform.position = this.FPController.SmoothPosition;
		this.m_Transform.localPosition += this.m_PositionSpring.State + this.m_PositionSpring2.State;
		this.DoCameraCollision();
		Quaternion lhs = Quaternion.AngleAxis(this.m_Yaw + this.m_InitialRotation.x, Vector3.up);
		Quaternion rhs = Quaternion.AngleAxis(0f, Vector3.left);
		base.Parent.rotation = vp_Utility.NaNSafeQuaternion(lhs * rhs, base.Parent.rotation);
		rhs = Quaternion.AngleAxis(-this.m_Pitch - this.m_InitialRotation.y, Vector3.left);
		base.Transform.rotation = vp_Utility.NaNSafeQuaternion(lhs * rhs, base.Transform.rotation);
		base.Transform.localEulerAngles += vp_Utility.NaNSafeVector3(Vector3.forward * this.m_RotationSpring.State.z, default(Vector3));
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0006F894 File Offset: 0x0006DA94
	protected virtual void DoCameraCollision()
	{
		this.m_CameraCollisionStartPos = this.FPController.Transform.TransformPoint(0f, this.PositionOffset.y, 0f);
		this.m_CameraCollisionEndPos = base.Transform.position + (base.Transform.position - this.m_CameraCollisionStartPos).normalized * this.FPController.CharacterController.radius;
		if (Physics.Linecast(this.m_CameraCollisionStartPos, this.m_CameraCollisionEndPos, out this.m_CameraHit, -1744831509) && !this.m_CameraHit.collider.isTrigger)
		{
			base.Transform.position = this.m_CameraHit.point - (this.m_CameraHit.point - this.m_CameraCollisionStartPos).normalized * this.FPController.CharacterController.radius;
		}
		if (base.Transform.localPosition.y < this.PositionGroundLimit)
		{
			base.Transform.localPosition = new Vector3(base.Transform.localPosition.x, this.PositionGroundLimit, base.Transform.localPosition.z);
		}
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x0006F9E1 File Offset: 0x0006DBE1
	public virtual void AddForce(Vector3 force)
	{
		this.m_PositionSpring.AddForce(force);
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x0006F9EF File Offset: 0x0006DBEF
	public virtual void AddForce(float x, float y, float z)
	{
		this.AddForce(new Vector3(x, y, z));
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x0006F9FF File Offset: 0x0006DBFF
	public virtual void AddForce2(Vector3 force)
	{
		this.m_PositionSpring2.AddForce(force);
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x0006FA0D File Offset: 0x0006DC0D
	public void AddForce2(float x, float y, float z)
	{
		this.AddForce2(new Vector3(x, y, z));
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x0006FA1D File Offset: 0x0006DC1D
	public virtual void AddRollForce(float force)
	{
		this.m_RotationSpring.AddForce(Vector3.forward * force);
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x0006FA35 File Offset: 0x0006DC35
	public virtual void AddRotationForce(Vector3 force)
	{
		this.m_RotationSpring.AddForce(force);
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x0006FA43 File Offset: 0x0006DC43
	public void AddRotationForce(float x, float y, float z)
	{
		this.AddRotationForce(new Vector3(x, y, z));
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x0006FA54 File Offset: 0x0006DC54
	protected virtual void UpdateMouseLook()
	{
		if (MainGUI.ForceCursor)
		{
			return;
		}
		this.m_MouseMove.x = Input.GetAxisRaw("Mouse X") * Time.timeScale;
		this.m_MouseMove.y = Input.GetAxisRaw("Mouse Y") * Time.timeScale;
		this.MouseSmoothSteps = Mathf.Clamp(this.MouseSmoothSteps, 1, 20);
		this.MouseSmoothWeight = Mathf.Clamp01(this.MouseSmoothWeight);
		while (this.m_MouseSmoothBuffer.Count > this.MouseSmoothSteps)
		{
			this.m_MouseSmoothBuffer.RemoveAt(0);
		}
		this.m_MouseSmoothBuffer.Add(this.m_MouseMove);
		float num = 1f;
		Vector2 a = Vector2.zero;
		float num2 = 0f;
		for (int i = this.m_MouseSmoothBuffer.Count - 1; i > 0; i--)
		{
			a += this.m_MouseSmoothBuffer[i] * num;
			num2 += 1f * num;
			num *= this.MouseSmoothWeight / base.Delta;
		}
		num2 = Mathf.Max(1f, num2);
		Vector2 vector = vp_Utility.NaNSafeVector2(a / num2, default(Vector2));
		float num3 = 0f;
		float num4 = Mathf.Abs(vector.x);
		float num5 = Mathf.Abs(vector.y);
		if (this.MouseAcceleration)
		{
			num3 = Mathf.Sqrt(num4 * num4 + num5 * num5) / base.Delta;
			num3 = ((num3 <= this.MouseAccelerationThreshold) ? 0f : num3);
		}
		float num6 = Camera.main.fieldOfView / 65f;
		this.m_Yaw += vector.x * (this.MouseSensitivityRestore.x * num6 + num3);
		this.m_Pitch -= vector.y * (this.MouseSensitivityRestore.y * num6 + num3);
		this.m_Yaw = ((this.m_Yaw < -360f) ? (this.m_Yaw += 360f) : this.m_Yaw);
		this.m_Yaw = ((this.m_Yaw > 360f) ? (this.m_Yaw -= 360f) : this.m_Yaw);
		this.m_Yaw = Mathf.Clamp(this.m_Yaw, this.RotationYawLimit.x, this.RotationYawLimit.y);
		this.m_Pitch = ((this.m_Pitch < -360f) ? (this.m_Pitch += 360f) : this.m_Pitch);
		this.m_Pitch = ((this.m_Pitch > 360f) ? (this.m_Pitch -= 360f) : this.m_Pitch);
		this.m_Pitch = Mathf.Clamp(this.m_Pitch, -this.RotationPitchLimit.x, -this.RotationPitchLimit.y);
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x0006FD48 File Offset: 0x0006DF48
	protected virtual void UpdateZoom()
	{
		if (this.m_FinalZoomTime <= Time.time)
		{
			return;
		}
		this.RenderingZoomDamping = Mathf.Max(this.RenderingZoomDamping, 0.01f);
		float t = 1f - (this.m_FinalZoomTime - Time.time) / this.RenderingZoomDamping;
		base.gameObject.GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(base.gameObject.GetComponent<Camera>().fieldOfView, this.RenderingFieldOfView, t);
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x0006FDBF File Offset: 0x0006DFBF
	public virtual void Zoom()
	{
		this.m_FinalZoomTime = Time.time + this.RenderingZoomDamping;
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x0006FDD3 File Offset: 0x0006DFD3
	public virtual void SnapZoom()
	{
		base.gameObject.GetComponent<Camera>().fieldOfView = this.RenderingFieldOfView;
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x0006FDEC File Offset: 0x0006DFEC
	protected virtual void UpdateShakes()
	{
		if (this.ShakeSpeed != 0f)
		{
			this.m_Yaw -= this.m_Shake.y;
			this.m_Pitch -= this.m_Shake.x;
			this.m_Shake = Vector3.Scale(vp_SmoothRandom.GetVector3Centered(this.ShakeSpeed), this.ShakeAmplitude);
			this.m_Yaw += this.m_Shake.y;
			this.m_Pitch += this.m_Shake.x;
			this.m_RotationSpring.AddForce(Vector3.forward * this.m_Shake.z * Time.timeScale);
		}
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x0006FEB0 File Offset: 0x0006E0B0
	protected virtual void UpdateBob()
	{
		if (this.BobAmplitude == Vector4.zero || this.BobRate == Vector4.zero)
		{
			return;
		}
		this.m_BobSpeed = ((this.BobRequireGroundContact && !this.FPController.Grounded) ? 0f : this.FPController.CharacterController.velocity.sqrMagnitude);
		this.m_BobSpeed = Mathf.Min(this.m_BobSpeed * this.BobInputVelocityScale, this.BobMaxInputVelocity);
		this.m_BobSpeed = Mathf.Round(this.m_BobSpeed * 1000f) / 1000f;
		if (this.m_BobSpeed == 0f)
		{
			this.m_BobSpeed = Mathf.Min(this.m_LastBobSpeed * 0.93f, this.BobMaxInputVelocity);
		}
		this.m_CurrentBobAmp.y = this.m_BobSpeed * (this.BobAmplitude.y * -0.0001f);
		this.m_CurrentBobVal.y = Mathf.Cos(Time.time * (this.BobRate.y * 10f)) * this.m_CurrentBobAmp.y;
		this.m_CurrentBobAmp.x = this.m_BobSpeed * (this.BobAmplitude.x * 0.0001f);
		this.m_CurrentBobVal.x = Mathf.Cos(Time.time * (this.BobRate.x * 10f)) * this.m_CurrentBobAmp.x;
		this.m_CurrentBobAmp.z = this.m_BobSpeed * (this.BobAmplitude.z * 0.0001f);
		this.m_CurrentBobVal.z = Mathf.Cos(Time.time * (this.BobRate.z * 10f)) * this.m_CurrentBobAmp.z;
		this.m_CurrentBobAmp.w = this.m_BobSpeed * (this.BobAmplitude.w * 0.0001f);
		this.m_CurrentBobVal.w = Mathf.Cos(Time.time * (this.BobRate.w * 10f)) * this.m_CurrentBobAmp.w;
		this.m_PositionSpring.AddForce(this.m_CurrentBobVal * Time.timeScale);
		this.AddRollForce(this.m_CurrentBobVal.w * Time.timeScale);
		this.m_LastBobSpeed = this.m_BobSpeed;
		this.DetectBobStep(this.m_BobSpeed, this.m_CurrentBobVal.y);
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x00070134 File Offset: 0x0006E334
	protected virtual void DetectBobStep(float speed, float upBob)
	{
		if (this.BobStepCallback == null)
		{
			return;
		}
		if (speed < this.BobStepThreshold)
		{
			return;
		}
		bool flag = this.m_LastUpBob < upBob;
		this.m_LastUpBob = upBob;
		if (flag && !this.m_BobWasElevating)
		{
			this.BobStepCallback();
		}
		this.m_BobWasElevating = flag;
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x00070188 File Offset: 0x0006E388
	protected virtual void UpdateSwaying()
	{
		Vector3 vector = base.Transform.InverseTransformDirection(this.FPController.CharacterController.velocity * 0.016f) * Time.timeScale;
		this.AddRollForce(vector.x * this.RotationStrafeRoll);
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x000701D8 File Offset: 0x0006E3D8
	protected virtual void UpdateEarthQuake()
	{
		if (this.Player == null)
		{
			return;
		}
		if (!this.Player.Earthquake.Active)
		{
			return;
		}
		if (this.m_PositionSpring.State.y >= this.m_PositionSpring.RestState.y)
		{
			Vector3 vector = this.Player.EarthQuakeForce.Get();
			vector.y = -vector.y;
			this.Player.EarthQuakeForce.Set(vector);
		}
		this.m_PositionSpring.AddForce(this.Player.EarthQuakeForce.Get() * this.PositionEarthQuakeFactor);
		this.m_RotationSpring.AddForce(Vector3.forward * (-this.Player.EarthQuakeForce.Get().x * 2f) * this.RotationEarthQuakeFactor);
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x000702CF File Offset: 0x0006E4CF
	protected virtual void UpdateSprings()
	{
		this.m_PositionSpring.FixedUpdate();
		this.m_PositionSpring2.FixedUpdate();
		this.m_RotationSpring.FixedUpdate();
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x000702F4 File Offset: 0x0006E4F4
	public virtual void DoBomb(Vector3 positionForce, float minRollForce, float maxRollForce)
	{
		this.AddForce2(positionForce);
		float num = Random.Range(minRollForce, maxRollForce);
		if (Random.value > 0.5f)
		{
			num = -num;
		}
		this.AddRollForce(num);
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x00070328 File Offset: 0x0006E528
	public override void Refresh()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.Stiffness = new Vector3(this.PositionSpringStiffness, this.PositionSpringStiffness, this.PositionSpringStiffness);
			this.m_PositionSpring.Damping = Vector3.one - new Vector3(this.PositionSpringDamping, this.PositionSpringDamping, this.PositionSpringDamping);
			this.m_PositionSpring.MinState.y = this.PositionGroundLimit;
			this.m_PositionSpring.RestState = this.PositionOffset;
		}
		if (this.m_PositionSpring2 != null)
		{
			this.m_PositionSpring2.Stiffness = new Vector3(this.PositionSpring2Stiffness, this.PositionSpring2Stiffness, this.PositionSpring2Stiffness);
			this.m_PositionSpring2.Damping = Vector3.one - new Vector3(this.PositionSpring2Damping, this.PositionSpring2Damping, this.PositionSpring2Damping);
			this.m_PositionSpring2.MinState.y = -this.PositionOffset.y + this.PositionGroundLimit;
		}
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.Stiffness = new Vector3(this.RotationSpringStiffness, this.RotationSpringStiffness, this.RotationSpringStiffness);
			this.m_RotationSpring.Damping = Vector3.one - new Vector3(this.RotationSpringDamping, this.RotationSpringDamping, this.RotationSpringDamping);
		}
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x0007048C File Offset: 0x0006E68C
	public virtual void SnapSprings()
	{
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.RestState = this.PositionOffset;
			this.m_PositionSpring.State = this.PositionOffset;
			this.m_PositionSpring.Stop(true);
		}
		if (this.m_PositionSpring2 != null)
		{
			this.m_PositionSpring2.RestState = Vector3.zero;
			this.m_PositionSpring2.State = Vector3.zero;
			this.m_PositionSpring2.Stop(true);
		}
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.RestState = Vector3.zero;
			this.m_RotationSpring.State = Vector3.zero;
			this.m_RotationSpring.Stop(true);
		}
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x00070538 File Offset: 0x0006E738
	public virtual void StopSprings()
	{
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.Stop(true);
		}
		if (this.m_PositionSpring2 != null)
		{
			this.m_PositionSpring2.Stop(true);
		}
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.Stop(true);
		}
		this.m_BobSpeed = 0f;
		this.m_LastBobSpeed = 0f;
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00070597 File Offset: 0x0006E797
	public virtual void Stop()
	{
		this.SnapSprings();
		this.SnapZoom();
		this.Refresh();
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x000705AB File Offset: 0x0006E7AB
	public virtual void SetRotation(Vector2 eulerAngles, bool stop = true, bool resetInitialRotation = true)
	{
		this.Angle = eulerAngles;
		if (stop)
		{
			this.Stop();
		}
		if (resetInitialRotation)
		{
			this.m_InitialRotation = Vector2.zero;
		}
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x000705CC File Offset: 0x0006E7CC
	protected virtual void OnMessage_FallImpact(float impact)
	{
		impact = Mathf.Abs(impact * 55f);
		float num = impact * this.PositionKneeling;
		float num2 = impact * this.RotationKneeling;
		num = Mathf.SmoothStep(0f, 1f, num);
		num2 = Mathf.SmoothStep(0f, 1f, num2);
		num2 = Mathf.SmoothStep(0f, 1f, num2);
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.AddSoftForce(Vector3.down * num, (float)this.PositionKneelingSoftness);
		}
		if (this.m_RotationSpring != null)
		{
			float d = (Random.value > 0.5f) ? (num2 * 2f) : (-(num2 * 2f));
			this.m_RotationSpring.AddSoftForce(Vector3.forward * d, (float)this.RotationKneelingSoftness);
		}
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x0000248C File Offset: 0x0000068C
	protected virtual void OnMessage_HeadImpact(float impact)
	{
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x00070698 File Offset: 0x0006E898
	protected virtual void OnMessage_GroundStomp(float impact)
	{
		this.AddForce2(new Vector3(0f, -1f, 0f) * impact);
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x000706BA File Offset: 0x0006E8BA
	protected virtual void OnMessage_BombShake(float impact)
	{
		this.DoBomb(new Vector3(1f, -10f, 1f) * impact, 1f, 2f);
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x000706E6 File Offset: 0x0006E8E6
	protected virtual bool CanStart_Walk()
	{
		this.Player == null;
		return true;
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x0600072E RID: 1838 RVA: 0x000706F6 File Offset: 0x0006E8F6
	// (set) Token: 0x0600072F RID: 1839 RVA: 0x000706FE File Offset: 0x0006E8FE
	protected virtual Vector2 OnValue_Rotation
	{
		get
		{
			return this.Angle;
		}
		set
		{
			this.Angle = value;
		}
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x00070707 File Offset: 0x0006E907
	protected virtual void OnMessage_Stop()
	{
		this.Stop();
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000731 RID: 1841 RVA: 0x0007070F File Offset: 0x0006E90F
	protected virtual Vector3 OnValue_Forward
	{
		get
		{
			return this.Forward;
		}
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x00070717 File Offset: 0x0006E917
	public void SetMouseFreeze(bool val)
	{
		this.MouseFreeze = val;
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x00070720 File Offset: 0x0006E920
	public void SetMouseSensitivity(float sens)
	{
		this.MouseSensitivityRestore = new Vector2(sens, sens);
	}

	// Token: 0x04000CD6 RID: 3286
	public vp_FPController FPController;

	// Token: 0x04000CD7 RID: 3287
	private bool MouseFreeze;

	// Token: 0x04000CD8 RID: 3288
	private Vector2 MouseSensitivityRestore = new Vector2(5f, 5f);

	// Token: 0x04000CD9 RID: 3289
	public int MouseSmoothSteps = 10;

	// Token: 0x04000CDA RID: 3290
	public float MouseSmoothWeight = 0.5f;

	// Token: 0x04000CDB RID: 3291
	public bool MouseAcceleration;

	// Token: 0x04000CDC RID: 3292
	public float MouseAccelerationThreshold = 0.4f;

	// Token: 0x04000CDD RID: 3293
	protected Vector2 m_MouseMove = Vector2.zero;

	// Token: 0x04000CDE RID: 3294
	protected List<Vector2> m_MouseSmoothBuffer = new List<Vector2>();

	// Token: 0x04000CDF RID: 3295
	public float RenderingFieldOfView = 65f;

	// Token: 0x04000CE0 RID: 3296
	public float RenderingZoomDamping = 0.2f;

	// Token: 0x04000CE1 RID: 3297
	public float m_FinalZoomTime;

	// Token: 0x04000CE2 RID: 3298
	public Vector3 PositionOffset = new Vector3(0f, 1.635f, 0f);

	// Token: 0x04000CE3 RID: 3299
	public float PositionGroundLimit = 0.1f;

	// Token: 0x04000CE4 RID: 3300
	public float PositionSpringStiffness = 0.01f;

	// Token: 0x04000CE5 RID: 3301
	public float PositionSpringDamping = 0.25f;

	// Token: 0x04000CE6 RID: 3302
	public float PositionSpring2Stiffness = 0.95f;

	// Token: 0x04000CE7 RID: 3303
	public float PositionSpring2Damping = 0.25f;

	// Token: 0x04000CE8 RID: 3304
	public float PositionKneeling = 0.025f;

	// Token: 0x04000CE9 RID: 3305
	public int PositionKneelingSoftness = 1;

	// Token: 0x04000CEA RID: 3306
	public float PositionEarthQuakeFactor = 1f;

	// Token: 0x04000CEB RID: 3307
	protected vp_Spring m_PositionSpring;

	// Token: 0x04000CEC RID: 3308
	protected vp_Spring m_PositionSpring2;

	// Token: 0x04000CED RID: 3309
	protected bool m_DrawCameraCollisionDebugLine;

	// Token: 0x04000CEE RID: 3310
	public Vector2 RotationPitchLimit = new Vector2(90f, -90f);

	// Token: 0x04000CEF RID: 3311
	public Vector2 RotationYawLimit = new Vector2(-360f, 360f);

	// Token: 0x04000CF0 RID: 3312
	public float RotationSpringStiffness = 0.01f;

	// Token: 0x04000CF1 RID: 3313
	public float RotationSpringDamping = 0.25f;

	// Token: 0x04000CF2 RID: 3314
	public float RotationKneeling = 0.025f;

	// Token: 0x04000CF3 RID: 3315
	public int RotationKneelingSoftness = 1;

	// Token: 0x04000CF4 RID: 3316
	public float RotationStrafeRoll = 0.01f;

	// Token: 0x04000CF5 RID: 3317
	public float RotationEarthQuakeFactor;

	// Token: 0x04000CF6 RID: 3318
	protected float m_Pitch;

	// Token: 0x04000CF7 RID: 3319
	protected float m_Yaw;

	// Token: 0x04000CF8 RID: 3320
	protected vp_Spring m_RotationSpring;

	// Token: 0x04000CF9 RID: 3321
	protected Vector2 m_InitialRotation = Vector2.zero;

	// Token: 0x04000CFA RID: 3322
	public float ShakeSpeed;

	// Token: 0x04000CFB RID: 3323
	public Vector3 ShakeAmplitude = new Vector3(10f, 10f, 0f);

	// Token: 0x04000CFC RID: 3324
	protected Vector3 m_Shake = Vector3.zero;

	// Token: 0x04000CFD RID: 3325
	public Vector4 BobRate = new Vector4(0f, 1.4f, 0f, 0.7f);

	// Token: 0x04000CFE RID: 3326
	public Vector4 BobAmplitude = new Vector4(0f, 0.25f, 0f, 0.5f);

	// Token: 0x04000CFF RID: 3327
	public float BobInputVelocityScale = 1f;

	// Token: 0x04000D00 RID: 3328
	public float BobMaxInputVelocity = 100f;

	// Token: 0x04000D01 RID: 3329
	public bool BobRequireGroundContact = true;

	// Token: 0x04000D02 RID: 3330
	protected float m_LastBobSpeed;

	// Token: 0x04000D03 RID: 3331
	protected Vector4 m_CurrentBobAmp = Vector4.zero;

	// Token: 0x04000D04 RID: 3332
	protected Vector4 m_CurrentBobVal = Vector4.zero;

	// Token: 0x04000D05 RID: 3333
	protected float m_BobSpeed;

	// Token: 0x04000D06 RID: 3334
	public vp_FPCamera.BobStepDelegate BobStepCallback;

	// Token: 0x04000D07 RID: 3335
	public float BobStepThreshold = 10f;

	// Token: 0x04000D08 RID: 3336
	protected float m_LastUpBob;

	// Token: 0x04000D09 RID: 3337
	protected bool m_BobWasElevating;

	// Token: 0x04000D0A RID: 3338
	protected Vector3 m_CameraCollisionStartPos = Vector3.zero;

	// Token: 0x04000D0B RID: 3339
	protected Vector3 m_CameraCollisionEndPos = Vector3.zero;

	// Token: 0x04000D0C RID: 3340
	protected RaycastHit m_CameraHit;

	// Token: 0x04000D0D RID: 3341
	private vp_FPPlayerEventHandler m_Player;

	// Token: 0x020008B1 RID: 2225
	// (Invoke) Token: 0x06004D16 RID: 19734
	public delegate void BobStepDelegate();
}
