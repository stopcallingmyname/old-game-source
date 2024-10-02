using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006B1 RID: 1713
	public class SubjectKeyIdentifier : Asn1Encodable
	{
		// Token: 0x06003F29 RID: 16169 RVA: 0x001799D5 File Offset: 0x00177BD5
		public static SubjectKeyIdentifier GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return SubjectKeyIdentifier.GetInstance(Asn1OctetString.GetInstance(obj, explicitly));
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x001799E4 File Offset: 0x00177BE4
		public static SubjectKeyIdentifier GetInstance(object obj)
		{
			if (obj is SubjectKeyIdentifier)
			{
				return (SubjectKeyIdentifier)obj;
			}
			if (obj is SubjectPublicKeyInfo)
			{
				return new SubjectKeyIdentifier((SubjectPublicKeyInfo)obj);
			}
			if (obj is Asn1OctetString)
			{
				return new SubjectKeyIdentifier((Asn1OctetString)obj);
			}
			if (obj is X509Extension)
			{
				return SubjectKeyIdentifier.GetInstance(X509Extension.ConvertValueToObject((X509Extension)obj));
			}
			throw new ArgumentException("Invalid SubjectKeyIdentifier: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x00179A56 File Offset: 0x00177C56
		public SubjectKeyIdentifier(byte[] keyID)
		{
			if (keyID == null)
			{
				throw new ArgumentNullException("keyID");
			}
			this.keyIdentifier = keyID;
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x00179A73 File Offset: 0x00177C73
		public SubjectKeyIdentifier(Asn1OctetString keyID)
		{
			this.keyIdentifier = keyID.GetOctets();
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x00179A87 File Offset: 0x00177C87
		public SubjectKeyIdentifier(SubjectPublicKeyInfo spki)
		{
			this.keyIdentifier = SubjectKeyIdentifier.GetDigest(spki);
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x00179A9B File Offset: 0x00177C9B
		public byte[] GetKeyIdentifier()
		{
			return this.keyIdentifier;
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x00179AA3 File Offset: 0x00177CA3
		public override Asn1Object ToAsn1Object()
		{
			return new DerOctetString(this.keyIdentifier);
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x00179AB0 File Offset: 0x00177CB0
		public static SubjectKeyIdentifier CreateSha1KeyIdentifier(SubjectPublicKeyInfo keyInfo)
		{
			return new SubjectKeyIdentifier(keyInfo);
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x00179AB8 File Offset: 0x00177CB8
		public static SubjectKeyIdentifier CreateTruncatedSha1KeyIdentifier(SubjectPublicKeyInfo keyInfo)
		{
			byte[] digest = SubjectKeyIdentifier.GetDigest(keyInfo);
			byte[] array = new byte[8];
			Array.Copy(digest, digest.Length - 8, array, 0, array.Length);
			byte[] array2 = array;
			int num = 0;
			array2[num] &= 15;
			byte[] array3 = array;
			int num2 = 0;
			array3[num2] |= 64;
			return new SubjectKeyIdentifier(array);
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x00179B08 File Offset: 0x00177D08
		private static byte[] GetDigest(SubjectPublicKeyInfo spki)
		{
			Sha1Digest sha1Digest = new Sha1Digest();
			byte[] array = new byte[((IDigest)sha1Digest).GetDigestSize()];
			byte[] bytes = spki.PublicKeyData.GetBytes();
			((IDigest)sha1Digest).BlockUpdate(bytes, 0, bytes.Length);
			((IDigest)sha1Digest).DoFinal(array, 0);
			return array;
		}

		// Token: 0x04002814 RID: 10260
		private readonly byte[] keyIdentifier;
	}
}
