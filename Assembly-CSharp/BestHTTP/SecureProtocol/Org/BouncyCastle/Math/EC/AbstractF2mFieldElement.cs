using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x02000325 RID: 805
	public abstract class AbstractF2mFieldElement : ECFieldElement
	{
		// Token: 0x06001EAA RID: 7850 RVA: 0x000E37C8 File Offset: 0x000E19C8
		public virtual ECFieldElement HalfTrace()
		{
			int fieldSize = this.FieldSize;
			if ((fieldSize & 1) == 0)
			{
				throw new InvalidOperationException("Half-trace only defined for odd m");
			}
			ECFieldElement ecfieldElement = this;
			ECFieldElement ecfieldElement2 = ecfieldElement;
			for (int i = 2; i < fieldSize; i += 2)
			{
				ecfieldElement = ecfieldElement.SquarePow(2);
				ecfieldElement2 = ecfieldElement2.Add(ecfieldElement);
			}
			return ecfieldElement2;
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x000E3810 File Offset: 0x000E1A10
		public virtual int Trace()
		{
			int fieldSize = this.FieldSize;
			ECFieldElement ecfieldElement = this;
			ECFieldElement ecfieldElement2 = ecfieldElement;
			for (int i = 1; i < fieldSize; i++)
			{
				ecfieldElement = ecfieldElement.Square();
				ecfieldElement2 = ecfieldElement2.Add(ecfieldElement);
			}
			if (ecfieldElement2.IsZero)
			{
				return 0;
			}
			if (ecfieldElement2.IsOne)
			{
				return 1;
			}
			throw new InvalidOperationException("Internal error in trace calculation");
		}
	}
}
