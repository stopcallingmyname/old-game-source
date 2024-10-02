using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class FontManager
{
	// Token: 0x060001F0 RID: 496 RVA: 0x00024E45 File Offset: 0x00023045
	public static void PreInit(Font _mainFont)
	{
		if (FontManager.font == null)
		{
			FontManager.font = new Font[4];
		}
		FontManager.font[0] = _mainFont;
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x00024E64 File Offset: 0x00023064
	public static void Init()
	{
		if (FontManager.font == null)
		{
			FontManager.font = new Font[4];
		}
		FontManager.font[1] = ContentLoader.LoadFont("Xolonium-Regular");
		FontManager.font[2] = ContentLoader.LoadFont("hoog_mini");
		FontManager.font[3] = ContentLoader.LoadFont("Ubuntu-B");
	}

	// Token: 0x040001AE RID: 430
	public static Font[] font;
}
