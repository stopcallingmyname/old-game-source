using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006F1 RID: 1777
	public class EncryptedData : Asn1Encodable
	{
		// Token: 0x0600411A RID: 16666 RVA: 0x00181A92 File Offset: 0x0017FC92
		public static EncryptedData GetInstance(object obj)
		{
			if (obj is EncryptedData)
			{
				return (EncryptedData)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new EncryptedData((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x00181AD4 File Offset: 0x0017FCD4
		private EncryptedData(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			if (((DerInteger)seq[0]).Value.IntValue != 0)
			{
				throw new ArgumentException("sequence not version 0");
			}
			this.data = (Asn1Sequence)seq[1];
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x00181B35 File Offset: 0x0017FD35
		public EncryptedData(DerObjectIdentifier contentType, AlgorithmIdentifier encryptionAlgorithm, Asn1Encodable content)
		{
			this.data = new BerSequence(new Asn1Encodable[]
			{
				contentType,
				encryptionAlgorithm.ToAsn1Object(),
				new BerTaggedObject(false, 0, content)
			});
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x0600411D RID: 16669 RVA: 0x00181B66 File Offset: 0x0017FD66
		public DerObjectIdentifier ContentType
		{
			get
			{
				return (DerObjectIdentifier)this.data[0];
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x0600411E RID: 16670 RVA: 0x00181B79 File Offset: 0x0017FD79
		public AlgorithmIdentifier EncryptionAlgorithm
		{
			get
			{
				return AlgorithmIdentifier.GetInstance(this.data[1]);
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x0600411F RID: 16671 RVA: 0x00181B8C File Offset: 0x0017FD8C
		public Asn1OctetString Content
		{
			get
			{
				if (this.data.Count == 3)
				{
					return Asn1OctetString.GetInstance((DerTaggedObject)this.data[2], false);
				}
				return null;
			}
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x00181BB5 File Offset: 0x0017FDB5
		public override Asn1Object ToAsn1Object()
		{
			return new BerSequence(new Asn1Encodable[]
			{
				new DerInteger(0),
				this.data
			});
		}

		// Token: 0x040029C0 RID: 10688
		private readonly Asn1Sequence data;
	}
}
