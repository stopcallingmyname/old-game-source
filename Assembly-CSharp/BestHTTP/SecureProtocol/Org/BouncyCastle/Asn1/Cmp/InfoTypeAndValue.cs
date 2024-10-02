using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007B8 RID: 1976
	public class InfoTypeAndValue : Asn1Encodable
	{
		// Token: 0x0600466D RID: 18029 RVA: 0x00193545 File Offset: 0x00191745
		private InfoTypeAndValue(Asn1Sequence seq)
		{
			this.infoType = DerObjectIdentifier.GetInstance(seq[0]);
			if (seq.Count > 1)
			{
				this.infoValue = seq[1];
			}
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x00193575 File Offset: 0x00191775
		public static InfoTypeAndValue GetInstance(object obj)
		{
			if (obj is InfoTypeAndValue)
			{
				return (InfoTypeAndValue)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new InfoTypeAndValue((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x001935B4 File Offset: 0x001917B4
		public InfoTypeAndValue(DerObjectIdentifier infoType)
		{
			this.infoType = infoType;
			this.infoValue = null;
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x001935CA File Offset: 0x001917CA
		public InfoTypeAndValue(DerObjectIdentifier infoType, Asn1Encodable optionalValue)
		{
			this.infoType = infoType;
			this.infoValue = optionalValue;
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06004671 RID: 18033 RVA: 0x001935E0 File Offset: 0x001917E0
		public virtual DerObjectIdentifier InfoType
		{
			get
			{
				return this.infoType;
			}
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06004672 RID: 18034 RVA: 0x001935E8 File Offset: 0x001917E8
		public virtual Asn1Encodable InfoValue
		{
			get
			{
				return this.infoValue;
			}
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x001935F0 File Offset: 0x001917F0
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.infoType
			});
			if (this.infoValue != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.infoValue
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002DD7 RID: 11735
		private readonly DerObjectIdentifier infoType;

		// Token: 0x04002DD8 RID: 11736
		private readonly Asn1Encodable infoValue;
	}
}
