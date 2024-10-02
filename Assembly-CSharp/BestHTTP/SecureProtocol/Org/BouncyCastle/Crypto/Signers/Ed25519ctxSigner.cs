using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc8032;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000497 RID: 1175
	public class Ed25519ctxSigner : ISigner
	{
		// Token: 0x06002E2A RID: 11818 RVA: 0x0011F977 File Offset: 0x0011DB77
		public Ed25519ctxSigner(byte[] context)
		{
			this.context = Arrays.Clone(context);
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06002E2B RID: 11819 RVA: 0x0011F996 File Offset: 0x0011DB96
		public virtual string AlgorithmName
		{
			get
			{
				return "Ed25519ctx";
			}
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x0011F9A0 File Offset: 0x0011DBA0
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			this.forSigning = forSigning;
			if (forSigning)
			{
				this.privateKey = (Ed25519PrivateKeyParameters)parameters;
				this.publicKey = this.privateKey.GeneratePublicKey();
			}
			else
			{
				this.privateKey = null;
				this.publicKey = (Ed25519PublicKeyParameters)parameters;
			}
			this.Reset();
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x0011F9EF File Offset: 0x0011DBEF
		public virtual void Update(byte b)
		{
			this.buffer.WriteByte(b);
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x0011F9FD File Offset: 0x0011DBFD
		public virtual void BlockUpdate(byte[] buf, int off, int len)
		{
			this.buffer.Write(buf, off, len);
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x0011FA0D File Offset: 0x0011DC0D
		public virtual byte[] GenerateSignature()
		{
			if (!this.forSigning || this.privateKey == null)
			{
				throw new InvalidOperationException("Ed25519ctxSigner not initialised for signature generation.");
			}
			return this.buffer.GenerateSignature(this.privateKey, this.publicKey, this.context);
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x0011FA47 File Offset: 0x0011DC47
		public virtual bool VerifySignature(byte[] signature)
		{
			if (this.forSigning || this.publicKey == null)
			{
				throw new InvalidOperationException("Ed25519ctxSigner not initialised for verification");
			}
			return this.buffer.VerifySignature(this.publicKey, this.context, signature);
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x0011FA7C File Offset: 0x0011DC7C
		public virtual void Reset()
		{
			this.buffer.Reset();
		}

		// Token: 0x04001ED0 RID: 7888
		private readonly Ed25519ctxSigner.Buffer buffer = new Ed25519ctxSigner.Buffer();

		// Token: 0x04001ED1 RID: 7889
		private readonly byte[] context;

		// Token: 0x04001ED2 RID: 7890
		private bool forSigning;

		// Token: 0x04001ED3 RID: 7891
		private Ed25519PrivateKeyParameters privateKey;

		// Token: 0x04001ED4 RID: 7892
		private Ed25519PublicKeyParameters publicKey;

		// Token: 0x0200094B RID: 2379
		private class Buffer : MemoryStream
		{
			// Token: 0x06004EF3 RID: 20211 RVA: 0x001B365C File Offset: 0x001B185C
			internal byte[] GenerateSignature(Ed25519PrivateKeyParameters privateKey, Ed25519PublicKeyParameters publicKey, byte[] ctx)
			{
				byte[] result;
				lock (this)
				{
					byte[] buffer = this.GetBuffer();
					int msgLen = (int)this.Position;
					byte[] array = new byte[Ed25519PrivateKeyParameters.SignatureSize];
					privateKey.Sign(Ed25519.Algorithm.Ed25519ctx, publicKey, ctx, buffer, 0, msgLen, array, 0);
					this.Reset();
					result = array;
				}
				return result;
			}

			// Token: 0x06004EF4 RID: 20212 RVA: 0x001B36C8 File Offset: 0x001B18C8
			internal bool VerifySignature(Ed25519PublicKeyParameters publicKey, byte[] ctx, byte[] signature)
			{
				bool result;
				lock (this)
				{
					byte[] buffer = this.GetBuffer();
					int mLen = (int)this.Position;
					byte[] encoded = publicKey.GetEncoded();
					bool flag2 = Ed25519.Verify(signature, 0, encoded, 0, ctx, buffer, 0, mLen);
					this.Reset();
					result = flag2;
				}
				return result;
			}

			// Token: 0x06004EF5 RID: 20213 RVA: 0x001B372C File Offset: 0x001B192C
			internal void Reset()
			{
				lock (this)
				{
					long position = this.Position;
					Array.Clear(this.GetBuffer(), 0, (int)position);
					this.Position = 0L;
				}
			}
		}
	}
}
