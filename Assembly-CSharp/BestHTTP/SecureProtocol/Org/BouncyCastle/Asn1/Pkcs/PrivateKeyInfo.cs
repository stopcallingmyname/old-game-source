using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006FD RID: 1789
	public class PrivateKeyInfo : Asn1Encodable
	{
		// Token: 0x06004163 RID: 16739 RVA: 0x00182DCD File Offset: 0x00180FCD
		public static PrivateKeyInfo GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return PrivateKeyInfo.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x00182DDB File Offset: 0x00180FDB
		public static PrivateKeyInfo GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is PrivateKeyInfo)
			{
				return (PrivateKeyInfo)obj;
			}
			return new PrivateKeyInfo(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x00182DFC File Offset: 0x00180FFC
		private static int GetVersionValue(DerInteger version)
		{
			BigInteger value = version.Value;
			if (value.CompareTo(BigInteger.Zero) < 0 || value.CompareTo(BigInteger.One) > 0)
			{
				throw new ArgumentException("invalid version for private key info", "version");
			}
			return value.IntValue;
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x00182E42 File Offset: 0x00181042
		public PrivateKeyInfo(AlgorithmIdentifier privateKeyAlgorithm, Asn1Encodable privateKey) : this(privateKeyAlgorithm, privateKey, null, null)
		{
		}

		// Token: 0x06004167 RID: 16743 RVA: 0x00182E4E File Offset: 0x0018104E
		public PrivateKeyInfo(AlgorithmIdentifier privateKeyAlgorithm, Asn1Encodable privateKey, Asn1Set attributes) : this(privateKeyAlgorithm, privateKey, attributes, null)
		{
		}

		// Token: 0x06004168 RID: 16744 RVA: 0x00182E5C File Offset: 0x0018105C
		public PrivateKeyInfo(AlgorithmIdentifier privateKeyAlgorithm, Asn1Encodable privateKey, Asn1Set attributes, byte[] publicKey)
		{
			this.version = new DerInteger((publicKey != null) ? BigInteger.One : BigInteger.Zero);
			this.privateKeyAlgorithm = privateKeyAlgorithm;
			this.privateKey = new DerOctetString(privateKey);
			this.attributes = attributes;
			this.publicKey = ((publicKey == null) ? null : new DerBitString(publicKey));
		}

		// Token: 0x06004169 RID: 16745 RVA: 0x00182EB8 File Offset: 0x001810B8
		private PrivateKeyInfo(Asn1Sequence seq)
		{
			IEnumerator enumerator = seq.GetEnumerator();
			this.version = DerInteger.GetInstance(CollectionUtilities.RequireNext(enumerator));
			int versionValue = PrivateKeyInfo.GetVersionValue(this.version);
			this.privateKeyAlgorithm = AlgorithmIdentifier.GetInstance(CollectionUtilities.RequireNext(enumerator));
			this.privateKey = Asn1OctetString.GetInstance(CollectionUtilities.RequireNext(enumerator));
			int num = -1;
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
				int tagNo = asn1TaggedObject.TagNo;
				if (tagNo <= num)
				{
					throw new ArgumentException("invalid optional field in private key info", "seq");
				}
				num = tagNo;
				if (tagNo != 0)
				{
					if (tagNo != 1)
					{
						throw new ArgumentException("unknown optional field in private key info", "seq");
					}
					if (versionValue < 1)
					{
						throw new ArgumentException("'publicKey' requires version v2(1) or later", "seq");
					}
					this.publicKey = DerBitString.GetInstance(asn1TaggedObject, false);
				}
				else
				{
					this.attributes = Asn1Set.GetInstance(asn1TaggedObject, false);
				}
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x0600416A RID: 16746 RVA: 0x00182F99 File Offset: 0x00181199
		public virtual Asn1Set Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x0600416B RID: 16747 RVA: 0x00182FA1 File Offset: 0x001811A1
		public virtual bool HasPublicKey
		{
			get
			{
				return this.publicKey != null;
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x0600416C RID: 16748 RVA: 0x00182FAC File Offset: 0x001811AC
		public virtual AlgorithmIdentifier PrivateKeyAlgorithm
		{
			get
			{
				return this.privateKeyAlgorithm;
			}
		}

		// Token: 0x0600416D RID: 16749 RVA: 0x00182FB4 File Offset: 0x001811B4
		public virtual Asn1Object ParsePrivateKey()
		{
			return Asn1Object.FromByteArray(this.privateKey.GetOctets());
		}

		// Token: 0x0600416E RID: 16750 RVA: 0x00182FC6 File Offset: 0x001811C6
		public virtual Asn1Object ParsePublicKey()
		{
			if (this.publicKey != null)
			{
				return Asn1Object.FromByteArray(this.publicKey.GetOctets());
			}
			return null;
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x0600416F RID: 16751 RVA: 0x00182FE2 File Offset: 0x001811E2
		public virtual DerBitString PublicKeyData
		{
			get
			{
				return this.publicKey;
			}
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x00182FEC File Offset: 0x001811EC
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				this.privateKeyAlgorithm,
				this.privateKey
			});
			if (this.attributes != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.attributes)
				});
			}
			if (this.publicKey != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.publicKey)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002A65 RID: 10853
		private readonly DerInteger version;

		// Token: 0x04002A66 RID: 10854
		private readonly AlgorithmIdentifier privateKeyAlgorithm;

		// Token: 0x04002A67 RID: 10855
		private readonly Asn1OctetString privateKey;

		// Token: 0x04002A68 RID: 10856
		private readonly Asn1Set attributes;

		// Token: 0x04002A69 RID: 10857
		private readonly DerBitString publicKey;
	}
}
