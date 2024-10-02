using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X500;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.IsisMtt.X509
{
	// Token: 0x0200072C RID: 1836
	public class ProfessionInfo : Asn1Encodable
	{
		// Token: 0x060042A0 RID: 17056 RVA: 0x00186F98 File Offset: 0x00185198
		public static ProfessionInfo GetInstance(object obj)
		{
			if (obj == null || obj is ProfessionInfo)
			{
				return (ProfessionInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new ProfessionInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060042A1 RID: 17057 RVA: 0x00186FE8 File Offset: 0x001851E8
		private ProfessionInfo(Asn1Sequence seq)
		{
			if (seq.Count > 5)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			Asn1Encodable asn1Encodable = (Asn1Encodable)enumerator.Current;
			if (asn1Encodable is Asn1TaggedObject)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)asn1Encodable;
				if (asn1TaggedObject.TagNo != 0)
				{
					throw new ArgumentException("Bad tag number: " + asn1TaggedObject.TagNo);
				}
				this.namingAuthority = NamingAuthority.GetInstance(asn1TaggedObject, true);
				enumerator.MoveNext();
				asn1Encodable = (Asn1Encodable)enumerator.Current;
			}
			this.professionItems = Asn1Sequence.GetInstance(asn1Encodable);
			if (enumerator.MoveNext())
			{
				asn1Encodable = (Asn1Encodable)enumerator.Current;
				if (asn1Encodable is Asn1Sequence)
				{
					this.professionOids = Asn1Sequence.GetInstance(asn1Encodable);
				}
				else if (asn1Encodable is DerPrintableString)
				{
					this.registrationNumber = DerPrintableString.GetInstance(asn1Encodable).GetString();
				}
				else
				{
					if (!(asn1Encodable is Asn1OctetString))
					{
						throw new ArgumentException("Bad object encountered: " + Platform.GetTypeName(asn1Encodable));
					}
					this.addProfessionInfo = Asn1OctetString.GetInstance(asn1Encodable);
				}
			}
			if (enumerator.MoveNext())
			{
				asn1Encodable = (Asn1Encodable)enumerator.Current;
				if (asn1Encodable is DerPrintableString)
				{
					this.registrationNumber = DerPrintableString.GetInstance(asn1Encodable).GetString();
				}
				else
				{
					if (!(asn1Encodable is DerOctetString))
					{
						throw new ArgumentException("Bad object encountered: " + Platform.GetTypeName(asn1Encodable));
					}
					this.addProfessionInfo = (DerOctetString)asn1Encodable;
				}
			}
			if (!enumerator.MoveNext())
			{
				return;
			}
			asn1Encodable = (Asn1Encodable)enumerator.Current;
			if (asn1Encodable is DerOctetString)
			{
				this.addProfessionInfo = (DerOctetString)asn1Encodable;
				return;
			}
			throw new ArgumentException("Bad object encountered: " + Platform.GetTypeName(asn1Encodable));
		}

		// Token: 0x060042A2 RID: 17058 RVA: 0x001871A4 File Offset: 0x001853A4
		public ProfessionInfo(NamingAuthority namingAuthority, DirectoryString[] professionItems, DerObjectIdentifier[] professionOids, string registrationNumber, Asn1OctetString addProfessionInfo)
		{
			this.namingAuthority = namingAuthority;
			this.professionItems = new DerSequence(professionItems);
			if (professionOids != null)
			{
				this.professionOids = new DerSequence(professionOids);
			}
			this.registrationNumber = registrationNumber;
			this.addProfessionInfo = addProfessionInfo;
		}

		// Token: 0x060042A3 RID: 17059 RVA: 0x001871F0 File Offset: 0x001853F0
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.namingAuthority != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.namingAuthority)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.professionItems
			});
			if (this.professionOids != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.professionOids
				});
			}
			if (this.registrationNumber != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerPrintableString(this.registrationNumber, true)
				});
			}
			if (this.addProfessionInfo != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.addProfessionInfo
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x060042A4 RID: 17060 RVA: 0x001872A4 File Offset: 0x001854A4
		public virtual Asn1OctetString AddProfessionInfo
		{
			get
			{
				return this.addProfessionInfo;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x060042A5 RID: 17061 RVA: 0x001872AC File Offset: 0x001854AC
		public virtual NamingAuthority NamingAuthority
		{
			get
			{
				return this.namingAuthority;
			}
		}

		// Token: 0x060042A6 RID: 17062 RVA: 0x001872B4 File Offset: 0x001854B4
		public virtual DirectoryString[] GetProfessionItems()
		{
			DirectoryString[] array = new DirectoryString[this.professionItems.Count];
			for (int i = 0; i < this.professionItems.Count; i++)
			{
				array[i] = DirectoryString.GetInstance(this.professionItems[i]);
			}
			return array;
		}

		// Token: 0x060042A7 RID: 17063 RVA: 0x00187300 File Offset: 0x00185500
		public virtual DerObjectIdentifier[] GetProfessionOids()
		{
			if (this.professionOids == null)
			{
				return new DerObjectIdentifier[0];
			}
			DerObjectIdentifier[] array = new DerObjectIdentifier[this.professionOids.Count];
			for (int i = 0; i < this.professionOids.Count; i++)
			{
				array[i] = DerObjectIdentifier.GetInstance(this.professionOids[i]);
			}
			return array;
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x060042A8 RID: 17064 RVA: 0x00187358 File Offset: 0x00185558
		public virtual string RegistrationNumber
		{
			get
			{
				return this.registrationNumber;
			}
		}

		// Token: 0x04002B75 RID: 11125
		public static readonly DerObjectIdentifier Rechtsanwltin = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".1");

		// Token: 0x04002B76 RID: 11126
		public static readonly DerObjectIdentifier Rechtsanwalt = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".2");

		// Token: 0x04002B77 RID: 11127
		public static readonly DerObjectIdentifier Rechtsbeistand = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".3");

		// Token: 0x04002B78 RID: 11128
		public static readonly DerObjectIdentifier Steuerberaterin = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".4");

		// Token: 0x04002B79 RID: 11129
		public static readonly DerObjectIdentifier Steuerberater = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".5");

		// Token: 0x04002B7A RID: 11130
		public static readonly DerObjectIdentifier Steuerbevollmchtigte = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".6");

		// Token: 0x04002B7B RID: 11131
		public static readonly DerObjectIdentifier Steuerbevollmchtigter = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".7");

		// Token: 0x04002B7C RID: 11132
		public static readonly DerObjectIdentifier Notarin = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".8");

		// Token: 0x04002B7D RID: 11133
		public static readonly DerObjectIdentifier Notar = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".9");

		// Token: 0x04002B7E RID: 11134
		public static readonly DerObjectIdentifier Notarvertreterin = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".10");

		// Token: 0x04002B7F RID: 11135
		public static readonly DerObjectIdentifier Notarvertreter = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".11");

		// Token: 0x04002B80 RID: 11136
		public static readonly DerObjectIdentifier Notariatsverwalterin = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".12");

		// Token: 0x04002B81 RID: 11137
		public static readonly DerObjectIdentifier Notariatsverwalter = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".13");

		// Token: 0x04002B82 RID: 11138
		public static readonly DerObjectIdentifier Wirtschaftsprferin = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".14");

		// Token: 0x04002B83 RID: 11139
		public static readonly DerObjectIdentifier Wirtschaftsprfer = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".15");

		// Token: 0x04002B84 RID: 11140
		public static readonly DerObjectIdentifier VereidigteBuchprferin = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".16");

		// Token: 0x04002B85 RID: 11141
		public static readonly DerObjectIdentifier VereidigterBuchprfer = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".17");

		// Token: 0x04002B86 RID: 11142
		public static readonly DerObjectIdentifier Patentanwltin = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".18");

		// Token: 0x04002B87 RID: 11143
		public static readonly DerObjectIdentifier Patentanwalt = new DerObjectIdentifier(NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern + ".19");

		// Token: 0x04002B88 RID: 11144
		private readonly NamingAuthority namingAuthority;

		// Token: 0x04002B89 RID: 11145
		private readonly Asn1Sequence professionItems;

		// Token: 0x04002B8A RID: 11146
		private readonly Asn1Sequence professionOids;

		// Token: 0x04002B8B RID: 11147
		private readonly string registrationNumber;

		// Token: 0x04002B8C RID: 11148
		private readonly Asn1OctetString addProfessionInfo;
	}
}
