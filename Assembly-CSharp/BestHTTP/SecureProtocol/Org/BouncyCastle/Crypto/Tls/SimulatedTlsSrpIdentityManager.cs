using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.Srp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200044B RID: 1099
	public class SimulatedTlsSrpIdentityManager : TlsSrpIdentityManager
	{
		// Token: 0x06002B38 RID: 11064 RVA: 0x0011459C File Offset: 0x0011279C
		public static SimulatedTlsSrpIdentityManager GetRfc5054Default(Srp6GroupParameters group, byte[] seedKey)
		{
			Srp6VerifierGenerator srp6VerifierGenerator = new Srp6VerifierGenerator();
			srp6VerifierGenerator.Init(group, TlsUtilities.CreateHash(2));
			HMac hmac = new HMac(TlsUtilities.CreateHash(2));
			hmac.Init(new KeyParameter(seedKey));
			return new SimulatedTlsSrpIdentityManager(group, srp6VerifierGenerator, hmac);
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x001145DC File Offset: 0x001127DC
		public SimulatedTlsSrpIdentityManager(Srp6GroupParameters group, Srp6VerifierGenerator verifierGenerator, IMac mac)
		{
			this.mGroup = group;
			this.mVerifierGenerator = verifierGenerator;
			this.mMac = mac;
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x001145FC File Offset: 0x001127FC
		public virtual TlsSrpLoginParameters GetLoginParameters(byte[] identity)
		{
			this.mMac.BlockUpdate(SimulatedTlsSrpIdentityManager.PREFIX_SALT, 0, SimulatedTlsSrpIdentityManager.PREFIX_SALT.Length);
			this.mMac.BlockUpdate(identity, 0, identity.Length);
			byte[] array = new byte[this.mMac.GetMacSize()];
			this.mMac.DoFinal(array, 0);
			this.mMac.BlockUpdate(SimulatedTlsSrpIdentityManager.PREFIX_PASSWORD, 0, SimulatedTlsSrpIdentityManager.PREFIX_PASSWORD.Length);
			this.mMac.BlockUpdate(identity, 0, identity.Length);
			byte[] array2 = new byte[this.mMac.GetMacSize()];
			this.mMac.DoFinal(array2, 0);
			BigInteger verifier = this.mVerifierGenerator.GenerateVerifier(array, identity, array2);
			return new TlsSrpLoginParameters(this.mGroup, verifier, array);
		}

		// Token: 0x04001DEB RID: 7659
		private static readonly byte[] PREFIX_PASSWORD = Strings.ToByteArray("password");

		// Token: 0x04001DEC RID: 7660
		private static readonly byte[] PREFIX_SALT = Strings.ToByteArray("salt");

		// Token: 0x04001DED RID: 7661
		protected readonly Srp6GroupParameters mGroup;

		// Token: 0x04001DEE RID: 7662
		protected readonly Srp6VerifierGenerator mVerifierGenerator;

		// Token: 0x04001DEF RID: 7663
		protected readonly IMac mMac;
	}
}
