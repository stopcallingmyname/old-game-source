using System;
using System.Text;

namespace BestHTTP.Forms
{
	// Token: 0x020007DE RID: 2014
	public class HTTPFieldData
	{
		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x060047A9 RID: 18345 RVA: 0x00196D9F File Offset: 0x00194F9F
		// (set) Token: 0x060047AA RID: 18346 RVA: 0x00196DA7 File Offset: 0x00194FA7
		public string Name { get; set; }

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x060047AB RID: 18347 RVA: 0x00196DB0 File Offset: 0x00194FB0
		// (set) Token: 0x060047AC RID: 18348 RVA: 0x00196DB8 File Offset: 0x00194FB8
		public string FileName { get; set; }

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x060047AD RID: 18349 RVA: 0x00196DC1 File Offset: 0x00194FC1
		// (set) Token: 0x060047AE RID: 18350 RVA: 0x00196DC9 File Offset: 0x00194FC9
		public string MimeType { get; set; }

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x060047AF RID: 18351 RVA: 0x00196DD2 File Offset: 0x00194FD2
		// (set) Token: 0x060047B0 RID: 18352 RVA: 0x00196DDA File Offset: 0x00194FDA
		public Encoding Encoding { get; set; }

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x060047B1 RID: 18353 RVA: 0x00196DE3 File Offset: 0x00194FE3
		// (set) Token: 0x060047B2 RID: 18354 RVA: 0x00196DEB File Offset: 0x00194FEB
		public string Text { get; set; }

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x060047B3 RID: 18355 RVA: 0x00196DF4 File Offset: 0x00194FF4
		// (set) Token: 0x060047B4 RID: 18356 RVA: 0x00196DFC File Offset: 0x00194FFC
		public byte[] Binary { get; set; }

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x060047B5 RID: 18357 RVA: 0x00196E08 File Offset: 0x00195008
		public byte[] Payload
		{
			get
			{
				if (this.Binary != null)
				{
					return this.Binary;
				}
				if (this.Encoding == null)
				{
					this.Encoding = Encoding.UTF8;
				}
				return this.Binary = this.Encoding.GetBytes(this.Text);
			}
		}
	}
}
