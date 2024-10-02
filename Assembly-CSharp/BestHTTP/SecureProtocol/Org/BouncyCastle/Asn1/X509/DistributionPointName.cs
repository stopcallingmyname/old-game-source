using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000699 RID: 1689
	public class DistributionPointName : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06003E71 RID: 15985 RVA: 0x00176FF0 File Offset: 0x001751F0
		public static DistributionPointName GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return DistributionPointName.GetInstance(Asn1TaggedObject.GetInstance(obj, true));
		}

		// Token: 0x06003E72 RID: 15986 RVA: 0x00177000 File Offset: 0x00175200
		public static DistributionPointName GetInstance(object obj)
		{
			if (obj == null || obj is DistributionPointName)
			{
				return (DistributionPointName)obj;
			}
			if (obj is Asn1TaggedObject)
			{
				return new DistributionPointName((Asn1TaggedObject)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003E73 RID: 15987 RVA: 0x0017704D File Offset: 0x0017524D
		public DistributionPointName(int type, Asn1Encodable name)
		{
			this.type = type;
			this.name = name;
		}

		// Token: 0x06003E74 RID: 15988 RVA: 0x00177063 File Offset: 0x00175263
		public DistributionPointName(GeneralNames name) : this(0, name)
		{
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06003E75 RID: 15989 RVA: 0x0017706D File Offset: 0x0017526D
		public int PointType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06003E76 RID: 15990 RVA: 0x00177075 File Offset: 0x00175275
		public Asn1Encodable Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x0017707D File Offset: 0x0017527D
		public DistributionPointName(Asn1TaggedObject obj)
		{
			this.type = obj.TagNo;
			if (this.type == 0)
			{
				this.name = GeneralNames.GetInstance(obj, false);
				return;
			}
			this.name = Asn1Set.GetInstance(obj, false);
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x001770B4 File Offset: 0x001752B4
		public override Asn1Object ToAsn1Object()
		{
			return new DerTaggedObject(false, this.type, this.name);
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x001770C8 File Offset: 0x001752C8
		public override string ToString()
		{
			string newLine = Platform.NewLine;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DistributionPointName: [");
			stringBuilder.Append(newLine);
			if (this.type == 0)
			{
				this.appendObject(stringBuilder, newLine, "fullName", this.name.ToString());
			}
			else
			{
				this.appendObject(stringBuilder, newLine, "nameRelativeToCRLIssuer", this.name.ToString());
			}
			stringBuilder.Append("]");
			stringBuilder.Append(newLine);
			return stringBuilder.ToString();
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x0017714C File Offset: 0x0017534C
		private void appendObject(StringBuilder buf, string sep, string name, string val)
		{
			string value = "    ";
			buf.Append(value);
			buf.Append(name);
			buf.Append(":");
			buf.Append(sep);
			buf.Append(value);
			buf.Append(value);
			buf.Append(val);
			buf.Append(sep);
		}

		// Token: 0x040027AF RID: 10159
		internal readonly Asn1Encodable name;

		// Token: 0x040027B0 RID: 10160
		internal readonly int type;

		// Token: 0x040027B1 RID: 10161
		public const int FullName = 0;

		// Token: 0x040027B2 RID: 10162
		public const int NameRelativeToCrlIssuer = 1;
	}
}
