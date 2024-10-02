using System;
using System.Collections.Generic;
using System.IO;
using BestHTTP.Extensions;

namespace BestHTTP.Cookies
{
	// Token: 0x02000814 RID: 2068
	public sealed class Cookie : IComparable<Cookie>, IEquatable<Cookie>
	{
		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x0600496F RID: 18799 RVA: 0x001A1B3A File Offset: 0x0019FD3A
		// (set) Token: 0x06004970 RID: 18800 RVA: 0x001A1B42 File Offset: 0x0019FD42
		public string Name { get; private set; }

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06004971 RID: 18801 RVA: 0x001A1B4B File Offset: 0x0019FD4B
		// (set) Token: 0x06004972 RID: 18802 RVA: 0x001A1B53 File Offset: 0x0019FD53
		public string Value { get; private set; }

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06004973 RID: 18803 RVA: 0x001A1B5C File Offset: 0x0019FD5C
		// (set) Token: 0x06004974 RID: 18804 RVA: 0x001A1B64 File Offset: 0x0019FD64
		public DateTime Date { get; internal set; }

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06004975 RID: 18805 RVA: 0x001A1B6D File Offset: 0x0019FD6D
		// (set) Token: 0x06004976 RID: 18806 RVA: 0x001A1B75 File Offset: 0x0019FD75
		public DateTime LastAccess { get; set; }

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06004977 RID: 18807 RVA: 0x001A1B7E File Offset: 0x0019FD7E
		// (set) Token: 0x06004978 RID: 18808 RVA: 0x001A1B86 File Offset: 0x0019FD86
		public DateTime Expires { get; private set; }

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06004979 RID: 18809 RVA: 0x001A1B8F File Offset: 0x0019FD8F
		// (set) Token: 0x0600497A RID: 18810 RVA: 0x001A1B97 File Offset: 0x0019FD97
		public long MaxAge { get; private set; }

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x0600497B RID: 18811 RVA: 0x001A1BA0 File Offset: 0x0019FDA0
		// (set) Token: 0x0600497C RID: 18812 RVA: 0x001A1BA8 File Offset: 0x0019FDA8
		public bool IsSession { get; private set; }

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x0600497D RID: 18813 RVA: 0x001A1BB1 File Offset: 0x0019FDB1
		// (set) Token: 0x0600497E RID: 18814 RVA: 0x001A1BB9 File Offset: 0x0019FDB9
		public string Domain { get; private set; }

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x0600497F RID: 18815 RVA: 0x001A1BC2 File Offset: 0x0019FDC2
		// (set) Token: 0x06004980 RID: 18816 RVA: 0x001A1BCA File Offset: 0x0019FDCA
		public string Path { get; private set; }

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06004981 RID: 18817 RVA: 0x001A1BD3 File Offset: 0x0019FDD3
		// (set) Token: 0x06004982 RID: 18818 RVA: 0x001A1BDB File Offset: 0x0019FDDB
		public bool IsSecure { get; private set; }

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06004983 RID: 18819 RVA: 0x001A1BE4 File Offset: 0x0019FDE4
		// (set) Token: 0x06004984 RID: 18820 RVA: 0x001A1BEC File Offset: 0x0019FDEC
		public bool IsHttpOnly { get; private set; }

