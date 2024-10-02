using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002BE RID: 702
	public class PkixNameConstraintValidator
	{
		// Token: 0x06001968 RID: 6504 RVA: 0x000BE580 File Offset: 0x000BC780
		private static bool WithinDNSubtree(Asn1Sequence dns, Asn1Sequence subtree)
		{
			if (subtree.Count < 1)
			{
				return false;
			}
			if (subtree.Count > dns.Count)
			{
				return false;
			}
			for (int i = subtree.Count - 1; i >= 0; i--)
			{
				if (!subtree[i].Equals(dns[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x000BE5D3 File Offset: 0x000BC7D3
		public void CheckPermittedDN(Asn1Sequence dns)
		{
			this.CheckPermittedDN(this.permittedSubtreesDN, dns);
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x000BE5E2 File Offset: 0x000BC7E2
		public void CheckExcludedDN(Asn1Sequence dns)
		{
			this.CheckExcludedDN(this.excludedSubtreesDN, dns);
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x000BE5F4 File Offset: 0x000BC7F4
		private void CheckPermittedDN(ISet permitted, Asn1Sequence dns)
		{
			if (permitted == null)
			{
				return;
			}
			if (permitted.Count == 0 && dns.Count == 0)
			{
				return;
			}
			foreach (object obj in permitted)
			{
				Asn1Sequence subtree = (Asn1Sequence)obj;
				if (PkixNameConstraintValidator.WithinDNSubtree(dns, subtree))
				{
					return;
				}
			}
			throw new PkixNameConstraintValidatorException("Subject distinguished name is not from a permitted subtree");
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x000BE648 File Offset: 0x000BC848
		private void CheckExcludedDN(ISet excluded, Asn1Sequence dns)
		{
			if (excluded.IsEmpty)
			{
				return;
			}
			foreach (object obj in excluded)
			{
				Asn1Sequence subtree = (Asn1Sequence)obj;
				if (PkixNameConstraintValidator.WithinDNSubtree(dns, subtree))
				{
					throw new PkixNameConstraintValidatorException("Subject distinguished name is from an excluded subtree");
				}
			}
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x000BE690 File Offset: 0x000BC890
		private ISet IntersectDN(ISet permitted, ISet dns)
		{
			ISet set = new HashSet();
			foreach (object obj in dns)
			{
				Asn1Sequence instance = Asn1Sequence.GetInstance(((GeneralSubtree)obj).Base.Name.ToAsn1Object());
				if (permitted == null)
				{
					if (instance != null)
					{
						set.Add(instance);
					}
				}
				else
				{
					foreach (object obj2 in permitted)
					{
						Asn1Sequence asn1Sequence = (Asn1Sequence)obj2;
						if (PkixNameConstraintValidator.WithinDNSubtree(instance, asn1Sequence))
						{
							set.Add(instance);
						}
						else if (PkixNameConstraintValidator.WithinDNSubtree(asn1Sequence, instance))
						{
							set.Add(asn1Sequence);
						}
					}
				}
			}
			return set;
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x000BE728 File Offset: 0x000BC928
		private ISet UnionDN(ISet excluded, Asn1Sequence dn)
		{
			if (!excluded.IsEmpty)
			{
				ISet set = new HashSet();
				foreach (object obj in excluded)
				{
					Asn1Sequence asn1Sequence = (Asn1Sequence)obj;
					if (PkixNameConstraintValidator.WithinDNSubtree(dn, asn1Sequence))
					{
						set.Add(asn1Sequence);
					}
					else if (PkixNameConstraintValidator.WithinDNSubtree(asn1Sequence, dn))
					{
						set.Add(dn);
					}
					else
					{
						set.Add(asn1Sequence);
						set.Add(dn);
					}
				}
				return set;
			}
			if (dn == null)
			{
				return excluded;
			}
			excluded.Add(dn);
			return excluded;
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x000BE7A4 File Offset: 0x000BC9A4
		private ISet IntersectEmail(ISet permitted, ISet emails)
		{
			ISet set = new HashSet();
			foreach (object obj in emails)
			{
				string text = this.ExtractNameAsString(((GeneralSubtree)obj).Base);
				if (permitted == null)
				{
					if (text != null)
					{
						set.Add(text);
					}
				}
				else
				{
					foreach (object obj2 in permitted)
					{
						string email = (string)obj2;
						this.intersectEmail(text, email, set);
					}
				}
			}
			return set;
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x000BE818 File Offset: 0x000BCA18
		private ISet UnionEmail(ISet excluded, string email)
		{
			if (!excluded.IsEmpty)
			{
				ISet set = new HashSet();
				foreach (object obj in excluded)
				{
					string email2 = (string)obj;
					this.unionEmail(email2, email, set);
				}
				return set;
			}
			if (email == null)
			{
				return excluded;
			}
			excluded.Add(email);
			return excluded;
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x000BE868 File Offset: 0x000BCA68
		private ISet IntersectIP(ISet permitted, ISet ips)
		{
			ISet set = new HashSet();
			foreach (object obj in ips)
			{
				byte[] octets = Asn1OctetString.GetInstance(((GeneralSubtree)obj).Base.Name).GetOctets();
				if (permitted == null)
				{
					if (octets != null)
					{
						set.Add(octets);
					}
				}
				else
				{
					foreach (object obj2 in permitted)
					{
						byte[] ipWithSubmask = (byte[])obj2;
						set.AddAll(this.IntersectIPRange(ipWithSubmask, octets));
					}
				}
			}
			return set;
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x000BE8EC File Offset: 0x000BCAEC
		private ISet UnionIP(ISet excluded, byte[] ip)
		{
			if (!excluded.IsEmpty)
			{
				ISet set = new HashSet();
				foreach (object obj in excluded)
				{
					byte[] ipWithSubmask = (byte[])obj;
					set.AddAll(this.UnionIPRange(ipWithSubmask, ip));
				}
				return set;
			}
			if (ip == null)
			{
				return excluded;
			}
			excluded.Add(ip);
			return excluded;
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x000BE944 File Offset: 0x000BCB44
		private ISet UnionIPRange(byte[] ipWithSubmask1, byte[] ipWithSubmask2)
		{
			ISet set = new HashSet();
			if (Arrays.AreEqual(ipWithSubmask1, ipWithSubmask2))
			{
				set.Add(ipWithSubmask1);
			}
			else
			{
				set.Add(ipWithSubmask1);
				set.Add(ipWithSubmask2);
			}
			return set;
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x000BE978 File Offset: 0x000BCB78
		private ISet IntersectIPRange(byte[] ipWithSubmask1, byte[] ipWithSubmask2)
		{
			if (ipWithSubmask1.Length != ipWithSubmask2.Length)
			{
				return new HashSet();
			}
			byte[][] array = this.ExtractIPsAndSubnetMasks(ipWithSubmask1, ipWithSubmask2);
			byte[] ip = array[0];
			byte[] array2 = array[1];
			byte[] ip2 = array[2];
			byte[] array3 = array[3];
			byte[][] array4 = this.MinMaxIPs(ip, array2, ip2, array3);
			byte[] ip3 = PkixNameConstraintValidator.Min(array4[1], array4[3]);
			if (PkixNameConstraintValidator.CompareTo(PkixNameConstraintValidator.Max(array4[0], array4[2]), ip3) == 1)
			{
				return new HashSet();
			}
			byte[] ip4 = PkixNameConstraintValidator.Or(array4[0], array4[2]);
			byte[] subnetMask = PkixNameConstraintValidator.Or(array2, array3);
			return new HashSet
			{
				this.IpWithSubnetMask(ip4, subnetMask)
			};
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x000BEA10 File Offset: 0x000BCC10
		private byte[] IpWithSubnetMask(byte[] ip, byte[] subnetMask)
		{
			int num = ip.Length;
			byte[] array = new byte[num * 2];
			Array.Copy(ip, 0, array, 0, num);
			Array.Copy(subnetMask, 0, array, num, num);
			return array;
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x000BEA40 File Offset: 0x000BCC40
		private byte[][] ExtractIPsAndSubnetMasks(byte[] ipWithSubmask1, byte[] ipWithSubmask2)
		{
			int num = ipWithSubmask1.Length / 2;
			byte[] array = new byte[num];
			byte[] array2 = new byte[num];
			Array.Copy(ipWithSubmask1, 0, array, 0, num);
			Array.Copy(ipWithSubmask1, num, array2, 0, num);
			byte[] array3 = new byte[num];
			byte[] array4 = new byte[num];
			Array.Copy(ipWithSubmask2, 0, array3, 0, num);
			Array.Copy(ipWithSubmask2, num, array4, 0, num);
			return new byte[][]
			{
				array,
				array2,
				array3,
				array4
			};
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x000BEAB0 File Offset: 0x000BCCB0
		private byte[][] MinMaxIPs(byte[] ip1, byte[] subnetmask1, byte[] ip2, byte[] subnetmask2)
		{
			int num = ip1.Length;
			byte[] array = new byte[num];
			byte[] array2 = new byte[num];
			byte[] array3 = new byte[num];
			byte[] array4 = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (ip1[i] & subnetmask1[i]);
				array2[i] = ((ip1[i] & subnetmask1[i]) | ~subnetmask1[i]);
				array3[i] = (ip2[i] & subnetmask2[i]);
				array4[i] = ((ip2[i] & subnetmask2[i]) | ~subnetmask2[i]);
			}
			return new byte[][]
			{
				array,
				array2,
				array3,
				array4
			};
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x000BEB50 File Offset: 0x000BCD50
		private void CheckPermittedEmail(ISet permitted, string email)
		{
			if (permitted == null)
			{
				return;
			}
			foreach (object obj in permitted)
			{
				string constraint = (string)obj;
				if (this.EmailIsConstrained(email, constraint))
				{
					return;
				}
			}
			if (email.Length == 0 && permitted.Count == 0)
			{
				return;
			}
			throw new PkixNameConstraintValidatorException("Subject email address is not from a permitted subtree.");
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x000BEBA4 File Offset: 0x000BCDA4
		private void CheckExcludedEmail(ISet excluded, string email)
		{
			if (excluded.IsEmpty)
			{
				return;
			}
			foreach (object obj in excluded)
			{
				string constraint = (string)obj;
				if (this.EmailIsConstrained(email, constraint))
				{
					throw new PkixNameConstraintValidatorException("Email address is from an excluded subtree.");
				}
			}
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x000BEBEC File Offset: 0x000BCDEC
		private void CheckPermittedIP(ISet permitted, byte[] ip)
		{
			if (permitted == null)
			{
				return;
			}
			foreach (object obj in permitted)
			{
				byte[] constraint = (byte[])obj;
				if (this.IsIPConstrained(ip, constraint))
				{
					return;
				}
			}
			if (ip.Length == 0 && permitted.Count == 0)
			{
				return;
			}
			throw new PkixNameConstraintValidatorException("IP is not from a permitted subtree.");
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x000BEC3C File Offset: 0x000BCE3C
		private void checkExcludedIP(ISet excluded, byte[] ip)
		{
			if (excluded.IsEmpty)
			{
				return;
			}
			foreach (object obj in excluded)
			{
				byte[] constraint = (byte[])obj;
				if (this.IsIPConstrained(ip, constraint))
				{
					throw new PkixNameConstraintValidatorException("IP is from an excluded subtree.");
				}
			}
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x000BEC84 File Offset: 0x000BCE84
		private bool IsIPConstrained(byte[] ip, byte[] constraint)
		{
			int num = ip.Length;
			if (num != constraint.Length / 2)
			{
				return false;
			}
			byte[] array = new byte[num];
			Array.Copy(constraint, num, array, 0, num);
			byte[] array2 = new byte[num];
			byte[] array3 = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = (constraint[i] & array[i]);
				array3[i] = (ip[i] & array[i]);
			}
			return Arrays.AreEqual(array2, array3);
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x000BECF4 File Offset: 0x000BCEF4
		private bool EmailIsConstrained(string email, string constraint)
		{
			string text = email.Substring(email.IndexOf('@') + 1);
			if (constraint.IndexOf('@') != -1)
			{
				if (Platform.ToUpperInvariant(email).Equals(Platform.ToUpperInvariant(constraint)))
				{
					return true;
				}
			}
			else if (!constraint[0].Equals('.'))
			{
				if (Platform.ToUpperInvariant(text).Equals(Platform.ToUpperInvariant(constraint)))
				{
					return true;
				}
			}
			else if (this.WithinDomain(text, constraint))
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x000BED68 File Offset: 0x000BCF68
		private bool WithinDomain(string testDomain, string domain)
		{
			string text = domain;
			if (Platform.StartsWith(text, "."))
			{
				text = text.Substring(1);
			}
			string[] array = text.Split(new char[]
			{
				'.'
			});
			string[] array2 = testDomain.Split(new char[]
			{
				'.'
			});
			if (array2.Length <= array.Length)
			{
				return false;
			}
			int num = array2.Length - array.Length;
			for (int i = -1; i < array.Length; i++)
			{
				if (i == -1)
				{
					if (array2[i + num].Equals(""))
					{
						return false;
					}
				}
				else if (!Platform.EqualsIgnoreCase(array2[i + num], array[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x000BEE04 File Offset: 0x000BD004
		private void CheckPermittedDNS(ISet permitted, string dns)
		{
			if (permitted == null)
			{
				return;
			}
			foreach (object obj in permitted)
			{
				string text = (string)obj;
				if (this.WithinDomain(dns, text) || Platform.ToUpperInvariant(dns).Equals(Platform.ToUpperInvariant(text)))
				{
					return;
				}
			}
			if (dns.Length == 0 && permitted.Count == 0)
			{
				return;
			}
			throw new PkixNameConstraintValidatorException("DNS is not from a permitted subtree.");
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x000BEE6C File Offset: 0x000BD06C
		private void checkExcludedDNS(ISet excluded, string dns)
		{
			if (excluded.IsEmpty)
			{
				return;
			}
			foreach (object obj in excluded)
			{
				string text = (string)obj;
				if (this.WithinDomain(dns, text) || Platform.EqualsIgnoreCase(dns, text))
				{
					throw new PkixNameConstraintValidatorException("DNS is from an excluded subtree.");
				}
			}
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x000BEEC0 File Offset: 0x000BD0C0
		private void unionEmail(string email1, string email2, ISet union)
		{
			if (email1.IndexOf('@') != -1)
			{
				string text = email1.Substring(email1.IndexOf('@') + 1);
				if (email2.IndexOf('@') != -1)
				{
					if (Platform.EqualsIgnoreCase(email1, email2))
					{
						union.Add(email1);
						return;
					}
					union.Add(email1);
					union.Add(email2);
					return;
				}
				else if (Platform.StartsWith(email2, "."))
				{
					if (this.WithinDomain(text, email2))
					{
						union.Add(email2);
						return;
					}
					union.Add(email1);
					union.Add(email2);
					return;
				}
				else
				{
					if (Platform.EqualsIgnoreCase(text, email2))
					{
						union.Add(email2);
						return;
					}
					union.Add(email1);
					union.Add(email2);
					return;
				}
			}
			else if (Platform.StartsWith(email1, "."))
			{
				if (email2.IndexOf('@') != -1)
				{
					string testDomain = email2.Substring(email1.IndexOf('@') + 1);
					if (this.WithinDomain(testDomain, email1))
					{
						union.Add(email1);
						return;
					}
					union.Add(email1);
					union.Add(email2);
					return;
				}
				else if (Platform.StartsWith(email2, "."))
				{
					if (this.WithinDomain(email1, email2) || Platform.EqualsIgnoreCase(email1, email2))
					{
						union.Add(email2);
						return;
					}
					if (this.WithinDomain(email2, email1))
					{
						union.Add(email1);
						return;
					}
					union.Add(email1);
					union.Add(email2);
					return;
				}
				else
				{
					if (this.WithinDomain(email2, email1))
					{
						union.Add(email1);
						return;
					}
					union.Add(email1);
					union.Add(email2);
					return;
				}
			}
			else if (email2.IndexOf('@') != -1)
			{
				if (Platform.EqualsIgnoreCase(email2.Substring(email1.IndexOf('@') + 1), email1))
				{
					union.Add(email1);
					return;
				}
				union.Add(email1);
				union.Add(email2);
				return;
			}
			else if (Platform.StartsWith(email2, "."))
			{
				if (this.WithinDomain(email1, email2))
				{
					union.Add(email2);
					return;
				}
				union.Add(email1);
				union.Add(email2);
				return;
			}
			else
			{
				if (Platform.EqualsIgnoreCase(email1, email2))
				{
					union.Add(email1);
					return;
				}
				union.Add(email1);
				union.Add(email2);
				return;
			}
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x000BF0A4 File Offset: 0x000BD2A4
		private void unionURI(string email1, string email2, ISet union)
		{
			if (email1.IndexOf('@') != -1)
			{
				string text = email1.Substring(email1.IndexOf('@') + 1);
				if (email2.IndexOf('@') != -1)
				{
					if (Platform.EqualsIgnoreCase(email1, email2))
					{
						union.Add(email1);
						return;
					}
					union.Add(email1);
					union.Add(email2);
					return;
				}
				else if (Platform.StartsWith(email2, "."))
				{
					if (this.WithinDomain(text, email2))
					{
						union.Add(email2);
						return;
					}
					union.Add(email1);
					union.Add(email2);
					return;
				}
				else
				{
					if (Platform.EqualsIgnoreCase(text, email2))
					{
						union.Add(email2);
						return;
					}
					union.Add(email1);
					union.Add(email2);
					return;
				}
			}
			else if (Platform.StartsWith(email1, "."))
			{
				if (email2.IndexOf('@') != -1)
				{
					string testDomain = email2.Substring(email1.IndexOf('@') + 1);
					if (this.WithinDomain(testDomain, email1))
					{
						union.Add(email1);
						return;
					}
					union.Add(email1);
					union.Add(email2);
					return;
				}
				else if (Platform.StartsWith(email2, "."))
				{
					if (this.WithinDomain(email1, email2) || Platform.EqualsIgnoreCase(email1, email2))
					{
						union.Add(email2);
						return;
					}
					if (this.WithinDomain(email2, email1))
					{
						union.Add(email1);
						return;
					}
					union.Add(email1);
					union.Add(email2);
					return;
				}
				else
				{
					if (this.WithinDomain(email2, email1))
					{
						union.Add(email1);
						return;
					}
					union.Add(email1);
					union.Add(email2);
					return;
				}
			}
			else if (email2.IndexOf('@') != -1)
			{
				if (Platform.EqualsIgnoreCase(email2.Substring(email1.IndexOf('@') + 1), email1))
				{
					union.Add(email1);
					return;
				}
				union.Add(email1);
				union.Add(email2);
				return;
			}
			else if (Platform.StartsWith(email2, "."))
			{
				if (this.WithinDomain(email1, email2))
				{
					union.Add(email2);
					return;
				}
				union.Add(email1);
				union.Add(email2);
				return;
			}
			else
			{
				if (Platform.EqualsIgnoreCase(email1, email2))
				{
					union.Add(email1);
					return;
				}
				union.Add(email1);
				union.Add(email2);
				return;
			}
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x000BF288 File Offset: 0x000BD488
		private ISet intersectDNS(ISet permitted, ISet dnss)
		{
			ISet set = new HashSet();
			foreach (object obj in dnss)
			{
				string text = this.ExtractNameAsString(((GeneralSubtree)obj).Base);
				if (permitted == null)
				{
					if (text != null)
					{
						set.Add(text);
					}
				}
				else
				{
					foreach (object obj2 in permitted)
					{
						string text2 = (string)obj2;
						if (this.WithinDomain(text2, text))
						{
							set.Add(text2);
						}
						else if (this.WithinDomain(text, text2))
						{
							set.Add(text);
						}
					}
				}
			}
			return set;
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x000BF318 File Offset: 0x000BD518
		protected ISet unionDNS(ISet excluded, string dns)
		{
			if (!excluded.IsEmpty)
			{
				ISet set = new HashSet();
				foreach (object obj in excluded)
				{
					string text = (string)obj;
					if (this.WithinDomain(text, dns))
					{
						set.Add(dns);
					}
					else if (this.WithinDomain(dns, text))
					{
						set.Add(text);
					}
					else
					{
						set.Add(text);
						set.Add(dns);
					}
				}
				return set;
			}
			if (dns == null)
			{
				return excluded;
			}
			excluded.Add(dns);
			return excluded;
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x000BF394 File Offset: 0x000BD594
		private void intersectEmail(string email1, string email2, ISet intersect)
		{
			if (email1.IndexOf('@') != -1)
			{
				string text = email1.Substring(email1.IndexOf('@') + 1);
				if (email2.IndexOf('@') != -1)
				{
					if (Platform.EqualsIgnoreCase(email1, email2))
					{
						intersect.Add(email1);
						return;
					}
				}
				else if (Platform.StartsWith(email2, "."))
				{
					if (this.WithinDomain(text, email2))
					{
						intersect.Add(email1);
						return;
					}
				}
				else if (Platform.EqualsIgnoreCase(text, email2))
				{
					intersect.Add(email1);
					return;
				}
			}
			else if (Platform.StartsWith(email1, "."))
			{
				if (email2.IndexOf('@') != -1)
				{
					string testDomain = email2.Substring(email1.IndexOf('@') + 1);
					if (this.WithinDomain(testDomain, email1))
					{
						intersect.Add(email2);
						return;
					}
				}
				else if (Platform.StartsWith(email2, "."))
				{
					if (this.WithinDomain(email1, email2) || Platform.EqualsIgnoreCase(email1, email2))
					{
						intersect.Add(email1);
						return;
					}
					if (this.WithinDomain(email2, email1))
					{
						intersect.Add(email2);
						return;
					}
				}
				else if (this.WithinDomain(email2, email1))
				{
					intersect.Add(email2);
					return;
				}
			}
			else if (email2.IndexOf('@') != -1)
			{
				if (Platform.EqualsIgnoreCase(email2.Substring(email2.IndexOf('@') + 1), email1))
				{
					intersect.Add(email2);
					return;
				}
			}
			else if (Platform.StartsWith(email2, "."))
			{
				if (this.WithinDomain(email1, email2))
				{
					intersect.Add(email1);
					return;
				}
			}
			else if (Platform.EqualsIgnoreCase(email1, email2))
			{
				intersect.Add(email1);
			}
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x000BF4F8 File Offset: 0x000BD6F8
		private void checkExcludedURI(ISet excluded, string uri)
		{
			if (excluded.IsEmpty)
			{
				return;
			}
			foreach (object obj in excluded)
			{
				string constraint = (string)obj;
				if (this.IsUriConstrained(uri, constraint))
				{
					throw new PkixNameConstraintValidatorException("URI is from an excluded subtree.");
				}
			}
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x000BF540 File Offset: 0x000BD740
		private ISet intersectURI(ISet permitted, ISet uris)
		{
			ISet set = new HashSet();
			foreach (object obj in uris)
			{
				string text = this.ExtractNameAsString(((GeneralSubtree)obj).Base);
				if (permitted == null)
				{
					if (text != null)
					{
						set.Add(text);
					}
				}
				else
				{
					foreach (object obj2 in permitted)
					{
						string email = (string)obj2;
						this.intersectURI(email, text, set);
					}
				}
			}
			return set;
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x000BF5B4 File Offset: 0x000BD7B4
		private ISet unionURI(ISet excluded, string uri)
		{
			if (!excluded.IsEmpty)
			{
				ISet set = new HashSet();
				foreach (object obj in excluded)
				{
					string email = (string)obj;
					this.unionURI(email, uri, set);
				}
				return set;
			}
			if (uri == null)
			{
				return excluded;
			}
			excluded.Add(uri);
			return excluded;
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x000BF604 File Offset: 0x000BD804
		private void intersectURI(string email1, string email2, ISet intersect)
		{
			if (email1.IndexOf('@') != -1)
			{
				string text = email1.Substring(email1.IndexOf('@') + 1);
				if (email2.IndexOf('@') != -1)
				{
					if (Platform.EqualsIgnoreCase(email1, email2))
					{
						intersect.Add(email1);
						return;
					}
				}
				else if (Platform.StartsWith(email2, "."))
				{
					if (this.WithinDomain(text, email2))
					{
						intersect.Add(email1);
						return;
					}
				}
				else if (Platform.EqualsIgnoreCase(text, email2))
				{
					intersect.Add(email1);
					return;
				}
			}
			else if (Platform.StartsWith(email1, "."))
			{
				if (email2.IndexOf('@') != -1)
				{
					string testDomain = email2.Substring(email1.IndexOf('@') + 1);
					if (this.WithinDomain(testDomain, email1))
					{
						intersect.Add(email2);
						return;
					}
				}
				else if (Platform.StartsWith(email2, "."))
				{
					if (this.WithinDomain(email1, email2) || Platform.EqualsIgnoreCase(email1, email2))
					{
						intersect.Add(email1);
						return;
					}
					if (this.WithinDomain(email2, email1))
					{
						intersect.Add(email2);
						return;
					}
				}
				else if (this.WithinDomain(email2, email1))
				{
					intersect.Add(email2);
					return;
				}
			}
			else if (email2.IndexOf('@') != -1)
			{
				if (Platform.EqualsIgnoreCase(email2.Substring(email2.IndexOf('@') + 1), email1))
				{
					intersect.Add(email2);
					return;
				}
			}
			else if (Platform.StartsWith(email2, "."))
			{
				if (this.WithinDomain(email1, email2))
				{
					intersect.Add(email1);
					return;
				}
			}
			else if (Platform.EqualsIgnoreCase(email1, email2))
			{
				intersect.Add(email1);
			}
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x000BF768 File Offset: 0x000BD968
		private void CheckPermittedURI(ISet permitted, string uri)
		{
			if (permitted == null)
			{
				return;
			}
			foreach (object obj in permitted)
			{
				string constraint = (string)obj;
				if (this.IsUriConstrained(uri, constraint))
				{
					return;
				}
			}
			if (uri.Length == 0 && permitted.Count == 0)
			{
				return;
			}
			throw new PkixNameConstraintValidatorException("URI is not from a permitted subtree.");
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x000BF7BC File Offset: 0x000BD9BC
		private bool IsUriConstrained(string uri, string constraint)
		{
			string text = PkixNameConstraintValidator.ExtractHostFromURL(uri);
			if (!Platform.StartsWith(constraint, "."))
			{
				if (Platform.EqualsIgnoreCase(text, constraint))
				{
					return true;
				}
			}
			else if (this.WithinDomain(text, constraint))
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x000BF7F8 File Offset: 0x000BD9F8
		private static string ExtractHostFromURL(string url)
		{
			string text = url.Substring(url.IndexOf(':') + 1);
			int num = Platform.IndexOf(text, "//");
			if (num != -1)
			{
				text = text.Substring(num + 2);
			}
			if (text.LastIndexOf(':') != -1)
			{
				text = text.Substring(0, text.LastIndexOf(':'));
			}
			text = text.Substring(text.IndexOf(':') + 1);
			text = text.Substring(text.IndexOf('@') + 1);
			if (text.IndexOf('/') != -1)
			{
				text = text.Substring(0, text.IndexOf('/'));
			}
			return text;
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x000BF88C File Offset: 0x000BDA8C
		public void checkPermitted(GeneralName name)
		{
			switch (name.TagNo)
			{
			case 1:
				this.CheckPermittedEmail(this.permittedSubtreesEmail, this.ExtractNameAsString(name));
				return;
			case 2:
				this.CheckPermittedDNS(this.permittedSubtreesDNS, DerIA5String.GetInstance(name.Name).GetString());
				return;
			case 3:
			case 5:
				break;
			case 4:
				this.CheckPermittedDN(Asn1Sequence.GetInstance(name.Name.ToAsn1Object()));
				return;
			case 6:
				this.CheckPermittedURI(this.permittedSubtreesURI, DerIA5String.GetInstance(name.Name).GetString());
				return;
			case 7:
			{
				byte[] octets = Asn1OctetString.GetInstance(name.Name).GetOctets();
				this.CheckPermittedIP(this.permittedSubtreesIP, octets);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x000BF948 File Offset: 0x000BDB48
		public void checkExcluded(GeneralName name)
		{
			switch (name.TagNo)
			{
			case 1:
				this.CheckExcludedEmail(this.excludedSubtreesEmail, this.ExtractNameAsString(name));
				return;
			case 2:
				this.checkExcludedDNS(this.excludedSubtreesDNS, DerIA5String.GetInstance(name.Name).GetString());
				return;
			case 3:
			case 5:
				break;
			case 4:
				this.CheckExcludedDN(Asn1Sequence.GetInstance(name.Name.ToAsn1Object()));
				return;
			case 6:
				this.checkExcludedURI(this.excludedSubtreesURI, DerIA5String.GetInstance(name.Name).GetString());
				return;
			case 7:
			{
				byte[] octets = Asn1OctetString.GetInstance(name.Name).GetOctets();
				this.checkExcludedIP(this.excludedSubtreesIP, octets);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x000BFA04 File Offset: 0x000BDC04
		public void IntersectPermittedSubtree(Asn1Sequence permitted)
		{
			IDictionary dictionary = Platform.CreateHashtable();
			foreach (object obj in permitted)
			{
				GeneralSubtree instance = GeneralSubtree.GetInstance(obj);
				int tagNo = instance.Base.TagNo;
				if (dictionary[tagNo] == null)
				{
					dictionary[tagNo] = new HashSet();
				}
				((ISet)dictionary[tagNo]).Add(instance);
			}
			foreach (object obj2 in dictionary)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
				switch ((int)dictionaryEntry.Key)
				{
				case 1:
					this.permittedSubtreesEmail = this.IntersectEmail(this.permittedSubtreesEmail, (ISet)dictionaryEntry.Value);
					break;
				case 2:
					this.permittedSubtreesDNS = this.intersectDNS(this.permittedSubtreesDNS, (ISet)dictionaryEntry.Value);
					break;
				case 4:
					this.permittedSubtreesDN = this.IntersectDN(this.permittedSubtreesDN, (ISet)dictionaryEntry.Value);
					break;
				case 6:
					this.permittedSubtreesURI = this.intersectURI(this.permittedSubtreesURI, (ISet)dictionaryEntry.Value);
					break;
				case 7:
					this.permittedSubtreesIP = this.IntersectIP(this.permittedSubtreesIP, (ISet)dictionaryEntry.Value);
					break;
				}
			}
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x000BFB73 File Offset: 0x000BDD73
		private string ExtractNameAsString(GeneralName name)
		{
			return DerIA5String.GetInstance(name.Name).GetString();
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x000BFB88 File Offset: 0x000BDD88
		public void IntersectEmptyPermittedSubtree(int nameType)
		{
			switch (nameType)
			{
			case 1:
				this.permittedSubtreesEmail = new HashSet();
				return;
			case 2:
				this.permittedSubtreesDNS = new HashSet();
				return;
			case 3:
			case 5:
				break;
			case 4:
				this.permittedSubtreesDN = new HashSet();
				return;
			case 6:
				this.permittedSubtreesURI = new HashSet();
				return;
			case 7:
				this.permittedSubtreesIP = new HashSet();
				break;
			default:
				return;
			}
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x000BFBF8 File Offset: 0x000BDDF8
		public void AddExcludedSubtree(GeneralSubtree subtree)
		{
			GeneralName @base = subtree.Base;
			switch (@base.TagNo)
			{
			case 1:
				this.excludedSubtreesEmail = this.UnionEmail(this.excludedSubtreesEmail, this.ExtractNameAsString(@base));
				return;
			case 2:
				this.excludedSubtreesDNS = this.unionDNS(this.excludedSubtreesDNS, this.ExtractNameAsString(@base));
				return;
			case 3:
			case 5:
				break;
			case 4:
				this.excludedSubtreesDN = this.UnionDN(this.excludedSubtreesDN, (Asn1Sequence)@base.Name.ToAsn1Object());
				return;
			case 6:
				this.excludedSubtreesURI = this.unionURI(this.excludedSubtreesURI, this.ExtractNameAsString(@base));
				return;
			case 7:
				this.excludedSubtreesIP = this.UnionIP(this.excludedSubtreesIP, Asn1OctetString.GetInstance(@base.Name).GetOctets());
				break;
			default:
				return;
			}
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x000BFCCC File Offset: 0x000BDECC
		private static byte[] Max(byte[] ip1, byte[] ip2)
		{
			for (int i = 0; i < ip1.Length; i++)
			{
				if (((int)ip1[i] & 65535) > ((int)ip2[i] & 65535))
				{
					return ip1;
				}
			}
			return ip2;
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x000BFD00 File Offset: 0x000BDF00
		private static byte[] Min(byte[] ip1, byte[] ip2)
		{
			for (int i = 0; i < ip1.Length; i++)
			{
				if (((int)ip1[i] & 65535) < ((int)ip2[i] & 65535))
				{
					return ip1;
				}
			}
			return ip2;
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x000BFD32 File Offset: 0x000BDF32
		private static int CompareTo(byte[] ip1, byte[] ip2)
		{
			if (Arrays.AreEqual(ip1, ip2))
			{
				return 0;
			}
			if (Arrays.AreEqual(PkixNameConstraintValidator.Max(ip1, ip2), ip1))
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x000BFD54 File Offset: 0x000BDF54
		private static byte[] Or(byte[] ip1, byte[] ip2)
		{
			byte[] array = new byte[ip1.Length];
			for (int i = 0; i < ip1.Length; i++)
			{
				array[i] = (ip1[i] | ip2[i]);
			}
			return array;
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x000BFD84 File Offset: 0x000BDF84
		[Obsolete("Use GetHashCode instead")]
		public int HashCode()
		{
			return this.GetHashCode();
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x000BFD8C File Offset: 0x000BDF8C
		public override int GetHashCode()
		{
			return this.HashCollection(this.excludedSubtreesDN) + this.HashCollection(this.excludedSubtreesDNS) + this.HashCollection(this.excludedSubtreesEmail) + this.HashCollection(this.excludedSubtreesIP) + this.HashCollection(this.excludedSubtreesURI) + this.HashCollection(this.permittedSubtreesDN) + this.HashCollection(this.permittedSubtreesDNS) + this.HashCollection(this.permittedSubtreesEmail) + this.HashCollection(this.permittedSubtreesIP) + this.HashCollection(this.permittedSubtreesURI);
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x000BFE1C File Offset: 0x000BE01C
		private int HashCollection(ICollection coll)
		{
			if (coll == null)
			{
				return 0;
			}
			int num = 0;
			foreach (object obj in coll)
			{
				if (obj is byte[])
				{
					num += Arrays.GetHashCode((byte[])obj);
				}
				else
				{
					num += obj.GetHashCode();
				}
			}
			return num;
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x000BFE6C File Offset: 0x000BE06C
		public override bool Equals(object o)
		{
			if (!(o is PkixNameConstraintValidator))
			{
				return false;
			}
			PkixNameConstraintValidator pkixNameConstraintValidator = (PkixNameConstraintValidator)o;
			return this.CollectionsAreEqual(pkixNameConstraintValidator.excludedSubtreesDN, this.excludedSubtreesDN) && this.CollectionsAreEqual(pkixNameConstraintValidator.excludedSubtreesDNS, this.excludedSubtreesDNS) && this.CollectionsAreEqual(pkixNameConstraintValidator.excludedSubtreesEmail, this.excludedSubtreesEmail) && this.CollectionsAreEqual(pkixNameConstraintValidator.excludedSubtreesIP, this.excludedSubtreesIP) && this.CollectionsAreEqual(pkixNameConstraintValidator.excludedSubtreesURI, this.excludedSubtreesURI) && this.CollectionsAreEqual(pkixNameConstraintValidator.permittedSubtreesDN, this.permittedSubtreesDN) && this.CollectionsAreEqual(pkixNameConstraintValidator.permittedSubtreesDNS, this.permittedSubtreesDNS) && this.CollectionsAreEqual(pkixNameConstraintValidator.permittedSubtreesEmail, this.permittedSubtreesEmail) && this.CollectionsAreEqual(pkixNameConstraintValidator.permittedSubtreesIP, this.permittedSubtreesIP) && this.CollectionsAreEqual(pkixNameConstraintValidator.permittedSubtreesURI, this.permittedSubtreesURI);
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x000BFF5C File Offset: 0x000BE15C
		private bool CollectionsAreEqual(ICollection coll1, ICollection coll2)
		{
			if (coll1 == coll2)
			{
				return true;
			}
			if (coll1 == null || coll2 == null)
			{
				return false;
			}
			if (coll1.Count != coll2.Count)
			{
				return false;
			}
			foreach (object o in coll1)
			{
				IEnumerator enumerator2 = coll2.GetEnumerator();
				bool flag = false;
				while (enumerator2.MoveNext())
				{
					object o2 = enumerator2.Current;
					if (this.SpecialEquals(o, o2))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x000BFFCF File Offset: 0x000BE1CF
		private bool SpecialEquals(object o1, object o2)
		{
			if (o1 == o2)
			{
				return true;
			}
			if (o1 == null || o2 == null)
			{
				return false;
			}
			if (o1 is byte[] && o2 is byte[])
			{
				return Arrays.AreEqual((byte[])o1, (byte[])o2);
			}
			return o1.Equals(o2);
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x000C0008 File Offset: 0x000BE208
		private string StringifyIP(byte[] ip)
		{
			string text = "";
			for (int i = 0; i < ip.Length / 2; i++)
			{
				text = text + (int)(ip[i] & byte.MaxValue) + ".";
			}
			text = text.Substring(0, text.Length - 1);
			text += "/";
			for (int j = ip.Length / 2; j < ip.Length; j++)
			{
				text = text + (int)(ip[j] & byte.MaxValue) + ".";
			}
			return text.Substring(0, text.Length - 1);
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x000C00A0 File Offset: 0x000BE2A0
		private string StringifyIPCollection(ISet ips)
		{
			string text = "";
			text += "[";
			foreach (object obj in ips)
			{
				text = text + this.StringifyIP((byte[])obj) + ",";
			}
			if (text.Length > 1)
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text + "]";
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x000C0114 File Offset: 0x000BE314
		public override string ToString()
		{
			string text = "";
			text += "permitted:\n";
			if (this.permittedSubtreesDN != null)
			{
				text += "DN:\n";
				text = text + this.permittedSubtreesDN.ToString() + "\n";
			}
			if (this.permittedSubtreesDNS != null)
			{
				text += "DNS:\n";
				text = text + this.permittedSubtreesDNS.ToString() + "\n";
			}
			if (this.permittedSubtreesEmail != null)
			{
				text += "Email:\n";
				text = text + this.permittedSubtreesEmail.ToString() + "\n";
			}
			if (this.permittedSubtreesURI != null)
			{
				text += "URI:\n";
				text = text + this.permittedSubtreesURI.ToString() + "\n";
			}
			if (this.permittedSubtreesIP != null)
			{
				text += "IP:\n";
				text = text + this.StringifyIPCollection(this.permittedSubtreesIP) + "\n";
			}
			text += "excluded:\n";
			if (!this.excludedSubtreesDN.IsEmpty)
			{
				text += "DN:\n";
				text = text + this.excludedSubtreesDN.ToString() + "\n";
			}
			if (!this.excludedSubtreesDNS.IsEmpty)
			{
				text += "DNS:\n";
				text = text + this.excludedSubtreesDNS.ToString() + "\n";
			}
			if (!this.excludedSubtreesEmail.IsEmpty)
			{
				text += "Email:\n";
				text = text + this.excludedSubtreesEmail.ToString() + "\n";
			}
			if (!this.excludedSubtreesURI.IsEmpty)
			{
				text += "URI:\n";
				text = text + this.excludedSubtreesURI.ToString() + "\n";
			}
			if (!this.excludedSubtreesIP.IsEmpty)
			{
				text += "IP:\n";
				text = text + this.StringifyIPCollection(this.excludedSubtreesIP) + "\n";
			}
			return text;
		}

		// Token: 0x04001882 RID: 6274
		private ISet excludedSubtreesDN = new HashSet();

		// Token: 0x04001883 RID: 6275
		private ISet excludedSubtreesDNS = new HashSet();

		// Token: 0x04001884 RID: 6276
		private ISet excludedSubtreesEmail = new HashSet();

		// Token: 0x04001885 RID: 6277
		private ISet excludedSubtreesURI = new HashSet();

		// Token: 0x04001886 RID: 6278
		private ISet excludedSubtreesIP = new HashSet();

		// Token: 0x04001887 RID: 6279
		private ISet permittedSubtreesDN;

		// Token: 0x04001888 RID: 6280
		private ISet permittedSubtreesDNS;

		// Token: 0x04001889 RID: 6281
		private ISet permittedSubtreesEmail;

		// Token: 0x0400188A RID: 6282
		private ISet permittedSubtreesURI;

		// Token: 0x0400188B RID: 6283
		private ISet permittedSubtreesIP;
	}
}
