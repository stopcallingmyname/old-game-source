using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000D4 RID: 212
public class vp_FPInput : MonoBehaviour
{
	// Token: 0x1700003E RID: 62
	// (get) Token: 0x0600076C RID: 1900 RVA: 0x00072A51 File Offset: 0x00070C51
	// (set) Token: 0x0600076D RID: 1901 RVA: 0x00072A59 File Offset: 0x00070C59
	public bool AllowGameplayInput
	{
		get
		{
			return this.m_AllowGameplayInput;
		}
		set
		{
			this.m_AllowGameplayInput = value;
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x0600076E RID: 1902 RVA: 0x00072A62 File Offset: 0x00070C62
	public Vector2 MousePos
	{
		get
		{
			return this.m_MousePos;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x0600076F RID: 1903 RVA: 0x00072A6A File Offset: 0x00070C6A
	// (set) Token: 0x06000770 RID: 1904 RVA: 0x00072A77 File Offset: 0x00070C77
	public int MouseSmoothSteps
	{
		get
		{
			return this.m_FPCamera.MouseSmoothSteps;
		}
		set
		{
			this.m_FPCamera.MouseSmoothSteps = (int)Mathf.Clamp((float)value, 1f, 10f);
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000771 RID: 1905 RVA: 0x00072A96 File Offset: 0x00070C96
	// (set) Token: 0x06000772 RID: 1906 RVA: 0x00072AA3 File Offset: 0x00070CA3
	public float MouseSmoothWeight
	{
		get
		{
			return this.m_FPCamera.MouseSmoothWeight;
		}
		set
		{
			this.m_FPCamera.MouseSmoothWeight = Mathf.Clamp01(value);
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000773 RID: 1907 RVA: 0x00072AB6 File Offset: 0x00070CB6
	// (set) Token: 0x06000774 RID: 1908 RVA: 0x00072AC3 File Offset: 0x00070CC3
	public bool MouseAcceleration
	{
		get
		{
			return this.m_FPCamera.MouseAcceleration;
		}
		set
		{
			this.m_FPCamera.MouseAcceleration = value;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000775 RID: 1909 RVA: 0x00072AD1 File Offset: 0x00070CD1
	// (set) Token: 0x06000776 RID: 1910 RVA: 0x00072ADE File Offset: 0x00070CDE
	public float MouseAccelerationThreshold
	{
		get
		{
			return this.m_FPCamera.MouseAccelerationThreshold;
		}
		set
		{
			this.m_FPCamera.MouseAccelerationThreshold = value;
		}
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x0000248C File Offset: 0x0000068C
	private void Start()
	{
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x00072AEC File Offset: 0x00070CEC
	private int GetWeaponID(string name)
	{
		for (int i = 0; i < 500; i++)
		{
			ITEM item = (ITEM)i;
			if (item.ToString() == name)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x00072B23 File Offset: 0x00070D23
	protected virtual void Update()
	{
		this.InputMove();
		this.InputWalk();
		this.InputJump();
		this.InputCrouch();
		this.InputAttack();
		this.InputZoom();
		this.InputReload();
		this.InputSetWeapon();
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x00072B58 File Offset: 0x00070D58
	protected virtual void InputMove()
	{
		if (Time.time < this.stopmove)
		{
			this.Player.InputMoveVector.Set(Vector2.zero);
			return;
		}
		this.Player.InputMoveVector.Set(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00072BBB File Offset: 0x00070DBB
	protected virtual void InputWalk()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			this.Player.Walk.TryStart(true);
			return;
		}
		this.Player.Walk.TryStop(true);
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x00072BEE File Offset: 0x00070DEE
	protected virtual void InputJump()
	{
		if (Input.GetButton("Jump"))
		{
			this.Player.Jump.TryStart(true);
			return;
		}
		this.Player.Jump.Stop(0f);
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x00072C24 File Offset: 0x00070E24
	protected virtual void InputCrouch()
	{
		if (Input.GetKey(KeyCode.LeftControl))
		{
			this.Player.Crouch.TryStart(true);
			return;
		}
		this.Player.Crouch.TryStop(true);
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x00072C58 File Offset: 0x00070E58
	protected virtual void InputZoom()
	{
		if (CONST.GetGameMode() == MODE.SNOWBALLS)
		{
			return;
		}
		if (this.MouseBlockZoom > Time.time)
		{
			return;
		}
		if (this.Player.Reload.Active)
		{
			return;
		}
		if (this.GetWeaponID(this.Player.CurrentWeaponName.Get()) == 315)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			if (this.swid == 172)
			{
				this.m_WeaponSystem.DetonateMyC4();
			}
			if (this.m_FPCamera.GetComponent<Camera>().fieldOfView > 64f)
			{
				this.Player.Zoom.TryStart(true);
				return;
			}
			this.Player.Zoom.TryStop(true);
		}
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x00072D14 File Offset: 0x00070F14
	protected virtual void InputAttack()
	{
		if (Cursor.lockState.Equals(CursorLockMode.None))
		{
			return;
		}
		if (Cursor.visible)
		{
			Cursor.visible = false;
		}
		if (this.Player.Reload.Active)
		{
			return;
		}
		if (Input.GetKey(KeyCode.Mouse0))
		{
			this.Player.Attack.TryStart(true);
			if (this.GetWeaponID(this.Player.CurrentWeaponName.Get()) == 161)
			{
				base.StartCoroutine(this.AutoReload());
				return;
			}
		}
		else
		{
			this.Player.Attack.TryStop(true);
		}
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x00072DBD File Offset: 0x00070FBD
	private IEnumerator AutoReload()
	{
		yield return new WaitForSeconds(0.5f);
		this.Player.Attack.TryStop(true);
		this.Player.Reload.TryStart(true);
		yield break;
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x00072DCC File Offset: 0x00070FCC
	protected virtual void InputReload()
	{
		if (CONST.GetGameMode() == MODE.SNOWBALLS)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.R) && this.m_WeaponSystem.WeaponCanReload(this.GetWeaponID(this.Player.CurrentWeaponName.Get())))
		{
			this.Player.Attack.TryStop(true);
			this.Player.Reload.TryStart(true);
		}
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x00072E38 File Offset: 0x00071038
	protected virtual void InputSetWeapon()
	{
		if (this.zombie)
		{
			return;
		}
		if (CONST.GetGameMode() == MODE.SNOWBALLS)
		{
			return;
		}
		if (this.HideWeapons)
		{
			return;
		}
		if (this.Player.Reload.Active)
		{
			return;
		}
		if (this.m_WeaponSystem == null)
		{
			this.m_WeaponSystem = (WeaponSystem)Object.FindObjectOfType(typeof(WeaponSystem));
		}
		bool flag = false;
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.cwid = 0;
			this.swid = 0;
			this.last_swid = 0;
			flag = true;
			this.lastweapon2 = 0;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			this.cwid = 1;
			this.swid = 0;
			this.last_swid = 0;
			flag = true;
			this.lastweapon2 = 0;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			this.cwid = 2;
			this.swid = 0;
			this.last_swid = 0;
			flag = true;
			this.lastweapon2 = 0;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			this.cwid = 3;
			this.swid = 0;
			this.last_swid = 0;
			flag = true;
			this.lastweapon2 = 0;
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			int num;
			if (this.last_swid == 0)
			{
				num = 0;
			}
			else
			{
				num = this.lastweapon2 + 1;
			}
			if (num > 2)
			{
				num = 0;
			}
			int num2 = num;
			while (this.m_WeaponSystem.GetAmmo(num) < 1 && this.m_WeaponSystem.GetAmmoWid(num) != 172)
			{
				num++;
				if (num > 2)
				{
					num = 0;
				}
				if (num == num2)
				{
					break;
				}
			}
			this.swid = (byte)this.m_WeaponSystem.GetAmmoWid(num);
			this.last_swid = (int)this.swid;
			this.lastweapon2 = num;
			flag = true;
		}
		if (Input.GetKeyDown(KeyCode.G) && this.m_WeaponSystem.GetGAmmo() > 0)
		{
			this.Player.Attack.TryStop(true);
			this.swid = (byte)this.m_WeaponSystem.g1wid;
			flag = true;
			this.lastweapon2 = 0;
		}
		if (Input.GetKeyDown(KeyCode.H) && this.m_WeaponSystem.GetHAmmo() > 0)
		{
			this.Player.Attack.TryStop(true);
			this.swid = (byte)this.m_WeaponSystem.g2wid;
			flag = true;
			this.lastweapon2 = 0;
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			if (this.m_WeaponSystem.GetAmmoMedkit_o() > 0)
			{
				this.swid = 38;
			}
			else if (this.m_WeaponSystem.GetAmmoMedkit_g() > 0)
			{
				this.swid = 37;
			}
			else if (this.m_WeaponSystem.GetAmmoMedkit_w() > 0)
			{
				this.swid = 36;
			}
			E_Menu e_Menu = (E_Menu)Object.FindObjectOfType(typeof(E_Menu));
			if (e_Menu)
			{
				e_Menu.SelectedItem[3][0] = (int)this.swid;
			}
			flag = true;
			this.lastweapon2 = 0;
		}
		if (!flag)
		{
			return;
		}
		if (this.swid > 0)
		{
			vp_Attempt<string>.Tryer<string> @try = this.Player.SetWeaponByName.Try;
			ITEM item = (ITEM)this.swid;
			@try(item.ToString());
		}
		else if (this.cwid > 0 && this.awid[(int)this.cwid] > 0)
		{
			vp_Attempt<string>.Tryer<string> try2 = this.Player.SetWeaponByName.Try;
			ITEM item = (ITEM)this.awid[(int)this.cwid];
			try2(item.ToString());
		}
		else if (this.cwid == 0)
		{
			this.Player.SetWeaponByName.Try("BLOCK");
		}
		base.GetComponent<Sound>().PlaySound_Stop(GameObject.Find("Player/MainCamera/TMPAudio").GetComponent<AudioSource>());
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x00073194 File Offset: 0x00071394
	public void RestoreSetWeapon()
	{
		this.swid = 0;
		if (this.cwid > 0 && this.awid[(int)this.cwid] > 0)
		{
			vp_Attempt<string>.Tryer<string> @try = this.Player.SetWeaponByName.Try;
			ITEM item = (ITEM)this.awid[(int)this.cwid];
			@try(item.ToString());
			return;
		}
		if (this.cwid == 0)
		{
			this.Player.SetWeaponByName.Try("BLOCK");
		}
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x00073218 File Offset: 0x00071418
	public void SetPrimaryWeapon(bool build_mode_and_block = false)
	{
		this.zombie = false;
		if (this.awid[2] > 0)
		{
			this.cwid = 2;
		}
		else if (this.awid[3] > 0)
		{
			this.cwid = 3;
		}
		else if (this.awid[1] > 0)
		{
			this.cwid = 1;
		}
		else
		{
			this.cwid = 0;
		}
		if (build_mode_and_block)
		{
			this.cwid = 0;
		}
		this.swid = 0;
		vp_Attempt<string>.Tryer<string> @try = this.Player.SetWeaponByName.Try;
		ITEM item = (ITEM)this.awid[(int)this.cwid];
		@try(item.ToString());
		this.HideWeapons = false;
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x000732B8 File Offset: 0x000714B8
	public void SetZombieWeapon()
	{
		this.zombie = true;
		this.Player.SetWeaponByName.Try("ZOMBIE");
		base.StartCoroutine(this.cr_SetZombieWeapon());
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x000732E9 File Offset: 0x000714E9
	private IEnumerator cr_SetZombieWeapon()
	{
		yield return new WaitForSeconds(1f);
		if (this.zombie && this.Player.CurrentWeaponName.Get()[0] != '8')
		{
			this.SetZombieWeapon();
			base.StartCoroutine(this.cr_SetZombieWeapon());
		}
		yield break;
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x0000248C File Offset: 0x0000068C
	protected virtual void UpdatePause()
	{
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x0000248C File Offset: 0x0000068C
	protected virtual void UpdateCursorLock()
	{
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x000732F8 File Offset: 0x000714F8
	protected virtual void Awake()
	{
		this.Player = (vp_FPPlayerEventHandler)base.transform.root.GetComponentInChildren(typeof(vp_FPPlayerEventHandler));
		this.m_FPCamera = base.GetComponentInChildren<vp_FPCamera>();
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x0007332B File Offset: 0x0007152B
	protected virtual void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x00073347 File Offset: 0x00071547
	protected virtual void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x0600078C RID: 1932 RVA: 0x00072A51 File Offset: 0x00070C51
	// (set) Token: 0x0600078D RID: 1933 RVA: 0x00072A59 File Offset: 0x00070C59
	protected virtual bool OnValue_AllowGameplayInput
	{
		get
		{
			return this.m_AllowGameplayInput;
		}
		set
		{
			this.m_AllowGameplayInput = value;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x0600078E RID: 1934 RVA: 0x00073363 File Offset: 0x00071563
	// (set) Token: 0x0600078F RID: 1935 RVA: 0x0007336A File Offset: 0x0007156A
	protected virtual bool OnValue_Pause
	{
		get
		{
			return vp_TimeUtility.Paused;
		}
		set
		{
			vp_TimeUtility.Paused = value;
		}
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x00073372 File Offset: 0x00071572
	public void SetHideWeapons(bool val)
	{
		this.HideWeapons = val;
		if (this.HideWeapons)
		{
			this.Player.SetWeapon.TryStart<int>(0);
		}
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x00073395 File Offset: 0x00071595
	public void SetActiveWeapons(int mwid, int pwid, int swid)
	{
		this.awid[0] = 0;
		this.awid[1] = mwid;
		this.awid[2] = pwid;
		this.awid[3] = swid;
		if (pwid > 0)
		{
			this.cwid = 2;
		}
	}

	// Token: 0x04000D66 RID: 3430
	public vp_FPPlayerEventHandler Player;

	// Token: 0x04000D67 RID: 3431
	protected vp_FPCamera m_FPCamera;

	// Token: 0x04000D68 RID: 3432
	protected WeaponSystem m_WeaponSystem;

	// Token: 0x04000D69 RID: 3433
	public Rect[] MouseCursorZones;

	// Token: 0x04000D6A RID: 3434
	protected Vector2 m_MousePos = Vector2.zero;

	// Token: 0x04000D6B RID: 3435
	public float MouseBlockZoom;

	// Token: 0x04000D6C RID: 3436
	public int[] awid = new int[4];

	// Token: 0x04000D6D RID: 3437
	public byte cwid;

	// Token: 0x04000D6E RID: 3438
	public byte swid;

	// Token: 0x04000D6F RID: 3439
	public bool zombie;

	// Token: 0x04000D70 RID: 3440
	public byte lastcwid;

	// Token: 0x04000D71 RID: 3441
	public float stopmove;

	// Token: 0x04000D72 RID: 3442
	protected bool m_AllowGameplayInput = true;

	// Token: 0x04000D73 RID: 3443
	private bool HideWeapons;

	// Token: 0x04000D74 RID: 3444
	public int lastweapon2;

	// Token: 0x04000D75 RID: 3445
	public int last_swid;
}
