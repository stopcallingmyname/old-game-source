using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000466 RID: 1126
	public abstract class TlsEccUtilities
	{
		// Token: 0x06002BE6 RID: 11238 RVA: 0x00116FC1 File Offset: 0x001151C1
		public static void AddSupportedEllipticCurvesExtension(IDictionary extensions, int[] namedCurves)
		{
			extensions[10] = TlsEccUtilities.CreateSupportedEllipticCurvesExtension(namedCurves);
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x00116FD6 File Offset: 0x001151D6
		public static void AddSupportedPointFormatsExtension(IDictionary extensions, byte[] ecPointFormats)
		{
			extensions[11] = TlsEccUtilities.CreateSupportedPointFormatsExtension(ecPointFormats);
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x00116FEC File Offset: 0x001151EC
		public static int[] GetSupportedEllipticCurvesExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 10);
			if (extensionData != null)
			{
				return TlsEccUtilities.ReadSupportedEllipticCurvesExtension(extensionData);
			}
			return null;
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x00117010 File Offset: 0x00115210
		public static byte[] GetSupportedPointFormatsExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 11);
			if (extensionData != null)
			{
				return TlsEccUtilities.ReadSupportedPointFormatsExtension(extensionData);
			}
			return null;
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x00117031 File Offset: 0x00115231
		public static byte[] CreateSupportedEllipticCurvesExtension(int[] namedCurves)
		{
			if (namedCurves == null || namedCurves.Length < 1)
			{
				throw new TlsFatalAlert(80);
			}
			return TlsUtilities.EncodeUint16ArrayWithUint16Length(namedCurves);
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x0011704A File Offset: 0x0011524A
		public static byte[] CreateSupportedPointFormatsExtension(byte[] ecPointFormats)
		{
			if (ecPointFormats == null || !Arrays.Contains(ecPointFormats, 0))
			{
				ecPointFormats = Arrays.Append(ecPointFormats, 0);
			}
			return TlsUtilities.EncodeUint8ArrayWithUint8Length(ecPointFormats);
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x00117068 File Offset: 0x00115268
		public static int[] ReadSupportedEllipticCurvesExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			MemoryStream memoryStream = new MemoryStream(extensionData, false);
			int num = TlsUtilities.ReadUint16(memoryStream);
			if (num < 2 || (num & 1) != 0)
			{
				throw new TlsFatalAlert(50);
			}
			int[] result = TlsUtilities.ReadUint16Array(num / 2, memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			return result;
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x001170B2 File Offset: 0x001152B2
		public static byte[] ReadSupportedPointFormatsExtension(byte[] extensionData)
		{
			byte[] array = TlsUtilities.DecodeUint8ArrayWithUint8Length(extensionData);
			if (!Arrays.Contains(array, 0))
			{
				throw new TlsFatalAlert(47);
			}
			return array;
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x001170CB File Offset: 0x001152CB
		public static string GetNameOfNamedCurve(int namedCurve)
		{
			if (!TlsEccUtilities.IsSupportedNamedCurve(namedCurve))
			{
				return null;
			}
			return TlsEccUtilities.CurveNames[namedCurve - 1];
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x001170E0 File Offset: 0x001152E0
		public static ECDomainParameters GetParametersForNamedCurve(int namedCurve)
		{
			string nameOfNamedCurve = TlsEccUtilities.GetNameOfNamedCurve(namedCurve);
			if (nameOfNamedCurve == null)
			{
				return null;
			}
			X9ECParameters byName = CustomNamedCurves.GetByName(nameOfNamedCurve);
			if (byName == null)
			{
				byName = ECNamedCurveTable.GetByName(nameOfNamedCurve);
				if (byName == null)
				{
					return null;
				}
			}
			return new ECDomainParameters(byName.Curve, byName.G, byName.N, byName.H, byName.GetSeed());
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x00117132 File Offset: 0x00115332
		public static bool HasAnySupportedNamedCurves()
		{
			return TlsEccUtilities.CurveNames.Length != 0;
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x00117140 File Offset: 0x00115340
		public static bool ContainsEccCipherSuites(int[] cipherSuites)
		{
			for (int i = 0; i < cipherSuites.Length; i++)
			{
				if (TlsEccUtilities.IsEccCipherSuite(cipherSuites[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x00117168 File Offset: 0x00115368
		public static bool IsEccCipherSuite(int cipherSuite)
		{
			if (cipherSuite <= 49307)
			{
				if (cipherSuite <= 49211)
				{
					if (cipherSuite - 49153 > 24 && cipherSuite - 49187 > 24)
					{
						return false;
					}
				}
				else if (cipherSuite - 49266 > 7 && cipherSuite - 49286 > 7 && cipherSuite - 49306 > 1)
				{
					return false;
				}
			}
			else if (cipherSuite <= 52393)
			{
				if (cipherSuite - 49324 > 3 && cipherSuite - 52392 > 1)
				{
					return false;
				}
			}
			else if (cipherSuite != 52396 && cipherSuite - 65282 > 3 && cipherSuite - 65300 > 1)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x001171FA File Offset: 0x001153FA
		public static bool AreOnSameCurve(ECDomainParameters a, ECDomainParameters b)
		{
			return a != null && a.Equals(b);
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x00117208 File Offset: 0x00115408
		public static bool IsSupportedNamedCurve(int namedCurve)
		{
			return namedCurve > 0 && namedCurve <= TlsEccUtilities.CurveNames.Length;
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x00117220 File Offset: 0x00115420
		public static bool IsCompressionPreferred(byte[] ecPointFormats, byte compressionFormat)
		{
			if (ecPointFormats == null)
			{
				return false;
			}
			foreach (byte b in ecPointFormats)
			{
				if (b == 0)
				{
					return false;
				}
				if (b == compressionFormat)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x00117250 File Offset: 0x00115450
		public static byte[] SerializeECFieldElement(int fieldSize, BigInteger x)
		{
			return BigIntegers.AsUnsignedByteArray((fieldSize + 7) / 8, x);
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x00117260 File Offset: 0x00115460
		public static byte[] SerializeECPoint(byte[] ecPointFormats, ECPoint point)
		{
			ECCurve curve = point.Curve;
			bool compressed = false;
			if (ECAlgorithms.IsFpCurve(curve))
			{
				compressed = TlsEccUtilities.IsCompressionPreferred(ecPointFormats, 1);
			}
			else if (ECAlgorithms.IsF2mCurve(curve))
			{
				compressed = TlsEccUtilities.IsCompressionPreferred(ecPointFormats, 2);
			}
			return point.GetEncoded(compressed);
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x0011729F File Offset: 0x0011549F
		public static byte[] SerializeECPublicKey(byte[] ecPointFormats, ECPublicKeyParameters keyParameters)
		{
			return TlsEccUtilities.SerializeECPoint(ecPointFormats, keyParameters.Q);
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x001172B0 File Offset: 0x001154B0
		public static BigInteger DeserializeECFieldElement(int fieldSize, byte[] encoding)
		{
			int num = (fieldSize + 7) / 8;
			if (encoding.Length != num)
			{
				throw new TlsFatalAlert(50);
			}
			return new BigInteger(1, encoding);
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x001172D8 File Offset: 0x001154D8
		public static ECPoint DeserializeECPoint(byte[] ecPointFormats, ECCurve curve, byte[] encoding)
		{
			if (encoding == null || encoding.Length < 1)
			{
				throw new TlsFatalAlert(47);
			}
			byte b;
			switch (encoding[0])
			{
			case 2:
			case 3:
				if (ECAlgorithms.IsF2mCurve(curve))
				{
					b = 2;
					goto IL_69;
				}
				if (ECAlgorithms.IsFpCurve(curve))
				{
					b = 1;
					goto IL_69;
				}
				throw new TlsFatalAlert(47);
			case 4:
				b = 0;
				goto IL_69;
			}
			throw new TlsFatalAlert(47);
			IL_69:
			if (b != 0 && (ecPointFormats == null || !Arrays.Contains(ecPointFormats, b)))
			{
				throw new TlsFatalAlert(47);
			}
			return curve.DecodePoint(encoding);
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x0011736C File Offset: 0x0011556C
		public static ECPublicKeyParameters DeserializeECPublicKey(byte[] ecPointFormats, ECDomainParameters curve_params, byte[] encoding)
		{
			ECPublicKeyParameters result;
			try
			{
				result = new ECPublicKeyParameters(TlsEccUtilities.DeserializeECPoint(ecPointFormats, curve_params.Curve, encoding), curve_params);
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(47, alertCause);
			}
			return result;
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x001173AC File Offset: 0x001155AC
		public static byte[] CalculateECDHBasicAgreement(ECPublicKeyParameters publicKey, ECPrivateKeyParameters privateKey)
		{
			ECDHBasicAgreement ecdhbasicAgreement = new ECDHBasicAgreement();
			ecdhbasicAgreement.Init(privateKey);
			BigInteger n = ecdhbasicAgreement.CalculateAgreement(publicKey);
			return BigIntegers.AsUnsignedByteArray(ecdhbasicAgreement.GetFieldSize(), n);
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x001173D8 File Offset: 0x001155D8
		public static AsymmetricCipherKeyPair GenerateECKeyPair(SecureRandom random, ECDomainParameters ecParams)
		{
			ECKeyPairGenerator eckeyPairGenerator = new ECKeyPairGenerator();
			eckeyPairGenerator.Init(new ECKeyGenerationParameters(ecParams, random));
			return eckeyPairGenerator.GenerateKeyPair();
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x001173F4 File Offset: 0x001155F4
		public static ECPrivateKeyParameters GenerateEphemeralClientKeyExchange(SecureRandom random, byte[] ecPointFormats, ECDomainParameters ecParams, Stream output)
		{
			AsymmetricCipherKeyPair asymmetricCipherKeyPair = TlsEccUtilities.GenerateECKeyPair(random, ecParams);
			ECPublicKeyParameters ecpublicKeyParameters = (ECPublicKeyParameters)asymmetricCipherKeyPair.Public;
			TlsEccUtilities.WriteECPoint(ecPointFormats, ecpublicKeyParameters.Q, output);
			return (ECPrivateKeyParameters)asymmetricCipherKeyPair.Private;
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x0011742C File Offset: 0x0011562C
		internal static ECPrivateKeyParameters GenerateEphemeralServerKeyExchange(SecureRandom random, int[] namedCurves, byte[] ecPointFormats, Stream output)
		{
			int num = -1;
			if (namedCurves == null)
			{
				num = 23;
			}
			else
			{
				foreach (int num2 in namedCurves)
				{
					if (NamedCurve.IsValid(num2) && TlsEccUtilities.IsSupportedNamedCurve(num2))
					{
						num = num2;
						break;
					}
				}
			}
			ECDomainParameters ecdomainParameters = null;
			if (num >= 0)
			{
				ecdomainParameters = TlsEccUtilities.GetParametersForNamedCurve(num);
			}
			else if (Arrays.Contains(namedCurves, 65281))
			{
				ecdomainParameters = TlsEccUtilities.GetParametersForNamedCurve(23);
			}
			else if (Arrays.Contains(namedCurves, 65282))
			{
				ecdomainParameters = TlsEccUtilities.GetParametersForNamedCurve(10);
			}
			if (ecdomainParameters == null)
			{
				throw new TlsFatalAlert(80);
			}
			if (num < 0)
			{
				TlsEccUtilities.WriteExplicitECParameters(ecPointFormats, ecdomainParameters, output);
			}
			else
			{
				TlsEccUtilities.WriteNamedECParameters(num, output);
			}
			return TlsEccUtilities.GenerateEphemeralClientKeyExchange(random, ecPointFormats, ecdomainParameters, output);
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x000947CE File Offset: 0x000929CE
		public static ECPublicKeyParameters ValidateECPublicKey(ECPublicKeyParameters key)
		{
			return key;
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x001174D0 File Offset: 0x001156D0
		public static int ReadECExponent(int fieldSize, Stream input)
		{
			BigInteger bigInteger = TlsEccUtilities.ReadECParameter(input);
			if (bigInteger.BitLength < 32)
			{
				int intValue = bigInteger.IntValue;
				if (intValue > 0 && intValue < fieldSize)
				{
					return intValue;
				}
			}
			throw new TlsFatalAlert(47);
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x00117506 File Offset: 0x00115706
		public static BigInteger ReadECFieldElement(int fieldSize, Stream input)
		{
			return TlsEccUtilities.DeserializeECFieldElement(fieldSize, TlsUtilities.ReadOpaque8(input));
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x00117514 File Offset: 0x00115714
		public static BigInteger ReadECParameter(Stream input)
		{
			return new BigInteger(1, TlsUtilities.ReadOpaque8(input));
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x00117524 File Offset: 0x00115724
		public static ECDomainParameters ReadECParameters(int[] namedCurves, byte[] ecPointFormats, Stream input)
		{
			ECDomainParameters result;
			try
			{
				switch (TlsUtilities.ReadUint8(input))
				{
				case 1:
				{
					TlsEccUtilities.CheckNamedCurve(namedCurves, 65281);
					BigInteger bigInteger = TlsEccUtilities.ReadECParameter(input);
					BigInteger a = TlsEccUtilities.ReadECFieldElement(bigInteger.BitLength, input);
					BigInteger b = TlsEccUtilities.ReadECFieldElement(bigInteger.BitLength, input);
					byte[] encoding = TlsUtilities.ReadOpaque8(input);
					BigInteger bigInteger2 = TlsEccUtilities.ReadECParameter(input);
					BigInteger bigInteger3 = TlsEccUtilities.ReadECParameter(input);
					ECCurve curve = new FpCurve(bigInteger, a, b, bigInteger2, bigInteger3);
					ECPoint g = TlsEccUtilities.DeserializeECPoint(ecPointFormats, curve, encoding);
					result = new ECDomainParameters(curve, g, bigInteger2, bigInteger3);
					break;
				}
				case 2:
				{
					TlsEccUtilities.CheckNamedCurve(namedCurves, 65282);
					int num = TlsUtilities.ReadUint16(input);
					byte b2 = TlsUtilities.ReadUint8(input);
					if (!ECBasisType.IsValid(b2))
					{
						throw new TlsFatalAlert(47);
					}
					int num2 = TlsEccUtilities.ReadECExponent(num, input);
					int k = -1;
					int k2 = -1;
					if (b2 == 2)
					{
						k = TlsEccUtilities.ReadECExponent(num, input);
						k2 = TlsEccUtilities.ReadECExponent(num, input);
					}
					BigInteger a2 = TlsEccUtilities.ReadECFieldElement(num, input);
					BigInteger b3 = TlsEccUtilities.ReadECFieldElement(num, input);
					byte[] encoding2 = TlsUtilities.ReadOpaque8(input);
					BigInteger bigInteger4 = TlsEccUtilities.ReadECParameter(input);
					BigInteger bigInteger5 = TlsEccUtilities.ReadECParameter(input);
					ECCurve curve2 = (b2 == 2) ? new F2mCurve(num, num2, k, k2, a2, b3, bigInteger4, bigInteger5) : new F2mCurve(num, num2, a2, b3, bigInteger4, bigInteger5);
					ECPoint g2 = TlsEccUtilities.DeserializeECPoint(ecPointFormats, curve2, encoding2);
					result = new ECDomainParameters(curve2, g2, bigInteger4, bigInteger5);
					break;
				}
				case 3:
				{
					int namedCurve = TlsUtilities.ReadUint16(input);
					if (!NamedCurve.RefersToASpecificNamedCurve(namedCurve))
					{
						throw new TlsFatalAlert(47);
					}
					TlsEccUtilities.CheckNamedCurve(namedCurves, namedCurve);
					result = TlsEccUtilities.GetParametersForNamedCurve(namedCurve);
					break;
				}
				default:
					throw new TlsFatalAlert(47);
				}
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(47, alertCause);
			}
			return result;
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x001176E4 File Offset: 0x001158E4
		private static void CheckNamedCurve(int[] namedCurves, int namedCurve)
		{
			if (namedCurves != null && !Arrays.Contains(namedCurves, namedCurve))
			{
				throw new TlsFatalAlert(47);
			}
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x001176FA File Offset: 0x001158FA
		public static void WriteECExponent(int k, Stream output)
		{
			TlsEccUtilities.WriteECParameter(BigInteger.ValueOf((long)k), output);
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x00117709 File Offset: 0x00115909
		public static void WriteECFieldElement(ECFieldElement x, Stream output)
		{
			TlsUtilities.WriteOpaque8(x.GetEncoded(), output);
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x00117717 File Offset: 0x00115917
		public static void WriteECFieldElement(int fieldSize, BigInteger x, Stream output)
		{
			TlsUtilities.WriteOpaque8(TlsEccUtilities.SerializeECFieldElement(fieldSize, x), output);
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x00117726 File Offset: 0x00115926
		public static void WriteECParameter(BigInteger x, Stream output)
		{
			TlsUtilities.WriteOpaque8(BigIntegers.AsUnsignedByteArray(x), output);
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x00117734 File Offset: 0x00115934
		public static void WriteExplicitECParameters(byte[] ecPointFormats, ECDomainParameters ecParameters, Stream output)
		{
			ECCurve curve = ecParameters.Curve;
			if (ECAlgorithms.IsFpCurve(curve))
			{
				TlsUtilities.WriteUint8(1, output);
				TlsEccUtilities.WriteECParameter(curve.Field.Characteristic, output);
			}
			else
			{
				if (!ECAlgorithms.IsF2mCurve(curve))
				{
					throw new ArgumentException("'ecParameters' not a known curve type");
				}
				int[] exponentsPresent = ((IPolynomialExtensionField)curve.Field).MinimalPolynomial.GetExponentsPresent();
				TlsUtilities.WriteUint8(2, output);
				int i = exponentsPresent[exponentsPresent.Length - 1];
				TlsUtilities.CheckUint16(i);
				TlsUtilities.WriteUint16(i, output);
				if (exponentsPresent.Length == 3)
				{
					TlsUtilities.WriteUint8(1, output);
					TlsEccUtilities.WriteECExponent(exponentsPresent[1], output);
				}
				else
				{
					if (exponentsPresent.Length != 5)
					{
						throw new ArgumentException("Only trinomial and pentomial curves are supported");
					}
					TlsUtilities.WriteUint8(2, output);
					TlsEccUtilities.WriteECExponent(exponentsPresent[1], output);
					TlsEccUtilities.WriteECExponent(exponentsPresent[2], output);
					TlsEccUtilities.WriteECExponent(exponentsPresent[3], output);
				}
			}
			TlsEccUtilities.WriteECFieldElement(curve.A, output);
			TlsEccUtilities.WriteECFieldElement(curve.B, output);
			TlsUtilities.WriteOpaque8(TlsEccUtilities.SerializeECPoint(ecPointFormats, ecParameters.G), output);
			TlsEccUtilities.WriteECParameter(ecParameters.N, output);
			TlsEccUtilities.WriteECParameter(ecParameters.H, output);
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x0011783F File Offset: 0x00115A3F
		public static void WriteECPoint(byte[] ecPointFormats, ECPoint point, Stream output)
		{
			TlsUtilities.WriteOpaque8(TlsEccUtilities.SerializeECPoint(ecPointFormats, point), output);
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x0011784E File Offset: 0x00115A4E
		public static void WriteNamedECParameters(int namedCurve, Stream output)
		{
			if (!NamedCurve.RefersToASpecificNamedCurve(namedCurve))
			{
				throw new TlsFatalAlert(80);
			}
			TlsUtilities.WriteUint8(3, output);
			TlsUtilities.CheckUint16(namedCurve);
			TlsUtilities.WriteUint16(namedCurve, output);
		}

		// Token: 0x04001E39 RID: 7737
		private static readonly string[] CurveNames = new string[]
		{
			"sect163k1",
			"sect163r1",
			"sect163r2",
			"sect193r1",
			"sect193r2",
			"sect233k1",
			"sect233r1",
			"sect239k1",
			"sect283k1",
			"sect283r1",
			"sect409k1",
			"sect409r1",
			"sect571k1",
			"sect571r1",
			"secp160k1",
			"secp160r1",
			"secp160r2",
			"secp192k1",
			"secp192r1",
			"secp224k1",
			"secp224r1",
			"secp256k1",
			"secp256r1",
			"secp384r1",
			"secp521r1",
			"brainpoolP256r1",
			"brainpoolP384r1",
			"brainpoolP512r1"
		};
	}
}
