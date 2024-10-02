using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006F8 RID: 1784
	public class PbeS2Parameters : Asn1Encodable
	{
		// Token: 0x06004143 RID: 16707 RVA: 0x0018209C File Offset: 0x0018029C
		public static PbeS2Parameters GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			PbeS2Parameters pbeS2Parameters = obj as PbeS2Parameters;
			if (pbeS2Parameters != null)
			{
				return pbeS2Parameters;
			}
			return new PbeS2Parameters(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x001820C5 File Offset: 0x001802C5
		public PbeS2Parameters(KeyDerivationFunc keyDevFunc, EncryptionScheme encScheme)
		{
			this.func = keyDevFunc;
			this.scheme = encScheme;
		}

		// Token: 0x06004145 RID: 16709 RVA: 0x001820DC File Offset: 0x001802DC
		[Obsolete("Use GetInstance() instead")]
		public PbeS2Parameters(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			Asn1Sequence asn1Sequence = (Asn1Sequence)seq[0].ToAsn1Object();
			if (asn1Sequence[0].Equals(PkcsObjectIdentifiers.IdPbkdf2))
			{
				this.func = new KeyDerivationFunc(PkcsObjectIdentifiers.IdPbkdf2, Pbkdf2Params.GetInstance(asn1Sequence[1]));
			}
			else
			{
				this.func = new KeyDerivationFunc(asn1Sequence);
			}
			this.scheme = EncryptionScheme.GetInstance(seq[1].ToAsn1Object());
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06004146 RID: 16710 RVA: 0x0018216E File Offset: 0x0018036E
		public KeyDerivationFunc KeyDerivationFunc
		{
			get
			{
				return this.func;
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06004147 RID: 16711 RVA: 0x00182176 File Offset: 0x00180376
		public EncryptionScheme EncryptionScheme
		{
			get
			{
				return this.scheme;
			}
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x0018217E File Offset: 0x0018037E
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.func,
				this.scheme
			});
		}

		// Token: 0x040029CA RID: 10698
		private readonly KeyDerivationFunc func;

		// Token: 0x040029CB RID: 10699
		private readonly EncryptionScheme scheme;
	}
}
