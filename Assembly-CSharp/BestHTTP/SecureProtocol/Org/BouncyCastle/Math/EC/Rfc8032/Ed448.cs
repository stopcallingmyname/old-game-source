using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc7748;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc8032
{
	// Token: 0x02000334 RID: 820
	public abstract class Ed448
	{
		// Token: 0x06001FB3 RID: 8115 RVA: 0x000E9F5C File Offset: 0x000E815C
		private static byte[] CalculateS(byte[] r, byte[] k, byte[] s)
		{
			uint[] array = new uint[28];
			Ed448.DecodeScalar(r, 0, array);
			uint[] array2 = new uint[14];
			Ed448.DecodeScalar(k, 0, array2);
			uint[] array3 = new uint[14];
			Ed448.DecodeScalar(s, 0, array3);
			Nat.MulAddTo(14, array2, array3, array);
			byte[] array4 = new byte[114];
			for (int i = 0; i < array.Length; i++)
			{
				Ed448.Encode32(array[i], array4, i * 4);
			}
			return Ed448.ReduceScalar(array4);
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x000E9FD2 File Offset: 0x000E81D2
		private static bool CheckContextVar(byte[] ctx)
		{
			return ctx != null && ctx.Length < 256;
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x000E9FE4 File Offset: 0x000E81E4
		private static bool CheckPointVar(byte[] p)
		{
			if ((p[56] & 127) != 0)
			{
				return false;
			}
			uint[] array = new uint[14];
			Ed448.Decode32(p, 0, array, 0, 14);
			return !Nat.Gte(14, array, Ed448.P);
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x000EA020 File Offset: 0x000E8220
		private static bool CheckScalarVar(byte[] s)
		{
			if (s[56] != 0)
			{
				return false;
			}
			uint[] array = new uint[14];
			Ed448.DecodeScalar(s, 0, array);
			return !Nat.Gte(14, array, Ed448.L);
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x000EA055 File Offset: 0x000E8255
		public static IXof CreatePrehash()
		{
			return Ed448.CreateXof();
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x000EA05C File Offset: 0x000E825C
		private static IXof CreateXof()
		{
			return new ShakeDigest(256);
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x000EA068 File Offset: 0x000E8268
		private static uint Decode16(byte[] bs, int off)
		{
			return (uint)((int)bs[off] | (int)bs[++off] << 8);
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x000E844C File Offset: 0x000E664C
		private static uint Decode24(byte[] bs, int off)
		{
			return (uint)((int)bs[off] | (int)bs[++off] << 8 | (int)bs[++off] << 16);
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x000E8468 File Offset: 0x000E6668
		private static uint Decode32(byte[] bs, int off)
		{
			return (uint)((int)bs[off] | (int)bs[++off] << 8 | (int)bs[++off] << 16 | (int)bs[++off] << 24);
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x000EA078 File Offset: 0x000E8278
		private static void Decode32(byte[] bs, int bsOff, uint[] n, int nOff, int nLen)
		{
			for (int i = 0; i < nLen; i++)
			{
				n[nOff + i] = Ed448.Decode32(bs, bsOff + i * 4);
			}
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x000EA0A4 File Offset: 0x000E82A4
		private static bool DecodePointVar(byte[] p, int pOff, bool negate, Ed448.PointExt r)
		{
			byte[] array = Arrays.CopyOfRange(p, pOff, pOff + 57);
			if (!Ed448.CheckPointVar(array))
			{
				return false;
			}
			int num = (array[56] & 128) >> 7;
			byte[] array2 = array;
			int num2 = 56;
			array2[num2] &= 127;
			X448Field.Decode(array, 0, r.y);
			uint[] array3 = X448Field.Create();
			uint[] array4 = X448Field.Create();
			X448Field.Sqr(r.y, array3);
			X448Field.Mul(array3, 39081U, array4);
			X448Field.Negate(array3, array3);
			X448Field.AddOne(array3);
			X448Field.AddOne(array4);
			if (!X448Field.SqrtRatioVar(array3, array4, r.x))
			{
				return false;
			}
			X448Field.Normalize(r.x);
			if (num == 1 && X448Field.IsZeroVar(r.x))
			{
				return false;
			}
			if (negate ^ (long)num != (long)((ulong)(r.x[0] & 1U)))
			{
				X448Field.Negate(r.x, r.x);
			}
			Ed448.PointExtendXY(r);
			return true;
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x000EA184 File Offset: 0x000E8384
		private static void DecodeScalar(byte[] k, int kOff, uint[] n)
		{
			Ed448.Decode32(k, kOff, n, 0, 14);
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x000EA191 File Offset: 0x000E8391
		private static void Dom4(IXof d, byte x, byte[] y)
		{
			d.BlockUpdate(Ed448.Dom4Prefix, 0, Ed448.Dom4Prefix.Length);
			d.Update(x);
			d.Update((byte)y.Length);
			d.BlockUpdate(y, 0, y.Length);
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x000E85D3 File Offset: 0x000E67D3
		private static void Encode24(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)n;
			bs[++off] = (byte)(n >> 8);
			bs[++off] = (byte)(n >> 16);
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x000E85F3 File Offset: 0x000E67F3
		private static void Encode32(uint n, byte[] bs, int off)
		{
			bs[off] = (byte)n;
			bs[++off] = (byte)(n >> 8);
			bs[++off] = (byte)(n >> 16);
			bs[++off] = (byte)(n >> 24);
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x000EA1C2 File Offset: 0x000E83C2
		private static void Encode56(ulong n, byte[] bs, int off)
		{
			Ed448.Encode32((uint)n, bs, off);
			Ed448.Encode24((uint)(n >> 32), bs, off + 4);
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x000EA1DC File Offset: 0x000E83DC
		private static void EncodePoint(Ed448.PointExt p, byte[] r, int rOff)
		{
			uint[] array = X448Field.Create();
			uint[] array2 = X448Field.Create();
			X448Field.Inv(p.z, array2);
			X448Field.Mul(p.x, array2, array);
			X448Field.Mul(p.y, array2, array2);
			X448Field.Normalize(array);
			X448Field.Normalize(array2);
			X448Field.Encode(array2, r, rOff);
			r[rOff + 57 - 1] = (byte)((array[0] & 1U) << 7);
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x000E86A8 File Offset: 0x000E68A8
		public static void GeneratePrivateKey(SecureRandom random, byte[] k)
		{
			random.NextBytes(k);
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x000EA240 File Offset: 0x000E8440
		public static void GeneratePublicKey(byte[] sk, int skOff, byte[] pk, int pkOff)
		{
			IXof xof = Ed448.CreateXof();
			byte[] array = new byte[114];
			xof.BlockUpdate(sk, skOff, Ed448.SecretKeySize);
			xof.DoFinal(array, 0, array.Length);
			byte[] array2 = new byte[57];
			Ed448.PruneScalar(array, 0, array2);
			Ed448.ScalarMultBaseEncoded(array2, pk, pkOff);
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x000EA28C File Offset: 0x000E848C
		private static sbyte[] GetWnaf(uint[] n, int width)
		{
			uint[] array = new uint[28];
			uint num = 0U;
			int num2 = array.Length;
			int num3 = 14;
			while (--num3 >= 0)
			{
				uint num4 = n[num3];
				array[--num2] = (num4 >> 16 | num << 16);
				num = (array[--num2] = num4);
			}
			sbyte[] array2 = new sbyte[448];
			int num5 = 1 << width;
			uint num6 = (uint)(num5 - 1);
			uint num7 = (uint)num5 >> 1;
			uint num8 = 0U;
			int i = 0;
			int j = 0;
			while (j < array.Length)
			{
				uint num9 = array[j];
				while (i < 16)
				{
					uint num10 = num9 >> i;
					if ((num10 & 1U) == num8)
					{
						i++;
					}
					else
					{
						uint num11 = (num10 & num6) + num8;
						num8 = (num11 & num7);
						num11 -= num8 << 1;
						num8 >>= width - 1;
						array2[(j << 4) + i] = (sbyte)num11;
						i += width;
					}
				}
				j++;
				i -= 16;
			}
			return array2;
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x000EA378 File Offset: 0x000E8578
		private static void ImplSign(IXof d, byte[] h, byte[] s, byte[] pk, int pkOff, byte[] ctx, byte phflag, byte[] m, int mOff, int mLen, byte[] sig, int sigOff)
		{
			Ed448.Dom4(d, phflag, ctx);
			d.BlockUpdate(h, 57, 57);
			d.BlockUpdate(m, mOff, mLen);
			d.DoFinal(h, 0, h.Length);
			byte[] array = Ed448.ReduceScalar(h);
			byte[] array2 = new byte[57];
			Ed448.ScalarMultBaseEncoded(array, array2, 0);
			Ed448.Dom4(d, phflag, ctx);
			d.BlockUpdate(array2, 0, 57);
			d.BlockUpdate(pk, pkOff, 57);
			d.BlockUpdate(m, mOff, mLen);
			d.DoFinal(h, 0, h.Length);
			byte[] k = Ed448.ReduceScalar(h);
			Array sourceArray = Ed448.CalculateS(array, k, s);
			Array.Copy(array2, 0, sig, sigOff, 57);
			Array.Copy(sourceArray, 0, sig, sigOff + 57, 57);
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x000EA42C File Offset: 0x000E862C
		private static void ImplSign(byte[] sk, int skOff, byte[] ctx, byte phflag, byte[] m, int mOff, int mLen, byte[] sig, int sigOff)
		{
			if (!Ed448.CheckContextVar(ctx))
			{
				throw new ArgumentException("ctx");
			}
			IXof xof = Ed448.CreateXof();
			byte[] array = new byte[114];
			xof.BlockUpdate(sk, skOff, Ed448.SecretKeySize);
			xof.DoFinal(array, 0, array.Length);
			byte[] array2 = new byte[57];
			Ed448.PruneScalar(array, 0, array2);
			byte[] array3 = new byte[57];
			Ed448.ScalarMultBaseEncoded(array2, array3, 0);
			Ed448.ImplSign(xof, array, array2, array3, 0, ctx, phflag, m, mOff, mLen, sig, sigOff);
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x000EA4A8 File Offset: 0x000E86A8
		private static void ImplSign(byte[] sk, int skOff, byte[] pk, int pkOff, byte[] ctx, byte phflag, byte[] m, int mOff, int mLen, byte[] sig, int sigOff)
		{
			if (!Ed448.CheckContextVar(ctx))
			{
				throw new ArgumentException("ctx");
			}
			IXof xof = Ed448.CreateXof();
			byte[] array = new byte[114];
			xof.BlockUpdate(sk, skOff, Ed448.SecretKeySize);
			xof.DoFinal(array, 0, array.Length);
			byte[] array2 = new byte[57];
			Ed448.PruneScalar(array, 0, array2);
			Ed448.ImplSign(xof, array, array2, pk, pkOff, ctx, phflag, m, mOff, mLen, sig, sigOff);
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x000EA518 File Offset: 0x000E8718
		private static bool ImplVerify(byte[] sig, int sigOff, byte[] pk, int pkOff, byte[] ctx, byte phflag, byte[] m, int mOff, int mLen)
		{
			if (!Ed448.CheckContextVar(ctx))
			{
				throw new ArgumentException("ctx");
			}
			byte[] array = Arrays.CopyOfRange(sig, sigOff, sigOff + 57);
			byte[] array2 = Arrays.CopyOfRange(sig, sigOff + 57, sigOff + Ed448.SignatureSize);
			if (!Ed448.CheckPointVar(array))
			{
				return false;
			}
			if (!Ed448.CheckScalarVar(array2))
			{
				return false;
			}
			Ed448.PointExt pointExt = new Ed448.PointExt();
			if (!Ed448.DecodePointVar(pk, pkOff, true, pointExt))
			{
				return false;
			}
			IXof xof = Ed448.CreateXof();
			byte[] array3 = new byte[114];
			Ed448.Dom4(xof, phflag, ctx);
			xof.BlockUpdate(array, 0, 57);
			xof.BlockUpdate(pk, pkOff, 57);
			xof.BlockUpdate(m, mOff, mLen);
			xof.DoFinal(array3, 0, array3.Length);
			byte[] k = Ed448.ReduceScalar(array3);
			uint[] array4 = new uint[14];
			Ed448.DecodeScalar(array2, 0, array4);
			uint[] array5 = new uint[14];
			Ed448.DecodeScalar(k, 0, array5);
			Ed448.PointExt pointExt2 = new Ed448.PointExt();
			Ed448.ScalarMultStraussVar(array4, array5, pointExt, pointExt2);
			byte[] array6 = new byte[57];
			Ed448.EncodePoint(pointExt2, array6, 0);
			return Arrays.AreEqual(array6, array);
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x000EA618 File Offset: 0x000E8818
		private static void PointAddVar(bool negate, Ed448.PointExt p, Ed448.PointExt r)
		{
			uint[] array = X448Field.Create();
			uint[] array2 = X448Field.Create();
			uint[] array3 = X448Field.Create();
			uint[] array4 = X448Field.Create();
			uint[] array5 = X448Field.Create();
			uint[] array6 = X448Field.Create();
			uint[] array7 = X448Field.Create();
			uint[] array8 = X448Field.Create();
			uint[] z;
			uint[] z2;
			uint[] z3;
			uint[] z4;
			if (negate)
			{
				z = array5;
				z2 = array2;
				z3 = array7;
				z4 = array6;
				X448Field.Sub(p.y, p.x, array8);
			}
			else
			{
				z = array2;
				z2 = array5;
				z3 = array6;
				z4 = array7;
				X448Field.Add(p.y, p.x, array8);
			}
			X448Field.Mul(p.z, r.z, array);
			X448Field.Sqr(array, array2);
			X448Field.Mul(p.x, r.x, array3);
			X448Field.Mul(p.y, r.y, array4);
			X448Field.Mul(array3, array4, array5);
			X448Field.Mul(array5, 39081U, array5);
			X448Field.Add(array2, array5, z3);
			X448Field.Sub(array2, array5, z4);
			X448Field.Add(r.x, r.y, array5);
			X448Field.Mul(array8, array5, array8);
			X448Field.Add(array4, array3, z);
			X448Field.Sub(array4, array3, z2);
			X448Field.Carry(z);
			X448Field.Sub(array8, array2, array8);
			X448Field.Mul(array8, array, array8);
			X448Field.Mul(array5, array, array5);
			X448Field.Mul(array6, array8, r.x);
			X448Field.Mul(array5, array7, r.y);
			X448Field.Mul(array6, array7, r.z);
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x000EA78C File Offset: 0x000E898C
		private static void PointAddPrecomp(Ed448.PointPrecomp p, Ed448.PointExt r)
		{
			uint[] array = X448Field.Create();
			uint[] array2 = X448Field.Create();
			uint[] array3 = X448Field.Create();
			uint[] array4 = X448Field.Create();
			uint[] array5 = X448Field.Create();
			uint[] array6 = X448Field.Create();
			uint[] array7 = X448Field.Create();
			X448Field.Sqr(r.z, array);
			X448Field.Mul(p.x, r.x, array2);
			X448Field.Mul(p.y, r.y, array3);
			X448Field.Mul(array2, array3, array4);
			X448Field.Mul(array4, 39081U, array4);
			X448Field.Add(array, array4, array5);
			X448Field.Sub(array, array4, array6);
			X448Field.Add(p.x, p.y, array);
			X448Field.Add(r.x, r.y, array4);
			X448Field.Mul(array, array4, array7);
			X448Field.Add(array3, array2, array);
			X448Field.Sub(array3, array2, array4);
			X448Field.Carry(array);
			X448Field.Sub(array7, array, array7);
			X448Field.Mul(array7, r.z, array7);
			X448Field.Mul(array4, r.z, array4);
			X448Field.Mul(array5, array7, r.x);
			X448Field.Mul(array4, array6, r.y);
			X448Field.Mul(array5, array6, r.z);
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x000EA8B4 File Offset: 0x000E8AB4
		private static Ed448.PointExt PointCopy(Ed448.PointExt p)
		{
			Ed448.PointExt pointExt = new Ed448.PointExt();
			X448Field.Copy(p.x, 0, pointExt.x, 0);
			X448Field.Copy(p.y, 0, pointExt.y, 0);
			X448Field.Copy(p.z, 0, pointExt.z, 0);
			return pointExt;
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x000EA904 File Offset: 0x000E8B04
		private static void PointDouble(Ed448.PointExt r)
		{
			uint[] array = X448Field.Create();
			uint[] array2 = X448Field.Create();
			uint[] array3 = X448Field.Create();
			uint[] array4 = X448Field.Create();
			uint[] array5 = X448Field.Create();
			uint[] array6 = X448Field.Create();
			X448Field.Add(r.x, r.y, array);
			X448Field.Sqr(array, array);
			X448Field.Sqr(r.x, array2);
			X448Field.Sqr(r.y, array3);
			X448Field.Add(array2, array3, array4);
			X448Field.Carry(array4);
			X448Field.Sqr(r.z, array5);
			X448Field.Add(array5, array5, array5);
			X448Field.Carry(array5);
			X448Field.Sub(array4, array5, array6);
			X448Field.Sub(array, array4, array);
			X448Field.Sub(array2, array3, array2);
			X448Field.Mul(array, array6, r.x);
			X448Field.Mul(array4, array2, r.y);
			X448Field.Mul(array4, array6, r.z);
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x000EA9D8 File Offset: 0x000E8BD8
		private static void PointExtendXY(Ed448.PointExt p)
		{
			X448Field.One(p.z);
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x000EA9E8 File Offset: 0x000E8BE8
		private static void PointLookup(int block, int index, Ed448.PointPrecomp p)
		{
			int num = block * 16 * 2 * 16;
			for (int i = 0; i < 16; i++)
			{
				int mask = (i ^ index) - 1 >> 31;
				Nat.CMov(16, mask, Ed448.precompBase, num, p.x, 0);
				num += 16;
				Nat.CMov(16, mask, Ed448.precompBase, num, p.y, 0);
				num += 16;
			}
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x000EAA4C File Offset: 0x000E8C4C
		private static Ed448.PointExt[] PointPrecompVar(Ed448.PointExt p, int count)
		{
			Ed448.PointExt pointExt = Ed448.PointCopy(p);
			Ed448.PointDouble(pointExt);
			Ed448.PointExt[] array = new Ed448.PointExt[count];
			array[0] = Ed448.PointCopy(p);
			for (int i = 1; i < count; i++)
			{
				array[i] = Ed448.PointCopy(array[i - 1]);
				Ed448.PointAddVar(false, pointExt, array[i]);
			}
			return array;
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x000EAA9A File Offset: 0x000E8C9A
		private static void PointSetNeutral(Ed448.PointExt p)
		{
			X448Field.Zero(p.x);
			X448Field.One(p.y);
			X448Field.One(p.z);
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x000EAAC0 File Offset: 0x000E8CC0
		public static void Precompute()
		{
			object obj = Ed448.precompLock;
			lock (obj)
			{
				if (Ed448.precompBase == null)
				{
					Ed448.PointExt pointExt = new Ed448.PointExt();
					X448Field.Copy(Ed448.B_x, 0, pointExt.x, 0);
					X448Field.Copy(Ed448.B_y, 0, pointExt.y, 0);
					Ed448.PointExtendXY(pointExt);
					Ed448.precompBaseTable = Ed448.PointPrecompVar(pointExt, 32);
					Ed448.precompBase = new uint[2560];
					int num = 0;
					for (int i = 0; i < 5; i++)
					{
						Ed448.PointExt[] array = new Ed448.PointExt[5];
						Ed448.PointExt pointExt2 = new Ed448.PointExt();
						Ed448.PointSetNeutral(pointExt2);
						for (int j = 0; j < 5; j++)
						{
							Ed448.PointAddVar(true, pointExt, pointExt2);
							Ed448.PointDouble(pointExt);
							array[j] = Ed448.PointCopy(pointExt);
							if (i + j != 8)
							{
								for (int k = 1; k < 18; k++)
								{
									Ed448.PointDouble(pointExt);
								}
							}
						}
						Ed448.PointExt[] array2 = new Ed448.PointExt[16];
						int num2 = 0;
						array2[num2++] = pointExt2;
						for (int l = 0; l < 4; l++)
						{
							int num3 = 1 << l;
							int m = 0;
							while (m < num3)
							{
								array2[num2] = Ed448.PointCopy(array2[num2 - num3]);
								Ed448.PointAddVar(false, array[l], array2[num2]);
								m++;
								num2++;
							}
						}
						for (int n = 0; n < 16; n++)
						{
							Ed448.PointExt pointExt3 = array2[n];
							X448Field.Inv(pointExt3.z, pointExt3.z);
							X448Field.Mul(pointExt3.x, pointExt3.z, pointExt3.x);
							X448Field.Mul(pointExt3.y, pointExt3.z, pointExt3.y);
							X448Field.Copy(pointExt3.x, 0, Ed448.precompBase, num);
							num += 16;
							X448Field.Copy(pointExt3.y, 0, Ed448.precompBase, num);
							num += 16;
						}
					}
				}
			}
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x000EACD4 File Offset: 0x000E8ED4
		private static void PruneScalar(byte[] n, int nOff, byte[] r)
		{
			Array.Copy(n, nOff, r, 0, 56);
			int num = 0;
			r[num] &= 252;
			int num2 = 55;
			r[num2] |= 128;
			r[56] = 0;
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x000EAD0C File Offset: 0x000E8F0C
		private static byte[] ReduceScalar(byte[] n)
		{
			ulong num = (ulong)Ed448.Decode32(n, 0);
			ulong num2 = (ulong)((ulong)Ed448.Decode24(n, 4) << 4);
			ulong num3 = (ulong)Ed448.Decode32(n, 7);
			ulong num4 = (ulong)((ulong)Ed448.Decode24(n, 11) << 4);
			ulong num5 = (ulong)Ed448.Decode32(n, 14);
			ulong num6 = (ulong)((ulong)Ed448.Decode24(n, 18) << 4);
			ulong num7 = (ulong)Ed448.Decode32(n, 21);
			ulong num8 = (ulong)((ulong)Ed448.Decode24(n, 25) << 4);
			ulong num9 = (ulong)Ed448.Decode32(n, 28);
			ulong num10 = (ulong)((ulong)Ed448.Decode24(n, 32) << 4);
			ulong num11 = (ulong)Ed448.Decode32(n, 35);
			ulong num12 = (ulong)((ulong)Ed448.Decode24(n, 39) << 4);
			ulong num13 = (ulong)Ed448.Decode32(n, 42);
			ulong num14 = (ulong)((ulong)Ed448.Decode24(n, 46) << 4);
			ulong num15 = (ulong)Ed448.Decode32(n, 49);
			ulong num16 = (ulong)((ulong)Ed448.Decode24(n, 53) << 4);
			ulong num17 = (ulong)Ed448.Decode32(n, 56);
			ulong num18 = (ulong)((ulong)Ed448.Decode24(n, 60) << 4);
			ulong num19 = (ulong)Ed448.Decode32(n, 63);
			ulong num20 = (ulong)((ulong)Ed448.Decode24(n, 67) << 4);
			ulong num21 = (ulong)Ed448.Decode32(n, 70);
			ulong num22 = (ulong)((ulong)Ed448.Decode24(n, 74) << 4);
			ulong num23 = (ulong)Ed448.Decode32(n, 77);
			ulong num24 = (ulong)((ulong)Ed448.Decode24(n, 81) << 4);
			ulong num25 = (ulong)Ed448.Decode32(n, 84);
			ulong num26 = (ulong)((ulong)Ed448.Decode24(n, 88) << 4);
			ulong num27 = (ulong)Ed448.Decode32(n, 91);
			ulong num28 = (ulong)((ulong)Ed448.Decode24(n, 95) << 4);
			ulong num29 = (ulong)Ed448.Decode32(n, 98);
			ulong num30 = (ulong)((ulong)Ed448.Decode24(n, 102) << 4);
			ulong num31 = (ulong)Ed448.Decode32(n, 105);
			ulong num32 = (ulong)((ulong)Ed448.Decode24(n, 109) << 4);
			ulong num33 = (ulong)Ed448.Decode16(n, 112);
			num17 += num33 * 43969588UL;
			num18 += num33 * 30366549UL;
			num19 += num33 * 163752818UL;
			num20 += num33 * 258169998UL;
			num21 += num33 * 96434764UL;
			num22 += num33 * 227822194UL;
			num23 += num33 * 149865618UL;
			num24 += num33 * 550336261UL;
			num32 += num31 >> 28;
			num31 &= 268435455UL;
			num16 += num32 * 43969588UL;
			num17 += num32 * 30366549UL;
			num18 += num32 * 163752818UL;
			num19 += num32 * 258169998UL;
			num20 += num32 * 96434764UL;
			num21 += num32 * 227822194UL;
			num22 += num32 * 149865618UL;
			num23 += num32 * 550336261UL;
			num15 += num31 * 43969588UL;
			num16 += num31 * 30366549UL;
			num17 += num31 * 163752818UL;
			num18 += num31 * 258169998UL;
			num19 += num31 * 96434764UL;
			num20 += num31 * 227822194UL;
			num21 += num31 * 149865618UL;
			num22 += num31 * 550336261UL;
			num30 += num29 >> 28;
			num29 &= 268435455UL;
			num14 += num30 * 43969588UL;
			num15 += num30 * 30366549UL;
			num16 += num30 * 163752818UL;
			num17 += num30 * 258169998UL;
			num18 += num30 * 96434764UL;
			num19 += num30 * 227822194UL;
			num20 += num30 * 149865618UL;
			num21 += num30 * 550336261UL;
			num13 += num29 * 43969588UL;
			num14 += num29 * 30366549UL;
			num15 += num29 * 163752818UL;
			num16 += num29 * 258169998UL;
			num17 += num29 * 96434764UL;
			num18 += num29 * 227822194UL;
			num19 += num29 * 149865618UL;
			num20 += num29 * 550336261UL;
			num28 += num27 >> 28;
			num27 &= 268435455UL;
			num12 += num28 * 43969588UL;
			num13 += num28 * 30366549UL;
			num14 += num28 * 163752818UL;
			num15 += num28 * 258169998UL;
			num16 += num28 * 96434764UL;
			num17 += num28 * 227822194UL;
			num18 += num28 * 149865618UL;
			num19 += num28 * 550336261UL;
			num11 += num27 * 43969588UL;
			num12 += num27 * 30366549UL;
			num13 += num27 * 163752818UL;
			num14 += num27 * 258169998UL;
			num15 += num27 * 96434764UL;
			num16 += num27 * 227822194UL;
			num17 += num27 * 149865618UL;
			num18 += num27 * 550336261UL;
			num26 += num25 >> 28;
			num25 &= 268435455UL;
			num10 += num26 * 43969588UL;
			num11 += num26 * 30366549UL;
			num12 += num26 * 163752818UL;
			num13 += num26 * 258169998UL;
			num14 += num26 * 96434764UL;
			num15 += num26 * 227822194UL;
			num16 += num26 * 149865618UL;
			num17 += num26 * 550336261UL;
			num22 += num21 >> 28;
			num21 &= 268435455UL;
			num23 += num22 >> 28;
			num22 &= 268435455UL;
			num24 += num23 >> 28;
			num23 &= 268435455UL;
			num25 += num24 >> 28;
			num24 &= 268435455UL;
			num9 += num25 * 43969588UL;
			num10 += num25 * 30366549UL;
			num11 += num25 * 163752818UL;
			num12 += num25 * 258169998UL;
			num13 += num25 * 96434764UL;
			num14 += num25 * 227822194UL;
			num15 += num25 * 149865618UL;
			num16 += num25 * 550336261UL;
			num8 += num24 * 43969588UL;
			num9 += num24 * 30366549UL;
			num10 += num24 * 163752818UL;
			num11 += num24 * 258169998UL;
			num12 += num24 * 96434764UL;
			num13 += num24 * 227822194UL;
			num14 += num24 * 149865618UL;
			num15 += num24 * 550336261UL;
			num7 += num23 * 43969588UL;
			num8 += num23 * 30366549UL;
			num9 += num23 * 163752818UL;
			num10 += num23 * 258169998UL;
			num11 += num23 * 96434764UL;
			num12 += num23 * 227822194UL;
			num13 += num23 * 149865618UL;
			num14 += num23 * 550336261UL;
			num19 += num18 >> 28;
			num18 &= 268435455UL;
			num20 += num19 >> 28;
			num19 &= 268435455UL;
			num21 += num20 >> 28;
			num20 &= 268435455UL;
			num22 += num21 >> 28;
			num21 &= 268435455UL;
			num6 += num22 * 43969588UL;
			num7 += num22 * 30366549UL;
			num8 += num22 * 163752818UL;
			num9 += num22 * 258169998UL;
			num10 += num22 * 96434764UL;
			num11 += num22 * 227822194UL;
			num12 += num22 * 149865618UL;
			num13 += num22 * 550336261UL;
			num5 += num21 * 43969588UL;
			num6 += num21 * 30366549UL;
			num7 += num21 * 163752818UL;
			num8 += num21 * 258169998UL;
			num9 += num21 * 96434764UL;
			num10 += num21 * 227822194UL;
			num11 += num21 * 149865618UL;
			num12 += num21 * 550336261UL;
			num4 += num20 * 43969588UL;
			num5 += num20 * 30366549UL;
			num6 += num20 * 163752818UL;
			num7 += num20 * 258169998UL;
			num8 += num20 * 96434764UL;
			num9 += num20 * 227822194UL;
			num10 += num20 * 149865618UL;
			num11 += num20 * 550336261UL;
			num16 += num15 >> 28;
			num15 &= 268435455UL;
			num17 += num16 >> 28;
			num16 &= 268435455UL;
			num18 += num17 >> 28;
			num17 &= 268435455UL;
			num19 += num18 >> 28;
			num18 &= 268435455UL;
			num3 += num19 * 43969588UL;
			num4 += num19 * 30366549UL;
			num5 += num19 * 163752818UL;
			num6 += num19 * 258169998UL;
			num7 += num19 * 96434764UL;
			num8 += num19 * 227822194UL;
			num9 += num19 * 149865618UL;
			num10 += num19 * 550336261UL;
			num2 += num18 * 43969588UL;
			num3 += num18 * 30366549UL;
			num4 += num18 * 163752818UL;
			num5 += num18 * 258169998UL;
			num6 += num18 * 96434764UL;
			num7 += num18 * 227822194UL;
			num8 += num18 * 149865618UL;
			num9 += num18 * 550336261UL;
			num17 *= 4UL;
			num17 += num16 >> 26;
			num16 &= 67108863UL;
			num17 += 1UL;
			num += num17 * 78101261UL;
			num2 += num17 * 141809365UL;
			num3 += num17 * 175155932UL;
			num4 += num17 * 64542499UL;
			num5 += num17 * 158326419UL;
			num6 += num17 * 191173276UL;
			num7 += num17 * 104575268UL;
			num8 += num17 * 137584065UL;
			num2 += num >> 28;
			num &= 268435455UL;
			num3 += num2 >> 28;
			num2 &= 268435455UL;
			num4 += num3 >> 28;
			num3 &= 268435455UL;
			num5 += num4 >> 28;
			num4 &= 268435455UL;
			num6 += num5 >> 28;
			num5 &= 268435455UL;
			num7 += num6 >> 28;
			num6 &= 268435455UL;
			num8 += num7 >> 28;
			num7 &= 268435455UL;
			num9 += num8 >> 28;
			num8 &= 268435455UL;
			num10 += num9 >> 28;
			num9 &= 268435455UL;
			num11 += num10 >> 28;
			num10 &= 268435455UL;
			num12 += num11 >> 28;
			num11 &= 268435455UL;
			num13 += num12 >> 28;
			num12 &= 268435455UL;
			num14 += num13 >> 28;
			num13 &= 268435455UL;
			num15 += num14 >> 28;
			num14 &= 268435455UL;
			num16 += num15 >> 28;
			num15 &= 268435455UL;
			num17 = num16 >> 26;
			num16 &= 67108863UL;
			num17 -= 1UL;
			num -= (num17 & 78101261UL);
			num2 -= (num17 & 141809365UL);
			num3 -= (num17 & 175155932UL);
			num4 -= (num17 & 64542499UL);
			num5 -= (num17 & 158326419UL);
			num6 -= (num17 & 191173276UL);
			num7 -= (num17 & 104575268UL);
			num8 -= (num17 & 137584065UL);
			num2 += num >> 28;
			num &= 268435455UL;
			num3 += num2 >> 28;
			num2 &= 268435455UL;
			num4 += num3 >> 28;
			num3 &= 268435455UL;
			num5 += num4 >> 28;
			num4 &= 268435455UL;
			num6 += num5 >> 28;
			num5 &= 268435455UL;
			num7 += num6 >> 28;
			num6 &= 268435455UL;
			num8 += num7 >> 28;
			num7 &= 268435455UL;
			num9 += num8 >> 28;
			num8 &= 268435455UL;
			num10 += num9 >> 28;
			num9 &= 268435455UL;
			num11 += num10 >> 28;
			num10 &= 268435455UL;
			num12 += num11 >> 28;
			num11 &= 268435455UL;
			num13 += num12 >> 28;
			num12 &= 268435455UL;
			num14 += num13 >> 28;
			num13 &= 268435455UL;
			num15 += num14 >> 28;
			num14 &= 268435455UL;
			num16 += num15 >> 28;
			num15 &= 268435455UL;
			byte[] array = new byte[57];
			Ed448.Encode56(num | num2 << 28, array, 0);
			Ed448.Encode56(num3 | num4 << 28, array, 7);
			Ed448.Encode56(num5 | num6 << 28, array, 14);
			Ed448.Encode56(num7 | num8 << 28, array, 21);
			Ed448.Encode56(num9 | num10 << 28, array, 28);
			Ed448.Encode56(num11 | num12 << 28, array, 35);
			Ed448.Encode56(num13 | num14 << 28, array, 42);
			Ed448.Encode56(num15 | num16 << 28, array, 49);
			return array;
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x000EBACC File Offset: 0x000E9CCC
		private static void ScalarMultBase(byte[] k, Ed448.PointExt r)
		{
			Ed448.Precompute();
			Ed448.PointSetNeutral(r);
			uint[] array = new uint[15];
			Ed448.DecodeScalar(k, 0, array);
			array[14] = 4U + Nat.CAdd(14, (int)(~array[0] & 1U), array, Ed448.L, array);
			Nat.ShiftDownBit(array.Length, array, 0U);
			Ed448.PointPrecomp pointPrecomp = new Ed448.PointPrecomp();
			int num = 17;
			for (;;)
			{
				int num2 = num;
				for (int i = 0; i < 5; i++)
				{
					uint num3 = 0U;
					for (int j = 0; j < 5; j++)
					{
						uint num4 = array[num2 >> 5] >> num2;
						num3 &= ~(1U << j);
						num3 ^= num4 << j;
						num2 += 18;
					}
					int num5 = (int)(num3 >> 4 & 1U);
					int index = (int)((num3 ^ (uint)(-(uint)num5)) & 15U);
					Ed448.PointLookup(i, index, pointPrecomp);
					X448Field.CNegate(num5, pointPrecomp.x);
					Ed448.PointAddPrecomp(pointPrecomp, r);
				}
				if (--num < 0)
				{
					break;
				}
				Ed448.PointDouble(r);
			}
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x000EBBBC File Offset: 0x000E9DBC
		private static void ScalarMultBaseEncoded(byte[] k, byte[] r, int rOff)
		{
			Ed448.PointExt pointExt = new Ed448.PointExt();
			Ed448.ScalarMultBase(k, pointExt);
			Ed448.EncodePoint(pointExt, r, rOff);
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x000EBBE0 File Offset: 0x000E9DE0
		internal static void ScalarMultBaseXY(byte[] k, int kOff, uint[] x, uint[] y)
		{
			byte[] array = new byte[57];
			Ed448.PruneScalar(k, kOff, array);
			Ed448.PointExt pointExt = new Ed448.PointExt();
			Ed448.ScalarMultBase(array, pointExt);
			X448Field.Copy(pointExt.x, 0, x, 0);
			X448Field.Copy(pointExt.y, 0, y, 0);
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x000EBC28 File Offset: 0x000E9E28
		private static void ScalarMultStraussVar(uint[] nb, uint[] np, Ed448.PointExt p, Ed448.PointExt r)
		{
			Ed448.Precompute();
			int num = 5;
			sbyte[] wnaf = Ed448.GetWnaf(nb, 7);
			sbyte[] wnaf2 = Ed448.GetWnaf(np, num);
			Ed448.PointExt[] array = Ed448.PointPrecompVar(p, 1 << num - 2);
			Ed448.PointSetNeutral(r);
			int num2 = 447;
			while (num2 > 0 && (wnaf[num2] | wnaf2[num2]) == 0)
			{
				num2--;
			}
			for (;;)
			{
				int num3 = (int)wnaf[num2];
				if (num3 != 0)
				{
					int num4 = num3 >> 31;
					int num5 = (num3 ^ num4) >> 1;
					Ed448.PointAddVar(num4 != 0, Ed448.precompBaseTable[num5], r);
				}
				int num6 = (int)wnaf2[num2];
				if (num6 != 0)
				{
					int num7 = num6 >> 31;
					int num8 = (num6 ^ num7) >> 1;
					Ed448.PointAddVar(num7 != 0, array[num8], r);
				}
				if (--num2 < 0)
				{
					break;
				}
				Ed448.PointDouble(r);
			}
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x000EBCE8 File Offset: 0x000E9EE8
		public static void Sign(byte[] sk, int skOff, byte[] ctx, byte[] m, int mOff, int mLen, byte[] sig, int sigOff)
		{
			byte phflag = 0;
			Ed448.ImplSign(sk, skOff, ctx, phflag, m, mOff, mLen, sig, sigOff);
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x000EBD0C File Offset: 0x000E9F0C
		public static void Sign(byte[] sk, int skOff, byte[] pk, int pkOff, byte[] ctx, byte[] m, int mOff, int mLen, byte[] sig, int sigOff)
		{
			byte phflag = 0;
			Ed448.ImplSign(sk, skOff, pk, pkOff, ctx, phflag, m, mOff, mLen, sig, sigOff);
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x000EBD34 File Offset: 0x000E9F34
		public static void SignPrehash(byte[] sk, int skOff, byte[] ctx, byte[] ph, int phOff, byte[] sig, int sigOff)
		{
			byte phflag = 1;
			Ed448.ImplSign(sk, skOff, ctx, phflag, ph, phOff, Ed448.PrehashSize, sig, sigOff);
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000EBD58 File Offset: 0x000E9F58
		public static void SignPrehash(byte[] sk, int skOff, byte[] pk, int pkOff, byte[] ctx, byte[] ph, int phOff, byte[] sig, int sigOff)
		{
			byte phflag = 1;
			Ed448.ImplSign(sk, skOff, pk, pkOff, ctx, phflag, ph, phOff, Ed448.PrehashSize, sig, sigOff);
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x000EBD80 File Offset: 0x000E9F80
		public static void SignPrehash(byte[] sk, int skOff, byte[] ctx, IXof ph, byte[] sig, int sigOff)
		{
			byte[] array = new byte[Ed448.PrehashSize];
			if (Ed448.PrehashSize != ph.DoFinal(array, 0, Ed448.PrehashSize))
			{
				throw new ArgumentException("ph");
			}
			byte phflag = 1;
			Ed448.ImplSign(sk, skOff, ctx, phflag, array, 0, array.Length, sig, sigOff);
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x000EBDCC File Offset: 0x000E9FCC
		public static void SignPrehash(byte[] sk, int skOff, byte[] pk, int pkOff, byte[] ctx, IXof ph, byte[] sig, int sigOff)
		{
			byte[] array = new byte[Ed448.PrehashSize];
			if (Ed448.PrehashSize != ph.DoFinal(array, 0, Ed448.PrehashSize))
			{
				throw new ArgumentException("ph");
			}
			byte phflag = 1;
			Ed448.ImplSign(sk, skOff, pk, pkOff, ctx, phflag, array, 0, array.Length, sig, sigOff);
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x000EBE1C File Offset: 0x000EA01C
		public static bool Verify(byte[] sig, int sigOff, byte[] pk, int pkOff, byte[] ctx, byte[] m, int mOff, int mLen)
		{
			byte phflag = 0;
			return Ed448.ImplVerify(sig, sigOff, pk, pkOff, ctx, phflag, m, mOff, mLen);
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x000EBE40 File Offset: 0x000EA040
		public static bool VerifyPrehash(byte[] sig, int sigOff, byte[] pk, int pkOff, byte[] ctx, byte[] ph, int phOff)
		{
			byte phflag = 1;
			return Ed448.ImplVerify(sig, sigOff, pk, pkOff, ctx, phflag, ph, phOff, Ed448.PrehashSize);
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x000EBE64 File Offset: 0x000EA064
		public static bool VerifyPrehash(byte[] sig, int sigOff, byte[] pk, int pkOff, byte[] ctx, IXof ph)
		{
			byte[] array = new byte[Ed448.PrehashSize];
			if (Ed448.PrehashSize != ph.DoFinal(array, 0, Ed448.PrehashSize))
			{
				throw new ArgumentException("ph");
			}
			byte phflag = 1;
			return Ed448.ImplVerify(sig, sigOff, pk, pkOff, ctx, phflag, array, 0, array.Length);
		}

		// Token: 0x040019A3 RID: 6563
		private const ulong M26UL = 67108863UL;

		// Token: 0x040019A4 RID: 6564
		private const ulong M28UL = 268435455UL;

		// Token: 0x040019A5 RID: 6565
		private const int PointBytes = 57;

		// Token: 0x040019A6 RID: 6566
		private const int ScalarUints = 14;

		// Token: 0x040019A7 RID: 6567
		private const int ScalarBytes = 57;

		// Token: 0x040019A8 RID: 6568
		public static readonly int PrehashSize = 64;

		// Token: 0x040019A9 RID: 6569
		public static readonly int PublicKeySize = 57;

		// Token: 0x040019AA RID: 6570
		public static readonly int SecretKeySize = 57;

		// Token: 0x040019AB RID: 6571
		public static readonly int SignatureSize = 114;

		// Token: 0x040019AC RID: 6572
		private static readonly byte[] Dom4Prefix = Strings.ToByteArray("SigEd448");

		// Token: 0x040019AD RID: 6573
		private static readonly uint[] P = new uint[]
		{
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040019AE RID: 6574
		private static readonly uint[] L = new uint[]
		{
			2874688755U,
			595116690U,
			2378534741U,
			560775794U,
			2933274256U,
			3293502281U,
			2093622249U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			1073741823U
		};

		// Token: 0x040019AF RID: 6575
		private static readonly BigInteger N = Nat.ToBigInteger(Ed448.L.Length, Ed448.L);

		// Token: 0x040019B0 RID: 6576
		private const int L_0 = 78101261;

		// Token: 0x040019B1 RID: 6577
		private const int L_1 = 141809365;

		// Token: 0x040019B2 RID: 6578
		private const int L_2 = 175155932;

		// Token: 0x040019B3 RID: 6579
		private const int L_3 = 64542499;

		// Token: 0x040019B4 RID: 6580
		private const int L_4 = 158326419;

		// Token: 0x040019B5 RID: 6581
		private const int L_5 = 191173276;

		// Token: 0x040019B6 RID: 6582
		private const int L_6 = 104575268;

		// Token: 0x040019B7 RID: 6583
		private const int L_7 = 137584065;

		// Token: 0x040019B8 RID: 6584
		private const int L4_0 = 43969588;

		// Token: 0x040019B9 RID: 6585
		private const int L4_1 = 30366549;

		// Token: 0x040019BA RID: 6586
		private const int L4_2 = 163752818;

		// Token: 0x040019BB RID: 6587
		private const int L4_3 = 258169998;

		// Token: 0x040019BC RID: 6588
		private const int L4_4 = 96434764;

		// Token: 0x040019BD RID: 6589
		private const int L4_5 = 227822194;

		// Token: 0x040019BE RID: 6590
		private const int L4_6 = 149865618;

		// Token: 0x040019BF RID: 6591
		private const int L4_7 = 550336261;

		// Token: 0x040019C0 RID: 6592
		private static readonly uint[] B_x = new uint[]
		{
			118276190U,
			40534716U,
			9670182U,
			135141552U,
			85017403U,
			259173222U,
			68333082U,
			171784774U,
			174973732U,
			15824510U,
			73756743U,
			57518561U,
			94773951U,
			248652241U,
			107736333U,
			82941708U
		};

		// Token: 0x040019C1 RID: 6593
		private static readonly uint[] B_y = new uint[]
		{
			36764180U,
			8885695U,
			130592152U,
			20104429U,
			163904957U,
			30304195U,
			121295871U,
			5901357U,
			125344798U,
			171541512U,
			175338348U,
			209069246U,
			3626697U,
			38307682U,
			24032956U,
			110359655U
		};

		// Token: 0x040019C2 RID: 6594
		private const int C_d = -39081;

		// Token: 0x040019C3 RID: 6595
		private const int WnafWidthBase = 7;

		// Token: 0x040019C4 RID: 6596
		private const int PrecompBlocks = 5;

		// Token: 0x040019C5 RID: 6597
		private const int PrecompTeeth = 5;

		// Token: 0x040019C6 RID: 6598
		private const int PrecompSpacing = 18;

		// Token: 0x040019C7 RID: 6599
		private const int PrecompPoints = 16;

		// Token: 0x040019C8 RID: 6600
		private const int PrecompMask = 15;

		// Token: 0x040019C9 RID: 6601
		private static readonly object precompLock = new object();

		// Token: 0x040019CA RID: 6602
		private static Ed448.PointExt[] precompBaseTable = null;

		// Token: 0x040019CB RID: 6603
		private static uint[] precompBase = null;

		// Token: 0x02000915 RID: 2325
		public enum Algorithm
		{
			// Token: 0x0400357A RID: 13690
			Ed448,
			// Token: 0x0400357B RID: 13691
			Ed448ph
		}

		// Token: 0x02000916 RID: 2326
		private class PointExt
		{
			// Token: 0x0400357C RID: 13692
			internal uint[] x = X448Field.Create();

			// Token: 0x0400357D RID: 13693
			internal uint[] y = X448Field.Create();

			// Token: 0x0400357E RID: 13694
			internal uint[] z = X448Field.Create();
		}

		// Token: 0x02000917 RID: 2327
		private class PointPrecomp
		{
			// Token: 0x0400357F RID: 13695
			internal uint[] x = X448Field.Create();

			// Token: 0x04003580 RID: 13696
			internal uint[] y = X448Field.Create();
		}
	}
}
