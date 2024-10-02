using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007C0 RID: 1984
	public class PkiHeader : Asn1Encodable
	{
		// Token: 0x060046A1 RID: 18081 RVA: 0x00193DF4 File Offset: 0x00191FF4
		private PkiHeader(Asn1Sequence seq)
		{
			this.pvno = DerInteger.GetInstance(seq[0]);
			this.sender = GeneralName.GetInstance(seq[1]);
			this.recipient = GeneralName.GetInstance(seq[2]);
			for (int i = 3; i < seq.Count; i++)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[i];
				switch (asn1TaggedObject.TagNo)
				{
				case 0:
					this.messageTime = DerGeneralizedTime.GetInstance(asn1TaggedObject, true);
					break;
				case 1:
					this.protectionAlg = AlgorithmIdentifier.GetInstance(asn1TaggedObject, true);
					break;
				case 2:
					this.senderKID = Asn1OctetString.GetInstance(asn1TaggedObject, true);
					break;
				case 3:
					this.recipKID = Asn1OctetString.GetInstance(asn1TaggedObject, true);
					break;
				case 4:
					this.transactionID = Asn1OctetString.GetInstance(asn1TaggedObject, true);
					break;
				case 5:
					this.senderNonce = Asn1OctetString.GetInstance(asn1TaggedObject, true);
					break;
				case 6:
					this.recipNonce = Asn1OctetString.GetInstance(asn1TaggedObject, true);
					break;
				case 7:
					this.freeText = PkiFreeText.GetInstance(asn1TaggedObject, true);
					break;
				case 8:
					this.generalInfo = Asn1Sequence.GetInstance(asn1TaggedObject, true);
					break;
				default:
					throw new ArgumentException("unknown tag number: " + asn1TaggedObject.TagNo, "seq");
				}
			}
		}

		// Token: 0x060046A2 RID: 18082 RVA: 0x00193F44 File Offset: 0x00192144
		public static PkiHeader GetInstance(object obj)
		{
			if (obj is PkiHeader)
			{
				return (PkiHeader)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PkiHeader((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060046A3 RID: 18083 RVA: 0x00193F83 File Offset: 0x00192183
		public PkiHeader(int pvno, GeneralName sender, GeneralName recipient) : this(new DerInteger(pvno), sender, recipient)
		{
		}

		// Token: 0x060046A4 RID: 18084 RVA: 0x00193F93 File Offset: 0x00192193
		private PkiHeader(DerInteger pvno, GeneralName sender, GeneralName recipient)
		{
			this.pvno = pvno;
			this.sender = sender;
			this.recipient = recipient;
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x060046A5 RID: 18085 RVA: 0x00193FB0 File Offset: 0x001921B0
		public virtual DerInteger Pvno
		{
			get
			{
				return this.pvno;
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x060046A6 RID: 18086 RVA: 0x00193FB8 File Offset: 0x001921B8
		public virtual GeneralName Sender
		{
			get
			{
				return this.sender;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x060046A7 RID: 18087 RVA: 0x00193FC0 File Offset: 0x001921C0
		public virtual GeneralName Recipient
		{
			get
			{
				return this.recipient;
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x060046A8 RID: 18088 RVA: 0x00193FC8 File Offset: 0x001921C8
		public virtual DerGeneralizedTime MessageTime
		{
			get
			{
				return this.messageTime;
			}
		}

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x060046A9 RID: 18089 RVA: 0x00193FD0 File Offset: 0x001921D0
		public virtual AlgorithmIdentifier ProtectionAlg
		{
			get
			{
				return this.protectionAlg;
			}
		}

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x060046AA RID: 18090 RVA: 0x00193FD8 File Offset: 0x001921D8
		public virtual Asn1OctetString SenderKID
		{
			get
			{
				return this.senderKID;
			}
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x060046AB RID: 18091 RVA: 0x00193FE0 File Offset: 0x001921E0
		public virtual Asn1OctetString RecipKID
		{
			get
			{
				return this.recipKID;
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x060046AC RID: 18092 RVA: 0x00193FE8 File Offset: 0x001921E8
		public virtual Asn1OctetString TransactionID
		{
			get
			{
				return this.transactionID;
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x060046AD RID: 18093 RVA: 0x00193FF0 File Offset: 0x001921F0
		public virtual Asn1OctetString SenderNonce
		{
			get
			{
				return this.senderNonce;
			}
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x060046AE RID: 18094 RVA: 0x00193FF8 File Offset: 0x001921F8
		public virtual Asn1OctetString RecipNonce
		{
			get
			{
				return this.recipNonce;
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x060046AF RID: 18095 RVA: 0x00194000 File Offset: 0x00192200
		public virtual PkiFreeText FreeText
		{
			get
			{
				return this.freeText;
			}
		}

		// Token: 0x060046B0 RID: 18096 RVA: 0x00194008 File Offset: 0x00192208
		public virtual InfoTypeAndValue[] GetGeneralInfo()
		{
			if (this.generalInfo == null)
			{
				return null;
			}
			InfoTypeAndValue[] array = new InfoTypeAndValue[this.generalInfo.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InfoTypeAndValue.GetInstance(this.generalInfo[i]);
			}
			return array;
		}

		// Token: 0x060046B1 RID: 18097 RVA: 0x00194054 File Offset: 0x00192254
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector v = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.pvno,
				this.sender,
				this.recipient
			});
			PkiHeader.AddOptional(v, 0, this.messageTime);
			PkiHeader.AddOptional(v, 1, this.protectionAlg);
			PkiHeader.AddOptional(v, 2, this.senderKID);
			PkiHeader.AddOptional(v, 3, this.recipKID);
			PkiHeader.AddOptional(v, 4, this.transactionID);
			PkiHeader.AddOptional(v, 5, this.senderNonce);
			PkiHeader.AddOptional(v, 6, this.recipNonce);
			PkiHeader.AddOptional(v, 7, this.freeText);
			PkiHeader.AddOptional(v, 8, this.generalInfo);
			return new DerSequence(v);
		}

		// Token: 0x060046B2 RID: 18098 RVA: 0x00194101 File Offset: 0x00192301
		private static void AddOptional(Asn1EncodableVector v, int tagNo, Asn1Encodable obj)
		{
			if (obj != null)
			{
				v.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, tagNo, obj)
				});
			}
		}

		// Token: 0x04002E1D RID: 11805
		public static readonly GeneralName NULL_NAME = new GeneralName(X509Name.GetInstance(new DerSequence()));

		// Token: 0x04002E1E RID: 11806
		public static readonly int CMP_1999 = 1;

		// Token: 0x04002E1F RID: 11807
		public static readonly int CMP_2000 = 2;

		// Token: 0x04002E20 RID: 11808
		private readonly DerInteger pvno;

		// Token: 0x04002E21 RID: 11809
		private readonly GeneralName sender;

		// Token: 0x04002E22 RID: 11810
		private readonly GeneralName recipient;

		// Token: 0x04002E23 RID: 11811
		private readonly DerGeneralizedTime messageTime;

		// Token: 0x04002E24 RID: 11812
		private readonly AlgorithmIdentifier protectionAlg;

		// Token: 0x04002E25 RID: 11813
		private readonly Asn1OctetString senderKID;

		// Token: 0x04002E26 RID: 11814
		private readonly Asn1OctetString recipKID;

		// Token: 0x04002E27 RID: 11815
		private readonly Asn1OctetString transactionID;

		// Token: 0x04002E28 RID: 11816
		private readonly Asn1OctetString senderNonce;

		// Token: 0x04002E29 RID: 11817
		private readonly Asn1OctetString recipNonce;

		// Token: 0x04002E2A RID: 11818
		private readonly PkiFreeText freeText;

		// Token: 0x04002E2B RID: 11819
		private readonly Asn1Sequence generalInfo;
	}
}
