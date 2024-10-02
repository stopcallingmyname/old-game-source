using System;
using System.IO;
using BestHTTP.Extensions;
using BestHTTP.PlatformSupport.FileSystem;

namespace BestHTTP
{
	// Token: 0x02000173 RID: 371
	internal sealed class FileConnection : ConnectionBase
	{
		// Token: 0x06000D1E RID: 3358 RVA: 0x00090800 File Offset: 0x0008EA00
		public FileConnection(string serverAddress) : base(serverAddress)
		{
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0009080C File Offset: 0x0008EA0C
		internal override void Abort(HTTPConnectionStates newState)
		{
			base.State = newState;
			HTTPConnectionStates state = base.State;
			if (state == HTTPConnectionStates.TimedOut)
			{
				base.TimedOutStart = DateTime.UtcNow;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0009083C File Offset: 0x0008EA3C
		protected override void ThreadFunc()
		{
			try
			{
				using (Stream stream = HTTPManager.IOService.CreateFileStream(base.CurrentRequest.CurrentUri.LocalPath, FileStreamModes.Open))
				{
					using (StreamList streamList = new StreamList(new Stream[]
					{
						new BufferPoolMemoryStream(),
						stream
					}))
					{
						streamList.Write("HTTP/1.1 200 Ok\r\n");
						streamList.Write("Content-Type: application/octet-stream\r\n");
						streamList.Write("Content-Length: " + stream.Length.ToString() + "\r\n");
						streamList.Write("\r\n");
						streamList.Seek(0L, SeekOrigin.Begin);
						base.CurrentRequest.Response = new HTTPResponse(base.CurrentRequest, streamList, base.CurrentRequest.UseStreaming, false);
						if (!base.CurrentRequest.Response.Receive(-1, true))
						{
							base.CurrentRequest.Response = null;
						}
					}
				}
			}
			catch (Exception exception)
			{
				if (base.CurrentRequest != null)
				{
					base.CurrentRequest.Response = null;
					HTTPConnectionStates state = base.State;
					if (state != HTTPConnectionStates.AbortRequested)
					{
						if (state != HTTPConnectionStates.TimedOut)
						{
							base.CurrentRequest.Exception = exception;
							base.CurrentRequest.State = HTTPRequestStates.Error;
						}
						else
						{
							base.CurrentRequest.State = HTTPRequestStates.TimedOut;
						}
					}
					else
					{
						base.CurrentRequest.State = HTTPRequestStates.Aborted;
					}
				}
			}
			finally
			{
				base.State = HTTPConnectionStates.Closed;
				if (base.CurrentRequest.State == HTTPRequestStates.Processing)
				{
					if (base.CurrentRequest.Response != null)
					{
						base.CurrentRequest.State = HTTPRequestStates.Finished;
					}
					else
					{
						base.CurrentRequest.State = HTTPRequestStates.Error;
					}
				}
			}
		}
	}
}
