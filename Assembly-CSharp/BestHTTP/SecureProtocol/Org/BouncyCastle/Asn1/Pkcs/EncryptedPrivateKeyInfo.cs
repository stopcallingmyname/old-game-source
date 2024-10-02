using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006F2 RID: 1778
	public class EncryptedPrivateKeyInfo : Asn1Encodable
	{
		// Token: 0x06004121 RID: 16673 RVA: 0x00181BD4 File Offset: 0x0017FDD4
		private EncryptedPrivateKeyInfo(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.algId = AlgorithmIdentifier.GetInstance(seq[0]);
			this.data = Asn1OctetString.GetInstance(seq[1]);
		}

		// Token: 0x06004122 RID: 16674 RVA: 0x00181C24 File Offset: 0x0017FE24
		public EncryptedPrivateKeyInfo(AlgorithmIdentifier algId, byte[] encoding)
		{
			this.algId = algId;
			this.data = new DerOctetString(encoding);
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x00181C3F File Offset: 0x0017FE3F
		public static EncryptedPrivateKeyInfo GetInstance(object obj)
		{
			if (obj is EncryptedPrivateKeyInfo)
			{
				return (EncryptedPrivateKeyInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new EncryptedPrivateKeyInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06004124 RID: 16676 RVA: 0x00181C7E File Offset: 0x0017FE7E
		public AlgorithmIdentifier EncryptionAlgorithm
		{
			get
			{
				return this.algId;
			}
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x00181C86 File Offset: 0x0017FE86
		public byte[] GetEncryptedData()
		{
			return this.data.GetOctets();
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x00181C93 File Offset: 0x0017FE93
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.algId,
				this.data
			});
		}

		// Token: 0x040029C1 RID: 10689
		private readonly AlgorithmIdentifier algId;

		// Token: 0x040029C2 RID: 10690
		private readonly Asn1OctetString data;
	}
}
