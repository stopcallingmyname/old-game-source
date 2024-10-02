using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D6 RID: 214
[RequireComponent(typeof(AudioSource))]
public class vp_FPWeapon : vp_Component
{
	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000795 RID: 1941 RVA: 0x000734CB File Offset: 0x000716CB
	public bool Wielded
	{
		get
		{
			return this.m_Wielded && base.Rendering;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06000796 RID: 1942 RVA: 0x000734DD File Offset: 0x000716DD
	public GameObject WeaponCamera
	{
		get
		{
			return this.m_WeaponCamera;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000797 RID: 1943 RVA: 0x000734E5 File Offset: 0x000716E5
	public GameObject WeaponModel
	{
		get
		{
			return this.m_WeaponModel;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06000798 RID: 1944 RVA: 0x000734ED File Offset: 0x000716ED
	public Vector3 DefaultPosition
	{
		get
		{
			return (Vector3)base.DefaultState.Preset.GetFieldValue("PositionOffset");
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06000799 RID: 1945 RVA: 0x00073509 File Offset: 0x00071709
	public Vector3 DefaultRotation
	{
		get
		{
			return (Vector3)base.DefaultState.Preset.GetFieldValue("RotationOffset");
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x0600079A RID: 1946 RVA: 0x00073525 File Offset: 0x00071725
	// (set) Token: 0x0600079B RID: 1947 RVA: 0x0007352D File Offset: 0x0007172D
	public bool DrawRetractionDebugLine
	{
		get
		{
			return this.m_DrawRetractionDebugLine;
		}
		set
		{
			this.m_DrawRetractionDebugLine = value;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x0600079C RID: 1948 RVA: 0x00073536 File Offset: 0x00071736
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

	// Token: 0x0600079D RID: 1949 RVA: 0x0007356C File Offset: 0x0007176C
	protected override void Awake()
	{
		base.Awake();
		if (base.transform.parent == null)
		{
			Debug.LogError("Error (" + this + ") Must not be placed in scene root. Disabling self.");
			vp_Utility.Activate(base.gameObject, false);
			return;
		}
		this.Controller = base.Transform.root.GetComponent<CharacterController>();
		if (this.Controller == null)
		{
			Debug.LogError("Error (" + this + ") Could not find CharacterController. Disabling self.");
			vp_Utility.Activate(base.gameObject, false);
			return;
		}
		base.Transform.eulerAngles = Vector3.zero;
		foreach (object obj in base.Transform.parent)
		{
			Camera camera = (Camera)((Transform)obj).GetComponent(typeof(Camera));
			if (camera != null)
			{
				this.m_WeaponCamera = camera.gameObject;
				break;
			}
		}
		if (base.GetComponent<Collider>() != null)
		{
			base.GetComponent<Collider>().enabled = false;
		}
		this.m_FPSCamera = base.transform.root.GetComponentInChildren<vp_FPCamera>();
		this.m_Input = base.transform.root.GetComponentInChildren<vp_FPInput>();
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x000736C8 File Offset: 0x000718C8
	protected override void Start()
	{
		base.Start();
		this.InstantiateWeaponModel();
		this.m_WeaponGroup = new GameObject(base.name + "Transform");
		this.m_WeaponGroupTransform = this.m_WeaponGroup.transform;
		this.m_WeaponGroupTransform.parent = base.Transform.parent;
		this.m_WeaponGroupTransform.localPosition = this.PositionOffset;
		vp_Layer.Set(this.m_WeaponGroup, 31, false);
		base.Transform.parent = this.m_WeaponGroupTransform;
		base.Transform.localPosition = Vector3.zero;
		this.m_WeaponGroupTransform.localEulerAngles = this.RotationOffset;
		if (this.m_WeaponCamera != null && vp_Utility.IsActive(this.m_WeaponCamera.gameObject))
		{
			vp_Layer.Set(base.gameObject, 31, true);
		}
		this.m_Pivot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		this.m_Pivot.name = "Pivot";
		this.m_Pivot.GetComponent<Collider>().enabled = false;
		this.m_Pivot.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		this.m_Pivot.transform.parent = this.m_WeaponGroupTransform;
		this.m_Pivot.transform.localPosition = Vector3.zero;
		this.m_Pivot.layer = 31;
		vp_Utility.Activate(this.m_Pivot.gameObject, false);
		Material material = new Material(Shader.Find("Transparent/Diffuse"));
		material.color = new Color(0f, 0f, 1f, 0.5f);
		this.m_Pivot.GetComponent<Renderer>().material = material;
		this.m_PositionSpring = new vp_Spring(this.m_WeaponGroup.gameObject.transform, vp_Spring.UpdateMode.Position, true);
		this.m_PositionSpring.RestState = this.PositionOffset;
		this.m_PositionPivotSpring = new vp_Spring(base.Transform, vp_Spring.UpdateMode.Position, true);
		this.m_PositionPivotSpring.RestState = this.PositionPivot;
		this.m_PositionSpring2 = new vp_Spring(base.Transform, vp_Spring.UpdateMode.PositionAdditive, true);
		this.m_PositionSpring2.MinVelocity = 1E-05f;
		this.m_RotationSpring = new vp_Spring(this.m_WeaponGroup.gameObject.transform, vp_Spring.UpdateMode.Rotation, true);
		this.m_RotationSpring.RestState = this.RotationOffset;
		this.m_RotationPivotSpring = new vp_Spring(base.Transform, vp_Spring.UpdateMode.Rotation, true);
		this.m_RotationPivotSpring.RestState = this.RotationPivot;
		this.m_RotationSpring2 = new vp_Spring(this.m_WeaponGroup.gameObject.transform, vp_Spring.UpdateMode.RotationAdditive, true);
		this.m_RotationSpring2.MinVelocity = 1E-05f;
		this.SnapSprings();
		this.Refresh();
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x00073984 File Offset: 0x00071B84
	public virtual void InstantiateWeaponModel()
	{
		if (this.WeaponPrefab != null)
		{
			if (this.m_WeaponModel != null && this.m_WeaponModel != base.gameObject)
			{
				Object.Destroy(this.m_WeaponModel);
			}
			this.m_WeaponModel = Object.Instantiate<GameObject>(this.WeaponPrefab);
			this.m_WeaponModel.transform.parent = base.transform;
			this.m_WeaponModel.transform.localPosition = Vector3.zero;
			this.m_WeaponModel.transform.localScale = new Vector3(1f, 1f, this.RenderingZScale);
			this.m_WeaponModel.transform.localEulerAngles = Vector3.zero;
			foreach (object obj in this.m_WeaponModel.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.name == "left_hand")
				{
					this.m_LeftHandModel = transform.gameObject;
				}
				else if (transform.gameObject.name == "right_hand")
				{
					this.m_RightHandModel = transform.gameObject;
				}
				if (transform.gameObject.name == "block")
				{
					foreach (object obj2 in transform.transform)
					{
						Transform transform2 = (Transform)obj2;
						if (transform2.gameObject.name == "top")
						{
							this.m_Top = transform2.gameObject;
						}
						else if (transform2.gameObject.name == "face")
						{
							this.m_Face = transform2.gameObject;
						}
					}
				}
			}
			if (this.m_WeaponCamera != null && vp_Utility.IsActive(this.m_WeaponCamera.gameObject))
			{
				vp_Layer.Set(this.m_WeaponModel, 31, true);
			}
		}
		else
		{
			this.m_WeaponModel = base.gameObject;
		}
		base.CacheRenderers();
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x00073BE0 File Offset: 0x00071DE0
	protected override void Init()
	{
		base.Init();
		this.ScheduleAmbientAnimation();
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x00073BEE File Offset: 0x00071DEE
	protected override void Update()
	{
		base.Update();
		this.UpdateMouseLook();
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x00073BFC File Offset: 0x00071DFC
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		this.UpdateSwaying();
		this.UpdateBob();
		this.UpdateEarthQuake();
		this.UpdateStep();
		this.UpdateShakes();
		this.UpdateSprings();
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x00073C28 File Offset: 0x00071E28
	public virtual void AddForce2(Vector3 positional, Vector3 angular)
	{
		this.m_PositionSpring2.AddForce(positional);
		this.m_RotationSpring2.AddForce(angular);
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x00073C42 File Offset: 0x00071E42
	public virtual void AddForce2(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot)
	{
		this.AddForce2(new Vector3(xPos, yPos, zPos), new Vector3(xRot, yRot, zRot));
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x00073C5D File Offset: 0x00071E5D
	public virtual void AddForce(Vector3 force)
	{
		this.m_PositionSpring.AddForce(force);
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x00073C6B File Offset: 0x00071E6B
	public virtual void AddForce(float x, float y, float z)
	{
		this.AddForce(new Vector3(x, y, z));
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x00073C7B File Offset: 0x00071E7B
	public virtual void AddForce(Vector3 positional, Vector3 angular)
	{
		this.m_PositionSpring.AddForce(positional);
		this.m_RotationSpring.AddForce(angular);
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x00073C95 File Offset: 0x00071E95
	public virtual void AddForce(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot)
	{
		this.AddForce(new Vector3(xPos, yPos, zPos), new Vector3(xRot, yRot, zRot));
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x00073CB0 File Offset: 0x00071EB0
	public virtual void AddSoftForce(Vector3 force, int frames)
	{
		this.m_PositionSpring.AddSoftForce(force, (float)frames);
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x00073CC0 File Offset: 0x00071EC0
	public virtual void AddSoftForce(float x, float y, float z, int frames)
	{
		this.AddSoftForce(new Vector3(x, y, z), frames);
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x00073CD2 File Offset: 0x00071ED2
	public virtual void AddSoftForce(Vector3 positional, Vector3 angular, int frames)
	{
		this.m_PositionSpring.AddSoftForce(positional, (float)frames);
		this.m_RotationSpring.AddSoftForce(angular, (float)frames);
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x00073CF0 File Offset: 0x00071EF0
	public virtual void AddSoftForce(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, int frames)
	{
		this.AddSoftForce(new Vector3(xPos, yPos, zPos), new Vector3(xRot, yRot, zRot), frames);
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x00073D10 File Offset: 0x00071F10
	protected virtual void UpdateMouseLook()
	{
		this.m_MouseMove.x = Input.GetAxisRaw("Mouse X") / base.Delta * Time.timeScale * Time.timeScale;
		this.m_MouseMove.y = Input.GetAxisRaw("Mouse Y") / base.Delta * Time.timeScale * Time.timeScale;
		this.m_MouseMove *= this.RotationInputVelocityScale;
		this.m_MouseMove = Vector3.Min(this.m_MouseMove, Vector3.one * this.RotationMaxInputVelocity);
		this.m_MouseMove = Vector3.Max(this.m_MouseMove, Vector3.one * -this.RotationMaxInputVelocity);
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x00073DDC File Offset: 0x00071FDC
	protected virtual void UpdateShakes()
	{
		if (this.ShakeSpeed != 0f)
		{
			this.m_Shake = Vector3.Scale(vp_SmoothRandom.GetVector3Centered(this.ShakeSpeed), this.ShakeAmplitude);
			this.m_RotationSpring.AddForce(this.m_Shake * Time.timeScale);
		}
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x00073E30 File Offset: 0x00072030
	protected virtual void UpdateRetraction(bool firstIteration = true)
	{
		if (this.RetractionDistance == 0f)
		{
			return;
		}
		Vector3 vector = this.WeaponModel.transform.TransformPoint(this.RetractionOffset);
		Vector3 end = vector + this.WeaponModel.transform.forward * this.RetractionDistance;
		RaycastHit raycastHit;
		if (Physics.Linecast(vector, end, out raycastHit, -1744831509) && !raycastHit.collider.isTrigger)
		{
			this.WeaponModel.transform.position = raycastHit.point - (raycastHit.point - vector).normalized * (this.RetractionDistance * 0.99f);
			this.WeaponModel.transform.localPosition = Vector3.forward * Mathf.Min(this.WeaponModel.transform.localPosition.z, 0f);
			return;
		}
		if (firstIteration && this.WeaponModel.transform.localPosition != Vector3.zero && this.WeaponModel != base.gameObject)
		{
			this.WeaponModel.transform.localPosition = Vector3.forward * Mathf.SmoothStep(this.WeaponModel.transform.localPosition.z, 0f, this.RetractionRelaxSpeed * Time.timeScale);
			this.UpdateRetraction(false);
		}
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x00073FA4 File Offset: 0x000721A4
	protected virtual void UpdateBob()
	{
		if (this.BobAmplitude == Vector4.zero || this.BobRate == Vector4.zero)
		{
			return;
		}
		this.m_BobSpeed = ((this.BobRequireGroundContact && !this.Controller.isGrounded) ? 0f : this.Controller.velocity.sqrMagnitude);
		this.m_BobSpeed = Mathf.Min(this.m_BobSpeed * this.BobInputVelocityScale, this.BobMaxInputVelocity);
		this.m_BobSpeed = Mathf.Round(this.m_BobSpeed * 1000f) / 1000f;
		if (this.m_BobSpeed == 0f)
		{
			this.m_BobSpeed = Mathf.Min(this.m_LastBobSpeed * 0.93f, this.BobMaxInputVelocity);
		}
		this.m_CurrentBobAmp.x = this.m_BobSpeed * (this.BobAmplitude.x * -0.0001f);
		this.m_CurrentBobVal.x = Mathf.Cos(Time.time * (this.BobRate.x * 10f)) * this.m_CurrentBobAmp.x;
		this.m_CurrentBobAmp.y = this.m_BobSpeed * (this.BobAmplitude.y * 0.0001f);
		this.m_CurrentBobVal.y = Mathf.Cos(Time.time * (this.BobRate.y * 10f)) * this.m_CurrentBobAmp.y;
		this.m_CurrentBobAmp.z = this.m_BobSpeed * (this.BobAmplitude.z * 0.0001f);
		this.m_CurrentBobVal.z = Mathf.Cos(Time.time * (this.BobRate.z * 10f)) * this.m_CurrentBobAmp.z;
		this.m_CurrentBobAmp.w = this.m_BobSpeed * (this.BobAmplitude.w * 0.0001f);
		this.m_CurrentBobVal.w = Mathf.Cos(Time.time * (this.BobRate.w * 10f)) * this.m_CurrentBobAmp.w;
		this.m_RotationSpring.AddForce(this.m_CurrentBobVal * Time.timeScale);
		this.m_PositionSpring.AddForce(Vector3.forward * this.m_CurrentBobVal.w * Time.timeScale);
		this.m_LastBobSpeed = this.m_BobSpeed;
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x00074220 File Offset: 0x00072420
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
		if (!this.Controller.isGrounded)
		{
			return;
		}
		Vector3 vector = this.Player.EarthQuakeForce.Get();
		this.AddForce(new Vector3(0f, 0f, -vector.z * 0.015f), new Vector3(vector.y * 2f, -vector.x, vector.x * 2f));
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x000742BC File Offset: 0x000724BC
	protected virtual void UpdateSprings()
	{
		this.m_PositionSpring.FixedUpdate();
		this.m_PositionPivotSpring.FixedUpdate();
		this.m_RotationPivotSpring.FixedUpdate();
		this.m_PositionSpring2.FixedUpdate();
		this.m_RotationSpring.FixedUpdate();
		this.m_RotationSpring2.FixedUpdate();
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x0007430C File Offset: 0x0007250C
	protected virtual void UpdateStep()
	{
		if (this.StepMinVelocity <= 0f || (this.BobRequireGroundContact && !this.Controller.isGrounded) || this.Controller.velocity.sqrMagnitude < this.StepMinVelocity)
		{
			return;
		}
		bool flag = this.m_LastUpBob < this.m_CurrentBobVal.x;
		this.m_LastUpBob = this.m_CurrentBobVal.x;
		if (flag && !this.m_BobWasElevating)
		{
			if (Mathf.Cos(Time.time * (this.BobRate.x * 5f)) > 0f)
			{
				this.m_PosStep = this.StepPositionForce - this.StepPositionForce * this.StepPositionBalance;
				this.m_RotStep = this.StepRotationForce - this.StepPositionForce * this.StepRotationBalance;
			}
			else
			{
				this.m_PosStep = this.StepPositionForce + this.StepPositionForce * this.StepPositionBalance;
				this.m_RotStep = Vector3.Scale(this.StepRotationForce - this.StepPositionForce * this.StepRotationBalance, -Vector3.one + Vector3.right * 2f);
			}
			this.AddSoftForce(this.m_PosStep * this.StepForceScale, this.m_RotStep * this.StepForceScale, this.StepSoftness);
		}
		this.m_BobWasElevating = flag;
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x00074498 File Offset: 0x00072698
	protected virtual void UpdateSwaying()
	{
		this.m_SwayVel = this.Controller.velocity * this.PositionInputVelocityScale;
		this.m_SwayVel = Vector3.Min(this.m_SwayVel, Vector3.one * this.PositionMaxInputVelocity);
		this.m_SwayVel = Vector3.Max(this.m_SwayVel, Vector3.one * -this.PositionMaxInputVelocity);
		this.m_SwayVel *= Time.timeScale;
		Vector3 vector = base.Transform.InverseTransformDirection(this.m_SwayVel / 60f);
		if (this.m_RotationSpring == null)
		{
			return;
		}
		this.m_RotationSpring.AddForce(new Vector3(this.m_MouseMove.y * (this.RotationLookSway.x * 0.025f), this.m_MouseMove.x * (this.RotationLookSway.y * -0.025f), this.m_MouseMove.x * (this.RotationLookSway.z * -0.025f)));
		this.m_FallSway = this.RotationFallSway * (this.m_SwayVel.y * 0.005f);
		if (this.Controller.isGrounded)
		{
			this.m_FallSway *= this.RotationSlopeSway;
		}
		this.m_FallSway.z = Mathf.Max(0f, this.m_FallSway.z);
		this.m_RotationSpring.AddForce(this.m_FallSway);
		this.m_PositionSpring.AddForce(Vector3.forward * -Mathf.Abs(this.m_SwayVel.y * (this.PositionFallRetract * 2.5E-05f)));
		this.m_PositionSpring.AddForce(new Vector3(vector.x * (this.PositionWalkSlide.x * 0.0016f), -Mathf.Abs(vector.x * (this.PositionWalkSlide.y * 0.0016f)), -vector.z * (this.PositionWalkSlide.z * 0.0016f)));
		this.m_RotationSpring.AddForce(new Vector3(-Mathf.Abs(vector.x * (this.RotationStrafeSway.x * 0.16f)), -(vector.x * (this.RotationStrafeSway.y * 0.16f)), vector.x * (this.RotationStrafeSway.z * 0.16f)));
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x00074710 File Offset: 0x00072910
	public virtual void ResetSprings(float positionReset, float rotationReset, float positionPauseTime = 0f, float rotationPauseTime = 0f)
	{
		this.m_PositionSpring.State = Vector3.Lerp(this.m_PositionSpring.State, this.m_PositionSpring.RestState, positionReset);
		this.m_RotationSpring.State = Vector3.Lerp(this.m_RotationSpring.State, this.m_RotationSpring.RestState, rotationReset);
		this.m_PositionPivotSpring.State = Vector3.Lerp(this.m_PositionPivotSpring.State, this.m_PositionPivotSpring.RestState, positionReset);
		this.m_RotationPivotSpring.State = Vector3.Lerp(this.m_RotationPivotSpring.State, this.m_RotationPivotSpring.RestState, rotationReset);
		if (positionPauseTime != 0f)
		{
			this.m_PositionSpring.ForceVelocityFadeIn(positionPauseTime);
		}
		if (rotationPauseTime != 0f)
		{
			this.m_RotationSpring.ForceVelocityFadeIn(rotationPauseTime);
		}
		if (positionPauseTime != 0f)
		{
			this.m_PositionPivotSpring.ForceVelocityFadeIn(positionPauseTime);
		}
		if (rotationPauseTime != 0f)
		{
			this.m_RotationPivotSpring.ForceVelocityFadeIn(rotationPauseTime);
		}
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x00074810 File Offset: 0x00072A10
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
			this.m_PositionSpring.RestState = this.PositionOffset - this.PositionPivot;
		}
		if (this.m_PositionPivotSpring != null)
		{
			this.m_PositionPivotSpring.Stiffness = new Vector3(this.PositionPivotSpringStiffness, this.PositionPivotSpringStiffness, this.PositionPivotSpringStiffness);
			this.m_PositionPivotSpring.Damping = Vector3.one - new Vector3(this.PositionPivotSpringDamping, this.PositionPivotSpringDamping, this.PositionPivotSpringDamping);
			this.m_PositionPivotSpring.RestState = this.PositionPivot;
		}
		if (this.m_RotationPivotSpring != null)
		{
			this.m_RotationPivotSpring.Stiffness = new Vector3(this.RotationPivotSpringStiffness, this.RotationPivotSpringStiffness, this.RotationPivotSpringStiffness);
			this.m_RotationPivotSpring.Damping = Vector3.one - new Vector3(this.RotationPivotSpringDamping, this.RotationPivotSpringDamping, this.RotationPivotSpringDamping);
			this.m_RotationPivotSpring.RestState = this.RotationPivot;
		}
		if (this.m_PositionSpring2 != null)
		{
			this.m_PositionSpring2.Stiffness = new Vector3(this.PositionSpring2Stiffness, this.PositionSpring2Stiffness, this.PositionSpring2Stiffness);
			this.m_PositionSpring2.Damping = Vector3.one - new Vector3(this.PositionSpring2Damping, this.PositionSpring2Damping, this.PositionSpring2Damping);
			this.m_PositionSpring2.RestState = Vector3.zero;
		}
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.Stiffness = new Vector3(this.RotationSpringStiffness, this.RotationSpringStiffness, this.RotationSpringStiffness);
			this.m_RotationSpring.Damping = Vector3.one - new Vector3(this.RotationSpringDamping, this.RotationSpringDamping, this.RotationSpringDamping);
			this.m_RotationSpring.RestState = this.RotationOffset;
		}
		if (this.m_RotationSpring2 != null)
		{
			this.m_RotationSpring2.Stiffness = new Vector3(this.RotationSpring2Stiffness, this.RotationSpring2Stiffness, this.RotationSpring2Stiffness);
			this.m_RotationSpring2.Damping = Vector3.one - new Vector3(this.RotationSpring2Damping, this.RotationSpring2Damping, this.RotationSpring2Damping);
			this.m_RotationSpring2.RestState = Vector3.zero;
		}
		if (base.Rendering && this.m_WeaponCamera != null && vp_Utility.IsActive(this.m_WeaponCamera.gameObject))
		{
			this.m_WeaponCamera.GetComponent<Camera>().nearClipPlane = this.RenderingClippingPlanes.x;
			this.m_WeaponCamera.GetComponent<Camera>().farClipPlane = this.RenderingClippingPlanes.y;
		}
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x00074AF8 File Offset: 0x00072CF8
	public override void Activate()
	{
		this.m_FPSCamera.RenderingFieldOfView = 65f;
		this.m_FPSCamera.GetComponent<Camera>().fieldOfView = 65f;
		this.m_Input.MouseBlockZoom = Time.time + 0.6f;
		this.m_Wielded = true;
		base.Rendering = true;
		this.m_DeactivationTimer.Cancel();
		if (this.m_WeaponGroup != null && !vp_Utility.IsActive(this.m_WeaponGroup))
		{
			vp_Utility.Activate(this.m_WeaponGroup, true);
		}
		this.SetPivotVisible(false);
		if (this.m_WeaponSystem == null)
		{
			this.m_WeaponSystem = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		this.m_WeaponSystem.OnWeaponSelect(this);
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x00074BBC File Offset: 0x00072DBC
	public override void Deactivate()
	{
		this.m_FPSCamera.RenderingFieldOfView = 65f;
		this.m_FPSCamera.GetComponent<Camera>().fieldOfView = 65f;
		this.m_Wielded = false;
		if (this.m_WeaponGroup != null && vp_Utility.IsActive(this.m_WeaponGroup))
		{
			vp_Utility.Activate(this.m_WeaponGroup, false);
		}
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x00074C1C File Offset: 0x00072E1C
	public virtual void SnapPivot()
	{
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.RestState = this.PositionOffset - this.PositionPivot;
			this.m_PositionSpring.State = this.PositionOffset - this.PositionPivot;
		}
		if (this.m_WeaponGroup != null)
		{
			this.m_WeaponGroupTransform.localPosition = this.PositionOffset - this.PositionPivot;
		}
		if (this.m_PositionPivotSpring != null)
		{
			this.m_PositionPivotSpring.RestState = this.PositionPivot;
			this.m_PositionPivotSpring.State = this.PositionPivot;
		}
		if (this.m_RotationPivotSpring != null)
		{
			this.m_RotationPivotSpring.RestState = this.RotationPivot;
			this.m_RotationPivotSpring.State = this.RotationPivot;
		}
		base.Transform.localPosition = this.PositionPivot;
		base.Transform.localEulerAngles = this.RotationPivot;
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00074D09 File Offset: 0x00072F09
	public virtual void SetPivotVisible(bool visible)
	{
		if (this.m_Pivot == null)
		{
			return;
		}
		vp_Utility.Activate(this.m_Pivot.gameObject, visible);
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00074D2B File Offset: 0x00072F2B
	public virtual void SnapToExit()
	{
		this.RotationOffset = this.RotationExitOffset;
		this.PositionOffset = this.PositionExitOffset;
		this.SnapSprings();
		this.SnapPivot();
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x00074D54 File Offset: 0x00072F54
	public virtual void SnapSprings()
	{
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.RestState = this.PositionOffset - this.PositionPivot;
			this.m_PositionSpring.State = this.PositionOffset - this.PositionPivot;
			this.m_PositionSpring.Stop(true);
		}
		if (this.m_WeaponGroup != null)
		{
			this.m_WeaponGroupTransform.localPosition = this.PositionOffset - this.PositionPivot;
		}
		if (this.m_PositionPivotSpring != null)
		{
			this.m_PositionPivotSpring.RestState = this.PositionPivot;
			this.m_PositionPivotSpring.State = this.PositionPivot;
			this.m_PositionPivotSpring.Stop(true);
		}
		base.Transform.localPosition = this.PositionPivot;
		if (this.m_PositionSpring2 != null)
		{
			this.m_PositionSpring2.RestState = Vector3.zero;
			this.m_PositionSpring2.State = Vector3.zero;
			this.m_PositionSpring2.Stop(true);
		}
		if (this.m_RotationPivotSpring != null)
		{
			this.m_RotationPivotSpring.RestState = this.RotationPivot;
			this.m_RotationPivotSpring.State = this.RotationPivot;
			this.m_RotationPivotSpring.Stop(true);
		}
		base.Transform.localEulerAngles = this.RotationPivot;
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.RestState = this.RotationOffset;
			this.m_RotationSpring.State = this.RotationOffset;
			this.m_RotationSpring.Stop(true);
		}
		if (this.m_RotationSpring2 != null)
		{
			this.m_RotationSpring2.RestState = Vector3.zero;
			this.m_RotationSpring2.State = Vector3.zero;
			this.m_RotationSpring2.Stop(true);
		}
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x00074F04 File Offset: 0x00073104
	public virtual void StopSprings()
	{
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.Stop(true);
		}
		if (this.m_PositionPivotSpring != null)
		{
			this.m_PositionPivotSpring.Stop(true);
		}
		if (this.m_PositionSpring2 != null)
		{
			this.m_PositionSpring2.Stop(true);
		}
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.Stop(true);
		}
		if (this.m_RotationPivotSpring != null)
		{
			this.m_RotationPivotSpring.Stop(true);
		}
		if (this.m_RotationSpring2 != null)
		{
			this.m_RotationSpring2.Stop(true);
		}
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x00074F8C File Offset: 0x0007318C
	public virtual void Wield(bool showWeapon = true)
	{
		if (showWeapon)
		{
			this.SnapToExit();
		}
		this.PositionOffset = (showWeapon ? this.DefaultPosition : this.PositionExitOffset);
		this.RotationOffset = (showWeapon ? this.DefaultRotation : this.RotationExitOffset);
		this.m_Wielded = showWeapon;
		this.Refresh();
		base.StateManager.CombineStates();
		if (base.Audio != null)
		{
			if (this.sound == null)
			{
				this.sound = (Sound)Object.FindObjectOfType(typeof(Sound));
			}
			if (this.SoundWield == null)
			{
				this.SoundWield = this.sound.GetSelect();
			}
			if ((showWeapon ? this.SoundWield : this.SoundUnWield) != null && vp_Utility.IsActive(base.gameObject))
			{
				base.Audio.pitch = Time.timeScale;
				base.Audio.PlayOneShot(showWeapon ? this.SoundWield : this.SoundUnWield, AudioListener.volume);
			}
		}
		if ((showWeapon ? this.AnimationWield : this.AnimationUnWield) != null && vp_Utility.IsActive(base.gameObject))
		{
			this.m_WeaponModel.GetComponent<Animation>().CrossFade((showWeapon ? this.AnimationWield : this.AnimationUnWield).name);
		}
		if (this.WeaponID == 221 && showWeapon)
		{
			this.m_WeaponModel.GetComponent<CustomAnimation>().DrawIn();
		}
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x00075104 File Offset: 0x00073304
	public virtual void ScheduleAmbientAnimation()
	{
		if (this.AnimationAmbient.Count == 0 || !vp_Utility.IsActive(base.gameObject))
		{
			return;
		}
		vp_Timer.In(Random.Range(this.AmbientInterval.x, this.AmbientInterval.y), delegate()
		{
			if (vp_Utility.IsActive(base.gameObject))
			{
				this.m_CurrentAmbientAnimation = Random.Range(0, this.AnimationAmbient.Count);
				if (this.AnimationAmbient[this.m_CurrentAmbientAnimation] != null)
				{
					this.m_WeaponModel.GetComponent<Animation>().CrossFadeQueued(this.AnimationAmbient[this.m_CurrentAmbientAnimation].name);
					this.ScheduleAmbientAnimation();
				}
			}
		}, this.m_AnimationAmbientTimer);
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00075160 File Offset: 0x00073360
	protected virtual void OnMessage_FallImpact(float impact)
	{
		if (this.m_PositionSpring != null)
		{
			this.m_PositionSpring.AddSoftForce(Vector3.down * impact * this.PositionKneeling, (float)this.PositionKneelingSoftness);
		}
		if (this.m_RotationSpring != null)
		{
			this.m_RotationSpring.AddSoftForce(Vector3.right * impact * this.RotationKneeling, (float)this.RotationKneelingSoftness);
		}
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x0000248C File Offset: 0x0000068C
	protected virtual void OnMessage_HeadImpact(float impact)
	{
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x000751CD File Offset: 0x000733CD
	protected virtual void OnMessage_GroundStomp(float impact)
	{
		this.AddForce(Vector3.zero, new Vector3(-0.25f, 0f, 0f) * impact);
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x000751F4 File Offset: 0x000733F4
	protected virtual void OnMessage_BombShake(float impact)
	{
		this.AddForce(Vector3.zero, new Vector3(-0.3f, 0.1f, 0.5f) * impact);
	}

	// Token: 0x04000D9E RID: 3486
	protected WeaponSystem m_WeaponSystem;

	// Token: 0x04000D9F RID: 3487
	protected vp_FPCamera m_FPSCamera;

	// Token: 0x04000DA0 RID: 3488
	public vp_FPInput m_Input;

	// Token: 0x04000DA1 RID: 3489
	public GameObject WeaponPrefab;

	// Token: 0x04000DA2 RID: 3490
	protected GameObject m_WeaponModel;

	// Token: 0x04000DA3 RID: 3491
	public GameObject m_LeftHandModel;

	// Token: 0x04000DA4 RID: 3492
	public GameObject m_RightHandModel;

	// Token: 0x04000DA5 RID: 3493
	public GameObject m_Face;

	// Token: 0x04000DA6 RID: 3494
	public GameObject m_Top;

	// Token: 0x04000DA7 RID: 3495
	protected CharacterController Controller;

	// Token: 0x04000DA8 RID: 3496
	public float RenderingZoomDamping = 0.5f;

	// Token: 0x04000DA9 RID: 3497
	protected float m_FinalZoomTime;

	// Token: 0x04000DAA RID: 3498
	public float RenderingFieldOfView = 65f;

	// Token: 0x04000DAB RID: 3499
	public Vector2 RenderingClippingPlanes = new Vector2(0.01f, 10f);

	// Token: 0x04000DAC RID: 3500
	public float RenderingZScale = 1f;

	// Token: 0x04000DAD RID: 3501
	public Vector3 PositionOffset = new Vector3(0.15f, -0.15f, -0.15f);

	// Token: 0x04000DAE RID: 3502
	public float PositionSpringStiffness = 0.01f;

	// Token: 0x04000DAF RID: 3503
	public float PositionSpringDamping = 0.25f;

	// Token: 0x04000DB0 RID: 3504
	public float PositionFallRetract = 1f;

	// Token: 0x04000DB1 RID: 3505
	public float PositionPivotSpringStiffness = 0.01f;

	// Token: 0x04000DB2 RID: 3506
	public float PositionPivotSpringDamping = 0.25f;

	// Token: 0x04000DB3 RID: 3507
	public float PositionSpring2Stiffness = 0.95f;

	// Token: 0x04000DB4 RID: 3508
	public float PositionSpring2Damping = 0.25f;

	// Token: 0x04000DB5 RID: 3509
	public float PositionKneeling = 0.06f;

	// Token: 0x04000DB6 RID: 3510
	public int PositionKneelingSoftness = 1;

	// Token: 0x04000DB7 RID: 3511
	public Vector3 PositionWalkSlide = new Vector3(0.5f, 0.75f, 0.5f);

	// Token: 0x04000DB8 RID: 3512
	public Vector3 PositionPivot = Vector3.zero;

	// Token: 0x04000DB9 RID: 3513
	public Vector3 RotationPivot = Vector3.zero;

	// Token: 0x04000DBA RID: 3514
	public float PositionInputVelocityScale = 1f;

	// Token: 0x04000DBB RID: 3515
	public float PositionMaxInputVelocity = 25f;

	// Token: 0x04000DBC RID: 3516
	protected vp_Spring m_PositionSpring;

	// Token: 0x04000DBD RID: 3517
	protected vp_Spring m_PositionSpring2;

	// Token: 0x04000DBE RID: 3518
	protected vp_Spring m_PositionPivotSpring;

	// Token: 0x04000DBF RID: 3519
	protected vp_Spring m_RotationPivotSpring;

	// Token: 0x04000DC0 RID: 3520
	protected GameObject m_WeaponCamera;

	// Token: 0x04000DC1 RID: 3521
	protected GameObject m_WeaponGroup;

	// Token: 0x04000DC2 RID: 3522
	protected GameObject m_Pivot;

	// Token: 0x04000DC3 RID: 3523
	protected Transform m_WeaponGroupTransform;

	// Token: 0x04000DC4 RID: 3524
	public Vector3 RotationOffset = Vector3.zero;

	// Token: 0x04000DC5 RID: 3525
	public float RotationSpringStiffness = 0.01f;

	// Token: 0x04000DC6 RID: 3526
	public float RotationSpringDamping = 0.25f;

	// Token: 0x04000DC7 RID: 3527
	public float RotationPivotSpringStiffness = 0.01f;

	// Token: 0x04000DC8 RID: 3528
	public float RotationPivotSpringDamping = 0.25f;

	// Token: 0x04000DC9 RID: 3529
	public float RotationSpring2Stiffness = 0.95f;

	// Token: 0x04000DCA RID: 3530
	public float RotationSpring2Damping = 0.25f;

	// Token: 0x04000DCB RID: 3531
	public float RotationKneeling;

	// Token: 0x04000DCC RID: 3532
	public int RotationKneelingSoftness = 1;

	// Token: 0x04000DCD RID: 3533
	public Vector3 RotationLookSway = new Vector3(1f, 0.7f, 0f);

	// Token: 0x04000DCE RID: 3534
	public Vector3 RotationStrafeSway = new Vector3(0.3f, 1f, 1.5f);

	// Token: 0x04000DCF RID: 3535
	public Vector3 RotationFallSway = new Vector3(1f, -0.5f, -3f);

	// Token: 0x04000DD0 RID: 3536
	public float RotationSlopeSway = 0.5f;

	// Token: 0x04000DD1 RID: 3537
	public float RotationInputVelocityScale = 1f;

	// Token: 0x04000DD2 RID: 3538
	public float RotationMaxInputVelocity = 15f;

	// Token: 0x04000DD3 RID: 3539
	protected vp_Spring m_RotationSpring;

	// Token: 0x04000DD4 RID: 3540
	protected vp_Spring m_RotationSpring2;

	// Token: 0x04000DD5 RID: 3541
	protected Vector3 m_SwayVel = Vector3.zero;

	// Token: 0x04000DD6 RID: 3542
	protected Vector3 m_FallSway = Vector3.zero;

	// Token: 0x04000DD7 RID: 3543
	public float RetractionDistance;

	// Token: 0x04000DD8 RID: 3544
	public Vector2 RetractionOffset = new Vector2(0f, 0f);

	// Token: 0x04000DD9 RID: 3545
	public float RetractionRelaxSpeed = 0.25f;

	// Token: 0x04000DDA RID: 3546
	protected bool m_DrawRetractionDebugLine;

	// Token: 0x04000DDB RID: 3547
	public float ShakeSpeed = 0.05f;

	// Token: 0x04000DDC RID: 3548
	public Vector3 ShakeAmplitude = new Vector3(0.25f, 0f, 2f);

	// Token: 0x04000DDD RID: 3549
	protected Vector3 m_Shake = Vector3.zero;

	// Token: 0x04000DDE RID: 3550
	public Vector4 BobRate = new Vector4(0.9f, 0.45f, 0f, 0f);

	// Token: 0x04000DDF RID: 3551
	public Vector4 BobAmplitude = new Vector4(0.35f, 0.5f, 0f, 0f);

	// Token: 0x04000DE0 RID: 3552
	public float BobInputVelocityScale = 1f;

	// Token: 0x04000DE1 RID: 3553
	public float BobMaxInputVelocity = 100f;

	// Token: 0x04000DE2 RID: 3554
	public bool BobRequireGroundContact = true;

	// Token: 0x04000DE3 RID: 3555
	protected float m_LastBobSpeed;

	// Token: 0x04000DE4 RID: 3556
	protected Vector4 m_CurrentBobAmp = Vector4.zero;

	// Token: 0x04000DE5 RID: 3557
	protected Vector4 m_CurrentBobVal = Vector4.zero;

	// Token: 0x04000DE6 RID: 3558
	protected float m_BobSpeed;

	// Token: 0x04000DE7 RID: 3559
	public Vector3 StepPositionForce = new Vector3(0f, -0.0012f, -0.0012f);

	// Token: 0x04000DE8 RID: 3560
	public Vector3 StepRotationForce = new Vector3(0f, 0f, 0f);

	// Token: 0x04000DE9 RID: 3561
	public int StepSoftness = 4;

	// Token: 0x04000DEA RID: 3562
	public float StepMinVelocity;

	// Token: 0x04000DEB RID: 3563
	public float StepPositionBalance;

	// Token: 0x04000DEC RID: 3564
	public float StepRotationBalance;

	// Token: 0x04000DED RID: 3565
	public float StepForceScale = 1f;

	// Token: 0x04000DEE RID: 3566
	protected float m_LastUpBob;

	// Token: 0x04000DEF RID: 3567
	protected bool m_BobWasElevating;

	// Token: 0x04000DF0 RID: 3568
	protected Vector3 m_PosStep = Vector3.zero;

	// Token: 0x04000DF1 RID: 3569
	protected Vector3 m_RotStep = Vector3.zero;

	// Token: 0x04000DF2 RID: 3570
	public AudioClip SoundWield;

	// Token: 0x04000DF3 RID: 3571
	public AudioClip SoundUnWield;

	// Token: 0x04000DF4 RID: 3572
	private Sound sound;

	// Token: 0x04000DF5 RID: 3573
	public AnimationClip AnimationWield;

	// Token: 0x04000DF6 RID: 3574
	public AnimationClip AnimationUnWield;

	// Token: 0x04000DF7 RID: 3575
	public List<Object> AnimationAmbient = new List<Object>();

	// Token: 0x04000DF8 RID: 3576
	protected List<bool> m_AmbAnimPlayed = new List<bool>();

	// Token: 0x04000DF9 RID: 3577
	public Vector2 AmbientInterval = new Vector2(2.5f, 7.5f);

	// Token: 0x04000DFA RID: 3578
	protected int m_CurrentAmbientAnimation;

	// Token: 0x04000DFB RID: 3579
	protected vp_Timer.Handle m_AnimationAmbientTimer = new vp_Timer.Handle();

	// Token: 0x04000DFC RID: 3580
	public Vector3 PositionExitOffset = new Vector3(0f, -1f, 0f);

	// Token: 0x04000DFD RID: 3581
	public Vector3 RotationExitOffset = new Vector3(40f, 0f, 0f);

	// Token: 0x04000DFE RID: 3582
	protected bool m_Wielded = true;

	// Token: 0x04000DFF RID: 3583
	protected Vector2 m_MouseMove = Vector2.zero;

	// Token: 0x04000E00 RID: 3584
	public int WeaponID;

	// Token: 0x04000E01 RID: 3585
	private vp_FPPlayerEventHandler m_Player;
}
