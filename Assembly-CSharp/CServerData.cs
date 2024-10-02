using System;

// Token: 0x02000020 RID: 32
public class CServerData
{
	// Token: 0x060000EF RID: 239 RVA: 0x00011D38 File Offset: 0x0000FF38
	public CServerData(int _type, int _gamemode, int _players, int _maxplayers, string _map, ulong _adminid, string _ip, int _port, int _password, int _country_id, int _lvl)
	{
		this.type = _type;
		this.gamemode = _gamemode;
		this.players = _players;
		this.maxplayers = _maxplayers;
		this.adminid = _adminid;
		this.ip = _ip;
		this.port = _port;
		this.password = _password;
		this.hover = false;
		this.country_id = _country_id;
		this.lvl = _lvl;
		if (((PlayerProfile.level < 6 && this.lvl > 5) || (PlayerProfile.level > 5 && this.lvl < 6)) && this.gamemode != 1 && this.gamemode != 8)
		{
			this.avaliable_by_lvl = false;
		}
		else
		{
			this.avaliable_by_lvl = true;
		}
		if (this.type > 0)
		{
			this.name = _map;
			return;
		}
		int.TryParse(_map, out this.map_id);
		this.SetMapNameAndSize();
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00011E0C File Offset: 0x0001000C
	~CServerData()
	{
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00011E34 File Offset: 0x00010034
	private void SetMapNameAndSize()
	{
		this.name = Lang.GetMapName(this.map_id);
		if (this.gamemode == 7 || this.gamemode == 9 || this.gamemode == 10)
		{
			if (this.maxplayers >= 64)
			{
				this.maxplayers -= 64;
				this.name += Lang.GetLabel(827);
			}
			else
			{
				this.name += Lang.GetLabel(828);
			}
		}
		int num = this.gamemode * 1000;
		if (num > 9000)
		{
			num /= 10;
		}
		this.name = string.Concat(new string[]
		{
			Lang.GetLabel(830 + this.gamemode),
			"#",
			(this.port - CONST.CFG.GAME_PORTS_OFFSET[(int)PlayerProfile.network] - num).ToString(),
			" - ",
			this.name
		});
		if (this.map_id == 0)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 1)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 2)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 3)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 4)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 5)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 6)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 7)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 8)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 9)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 10)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 11)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 12)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 13)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 14)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 15)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 16)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 17)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 18)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 19)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 20)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 21)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 22)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 23)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 24)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 25)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 26)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 27)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 28)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 29)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 30)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 31)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 32)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 33)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 34)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 35)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 36)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 37)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 38)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 39)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 401)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 402)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 403)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 404)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 405)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 406)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 407)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 408)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 501)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 502)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 503)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 504)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 505)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 506)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 507)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 508)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 509)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 510)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 511)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 512)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 513)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 514)
		{
			this.map_size = 0;
			return;
		}
		if (this.map_id == 515)
		{
			this.map_size = 0;
			return;
		}
		if (this.map_id == 516)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 517)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 518)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 519)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 520)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 521)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 522)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 523)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 524)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 525)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 526)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 527)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 528)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 529)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 530)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 531)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 532)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 533)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 534)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 535)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 536)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 537)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 538)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 539)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 540)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 541)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 542)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 543)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 544)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 545)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 546)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 547)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 548)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 549)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 550)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 551)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 552)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 601)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 602)
		{
			this.map_size = 0;
			return;
		}
		if (this.map_id == 603)
		{
			this.map_size = 0;
			return;
		}
		if (this.map_id == 604)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 605)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 606)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 607)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 608)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 609)
		{
			this.map_size = 0;
			return;
		}
		if (this.map_id == 610)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 611)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 612)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 613)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 614)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 615)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 616)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 617)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 618)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 619)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 620)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 621)
		{
			this.map_size = 0;
			return;
		}
		if (this.map_id == 622)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 623)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 624)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 625)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 626)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 901)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 701)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 702)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 703)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 704)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 705)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 706)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 707)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 708)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 709)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 710)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 711)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 712)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 1101)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 1102)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 1103)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 1104)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 1105)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 1106)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 301)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 302)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 303)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 304)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 305)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 306)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 307)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 308)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 309)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 310)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 311)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 312)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 313)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 314)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 315)
		{
			this.map_size = 1;
			return;
		}
		if (this.map_id == 316)
		{
			this.map_size = 2;
			return;
		}
		if (this.map_id == 317)
		{
			this.map_size = 1;
		}
	}

	// Token: 0x04000100 RID: 256
	public int type;

	// Token: 0x04000101 RID: 257
	public int gamemode;

	// Token: 0x04000102 RID: 258
	public int players;

	// Token: 0x04000103 RID: 259
	public int maxplayers;

	// Token: 0x04000104 RID: 260
	public int map_id;

	// Token: 0x04000105 RID: 261
	public string name;

	// Token: 0x04000106 RID: 262
	public ulong adminid;

	// Token: 0x04000107 RID: 263
	public string ip;

	// Token: 0x04000108 RID: 264
	public int port;

	// Token: 0x04000109 RID: 265
	public int password;

	// Token: 0x0400010A RID: 266
	public bool hover;

	// Token: 0x0400010B RID: 267
	public int country_id;

	// Token: 0x0400010C RID: 268
	public int map_size;

	// Token: 0x0400010D RID: 269
	public int lvl;

	// Token: 0x0400010E RID: 270
	public bool avaliable_by_lvl;
}
