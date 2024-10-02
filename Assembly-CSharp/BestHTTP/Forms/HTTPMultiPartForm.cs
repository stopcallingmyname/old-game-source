using System;
using BestHTTP.Extensions;

namespace BestHTTP.Forms
{
	// Token: 0x020007E1 RID: 2017
	public sealed class HTTPMultiPartForm : HTTPFormBase
	{
		// Token: 0x060047C9 RID: 18377 RVA: 0x0019701C File Offset: 0x0019521C
		public HTTPMultiPartForm()
		{
			this.Boundary = "BestHTTP_HTTPMultiPartForm_" + this.GetHashCode().ToString("X");
		}

		// Token: 0x060047CA RID: 18378 RVA: 0x00197052 File Offset: 0x00195252
		public override void PrepareRequest(HTTPRequest request)
		{
			request.SetHeader("Content-Type", "multipart/form-data; boundary=" + this.Boundary);
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x00197070 File Offset: 0x00195270
		public override byte[] GetData()
		{
			if (this.CachedData != null)
			{
				return this.CachedData;
			}
			byte[] array;
			using (BufferPoolMemoryStream bufferPoolMemoryStream = new BufferPoolMemoryStream())
			{
				for (int i = 0; i < base.Fields.Count; i++)
				{
					HTTPFieldData httpfieldData = base.Fields[i];
					bufferPoolMemoryStream.WriteLine("--" + this.Boundary);
					bufferPoolMemoryStream.WriteLine("Content-Disposition: form-data; name=\"" + httpfieldData.Name + "\"" + ((!string.IsNullOrEmpty(httpfieldData.FileName)) ? ("; filename=\"" + httpfieldData.FileName + "\"") : string.Empty));
					if (!string.IsNullOrEmpty(httpfieldData.MimeType))
					{
						bufferPoolMemoryStream.WriteLine("Content-Type: " + httpfieldData.MimeType);
					}
					bufferPoolMemoryStream.WriteLine("Content-Length: " + httpfieldData.Payload.Length.ToString());
					bufferPoolMemoryStream.WriteLine();
					bufferPoolMemoryStream.Write(httpfieldData.Payload, 0, httpfieldData.Payload.Length);
					bufferPoolMemoryStream.Write(HTTPRequest.EOL, 0, HTTPRequest.EOL.Length);
				}
				bufferPoolMemoryStream.WriteLine("--" + this.Boundary + "--");
				base.IsChanged = false;
				array = (this.CachedData = bufferPoolMemoryStream.ToArray());
				array = array;
			}
			return array;
		}

		// Token: 0x04002EDD RID: 11997
		private string Boundary;

		// Token: 0x04002EDE RID: 11998
		private byte[] CachedData;
	}
}
