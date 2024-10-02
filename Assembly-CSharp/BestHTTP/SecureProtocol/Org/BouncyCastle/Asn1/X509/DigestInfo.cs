using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000696 RID: 1686
	public class DigestInfo : Asn1Encodable
	{
		// Token: 0x06003E5A RID: 15962 RVA: 0x00176B47 File Offset: 0x00174D47
		public static DigestInfo GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return DigestInfo.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x00176B55 File Offset: 0x00174D55
		public static DigestInfo GetInstance(object obj)
		{
			if (obj is DigestInfo)
			{
				return (DigestInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new DigestInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x00176B94 File Offset: 0x00174D94
		public DigestInfo(AlgorithmIdentifier algID, byte[] digest)
		{
			this.digest = digest;
			this.algID = algID;
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x00176BAC File Offset: 0x00174DAC
		private DigestInfo(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.algID = AlgorithmIdentifier.GetInstance(seq[0]);
			this.digest = Asn1OctetString.GetInstance(seq[1]).GetOctets();
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06003E5E RID: 15966 RVA: 0x00176C01 File Offset: 0x00174E01
		public AlgorithmIdentifier AlgorithmID
		{
			get
			{
				return this.algID;
			}
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x00176C09 File Offset: 0x00174E09
		public byte[] GetDigest()
		{
			return this.digest;
		}

		// Token: 0x06003E60 RID: 15968 RVA: 0x00176C11 File Offset: 0x00174E11
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.algID,
				new DerOctetString(this.digest)
			});
		}

		// Token: 0x040027A3 RID: 10147
		private readonly byte[] digest;

		// Token: 0x040027A4 RID: 10148
		private readonly AlgorithmIdentifier algID;
	}
}
