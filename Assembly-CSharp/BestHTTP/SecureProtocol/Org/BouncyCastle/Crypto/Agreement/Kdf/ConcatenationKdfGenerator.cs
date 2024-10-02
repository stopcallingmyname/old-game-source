using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.Kdf
{
	// Token: 0x020005D3 RID: 1491
	public class ConcatenationKdfGenerator : IDerivationFunction
	{
		// Token: 0x06003924 RID: 14628 RVA: 0x00165B18 File Offset: 0x00163D18
		public ConcatenationKdfGenerator(IDigest digest)
		{
			this.mDigest = digest;
			this.mHLen = digest.GetDigestSize();
		}

		// Token: 0x06003925 RID: 14629 RVA: 0x00165B34 File Offset: 0x00163D34
		public virtual void Init(IDerivationParameters param)
		{
			if (!(param is KdfParameters))
			{
				throw new ArgumentException("KDF parameters required for ConcatenationKdfGenerator");
			}
			KdfParameters kdfParameters = (KdfParameters)param;
			this.mShared = kdfParameters.GetSharedSecret();
			this.mOtherInfo = kdfParameters.GetIV();
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06003926 RID: 14630 RVA: 0x00165B73 File Offset: 0x00163D73
		public virtual IDigest Digest
		{
			get
			{
				return this.mDigest;
			}
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x00165B7C File Offset: 0x00163D7C
		public virtual int GenerateBytes(byte[] outBytes, int outOff, int len)
		{
			if (outBytes.Length - len < outOff)
			{
				throw new DataLengthException("output buffer too small");
			}
			byte[] array = new byte[this.mHLen];
			byte[] array2 = new byte[4];
			uint n = 1U;
			int num = 0;
			this.mDigest.Reset();
			if (len > this.mHLen)
			{
				do
				{
					Pack.UInt32_To_BE(n, array2);
					this.mDigest.BlockUpdate(array2, 0, array2.Length);
					this.mDigest.BlockUpdate(this.mShared, 0, this.mShared.Length);
					this.mDigest.BlockUpdate(this.mOtherInfo, 0, this.mOtherInfo.Length);
					this.mDigest.DoFinal(array, 0);
					Array.Copy(array, 0, outBytes, outOff + num, this.mHLen);
					num += this.mHLen;
				}
				while ((ulong)n++ < (ulong)((long)(len / this.mHLen)));
			}
			if (num < len)
			{
				Pack.UInt32_To_BE(n, array2);
				this.mDigest.BlockUpdate(array2, 0, array2.Length);
				this.mDigest.BlockUpdate(this.mShared, 0, this.mShared.Length);
				this.mDigest.BlockUpdate(this.mOtherInfo, 0, this.mOtherInfo.Length);
				this.mDigest.DoFinal(array, 0);
				Array.Copy(array, 0, outBytes, outOff + num, len - num);
			}
			return len;
		}

		// Token: 0x04002577 RID: 9591
		private readonly IDigest mDigest;

		// Token: 0x04002578 RID: 9592
		private byte[] mShared;

		// Token: 0x04002579 RID: 9593
		private byte[] mOtherInfo;

		// Token: 0x0400257A RID: 9594
		private int mHLen;
	}
}
