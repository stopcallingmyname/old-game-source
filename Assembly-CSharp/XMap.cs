using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

// Token: 0x02000124 RID: 292
[XmlRoot("Map")]
public class XMap
{
	// Token: 0x06000A4E RID: 2638 RVA: 0x0008349C File Offset: 0x0008169C
	public void SetName(string name)
	{
		this.MapName = name;
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x000834A8 File Offset: 0x000816A8
	public void Save(string path)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(XMap));
		using (FileStream fileStream = new FileStream(path, FileMode.Create))
		{
			xmlSerializer.Serialize(fileStream, this);
		}
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x000834F4 File Offset: 0x000816F4
	public static XMap Load(string path)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(XMap));
		XMap result;
		using (FileStream fileStream = new FileStream(path, FileMode.Open))
		{
			result = (xmlSerializer.Deserialize(fileStream) as XMap);
		}
		return result;
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x00083544 File Offset: 0x00081744
	public static XMap InternalLoad(string levelname)
	{
		TextAsset textAsset = (TextAsset)Resources.Load(levelname, typeof(TextAsset));
		if (textAsset == null)
		{
			Debug.LogError("Could not load text asset " + levelname);
			return null;
		}
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(XMap));
		XmlTextReader xmlReader = new XmlTextReader(new StringReader(textAsset.ToString()));
		return (XMap)xmlSerializer.Deserialize(xmlReader);
	}

	// Token: 0x0400100F RID: 4111
	[XmlArray("Chunks")]
	[XmlArrayItem("Chunk")]
	public List<XChunk> Chunks = new List<XChunk>();

	// Token: 0x04001010 RID: 4112
	[XmlAttribute("name")]
	public string MapName;
}
