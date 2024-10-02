using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003FD RID: 1021
	public class BasicTlsPskIdentity : TlsPskIdentity
	{
		// Token: 0x0600294C RID: 10572 RVA: 0x0010DF32 File Offset: 0x0010C132
		public BasicTlsPskIdentity(byte[] identity, byte[] psk)
		{
			this.mIdentity = Arrays.Clone(identity);
			this.mPsk = Arrays.Clone(psk);
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x0010DF52 File Offset: 0x0010C152
		public BasicTlsPskIdentity(string identity, byte[] psk)
		{
			this.mIdentity = Strings.ToUtf8ByteArray(identity);
			this.mPsk = Arrays.Clone(psk);
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void SkipIdentityHint()
		{
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void NotifyIdentityHint(byte[] psk_identity_hint)
		{
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x0010DF72 File Offset: 0x0010C172
		public virtual byte[] GetPskIdentity()
		{
			return this.mIdentity;
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x0010DF7A File Offset: 0x0010C17A
		public virtual byte[] GetPsk()
		{
			return this.mPsk;
		}

		// Token: 0x04001B55 RID: 6997
		protected byte[] mIdentity;

		// Token: 0x04001B56 RID: 6998
		protected byte[] mPsk;
	}
}
