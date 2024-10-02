using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class BlockSetViewer : MonoBehaviour
{
	// Token: 0x06000038 RID: 56 RVA: 0x00002B90 File Offset: 0x00000D90
	public void SetBlockSet(BlockSet blockSet)
	{
		this.blockSet = blockSet;
		this.index = Mathf.Clamp(this.index, 0, blockSet.GetBlockCount());
		this.BuildBlock(blockSet.GetBlock(this.index));
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002BC4 File Offset: 0x00000DC4
	private void BuildBlock(Block block)
	{
		base.GetComponent<Renderer>().material = block.GetAtlas().GetMaterial();
		MeshFilter component = base.GetComponent<MeshFilter>();
		block.Build().ToMesh(component.mesh);
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002C00 File Offset: 0x00000E00
	private void OnGUI()
	{
		Rect position = new Rect((float)(Screen.width - 180), 0f, 180f, (float)Screen.height);
		int num = this.index;
		this.index = BlockSetViewer.DrawList(position, this.index, this.blockSet.GetBlocks(), ref this.scrollPosition);
		if (num != this.index)
		{
			this.BuildBlock(this.blockSet.GetBlock(this.index));
		}
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00002C78 File Offset: 0x00000E78
	private static int DrawList(Rect position, int selected, Block[] list, ref Vector2 scrollPosition)
	{
		GUILayout.BeginArea(position, GUI.skin.box);
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, Array.Empty<GUILayoutOption>());
		for (int i = 0; i < list.Length; i++)
		{
			if (list[i] != null && BlockSetViewer.DrawItem(list[i], i == selected))
			{
				selected = i;
				Event.current.Use();
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		return selected;
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002CE4 File Offset: 0x00000EE4
	private static bool DrawItem(Block block, bool selected)
	{
		Rect rect = GUILayoutUtility.GetRect(0f, 40f, new GUILayoutOption[]
		{
			GUILayout.ExpandWidth(true)
		});
		if (selected)
		{
			GUI.Box(rect, GUIContent.none);
		}
		GUIStyle guistyle = new GUIStyle(GUI.skin.label);
		guistyle.alignment = TextAnchor.MiddleLeft;
		Rect position = new Rect(rect.x + 4f, rect.y + 4f, rect.height - 8f, rect.height - 8f);
		block.DrawPreview(position);
		Rect position2 = rect;
		position2.xMin = position.xMax + 4f;
		GUI.Label(position2, block.GetName(), guistyle);
		return Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains(Event.current.mousePosition);
	}

	// Token: 0x04000024 RID: 36
	private BlockSet blockSet;

	// Token: 0x04000025 RID: 37
	private int index;

	// Token: 0x04000026 RID: 38
	private Vector2 scrollPosition;
}
