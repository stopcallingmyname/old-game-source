using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x0200049C RID: 1180
	public class GenericSigner : ISigner
	{
		// Token: 0x06002E52 RID: 11858 RVA: 0x0011FF6D File Offset: 0x0011E16D
		public GenericSigner(IAsymmetricBlockCipher engine, IDigest digest)
		{
			this.engine = engine;
			this.digest = digest;
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06002E53 RID: 11859 RVA: 0x0011FF84 File Offset: 0x0011E184
		public virtual string AlgorithmName
		{
			get
			{
				return string.Concat(new string[]
				{
					"Generic(",
					this.engine.AlgorithmName,
					"/",
					this.digest.AlgorithmName,
					")"
				});
			}
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x0011FFD0 File Offset: 0x0011E1D0
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
				throw new InvalidKeyException("Signing requires private key.");
			}
			if (!forSigning && asymmetricKeyParameter.IsPrivate)
			{
				throw new InvalidKeyException("Verification requires public key.");
			}
			this.Reset();
			this.engine.Init(forSigning, parameters);
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x00120045 File Offset: 0x0011E245
		public virtual void Update(byte input)
		{
			this.digest.Update(input);
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x00120053 File Offset: 0x0011E253
		public virtual void BlockUpdate(byte[] input, int inOff, int length)
		{
			this.digest.BlockUpdate(input, inOff, length);
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x00120064 File Offset: 0x0011E264
		public virtual byte[] GenerateSignature()
		{
			if (!this.forSigning)
			{
				throw new InvalidOperationException("GenericSigner not initialised for signature generation.");
			}
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			return this.engine.ProcessBlock(array, 0, array.Length);
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x001200B4 File Offset: 0x0011E2B4
		public virtual bool VerifySignature(byte[] signature)
		{
			if (this.forSigning)
			{
				throw new InvalidOperationException("GenericSigner not initialised for verification");
			}
			byte[] array = new byte[this.digest.GetDigestSize()];
			this.digest.DoFinal(array, 0);
			bool result;
			try
			{
				byte[] array2 = this.engine.ProcessBlock(signature, 0, signature.Length);
				if (array2.Length < array.Length)
				{
					byte[] array3 = new byte[array.Length];
					Array.Copy(array2, 0, array3, array3.Length - array2.Length, array2.Length);
					array2 = array3;
				}
				result = Arrays.ConstantTimeAreEqual(array2, array);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x0012014C File Offset: 0x0011E34C
		public virtual void Reset()
		{
			this.digest.Reset();
		}

		// Token: 0x04001EE8 RID: 7912
		private readonly IAsymmetricBlockCipher engine;

		// Token: 0x04001EE9 RID: 7913
		private readonly IDigest digest;

		// Token: 0x04001EEA RID: 7914
		private bool forSigning;
	}
}
