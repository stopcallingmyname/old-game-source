using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200041C RID: 1052
	internal class DeferredHash : TlsHandshakeHash, IDigest
	{
		// Token: 0x06002A12 RID: 10770 RVA: 0x0010FDA0 File Offset: 0x0010DFA0
		internal DeferredHash()
		{
			this.mBuf = new DigestInputBuffer();
			this.mHashes = Platform.CreateHashtable();
			this.mPrfHashAlgorithm = -1;
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x0010FDC5 File Offset: 0x0010DFC5
		private DeferredHash(byte prfHashAlgorithm, IDigest prfHash)
		{
			this.mBuf = null;
			this.mHashes = Platform.CreateHashtable();
			this.mPrfHashAlgorithm = (int)prfHashAlgorithm;
			this.mHashes[prfHashAlgorithm] = prfHash;
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x0010FDF8 File Offset: 0x0010DFF8
		public virtual void Init(TlsContext context)
		{
			this.mContext = context;
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x0010FE04 File Offset: 0x0010E004
		public virtual TlsHandshakeHash NotifyPrfDetermined()
		{
			int prfAlgorithm = this.mContext.SecurityParameters.PrfAlgorithm;
			if (prfAlgorithm == 0)
			{
				CombinedHash combinedHash = new CombinedHash();
				combinedHash.Init(this.mContext);
				this.mBuf.UpdateDigest(combinedHash);
				return combinedHash.NotifyPrfDetermined();
			}
			this.mPrfHashAlgorithm = (int)TlsUtilities.GetHashAlgorithmForPrfAlgorithm(prfAlgorithm);
			this.CheckTrackingHash((byte)this.mPrfHashAlgorithm);
			return this;
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x0010FE64 File Offset: 0x0010E064
		public virtual void TrackHashAlgorithm(byte hashAlgorithm)
		{
			if (this.mBuf == null)
			{
				throw new InvalidOperationException("Too late to track more hash algorithms");
			}
			this.CheckTrackingHash(hashAlgorithm);
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x0010FE80 File Offset: 0x0010E080
		public virtual void SealHashAlgorithms()
		{
			this.CheckStopBuffering();
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x0010FE88 File Offset: 0x0010E088
		public virtual TlsHandshakeHash StopTracking()
		{
			byte b = (byte)this.mPrfHashAlgorithm;
			IDigest digest = TlsUtilities.CloneHash(b, (IDigest)this.mHashes[b]);
			if (this.mBuf != null)
			{
				this.mBuf.UpdateDigest(digest);
			}
			DeferredHash deferredHash = new DeferredHash(b, digest);
			deferredHash.Init(this.mContext);
			return deferredHash;
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x0010FEE4 File Offset: 0x0010E0E4
		public virtual IDigest ForkPrfHash()
		{
			this.CheckStopBuffering();
			byte b = (byte)this.mPrfHashAlgorithm;
			if (this.mBuf != null)
			{
				IDigest digest = TlsUtilities.CreateHash(b);
				this.mBuf.UpdateDigest(digest);
				return digest;
			}
			return TlsUtilities.CloneHash(b, (IDigest)this.mHashes[b]);
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x0010FF38 File Offset: 0x0010E138
		public virtual byte[] GetFinalHash(byte hashAlgorithm)
		{
			IDigest digest = (IDigest)this.mHashes[hashAlgorithm];
			if (digest == null)
			{
				throw new InvalidOperationException("HashAlgorithm." + HashAlgorithm.GetText(hashAlgorithm) + " is not being tracked");
			}
			digest = TlsUtilities.CloneHash(hashAlgorithm, digest);
			if (this.mBuf != null)
			{
				this.mBuf.UpdateDigest(digest);
			}
			return DigestUtilities.DoFinal(digest);
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06002A1B RID: 10779 RVA: 0x0010FF9C File Offset: 0x0010E19C
		public virtual string AlgorithmName
		{
			get
			{
				throw new InvalidOperationException("Use Fork() to get a definite IDigest");
			}
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x0010FF9C File Offset: 0x0010E19C
		public virtual int GetByteLength()
		{
			throw new InvalidOperationException("Use Fork() to get a definite IDigest");
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x0010FF9C File Offset: 0x0010E19C
		public virtual int GetDigestSize()
		{
			throw new InvalidOperationException("Use Fork() to get a definite IDigest");
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x0010FFA8 File Offset: 0x0010E1A8
		public virtual void Update(byte input)
		{
			if (this.mBuf != null)
			{
				this.mBuf.WriteByte(input);
				return;
			}
			foreach (object obj in this.mHashes.Values)
			{
				((IDigest)obj).Update(input);
			}
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x0011001C File Offset: 0x0010E21C
		public virtual void BlockUpdate(byte[] input, int inOff, int len)
		{
			if (this.mBuf != null)
			{
				this.mBuf.Write(input, inOff, len);
				return;
			}
			foreach (object obj in this.mHashes.Values)
			{
				((IDigest)obj).BlockUpdate(input, inOff, len);
			}
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x0010FF9C File Offset: 0x0010E19C
		public virtual int DoFinal(byte[] output, int outOff)
		{
			throw new InvalidOperationException("Use Fork() to get a definite IDigest");
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x00110094 File Offset: 0x0010E294
		public virtual void Reset()
		{
			if (this.mBuf != null)
			{
				this.mBuf.SetLength(0L);
				return;
			}
			foreach (object obj in this.mHashes.Values)
			{
				((IDigest)obj).Reset();
			}
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x00110108 File Offset: 0x0010E308
		protected virtual void CheckStopBuffering()
		{
			if (this.mBuf != null && this.mHashes.Count <= 4)
			{
				foreach (object obj in this.mHashes.Values)
				{
					IDigest d = (IDigest)obj;
					this.mBuf.UpdateDigest(d);
				}
				this.mBuf = null;
			}
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x00110188 File Offset: 0x0010E388
		protected virtual void CheckTrackingHash(byte hashAlgorithm)
		{
			if (!this.mHashes.Contains(hashAlgorithm))
			{
				IDigest value = TlsUtilities.CreateHash(hashAlgorithm);
				this.mHashes[hashAlgorithm] = value;
			}
		}

		// Token: 0x04001CBA RID: 7354
		protected const int BUFFERING_HASH_LIMIT = 4;

		// Token: 0x04001CBB RID: 7355
		protected TlsContext mContext;

		// Token: 0x04001CBC RID: 7356
		private DigestInputBuffer mBuf;

		// Token: 0x04001CBD RID: 7357
		private IDictionary mHashes;

		// Token: 0x04001CBE RID: 7358
		private int mPrfHashAlgorithm;
	}
}
