using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.Extensions;

namespace BestHTTP.Authentication
{
	// Token: 0x0200081C RID: 2076
	public sealed class Digest
	{
		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06004A02 RID: 18946 RVA: 0x001A444F File Offset: 0x001A264F
		// (set) Token: 0x06004A03 RID: 18947 RVA: 0x001A4457 File Offset: 0x001A2657
		public Uri Uri { get; private set; }

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06004A04 RID: 18948 RVA: 0x001A4460 File Offset: 0x001A2660
		// (set) Token: 0x06004A05 RID: 18949 RVA: 0x001A4468 File Offset: 0x001A2668
		public AuthenticationTypes Type { get; private set; }

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06004A06 RID: 18950 RVA: 0x001A4471 File Offset: 0x001A2671
		// (set) Token: 0x06004A07 RID: 18951 RVA: 0x001A4479 File Offset: 0x001A2679
		public string Realm { get; private set; }

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06004A08 RID: 18952 RVA: 0x001A4482 File Offset: 0x001A2682
		// (set) Token: 0x06004A09 RID: 18953 RVA: 0x001A448A File Offset: 0x001A268A
		public bool Stale { get; private set; }

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06004A0A RID: 18954 RVA: 0x001A4493 File Offset: 0x001A2693
		// (set) Token: 0x06004A0B RID: 18955 RVA: 0x001A449B File Offset: 0x001A269B
		private string Nonce { get; set; }

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06004A0C RID: 18956 RVA: 0x001A44A4 File Offset: 0x001A26A4
		// (set) Token: 0x06004A0D RID: 18957 RVA: 0x001A44AC File Offset: 0x001A26AC
		private string Opaque { get; set; }

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06004A0E RID: 18958 RVA: 0x001A44B5 File Offset: 0x001A26B5
		// (set) Token: 0x06004A0F RID: 18959 RVA: 0x001A44BD File Offset: 0x001A26BD
		private string Algorithm { get; set; }

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06004A10 RID: 18960 RVA: 0x001A44C6 File Offset: 0x001A26C6
		// (set) Token: 0x06004A11 RID: 18961 RVA: 0x001A44CE File Offset: 0x001A26CE
		public List<string> ProtectedUris { get; private set; }

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06004A12 RID: 18962 RVA: 0x001A44D7 File Offset: 0x001A26D7
		// (set) Token: 0x06004A13 RID: 18963 RVA: 0x001A44DF File Offset: 0x001A26DF
		private string QualityOfProtections { get; set; }

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06004A14 RID: 18964 RVA: 0x001A44E8 File Offset: 0x001A26E8
		// (set) Token: 0x06004A15 RID: 18965 RVA: 0x001A44F0 File Offset: 0x001A26F0
		private int NonceCount { get; set; }

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06004A16 RID: 18966 RVA: 0x001A44F9 File Offset: 0x001A26F9
		// (set) Token: 0x06004A17 RID: 18967 RVA: 0x001A4501 File Offset: 0x001A2701
		private string HA1Sess { get; set; }

		// Token: 0x06004A18 RID: 18968 RVA: 0x001A450A File Offset: 0x001A270A
		internal Digest(Uri uri)
		{
			this.Uri = uri;
			this.Algorithm = "md5";
		}

		// Token: 0x06004A19 RID: 18969 RVA: 0x001A4524 File Offset: 0x001A2724
		public void ParseChallange(string header)
		{
			this.Type = AuthenticationTypes.Unknown;
			this.Stale = false;
			this.Opaque = null;
			this.HA1Sess = null;
			this.NonceCount = 0;
			this.QualityOfProtections = null;
			if (this.ProtectedUris != null)
			{
				this.ProtectedUris.Clear();
			}
			foreach (HeaderValue headerValue in new WWWAuthenticateHeaderParser(header).Values)
			{
				string key = headerValue.Key;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(key);
				if (num <= 1863671838U)
				{
					if (num <= 474311018U)
					{
						if (num != 87360061U)
						{
							if (num == 474311018U)
							{
								if (key == "algorithm")
								{
									this.Algorithm = headerValue.Value;
								}
							}
						}
						else if (key == "basic")
						{
							this.Type = AuthenticationTypes.Basic;
						}
					}
					else if (num != 1749328254U)
					{
						if (num == 1863671838U)
						{
							if (key == "opaque")
							{
								this.Opaque = headerValue.Value;
							}
						}
					}
					else if (key == "stale")
					{
						this.Stale = bool.Parse(headerValue.Value);
					}
				}
				else if (num <= 3885209585U)
				{
					if (num != 1914854288U)
					{
						if (num == 3885209585U)
						{
							if (key == "domain")
							{
								if (!string.IsNullOrEmpty(headerValue.Value) && headerValue.Value.Length != 0)
								{
									if (this.ProtectedUris == null)
									{
										this.ProtectedUris = new List<string>();
									}
									int num2 = 0;
									string item = headerValue.Value.Read(ref num2, ' ', true);
									do
									{
										this.ProtectedUris.Add(item);
										item = headerValue.Value.Read(ref num2, ' ', true);
									}
									while (num2 < headerValue.Value.Length);
								}
							}
						}
					}
					else if (key == "realm")
					{
						this.Realm = headerValue.Value;
					}
				}
				else if (num != 4143537083U)
				{
					if (num != 4178082296U)
					{
						if (num == 4179908061U)
						{
							if (key == "digest")
							{
								this.Type = AuthenticationTypes.Digest;
							}
						}
					}
					else if (key == "nonce")
					{
						this.Nonce = headerValue.Value;
					}
				}
				else if (key == "qop")
				{
					this.QualityOfProtections = headerValue.Value;
				}
			}
		}

