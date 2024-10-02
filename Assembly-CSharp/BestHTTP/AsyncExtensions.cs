using System;
using System.Threading;
using System.Threading.Tasks;
using LitJson;

namespace BestHTTP
{
	// Token: 0x02000190 RID: 400
	public static class AsyncExtensions
	{
		// Token: 0x06000E9D RID: 3741 RVA: 0x00096A54 File Offset: 0x00094C54
		public static Task<T> GetFromJsonResultAsync<T>(this HTTPRequest request, CancellationToken token = default(CancellationToken))
		{
			return HTTPRequestAsyncExtensions.CreateTask<T>(request, token, delegate(HTTPRequest req, HTTPResponse resp, TaskCompletionSource<T> tcs)
			{
				switch (req.State)
				{
				case HTTPRequestStates.Finished:
					if (resp.IsSuccess)
					{
						tcs.TrySetResult(JsonMapper.ToObject<T>(resp.DataAsText));
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
	}
}
