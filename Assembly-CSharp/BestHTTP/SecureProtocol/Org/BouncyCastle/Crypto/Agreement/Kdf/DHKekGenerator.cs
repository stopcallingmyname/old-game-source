using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.Kdf
{
	// Token: 0x020005D5 RID: 1493
	public class DHKekGenerator : IDerivationFunction
	{
		// Token: 0x0600392E RID: 14638 RVA: 0x00165D0E File Offset: 0x00163F0E
		public DHKekGenerator(IDigest digest)
		{
			this.digest = digest;
		}

		// Token: 0x0600392F RID: 14639 RVA: 0x00165D20 File Offset: 0x00163F20
		public virtual void Init(IDerivationParameters param)
		{
			DHKdfParameters dhkdfParameters = (DHKdfParameters)param;
			this.algorithm = dhkdfParameters.Algorithm;
			this.keySize = dhkdfParameters.KeySize;
			this.z = dhkdfParameters.GetZ();
			this.partyAInfo = dhkdfParameters.GetExtraInfo();
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06003930 RID: 14640 RVA: 0x00165D64 File Offset: 0x00163F64
		public virtual IDigest Digest
		{
			get
			{
				return this.digest;
			}
		}

		// Token: 0x06003931 RID: 14641 RVA: 0x00165D6C File Offset: 0x00163F6C
		public virtual int GenerateBytes(byte[] outBytes, int outOff, int len)
		{
			if (outBytes.Length - len < outOff)
			{
				throw new DataLengthException("output buffer too small");
			}
			long num = (long)len;
			int digestSize = this.digest.GetDigestSize();
			if (num > 8589934591L)
			{
				throw new ArgumentException("Output length too large");
			}
			int num2 = (int)((num + (long)digestSize - 1L) / (long)digestSize);
			byte[] array = new byte[this.digest.GetDigestSize()];
			uint num3 = 1U;
			for (int i = 0; i < num2; i++)
			{
				this.digest.BlockUpdate(this.z, 0, this.z.Length);
				DerSequence derSequence = new DerSequence(new Asn1Encodable[]
				{
					this.algorithm,
					new DerOctetString(Pack.UInt32_To_BE(num3))
				});
				Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
				{
					derSequence
				});
				if (this.partyAInfo != null)
				{
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						new DerTaggedObject(true, 0, new DerOctetString(this.partyAInfo))
					});
				}
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 2, new DerOctetString(Pack.UInt32_To_BE((uint)this.keySize)))
				});
				byte[] derEncoded = new DerSequence(asn1EncodableVector).GetDerEncoded();
				this.digest.BlockUpdate(derEncoded, 0, derEncoded.Length);
				this.digest.DoFinal(array, 0);
				if (len > digestSize)
				{
					Array.Copy(array, 0, outBytes, outOff, digestSize);
					outOff += digestSize;
					len -= digestSize;
				}
				else
				{
					Array.Copy(array, 0, outBytes, outOff, len);
				}
				num3 += 1U;
			}
			this.digest.Reset();
			return (int)num;
		}

		// Token: 0x0400257F RID: 9599
		private readonly IDigest digest;

		// Token: 0x04002580 RID: 9600
		private DerObjectIdentifier algorithm;

		// Token: 0x04002581 RID: 9601
		private int keySize;

		// Token: 0x04002582 RID: 9602
		private byte[] z;

		// Token: 0x04002583 RID: 9603
		private byte[] partyAInfo;
	}
}
