using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.Ecc
{
	// Token: 0x020007A9 RID: 1961
	public class MQVuserKeyingMaterial : Asn1Encodable
	{
		// Token: 0x06004614 RID: 17940 RVA: 0x001924FE File Offset: 0x001906FE
		public MQVuserKeyingMaterial(OriginatorPublicKey ephemeralPublicKey, Asn1OctetString addedukm)
		{
			this.ephemeralPublicKey = ephemeralPublicKey;
			this.addedukm = addedukm;
		}

		// Token: 0x06004615 RID: 17941 RVA: 0x00192514 File Offset: 0x00190714
		private MQVuserKeyingMaterial(Asn1Sequence seq)
		{
			this.ephemeralPublicKey = OriginatorPublicKey.GetInstance(seq[0]);
			if (seq.Count > 1)
			{
				this.addedukm = Asn1OctetString.GetInstance((Asn1TaggedObject)seq[1], true);
			}
		}

		// Token: 0x06004616 RID: 17942 RVA: 0x0019254F File Offset: 0x0019074F
		public static MQVuserKeyingMaterial GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return MQVuserKeyingMaterial.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06004617 RID: 17943 RVA: 0x0019255D File Offset: 0x0019075D
		public static MQVuserKeyingMaterial GetInstance(object obj)
		{
			if (obj == null || obj is MQVuserKeyingMaterial)
			{
				return (MQVuserKeyingMaterial)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new MQVuserKeyingMaterial((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid MQVuserKeyingMaterial: " + Platform.GetTypeName(obj));
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06004618 RID: 17944 RVA: 0x0019259A File Offset: 0x0019079A
		public OriginatorPublicKey EphemeralPublicKey
		{
			get
			{
				return this.ephemeralPublicKey;
			}
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06004619 RID: 17945 RVA: 0x001925A2 File Offset: 0x001907A2
		public Asn1OctetString AddedUkm
		{
			get
			{
				return this.addedukm;
			}
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x001925AC File Offset: 0x001907AC
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.ephemeralPublicKey
			});
			if (this.addedukm != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.addedukm)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002D9E RID: 11678
		private OriginatorPublicKey ephemeralPublicKey;

		// Token: 0x04002D9F RID: 11679
		private Asn1OctetString addedukm;
	}
}
