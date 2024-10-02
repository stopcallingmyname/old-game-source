using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000685 RID: 1669
	public class AlgorithmIdentifier : Asn1Encodable
	{
		// Token: 0x06003DD8 RID: 15832 RVA: 0x0017562C File Offset: 0x0017382C
		public static AlgorithmIdentifier GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return AlgorithmIdentifier.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x0017563A File Offset: 0x0017383A
		public static AlgorithmIdentifier GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is AlgorithmIdentifier)
			{
				return (AlgorithmIdentifier)obj;
			}
			return new AlgorithmIdentifier(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x0017565B File Offset: 0x0017385B
		public AlgorithmIdentifier(DerObjectIdentifier algorithm)
		{
			this.algorithm = algorithm;
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x0017566A File Offset: 0x0017386A
		[Obsolete("Use version taking a DerObjectIdentifier")]
		public AlgorithmIdentifier(string algorithm)
		{
			this.algorithm = new DerObjectIdentifier(algorithm);
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x0017567E File Offset: 0x0017387E
		public AlgorithmIdentifier(DerObjectIdentifier algorithm, Asn1Encodable parameters)
		{
			this.algorithm = algorithm;
			this.parameters = parameters;
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x00175694 File Offset: 0x00173894
		internal AlgorithmIdentifier(Asn1Sequence seq)
		{
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.algorithm = DerObjectIdentifier.GetInstance(seq[0]);
			this.parameters = ((seq.Count < 2) ? null : seq[1]);
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06003DDE RID: 15838 RVA: 0x001756FF File Offset: 0x001738FF
		public virtual DerObjectIdentifier Algorithm
		{
			get
			{
				return this.algorithm;
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06003DDF RID: 15839 RVA: 0x001756FF File Offset: 0x001738FF
		[Obsolete("Use 'Algorithm' property instead")]
		public virtual DerObjectIdentifier ObjectID
		{
			get
			{
				return this.algorithm;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06003DE0 RID: 15840 RVA: 0x00175707 File Offset: 0x00173907
		public virtual Asn1Encodable Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x00175710 File Offset: 0x00173910
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.algorithm
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.parameters
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002776 RID: 10102
		private readonly DerObjectIdentifier algorithm;

		// Token: 0x04002777 RID: 10103
		private readonly Asn1Encodable parameters;
	}
}
