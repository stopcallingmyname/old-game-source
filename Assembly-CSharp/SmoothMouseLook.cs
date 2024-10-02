using System;
using UnityEngine;

// Token: 0x02000127 RID: 295
[AddComponentMenu("Camera-Control/Mouse Look")]
public class SmoothMouseLook : MonoBehaviour
{
	// Token: 0x06000A5F RID: 2655 RVA: 0x00084CE0 File Offset: 0x00082EE0
	private void Update()
	{
		if (!this.Active)
		{
			return;
		}
		this.zoom_sensitivity = this._camera.GetComponent<Camera>().fieldOfView / 65f;
		if (this.axes == SmoothMouseLook.RotationAxes.MouseXAndY)
		{
			this.xInputOld = this.xInput;
			this.yInputOld = this.yInput;
			this.xInput = Input.GetAxis("Mouse X") * this.sensitivityX * this.zoom_sensitivity;
			this.yInput = Input.GetAxis("Mouse Y") * this.sensitivityY * this.zoom_sensitivity;
			this.averageXInput = this.xInput + this.xInputOld;
			this.averageYInput = this.yInput + this.yInputOld;
			this.averageXInput *= 0.5f;
			this.averageYInput *= 0.5f;
			this.rotationX += this.averageXInput;
			this.rotationY += this.averageYInput;
			this.rotationX = SmoothMouseLook.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
			this.rotationY = SmoothMouseLook.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
			Quaternion rhs = Quaternion.AngleAxis(this.rotationX, Vector3.up);
			Quaternion rhs2 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
			base.transform.localRotation = this.originalRotation * rhs * rhs2;
			return;
		}
		if (this.axes == SmoothMouseLook.RotationAxes.MouseX)
		{
			this.xInputOld = this.xInput;
			this.xInput = Input.GetAxis("Mouse X") * this.sensitivityX * this.zoom_sensitivity;
			this.averageXInput = this.xInput + this.xInputOld;
			this.averageXInput *= 0.5f;
			this.rotationX += this.averageXInput;
			this.rotationX = SmoothMouseLook.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
			Quaternion rhs3 = Quaternion.AngleAxis(this.rotationX, Vector3.up);
			base.transform.localRotation = this.originalRotation * rhs3;
			return;
		}
		this.yInputOld = this.yInput;
		this.yInput = Input.GetAxis("Mouse Y") * this.sensitivityY * this.zoom_sensitivity;
		this.averageYInput = this.yInput + this.yInputOld;
		this.averageYInput *= 0.5f;
		this.rotationY += this.averageYInput;
		this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY * this.zoom_sensitivity;
		this.rotationY = SmoothMouseLook.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
		Quaternion rhs4 = Quaternion.AngleAxis(this.rotationY, Vector3.left);
		base.transform.localRotation = this.originalRotation * rhs4;
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x00084FDC File Offset: 0x000831DC
	private void Start()
	{
		if (base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().freezeRotation = true;
		}
		this.originalRotation = base.transform.localRotation;
		this._camera = GameObject.Find("Main Camera");
		this.sensitivityX = Config.Sensitivity;
		this.sensitivityY = Config.Sensitivity;
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x00031423 File Offset: 0x0002F623
	public static float ClampAngle(float angle, float min, float max)
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

	// Token: 0x0400106B RID: 4203
	public bool Active = true;

	// Token: 0x0400106C RID: 4204
	public SmoothMouseLook.RotationAxes axes;

	// Token: 0x0400106D RID: 4205
	public float sensitivityX = 3f;

	// Token: 0x0400106E RID: 4206
	public float sensitivityY = 3f;

	// Token: 0x0400106F RID: 4207
	public float minimumX = -360f;

	// Token: 0x04001070 RID: 4208
	public float maximumX = 360f;

	// Token: 0x04001071 RID: 4209
	public float minimumY = -60f;

	// Token: 0x04001072 RID: 4210
	public float maximumY = 60f;

	// Token: 0x04001073 RID: 4211
	private float xInput;

	// Token: 0x04001074 RID: 4212
	private float yInput;

	// Token: 0x04001075 RID: 4213
	private float xInputOld;

	// Token: 0x04001076 RID: 4214
	private float yInputOld;

	// Token: 0x04001077 RID: 4215
	private float averageXInput;

	// Token: 0x04001078 RID: 4216
	private float averageYInput;

	// Token: 0x04001079 RID: 4217
	private float rotationX;

	// Token: 0x0400107A RID: 4218
	private float rotationY;

	// Token: 0x0400107B RID: 4219
	private Quaternion originalRotation;

	// Token: 0x0400107C RID: 4220
	private GameObject _camera;

	// Token: 0x0400107D RID: 4221
	private float zoom_sensitivity = 1f;

	// Token: 0x020008CA RID: 2250
	public enum RotationAxes
	{
		// Token: 0x0400340F RID: 13327
		MouseXAndY,
		// Token: 0x04003410 RID: 13328
		MouseX,
		// Token: 0x04003411 RID: 13329
		MouseY
	}
}
