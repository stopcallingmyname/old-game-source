using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200064B RID: 1611
	public class DerBoolean : Asn1Object
	{
		// Token: 0x06003C38 RID: 15416 RVA: 0x00170E61 File Offset: 0x0016F061
		public static DerBoolean GetInstance(object obj)
		{
			if (obj == null || obj is DerBoolean)
			{
				return (DerBoolean)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003C39 RID: 15417 RVA: 0x00170E8A File Offset: 0x0016F08A
		public static DerBoolean GetInstance(bool value)
		{
			if (!value)
			{
				return DerBoolean.False;
			}
			return DerBoolean.True;
		}

		// Token: 0x06003C3A RID: 15418 RVA: 0x00170E9C File Offset: 0x0016F09C
		public static DerBoolean GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerBoolean)
			{
				return DerBoolean.GetInstance(@object);
			}
			return DerBoolean.FromOctetString(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06003C3B RID: 15419 RVA: 0x00170ED2 File Offset: 0x0016F0D2
		public DerBoolean(byte[] val)
		{
			if (val.Length != 1)
			{
				throw new ArgumentException("byte value should have 1 byte in it", "val");
			}
			this.value = val[0];
		}

		// Token: 0x06003C3C RID: 15420 RVA: 0x00170EF9 File Offset: 0x0016F0F9
		private DerBoolean(bool value)
		{
			this.value = (value ? byte.MaxValue : 0);
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x00170F12 File Offset: 0x0016F112
		public bool IsTrue
		{
			get
			{
				return this.value > 0;
			}
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x00170F1D File Offset: 0x0016F11D
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(1, new byte[]
			{
				this.value
			});
		}

		// Token: 0x06003C3F RID: 15423 RVA: 0x00170F38 File Offset: 0x0016F138
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerBoolean derBoolean = asn1Object as DerBoolean;
			return derBoolean != null && this.IsTrue == derBoolean.IsTrue;
		}

		// Token: 0x06003C40 RID: 15424 RVA: 0x00170F60 File Offset: 0x0016F160
		protected override int Asn1GetHashCode()
		{
			return this.IsTrue.GetHashCode();
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x00170F7B File Offset: 0x0016F17B
		public override string ToString()
		{
			if (!this.IsTrue)
			{
				return "FALSE";
			}
			return "TRUE";
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x00170F90 File Offset: 0x0016F190
		internal static DerBoolean FromOctetString(byte[] value)
		{
			if (value.Length != 1)
			{
				throw new ArgumentException("BOOLEAN value should have 1 byte in it", "value");
			}
			byte b = value[0];
			if (b == 0)
			{
				return DerBoolean.False;
			}
			if (b != 255)
			{
				return new DerBoolean(value);
			}
			return DerBoolean.True;
		}

		// Token: 0x040026DD RID: 9949
		private readonly byte value;

		// Token: 0x040026DE RID: 9950
		public static readonly DerBoolean False = new DerBoolean(false);

		// Token: 0x040026DF RID: 9951
		public static readonly DerBoolean True = new DerBoolean(true);
	}
}
