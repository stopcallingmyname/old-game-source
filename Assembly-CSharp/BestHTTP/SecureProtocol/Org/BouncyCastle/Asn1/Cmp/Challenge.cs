using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007B1 RID: 1969
	public class Challenge : Asn1Encodable
	{
		// Token: 0x0600464A RID: 17994 RVA: 0x00192E88 File Offset: 0x00191088
		private Challenge(Asn1Sequence seq)
		{
			int index = 0;
			if (seq.Count == 3)
			{
				this.owf = AlgorithmIdentifier.GetInstance(seq[index++]);
			}
			this.witness = Asn1OctetString.GetInstance(seq[index++]);
			this.challenge = Asn1OctetString.GetInstance(seq[index]);
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x00192EE4 File Offset: 0x001910E4
		public static Challenge GetInstance(object obj)
		{
			if (obj is Challenge)
			{
				return (Challenge)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Challenge((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x0600464C RID: 17996 RVA: 0x00192F23 File Offset: 0x00191123
		public virtual AlgorithmIdentifier Owf
		{
			get
			{
				return this.owf;
			}
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x00192F2C File Offset: 0x0019112C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.owf
			});
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.witness
			});
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.challenge
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002DB2 RID: 11698
		private readonly AlgorithmIdentifier owf;

		// Token: 0x04002DB3 RID: 11699
		private readonly Asn1OctetString witness;

		// Token: 0x04002DB4 RID: 11700
		private readonly Asn1OctetString challenge;
	}
}
