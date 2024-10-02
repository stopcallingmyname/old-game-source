using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp
{
	// Token: 0x020006DD RID: 1757
	public class TstInfo : Asn1Encodable
	{
		// Token: 0x0600409E RID: 16542 RVA: 0x0017FBDC File Offset: 0x0017DDDC
		public static TstInfo GetInstance(object o)
		{
			if (o == null || o is TstInfo)
			{
				return (TstInfo)o;
			}
			if (o is Asn1Sequence)
			{
				return new TstInfo((Asn1Sequence)o);
			}
			if (o is Asn1OctetString)
			{
				try
				{
					return TstInfo.GetInstance(Asn1Object.FromByteArray(((Asn1OctetString)o).GetOctets()));
				}
				catch (IOException)
				{
					throw new ArgumentException("Bad object format in 'TstInfo' factory.");
				}
			}
			throw new ArgumentException("Unknown object in 'TstInfo' factory: " + Platform.GetTypeName(o));
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x0017FC64 File Offset: 0x0017DE64
		private TstInfo(Asn1Sequence seq)
		{
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			this.version = DerInteger.GetInstance(enumerator.Current);
			enumerator.MoveNext();
			this.tsaPolicyId = DerObjectIdentifier.GetInstance(enumerator.Current);
			enumerator.MoveNext();
			this.messageImprint = MessageImprint.GetInstance(enumerator.Current);
			enumerator.MoveNext();
			this.serialNumber = DerInteger.GetInstance(enumerator.Current);
			enumerator.MoveNext();
			this.genTime = DerGeneralizedTime.GetInstance(enumerator.Current);
			this.ordering = DerBoolean.False;
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Asn1Object asn1Object = (Asn1Object)obj;
				if (asn1Object is Asn1TaggedObject)
				{
					DerTaggedObject derTaggedObject = (DerTaggedObject)asn1Object;
					int tagNo = derTaggedObject.TagNo;
					if (tagNo != 0)
					{
						if (tagNo != 1)
						{
							throw new ArgumentException("Unknown tag value " + derTaggedObject.TagNo);
						}
						this.extensions = X509Extensions.GetInstance(derTaggedObject, false);
					}
					else
					{
						this.tsa = GeneralName.GetInstance(derTaggedObject, true);
					}
				}
				if (asn1Object is DerSequence)
				{
					this.accuracy = Accuracy.GetInstance(asn1Object);
				}
				if (asn1Object is DerBoolean)
				{
					this.ordering = DerBoolean.GetInstance(asn1Object);
				}
				if (asn1Object is DerInteger)
				{
					this.nonce = DerInteger.GetInstance(asn1Object);
				}
			}
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x0017FDB4 File Offset: 0x0017DFB4
		public TstInfo(DerObjectIdentifier tsaPolicyId, MessageImprint messageImprint, DerInteger serialNumber, DerGeneralizedTime genTime, Accuracy accuracy, DerBoolean ordering, DerInteger nonce, GeneralName tsa, X509Extensions extensions)
		{
			this.version = new DerInteger(1);
			this.tsaPolicyId = tsaPolicyId;
			this.messageImprint = messageImprint;
			this.serialNumber = serialNumber;
			this.genTime = genTime;
			this.accuracy = accuracy;
			this.ordering = ordering;
			this.nonce = nonce;
			this.tsa = tsa;
			this.extensions = extensions;
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x060040A1 RID: 16545 RVA: 0x0017FE18 File Offset: 0x0017E018
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x060040A2 RID: 16546 RVA: 0x0017FE20 File Offset: 0x0017E020
		public MessageImprint MessageImprint
		{
			get
			{
				return this.messageImprint;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x060040A3 RID: 16547 RVA: 0x0017FE28 File Offset: 0x0017E028
		public DerObjectIdentifier Policy
		{
			get
			{
				return this.tsaPolicyId;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060040A4 RID: 16548 RVA: 0x0017FE30 File Offset: 0x0017E030
		public DerInteger SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x060040A5 RID: 16549 RVA: 0x0017FE38 File Offset: 0x0017E038
		public Accuracy Accuracy
		{
			get
			{
				return this.accuracy;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x060040A6 RID: 16550 RVA: 0x0017FE40 File Offset: 0x0017E040
		public DerGeneralizedTime GenTime
		{
			get
			{
				return this.genTime;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x060040A7 RID: 16551 RVA: 0x0017FE48 File Offset: 0x0017E048
		public DerBoolean Ordering
		{
			get
			{
				return this.ordering;
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x060040A8 RID: 16552 RVA: 0x0017FE50 File Offset: 0x0017E050
		public DerInteger Nonce
		{
			get
			{
				return this.nonce;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x060040A9 RID: 16553 RVA: 0x0017FE58 File Offset: 0x0017E058
		public GeneralName Tsa
		{
			get
			{
				return this.tsa;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x060040AA RID: 16554 RVA: 0x0017FE60 File Offset: 0x0017E060
		public X509Extensions Extensions
		{
			get
			{
				return this.extensions;
			}
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x0017FE68 File Offset: 0x0017E068
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				this.tsaPolicyId,
				this.messageImprint,
				this.serialNumber,
				this.genTime
			});
			if (this.accuracy != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.accuracy
				});
			}
			if (this.ordering != null && this.ordering.IsTrue)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.ordering
				});
			}
			if (this.nonce != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.nonce
				});
			}
			if (this.tsa != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.tsa)
				});
			}
			if (this.extensions != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.extensions)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002935 RID: 10549
		private readonly DerInteger version;

		// Token: 0x04002936 RID: 10550
		private readonly DerObjectIdentifier tsaPolicyId;

		// Token: 0x04002937 RID: 10551
		private readonly MessageImprint messageImprint;

		// Token: 0x04002938 RID: 10552
		private readonly DerInteger serialNumber;

		// Token: 0x04002939 RID: 10553
		private readonly DerGeneralizedTime genTime;

		// Token: 0x0400293A RID: 10554
		private readonly Accuracy accuracy;

		// Token: 0x0400293B RID: 10555
		private readonly DerBoolean ordering;

		// Token: 0x0400293C RID: 10556
		private readonly DerInteger nonce;

		// Token: 0x0400293D RID: 10557
		private readonly GeneralName tsa;

		// Token: 0x0400293E RID: 10558
		private readonly X509Extensions extensions;
	}
}
