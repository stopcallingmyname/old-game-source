using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200078D RID: 1933
	public class IssuerAndSerialNumber : Asn1Encodable
	{
		// Token: 0x0600452C RID: 17708 RVA: 0x0018FE8C File Offset: 0x0018E08C
		public static IssuerAndSerialNumber GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			IssuerAndSerialNumber issuerAndSerialNumber = obj as IssuerAndSerialNumber;
			if (issuerAndSerialNumber != null)
			{
				return issuerAndSerialNumber;
			}
			return new IssuerAndSerialNumber(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x0018FEB5 File Offset: 0x0018E0B5
		[Obsolete("Use GetInstance() instead")]
		public IssuerAndSerialNumber(Asn1Sequence seq)
		{
			this.name = X509Name.GetInstance(seq[0]);
			this.serialNumber = (DerInteger)seq[1];
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x0018FEE1 File Offset: 0x0018E0E1
		public IssuerAndSerialNumber(X509Name name, BigInteger serialNumber)
		{
			this.name = name;
			this.serialNumber = new DerInteger(serialNumber);
		}

		// Token: 0x0600452F RID: 17711 RVA: 0x0018FEFC File Offset: 0x0018E0FC
		public IssuerAndSerialNumber(X509Name name, DerInteger serialNumber)
		{
			this.name = name;
			this.serialNumber = serialNumber;
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x06004530 RID: 17712 RVA: 0x0018FF12 File Offset: 0x0018E112
		public X509Name Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x06004531 RID: 17713 RVA: 0x0018FF1A File Offset: 0x0018E11A
		public DerInteger SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x0018FF22 File Offset: 0x0018E122
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.name,
				this.serialNumber
			});
		}

		// Token: 0x04002D46 RID: 11590
		private X509Name name;

		// Token: 0x04002D47 RID: 11591
		private DerInteger serialNumber;
	}
}
