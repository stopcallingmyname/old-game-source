using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000489 RID: 1161
	public class TlsSrpLoginParameters
	{
		// Token: 0x06002D4A RID: 11594 RVA: 0x0011BA19 File Offset: 0x00119C19
		public TlsSrpLoginParameters(Srp6GroupParameters group, BigInteger verifier, byte[] salt)
		{
			this.mGroup = group;
			this.mVerifier = verifier;
			this.mSalt = salt;
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06002D4B RID: 11595 RVA: 0x0011BA36 File Offset: 0x00119C36
		public virtual Srp6GroupParameters Group
		{
			get
			{
				return this.mGroup;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06002D4C RID: 11596 RVA: 0x0011BA3E File Offset: 0x00119C3E
		public virtual byte[] Salt
		{
			get
			{
				return this.mSalt;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06002D4D RID: 11597 RVA: 0x0011BA46 File Offset: 0x00119C46
		public virtual BigInteger Verifier
		{
			get
			{
				return this.mVerifier;
			}
		}

		// Token: 0x04001EAA RID: 7850
		protected readonly Srp6GroupParameters mGroup;

		// Token: 0x04001EAB RID: 7851
		protected readonly BigInteger mVerifier;

		// Token: 0x04001EAC RID: 7852
		protected readonly byte[] mSalt;
	}
}
