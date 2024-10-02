using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000492 RID: 1170
	public class DsaDigestSigner : ISigner
	{
		// Token: 0x06002DFE RID: 11774 RVA: 0x0011EC22 File Offset: 0x0011CE22
		public DsaDigestSigner(IDsa dsa, IDigest digest)
		{
			this.dsa = dsa;
			this.digest = digest;
			this.encoding = StandardDsaEncoding.Instance;
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x0011EC43 File Offset: 0x0011CE43
		public DsaDigestSigner(IDsaExt dsa, IDigest digest, IDsaEncoding encoding)
		{
			this.dsa = dsa;
			this.digest = digest;
			this.encoding = encoding;
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06002E00 RID: 11776 RVA: 0x0011EC60 File Offset: 0x0011CE60
		public virtual string AlgorithmName
		{
			get
			{
				return this.digest.AlgorithmName + "with" + this.dsa.AlgorithmName;
			}
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x0011EC84 File Offset: 0x0011CE84
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			this.forSigning = forSigning;
			AsymmetricKeyParameter asymmetricKeyParameter;
			if (parameters is ParametersWithRandom)
			{
				asymmetricKeyParameter = (AsymmetricKeyParameter)((ParametersWithRandom)parameters).Parameters;
			}
			else
			{
				asymmetricKeyParameter = (AsymmetricKeyParameter)parameters;
			}
			if (forSigning && !asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Signing Requires Private Key.");
			}
			if (!forSigning && asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Verification Requires Public Key.");
			}
			this.Reset();
			this.dsa.Init(forSigning, parameters);
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x0011ECF9 File Offset: 0x0011CEF9
		public virtual void Update(byte input)
		{
			this.digest.Update(input);
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x0011ED07 File Offset: 0x0011CF07
		public virtual void BlockUpdate(byte[] input, int inOff, int length)
		{
			this.digest.BlockUpdate(input, inOff, length);
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x0011ED18 File Offset: 0x0011CF18
		public virtual byte[] GenerateSignature()
		{
			if (!this.forSigning)
			{
				throw new InvalidOperationException("DSADigestSigner not initialised for signature generation.");
			}
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			BigInteger[] array2 = this.dsa.GenerateSignature(array);
			byte[] result;
			try
			{
				result = this.encoding.Encode(this.GetOrder(), array2[0], array2[1]);
			}
			catch (Exception)
			{
				throw new InvalidOperationException("unable to encode signature");
			}
			return result;
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x0011ED9C File Offset: 0x0011CF9C
		public virtual bool VerifySignature(byte[] signature)
		{
			if (this.forSigning)
			{
				throw new InvalidOperationException("DSADigestSigner not initialised for verification");
			}
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			bool result;
			try
			{
				BigInteger[] array2 = this.encoding.Decode(this.GetOrder(), signature);
				result = this.dsa.VerifySignature(array, array2[0], array2[1]);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x0011EE1C File Offset: 0x0011D01C
		public virtual void Reset()
		{
			this.digest.Reset();
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x0011EE29 File Offset: 0x0011D029
		protected virtual BigInteger GetOrder()
		{
			if (!(this.dsa is IDsaExt))
			{
				return null;
			}
			return ((IDsaExt)this.dsa).Order;
		}

		// Token: 0x04001EC0 RID: 7872
		private readonly IDsa dsa;

		// Token: 0x04001EC1 RID: 7873
		private readonly IDigest digest;

		// Token: 0x04001EC2 RID: 7874
		private readonly IDsaEncoding encoding;

		// Token: 0x04001EC3 RID: 7875
		private bool forSigning;
	}
}
