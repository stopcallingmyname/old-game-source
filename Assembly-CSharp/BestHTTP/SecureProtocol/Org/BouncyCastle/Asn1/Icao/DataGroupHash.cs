using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Icao
{
	// Token: 0x02000731 RID: 1841
	public class DataGroupHash : Asn1Encodable
	{
		// Token: 0x060042C4 RID: 17092 RVA: 0x001879FD File Offset: 0x00185BFD
		public static DataGroupHash GetInstance(object obj)
		{
			if (obj is DataGroupHash)
			{
				return (DataGroupHash)obj;
			}
			if (obj != null)
			{
				return new DataGroupHash(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x00187A20 File Offset: 0x00185C20
		private DataGroupHash(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.dataGroupNumber = DerInteger.GetInstance(seq[0]);
			this.dataGroupHashValue = Asn1OctetString.GetInstance(seq[1]);
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x00187A70 File Offset: 0x00185C70
		public DataGroupHash(int dataGroupNumber, Asn1OctetString dataGroupHashValue)
		{
			this.dataGroupNumber = new DerInteger(dataGroupNumber);
			this.dataGroupHashValue = dataGroupHashValue;
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x060042C7 RID: 17095 RVA: 0x00187A8B File Offset: 0x00185C8B
		public int DataGroupNumber
		{
			get
			{
				return this.dataGroupNumber.Value.IntValue;
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x060042C8 RID: 17096 RVA: 0x00187A9D File Offset: 0x00185C9D
		public Asn1OctetString DataGroupHashValue
		{
			get
			{
				return this.dataGroupHashValue;
			}
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x00187AA5 File Offset: 0x00185CA5
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.dataGroupNumber,
				this.dataGroupHashValue
			});
		}

		// Token: 0x04002B95 RID: 11157
		private readonly DerInteger dataGroupNumber;

		// Token: 0x04002B96 RID: 11158
		private readonly Asn1OctetString dataGroupHashValue;
	}
}
