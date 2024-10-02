using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200040F RID: 1039
	internal class CombinedHash : TlsHandshakeHash, IDigest
	{
		// Token: 0x060029AB RID: 10667 RVA: 0x0010EF01 File Offset: 0x0010D101
		internal CombinedHash()
		{
			this.mMd5 = TlsUtilities.CreateHash(1);
			this.mSha1 = TlsUtilities.CreateHash(2);
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x0010EF21 File Offset: 0x0010D121
		internal CombinedHash(CombinedHash t)
		{
			this.mContext = t.mContext;
			this.mMd5 = TlsUtilities.CloneHash(1, t.mMd5);
			this.mSha1 = TlsUtilities.CloneHash(2, t.mSha1);
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x0010EF59 File Offset: 0x0010D159
		public virtual void Init(TlsContext context)
		{
			this.mContext = context;
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x000947CE File Offset: 0x000929CE
		public virtual TlsHandshakeHash NotifyPrfDetermined()
		{
			return this;
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x0010EF62 File Offset: 0x0010D162
		public virtual void TrackHashAlgorithm(byte hashAlgorithm)
		{
			throw new InvalidOperationException("CombinedHash only supports calculating the legacy PRF for handshake hash");
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void SealHashAlgorithms()
		{
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x0010EF6E File Offset: 0x0010D16E
		public virtual TlsHandshakeHash StopTracking()
		{
			return new CombinedHash(this);
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x0010EF6E File Offset: 0x0010D16E
		public virtual IDigest ForkPrfHash()
		{
			return new CombinedHash(this);
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x0010EF76 File Offset: 0x0010D176
		public virtual byte[] GetFinalHash(byte hashAlgorithm)
		{
			throw new InvalidOperationException("CombinedHash doesn't support multiple hashes");
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x0010EF82 File Offset: 0x0010D182
		public virtual string AlgorithmName
		{
			get
			{
				return this.mMd5.AlgorithmName + " and " + this.mSha1.AlgorithmName;
			}
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x0010EFA4 File Offset: 0x0010D1A4
		public virtual int GetByteLength()
		{
			return Math.Max(this.mMd5.GetByteLength(), this.mSha1.GetByteLength());
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x0010EFC1 File Offset: 0x0010D1C1
		public virtual int GetDigestSize()
		{
			return this.mMd5.GetDigestSize() + this.mSha1.GetDigestSize();
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x0010EFDA File Offset: 0x0010D1DA
		public virtual void Update(byte input)
		{
			this.mMd5.Update(input);
			this.mSha1.Update(input);
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x0010EFF4 File Offset: 0x0010D1F4
		public virtual void BlockUpdate(byte[] input, int inOff, int len)
		{
			this.mMd5.BlockUpdate(input, inOff, len);
			this.mSha1.BlockUpdate(input, inOff, len);
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x0010F014 File Offset: 0x0010D214
		public virtual int DoFinal(byte[] output, int outOff)
		{
			if (this.mContext != null && TlsUtilities.IsSsl(this.mContext))
			{
				this.Ssl3Complete(this.mMd5, Ssl3Mac.IPAD, Ssl3Mac.OPAD, 48);
				this.Ssl3Complete(this.mSha1, Ssl3Mac.IPAD, Ssl3Mac.OPAD, 40);
			}
			int num = this.mMd5.DoFinal(output, outOff);
			int num2 = this.mSha1.DoFinal(output, outOff + num);
			return num + num2;
		}

		// Token: 0x060029BA RID: 10682 RVA: 0x0010F087 File Offset: 0x0010D287
		public virtual void Reset()
		{
			this.mMd5.Reset();
			this.mSha1.Reset();
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x0010F0A0 File Offset: 0x0010D2A0
		protected virtual void Ssl3Complete(IDigest d, byte[] ipad, byte[] opad, int padLength)
		{
			byte[] masterSecret = this.mContext.SecurityParameters.masterSecret;
			d.BlockUpdate(masterSecret, 0, masterSecret.Length);
			d.BlockUpdate(ipad, 0, padLength);
			byte[] array = DigestUtilities.DoFinal(d);
			d.BlockUpdate(masterSecret, 0, masterSecret.Length);
			d.BlockUpdate(opad, 0, padLength);
			d.BlockUpdate(array, 0, array.Length);
		}

		// Token: 0x04001C9B RID: 7323
		protected TlsContext mContext;

		// Token: 0x04001C9C RID: 7324
		protected IDigest mMd5;

		// Token: 0x04001C9D RID: 7325
		protected IDigest mSha1;
	}
}
