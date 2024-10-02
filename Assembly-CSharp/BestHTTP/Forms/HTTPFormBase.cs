using System;
using System.Collections.Generic;
using System.Text;

namespace BestHTTP.Forms
{
	// Token: 0x020007DF RID: 2015
	public class HTTPFormBase
	{
		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x060047B7 RID: 18359 RVA: 0x00196E51 File Offset: 0x00195051
		// (set) Token: 0x060047B8 RID: 18360 RVA: 0x00196E59 File Offset: 0x00195059
		public List<HTTPFieldData> Fields { get; set; }

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x060047B9 RID: 18361 RVA: 0x00196E62 File Offset: 0x00195062
		public bool IsEmpty
		{
			get
			{
				return this.Fields == null || this.Fields.Count == 0;
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x060047BA RID: 18362 RVA: 0x00196E7C File Offset: 0x0019507C
		// (set) Token: 0x060047BB RID: 18363 RVA: 0x00196E84 File Offset: 0x00195084
		public bool IsChanged { get; protected set; }

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x060047BC RID: 18364 RVA: 0x00196E8D File Offset: 0x0019508D
		// (set) Token: 0x060047BD RID: 18365 RVA: 0x00196E95 File Offset: 0x00195095
		public bool HasBinary { get; protected set; }

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x060047BE RID: 18366 RVA: 0x00196E9E File Offset: 0x0019509E
		// (set) Token: 0x060047BF RID: 18367 RVA: 0x00196EA6 File Offset: 0x001950A6
		public bool HasLongValue { get; protected set; }

		// Token: 0x060047C0 RID: 18368 RVA: 0x00196EAF File Offset: 0x001950AF
		public void AddBinaryData(string fieldName, byte[] content)
		{
			this.AddBinaryData(fieldName, content, null, null);
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x00196EBB File Offset: 0x001950BB
		public void AddBinaryData(string fieldName, byte[] content, string fileName)
		{
			this.AddBinaryData(fieldName, content, fileName, null);
		}

		// Token: 0x060047C2 RID: 18370 RVA: 0x00196EC8 File Offset: 0x001950C8
		public void AddBinaryData(string fieldName, byte[] content, string fileName, string mimeType)
		{
			if (this.Fields == null)
			{
				this.Fields = new List<HTTPFieldData>();
			}
			HTTPFieldData httpfieldData = new HTTPFieldData();
			httpfieldData.Name = fieldName;
			if (fileName == null)
			{
				httpfieldData.FileName = fieldName + ".dat";
			}
			else
			{
				httpfieldData.FileName = fileName;
			}
			if (mimeType == null)
			{
				httpfieldData.MimeType = "application/octet-stream";
			}
			else
			{
				httpfieldData.MimeType = mimeType;
			}
			httpfieldData.Binary = content;
			this.Fields.Add(httpfieldData);
			this.HasBinary = (this.IsChanged = true);
		}

		// Token: 0x060047C3 RID: 18371 RVA: 0x00196F4E File Offset: 0x0019514E
		public void AddField(string fieldName, string value)
		{
			this.AddField(fieldName, value, Encoding.UTF8);
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x00196F60 File Offset: 0x00195160
		public void AddField(string fieldName, string value, Encoding e)
		{
			if (this.Fields == null)
			{
				this.Fields = new List<HTTPFieldData>();
			}
			HTTPFieldData httpfieldData = new HTTPFieldData();
			httpfieldData.Name = fieldName;
			httpfieldData.FileName = null;
			if (e != null)
			{
				httpfieldData.MimeType = "text/plain; charset=" + e.WebName;
			}
			httpfieldData.Text = value;
			httpfieldData.Encoding = e;
			this.Fields.Add(httpfieldData);
			this.IsChanged = true;
			this.HasLongValue |= (value.Length > 256);
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x00196FE8 File Offset: 0x001951E8
		public virtual void CopyFrom(HTTPFormBase fields)
		{
			this.Fields = new List<HTTPFieldData>(fields.Fields);
			this.IsChanged = true;
			this.HasBinary = fields.HasBinary;
			this.HasLongValue = fields.HasLongValue;
		}

		// Token: 0x060047C6 RID: 18374 RVA: 0x000947C7 File Offset: 0x000929C7
		public virtual void PrepareRequest(HTTPRequest request)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060047C7 RID: 18375 RVA: 0x000947C7 File Offset: 0x000929C7
		public virtual byte[] GetData()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04002ED3 RID: 11987
		private const int LongLength = 256;
	}
}
