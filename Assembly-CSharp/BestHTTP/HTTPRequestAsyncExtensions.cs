using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace BestHTTP
{
	// Token: 0x02000174 RID: 372
	public static class HTTPRequestAsyncExtensions
	{
		// Token: 0x06000D21 RID: 3361 RVA: 0x00090A2C File Offset: 0x0008EC2C
		public static Task<HTTPResponse> GetHTTPResponseAsync(this HTTPRequest request, CancellationToken token = default(CancellationToken))
		{
			return HTTPRequestAsyncExtensions.CreateTask<HTTPResponse>(request, token, delegate(HTTPRequest req, HTTPResponse resp, TaskCompletionSource<HTTPResponse> tcs)
			{
				switch (req.State)
				{
				case HTTPRequestStates.Finished:
					tcs.TrySetResult(resp);
					return;
				case HTTPRequestStates.Error:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
					tcs.TrySetException(req.Exception ?? new Exception("No Exception"));
					return;
				case HTTPRequestStates.Aborted:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Request Aborted!");
					tcs.TrySetCanceled();
					return;
				case HTTPRequestStates.ConnectionTimedOut:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Connection Timed Out!");
					tcs.TrySetException(new Exception("Connection Timed Out!"));
					return;
				case HTTPRequestStates.TimedOut:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Processing the request Timed Out!");
					tcs.TrySetException(new Exception("Processing the request Timed Out!"));
					return;
				default:
					return;
				}
			});
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00090A60 File Offset: 0x0008EC60
		public static Task<string> GetAsStringAsync(this HTTPRequest request, CancellationToken token = default(CancellationToken))
		{
			return HTTPRequestAsyncExtensions.CreateTask<string>(request, token, delegate(HTTPRequest req, HTTPResponse resp, TaskCompletionSource<string> tcs)
			{
				switch (req.State)
				{
				case HTTPRequestStates.Finished:
					if (resp.IsSuccess)
					{
						tcs.TrySetResult(resp.DataAsText);
						return;
					}
					tcs.TrySetException(new Exception(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText)));
					return;
				case HTTPRequestStates.Error:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
					tcs.TrySetException(req.Exception ?? new Exception("No Exception"));
					return;
				case HTTPRequestStates.Aborted:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Request Aborted!");
					tcs.TrySetCanceled();
					return;
				case HTTPRequestStates.ConnectionTimedOut:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Connection Timed Out!");
					tcs.TrySetException(new Exception("Connection Timed Out!"));
					return;
				case HTTPRequestStates.TimedOut:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Processing the request Timed Out!");
					tcs.TrySetException(new Exception("Processing the request Timed Out!"));
					return;
				default:
					return;
				}
			});
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00090A94 File Offset: 0x0008EC94
		public static Task<Texture2D> GetAsTexture2DAsync(this HTTPRequest request, CancellationToken token = default(CancellationToken))
		{
			return HTTPRequestAsyncExtensions.CreateTask<Texture2D>(request, token, delegate(HTTPRequest req, HTTPResponse resp, TaskCompletionSource<Texture2D> tcs)
			{
				switch (req.State)
				{
				case HTTPRequestStates.Finished:
					if (resp.IsSuccess)
					{
						tcs.TrySetResult(resp.DataAsTexture2D);
						return;
					}
					tcs.TrySetException(new Exception(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText)));
					return;
				case HTTPRequestStates.Error:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
					tcs.TrySetException(req.Exception ?? new Exception("No Exception"));
					return;
				case HTTPRequestStates.Aborted:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Request Aborted!");
					tcs.TrySetCanceled();
					return;
				case HTTPRequestStates.ConnectionTimedOut:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Connection Timed Out!");
					tcs.TrySetException(new Exception("Connection Timed Out!"));
					return;
				case HTTPRequestStates.TimedOut:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Processing the request Timed Out!");
					tcs.TrySetException(new Exception("Processing the request Timed Out!"));
					return;
				default:
					return;
				}
			});
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00090AC8 File Offset: 0x0008ECC8
		public static Task<byte[]> GetRawDataAsync(this HTTPRequest request, CancellationToken token = default(CancellationToken))
		{
			return HTTPRequestAsyncExtensions.CreateTask<byte[]>(request, token, delegate(HTTPRequest req, HTTPResponse resp, TaskCompletionSource<byte[]> tcs)
			{
				switch (req.State)
				{
				case HTTPRequestStates.Finished:
					if (resp.IsSuccess)
					{
						tcs.TrySetResult(resp.Data);
						return;
					}
					tcs.TrySetException(new Exception(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText)));
					return;
				case HTTPRequestStates.Error:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
					tcs.TrySetException(req.Exception ?? new Exception("No Exception"));
					return;
				case HTTPRequestStates.Aborted:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Request Aborted!");
					tcs.TrySetCanceled();
					return;
				case HTTPRequestStates.ConnectionTimedOut:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Connection Timed Out!");
					tcs.TrySetException(new Exception("Connection Timed Out!"));
					return;
				case HTTPRequestStates.TimedOut:
					HTTPRequestAsyncExtensions.VerboseLogging(request, "Processing the request Timed Out!");
					tcs.TrySetException(new Exception("Processing the request Timed Out!"));
					return;
				default:
					return;
				}
			});
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00090AFC File Offset: 0x0008ECFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static Task<T> CreateTask<T>(HTTPRequest request, CancellationToken token, Action<HTTPRequest, HTTPResponse, TaskCompletionSource<T>> callback)
		{
			TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
			request.Callback = delegate(HTTPRequest req, HTTPResponse resp)
			{
				callback(req, resp, tcs);
			};
			if (token.CanBeCanceled)
			{
				token.Register(delegate(object state)
				{
					HTTPRequest httprequest = state as HTTPRequest;
					if (httprequest == null)
					{
						return;
					}
					httprequest.Abort();
				}, request);
			}
			if (request.State == HTTPRequestStates.Initial)
			{
				request.Send();
			}
			return tcs.Task;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x00090B7E File Offset: 0x0008ED7E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void VerboseLogging(HTTPRequest request, string str)
		{
			HTTPManager.Logger.Verbose("HTTPRequestAsyncExtensions", "'" + request.CurrentUri.ToString() + "' - " + str);
		}
	}
}
