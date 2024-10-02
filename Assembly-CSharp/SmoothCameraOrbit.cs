using System;
using UnityEngine;

// Token: 0x0200013E RID: 318
[AddComponentMenu("Camera-Control/Smooth Mouse Orbit - Unluck Software")]
public class SmoothCameraOrbit : MonoBehaviour
{
	// Token: 0x06000AEF RID: 2799 RVA: 0x000895A1 File Offset: 0x000877A1
	private void Start()
	{
		this.Init();
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x000895A1 File Offset: 0x000877A1
	private void OnEnable()
	{
		this.Init();
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x000895AC File Offset: 0x000877AC
	public void Init()
	{
		if (!this.target)
		{
			this.target = new GameObject("Cam Target")
			{
				transform = 
				{
					position = base.transform.position + base.transform.forward * this.distance
				}
			}.transform;
		}
		this.currentDistance = this.distance;
		this.desiredDistance = this.distance;
		this.position = base.transform.position;
		this.rotation = base.transform.rotation;
		this.currentRotation = base.transform.rotation;
		this.desiredRotation = base.transform.rotation;
		this.xDeg = Vector3.Angle(Vector3.right, base.transform.right);
		this.yDeg = Vector3.Angle(Vector3.up, base.transform.up);
		this.position = this.target.position - (this.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x000896DC File Offset: 0x000878DC
	private void LateUpdate()
	{
		if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl))
		{
			this.desiredDistance -= Input.GetAxis("Mouse Y") * 0.02f * (float)this.zoomRate * 0.125f * Mathf.Abs(this.desiredDistance);
		}
		else if (Input.GetMouseButton(0))
		{
			this.xDeg += Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
			this.yDeg -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
			this.yDeg = SmoothCameraOrbit.ClampAngle(this.yDeg, (float)this.yMinLimit, (float)this.yMaxLimit);
			this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			this.currentRotation = base.transform.rotation;
			this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation, 0.02f * this.zoomDampening);
			base.transform.rotation = this.rotation;
			this.idleTimer = 0f;
			this.idleSmooth = 0f;
		}
		else
		{
			this.idleTimer += 0.02f;
			if (this.idleTimer > this.autoRotate && this.autoRotate > 0f)
			{
				this.idleSmooth += (0.02f + this.idleSmooth) * 0.005f;
				this.idleSmooth = Mathf.Clamp(this.idleSmooth, 0f, 1f);
				this.xDeg += this.xSpeed * Time.deltaTime * this.idleSmooth * this.autoRotateSpeed;
			}
			this.yDeg = SmoothCameraOrbit.ClampAngle(this.yDeg, (float)this.yMinLimit, (float)this.yMaxLimit);
			this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			this.currentRotation = base.transform.rotation;
			this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation, 0.02f * this.zoomDampening * 2f);
			base.transform.rotation = this.rotation;
		}
		this.desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * 0.02f * (float)this.zoomRate * Mathf.Abs(this.desiredDistance);
		this.desiredDistance = Mathf.Clamp(this.desiredDistance, this.minDistance, this.maxDistance);
		this.currentDistance = Mathf.Lerp(this.currentDistance, this.desiredDistance, 0.02f * this.zoomDampening);
		this.position = this.target.position - (this.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
		base.transform.position = this.position;
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x00031423 File Offset: 0x0002F623
	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	// Token: 0x0400117C RID: 4476
	public Transform target;

	// Token: 0x0400117D RID: 4477
	public Vector3 targetOffset;

	// Token: 0x0400117E RID: 4478
	public float distance = 5f;

	// Token: 0x0400117F RID: 4479
	public float maxDistance = 20f;

	// Token: 0x04001180 RID: 4480
	public float minDistance = 0.6f;

	// Token: 0x04001181 RID: 4481
	public float xSpeed = 200f;

	// Token: 0x04001182 RID: 4482
	public float ySpeed = 200f;

	// Token: 0x04001183 RID: 4483
	public int yMinLimit = -80;

	// Token: 0x04001184 RID: 4484
	public int yMaxLimit = 80;

	// Token: 0x04001185 RID: 4485
	public int zoomRate = 40;

	// Token: 0x04001186 RID: 4486
	public float panSpeed = 0.3f;

	// Token: 0x04001187 RID: 4487
	public float zoomDampening = 5f;

	// Token: 0x04001188 RID: 4488
	public float autoRotate = 1f;

	// Token: 0x04001189 RID: 4489
	public float autoRotateSpeed = 0.1f;

	// Token: 0x0400118A RID: 4490
	private float xDeg;

	// Token: 0x0400118B RID: 4491
	private float yDeg;

	// Token: 0x0400118C RID: 4492
	private float currentDistance;

	// Token: 0x0400118D RID: 4493
	private float desiredDistance;

	// Token: 0x0400118E RID: 4494
	private Quaternion currentRotation;

	// Token: 0x0400118F RID: 4495
	private Quaternion desiredRotation;

	// Token: 0x04001190 RID: 4496
	private Quaternion rotation;

	// Token: 0x04001191 RID: 4497
	private Vector3 position;

	// Token: 0x04001192 RID: 4498
	private float idleTimer;

	// Token: 0x04001193 RID: 4499
	private float idleSmooth;
}
