using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006AC RID: 1708
	public class PrivateKeyUsagePeriod : Asn1Encodable
	{
		// Token: 0x06003F0A RID: 16138 RVA: 0x00179328 File Offset: 0x00177528
		public static PrivateKeyUsagePeriod GetInstance(object obj)
		{
			if (obj is PrivateKeyUsagePeriod)
			{
				return (PrivateKeyUsagePeriod)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PrivateKeyUsagePeriod((Asn1Sequence)obj);
			}
			if (obj is X509Extension)
			{
				return PrivateKeyUsagePeriod.GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));
			}
			throw new ArgumentException("unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003F0B RID: 16139 RVA: 0x0017938C File Offset: 0x0017758C
		private PrivateKeyUsagePeriod(Asn1Sequence seq)
		{
			foreach (object obj in seq)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
				if (asn1TaggedObject.TagNo == 0)
				{
					this._notBefore = DerGeneralizedTime.GetInstance(asn1TaggedObject, false);
				}
				else if (asn1TaggedObject.TagNo == 1)
				{
					this._notAfter = DerGeneralizedTime.GetInstance(asn1TaggedObject, false);
				}
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06003F0C RID: 16140 RVA: 0x0017940C File Offset: 0x0017760C
		public DerGeneralizedTime NotBefore
		{
			get
			{
				return this._notBefore;
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06003F0D RID: 16141 RVA: 0x00179414 File Offset: 0x00177614
		public DerGeneralizedTime NotAfter
		{
			get
			{
				return this._notAfter;
			}
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x0017941C File Offset: 0x0017761C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this._notBefore != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this._notBefore)
				});
			}
			if (this._notAfter != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this._notAfter)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002804 RID: 10244
		private DerGeneralizedTime _notBefore;

		// Token: 0x04002805 RID: 10245
		private DerGeneralizedTime _notAfter;
	}
}
