using System;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class LiftMeUp : MonoBehaviour
{
	// Token: 0x06000456 RID: 1110 RVA: 0x00054E17 File Offset: 0x00053017
	private void Awake()
	{
		this.lift = false;
		this.lift_sound = Resources.Load<AudioClip>("Sound/elevator_sound_full");
		this.lift_wait = Resources.Load<AudioClip>("Sound/wait_music");
		this.audio_lift = base.gameObject.AddComponent<AudioSource>();
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00054E54 File Offset: 0x00053054
	public void PlaySound()
	{
		if (this.audio_lift.isPlaying)
		{
			this.BreakSound();
		}
		this.timer = Time.time + 10f;
		this.audio_lift.clip = this.lift_sound;
		this.audio_lift.Play();
		this.lift = true;
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x00054EA8 File Offset: 0x000530A8
	public void PlayWait()
	{
		if (this.audio_lift.isPlaying)
		{
			return;
		}
		this.timer = Time.time + 90f;
		this.audio_lift.clip = this.lift_wait;
		this.audio_lift.Play();
		this.lift = true;
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x00054EF7 File Offset: 0x000530F7
	private void Update()
	{
		if (this.lift)
		{
			if (this.timer < Time.time)
			{
				this.BreakSound();
				return;
			}
		}
		else if (this.audio_lift.isPlaying)
		{
			this.BreakSound();
		}
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x00054F28 File Offset: 0x00053128
	public void BreakSound()
	{
		this.audio_lift.Stop();
		this.lift = false;
	}

	// Token: 0x0400092E RID: 2350
	private float timer;

	// Token: 0x0400092F RID: 2351
	private bool lift;

	// Token: 0x04000930 RID: 2352
	private AudioSource audio_lift;

	// Token: 0x04000931 RID: 2353
	private AudioClip lift_sound;

	// Token: 0x04000932 RID: 2354
	private AudioClip lift_wait;
}
