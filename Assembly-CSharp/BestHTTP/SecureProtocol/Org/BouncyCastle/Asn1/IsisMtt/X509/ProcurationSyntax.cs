using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X500;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.IsisMtt.X509
{
	// Token: 0x0200072B RID: 1835
	public class ProcurationSyntax : Asn1Encodable
	{
		// Token: 0x06004297 RID: 17047 RVA: 0x00186D44 File Offset: 0x00184F44
		public static ProcurationSyntax GetInstance(object obj)
		{
			if (obj == null || obj is ProcurationSyntax)
			{
				return (ProcurationSyntax)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new ProcurationSyntax((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x00186D94 File Offset: 0x00184F94
		private ProcurationSyntax(Asn1Sequence seq)
		{
			if (seq.Count < 1 || seq.Count > 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			foreach (object obj in seq)
			{
				Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(obj);
				switch (instance.TagNo)
				{
				case 1:
					this.country = DerPrintableString.GetInstance(instance, true).GetString();
					break;
				case 2:
					this.typeOfSubstitution = DirectoryString.GetInstance(instance, true);
					break;
				case 3:
				{
					Asn1Object @object = instance.GetObject();
					if (@object is Asn1TaggedObject)
					{
						this.thirdPerson = GeneralName.GetInstance(@object);
					}
					else
					{
						this.certRef = IssuerSerial.GetInstance(@object);
					}
					break;
				}
				default:
					throw new ArgumentException("Bad tag number: " + instance.TagNo);
				}
			}
		}

		// Token: 0x06004299 RID: 17049 RVA: 0x00186E7D File Offset: 0x0018507D
		public ProcurationSyntax(string country, DirectoryString typeOfSubstitution, IssuerSerial certRef)
		{
			this.country = country;
			this.typeOfSubstitution = typeOfSubstitution;
			this.thirdPerson = null;
			this.certRef = certRef;
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x00186EA1 File Offset: 0x001850A1
		public ProcurationSyntax(string country, DirectoryString typeOfSubstitution, GeneralName thirdPerson)
		{
			this.country = country;
			this.typeOfSubstitution = typeOfSubstitution;
			this.thirdPerson = thirdPerson;
			this.certRef = null;
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x0600429B RID: 17051 RVA: 0x00186EC5 File Offset: 0x001850C5
		public virtual string Country
		{
			get
			{
				return this.country;
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x0600429C RID: 17052 RVA: 0x00186ECD File Offset: 0x001850CD
		public virtual DirectoryString TypeOfSubstitution
		{
			get
			{
				return this.typeOfSubstitution;
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x0600429D RID: 17053 RVA: 0x00186ED5 File Offset: 0x001850D5
		public virtual GeneralName ThirdPerson
		{
			get
			{
				return this.thirdPerson;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x0600429E RID: 17054 RVA: 0x00186EDD File Offset: 0x001850DD
		public virtual IssuerSerial CertRef
		{
			get
			{
				return this.certRef;
			}
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x00186EE8 File Offset: 0x001850E8
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.country != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, new DerPrintableString(this.country, true))
				});
			}
			if (this.typeOfSubstitution != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 2, this.typeOfSubstitution)
				});
			}
			if (this.thirdPerson != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 3, this.thirdPerson)
				});
			}
			else
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 3, this.certRef)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002B71 RID: 11121
		private readonly string country;

		// Token: 0x04002B72 RID: 11122
		private readonly DirectoryString typeOfSubstitution;

		// Token: 0x04002B73 RID: 11123
		private readonly GeneralName thirdPerson;

		// Token: 0x04002B74 RID: 11124
		private readonly IssuerSerial certRef;
	}
}
