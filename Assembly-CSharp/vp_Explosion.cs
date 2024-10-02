using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E3 RID: 227
[RequireComponent(typeof(AudioSource))]
public class vp_Explosion : MonoBehaviour
{
	// Token: 0x06000834 RID: 2100 RVA: 0x00078B88 File Offset: 0x00076D88
	private void Awake()
	{
		Transform transform = base.transform;
		this.m_Audio = base.GetComponent<AudioSource>();
		foreach (GameObject gameObject in this.FXPrefabs)
		{
			Component[] components = gameObject.GetComponents<vp_Explosion>();
			if (components.Length == 0)
			{
				Object.Instantiate<GameObject>(gameObject, transform.position, transform.rotation);
			}
			else
			{
				Debug.LogError("Error: vp_Explosion->FXPrefab must not be a vp_Explosion (risk of infinite loop).");
			}
		}
		foreach (Collider collider in Physics.OverlapSphere(transform.position, this.Radius, -738197525))
		{
			if (collider != base.GetComponent<Collider>())
			{
				float num = 1f - Vector3.Distance(transform.position, collider.transform.position) / this.Radius;
				if (collider.GetComponent<Rigidbody>())
				{
					RaycastHit raycastHit;
					if (!Physics.Raycast(new Ray(collider.transform.position, -Vector3.up), out raycastHit, 1f))
					{
						this.UpForce = 0f;
					}
					collider.GetComponent<Rigidbody>().AddExplosionForce(this.Force / Time.timeScale, transform.position, this.Radius, this.UpForce);
				}
				else if (collider.gameObject.layer == 30)
				{
					vp_FPPlayerEventHandler component = collider.GetComponent<vp_FPPlayerEventHandler>();
					if (component)
					{
						component.ForceImpact.Send((collider.transform.position - transform.position).normalized * this.Force * 0.001f * num);
						component.BombShake.Send(num * this.CameraShake);
					}
				}
				if (collider.gameObject.layer != 29)
				{
					collider.gameObject.BroadcastMessage(this.DamageMessageName, num * this.Damage, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		this.m_Audio.clip = this.Sound;
		this.m_Audio.pitch = Random.Range(this.SoundMinPitch, this.SoundMaxPitch) * Time.timeScale;
		if (!this.m_Audio.playOnAwake)
		{
			this.m_Audio.Play();
		}
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x00078DF8 File Offset: 0x00076FF8
	private void Update()
	{
		if (!this.m_Audio.isPlaying)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000E77 RID: 3703
	public float Radius = 15f;

	// Token: 0x04000E78 RID: 3704
	public float Force = 1000f;

	// Token: 0x04000E79 RID: 3705
	public float UpForce = 10f;

	// Token: 0x04000E7A RID: 3706
	public float Damage = 10f;

	// Token: 0x04000E7B RID: 3707
	public float CameraShake = 1f;

	// Token: 0x04000E7C RID: 3708
	public string DamageMessageName = "Damage";

	// Token: 0x04000E7D RID: 3709
	private AudioSource m_Audio;

	// Token: 0x04000E7E RID: 3710
	public AudioClip Sound;

	// Token: 0x04000E7F RID: 3711
	public float SoundMinPitch = 0.8f;

	// Token: 0x04000E80 RID: 3712
	public float SoundMaxPitch = 1.2f;

	// Token: 0x04000E81 RID: 3713
	public List<GameObject> FXPrefabs = new List<GameObject>();
}
