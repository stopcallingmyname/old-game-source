using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X500;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509.SigI
{
	// Token: 0x020006CB RID: 1739
	public class PersonalData : Asn1Encodable
	{
		// Token: 0x06004024 RID: 16420 RVA: 0x0017D868 File Offset: 0x0017BA68
		public static PersonalData GetInstance(object obj)
		{
			if (obj == null || obj is PersonalData)
			{
				return (PersonalData)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PersonalData((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x0017D8B8 File Offset: 0x0017BAB8
		private PersonalData(Asn1Sequence seq)
		{
			if (seq.Count < 1)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			this.nameOrPseudonym = NameOrPseudonym.GetInstance(enumerator.Current);
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(obj);
				switch (instance.TagNo)
				{
				case 0:
					this.nameDistinguisher = DerInteger.GetInstance(instance, false).Value;
					break;
				case 1:
					this.dateOfBirth = DerGeneralizedTime.GetInstance(instance, false);
					break;
				case 2:
					this.placeOfBirth = DirectoryString.GetInstance(instance, true);
					break;
				case 3:
					this.gender = DerPrintableString.GetInstance(instance, false).GetString();
					break;
				case 4:
					this.postalAddress = DirectoryString.GetInstance(instance, true);
					break;
				default:
					throw new ArgumentException("Bad tag number: " + instance.TagNo);
				}
			}
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x0017D9BD File Offset: 0x0017BBBD
		public PersonalData(NameOrPseudonym nameOrPseudonym, BigInteger nameDistinguisher, DerGeneralizedTime dateOfBirth, DirectoryString placeOfBirth, string gender, DirectoryString postalAddress)
		{
			this.nameOrPseudonym = nameOrPseudonym;
			this.dateOfBirth = dateOfBirth;
			this.gender = gender;
			this.nameDistinguisher = nameDistinguisher;
			this.postalAddress = postalAddress;
			this.placeOfBirth = placeOfBirth;
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06004027 RID: 16423 RVA: 0x0017D9F2 File Offset: 0x0017BBF2
		public NameOrPseudonym NameOrPseudonym
		{
			get
			{
				return this.nameOrPseudonym;
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06004028 RID: 16424 RVA: 0x0017D9FA File Offset: 0x0017BBFA
		public BigInteger NameDistinguisher
		{
			get
			{
				return this.nameDistinguisher;
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06004029 RID: 16425 RVA: 0x0017DA02 File Offset: 0x0017BC02
		public DerGeneralizedTime DateOfBirth
		{
			get
			{
				return this.dateOfBirth;
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x0600402A RID: 16426 RVA: 0x0017DA0A File Offset: 0x0017BC0A
		public DirectoryString PlaceOfBirth
		{
			get
			{
				return this.placeOfBirth;
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x0600402B RID: 16427 RVA: 0x0017DA12 File Offset: 0x0017BC12
		public string Gender
		{
			get
			{
				return this.gender;
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x0600402C RID: 16428 RVA: 0x0017DA1A File Offset: 0x0017BC1A
		public DirectoryString PostalAddress
		{
			get
			{
				return this.postalAddress;
			}
		}

		// Token: 0x0600402D RID: 16429 RVA: 0x0017DA24 File Offset: 0x0017BC24
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.nameOrPseudonym
			});
			if (this.nameDistinguisher != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, new DerInteger(this.nameDistinguisher))
				});
			}
			if (this.dateOfBirth != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.dateOfBirth)
				});
			}
			if (this.placeOfBirth != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 2, this.placeOfBirth)
				});
			}
			if (this.gender != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 3, new DerPrintableString(this.gender, true))
				});
			}
			if (this.postalAddress != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 4, this.postalAddress)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x040028D1 RID: 10449
		private readonly NameOrPseudonym nameOrPseudonym;

		// Token: 0x040028D2 RID: 10450
		private readonly BigInteger nameDistinguisher;

		// Token: 0x040028D3 RID: 10451
		private readonly DerGeneralizedTime dateOfBirth;

		// Token: 0x040028D4 RID: 10452
		private readonly DirectoryString placeOfBirth;

		// Token: 0x040028D5 RID: 10453
		private readonly string gender;

		// Token: 0x040028D6 RID: 10454
		private readonly DirectoryString postalAddress;
	}
}
