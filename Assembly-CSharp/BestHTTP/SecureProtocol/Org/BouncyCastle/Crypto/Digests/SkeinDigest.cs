using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005BE RID: 1470
	public class SkeinDigest : IDigest, IMemoable
	{
		// Token: 0x06003865 RID: 14437 RVA: 0x00161AEB File Offset: 0x0015FCEB
		public SkeinDigest(int stateSizeBits, int digestSizeBits)
		{
			this.engine = new SkeinEngine(stateSizeBits, digestSizeBits);
			this.Init(null);
		}

		// Token: 0x06003866 RID: 14438 RVA: 0x00161B07 File Offset: 0x0015FD07
		public SkeinDigest(SkeinDigest digest)
		{
			this.engine = new SkeinEngine(digest.engine);
		}

		// Token: 0x06003867 RID: 14439 RVA: 0x00161B20 File Offset: 0x0015FD20
		public void Reset(IMemoable other)
		{
			SkeinDigest skeinDigest = (SkeinDigest)other;
			this.engine.Reset(skeinDigest.engine);
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x00161B45 File Offset: 0x0015FD45
		public IMemoable Copy()
		{
			return new SkeinDigest(this);
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06003869 RID: 14441 RVA: 0x00161B50 File Offset: 0x0015FD50
		public string AlgorithmName
		{
			get
			{
				return string.Concat(new object[]
				{
					"Skein-",
					this.engine.BlockSize * 8,
					"-",
					this.engine.OutputSize * 8
				});
			}
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x00161BA2 File Offset: 0x0015FDA2
		public int GetDigestSize()
		{
			return this.engine.OutputSize;
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x00161BAF File Offset: 0x0015FDAF
		public int GetByteLength()
		{
			return this.engine.BlockSize;
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x00161BBC File Offset: 0x0015FDBC
		public void Init(SkeinParameters parameters)
		{
			this.engine.Init(parameters);
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x00161BCA File Offset: 0x0015FDCA
		public void Reset()
		{
			this.engine.Reset();
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x00161BD7 File Offset: 0x0015FDD7
		public void Update(byte inByte)
		{
			this.engine.Update(inByte);
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x00161BE5 File Offset: 0x0015FDE5
		public void BlockUpdate(byte[] inBytes, int inOff, int len)
		{
			this.engine.Update(inBytes, inOff, len);
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x00161BF5 File Offset: 0x0015FDF5
		public int DoFinal(byte[] outBytes, int outOff)
		{
			return this.engine.DoFinal(outBytes, outOff);
		}

		// Token: 0x040024B9 RID: 9401
		public const int SKEIN_256 = 256;

		// Token: 0x040024BA RID: 9402
		public const int SKEIN_512 = 512;

		// Token: 0x040024BB RID: 9403
		public const int SKEIN_1024 = 1024;

		// Token: 0x040024BC RID: 9404
		private readonly SkeinEngine engine;
	}
}
