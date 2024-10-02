using System;
using System.Text;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006AE RID: 1710
	public class RoleSyntax : Asn1Encodable
	{
		// Token: 0x06003F11 RID: 16145 RVA: 0x00179482 File Offset: 0x00177682
		public static RoleSyntax GetInstance(object obj)
		{
			if (obj is RoleSyntax)
			{
				return (RoleSyntax)obj;
			}
			if (obj != null)
			{
				return new RoleSyntax(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x001794A4 File Offset: 0x001776A4
		public RoleSyntax(GeneralNames roleAuthority, GeneralName roleName)
		{
			if (roleName == null || roleName.TagNo != 6 || ((IAsn1String)roleName.Name).GetString().Equals(""))
			{
				throw new ArgumentException("the role name MUST be non empty and MUST use the URI option of GeneralName");
			}
			this.roleAuthority = roleAuthority;
			this.roleName = roleName;
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x001794F8 File Offset: 0x001776F8
		public RoleSyntax(GeneralName roleName) : this(null, roleName)
		{
		}

		// Token: 0x06003F14 RID: 16148 RVA: 0x00179502 File Offset: 0x00177702
		public RoleSyntax(string roleName) : this(new GeneralName(6, (roleName == null) ? "" : roleName))
		{
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x0017951C File Offset: 0x0017771C
		private RoleSyntax(Asn1Sequence seq)
		{
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			for (int num = 0; num != seq.Count; num++)
			{
				Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(seq[num]);
				int tagNo = instance.TagNo;
				if (tagNo != 0)
				{
					if (tagNo != 1)
					{
						throw new ArgumentException("Unknown tag in RoleSyntax");
					}
					this.roleName = GeneralName.GetInstance(instance, true);
				}
				else
				{
					this.roleAuthority = GeneralNames.GetInstance(instance, false);
				}
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06003F16 RID: 16150 RVA: 0x001795B3 File Offset: 0x001777B3
		public GeneralNames RoleAuthority
		{
			get
			{
				return this.roleAuthority;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06003F17 RID: 16151 RVA: 0x001795BB File Offset: 0x001777BB
		public GeneralName RoleName
		{
			get
			{
				return this.roleName;
			}
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x001795C3 File Offset: 0x001777C3
		public string GetRoleNameAsString()
		{
			return ((IAsn1String)this.roleName.Name).GetString();
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x001795DC File Offset: 0x001777DC
		public string[] GetRoleAuthorityAsString()
		{
			if (this.roleAuthority == null)
			{
				return new string[0];
			}
			GeneralName[] names = this.roleAuthority.GetNames();
			string[] array = new string[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				Asn1Encodable name = names[i].Name;
				if (name is IAsn1String)
				{
					array[i] = ((IAsn1String)name).GetString();
				}
				else
				{
					array[i] = name.ToString();
				}
			}
			return array;
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x00179648 File Offset: 0x00177848
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.roleAuthority != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.roleAuthority)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				new DerTaggedObject(true, 1, this.roleName)
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x001796A8 File Offset: 0x001778A8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("Name: " + this.GetRoleNameAsString() + " - Auth: ");
			if (this.roleAuthority == null || this.roleAuthority.GetNames().Length == 0)
			{
				stringBuilder.Append("N/A");
			}
			else
			{
				string[] roleAuthorityAsString = this.GetRoleAuthorityAsString();
				stringBuilder.Append('[').Append(roleAuthorityAsString[0]);
				for (int i = 1; i < roleAuthorityAsString.Length; i++)
				{
					stringBuilder.Append(", ").Append(roleAuthorityAsString[i]);
				}
				stringBuilder.Append(']');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400280F RID: 10255
		private readonly GeneralNames roleAuthority;

		// Token: 0x04002810 RID: 10256
		private readonly GeneralName roleName;
	}
}
