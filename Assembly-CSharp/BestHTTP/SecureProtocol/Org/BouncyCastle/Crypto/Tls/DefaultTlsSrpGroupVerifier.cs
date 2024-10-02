using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.Srp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200041B RID: 1051
	public class DefaultTlsSrpGroupVerifier : TlsSrpGroupVerifier
	{
		// Token: 0x06002A0C RID: 10764 RVA: 0x0010FC68 File Offset: 0x0010DE68
		static DefaultTlsSrpGroupVerifier()
		{
			DefaultTlsSrpGroupVerifier.DefaultGroups.Add(Srp6StandardGroups.rfc5054_1024);
			DefaultTlsSrpGroupVerifier.DefaultGroups.Add(Srp6StandardGroups.rfc5054_1536);
			DefaultTlsSrpGroupVerifier.DefaultGroups.Add(Srp6StandardGroups.rfc5054_2048);
			DefaultTlsSrpGroupVerifier.DefaultGroups.Add(Srp6StandardGroups.rfc5054_3072);
			DefaultTlsSrpGroupVerifier.DefaultGroups.Add(Srp6StandardGroups.rfc5054_4096);
			DefaultTlsSrpGroupVerifier.DefaultGroups.Add(Srp6StandardGroups.rfc5054_6144);
			DefaultTlsSrpGroupVerifier.DefaultGroups.Add(Srp6StandardGroups.rfc5054_8192);
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x0010FCEF File Offset: 0x0010DEEF
		public DefaultTlsSrpGroupVerifier() : this(DefaultTlsSrpGroupVerifier.DefaultGroups)
		{
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x0010FCFC File Offset: 0x0010DEFC
		public DefaultTlsSrpGroupVerifier(IList groups)
		{
			this.mGroups = groups;
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x0010FD0C File Offset: 0x0010DF0C
		public virtual bool Accept(Srp6GroupParameters group)
		{
			foreach (object obj in this.mGroups)
			{
				Srp6GroupParameters b = (Srp6GroupParameters)obj;
				if (this.AreGroupsEqual(group, b))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x0010FD70 File Offset: 0x0010DF70
		protected virtual bool AreGroupsEqual(Srp6GroupParameters a, Srp6GroupParameters b)
		{
			return a == b || (this.AreParametersEqual(a.N, b.N) && this.AreParametersEqual(a.G, b.G));
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x0010F7E0 File Offset: 0x0010D9E0
		protected virtual bool AreParametersEqual(BigInteger a, BigInteger b)
		{
			return a == b || a.Equals(b);
		}

		// Token: 0x04001CB8 RID: 7352
		protected static readonly IList DefaultGroups = Platform.CreateArrayList();

		// Token: 0x04001CB9 RID: 7353
		protected readonly IList mGroups;
	}
}
