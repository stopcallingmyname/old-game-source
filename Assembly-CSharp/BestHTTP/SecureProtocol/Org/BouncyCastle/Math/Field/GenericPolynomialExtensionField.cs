using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field
{
	// Token: 0x02000315 RID: 789
	internal class GenericPolynomialExtensionField : IPolynomialExtensionField, IExtensionField, IFiniteField
	{
		// Token: 0x06001DEB RID: 7659 RVA: 0x000E1088 File Offset: 0x000DF288
		internal GenericPolynomialExtensionField(IFiniteField subfield, IPolynomial polynomial)
		{
			this.subfield = subfield;
			this.minimalPolynomial = polynomial;
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001DEC RID: 7660 RVA: 0x000E109E File Offset: 0x000DF29E
		public virtual BigInteger Characteristic
		{
			get
			{
				return this.subfield.Characteristic;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06001DED RID: 7661 RVA: 0x000E10AB File Offset: 0x000DF2AB
		public virtual int Dimension
		{
			get
			{
				return this.subfield.Dimension * this.minimalPolynomial.Degree;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001DEE RID: 7662 RVA: 0x000E10C4 File Offset: 0x000DF2C4
		public virtual IFiniteField Subfield
		{
			get
			{
				return this.subfield;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001DEF RID: 7663 RVA: 0x000E10CC File Offset: 0x000DF2CC
		public virtual int Degree
		{
			get
			{
				return this.minimalPolynomial.Degree;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001DF0 RID: 7664 RVA: 0x000E10D9 File Offset: 0x000DF2D9
		public virtual IPolynomial MinimalPolynomial
		{
			get
			{
				return this.minimalPolynomial;
			}
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x000E10E4 File Offset: 0x000DF2E4
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			GenericPolynomialExtensionField genericPolynomialExtensionField = obj as GenericPolynomialExtensionField;
			return genericPolynomialExtensionField != null && this.subfield.Equals(genericPolynomialExtensionField.subfield) && this.minimalPolynomial.Equals(genericPolynomialExtensionField.minimalPolynomial);
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x000E1129 File Offset: 0x000DF329
		public override int GetHashCode()
		{
			return this.subfield.GetHashCode() ^ Integers.RotateLeft(this.minimalPolynomial.GetHashCode(), 16);
		}

		// Token: 0x04001949 RID: 6473
		protected readonly IFiniteField subfield;

		// Token: 0x0400194A RID: 6474
		protected readonly IPolynomial minimalPolynomial;
	}
}
