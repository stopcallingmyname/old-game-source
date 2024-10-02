using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006A1 RID: 1697
	public class IssuerSerial : Asn1Encodable
	{
		// Token: 0x06003EC1 RID: 16065 RVA: 0x00178440 File Offset: 0x00176640
		public static IssuerSerial GetInstance(object obj)
		{
			if (obj == null || obj is IssuerSerial)
			{
				return (IssuerSerial)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new IssuerSerial((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x0017848D File Offset: 0x0017668D
		public static IssuerSerial GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return IssuerSerial.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x0017849C File Offset: 0x0017669C
		private IssuerSerial(Asn1Sequence seq)
		{
			if (seq.Count != 2 && seq.Count != 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.issuer = GeneralNames.GetInstance(seq[0]);
			this.serial = DerInteger.GetInstance(seq[1]);
			if (seq.Count == 3)
			{
				this.issuerUid = DerBitString.GetInstance(seq[2]);
			}
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x0017851B File Offset: 0x0017671B
		public IssuerSerial(GeneralNames issuer, DerInteger serial)
		{
			this.issuer = issuer;
			this.serial = serial;
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06003EC5 RID: 16069 RVA: 0x00178531 File Offset: 0x00176731
		public GeneralNames Issuer
		{
			get
			{
				return this.issuer;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06003EC6 RID: 16070 RVA: 0x00178539 File Offset: 0x00176739
		public DerInteger Serial
		{
			get
			{
				return this.serial;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06003EC7 RID: 16071 RVA: 0x00178541 File Offset: 0x00176741
		public DerBitString IssuerUid
		{
			get
			{
				return this.issuerUid;
			}
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x0017854C File Offset: 0x0017674C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.issuer,
				this.serial
			});
			if (this.issuerUid != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.issuerUid
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x040027D1 RID: 10193
		internal readonly GeneralNames issuer;

		// Token: 0x040027D2 RID: 10194
		internal readonly DerInteger serial;

		// Token: 0x040027D3 RID: 10195
		internal readonly DerBitString issuerUid;
	}
}
