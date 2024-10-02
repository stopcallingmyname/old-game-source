using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X500;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000759 RID: 1881
	public class SignerLocation : Asn1Encodable
	{
		// Token: 0x060043C0 RID: 17344 RVA: 0x0018B7A4 File Offset: 0x001899A4
		public SignerLocation(Asn1Sequence seq)
		{
			foreach (object obj in seq)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
				switch (asn1TaggedObject.TagNo)
				{
				case 0:
					this.countryName = DirectoryString.GetInstance(asn1TaggedObject, true);
					break;
				case 1:
					this.localityName = DirectoryString.GetInstance(asn1TaggedObject, true);
					break;
				case 2:
				{
					bool explicitly = asn1TaggedObject.IsExplicit();
					this.postalAddress = Asn1Sequence.GetInstance(asn1TaggedObject, explicitly);
					if (this.postalAddress != null && this.postalAddress.Count > 6)
					{
						throw new ArgumentException("postal address must contain less than 6 strings");
					}
					break;
				}
				default:
					throw new ArgumentException("illegal tag");
				}
			}
		}

		// Token: 0x060043C1 RID: 17345 RVA: 0x0018B87C File Offset: 0x00189A7C
		private SignerLocation(DirectoryString countryName, DirectoryString localityName, Asn1Sequence postalAddress)
		{
			if (postalAddress != null && postalAddress.Count > 6)
			{
				throw new ArgumentException("postal address must contain less than 6 strings");
			}
			this.countryName = countryName;
			this.localityName = localityName;
			this.postalAddress = postalAddress;
		}

		// Token: 0x060043C2 RID: 17346 RVA: 0x0018B8B0 File Offset: 0x00189AB0
		public SignerLocation(DirectoryString countryName, DirectoryString localityName, DirectoryString[] postalAddress) : this(countryName, localityName, new DerSequence(postalAddress))
		{
		}

		// Token: 0x060043C3 RID: 17347 RVA: 0x0018B8CD File Offset: 0x00189ACD
		public SignerLocation(DerUtf8String countryName, DerUtf8String localityName, Asn1Sequence postalAddress) : this(DirectoryString.GetInstance(countryName), DirectoryString.GetInstance(localityName), postalAddress)
		{
		}

		// Token: 0x060043C4 RID: 17348 RVA: 0x0018B8E2 File Offset: 0x00189AE2
		public static SignerLocation GetInstance(object obj)
		{
			if (obj == null || obj is SignerLocation)
			{
				return (SignerLocation)obj;
			}
			return new SignerLocation(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x060043C5 RID: 17349 RVA: 0x0018B901 File Offset: 0x00189B01
		public DirectoryString Country
		{
			get
			{
				return this.countryName;
			}
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x060043C6 RID: 17350 RVA: 0x0018B909 File Offset: 0x00189B09
		public DirectoryString Locality
		{
			get
			{
				return this.localityName;
			}
		}

		// Token: 0x060043C7 RID: 17351 RVA: 0x0018B914 File Offset: 0x00189B14
		public DirectoryString[] GetPostal()
		{
			if (this.postalAddress == null)
			{
				return null;
			}
			DirectoryString[] array = new DirectoryString[this.postalAddress.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = DirectoryString.GetInstance(this.postalAddress[num]);
			}
			return array;
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x060043C8 RID: 17352 RVA: 0x0018B95F File Offset: 0x00189B5F
		[Obsolete("Use 'Country' property instead")]
		public DerUtf8String CountryName
		{
			get
			{
				if (this.countryName != null)
				{
					return new DerUtf8String(this.countryName.GetString());
				}
				return null;
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x060043C9 RID: 17353 RVA: 0x0018B97B File Offset: 0x00189B7B
		[Obsolete("Use 'Locality' property instead")]
		public DerUtf8String LocalityName
		{
			get
			{
				if (this.localityName != null)
				{
					return new DerUtf8String(this.localityName.GetString());
				}
				return null;
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x060043CA RID: 17354 RVA: 0x0018B997 File Offset: 0x00189B97
		public Asn1Sequence PostalAddress
		{
			get
			{
				return this.postalAddress;
			}
		}

		// Token: 0x060043CB RID: 17355 RVA: 0x0018B9A0 File Offset: 0x00189BA0
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.countryName != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.countryName)
				});
			}
			if (this.localityName != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, this.localityName)
				});
			}
			if (this.postalAddress != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 2, this.postalAddress)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002C59 RID: 11353
		private DirectoryString countryName;

		// Token: 0x04002C5A RID: 11354
		private DirectoryString localityName;

		// Token: 0x04002C5B RID: 11355
		private Asn1Sequence postalAddress;
	}
}
