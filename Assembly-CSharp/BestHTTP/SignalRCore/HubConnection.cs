using System;
using System.Collections.Generic;
using System.Threading;
using BestHTTP.Extensions;
using BestHTTP.Futures;
using BestHTTP.SignalRCore.Authentication;
using BestHTTP.SignalRCore.Messages;
using BestHTTP.SignalRCore.Transports;

namespace BestHTTP.SignalRCore
{
	// Token: 0x020001E6 RID: 486
	public sealed class HubConnection : IHeartbeat
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060011DD RID: 4573 RVA: 0x000A2CA2 File Offset: 0x000A0EA2
		// (set) Token: 0x060011DE RID: 4574 RVA: 0x000A2CAA File Offset: 0x000A0EAA
		public Uri Uri { get; private set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060011DF RID: 4575 RVA: 0x000A2CB3 File Offset: 0x000A0EB3
		// (set) Token: 0x060011E0 RID: 4576 RVA: 0x000A2CBB File Offset: 0x000A0EBB
		public ConnectionStates State { get; private set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060011E1 RID: 4577 RVA: 0x000A2CC4 File Offset: 0x000A0EC4
		// (set) Token: 0x060011E2 RID: 4578 RVA: 0x000A2CCC File Offset: 0x000A0ECC
		public ITransport Transport { get; private set; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060011E3 RID: 4579 RVA: 0x000A2CD5 File Offset: 0x000A0ED5
		// (set) Token: 0x060011E4 RID: 4580 RVA: 0x000A2CDD File Offset: 0x000A0EDD
		public IProtocol Protocol { get; private set; }

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060011E5 RID: 4581 RVA: 0x000A2CE8 File Offset: 0x000A0EE8
		// (remove) Token: 0x060011E6 RID: 4582 RVA: 0x000A2D20 File Offset: 0x000A0F20
		public event Action<HubConnection, Uri, Uri> OnRedirected;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060011E7 RID: 4583 RVA: 0x000A2D58 File Offset: 0x000A0F58
		// (remove) Token: 0x060011E8 RID: 4584 RVA: 0x000A2D90 File Offset: 0x000A0F90
		public event Action<HubConnection> OnConnected;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060011E9 RID: 4585 RVA: 0x000A2DC8 File Offset: 0x000A0FC8
		// (remove) Token: 0x060011EA RID: 4586 RVA: 0x000A2E00 File Offset: 0x000A1000
		public event Action<HubConnection, string> OnError;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060011EB RID: 4587 RVA: 0x000A2E38 File Offset: 0x000A1038
		// (remove) Token: 0x060011EC RID: 4588 RVA: 0x000A2E70 File Offset: 0x000A1070
		public event Action<HubConnection> OnClosed;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060011ED RID: 4589 RVA: 0x000A2EA8 File Offset: 0x000A10A8
		// (remove) Token: 0x060011EE RID: 4590 RVA: 0x000A2EE0 File Offset: 0x000A10E0
		public event Func<HubConnection, Message, bool> OnMessage;

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060011EF RID: 4591 RVA: 0x000A2F15 File Offset: 0x000A1115
		// (set) Token: 0x060011F0 RID: 4592 RVA: 0x000A2F1D File Offset: 0x000A111D
		public IAuthenticationProvider AuthenticationProvider { get; set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x000A2F26 File Offset: 0x000A1126
		// (set) Token: 0x060011F2 RID: 4594 RVA: 0x000A2F2E File Offset: 0x000A112E
		public NegotiationResult NegotiationResult { get; private set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060011F3 RID: 4595 RVA: 0x000A2F37 File Offset: 0x000A1137
		// (set) Token: 0x060011F4 RID: 4596 RVA: 0x000A2F3F File Offset: 0x000A113F
		public HubOptions Options { get; private set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060011F5 RID: 4597 RVA: 0x000A2F48 File Offset: 0x000A1148
		// (set) Token: 0x060011F6 RID: 4598 RVA: 0x000A2F50 File Offset: 0x000A1150
		public int RedirectCount { get; private set; }

		// Token: 0x060011F7 RID: 4599 RVA: 0x000A2F59 File Offset: 0x000A1159
		public HubConnection(Uri hubUri, IProtocol protocol) : this(hubUri, protocol, new HubOptions())
		{
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x000A2F68 File Offset: 0x000A1168
		public HubConnection(Uri hubUri, IProtocol protocol, HubOptions options)
		{
			this.Uri = hubUri;
			this.State = ConnectionStates.Initial;
			this.Options = options;
			this.Protocol = protocol;
			this.Protocol.Connection = this;
			this.AuthenticationProvider = new DefaultAccessTokenAuthenticator(this);
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x000A2FCC File Offset: 0x000A11CC
		public void StartConnect()
		{
			if (this.State != ConnectionStates.Initial && this.State != ConnectionStates.Redirected)
			{
				HTTPManager.Logger.Warning("HubConnection", "StartConnect - Expected Initial or Redirected state, got " + this.State.ToString());
				return;
			}
			HTTPManager.Logger.Verbose("HubConnection", "StartConnect");
			if (this.AuthenticationProvider != null && this.AuthenticationProvider.IsPreAuthRequired)
			{
				HTTPManager.Logger.Information("HubConnection", "StartConnect - Authenticating");
				this.SetState(ConnectionStates.Authenticating, null);
				this.AuthenticationProvider.OnAuthenticationSucceded += this.OnAuthenticationSucceded;
				this.AuthenticationProvider.OnAuthenticationFailed += this.OnAuthenticationFailed;
				this.AuthenticationProvider.StartAuthentication();
				return;
			}
			this.StartNegotiation();
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x000A30A0 File Offset: 0x000A12A0
		private void OnAuthenticationSucceded(IAuthenticationProvider provider)
		{
			HTTPManager.Logger.Verbose("HubConnection", "OnAuthenticationSucceded");
			this.AuthenticationProvider.OnAuthenticationSucceded -= this.OnAuthenticationSucceded;
			this.AuthenticationProvider.OnAuthenticationFailed -= this.OnAuthenticationFailed;
			this.StartNegotiation();
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x000A30F8 File Offset: 0x000A12F8
		private void OnAuthenticationFailed(IAuthenticationProvider provider, string reason)
		{
			HTTPManager.Logger.Error("HubConnection", "OnAuthenticationFailed: " + reason);
			this.AuthenticationProvider.OnAuthenticationSucceded -= this.OnAuthenticationSucceded;
			this.AuthenticationProvider.OnAuthenticationFailed -= this.OnAuthenticationFailed;
			this.SetState(ConnectionStates.Closed, reason);
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x000A3158 File Offset: 0x000A1358
		private void StartNegotiation()
		{
			HTTPManager.Logger.Verbose("HubConnection", "StartNegotiation");
			if (this.State == ConnectionStates.CloseInitiated)
			{
				this.SetState(ConnectionStates.Closed, null);
				return;
			}
			if (this.Options.SkipNegotiation)
			{
				HTTPManager.Logger.Verbose("HubConnection", "Skipping negotiation");
				this.ConnectImpl();
				return;
			}
			this.SetState(ConnectionStates.Negotiating, null);
			UriBuilder uriBuilder = new UriBuilder(this.Uri);
			if (uriBuilder.Path.EndsWith("/"))
			{
				UriBuilder uriBuilder2 = uriBuilder;
				uriBuilder2.Path += "negotiate";
			}
			else
			{
				UriBuilder uriBuilder3 = uriBuilder;
				uriBuilder3.Path += "/negotiate";
			}
			HTTPRequest httprequest = new HTTPRequest(uriBuilder.Uri, HTTPMethods.Post, new OnRequestFinishedDelegate(this.OnNegotiationRequestFinished));
			if (this.AuthenticationProvider != null)
			{
				this.AuthenticationProvider.PrepareRequest(httprequest);
			}
			httprequest.Send();
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x000A323C File Offset: 0x000A143C
		private void ConnectImpl()
		{
			HTTPManager.Logger.Verbose("HubConnection", "ConnectImpl");
			if (this.Options.PreferedTransport == TransportTypes.WebSocket)
			{
				if (this.NegotiationResult != null && !this.IsTransportSupported("WebSockets"))
				{
					this.SetState(ConnectionStates.Closed, "The 'WebSockets' transport isn't supported by the server!");
					return;
				}
				this.Transport = new WebSocketTransport(this);
				this.Transport.OnStateChanged += this.Transport_OnStateChanged;
			}
			else
			{
				this.SetState(ConnectionStates.Closed, "Unsupportted transport: " + this.Options.PreferedTransport);
			}
			this.Transport.StartConnect();
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x000A32E0 File Offset: 0x000A14E0
		private bool IsTransportSupported(string transportName)
		{
			if (this.NegotiationResult.SupportedTransports == null)
			{
				return true;
			}
			for (int i = 0; i < this.NegotiationResult.SupportedTransports.Count; i++)
			{
				if (this.NegotiationResult.SupportedTransports[i].Name == transportName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x000A3338 File Offset: 0x000A1538
		private void OnNegotiationRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			if (this.State == ConnectionStates.CloseInitiated)
			{
				this.SetState(ConnectionStates.Closed, null);
				return;
			}
			string text = null;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess)
				{
					HTTPManager.Logger.Information("HubConnection", "Negotiation Request Finished Successfully! Response: " + resp.DataAsText);
					this.NegotiationResult = NegotiationResult.Parse(resp.DataAsText, out text, this);
					if (string.IsNullOrEmpty(text))
					{
						if (this.NegotiationResult.Url != null)
						{
							this.SetState(ConnectionStates.Redirected, null);
							int num = this.RedirectCount + 1;
							this.RedirectCount = num;
							if (num >= this.Options.MaxRedirects)
							{
								text = string.Format("MaxRedirects ({0:N0}) reached!", this.Options.MaxRedirects);
							}
							else
							{
								Uri uri = this.Uri;
								this.Uri = this.NegotiationResult.Url;
								if (this.OnRedirected != null)
								{
									try
									{
										this.OnRedirected(this, uri, this.Uri);
									}
									catch (Exception ex)
									{
										HTTPManager.Logger.Exception("HubConnection", "OnNegotiationRequestFinished - OnRedirected", ex);
									}
								}
								this.StartConnect();
							}
						}
						else
						{
							this.ConnectImpl();
						}
					}
				}
				else
				{
					text = string.Format("Negotiation Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText);
				}
				break;
			case HTTPRequestStates.Error:
				text = "Negotiation Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.Aborted:
				text = "Negotiation Request Aborted!";
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = "Negotiation Request - Connection Timed Out!";
				break;
			case HTTPRequestStates.TimedOut:
				text = "Negotiation Request - Processing the request Timed Out!";
				break;
			}
			if (text != null)
			{
				this.SetState(ConnectionStates.Closed, text);
			}
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x000A3518 File Offset: 0x000A1718
		public void StartClose()
		{
			HTTPManager.Logger.Verbose("HubConnection", "StartClose");
			this.SetState(ConnectionStates.CloseInitiated, null);
			if (this.Transport != null)
			{
				this.Transport.StartClose();
			}
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x000A354C File Offset: 0x000A174C
		public IFuture<StreamItemContainer<TResult>> Stream<TResult>(string target, params object[] args)
		{
			Future<StreamItemContainer<TResult>> future = new Future<StreamItemContainer<TResult>>();
			long id = this.InvokeImp(target, args, delegate(Message message)
			{
				MessageTypes type = message.type;
				if (type != MessageTypes.StreamItem)
				{
					if (type != MessageTypes.Completion)
					{
						return;
					}
					if (string.IsNullOrEmpty(message.error))
					{
						StreamItemContainer<TResult> value = future.value;
						if (!value.IsCanceled && message.result != null)
						{
							TResult item = (TResult)((object)this.Protocol.ConvertTo(typeof(TResult), message.result));
							value.AddItem(item);
						}
						future.Assign(value);
						return;
					}
					future.Fail(new Exception(message.error));
				}
				else
				{
					StreamItemContainer<TResult> value2 = future.value;
					if (!value2.IsCanceled)
					{
						value2.AddItem((TResult)((object)this.Protocol.ConvertTo(typeof(TResult), message.item)));
						future.AssignItem(value2);
						return;
					}
				}
			}, true);
			future.BeginProcess(new StreamItemContainer<TResult>(id));
			return future;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x000A35A0 File Offset: 0x000A17A0
		public void CancelStream<T>(StreamItemContainer<T> container)
		{
			Message message = new Message
			{
				type = MessageTypes.CancelInvocation,
				invocationId = container.id.ToString()
			};
			container.IsCanceled = true;
			this.SendMessage(message);
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x000A35E4 File Offset: 0x000A17E4
		public IFuture<TResult> Invoke<TResult>(string target, params object[] args)
		{
			Future<TResult> future = new Future<TResult>();
			this.InvokeImp(target, args, delegate(Message message)
			{
				if (string.IsNullOrEmpty(message.error))
				{
					future.Assign((TResult)((object)this.Protocol.ConvertTo(typeof(TResult), message.result)));
					return;
				}
				future.Fail(new Exception(message.error));
			}, false);
			return future;
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x000A3628 File Offset: 0x000A1828
		public IFuture<bool> Send(string target, params object[] args)
		{
			Future<bool> future = new Future<bool>();
			this.InvokeImp(target, args, delegate(Message message)
			{
				if (string.IsNullOrEmpty(message.error))
				{
					future.Assign(true);
					return;
				}
				future.Fail(new Exception(message.error));
			}, false);
			return future;
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x000A3664 File Offset: 0x000A1864
		private long InvokeImp(string target, object[] args, Action<Message> callback, bool isStreamingInvocation = false)
		{
			if (this.State != ConnectionStates.Connected)
			{
				return -1L;
			}
			long num = Interlocked.Increment(ref this.lastInvocationId);
			Message message = new Message
			{
				type = (isStreamingInvocation ? MessageTypes.StreamInvocation : MessageTypes.Invocation),
				invocationId = num.ToString(),
				target = target,
				arguments = args,
				nonblocking = (callback == null)
			};
			this.SendMessage(message);
			if (callback != null)
			{
				this.invocations.Add(num, callback);
			}
			return num;
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x000A36E4 File Offset: 0x000A18E4
		internal void SendMessage(Message message)
		{
			byte[] msg = this.Protocol.EncodeMessage(message);
			this.Transport.Send(msg);
			this.lastMessageSent = DateTime.UtcNow;
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x000A3718 File Offset: 0x000A1918
		internal UploadItemController<StreamItemContainer<TResult>> UploadStreamWithDownStream<TResult>(string target, int paramCount)
		{
			Future<StreamItemContainer<TResult>> future = new Future<StreamItemContainer<TResult>>();
			Action<Message> value = delegate(Message message)
			{
				MessageTypes type = message.type;
				if (type != MessageTypes.StreamItem)
				{
					if (type != MessageTypes.Completion)
					{
						return;
					}
					if (string.IsNullOrEmpty(message.error))
					{
						StreamItemContainer<TResult> value2 = future.value;
						if (!value2.IsCanceled && message.result != null)
						{
							TResult item = (TResult)((object)this.Protocol.ConvertTo(typeof(TResult), message.result));
							value2.AddItem(item);
						}
						future.Assign(value2);
						return;
					}
					future.Fail(new Exception(message.error));
				}
				else
				{
					StreamItemContainer<TResult> value3 = future.value;
					if (!value3.IsCanceled)
					{
						value3.AddItem((TResult)((object)this.Protocol.ConvertTo(typeof(TResult), message.item)));
						future.AssignItem(value3);
						return;
					}
				}
			};
			long num = Interlocked.Increment(ref this.lastInvocationId);
			int[] array = new int[paramCount];
			for (int i = 0; i < paramCount; i++)
			{
				array[i] = Interlocked.Increment(ref this.lastStreamId);
			}
			UploadItemController<StreamItemContainer<TResult>> result = new UploadItemController<StreamItemContainer<TResult>>(this, num, array, future);
			Message message2 = new Message
			{
				type = MessageTypes.StreamInvocation,
				invocationId = num.ToString(),
				target = target,
				arguments = new object[0],
				streamIds = array,
				nonblocking = false
			};
			this.SendMessage(message2);
			this.invocations.Add(num, value);
			future.BeginProcess(new StreamItemContainer<TResult>(num));
			return result;
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x000A37F8 File Offset: 0x000A19F8
		internal UploadItemController<TResult> Upload<TResult>(string target, int paramCount)
		{
			Future<TResult> future = new Future<TResult>();
			Action<Message> value = delegate(Message message)
			{
				if (string.IsNullOrEmpty(message.error))
				{
					future.Assign((TResult)((object)this.Protocol.ConvertTo(typeof(TResult), message.result)));
					return;
				}
				future.Fail(new Exception(message.error));
			};
			long num = Interlocked.Increment(ref this.lastInvocationId);
			int[] array = new int[paramCount];
			for (int i = 0; i < paramCount; i++)
			{
				array[i] = Interlocked.Increment(ref this.lastStreamId);
			}
			UploadItemController<TResult> result = new UploadItemController<TResult>(this, num, array, future);
			Message message2 = new Message
			{
				type = MessageTypes.Invocation,
				invocationId = num.ToString(),
				target = target,
				arguments = new object[0],
				streamIds = array,
				nonblocking = false
			};
			this.SendMessage(message2);
			this.invocations.Add(num, value);
			return result;
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x000A38C8 File Offset: 0x000A1AC8
		public void On(string methodName, Action callback)
		{
			this.On(methodName, null, delegate(object[] args)
			{
				callback();
			});
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x000A38F8 File Offset: 0x000A1AF8
		public void On<T1>(string methodName, Action<T1> callback)
		{
			this.On(methodName, new Type[]
			{
				typeof(T1)
			}, delegate(object[] args)
			{
				callback((T1)((object)args[0]));
			});
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x000A3938 File Offset: 0x000A1B38
		public void On<T1, T2>(string methodName, Action<T1, T2> callback)
		{
			this.On(methodName, new Type[]
			{
				typeof(T1),
				typeof(T2)
			}, delegate(object[] args)
			{
				callback((T1)((object)args[0]), (T2)((object)args[1]));
			});
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x000A3988 File Offset: 0x000A1B88
		public void On<T1, T2, T3>(string methodName, Action<T1, T2, T3> callback)
		{
			this.On(methodName, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3)
			}, delegate(object[] args)
			{
				callback((T1)((object)args[0]), (T2)((object)args[1]), (T3)((object)args[2]));
			});
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x000A39E4 File Offset: 0x000A1BE4
		public void On<T1, T2, T3, T4>(string methodName, Action<T1, T2, T3, T4> callback)
		{
			this.On(methodName, new Type[]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4)
			}, delegate(object[] args)
			{
				callback((T1)((object)args[0]), (T2)((object)args[1]), (T3)((object)args[2]), (T4)((object)args[3]));
			});
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x000A3A4C File Offset: 0x000A1C4C
		public void On(string methodName, Type[] paramTypes, Action<object[]> callback)
		{
			Subscription subscription = null;
			if (!this.subscriptions.TryGetValue(methodName, out subscription))
			{
				this.subscriptions.Add(methodName, subscription = new Subscription());
			}
			subscription.Add(paramTypes, callback);
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x000A3A88 File Offset: 0x000A1C88
		internal void OnMessages(List<Message> messages)
		{
			int i = 0;
			while (i < messages.Count)
			{
				Message message = messages[i];
				try
				{
					if (this.OnMessage != null && !this.OnMessage(this, message))
					{
						break;
					}
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("HubConnection", "Exception in OnMessage user code!", ex);
				}
				switch (message.type)
				{
				case MessageTypes.Invocation:
				{
					Subscription subscription = null;
					if (this.subscriptions.TryGetValue(message.target, out subscription))
					{
						for (int j = 0; j < subscription.callbacks.Count; j++)
						{
							CallbackDescriptor callbackDescriptor = subscription.callbacks[j];
							object[] obj = null;
							try
							{
								obj = this.Protocol.GetRealArguments(callbackDescriptor.ParamTypes, message.arguments);
							}
							catch (Exception ex2)
							{
								HTTPManager.Logger.Exception("HubConnection", "OnMessages - Invocation - GetRealArguments", ex2);
							}
							try
							{
								callbackDescriptor.Callback(obj);
							}
							catch (Exception ex3)
							{
								HTTPManager.Logger.Exception("HubConnection", "OnMessages - Invocation - Invoke", ex3);
							}
						}
					}
					break;
				}
				case MessageTypes.StreamItem:
				{
					long key;
					Action<Message> action;
					if (long.TryParse(message.invocationId, out key) && this.invocations.TryGetValue(key, out action) && action != null)
					{
						try
						{
							action(message);
							break;
						}
						catch (Exception ex4)
						{
							HTTPManager.Logger.Exception("HubConnection", "OnMessages - StreamItem - callback", ex4);
							break;
						}
						goto IL_178;
					}
					break;
				}
				case MessageTypes.Completion:
					goto IL_178;
				case MessageTypes.Close:
					this.SetState(ConnectionStates.Closed, message.error);
					break;
				}
				IL_1DD:
				i++;
				continue;
				IL_178:
				long key2;
				if (long.TryParse(message.invocationId, out key2))
				{
					Action<Message> action2;
					if (this.invocations.TryGetValue(key2, out action2) && action2 != null)
					{
						try
						{
							action2(message);
						}
						catch (Exception ex5)
						{
							HTTPManager.Logger.Exception("HubConnection", "OnMessages - Completion - callback", ex5);
						}
					}
					this.invocations.Remove(key2);
					goto IL_1DD;
				}
				goto IL_1DD;
			}
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x000A3CC4 File Offset: 0x000A1EC4
		private void Transport_OnStateChanged(TransportStates oldState, TransportStates newState)
		{
			HTTPManager.Logger.Verbose("HubConnection", string.Format("Transport_OnStateChanged - oldState: {0} newState: {1}", oldState.ToString(), newState.ToString()));
			switch (newState)
			{
			case TransportStates.Connected:
				this.SetState(ConnectionStates.Connected, null);
				return;
			case TransportStates.Closing:
				break;
			case TransportStates.Failed:
				this.SetState(ConnectionStates.Closed, this.Transport.ErrorReason);
				return;
			case TransportStates.Closed:
				this.SetState(ConnectionStates.Closed, null);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x000A3D44 File Offset: 0x000A1F44
		private void SetState(ConnectionStates state, string errorReason = null)
		{
			if (string.IsNullOrEmpty(errorReason))
			{
				HTTPManager.Logger.Information("HubConnection", string.Concat(new string[]
				{
					"SetState - from State: '",
					this.State.ToString(),
					"' to State: '",
					state.ToString(),
					"'"
				}));
			}
			else
			{
				HTTPManager.Logger.Information("HubConnection", string.Concat(new string[]
				{
					"SetState - from State: '",
					this.State.ToString(),
					"' to State: '",
					state.ToString(),
					"' errorReason: '",
					errorReason,
					"'"
				}));
			}
			if (this.State == state)
			{
				return;
			}
			this.State = state;
			switch (state)
			{
			case ConnectionStates.Initial:
			case ConnectionStates.Authenticating:
			case ConnectionStates.Negotiating:
			case ConnectionStates.Redirected:
			case ConnectionStates.CloseInitiated:
				break;
			case ConnectionStates.Connected:
				try
				{
					if (this.OnConnected != null)
					{
						this.OnConnected(this);
					}
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("HubConnection", "Exception in OnConnected user code!", ex);
				}
				HTTPManager.Heartbeats.Subscribe(this);
				this.lastMessageSent = DateTime.UtcNow;
				return;
			case ConnectionStates.Closed:
				if (string.IsNullOrEmpty(errorReason))
				{
					if (this.OnClosed == null)
					{
						goto IL_1A7;
					}
					try
					{
						this.OnClosed(this);
						goto IL_1A7;
					}
					catch (Exception ex2)
					{
						HTTPManager.Logger.Exception("HubConnection", "Exception in OnClosed user code!", ex2);
						goto IL_1A7;
					}
				}
				if (this.OnError != null)
				{
					try
					{
						this.OnError(this, errorReason);
					}
					catch (Exception ex3)
					{
						HTTPManager.Logger.Exception("HubConnection", "Exception in OnError user code!", ex3);
					}
				}
				IL_1A7:
				HTTPManager.Heartbeats.Unsubscribe(this);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x000A3F2C File Offset: 0x000A212C
		void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
		{
			ConnectionStates state = this.State;
			if (state == ConnectionStates.Connected && this.Options.PingInterval != TimeSpan.Zero && DateTime.UtcNow - this.lastMessageSent >= this.Options.PingInterval)
			{
				this.SendMessage(new Message
				{
					type = MessageTypes.Ping
				});
			}
		}

		// Token: 0x040014FF RID: 5375
		public static readonly object[] EmptyArgs = new object[0];

		// Token: 0x0400150D RID: 5389
		private long lastInvocationId;

		// Token: 0x0400150E RID: 5390
		private int lastStreamId;

		// Token: 0x0400150F RID: 5391
		private Dictionary<long, Action<Message>> invocations = new Dictionary<long, Action<Message>>();

		// Token: 0x04001510 RID: 5392
		private Dictionary<string, Subscription> subscriptions = new Dictionary<string, Subscription>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04001511 RID: 5393
		private DateTime lastMessageSent;
	}
}