		// Token: 0x06004985 RID: 18821 RVA: 0x001A1BF5 File Offset: 0x0019FDF5
		public Cookie(string name, string value) : this(name, value, "/", string.Empty)
		{
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x001A1C09 File Offset: 0x0019FE09
		public Cookie(string name, string value, string path) : this(name, value, path, string.Empty)
		{
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x001A1C19 File Offset: 0x0019FE19
		public Cookie(string name, string value, string path, string domain) : this()
		{
			this.Name = name;
			this.Value = value;
			this.Path = path;
			this.Domain = domain;
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x001A1C3E File Offset: 0x0019FE3E
		public Cookie(Uri uri, string name, string value, DateTime expires, bool isSession = true) : this(name, value, uri.AbsolutePath, uri.Host)
		{
			this.Expires = expires;
			this.IsSession = isSession;
			this.Date = DateTime.UtcNow;
		}

		// Token: 0x06004989 RID: 18825 RVA: 0x001A1C6F File Offset: 0x0019FE6F
		public Cookie(Uri uri, string name, string value, long maxAge = -1L, bool isSession = true) : this(name, value, uri.AbsolutePath, uri.Host)
		{
			this.MaxAge = maxAge;
			this.IsSession = isSession;
			this.Date = DateTime.UtcNow;
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x001A1CA0 File Offset: 0x0019FEA0
		internal Cookie()
		{
			this.IsSession = true;
			this.MaxAge = -1L;
			this.LastAccess = DateTime.UtcNow;
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x001A1CC4 File Offset: 0x0019FEC4
		public bool WillExpireInTheFuture()
		{
			if (this.IsSession)
			{
				return true;
			}
			if (this.MaxAge == -1L)
			{
				return this.Expires > DateTime.UtcNow;
			}
			return Math.Max(0L, (long)(DateTime.UtcNow - this.Date).TotalSeconds) < this.MaxAge;
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x001A1D20 File Offset: 0x0019FF20
		public uint GuessSize()
		{
			return (uint)(((this.Name != null) ? (this.Name.Length * 2) : 0) + ((this.Value != null) ? (this.Value.Length * 2) : 0) + ((this.Domain != null) ? (this.Domain.Length * 2) : 0) + ((this.Path != null) ? (this.Path.Length * 2) : 0) + 32 + 3);
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x001A1D98 File Offset: 0x0019FF98
		public static Cookie Parse(string header, Uri defaultDomain)
		{
			Cookie cookie = new Cookie();
			try
			{
				foreach (HeaderValue headerValue in Cookie.ParseCookieHeader(header))
				{
					string a = headerValue.Key.ToLowerInvariant();
					if (!(a == "path"))
					{
						if (!(a == "domain"))
						{
							if (!(a == "expires"))
							{
								if (!(a == "max-age"))
								{
									if (!(a == "secure"))
									{
										if (!(a == "httponly"))
										{
											cookie.Name = headerValue.Key;
											cookie.Value = headerValue.Value;
										}
										else
										{
											cookie.IsHttpOnly = true;
										}
									}
									else
									{
										cookie.IsSecure = true;
									}
								}
								else
								{
									cookie.MaxAge = headerValue.Value.ToInt64(-1L);
									cookie.IsSession = false;
								}
							}
							else
							{
								cookie.Expires = headerValue.Value.ToDateTime(DateTime.FromBinary(0L));
								cookie.IsSession = false;
							}
						}
						else
						{
							if (string.IsNullOrEmpty(headerValue.Value))
							{
								return null;
							}
							cookie.Domain = (headerValue.Value.StartsWith(".") ? headerValue.Value.Substring(1) : headerValue.Value);
						}
					}
					else
					{
						cookie.Path = ((string.IsNullOrEmpty(headerValue.Value) || !headerValue.Value.StartsWith("/")) ? "/" : (cookie.Path = headerValue.Value));
					}
				}
				if (HTTPManager.EnablePrivateBrowsing)
				{
					cookie.IsSession = true;
				}
				if (string.IsNullOrEmpty(cookie.Domain))
				{
					cookie.Domain = defaultDomain.Host;
				}
				if (string.IsNullOrEmpty(cookie.Path))
				{
					cookie.Path = defaultDomain.AbsolutePath;
				}
				cookie.Date = (cookie.LastAccess = DateTime.UtcNow);
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Warning("Cookie", string.Concat(new string[]
				{
					"Parse - Couldn't parse header: ",
					header,
					" exception: ",
					ex.ToString(),
					" ",
					ex.StackTrace
				}));
			}
			return cookie;
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x001A2018 File Offset: 0x001A0218
		internal void SaveTo(BinaryWriter stream)
		{
			stream.Write(1);
			stream.Write(this.Name ?? string.Empty);
			stream.Write(this.Value ?? string.Empty);
			stream.Write(this.Date.ToBinary());
			stream.Write(this.LastAccess.ToBinary());
			stream.Write(this.Expires.ToBinary());
			stream.Write(this.MaxAge);
			stream.Write(this.IsSession);
			stream.Write(this.Domain ?? string.Empty);
			stream.Write(this.Path ?? string.Empty);
			stream.Write(this.IsSecure);
			stream.Write(this.IsHttpOnly);
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x001A20EC File Offset: 0x001A02EC
		internal void LoadFrom(BinaryReader stream)
		{
			stream.ReadInt32();
			this.Name = stream.ReadString();
			this.Value = stream.ReadString();
			this.Date = DateTime.FromBinary(stream.ReadInt64());
			this.LastAccess = DateTime.FromBinary(stream.ReadInt64());
			this.Expires = DateTime.FromBinary(stream.ReadInt64());
			this.MaxAge = stream.ReadInt64();
			this.IsSession = stream.ReadBoolean();
			this.Domain = stream.ReadString();
			this.Path = stream.ReadString();
			this.IsSecure = stream.ReadBoolean();
			this.IsHttpOnly = stream.ReadBoolean();
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x001A2193 File Offset: 0x001A0393
		public override string ToString()
		{
			return this.Name + "=" + this.Value;
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x001A21AB File Offset: 0x001A03AB
		public override bool Equals(object obj)
		{
			return obj != null && this.Equals(obj as Cookie);
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x001A21C0 File Offset: 0x001A03C0
		public bool Equals(Cookie cookie)
		{
			return cookie != null && (this == cookie || (this.Name.Equals(cookie.Name, StringComparison.Ordinal) && ((this.Domain == null && cookie.Domain == null) || this.Domain.Equals(cookie.Domain, StringComparison.Ordinal)) && ((this.Path == null && cookie.Path == null) || this.Path.Equals(cookie.Path, StringComparison.Ordinal))));
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x001A2236 File Offset: 0x001A0436
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x001A2244 File Offset: 0x001A0444
		private static string ReadValue(string str, ref int pos)
		{
			string empty = string.Empty;
			if (str == null)
			{
				return empty;
			}
			return str.Read(ref pos, ';', true);
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x001A2268 File Offset: 0x001A0468
		private static List<HeaderValue> ParseCookieHeader(string str)
		{
			List<HeaderValue> list = new List<HeaderValue>();
			if (str == null)
			{
				return list;
			}
			int i = 0;
			while (i < str.Length)
			{
				HeaderValue headerValue = new HeaderValue(str.Read(ref i, (char ch) => ch != '=' && ch != ';', true).Trim());
				if (i < str.Length && str[i - 1] == '=')
				{
					headerValue.Value = Cookie.ReadValue(str, ref i);
				}
				list.Add(headerValue);
			}
			return list;
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x001A22EC File Offset: 0x001A04EC
		public int CompareTo(Cookie other)
		{
			return this.LastAccess.CompareTo(other.LastAccess);
		}

		// Token: 0x04003051 RID: 12369
		private const int Version = 1;
	}
}
