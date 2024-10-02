using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.GM;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Rosstandart;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.TeleTrust;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tsp
{
	// Token: 0x020002AB RID: 683
	public abstract class TspAlgorithms
	{
		// Token: 0x060018EF RID: 6383 RVA: 0x000BB190 File Offset: 0x000B9390
		static TspAlgorithms()
		{
			string[] array = new string[]
			{
				TspAlgorithms.Gost3411,
				TspAlgorithms.Gost3411_2012_256,
				TspAlgorithms.Gost3411_2012_512,
				TspAlgorithms.MD5,
				TspAlgorithms.RipeMD128,
				TspAlgorithms.RipeMD160,
				TspAlgorithms.RipeMD256,
				TspAlgorithms.Sha1,
				TspAlgorithms.Sha224,
				TspAlgorithms.Sha256,
				TspAlgorithms.Sha384,
				TspAlgorithms.Sha512,
				TspAlgorithms.SM3
			};
			TspAlgorithms.Allowed = Platform.CreateArrayList();
			foreach (string value in array)
			{
				TspAlgorithms.Allowed.Add(value);
			}
		}

		// Token: 0x04001858 RID: 6232
		public static readonly string MD5 = PkcsObjectIdentifiers.MD5.Id;

		// Token: 0x04001859 RID: 6233
		public static readonly string Sha1 = OiwObjectIdentifiers.IdSha1.Id;

		// Token: 0x0400185A RID: 6234
		public static readonly string Sha224 = NistObjectIdentifiers.IdSha224.Id;

		// Token: 0x0400185B RID: 6235
		public static readonly string Sha256 = NistObjectIdentifiers.IdSha256.Id;

		// Token: 0x0400185C RID: 6236
		public static readonly string Sha384 = NistObjectIdentifiers.IdSha384.Id;

		// Token: 0x0400185D RID: 6237
		public static readonly string Sha512 = NistObjectIdentifiers.IdSha512.Id;

		// Token: 0x0400185E RID: 6238
		public static readonly string RipeMD128 = TeleTrusTObjectIdentifiers.RipeMD128.Id;

		// Token: 0x0400185F RID: 6239
		public static readonly string RipeMD160 = TeleTrusTObjectIdentifiers.RipeMD160.Id;

		// Token: 0x04001860 RID: 6240
		public static readonly string RipeMD256 = TeleTrusTObjectIdentifiers.RipeMD256.Id;

		// Token: 0x04001861 RID: 6241
		public static readonly string Gost3411 = CryptoProObjectIdentifiers.GostR3411.Id;

		// Token: 0x04001862 RID: 6242
		public static readonly string Gost3411_2012_256 = RosstandartObjectIdentifiers.id_tc26_gost_3411_12_256.Id;

		// Token: 0x04001863 RID: 6243
		public static readonly string Gost3411_2012_512 = RosstandartObjectIdentifiers.id_tc26_gost_3411_12_512.Id;

		// Token: 0x04001864 RID: 6244
		public static readonly string SM3 = GMObjectIdentifiers.sm3.Id;

		// Token: 0x04001865 RID: 6245
		public static readonly IList Allowed;
	}
}
