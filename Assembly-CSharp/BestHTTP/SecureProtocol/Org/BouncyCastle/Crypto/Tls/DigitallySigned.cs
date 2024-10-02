using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200041E RID: 1054
	public class DigitallySigned
	{
		// Token: 0x06002A26 RID: 10790 RVA: 0x001101D7 File Offset: 0x0010E3D7
		public DigitallySigned(SignatureAndHashAlgorithm algorithm, byte[] signature)
		{
			if (signature == null)
			{
				throw new ArgumentNullException("signature");
			}
			this.mAlgorithm = algorithm;
			this.mSignature = signature;
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06002A27 RID: 10791 RVA: 0x001101FB File Offset: 0x0010E3FB
		public virtual SignatureAndHashAlgorithm Algorithm
		{
			get
			{
				return this.mAlgorithm;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06002A28 RID: 10792 RVA: 0x00110203 File Offset: 0x0010E403
		public virtual byte[] Signature
		{
			get
			{
				return this.mSignature;
			}
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x0011020B File Offset: 0x0010E40B
		public virtual void Encode(Stream output)
		{
			if (this.mAlgorithm != null)
			{
				this.mAlgorithm.Encode(output);
			}
			TlsUtilities.WriteOpaque16(this.mSignature, output);
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x00110230 File Offset: 0x0010E430
		public static DigitallySigned Parse(TlsContext context, Stream input)
		{
			SignatureAndHashAlgorithm algorithm = null;
			if (TlsUtilities.IsTlsV12(context))
			{
				algorithm = SignatureAndHashAlgorithm.Parse(input);
			}
			byte[] signature = TlsUtilities.ReadOpaque16(input);
			return new DigitallySigned(algorithm, signature);
		}

		// Token: 0x04001CBF RID: 7359
		protected readonly SignatureAndHashAlgorithm mAlgorithm;

		// Token: 0x04001CC0 RID: 7360
		protected readonly byte[] mSignature;
	}
}
