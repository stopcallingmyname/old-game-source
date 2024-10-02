using System;
using System.Threading;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F3 RID: 1011
	internal abstract class AbstractTlsContext : TlsContext
	{
		// Token: 0x060028EC RID: 10476 RVA: 0x0010D558 File Offset: 0x0010B758
		private static long NextCounterValue()
		{
			return Interlocked.Increment(ref AbstractTlsContext.counter);
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x0010D564 File Offset: 0x0010B764
		internal AbstractTlsContext(SecureRandom secureRandom, SecurityParameters securityParameters)
		{
			IDigest digest = TlsUtilities.CreateHash(4);
			byte[] array = new byte[digest.GetDigestSize()];
			secureRandom.NextBytes(array);
			this.mNonceRandom = new DigestRandomGenerator(digest);
			this.mNonceRandom.AddSeedMaterial(AbstractTlsContext.NextCounterValue());
			this.mNonceRandom.AddSeedMaterial(Times.NanoTime());
			this.mNonceRandom.AddSeedMaterial(array);
			this.mSecureRandom = secureRandom;
			this.mSecurityParameters = securityParameters;
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x060028EE RID: 10478 RVA: 0x0010D5D7 File Offset: 0x0010B7D7
		public virtual IRandomGenerator NonceRandomGenerator
		{
			get
			{
				return this.mNonceRandom;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x060028EF RID: 10479 RVA: 0x0010D5DF File Offset: 0x0010B7DF
		public virtual SecureRandom SecureRandom
		{
			get
			{
				return this.mSecureRandom;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x060028F0 RID: 10480 RVA: 0x0010D5E7 File Offset: 0x0010B7E7
		public virtual SecurityParameters SecurityParameters
		{
			get
			{
				return this.mSecurityParameters;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x060028F1 RID: 10481
		public abstract bool IsServer { get; }

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x060028F2 RID: 10482 RVA: 0x0010D5EF File Offset: 0x0010B7EF
		public virtual ProtocolVersion ClientVersion
		{
			get
			{
				return this.mClientVersion;
			}
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x0010D5F7 File Offset: 0x0010B7F7
		internal virtual void SetClientVersion(ProtocolVersion clientVersion)
		{
			this.mClientVersion = clientVersion;
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x060028F4 RID: 10484 RVA: 0x0010D600 File Offset: 0x0010B800
		public virtual ProtocolVersion ServerVersion
		{
			get
			{
				return this.mServerVersion;
			}
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x0010D608 File Offset: 0x0010B808
		internal virtual void SetServerVersion(ProtocolVersion serverVersion)
		{
			this.mServerVersion = serverVersion;
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x060028F6 RID: 10486 RVA: 0x0010D611 File Offset: 0x0010B811
		public virtual TlsSession ResumableSession
		{
			get
			{
				return this.mSession;
			}
		}

		// Token: 0x060028F7 RID: 10487 RVA: 0x0010D619 File Offset: 0x0010B819
		internal virtual void SetResumableSession(TlsSession session)
		{
			this.mSession = session;
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x060028F8 RID: 10488 RVA: 0x0010D622 File Offset: 0x0010B822
		// (set) Token: 0x060028F9 RID: 10489 RVA: 0x0010D62A File Offset: 0x0010B82A
		public virtual object UserObject
		{
			get
			{
				return this.mUserObject;
			}
			set
			{
				this.mUserObject = value;
			}
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x0010D634 File Offset: 0x0010B834
		public virtual byte[] ExportKeyingMaterial(string asciiLabel, byte[] context_value, int length)
		{
			if (context_value != null && !TlsUtilities.IsValidUint16(context_value.Length))
			{
				throw new ArgumentException("must have length less than 2^16 (or be null)", "context_value");
			}
			SecurityParameters securityParameters = this.SecurityParameters;
			if (!securityParameters.IsExtendedMasterSecret)
			{
				throw new InvalidOperationException("cannot export keying material without extended_master_secret");
			}
			byte[] clientRandom = securityParameters.ClientRandom;
			byte[] serverRandom = securityParameters.ServerRandom;
			int num = clientRandom.Length + serverRandom.Length;
			if (context_value != null)
			{
				num += 2 + context_value.Length;
			}
			byte[] array = new byte[num];
			int num2 = 0;
			Array.Copy(clientRandom, 0, array, num2, clientRandom.Length);
			num2 += clientRandom.Length;
			Array.Copy(serverRandom, 0, array, num2, serverRandom.Length);
			num2 += serverRandom.Length;
			if (context_value != null)
			{
				TlsUtilities.WriteUint16(context_value.Length, array, num2);
				num2 += 2;
				Array.Copy(context_value, 0, array, num2, context_value.Length);
				num2 += context_value.Length;
			}
			if (num2 != num)
			{
				throw new InvalidOperationException("error in calculation of seed for export");
			}
			return TlsUtilities.PRF(this, securityParameters.MasterSecret, asciiLabel, array, length);
		}

		// Token: 0x04001B16 RID: 6934
		private static long counter = Times.NanoTime();

		// Token: 0x04001B17 RID: 6935
		private readonly IRandomGenerator mNonceRandom;

		// Token: 0x04001B18 RID: 6936
		private readonly SecureRandom mSecureRandom;

		// Token: 0x04001B19 RID: 6937
		private readonly SecurityParameters mSecurityParameters;

		// Token: 0x04001B1A RID: 6938
		private ProtocolVersion mClientVersion;

		// Token: 0x04001B1B RID: 6939
		private ProtocolVersion mServerVersion;

		// Token: 0x04001B1C RID: 6940
		private TlsSession mSession;

		// Token: 0x04001B1D RID: 6941
		private object mUserObject;
	}
}
