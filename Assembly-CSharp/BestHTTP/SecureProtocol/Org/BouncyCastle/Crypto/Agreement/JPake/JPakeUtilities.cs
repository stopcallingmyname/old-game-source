using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.JPake
{
	// Token: 0x020005DD RID: 1501
	public abstract class JPakeUtilities
	{
		// Token: 0x06003956 RID: 14678 RVA: 0x00166A70 File Offset: 0x00164C70
		public static BigInteger GenerateX1(BigInteger q, SecureRandom random)
		{
			BigInteger zero = JPakeUtilities.Zero;
			BigInteger max = q.Subtract(JPakeUtilities.One);
			return BigIntegers.CreateRandomInRange(zero, max, random);
		}

		// Token: 0x06003957 RID: 14679 RVA: 0x00166A98 File Offset: 0x00164C98
		public static BigInteger GenerateX2(BigInteger q, SecureRandom random)
		{
			BigInteger one = JPakeUtilities.One;
			BigInteger max = q.Subtract(JPakeUtilities.One);
			return BigIntegers.CreateRandomInRange(one, max, random);
		}

		// Token: 0x06003958 RID: 14680 RVA: 0x00166ABD File Offset: 0x00164CBD
		public static BigInteger CalculateS(char[] password)
		{
			return new BigInteger(Encoding.UTF8.GetBytes(password));
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x00133B73 File Offset: 0x00131D73
		public static BigInteger CalculateGx(BigInteger p, BigInteger g, BigInteger x)
		{
			return g.ModPow(x, p);
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x00166ACF File Offset: 0x00164CCF
		public static BigInteger CalculateGA(BigInteger p, BigInteger gx1, BigInteger gx3, BigInteger gx4)
		{
			return gx1.Multiply(gx3).Multiply(gx4).Mod(p);
		}

		// Token: 0x0600395B RID: 14683 RVA: 0x00166AE4 File Offset: 0x00164CE4
		public static BigInteger CalculateX2s(BigInteger q, BigInteger x2, BigInteger s)
		{
			return x2.Multiply(s).Mod(q);
		}

		// Token: 0x0600395C RID: 14684 RVA: 0x00166AF3 File Offset: 0x00164CF3
		public static BigInteger CalculateA(BigInteger p, BigInteger q, BigInteger gA, BigInteger x2s)
		{
			return gA.ModPow(x2s, p);
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x00166B00 File Offset: 0x00164D00
		public static BigInteger[] CalculateZeroKnowledgeProof(BigInteger p, BigInteger q, BigInteger g, BigInteger gx, BigInteger x, string participantId, IDigest digest, SecureRandom random)
		{
			BigInteger zero = JPakeUtilities.Zero;
			BigInteger max = q.Subtract(JPakeUtilities.One);
			BigInteger bigInteger = BigIntegers.CreateRandomInRange(zero, max, random);
			BigInteger bigInteger2 = g.ModPow(bigInteger, p);
			BigInteger val = JPakeUtilities.CalculateHashForZeroKnowledgeProof(g, bigInteger2, gx, participantId, digest);
			return new BigInteger[]
			{
				bigInteger2,
				bigInteger.Subtract(x.Multiply(val)).Mod(q)
			};
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x00166B5E File Offset: 0x00164D5E
		private static BigInteger CalculateHashForZeroKnowledgeProof(BigInteger g, BigInteger gr, BigInteger gx, string participantId, IDigest digest)
		{
			digest.Reset();
			JPakeUtilities.UpdateDigestIncludingSize(digest, g);
			JPakeUtilities.UpdateDigestIncludingSize(digest, gr);
			JPakeUtilities.UpdateDigestIncludingSize(digest, gx);
			JPakeUtilities.UpdateDigestIncludingSize(digest, participantId);
			return new BigInteger(DigestUtilities.DoFinal(digest));
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x00166B93 File Offset: 0x00164D93
		public static void ValidateGx4(BigInteger gx4)
		{
			if (gx4.Equals(JPakeUtilities.One))
			{
				throw new CryptoException("g^x validation failed.  g^x should not be 1.");
			}
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x00166BAD File Offset: 0x00164DAD
		public static void ValidateGa(BigInteger ga)
		{
			if (ga.Equals(JPakeUtilities.One))
			{
				throw new CryptoException("ga is equal to 1.  It should not be.  The chances of this happening are on the order of 2^160 for a 160-bit q.  Try again.");
			}
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x00166BC8 File Offset: 0x00164DC8
		public static void ValidateZeroKnowledgeProof(BigInteger p, BigInteger q, BigInteger g, BigInteger gx, BigInteger[] zeroKnowledgeProof, string participantId, IDigest digest)
		{
			BigInteger bigInteger = zeroKnowledgeProof[0];
			BigInteger e = zeroKnowledgeProof[1];
			BigInteger e2 = JPakeUtilities.CalculateHashForZeroKnowledgeProof(g, bigInteger, gx, participantId, digest);
			if (gx.CompareTo(JPakeUtilities.Zero) != 1 || gx.CompareTo(p) != -1 || gx.ModPow(q, p).CompareTo(JPakeUtilities.One) != 0 || g.ModPow(e, p).Multiply(gx.ModPow(e2, p)).Mod(p).CompareTo(bigInteger) != 0)
			{
				throw new CryptoException("Zero-knowledge proof validation failed");
			}
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x00166C46 File Offset: 0x00164E46
		public static BigInteger CalculateKeyingMaterial(BigInteger p, BigInteger q, BigInteger gx4, BigInteger x2, BigInteger s, BigInteger B)
		{
			return gx4.ModPow(x2.Multiply(s).Negate().Mod(q), p).Multiply(B).ModPow(x2, p);
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x00166C70 File Offset: 0x00164E70
		public static void ValidateParticipantIdsDiffer(string participantId1, string participantId2)
		{
			if (participantId1.Equals(participantId2))
			{
				throw new CryptoException("Both participants are using the same participantId (" + participantId1 + "). This is not allowed. Each participant must use a unique participantId.");
			}
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x00166C91 File Offset: 0x00164E91
		public static void ValidateParticipantIdsEqual(string expectedParticipantId, string actualParticipantId)
		{
			if (!expectedParticipantId.Equals(actualParticipantId))
			{
				throw new CryptoException(string.Concat(new string[]
				{
					"Received payload from incorrect partner (",
					actualParticipantId,
					"). Expected to receive payload from ",
					expectedParticipantId,
					"."
				}));
			}
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x00166CCD File Offset: 0x00164ECD
		public static void ValidateNotNull(object obj, string description)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(description);
			}
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x00166CDC File Offset: 0x00164EDC
		public static BigInteger CalculateMacTag(string participantId, string partnerParticipantId, BigInteger gx1, BigInteger gx2, BigInteger gx3, BigInteger gx4, BigInteger keyingMaterial, IDigest digest)
		{
			byte[] array = JPakeUtilities.CalculateMacKey(keyingMaterial, digest);
			HMac hmac = new HMac(digest);
			hmac.Init(new KeyParameter(array));
			Arrays.Fill(array, 0);
			JPakeUtilities.UpdateMac(hmac, "KC_1_U");
			JPakeUtilities.UpdateMac(hmac, participantId);
			JPakeUtilities.UpdateMac(hmac, partnerParticipantId);
			JPakeUtilities.UpdateMac(hmac, gx1);
			JPakeUtilities.UpdateMac(hmac, gx2);
			JPakeUtilities.UpdateMac(hmac, gx3);
			JPakeUtilities.UpdateMac(hmac, gx4);
			return new BigInteger(MacUtilities.DoFinal(hmac));
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x00166D4E File Offset: 0x00164F4E
		private static byte[] CalculateMacKey(BigInteger keyingMaterial, IDigest digest)
		{
			digest.Reset();
			JPakeUtilities.UpdateDigest(digest, keyingMaterial);
			JPakeUtilities.UpdateDigest(digest, "JPAKE_KC");
			return DigestUtilities.DoFinal(digest);
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x00166D6E File Offset: 0x00164F6E
		public static void ValidateMacTag(string participantId, string partnerParticipantId, BigInteger gx1, BigInteger gx2, BigInteger gx3, BigInteger gx4, BigInteger keyingMaterial, IDigest digest, BigInteger partnerMacTag)
		{
			if (!JPakeUtilities.CalculateMacTag(partnerParticipantId, participantId, gx3, gx4, gx1, gx2, keyingMaterial, digest).Equals(partnerMacTag))
			{
				throw new CryptoException("Partner MacTag validation failed. Therefore, the password, MAC, or digest algorithm of each participant does not match.");
			}
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x00166D95 File Offset: 0x00164F95
		private static void UpdateDigest(IDigest digest, BigInteger bigInteger)
		{
			JPakeUtilities.UpdateDigest(digest, BigIntegers.AsUnsignedByteArray(bigInteger));
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x00166DA3 File Offset: 0x00164FA3
		private static void UpdateDigest(IDigest digest, string str)
		{
			JPakeUtilities.UpdateDigest(digest, Encoding.UTF8.GetBytes(str));
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x00166DB6 File Offset: 0x00164FB6
		private static void UpdateDigest(IDigest digest, byte[] bytes)
		{
			digest.BlockUpdate(bytes, 0, bytes.Length);
			Arrays.Fill(bytes, 0);
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x00166DCA File Offset: 0x00164FCA
		private static void UpdateDigestIncludingSize(IDigest digest, BigInteger bigInteger)
		{
			JPakeUtilities.UpdateDigestIncludingSize(digest, BigIntegers.AsUnsignedByteArray(bigInteger));
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x00166DD8 File Offset: 0x00164FD8
		private static void UpdateDigestIncludingSize(IDigest digest, string str)
		{
			JPakeUtilities.UpdateDigestIncludingSize(digest, Encoding.UTF8.GetBytes(str));
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x00166DEB File Offset: 0x00164FEB
		private static void UpdateDigestIncludingSize(IDigest digest, byte[] bytes)
		{
			digest.BlockUpdate(JPakeUtilities.IntToByteArray(bytes.Length), 0, 4);
			digest.BlockUpdate(bytes, 0, bytes.Length);
			Arrays.Fill(bytes, 0);
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x00166E0F File Offset: 0x0016500F
		private static void UpdateMac(IMac mac, BigInteger bigInteger)
		{
			JPakeUtilities.UpdateMac(mac, BigIntegers.AsUnsignedByteArray(bigInteger));
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x00166E1D File Offset: 0x0016501D
		private static void UpdateMac(IMac mac, string str)
		{
			JPakeUtilities.UpdateMac(mac, Encoding.UTF8.GetBytes(str));
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x00166E30 File Offset: 0x00165030
		private static void UpdateMac(IMac mac, byte[] bytes)
		{
			mac.BlockUpdate(bytes, 0, bytes.Length);
			Arrays.Fill(bytes, 0);
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x00166E44 File Offset: 0x00165044
		private static byte[] IntToByteArray(int value)
		{
			return Pack.UInt32_To_BE((uint)value);
		}

		// Token: 0x040025B0 RID: 9648
		public static readonly BigInteger Zero = BigInteger.Zero;

		// Token: 0x040025B1 RID: 9649
		public static readonly BigInteger One = BigInteger.One;
	}
}
