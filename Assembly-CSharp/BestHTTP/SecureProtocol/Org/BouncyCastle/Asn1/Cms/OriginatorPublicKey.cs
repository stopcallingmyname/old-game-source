using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000796 RID: 1942
	public class OriginatorPublicKey : Asn1Encodable
	{
		// Token: 0x0600457A RID: 17786 RVA: 0x00190A26 File Offset: 0x0018EC26
		public OriginatorPublicKey(AlgorithmIdentifier algorithm, byte[] publicKey)
		{
			this.mAlgorithm = algorithm;
			this.mPublicKey = new DerBitString(publicKey);
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x00190A41 File Offset: 0x0018EC41
		[Obsolete("Use 'GetInstance' instead")]
		public OriginatorPublicKey(Asn1Sequence seq)
		{
			this.mAlgorithm = AlgorithmIdentifier.GetInstance(seq[0]);
			this.mPublicKey = DerBitString.GetInstance(seq[1]);
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x00190A6D File Offset: 0x0018EC6D
		public static OriginatorPublicKey GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return OriginatorPublicKey.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x00190A7B File Offset: 0x0018EC7B
		public static OriginatorPublicKey GetInstance(object obj)
		{
			if (obj == null || obj is OriginatorPublicKey)
			{
				return (OriginatorPublicKey)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OriginatorPublicKey(Asn1Sequence.GetInstance(obj));
			}
			throw new ArgumentException("Invalid OriginatorPublicKey: " + Platform.GetTypeName(obj));
		}

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x0600457E RID: 17790 RVA: 0x00190AB8 File Offset: 0x0018ECB8
		public AlgorithmIdentifier Algorithm
		{
			get
			{
				return this.mAlgorithm;
			}
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x0600457F RID: 17791 RVA: 0x00190AC0 File Offset: 0x0018ECC0
		public DerBitString PublicKey
		{
			get
			{
				return this.mPublicKey;
			}
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x00190AC8 File Offset: 0x0018ECC8
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.mAlgorithm,
				this.mPublicKey
			});
		}

		// Token: 0x04002D61 RID: 11617
		private readonly AlgorithmIdentifier mAlgorithm;

		// Token: 0x04002D62 RID: 11618
		private readonly DerBitString mPublicKey;
	}
}
