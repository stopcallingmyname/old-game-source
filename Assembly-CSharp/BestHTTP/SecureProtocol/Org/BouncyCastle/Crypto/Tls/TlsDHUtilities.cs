using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000462 RID: 1122
	public abstract class TlsDHUtilities
	{
		// Token: 0x06002BBF RID: 11199 RVA: 0x001168E2 File Offset: 0x00114AE2
		private static BigInteger FromHex(string hex)
		{
			return new BigInteger(1, Hex.Decode(hex));
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x001168F0 File Offset: 0x00114AF0
		private static DHParameters FromSafeP(string hexP)
		{
			BigInteger bigInteger = TlsDHUtilities.FromHex(hexP);
			BigInteger q = bigInteger.ShiftRight(1);
			return new DHParameters(bigInteger, TlsDHUtilities.Two, q);
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x00116916 File Offset: 0x00114B16
		public static void AddNegotiatedDheGroupsClientExtension(IDictionary extensions, byte[] dheGroups)
		{
			extensions[ExtensionType.negotiated_ff_dhe_groups] = TlsDHUtilities.CreateNegotiatedDheGroupsClientExtension(dheGroups);
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x0011692E File Offset: 0x00114B2E
		public static void AddNegotiatedDheGroupsServerExtension(IDictionary extensions, byte dheGroup)
		{
			extensions[ExtensionType.negotiated_ff_dhe_groups] = TlsDHUtilities.CreateNegotiatedDheGroupsServerExtension(dheGroup);
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x00116948 File Offset: 0x00114B48
		public static byte[] GetNegotiatedDheGroupsClientExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, ExtensionType.negotiated_ff_dhe_groups);
			if (extensionData != null)
			{
				return TlsDHUtilities.ReadNegotiatedDheGroupsClientExtension(extensionData);
			}
			return null;
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x0011696C File Offset: 0x00114B6C
		public static short GetNegotiatedDheGroupsServerExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, ExtensionType.negotiated_ff_dhe_groups);
			if (extensionData != null)
			{
				return (short)TlsDHUtilities.ReadNegotiatedDheGroupsServerExtension(extensionData);
			}
			return -1;
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x00116990 File Offset: 0x00114B90
		public static byte[] CreateNegotiatedDheGroupsClientExtension(byte[] dheGroups)
		{
			if (dheGroups == null || dheGroups.Length < 1 || dheGroups.Length > 255)
			{
				throw new TlsFatalAlert(80);
			}
			return TlsUtilities.EncodeUint8ArrayWithUint8Length(dheGroups);
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x001169B3 File Offset: 0x00114BB3
		public static byte[] CreateNegotiatedDheGroupsServerExtension(byte dheGroup)
		{
			return TlsUtilities.EncodeUint8(dheGroup);
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x001169BB File Offset: 0x00114BBB
		public static byte[] ReadNegotiatedDheGroupsClientExtension(byte[] extensionData)
		{
			byte[] array = TlsUtilities.DecodeUint8ArrayWithUint8Length(extensionData);
			if (array.Length < 1)
			{
				throw new TlsFatalAlert(50);
			}
			return array;
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x001169D1 File Offset: 0x00114BD1
		public static byte ReadNegotiatedDheGroupsServerExtension(byte[] extensionData)
		{
			return TlsUtilities.DecodeUint8(extensionData);
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x001169D9 File Offset: 0x00114BD9
		public static DHParameters GetParametersForDHEGroup(short dheGroup)
		{
			switch (dheGroup)
			{
			case 0:
				return TlsDHUtilities.draft_ffdhe2432;
			case 1:
				return TlsDHUtilities.draft_ffdhe3072;
			case 2:
				return TlsDHUtilities.draft_ffdhe4096;
			case 3:
				return TlsDHUtilities.draft_ffdhe6144;
			case 4:
				return TlsDHUtilities.draft_ffdhe8192;
			default:
				return null;
			}
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x00116A18 File Offset: 0x00114C18
		public static bool ContainsDheCipherSuites(int[] cipherSuites)
		{
			for (int i = 0; i < cipherSuites.Length; i++)
			{
				if (TlsDHUtilities.IsDheCipherSuite(cipherSuites[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x00116A40 File Offset: 0x00114C40
		public static bool IsDheCipherSuite(int cipherSuite)
		{
			if (cipherSuite <= 145)
			{
				if (cipherSuite <= 64)
				{
					if (cipherSuite <= 45)
					{
						switch (cipherSuite)
						{
						case 17:
						case 18:
						case 19:
						case 20:
						case 21:
						case 22:
						case 24:
						case 27:
							break;
						case 23:
						case 25:
						case 26:
							return false;
						default:
							if (cipherSuite != 45)
							{
								return false;
							}
							break;
						}
					}
					else if (cipherSuite - 50 > 2 && cipherSuite - 56 > 2 && cipherSuite != 64)
					{
						return false;
					}
				}
				else if (cipherSuite <= 103)
				{
					if (cipherSuite - 68 > 2 && cipherSuite != 103)
					{
						return false;
					}
				}
				else if (cipherSuite - 106 > 3 && cipherSuite - 135 > 2 && cipherSuite - 142 > 3)
				{
					return false;
				}
			}
			else if (cipherSuite <= 49297)
			{
				if (cipherSuite <= 191)
				{
					switch (cipherSuite)
					{
					case 153:
					case 154:
					case 155:
					case 158:
					case 159:
					case 162:
					case 163:
					case 166:
					case 167:
					case 170:
					case 171:
					case 178:
					case 179:
					case 180:
					case 181:
						break;
					case 156:
					case 157:
					case 160:
					case 161:
					case 164:
					case 165:
					case 168:
					case 169:
					case 172:
					case 173:
					case 174:
					case 175:
					case 176:
					case 177:
						return false;
					default:
						if (cipherSuite - 189 > 2)
						{
							return false;
						}
						break;
					}
				}
				else if (cipherSuite - 195 > 2)
				{
					switch (cipherSuite)
					{
					case 49276:
					case 49277:
					case 49280:
					case 49281:
					case 49284:
					case 49285:
						break;
					case 49278:
					case 49279:
					case 49282:
					case 49283:
						return false;
					default:
						if (cipherSuite - 49296 > 1)
						{
							return false;
						}
						break;
					}
				}
			}
			else if (cipherSuite <= 52394)
			{
				if (cipherSuite - 49302 > 1)
				{
					switch (cipherSuite)
					{
					case 49310:
					case 49311:
					case 49314:
					case 49315:
					case 49318:
					case 49319:
					case 49322:
					case 49323:
						break;
					case 49312:
					case 49313:
					case 49316:
					case 49317:
					case 49320:
					case 49321:
						return false;
					default:
						if (cipherSuite != 52394)
						{
							return false;
						}
						break;
					}
				}
			}
			else if (cipherSuite != 52397 && cipherSuite - 65280 > 1 && cipherSuite - 65298 > 1)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x00116C80 File Offset: 0x00114E80
		public static bool AreCompatibleParameters(DHParameters a, DHParameters b)
		{
			return a.P.Equals(b.P) && a.G.Equals(b.G) && (a.Q == null || b.Q == null || a.Q.Equals(b.Q));
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x00116CD8 File Offset: 0x00114ED8
		public static byte[] CalculateDHBasicAgreement(DHPublicKeyParameters publicKey, DHPrivateKeyParameters privateKey)
		{
			DHBasicAgreement dhbasicAgreement = new DHBasicAgreement();
			dhbasicAgreement.Init(privateKey);
			return BigIntegers.AsUnsignedByteArray(dhbasicAgreement.CalculateAgreement(publicKey));
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x00116CF1 File Offset: 0x00114EF1
		public static AsymmetricCipherKeyPair GenerateDHKeyPair(SecureRandom random, DHParameters dhParams)
		{
			DHBasicKeyPairGenerator dhbasicKeyPairGenerator = new DHBasicKeyPairGenerator();
			dhbasicKeyPairGenerator.Init(new DHKeyGenerationParameters(random, dhParams));
			return dhbasicKeyPairGenerator.GenerateKeyPair();
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x00116D0A File Offset: 0x00114F0A
		public static DHPrivateKeyParameters GenerateEphemeralClientKeyExchange(SecureRandom random, DHParameters dhParams, Stream output)
		{
			AsymmetricCipherKeyPair asymmetricCipherKeyPair = TlsDHUtilities.GenerateDHKeyPair(random, dhParams);
			TlsDHUtilities.WriteDHParameter(((DHPublicKeyParameters)asymmetricCipherKeyPair.Public).Y, output);
			return (DHPrivateKeyParameters)asymmetricCipherKeyPair.Private;
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x00116D33 File Offset: 0x00114F33
		public static DHPrivateKeyParameters GenerateEphemeralServerKeyExchange(SecureRandom random, DHParameters dhParams, Stream output)
		{
			AsymmetricCipherKeyPair asymmetricCipherKeyPair = TlsDHUtilities.GenerateDHKeyPair(random, dhParams);
			DHPublicKeyParameters dhpublicKeyParameters = (DHPublicKeyParameters)asymmetricCipherKeyPair.Public;
			TlsDHUtilities.WriteDHParameters(dhParams, output);
			TlsDHUtilities.WriteDHParameter(dhpublicKeyParameters.Y, output);
			return (DHPrivateKeyParameters)asymmetricCipherKeyPair.Private;
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x00116D63 File Offset: 0x00114F63
		public static BigInteger ReadDHParameter(Stream input)
		{
			return new BigInteger(1, TlsUtilities.ReadOpaque16(input));
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x00116D74 File Offset: 0x00114F74
		public static DHParameters ReadDHParameters(Stream input)
		{
			BigInteger p = TlsDHUtilities.ReadDHParameter(input);
			BigInteger g = TlsDHUtilities.ReadDHParameter(input);
			return new DHParameters(p, g);
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x00116D94 File Offset: 0x00114F94
		public static DHParameters ReceiveDHParameters(TlsDHVerifier dhVerifier, Stream input)
		{
			DHParameters dhparameters = TlsDHUtilities.ReadDHParameters(input);
			if (!dhVerifier.Accept(dhparameters))
			{
				throw new TlsFatalAlert(71);
			}
			return dhparameters;
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x00116DBA File Offset: 0x00114FBA
		public static void WriteDHParameter(BigInteger x, Stream output)
		{
			TlsUtilities.WriteOpaque16(BigIntegers.AsUnsignedByteArray(x), output);
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x00116DC8 File Offset: 0x00114FC8
		public static void WriteDHParameters(DHParameters dhParameters, Stream output)
		{
			TlsDHUtilities.WriteDHParameter(dhParameters.P, output);
			TlsDHUtilities.WriteDHParameter(dhParameters.G, output);
		}

		// Token: 0x04001E2E RID: 7726
		internal static readonly BigInteger Two = BigInteger.Two;

		// Token: 0x04001E2F RID: 7727
		private static readonly string draft_ffdhe2432_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE13098533C8B3FFFFFFFFFFFFFFFF";

		// Token: 0x04001E30 RID: 7728
		internal static readonly DHParameters draft_ffdhe2432 = TlsDHUtilities.FromSafeP(TlsDHUtilities.draft_ffdhe2432_p);

		// Token: 0x04001E31 RID: 7729
		private static readonly string draft_ffdhe3072_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE130985139270B4130C93BC437944F4FD4452E2D74DD364F2E21E71F54BFF5CAE82AB9C9DF69EE86D2BC522363A0DABC521979B0DEADA1DBF9A42D5C4484E0ABCD06BFA53DDEF3C1B20EE3FD59D7C25E41D2B66C62E37FFFFFFFFFFFFFFFF";

		// Token: 0x04001E32 RID: 7730
		internal static readonly DHParameters draft_ffdhe3072 = TlsDHUtilities.FromSafeP(TlsDHUtilities.draft_ffdhe3072_p);

		// Token: 0x04001E33 RID: 7731
		private static readonly string draft_ffdhe4096_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE130985139270B4130C93BC437944F4FD4452E2D74DD364F2E21E71F54BFF5CAE82AB9C9DF69EE86D2BC522363A0DABC521979B0DEADA1DBF9A42D5C4484E0ABCD06BFA53DDEF3C1B20EE3FD59D7C25E41D2B669E1EF16E6F52C3164DF4FB7930E9E4E58857B6AC7D5F42D69F6D187763CF1D5503400487F55BA57E31CC7A7135C886EFB4318AED6A1E012D9E6832A907600A918130C46DC778F971AD0038092999A333CB8B7A1A1DB93D7140003C2A4ECEA9F98D0ACC0A8291CDCEC97DCF8EC9B55A7F88A46B4DB5A851F44182E1C68A007E5E655F6AFFFFFFFFFFFFFFFF";

		// Token: 0x04001E34 RID: 7732
		internal static readonly DHParameters draft_ffdhe4096 = TlsDHUtilities.FromSafeP(TlsDHUtilities.draft_ffdhe4096_p);

		// Token: 0x04001E35 RID: 7733
		private static readonly string draft_ffdhe6144_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE130985139270B4130C93BC437944F4FD4452E2D74DD364F2E21E71F54BFF5CAE82AB9C9DF69EE86D2BC522363A0DABC521979B0DEADA1DBF9A42D5C4484E0ABCD06BFA53DDEF3C1B20EE3FD59D7C25E41D2B669E1EF16E6F52C3164DF4FB7930E9E4E58857B6AC7D5F42D69F6D187763CF1D5503400487F55BA57E31CC7A7135C886EFB4318AED6A1E012D9E6832A907600A918130C46DC778F971AD0038092999A333CB8B7A1A1DB93D7140003C2A4ECEA9F98D0ACC0A8291CDCEC97DCF8EC9B55A7F88A46B4DB5A851F44182E1C68A007E5E0DD9020BFD64B645036C7A4E677D2C38532A3A23BA4442CAF53EA63BB454329B7624C8917BDD64B1C0FD4CB38E8C334C701C3ACDAD0657FCCFEC719B1F5C3E4E46041F388147FB4CFDB477A52471F7A9A96910B855322EDB6340D8A00EF092350511E30ABEC1FFF9E3A26E7FB29F8C183023C3587E38DA0077D9B4763E4E4B94B2BBC194C6651E77CAF992EEAAC0232A281BF6B3A739C1226116820AE8DB5847A67CBEF9C9091B462D538CD72B03746AE77F5E62292C311562A846505DC82DB854338AE49F5235C95B91178CCF2DD5CACEF403EC9D1810C6272B045B3B71F9DC6B80D63FDD4A8E9ADB1E6962A69526D43161C1A41D570D7938DAD4A40E329CD0E40E65FFFFFFFFFFFFFFFF";

		// Token: 0x04001E36 RID: 7734
		internal static readonly DHParameters draft_ffdhe6144 = TlsDHUtilities.FromSafeP(TlsDHUtilities.draft_ffdhe6144_p);

		// Token: 0x04001E37 RID: 7735
		private static readonly string draft_ffdhe8192_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE130985139270B4130C93BC437944F4FD4452E2D74DD364F2E21E71F54BFF5CAE82AB9C9DF69EE86D2BC522363A0DABC521979B0DEADA1DBF9A42D5C4484E0ABCD06BFA53DDEF3C1B20EE3FD59D7C25E41D2B669E1EF16E6F52C3164DF4FB7930E9E4E58857B6AC7D5F42D69F6D187763CF1D5503400487F55BA57E31CC7A7135C886EFB4318AED6A1E012D9E6832A907600A918130C46DC778F971AD0038092999A333CB8B7A1A1DB93D7140003C2A4ECEA9F98D0ACC0A8291CDCEC97DCF8EC9B55A7F88A46B4DB5A851F44182E1C68A007E5E0DD9020BFD64B645036C7A4E677D2C38532A3A23BA4442CAF53EA63BB454329B7624C8917BDD64B1C0FD4CB38E8C334C701C3ACDAD0657FCCFEC719B1F5C3E4E46041F388147FB4CFDB477A52471F7A9A96910B855322EDB6340D8A00EF092350511E30ABEC1FFF9E3A26E7FB29F8C183023C3587E38DA0077D9B4763E4E4B94B2BBC194C6651E77CAF992EEAAC0232A281BF6B3A739C1226116820AE8DB5847A67CBEF9C9091B462D538CD72B03746AE77F5E62292C311562A846505DC82DB854338AE49F5235C95B91178CCF2DD5CACEF403EC9D1810C6272B045B3B71F9DC6B80D63FDD4A8E9ADB1E6962A69526D43161C1A41D570D7938DAD4A40E329CCFF46AAA36AD004CF600C8381E425A31D951AE64FDB23FCEC9509D43687FEB69EDD1CC5E0B8CC3BDF64B10EF86B63142A3AB8829555B2F747C932665CB2C0F1CC01BD70229388839D2AF05E454504AC78B7582822846C0BA35C35F5C59160CC046FD8251541FC68C9C86B022BB7099876A460E7451A8A93109703FEE1C217E6C3826E52C51AA691E0E423CFC99E9E31650C1217B624816CDAD9A95F9D5B8019488D9C0A0A1FE3075A577E23183F81D4A3F2FA4571EFC8CE0BA8A4FE8B6855DFE72B0A66EDED2FBABFBE58A30FAFABE1C5D71A87E2F741EF8C1FE86FEA6BBFDE530677F0D97D11D49F7A8443D0822E506A9F4614E011E2A94838FF88CD68C8BB7C5C6424CFFFFFFFFFFFFFFFF";

		// Token: 0x04001E38 RID: 7736
		internal static readonly DHParameters draft_ffdhe8192 = TlsDHUtilities.FromSafeP(TlsDHUtilities.draft_ffdhe8192_p);
	}
}
