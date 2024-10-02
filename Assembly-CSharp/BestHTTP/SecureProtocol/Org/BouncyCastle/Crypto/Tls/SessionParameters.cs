using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000447 RID: 1095
	public sealed class SessionParameters
	{
		// Token: 0x06002B23 RID: 11043 RVA: 0x0011438C File Offset: 0x0011258C
		private SessionParameters(int cipherSuite, byte compressionAlgorithm, byte[] masterSecret, Certificate peerCertificate, byte[] pskIdentity, byte[] srpIdentity, byte[] encodedServerExtensions, bool extendedMasterSecret)
		{
			this.mCipherSuite = cipherSuite;
			this.mCompressionAlgorithm = compressionAlgorithm;
			this.mMasterSecret = Arrays.Clone(masterSecret);
			this.mPeerCertificate = peerCertificate;
			this.mPskIdentity = Arrays.Clone(pskIdentity);
			this.mSrpIdentity = Arrays.Clone(srpIdentity);
			this.mEncodedServerExtensions = encodedServerExtensions;
			this.mExtendedMasterSecret = extendedMasterSecret;
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x001143EB File Offset: 0x001125EB
		public void Clear()
		{
			if (this.mMasterSecret != null)
			{
				Arrays.Fill(this.mMasterSecret, 0);
			}
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x00114401 File Offset: 0x00112601
		public SessionParameters Copy()
		{
			return new SessionParameters(this.mCipherSuite, this.mCompressionAlgorithm, this.mMasterSecret, this.mPeerCertificate, this.mPskIdentity, this.mSrpIdentity, this.mEncodedServerExtensions, this.mExtendedMasterSecret);
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06002B26 RID: 11046 RVA: 0x00114438 File Offset: 0x00112638
		public int CipherSuite
		{
			get
			{
				return this.mCipherSuite;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06002B27 RID: 11047 RVA: 0x00114440 File Offset: 0x00112640
		public byte CompressionAlgorithm
		{
			get
			{
				return this.mCompressionAlgorithm;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06002B28 RID: 11048 RVA: 0x00114448 File Offset: 0x00112648
		public bool IsExtendedMasterSecret
		{
			get
			{
				return this.mExtendedMasterSecret;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06002B29 RID: 11049 RVA: 0x00114450 File Offset: 0x00112650
		public byte[] MasterSecret
		{
			get
			{
				return this.mMasterSecret;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06002B2A RID: 11050 RVA: 0x00114458 File Offset: 0x00112658
		public Certificate PeerCertificate
		{
			get
			{
				return this.mPeerCertificate;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06002B2B RID: 11051 RVA: 0x00114460 File Offset: 0x00112660
		public byte[] PskIdentity
		{
			get
			{
				return this.mPskIdentity;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06002B2C RID: 11052 RVA: 0x00114468 File Offset: 0x00112668
		public byte[] SrpIdentity
		{
			get
			{
				return this.mSrpIdentity;
			}
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x00114470 File Offset: 0x00112670
		public IDictionary ReadServerExtensions()
		{
			if (this.mEncodedServerExtensions == null)
			{
				return null;
			}
			return TlsProtocol.ReadExtensions(new MemoryStream(this.mEncodedServerExtensions, false));
		}

		// Token: 0x04001DDD RID: 7645
		private int mCipherSuite;

		// Token: 0x04001DDE RID: 7646
		private byte mCompressionAlgorithm;

		// Token: 0x04001DDF RID: 7647
		private byte[] mMasterSecret;

		// Token: 0x04001DE0 RID: 7648
		private Certificate mPeerCertificate;

		// Token: 0x04001DE1 RID: 7649
		private byte[] mPskIdentity;

		// Token: 0x04001DE2 RID: 7650
		private byte[] mSrpIdentity;

		// Token: 0x04001DE3 RID: 7651
		private byte[] mEncodedServerExtensions;

		// Token: 0x04001DE4 RID: 7652
		private bool mExtendedMasterSecret;

		// Token: 0x02000947 RID: 2375
		public sealed class Builder
		{
			// Token: 0x06004EE0 RID: 20192 RVA: 0x001B348C File Offset: 0x001B168C
			public SessionParameters Build()
			{
				this.Validate(this.mCipherSuite >= 0, "cipherSuite");
				this.Validate(this.mCompressionAlgorithm >= 0, "compressionAlgorithm");
				this.Validate(this.mMasterSecret != null, "masterSecret");
				return new SessionParameters(this.mCipherSuite, (byte)this.mCompressionAlgorithm, this.mMasterSecret, this.mPeerCertificate, this.mPskIdentity, this.mSrpIdentity, this.mEncodedServerExtensions, this.mExtendedMasterSecret);
			}

			// Token: 0x06004EE1 RID: 20193 RVA: 0x001B3511 File Offset: 0x001B1711
			public SessionParameters.Builder SetCipherSuite(int cipherSuite)
			{
				this.mCipherSuite = cipherSuite;
				return this;
			}

			// Token: 0x06004EE2 RID: 20194 RVA: 0x001B351B File Offset: 0x001B171B
			public SessionParameters.Builder SetCompressionAlgorithm(byte compressionAlgorithm)
			{
				this.mCompressionAlgorithm = (short)compressionAlgorithm;
				return this;
			}

			// Token: 0x06004EE3 RID: 20195 RVA: 0x001B3525 File Offset: 0x001B1725
			public SessionParameters.Builder SetExtendedMasterSecret(bool extendedMasterSecret)
			{
				this.mExtendedMasterSecret = extendedMasterSecret;
				return this;
			}

			// Token: 0x06004EE4 RID: 20196 RVA: 0x001B352F File Offset: 0x001B172F
			public SessionParameters.Builder SetMasterSecret(byte[] masterSecret)
			{
				this.mMasterSecret = masterSecret;
				return this;
			}

			// Token: 0x06004EE5 RID: 20197 RVA: 0x001B3539 File Offset: 0x001B1739
			public SessionParameters.Builder SetPeerCertificate(Certificate peerCertificate)
			{
				this.mPeerCertificate = peerCertificate;
				return this;
			}

			// Token: 0x06004EE6 RID: 20198 RVA: 0x001B3543 File Offset: 0x001B1743
			public SessionParameters.Builder SetPskIdentity(byte[] pskIdentity)
			{
				this.mPskIdentity = pskIdentity;
				return this;
			}

			// Token: 0x06004EE7 RID: 20199 RVA: 0x001B354D File Offset: 0x001B174D
			public SessionParameters.Builder SetSrpIdentity(byte[] srpIdentity)
			{
				this.mSrpIdentity = srpIdentity;
				return this;
			}

			// Token: 0x06004EE8 RID: 20200 RVA: 0x001B3558 File Offset: 0x001B1758
			public SessionParameters.Builder SetServerExtensions(IDictionary serverExtensions)
			{
				if (serverExtensions == null)
				{
					this.mEncodedServerExtensions = null;
				}
				else
				{
					MemoryStream memoryStream = new MemoryStream();
					TlsProtocol.WriteExtensions(memoryStream, serverExtensions);
					this.mEncodedServerExtensions = memoryStream.ToArray();
				}
				return this;
			}

			// Token: 0x06004EE9 RID: 20201 RVA: 0x001B358B File Offset: 0x001B178B
			private void Validate(bool condition, string parameter)
			{
				if (!condition)
				{
					throw new InvalidOperationException("Required session parameter '" + parameter + "' not configured");
				}
			}

			// Token: 0x04003618 RID: 13848
			private int mCipherSuite = -1;

			// Token: 0x04003619 RID: 13849
			private short mCompressionAlgorithm = -1;

			// Token: 0x0400361A RID: 13850
			private byte[] mMasterSecret;

			// Token: 0x0400361B RID: 13851
			private Certificate mPeerCertificate;

			// Token: 0x0400361C RID: 13852
			private byte[] mPskIdentity;

			// Token: 0x0400361D RID: 13853
			private byte[] mSrpIdentity;

			// Token: 0x0400361E RID: 13854
			private byte[] mEncodedServerExtensions;

			// Token: 0x0400361F RID: 13855
			private bool mExtendedMasterSecret;
		}
	}
}
