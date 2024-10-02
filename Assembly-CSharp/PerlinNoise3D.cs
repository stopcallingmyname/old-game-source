using System;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class PerlinNoise3D
{
	// Token: 0x06000A1E RID: 2590 RVA: 0x00081C80 File Offset: 0x0007FE80
	public PerlinNoise3D(float scale)
	{
		this.scale = scale;
		for (int i = 0; i < 256; i++)
		{
			float num = 1f - 2f * Random.value;
			float num2 = Mathf.Sqrt(1f - num * num);
			float f = 6.2831855f * Random.value;
			this.gradients[i].x = num2 * Mathf.Cos(f);
			this.gradients[i].y = num2 * Mathf.Sin(f);
			this.gradients[i].z = num;
		}
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x00081D51 File Offset: 0x0007FF51
	public float Noise(float x, float y, float z)
	{
		return this.PerlinNoise(x * this.scale, y * this.scale, z * this.scale) + 0.5f;
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x00081D78 File Offset: 0x0007FF78
	private float PerlinNoise(float x, float y, float z)
	{
		int num = (int)Mathf.Floor(x);
		float num2 = x - (float)num;
		float fx = num2 - 1f;
		float t = PerlinNoise3D.Smooth(num2);
		int num3 = (int)Mathf.Floor(y);
		float num4 = y - (float)num3;
		float fy = num4 - 1f;
		float t2 = PerlinNoise3D.Smooth(num4);
		int num5 = (int)Mathf.Floor(z);
		float num6 = z - (float)num5;
		float fz = num6 - 1f;
		float t3 = PerlinNoise3D.Smooth(num6);
		float a = this.Lattice(num, num3, num5, num2, num4, num6);
		float b = this.Lattice(num + 1, num3, num5, fx, num4, num6);
		float a2 = Mathf.Lerp(a, b, t);
		float a3 = this.Lattice(num, num3 + 1, num5, num2, fy, num6);
		b = this.Lattice(num + 1, num3 + 1, num5, fx, fy, num6);
		float b2 = Mathf.Lerp(a3, b, t);
		float a4 = Mathf.Lerp(a2, b2, t2);
		float a5 = this.Lattice(num, num3, num5 + 1, num2, num4, fz);
		b = this.Lattice(num + 1, num3, num5 + 1, fx, num4, fz);
		float a6 = Mathf.Lerp(a5, b, t);
		float a7 = this.Lattice(num, num3 + 1, num5 + 1, num2, fy, fz);
		b = this.Lattice(num + 1, num3 + 1, num5 + 1, fx, fy, fz);
		b2 = Mathf.Lerp(a7, b, t);
		float b3 = Mathf.Lerp(a6, b2, t2);
		return Mathf.Lerp(a4, b3, t3);
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x00081EC8 File Offset: 0x000800C8
	private int Index(int ix, int iy, int iz)
	{
		return this.Permutate(ix + this.Permutate(iy + this.Permutate(iz)));
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x00081EE4 File Offset: 0x000800E4
	private int Permutate(int x)
	{
		int num = 255;
		return (int)this.perm[x & num];
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x00081F04 File Offset: 0x00080104
	private float Lattice(int ix, int iy, int iz, float fx, float fy, float fz)
	{
		int num = this.Index(ix, iy, iz);
		return this.gradients[num].x * fx + this.gradients[num].y * fy + this.gradients[num].z * fz;
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x00081F59 File Offset: 0x00080159
	private static float Smooth(float x)
	{
		return x * x * (3f - 2f * x);
	}

	// Token: 0x04000FCE RID: 4046
	private const int GradientSizeTable = 256;

	// Token: 0x04000FCF RID: 4047
	private Vector3[] gradients = new Vector3[256];

	// Token: 0x04000FD0 RID: 4048
	private short[] perm = new short[]
	{
		225,
		155,
		210,
		108,
		175,
		199,
		221,
		144,
		203,
		116,
		70,
		213,
		69,
		158,
		33,
		252,
		5,
		82,
		173,
		133,
		222,
		139,
		174,
		27,
		9,
		71,
		90,
		246,
		75,
		130,
		91,
		191,
		169,
		138,
		2,
		151,
		194,
		235,
		81,
		7,
		25,
		113,
		228,
		159,
		205,
		253,
		134,
		142,
		248,
		65,
		224,
		217,
		22,
		121,
		229,
		63,
		89,
		103,
		96,
		104,
		156,
		17,
		201,
		129,
		36,
		8,
		165,
		110,
		237,
		117,
		231,
		56,
		132,
		211,
		152,
		20,
		181,
		111,
		239,
		218,
		170,
		163,
		51,
		172,
		157,
		47,
		80,
		212,
		176,
		250,
		87,
		49,
		99,
		242,
		136,
		189,
		162,
		115,
		44,
		43,
		124,
		94,
		150,
		16,
		141,
		247,
		32,
		10,
		198,
		223,
		255,
		72,
		53,
		131,
		84,
		57,
		220,
		197,
		58,
		50,
		208,
		11,
		241,
		28,
		3,
		192,
		62,
		202,
		18,
		215,
		153,
		24,
		76,
		41,
		15,
		179,
		39,
		46,
		55,
		6,
		128,
		167,
		23,
		188,
		106,
		34,
		187,
		140,
		164,
		73,
		112,
		182,
		244,
		195,
		227,
		13,
		35,
		77,
		196,
		185,
		26,
		200,
		226,
		119,
		31,
		123,
		168,
		125,
		249,
		68,
		183,
		230,
		177,
		135,
		160,
		180,
		12,
		1,
		243,
		148,
		102,
		166,
		38,
		238,
		251,
		37,
		240,
		126,
		64,
		74,
		161,
		40,
		184,
		149,
		171,
		178,
		101,
		66,
		29,
		59,
		146,
		61,
		254,
		107,
		42,
		86,
		154,
		4,
		236,
		232,
		120,
		21,
		233,
		209,
		45,
		98,
		193,
		114,
		78,
		19,
		206,
		14,
		118,
		127,
		48,
		79,
		147,
		85,
		30,
		207,
		219,
		54,
		88,
		234,
		190,
		122,
		95,
		67,
		143,
		109,
		137,
		214,
		145,
		93,
		92,
		100,
		245,
		0,
		216,
		186,
		60,
		83,
		105,
		97,
		204,
		52
	};

	// Token: 0x04000FD1 RID: 4049
	private float scale = 1f;
}
