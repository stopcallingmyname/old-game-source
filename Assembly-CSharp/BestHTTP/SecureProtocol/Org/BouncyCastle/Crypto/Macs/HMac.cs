using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x02000537 RID: 1335
	public class HMac : IMac
	{
		// Token: 0x0600328B RID: 12939 RVA: 0x0013069C File Offset: 0x0012E89C
		public HMac(IDigest digest)
		{
			this.digest = digest;
			this.digestSize = digest.GetDigestSize();
			this.blockLength = digest.GetByteLength();
			this.inputPad = new byte[this.blockLength];
			this.outputBuf = new byte[this.blockLength + this.digestSize];
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x0600328C RID: 12940 RVA: 0x001306F7 File Offset: 0x0012E8F7
		public virtual string AlgorithmName
		{
			get
			{
				return this.digest.AlgorithmName + "/HMAC";
			}
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x0013070E File Offset: 0x0012E90E
		public virtual IDigest GetUnderlyingDigest()
		{
			return this.digest;
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x00130718 File Offset: 0x0012E918
		public virtual void Init(ICipherParameters parameters)
		{
			this.digest.Reset();
			byte[] key = ((KeyParameter)parameters).GetKey();
			int num = key.Length;
			if (num > this.blockLength)
			{
				this.digest.BlockUpdate(key, 0, num);
				this.digest.DoFinal(this.inputPad, 0);
				num = this.digestSize;
			}
			else
			{
				Array.Copy(key, 0, this.inputPad, 0, num);
			}
			Array.Clear(this.inputPad, num, this.blockLength - num);
			Array.Copy(this.inputPad, 0, this.outputBuf, 0, this.blockLength);
			HMac.XorPad(this.inputPad, this.blockLength, 54);
			HMac.XorPad(this.outputBuf, this.blockLength, 92);
			if (this.digest is IMemoable)
			{
				this.opadState = ((IMemoable)this.digest).Copy();
				((IDigest)this.opadState).BlockUpdate(this.outputBuf, 0, this.blockLength);
			}
			this.digest.BlockUpdate(this.inputPad, 0, this.inputPad.Length);
			if (this.digest is IMemoable)
			{
				this.ipadState = ((IMemoable)this.digest).Copy();
			}
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x00130852 File Offset: 0x0012EA52
		public virtual int GetMacSize()
		{
			return this.digestSize;
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x0013085A File Offset: 0x0012EA5A
		public virtual void Update(byte input)
		{
			this.digest.Update(input);
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x00130868 File Offset: 0x0012EA68
		public virtual void BlockUpdate(byte[] input, int inOff, int len)
		{
			this.digest.BlockUpdate(input, inOff, len);
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x00130878 File Offset: 0x0012EA78
		public virtual int DoFinal(byte[] output, int outOff)
		{
			this.digest.DoFinal(this.outputBuf, this.blockLength);
			if (this.opadState != null)
			{
				((IMemoable)this.digest).Reset(this.opadState);
				this.digest.BlockUpdate(this.outputBuf, this.blockLength, this.digest.GetDigestSize());
			}
			else
			{
				this.digest.BlockUpdate(this.outputBuf, 0, this.outputBuf.Length);
			}
			int result = this.digest.DoFinal(output, outOff);
			Array.Clear(this.outputBuf, this.blockLength, this.digestSize);
			if (this.ipadState != null)
			{
				((IMemoable)this.digest).Reset(this.ipadState);
				return result;
			}
			this.digest.BlockUpdate(this.inputPad, 0, this.inputPad.Length);
			return result;
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x00130956 File Offset: 0x0012EB56
		public virtual void Reset()
		{
			this.digest.Reset();
			this.digest.BlockUpdate(this.inputPad, 0, this.inputPad.Length);
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x00130980 File Offset: 0x0012EB80
		private static void XorPad(byte[] pad, int len, byte n)
		{
			for (int i = 0; i < len; i++)
			{
				int num = i;
				pad[num] ^= n;
			}
		}

		// Token: 0x04002126 RID: 8486
		private const byte IPAD = 54;

		// Token: 0x04002127 RID: 8487
		private const byte OPAD = 92;

		// Token: 0x04002128 RID: 8488
		private readonly IDigest digest;

		// Token: 0x04002129 RID: 8489
		private readonly int digestSize;

		// Token: 0x0400212A RID: 8490
		private readonly int blockLength;

		// Token: 0x0400212B RID: 8491
		private IMemoable ipadState;

		// Token: 0x0400212C RID: 8492
		private IMemoable opadState;

		// Token: 0x0400212D RID: 8493
		private readonly byte[] inputPad;

		// Token: 0x0400212E RID: 8494
		private readonly byte[] outputBuf;
	}
}
