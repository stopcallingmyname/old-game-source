using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004BA RID: 1210
	public class X931SecureRandomBuilder
	{
		// Token: 0x06002F2F RID: 12079 RVA: 0x00124324 File Offset: 0x00122524
		public X931SecureRandomBuilder() : this(new SecureRandom(), false)
		{
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x00124332 File Offset: 0x00122532
		public X931SecureRandomBuilder(SecureRandom entropySource, bool predictionResistant)
		{
			this.mRandom = entropySource;
			this.mEntropySourceProvider = new BasicEntropySourceProvider(this.mRandom, predictionResistant);
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x00124353 File Offset: 0x00122553
		public X931SecureRandomBuilder(IEntropySourceProvider entropySourceProvider)
		{
			this.mRandom = null;
			this.mEntropySourceProvider = entropySourceProvider;
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x00124369 File Offset: 0x00122569
		public X931SecureRandomBuilder SetDateTimeVector(byte[] dateTimeVector)
		{
			this.mDateTimeVector = dateTimeVector;
			return this;
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x00124374 File Offset: 0x00122574
		public X931SecureRandom Build(IBlockCipher engine, KeyParameter key, bool predictionResistant)
		{
			if (this.mDateTimeVector == null)
			{
				this.mDateTimeVector = new byte[engine.GetBlockSize()];
				Pack.UInt64_To_BE((ulong)DateTimeUtilities.CurrentUnixMs(), this.mDateTimeVector, 0);
			}
			engine.Init(true, key);
			return new X931SecureRandom(this.mRandom, new X931Rng(engine, this.mDateTimeVector, this.mEntropySourceProvider.Get(engine.GetBlockSize() * 8)), predictionResistant);
		}

		// Token: 0x04001F83 RID: 8067
		private readonly SecureRandom mRandom;

		// Token: 0x04001F84 RID: 8068
		private IEntropySourceProvider mEntropySourceProvider;

		// Token: 0x04001F85 RID: 8069
		private byte[] mDateTimeVector;
	}
}
