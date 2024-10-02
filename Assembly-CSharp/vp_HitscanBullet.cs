using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E4 RID: 228
[RequireComponent(typeof(AudioSource))]
public class vp_HitscanBullet : MonoBehaviour
{
	// Token: 0x06000837 RID: 2103 RVA: 0x00078E8C File Offset: 0x0007708C
	private void Start()
	{
		Transform transform = base.transform;
		this.m_Audio = base.GetComponent<AudioSource>();
		Ray ray = new Ray(transform.position, base.transform.forward);
		RaycastHit raycastHit;
		if (!Physics.Raycast(ray, out raycastHit, this.Range, this.IgnoreLocalPlayer ? -1811939349 : -738197525))
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Vector3 localScale = transform.localScale;
		transform.parent = raycastHit.transform;
		transform.localPosition = raycastHit.transform.InverseTransformPoint(raycastHit.point);
		transform.rotation = Quaternion.LookRotation(raycastHit.normal);
		if (raycastHit.transform.lossyScale == Vector3.one)
		{
			transform.Rotate(Vector3.forward, (float)Random.Range(0, 360), Space.Self);
		}
		else
		{
			transform.parent = null;
			transform.localScale = localScale;
			transform.parent = raycastHit.transform;
		}
		Rigidbody attachedRigidbody = raycastHit.collider.attachedRigidbody;
		if (attachedRigidbody != null && !attachedRigidbody.isKinematic)
		{
			attachedRigidbody.AddForceAtPosition(ray.direction * this.Force / Time.timeScale, raycastHit.point);
		}
		if (this.m_ImpactPrefab != null)
		{
			Object.Instantiate<GameObject>(this.m_ImpactPrefab, transform.position, transform.rotation);
		}
		if (this.m_DustPrefab != null)
		{
			Object.Instantiate<GameObject>(this.m_DustPrefab, transform.position, transform.rotation);
		}
		if (this.m_SparkPrefab != null && Random.value < this.m_SparkFactor)
		{
			Object.Instantiate<GameObject>(this.m_SparkPrefab, transform.position, transform.rotation);
		}
		if (this.m_DebrisPrefab != null)
		{
			Object.Instantiate<GameObject>(this.m_DebrisPrefab, transform.position, transform.rotation);
		}
		if (this.m_ImpactSounds.Count > 0)
		{
			this.m_Audio.pitch = Random.Range(this.SoundImpactPitch.x, this.SoundImpactPitch.y) * Time.timeScale;
			this.m_Audio.PlayOneShot(this.m_ImpactSounds[Random.Range(0, this.m_ImpactSounds.Count)], AudioListener.volume);
		}
		raycastHit.collider.SendMessageUpwards(this.DamageMethodName, this.Damage, SendMessageOptions.DontRequireReceiver);
		if (this.NoDecalOnTheseLayers.Length != 0)
		{
			foreach (int num in this.NoDecalOnTheseLayers)
			{
				if (raycastHit.transform.gameObject.layer == num)
				{
					this.TryDestroy();
					return;
				}
			}
		}
		if (base.gameObject.GetComponent<Renderer>() != null)
		{
			vp_DecalManager.Add(base.gameObject);
			return;
		}
		vp_Timer.In(1f, new vp_Timer.Callback(this.TryDestroy), null);
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x00079174 File Offset: 0x00077374
	private void TryDestroy()
	{
		if (this == null)
		{
			return;
		}
		if (!this.m_Audio.isPlaying)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		vp_Timer.In(1f, new vp_Timer.Callback(this.TryDestroy), null);
	}

	// Token: 0x04000E82 RID: 3714
	public bool IgnoreLocalPlayer = true;

	// Token: 0x04000E83 RID: 3715
	public float Range = 100f;

	// Token: 0x04000E84 RID: 3716
	public float Force = 100f;

	// Token: 0x04000E85 RID: 3717
	public float Damage = 1f;

	// Token: 0x04000E86 RID: 3718
	public string DamageMethodName = "Damage";

	// Token: 0x04000E87 RID: 3719
	public float m_SparkFactor = 0.5f;

	// Token: 0x04000E88 RID: 3720
	public GameObject m_ImpactPrefab;

	// Token: 0x04000E89 RID: 3721
	public GameObject m_DustPrefab;

	// Token: 0x04000E8A RID: 3722
	public GameObject m_SparkPrefab;

	// Token: 0x04000E8B RID: 3723
	public GameObject m_DebrisPrefab;

	// Token: 0x04000E8C RID: 3724
	protected AudioSource m_Audio;

	// Token: 0x04000E8D RID: 3725
	public List<AudioClip> m_ImpactSounds = new List<AudioClip>();

	// Token: 0x04000E8E RID: 3726
	public Vector2 SoundImpactPitch = new Vector2(1f, 1.5f);

	// Token: 0x04000E8F RID: 3727
	public int[] NoDecalOnTheseLayers;
}
