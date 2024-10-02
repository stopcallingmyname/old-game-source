using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006F4 RID: 1780
	public class IssuerAndSerialNumber : Asn1Encodable
	{
		// Token: 0x0600412D RID: 16685 RVA: 0x00181D4B File Offset: 0x0017FF4B
		public static IssuerAndSerialNumber GetInstance(object obj)
		{
			if (obj is IssuerAndSerialNumber)
			{
				return (IssuerAndSerialNumber)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new IssuerAndSerialNumber((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x00181D8C File Offset: 0x0017FF8C
		private IssuerAndSerialNumber(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.name = X509Name.GetInstance(seq[0]);
			this.certSerialNumber = DerInteger.GetInstance(seq[1]);
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x00181DDC File Offset: 0x0017FFDC
		public IssuerAndSerialNumber(X509Name name, BigInteger certSerialNumber)
		{
			this.name = name;
			this.certSerialNumber = new DerInteger(certSerialNumber);
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x00181DF7 File Offset: 0x0017FFF7
		public IssuerAndSerialNumber(X509Name name, DerInteger certSerialNumber)
		{
			this.name = name;
			this.certSerialNumber = certSerialNumber;
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06004131 RID: 16689 RVA: 0x00181E0D File Offset: 0x0018000D
		public X509Name Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06004132 RID: 16690 RVA: 0x00181E15 File Offset: 0x00180015
		public DerInteger CertificateSerialNumber
		{
			get
			{
				return this.certSerialNumber;
			}
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x00181E1D File Offset: 0x0018001D
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.name,
				this.certSerialNumber
			});
		}

		// Token: 0x040029C3 RID: 10691
		private readonly X509Name name;

		// Token: 0x040029C4 RID: 10692
		private readonly DerInteger certSerialNumber;
	}
}
