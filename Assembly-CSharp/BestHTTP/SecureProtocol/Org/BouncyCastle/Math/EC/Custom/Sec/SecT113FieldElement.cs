using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000382 RID: 898
	internal class SecT113FieldElement : AbstractF2mFieldElement
	{
		// Token: 0x06002363 RID: 9059 RVA: 0x000FB2B0 File Offset: 0x000F94B0
		public SecT113FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 113)
			{
				throw new ArgumentException("value invalid for SecT113FieldElement", "x");
			}
			this.x = SecT113Field.FromBigInteger(x);
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x000FB2EA File Offset: 0x000F94EA
		public SecT113FieldElement()
		{
			this.x = Nat128.Create64();
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x000FB2FD File Offset: 0x000F94FD
		protected internal SecT113FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06002366 RID: 9062 RVA: 0x000FB30C File Offset: 0x000F950C
		public override bool IsOne
		{
			get
			{
				return Nat128.IsOne64(this.x);
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06002367 RID: 9063 RVA: 0x000FB319 File Offset: 0x000F9519
		public override bool IsZero
		{
			get
			{
				return Nat128.IsZero64(this.x);
			}
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x000FB326 File Offset: 0x000F9526
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x000FB337 File Offset: 0x000F9537
		public override BigInteger ToBigInteger()
		{
			return Nat128.ToBigInteger64(this.x);
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x0600236A RID: 9066 RVA: 0x000FB344 File Offset: 0x000F9544
		public override string FieldName
		{
			get
			{
				return "SecT113Field";
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x0600236B RID: 9067 RVA: 0x000FB34B File Offset: 0x000F954B
		public override int FieldSize
		{
			get
			{
				return 113;
			}
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x000FB350 File Offset: 0x000F9550
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat128.Create64();
			SecT113Field.Add(this.x, ((SecT113FieldElement)b).x, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x000FB380 File Offset: 0x000F9580
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat128.Create64();
			SecT113Field.AddOne(this.x, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x000E3A75 File Offset: 0x000E1C75
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x000FB3A8 File Offset: 0x000F95A8
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat128.Create64();
			SecT113Field.Multiply(this.x, ((SecT113FieldElement)b).x, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x000E3AB3 File Offset: 0x000E1CB3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x000FB3D8 File Offset: 0x000F95D8
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT113FieldElement)b).x;
			ulong[] array2 = ((SecT113FieldElement)x).x;
			ulong[] y3 = ((SecT113FieldElement)y).x;
			ulong[] array3 = Nat128.CreateExt64();
			SecT113Field.MultiplyAddToExt(array, y2, array3);
			SecT113Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat128.Create64();
			SecT113Field.Reduce(array3, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x000FB43A File Offset: 0x000F963A
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x000947CE File Offset: 0x000929CE
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x000FB448 File Offset: 0x000F9648
		public override ECFieldElement Square()
		{
			ulong[] z = Nat128.Create64();
			SecT113Field.Square(this.x, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x000E3BA1 File Offset: 0x000E1DA1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x000FB470 File Offset: 0x000F9670
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT113FieldElement)x).x;
			ulong[] y2 = ((SecT113FieldElement)y).x;
			ulong[] array3 = Nat128.CreateExt64();
			SecT113Field.SquareAddToExt(array, array3);
			SecT113Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat128.Create64();
			SecT113Field.Reduce(array3, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x000FB4C4 File Offset: 0x000F96C4
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat128.Create64();
			SecT113Field.SquareN(this.x, pow, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x000FB4F0 File Offset: 0x000F96F0
		public override int Trace()
		{
			return (int)SecT113Field.Trace(this.x);
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x000FB500 File Offset: 0x000F9700
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat128.Create64();
			SecT113Field.Invert(this.x, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x000FB528 File Offset: 0x000F9728
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat128.Create64();
			SecT113Field.Sqrt(this.x, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x0600237B RID: 9083 RVA: 0x000A7398 File Offset: 0x000A5598
		public virtual int Representation
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x0600237C RID: 9084 RVA: 0x000FB34B File Offset: 0x000F954B
		public virtual int M
		{
			get
			{
				return 113;
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x0600237D RID: 9085 RVA: 0x000FB54D File Offset: 0x000F974D
		public virtual int K1
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x0600237E RID: 9086 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x0600237F RID: 9087 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x000FB551 File Offset: 0x000F9751
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT113FieldElement);
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x000FB551 File Offset: 0x000F9751
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT113FieldElement);
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x000FB55F File Offset: 0x000F975F
		public virtual bool Equals(SecT113FieldElement other)
		{
			return this == other || (other != null && Nat128.Eq64(this.x, other.x));
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x000FB57D File Offset: 0x000F977D
		public override int GetHashCode()
		{
			return 113009 ^ Arrays.GetHashCode(this.x, 0, 2);
		}

		// Token: 0x04001A7A RID: 6778
		protected internal readonly ulong[] x;
	}
}
