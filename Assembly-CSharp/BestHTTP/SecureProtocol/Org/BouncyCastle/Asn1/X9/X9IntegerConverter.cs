using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000682 RID: 1666
	public abstract class X9IntegerConverter
	{
		// Token: 0x06003DCA RID: 15818 RVA: 0x00174FDA File Offset: 0x001731DA
		public static int GetByteLength(ECFieldElement fe)
		{
			return (fe.FieldSize + 7) / 8;
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x00174FE6 File Offset: 0x001731E6
		public static int GetByteLength(ECCurve c)
		{
			return (c.FieldSize + 7) / 8;
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x00174FF4 File Offset: 0x001731F4
		public static byte[] IntegerToBytes(BigInteger s, int qLength)
		{
			byte[] array = s.ToByteArrayUnsigned();
			if (qLength < array.Length)
			{
				byte[] array2 = new byte[qLength];
				Array.Copy(array, array.Length - array2.Length, array2, 0, array2.Length);
				return array2;
			}
			if (qLength > array.Length)
			{
				byte[] array3 = new byte[qLength];
				Array.Copy(array, 0, array3, array3.Length - array.Length, array.Length);
				return array3;
			}
			return array;
		}
	}
}
