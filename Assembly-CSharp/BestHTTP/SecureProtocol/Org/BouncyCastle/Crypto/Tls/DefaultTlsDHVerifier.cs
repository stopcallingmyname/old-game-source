using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000417 RID: 1047
	public class DefaultTlsDHVerifier : TlsDHVerifier
	{
		// Token: 0x060029EA RID: 10730 RVA: 0x0010F6C4 File Offset: 0x0010D8C4
		private static void AddDefaultGroup(DHParameters dhParameters)
		{
			DefaultTlsDHVerifier.DefaultGroups.Add(dhParameters);
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x0010F6D4 File Offset: 0x0010D8D4
		static DefaultTlsDHVerifier()
		{
			DefaultTlsDHVerifier.AddDefaultGroup(DHStandardGroups.rfc7919_ffdhe2048);
			DefaultTlsDHVerifier.AddDefaultGroup(DHStandardGroups.rfc7919_ffdhe3072);
			DefaultTlsDHVerifier.AddDefaultGroup(DHStandardGroups.rfc7919_ffdhe4096);
			DefaultTlsDHVerifier.AddDefaultGroup(DHStandardGroups.rfc7919_ffdhe6144);
			DefaultTlsDHVerifier.AddDefaultGroup(DHStandardGroups.rfc7919_ffdhe8192);
			DefaultTlsDHVerifier.AddDefaultGroup(DHStandardGroups.rfc3526_1536);
			DefaultTlsDHVerifier.AddDefaultGroup(DHStandardGroups.rfc3526_2048);
			DefaultTlsDHVerifier.AddDefaultGroup(DHStandardGroups.rfc3526_3072);
			DefaultTlsDHVerifier.AddDefaultGroup(DHStandardGroups.rfc3526_4096);
			DefaultTlsDHVerifier.AddDefaultGroup(DHStandardGroups.rfc3526_6144);
			DefaultTlsDHVerifier.AddDefaultGroup(DHStandardGroups.rfc3526_8192);
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x0010F763 File Offset: 0x0010D963
		public DefaultTlsDHVerifier() : this(DefaultTlsDHVerifier.DefaultMinimumPrimeBits)
		{
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x0010F770 File Offset: 0x0010D970
		public DefaultTlsDHVerifier(int minimumPrimeBits) : this(DefaultTlsDHVerifier.DefaultGroups, minimumPrimeBits)
		{
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x0010F77E File Offset: 0x0010D97E
		public DefaultTlsDHVerifier(IList groups, int minimumPrimeBits)
		{
			this.mGroups = groups;
			this.mMinimumPrimeBits = minimumPrimeBits;
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x0010F794 File Offset: 0x0010D994
		public virtual bool Accept(DHParameters dhParameters)
		{
			return this.CheckMinimumPrimeBits(dhParameters) && this.CheckGroup(dhParameters);
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x060029F0 RID: 10736 RVA: 0x0010F7A8 File Offset: 0x0010D9A8
		public virtual int MinimumPrimeBits
		{
			get
			{
				return this.mMinimumPrimeBits;
			}
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x0010F7B0 File Offset: 0x0010D9B0
		protected virtual bool AreGroupsEqual(DHParameters a, DHParameters b)
		{
			return a == b || (this.AreParametersEqual(a.P, b.P) && this.AreParametersEqual(a.G, b.G));
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x0010F7E0 File Offset: 0x0010D9E0
		protected virtual bool AreParametersEqual(BigInteger a, BigInteger b)
		{
			return a == b || a.Equals(b);
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x0010F7F0 File Offset: 0x0010D9F0
		protected virtual bool CheckGroup(DHParameters dhParameters)
		{
			foreach (object obj in this.mGroups)
			{
				DHParameters b = (DHParameters)obj;
				if (this.AreGroupsEqual(dhParameters, b))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060029F4 RID: 10740 RVA: 0x0010F854 File Offset: 0x0010DA54
		protected virtual bool CheckMinimumPrimeBits(DHParameters dhParameters)
		{
			return dhParameters.P.BitLength >= this.MinimumPrimeBits;
		}

		// Token: 0x04001CAC RID: 7340
		public static readonly int DefaultMinimumPrimeBits = 2048;

		// Token: 0x04001CAD RID: 7341
		protected static readonly IList DefaultGroups = Platform.CreateArrayList();

		// Token: 0x04001CAE RID: 7342
		protected readonly IList mGroups;

		// Token: 0x04001CAF RID: 7343
		protected readonly int mMinimumPrimeBits;
	}
}
