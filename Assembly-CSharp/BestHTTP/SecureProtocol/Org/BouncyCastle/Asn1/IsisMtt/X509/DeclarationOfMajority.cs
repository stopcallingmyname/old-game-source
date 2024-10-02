using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.IsisMtt.X509
{
	// Token: 0x02000728 RID: 1832
	public class DeclarationOfMajority : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x0600427D RID: 17021 RVA: 0x001867DD File Offset: 0x001849DD
		public DeclarationOfMajority(int notYoungerThan)
		{
			this.declaration = new DerTaggedObject(false, 0, new DerInteger(notYoungerThan));
		}

		// Token: 0x0600427E RID: 17022 RVA: 0x001867F8 File Offset: 0x001849F8
		public DeclarationOfMajority(bool fullAge, string country)
		{
			if (country.Length > 2)
			{
				throw new ArgumentException("country can only be 2 characters");
			}
			DerPrintableString derPrintableString = new DerPrintableString(country, true);
			DerSequence obj;
			if (fullAge)
			{
				obj = new DerSequence(derPrintableString);
			}
			else
			{
				obj = new DerSequence(new Asn1Encodable[]
				{
					DerBoolean.False,
					derPrintableString
				});
			}
			this.declaration = new DerTaggedObject(false, 1, obj);
		}

		// Token: 0x0600427F RID: 17023 RVA: 0x00186859 File Offset: 0x00184A59
		public DeclarationOfMajority(DerGeneralizedTime dateOfBirth)
		{
			this.declaration = new DerTaggedObject(false, 2, dateOfBirth);
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x00186870 File Offset: 0x00184A70
		public static DeclarationOfMajority GetInstance(object obj)
		{
			if (obj == null || obj is DeclarationOfMajority)
			{
				return (DeclarationOfMajority)obj;
			}
			if (obj is Asn1TaggedObject)
			{
				return new DeclarationOfMajority((Asn1TaggedObject)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004281 RID: 17025 RVA: 0x001868BD File Offset: 0x00184ABD
		private DeclarationOfMajority(Asn1TaggedObject o)
		{
			if (o.TagNo > 2)
			{
				throw new ArgumentException("Bad tag number: " + o.TagNo);
			}
			this.declaration = o;
		}

		// Token: 0x06004282 RID: 17026 RVA: 0x001868F0 File Offset: 0x00184AF0
		public override Asn1Object ToAsn1Object()
		{
			return this.declaration;
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06004283 RID: 17027 RVA: 0x001868F8 File Offset: 0x00184AF8
		public DeclarationOfMajority.Choice Type
		{
			get
			{
				return (DeclarationOfMajority.Choice)this.declaration.TagNo;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06004284 RID: 17028 RVA: 0x00186908 File Offset: 0x00184B08
		public virtual int NotYoungerThan
		{
			get
			{
				if (this.declaration.TagNo == 0)
				{
					return DerInteger.GetInstance(this.declaration, false).Value.IntValue;
				}
				return -1;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06004285 RID: 17029 RVA: 0x0018693C File Offset: 0x00184B3C
		public virtual Asn1Sequence FullAgeAtCountry
		{
			get
			{
				DeclarationOfMajority.Choice tagNo = (DeclarationOfMajority.Choice)this.declaration.TagNo;
				if (tagNo == DeclarationOfMajority.Choice.FullAgeAtCountry)
				{
					return Asn1Sequence.GetInstance(this.declaration, false);
				}
				return null;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06004286 RID: 17030 RVA: 0x00186968 File Offset: 0x00184B68
		public virtual DerGeneralizedTime DateOfBirth
		{
			get
			{
				DeclarationOfMajority.Choice tagNo = (DeclarationOfMajority.Choice)this.declaration.TagNo;
				if (tagNo == DeclarationOfMajority.Choice.DateOfBirth)
				{
					return DerGeneralizedTime.GetInstance(this.declaration, false);
				}
				return null;
			}
		}

		// Token: 0x04002B69 RID: 11113
		private readonly Asn1TaggedObject declaration;

		// Token: 0x020009D5 RID: 2517
		public enum Choice
		{
			// Token: 0x04003792 RID: 14226
			NotYoungerThan,
			// Token: 0x04003793 RID: 14227
			FullAgeAtCountry,
			// Token: 0x04003794 RID: 14228
			DateOfBirth
		}
	}
}
