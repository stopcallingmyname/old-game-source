using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs
{
	// Token: 0x020002C9 RID: 713
	public class Pkcs10CertificationRequestDelaySigned : Pkcs10CertificationRequest
	{
		// Token: 0x06001A3E RID: 6718 RVA: 0x000C4B01 File Offset: 0x000C2D01
		protected Pkcs10CertificationRequestDelaySigned()
		{
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x000C4B09 File Offset: 0x000C2D09
		public Pkcs10CertificationRequestDelaySigned(byte[] encoded) : base(encoded)
		{
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x000C4B12 File Offset: 0x000C2D12
		public Pkcs10CertificationRequestDelaySigned(Asn1Sequence seq) : base(seq)
		{
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x000C4B1B File Offset: 0x000C2D1B
		public Pkcs10CertificationRequestDelaySigned(Stream input) : base(input)
		{
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x000C4B24 File Offset: 0x000C2D24
		public Pkcs10CertificationRequestDelaySigned(string signatureAlgorithm, X509Name subject, AsymmetricKeyParameter publicKey, Asn1Set attributes, AsymmetricKeyParameter signingKey) : base(signatureAlgorithm, subject, publicKey, attributes, signingKey)
		{
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x000C4B34 File Offset: 0x000C2D34
		public Pkcs10CertificationRequestDelaySigned(string signatureAlgorithm, X509Name subject, AsymmetricKeyParameter publicKey, Asn1Set attributes)
		{
			if (signatureAlgorithm == null)
			{
				throw new ArgumentNullException("signatureAlgorithm");
			}
			if (subject == null)
			{
				throw new ArgumentNullException("subject");
			}
			if (publicKey == null)
			{
				throw new ArgumentNullException("publicKey");
			}
			if (publicKey.IsPrivate)
			{
				throw new ArgumentException("expected public key", "publicKey");
			}
			string text = Platform.ToUpperInvariant(signatureAlgorithm);
			DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)Pkcs10CertificationRequest.algorithms[text];
			if (derObjectIdentifier == null)
			{
				try
				{
					derObjectIdentifier = new DerObjectIdentifier(text);
				}
				catch (Exception innerException)
				{
					throw new ArgumentException("Unknown signature type requested", innerException);
				}
			}
			if (Pkcs10CertificationRequest.noParams.Contains(derObjectIdentifier))
			{
				this.sigAlgId = new AlgorithmIdentifier(derObjectIdentifier);
			}
			else if (Pkcs10CertificationRequest.exParams.Contains(text))
			{
				this.sigAlgId = new AlgorithmIdentifier(derObjectIdentifier, (Asn1Encodable)Pkcs10CertificationRequest.exParams[text]);
			}
			else
			{
				this.sigAlgId = new AlgorithmIdentifier(derObjectIdentifier, DerNull.Instance);
			}
			SubjectPublicKeyInfo pkInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
			this.reqInfo = new CertificationRequestInfo(subject, pkInfo, attributes);
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x000C4C38 File Offset: 0x000C2E38
		public byte[] GetDataToSign()
		{
			return this.reqInfo.GetDerEncoded();
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x000C4C45 File Offset: 0x000C2E45
		public void SignRequest(byte[] signedData)
		{
			this.sigBits = new DerBitString(signedData);
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x000C4C53 File Offset: 0x000C2E53
		public void SignRequest(DerBitString signedData)
		{
			this.sigBits = signedData;
		}
	}
}
