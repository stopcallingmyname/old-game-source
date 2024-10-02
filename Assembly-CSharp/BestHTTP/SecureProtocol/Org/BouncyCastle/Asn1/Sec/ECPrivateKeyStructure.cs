using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Sec
{
	// Token: 0x020006E6 RID: 1766
	public class ECPrivateKeyStructure : Asn1Encodable
	{
		// Token: 0x060040D1 RID: 16593 RVA: 0x001808A3 File Offset: 0x0017EAA3
		public static ECPrivateKeyStructure GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is ECPrivateKeyStructure)
			{
				return (ECPrivateKeyStructure)obj;
			}
			return new ECPrivateKeyStructure(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x001808C4 File Offset: 0x0017EAC4
		[Obsolete("Use 'GetInstance' instead")]
		public ECPrivateKeyStructure(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			this.seq = seq;
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x001808E1 File Offset: 0x0017EAE1
		[Obsolete("Use constructor which takes 'orderBitLength' instead, to guarantee correct encoding")]
		public ECPrivateKeyStructure(BigInteger key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.seq = new DerSequence(new Asn1Encodable[]
			{
				new DerInteger(1),
				new DerOctetString(key.ToByteArrayUnsigned())
			});
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x0018091F File Offset: 0x0017EB1F
		public ECPrivateKeyStructure(int orderBitLength, BigInteger key) : this(orderBitLength, key, null)
		{
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x0018092A File Offset: 0x0017EB2A
		[Obsolete("Use constructor which takes 'orderBitLength' instead, to guarantee correct encoding")]
		public ECPrivateKeyStructure(BigInteger key, Asn1Encodable parameters) : this(key, null, parameters)
		{
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x00180938 File Offset: 0x0017EB38
		[Obsolete("Use constructor which takes 'orderBitLength' instead, to guarantee correct encoding")]
		public ECPrivateKeyStructure(BigInteger key, DerBitString publicKey, Asn1Encodable parameters)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				new DerInteger(1),
				new DerOctetString(key.ToByteArrayUnsigned())
			});
			if (parameters != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, parameters)
				});
			}
			if (publicKey != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, publicKey)
				});
			}
			this.seq = new DerSequence(asn1EncodableVector);
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x001809BC File Offset: 0x0017EBBC
		public ECPrivateKeyStructure(int orderBitLength, BigInteger key, Asn1Encodable parameters) : this(orderBitLength, key, null, parameters)
		{
		}

		// Token: 0x060040D8 RID: 16600 RVA: 0x001809C8 File Offset: 0x0017EBC8
		public ECPrivateKeyStructure(int orderBitLength, BigInteger key, DerBitString publicKey, Asn1Encodable parameters)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (orderBitLength < key.BitLength)
			{
				throw new ArgumentException("must be >= key bitlength", "orderBitLength");
			}
			byte[] str = BigIntegers.AsUnsignedByteArray((orderBitLength + 7) / 8, key);
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				new DerInteger(1),
				new DerOctetString(str)
			});
			if (parameters != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, parameters)
				});
			}
			if (publicKey != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, publicKey)
				});
			}
			this.seq = new DerSequence(asn1EncodableVector);
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x00180A70 File Offset: 0x0017EC70
		public virtual BigInteger GetKey()
		{
			Asn1OctetString asn1OctetString = (Asn1OctetString)this.seq[1];
			return new BigInteger(1, asn1OctetString.GetOctets());
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x00180A9B File Offset: 0x0017EC9B
		public virtual DerBitString GetPublicKey()
		{
			return (DerBitString)this.GetObjectInTag(1);
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x00180AA9 File Offset: 0x0017ECA9
		public virtual Asn1Object GetParameters()
		{
			return this.GetObjectInTag(0);
		}

		// Token: 0x060040DC RID: 16604 RVA: 0x00180AB4 File Offset: 0x0017ECB4
		private Asn1Object GetObjectInTag(int tagNo)
		{
			foreach (object obj in this.seq)
			{
				Asn1Object asn1Object = ((Asn1Encodable)obj).ToAsn1Object();
				if (asn1Object is Asn1TaggedObject)
				{
					Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)asn1Object;
					if (asn1TaggedObject.TagNo == tagNo)
					{
						return asn1TaggedObject.GetObject();
					}
				}
			}
			return null;
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x00180B34 File Offset: 0x0017ED34
		public override Asn1Object ToAsn1Object()
		{
			return this.seq;
		}

		// Token: 0x04002975 RID: 10613
		private readonly Asn1Sequence seq;
	}
}
