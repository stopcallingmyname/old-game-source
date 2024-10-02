using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200040B RID: 1035
	public abstract class CipherSuite
	{
		// Token: 0x060029A6 RID: 10662 RVA: 0x0010EEEC File Offset: 0x0010D0EC
		public static bool IsScsv(int cipherSuite)
		{
			return cipherSuite == 255 || cipherSuite == 22016;
		}

		// Token: 0x04001B7D RID: 7037
		public const int TLS_NULL_WITH_NULL_NULL = 0;

		// Token: 0x04001B7E RID: 7038
		public const int TLS_RSA_WITH_NULL_MD5 = 1;

		// Token: 0x04001B7F RID: 7039
		public const int TLS_RSA_WITH_NULL_SHA = 2;

		// Token: 0x04001B80 RID: 7040
		public const int TLS_RSA_EXPORT_WITH_RC4_40_MD5 = 3;

		// Token: 0x04001B81 RID: 7041
		public const int TLS_RSA_WITH_RC4_128_MD5 = 4;

		// Token: 0x04001B82 RID: 7042
		public const int TLS_RSA_WITH_RC4_128_SHA = 5;

		// Token: 0x04001B83 RID: 7043
		public const int TLS_RSA_EXPORT_WITH_RC2_CBC_40_MD5 = 6;

		// Token: 0x04001B84 RID: 7044
		public const int TLS_RSA_WITH_IDEA_CBC_SHA = 7;

		// Token: 0x04001B85 RID: 7045
		public const int TLS_RSA_EXPORT_WITH_DES40_CBC_SHA = 8;

		// Token: 0x04001B86 RID: 7046
		public const int TLS_RSA_WITH_DES_CBC_SHA = 9;

		// Token: 0x04001B87 RID: 7047
		public const int TLS_RSA_WITH_3DES_EDE_CBC_SHA = 10;

		// Token: 0x04001B88 RID: 7048
		public const int TLS_DH_DSS_EXPORT_WITH_DES40_CBC_SHA = 11;

		// Token: 0x04001B89 RID: 7049
		public const int TLS_DH_DSS_WITH_DES_CBC_SHA = 12;

		// Token: 0x04001B8A RID: 7050
		public const int TLS_DH_DSS_WITH_3DES_EDE_CBC_SHA = 13;

		// Token: 0x04001B8B RID: 7051
		public const int TLS_DH_RSA_EXPORT_WITH_DES40_CBC_SHA = 14;

		// Token: 0x04001B8C RID: 7052
		public const int TLS_DH_RSA_WITH_DES_CBC_SHA = 15;

		// Token: 0x04001B8D RID: 7053
		public const int TLS_DH_RSA_WITH_3DES_EDE_CBC_SHA = 16;

		// Token: 0x04001B8E RID: 7054
		public const int TLS_DHE_DSS_EXPORT_WITH_DES40_CBC_SHA = 17;

		// Token: 0x04001B8F RID: 7055
		public const int TLS_DHE_DSS_WITH_DES_CBC_SHA = 18;

		// Token: 0x04001B90 RID: 7056
		public const int TLS_DHE_DSS_WITH_3DES_EDE_CBC_SHA = 19;

		// Token: 0x04001B91 RID: 7057
		public const int TLS_DHE_RSA_EXPORT_WITH_DES40_CBC_SHA = 20;

		// Token: 0x04001B92 RID: 7058
		public const int TLS_DHE_RSA_WITH_DES_CBC_SHA = 21;

		// Token: 0x04001B93 RID: 7059
		public const int TLS_DHE_RSA_WITH_3DES_EDE_CBC_SHA = 22;

		// Token: 0x04001B94 RID: 7060
		public const int TLS_DH_anon_EXPORT_WITH_RC4_40_MD5 = 23;

		// Token: 0x04001B95 RID: 7061
		public const int TLS_DH_anon_WITH_RC4_128_MD5 = 24;

		// Token: 0x04001B96 RID: 7062
		public const int TLS_DH_anon_EXPORT_WITH_DES40_CBC_SHA = 25;

		// Token: 0x04001B97 RID: 7063
		public const int TLS_DH_anon_WITH_DES_CBC_SHA = 26;

		// Token: 0x04001B98 RID: 7064
		public const int TLS_DH_anon_WITH_3DES_EDE_CBC_SHA = 27;

		// Token: 0x04001B99 RID: 7065
		public const int TLS_RSA_WITH_AES_128_CBC_SHA = 47;

		// Token: 0x04001B9A RID: 7066
		public const int TLS_DH_DSS_WITH_AES_128_CBC_SHA = 48;

		// Token: 0x04001B9B RID: 7067
		public const int TLS_DH_RSA_WITH_AES_128_CBC_SHA = 49;

		// Token: 0x04001B9C RID: 7068
		public const int TLS_DHE_DSS_WITH_AES_128_CBC_SHA = 50;

		// Token: 0x04001B9D RID: 7069
		public const int TLS_DHE_RSA_WITH_AES_128_CBC_SHA = 51;

		// Token: 0x04001B9E RID: 7070
		public const int TLS_DH_anon_WITH_AES_128_CBC_SHA = 52;

		// Token: 0x04001B9F RID: 7071
		public const int TLS_RSA_WITH_AES_256_CBC_SHA = 53;

		// Token: 0x04001BA0 RID: 7072
		public const int TLS_DH_DSS_WITH_AES_256_CBC_SHA = 54;

		// Token: 0x04001BA1 RID: 7073
		public const int TLS_DH_RSA_WITH_AES_256_CBC_SHA = 55;

		// Token: 0x04001BA2 RID: 7074
		public const int TLS_DHE_DSS_WITH_AES_256_CBC_SHA = 56;

		// Token: 0x04001BA3 RID: 7075
		public const int TLS_DHE_RSA_WITH_AES_256_CBC_SHA = 57;

		// Token: 0x04001BA4 RID: 7076
		public const int TLS_DH_anon_WITH_AES_256_CBC_SHA = 58;

		// Token: 0x04001BA5 RID: 7077
		public const int TLS_RSA_WITH_CAMELLIA_128_CBC_SHA = 65;

		// Token: 0x04001BA6 RID: 7078
		public const int TLS_DH_DSS_WITH_CAMELLIA_128_CBC_SHA = 66;

		// Token: 0x04001BA7 RID: 7079
		public const int TLS_DH_RSA_WITH_CAMELLIA_128_CBC_SHA = 67;

		// Token: 0x04001BA8 RID: 7080
		public const int TLS_DHE_DSS_WITH_CAMELLIA_128_CBC_SHA = 68;

		// Token: 0x04001BA9 RID: 7081
		public const int TLS_DHE_RSA_WITH_CAMELLIA_128_CBC_SHA = 69;

		// Token: 0x04001BAA RID: 7082
		public const int TLS_DH_anon_WITH_CAMELLIA_128_CBC_SHA = 70;

		// Token: 0x04001BAB RID: 7083
		public const int TLS_RSA_WITH_CAMELLIA_256_CBC_SHA = 132;

		// Token: 0x04001BAC RID: 7084
		public const int TLS_DH_DSS_WITH_CAMELLIA_256_CBC_SHA = 133;

		// Token: 0x04001BAD RID: 7085
		public const int TLS_DH_RSA_WITH_CAMELLIA_256_CBC_SHA = 134;

		// Token: 0x04001BAE RID: 7086
		public const int TLS_DHE_DSS_WITH_CAMELLIA_256_CBC_SHA = 135;

		// Token: 0x04001BAF RID: 7087
		public const int TLS_DHE_RSA_WITH_CAMELLIA_256_CBC_SHA = 136;

		// Token: 0x04001BB0 RID: 7088
		public const int TLS_DH_anon_WITH_CAMELLIA_256_CBC_SHA = 137;

		// Token: 0x04001BB1 RID: 7089
		public const int TLS_RSA_WITH_CAMELLIA_128_CBC_SHA256 = 186;

		// Token: 0x04001BB2 RID: 7090
		public const int TLS_DH_DSS_WITH_CAMELLIA_128_CBC_SHA256 = 187;

		// Token: 0x04001BB3 RID: 7091
		public const int TLS_DH_RSA_WITH_CAMELLIA_128_CBC_SHA256 = 188;

		// Token: 0x04001BB4 RID: 7092
		public const int TLS_DHE_DSS_WITH_CAMELLIA_128_CBC_SHA256 = 189;

		// Token: 0x04001BB5 RID: 7093
		public const int TLS_DHE_RSA_WITH_CAMELLIA_128_CBC_SHA256 = 190;

		// Token: 0x04001BB6 RID: 7094
		public const int TLS_DH_anon_WITH_CAMELLIA_128_CBC_SHA256 = 191;

		// Token: 0x04001BB7 RID: 7095
		public const int TLS_RSA_WITH_CAMELLIA_256_CBC_SHA256 = 192;

		// Token: 0x04001BB8 RID: 7096
		public const int TLS_DH_DSS_WITH_CAMELLIA_256_CBC_SHA256 = 193;

		// Token: 0x04001BB9 RID: 7097
		public const int TLS_DH_RSA_WITH_CAMELLIA_256_CBC_SHA256 = 194;

		// Token: 0x04001BBA RID: 7098
		public const int TLS_DHE_DSS_WITH_CAMELLIA_256_CBC_SHA256 = 195;

		// Token: 0x04001BBB RID: 7099
		public const int TLS_DHE_RSA_WITH_CAMELLIA_256_CBC_SHA256 = 196;

		// Token: 0x04001BBC RID: 7100
		public const int TLS_DH_anon_WITH_CAMELLIA_256_CBC_SHA256 = 197;

		// Token: 0x04001BBD RID: 7101
		public const int TLS_RSA_WITH_SEED_CBC_SHA = 150;

		// Token: 0x04001BBE RID: 7102
		public const int TLS_DH_DSS_WITH_SEED_CBC_SHA = 151;

		// Token: 0x04001BBF RID: 7103
		public const int TLS_DH_RSA_WITH_SEED_CBC_SHA = 152;

		// Token: 0x04001BC0 RID: 7104
		public const int TLS_DHE_DSS_WITH_SEED_CBC_SHA = 153;

		// Token: 0x04001BC1 RID: 7105
		public const int TLS_DHE_RSA_WITH_SEED_CBC_SHA = 154;

		// Token: 0x04001BC2 RID: 7106
		public const int TLS_DH_anon_WITH_SEED_CBC_SHA = 155;

		// Token: 0x04001BC3 RID: 7107
		public const int TLS_PSK_WITH_RC4_128_SHA = 138;

		// Token: 0x04001BC4 RID: 7108
		public const int TLS_PSK_WITH_3DES_EDE_CBC_SHA = 139;

		// Token: 0x04001BC5 RID: 7109
		public const int TLS_PSK_WITH_AES_128_CBC_SHA = 140;

		// Token: 0x04001BC6 RID: 7110
		public const int TLS_PSK_WITH_AES_256_CBC_SHA = 141;

		// Token: 0x04001BC7 RID: 7111
		public const int TLS_DHE_PSK_WITH_RC4_128_SHA = 142;

		// Token: 0x04001BC8 RID: 7112
		public const int TLS_DHE_PSK_WITH_3DES_EDE_CBC_SHA = 143;

		// Token: 0x04001BC9 RID: 7113
		public const int TLS_DHE_PSK_WITH_AES_128_CBC_SHA = 144;

		// Token: 0x04001BCA RID: 7114
		public const int TLS_DHE_PSK_WITH_AES_256_CBC_SHA = 145;

		// Token: 0x04001BCB RID: 7115
		public const int TLS_RSA_PSK_WITH_RC4_128_SHA = 146;

		// Token: 0x04001BCC RID: 7116
		public const int TLS_RSA_PSK_WITH_3DES_EDE_CBC_SHA = 147;

		// Token: 0x04001BCD RID: 7117
		public const int TLS_RSA_PSK_WITH_AES_128_CBC_SHA = 148;

		// Token: 0x04001BCE RID: 7118
		public const int TLS_RSA_PSK_WITH_AES_256_CBC_SHA = 149;

		// Token: 0x04001BCF RID: 7119
		public const int TLS_ECDH_ECDSA_WITH_NULL_SHA = 49153;

		// Token: 0x04001BD0 RID: 7120
		public const int TLS_ECDH_ECDSA_WITH_RC4_128_SHA = 49154;

		// Token: 0x04001BD1 RID: 7121
		public const int TLS_ECDH_ECDSA_WITH_3DES_EDE_CBC_SHA = 49155;

		// Token: 0x04001BD2 RID: 7122
		public const int TLS_ECDH_ECDSA_WITH_AES_128_CBC_SHA = 49156;

		// Token: 0x04001BD3 RID: 7123
		public const int TLS_ECDH_ECDSA_WITH_AES_256_CBC_SHA = 49157;

		// Token: 0x04001BD4 RID: 7124
		public const int TLS_ECDHE_ECDSA_WITH_NULL_SHA = 49158;

		// Token: 0x04001BD5 RID: 7125
		public const int TLS_ECDHE_ECDSA_WITH_RC4_128_SHA = 49159;

		// Token: 0x04001BD6 RID: 7126
		public const int TLS_ECDHE_ECDSA_WITH_3DES_EDE_CBC_SHA = 49160;

		// Token: 0x04001BD7 RID: 7127
		public const int TLS_ECDHE_ECDSA_WITH_AES_128_CBC_SHA = 49161;

		// Token: 0x04001BD8 RID: 7128
		public const int TLS_ECDHE_ECDSA_WITH_AES_256_CBC_SHA = 49162;

		// Token: 0x04001BD9 RID: 7129
		public const int TLS_ECDH_RSA_WITH_NULL_SHA = 49163;

		// Token: 0x04001BDA RID: 7130
		public const int TLS_ECDH_RSA_WITH_RC4_128_SHA = 49164;

		// Token: 0x04001BDB RID: 7131
		public const int TLS_ECDH_RSA_WITH_3DES_EDE_CBC_SHA = 49165;

		// Token: 0x04001BDC RID: 7132
		public const int TLS_ECDH_RSA_WITH_AES_128_CBC_SHA = 49166;

		// Token: 0x04001BDD RID: 7133
		public const int TLS_ECDH_RSA_WITH_AES_256_CBC_SHA = 49167;

		// Token: 0x04001BDE RID: 7134
		public const int TLS_ECDHE_RSA_WITH_NULL_SHA = 49168;

		// Token: 0x04001BDF RID: 7135
		public const int TLS_ECDHE_RSA_WITH_RC4_128_SHA = 49169;

		// Token: 0x04001BE0 RID: 7136
		public const int TLS_ECDHE_RSA_WITH_3DES_EDE_CBC_SHA = 49170;

		// Token: 0x04001BE1 RID: 7137
		public const int TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA = 49171;

		// Token: 0x04001BE2 RID: 7138
		public const int TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA = 49172;

		// Token: 0x04001BE3 RID: 7139
		public const int TLS_ECDH_anon_WITH_NULL_SHA = 49173;

		// Token: 0x04001BE4 RID: 7140
		public const int TLS_ECDH_anon_WITH_RC4_128_SHA = 49174;

		// Token: 0x04001BE5 RID: 7141
		public const int TLS_ECDH_anon_WITH_3DES_EDE_CBC_SHA = 49175;

		// Token: 0x04001BE6 RID: 7142
		public const int TLS_ECDH_anon_WITH_AES_128_CBC_SHA = 49176;

		// Token: 0x04001BE7 RID: 7143
		public const int TLS_ECDH_anon_WITH_AES_256_CBC_SHA = 49177;

		// Token: 0x04001BE8 RID: 7144
		public const int TLS_PSK_WITH_NULL_SHA = 44;

		// Token: 0x04001BE9 RID: 7145
		public const int TLS_DHE_PSK_WITH_NULL_SHA = 45;

		// Token: 0x04001BEA RID: 7146
		public const int TLS_RSA_PSK_WITH_NULL_SHA = 46;

		// Token: 0x04001BEB RID: 7147
		public const int TLS_SRP_SHA_WITH_3DES_EDE_CBC_SHA = 49178;

		// Token: 0x04001BEC RID: 7148
		public const int TLS_SRP_SHA_RSA_WITH_3DES_EDE_CBC_SHA = 49179;

		// Token: 0x04001BED RID: 7149
		public const int TLS_SRP_SHA_DSS_WITH_3DES_EDE_CBC_SHA = 49180;

		// Token: 0x04001BEE RID: 7150
		public const int TLS_SRP_SHA_WITH_AES_128_CBC_SHA = 49181;

		// Token: 0x04001BEF RID: 7151
		public const int TLS_SRP_SHA_RSA_WITH_AES_128_CBC_SHA = 49182;

		// Token: 0x04001BF0 RID: 7152
		public const int TLS_SRP_SHA_DSS_WITH_AES_128_CBC_SHA = 49183;

		// Token: 0x04001BF1 RID: 7153
		public const int TLS_SRP_SHA_WITH_AES_256_CBC_SHA = 49184;

		// Token: 0x04001BF2 RID: 7154
		public const int TLS_SRP_SHA_RSA_WITH_AES_256_CBC_SHA = 49185;

		// Token: 0x04001BF3 RID: 7155
		public const int TLS_SRP_SHA_DSS_WITH_AES_256_CBC_SHA = 49186;

		// Token: 0x04001BF4 RID: 7156
		public const int TLS_RSA_WITH_NULL_SHA256 = 59;

		// Token: 0x04001BF5 RID: 7157
		public const int TLS_RSA_WITH_AES_128_CBC_SHA256 = 60;

		// Token: 0x04001BF6 RID: 7158
		public const int TLS_RSA_WITH_AES_256_CBC_SHA256 = 61;

		// Token: 0x04001BF7 RID: 7159
		public const int TLS_DH_DSS_WITH_AES_128_CBC_SHA256 = 62;

		// Token: 0x04001BF8 RID: 7160
		public const int TLS_DH_RSA_WITH_AES_128_CBC_SHA256 = 63;

		// Token: 0x04001BF9 RID: 7161
		public const int TLS_DHE_DSS_WITH_AES_128_CBC_SHA256 = 64;

		// Token: 0x04001BFA RID: 7162
		public const int TLS_DHE_RSA_WITH_AES_128_CBC_SHA256 = 103;

		// Token: 0x04001BFB RID: 7163
		public const int TLS_DH_DSS_WITH_AES_256_CBC_SHA256 = 104;

		// Token: 0x04001BFC RID: 7164
		public const int TLS_DH_RSA_WITH_AES_256_CBC_SHA256 = 105;

		// Token: 0x04001BFD RID: 7165
		public const int TLS_DHE_DSS_WITH_AES_256_CBC_SHA256 = 106;

		// Token: 0x04001BFE RID: 7166
		public const int TLS_DHE_RSA_WITH_AES_256_CBC_SHA256 = 107;

		// Token: 0x04001BFF RID: 7167
		public const int TLS_DH_anon_WITH_AES_128_CBC_SHA256 = 108;

		// Token: 0x04001C00 RID: 7168
		public const int TLS_DH_anon_WITH_AES_256_CBC_SHA256 = 109;

		// Token: 0x04001C01 RID: 7169
		public const int TLS_RSA_WITH_AES_128_GCM_SHA256 = 156;

		// Token: 0x04001C02 RID: 7170
		public const int TLS_RSA_WITH_AES_256_GCM_SHA384 = 157;

		// Token: 0x04001C03 RID: 7171
		public const int TLS_DHE_RSA_WITH_AES_128_GCM_SHA256 = 158;

		// Token: 0x04001C04 RID: 7172
		public const int TLS_DHE_RSA_WITH_AES_256_GCM_SHA384 = 159;

		// Token: 0x04001C05 RID: 7173
		public const int TLS_DH_RSA_WITH_AES_128_GCM_SHA256 = 160;

		// Token: 0x04001C06 RID: 7174
		public const int TLS_DH_RSA_WITH_AES_256_GCM_SHA384 = 161;

		// Token: 0x04001C07 RID: 7175
		public const int TLS_DHE_DSS_WITH_AES_128_GCM_SHA256 = 162;

		// Token: 0x04001C08 RID: 7176
		public const int TLS_DHE_DSS_WITH_AES_256_GCM_SHA384 = 163;

		// Token: 0x04001C09 RID: 7177
		public const int TLS_DH_DSS_WITH_AES_128_GCM_SHA256 = 164;

		// Token: 0x04001C0A RID: 7178
		public const int TLS_DH_DSS_WITH_AES_256_GCM_SHA384 = 165;

		// Token: 0x04001C0B RID: 7179
		public const int TLS_DH_anon_WITH_AES_128_GCM_SHA256 = 166;

		// Token: 0x04001C0C RID: 7180
		public const int TLS_DH_anon_WITH_AES_256_GCM_SHA384 = 167;

		// Token: 0x04001C0D RID: 7181
		public const int TLS_ECDHE_ECDSA_WITH_AES_128_CBC_SHA256 = 49187;

		// Token: 0x04001C0E RID: 7182
		public const int TLS_ECDHE_ECDSA_WITH_AES_256_CBC_SHA384 = 49188;

		// Token: 0x04001C0F RID: 7183
		public const int TLS_ECDH_ECDSA_WITH_AES_128_CBC_SHA256 = 49189;

		// Token: 0x04001C10 RID: 7184
		public const int TLS_ECDH_ECDSA_WITH_AES_256_CBC_SHA384 = 49190;

		// Token: 0x04001C11 RID: 7185
		public const int TLS_ECDHE_RSA_WITH_AES_128_CBC_SHA256 = 49191;

		// Token: 0x04001C12 RID: 7186
		public const int TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA384 = 49192;

		// Token: 0x04001C13 RID: 7187
		public const int TLS_ECDH_RSA_WITH_AES_128_CBC_SHA256 = 49193;

		// Token: 0x04001C14 RID: 7188
		public const int TLS_ECDH_RSA_WITH_AES_256_CBC_SHA384 = 49194;

		// Token: 0x04001C15 RID: 7189
		public const int TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256 = 49195;

		// Token: 0x04001C16 RID: 7190
		public const int TLS_ECDHE_ECDSA_WITH_AES_256_GCM_SHA384 = 49196;

		// Token: 0x04001C17 RID: 7191
		public const int TLS_ECDH_ECDSA_WITH_AES_128_GCM_SHA256 = 49197;

		// Token: 0x04001C18 RID: 7192
		public const int TLS_ECDH_ECDSA_WITH_AES_256_GCM_SHA384 = 49198;

		// Token: 0x04001C19 RID: 7193
		public const int TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256 = 49199;

		// Token: 0x04001C1A RID: 7194
		public const int TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384 = 49200;

		// Token: 0x04001C1B RID: 7195
		public const int TLS_ECDH_RSA_WITH_AES_128_GCM_SHA256 = 49201;

		// Token: 0x04001C1C RID: 7196
		public const int TLS_ECDH_RSA_WITH_AES_256_GCM_SHA384 = 49202;

		// Token: 0x04001C1D RID: 7197
		public const int TLS_PSK_WITH_AES_128_GCM_SHA256 = 168;

		// Token: 0x04001C1E RID: 7198
		public const int TLS_PSK_WITH_AES_256_GCM_SHA384 = 169;

		// Token: 0x04001C1F RID: 7199
		public const int TLS_DHE_PSK_WITH_AES_128_GCM_SHA256 = 170;

		// Token: 0x04001C20 RID: 7200
		public const int TLS_DHE_PSK_WITH_AES_256_GCM_SHA384 = 171;

		// Token: 0x04001C21 RID: 7201
		public const int TLS_RSA_PSK_WITH_AES_128_GCM_SHA256 = 172;

		// Token: 0x04001C22 RID: 7202
		public const int TLS_RSA_PSK_WITH_AES_256_GCM_SHA384 = 173;

		// Token: 0x04001C23 RID: 7203
		public const int TLS_PSK_WITH_AES_128_CBC_SHA256 = 174;

		// Token: 0x04001C24 RID: 7204
		public const int TLS_PSK_WITH_AES_256_CBC_SHA384 = 175;

		// Token: 0x04001C25 RID: 7205
		public const int TLS_PSK_WITH_NULL_SHA256 = 176;

		// Token: 0x04001C26 RID: 7206
		public const int TLS_PSK_WITH_NULL_SHA384 = 177;

		// Token: 0x04001C27 RID: 7207
		public const int TLS_DHE_PSK_WITH_AES_128_CBC_SHA256 = 178;

		// Token: 0x04001C28 RID: 7208
		public const int TLS_DHE_PSK_WITH_AES_256_CBC_SHA384 = 179;

		// Token: 0x04001C29 RID: 7209
		public const int TLS_DHE_PSK_WITH_NULL_SHA256 = 180;

		// Token: 0x04001C2A RID: 7210
		public const int TLS_DHE_PSK_WITH_NULL_SHA384 = 181;

		// Token: 0x04001C2B RID: 7211
		public const int TLS_RSA_PSK_WITH_AES_128_CBC_SHA256 = 182;

		// Token: 0x04001C2C RID: 7212
		public const int TLS_RSA_PSK_WITH_AES_256_CBC_SHA384 = 183;

		// Token: 0x04001C2D RID: 7213
		public const int TLS_RSA_PSK_WITH_NULL_SHA256 = 184;

		// Token: 0x04001C2E RID: 7214
		public const int TLS_RSA_PSK_WITH_NULL_SHA384 = 185;

		// Token: 0x04001C2F RID: 7215
		public const int TLS_ECDHE_PSK_WITH_RC4_128_SHA = 49203;

		// Token: 0x04001C30 RID: 7216
		public const int TLS_ECDHE_PSK_WITH_3DES_EDE_CBC_SHA = 49204;

		// Token: 0x04001C31 RID: 7217
		public const int TLS_ECDHE_PSK_WITH_AES_128_CBC_SHA = 49205;

		// Token: 0x04001C32 RID: 7218
		public const int TLS_ECDHE_PSK_WITH_AES_256_CBC_SHA = 49206;

		// Token: 0x04001C33 RID: 7219
		public const int TLS_ECDHE_PSK_WITH_AES_128_CBC_SHA256 = 49207;

		// Token: 0x04001C34 RID: 7220
		public const int TLS_ECDHE_PSK_WITH_AES_256_CBC_SHA384 = 49208;

		// Token: 0x04001C35 RID: 7221
		public const int TLS_ECDHE_PSK_WITH_NULL_SHA = 49209;

		// Token: 0x04001C36 RID: 7222
		public const int TLS_ECDHE_PSK_WITH_NULL_SHA256 = 49210;

		// Token: 0x04001C37 RID: 7223
		public const int TLS_ECDHE_PSK_WITH_NULL_SHA384 = 49211;

		// Token: 0x04001C38 RID: 7224
		public const int TLS_EMPTY_RENEGOTIATION_INFO_SCSV = 255;

		// Token: 0x04001C39 RID: 7225
		public const int TLS_ECDHE_ECDSA_WITH_CAMELLIA_128_CBC_SHA256 = 49266;

		// Token: 0x04001C3A RID: 7226
		public const int TLS_ECDHE_ECDSA_WITH_CAMELLIA_256_CBC_SHA384 = 49267;

		// Token: 0x04001C3B RID: 7227
		public const int TLS_ECDH_ECDSA_WITH_CAMELLIA_128_CBC_SHA256 = 49268;

		// Token: 0x04001C3C RID: 7228
		public const int TLS_ECDH_ECDSA_WITH_CAMELLIA_256_CBC_SHA384 = 49269;

		// Token: 0x04001C3D RID: 7229
		public const int TLS_ECDHE_RSA_WITH_CAMELLIA_128_CBC_SHA256 = 49270;

		// Token: 0x04001C3E RID: 7230
		public const int TLS_ECDHE_RSA_WITH_CAMELLIA_256_CBC_SHA384 = 49271;

		// Token: 0x04001C3F RID: 7231
		public const int TLS_ECDH_RSA_WITH_CAMELLIA_128_CBC_SHA256 = 49272;

		// Token: 0x04001C40 RID: 7232
		public const int TLS_ECDH_RSA_WITH_CAMELLIA_256_CBC_SHA384 = 49273;

		// Token: 0x04001C41 RID: 7233
		public const int TLS_RSA_WITH_CAMELLIA_128_GCM_SHA256 = 49274;

		// Token: 0x04001C42 RID: 7234
		public const int TLS_RSA_WITH_CAMELLIA_256_GCM_SHA384 = 49275;

		// Token: 0x04001C43 RID: 7235
		public const int TLS_DHE_RSA_WITH_CAMELLIA_128_GCM_SHA256 = 49276;

		// Token: 0x04001C44 RID: 7236
		public const int TLS_DHE_RSA_WITH_CAMELLIA_256_GCM_SHA384 = 49277;

		// Token: 0x04001C45 RID: 7237
		public const int TLS_DH_RSA_WITH_CAMELLIA_128_GCM_SHA256 = 49278;

		// Token: 0x04001C46 RID: 7238
		public const int TLS_DH_RSA_WITH_CAMELLIA_256_GCM_SHA384 = 49279;

		// Token: 0x04001C47 RID: 7239
		public const int TLS_DHE_DSS_WITH_CAMELLIA_128_GCM_SHA256 = 49280;

		// Token: 0x04001C48 RID: 7240
		public const int TLS_DHE_DSS_WITH_CAMELLIA_256_GCM_SHA384 = 49281;

		// Token: 0x04001C49 RID: 7241
		public const int TLS_DH_DSS_WITH_CAMELLIA_128_GCM_SHA256 = 49282;

		// Token: 0x04001C4A RID: 7242
		public const int TLS_DH_DSS_WITH_CAMELLIA_256_GCM_SHA384 = 49283;

		// Token: 0x04001C4B RID: 7243
		public const int TLS_DH_anon_WITH_CAMELLIA_128_GCM_SHA256 = 49284;

		// Token: 0x04001C4C RID: 7244
		public const int TLS_DH_anon_WITH_CAMELLIA_256_GCM_SHA384 = 49285;

		// Token: 0x04001C4D RID: 7245
		public const int TLS_ECDHE_ECDSA_WITH_CAMELLIA_128_GCM_SHA256 = 49286;

		// Token: 0x04001C4E RID: 7246
		public const int TLS_ECDHE_ECDSA_WITH_CAMELLIA_256_GCM_SHA384 = 49287;

		// Token: 0x04001C4F RID: 7247
		public const int TLS_ECDH_ECDSA_WITH_CAMELLIA_128_GCM_SHA256 = 49288;

		// Token: 0x04001C50 RID: 7248
		public const int TLS_ECDH_ECDSA_WITH_CAMELLIA_256_GCM_SHA384 = 49289;

		// Token: 0x04001C51 RID: 7249
		public const int TLS_ECDHE_RSA_WITH_CAMELLIA_128_GCM_SHA256 = 49290;

		// Token: 0x04001C52 RID: 7250
		public const int TLS_ECDHE_RSA_WITH_CAMELLIA_256_GCM_SHA384 = 49291;

		// Token: 0x04001C53 RID: 7251
		public const int TLS_ECDH_RSA_WITH_CAMELLIA_128_GCM_SHA256 = 49292;

		// Token: 0x04001C54 RID: 7252
		public const int TLS_ECDH_RSA_WITH_CAMELLIA_256_GCM_SHA384 = 49293;

		// Token: 0x04001C55 RID: 7253
		public const int TLS_PSK_WITH_CAMELLIA_128_GCM_SHA256 = 49294;

		// Token: 0x04001C56 RID: 7254
		public const int TLS_PSK_WITH_CAMELLIA_256_GCM_SHA384 = 49295;

		// Token: 0x04001C57 RID: 7255
		public const int TLS_DHE_PSK_WITH_CAMELLIA_128_GCM_SHA256 = 49296;

		// Token: 0x04001C58 RID: 7256
		public const int TLS_DHE_PSK_WITH_CAMELLIA_256_GCM_SHA384 = 49297;

		// Token: 0x04001C59 RID: 7257
		public const int TLS_RSA_PSK_WITH_CAMELLIA_128_GCM_SHA256 = 49298;

		// Token: 0x04001C5A RID: 7258
		public const int TLS_RSA_PSK_WITH_CAMELLIA_256_GCM_SHA384 = 49299;

		// Token: 0x04001C5B RID: 7259
		public const int TLS_PSK_WITH_CAMELLIA_128_CBC_SHA256 = 49300;

		// Token: 0x04001C5C RID: 7260
		public const int TLS_PSK_WITH_CAMELLIA_256_CBC_SHA384 = 49301;

		// Token: 0x04001C5D RID: 7261
		public const int TLS_DHE_PSK_WITH_CAMELLIA_128_CBC_SHA256 = 49302;

		// Token: 0x04001C5E RID: 7262
		public const int TLS_DHE_PSK_WITH_CAMELLIA_256_CBC_SHA384 = 49303;

		// Token: 0x04001C5F RID: 7263
		public const int TLS_RSA_PSK_WITH_CAMELLIA_128_CBC_SHA256 = 49304;

		// Token: 0x04001C60 RID: 7264
		public const int TLS_RSA_PSK_WITH_CAMELLIA_256_CBC_SHA384 = 49305;

		// Token: 0x04001C61 RID: 7265
		public const int TLS_ECDHE_PSK_WITH_CAMELLIA_128_CBC_SHA256 = 49306;

		// Token: 0x04001C62 RID: 7266
		public const int TLS_ECDHE_PSK_WITH_CAMELLIA_256_CBC_SHA384 = 49307;

		// Token: 0x04001C63 RID: 7267
		public const int TLS_RSA_WITH_AES_128_CCM = 49308;

		// Token: 0x04001C64 RID: 7268
		public const int TLS_RSA_WITH_AES_256_CCM = 49309;

		// Token: 0x04001C65 RID: 7269
		public const int TLS_DHE_RSA_WITH_AES_128_CCM = 49310;

		// Token: 0x04001C66 RID: 7270
		public const int TLS_DHE_RSA_WITH_AES_256_CCM = 49311;

		// Token: 0x04001C67 RID: 7271
		public const int TLS_RSA_WITH_AES_128_CCM_8 = 49312;

		// Token: 0x04001C68 RID: 7272
		public const int TLS_RSA_WITH_AES_256_CCM_8 = 49313;

		// Token: 0x04001C69 RID: 7273
		public const int TLS_DHE_RSA_WITH_AES_128_CCM_8 = 49314;

		// Token: 0x04001C6A RID: 7274
		public const int TLS_DHE_RSA_WITH_AES_256_CCM_8 = 49315;

		// Token: 0x04001C6B RID: 7275
		public const int TLS_PSK_WITH_AES_128_CCM = 49316;

		// Token: 0x04001C6C RID: 7276
		public const int TLS_PSK_WITH_AES_256_CCM = 49317;

		// Token: 0x04001C6D RID: 7277
		public const int TLS_DHE_PSK_WITH_AES_128_CCM = 49318;

		// Token: 0x04001C6E RID: 7278
		public const int TLS_DHE_PSK_WITH_AES_256_CCM = 49319;

		// Token: 0x04001C6F RID: 7279
		public const int TLS_PSK_WITH_AES_128_CCM_8 = 49320;

		// Token: 0x04001C70 RID: 7280
		public const int TLS_PSK_WITH_AES_256_CCM_8 = 49321;

		// Token: 0x04001C71 RID: 7281
		public const int TLS_PSK_DHE_WITH_AES_128_CCM_8 = 49322;

		// Token: 0x04001C72 RID: 7282
		public const int TLS_PSK_DHE_WITH_AES_256_CCM_8 = 49323;

		// Token: 0x04001C73 RID: 7283
		public const int TLS_ECDHE_ECDSA_WITH_AES_128_CCM = 49324;

		// Token: 0x04001C74 RID: 7284
		public const int TLS_ECDHE_ECDSA_WITH_AES_256_CCM = 49325;

		// Token: 0x04001C75 RID: 7285
		public const int TLS_ECDHE_ECDSA_WITH_AES_128_CCM_8 = 49326;

		// Token: 0x04001C76 RID: 7286
		public const int TLS_ECDHE_ECDSA_WITH_AES_256_CCM_8 = 49327;

		// Token: 0x04001C77 RID: 7287
		public const int TLS_FALLBACK_SCSV = 22016;

		// Token: 0x04001C78 RID: 7288
		public const int DRAFT_TLS_ECDHE_RSA_WITH_CHACHA20_POLY1305_SHA256 = 52392;

		// Token: 0x04001C79 RID: 7289
		public const int DRAFT_TLS_ECDHE_ECDSA_WITH_CHACHA20_POLY1305_SHA256 = 52393;

		// Token: 0x04001C7A RID: 7290
		public const int DRAFT_TLS_DHE_RSA_WITH_CHACHA20_POLY1305_SHA256 = 52394;

		// Token: 0x04001C7B RID: 7291
		public const int DRAFT_TLS_PSK_WITH_CHACHA20_POLY1305_SHA256 = 52395;

		// Token: 0x04001C7C RID: 7292
		public const int DRAFT_TLS_ECDHE_PSK_WITH_CHACHA20_POLY1305_SHA256 = 52396;

		// Token: 0x04001C7D RID: 7293
		public const int DRAFT_TLS_DHE_PSK_WITH_CHACHA20_POLY1305_SHA256 = 52397;

		// Token: 0x04001C7E RID: 7294
		public const int DRAFT_TLS_RSA_PSK_WITH_CHACHA20_POLY1305_SHA256 = 52398;

		// Token: 0x04001C7F RID: 7295
		public const int DRAFT_TLS_DHE_RSA_WITH_AES_128_OCB = 65280;

		// Token: 0x04001C80 RID: 7296
		public const int DRAFT_TLS_DHE_RSA_WITH_AES_256_OCB = 65281;

		// Token: 0x04001C81 RID: 7297
		public const int DRAFT_TLS_ECDHE_RSA_WITH_AES_128_OCB = 65282;

		// Token: 0x04001C82 RID: 7298
		public const int DRAFT_TLS_ECDHE_RSA_WITH_AES_256_OCB = 65283;

		// Token: 0x04001C83 RID: 7299
		public const int DRAFT_TLS_ECDHE_ECDSA_WITH_AES_128_OCB = 65284;

		// Token: 0x04001C84 RID: 7300
		public const int DRAFT_TLS_ECDHE_ECDSA_WITH_AES_256_OCB = 65285;

		// Token: 0x04001C85 RID: 7301
		public const int DRAFT_TLS_PSK_WITH_AES_128_OCB = 65296;

		// Token: 0x04001C86 RID: 7302
		public const int DRAFT_TLS_PSK_WITH_AES_256_OCB = 65297;

		// Token: 0x04001C87 RID: 7303
		public const int DRAFT_TLS_DHE_PSK_WITH_AES_128_OCB = 65298;

		// Token: 0x04001C88 RID: 7304
		public const int DRAFT_TLS_DHE_PSK_WITH_AES_256_OCB = 65299;

		// Token: 0x04001C89 RID: 7305
		public const int DRAFT_TLS_ECDHE_PSK_WITH_AES_128_OCB = 65300;

		// Token: 0x04001C8A RID: 7306
		public const int DRAFT_TLS_ECDHE_PSK_WITH_AES_256_OCB = 65301;
	}
}
