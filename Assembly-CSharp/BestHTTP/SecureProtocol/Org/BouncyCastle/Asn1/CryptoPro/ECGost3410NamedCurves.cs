﻿using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Rosstandart;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro
{
	// Token: 0x0200075E RID: 1886
	public sealed class ECGost3410NamedCurves
	{
		// Token: 0x060043D8 RID: 17368 RVA: 0x00022F1F File Offset: 0x0002111F
		private ECGost3410NamedCurves()
		{
		}

		// Token: 0x060043D9 RID: 17369 RVA: 0x0018BF20 File Offset: 0x0018A120
		static ECGost3410NamedCurves()
		{
			BigInteger q = new BigInteger("115792089237316195423570985008687907853269984665640564039457584007913129639319");
			BigInteger bigInteger = new BigInteger("115792089237316195423570985008687907853073762908499243225378155805079068850323");
			FpCurve fpCurve = new FpCurve(q, new BigInteger("115792089237316195423570985008687907853269984665640564039457584007913129639316"), new BigInteger("166"), bigInteger, BigInteger.One);
			ECDomainParameters value = new ECDomainParameters(fpCurve, fpCurve.CreatePoint(new BigInteger("1"), new BigInteger("64033881142927202683649881450433473985931760268884941288852745803908878638612")), bigInteger, BigInteger.One);
			ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProA] = value;
			BigInteger q2 = new BigInteger("115792089237316195423570985008687907853269984665640564039457584007913129639319");
			bigInteger = new BigInteger("115792089237316195423570985008687907853073762908499243225378155805079068850323");
			fpCurve = new FpCurve(q2, new BigInteger("115792089237316195423570985008687907853269984665640564039457584007913129639316"), new BigInteger("166"), bigInteger, BigInteger.One);
			value = new ECDomainParameters(fpCurve, fpCurve.CreatePoint(new BigInteger("1"), new BigInteger("64033881142927202683649881450433473985931760268884941288852745803908878638612")), bigInteger, BigInteger.One);
			ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchA] = value;
			BigInteger q3 = new BigInteger("57896044618658097711785492504343953926634992332820282019728792003956564823193");
			bigInteger = new BigInteger("57896044618658097711785492504343953927102133160255826820068844496087732066703");
			fpCurve = new FpCurve(q3, new BigInteger("57896044618658097711785492504343953926634992332820282019728792003956564823190"), new BigInteger("28091019353058090096996979000309560759124368558014865957655842872397301267595"), bigInteger, BigInteger.One);
			value = new ECDomainParameters(fpCurve, fpCurve.CreatePoint(new BigInteger("1"), new BigInteger("28792665814854611296992347458380284135028636778229113005756334730996303888124")), bigInteger, BigInteger.One);
			ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProB] = value;
			BigInteger q4 = new BigInteger("70390085352083305199547718019018437841079516630045180471284346843705633502619");
			bigInteger = new BigInteger("70390085352083305199547718019018437840920882647164081035322601458352298396601");
			fpCurve = new FpCurve(q4, new BigInteger("70390085352083305199547718019018437841079516630045180471284346843705633502616"), new BigInteger("32858"), bigInteger, BigInteger.One);
			value = new ECDomainParameters(fpCurve, fpCurve.CreatePoint(new BigInteger("0"), new BigInteger("29818893917731240733471273240314769927240550812383695689146495261604565990247")), bigInteger, BigInteger.One);
			ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchB] = value;
			BigInteger q5 = new BigInteger("70390085352083305199547718019018437841079516630045180471284346843705633502619");
			bigInteger = new BigInteger("70390085352083305199547718019018437840920882647164081035322601458352298396601");
			fpCurve = new FpCurve(q5, new BigInteger("70390085352083305199547718019018437841079516630045180471284346843705633502616"), new BigInteger("32858"), bigInteger, BigInteger.One);
			value = new ECDomainParameters(fpCurve, fpCurve.CreatePoint(new BigInteger("0"), new BigInteger("29818893917731240733471273240314769927240550812383695689146495261604565990247")), bigInteger, BigInteger.One);
			ECGost3410NamedCurves.parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProC] = value;
			BigInteger q6 = new BigInteger("115792089237316195423570985008687907853269984665640564039457584007913129639319");
			bigInteger = new BigInteger("115792089237316195423570985008687907853073762908499243225378155805079068850323");
			fpCurve = new FpCurve(q6, new BigInteger("115792089237316195423570985008687907853269984665640564039457584007913129639316"), new BigInteger("166"), bigInteger, BigInteger.One);
			value = new ECDomainParameters(fpCurve, fpCurve.CreatePoint(new BigInteger("1"), new BigInteger("64033881142927202683649881450433473985931760268884941288852745803908878638612")), bigInteger, BigInteger.One);
			ECGost3410NamedCurves.parameters[RosstandartObjectIdentifiers.id_tc26_gost_3410_12_256_paramSetA] = value;
			BigInteger q7 = new BigInteger("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDC7", 16);
			bigInteger = new BigInteger("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF27E69532F48D89116FF22B8D4E0560609B4B38ABFAD2B85DCACDB1411F10B275", 16);
			fpCurve = new FpCurve(q7, new BigInteger("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDC4", 16), new BigInteger("E8C2505DEDFC86DDC1BD0B2B6667F1DA34B82574761CB0E879BD081CFD0B6265EE3CB090F30D27614CB4574010DA90DD862EF9D4EBEE4761503190785A71C760", 16), bigInteger, BigInteger.One);
			value = new ECDomainParameters(fpCurve, fpCurve.CreatePoint(new BigInteger("00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000003"), new BigInteger("7503CFE87A836AE3A61B8816E25450E6CE5E1C93ACF1ABC1778064FDCBEFA921DF1626BE4FD036E93D75E6A50E3A41E98028FE5FC235F5B889A589CB5215F2A4", 16)), bigInteger, BigInteger.One);
			ECGost3410NamedCurves.parameters[RosstandartObjectIdentifiers.id_tc26_gost_3410_12_512_paramSetA] = value;
			BigInteger q8 = new BigInteger("8000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000006F", 16);
			bigInteger = new BigInteger("800000000000000000000000000000000000000000000000000000000000000149A1EC142565A545ACFDB77BD9D40CFA8B996712101BEA0EC6346C54374F25BD", 16);
			fpCurve = new FpCurve(q8, new BigInteger("8000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000006C", 16), new BigInteger("687D1B459DC841457E3E06CF6F5E2517B97C7D614AF138BCBF85DC806C4B289F3E965D2DB1416D217F8B276FAD1AB69C50F78BEE1FA3106EFB8CCBC7C5140116", 16), bigInteger, BigInteger.One);
			value = new ECDomainParameters(fpCurve, fpCurve.CreatePoint(new BigInteger("00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002"), new BigInteger("1A8F7EDA389B094C2C071E3647A8940F3C123B697578C213BE6DD9E6C8EC7335DCB228FD1EDF4A39152CBCAAF8C0398828041055F94CEEEC7E21340780FE41BD", 16)), bigInteger, BigInteger.One);
			ECGost3410NamedCurves.parameters[RosstandartObjectIdentifiers.id_tc26_gost_3410_12_512_paramSetB] = value;
			BigInteger q9 = new BigInteger("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFDC7", 16);
			bigInteger = new BigInteger("3FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC98CDBA46506AB004C33A9FF5147502CC8EDA9E7A769A12694623CEF47F023ED", 16);
			fpCurve = new FpCurve(q9, new BigInteger("DC9203E514A721875485A529D2C722FB187BC8980EB866644DE41C68E143064546E861C0E2C9EDD92ADE71F46FCF50FF2AD97F951FDA9F2A2EB6546F39689BD3", 16), new BigInteger("B4C4EE28CEBC6C2C8AC12952CF37F16AC7EFB6A9F69F4B57FFDA2E4F0DE5ADE038CBC2FFF719D2C18DE0284B8BFEF3B52B8CC7A5F5BF0A3C8D2319A5312557E1", 16), bigInteger, BigInteger.One);
			value = new ECDomainParameters(fpCurve, fpCurve.CreatePoint(new BigInteger("E2E31EDFC23DE7BDEBE241CE593EF5DE2295B7A9CBAEF021D385F7074CEA043AA27272A7AE602BF2A7B9033DB9ED3610C6FB85487EAE97AAC5BC7928C1950148", 16), new BigInteger("F5CE40D95B5EB899ABBCCFF5911CB8577939804D6527378B8C108C3D2090FF9BE18E2D33E3021ED2EF32D85822423B6304F726AA854BAE07D0396E9A9ADDC40F", 16)), bigInteger, BigInteger.One);
			ECGost3410NamedCurves.parameters[RosstandartObjectIdentifiers.id_tc26_gost_3410_12_512_paramSetC] = value;
			ECGost3410NamedCurves.objIds["GostR3410-2001-CryptoPro-A"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProA;
			ECGost3410NamedCurves.objIds["GostR3410-2001-CryptoPro-B"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProB;
			ECGost3410NamedCurves.objIds["GostR3410-2001-CryptoPro-C"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProC;
			ECGost3410NamedCurves.objIds["GostR3410-2001-CryptoPro-XchA"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchA;
			ECGost3410NamedCurves.objIds["GostR3410-2001-CryptoPro-XchB"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchB;
			ECGost3410NamedCurves.objIds["Tc26-Gost-3410-12-256-paramSetA"] = RosstandartObjectIdentifiers.id_tc26_gost_3410_12_256_paramSetA;
			ECGost3410NamedCurves.objIds["Tc26-Gost-3410-12-512-paramSetA"] = RosstandartObjectIdentifiers.id_tc26_gost_3410_12_512_paramSetA;
			ECGost3410NamedCurves.objIds["Tc26-Gost-3410-12-512-paramSetB"] = RosstandartObjectIdentifiers.id_tc26_gost_3410_12_512_paramSetB;
			ECGost3410NamedCurves.objIds["Tc26-Gost-3410-12-512-paramSetC"] = RosstandartObjectIdentifiers.id_tc26_gost_3410_12_512_paramSetC;
			ECGost3410NamedCurves.names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProA] = "GostR3410-2001-CryptoPro-A";
			ECGost3410NamedCurves.names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProB] = "GostR3410-2001-CryptoPro-B";
			ECGost3410NamedCurves.names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProC] = "GostR3410-2001-CryptoPro-C";
			ECGost3410NamedCurves.names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchA] = "GostR3410-2001-CryptoPro-XchA";
			ECGost3410NamedCurves.names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchB] = "GostR3410-2001-CryptoPro-XchB";
			ECGost3410NamedCurves.names[RosstandartObjectIdentifiers.id_tc26_gost_3410_12_256_paramSetA] = "Tc26-Gost-3410-12-256-paramSetA";
			ECGost3410NamedCurves.names[RosstandartObjectIdentifiers.id_tc26_gost_3410_12_512_paramSetA] = "Tc26-Gost-3410-12-512-paramSetA";
			ECGost3410NamedCurves.names[RosstandartObjectIdentifiers.id_tc26_gost_3410_12_512_paramSetB] = "Tc26-Gost-3410-12-512-paramSetB";
			ECGost3410NamedCurves.names[RosstandartObjectIdentifiers.id_tc26_gost_3410_12_512_paramSetC] = "Tc26-Gost-3410-12-512-paramSetC";
		}

		// Token: 0x060043DA RID: 17370 RVA: 0x0018C49F File Offset: 0x0018A69F
		public static ECDomainParameters GetByOid(DerObjectIdentifier oid)
		{
			return (ECDomainParameters)ECGost3410NamedCurves.parameters[oid];
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x060043DB RID: 17371 RVA: 0x0018C4B1 File Offset: 0x0018A6B1
		public static IEnumerable Names
		{
			get
			{
				return new EnumerableProxy(ECGost3410NamedCurves.names.Values);
			}
		}

		// Token: 0x060043DC RID: 17372 RVA: 0x0018C4C4 File Offset: 0x0018A6C4
		public static ECDomainParameters GetByName(string name)
		{
			DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)ECGost3410NamedCurves.objIds[name];
			if (derObjectIdentifier != null)
			{
				return (ECDomainParameters)ECGost3410NamedCurves.parameters[derObjectIdentifier];
			}
			return null;
		}

		// Token: 0x060043DD RID: 17373 RVA: 0x0018C4F7 File Offset: 0x0018A6F7
		public static string GetName(DerObjectIdentifier oid)
		{
			return (string)ECGost3410NamedCurves.names[oid];
		}

		// Token: 0x060043DE RID: 17374 RVA: 0x0018C509 File Offset: 0x0018A709
		public static DerObjectIdentifier GetOid(string name)
		{
			return (DerObjectIdentifier)ECGost3410NamedCurves.objIds[name];
		}

		// Token: 0x04002C90 RID: 11408
		internal static readonly IDictionary objIds = Platform.CreateHashtable();

		// Token: 0x04002C91 RID: 11409
		internal static readonly IDictionary parameters = Platform.CreateHashtable();

		// Token: 0x04002C92 RID: 11410
		internal static readonly IDictionary names = Platform.CreateHashtable();
	}
}
