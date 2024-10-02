using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000678 RID: 1656
	public class KeySpecificInfo : Asn1Encodable
	{
		// Token: 0x06003D80 RID: 15744 RVA: 0x0017417D File Offset: 0x0017237D
		public KeySpecificInfo(DerObjectIdentifier algorithm, Asn1OctetString counter)
		{
			this.algorithm = algorithm;
			this.counter = counter;
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x00174194 File Offset: 0x00172394
		public KeySpecificInfo(Asn1Sequence seq)
		{
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			this.algorithm = (DerObjectIdentifier)enumerator.Current;
			enumerator.MoveNext();
			this.counter = (Asn1OctetString)enumerator.Current;
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06003D82 RID: 15746 RVA: 0x001741DE File Offset: 0x001723DE
		public DerObjectIdentifier Algorithm
		{
			get
			{
				return this.algorithm;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06003D83 RID: 15747 RVA: 0x001741E6 File Offset: 0x001723E6
		public Asn1OctetString Counter
		{
			get
			{
				return this.counter;
			}
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x001741EE File Offset: 0x001723EE
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.algorithm,
				this.counter
			});
		}

		// Token: 0x04002717 RID: 10007
		private DerObjectIdentifier algorithm;

		// Token: 0x04002718 RID: 10008
		private Asn1OctetString counter;
	}
}
