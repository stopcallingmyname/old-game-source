using System;
using System.Linq;
using System.Text;
using BestHTTP.JSON;

namespace BestHTTP.Forms
{
	// Token: 0x020007E3 RID: 2019
	public sealed class RawJsonForm : HTTPFormBase
	{
		// Token: 0x060047D0 RID: 18384 RVA: 0x00197371 File Offset: 0x00195571
		public override void PrepareRequest(HTTPRequest request)
		{
			request.SetHeader("Content-Type", "application/json");
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x00197384 File Offset: 0x00195584
		public override byte[] GetData()
		{
			if (this.CachedData != null && !base.IsChanged)
			{
				return this.CachedData;
			}
			string s = Json.Encode(base.Fields.ToDictionary((HTTPFieldData x) => x.Name, (HTTPFieldData x) => x.Text));
			base.IsChanged = false;
			return this.CachedData = Encoding.UTF8.GetBytes(s);
		}

		// Token: 0x04002EE1 RID: 12001
		private byte[] CachedData;
	}
}
