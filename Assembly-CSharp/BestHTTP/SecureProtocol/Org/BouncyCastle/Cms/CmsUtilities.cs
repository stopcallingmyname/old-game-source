using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000605 RID: 1541
	internal class CmsUtilities
	{
		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06003A97 RID: 14999 RVA: 0x0016B670 File Offset: 0x00169870
		internal static int MaximumMemory
		{
			get
			{
				long num = 2147483647L;
				if (num > 2147483647L)
				{
					return int.MaxValue;
				}
				return (int)num;
			}
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x0016B695 File Offset: 0x00169895
		internal static ContentInfo ReadContentInfo(byte[] input)
		{
			return CmsUtilities.ReadContentInfo(new Asn1InputStream(input));
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x0016B6A2 File Offset: 0x001698A2
		internal static ContentInfo ReadContentInfo(Stream input)
		{
			return CmsUtilities.ReadContentInfo(new Asn1InputStream(input, CmsUtilities.MaximumMemory));
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x0016B6B4 File Offset: 0x001698B4
		private static ContentInfo ReadContentInfo(Asn1InputStream aIn)
		{
			ContentInfo instance;
			try
			{
				instance = ContentInfo.GetInstance(aIn.ReadObject());
			}
			catch (IOException e)
			{
				throw new CmsException("IOException reading content.", e);
			}
			catch (InvalidCastException e2)
			{
				throw new CmsException("Malformed content.", e2);
			}
			catch (ArgumentException e3)
			{
				throw new CmsException("Malformed content.", e3);
			}
			return instance;
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x0016B720 File Offset: 0x00169920
		public static byte[] StreamToByteArray(Stream inStream)
		{
			return Streams.ReadAll(inStream);
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x0016B728 File Offset: 0x00169928
		public static byte[] StreamToByteArray(Stream inStream, int limit)
		{
			return Streams.ReadAllLimited(inStream, limit);
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x0016B734 File Offset: 0x00169934
		public static IList GetCertificatesFromStore(IX509Store certStore)
		{
			IList result;
			try
			{
				IList list = Platform.CreateArrayList();
				if (certStore != null)
				{
					foreach (object obj in certStore.GetMatches(null))
					{
						X509Certificate x509Certificate = (X509Certificate)obj;
						list.Add(X509CertificateStructure.GetInstance(Asn1Object.FromByteArray(x509Certificate.GetEncoded())));
					}
				}
				result = list;
			}
			catch (CertificateEncodingException e)
			{
				throw new CmsException("error encoding certs", e);
			}
			catch (Exception e2)
			{
				throw new CmsException("error processing certs", e2);
			}
			return result;
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x0016B7E4 File Offset: 0x001699E4
		public static IList GetCrlsFromStore(IX509Store crlStore)
		{
			IList result;
			try
			{
				IList list = Platform.CreateArrayList();
				if (crlStore != null)
				{
					foreach (object obj in crlStore.GetMatches(null))
					{
						X509Crl x509Crl = (X509Crl)obj;
						list.Add(CertificateList.GetInstance(Asn1Object.FromByteArray(x509Crl.GetEncoded())));
					}
				}
				result = list;
			}
			catch (CrlException e)
			{
				throw new CmsException("error encoding crls", e);
			}
			catch (Exception e2)
			{
				throw new CmsException("error processing crls", e2);
			}
			return result;
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x0016B894 File Offset: 0x00169A94
		public static Asn1Set CreateBerSetFromList(IList berObjects)
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in berObjects)
			{
				Asn1Encodable asn1Encodable = (Asn1Encodable)obj;
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					asn1Encodable
				});
			}
			return new BerSet(asn1EncodableVector);
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x0016B904 File Offset: 0x00169B04
		public static Asn1Set CreateDerSetFromList(IList derObjects)
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in derObjects)
			{
				Asn1Encodable asn1Encodable = (Asn1Encodable)obj;
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					asn1Encodable
				});
			}
			return new DerSet(asn1EncodableVector);
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x0016B974 File Offset: 0x00169B74
		internal static Stream CreateBerOctetOutputStream(Stream s, int tagNo, bool isExplicit, int bufferSize)
		{
			return new BerOctetStringGenerator(s, tagNo, isExplicit).GetOctetOutputStream(bufferSize);
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x0016B984 File Offset: 0x00169B84
		internal static TbsCertificateStructure GetTbsCertificateStructure(X509Certificate cert)
		{
			return TbsCertificateStructure.GetInstance(Asn1Object.FromByteArray(cert.GetTbsCertificate()));
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x0016B998 File Offset: 0x00169B98
		internal static IssuerAndSerialNumber GetIssuerAndSerialNumber(X509Certificate cert)
		{
			TbsCertificateStructure tbsCertificateStructure = CmsUtilities.GetTbsCertificateStructure(cert);
			return new IssuerAndSerialNumber(tbsCertificateStructure.Issuer, tbsCertificateStructure.SerialNumber.Value);
		}
	}
}
