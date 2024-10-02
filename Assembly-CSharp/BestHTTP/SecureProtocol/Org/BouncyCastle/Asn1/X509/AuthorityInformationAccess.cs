using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200068C RID: 1676
	public class AuthorityInformationAccess : Asn1Encodable
	{
		// Token: 0x06003E12 RID: 15890 RVA: 0x00175E5D File Offset: 0x0017405D
		public static AuthorityInformationAccess GetInstance(object obj)
		{
			if (obj is AuthorityInformationAccess)
			{
				return (AuthorityInformationAccess)obj;
			}
			if (obj == null)
			{
				return null;
			}
			return new AuthorityInformationAccess(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x00175E80 File Offset: 0x00174080
		private AuthorityInformationAccess(Asn1Sequence seq)
		{
			if (seq.Count < 1)
			{
				throw new ArgumentException("sequence may not be empty");
			}
			this.descriptions = new AccessDescription[seq.Count];
			for (int i = 0; i < seq.Count; i++)
			{
				this.descriptions[i] = AccessDescription.GetInstance(seq[i]);
			}
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x00175EDD File Offset: 0x001740DD
		public AuthorityInformationAccess(AccessDescription description)
		{
			this.descriptions = new AccessDescription[]
			{
				description
			};
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x00175EF5 File Offset: 0x001740F5
		public AuthorityInformationAccess(DerObjectIdentifier oid, GeneralName location) : this(new AccessDescription(oid, location))
		{
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x00175F04 File Offset: 0x00174104
		public AccessDescription[] GetAccessDescriptions()
		{
			return (AccessDescription[])this.descriptions.Clone();
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x00175F18 File Offset: 0x00174118
		public override Asn1Object ToAsn1Object()
		{
			Asn1Encodable[] v = this.descriptions;
			return new DerSequence(v);
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x00175F34 File Offset: 0x00174134
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string newLine = Platform.NewLine;
			stringBuilder.Append("AuthorityInformationAccess:");
			stringBuilder.Append(newLine);
			foreach (AccessDescription value in this.descriptions)
			{
				stringBuilder.Append("    ");
				stringBuilder.Append(value);
				stringBuilder.Append(newLine);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400278B RID: 10123
		private readonly AccessDescription[] descriptions;
	}
}
