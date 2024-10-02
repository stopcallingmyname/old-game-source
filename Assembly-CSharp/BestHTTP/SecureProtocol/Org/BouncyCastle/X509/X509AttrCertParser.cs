using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x0200023F RID: 575
	public class X509AttrCertParser
	{
		// Token: 0x060014B0 RID: 5296 RVA: 0x000AB508 File Offset: 0x000A9708
		private IX509AttributeCertificate ReadDerCertificate(Asn1InputStream dIn)
		{
			Asn1Sequence asn1Sequence = (Asn1Sequence)dIn.ReadObject();
			if (asn1Sequence.Count > 1 && asn1Sequence[0] is DerObjectIdentifier && asn1Sequence[0].Equals(PkcsObjectIdentifiers.SignedData))
			{
				this.sData = SignedData.GetInstance(Asn1Sequence.GetInstance((Asn1TaggedObject)asn1Sequence[1], true)).Certificates;
				return this.GetCertificate();
			}
			return new X509V2AttributeCertificate(AttributeCertificate.GetInstance(asn1Sequence));
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x000AB580 File Offset: 0x000A9780
		private IX509AttributeCertificate GetCertificate()
		{
			if (this.sData != null)
			{
				while (this.sDataObjectCount < this.sData.Count)
				{
					Asn1Set asn1Set = this.sData;
					int num = this.sDataObjectCount;
					this.sDataObjectCount = num + 1;
					object obj = asn1Set[num];
					if (obj is Asn1TaggedObject && ((Asn1TaggedObject)obj).TagNo == 2)
					{
						return new X509V2AttributeCertificate(AttributeCertificate.GetInstance(Asn1Sequence.GetInstance((Asn1TaggedObject)obj, false)));
					}
				}
			}
			return null;
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x000AB5F8 File Offset: 0x000A97F8
		private IX509AttributeCertificate ReadPemCertificate(Stream inStream)
		{
			Asn1Sequence asn1Sequence = X509AttrCertParser.PemAttrCertParser.ReadPemObject(inStream);
			if (asn1Sequence != null)
			{
				return new X509V2AttributeCertificate(AttributeCertificate.GetInstance(asn1Sequence));
			}
			return null;
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x000AB621 File Offset: 0x000A9821
		public IX509AttributeCertificate ReadAttrCert(byte[] input)
		{
			return this.ReadAttrCert(new MemoryStream(input, false));
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x000AB630 File Offset: 0x000A9830
		public ICollection ReadAttrCerts(byte[] input)
		{
			return this.ReadAttrCerts(new MemoryStream(input, false));
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x000AB640 File Offset: 0x000A9840
		public IX509AttributeCertificate ReadAttrCert(Stream inStream)
		{
			if (inStream == null)
			{
				throw new ArgumentNullException("inStream");
			}
			if (!inStream.CanRead)
			{
				throw new ArgumentException("inStream must be read-able", "inStream");
			}
			if (this.currentStream == null)
			{
				this.currentStream = inStream;
				this.sData = null;
				this.sDataObjectCount = 0;
			}
			else if (this.currentStream != inStream)
			{
				this.currentStream = inStream;
				this.sData = null;
				this.sDataObjectCount = 0;
			}
			IX509AttributeCertificate result;
			try
			{
				if (this.sData != null)
				{
					if (this.sDataObjectCount != this.sData.Count)
					{
						result = this.GetCertificate();
					}
					else
					{
						this.sData = null;
						this.sDataObjectCount = 0;
						result = null;
					}
				}
				else
				{
					PushbackStream pushbackStream = new PushbackStream(inStream);
					int num = pushbackStream.ReadByte();
					if (num < 0)
					{
						result = null;
					}
					else
					{
						pushbackStream.Unread(num);
						if (num != 48)
						{
							result = this.ReadPemCertificate(pushbackStream);
						}
						else
						{
							result = this.ReadDerCertificate(new Asn1InputStream(pushbackStream));
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new CertificateException(ex.ToString());
			}
			return result;
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x000AB740 File Offset: 0x000A9940
		public ICollection ReadAttrCerts(Stream inStream)
		{
			IList list = Platform.CreateArrayList();
			IX509AttributeCertificate value;
			while ((value = this.ReadAttrCert(inStream)) != null)
			{
				list.Add(value);
			}
			return list;
		}

		// Token: 0x0400161B RID: 5659
		private static readonly PemParser PemAttrCertParser = new PemParser("ATTRIBUTE CERTIFICATE");

		// Token: 0x0400161C RID: 5660
		private Asn1Set sData;

		// Token: 0x0400161D RID: 5661
		private int sDataObjectCount;

		// Token: 0x0400161E RID: 5662
		private Stream currentStream;
	}
}
