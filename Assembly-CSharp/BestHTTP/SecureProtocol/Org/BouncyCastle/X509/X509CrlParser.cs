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
	// Token: 0x02000247 RID: 583
	public class X509CrlParser
	{
		// Token: 0x0600151B RID: 5403 RVA: 0x000AD1EC File Offset: 0x000AB3EC
		public X509CrlParser() : this(false)
		{
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x000AD1F5 File Offset: 0x000AB3F5
		public X509CrlParser(bool lazyAsn1)
		{
			this.lazyAsn1 = lazyAsn1;
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x000AD204 File Offset: 0x000AB404
		private X509Crl ReadPemCrl(Stream inStream)
		{
			Asn1Sequence asn1Sequence = X509CrlParser.PemCrlParser.ReadPemObject(inStream);
			if (asn1Sequence != null)
			{
				return this.CreateX509Crl(CertificateList.GetInstance(asn1Sequence));
			}
			return null;
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x000AD230 File Offset: 0x000AB430
		private X509Crl ReadDerCrl(Asn1InputStream dIn)
		{
			Asn1Sequence asn1Sequence = (Asn1Sequence)dIn.ReadObject();
			if (asn1Sequence.Count > 1 && asn1Sequence[0] is DerObjectIdentifier && asn1Sequence[0].Equals(PkcsObjectIdentifiers.SignedData))
			{
				this.sCrlData = SignedData.GetInstance(Asn1Sequence.GetInstance((Asn1TaggedObject)asn1Sequence[1], true)).Crls;
				return this.GetCrl();
			}
			return this.CreateX509Crl(CertificateList.GetInstance(asn1Sequence));
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x000AD2A8 File Offset: 0x000AB4A8
		private X509Crl GetCrl()
		{
			if (this.sCrlData == null || this.sCrlDataObjectCount >= this.sCrlData.Count)
			{
				return null;
			}
			Asn1Set asn1Set = this.sCrlData;
			int num = this.sCrlDataObjectCount;
			this.sCrlDataObjectCount = num + 1;
			return this.CreateX509Crl(CertificateList.GetInstance(asn1Set[num]));
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x000AD2F9 File Offset: 0x000AB4F9
		protected virtual X509Crl CreateX509Crl(CertificateList c)
		{
			return new X509Crl(c);
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x000AD301 File Offset: 0x000AB501
		public X509Crl ReadCrl(byte[] input)
		{
			return this.ReadCrl(new MemoryStream(input, false));
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x000AD310 File Offset: 0x000AB510
		public ICollection ReadCrls(byte[] input)
		{
			return this.ReadCrls(new MemoryStream(input, false));
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x000AD320 File Offset: 0x000AB520
		public X509Crl ReadCrl(Stream inStream)
		{
			if (inStream == null)
			{
				throw new ArgumentNullException("inStream");
			}
			if (!inStream.CanRead)
			{
				throw new ArgumentException("inStream must be read-able", "inStream");
			}
			if (this.currentCrlStream == null)
			{
				this.currentCrlStream = inStream;
				this.sCrlData = null;
				this.sCrlDataObjectCount = 0;
			}
			else if (this.currentCrlStream != inStream)
			{
				this.currentCrlStream = inStream;
				this.sCrlData = null;
				this.sCrlDataObjectCount = 0;
			}
			X509Crl result;
			try
			{
				if (this.sCrlData != null)
				{
					if (this.sCrlDataObjectCount != this.sCrlData.Count)
					{
						result = this.GetCrl();
					}
					else
					{
						this.sCrlData = null;
						this.sCrlDataObjectCount = 0;
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
							result = this.ReadPemCrl(pushbackStream);
						}
						else
						{
							Asn1InputStream dIn = this.lazyAsn1 ? new LazyAsn1InputStream(pushbackStream) : new Asn1InputStream(pushbackStream);
							result = this.ReadDerCrl(dIn);
						}
					}
				}
			}
			catch (CrlException ex)
			{
				throw ex;
			}
			catch (Exception ex2)
			{
				throw new CrlException(ex2.ToString());
			}
			return result;
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x000AD440 File Offset: 0x000AB640
		public ICollection ReadCrls(Stream inStream)
		{
			IList list = Platform.CreateArrayList();
			X509Crl value;
			while ((value = this.ReadCrl(inStream)) != null)
			{
				list.Add(value);
			}
			return list;
		}

		// Token: 0x04001634 RID: 5684
		private static readonly PemParser PemCrlParser = new PemParser("CRL");

		// Token: 0x04001635 RID: 5685
		private readonly bool lazyAsn1;

		// Token: 0x04001636 RID: 5686
		private Asn1Set sCrlData;

		// Token: 0x04001637 RID: 5687
		private int sCrlDataObjectCount;

		// Token: 0x04001638 RID: 5688
		private Stream currentCrlStream;
	}
}
