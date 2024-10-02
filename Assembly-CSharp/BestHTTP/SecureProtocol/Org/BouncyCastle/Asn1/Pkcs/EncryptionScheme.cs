using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006F3 RID: 1779
	public class EncryptionScheme : AlgorithmIdentifier
	{
		// Token: 0x06004127 RID: 16679 RVA: 0x00181CB2 File Offset: 0x0017FEB2
		public EncryptionScheme(DerObjectIdentifier objectID) : base(objectID)
		{
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x00181CBB File Offset: 0x0017FEBB
		public EncryptionScheme(DerObjectIdentifier objectID, Asn1Encodable parameters) : base(objectID, parameters)
		{
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x00181CC5 File Offset: 0x0017FEC5
		internal EncryptionScheme(Asn1Sequence seq) : this((DerObjectIdentifier)seq[0], seq[1])
		{
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x00181CE0 File Offset: 0x0017FEE0
		public new static EncryptionScheme GetInstance(object obj)
		{
			if (obj is EncryptionScheme)
			{
				return (EncryptionScheme)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new EncryptionScheme((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x0600412B RID: 16683 RVA: 0x00181D1F File Offset: 0x0017FF1F
		public Asn1Object Asn1Object
		{
			get
			{
				return this.Parameters.ToAsn1Object();
			}
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x00181D2C File Offset: 0x0017FF2C
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.Algorithm,
				this.Parameters
			});
		}
	}
}
