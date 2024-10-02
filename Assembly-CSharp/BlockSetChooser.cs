using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class BlockSetChooser : MonoBehaviour
{
	// Token: 0x06000033 RID: 51 RVA: 0x000029F1 File Offset: 0x00000BF1
	private void Start()
	{
		this.viewer = base.GetComponent<BlockSetViewer>();
		this.viewer.SetBlockSet(this.blockSetList[this.index]);
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002A18 File Offset: 0x00000C18
	private void OnGUI()
	{
		int num = this.index;
		Rect position = new Rect(0f, 0f, 180f, (float)Screen.height);
		this.index = BlockSetChooser.DrawList(position, this.index, this.blockSetList, ref this.scrollPosition);
		if (this.index != num)
		{
			this.viewer.SetBlockSet(this.blockSetList[this.index]);
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002A88 File Offset: 0x00000C88
	private static int DrawList(Rect position, int selected, BlockSet[] list, ref Vector2 scrollPosition)
	{
		GUILayout.BeginArea(position, GUI.skin.box);
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, Array.Empty<GUILayoutOption>());
		for (int i = 0; i < list.Length; i++)
		{
			if (!(list[i] == null) && BlockSetChooser.DrawItem(list[i].name, i == selected))
			{
				selected = i;
				Event.current.Use();
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		return selected;
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002B00 File Offset: 0x00000D00
	private static bool DrawItem(string name, bool selected)
	{
		Rect rect = GUILayoutUtility.GetRect(new GUIContent(name), GUI.skin.box);
		if (selected)
		{
			GUI.Box(rect, GUIContent.none);
		}
		GUIStyle guistyle = new GUIStyle(GUI.skin.label);
		guistyle.padding = GUI.skin.box.padding;
		guistyle.alignment = TextAnchor.MiddleLeft;
		GUI.Label(rect, name, guistyle);
		return Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains(Event.current.mousePosition);
	}

	// Token: 0x04000020 RID: 32
	[SerializeField]
	private BlockSet[] blockSetList;

	// Token: 0x04000021 RID: 33
	private int index;

	// Token: 0x04000022 RID: 34
	private Vector2 scrollPosition;

	// Token: 0x04000023 RID: 35
	private BlockSetViewer viewer;
}
