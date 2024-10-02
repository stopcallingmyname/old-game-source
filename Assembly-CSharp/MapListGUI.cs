using System;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class MapListGUI : MonoBehaviour
{
	// Token: 0x060003B0 RID: 944 RVA: 0x000463B8 File Offset: 0x000445B8
	private void Awake()
	{
		this.r_window = new Rect(0f, 0f, 600f, 400f);
		this.r_window.center = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x0004640A File Offset: 0x0004460A
	public void SetActive(bool val, int gamemode)
	{
		this.active = val;
		MainGUI.ForceCursor = this.active;
		this.gamemode = gamemode;
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x00046425 File Offset: 0x00044625
	private void OnGUI()
	{
		if (!this.active)
		{
			return;
		}
		GUI.Window(0, this.r_window, new GUI.WindowFunction(this.DrawWindow), "", GUIManager.gs_empty);
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x00046454 File Offset: 0x00044654
	private void DrawWindow(int wid)
	{
		Vector2 vector = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		vector.x -= this.r_window.x;
		vector.y -= this.r_window.y;
		GUI.color = new Color(1f, 1f, 1f, 0.8f);
		GUI.DrawTexture(new Rect(0f, 0f, 600f, 32f), GUIManager.tex_black);
		GUI.color = new Color(1f, 1f, 1f, 1f);
		if (this.gamemode == 0)
		{
			GUI.Label(new Rect(0f, 0f, 600f, 32f), Lang.GetLabel(505), GUIManager.gs_style1);
		}
		else if (this.gamemode == 5)
		{
			GUI.Label(new Rect(0f, 0f, 600f, 32f), Lang.GetLabel(506), GUIManager.gs_style1);
		}
		GUI.color = new Color(1f, 1f, 1f, 0.6f);
		GUI.DrawTexture(new Rect(0f, 34f, 600f, 366f), GUIManager.tex_black);
		GUI.color = new Color(1f, 1f, 1f, 1f);
		int x = 172;
		int num = 16;
		if (this.gamemode == 0)
		{
			this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, 34f, 600f, 320f), this.scrollViewVector, new Rect(0f, 0f, 0f, 2100f));
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 1, (Lang.current == 0) ? "ЦИТАДЕЛЬ" : "OLD CITADEL");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 2, (Lang.current == 0) ? "ЗАМОК" : "CASTLE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 3, (Lang.current == 0) ? "ФОРПОСТ 2" : "FORTPOST 2");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 4, (Lang.current == 0) ? "ХОЛМЫ" : "HILLS");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 5, (Lang.current == 0) ? "ДЕРЕВНЯ 2" : "VILLAGE 2");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 6, (Lang.current == 0) ? "ОСТРОВА" : "ISLAND");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 7, (Lang.current == 0) ? "СОБОР 2" : "CATHEDRAL 2");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 8, (Lang.current == 0) ? "ГОРОД" : "TOWN");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 9, (Lang.current == 0) ? "ТЕХНО" : "TECHNO");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 10, (Lang.current == 0) ? "ЭМИДРИС" : "EMIDRIS");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 11, (Lang.current == 0) ? "СЛИЗНЯК" : "SLIME");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 12, (Lang.current == 0) ? "СТОЛИЦА" : "CAPITAL");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 13, (Lang.current == 0) ? "БРЕСТСКАЯ КРЕПОСТЬ" : "BREST FORTRESS");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 14, (Lang.current == 0) ? "ВОДОВОРОТ" : "SWIRL");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 15, (Lang.current == 0) ? "ПРИПЯТЬ" : "PRIPYAT");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 16, (Lang.current == 0) ? "ПРИПЯТЬ 2" : "PRIPYAT 2");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 17, (Lang.current == 0) ? "КЛАЗМОС" : "KLAZMOS");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 18, (Lang.current == 0) ? "ВОСТОК" : "EAST");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 19, (Lang.current == 0) ? "ЭПИК" : "EPIC");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 20, (Lang.current == 0) ? "БАШНИ" : "TOWERS");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 21, (Lang.current == 0) ? "ПАРК" : "PARK");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 22, (Lang.current == 0) ? "ЗИМНИЙ САД" : "WINTER GARDEN");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 23, (Lang.current == 0) ? "ОБЩЕЖИТИЕ" : "HOSTEL");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 24, (Lang.current == 0) ? "ХРАМ" : "TEMPLE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 25, (Lang.current == 0) ? "МЯСОРУБКА" : "HASHER");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 26, (Lang.current == 0) ? "ПОРТ" : "HARBOR");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 27, (Lang.current == 0) ? "ДЖУНГЛИ" : "JUNGLE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 28, (Lang.current == 0) ? "СЕКРЕТНАЯ БАЗА" : "SECRET BASE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 29, (Lang.current == 0) ? "ЛЕСНОЕ ОЗЕРО" : "FOREST LAKE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 30, (Lang.current == 0) ? "ВЫШКА" : "TOWER");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 31, (Lang.current == 0) ? "ЯПОНИЯ" : "JAPAN");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 32, (Lang.current == 0) ? "ПОДЗЕМКА" : "UNDERGROUND");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 33, (Lang.current == 0) ? "УБЕЖИЩЕ" : "ASYLUM");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 34, (Lang.current == 0) ? "ЗВЕЗДА" : "STAR");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 35, (Lang.current == 0) ? "ТУРНИРНАЯ БИТВА" : "TOURNAMENT BATTLE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 36, (Lang.current == 0) ? "СИТИ-17" : "CITY-17");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 37, (Lang.current == 0) ? "ОСАДА" : "SIEGE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 38, (Lang.current == 0) ? "СИБИРЬ" : "SIBERIA");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 39, (Lang.current == 0) ? "МЕРТВЫЙ ГОРОД" : "DEAD CITY");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 40, (Lang.current == 0) ? "ЗИМНИЙ ЗАМОК" : "WINTER CASTLE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 41, (Lang.current == 0) ? "ОСТРОВА 2017" : "ISLANDS 2017");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 42, (Lang.current == 0) ? "ДЕРЕВНЯ" : "VILLAGE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 43, (Lang.current == 0) ? "СОБОР" : "CATHEDRAL");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 44, (Lang.current == 0) ? "ЗИМНИЙ ЗАМОК" : "WINTER CASTLE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 45, (Lang.current == 0) ? "ПУЗЫРИ" : "BUBBLES");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 46, (Lang.current == 0) ? "СЕЛО" : "HAMLET");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 47, (Lang.current == 0) ? "ЛЕСНАЯ КРЕПОСТЬ" : "FOREST CASTLE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 48, (Lang.current == 0) ? "НОВЫЙ ГОРОД" : "NEW CITY");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 49, (Lang.current == 0) ? "АЦТЕК" : "AZTEC");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 50, (Lang.current == 0) ? "ФОРПОСТ" : "FORTPOST");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 51, (Lang.current == 0) ? "ЗИМНЯЯ ЦИТАДЕЛЬ" : "WINTER CITADEL");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 52, (Lang.current == 0) ? "СТАРАЯ ЦИТАДЕЛЬ" : "OLD CITADEL");
			num += 40;
			GUIManager.EndScrollView();
		}
		else if (this.gamemode == 5)
		{
			this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, 34f, 600f, 320f), this.scrollViewVector, new Rect(0f, 0f, 0f, 2646f));
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 501, (Lang.current == 0) ? "ДАСТ2" : "DUST2");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 502, (Lang.current == 0) ? "ИНФЕРНО" : "INFERNO");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 503, (Lang.current == 0) ? "ТРЕЙН" : "TRAIN");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 504, (Lang.current == 0) ? "АЦТЕК" : "AZTEC");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 505, (Lang.current == 0) ? "НЮК" : "NUKE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 506, (Lang.current == 0) ? "ОФИС" : "OFFICE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 507, (Lang.current == 0) ? "КЛАНМИЛЛ" : "CLANMILL");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 508, (Lang.current == 0) ? "ДАСТ1" : "DUST 1");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 509, (Lang.current == 0) ? "МЭНШЕН" : "MANSION");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 510, (Lang.current == 0) ? "ФЛАЙ2" : "FLY 2");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 511, (Lang.current == 0) ? "ТУСКАН" : "TUSCAN");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 512, (Lang.current == 0) ? "ДАСТ 2002" : "DUST 2002");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 513, (Lang.current == 0) ? "МИНИДАСТ" : "MINIDUST");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 514, (Lang.current == 0) ? "БАССЕЙН" : "POOL");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 515, (Lang.current == 0) ? "ПЕРЕКРЁСТОК" : "CROSSROADS");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 516, (Lang.current == 0) ? "РЕКА" : "RIVER");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 517, (Lang.current == 0) ? "БАЗА В ПУСТЫНЕ" : "DESERT BASE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 518, (Lang.current == 0) ? "СТАНЦИЯ" : "STATION");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 519, (Lang.current == 0) ? "ПЫЛЬ" : "DUST");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 520, (Lang.current == 0) ? "МОНО" : "MONO");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 521, (Lang.current == 0) ? "МУРАВЕЙНИК" : "ANTHILL");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 522, (Lang.current == 0) ? "УБЕЖИЩЕ2" : "ASYLUM 2");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 523, (Lang.current == 0) ? "ЛАБОРАТОРИЯ" : "LAB");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 524, (Lang.current == 0) ? "КВАРТАЛ" : "QUARTER");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 525, (Lang.current == 0) ? "ИНДИЯ" : "INDIA");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 526, (Lang.current == 0) ? "МИНИДАСТ2" : "MINIDUST 2");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 527, (Lang.current == 0) ? "КОБЛ" : "CBBLE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 528, (Lang.current == 0) ? "РЭД" : "RED");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 529, (Lang.current == 0) ? "АЦТЕРИАЛ" : "AZTERIAL");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 530, (Lang.current == 0) ? "ФАБРИКА" : "FACTORY");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 531, (Lang.current == 0) ? "СКЛАДЫ" : "WAREHOUSE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 532, (Lang.current == 0) ? "ДЕЛОВОЙ ЦЕНТР" : "BUSINESS CENTER");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 533, (Lang.current == 0) ? "БАШНИ" : "TOWERS");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 534, (Lang.current == 0) ? "ЛАГЕРЬ" : "CAMP");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 535, (Lang.current == 0) ? "АССАУЛТ" : "ASSAULT");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 536, (Lang.current == 0) ? "ТЕРМИНАЛ" : "TERMINAL");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 537, (Lang.current == 0) ? "ХИМЛАБ" : "CHEMLAB");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 538, (Lang.current == 0) ? "МЕТРО" : "METRO");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 539, (Lang.current == 0) ? "ОКОПЫ" : "TRENCH");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 540, (Lang.current == 0) ? "РУИНЫ" : "RUINS");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 541, (Lang.current == 0) ? "КАНАЛИЗАЦИЯ" : "CANALIZATION");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 542, (Lang.current == 0) ? "ЛЮКСВИЛЬ" : "LUXVILL");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 543, (Lang.current == 0) ? "ВОКЗАЛ" : "RAILWAY STATION");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 544, (Lang.current == 0) ? "ИТАЛИЯ" : "ITALY");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 545, (Lang.current == 0) ? "ТУРНИРНАЯ КОНТРА" : "TOURNAMENT CONTRA");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 546, (Lang.current == 0) ? "ПОДЗЕМКА1337" : "SUBWAY1337");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 547, (Lang.current == 0) ? "АНГАР" : "HANGAR");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 548, (Lang.current == 0) ? "2013" : "2013");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 549, (Lang.current == 0) ? "БЕРЛИН" : "BERLIN");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 550, (Lang.current == 0) ? "РАЗВАЛИНЫ" : "DEBRIS");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 551, (Lang.current == 0) ? "ГРАНЬ" : "FACE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 552, (Lang.current == 0) ? "КЭШ" : "CACHE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 553, (Lang.current == 0) ? "КЭШ" : "CACHE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 554, (Lang.current == 0) ? "КЭШ" : "CACHE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 555, (Lang.current == 0) ? "КЭШ" : "CACHE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 556, (Lang.current == 0) ? "КЭШ" : "CACHE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 557, (Lang.current == 0) ? "АДРЕНАЛИН" : "ADRENALIN");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 558, (Lang.current == 0) ? "ЗИМНЯЯ ФАБРИКА" : "WINTER FACTORY");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 559, (Lang.current == 0) ? "ЗИМНИЙ АССАУЛТ" : "WINTER ASSAULT");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 560, (Lang.current == 0) ? "ЗИМНИЙ МЕНШЕН" : "WINTER MANSION");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 561, (Lang.current == 0) ? "ЗИМНИЙ ДАСТ1" : "WINTER DUST 1");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 562, (Lang.current == 0) ? "СТАРЫЙ ОФИС" : "OLD OFFICE");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 563, (Lang.current == 0) ? "ТУСКАН МИНИ" : "TUSCAN MINI");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 564, (Lang.current == 0) ? "АРЕНА МАЙЯ" : "ARENA MAYA");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 565, (Lang.current == 0) ? "НОВЫЙ ДАСТ" : "NEW DUST");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 566, (Lang.current == 0) ? "ЦЕПЬ" : "CHAIN");
			num += 40;
			this.DrawSlot(vector + this.scrollViewVector - new Vector2(0f, 34f), x, num, 567, (Lang.current == 0) ? "КАНЬЕН" : "CANYON");
			num += 40;
			GUIManager.EndScrollView();
		}
		if (GUIManager.DrawButton(new Rect(462f, 358f, 128f, 32f), vector, new Color(0f, 1f, 0f, 1f), Lang.GetLabel(123)))
		{
			this.SetActive(false, 0);
		}
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x00048648 File Offset: 0x00046848
	private void DrawSlot(Vector2 mpos, int x, int y, int mapid, string mapname)
	{
		if (GUIManager.DrawButton(new Rect((float)x, (float)y, 256f, 32f), mpos, new Color(0f, 1f, 0f, 1f), mapname))
		{
			if (this.cscl == null)
			{
				this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
			}
			this.cscl.send_private_settings(this.gamemode, mapid);
			this.SetActive(false, 0);
		}
	}

	// Token: 0x04000823 RID: 2083
	public bool active;

	// Token: 0x04000824 RID: 2084
	private Rect r_window;

	// Token: 0x04000825 RID: 2085
	private PlayerControl cspc;

	// Token: 0x04000826 RID: 2086
	private Client cscl;

	// Token: 0x04000827 RID: 2087
	private bool dataload;

	// Token: 0x04000828 RID: 2088
	private int gamemode;

	// Token: 0x04000829 RID: 2089
	private Vector2 scrollViewVector = Vector2.zero;
}
