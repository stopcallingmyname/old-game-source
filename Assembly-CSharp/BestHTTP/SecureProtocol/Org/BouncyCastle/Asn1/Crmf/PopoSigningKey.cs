using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000775 RID: 1909
	public class PopoSigningKey : Asn1Encodable
	{
		// Token: 0x06004481 RID: 17537 RVA: 0x0018DDAC File Offset: 0x0018BFAC
		private PopoSigningKey(Asn1Sequence seq)
		{
			int index = 0;
			if (seq[index] is Asn1TaggedObject)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[index++];
				if (asn1TaggedObject.TagNo != 0)
				{
					throw new ArgumentException("Unknown PopoSigningKeyInput tag: " + asn1TaggedObject.TagNo, "seq");
				}
				this.poposkInput = PopoSigningKeyInput.GetInstance(asn1TaggedObject.GetObject());
			}
			this.algorithmIdentifier = AlgorithmIdentifier.GetInstance(seq[index++]);
			this.signature = DerBitString.GetInstance(seq[index]);
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x0018DE41 File Offset: 0x0018C041
		public static PopoSigningKey GetInstance(object obj)
		{
			if (obj is PopoSigningKey)
			{
				return (PopoSigningKey)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PopoSigningKey((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x0018DE80 File Offset: 0x0018C080
		public static PopoSigningKey GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return PopoSigningKey.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x0018DE8E File Offset: 0x0018C08E
		public PopoSigningKey(PopoSigningKeyInput poposkIn, AlgorithmIdentifier aid, DerBitString signature)
		{
			this.poposkInput = poposkIn;
			this.algorithmIdentifier = aid;
			this.signature = signature;
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06004485 RID: 17541 RVA: 0x0018DEAB File Offset: 0x0018C0AB
		public virtual PopoSigningKeyInput PoposkInput
		{
			get
			{
				return this.poposkInput;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06004486 RID: 17542 RVA: 0x0018DEB3 File Offset: 0x0018C0B3
		public virtual AlgorithmIdentifier AlgorithmIdentifier
		{
			get
			{
				return this.algorithmIdentifier;
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06004487 RID: 17543 RVA: 0x0018DEBB File Offset: 0x0018C0BB
		public virtual DerBitString Signature
		{
			get
			{
				return this.signature;
			}
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x0018DEC4 File Offset: 0x0018C0C4
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.poposkInput != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.poposkInput)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.algorithmIdentifier
			});
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.signature
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002CEB RID: 11499
		private readonly PopoSigningKeyInput poposkInput;

		// Token: 0x04002CEC RID: 11500
		private readonly AlgorithmIdentifier algorithmIdentifier;

		// Token: 0x04002CED RID: 11501
		private readonly DerBitString signature;
	}
}
