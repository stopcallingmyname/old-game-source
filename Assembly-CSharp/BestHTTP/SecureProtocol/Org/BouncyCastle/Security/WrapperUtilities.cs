using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Kisa;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ntt;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002EC RID: 748
	public sealed class WrapperUtilities
	{
		// Token: 0x06001B61 RID: 7009 RVA: 0x00022F1F File Offset: 0x0002111F
		private WrapperUtilities()
		{
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x000CFFE0 File Offset: 0x000CE1E0
		static WrapperUtilities()
		{
			((WrapperUtilities.WrapAlgorithm)Enums.GetArbitraryValue(typeof(WrapperUtilities.WrapAlgorithm))).ToString();
			WrapperUtilities.algorithms[NistObjectIdentifiers.IdAes128Wrap.Id] = "AESWRAP";
			WrapperUtilities.algorithms[NistObjectIdentifiers.IdAes192Wrap.Id] = "AESWRAP";
			WrapperUtilities.algorithms[NistObjectIdentifiers.IdAes256Wrap.Id] = "AESWRAP";
			WrapperUtilities.algorithms[NttObjectIdentifiers.IdCamellia128Wrap.Id] = "CAMELLIAWRAP";
			WrapperUtilities.algorithms[NttObjectIdentifiers.IdCamellia192Wrap.Id] = "CAMELLIAWRAP";
			WrapperUtilities.algorithms[NttObjectIdentifiers.IdCamellia256Wrap.Id] = "CAMELLIAWRAP";
			WrapperUtilities.algorithms[PkcsObjectIdentifiers.IdAlgCms3DesWrap.Id] = "DESEDEWRAP";
			WrapperUtilities.algorithms["TDEAWRAP"] = "DESEDEWRAP";
			WrapperUtilities.algorithms[PkcsObjectIdentifiers.IdAlgCmsRC2Wrap.Id] = "RC2WRAP";
			WrapperUtilities.algorithms[KisaObjectIdentifiers.IdNpkiAppCmsSeedWrap.Id] = "SEEDWRAP";
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x000D010F File Offset: 0x000CE30F
		public static IWrapper GetWrapper(DerObjectIdentifier oid)
		{
			return WrapperUtilities.GetWrapper(oid.Id);
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x000D011C File Offset: 0x000CE31C
		public static IWrapper GetWrapper(string algorithm)
		{
			string text = Platform.ToUpperInvariant(algorithm);
			string text2 = (string)WrapperUtilities.algorithms[text];
			if (text2 == null)
			{
				text2 = text;
			}
			try
			{
				switch ((WrapperUtilities.WrapAlgorithm)Enums.GetEnumValue(typeof(WrapperUtilities.WrapAlgorithm), text2))
				{
				case WrapperUtilities.WrapAlgorithm.AESWRAP:
					return new AesWrapEngine();
				case WrapperUtilities.WrapAlgorithm.CAMELLIAWRAP:
					return new CamelliaWrapEngine();
				case WrapperUtilities.WrapAlgorithm.DESEDEWRAP:
					return new DesEdeWrapEngine();
				case WrapperUtilities.WrapAlgorithm.RC2WRAP:
					return new RC2WrapEngine();
				case WrapperUtilities.WrapAlgorithm.SEEDWRAP:
					return new SeedWrapEngine();
				case WrapperUtilities.WrapAlgorithm.DESEDERFC3211WRAP:
					return new Rfc3211WrapEngine(new DesEdeEngine());
				case WrapperUtilities.WrapAlgorithm.AESRFC3211WRAP:
					return new Rfc3211WrapEngine(new AesEngine());
				case WrapperUtilities.WrapAlgorithm.CAMELLIARFC3211WRAP:
					return new Rfc3211WrapEngine(new CamelliaEngine());
				}
			}
			catch (ArgumentException)
			{
			}
			IBufferedCipher cipher = CipherUtilities.GetCipher(algorithm);
			if (cipher != null)
			{
				return new WrapperUtilities.BufferedCipherWrapper(cipher);
			}
			throw new SecurityUtilityException("Wrapper " + algorithm + " not recognised.");
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x000D021C File Offset: 0x000CE41C
		public static string GetAlgorithmName(DerObjectIdentifier oid)
		{
			return (string)WrapperUtilities.algorithms[oid.Id];
		}

		// Token: 0x040018F4 RID: 6388
		private static readonly IDictionary algorithms = Platform.CreateHashtable();

		// Token: 0x02000907 RID: 2311
		private enum WrapAlgorithm
		{
			// Token: 0x04003546 RID: 13638
			AESWRAP,
			// Token: 0x04003547 RID: 13639
			CAMELLIAWRAP,
			// Token: 0x04003548 RID: 13640
			DESEDEWRAP,
			// Token: 0x04003549 RID: 13641
			RC2WRAP,
			// Token: 0x0400354A RID: 13642
			SEEDWRAP,
			// Token: 0x0400354B RID: 13643
			DESEDERFC3211WRAP,
			// Token: 0x0400354C RID: 13644
			AESRFC3211WRAP,
			// Token: 0x0400354D RID: 13645
			CAMELLIARFC3211WRAP
		}

		// Token: 0x02000908 RID: 2312
		private class BufferedCipherWrapper : IWrapper
		{
			// Token: 0x06004E31 RID: 20017 RVA: 0x001B0F12 File Offset: 0x001AF112
			public BufferedCipherWrapper(IBufferedCipher cipher)
			{
				this.cipher = cipher;
			}

			// Token: 0x17000C2A RID: 3114
			// (get) Token: 0x06004E32 RID: 20018 RVA: 0x001B0F21 File Offset: 0x001AF121
			public string AlgorithmName
			{
				get
				{
					return this.cipher.AlgorithmName;
				}
			}

			// Token: 0x06004E33 RID: 20019 RVA: 0x001B0F2E File Offset: 0x001AF12E
			public void Init(bool forWrapping, ICipherParameters parameters)
			{
				this.forWrapping = forWrapping;
				this.cipher.Init(forWrapping, parameters);
			}

			// Token: 0x06004E34 RID: 20020 RVA: 0x001B0F44 File Offset: 0x001AF144
			public byte[] Wrap(byte[] input, int inOff, int length)
			{
				if (!this.forWrapping)
				{
					throw new InvalidOperationException("Not initialised for wrapping");
				}
				return this.cipher.DoFinal(input, inOff, length);
			}

			// Token: 0x06004E35 RID: 20021 RVA: 0x001B0F67 File Offset: 0x001AF167
			public byte[] Unwrap(byte[] input, int inOff, int length)
			{
				if (this.forWrapping)
				{
					throw new InvalidOperationException("Not initialised for unwrapping");
				}
				return this.cipher.DoFinal(input, inOff, length);
			}

			// Token: 0x0400354E RID: 13646
			private readonly IBufferedCipher cipher;

			// Token: 0x0400354F RID: 13647
			private bool forWrapping;
		}
	}
}
