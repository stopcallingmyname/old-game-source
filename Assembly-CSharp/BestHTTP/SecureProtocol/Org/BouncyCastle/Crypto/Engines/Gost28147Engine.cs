using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000578 RID: 1400
	public class Gost28147Engine : IBlockCipher
	{
		// Token: 0x060034B4 RID: 13492 RVA: 0x0014251C File Offset: 0x0014071C
		static Gost28147Engine()
		{
			Gost28147Engine.AddSBox("Default", Gost28147Engine.Sbox_Default);
			Gost28147Engine.AddSBox("E-TEST", Gost28147Engine.ESbox_Test);
			Gost28147Engine.AddSBox("E-A", Gost28147Engine.ESbox_A);
			Gost28147Engine.AddSBox("E-B", Gost28147Engine.ESbox_B);
			Gost28147Engine.AddSBox("E-C", Gost28147Engine.ESbox_C);
			Gost28147Engine.AddSBox("E-D", Gost28147Engine.ESbox_D);
			Gost28147Engine.AddSBox("D-TEST", Gost28147Engine.DSbox_Test);
			Gost28147Engine.AddSBox("D-A", Gost28147Engine.DSbox_A);
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x0014267B File Offset: 0x0014087B
		private static void AddSBox(string sBoxName, byte[] sBox)
		{
			Gost28147Engine.sBoxes.Add(Platform.ToUpperInvariant(sBoxName), sBox);
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x001426A4 File Offset: 0x001408A4
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (parameters is ParametersWithSBox)
			{
				ParametersWithSBox parametersWithSBox = (ParametersWithSBox)parameters;
				byte[] sbox = parametersWithSBox.GetSBox();
				if (sbox.Length != Gost28147Engine.Sbox_Default.Length)
				{
					throw new ArgumentException("invalid S-box passed to GOST28147 init");
				}
				this.S = Arrays.Clone(sbox);
				if (parametersWithSBox.Parameters != null)
				{
					this.workingKey = this.generateWorkingKey(forEncryption, ((KeyParameter)parametersWithSBox.Parameters).GetKey());
					return;
				}
			}
			else
			{
				if (parameters is KeyParameter)
				{
					this.workingKey = this.generateWorkingKey(forEncryption, ((KeyParameter)parameters).GetKey());
					return;
				}
				if (parameters != null)
				{
					throw new ArgumentException("invalid parameter passed to Gost28147 init - " + Platform.GetTypeName(parameters));
				}
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x060034B8 RID: 13496 RVA: 0x0014274A File Offset: 0x0014094A
		public virtual string AlgorithmName
		{
			get
			{
				return "Gost28147";
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x060034B9 RID: 13497 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x000FCEE9 File Offset: 0x000FB0E9
		public virtual int GetBlockSize()
		{
			return 8;
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x00142754 File Offset: 0x00140954
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (this.workingKey == null)
			{
				throw new InvalidOperationException("Gost28147 engine not initialised");
			}
			Check.DataLength(input, inOff, 8, "input buffer too short");
			Check.OutputLength(output, outOff, 8, "output buffer too short");
			this.Gost28147Func(this.workingKey, input, inOff, output, outOff);
			return 8;
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void Reset()
		{
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x001427A4 File Offset: 0x001409A4
		private int[] generateWorkingKey(bool forEncryption, byte[] userKey)
		{
			this.forEncryption = forEncryption;
			if (userKey.Length != 32)
			{
				throw new ArgumentException("Key length invalid. Key needs to be 32 byte - 256 bit!!!");
			}
			int[] array = new int[8];
			for (int num = 0; num != 8; num++)
			{
				array[num] = Gost28147Engine.bytesToint(userKey, num * 4);
			}
			return array;
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x001427EC File Offset: 0x001409EC
		private int Gost28147_mainStep(int n1, int key)
		{
			int num = key + n1;
			int num2 = (int)this.S[num & 15] + ((int)this.S[16 + (num >> 4 & 15)] << 4) + ((int)this.S[32 + (num >> 8 & 15)] << 8) + ((int)this.S[48 + (num >> 12 & 15)] << 12) + ((int)this.S[64 + (num >> 16 & 15)] << 16) + ((int)this.S[80 + (num >> 20 & 15)] << 20) + ((int)this.S[96 + (num >> 24 & 15)] << 24) + ((int)this.S[112 + (num >> 28 & 15)] << 28);
			int num3 = num2 << 11;
			int num4 = (int)((uint)num2 >> 21);
			return num3 | num4;
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x001428A4 File Offset: 0x00140AA4
		private void Gost28147Func(int[] workingKey, byte[] inBytes, int inOff, byte[] outBytes, int outOff)
		{
			int num = Gost28147Engine.bytesToint(inBytes, inOff);
			int num2 = Gost28147Engine.bytesToint(inBytes, inOff + 4);
			if (this.forEncryption)
			{
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						int num3 = num;
						int num4 = this.Gost28147_mainStep(num, workingKey[j]);
						num = (num2 ^ num4);
						num2 = num3;
					}
				}
				for (int k = 7; k > 0; k--)
				{
					int num5 = num;
					num = (num2 ^ this.Gost28147_mainStep(num, workingKey[k]));
					num2 = num5;
				}
			}
			else
			{
				for (int l = 0; l < 8; l++)
				{
					int num6 = num;
					num = (num2 ^ this.Gost28147_mainStep(num, workingKey[l]));
					num2 = num6;
				}
				for (int m = 0; m < 3; m++)
				{
					int num7 = 7;
					while (num7 >= 0 && (m != 2 || num7 != 0))
					{
						int num8 = num;
						num = (num2 ^ this.Gost28147_mainStep(num, workingKey[num7]));
						num2 = num8;
						num7--;
					}
				}
			}
			num2 ^= this.Gost28147_mainStep(num, workingKey[0]);
			Gost28147Engine.intTobytes(num, outBytes, outOff);
			Gost28147Engine.intTobytes(num2, outBytes, outOff + 4);
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x00130350 File Offset: 0x0012E550
		private static int bytesToint(byte[] inBytes, int inOff)
		{
			return (int)((long)((long)inBytes[inOff + 3] << 24) & (long)((ulong)-16777216)) + ((int)inBytes[inOff + 2] << 16 & 16711680) + ((int)inBytes[inOff + 1] << 8 & 65280) + (int)(inBytes[inOff] & byte.MaxValue);
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x0013038A File Offset: 0x0012E58A
		private static void intTobytes(int num, byte[] outBytes, int outOff)
		{
			outBytes[outOff + 3] = (byte)(num >> 24);
			outBytes[outOff + 2] = (byte)(num >> 16);
			outBytes[outOff + 1] = (byte)(num >> 8);
			outBytes[outOff] = (byte)num;
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x00142994 File Offset: 0x00140B94
		public static byte[] GetSBox(string sBoxName)
		{
			byte[] array = (byte[])Gost28147Engine.sBoxes[Platform.ToUpperInvariant(sBoxName)];
			if (array == null)
			{
				throw new ArgumentException("Unknown S-Box - possible types: \"Default\", \"E-Test\", \"E-A\", \"E-B\", \"E-C\", \"E-D\", \"D-Test\", \"D-A\".");
			}
			return Arrays.Clone(array);
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x001429C0 File Offset: 0x00140BC0
		public static string GetSBoxName(byte[] sBox)
		{
			foreach (object obj in Gost28147Engine.sBoxes.Keys)
			{
				string text = (string)obj;
				if (Arrays.AreEqual((byte[])Gost28147Engine.sBoxes[text], sBox))
				{
					return text;
				}
			}
			throw new ArgumentException("SBOX provided did not map to a known one");
		}

		// Token: 0x04002284 RID: 8836
		private const int BlockSize = 8;

		// Token: 0x04002285 RID: 8837
		private int[] workingKey;

		// Token: 0x04002286 RID: 8838
		private bool forEncryption;

		// Token: 0x04002287 RID: 8839
		private byte[] S = Gost28147Engine.Sbox_Default;

		// Token: 0x04002288 RID: 8840
		private static readonly byte[] Sbox_Default = new byte[]
		{
			4,
			10,
			9,
			2,
			13,
			8,
			0,
			14,
			6,
			11,
			1,
			12,
			7,
			15,
			5,
			3,
			14,
			11,
			4,
			12,
			6,
			13,
			15,
			10,
			2,
			3,
			8,
			1,
			0,
			7,
			5,
			9,
			5,
			8,
			1,
			13,
			10,
			3,
			4,
			2,
			14,
			15,
			12,
			7,
			6,
			0,
			9,
			11,
			7,
			13,
			10,
			1,
			0,
			8,
			9,
			15,
			14,
			4,
			6,
			12,
			11,
			2,
			5,
			3,
			6,
			12,
			7,
			1,
			5,
			15,
			13,
			8,
			4,
			10,
			9,
			14,
			0,
			3,
			11,
			2,
			4,
			11,
			10,
			0,
			7,
			2,
			1,
			13,
			3,
			6,
			8,
			5,
			9,
			12,
			15,
			14,
			13,
			11,
			4,
			1,
			3,
			15,
			5,
			9,
			0,
			10,
			14,
			7,
			6,
			8,
			2,
			12,
			1,
			15,
			13,
			0,
			5,
			7,
			10,
			4,
			9,
			2,
			3,
			14,
			6,
			11,
			8,
			12
		};

		// Token: 0x04002289 RID: 8841
		private static readonly byte[] ESbox_Test = new byte[]
		{
			4,
			2,
			15,
			5,
			9,
			1,
			0,
			8,
			14,
			3,
			11,
			12,
			13,
			7,
			10,
			6,
			12,
			9,
			15,
			14,
			8,
			1,
			3,
			10,
			2,
			7,
			4,
			13,
			6,
			0,
			11,
			5,
			13,
			8,
			14,
			12,
			7,
			3,
			9,
			10,
			1,
			5,
			2,
			4,
			6,
			15,
			0,
			11,
			14,
			9,
			11,
			2,
			5,
			15,
			7,
			1,
			0,
			13,
			12,
			6,
			10,
			4,
			3,
			8,
			3,
			14,
			5,
			9,
			6,
			8,
			0,
			13,
			10,
			11,
			7,
			12,
			2,
			1,
			15,
			4,
			8,
			15,
			6,
			11,
			1,
			9,
			12,
			5,
			13,
			3,
			7,
			10,
			0,
			14,
			2,
			4,
			9,
			11,
			12,
			0,
			3,
			6,
			7,
			5,
			4,
			8,
			14,
			15,
			1,
			10,
			2,
			13,
			12,
			6,
			5,
			2,
			11,
			0,
			9,
			13,
			3,
			14,
			7,
			10,
			15,
			4,
			1,
			8
		};

		// Token: 0x0400228A RID: 8842
		private static readonly byte[] ESbox_A = new byte[]
		{
			9,
			6,
			3,
			2,
			8,
			11,
			1,
			7,
			10,
			4,
			14,
			15,
			12,
			0,
			13,
			5,
			3,
			7,
			14,
			9,
			8,
			10,
			15,
			0,
			5,
			2,
			6,
			12,
			11,
			4,
			13,
			1,
			14,
			4,
			6,
			2,
			11,
			3,
			13,
			8,
			12,
			15,
			5,
			10,
			0,
			7,
			1,
			9,
			14,
			7,
			10,
			12,
			13,
			1,
			3,
			9,
			0,
			2,
			11,
			4,
			15,
			8,
			5,
			6,
			11,
			5,
			1,
			9,
			8,
			13,
			15,
			0,
			14,
			4,
			2,
			3,
			12,
			7,
			10,
			6,
			3,
			10,
			13,
			12,
			1,
			2,
			0,
			11,
			7,
			5,
			9,
			4,
			8,
			15,
			14,
			6,
			1,
			13,
			2,
			9,
			7,
			10,
			6,
			0,
			8,
			12,
			4,
			5,
			15,
			3,
			11,
			14,
			11,
			10,
			15,
			5,
			0,
			12,
			14,
			8,
			6,
			2,
			3,
			9,
			1,
			7,
			13,
			4
		};

		// Token: 0x0400228B RID: 8843
		private static readonly byte[] ESbox_B = new byte[]
		{
			8,
			4,
			11,
			1,
			3,
			5,
			0,
			9,
			2,
			14,
			10,
			12,
			13,
			6,
			7,
			15,
			0,
			1,
			2,
			10,
			4,
			13,
			5,
			12,
			9,
			7,
			3,
			15,
			11,
			8,
			6,
			14,
			14,
			12,
			0,
			10,
			9,
			2,
			13,
			11,
			7,
			5,
			8,
			15,
			3,
			6,
			1,
			4,
			7,
			5,
			0,
			13,
			11,
			6,
			1,
			2,
			3,
			10,
			12,
			15,
			4,
			14,
			9,
			8,
			2,
			7,
			12,
			15,
			9,
			5,
			10,
			11,
			1,
			4,
			0,
			13,
			6,
			8,
			14,
			3,
			8,
			3,
			2,
			6,
			4,
			13,
			14,
			11,
			12,
			1,
			7,
			15,
			10,
			0,
			9,
			5,
			5,
			2,
			10,
			11,
			9,
			1,
			12,
			3,
			7,
			4,
			13,
			0,
			6,
			15,
			8,
			14,
			0,
			4,
			11,
			14,
			8,
			3,
			7,
			1,
			10,
			2,
			9,
			6,
			15,
			13,
			5,
			12
		};

		// Token: 0x0400228C RID: 8844
		private static readonly byte[] ESbox_C = new byte[]
		{
			1,
			11,
			12,
			2,
			9,
			13,
			0,
			15,
			4,
			5,
			8,
			14,
			10,
			7,
			6,
			3,
			0,
			1,
			7,
			13,
			11,
			4,
			5,
			2,
			8,
			14,
			15,
			12,
			9,
			10,
			6,
			3,
			8,
			2,
			5,
			0,
			4,
			9,
			15,
			10,
			3,
			7,
			12,
			13,
			6,
			14,
			1,
			11,
			3,
			6,
			0,
			1,
			5,
			13,
			10,
			8,
			11,
			2,
			9,
			7,
			14,
			15,
			12,
			4,
			8,
			13,
			11,
			0,
			4,
			5,
			1,
			2,
			9,
			3,
			12,
			14,
			6,
			15,
			10,
			7,
			12,
			9,
			11,
			1,
			8,
			14,
			2,
			4,
			7,
			3,
			6,
			5,
			10,
			0,
			15,
			13,
			10,
			9,
			6,
			8,
			13,
			14,
			2,
			0,
			15,
			3,
			5,
			11,
			4,
			1,
			12,
			7,
			7,
			4,
			0,
			5,
			10,
			2,
			15,
			14,
			12,
			6,
			1,
			11,
			13,
			9,
			3,
			8
		};

		// Token: 0x0400228D RID: 8845
		private static readonly byte[] ESbox_D = new byte[]
		{
			15,
			12,
			2,
			10,
			6,
			4,
			5,
			0,
			7,
			9,
			14,
			13,
			1,
			11,
			8,
			3,
			11,
			6,
			3,
			4,
			12,
			15,
			14,
			2,
			7,
			13,
			8,
			0,
			5,
			10,
			9,
			1,
			1,
			12,
			11,
			0,
			15,
			14,
			6,
			5,
			10,
			13,
			4,
			8,
			9,
			3,
			7,
			2,
			1,
			5,
			14,
			12,
			10,
			7,
			0,
			13,
			6,
			2,
			11,
			4,
			9,
			3,
			15,
			8,
			0,
			12,
			8,
			9,
			13,
			2,
			10,
			11,
			7,
			3,
			6,
			5,
			4,
			14,
			15,
			1,
			8,
			0,
			15,
			3,
			2,
			5,
			14,
			11,
			1,
			10,
			4,
			7,
			12,
			9,
			13,
			6,
			3,
			0,
			6,
			15,
			1,
			14,
			9,
			2,
			13,
			8,
			12,
			4,
			11,
			10,
			5,
			7,
			1,
			10,
			6,
			8,
			15,
			11,
			0,
			4,
			12,
			3,
			5,
			9,
			7,
			13,
			2,
			14
		};

		// Token: 0x0400228E RID: 8846
		private static readonly byte[] DSbox_Test = new byte[]
		{
			4,
			10,
			9,
			2,
			13,
			8,
			0,
			14,
			6,
			11,
			1,
			12,
			7,
			15,
			5,
			3,
			14,
			11,
			4,
			12,
			6,
			13,
			15,
			10,
			2,
			3,
			8,
			1,
			0,
			7,
			5,
			9,
			5,
			8,
			1,
			13,
			10,
			3,
			4,
			2,
			14,
			15,
			12,
			7,
			6,
			0,
			9,
			11,
			7,
			13,
			10,
			1,
			0,
			8,
			9,
			15,
			14,
			4,
			6,
			12,
			11,
			2,
			5,
			3,
			6,
			12,
			7,
			1,
			5,
			15,
			13,
			8,
			4,
			10,
			9,
			14,
			0,
			3,
			11,
			2,
			4,
			11,
			10,
			0,
			7,
			2,
			1,
			13,
			3,
			6,
			8,
			5,
			9,
			12,
			15,
			14,
			13,
			11,
			4,
			1,
			3,
			15,
			5,
			9,
			0,
			10,
			14,
			7,
			6,
			8,
			2,
			12,
			1,
			15,
			13,
			0,
			5,
			7,
			10,
			4,
			9,
			2,
			3,
			14,
			6,
			11,
			8,
			12
		};

		// Token: 0x0400228F RID: 8847
		private static readonly byte[] DSbox_A = new byte[]
		{
			10,
			4,
			5,
			6,
			8,
			1,
			3,
			7,
			13,
			12,
			14,
			0,
			9,
			2,
			11,
			15,
			5,
			15,
			4,
			0,
			2,
			13,
			11,
			9,
			1,
			7,
			6,
			3,
			12,
			14,
			10,
			8,
			7,
			15,
			12,
			14,
			9,
			4,
			1,
			0,
			3,
			11,
			5,
			2,
			6,
			10,
			8,
			13,
			4,
			10,
			7,
			12,
			0,
			15,
			2,
			8,
			14,
			1,
			6,
			5,
			13,
			11,
			9,
			3,
			7,
			6,
			4,
			11,
			9,
			12,
			2,
			10,
			1,
			8,
			0,
			14,
			15,
			13,
			3,
			5,
			7,
			6,
			2,
			4,
			13,
			9,
			15,
			0,
			10,
			1,
			5,
			11,
			8,
			14,
			12,
			3,
			13,
			14,
			4,
			1,
			7,
			0,
			5,
			10,
			3,
			12,
			8,
			15,
			6,
			2,
			9,
			11,
			1,
			3,
			10,
			9,
			5,
			11,
			4,
			15,
			8,
			6,
			7,
			14,
			13,
			0,
			2,
			12
		};

		// Token: 0x04002290 RID: 8848
		private static readonly IDictionary sBoxes = Platform.CreateHashtable();
	}
}
