using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x02000244 RID: 580
	public class X509CertPairParser
	{
		// Token: 0x060014F4 RID: 5364 RVA: 0x000AC5AA File Offset: 0x000AA7AA
		private X509CertificatePair ReadDerCrossCertificatePair(Stream inStream)
		{
			return new X509CertificatePair(CertificatePair.GetInstance((Asn1Sequence)new Asn1InputStream(inStream).ReadObject()));
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x000AC5C6 File Offset: 0x000AA7C6
		public X509CertificatePair ReadCertPair(byte[] input)
		{
			return this.ReadCertPair(new MemoryStream(input, false));
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x000AC5D5 File Offset: 0x000AA7D5
		public ICollection ReadCertPairs(byte[] input)
		{
			return this.ReadCertPairs(new MemoryStream(input, false));
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x000AC5E4 File Offset: 0x000AA7E4
		public X509CertificatePair ReadCertPair(Stream inStream)
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
			}
			else if (this.currentStream != inStream)
			{
				this.currentStream = inStream;
			}
			X509CertificatePair result;
			try
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
					result = this.ReadDerCrossCertificatePair(pushbackStream);
				}
			}
			catch (Exception ex)
			{
				throw new CertificateException(ex.ToString());
			}
			return result;
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x000AC67C File Offset: 0x000AA87C
		public ICollection ReadCertPairs(Stream inStream)
		{
			IList list = Platform.CreateArrayList();
			X509CertificatePair value;
			while ((value = this.ReadCertPair(inStream)) != null)
			{
				list.Add(value);
			}
			return list;
		}

		// Token: 0x0400162B RID: 5675
		private Stream currentStream;
	}
}
