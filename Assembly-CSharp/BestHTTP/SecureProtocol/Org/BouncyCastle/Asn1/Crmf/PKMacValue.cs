using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000773 RID: 1907
	public class PKMacValue : Asn1Encodable
	{
		// Token: 0x06004473 RID: 17523 RVA: 0x0018DBE6 File Offset: 0x0018BDE6
		private PKMacValue(Asn1Sequence seq)
		{
			this.algID = AlgorithmIdentifier.GetInstance(seq[0]);
			this.macValue = DerBitString.GetInstance(seq[1]);
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x0018DC12 File Offset: 0x0018BE12
		public static PKMacValue GetInstance(object obj)
		{
			if (obj is PKMacValue)
			{
				return (PKMacValue)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PKMacValue((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x0018DC51 File Offset: 0x0018BE51
		public static PKMacValue GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return PKMacValue.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x0018DC5F File Offset: 0x0018BE5F
		public PKMacValue(PbmParameter pbmParams, DerBitString macValue) : this(new AlgorithmIdentifier(CmpObjectIdentifiers.passwordBasedMac, pbmParams), macValue)
		{
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x0018DC73 File Offset: 0x0018BE73
		public PKMacValue(AlgorithmIdentifier algID, DerBitString macValue)
		{
			this.algID = algID;
			this.macValue = macValue;
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06004478 RID: 17528 RVA: 0x0018DC89 File Offset: 0x0018BE89
		public virtual AlgorithmIdentifier AlgID
		{
			get
			{
				return this.algID;
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06004479 RID: 17529 RVA: 0x0018DC91 File Offset: 0x0018BE91
		public virtual DerBitString MacValue
		{
			get
			{
				return this.macValue;
			}
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x0018DC99 File Offset: 0x0018BE99
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.algID,
				this.macValue
			});
		}

		// Token: 0x04002CE2 RID: 11490
		private readonly AlgorithmIdentifier algID;

		// Token: 0x04002CE3 RID: 11491
		private readonly DerBitString macValue;
	}
}
