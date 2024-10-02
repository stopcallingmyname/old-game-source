using System;
using System.Collections;
using System.IO;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006C6 RID: 1734
	public class X509Name : Asn1Encodable
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06003FF2 RID: 16370 RVA: 0x0017BE4F File Offset: 0x0017A04F
		// (set) Token: 0x06003FF3 RID: 16371 RVA: 0x0017BE58 File Offset: 0x0017A058
		public static bool DefaultReverse
		{
			get
			{
				return X509Name.defaultReverse[0];
			}
			set
			{
				X509Name.defaultReverse[0] = value;
			}
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x0017BE64 File Offset: 0x0017A064
		static X509Name()
		{
			X509Name.DefaultSymbols.Add(X509Name.C, "C");
			X509Name.DefaultSymbols.Add(X509Name.O, "O");
			X509Name.DefaultSymbols.Add(X509Name.T, "T");
			X509Name.DefaultSymbols.Add(X509Name.OU, "OU");
			X509Name.DefaultSymbols.Add(X509Name.CN, "CN");
			X509Name.DefaultSymbols.Add(X509Name.L, "L");
			X509Name.DefaultSymbols.Add(X509Name.ST, "ST");
			X509Name.DefaultSymbols.Add(X509Name.SerialNumber, "SERIALNUMBER");
			X509Name.DefaultSymbols.Add(X509Name.EmailAddress, "E");
			X509Name.DefaultSymbols.Add(X509Name.DC, "DC");
			X509Name.DefaultSymbols.Add(X509Name.UID, "UID");
			X509Name.DefaultSymbols.Add(X509Name.Street, "STREET");
			X509Name.DefaultSymbols.Add(X509Name.Surname, "SURNAME");
			X509Name.DefaultSymbols.Add(X509Name.GivenName, "GIVENNAME");
			X509Name.DefaultSymbols.Add(X509Name.Initials, "INITIALS");
			X509Name.DefaultSymbols.Add(X509Name.Generation, "GENERATION");
			X509Name.DefaultSymbols.Add(X509Name.UnstructuredAddress, "unstructuredAddress");
			X509Name.DefaultSymbols.Add(X509Name.UnstructuredName, "unstructuredName");
			X509Name.DefaultSymbols.Add(X509Name.UniqueIdentifier, "UniqueIdentifier");
			X509Name.DefaultSymbols.Add(X509Name.DnQualifier, "DN");
			X509Name.DefaultSymbols.Add(X509Name.Pseudonym, "Pseudonym");
			X509Name.DefaultSymbols.Add(X509Name.PostalAddress, "PostalAddress");
			X509Name.DefaultSymbols.Add(X509Name.NameAtBirth, "NameAtBirth");
			X509Name.DefaultSymbols.Add(X509Name.CountryOfCitizenship, "CountryOfCitizenship");
			X509Name.DefaultSymbols.Add(X509Name.CountryOfResidence, "CountryOfResidence");
			X509Name.DefaultSymbols.Add(X509Name.Gender, "Gender");
			X509Name.DefaultSymbols.Add(X509Name.PlaceOfBirth, "PlaceOfBirth");
			X509Name.DefaultSymbols.Add(X509Name.DateOfBirth, "DateOfBirth");
			X509Name.DefaultSymbols.Add(X509Name.PostalCode, "PostalCode");
			X509Name.DefaultSymbols.Add(X509Name.BusinessCategory, "BusinessCategory");
			X509Name.DefaultSymbols.Add(X509Name.TelephoneNumber, "TelephoneNumber");
			X509Name.RFC2253Symbols.Add(X509Name.C, "C");
			X509Name.RFC2253Symbols.Add(X509Name.O, "O");
			X509Name.RFC2253Symbols.Add(X509Name.OU, "OU");
			X509Name.RFC2253Symbols.Add(X509Name.CN, "CN");
			X509Name.RFC2253Symbols.Add(X509Name.L, "L");
			X509Name.RFC2253Symbols.Add(X509Name.ST, "ST");
			X509Name.RFC2253Symbols.Add(X509Name.Street, "STREET");
			X509Name.RFC2253Symbols.Add(X509Name.DC, "DC");
			X509Name.RFC2253Symbols.Add(X509Name.UID, "UID");
			X509Name.RFC1779Symbols.Add(X509Name.C, "C");
			X509Name.RFC1779Symbols.Add(X509Name.O, "O");
			X509Name.RFC1779Symbols.Add(X509Name.OU, "OU");
			X509Name.RFC1779Symbols.Add(X509Name.CN, "CN");
			X509Name.RFC1779Symbols.Add(X509Name.L, "L");
			X509Name.RFC1779Symbols.Add(X509Name.ST, "ST");
			X509Name.RFC1779Symbols.Add(X509Name.Street, "STREET");
			X509Name.DefaultLookup.Add("c", X509Name.C);
			X509Name.DefaultLookup.Add("o", X509Name.O);
			X509Name.DefaultLookup.Add("t", X509Name.T);
			X509Name.DefaultLookup.Add("ou", X509Name.OU);
			X509Name.DefaultLookup.Add("cn", X509Name.CN);
			X509Name.DefaultLookup.Add("l", X509Name.L);
			X509Name.DefaultLookup.Add("st", X509Name.ST);
			X509Name.DefaultLookup.Add("serialnumber", X509Name.SerialNumber);
			X509Name.DefaultLookup.Add("street", X509Name.Street);
			X509Name.DefaultLookup.Add("emailaddress", X509Name.E);
			X509Name.DefaultLookup.Add("dc", X509Name.DC);
			X509Name.DefaultLookup.Add("e", X509Name.E);
			X509Name.DefaultLookup.Add("uid", X509Name.UID);
			X509Name.DefaultLookup.Add("surname", X509Name.Surname);
			X509Name.DefaultLookup.Add("givenname", X509Name.GivenName);
			X509Name.DefaultLookup.Add("initials", X509Name.Initials);
			X509Name.DefaultLookup.Add("generation", X509Name.Generation);
			X509Name.DefaultLookup.Add("unstructuredaddress", X509Name.UnstructuredAddress);
			X509Name.DefaultLookup.Add("unstructuredname", X509Name.UnstructuredName);
			X509Name.DefaultLookup.Add("uniqueidentifier", X509Name.UniqueIdentifier);
			X509Name.DefaultLookup.Add("dn", X509Name.DnQualifier);
			X509Name.DefaultLookup.Add("pseudonym", X509Name.Pseudonym);
			X509Name.DefaultLookup.Add("postaladdress", X509Name.PostalAddress);
			X509Name.DefaultLookup.Add("nameofbirth", X509Name.NameAtBirth);
			X509Name.DefaultLookup.Add("countryofcitizenship", X509Name.CountryOfCitizenship);
			X509Name.DefaultLookup.Add("countryofresidence", X509Name.CountryOfResidence);
			X509Name.DefaultLookup.Add("gender", X509Name.Gender);
			X509Name.DefaultLookup.Add("placeofbirth", X509Name.PlaceOfBirth);
			X509Name.DefaultLookup.Add("dateofbirth", X509Name.DateOfBirth);
			X509Name.DefaultLookup.Add("postalcode", X509Name.PostalCode);
			X509Name.DefaultLookup.Add("businesscategory", X509Name.BusinessCategory);
			X509Name.DefaultLookup.Add("telephonenumber", X509Name.TelephoneNumber);
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x0017C6BA File Offset: 0x0017A8BA
		public static X509Name GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return X509Name.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x0017C6C8 File Offset: 0x0017A8C8
		public static X509Name GetInstance(object obj)
		{
			if (obj == null || obj is X509Name)
			{
				return (X509Name)obj;
			}
			if (obj != null)
			{
				return new X509Name(Asn1Sequence.GetInstance(obj));
			}
			throw new ArgumentException("null object in factory", "obj");
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x0017C6FA File Offset: 0x0017A8FA
		protected X509Name()
		{
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x0017C724 File Offset: 0x0017A924
		protected X509Name(Asn1Sequence seq)
		{
			this.seq = seq;
			foreach (object obj in seq)
			{
				Asn1Set instance = Asn1Set.GetInstance(((Asn1Encodable)obj).ToAsn1Object());
				for (int i = 0; i < instance.Count; i++)
				{
					Asn1Sequence instance2 = Asn1Sequence.GetInstance(instance[i].ToAsn1Object());
					if (instance2.Count != 2)
					{
						throw new ArgumentException("badly sized pair");
					}
					this.ordering.Add(DerObjectIdentifier.GetInstance(instance2[0].ToAsn1Object()));
					Asn1Object asn1Object = instance2[1].ToAsn1Object();
					if (asn1Object is IAsn1String && !(asn1Object is DerUniversalString))
					{
						string text = ((IAsn1String)asn1Object).GetString();
						if (Platform.StartsWith(text, "#"))
						{
							text = "\\" + text;
						}
						this.values.Add(text);
					}
					else
					{
						this.values.Add("#" + Hex.ToHexString(asn1Object.GetEncoded()));
					}
					this.added.Add(i != 0);
				}
			}
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x0017C8AC File Offset: 0x0017AAAC
		public X509Name(IList ordering, IDictionary attributes) : this(ordering, attributes, new X509DefaultEntryConverter())
		{
		}

		// Token: 0x06003FFA RID: 16378 RVA: 0x0017C8BC File Offset: 0x0017AABC
		public X509Name(IList ordering, IDictionary attributes, X509NameEntryConverter converter)
		{
			this.converter = converter;
			foreach (object obj in ordering)
			{
				DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)obj;
				object obj2 = attributes[derObjectIdentifier];
				if (obj2 == null)
				{
					throw new ArgumentException("No attribute for object id - " + derObjectIdentifier + " - passed to distinguished name");
				}
				this.ordering.Add(derObjectIdentifier);
				this.added.Add(false);
				this.values.Add(obj2);
			}
		}

		// Token: 0x06003FFB RID: 16379 RVA: 0x0017C984 File Offset: 0x0017AB84
		public X509Name(IList oids, IList values) : this(oids, values, new X509DefaultEntryConverter())
		{
		}

		// Token: 0x06003FFC RID: 16380 RVA: 0x0017C994 File Offset: 0x0017AB94
		public X509Name(IList oids, IList values, X509NameEntryConverter converter)
		{
			this.converter = converter;
			if (oids.Count != values.Count)
			{
				throw new ArgumentException("'oids' must be same length as 'values'.");
			}
			for (int i = 0; i < oids.Count; i++)
			{
				this.ordering.Add(oids[i]);
				this.values.Add(values[i]);
				this.added.Add(false);
			}
		}

		// Token: 0x06003FFD RID: 16381 RVA: 0x0017CA31 File Offset: 0x0017AC31
		public X509Name(string dirName) : this(X509Name.DefaultReverse, X509Name.DefaultLookup, dirName)
		{
		}

		// Token: 0x06003FFE RID: 16382 RVA: 0x0017CA44 File Offset: 0x0017AC44
		public X509Name(string dirName, X509NameEntryConverter converter) : this(X509Name.DefaultReverse, X509Name.DefaultLookup, dirName, converter)
		{
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x0017CA58 File Offset: 0x0017AC58
		public X509Name(bool reverse, string dirName) : this(reverse, X509Name.DefaultLookup, dirName)
		{
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x0017CA67 File Offset: 0x0017AC67
		public X509Name(bool reverse, string dirName, X509NameEntryConverter converter) : this(reverse, X509Name.DefaultLookup, dirName, converter)
		{
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x0017CA77 File Offset: 0x0017AC77
		public X509Name(bool reverse, IDictionary lookUp, string dirName) : this(reverse, lookUp, dirName, new X509DefaultEntryConverter())
		{
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x0017CA88 File Offset: 0x0017AC88
		private DerObjectIdentifier DecodeOid(string name, IDictionary lookUp)
		{
			if (Platform.StartsWith(Platform.ToUpperInvariant(name), "OID."))
			{
				return new DerObjectIdentifier(name.Substring(4));
			}
			if (name[0] >= '0' && name[0] <= '9')
			{
				return new DerObjectIdentifier(name);
			}
			DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)lookUp[Platform.ToLowerInvariant(name)];
			if (derObjectIdentifier == null)
			{
				throw new ArgumentException("Unknown object id - " + name + " - passed to distinguished name");
			}
			return derObjectIdentifier;
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x0017CAFC File Offset: 0x0017ACFC
		public X509Name(bool reverse, IDictionary lookUp, string dirName, X509NameEntryConverter converter)
		{
			this.converter = converter;
			X509NameTokenizer x509NameTokenizer = new X509NameTokenizer(dirName);
			while (x509NameTokenizer.HasMoreTokens())
			{
				string text = x509NameTokenizer.NextToken();
				int num = text.IndexOf('=');
				if (num == -1)
				{
					throw new ArgumentException("badly formated directory string");
				}
				string name = text.Substring(0, num);
				string text2 = text.Substring(num + 1);
				DerObjectIdentifier value = this.DecodeOid(name, lookUp);
				if (text2.IndexOf('+') > 0)
				{
					X509NameTokenizer x509NameTokenizer2 = new X509NameTokenizer(text2, '+');
					string value2 = x509NameTokenizer2.NextToken();
					this.ordering.Add(value);
					this.values.Add(value2);
					this.added.Add(false);
					while (x509NameTokenizer2.HasMoreTokens())
					{
						string text3 = x509NameTokenizer2.NextToken();
						int num2 = text3.IndexOf('=');
						string name2 = text3.Substring(0, num2);
						string value3 = text3.Substring(num2 + 1);
						this.ordering.Add(this.DecodeOid(name2, lookUp));
						this.values.Add(value3);
						this.added.Add(true);
					}
				}
				else
				{
					this.ordering.Add(value);
					this.values.Add(text2);
					this.added.Add(false);
				}
			}
			if (reverse)
			{
				IList list = Platform.CreateArrayList();
				IList list2 = Platform.CreateArrayList();
				IList list3 = Platform.CreateArrayList();
				int num3 = 1;
				for (int i = 0; i < this.ordering.Count; i++)
				{
					if (!(bool)this.added[i])
					{
						num3 = 0;
					}
					int index = num3++;
					list.Insert(index, this.ordering[i]);
					list2.Insert(index, this.values[i]);
					list3.Insert(index, this.added[i]);
				}
				this.ordering = list;
				this.values = list2;
				this.added = list3;
			}
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x0017CD20 File Offset: 0x0017AF20
		public IList GetOidList()
		{
			return Platform.CreateArrayList(this.ordering);
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x0017CD2D File Offset: 0x0017AF2D
		public IList GetValueList()
		{
			return this.GetValueList(null);
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x0017CD38 File Offset: 0x0017AF38
		public IList GetValueList(DerObjectIdentifier oid)
		{
			IList list = Platform.CreateArrayList();
			for (int num = 0; num != this.values.Count; num++)
			{
				if (oid == null || oid.Equals(this.ordering[num]))
				{
					string text = (string)this.values[num];
					if (Platform.StartsWith(text, "\\#"))
					{
						text = text.Substring(1);
					}
					list.Add(text);
				}
			}
			return list;
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x0017CDA8 File Offset: 0x0017AFA8
		public override Asn1Object ToAsn1Object()
		{
			if (this.seq == null)
			{
				Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
				Asn1EncodableVector asn1EncodableVector2 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
				DerObjectIdentifier derObjectIdentifier = null;
				for (int num = 0; num != this.ordering.Count; num++)
				{
					DerObjectIdentifier derObjectIdentifier2 = (DerObjectIdentifier)this.ordering[num];
					string value = (string)this.values[num];
					if (derObjectIdentifier != null && !(bool)this.added[num])
					{
						asn1EncodableVector.Add(new Asn1Encodable[]
						{
							new DerSet(asn1EncodableVector2)
						});
						asn1EncodableVector2 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
					}
					asn1EncodableVector2.Add(new Asn1Encodable[]
					{
						new DerSequence(new Asn1Encodable[]
						{
							derObjectIdentifier2,
							this.converter.GetConvertedValue(derObjectIdentifier2, value)
						})
					});
					derObjectIdentifier = derObjectIdentifier2;
				}
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerSet(asn1EncodableVector2)
				});
				this.seq = new DerSequence(asn1EncodableVector);
			}
			return this.seq;
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x0017CEAC File Offset: 0x0017B0AC
		public bool Equivalent(X509Name other, bool inOrder)
		{
			if (!inOrder)
			{
				return this.Equivalent(other);
			}
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			int count = this.ordering.Count;
			if (count != other.ordering.Count)
			{
				return false;
			}
			for (int i = 0; i < count; i++)
			{
				object obj = (DerObjectIdentifier)this.ordering[i];
				DerObjectIdentifier obj2 = (DerObjectIdentifier)other.ordering[i];
				if (!obj.Equals(obj2))
				{
					return false;
				}
				string s = (string)this.values[i];
				string s2 = (string)other.values[i];
				if (!X509Name.equivalentStrings(s, s2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x0017CF54 File Offset: 0x0017B154
		public bool Equivalent(X509Name other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			int count = this.ordering.Count;
			if (count != other.ordering.Count)
			{
				return false;
			}
			bool[] array = new bool[count];
			int num;
			int num2;
			int num3;
			if (this.ordering[0].Equals(other.ordering[0]))
			{
				num = 0;
				num2 = count;
				num3 = 1;
			}
			else
			{
				num = count - 1;
				num2 = -1;
				num3 = -1;
			}
			for (int num4 = num; num4 != num2; num4 += num3)
			{
				bool flag = false;
				DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)this.ordering[num4];
				string s = (string)this.values[num4];
				for (int i = 0; i < count; i++)
				{
					if (!array[i])
					{
						DerObjectIdentifier obj = (DerObjectIdentifier)other.ordering[i];
						if (derObjectIdentifier.Equals(obj))
						{
							string s2 = (string)other.values[i];
							if (X509Name.equivalentStrings(s, s2))
							{
								array[i] = true;
								flag = true;
								break;
							}
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x0017D068 File Offset: 0x0017B268
		private static bool equivalentStrings(string s1, string s2)
		{
			string text = X509Name.canonicalize(s1);
			string text2 = X509Name.canonicalize(s2);
			if (!text.Equals(text2))
			{
				text = X509Name.stripInternalSpaces(text);
				text2 = X509Name.stripInternalSpaces(text2);
				if (!text.Equals(text2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x0017D0A8 File Offset: 0x0017B2A8
		private static string canonicalize(string s)
		{
			string text = Platform.ToLowerInvariant(s).Trim();
			if (Platform.StartsWith(text, "#"))
			{
				Asn1Object asn1Object = X509Name.decodeObject(text);
				if (asn1Object is IAsn1String)
				{
					text = Platform.ToLowerInvariant(((IAsn1String)asn1Object).GetString()).Trim();
				}
			}
			return text;
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x0017D0F4 File Offset: 0x0017B2F4
		private static Asn1Object decodeObject(string v)
		{
			Asn1Object result;
			try
			{
				result = Asn1Object.FromByteArray(Hex.Decode(v.Substring(1)));
			}
			catch (IOException ex)
			{
				throw new InvalidOperationException("unknown encoding in name: " + ex.Message, ex);
			}
			return result;
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x0017D140 File Offset: 0x0017B340
		private static string stripInternalSpaces(string str)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (str.Length != 0)
			{
				char c = str[0];
				stringBuilder.Append(c);
				for (int i = 1; i < str.Length; i++)
				{
					char c2 = str[i];
					if (c != ' ' || c2 != ' ')
					{
						stringBuilder.Append(c2);
					}
					c = c2;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x0017D1A0 File Offset: 0x0017B3A0
		private void AppendValue(StringBuilder buf, IDictionary oidSymbols, DerObjectIdentifier oid, string val)
		{
			string text = (string)oidSymbols[oid];
			if (text != null)
			{
				buf.Append(text);
			}
			else
			{
				buf.Append(oid.Id);
			}
			buf.Append('=');
			int num = buf.Length;
			buf.Append(val);
			int num2 = buf.Length;
			if (Platform.StartsWith(val, "\\#"))
			{
				num += 2;
			}
			while (num != num2)
			{
				if (buf[num] == ',' || buf[num] == '"' || buf[num] == '\\' || buf[num] == '+' || buf[num] == '=' || buf[num] == '<' || buf[num] == '>' || buf[num] == ';')
				{
					buf.Insert(num++, "\\");
					num2++;
				}
				num++;
			}
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x0017D280 File Offset: 0x0017B480
		public string ToString(bool reverse, IDictionary oidSymbols)
		{
			ArrayList arrayList = new ArrayList();
			StringBuilder stringBuilder = null;
			for (int i = 0; i < this.ordering.Count; i++)
			{
				if ((bool)this.added[i])
				{
					stringBuilder.Append('+');
					this.AppendValue(stringBuilder, oidSymbols, (DerObjectIdentifier)this.ordering[i], (string)this.values[i]);
				}
				else
				{
					stringBuilder = new StringBuilder();
					this.AppendValue(stringBuilder, oidSymbols, (DerObjectIdentifier)this.ordering[i], (string)this.values[i]);
					arrayList.Add(stringBuilder);
				}
			}
			if (reverse)
			{
				arrayList.Reverse();
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			if (arrayList.Count > 0)
			{
				stringBuilder2.Append(arrayList[0].ToString());
				for (int j = 1; j < arrayList.Count; j++)
				{
					stringBuilder2.Append(',');
					stringBuilder2.Append(arrayList[j].ToString());
				}
			}
			return stringBuilder2.ToString();
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x0017D394 File Offset: 0x0017B594
		public override string ToString()
		{
			return this.ToString(X509Name.DefaultReverse, X509Name.DefaultSymbols);
		}

		// Token: 0x04002888 RID: 10376
		public static readonly DerObjectIdentifier C = new DerObjectIdentifier("2.5.4.6");

		// Token: 0x04002889 RID: 10377
		public static readonly DerObjectIdentifier O = new DerObjectIdentifier("2.5.4.10");

		// Token: 0x0400288A RID: 10378
		public static readonly DerObjectIdentifier OU = new DerObjectIdentifier("2.5.4.11");

		// Token: 0x0400288B RID: 10379
		public static readonly DerObjectIdentifier T = new DerObjectIdentifier("2.5.4.12");

		// Token: 0x0400288C RID: 10380
		public static readonly DerObjectIdentifier CN = new DerObjectIdentifier("2.5.4.3");

		// Token: 0x0400288D RID: 10381
		public static readonly DerObjectIdentifier Street = new DerObjectIdentifier("2.5.4.9");

		// Token: 0x0400288E RID: 10382
		public static readonly DerObjectIdentifier SerialNumber = new DerObjectIdentifier("2.5.4.5");

		// Token: 0x0400288F RID: 10383
		public static readonly DerObjectIdentifier L = new DerObjectIdentifier("2.5.4.7");

		// Token: 0x04002890 RID: 10384
		public static readonly DerObjectIdentifier ST = new DerObjectIdentifier("2.5.4.8");

		// Token: 0x04002891 RID: 10385
		public static readonly DerObjectIdentifier Surname = new DerObjectIdentifier("2.5.4.4");

		// Token: 0x04002892 RID: 10386
		public static readonly DerObjectIdentifier GivenName = new DerObjectIdentifier("2.5.4.42");

		// Token: 0x04002893 RID: 10387
		public static readonly DerObjectIdentifier Initials = new DerObjectIdentifier("2.5.4.43");

		// Token: 0x04002894 RID: 10388
		public static readonly DerObjectIdentifier Generation = new DerObjectIdentifier("2.5.4.44");

		// Token: 0x04002895 RID: 10389
		public static readonly DerObjectIdentifier UniqueIdentifier = new DerObjectIdentifier("2.5.4.45");

		// Token: 0x04002896 RID: 10390
		public static readonly DerObjectIdentifier BusinessCategory = new DerObjectIdentifier("2.5.4.15");

		// Token: 0x04002897 RID: 10391
		public static readonly DerObjectIdentifier PostalCode = new DerObjectIdentifier("2.5.4.17");

		// Token: 0x04002898 RID: 10392
		public static readonly DerObjectIdentifier DnQualifier = new DerObjectIdentifier("2.5.4.46");

		// Token: 0x04002899 RID: 10393
		public static readonly DerObjectIdentifier Pseudonym = new DerObjectIdentifier("2.5.4.65");

		// Token: 0x0400289A RID: 10394
		public static readonly DerObjectIdentifier DateOfBirth = new DerObjectIdentifier("1.3.6.1.5.5.7.9.1");

		// Token: 0x0400289B RID: 10395
		public static readonly DerObjectIdentifier PlaceOfBirth = new DerObjectIdentifier("1.3.6.1.5.5.7.9.2");

		// Token: 0x0400289C RID: 10396
		public static readonly DerObjectIdentifier Gender = new DerObjectIdentifier("1.3.6.1.5.5.7.9.3");

		// Token: 0x0400289D RID: 10397
		public static readonly DerObjectIdentifier CountryOfCitizenship = new DerObjectIdentifier("1.3.6.1.5.5.7.9.4");

		// Token: 0x0400289E RID: 10398
		public static readonly DerObjectIdentifier CountryOfResidence = new DerObjectIdentifier("1.3.6.1.5.5.7.9.5");

		// Token: 0x0400289F RID: 10399
		public static readonly DerObjectIdentifier NameAtBirth = new DerObjectIdentifier("1.3.36.8.3.14");

		// Token: 0x040028A0 RID: 10400
		public static readonly DerObjectIdentifier PostalAddress = new DerObjectIdentifier("2.5.4.16");

		// Token: 0x040028A1 RID: 10401
		public static readonly DerObjectIdentifier DmdName = new DerObjectIdentifier("2.5.4.54");

		// Token: 0x040028A2 RID: 10402
		public static readonly DerObjectIdentifier TelephoneNumber = X509ObjectIdentifiers.id_at_telephoneNumber;

		// Token: 0x040028A3 RID: 10403
		public static readonly DerObjectIdentifier OrganizationIdentifier = X509ObjectIdentifiers.id_at_organizationIdentifier;

		// Token: 0x040028A4 RID: 10404
		public static readonly DerObjectIdentifier Name = X509ObjectIdentifiers.id_at_name;

		// Token: 0x040028A5 RID: 10405
		public static readonly DerObjectIdentifier EmailAddress = PkcsObjectIdentifiers.Pkcs9AtEmailAddress;

		// Token: 0x040028A6 RID: 10406
		public static readonly DerObjectIdentifier UnstructuredName = PkcsObjectIdentifiers.Pkcs9AtUnstructuredName;

		// Token: 0x040028A7 RID: 10407
		public static readonly DerObjectIdentifier UnstructuredAddress = PkcsObjectIdentifiers.Pkcs9AtUnstructuredAddress;

		// Token: 0x040028A8 RID: 10408
		public static readonly DerObjectIdentifier E = X509Name.EmailAddress;

		// Token: 0x040028A9 RID: 10409
		public static readonly DerObjectIdentifier DC = new DerObjectIdentifier("0.9.2342.19200300.100.1.25");

		// Token: 0x040028AA RID: 10410
		public static readonly DerObjectIdentifier UID = new DerObjectIdentifier("0.9.2342.19200300.100.1.1");

		// Token: 0x040028AB RID: 10411
		private static readonly bool[] defaultReverse = new bool[1];

		// Token: 0x040028AC RID: 10412
		public static readonly Hashtable DefaultSymbols = new Hashtable();

		// Token: 0x040028AD RID: 10413
		public static readonly Hashtable RFC2253Symbols = new Hashtable();

		// Token: 0x040028AE RID: 10414
		public static readonly Hashtable RFC1779Symbols = new Hashtable();

		// Token: 0x040028AF RID: 10415
		public static readonly Hashtable DefaultLookup = new Hashtable();

		// Token: 0x040028B0 RID: 10416
		private readonly IList ordering = Platform.CreateArrayList();

		// Token: 0x040028B1 RID: 10417
		private readonly X509NameEntryConverter converter;

		// Token: 0x040028B2 RID: 10418
		private IList values = Platform.CreateArrayList();

		// Token: 0x040028B3 RID: 10419
		private IList added = Platform.CreateArrayList();

		// Token: 0x040028B4 RID: 10420
		private Asn1Sequence seq;
	}
}
