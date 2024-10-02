using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.Srp
{
	// Token: 0x020005D2 RID: 1490
	public class Srp6VerifierGenerator
	{
		// Token: 0x06003921 RID: 14625 RVA: 0x00165AB8 File Offset: 0x00163CB8
		public virtual void Init(BigInteger N, BigInteger g, IDigest digest)
		{
			this.N = N;
			this.g = g;
			this.digest = digest;
		}

		// Token: 0x06003922 RID: 14626 RVA: 0x00165ACF File Offset: 0x00163CCF
		public virtual void Init(Srp6GroupParameters group, IDigest digest)
		{
			this.Init(group.N, group.G, digest);
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x00165AE4 File Offset: 0x00163CE4
		public virtual BigInteger GenerateVerifier(byte[] salt, byte[] identity, byte[] password)
		{
			BigInteger e = Srp6Utilities.CalculateX(this.digest, this.N, salt, identity, password);
			return this.g.ModPow(e, this.N);
		}

		// Token: 0x04002574 RID: 9588
		protected BigInteger N;

		// Token: 0x04002575 RID: 9589
		protected BigInteger g;

		// Token: 0x04002576 RID: 9590
		protected IDigest digest;
	}
}
