using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000422 RID: 1058
	public abstract class DtlsProtocol
	{
		// Token: 0x06002A44 RID: 10820 RVA: 0x001111EF File Offset: 0x0010F3EF
		protected DtlsProtocol(SecureRandom secureRandom)
		{
			if (secureRandom == null)
			{
				throw new ArgumentNullException("secureRandom");
			}
			this.mSecureRandom = secureRandom;
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x0011120C File Offset: 0x0010F40C
		protected virtual void ProcessFinished(byte[] body, byte[] expected_verify_data)
		{
			MemoryStream memoryStream = new MemoryStream(body, false);
			byte[] b = TlsUtilities.ReadFully(expected_verify_data.Length, memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			if (!Arrays.ConstantTimeAreEqual(expected_verify_data, b))
			{
				throw new TlsFatalAlert(40);
			}
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x00111244 File Offset: 0x0010F444
		internal static void ApplyMaxFragmentLengthExtension(DtlsRecordLayer recordLayer, short maxFragmentLength)
		{
			if (maxFragmentLength >= 0)
			{
				if (!MaxFragmentLength.IsValid((byte)maxFragmentLength))
				{
					throw new TlsFatalAlert(80);
				}
				int plaintextLimit = 1 << (int)(8 + maxFragmentLength);
				recordLayer.SetPlaintextLimit(plaintextLimit);
			}
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x00111278 File Offset: 0x0010F478
		protected static short EvaluateMaxFragmentLengthExtension(bool resumedSession, IDictionary clientExtensions, IDictionary serverExtensions, byte alertDescription)
		{
			short maxFragmentLengthExtension = TlsExtensionsUtilities.GetMaxFragmentLengthExtension(serverExtensions);
			if (maxFragmentLengthExtension >= 0 && (!MaxFragmentLength.IsValid((byte)maxFragmentLengthExtension) || (!resumedSession && maxFragmentLengthExtension != TlsExtensionsUtilities.GetMaxFragmentLengthExtension(clientExtensions))))
			{
				throw new TlsFatalAlert(alertDescription);
			}
			return maxFragmentLengthExtension;
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x001112B0 File Offset: 0x0010F4B0
		protected static byte[] GenerateCertificate(Certificate certificate)
		{
			MemoryStream memoryStream = new MemoryStream();
			certificate.Encode(memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x001112D0 File Offset: 0x0010F4D0
		protected static byte[] GenerateSupplementalData(IList supplementalData)
		{
			MemoryStream memoryStream = new MemoryStream();
			TlsProtocol.WriteSupplementalData(memoryStream, supplementalData);
			return memoryStream.ToArray();
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x001112E4 File Offset: 0x0010F4E4
		protected static void ValidateSelectedCipherSuite(int selectedCipherSuite, byte alertDescription)
		{
			int encryptionAlgorithm = TlsUtilities.GetEncryptionAlgorithm(selectedCipherSuite);
			if (encryptionAlgorithm - 1 <= 1)
			{
				throw new TlsFatalAlert(alertDescription);
			}
		}

		// Token: 0x04001CC5 RID: 7365
		protected readonly SecureRandom mSecureRandom;
	}
}
