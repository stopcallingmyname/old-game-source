using System;
using System.IO;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000657 RID: 1623
	public class DerObjectIdentifier : Asn1Object
	{
		// Token: 0x06003CB2 RID: 15538 RVA: 0x00172148 File Offset: 0x00170348
		public static DerObjectIdentifier GetInstance(object obj)
		{
			if (obj == null || obj is DerObjectIdentifier)
			{
				return (DerObjectIdentifier)obj;
			}
			if (obj is byte[])
			{
				return DerObjectIdentifier.FromOctetString((byte[])obj);
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x00172198 File Offset: 0x00170398
		public static DerObjectIdentifier GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			Asn1Object @object = obj.GetObject();
			if (explicitly || @object is DerObjectIdentifier)
			{
				return DerObjectIdentifier.GetInstance(@object);
			}
			return DerObjectIdentifier.FromOctetString(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x001721CE File Offset: 0x001703CE
		public DerObjectIdentifier(string identifier)
		{
			if (identifier == null)
			{
				throw new ArgumentNullException("identifier");
			}
			if (!DerObjectIdentifier.IsValidIdentifier(identifier))
			{
				throw new FormatException("string " + identifier + " not an OID");
			}
			this.identifier = identifier;
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x0017220C File Offset: 0x0017040C
		internal DerObjectIdentifier(DerObjectIdentifier oid, string branchID)
		{
			if (!DerObjectIdentifier.IsValidBranchID(branchID, 0))
			{
				throw new ArgumentException("string " + branchID + " not a valid OID branch", "branchID");
			}
			this.identifier = oid.Id + "." + branchID;
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06003CB6 RID: 15542 RVA: 0x0017225A File Offset: 0x0017045A
		public string Id
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x00172262 File Offset: 0x00170462
		public virtual DerObjectIdentifier Branch(string branchID)
		{
			return new DerObjectIdentifier(this, branchID);
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x0017226C File Offset: 0x0017046C
		public virtual bool On(DerObjectIdentifier stem)
		{
			string id = this.Id;
			string id2 = stem.Id;
			return id.Length > id2.Length && id[id2.Length] == '.' && Platform.StartsWith(id, id2);
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x001722AE File Offset: 0x001704AE
		internal DerObjectIdentifier(byte[] bytes)
		{
			this.identifier = DerObjectIdentifier.MakeOidStringFromBytes(bytes);
			this.body = Arrays.Clone(bytes);
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x001722D0 File Offset: 0x001704D0
		private void WriteField(Stream outputStream, long fieldValue)
		{
			byte[] array = new byte[9];
			int num = 8;
			array[num] = (byte)(fieldValue & 127L);
			while (fieldValue >= 128L)
			{
				fieldValue >>= 7;
				array[--num] = (byte)((fieldValue & 127L) | 128L);
			}
			outputStream.Write(array, num, 9 - num);
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x00172320 File Offset: 0x00170520
		private void WriteField(Stream outputStream, BigInteger fieldValue)
		{
			int num = (fieldValue.BitLength + 6) / 7;
			if (num == 0)
			{
				outputStream.WriteByte(0);
				return;
			}
			BigInteger bigInteger = fieldValue;
			byte[] array = new byte[num];
			for (int i = num - 1; i >= 0; i--)
			{
				array[i] = (byte)((bigInteger.IntValue & 127) | 128);
				bigInteger = bigInteger.ShiftRight(7);
			}
			byte[] array2 = array;
			int num2 = num - 1;
			array2[num2] &= 127;
			outputStream.Write(array, 0, array.Length);
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x00172390 File Offset: 0x00170590
		private void DoOutput(MemoryStream bOut)
		{
			OidTokenizer oidTokenizer = new OidTokenizer(this.identifier);
			string text = oidTokenizer.NextToken();
			int num = int.Parse(text) * 40;
			text = oidTokenizer.NextToken();
			if (text.Length <= 18)
			{
				this.WriteField(bOut, (long)num + long.Parse(text));
			}
			else
			{
				this.WriteField(bOut, new BigInteger(text).Add(BigInteger.ValueOf((long)num)));
			}
			while (oidTokenizer.HasMoreTokens)
			{
				text = oidTokenizer.NextToken();
				if (text.Length <= 18)
				{
					this.WriteField(bOut, long.Parse(text));
				}
				else
				{
					this.WriteField(bOut, new BigInteger(text));
				}
			}
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x00172430 File Offset: 0x00170630
		internal byte[] GetBody()
		{
			lock (this)
			{
				if (this.body == null)
				{
					MemoryStream memoryStream = new MemoryStream();
					this.DoOutput(memoryStream);
					this.body = memoryStream.ToArray();
				}
			}
			return this.body;
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x0017248C File Offset: 0x0017068C
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(6, this.GetBody());
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x0017249B File Offset: 0x0017069B
		protected override int Asn1GetHashCode()
		{
			return this.identifier.GetHashCode();
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x001724A8 File Offset: 0x001706A8
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerObjectIdentifier derObjectIdentifier = asn1Object as DerObjectIdentifier;
			return derObjectIdentifier != null && this.identifier.Equals(derObjectIdentifier.identifier);
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x0017225A File Offset: 0x0017045A
		public override string ToString()
		{
			return this.identifier;
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x001724D4 File Offset: 0x001706D4
		private static bool IsValidBranchID(string branchID, int start)
		{
			bool flag = false;
			int num = branchID.Length;
			while (--num >= start)
			{
				char c = branchID[num];
				if ('0' <= c && c <= '9')
				{
					flag = true;
				}
				else
				{
					if (c != '.')
					{
						return false;
					}
					if (!flag)
					{
						return false;
					}
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x0017251C File Offset: 0x0017071C
		private static bool IsValidIdentifier(string identifier)
		{
			if (identifier.Length < 3 || identifier[1] != '.')
			{
				return false;
			}
			char c = identifier[0];
			return c >= '0' && c <= '2' && DerObjectIdentifier.IsValidBranchID(identifier, 2);
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x0017255C File Offset: 0x0017075C
		private static string MakeOidStringFromBytes(byte[] bytes)
		{
			StringBuilder stringBuilder = new StringBuilder();
			long num = 0L;
			BigInteger bigInteger = null;
			bool flag = true;
			for (int num2 = 0; num2 != bytes.Length; num2++)
			{
				int num3 = (int)bytes[num2];
				if (num <= 72057594037927808L)
				{
					num += (long)(num3 & 127);
					if ((num3 & 128) == 0)
					{
						if (flag)
						{
							if (num < 40L)
							{
								stringBuilder.Append('0');
							}
							else if (num < 80L)
							{
								stringBuilder.Append('1');
								num -= 40L;
							}
							else
							{
								stringBuilder.Append('2');
								num -= 80L;
							}
							flag = false;
						}
						stringBuilder.Append('.');
						stringBuilder.Append(num);
						num = 0L;
					}
					else
					{
						num <<= 7;
					}
				}
				else
				{
					if (bigInteger == null)
					{
						bigInteger = BigInteger.ValueOf(num);
					}
					bigInteger = bigInteger.Or(BigInteger.ValueOf((long)(num3 & 127)));
					if ((num3 & 128) == 0)
					{
						if (flag)
						{
							stringBuilder.Append('2');
							bigInteger = bigInteger.Subtract(BigInteger.ValueOf(80L));
							flag = false;
						}
						stringBuilder.Append('.');
						stringBuilder.Append(bigInteger);
						bigInteger = null;
						num = 0L;
					}
					else
					{
						bigInteger = bigInteger.ShiftLeft(7);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x00172674 File Offset: 0x00170874
		internal static DerObjectIdentifier FromOctetString(byte[] enc)
		{
			int num = Arrays.GetHashCode(enc) & 1023;
			DerObjectIdentifier[] obj = DerObjectIdentifier.cache;
			DerObjectIdentifier result;
			lock (obj)
			{
				DerObjectIdentifier derObjectIdentifier = DerObjectIdentifier.cache[num];
				if (derObjectIdentifier != null && Arrays.AreEqual(enc, derObjectIdentifier.GetBody()))
				{
					result = derObjectIdentifier;
				}
				else
				{
					result = (DerObjectIdentifier.cache[num] = new DerObjectIdentifier(enc));
				}
			}
			return result;
		}

		// Token: 0x040026F4 RID: 9972
		private readonly string identifier;

		// Token: 0x040026F5 RID: 9973
		private byte[] body;

		// Token: 0x040026F6 RID: 9974
		private const long LONG_LIMIT = 72057594037927808L;

		// Token: 0x040026F7 RID: 9975
		private static readonly DerObjectIdentifier[] cache = new DerObjectIdentifier[1024];
	}
}
