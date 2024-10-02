using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BestHTTP.PlatformSupport.TcpClient.General
{
	// Token: 0x020007D6 RID: 2006
	public class TcpClient : IDisposable
	{
		// Token: 0x0600473A RID: 18234 RVA: 0x0019589A File Offset: 0x00193A9A
		private void Init(AddressFamily family)
		{
			this.active = false;
			if (this.client != null)
			{
				this.client.Close();
				this.client = null;
			}
			this.client = new Socket(family, SocketType.Stream, ProtocolType.Tcp);
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x001958CB File Offset: 0x00193ACB
		public TcpClient()
		{
			this.Init(AddressFamily.InterNetwork);
			this.ConnectTimeout = TimeSpan.FromSeconds(2.0);
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x001958EE File Offset: 0x00193AEE
		public TcpClient(AddressFamily family)
		{
			if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6)
			{
				throw new ArgumentException("Family must be InterNetwork or InterNetworkV6", "family");
			}
			this.Init(family);
			this.ConnectTimeout = TimeSpan.FromSeconds(2.0);
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x0019592A File Offset: 0x00193B2A
		public TcpClient(IPEndPoint localEP)
		{
			this.Init(localEP.AddressFamily);
			this.ConnectTimeout = TimeSpan.FromSeconds(2.0);
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x00195952 File Offset: 0x00193B52
		public TcpClient(string hostname, int port)
		{
			this.ConnectTimeout = TimeSpan.FromSeconds(2.0);
			this.Connect(hostname, port);
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x0600473F RID: 18239 RVA: 0x00195976 File Offset: 0x00193B76
		// (set) Token: 0x06004740 RID: 18240 RVA: 0x0019597E File Offset: 0x00193B7E
		protected bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06004741 RID: 18241 RVA: 0x00195987 File Offset: 0x00193B87
		// (set) Token: 0x06004742 RID: 18242 RVA: 0x0019598F File Offset: 0x00193B8F
		public Socket Client
		{
			get
			{
				return this.client;
			}
			set
			{
				this.client = value;
				this.stream = null;
			}
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06004743 RID: 18243 RVA: 0x0019599F File Offset: 0x00193B9F
		public int Available
		{
			get
			{
				return this.client.Available;
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06004744 RID: 18244 RVA: 0x001959AC File Offset: 0x00193BAC
		public bool Connected
		{
			get
			{
				return this.client.Connected;
			}
		}

		// Token: 0x06004745 RID: 18245 RVA: 0x001959BC File Offset: 0x00193BBC
		public bool IsConnected()
		{
			bool result;
			try
			{
				result = (!this.Client.Poll(1, SelectMode.SelectRead) || this.Client.Available != 0);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06004746 RID: 18246 RVA: 0x00195A04 File Offset: 0x00193C04
		// (set) Token: 0x06004747 RID: 18247 RVA: 0x00195A11 File Offset: 0x00193C11
		public bool ExclusiveAddressUse
		{
			get
			{
				return this.client.ExclusiveAddressUse;
			}
			set
			{
				this.client.ExclusiveAddressUse = value;
			}
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x00195A1F File Offset: 0x00193C1F
		internal void SetTcpClient(Socket s)
		{
			this.Client = s;
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06004749 RID: 18249 RVA: 0x00195A28 File Offset: 0x00193C28
		// (set) Token: 0x0600474A RID: 18250 RVA: 0x00195A55 File Offset: 0x00193C55
		public LingerOption LingerState
		{
			get
			{
				if ((this.values & TcpClient.Properties.LingerState) != (TcpClient.Properties)0U)
				{
					return this.linger_state;
				}
				return (LingerOption)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.linger_state = value;
					this.values |= TcpClient.Properties.LingerState;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, value);
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x0600474B RID: 18251 RVA: 0x00195A90 File Offset: 0x00193C90
		// (set) Token: 0x0600474C RID: 18252 RVA: 0x00195AB5 File Offset: 0x00193CB5
		public bool NoDelay
		{
			get
			{
				if ((this.values & TcpClient.Properties.NoDelay) != (TcpClient.Properties)0U)
				{
					return this.no_delay;
				}
				return (bool)this.client.GetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.no_delay = value;
					this.values |= TcpClient.Properties.NoDelay;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, value ? 1 : 0);
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x0600474D RID: 18253 RVA: 0x00195AEE File Offset: 0x00193CEE
		// (set) Token: 0x0600474E RID: 18254 RVA: 0x00195B1B File Offset: 0x00193D1B
		public int ReceiveBufferSize
		{
			get
			{
				if ((this.values & TcpClient.Properties.ReceiveBufferSize) != (TcpClient.Properties)0U)
				{
					return this.recv_buffer_size;
				}
				return (int)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.recv_buffer_size = value;
					this.values |= TcpClient.Properties.ReceiveBufferSize;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, value);
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x0600474F RID: 18255 RVA: 0x00195B56 File Offset: 0x00193D56
		// (set) Token: 0x06004750 RID: 18256 RVA: 0x00195B83 File Offset: 0x00193D83
		public int ReceiveTimeout
		{
			get
			{
				if ((this.values & TcpClient.Properties.ReceiveTimeout) != (TcpClient.Properties)0U)
				{
					return this.recv_timeout;
				}
				return (int)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.recv_timeout = value;
					this.values |= TcpClient.Properties.ReceiveTimeout;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, value);
			}
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06004751 RID: 18257 RVA: 0x00195BBE File Offset: 0x00193DBE
		// (set) Token: 0x06004752 RID: 18258 RVA: 0x00195BEC File Offset: 0x00193DEC
		public int SendBufferSize
		{
			get
			{
				if ((this.values & TcpClient.Properties.SendBufferSize) != (TcpClient.Properties)0U)
				{
					return this.send_buffer_size;
				}
				return (int)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.send_buffer_size = value;
					this.values |= TcpClient.Properties.SendBufferSize;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, value);
			}
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06004753 RID: 18259 RVA: 0x00195C28 File Offset: 0x00193E28
		// (set) Token: 0x06004754 RID: 18260 RVA: 0x00195C56 File Offset: 0x00193E56
		public int SendTimeout
		{
			get
			{
				if ((this.values & TcpClient.Properties.SendTimeout) != (TcpClient.Properties)0U)
				{
					return this.send_timeout;
				}
				return (int)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.send_timeout = value;
					this.values |= TcpClient.Properties.SendTimeout;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, value);
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06004755 RID: 18261 RVA: 0x00195C92 File Offset: 0x00193E92
		// (set) Token: 0x06004756 RID: 18262 RVA: 0x00195C9A File Offset: 0x00193E9A
		public TimeSpan ConnectTimeout { get; set; }

		// Token: 0x06004757 RID: 18263 RVA: 0x00195CA3 File Offset: 0x00193EA3
		public void Close()
		{
			((IDisposable)this).Dispose();
		}

		// Token: 0x06004758 RID: 18264 RVA: 0x00195CAC File Offset: 0x00193EAC
		public void Connect(IPEndPoint remoteEP)
		{
			try
			{
				if (this.ConnectTimeout > TimeSpan.Zero)
				{
					ManualResetEvent mre = new ManualResetEvent(false);
					IAsyncResult asyncResult = this.client.BeginConnect(remoteEP, delegate(IAsyncResult res)
					{
						mre.Set();
					}, null);
					this.active = mre.WaitOne(this.ConnectTimeout);
					if (!this.active)
					{
						try
						{
							this.client.Disconnect(true);
						}
						catch
						{
						}
						throw new TimeoutException("Connection timed out!");
					}
					this.client.EndConnect(asyncResult);
				}
				else
				{
					this.client.Connect(remoteEP);
					this.active = true;
				}
			}
			finally
			{
				this.CheckDisposed();
			}
		}

		// Token: 0x06004759 RID: 18265 RVA: 0x00195D7C File Offset: 0x00193F7C
		public void Connect(IPAddress address, int port)
		{
			this.Connect(new IPEndPoint(address, port));
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x00195D8C File Offset: 0x00193F8C
		private void SetOptions()
		{
			TcpClient.Properties properties = this.values;
			this.values = (TcpClient.Properties)0U;
			if ((properties & TcpClient.Properties.LingerState) != (TcpClient.Properties)0U)
			{
				this.LingerState = this.linger_state;
			}
			if ((properties & TcpClient.Properties.NoDelay) != (TcpClient.Properties)0U)
			{
				this.NoDelay = this.no_delay;
			}
			if ((properties & TcpClient.Properties.ReceiveBufferSize) != (TcpClient.Properties)0U)
			{
				this.ReceiveBufferSize = this.recv_buffer_size;
			}
			if ((properties & TcpClient.Properties.ReceiveTimeout) != (TcpClient.Properties)0U)
			{
				this.ReceiveTimeout = this.recv_timeout;
			}
			if ((properties & TcpClient.Properties.SendBufferSize) != (TcpClient.Properties)0U)
			{
				this.SendBufferSize = this.send_buffer_size;
			}
			if ((properties & TcpClient.Properties.SendTimeout) != (TcpClient.Properties)0U)
			{
				this.SendTimeout = this.send_timeout;
			}
		}

		// Token: 0x0600475B RID: 18267 RVA: 0x00195E10 File Offset: 0x00194010
		public void Connect(string hostname, int port)
		{
			if (!(this.ConnectTimeout > TimeSpan.Zero))
			{
				IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
				this.Connect(hostAddresses, port);
				return;
			}
			ManualResetEvent mre = new ManualResetEvent(false);
			IAsyncResult asyncResult = Dns.BeginGetHostAddresses(hostname, delegate(IAsyncResult res)
			{
				mre.Set();
			}, null);
			if (mre.WaitOne(this.ConnectTimeout))
			{
				IPAddress[] ipAddresses = Dns.EndGetHostAddresses(asyncResult);
				this.Connect(ipAddresses, port);
				return;
			}
			throw new TimeoutException("DNS resolve timed out!");
		}

		// Token: 0x0600475C RID: 18268 RVA: 0x00195E94 File Offset: 0x00194094
		public void Connect(IPAddress[] ipAddresses, int port)
		{
			this.CheckDisposed();
			if (ipAddresses == null)
			{
				throw new ArgumentNullException("ipAddresses");
			}
			for (int i = 0; i < ipAddresses.Length; i++)
			{
				try
				{
					IPAddress ipaddress = ipAddresses[i];
					if (ipaddress.Equals(IPAddress.Any) || ipaddress.Equals(IPAddress.IPv6Any))
					{
						throw new SocketException(10049);
					}
					this.Init(ipaddress.AddressFamily);
					if (ipaddress.AddressFamily != AddressFamily.InterNetwork && ipaddress.AddressFamily != AddressFamily.InterNetworkV6)
					{
						throw new NotSupportedException("This method is only valid for sockets in the InterNetwork and InterNetworkV6 families");
					}
					this.Connect(new IPEndPoint(ipaddress, port));
					if (this.values != (TcpClient.Properties)0U)
					{
						this.SetOptions();
					}
					this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
					HTTPManager.Logger.Information("TcpClient", string.Format("Connected to {0}:{1}", ipaddress.ToString(), port.ToString()));
					break;
				}
				catch (Exception ex)
				{
					this.Init(AddressFamily.InterNetwork);
					if (i == ipAddresses.Length - 1)
					{
						throw ex;
					}
				}
			}
		}

		// Token: 0x0600475D RID: 18269 RVA: 0x00195F98 File Offset: 0x00194198
		public void EndConnect(IAsyncResult asyncResult)
		{
			this.client.EndConnect(asyncResult);
		}

		// Token: 0x0600475E RID: 18270 RVA: 0x00195FA6 File Offset: 0x001941A6
		public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state)
		{
			return this.client.BeginConnect(address, port, requestCallback, state);
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x00195FB8 File Offset: 0x001941B8
		public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state)
		{
			return this.client.BeginConnect(addresses, port, requestCallback, state);
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x00195FCA File Offset: 0x001941CA
		public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state)
		{
			return this.client.BeginConnect(host, port, requestCallback, state);
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x00195FDC File Offset: 0x001941DC
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x00195FEC File Offset: 0x001941EC
		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			if (disposing)
			{
				NetworkStream networkStream = this.stream;
				this.stream = null;
				if (networkStream != null)
				{
					networkStream.Close();
					this.active = false;
					return;
				}
				if (this.client != null)
				{
					this.client.Close();
					this.client = null;
				}
			}
		}

		// Token: 0x06004763 RID: 18275 RVA: 0x00196048 File Offset: 0x00194248
		~TcpClient()
		{
			this.Dispose(false);
		}

		// Token: 0x06004764 RID: 18276 RVA: 0x00196078 File Offset: 0x00194278
		public Stream GetStream()
		{
			Stream result;
			try
			{
				if (this.stream == null)
				{
					this.stream = new NetworkStream(this.client, true);
				}
				result = this.stream;
			}
			finally
			{
				this.CheckDisposed();
			}
			return result;
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x001960C0 File Offset: 0x001942C0
		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x04002EA3 RID: 11939
		private NetworkStream stream;

		// Token: 0x04002EA4 RID: 11940
		private bool active;

		// Token: 0x04002EA5 RID: 11941
		private Socket client;

		// Token: 0x04002EA6 RID: 11942
		private bool disposed;

		// Token: 0x04002EA7 RID: 11943
		private TcpClient.Properties values;

		// Token: 0x04002EA8 RID: 11944
		private int recv_timeout;

		// Token: 0x04002EA9 RID: 11945
		private int send_timeout;

		// Token: 0x04002EAA RID: 11946
		private int recv_buffer_size;

		// Token: 0x04002EAB RID: 11947
		private int send_buffer_size;

		// Token: 0x04002EAC RID: 11948
		private LingerOption linger_state;

		// Token: 0x04002EAD RID: 11949
		private bool no_delay;

		// Token: 0x020009DF RID: 2527
		private enum Properties : uint
		{
			// Token: 0x040037A8 RID: 14248
			LingerState = 1U,
			// Token: 0x040037A9 RID: 14249
			NoDelay,
			// Token: 0x040037AA RID: 14250
			ReceiveBufferSize = 4U,
			// Token: 0x040037AB RID: 14251
			ReceiveTimeout = 8U,
			// Token: 0x040037AC RID: 14252
			SendBufferSize = 16U,
			// Token: 0x040037AD RID: 14253
			SendTimeout = 32U
		}
	}
}
