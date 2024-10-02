using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.OpenSsl;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002B4 RID: 692
	public class PkixCertPath
	{
		// Token: 0x06001918 RID: 6424 RVA: 0x000BBFA0 File Offset: 0x000BA1A0
		static PkixCertPath()
		{
			IList list = Platform.CreateArrayList();
			list.Add("PkiPath");
			list.Add("PEM");
			list.Add("PKCS7");
			PkixCertPath.certPathEncodings = CollectionUtilities.ReadOnly(list);
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x000BBFD8 File Offset: 0x000BA1D8
		private static IList SortCerts(IList certs)
		{
			if (certs.Count < 2)
			{
				return certs;
			}
			X509Name issuerDN = ((X509Certificate)certs[0]).IssuerDN;
			bool flag = true;
			for (int num = 1; num != certs.Count; num++)
			{
				X509Certificate x509Certificate = (X509Certificate)certs[num];
				if (!issuerDN.Equivalent(x509Certificate.SubjectDN, true))
				{
					flag = false;
					break;
				}
				issuerDN = ((X509Certificate)certs[num]).IssuerDN;
			}
			if (flag)
			{
				return certs;
			}
			IList list = Platform.CreateArrayList(certs.Count);
			IList result = Platform.CreateArrayList(certs);
			for (int i = 0; i < certs.Count; i++)
			{
				X509Certificate x509Certificate2 = (X509Certificate)certs[i];
				bool flag2 = false;
				X509Name subjectDN = x509Certificate2.SubjectDN;
				using (IEnumerator enumerator = certs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((X509Certificate)enumerator.Current).IssuerDN.Equivalent(subjectDN, true))
						{
							flag2 = true;
							break;
						}
					}
				}
				if (!flag2)
				{
					list.Add(x509Certificate2);
					certs.RemoveAt(i);
				}
			}
			if (list.Count > 1)
			{
				return result;
			}
			for (int num2 = 0; num2 != list.Count; num2++)
			{
				issuerDN = ((X509Certificate)list[num2]).IssuerDN;
				for (int j = 0; j < certs.Count; j++)
				{
					X509Certificate x509Certificate3 = (X509Certificate)certs[j];
					if (issuerDN.Equivalent(x509Certificate3.SubjectDN, true))
					{
						list.Add(x509Certificate3);
						certs.RemoveAt(j);
						break;
					}
				}
			}
			if (certs.Count > 0)
			{
				return result;
			}
			return list;
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x000BC194 File Offset: 0x000BA394
		public PkixCertPath(ICollection certificates)
		{
			this.certificates = PkixCertPath.SortCerts(Platform.CreateArrayList(certificates));
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x000BC1AD File Offset: 0x000BA3AD
		public PkixCertPath(Stream inStream) : this(inStream, "PkiPath")
		{
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x000BC1BC File Offset: 0x000BA3BC
		public PkixCertPath(Stream inStream, string encoding)
		{
			string text = Platform.ToUpperInvariant(encoding);
			IList list;
			try
			{
				if (text.Equals(Platform.ToUpperInvariant("PkiPath")))
				{
					Asn1Object asn1Object = new Asn1InputStream(inStream).ReadObject();
					if (!(asn1Object is Asn1Sequence))
					{
						throw new CertificateException("input stream does not contain a ASN1 SEQUENCE while reading PkiPath encoded data to load CertPath");
					}
					list = Platform.CreateArrayList();
					using (IEnumerator enumerator = ((Asn1Sequence)asn1Object).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							Stream inStream2 = new MemoryStream(((Asn1Encodable)obj).GetEncoded("DER"), false);
							list.Insert(0, new X509CertificateParser().ReadCertificate(inStream2));
						}
						goto IL_DA;
					}
				}
				if (!text.Equals("PKCS7") && !text.Equals("PEM"))
				{
					throw new CertificateException("unsupported encoding: " + encoding);
				}
				list = Platform.CreateArrayList(new X509CertificateParser().ReadCertificates(inStream));
				IL_DA:;
			}
			catch (IOException ex)
			{
				throw new CertificateException("IOException throw while decoding CertPath:\n" + ex.ToString());
			}
			this.certificates = PkixCertPath.SortCerts(list);
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x0600191D RID: 6429 RVA: 0x000BC2E8 File Offset: 0x000BA4E8
		public virtual IEnumerable Encodings
		{
			get
			{
				return new EnumerableProxy(PkixCertPath.certPathEncodings);
			}
		}

		// Token: 0x0600191E RID: 6430 RVA: 0x000BC2F4 File Offset: 0x000BA4F4
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			PkixCertPath pkixCertPath = obj as PkixCertPath;
			if (pkixCertPath == null)
			{
				return false;
			}
			IList list = this.Certificates;
			IList list2 = pkixCertPath.Certificates;
			if (list.Count != list2.Count)
			{
				return false;
			}
			IEnumerator enumerator = list.GetEnumerator();
			IEnumerator enumerator2 = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator2.MoveNext();
				if (!object.Equals(enumerator.Current, enumerator2.Current))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x000BC369 File Offset: 0x000BA569
		public override int GetHashCode()
		{
			return this.Certificates.GetHashCode();
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x000BC378 File Offset: 0x000BA578
		public virtual byte[] GetEncoded()
		{
			foreach (object obj in this.Encodings)
			{
				if (obj is string)
				{
					return this.GetEncoded((string)obj);
				}
			}
			return null;
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x000BC3E0 File Offset: 0x000BA5E0
		public virtual byte[] GetEncoded(string encoding)
		{
			if (Platform.EqualsIgnoreCase(encoding, "PkiPath"))
			{
				Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
				for (int i = this.certificates.Count - 1; i >= 0; i--)
				{
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						this.ToAsn1Object((X509Certificate)this.certificates[i])
					});
				}
				return this.ToDerEncoded(new DerSequence(asn1EncodableVector));
			}
			if (Platform.EqualsIgnoreCase(encoding, "PKCS7"))
			{
				ContentInfo contentInfo = new ContentInfo(PkcsObjectIdentifiers.Data, null);
				Asn1EncodableVector asn1EncodableVector2 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
				for (int num = 0; num != this.certificates.Count; num++)
				{
					asn1EncodableVector2.Add(new Asn1Encodable[]
					{
						this.ToAsn1Object((X509Certificate)this.certificates[num])
					});
				}
				SignedData content = new SignedData(new DerInteger(1), new DerSet(), contentInfo, new DerSet(asn1EncodableVector2), null, new DerSet());
				return this.ToDerEncoded(new ContentInfo(PkcsObjectIdentifiers.SignedData, content));
			}
			if (Platform.EqualsIgnoreCase(encoding, "PEM"))
			{
				MemoryStream memoryStream = new MemoryStream();
				PemWriter pemWriter = new PemWriter(new StreamWriter(memoryStream));
				try
				{
					for (int num2 = 0; num2 != this.certificates.Count; num2++)
					{
						pemWriter.WriteObject(this.certificates[num2]);
					}
					Platform.Dispose(pemWriter.Writer);
				}
				catch (Exception)
				{
					throw new CertificateEncodingException("can't encode certificate for PEM encoded path");
				}
				return memoryStream.ToArray();
			}
			throw new CertificateEncodingException("unsupported encoding: " + encoding);
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001922 RID: 6434 RVA: 0x000BC57C File Offset: 0x000BA77C
		public virtual IList Certificates
		{
			get
			{
				return CollectionUtilities.ReadOnly(this.certificates);
			}
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x000BC58C File Offset: 0x000BA78C
		private Asn1Object ToAsn1Object(X509Certificate cert)
		{
			Asn1Object result;
			try
			{
				result = Asn1Object.FromByteArray(cert.GetEncoded());
			}
			catch (Exception e)
			{
				throw new CertificateEncodingException("Exception while encoding certificate", e);
			}
			return result;
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x000BC5C8 File Offset: 0x000BA7C8
		private byte[] ToDerEncoded(Asn1Encodable obj)
		{
			byte[] encoded;
			try
			{
				encoded = obj.GetEncoded("DER");
			}
			catch (IOException e)
			{
				throw new CertificateEncodingException("Exception thrown", e);
			}
			return encoded;
		}

		// Token: 0x04001872 RID: 6258
		internal static readonly IList certPathEncodings;

		// Token: 0x04001873 RID: 6259
		private readonly IList certificates;
	}
}
