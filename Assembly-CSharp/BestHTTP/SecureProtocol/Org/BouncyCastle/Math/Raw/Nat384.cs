﻿using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Raw
{
	// Token: 0x02000310 RID: 784
	internal abstract class Nat384
	{
		// Token: 0x06001DCD RID: 7629 RVA: 0x000E0A0C File Offset: 0x000DEC0C
		public static void Mul(uint[] x, uint[] y, uint[] zz)
		{
			Nat192.Mul(x, y, zz);
			Nat192.Mul(x, 6, y, 6, zz, 12);
			uint num = Nat192.AddToEachOther(zz, 6, zz, 12);
			uint cIn = num + Nat192.AddTo(zz, 0, zz, 6, 0U);
			num += Nat192.AddTo(zz, 18, zz, 12, cIn);
			uint[] array = Nat192.Create();
			uint[] array2 = Nat192.Create();
			bool flag = Nat192.Diff(x, 6, x, 0, array, 0) != Nat192.Diff(y, 6, y, 0, array2, 0);
			uint[] array3 = Nat192.CreateExt();
			Nat192.Mul(array, array2, array3);
			num += (flag ? Nat.AddTo(12, array3, 0, zz, 6) : ((uint)Nat.SubFrom(12, array3, 0, zz, 6)));
			Nat.AddWordAt(24, num, zz, 18);
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x000E0ABC File Offset: 0x000DECBC
		public static void Square(uint[] x, uint[] zz)
		{
			Nat192.Square(x, zz);
			Nat192.Square(x, 6, zz, 12);
			uint num = Nat192.AddToEachOther(zz, 6, zz, 12);
			uint cIn = num + Nat192.AddTo(zz, 0, zz, 6, 0U);
			num += Nat192.AddTo(zz, 18, zz, 12, cIn);
			uint[] array = Nat192.Create();
			Nat192.Diff(x, 6, x, 0, array, 0);
			uint[] array2 = Nat192.CreateExt();
			Nat192.Square(array, array2);
			num += (uint)Nat.SubFrom(12, array2, 0, zz, 6);
			Nat.AddWordAt(24, num, zz, 18);
		}
	}
}
