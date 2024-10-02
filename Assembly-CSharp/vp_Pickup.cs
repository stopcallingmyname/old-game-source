using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E7 RID: 231
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
public abstract class vp_Pickup : MonoBehaviour
{
	// Token: 0x0600085A RID: 2138 RVA: 0x0007A32C File Offset: 0x0007852C
	protected virtual void Start()
	{
		this.m_Transform = base.transform;
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Audio = base.GetComponent<AudioSource>();
		if (Camera.main != null)
		{
			this.m_CameraMainTransform = Camera.main.transform;
		}
		base.GetComponent<Collider>().isTrigger = true;
		this.m_Audio.clip = this.PickupSound;
		this.m_Audio.playOnAwake = false;
		this.m_Audio.minDistance = 3f;
		this.m_Audio.maxDistance = 150f;
		this.m_Audio.rolloffMode = AudioRolloffMode.Linear;
		this.m_Audio.dopplerLevel = 0f;
		this.m_SpawnPosition = this.m_Transform.position;
		this.m_SpawnScale = this.m_Transform.localScale;
		this.RespawnScaleUpDuration = ((this.m_Rigidbody == null) ? Mathf.Abs(this.RespawnScaleUpDuration) : 0f);
		if (this.BobOffset == -1f)
		{
			this.BobOffset = Random.value;
		}
		if (this.RecipientTags.Count == 0)
		{
			this.RecipientTags.Add("Player");
		}
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00078254 File Offset: 0x00076454
	protected virtual void Update()
	{
		this.UpdateMotion();
		if (this.m_Depleted && !this.m_Audio.isPlaying)
		{
			this.Remove();
		}
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x0007A45C File Offset: 0x0007865C
	protected virtual void UpdateMotion()
	{
		if (this.m_Rigidbody != null)
		{
			return;
		}
		if (this.Billboard)
		{
			if (this.m_CameraMainTransform != null)
			{
				this.m_Transform.localEulerAngles = this.m_CameraMainTransform.eulerAngles;
			}
		}
		else
		{
			this.m_Transform.localEulerAngles += this.Spin * Time.deltaTime;
		}
		if (this.BobRate != 0f && this.BobAmp != 0f)
		{
			this.m_Transform.position = this.m_SpawnPosition + Vector3.up * (Mathf.Cos((Time.time + this.BobOffset) * (this.BobRate * 10f)) * this.BobAmp);
		}
		if (this.m_Transform.localScale != this.m_SpawnScale)
		{
			this.m_Transform.localScale = Vector3.Lerp(this.m_Transform.localScale, this.m_SpawnScale, Time.deltaTime / this.RespawnScaleUpDuration);
		}
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x0007A570 File Offset: 0x00078770
	protected virtual void OnTriggerEnter(Collider col)
	{
		if (this.m_Depleted)
		{
			return;
		}
		foreach (string b in this.RecipientTags)
		{
			if (col.gameObject.tag == b)
			{
				goto IL_4E;
			}
		}
		return;
		IL_4E:
		if (col != this.m_LastCollider)
		{
			this.m_Recipient = col.gameObject.GetComponent<vp_FPPlayerEventHandler>();
		}
		if (this.m_Recipient == null)
		{
			return;
		}
		if (this.TryGive(this.m_Recipient))
		{
			this.m_Audio.pitch = (this.PickupSoundSlomo ? Time.timeScale : 1f);
			this.m_Audio.Play();
			base.GetComponent<Renderer>().enabled = false;
			this.m_Depleted = true;
			this.m_Recipient.HUDText.Send(this.GiveMessage);
			return;
		}
		if (!this.m_AlreadyFailed)
		{
			this.m_Audio.pitch = (this.FailSoundSlomo ? Time.timeScale : 1f);
			this.m_Audio.PlayOneShot(this.PickupFailSound);
			this.m_AlreadyFailed = true;
			this.m_Recipient.HUDText.Send(this.FailMessage);
		}
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x0007A6CC File Offset: 0x000788CC
	protected virtual void OnTriggerExit(Collider col)
	{
		this.m_AlreadyFailed = false;
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x0007A6D5 File Offset: 0x000788D5
	protected virtual bool TryGive(vp_FPPlayerEventHandler player)
	{
		return player.AddItem.Try(new object[]
		{
			this.InventoryName,
			1
		});
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x0007A704 File Offset: 0x00078904
	protected virtual void Remove()
	{
		if (this.RespawnDuration == 0f)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (!this.m_RespawnTimer.Active)
		{
			vp_Utility.Activate(base.gameObject, false);
			vp_Timer.In(this.RespawnDuration, new vp_Timer.Callback(this.Respawn), this.m_RespawnTimer);
		}
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x0007A764 File Offset: 0x00078964
	protected virtual void Respawn()
	{
		if (Camera.main != null)
		{
			this.m_CameraMainTransform = Camera.main.transform;
		}
		this.m_RespawnTimer.Cancel();
		this.m_Transform.position = this.m_SpawnPosition;
		if (this.m_Rigidbody == null && this.RespawnScaleUpDuration > 0f)
		{
			this.m_Transform.localScale = Vector3.zero;
		}
		base.GetComponent<Renderer>().enabled = true;
		vp_Utility.Activate(base.gameObject, true);
		this.m_Audio.pitch = (this.RespawnSoundSlomo ? Time.timeScale : 1f);
		this.m_Audio.PlayOneShot(this.RespawnSound);
		this.m_Depleted = false;
		if (this.BobOffset == -1f)
		{
			this.BobOffset = Random.value;
		}
	}

	// Token: 0x04000EC1 RID: 3777
	protected Transform m_Transform;

	// Token: 0x04000EC2 RID: 3778
	protected Rigidbody m_Rigidbody;

	// Token: 0x04000EC3 RID: 3779
	protected AudioSource m_Audio;

	// Token: 0x04000EC4 RID: 3780
	public string InventoryName = "Unnamed";

	// Token: 0x04000EC5 RID: 3781
	public List<string> RecipientTags = new List<string>();

	// Token: 0x04000EC6 RID: 3782
	private Collider m_LastCollider;

	// Token: 0x04000EC7 RID: 3783
	private vp_FPPlayerEventHandler m_Recipient;

	// Token: 0x04000EC8 RID: 3784
	public string GiveMessage = "Picked up an item";

	// Token: 0x04000EC9 RID: 3785
	public string FailMessage = "You currently can't pick up this item!";

	// Token: 0x04000ECA RID: 3786
	protected Vector3 m_SpawnPosition = Vector3.zero;

	// Token: 0x04000ECB RID: 3787
	protected Vector3 m_SpawnScale = Vector3.zero;

	// Token: 0x04000ECC RID: 3788
	public bool Billboard;

	// Token: 0x04000ECD RID: 3789
	public Vector3 Spin = Vector3.zero;

	// Token: 0x04000ECE RID: 3790
	public float BobAmp;

	// Token: 0x04000ECF RID: 3791
	public float BobRate;

	// Token: 0x04000ED0 RID: 3792
	public float BobOffset = -1f;

	// Token: 0x04000ED1 RID: 3793
	public float RespawnDuration = 10f;

	// Token: 0x04000ED2 RID: 3794
	public float RespawnScaleUpDuration;

	// Token: 0x04000ED3 RID: 3795
	public AudioClip PickupSound;

	// Token: 0x04000ED4 RID: 3796
	public AudioClip PickupFailSound;

	// Token: 0x04000ED5 RID: 3797
	public AudioClip RespawnSound;

	// Token: 0x04000ED6 RID: 3798
	public bool PickupSoundSlomo = true;

	// Token: 0x04000ED7 RID: 3799
	public bool FailSoundSlomo = true;

	// Token: 0x04000ED8 RID: 3800
	public bool RespawnSoundSlomo = true;

	// Token: 0x04000ED9 RID: 3801
	protected bool m_Depleted;

	// Token: 0x04000EDA RID: 3802
	protected bool m_AlreadyFailed;

	// Token: 0x04000EDB RID: 3803
	protected vp_Timer.Handle m_RespawnTimer = new vp_Timer.Handle();

	// Token: 0x04000EDC RID: 3804
	private Transform m_CameraMainTransform;
}
