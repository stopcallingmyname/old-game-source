using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp
{
	// Token: 0x020006DA RID: 1754
	public class MessageImprint : Asn1Encodable
	{
		// Token: 0x06004088 RID: 16520 RVA: 0x0017F7CB File Offset: 0x0017D9CB
		public static MessageImprint GetInstance(object o)
		{
			if (o == null || o is MessageImprint)
			{
				return (MessageImprint)o;
			}
			if (o is Asn1Sequence)
			{
				return new MessageImprint((Asn1Sequence)o);
			}
			throw new ArgumentException("Unknown object in 'MessageImprint' factory: " + Platform.GetTypeName(o));
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x0017F808 File Offset: 0x0017DA08
		private MessageImprint(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.hashAlgorithm = AlgorithmIdentifier.GetInstance(seq[0]);
			this.hashedMessage = Asn1OctetString.GetInstance(seq[1]).GetOctets();
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x0017F85D File Offset: 0x0017DA5D
		public MessageImprint(AlgorithmIdentifier hashAlgorithm, byte[] hashedMessage)
		{
			this.hashAlgorithm = hashAlgorithm;
			this.hashedMessage = hashedMessage;
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x0600408B RID: 16523 RVA: 0x0017F873 File Offset: 0x0017DA73
		public AlgorithmIdentifier HashAlgorithm
		{
			get
			{
				return this.hashAlgorithm;
			}
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x0017F87B File Offset: 0x0017DA7B
		public byte[] GetHashedMessage()
		{
			return this.hashedMessage;
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x0017F883 File Offset: 0x0017DA83
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.hashAlgorithm,
				new DerOctetString(this.hashedMessage)
			});
		}

		// Token: 0x0400292B RID: 10539
		private readonly AlgorithmIdentifier hashAlgorithm;

		// Token: 0x0400292C RID: 10540
		private readonly byte[] hashedMessage;
	}
}
