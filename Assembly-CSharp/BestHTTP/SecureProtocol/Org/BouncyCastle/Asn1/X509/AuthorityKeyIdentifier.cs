using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200068D RID: 1677
	public class AuthorityKeyIdentifier : Asn1Encodable
	{
		// Token: 0x06003E19 RID: 15897 RVA: 0x00175F9E File Offset: 0x0017419E
		public static AuthorityKeyIdentifier GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return AuthorityKeyIdentifier.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x00175FAC File Offset: 0x001741AC
		public static AuthorityKeyIdentifier GetInstance(object obj)
		{
			if (obj is AuthorityKeyIdentifier)
			{
				return (AuthorityKeyIdentifier)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new AuthorityKeyIdentifier((Asn1Sequence)obj);
			}
			if (obj is X509Extension)
			{
				return AuthorityKeyIdentifier.GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x00176010 File Offset: 0x00174210
		protected internal AuthorityKeyIdentifier(Asn1Sequence seq)
		{
			foreach (object obj in seq)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
				switch (asn1TaggedObject.TagNo)
				{
				case 0:
					this.keyidentifier = Asn1OctetString.GetInstance(asn1TaggedObject, false);
					break;
				case 1:
					this.certissuer = GeneralNames.GetInstance(asn1TaggedObject, false);
					break;
				case 2:
					this.certserno = DerInteger.GetInstance(asn1TaggedObject, false);
					break;
				default:
					throw new ArgumentException("illegal tag");
				}
			}
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x001760B8 File Offset: 0x001742B8
		public AuthorityKeyIdentifier(SubjectPublicKeyInfo spki)
		{
			Sha1Digest sha1Digest = new Sha1Digest();
			byte[] array = new byte[((IDigest)sha1Digest).GetDigestSize()];
			byte[] bytes = spki.PublicKeyData.GetBytes();
			((IDigest)sha1Digest).BlockUpdate(bytes, 0, bytes.Length);
			((IDigest)sha1Digest).DoFinal(array, 0);
			this.keyidentifier = new DerOctetString(array);
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x00176108 File Offset: 0x00174308
		public AuthorityKeyIdentifier(SubjectPublicKeyInfo spki, GeneralNames name, BigInteger serialNumber)
		{
			Sha1Digest sha1Digest = new Sha1Digest();
			byte[] array = new byte[((IDigest)sha1Digest).GetDigestSize()];
			byte[] bytes = spki.PublicKeyData.GetBytes();
			((IDigest)sha1Digest).BlockUpdate(bytes, 0, bytes.Length);
			((IDigest)sha1Digest).DoFinal(array, 0);
			this.keyidentifier = new DerOctetString(array);
			this.certissuer = name;
			this.certserno = new DerInteger(serialNumber);
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x0017616A File Offset: 0x0017436A
		public AuthorityKeyIdentifier(GeneralNames name, BigInteger serialNumber)
		{
			this.keyidentifier = null;
			this.certissuer = GeneralNames.GetInstance(name.ToAsn1Object());
			this.certserno = new DerInteger(serialNumber);
		}

		// Token: 0x06003E1F RID: 15903 RVA: 0x00176196 File Offset: 0x00174396
		public AuthorityKeyIdentifier(byte[] keyIdentifier)
		{
			this.keyidentifier = new DerOctetString(keyIdentifier);
			this.certissuer = null;
			this.certserno = null;
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x001761B8 File Offset: 0x001743B8
		public AuthorityKeyIdentifier(byte[] keyIdentifier, GeneralNames name, BigInteger serialNumber)
		{
			this.keyidentifier = new DerOctetString(keyIdentifier);
			this.certissuer = GeneralNames.GetInstance(name.ToAsn1Object());
			this.certserno = new DerInteger(serialNumber);
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x001761E9 File Offset: 0x001743E9
		public byte[] GetKeyIdentifier()
		{
			if (this.keyidentifier != null)
			{
				return this.keyidentifier.GetOctets();
			}
			return null;
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06003E22 RID: 15906 RVA: 0x00176200 File Offset: 0x00174400
		public GeneralNames AuthorityCertIssuer
		{
			get
			{
				return this.certissuer;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06003E23 RID: 15907 RVA: 0x00176208 File Offset: 0x00174408
		public BigInteger AuthorityCertSerialNumber
		{
			get
			{
				if (this.certserno != null)
				{
					return this.certserno.Value;
				}
				return null;
			}
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x00176220 File Offset: 0x00174420
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.keyidentifier != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.keyidentifier)
				});
			}
			if (this.certissuer != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.certissuer)
				});
			}
			if (this.certserno != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 2, this.certserno)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x06003E25 RID: 15909 RVA: 0x001762AA File Offset: 0x001744AA
		public override string ToString()
		{
			return "AuthorityKeyIdentifier: KeyID(" + this.keyidentifier.GetOctets() + ")";
		}

		// Token: 0x0400278C RID: 10124
		internal readonly Asn1OctetString keyidentifier;

		// Token: 0x0400278D RID: 10125
		internal readonly GeneralNames certissuer;

		// Token: 0x0400278E RID: 10126
		internal readonly DerInteger certserno;
	}
}
