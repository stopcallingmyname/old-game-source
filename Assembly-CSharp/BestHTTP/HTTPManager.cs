using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using BestHTTP.Caching;
using BestHTTP.Cookies;
using BestHTTP.Extensions;
using BestHTTP.Logger;
using BestHTTP.PlatformSupport.FileSystem;
using BestHTTP.Statistics;
using Org.BouncyCastle.Crypto.Tls;
using UnityEngine;

namespace BestHTTP
{
	// Token: 0x02000179 RID: 377
	public static class HTTPManager
	{
		// Token: 0x06000D3A RID: 3386 RVA: 0x00091D58 File Offset: 0x0008FF58
		static HTTPManager()
		{
			HTTPManager.MaxConnectionIdleTime = TimeSpan.FromSeconds(20.0);
			HTTPManager.IsCookiesEnabled = true;
			HTTPManager.CookieJarSize = 10485760U;
			HTTPManager.EnablePrivateBrowsing = false;
			HTTPManager.ConnectTimeout = TimeSpan.FromSeconds(20.0);
			HTTPManager.RequestTimeout = TimeSpan.FromSeconds(60.0);
			HTTPManager.logger = new DefaultLogger();
			HTTPManager.DefaultCertificateVerifyer = null;
			HTTPManager.UseAlternateSSLDefaultValue = true;
			HTTPManager.IOService = new DefaultIOService();
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x00091E4A File Offset: 0x0009004A
		// (set) Token: 0x06000D3C RID: 3388 RVA: 0x00091E51 File Offset: 0x00090051
		public static byte MaxConnectionPerServer
		{
			get
			{
				return HTTPManager.maxConnectionPerServer;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("MaxConnectionPerServer must be greater than 0!");
				}
				HTTPManager.maxConnectionPerServer = value;
			}
		} = 6;

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x00091E68 File Offset: 0x00090068
		// (set) Token: 0x06000D3E RID: 3390 RVA: 0x00091E6F File Offset: 0x0009006F
		public static bool KeepAliveDefaultValue { get; set; } = true;

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x00091E77 File Offset: 0x00090077
		// (set) Token: 0x06000D40 RID: 3392 RVA: 0x00091E7E File Offset: 0x0009007E
		public static bool IsCachingDisabled { get; set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x00091E86 File Offset: 0x00090086
		// (set) Token: 0x06000D42 RID: 3394 RVA: 0x00091E8D File Offset: 0x0009008D
		public static TimeSpan MaxConnectionIdleTime { get; set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00091E95 File Offset: 0x00090095
		// (set) Token: 0x06000D44 RID: 3396 RVA: 0x00091E9C File Offset: 0x0009009C
		public static bool IsCookiesEnabled { get; set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x00091EA4 File Offset: 0x000900A4
		// (set) Token: 0x06000D46 RID: 3398 RVA: 0x00091EAB File Offset: 0x000900AB
		public static uint CookieJarSize { get; set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000D47 RID: 3399 RVA: 0x00091EB3 File Offset: 0x000900B3
		// (set) Token: 0x06000D48 RID: 3400 RVA: 0x00091EBA File Offset: 0x000900BA
		public static bool EnablePrivateBrowsing { get; set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x00091EC2 File Offset: 0x000900C2
		// (set) Token: 0x06000D4A RID: 3402 RVA: 0x00091EC9 File Offset: 0x000900C9
		public static TimeSpan ConnectTimeout { get; set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00091ED1 File Offset: 0x000900D1
		// (set) Token: 0x06000D4C RID: 3404 RVA: 0x00091ED8 File Offset: 0x000900D8
		public static TimeSpan RequestTimeout { get; set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000D4D RID: 3405 RVA: 0x00091EE0 File Offset: 0x000900E0
		// (set) Token: 0x06000D4E RID: 3406 RVA: 0x00091EE7 File Offset: 0x000900E7
		public static Func<string> RootCacheFolderProvider { get; set; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000D4F RID: 3407 RVA: 0x00091EEF File Offset: 0x000900EF
		// (set) Token: 0x06000D50 RID: 3408 RVA: 0x00091EF6 File Offset: 0x000900F6
		public static Proxy Proxy { get; set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000D51 RID: 3409 RVA: 0x00091EFE File Offset: 0x000900FE
		public static HeartbeatManager Heartbeats
		{
			get
			{
				if (HTTPManager.heartbeats == null)
				{
					HTTPManager.heartbeats = new HeartbeatManager();
				}
				return HTTPManager.heartbeats;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000D52 RID: 3410 RVA: 0x00091F16 File Offset: 0x00090116
		// (set) Token: 0x06000D53 RID: 3411 RVA: 0x00091F39 File Offset: 0x00090139
		public static BestHTTP.Logger.ILogger Logger
		{
			get
			{
				if (HTTPManager.logger == null)
				{
					HTTPManager.logger = new DefaultLogger();
					HTTPManager.logger.Level = Loglevels.None;
				}
				return HTTPManager.logger;
			}
			set
			{
				HTTPManager.logger = value;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x00091F41 File Offset: 0x00090141
		// (set) Token: 0x06000D55 RID: 3413 RVA: 0x00091F48 File Offset: 0x00090148
		public static ICertificateVerifyer DefaultCertificateVerifyer { get; set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x00091F50 File Offset: 0x00090150
		// (set) Token: 0x06000D57 RID: 3415 RVA: 0x00091F57 File Offset: 0x00090157
		public static IClientCredentialsProvider DefaultClientCredentialsProvider { get; set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x00091F5F File Offset: 0x0009015F
		// (set) Token: 0x06000D59 RID: 3417 RVA: 0x00091F66 File Offset: 0x00090166
		public static bool UseAlternateSSLDefaultValue { get; set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00091F6E File Offset: 0x0009016E
		// (set) Token: 0x06000D5B RID: 3419 RVA: 0x00091F75 File Offset: 0x00090175
		public static Func<HTTPRequest, X509Certificate, X509Chain, bool> DefaultCertificationValidator { get; set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x00091F7D File Offset: 0x0009017D
		// (set) Token: 0x06000D5D RID: 3421 RVA: 0x00091F84 File Offset: 0x00090184
		internal static int MaxPathLength { get; set; } = 255;

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00091F8C File Offset: 0x0009018C
		// (set) Token: 0x06000D5F RID: 3423 RVA: 0x00091F93 File Offset: 0x00090193
		internal static bool IsQuitting { get; private set; }

		// Token: 0x06000D60 RID: 3424 RVA: 0x00091F9B File Offset: 0x0009019B
		public static void Setup()
		{
			HTTPUpdateDelegator.CheckInstance();
			HTTPCacheService.CheckSetup();
			CookieJar.SetupFolder();
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00091FAC File Offset: 0x000901AC
		public static HTTPRequest SendRequest(string url, OnRequestFinishedDelegate callback)
		{
			return HTTPManager.SendRequest(new HTTPRequest(new Uri(url), HTTPMethods.Get, callback));
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00091FC0 File Offset: 0x000901C0
		public static HTTPRequest SendRequest(string url, HTTPMethods methodType, OnRequestFinishedDelegate callback)
		{
			return HTTPManager.SendRequest(new HTTPRequest(new Uri(url), methodType, callback));
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00091FD4 File Offset: 0x000901D4
		public static HTTPRequest SendRequest(string url, HTTPMethods methodType, bool isKeepAlive, OnRequestFinishedDelegate callback)
		{
			return HTTPManager.SendRequest(new HTTPRequest(new Uri(url), methodType, isKeepAlive, callback));
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00091FE9 File Offset: 0x000901E9
		public static HTTPRequest SendRequest(string url, HTTPMethods methodType, bool isKeepAlive, bool disableCache, OnRequestFinishedDelegate callback)
		{
			return HTTPManager.SendRequest(new HTTPRequest(new Uri(url), methodType, isKeepAlive, disableCache, callback));
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00092000 File Offset: 0x00090200
		public static HTTPRequest SendRequest(HTTPRequest request)
		{
			object locker = HTTPManager.Locker;
			lock (locker)
			{
				HTTPManager.Setup();
				if (HTTPManager.IsCallingCallbacks)
				{
					request.State = HTTPRequestStates.Queued;
					HTTPManager.RequestQueue.Add(request);
				}
				else
				{
					HTTPManager.SendRequestImpl(request);
				}
			}
			return request;
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00092064 File Offset: 0x00090264
		public static GeneralStatistics GetGeneralStatistics(StatisticsQueryFlags queryFlags)
		{
			GeneralStatistics result = default(GeneralStatistics);
			result.QueryFlags = queryFlags;
			if ((queryFlags & StatisticsQueryFlags.Connections) != (StatisticsQueryFlags)0)
			{
				int num = 0;
				foreach (KeyValuePair<string, List<ConnectionBase>> keyValuePair in HTTPManager.Connections)
				{
					if (keyValuePair.Value != null)
					{
						num += keyValuePair.Value.Count;
					}
				}
				result.Connections = num;
				result.ActiveConnections = HTTPManager.ActiveConnections.Count;
				result.FreeConnections = HTTPManager.FreeConnections.Count;
				result.RecycledConnections = HTTPManager.RecycledConnections.Count;
				result.RequestsInQueue = HTTPManager.RequestQueue.Count;
			}
			if ((queryFlags & StatisticsQueryFlags.Cache) != (StatisticsQueryFlags)0)
			{
				result.CacheEntityCount = HTTPCacheService.GetCacheEntityCount();
				result.CacheSize = HTTPCacheService.GetCacheSize();
			}
			if ((queryFlags & StatisticsQueryFlags.Cookies) != (StatisticsQueryFlags)0)
			{
				List<Cookie> all = CookieJar.GetAll();
				result.CookieCount = all.Count;
				uint num2 = 0U;
				for (int i = 0; i < all.Count; i++)
				{
					num2 += all[i].GuessSize();
				}
				result.CookieJarSize = num2;
			}
			return result;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0009219C File Offset: 0x0009039C
		private static void SendRequestImpl(HTTPRequest request)
		{
			ConnectionBase conn = HTTPManager.FindOrCreateFreeConnection(request);
			if (conn != null)
			{
				if (HTTPManager.ActiveConnections.Find((ConnectionBase c) => c == conn) == null)
				{
					HTTPManager.ActiveConnections.Add(conn);
				}
				HTTPManager.FreeConnections.Remove(conn);
				request.State = HTTPRequestStates.Processing;
				request.Prepare();
				conn.Process(request);
				return;
			}
			request.State = HTTPRequestStates.Queued;
			HTTPManager.RequestQueue.Add(request);
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x00092228 File Offset: 0x00090428
		private static string GetKeyForRequest(HTTPRequest request)
		{
			if (request.CurrentUri.IsFile)
			{
				return request.CurrentUri.ToString();
			}
			return ((request.Proxy != null) ? new UriBuilder(request.Proxy.Address.Scheme, request.Proxy.Address.Host, request.Proxy.Address.Port).Uri.ToString() : string.Empty) + new UriBuilder(request.CurrentUri.Scheme, request.CurrentUri.Host, request.CurrentUri.Port).Uri.ToString();
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x000922D1 File Offset: 0x000904D1
		private static ConnectionBase CreateConnection(HTTPRequest request, string serverUrl)
		{
			if (request.CurrentUri.IsFile && Application.platform != RuntimePlatform.WebGLPlayer)
			{
				return new FileConnection(serverUrl);
			}
			return new HTTPConnection(serverUrl);
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x000922F8 File Offset: 0x000904F8
		private static ConnectionBase FindOrCreateFreeConnection(HTTPRequest request)
		{
			ConnectionBase connectionBase = null;
			string keyForRequest = HTTPManager.GetKeyForRequest(request);
			List<ConnectionBase> list;
			if (HTTPManager.Connections.TryGetValue(keyForRequest, out list))
			{
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsActive)
					{
						num++;
					}
				}
				if (num <= (int)HTTPManager.MaxConnectionPerServer)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (connectionBase != null)
						{
							break;
						}
						ConnectionBase connectionBase2 = list[j];
						if (connectionBase2 != null && connectionBase2.IsFree && (!connectionBase2.HasProxy || connectionBase2.LastProcessedUri == null || connectionBase2.LastProcessedUri.Host.Equals(request.CurrentUri.Host, StringComparison.OrdinalIgnoreCase)))
						{
							connectionBase = connectionBase2;
						}
					}
				}
			}
			else
			{
				HTTPManager.Connections.Add(keyForRequest, list = new List<ConnectionBase>((int)HTTPManager.MaxConnectionPerServer));
			}
			if (connectionBase == null)
			{
				if (list.Count >= (int)HTTPManager.MaxConnectionPerServer)
				{
					return null;
				}
				list.Add(connectionBase = HTTPManager.CreateConnection(request, keyForRequest));
			}
			return connectionBase;
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x000923FC File Offset: 0x000905FC
		private static bool CanProcessFromQueue()
		{
			for (int i = 0; i < HTTPManager.RequestQueue.Count; i++)
			{
				if (HTTPManager.FindOrCreateFreeConnection(HTTPManager.RequestQueue[i]) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00092433 File Offset: 0x00090633
		private static void RecycleConnection(ConnectionBase conn)
		{
			conn.Recycle(new HTTPConnectionRecycledDelegate(HTTPManager.OnConnectionRecylced));
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00092448 File Offset: 0x00090648
		private static void OnConnectionRecylced(ConnectionBase conn)
		{
			List<ConnectionBase> recycledConnections = HTTPManager.RecycledConnections;
			lock (recycledConnections)
			{
				HTTPManager.RecycledConnections.Add(conn);
			}
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0009248C File Offset: 0x0009068C
		internal static ConnectionBase GetConnectionWith(HTTPRequest request)
		{
			object locker = HTTPManager.Locker;
			ConnectionBase result;
			lock (locker)
			{
				for (int i = 0; i < HTTPManager.ActiveConnections.Count; i++)
				{
					ConnectionBase connectionBase = HTTPManager.ActiveConnections[i];
					if (connectionBase.CurrentRequest == request)
					{
						return connectionBase;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x000924FC File Offset: 0x000906FC
		internal static bool RemoveFromQueue(HTTPRequest request)
		{
			return HTTPManager.RequestQueue.Remove(request);
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0009250C File Offset: 0x0009070C
		internal static string GetRootCacheFolder()
		{
			try
			{
				if (HTTPManager.RootCacheFolderProvider != null)
				{
					return HTTPManager.RootCacheFolderProvider();
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("HTTPManager", "GetRootCacheFolder", ex);
			}
			return Application.persistentDataPath;
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x00092560 File Offset: 0x00090760
		public static void OnUpdate()
		{
			if (Monitor.TryEnter(HTTPManager.Locker))
			{
				try
				{
					HTTPManager.IsCallingCallbacks = true;
					try
					{
						int i = 0;
						while (i < HTTPManager.ActiveConnections.Count)
						{
							ConnectionBase connectionBase = HTTPManager.ActiveConnections[i];
							switch (connectionBase.State)
							{
							case HTTPConnectionStates.Processing:
								goto IL_60;
							case HTTPConnectionStates.Redirected:
								goto IL_185;
							case HTTPConnectionStates.Upgraded:
								connectionBase.HandleCallback();
								break;
							case HTTPConnectionStates.WaitForProtocolShutdown:
							{
								IProtocol protocol = connectionBase.CurrentRequest.Response as IProtocol;
								if (protocol != null)
								{
									protocol.HandleEvents();
								}
								if (protocol == null || protocol.IsClosed)
								{
									connectionBase.HandleCallback();
									connectionBase.Dispose();
									HTTPManager.RecycleConnection(connectionBase);
								}
								break;
							}
							case HTTPConnectionStates.WaitForRecycle:
								connectionBase.CurrentRequest.FinishStreaming();
								connectionBase.HandleCallback();
								HTTPManager.RecycleConnection(connectionBase);
								break;
							case HTTPConnectionStates.Free:
								HTTPManager.RecycleConnection(connectionBase);
								break;
							case HTTPConnectionStates.AbortRequested:
							{
								IProtocol protocol = connectionBase.CurrentRequest.Response as IProtocol;
								if (protocol != null)
								{
									protocol.HandleEvents();
									if (protocol.IsClosed)
									{
										connectionBase.HandleCallback();
										connectionBase.Dispose();
										HTTPManager.RecycleConnection(connectionBase);
									}
								}
								if (connectionBase.CurrentRequest != null)
								{
									goto IL_60;
								}
								break;
							}
							case HTTPConnectionStates.TimedOut:
								goto IL_108;
							case HTTPConnectionStates.Closed:
								connectionBase.CurrentRequest.FinishStreaming();
								connectionBase.HandleCallback();
								HTTPManager.RecycleConnection(connectionBase);
								break;
							}
							IL_25C:
							i++;
							continue;
							IL_60:
							connectionBase.HandleProgressCallback();
							if (connectionBase.CurrentRequest.UseStreaming && connectionBase.CurrentRequest.Response != null && connectionBase.CurrentRequest.Response.HasStreamedFragments())
							{
								connectionBase.HandleCallback();
							}
							try
							{
								if (((!connectionBase.CurrentRequest.UseStreaming && connectionBase.CurrentRequest.UploadStream == null) || connectionBase.CurrentRequest.EnableTimoutForStreaming) && DateTime.UtcNow - connectionBase.StartTime > connectionBase.CurrentRequest.Timeout)
								{
									connectionBase.Abort(HTTPConnectionStates.TimedOut);
								}
								goto IL_25C;
							}
							catch (OverflowException)
							{
								HTTPManager.Logger.Warning("HTTPManager", "TimeSpan overflow");
								goto IL_25C;
							}
							IL_108:
							try
							{
								if (DateTime.UtcNow - connectionBase.TimedOutStart > TimeSpan.FromMilliseconds(500.0))
								{
									HTTPManager.Logger.Information("HTTPManager", "Hard aborting connection because of a long waiting TimedOut state");
									connectionBase.CurrentRequest.Response = null;
									connectionBase.CurrentRequest.State = HTTPRequestStates.TimedOut;
									connectionBase.HandleCallback();
									HTTPManager.RecycleConnection(connectionBase);
								}
								goto IL_25C;
							}
							catch (OverflowException)
							{
								HTTPManager.Logger.Warning("HTTPManager", "TimeSpan overflow");
								goto IL_25C;
							}
							IL_185:
							HTTPManager.SendRequest(connectionBase.CurrentRequest);
							HTTPManager.RecycleConnection(connectionBase);
							goto IL_25C;
						}
					}
					finally
					{
						HTTPManager.IsCallingCallbacks = false;
					}
					if (Monitor.TryEnter(HTTPManager.RecycledConnections))
					{
						try
						{
							if (HTTPManager.RecycledConnections.Count > 0)
							{
								for (int j = 0; j < HTTPManager.RecycledConnections.Count; j++)
								{
									ConnectionBase connectionBase2 = HTTPManager.RecycledConnections[j];
									if (connectionBase2.IsFree)
									{
										HTTPManager.ActiveConnections.Remove(connectionBase2);
										HTTPManager.FreeConnections.Add(connectionBase2);
									}
								}
								HTTPManager.RecycledConnections.Clear();
							}
						}
						finally
						{
							Monitor.Exit(HTTPManager.RecycledConnections);
						}
					}
					if (HTTPManager.FreeConnections.Count > 0)
					{
						for (int k = 0; k < HTTPManager.FreeConnections.Count; k++)
						{
							ConnectionBase connectionBase3 = HTTPManager.FreeConnections[k];
							if (connectionBase3.IsRemovable)
							{
								List<ConnectionBase> list = null;
								if (HTTPManager.Connections.TryGetValue(connectionBase3.ServerAddress, out list))
								{
									list.Remove(connectionBase3);
								}
								connectionBase3.Dispose();
								HTTPManager.FreeConnections.RemoveAt(k);
								k--;
							}
						}
					}
					if (HTTPManager.CanProcessFromQueue())
					{
						if (HTTPManager.RequestQueue.Find((HTTPRequest req) => req.Priority != 0) != null)
						{
							HTTPManager.RequestQueue.Sort((HTTPRequest req1, HTTPRequest req2) => req1.Priority - req2.Priority);
						}
						HTTPRequest[] array = HTTPManager.RequestQueue.ToArray();
						HTTPManager.RequestQueue.Clear();
						for (int l = 0; l < array.Length; l++)
						{
							HTTPManager.SendRequest(array[l]);
						}
					}
				}
				finally
				{
					Monitor.Exit(HTTPManager.Locker);
				}
			}
			if (HTTPManager.heartbeats != null)
			{
				HTTPManager.heartbeats.Update();
			}
			VariableSizedBufferPool.Maintain();
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00092A08 File Offset: 0x00090C08
		public static void OnQuit()
		{
			object locker = HTTPManager.Locker;
			lock (locker)
			{
				HTTPManager.IsQuitting = true;
				HTTPCacheService.SaveLibrary();
				CookieJar.Persist();
				HTTPManager.AbortAll(true);
				HTTPManager.OnUpdate();
			}
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00092A5C File Offset: 0x00090C5C
		public static void AbortAll(bool allowCallbacks = false)
		{
			object locker = HTTPManager.Locker;
			lock (locker)
			{
				HTTPRequest[] array = HTTPManager.RequestQueue.ToArray();
				HTTPManager.RequestQueue.Clear();
				foreach (HTTPRequest httprequest in array)
				{
					try
					{
						if (!allowCallbacks)
						{
							httprequest.Callback = null;
						}
						httprequest.Abort();
					}
					catch
					{
					}
				}
				foreach (KeyValuePair<string, List<ConnectionBase>> keyValuePair in HTTPManager.Connections)
				{
					foreach (ConnectionBase connectionBase in keyValuePair.Value)
					{
						try
						{
							if (connectionBase.CurrentRequest != null)
							{
								if (!allowCallbacks)
								{
									connectionBase.CurrentRequest.Callback = null;
								}
								connectionBase.CurrentRequest.State = HTTPRequestStates.Aborted;
							}
							connectionBase.Abort(HTTPConnectionStates.Closed);
							connectionBase.Dispose();
						}
						catch
						{
						}
					}
					keyValuePair.Value.Clear();
				}
				HTTPManager.Connections.Clear();
			}
		}

		// Token: 0x040012B0 RID: 4784
		private static byte maxConnectionPerServer;

		// Token: 0x040012BB RID: 4795
		private static HeartbeatManager heartbeats;

		// Token: 0x040012BC RID: 4796
		private static BestHTTP.Logger.ILogger logger;

		// Token: 0x040012C1 RID: 4801
		public static bool TryToMinimizeTCPLatency = true;

		// Token: 0x040012C2 RID: 4802
		public static int SendBufferSize = 66560;

		// Token: 0x040012C3 RID: 4803
		public static int ReceiveBufferSize = 66560;

		// Token: 0x040012C4 RID: 4804
		public static IIOService IOService;

		// Token: 0x040012C6 RID: 4806
		public static string UserAgent = "BestHTTP 1.12.1";

		// Token: 0x040012C7 RID: 4807
		private static Dictionary<string, List<ConnectionBase>> Connections = new Dictionary<string, List<ConnectionBase>>();

		// Token: 0x040012C8 RID: 4808
		private static List<ConnectionBase> ActiveConnections = new List<ConnectionBase>();

		// Token: 0x040012C9 RID: 4809
		private static List<ConnectionBase> FreeConnections = new List<ConnectionBase>();

		// Token: 0x040012CA RID: 4810
		private static List<ConnectionBase> RecycledConnections = new List<ConnectionBase>();

		// Token: 0x040012CB RID: 4811
		private static List<HTTPRequest> RequestQueue = new List<HTTPRequest>();

		// Token: 0x040012CC RID: 4812
		private static bool IsCallingCallbacks;

		// Token: 0x040012CD RID: 4813
		internal static object Locker = new object();
	}
}
