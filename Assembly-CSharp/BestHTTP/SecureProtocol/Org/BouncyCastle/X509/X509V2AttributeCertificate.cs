using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x0200024D RID: 589
	public class X509V2AttributeCertificate : X509ExtensionBase, IX509AttributeCertificate, IX509Extension
	{
		// Token: 0x06001549 RID: 5449 RVA: 0x000ADEE0 File Offset: 0x000AC0E0
		private static AttributeCertificate GetObject(Stream input)
		{
			AttributeCertificate instance;
			try
			{
				instance = AttributeCertificate.GetInstance(Asn1Object.FromStream(input));
			}
			catch (IOException ex)
			{
				throw ex;
			}
			catch (Exception innerException)
			{
				throw new IOException("exception decoding certificate structure", innerException);
			}
			return instance;
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x000ADF28 File Offset: 0x000AC128
		public X509V2AttributeCertificate(Stream encIn) : this(X509V2AttributeCertificate.GetObject(encIn))
		{
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x000ADF36 File Offset: 0x000AC136
		public X509V2AttributeCertificate(byte[] encoded) : this(new MemoryStream(encoded, false))
		{
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x000ADF48 File Offset: 0x000AC148
		internal X509V2AttributeCertificate(AttributeCertificate cert)
		{
			this.cert = cert;
			try
			{
				this.notAfter = cert.ACInfo.AttrCertValidityPeriod.NotAfterTime.ToDateTime();
				this.notBefore = cert.ACInfo.AttrCertValidityPeriod.NotBeforeTime.ToDateTime();
			}
			catch (Exception innerException)
			{
				throw new IOException("invalid data structure in certificate!", innerException);
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x0600154D RID: 5453 RVA: 0x000ADFB8 File Offset: 0x000AC1B8
		public virtual int Version
		{
			get
			{
				return this.cert.ACInfo.Version.Value.IntValue + 1;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x000ADFD6 File Offset: 0x000AC1D6
		public virtual BigInteger SerialNumber
		{
			get
			{
				return this.cert.ACInfo.SerialNumber.Value;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600154F RID: 5455 RVA: 0x000ADFED File Offset: 0x000AC1ED
		public virtual AttributeCertificateHolder Holder
		{
			get
			{
				return new AttributeCertificateHolder((Asn1Sequence)this.cert.ACInfo.Holder.ToAsn1Object());
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06001550 RID: 5456 RVA: 0x000AE00E File Offset: 0x000AC20E
		public virtual AttributeCertificateIssuer Issuer
		{
			get
			{
				return new AttributeCertificateIssuer(this.cert.ACInfo.Issuer);
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06001551 RID: 5457 RVA: 0x000AE025 File Offset: 0x000AC225
		public virtual DateTime NotBefore
		{
			get
			{
				return this.notBefore;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001552 RID: 5458 RVA: 0x000AE02D File Offset: 0x000AC22D
		public virtual DateTime NotAfter
		{
			get
			{
				return this.notAfter;
			}
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x000AE038 File Offset: 0x000AC238
		public virtual bool[] GetIssuerUniqueID()
		{
			DerBitString issuerUniqueID = this.cert.ACInfo.IssuerUniqueID;
			if (issuerUniqueID != null)
			{
				byte[] bytes = issuerUniqueID.GetBytes();
				bool[] array = new bool[bytes.Length * 8 - issuerUniqueID.PadBits];
				for (int num = 0; num != array.Length; num++)
				{
					array[num] = (((int)bytes[num / 8] & 128 >> num % 8) != 0);
				}
				return array;
			}
			return null;
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x000AE09B File Offset: 0x000AC29B
		public virtual bool IsValidNow
		{
			get
			{
				return this.IsValid(DateTime.UtcNow);
			}
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x000AE0A8 File Offset: 0x000AC2A8
		public virtual bool IsValid(DateTime date)
		{
			return date.CompareTo(this.NotBefore) >= 0 && date.CompareTo(this.NotAfter) <= 0;
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x000AE0CF File Offset: 0x000AC2CF
		public virtual void CheckValidity()
		{
			this.CheckValidity(DateTime.UtcNow);
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x000AE0DC File Offset: 0x000AC2DC
		public virtual void CheckValidity(DateTime date)
		{
			if (date.CompareTo(this.NotAfter) > 0)
			{
				throw new CertificateExpiredException("certificate expired on " + this.NotAfter);
			}
			if (date.CompareTo(this.NotBefore) < 0)
			{
				throw new CertificateNotYetValidException("certificate not valid until " + this.NotBefore);
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06001558 RID: 5464 RVA: 0x000AE13F File Offset: 0x000AC33F
		public virtual AlgorithmIdentifier SignatureAlgorithm
		{
			get
			{
				return this.cert.SignatureAlgorithm;
			}
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x000AE14C File Offset: 0x000AC34C
		public virtual byte[] GetSignature()
		{
			return this.cert.GetSignatureOctets();
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x000AE159 File Offset: 0x000AC359
		public virtual void Verify(AsymmetricKeyParameter key)
		{
			this.CheckSignature(new Asn1VerifierFactory(this.cert.SignatureAlgorithm, key));
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x000AE172 File Offset: 0x000AC372
		public virtual void Verify(IVerifierFactoryProvider verifierProvider)
		{
			this.CheckSignature(verifierProvider.CreateVerifierFactory(this.cert.SignatureAlgorithm));
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x000AE18C File Offset: 0x000AC38C
		protected virtual void CheckSignature(IVerifierFactory verifier)
		{
			if (!this.cert.SignatureAlgorithm.Equals(this.cert.ACInfo.Signature))
			{
				throw new CertificateException("Signature algorithm in certificate info not same as outer certificate");
			}
			IStreamCalculator streamCalculator = verifier.CreateCalculator();
			try
			{
				byte[] encoded = this.cert.ACInfo.GetEncoded();
				streamCalculator.Stream.Write(encoded, 0, encoded.Length);
				Platform.Dispose(streamCalculator.Stream);
			}
			catch (IOException exception)
			{
				throw new SignatureException("Exception encoding certificate info object", exception);
			}
			if (!((IVerifier)streamCalculator.GetResult()).IsVerified(this.GetSignature()))
			{
				throw new InvalidKeyException("Public key presented not for certificate signature");
			}
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x000AE23C File Offset: 0x000AC43C
		public virtual byte[] GetEncoded()
		{
			return this.cert.GetEncoded();
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x000AE249 File Offset: 0x000AC449
		protected override X509Extensions GetX509Extensions()
		{
			return this.cert.ACInfo.Extensions;
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x000AE25C File Offset: 0x000AC45C
		public virtual X509Attribute[] GetAttributes()
		{
			Asn1Sequence attributes = this.cert.ACInfo.Attributes;
			X509Attribute[] array = new X509Attribute[attributes.Count];
			for (int num = 0; num != attributes.Count; num++)
			{
				array[num] = new X509Attribute(attributes[num]);
			}
			return array;
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x000AE2A8 File Offset: 0x000AC4A8
		public virtual X509Attribute[] GetAttributes(string oid)
		{
			Asn1Sequence attributes = this.cert.ACInfo.Attributes;
			IList list = Platform.CreateArrayList();
			for (int num = 0; num != attributes.Count; num++)
			{
				X509Attribute x509Attribute = new X509Attribute(attributes[num]);
				if (x509Attribute.Oid.Equals(oid))
				{
					list.Add(x509Attribute);
				}
			}
			if (list.Count < 1)
			{
				return null;
			}
			X509Attribute[] array = new X509Attribute[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				array[i] = (X509Attribute)list[i];
			}
			return array;
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x000AE344 File Offset: 0x000AC544
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			X509V2AttributeCertificate x509V2AttributeCertificate = obj as X509V2AttributeCertificate;
			return x509V2AttributeCertificate != null && this.cert.Equals(x509V2AttributeCertificate.cert);
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x000AE374 File Offset: 0x000AC574
		public override int GetHashCode()
		{
			return this.cert.GetHashCode();
		}

		// Token: 0x0400164B RID: 5707
		private readonly AttributeCertificate cert;

		// Token: 0x0400164C RID: 5708
		private readonly DateTime notBefore;

		// Token: 0x0400164D RID: 5709
		private readonly DateTime notAfter;
	}
}
