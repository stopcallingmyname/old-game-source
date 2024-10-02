using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.GM
{
	// Token: 0x02000738 RID: 1848
	public abstract class GMObjectIdentifiers
	{
		// Token: 0x04002BC9 RID: 11209
		public static readonly DerObjectIdentifier sm_scheme = new DerObjectIdentifier("1.2.156.10197.1");

		// Token: 0x04002BCA RID: 11210
		public static readonly DerObjectIdentifier sm6_ecb = GMObjectIdentifiers.sm_scheme.Branch("101.1");

		// Token: 0x04002BCB RID: 11211
		public static readonly DerObjectIdentifier sm6_cbc = GMObjectIdentifiers.sm_scheme.Branch("101.2");

		// Token: 0x04002BCC RID: 11212
		public static readonly DerObjectIdentifier sm6_ofb128 = GMObjectIdentifiers.sm_scheme.Branch("101.3");

		// Token: 0x04002BCD RID: 11213
		public static readonly DerObjectIdentifier sm6_cfb128 = GMObjectIdentifiers.sm_scheme.Branch("101.4");

		// Token: 0x04002BCE RID: 11214
		public static readonly DerObjectIdentifier sm1_ecb = GMObjectIdentifiers.sm_scheme.Branch("102.1");

		// Token: 0x04002BCF RID: 11215
		public static readonly DerObjectIdentifier sm1_cbc = GMObjectIdentifiers.sm_scheme.Branch("102.2");

		// Token: 0x04002BD0 RID: 11216
		public static readonly DerObjectIdentifier sm1_ofb128 = GMObjectIdentifiers.sm_scheme.Branch("102.3");

		// Token: 0x04002BD1 RID: 11217
		public static readonly DerObjectIdentifier sm1_cfb128 = GMObjectIdentifiers.sm_scheme.Branch("102.4");

		// Token: 0x04002BD2 RID: 11218
		public static readonly DerObjectIdentifier sm1_cfb1 = GMObjectIdentifiers.sm_scheme.Branch("102.5");

		// Token: 0x04002BD3 RID: 11219
		public static readonly DerObjectIdentifier sm1_cfb8 = GMObjectIdentifiers.sm_scheme.Branch("102.6");

		// Token: 0x04002BD4 RID: 11220
		public static readonly DerObjectIdentifier ssf33_ecb = GMObjectIdentifiers.sm_scheme.Branch("103.1");

		// Token: 0x04002BD5 RID: 11221
		public static readonly DerObjectIdentifier ssf33_cbc = GMObjectIdentifiers.sm_scheme.Branch("103.2");

		// Token: 0x04002BD6 RID: 11222
		public static readonly DerObjectIdentifier ssf33_ofb128 = GMObjectIdentifiers.sm_scheme.Branch("103.3");

		// Token: 0x04002BD7 RID: 11223
		public static readonly DerObjectIdentifier ssf33_cfb128 = GMObjectIdentifiers.sm_scheme.Branch("103.4");

		// Token: 0x04002BD8 RID: 11224
		public static readonly DerObjectIdentifier ssf33_cfb1 = GMObjectIdentifiers.sm_scheme.Branch("103.5");

		// Token: 0x04002BD9 RID: 11225
		public static readonly DerObjectIdentifier ssf33_cfb8 = GMObjectIdentifiers.sm_scheme.Branch("103.6");

		// Token: 0x04002BDA RID: 11226
		public static readonly DerObjectIdentifier sms4_ecb = GMObjectIdentifiers.sm_scheme.Branch("104.1");

		// Token: 0x04002BDB RID: 11227
		public static readonly DerObjectIdentifier sms4_cbc = GMObjectIdentifiers.sm_scheme.Branch("104.2");

		// Token: 0x04002BDC RID: 11228
		public static readonly DerObjectIdentifier sms4_ofb128 = GMObjectIdentifiers.sm_scheme.Branch("104.3");

		// Token: 0x04002BDD RID: 11229
		public static readonly DerObjectIdentifier sms4_cfb128 = GMObjectIdentifiers.sm_scheme.Branch("104.4");

		// Token: 0x04002BDE RID: 11230
		public static readonly DerObjectIdentifier sms4_cfb1 = GMObjectIdentifiers.sm_scheme.Branch("104.5");

		// Token: 0x04002BDF RID: 11231
		public static readonly DerObjectIdentifier sms4_cfb8 = GMObjectIdentifiers.sm_scheme.Branch("104.6");

		// Token: 0x04002BE0 RID: 11232
		public static readonly DerObjectIdentifier sms4_ctr = GMObjectIdentifiers.sm_scheme.Branch("104.7");

		// Token: 0x04002BE1 RID: 11233
		public static readonly DerObjectIdentifier sms4_gcm = GMObjectIdentifiers.sm_scheme.Branch("104.8");

		// Token: 0x04002BE2 RID: 11234
		public static readonly DerObjectIdentifier sms4_ccm = GMObjectIdentifiers.sm_scheme.Branch("104.9");

		// Token: 0x04002BE3 RID: 11235
		public static readonly DerObjectIdentifier sms4_xts = GMObjectIdentifiers.sm_scheme.Branch("104.10");

		// Token: 0x04002BE4 RID: 11236
		public static readonly DerObjectIdentifier sms4_wrap = GMObjectIdentifiers.sm_scheme.Branch("104.11");

		// Token: 0x04002BE5 RID: 11237
		public static readonly DerObjectIdentifier sms4_wrap_pad = GMObjectIdentifiers.sm_scheme.Branch("104.12");

		// Token: 0x04002BE6 RID: 11238
		public static readonly DerObjectIdentifier sms4_ocb = GMObjectIdentifiers.sm_scheme.Branch("104.100");

		// Token: 0x04002BE7 RID: 11239
		public static readonly DerObjectIdentifier sm5 = GMObjectIdentifiers.sm_scheme.Branch("201");

		// Token: 0x04002BE8 RID: 11240
		public static readonly DerObjectIdentifier sm2p256v1 = GMObjectIdentifiers.sm_scheme.Branch("301");

		// Token: 0x04002BE9 RID: 11241
		public static readonly DerObjectIdentifier sm2sign = GMObjectIdentifiers.sm_scheme.Branch("301.1");

		// Token: 0x04002BEA RID: 11242
		public static readonly DerObjectIdentifier sm2exchange = GMObjectIdentifiers.sm_scheme.Branch("301.2");

		// Token: 0x04002BEB RID: 11243
		public static readonly DerObjectIdentifier sm2encrypt = GMObjectIdentifiers.sm_scheme.Branch("301.3");

		// Token: 0x04002BEC RID: 11244
		public static readonly DerObjectIdentifier wapip192v1 = GMObjectIdentifiers.sm_scheme.Branch("301.101");

		// Token: 0x04002BED RID: 11245
		public static readonly DerObjectIdentifier sm2encrypt_recommendedParameters = GMObjectIdentifiers.sm2encrypt.Branch("1");

		// Token: 0x04002BEE RID: 11246
		public static readonly DerObjectIdentifier sm2encrypt_specifiedParameters = GMObjectIdentifiers.sm2encrypt.Branch("2");

		// Token: 0x04002BEF RID: 11247
		public static readonly DerObjectIdentifier sm2encrypt_with_sm3 = GMObjectIdentifiers.sm2encrypt.Branch("2.1");

		// Token: 0x04002BF0 RID: 11248
		public static readonly DerObjectIdentifier sm2encrypt_with_sha1 = GMObjectIdentifiers.sm2encrypt.Branch("2.2");

		// Token: 0x04002BF1 RID: 11249
		public static readonly DerObjectIdentifier sm2encrypt_with_sha224 = GMObjectIdentifiers.sm2encrypt.Branch("2.3");

		// Token: 0x04002BF2 RID: 11250
		public static readonly DerObjectIdentifier sm2encrypt_with_sha256 = GMObjectIdentifiers.sm2encrypt.Branch("2.4");

		// Token: 0x04002BF3 RID: 11251
		public static readonly DerObjectIdentifier sm2encrypt_with_sha384 = GMObjectIdentifiers.sm2encrypt.Branch("2.5");

		// Token: 0x04002BF4 RID: 11252
		public static readonly DerObjectIdentifier sm2encrypt_with_sha512 = GMObjectIdentifiers.sm2encrypt.Branch("2.6");

		// Token: 0x04002BF5 RID: 11253
		public static readonly DerObjectIdentifier sm2encrypt_with_rmd160 = GMObjectIdentifiers.sm2encrypt.Branch("2.7");

		// Token: 0x04002BF6 RID: 11254
		public static readonly DerObjectIdentifier sm2encrypt_with_whirlpool = GMObjectIdentifiers.sm2encrypt.Branch("2.8");

		// Token: 0x04002BF7 RID: 11255
		public static readonly DerObjectIdentifier sm2encrypt_with_blake2b512 = GMObjectIdentifiers.sm2encrypt.Branch("2.9");

		// Token: 0x04002BF8 RID: 11256
		public static readonly DerObjectIdentifier sm2encrypt_with_blake2s256 = GMObjectIdentifiers.sm2encrypt.Branch("2.10");

		// Token: 0x04002BF9 RID: 11257
		public static readonly DerObjectIdentifier sm2encrypt_with_md5 = GMObjectIdentifiers.sm2encrypt.Branch("2.11");

		// Token: 0x04002BFA RID: 11258
		public static readonly DerObjectIdentifier id_sm9PublicKey = GMObjectIdentifiers.sm_scheme.Branch("302");

		// Token: 0x04002BFB RID: 11259
		public static readonly DerObjectIdentifier sm9sign = GMObjectIdentifiers.sm_scheme.Branch("302.1");

		// Token: 0x04002BFC RID: 11260
		public static readonly DerObjectIdentifier sm9keyagreement = GMObjectIdentifiers.sm_scheme.Branch("302.2");

		// Token: 0x04002BFD RID: 11261
		public static readonly DerObjectIdentifier sm9encrypt = GMObjectIdentifiers.sm_scheme.Branch("302.3");

		// Token: 0x04002BFE RID: 11262
		public static readonly DerObjectIdentifier sm3 = GMObjectIdentifiers.sm_scheme.Branch("401");

		// Token: 0x04002BFF RID: 11263
		public static readonly DerObjectIdentifier hmac_sm3 = GMObjectIdentifiers.sm3.Branch("2");

		// Token: 0x04002C00 RID: 11264
		public static readonly DerObjectIdentifier sm2sign_with_sm3 = GMObjectIdentifiers.sm_scheme.Branch("501");

		// Token: 0x04002C01 RID: 11265
		public static readonly DerObjectIdentifier sm2sign_with_sha1 = GMObjectIdentifiers.sm_scheme.Branch("502");

		// Token: 0x04002C02 RID: 11266
		public static readonly DerObjectIdentifier sm2sign_with_sha256 = GMObjectIdentifiers.sm_scheme.Branch("503");

		// Token: 0x04002C03 RID: 11267
		public static readonly DerObjectIdentifier sm2sign_with_sha512 = GMObjectIdentifiers.sm_scheme.Branch("504");

		// Token: 0x04002C04 RID: 11268
		public static readonly DerObjectIdentifier sm2sign_with_sha224 = GMObjectIdentifiers.sm_scheme.Branch("505");

		// Token: 0x04002C05 RID: 11269
		public static readonly DerObjectIdentifier sm2sign_with_sha384 = GMObjectIdentifiers.sm_scheme.Branch("506");

		// Token: 0x04002C06 RID: 11270
		public static readonly DerObjectIdentifier sm2sign_with_rmd160 = GMObjectIdentifiers.sm_scheme.Branch("507");

		// Token: 0x04002C07 RID: 11271
		public static readonly DerObjectIdentifier sm2sign_with_whirlpool = GMObjectIdentifiers.sm_scheme.Branch("520");

		// Token: 0x04002C08 RID: 11272
		public static readonly DerObjectIdentifier sm2sign_with_blake2b512 = GMObjectIdentifiers.sm_scheme.Branch("521");

		// Token: 0x04002C09 RID: 11273
		public static readonly DerObjectIdentifier sm2sign_with_blake2s256 = GMObjectIdentifiers.sm_scheme.Branch("522");
	}
}
