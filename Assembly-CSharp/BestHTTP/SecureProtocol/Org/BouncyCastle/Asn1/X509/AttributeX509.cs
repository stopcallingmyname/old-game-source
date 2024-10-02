using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000688 RID: 1672
	public class AttributeX509 : Asn1Encodable
	{
		// Token: 0x06003DEF RID: 15855 RVA: 0x00175938 File Offset: 0x00173B38
		public static AttributeX509 GetInstance(object obj)
		{
			if (obj == null || obj is AttributeX509)
			{
				return (AttributeX509)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new AttributeX509((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x00175988 File Offset: 0x00173B88
		private AttributeX509(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.attrType = DerObjectIdentifier.GetInstance(seq[0]);
			this.attrValues = Asn1Set.GetInstance(seq[1]);
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x001759E3 File Offset: 0x00173BE3
		public AttributeX509(DerObjectIdentifier attrType, Asn1Set attrValues)
		{
			this.attrType = attrType;
			this.attrValues = attrValues;
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06003DF2 RID: 15858 RVA: 0x001759F9 File Offset: 0x00173BF9
		public DerObjectIdentifier AttrType
		{
			get
			{
				return this.attrType;
			}
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x00175A01 File Offset: 0x00173C01
		public Asn1Encodable[] GetAttributeValues()
		{
			return this.attrValues.ToArray();
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06003DF4 RID: 15860 RVA: 0x00175A0E File Offset: 0x00173C0E
		public Asn1Set AttrValues
		{
			get
			{
				return this.attrValues;
			}
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x00175A16 File Offset: 0x00173C16
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.attrType,
				this.attrValues
			});
		}

		// Token: 0x0400277C RID: 10108
		private readonly DerObjectIdentifier attrType;

		// Token: 0x0400277D RID: 10109
		private readonly Asn1Set attrValues;
	}
}
