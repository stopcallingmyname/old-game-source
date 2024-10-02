using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000764 RID: 1892
	public class AttributeTypeAndValue : Asn1Encodable
	{
		// Token: 0x06004403 RID: 17411 RVA: 0x0018CBAA File Offset: 0x0018ADAA
		private AttributeTypeAndValue(Asn1Sequence seq)
		{
			this.type = (DerObjectIdentifier)seq[0];
			this.value = seq[1];
		}

		// Token: 0x06004404 RID: 17412 RVA: 0x0018CBD1 File Offset: 0x0018ADD1
		public static AttributeTypeAndValue GetInstance(object obj)
		{
			if (obj is AttributeTypeAndValue)
			{
				return (AttributeTypeAndValue)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new AttributeTypeAndValue((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004405 RID: 17413 RVA: 0x0018CC10 File Offset: 0x0018AE10
		public AttributeTypeAndValue(string oid, Asn1Encodable value) : this(new DerObjectIdentifier(oid), value)
		{
		}

		// Token: 0x06004406 RID: 17414 RVA: 0x0018CC1F File Offset: 0x0018AE1F
		public AttributeTypeAndValue(DerObjectIdentifier type, Asn1Encodable value)
		{
			this.type = type;
			this.value = value;
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06004407 RID: 17415 RVA: 0x0018CC35 File Offset: 0x0018AE35
		public virtual DerObjectIdentifier Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06004408 RID: 17416 RVA: 0x0018CC3D File Offset: 0x0018AE3D
		public virtual Asn1Encodable Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x0018CC45 File Offset: 0x0018AE45
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.type,
				this.value
			});
		}

		// Token: 0x04002CA7 RID: 11431
		private readonly DerObjectIdentifier type;

		// Token: 0x04002CA8 RID: 11432
		private readonly Asn1Encodable value;
	}
}
