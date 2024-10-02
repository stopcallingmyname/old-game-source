using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc8032;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x0200049B RID: 1179
	public class Ed448Signer : ISigner
	{
		// Token: 0x06002E4A RID: 11850 RVA: 0x0011FE5E File Offset: 0x0011E05E
		public Ed448Signer(byte[] context)
		{
			this.context = Arrays.Clone(context);
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06002E4B RID: 11851 RVA: 0x0011FE7D File Offset: 0x0011E07D
		public virtual string AlgorithmName
		{
			get
			{
				return "Ed448";
			}
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x0011FE84 File Offset: 0x0011E084
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			this.forSigning = forSigning;
			if (forSigning)
			{
				this.privateKey = (Ed448PrivateKeyParameters)parameters;
				this.publicKey = this.privateKey.GeneratePublicKey();
			}
			else
			{
				this.privateKey = null;
				this.publicKey = (Ed448PublicKeyParameters)parameters;
			}
			this.Reset();
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x0011FED3 File Offset: 0x0011E0D3
		public virtual void Update(byte b)
		{
			this.buffer.WriteByte(b);
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x0011FEE1 File Offset: 0x0011E0E1
		public virtual void BlockUpdate(byte[] buf, int off, int len)
		{
			this.buffer.Write(buf, off, len);
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x0011FEF1 File Offset: 0x0011E0F1
		public virtual byte[] GenerateSignature()
		{
			if (!this.forSigning || this.privateKey == null)
			{
				throw new InvalidOperationException("Ed448Signer not initialised for signature generation.");
			}
			return this.buffer.GenerateSignature(this.privateKey, this.publicKey, this.context);
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x0011FF2B File Offset: 0x0011E12B
		public virtual bool VerifySignature(byte[] signature)
		{
			if (this.forSigning || this.publicKey == null)
			{
				throw new InvalidOperationException("Ed448Signer not initialised for verification");
			}
			return this.buffer.VerifySignature(this.publicKey, this.context, signature);
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x0011FF60 File Offset: 0x0011E160
		public virtual void Reset()
		{
			this.buffer.Reset();
		}

		// Token: 0x04001EE3 RID: 7907
		private readonly Ed448Signer.Buffer buffer = new Ed448Signer.Buffer();

		// Token: 0x04001EE4 RID: 7908
		private readonly byte[] context;

		// Token: 0x04001EE5 RID: 7909
		private bool forSigning;

		// Token: 0x04001EE6 RID: 7910
		private Ed448PrivateKeyParameters privateKey;

		// Token: 0x04001EE7 RID: 7911
		private Ed448PublicKeyParameters publicKey;

		// Token: 0x0200094D RID: 2381
		private class Buffer : MemoryStream
		{
			// Token: 0x06004EFB RID: 20219 RVA: 0x001B38A4 File Offset: 0x001B1AA4
			internal byte[] GenerateSignature(Ed448PrivateKeyParameters privateKey, Ed448PublicKeyParameters publicKey, byte[] ctx)
			{
				byte[] result;
				lock (this)
				{
					byte[] buffer = this.GetBuffer();
					int msgLen = (int)this.Position;
					byte[] array = new byte[Ed448PrivateKeyParameters.SignatureSize];
					privateKey.Sign(Ed448.Algorithm.Ed448, publicKey, ctx, buffer, 0, msgLen, array, 0);
					this.Reset();
					result = array;
				}
				return result;
			}

			// Token: 0x06004EFC RID: 20220 RVA: 0x001B3910 File Offset: 0x001B1B10
			internal bool VerifySignature(Ed448PublicKeyParameters publicKey, byte[] ctx, byte[] signature)
			{
				bool result;
				lock (this)
				{
					byte[] buffer = this.GetBuffer();
					int mLen = (int)this.Position;
					byte[] encoded = publicKey.GetEncoded();
					bool flag2 = Ed448.Verify(signature, 0, encoded, 0, ctx, buffer, 0, mLen);
					this.Reset();
					result = flag2;
				}
				return result;
			}

			// Token: 0x06004EFD RID: 20221 RVA: 0x001B3974 File Offset: 0x001B1B74
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
