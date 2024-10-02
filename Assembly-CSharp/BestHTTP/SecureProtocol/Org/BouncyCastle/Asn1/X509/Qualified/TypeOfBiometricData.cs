using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509.Qualified
{
	// Token: 0x020006D4 RID: 1748
	public class TypeOfBiometricData : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x0600405A RID: 16474 RVA: 0x0017E330 File Offset: 0x0017C530
		public static TypeOfBiometricData GetInstance(object obj)
		{
			if (obj == null || obj is TypeOfBiometricData)
			{
				return (TypeOfBiometricData)obj;
			}
			if (obj is DerInteger)
			{
				return new TypeOfBiometricData(DerInteger.GetInstance(obj).Value.IntValue);
			}
			if (obj is DerObjectIdentifier)
			{
				return new TypeOfBiometricData(DerObjectIdentifier.GetInstance(obj));
			}
			throw new ArgumentException("unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x0017E39B File Offset: 0x0017C59B
		public TypeOfBiometricData(int predefinedBiometricType)
		{
			if (predefinedBiometricType == 0 || predefinedBiometricType == 1)
			{
				this.obj = new DerInteger(predefinedBiometricType);
				return;
			}
			throw new ArgumentException("unknow PredefinedBiometricType : " + predefinedBiometricType);
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x0017E3CC File Offset: 0x0017C5CC
		public TypeOfBiometricData(DerObjectIdentifier biometricDataOid)
		{
			this.obj = biometricDataOid;
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x0600405D RID: 16477 RVA: 0x0017E3DB File Offset: 0x0017C5DB
		public bool IsPredefined
		{
			get
			{
				return this.obj is DerInteger;
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x0600405E RID: 16478 RVA: 0x0017E3EB File Offset: 0x0017C5EB
		public int PredefinedBiometricType
		{
			get
			{
				return ((DerInteger)this.obj).Value.IntValue;
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x0600405F RID: 16479 RVA: 0x0017E402 File Offset: 0x0017C602
		public DerObjectIdentifier BiometricDataOid
		{
			get
			{
				return (DerObjectIdentifier)this.obj;
			}
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x0017E40F File Offset: 0x0017C60F
		public override Asn1Object ToAsn1Object()
		{
			return this.obj.ToAsn1Object();
		}

		// Token: 0x040028F5 RID: 10485
		public const int Picture = 0;

		// Token: 0x040028F6 RID: 10486
		public const int HandwrittenSignature = 1;

		// Token: 0x040028F7 RID: 10487
		internal Asn1Encodable obj;
	}
}
