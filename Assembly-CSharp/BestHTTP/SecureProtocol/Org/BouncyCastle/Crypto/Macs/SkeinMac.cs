using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x0200053B RID: 1339
	public class SkeinMac : IMac
	{
		// Token: 0x060032B9 RID: 12985 RVA: 0x00131B5F File Offset: 0x0012FD5F
		public SkeinMac(int stateSizeBits, int digestSizeBits)
		{
			this.engine = new SkeinEngine(stateSizeBits, digestSizeBits);
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x00131B74 File Offset: 0x0012FD74
		public SkeinMac(SkeinMac mac)
		{
			this.engine = new SkeinEngine(mac.engine);
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x060032BB RID: 12987 RVA: 0x00131B90 File Offset: 0x0012FD90
		public string AlgorithmName
		{
			get
			{
				return string.Concat(new object[]
				{
					"Skein-MAC-",
					this.engine.BlockSize * 8,
					"-",
					this.engine.OutputSize * 8
				});
			}
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x00131BE4 File Offset: 0x0012FDE4
		public void Init(ICipherParameters parameters)
		{
			SkeinParameters skeinParameters;
			if (parameters is SkeinParameters)
			{
				skeinParameters = (SkeinParameters)parameters;
			}
			else
			{
				if (!(parameters is KeyParameter))
				{
					throw new ArgumentException("Invalid parameter passed to Skein MAC init - " + Platform.GetTypeName(parameters));
				}
				skeinParameters = new SkeinParameters.Builder().SetKey(((KeyParameter)parameters).GetKey()).Build();
			}
			if (skeinParameters.GetKey() == null)
			{
				throw new ArgumentException("Skein MAC requires a key parameter.");
			}
			this.engine.Init(skeinParameters);
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x00131C5C File Offset: 0x0012FE5C
		public int GetMacSize()
		{
			return this.engine.OutputSize;
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x00131C69 File Offset: 0x0012FE69
		public void Reset()
		{
			this.engine.Reset();
		}

		// Token: 0x060032BF RID: 12991 RVA: 0x00131C76 File Offset: 0x0012FE76
		public void Update(byte inByte)
		{
			this.engine.Update(inByte);
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x00131C84 File Offset: 0x0012FE84
		public void BlockUpdate(byte[] input, int inOff, int len)
		{
			this.engine.Update(input, inOff, len);
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x00131C94 File Offset: 0x0012FE94
		public int DoFinal(byte[] output, int outOff)
		{
			return this.engine.DoFinal(output, outOff);
		}

		// Token: 0x04002159 RID: 8537
		public const int SKEIN_256 = 256;

		// Token: 0x0400215A RID: 8538
		public const int SKEIN_512 = 512;

		// Token: 0x0400215B RID: 8539
		public const int SKEIN_1024 = 1024;

		// Token: 0x0400215C RID: 8540
		private readonly SkeinEngine engine;
	}
}
