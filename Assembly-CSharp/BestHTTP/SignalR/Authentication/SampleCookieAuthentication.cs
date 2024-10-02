using System;
using BestHTTP.Cookies;

namespace BestHTTP.SignalR.Authentication
{
	// Token: 0x0200022C RID: 556
	public sealed class SampleCookieAuthentication : IAuthenticationProvider
	{
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x000A92F0 File Offset: 0x000A74F0
		// (set) Token: 0x06001406 RID: 5126 RVA: 0x000A92F8 File Offset: 0x000A74F8
		public Uri AuthUri { get; private set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06001407 RID: 5127 RVA: 0x000A9301 File Offset: 0x000A7501
		// (set) Token: 0x06001408 RID: 5128 RVA: 0x000A9309 File Offset: 0x000A7509
		public string UserName { get; private set; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06001409 RID: 5129 RVA: 0x000A9312 File Offset: 0x000A7512
		// (set) Token: 0x0600140A RID: 5130 RVA: 0x000A931A File Offset: 0x000A751A
		public string Password { get; private set; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x000A9323 File Offset: 0x000A7523
		// (set) Token: 0x0600140C RID: 5132 RVA: 0x000A932B File Offset: 0x000A752B
		public string UserRoles { get; private set; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x0600140D RID: 5133 RVA: 0x000A9334 File Offset: 0x000A7534
		// (set) Token: 0x0600140E RID: 5134 RVA: 0x000A933C File Offset: 0x000A753C
		public bool IsPreAuthRequired { get; private set; }

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x0600140F RID: 5135 RVA: 0x000A9348 File Offset: 0x000A7548
		// (remove) Token: 0x06001410 RID: 5136 RVA: 0x000A9380 File Offset: 0x000A7580
		public event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06001411 RID: 5137 RVA: 0x000A93B8 File Offset: 0x000A75B8
		// (remove) Token: 0x06001412 RID: 5138 RVA: 0x000A93F0 File Offset: 0x000A75F0
		public event OnAuthenticationFailedDelegate OnAuthenticationFailed;

		// Token: 0x06001413 RID: 5139 RVA: 0x000A9425 File Offset: 0x000A7625
		public SampleCookieAuthentication(Uri authUri, string user, string passwd, string roles)
		{
			this.AuthUri = authUri;
			this.UserName = user;
			this.Password = passwd;
			this.UserRoles = roles;
			this.IsPreAuthRequired = true;
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x000A9454 File Offset: 0x000A7654
		public void StartAuthentication()
		{
			this.AuthRequest = new HTTPRequest(this.AuthUri, HTTPMethods.Post, new OnRequestFinishedDelegate(this.OnAuthRequestFinished));
			this.AuthRequest.AddField("userName", this.UserName);
			this.AuthRequest.AddField("Password", this.Password);
			this.AuthRequest.AddField("roles", this.UserRoles);
			this.AuthRequest.Send();
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x000A94CD File Offset: 0x000A76CD
		public void PrepareRequest(HTTPRequest request, RequestTypes type)
		{
			request.Cookies.Add(this.Cookie);
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x000A94E0 File Offset: 0x000A76E0
		private void OnAuthRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			this.AuthRequest = null;
			string reason = string.Empty;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess)
				{
					Cookie cookie;
					if (resp.Cookies == null)
					{
						cookie = null;
					}
					else
					{
						cookie = resp.Cookies.Find((Cookie c) => c.Name.Equals(".ASPXAUTH"));
					}
					this.Cookie = cookie;
					if (this.Cookie != null)
					{
						HTTPManager.Logger.Information("CookieAuthentication", "Auth. Cookie found!");
						if (this.OnAuthenticationSucceded != null)
						{
							this.OnAuthenticationSucceded(this);
						}
						return;
					}
					HTTPManager.Logger.Warning("CookieAuthentication", reason = "Auth. Cookie NOT found!");
				}
				else
				{
					HTTPManager.Logger.Warning("CookieAuthentication", reason = string.Format("Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText));
				}
				break;
			case HTTPRequestStates.Error:
				HTTPManager.Logger.Warning("CookieAuthentication", reason = "Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
				break;
			case HTTPRequestStates.Aborted:
				HTTPManager.Logger.Warning("CookieAuthentication", reason = "Request Aborted!");
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				HTTPManager.Logger.Error("CookieAuthentication", reason = "Connection Timed Out!");
				break;
			case HTTPRequestStates.TimedOut:
				HTTPManager.Logger.Error("CookieAuthentication", reason = "Processing the request Timed Out!");
				break;
			}
			if (this.OnAuthenticationFailed != null)
			{
				this.OnAuthenticationFailed(this, reason);
			}
		}

		// Token: 0x040015EF RID: 5615
		private HTTPRequest AuthRequest;

		// Token: 0x040015F0 RID: 5616
		private Cookie Cookie;
	}
}
