using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200063E RID: 1598
	public class BerSequence : DerSequence
	{
		// Token: 0x06003BE5 RID: 15333 RVA: 0x0016FDA8 File Offset: 0x0016DFA8
		public new static BerSequence FromVector(Asn1EncodableVector v)
		{
			if (v.Count >= 1)
			{
				return new BerSequence(v);
			}
			return BerSequence.Empty;
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x0016FDBF File Offset: 0x0016DFBF
		public BerSequence()
		{
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x0016FDC7 File Offset: 0x0016DFC7
		public BerSequence(Asn1Encodable obj) : base(obj)
		{
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x0016FDD0 File Offset: 0x0016DFD0
		public BerSequence(params Asn1Encodable[] v) : base(v)
		{
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x0016FDD9 File Offset: 0x0016DFD9
		public BerSequence(Asn1EncodableVector v) : base(v)
		{
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x0016FDE4 File Offset: 0x0016DFE4
		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteByte(48);
				derOut.WriteByte(128);
				foreach (object obj in this)
				{
					Asn1Encodable obj2 = (Asn1Encodable)obj;
					derOut.WriteObject(obj2);
				}
				derOut.WriteByte(0);
				derOut.WriteByte(0);
				return;
			}
			base.Encode(derOut);
		}

		// Token: 0x040026C9 RID: 9929
		public new static readonly BerSequence Empty = new BerSequence();
	}
}
