using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc8032;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000499 RID: 1177
	public class Ed25519Signer : ISigner
	{
		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06002E3B RID: 11835 RVA: 0x0011FC09 File Offset: 0x0011DE09
		public virtual string AlgorithmName
		{
			get
			{
				return "Ed25519";
			}
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x0011FC10 File Offset: 0x0011DE10
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

		// Token: 0x06002E3D RID: 11837 RVA: 0x0011FC5F File Offset: 0x0011DE5F
		public virtual void Update(byte b)
		{
			this.buffer.WriteByte(b);
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x0011FC6D File Offset: 0x0011DE6D
		public virtual void BlockUpdate(byte[] buf, int off, int len)
		{
			this.buffer.Write(buf, off, len);
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x0011FC7D File Offset: 0x0011DE7D
		public virtual byte[] GenerateSignature()
		{
			if (!this.forSigning || this.privateKey == null)
			{
				throw new InvalidOperationException("Ed25519Signer not initialised for signature generation.");
			}
			return this.buffer.GenerateSignature(this.privateKey, this.publicKey);
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x0011FCB1 File Offset: 0x0011DEB1
		public virtual bool VerifySignature(byte[] signature)
		{
			if (this.forSigning || this.publicKey == null)
			{
				throw new InvalidOperationException("Ed25519Signer not initialised for verification");
			}
			return this.buffer.VerifySignature(this.publicKey, signature);
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x0011FCE0 File Offset: 0x0011DEE0
		public virtual void Reset()
		{
			this.buffer.Reset();
		}

		// Token: 0x04001EDA RID: 7898
		private readonly Ed25519Signer.Buffer buffer = new Ed25519Signer.Buffer();

		// Token: 0x04001EDB RID: 7899
		private bool forSigning;

		// Token: 0x04001EDC RID: 7900
		private Ed25519PrivateKeyParameters privateKey;

		// Token: 0x04001EDD RID: 7901
		private Ed25519PublicKeyParameters publicKey;

		// Token: 0x0200094C RID: 2380
		private class Buffer : MemoryStream
		{
			// Token: 0x06004EF7 RID: 20215 RVA: 0x001B3780 File Offset: 0x001B1980
			internal byte[] GenerateSignature(Ed25519PrivateKeyParameters privateKey, Ed25519PublicKeyParameters publicKey)
			{
				byte[] result;
				lock (this)
				{
					byte[] buffer = this.GetBuffer();
					int msgLen = (int)this.Position;
					byte[] array = new byte[Ed25519PrivateKeyParameters.SignatureSize];
					privateKey.Sign(Ed25519.Algorithm.Ed25519, publicKey, null, buffer, 0, msgLen, array, 0);
					this.Reset();
					result = array;
				}
				return result;
			}

			// Token: 0x06004EF8 RID: 20216 RVA: 0x001B37EC File Offset: 0x001B19EC
			internal bool VerifySignature(Ed25519PublicKeyParameters publicKey, byte[] signature)
			{
				bool result;
				lock (this)
				{
					byte[] buffer = this.GetBuffer();
					int mLen = (int)this.Position;
					byte[] encoded = publicKey.GetEncoded();
					bool flag2 = Ed25519.Verify(signature, 0, encoded, 0, buffer, 0, mLen);
					this.Reset();
					result = flag2;
				}
				return result;
			}

			// Token: 0x06004EF9 RID: 20217 RVA: 0x001B3850 File Offset: 0x001B1A50
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
