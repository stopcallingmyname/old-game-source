using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs
{
	// Token: 0x020002CB RID: 715
	public class Pkcs12Store
	{
		// Token: 0x06001A4E RID: 6734 RVA: 0x000C4D6B File Offset: 0x000C2F6B
		private static SubjectKeyIdentifier CreateSubjectKeyID(AsymmetricKeyParameter pubKey)
		{
			return new SubjectKeyIdentifier(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pubKey));
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x000C4D78 File Offset: 0x000C2F78
		internal Pkcs12Store(DerObjectIdentifier keyAlgorithm, DerObjectIdentifier certAlgorithm, bool useDerEncoding)
		{
			this.keyAlgorithm = keyAlgorithm;
			this.certAlgorithm = certAlgorithm;
			this.useDerEncoding = useDerEncoding;
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x000C4DD7 File Offset: 0x000C2FD7
		public Pkcs12Store() : this(PkcsObjectIdentifiers.PbeWithShaAnd3KeyTripleDesCbc, PkcsObjectIdentifiers.PbewithShaAnd40BitRC2Cbc, false)
		{
		}

		// Token: 0x06001A51 RID: 6737 RVA: 0x000C4DEA File Offset: 0x000C2FEA
		public Pkcs12Store(Stream input, char[] password) : this()
		{
			this.Load(input, password);
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x000C4DFC File Offset: 0x000C2FFC
		protected virtual void LoadKeyBag(PrivateKeyInfo privKeyInfo, Asn1Set bagAttributes)
		{
			AsymmetricKeyParameter key = PrivateKeyFactory.CreateKey(privKeyInfo);
			IDictionary dictionary = Platform.CreateHashtable();
			AsymmetricKeyEntry value = new AsymmetricKeyEntry(key, dictionary);
			string text = null;
			Asn1OctetString asn1OctetString = null;
			if (bagAttributes != null)
			{
				foreach (object obj in bagAttributes)
				{
					Asn1Sequence asn1Sequence = (Asn1Sequence)obj;
					DerObjectIdentifier instance = DerObjectIdentifier.GetInstance(asn1Sequence[0]);
					Asn1Set instance2 = Asn1Set.GetInstance(asn1Sequence[1]);
					if (instance2.Count > 0)
					{
						Asn1Encodable asn1Encodable = instance2[0];
						if (dictionary.Contains(instance.Id))
						{
							if (!dictionary[instance.Id].Equals(asn1Encodable))
							{
								throw new IOException("attempt to add existing attribute with different value");
							}
						}
						else
						{
							dictionary.Add(instance.Id, asn1Encodable);
						}
						if (instance.Equals(PkcsObjectIdentifiers.Pkcs9AtFriendlyName))
						{
							text = ((DerBmpString)asn1Encodable).GetString();
							this.keys[text] = value;
						}
						else if (instance.Equals(PkcsObjectIdentifiers.Pkcs9AtLocalKeyID))
						{
							asn1OctetString = (Asn1OctetString)asn1Encodable;
						}
					}
				}
			}
			if (asn1OctetString == null)
			{
				this.unmarkedKeyEntry = value;
				return;
			}
			string text2 = Hex.ToHexString(asn1OctetString.GetOctets());
			if (text == null)
			{
				this.keys[text2] = value;
				return;
			}
			this.localIds[text] = text2;
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x000C4F60 File Offset: 0x000C3160
		protected virtual void LoadPkcs8ShroudedKeyBag(EncryptedPrivateKeyInfo encPrivKeyInfo, Asn1Set bagAttributes, char[] password, bool wrongPkcs12Zero)
		{
			if (password != null)
			{
				PrivateKeyInfo privKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(password, wrongPkcs12Zero, encPrivKeyInfo);
				this.LoadKeyBag(privKeyInfo, bagAttributes);
			}
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x000C4F84 File Offset: 0x000C3184
		public void Load(Stream input, char[] password)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			Pfx pfx = new Pfx((Asn1Sequence)Asn1Object.FromStream(input));
			ContentInfo authSafe = pfx.AuthSafe;
			bool wrongPkcs12Zero = false;
			if (password != null && pfx.MacData != null)
			{
				MacData macData = pfx.MacData;
				DigestInfo mac = macData.Mac;
				AlgorithmIdentifier algorithmID = mac.AlgorithmID;
				byte[] salt = macData.GetSalt();
				int intValue = macData.IterationCount.IntValue;
				byte[] octets = ((Asn1OctetString)authSafe.Content).GetOctets();
				byte[] a = Pkcs12Store.CalculatePbeMac(algorithmID.Algorithm, salt, intValue, password, false, octets);
				byte[] digest = mac.GetDigest();
				if (!Arrays.ConstantTimeAreEqual(a, digest))
				{
					if (password.Length != 0)
					{
						throw new IOException("PKCS12 key store MAC invalid - wrong password or corrupted file.");
					}
					if (!Arrays.ConstantTimeAreEqual(Pkcs12Store.CalculatePbeMac(algorithmID.Algorithm, salt, intValue, password, true, octets), digest))
					{
						throw new IOException("PKCS12 key store MAC invalid - wrong password or corrupted file.");
					}
					wrongPkcs12Zero = true;
				}
			}
			this.keys.Clear();
			this.localIds.Clear();
			this.unmarkedKeyEntry = null;
			IList list = Platform.CreateArrayList();
			if (authSafe.ContentType.Equals(PkcsObjectIdentifiers.Data))
			{
				foreach (ContentInfo contentInfo2 in new AuthenticatedSafe((Asn1Sequence)Asn1Object.FromByteArray(((Asn1OctetString)authSafe.Content).GetOctets())).GetContentInfo())
				{
					DerObjectIdentifier contentType = contentInfo2.ContentType;
					byte[] array = null;
					if (contentType.Equals(PkcsObjectIdentifiers.Data))
					{
						array = ((Asn1OctetString)contentInfo2.Content).GetOctets();
					}
					else if (contentType.Equals(PkcsObjectIdentifiers.EncryptedData) && password != null)
					{
						EncryptedData instance = EncryptedData.GetInstance(contentInfo2.Content);
						array = Pkcs12Store.CryptPbeData(false, instance.EncryptionAlgorithm, password, wrongPkcs12Zero, instance.Content.GetOctets());
					}
					if (array != null)
					{
						foreach (object obj in ((Asn1Sequence)Asn1Object.FromByteArray(array)))
						{
							SafeBag safeBag = new SafeBag((Asn1Sequence)obj);
							if (safeBag.BagID.Equals(PkcsObjectIdentifiers.CertBag))
							{
								list.Add(safeBag);
							}
							else if (safeBag.BagID.Equals(PkcsObjectIdentifiers.Pkcs8ShroudedKeyBag))
							{
								this.LoadPkcs8ShroudedKeyBag(EncryptedPrivateKeyInfo.GetInstance(safeBag.BagValue), safeBag.BagAttributes, password, wrongPkcs12Zero);
							}
							else if (safeBag.BagID.Equals(PkcsObjectIdentifiers.KeyBag))
							{
								this.LoadKeyBag(PrivateKeyInfo.GetInstance(safeBag.BagValue), safeBag.BagAttributes);
							}
						}
					}
				}
			}
			this.certs.Clear();
			this.chainCerts.Clear();
			this.keyCerts.Clear();
			foreach (object obj2 in list)
			{
				SafeBag safeBag2 = (SafeBag)obj2;
				byte[] octets2 = ((Asn1OctetString)new CertBag((Asn1Sequence)safeBag2.BagValue).CertValue).GetOctets();
				X509Certificate x509Certificate = new X509CertificateParser().ReadCertificate(octets2);
				IDictionary dictionary = Platform.CreateHashtable();
				Asn1OctetString asn1OctetString = null;
				string text = null;
				if (safeBag2.BagAttributes != null)
				{
					foreach (object obj3 in safeBag2.BagAttributes)
					{
						Asn1Sequence asn1Sequence = (Asn1Sequence)obj3;
						DerObjectIdentifier instance2 = DerObjectIdentifier.GetInstance(asn1Sequence[0]);
						Asn1Set instance3 = Asn1Set.GetInstance(asn1Sequence[1]);
						if (instance3.Count > 0)
						{
							Asn1Encodable asn1Encodable = instance3[0];
							if (dictionary.Contains(instance2.Id))
							{
								if (!dictionary[instance2.Id].Equals(asn1Encodable))
								{
									throw new IOException("attempt to add existing attribute with different value");
								}
							}
							else
							{
								dictionary.Add(instance2.Id, asn1Encodable);
							}
							if (instance2.Equals(PkcsObjectIdentifiers.Pkcs9AtFriendlyName))
							{
								text = ((DerBmpString)asn1Encodable).GetString();
							}
							else if (instance2.Equals(PkcsObjectIdentifiers.Pkcs9AtLocalKeyID))
							{
								asn1OctetString = (Asn1OctetString)asn1Encodable;
							}
						}
					}
				}
				Pkcs12Store.CertId certId = new Pkcs12Store.CertId(x509Certificate.GetPublicKey());
				X509CertificateEntry value = new X509CertificateEntry(x509Certificate, dictionary);
				this.chainCerts[certId] = value;
				if (this.unmarkedKeyEntry != null)
				{
					if (this.keyCerts.Count == 0)
					{
						string text2 = Hex.ToHexString(certId.Id);
						this.keyCerts[text2] = value;
						this.keys[text2] = this.unmarkedKeyEntry;
					}
					else
					{
						this.keys["unmarked"] = this.unmarkedKeyEntry;
					}
				}
				else
				{
					if (asn1OctetString != null)
					{
						string key = Hex.ToHexString(asn1OctetString.GetOctets());
						this.keyCerts[key] = value;
					}
					if (text != null)
					{
						this.certs[text] = value;
					}
				}
			}
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x000C54B8 File Offset: 0x000C36B8
		public AsymmetricKeyEntry GetKey(string alias)
		{
			if (alias == null)
			{
				throw new ArgumentNullException("alias");
			}
			return (AsymmetricKeyEntry)this.keys[alias];
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x000C54D9 File Offset: 0x000C36D9
		public bool IsCertificateEntry(string alias)
		{
			if (alias == null)
			{
				throw new ArgumentNullException("alias");
			}
			return this.certs[alias] != null && this.keys[alias] == null;
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x000C5508 File Offset: 0x000C3708
		public bool IsKeyEntry(string alias)
		{
			if (alias == null)
			{
				throw new ArgumentNullException("alias");
			}
			return this.keys[alias] != null;
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x000C5528 File Offset: 0x000C3728
		private IDictionary GetAliasesTable()
		{
			IDictionary dictionary = Platform.CreateHashtable();
			foreach (object obj in this.certs.Keys)
			{
				string key = (string)obj;
				dictionary[key] = "cert";
			}
			foreach (object obj2 in this.keys.Keys)
			{
				string key2 = (string)obj2;
				if (dictionary[key2] == null)
				{
					dictionary[key2] = "key";
				}
			}
			return dictionary;
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06001A59 RID: 6745 RVA: 0x000C55F0 File Offset: 0x000C37F0
		public IEnumerable Aliases
		{
			get
			{
				return new EnumerableProxy(this.GetAliasesTable().Keys);
			}
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x000C5602 File Offset: 0x000C3802
		public bool ContainsAlias(string alias)
		{
			return this.certs[alias] != null || this.keys[alias] != null;
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x000C5624 File Offset: 0x000C3824
		public X509CertificateEntry GetCertificate(string alias)
		{
			if (alias == null)
			{
				throw new ArgumentNullException("alias");
			}
			X509CertificateEntry x509CertificateEntry = (X509CertificateEntry)this.certs[alias];
			if (x509CertificateEntry == null)
			{
				string text = (string)this.localIds[alias];
				if (text != null)
				{
					x509CertificateEntry = (X509CertificateEntry)this.keyCerts[text];
				}
				else
				{
					x509CertificateEntry = (X509CertificateEntry)this.keyCerts[alias];
				}
			}
			return x509CertificateEntry;
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x000C5690 File Offset: 0x000C3890
		public string GetCertificateAlias(X509Certificate cert)
		{
			if (cert == null)
			{
				throw new ArgumentNullException("cert");
			}
			foreach (object obj in this.certs)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (((X509CertificateEntry)dictionaryEntry.Value).Certificate.Equals(cert))
				{
					return (string)dictionaryEntry.Key;
				}
			}
			foreach (object obj2 in this.keyCerts)
			{
				DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
				if (((X509CertificateEntry)dictionaryEntry2.Value).Certificate.Equals(cert))
				{
					return (string)dictionaryEntry2.Key;
				}
			}
			return null;
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x000C578C File Offset: 0x000C398C
		public X509CertificateEntry[] GetCertificateChain(string alias)
		{
			if (alias == null)
			{
				throw new ArgumentNullException("alias");
			}
			if (!this.IsKeyEntry(alias))
			{
				return null;
			}
			X509CertificateEntry x509CertificateEntry = this.GetCertificate(alias);
			if (x509CertificateEntry != null)
			{
				IList list = Platform.CreateArrayList();
				while (x509CertificateEntry != null)
				{
					X509Certificate certificate = x509CertificateEntry.Certificate;
					X509CertificateEntry x509CertificateEntry2 = null;
					Asn1OctetString extensionValue = certificate.GetExtensionValue(X509Extensions.AuthorityKeyIdentifier);
					if (extensionValue != null)
					{
						AuthorityKeyIdentifier instance = AuthorityKeyIdentifier.GetInstance(Asn1Object.FromByteArray(extensionValue.GetOctets()));
						if (instance.GetKeyIdentifier() != null)
						{
							x509CertificateEntry2 = (X509CertificateEntry)this.chainCerts[new Pkcs12Store.CertId(instance.GetKeyIdentifier())];
						}
					}
					if (x509CertificateEntry2 == null)
					{
						X509Name issuerDN = certificate.IssuerDN;
						X509Name subjectDN = certificate.SubjectDN;
						if (!issuerDN.Equivalent(subjectDN))
						{
							foreach (object obj in this.chainCerts.Keys)
							{
								Pkcs12Store.CertId key = (Pkcs12Store.CertId)obj;
								X509CertificateEntry x509CertificateEntry3 = (X509CertificateEntry)this.chainCerts[key];
								X509Certificate certificate2 = x509CertificateEntry3.Certificate;
								if (certificate2.SubjectDN.Equivalent(issuerDN))
								{
									try
									{
										certificate.Verify(certificate2.GetPublicKey());
										x509CertificateEntry2 = x509CertificateEntry3;
										break;
									}
									catch (InvalidKeyException)
									{
									}
								}
							}
						}
					}
					list.Add(x509CertificateEntry);
					if (x509CertificateEntry2 != x509CertificateEntry)
					{
						x509CertificateEntry = x509CertificateEntry2;
					}
					else
					{
						x509CertificateEntry = null;
					}
				}
				X509CertificateEntry[] array = new X509CertificateEntry[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					array[i] = (X509CertificateEntry)list[i];
				}
				return array;
			}
			return null;
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x000C5938 File Offset: 0x000C3B38
		public void SetCertificateEntry(string alias, X509CertificateEntry certEntry)
		{
			if (alias == null)
			{
				throw new ArgumentNullException("alias");
			}
			if (certEntry == null)
			{
				throw new ArgumentNullException("certEntry");
			}
			if (this.keys[alias] != null)
			{
				throw new ArgumentException("There is a key entry with the name " + alias + ".");
			}
			this.certs[alias] = certEntry;
			this.chainCerts[new Pkcs12Store.CertId(certEntry.Certificate.GetPublicKey())] = certEntry;
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x000C59B0 File Offset: 0x000C3BB0
		public void SetKeyEntry(string alias, AsymmetricKeyEntry keyEntry, X509CertificateEntry[] chain)
		{
			if (alias == null)
			{
				throw new ArgumentNullException("alias");
			}
			if (keyEntry == null)
			{
				throw new ArgumentNullException("keyEntry");
			}
			if (keyEntry.Key.IsPrivate && chain == null)
			{
				throw new ArgumentException("No certificate chain for private key");
			}
			if (this.keys[alias] != null)
			{
				this.DeleteEntry(alias);
			}
			this.keys[alias] = keyEntry;
			this.certs[alias] = chain[0];
			for (int num = 0; num != chain.Length; num++)
			{
				this.chainCerts[new Pkcs12Store.CertId(chain[num].Certificate.GetPublicKey())] = chain[num];
			}
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x000C5A54 File Offset: 0x000C3C54
		public void DeleteEntry(string alias)
		{
			if (alias == null)
			{
				throw new ArgumentNullException("alias");
			}
			AsymmetricKeyEntry asymmetricKeyEntry = (AsymmetricKeyEntry)this.keys[alias];
			if (asymmetricKeyEntry != null)
			{
				this.keys.Remove(alias);
			}
			X509CertificateEntry x509CertificateEntry = (X509CertificateEntry)this.certs[alias];
			if (x509CertificateEntry != null)
			{
				this.certs.Remove(alias);
				this.chainCerts.Remove(new Pkcs12Store.CertId(x509CertificateEntry.Certificate.GetPublicKey()));
			}
			if (asymmetricKeyEntry != null)
			{
				string text = (string)this.localIds[alias];
				if (text != null)
				{
					this.localIds.Remove(alias);
					x509CertificateEntry = (X509CertificateEntry)this.keyCerts[text];
				}
				if (x509CertificateEntry != null)
				{
					this.keyCerts.Remove(text);
					this.chainCerts.Remove(new Pkcs12Store.CertId(x509CertificateEntry.Certificate.GetPublicKey()));
				}
			}
			if (x509CertificateEntry == null && asymmetricKeyEntry == null)
			{
				throw new ArgumentException("no such entry as " + alias);
			}
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x000C5B48 File Offset: 0x000C3D48
		public bool IsEntryOfType(string alias, Type entryType)
		{
			if (entryType == typeof(X509CertificateEntry))
			{
				return this.IsCertificateEntry(alias);
			}
			return entryType == typeof(AsymmetricKeyEntry) && this.IsKeyEntry(alias) && this.GetCertificate(alias) != null;
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x000C5B98 File Offset: 0x000C3D98
		[Obsolete("Use 'Count' property instead")]
		public int Size()
		{
			return this.Count;
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06001A63 RID: 6755 RVA: 0x000C5BA0 File Offset: 0x000C3DA0
		public int Count
		{
			get
			{
				return this.GetAliasesTable().Count;
			}
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x000C5BB0 File Offset: 0x000C3DB0
		public void Save(Stream stream, char[] password, SecureRandom random)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (random == null)
			{
				throw new ArgumentNullException("random");
			}
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in this.keys.Keys)
			{
				string text = (string)obj;
				byte[] array = new byte[20];
				random.NextBytes(array);
				AsymmetricKeyEntry asymmetricKeyEntry = (AsymmetricKeyEntry)this.keys[text];
				DerObjectIdentifier oid;
				Asn1Encodable asn1Encodable;
				if (password == null)
				{
					oid = PkcsObjectIdentifiers.KeyBag;
					asn1Encodable = PrivateKeyInfoFactory.CreatePrivateKeyInfo(asymmetricKeyEntry.Key);
				}
				else
				{
					oid = PkcsObjectIdentifiers.Pkcs8ShroudedKeyBag;
					asn1Encodable = EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(this.keyAlgorithm, password, array, 1024, asymmetricKeyEntry.Key);
				}
				Asn1EncodableVector asn1EncodableVector2 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
				foreach (object obj2 in asymmetricKeyEntry.BagAttributeKeys)
				{
					string text2 = (string)obj2;
					Asn1Encodable obj3 = asymmetricKeyEntry[text2];
					if (!text2.Equals(PkcsObjectIdentifiers.Pkcs9AtFriendlyName.Id))
					{
						asn1EncodableVector2.Add(new Asn1Encodable[]
						{
							new DerSequence(new Asn1Encodable[]
							{
								new DerObjectIdentifier(text2),
								new DerSet(obj3)
							})
						});
					}
				}
				asn1EncodableVector2.Add(new Asn1Encodable[]
				{
					new DerSequence(new Asn1Encodable[]
					{
						PkcsObjectIdentifiers.Pkcs9AtFriendlyName,
						new DerSet(new DerBmpString(text))
					})
				});
				if (asymmetricKeyEntry[PkcsObjectIdentifiers.Pkcs9AtLocalKeyID] == null)
				{
					SubjectKeyIdentifier obj4 = Pkcs12Store.CreateSubjectKeyID(this.GetCertificate(text).Certificate.GetPublicKey());
					asn1EncodableVector2.Add(new Asn1Encodable[]
					{
						new DerSequence(new Asn1Encodable[]
						{
							PkcsObjectIdentifiers.Pkcs9AtLocalKeyID,
							new DerSet(obj4)
						})
					});
				}
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new SafeBag(oid, asn1Encodable.ToAsn1Object(), new DerSet(asn1EncodableVector2))
				});
			}
			byte[] derEncoded = new DerSequence(asn1EncodableVector).GetDerEncoded();
			ContentInfo contentInfo = new ContentInfo(PkcsObjectIdentifiers.Data, new BerOctetString(derEncoded));
			byte[] array2 = new byte[20];
			random.NextBytes(array2);
			Asn1EncodableVector asn1EncodableVector3 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			Pkcs12PbeParams pkcs12PbeParams = new Pkcs12PbeParams(array2, 1024);
			AlgorithmIdentifier algorithmIdentifier = new AlgorithmIdentifier(this.certAlgorithm, pkcs12PbeParams.ToAsn1Object());
			ISet set = new HashSet();
			foreach (object obj5 in this.keys.Keys)
			{
				string text3 = (string)obj5;
				X509CertificateEntry certificate = this.GetCertificate(text3);
				CertBag certBag = new CertBag(PkcsObjectIdentifiers.X509Certificate, new DerOctetString(certificate.Certificate.GetEncoded()));
				Asn1EncodableVector asn1EncodableVector4 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
				foreach (object obj6 in certificate.BagAttributeKeys)
				{
					string text4 = (string)obj6;
					Asn1Encodable obj7 = certificate[text4];
					if (!text4.Equals(PkcsObjectIdentifiers.Pkcs9AtFriendlyName.Id))
					{
						asn1EncodableVector4.Add(new Asn1Encodable[]
						{
							new DerSequence(new Asn1Encodable[]
							{
								new DerObjectIdentifier(text4),
								new DerSet(obj7)
							})
						});
					}
				}
				asn1EncodableVector4.Add(new Asn1Encodable[]
				{
					new DerSequence(new Asn1Encodable[]
					{
						PkcsObjectIdentifiers.Pkcs9AtFriendlyName,
						new DerSet(new DerBmpString(text3))
					})
				});
				if (certificate[PkcsObjectIdentifiers.Pkcs9AtLocalKeyID] == null)
				{
					SubjectKeyIdentifier obj8 = Pkcs12Store.CreateSubjectKeyID(certificate.Certificate.GetPublicKey());
					asn1EncodableVector4.Add(new Asn1Encodable[]
					{
						new DerSequence(new Asn1Encodable[]
						{
							PkcsObjectIdentifiers.Pkcs9AtLocalKeyID,
							new DerSet(obj8)
						})
					});
				}
				asn1EncodableVector3.Add(new Asn1Encodable[]
				{
					new SafeBag(PkcsObjectIdentifiers.CertBag, certBag.ToAsn1Object(), new DerSet(asn1EncodableVector4))
				});
				set.Add(certificate.Certificate);
			}
			foreach (object obj9 in this.certs.Keys)
			{
				string text5 = (string)obj9;
				X509CertificateEntry x509CertificateEntry = (X509CertificateEntry)this.certs[text5];
				if (this.keys[text5] == null)
				{
					CertBag certBag2 = new CertBag(PkcsObjectIdentifiers.X509Certificate, new DerOctetString(x509CertificateEntry.Certificate.GetEncoded()));
					Asn1EncodableVector asn1EncodableVector5 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
					foreach (object obj10 in x509CertificateEntry.BagAttributeKeys)
					{
						string text6 = (string)obj10;
						if (!text6.Equals(PkcsObjectIdentifiers.Pkcs9AtLocalKeyID.Id))
						{
							Asn1Encodable obj11 = x509CertificateEntry[text6];
							if (!text6.Equals(PkcsObjectIdentifiers.Pkcs9AtFriendlyName.Id))
							{
								asn1EncodableVector5.Add(new Asn1Encodable[]
								{
									new DerSequence(new Asn1Encodable[]
									{
										new DerObjectIdentifier(text6),
										new DerSet(obj11)
									})
								});
							}
						}
					}
					asn1EncodableVector5.Add(new Asn1Encodable[]
					{
						new DerSequence(new Asn1Encodable[]
						{
							PkcsObjectIdentifiers.Pkcs9AtFriendlyName,
							new DerSet(new DerBmpString(text5))
						})
					});
					asn1EncodableVector3.Add(new Asn1Encodable[]
					{
						new SafeBag(PkcsObjectIdentifiers.CertBag, certBag2.ToAsn1Object(), new DerSet(asn1EncodableVector5))
					});
					set.Add(x509CertificateEntry.Certificate);
				}
			}
			foreach (object obj12 in this.chainCerts.Keys)
			{
				Pkcs12Store.CertId key = (Pkcs12Store.CertId)obj12;
				X509CertificateEntry x509CertificateEntry2 = (X509CertificateEntry)this.chainCerts[key];
				if (!set.Contains(x509CertificateEntry2.Certificate))
				{
					CertBag certBag3 = new CertBag(PkcsObjectIdentifiers.X509Certificate, new DerOctetString(x509CertificateEntry2.Certificate.GetEncoded()));
					Asn1EncodableVector asn1EncodableVector6 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
					foreach (object obj13 in x509CertificateEntry2.BagAttributeKeys)
					{
						string text7 = (string)obj13;
						if (!text7.Equals(PkcsObjectIdentifiers.Pkcs9AtLocalKeyID.Id))
						{
							asn1EncodableVector6.Add(new Asn1Encodable[]
							{
								new DerSequence(new Asn1Encodable[]
								{
									new DerObjectIdentifier(text7),
									new DerSet(x509CertificateEntry2[text7])
								})
							});
						}
					}
					asn1EncodableVector3.Add(new Asn1Encodable[]
					{
						new SafeBag(PkcsObjectIdentifiers.CertBag, certBag3.ToAsn1Object(), new DerSet(asn1EncodableVector6))
					});
				}
			}
			byte[] derEncoded2 = new DerSequence(asn1EncodableVector3).GetDerEncoded();
			ContentInfo contentInfo2;
			if (password == null)
			{
				contentInfo2 = new ContentInfo(PkcsObjectIdentifiers.Data, new BerOctetString(derEncoded2));
			}
			else
			{
				byte[] str = Pkcs12Store.CryptPbeData(true, algorithmIdentifier, password, false, derEncoded2);
				EncryptedData encryptedData = new EncryptedData(PkcsObjectIdentifiers.Data, algorithmIdentifier, new BerOctetString(str));
				contentInfo2 = new ContentInfo(PkcsObjectIdentifiers.EncryptedData, encryptedData.ToAsn1Object());
			}
			byte[] encoded = new AuthenticatedSafe(new ContentInfo[]
			{
				contentInfo,
				contentInfo2
			}).GetEncoded(this.useDerEncoding ? "DER" : "BER");
			ContentInfo contentInfo3 = new ContentInfo(PkcsObjectIdentifiers.Data, new BerOctetString(encoded));
			MacData macData = null;
			if (password != null)
			{
				byte[] array3 = new byte[20];
				random.NextBytes(array3);
				byte[] digest = Pkcs12Store.CalculatePbeMac(OiwObjectIdentifiers.IdSha1, array3, 1024, password, false, encoded);
				macData = new MacData(new DigestInfo(new AlgorithmIdentifier(OiwObjectIdentifiers.IdSha1, DerNull.Instance), digest), array3, 1024);
			}
			Pfx obj14 = new Pfx(contentInfo3, macData);
			DerOutputStream derOutputStream;
			if (this.useDerEncoding)
			{
				derOutputStream = new DerOutputStream(stream);
			}
			else
			{
				derOutputStream = new BerOutputStream(stream);
			}
			derOutputStream.WriteObject(obj14);
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x000C64B0 File Offset: 0x000C46B0
		internal static byte[] CalculatePbeMac(DerObjectIdentifier oid, byte[] salt, int itCount, char[] password, bool wrongPkcs12Zero, byte[] data)
		{
			Asn1Encodable pbeParameters = PbeUtilities.GenerateAlgorithmParameters(oid, salt, itCount);
			ICipherParameters parameters = PbeUtilities.GenerateCipherParameters(oid, password, wrongPkcs12Zero, pbeParameters);
			IMac mac = (IMac)PbeUtilities.CreateEngine(oid);
			mac.Init(parameters);
			return MacUtilities.DoFinal(mac, data);
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x000C64EC File Offset: 0x000C46EC
		private static byte[] CryptPbeData(bool forEncryption, AlgorithmIdentifier algId, char[] password, bool wrongPkcs12Zero, byte[] data)
		{
			IBufferedCipher bufferedCipher = PbeUtilities.CreateEngine(algId.Algorithm) as IBufferedCipher;
			if (bufferedCipher == null)
			{
				throw new Exception("Unknown encryption algorithm: " + algId.Algorithm);
			}
			Pkcs12PbeParams instance = Pkcs12PbeParams.GetInstance(algId.Parameters);
			ICipherParameters parameters = PbeUtilities.GenerateCipherParameters(algId.Algorithm, password, wrongPkcs12Zero, instance);
			bufferedCipher.Init(forEncryption, parameters);
			return bufferedCipher.DoFinal(data);
		}

		// Token: 0x040018BD RID: 6333
		private readonly Pkcs12Store.IgnoresCaseHashtable keys = new Pkcs12Store.IgnoresCaseHashtable();

		// Token: 0x040018BE RID: 6334
		private readonly IDictionary localIds = Platform.CreateHashtable();

		// Token: 0x040018BF RID: 6335
		private readonly Pkcs12Store.IgnoresCaseHashtable certs = new Pkcs12Store.IgnoresCaseHashtable();

		// Token: 0x040018C0 RID: 6336
		private readonly IDictionary chainCerts = Platform.CreateHashtable();

		// Token: 0x040018C1 RID: 6337
		private readonly IDictionary keyCerts = Platform.CreateHashtable();

		// Token: 0x040018C2 RID: 6338
		private readonly DerObjectIdentifier keyAlgorithm;

		// Token: 0x040018C3 RID: 6339
		private readonly DerObjectIdentifier certAlgorithm;

		// Token: 0x040018C4 RID: 6340
		private readonly bool useDerEncoding;

		// Token: 0x040018C5 RID: 6341
		private AsymmetricKeyEntry unmarkedKeyEntry;

		// Token: 0x040018C6 RID: 6342
		private const int MinIterations = 1024;

		// Token: 0x040018C7 RID: 6343
		private const int SaltSize = 20;

		// Token: 0x020008FF RID: 2303
		internal class CertId
		{
			// Token: 0x06004E24 RID: 20004 RVA: 0x001B0D6C File Offset: 0x001AEF6C
			internal CertId(AsymmetricKeyParameter pubKey)
			{
				this.id = Pkcs12Store.CreateSubjectKeyID(pubKey).GetKeyIdentifier();
			}

			// Token: 0x06004E25 RID: 20005 RVA: 0x001B0D85 File Offset: 0x001AEF85
			internal CertId(byte[] id)
			{
				this.id = id;
			}

			// Token: 0x17000C26 RID: 3110
			// (get) Token: 0x06004E26 RID: 20006 RVA: 0x001B0D94 File Offset: 0x001AEF94
			internal byte[] Id
			{
				get
				{
					return this.id;
				}
			}

			// Token: 0x06004E27 RID: 20007 RVA: 0x001B0D9C File Offset: 0x001AEF9C
			public override int GetHashCode()
			{
				return Arrays.GetHashCode(this.id);
			}

			// Token: 0x06004E28 RID: 20008 RVA: 0x001B0DAC File Offset: 0x001AEFAC
			public override bool Equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				Pkcs12Store.CertId certId = obj as Pkcs12Store.CertId;
				return certId != null && Arrays.AreEqual(this.id, certId.id);
			}

			// Token: 0x040034B1 RID: 13489
			private readonly byte[] id;
		}

		// Token: 0x02000900 RID: 2304
		private class IgnoresCaseHashtable : IEnumerable
		{
			// Token: 0x06004E29 RID: 20009 RVA: 0x001B0DDC File Offset: 0x001AEFDC
			public void Clear()
			{
				this.orig.Clear();
				this.keys.Clear();
			}

			// Token: 0x06004E2A RID: 20010 RVA: 0x001B0DF4 File Offset: 0x001AEFF4
			public IEnumerator GetEnumerator()
			{
				return this.orig.GetEnumerator();
			}

			// Token: 0x17000C27 RID: 3111
			// (get) Token: 0x06004E2B RID: 20011 RVA: 0x001B0E01 File Offset: 0x001AF001
			public ICollection Keys
			{
				get
				{
					return this.orig.Keys;
				}
			}

			// Token: 0x06004E2C RID: 20012 RVA: 0x001B0E10 File Offset: 0x001AF010
			public object Remove(string alias)
			{
				string key = Platform.ToUpperInvariant(alias);
				string text = (string)this.keys[key];
				if (text == null)
				{
					return null;
				}
				this.keys.Remove(key);
				object result = this.orig[text];
				this.orig.Remove(text);
				return result;
			}

			// Token: 0x17000C28 RID: 3112
			public object this[string alias]
			{
				get
				{
					string key = Platform.ToUpperInvariant(alias);
					string text = (string)this.keys[key];
					if (text == null)
					{
						return null;
					}
					return this.orig[text];
				}
				set
				{
					string key = Platform.ToUpperInvariant(alias);
					string text = (string)this.keys[key];
					if (text != null)
					{
						this.orig.Remove(text);
					}
					this.keys[key] = alias;
					this.orig[alias] = value;
				}
			}

			// Token: 0x17000C29 RID: 3113
			// (get) Token: 0x06004E2F RID: 20015 RVA: 0x001B0EE7 File Offset: 0x001AF0E7
			public ICollection Values
			{
				get
				{
					return this.orig.Values;
				}
			}

			// Token: 0x040034B2 RID: 13490
			private readonly IDictionary orig = Platform.CreateHashtable();

			// Token: 0x040034B3 RID: 13491
			private readonly IDictionary keys = Platform.CreateHashtable();
		}
	}
}
