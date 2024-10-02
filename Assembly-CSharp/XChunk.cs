using System;
using System.Collections.Generic;
using System.Xml.Serialization;

// Token: 0x02000123 RID: 291
public class XChunk
{
	// Token: 0x0400100D RID: 4109
	public Vector3i pos;

	// Token: 0x0400100E RID: 4110
	[XmlArray("Blocks")]
	[XmlArrayItem("Block")]
	public List<XBlock> Blocks = new List<XBlock>();
}
