using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200069D RID: 1693
	public class GeneralNames : Asn1Encodable
	{
		// Token: 0x06003E9E RID: 16030 RVA: 0x00177B58 File Offset: 0x00175D58
		public static GeneralNames GetInstance(object obj)
		{
			if (obj == null || obj is GeneralNames)
			{
				return (GeneralNames)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new GeneralNames((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x00177BA5 File Offset: 0x00175DA5
		public static GeneralNames GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return GeneralNames.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x00177BB3 File Offset: 0x00175DB3
		public GeneralNames(GeneralName name)
		{
			this.names = new GeneralName[]
			{
				name
			};
		}

		// Token: 0x06003EA1 RID: 16033 RVA: 0x00177BCB File Offset: 0x00175DCB
		public GeneralNames(GeneralName[] names)
		{
			this.names = (GeneralName[])names.Clone();
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x00177BE4 File Offset: 0x00175DE4
		private GeneralNames(Asn1Sequence seq)
		{
			this.names = new GeneralName[seq.Count];
			for (int num = 0; num != seq.Count; num++)
			{
				this.names[num] = GeneralName.GetInstance(seq[num]);
			}
		}

		// Token: 0x06003EA3 RID: 16035 RVA: 0x00177C2D File Offset: 0x00175E2D
		public GeneralName[] GetNames()
		{
			return (GeneralName[])this.names.Clone();
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x00177C40 File Offset: 0x00175E40
		public override Asn1Object ToAsn1Object()
		{
			Asn1Encodable[] v = this.names;
			return new DerSequence(v);
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x00177C5C File Offset: 0x00175E5C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string newLine = Platform.NewLine;
			stringBuilder.Append("GeneralNames:");
			stringBuilder.Append(newLine);
			foreach (GeneralName value in this.names)
			{
				stringBuilder.Append("    ");
				stringBuilder.Append(value);
				stringBuilder.Append(newLine);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040027C3 RID: 10179
		private readonly GeneralName[] names;
	}
}
