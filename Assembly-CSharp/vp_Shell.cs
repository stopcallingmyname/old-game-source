using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EB RID: 235
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class vp_Shell : MonoBehaviour
{
	// Token: 0x06000883 RID: 2179 RVA: 0x0007B554 File Offset: 0x00079754
	private void Start()
	{
		this.m_Transform = base.transform;
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Audio = base.GetComponent<AudioSource>();
		this.m_RestAngleFunc = null;
		this.m_RemoveTime = Time.time + this.LifeTime;
		this.m_RestTime = Time.time + this.LifeTime * 0.25f;
		this.m_Rigidbody.maxAngularVelocity = 100f;
		base.GetComponent<AudioSource>().playOnAwake = false;
		base.GetComponent<AudioSource>().dopplerLevel = 0f;
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x0007B5E4 File Offset: 0x000797E4
	private void Update()
	{
		if (this.m_RestAngleFunc == null)
		{
			if (Time.time > this.m_RestTime)
			{
				this.DecideRestAngle();
			}
		}
		else
		{
			this.m_RestAngleFunc();
		}
		if (Time.time > this.m_RemoveTime)
		{
			this.m_Transform.localScale = Vector3.Lerp(this.m_Transform.localScale, Vector3.zero, Time.deltaTime * 60f * 0.2f);
			if (Time.time > this.m_RemoveTime + 0.5f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x0007B678 File Offset: 0x00079878
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.magnitude > 2f)
		{
			if (Random.value > 0.5f)
			{
				this.m_Rigidbody.AddRelativeTorque(-Random.rotation.eulerAngles * 0.15f);
			}
			else
			{
				this.m_Rigidbody.AddRelativeTorque(Random.rotation.eulerAngles * 0.15f);
			}
			if (this.m_Audio != null && this.m_BounceSounds.Count > 0)
			{
				this.m_Audio.pitch = Time.timeScale;
				this.m_Audio.PlayOneShot(this.m_BounceSounds[Random.Range(0, this.m_BounceSounds.Count)], AudioListener.volume);
				return;
			}
		}
		else if (Random.value > this.m_Persistence)
		{
			base.GetComponent<Collider>().enabled = false;
			this.m_RemoveTime = Time.time + 0.5f;
		}
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x0007B778 File Offset: 0x00079978
	protected void DecideRestAngle()
	{
		if (Mathf.Abs(this.m_Transform.eulerAngles.x - 270f) < 55f)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(new Ray(this.m_Transform.position, Vector3.down), out raycastHit, 1f) && raycastHit.normal == Vector3.up)
			{
				this.m_RestAngleFunc = new vp_Shell.RestAngleFunc(this.UpRight);
				this.m_Rigidbody.constraints = (RigidbodyConstraints)80;
			}
			return;
		}
		this.m_RestAngleFunc = new vp_Shell.RestAngleFunc(this.TippedOver);
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x0007B810 File Offset: 0x00079A10
	protected void UpRight()
	{
		this.m_Transform.rotation = Quaternion.Lerp(this.m_Transform.rotation, Quaternion.Euler(-90f, this.m_Transform.rotation.y, this.m_Transform.rotation.z), Time.time * (Time.deltaTime * 60f * 0.05f));
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x0007B87C File Offset: 0x00079A7C
	protected void TippedOver()
	{
		this.m_Transform.localRotation = Quaternion.Lerp(this.m_Transform.localRotation, Quaternion.Euler(0f, this.m_Transform.localEulerAngles.y, this.m_Transform.localEulerAngles.z), Time.time * (Time.deltaTime * 60f * 0.005f));
	}

	// Token: 0x04000F04 RID: 3844
	private Transform m_Transform;

	// Token: 0x04000F05 RID: 3845
	private Rigidbody m_Rigidbody;

	// Token: 0x04000F06 RID: 3846
	private AudioSource m_Audio;

	// Token: 0x04000F07 RID: 3847
	public float LifeTime = 10f;

	// Token: 0x04000F08 RID: 3848
	protected float m_RemoveTime;

	// Token: 0x04000F09 RID: 3849
	public float m_Persistence = 1f;

	// Token: 0x04000F0A RID: 3850
	protected vp_Shell.RestAngleFunc m_RestAngleFunc;

	// Token: 0x04000F0B RID: 3851
	protected float m_RestTime;

	// Token: 0x04000F0C RID: 3852
	public List<AudioClip> m_BounceSounds = new List<AudioClip>();

	// Token: 0x020008BB RID: 2235
	// (Invoke) Token: 0x06004D2C RID: 19756
	public delegate void RestAngleFunc();
}
