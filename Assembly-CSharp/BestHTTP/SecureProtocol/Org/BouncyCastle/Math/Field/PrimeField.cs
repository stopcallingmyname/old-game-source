using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field
{
	// Token: 0x0200031B RID: 795
	internal class PrimeField : IFiniteField
	{
		// Token: 0x06001DFF RID: 7679 RVA: 0x000E11BD File Offset: 0x000DF3BD
		internal PrimeField(BigInteger characteristic)
		{
			this.characteristic = characteristic;
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06001E00 RID: 7680 RVA: 0x000E11CC File Offset: 0x000DF3CC
		public virtual BigInteger Characteristic
		{
			get
			{
				return this.characteristic;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06001E01 RID: 7681 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual int Dimension
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x000E11D4 File Offset: 0x000DF3D4
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			PrimeField primeField = obj as PrimeField;
			return primeField != null && this.characteristic.Equals(primeField.characteristic);
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x000E1204 File Offset: 0x000DF404
		public override int GetHashCode()
		{
			return this.characteristic.GetHashCode();
		}

		// Token: 0x0400194C RID: 6476
		protected readonly BigInteger characteristic;
	}
}