		// Token: 0x06004A1A RID: 18970 RVA: 0x001A47F8 File Offset: 0x001A29F8
		public string GenerateResponseHeader(HTTPRequest request, Credentials credentials, bool isProxy = false)
		{
			try
			{
				AuthenticationTypes type = this.Type;
				if (type == AuthenticationTypes.Basic)
				{
					return "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", credentials.UserName, credentials.Password)));
				}
				if (type == AuthenticationTypes.Digest)
				{
					int nonceCount = this.NonceCount;
					this.NonceCount = nonceCount + 1;
					string text = string.Empty;
					string text2 = new Random(request.GetHashCode()).Next(int.MinValue, int.MaxValue).ToString("X8");
					string text3 = this.NonceCount.ToString("X8");
					string a = this.Algorithm.TrimAndLower();
					if (!(a == "md5"))
					{
						if (!(a == "md5-sess"))
						{
							return string.Empty;
						}
						if (string.IsNullOrEmpty(this.HA1Sess))
						{
							this.HA1Sess = string.Format("{0}:{1}:{2}:{3}:{4}", new object[]
							{
								credentials.UserName,
								this.Realm,
								credentials.Password,
								this.Nonce,
								text3
							}).CalculateMD5Hash();
						}
						text = this.HA1Sess;
					}
					else
					{
						text = string.Format("{0}:{1}:{2}", credentials.UserName, this.Realm, credentials.Password).CalculateMD5Hash();
					}
					string text4 = string.Empty;
					string text5 = (this.QualityOfProtections != null) ? this.QualityOfProtections.TrimAndLower() : null;
					string text6 = isProxy ? "CONNECT" : request.MethodType.ToString().ToUpper();
					string text7 = isProxy ? (request.CurrentUri.Host + ":" + request.CurrentUri.Port) : request.CurrentUri.GetRequestPathAndQueryURL();
					if (text5 == null)
					{
						string arg = (request.MethodType.ToString().ToUpper() + ":" + request.CurrentUri.GetRequestPathAndQueryURL()).CalculateMD5Hash();
						text4 = string.Format("{0}:{1}:{2}", text, this.Nonce, arg).CalculateMD5Hash();
					}
					else if (text5.Contains("auth-int"))
					{
						text5 = "auth-int";
						byte[] array = request.GetEntityBody();
						if (array == null)
						{
							array = VariableSizedBufferPool.NoData;
						}
						string text8 = string.Format("{0}:{1}:{2}", text6, text7, array.CalculateMD5Hash()).CalculateMD5Hash();
						text4 = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", new object[]
						{
							text,
							this.Nonce,
							text3,
							text2,
							text5,
							text8
						}).CalculateMD5Hash();
					}
					else
					{
						if (!text5.Contains("auth"))
						{
							return string.Empty;
						}
						text5 = "auth";
						string text9 = (text6 + ":" + text7).CalculateMD5Hash();
						text4 = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", new object[]
						{
							text,
							this.Nonce,
							text3,
							text2,
							text5,
							text9
						}).CalculateMD5Hash();
					}
					string text10 = string.Format("Digest username=\"{0}\", realm=\"{1}\", nonce=\"{2}\", uri=\"{3}\", cnonce=\"{4}\", response=\"{5}\"", new object[]
					{
						credentials.UserName,
						this.Realm,
						this.Nonce,
						text7,
						text2,
						text4
					});
					if (text5 != null)
					{
						text10 = string.Concat(new string[]
						{
							text10,
							", qop=\"",
							text5,
							"\", nc=",
							text3
						});
					}
					if (!string.IsNullOrEmpty(this.Opaque))
					{
						text10 = text10 + ", opaque=\"" + this.Opaque + "\"";
					}
					return text10;
				}
			}
			catch
			{
			}
			return string.Empty;
		}

		// Token: 0x06004A1B RID: 18971 RVA: 0x001A4BD8 File Offset: 0x001A2DD8
		public bool IsUriProtected(Uri uri)
		{
			if (string.CompareOrdinal(uri.Host, this.Uri.Host) != 0)
			{
				return false;
			}
			string text = uri.ToString();
			if (this.ProtectedUris != null && this.ProtectedUris.Count > 0)
			{
				for (int i = 0; i < this.ProtectedUris.Count; i++)
				{
					if (text.Contains(this.ProtectedUris[i]))
					{
						return true;
					}
				}
			}
			return true;
		}
	}
}
