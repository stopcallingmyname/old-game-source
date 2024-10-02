using System;
using UnityEngine;

// Token: 0x0200012E RID: 302
public class PlayerControl : MonoBehaviour
{
	// Token: 0x06000A72 RID: 2674 RVA: 0x000857BC File Offset: 0x000839BC
	private void Awake()
	{
		this.Map = GameObject.Find("Map");
		this.Gui = GameObject.Find("GUI");
		this.SetSpectator();
		this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		this.DeathSound = ContentLoader.LoadSound("death");
		this.HitSound = ContentLoader.LoadSound("hit");
		this.TraceHitSound = ContentLoader.LoadSound("tracehit");
		this.Headshot = (Resources.Load("Sound/Award/headshot") as AudioClip);
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x0008584E File Offset: 0x00083A4E
	private void Update()
	{
		if (this.freeze)
		{
			base.gameObject.transform.position = new Vector3(128f, 32f, 128f);
		}
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x0008587C File Offset: 0x00083A7C
	public void SetSpectator()
	{
		this.freeze = true;
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x00085885 File Offset: 0x00083A85
	public void UnSetSpectator()
	{
		this.freeze = false;
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0008588E File Offset: 0x00083A8E
	public bool isSpectator()
	{
		return this.freeze;
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x00085896 File Offset: 0x00083A96
	public void Spawn(int x, int y, int z)
	{
		base.gameObject.transform.position = new Vector3((float)x, (float)y, (float)z);
		this.UnSetSpectator();
		this.Gui.GetComponent<MainGUI>().CloseAll();
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x000858C9 File Offset: 0x00083AC9
	public int GetTeam()
	{
		return (int)RemotePlayersUpdater.Instance.Bots[this.cscl.myindex].Team;
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x000858E6 File Offset: 0x00083AE6
	public void StartMap(string mapname)
	{
		MonoBehaviour.print("STARTMAP: " + mapname);
		PlayerControl.mapid = mapname;
		this.Map.GetComponent<ZipLoader>().WebLoadMap(mapname);
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0008590F File Offset: 0x00083B0F
	public void SetHit()
	{
		base.GetComponent<WeaponSystem>().GetComponent<AudioSource>().PlayOneShot(this.HitSound, AudioListener.volume);
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x0008592C File Offset: 0x00083B2C
	public void SetTraceHit()
	{
		base.GetComponent<WeaponSystem>().GetComponent<AudioSource>().PlayOneShot(this.TraceHitSound, AudioListener.volume);
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x00085949 File Offset: 0x00083B49
	public static string GetMapID()
	{
		return PlayerControl.mapid;
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x00085950 File Offset: 0x00083B50
	public void SetSky(int _value, bool _prazd = false)
	{
		if (_value == 3 || _value == 7 || _value == 10)
		{
			this.Map.GetComponent<Sky>().SetSky(1, _prazd);
			return;
		}
		this.Map.GetComponent<Sky>().SetSky(0, _prazd);
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x00085984 File Offset: 0x00083B84
	public void AwardHeadshot()
	{
		base.GetComponent<WeaponSystem>().GetComponent<AudioSource>().PlayOneShot(this.Headshot, AudioListener.volume);
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x000859A1 File Offset: 0x00083BA1
	public void SetPrivateServer(int flag)
	{
		PlayerControl.privateserver = 1;
		PlayerControl.privateadmin = flag;
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x000859AF File Offset: 0x00083BAF
	public int isPrivateServer()
	{
		return PlayerControl.privateserver;
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x000859B6 File Offset: 0x00083BB6
	public static int isPrivateAdmin()
	{
		return PlayerControl.privateadmin;
	}

	// Token: 0x04001086 RID: 4230
	private bool freeze;

	// Token: 0x04001087 RID: 4231
	private Client cscl;

	// Token: 0x04001088 RID: 4232
	private GameObject Gui;

	// Token: 0x04001089 RID: 4233
	private GameObject MainCamera;

	// Token: 0x0400108A RID: 4234
	private GameObject WeaponCamera;

	// Token: 0x0400108B RID: 4235
	private GameObject Map;

	// Token: 0x0400108C RID: 4236
	private static string mapid;

	// Token: 0x0400108D RID: 4237
	private static int privateserver;

	// Token: 0x0400108E RID: 4238
	private static int privateadmin;

	// Token: 0x0400108F RID: 4239
	private AudioClip DeathSound;

	// Token: 0x04001090 RID: 4240
	private AudioClip HitSound;

	// Token: 0x04001091 RID: 4241
	private AudioClip TraceHitSound;

	// Token: 0x04001092 RID: 4242
	private AudioClip Headshot;

	// Token: 0x04001093 RID: 4243
	private GameObject FollowCam;
}
