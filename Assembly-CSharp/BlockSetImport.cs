using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEngine;

// Token: 0x020000FC RID: 252
public class BlockSetImport
{
	// Token: 0x0600091A RID: 2330 RVA: 0x0007DF18 File Offset: 0x0007C118
	public static void Import(BlockSet blockSet, string xml)
	{
		if (xml != null && xml.Length > 0)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			BlockSetImport.ReadBlockSet(blockSet, xmlDocument);
		}
		Block[] blocks = blockSet.GetBlocks();
		for (int i = 0; i < blocks.Length; i++)
		{
			blocks[i].Init(blockSet);
		}
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x0007DF64 File Offset: 0x0007C164
	private static void ReadBlockSet(BlockSet blockSet, XmlDocument document)
	{
		XmlNode blockSetNode = BlockSetImport.FindNodeByName(document, "BlockSet");
		Atlas[] atlases = BlockSetImport.ReadAtlasList(blockSetNode);
		blockSet.SetAtlases(atlases);
		Block[] blocks = BlockSetImport.ReadBlockList(blockSetNode);
		blockSet.SetBlocks(blocks);
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x0007DF98 File Offset: 0x0007C198
	private static Atlas[] ReadAtlasList(XmlNode blockSetNode)
	{
		XmlNode xmlNode = BlockSetImport.FindNodeByName(blockSetNode, "AtlasList");
		List<Atlas> list = new List<Atlas>();
		foreach (object obj in xmlNode.ChildNodes)
		{
			Atlas item = BlockSetImport.ReadAtlas((XmlNode)obj);
			list.Add(item);
		}
		return list.ToArray();
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x0007E00C File Offset: 0x0007C20C
	private static Atlas ReadAtlas(XmlNode node)
	{
		Atlas atlas = new Atlas();
		foreach (object obj in node)
		{
			XmlNode xmlNode = (XmlNode)obj;
			if (BlockSetImport.GetField(atlas.GetType(), xmlNode.Name).FieldType.IsSubclassOf(typeof(Object)))
			{
				BlockSetImport.ReadResourceField(xmlNode, atlas);
			}
			else
			{
				BlockSetImport.ReadField(xmlNode, atlas);
			}
		}
		return atlas;
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x0007E098 File Offset: 0x0007C298
	private static Block[] ReadBlockList(XmlNode blockSetNode)
	{
		XmlNode xmlNode = BlockSetImport.FindNodeByName(blockSetNode, "BlockList");
		List<Block> list = new List<Block>();
		foreach (object obj in xmlNode.ChildNodes)
		{
			Block item = BlockSetImport.ReadBlock((XmlNode)obj);
			list.Add(item);
		}
		return list.ToArray();
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x0007E10C File Offset: 0x0007C30C
	private static Block ReadBlock(XmlNode node)
	{
		Block block = (Block)Activator.CreateInstance(Assembly.Load("Assembly-CSharp").GetType(node.Name));
		foreach (object obj in node)
		{
			BlockSetImport.ReadField((XmlNode)obj, block);
		}
		return block;
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x0007E180 File Offset: 0x0007C380
	private static XmlNode FindNodeByName(XmlNode node, string name)
	{
		foreach (object obj in node)
		{
			XmlNode xmlNode = (XmlNode)obj;
			if (xmlNode.Name.Equals(name))
			{
				return xmlNode;
			}
		}
		return null;
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x0007E1E4 File Offset: 0x0007C3E4
	private static FieldInfo GetField(Type type, string name)
	{
		FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		if (field != null)
		{
			return field;
		}
		if (type != typeof(object))
		{
			return BlockSetImport.GetField(type.BaseType, name);
		}
		return null;
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x0007E228 File Offset: 0x0007C428
	private static void ReadField(XmlNode fieldNode, object obj)
	{
		FieldInfo field = BlockSetImport.GetField(obj.GetType(), fieldNode.Name);
		object value = BlockSetImport.Parse(field.FieldType, fieldNode.InnerText);
		field.SetValue(obj, value);
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x0007E260 File Offset: 0x0007C460
	private static object Parse(Type type, string val)
	{
		if (type == typeof(bool))
		{
			return bool.Parse(val);
		}
		if (type == typeof(byte))
		{
			return byte.Parse(val);
		}
		if (type == typeof(short))
		{
			return short.Parse(val);
		}
		if (type == typeof(int))
		{
			return int.Parse(val);
		}
		if (type == typeof(long))
		{
			return long.Parse(val);
		}
		if (type == typeof(float))
		{
			return float.Parse(val);
		}
		if (type == typeof(double))
		{
			return double.Parse(val);
		}
		if (type == typeof(string))
		{
			return val;
		}
		throw new Exception("Unsupported type: " + type.ToString());
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x0007E368 File Offset: 0x0007C568
	private static void ReadResourceField(XmlNode fieldNode, object obj)
	{
		FieldInfo field = BlockSetImport.GetField(obj.GetType(), fieldNode.Name);
		Object value = Resources.Load(fieldNode.InnerText, field.FieldType);
		field.SetValue(obj, value);
	}
}
