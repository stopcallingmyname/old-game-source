using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200044F RID: 1103
	public class Ssl3Mac : IMac
	{
		// Token: 0x06002B51 RID: 11089 RVA: 0x001148D5 File Offset: 0x00112AD5
		public Ssl3Mac(IDigest digest)
		{
			this.digest = digest;
			if (digest.GetDigestSize() == 20)
			{
				this.padLength = 40;
				return;
			}
			this.padLength = 48;
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06002B52 RID: 11090 RVA: 0x001148FF File Offset: 0x00112AFF
		public virtual string AlgorithmName
		{
			get
			{
				return this.digest.AlgorithmName + "/SSL3MAC";
			}
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x00114916 File Offset: 0x00112B16
		public virtual void Init(ICipherParameters parameters)
		{
			this.secret = Arrays.Clone(((KeyParameter)parameters).GetKey());
			this.Reset();
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x00114934 File Offset: 0x00112B34
		public virtual int GetMacSize()
		{
			return this.digest.GetDigestSize();
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x00114941 File Offset: 0x00112B41
		public virtual void Update(byte input)
		{
			this.digest.Update(input);
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x0011494F File Offset: 0x00112B4F
		public virtual void BlockUpdate(byte[] input, int inOff, int len)
		{
			this.digest.BlockUpdate(input, inOff, len);
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x00114960 File Offset: 0x00112B60
		public virtual int DoFinal(byte[] output, int outOff)
		{
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			this.digest.BlockUpdate(this.secret, 0, this.secret.Length);
			this.digest.BlockUpdate(Ssl3Mac.OPAD, 0, this.padLength);
			this.digest.BlockUpdate(array, 0, array.Length);
			int result = this.digest.DoFinal(output, outOff);
			this.Reset();
			return result;
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x001149E0 File Offset: 0x00112BE0
		public virtual void Reset()
		{
			this.digest.Reset();
			this.digest.BlockUpdate(this.secret, 0, this.secret.Length);
			this.digest.BlockUpdate(Ssl3Mac.IPAD, 0, this.padLength);
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x00114A1E File Offset: 0x00112C1E
		private static byte[] GenPad(byte b, int count)
		{
			byte[] array = new byte[count];
			Arrays.Fill(array, b);
			return array;
		}

		// Token: 0x04001DFC RID: 7676
		private const byte IPAD_BYTE = 54;

		// Token: 0x04001DFD RID: 7677
		private const byte OPAD_BYTE = 92;

		// Token: 0x04001DFE RID: 7678
		internal static readonly byte[] IPAD = Ssl3Mac.GenPad(54, 48);

		// Token: 0x04001DFF RID: 7679
		internal static readonly byte[] OPAD = Ssl3Mac.GenPad(92, 48);

		// Token: 0x04001E00 RID: 7680
		private readonly IDigest digest;

		// Token: 0x04001E01 RID: 7681
		private readonly int padLength;

		// Token: 0x04001E02 RID: 7682
		private byte[] secret;
	}
}
