using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200010D RID: 269
public class GUIUtils
{
	// Token: 0x060009B2 RID: 2482 RVA: 0x0008057C File Offset: 0x0007E77C
	public static Rect[] Separate(Rect mainRect, int xCount, int yCount)
	{
		float num = mainRect.width / (float)xCount;
		float num2 = mainRect.height / (float)yCount;
		List<Rect> list = new List<Rect>();
		for (int i = 0; i < yCount; i++)
		{
			for (int j = 0; j < xCount; j++)
			{
				Rect item = new Rect(mainRect.x + num * (float)j, mainRect.y + num2 * (float)i, num, num2);
				list.Add(item);
			}
		}
		return list.ToArray();
	}
}
