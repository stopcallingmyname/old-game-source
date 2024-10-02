using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003FB RID: 1019
	public abstract class AlertDescription
	{
		// Token: 0x06002946 RID: 10566 RVA: 0x0010DCE8 File Offset: 0x0010BEE8
		public static string GetName(byte alertDescription)
		{
			if (alertDescription <= 70)
			{
				if (alertDescription <= 22)
				{
					if (alertDescription == 0)
					{
						return "close_notify";
					}
					if (alertDescription == 10)
					{
						return "unexpected_message";
					}
					switch (alertDescription)
					{
					case 20:
						return "bad_record_mac";
					case 21:
						return "decryption_failed";
					case 22:
						return "record_overflow";
					}
				}
				else
				{
					switch (alertDescription)
					{
					case 30:
						return "decompression_failure";
					case 31:
					case 32:
					case 33:
					case 34:
					case 35:
					case 36:
					case 37:
					case 38:
					case 39:
						break;
					case 40:
						return "handshake_failure";
					case 41:
						return "no_certificate";
					case 42:
						return "bad_certificate";
					case 43:
						return "unsupported_certificate";
					case 44:
						return "certificate_revoked";
					case 45:
						return "certificate_expired";
					case 46:
						return "certificate_unknown";
					case 47:
						return "illegal_parameter";
					case 48:
						return "unknown_ca";
					case 49:
						return "access_denied";
					case 50:
						return "decode_error";
					case 51:
						return "decrypt_error";
					default:
						if (alertDescription == 60)
						{
							return "export_restriction";
						}
						if (alertDescription == 70)
						{
							return "protocol_version";
						}
						break;
					}
				}
			}
			else if (alertDescription <= 86)
			{
				if (alertDescription == 71)
				{
					return "insufficient_security";
				}
				if (alertDescription == 80)
				{
					return "internal_error";
				}
				if (alertDescription == 86)
				{
					return "inappropriate_fallback";
				}
			}
			else
			{
				if (alertDescription == 90)
				{
					return "user_canceled";
				}
				if (alertDescription == 100)
				{
					return "no_renegotiation";
				}
				switch (alertDescription)
				{
				case 110:
					return "unsupported_extension";
				case 111:
					return "certificate_unobtainable";
				case 112:
					return "unrecognized_name";
				case 113:
					return "bad_certificate_status_response";
				case 114:
					return "bad_certificate_hash_value";
				case 115:
					return "unknown_psk_identity";
				}
			}
			return "UNKNOWN";
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x0010DEB7 File Offset: 0x0010C0B7
		public static string GetText(byte alertDescription)
		{
			return string.Concat(new object[]
			{
				AlertDescription.GetName(alertDescription),
				"(",
				alertDescription,
				")"
			});
		}

		// Token: 0x04001B34 RID: 6964
		public const byte close_notify = 0;

		// Token: 0x04001B35 RID: 6965
		public const byte unexpected_message = 10;

		// Token: 0x04001B36 RID: 6966
		public const byte bad_record_mac = 20;

		// Token: 0x04001B37 RID: 6967
		public const byte decryption_failed = 21;

		// Token: 0x04001B38 RID: 6968
		public const byte record_overflow = 22;

		// Token: 0x04001B39 RID: 6969
		public const byte decompression_failure = 30;

		// Token: 0x04001B3A RID: 6970
		public const byte handshake_failure = 40;

		// Token: 0x04001B3B RID: 6971
		public const byte no_certificate = 41;

		// Token: 0x04001B3C RID: 6972
		public const byte bad_certificate = 42;

		// Token: 0x04001B3D RID: 6973
		public const byte unsupported_certificate = 43;

		// Token: 0x04001B3E RID: 6974
		public const byte certificate_revoked = 44;

		// Token: 0x04001B3F RID: 6975
		public const byte certificate_expired = 45;

		// Token: 0x04001B40 RID: 6976
		public const byte certificate_unknown = 46;

		// Token: 0x04001B41 RID: 6977
		public const byte illegal_parameter = 47;

		// Token: 0x04001B42 RID: 6978
		public const byte unknown_ca = 48;

		// Token: 0x04001B43 RID: 6979
		public const byte access_denied = 49;

		// Token: 0x04001B44 RID: 6980
		public const byte decode_error = 50;

		// Token: 0x04001B45 RID: 6981
		public const byte decrypt_error = 51;

		// Token: 0x04001B46 RID: 6982
		public const byte export_restriction = 60;

		// Token: 0x04001B47 RID: 6983
		public const byte protocol_version = 70;

		// Token: 0x04001B48 RID: 6984
		public const byte insufficient_security = 71;

		// Token: 0x04001B49 RID: 6985
		public const byte internal_error = 80;

		// Token: 0x04001B4A RID: 6986
		public const byte user_canceled = 90;

		// Token: 0x04001B4B RID: 6987
		public const byte no_renegotiation = 100;

		// Token: 0x04001B4C RID: 6988
		public const byte unsupported_extension = 110;

		// Token: 0x04001B4D RID: 6989
		public const byte certificate_unobtainable = 111;

		// Token: 0x04001B4E RID: 6990
		public const byte unrecognized_name = 112;

		// Token: 0x04001B4F RID: 6991
		public const byte bad_certificate_status_response = 113;

		// Token: 0x04001B50 RID: 6992
		public const byte bad_certificate_hash_value = 114;

		// Token: 0x04001B51 RID: 6993
		public const byte unknown_psk_identity = 115;

		// Token: 0x04001B52 RID: 6994
		public const byte inappropriate_fallback = 86;
	}
}
