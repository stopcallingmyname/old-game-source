using System;
using System.Collections;
using System.Text;
using BestHTTP;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public class UTILS
{
	// Token: 0x060005A4 RID: 1444 RVA: 0x000445E0 File Offset: 0x000427E0
	public static float ANGLE_SIGNED(Vector3 v1, Vector3 v2, Vector3 n)
	{
		return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * 57.29578f;
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x00067D63 File Offset: 0x00065F63
	public static IEnumerator PostMyItem(int _type)
	{
		if (string.IsNullOrEmpty(PlayerProfile.screenShotURL))
		{
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		int num = 780;
		int num2 = 524;
		if (_type == 2 || _type == 3)
		{
			num = 800;
			num2 = 600;
		}
		else if (_type == 4)
		{
			num = 400;
			num2 = 500;
		}
		Texture2D texture2D = new Texture2D(num, num2, TextureFormat.RGB24, false);
		texture2D.ReadPixels(new Rect((float)(Screen.width / 2 - num / 2), (float)(Screen.height / 2 - num2 / 2), (float)num, (float)num2), 0, 0);
		texture2D.Apply(false);
		byte[] contents = texture2D.EncodeToPNG();
		WWWForm wwwform = new WWWForm();
		wwwform.AddBinaryData("photo", contents);
		WWW w = new WWW(PlayerProfile.screenShotURL, wwwform);
		yield return w;
		if (!string.IsNullOrEmpty(w.error))
		{
			Debug.Log(w.error);
		}
		else if (_type == 1)
		{
			Application.ExternalCall("PostMyItem", new object[]
			{
				w.text
			});
		}
		else if (_type == 2)
		{
			Application.ExternalCall("PostMyGift", new object[]
			{
				w.text
			});
		}
		else if (_type == 3)
		{
			Application.ExternalCall("PostMyBox", new object[]
			{
				w.text
			});
		}
		else if (_type == 4)
		{
			Application.ExternalCall("PostNewLvl", new object[]
			{
				w.text
			});
		}
		yield break;
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x00067D72 File Offset: 0x00065F72
	public static void DownloadImage(string url, OnRequestFinishedDelegate _callback)
	{
		if (string.IsNullOrEmpty(url))
		{
			return;
		}
		new HTTPRequest(new Uri(url), _callback).Send();
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00067D8F File Offset: 0x00065F8F
	public static void BEGIN_WRITE(int size)
	{
		UTILS.tmpBuffer = new byte[size];
		UTILS.pos = 0;
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x00067DA2 File Offset: 0x00065FA2
	public static void WRITE_BYTE(byte bvalue)
	{
		UTILS.tmpBuffer[UTILS.pos] = bvalue;
		UTILS.pos++;
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x00067DBC File Offset: 0x00065FBC
	public static void WRITE_SHORT(short svalue)
	{
		byte[] array = UTILS.EncodeShort(svalue);
		UTILS.tmpBuffer[UTILS.pos] = array[0];
		UTILS.tmpBuffer[UTILS.pos + 1] = array[1];
		UTILS.pos += 2;
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x00067DFC File Offset: 0x00065FFC
	public static void WRITE_FLOAT(float fvalue)
	{
		byte[] array = UTILS.EncodeFloat(fvalue);
		UTILS.tmpBuffer[UTILS.pos] = array[0];
		UTILS.tmpBuffer[UTILS.pos + 1] = array[1];
		UTILS.tmpBuffer[UTILS.pos + 2] = array[2];
		UTILS.tmpBuffer[UTILS.pos + 3] = array[3];
		UTILS.pos += 4;
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x00067E5C File Offset: 0x0006605C
	public static void WRITE_LONG(int ivalue)
	{
		byte[] array = UTILS.EncodeInteger(ivalue);
		UTILS.tmpBuffer[UTILS.pos] = array[0];
		UTILS.tmpBuffer[UTILS.pos + 1] = array[1];
		UTILS.tmpBuffer[UTILS.pos + 2] = array[2];
		UTILS.tmpBuffer[UTILS.pos + 3] = array[3];
		UTILS.pos += 4;
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x00067EBC File Offset: 0x000660BC
	public static void WRITE_STRING(string svalue)
	{
		UTF8Encoding utf8Encoding = new UTF8Encoding();
		int byteCount = utf8Encoding.GetByteCount(svalue);
		UTILS.WRITE_LONG(byteCount);
		byte[] array = new byte[byteCount];
		Buffer.BlockCopy(utf8Encoding.GetBytes(svalue), 0, array, 0, byteCount);
		for (int i = 0; i < byteCount; i++)
		{
			UTILS.WRITE_BYTE(array[i]);
		}
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x00067F06 File Offset: 0x00066106
	public static int WRITE_LEN()
	{
		return UTILS.pos;
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x00067F0D File Offset: 0x0006610D
	public static byte[] END_WRITE()
	{
		return UTILS.tmpBuffer;
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x00067F14 File Offset: 0x00066114
	public static void BEGIN_READ(byte[] inBytes, int _pos = 0)
	{
		UTILS.tmpBuffer = inBytes;
		UTILS.readlen = UTILS.tmpBuffer.Length;
		UTILS.pos = _pos;
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x00067F06 File Offset: 0x00066106
	public static int GET_POS()
	{
		return UTILS.pos;
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x00067F2E File Offset: 0x0006612E
	public static int GET_LEN()
	{
		return UTILS.readlen;
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x00067F38 File Offset: 0x00066138
	public static byte READ_BYTE()
	{
		if (UTILS.pos >= UTILS.readlen)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"PACKET: ",
				UTILS.tmpBuffer[1],
				" - POS = ",
				UTILS.pos,
				" LEN = ",
				UTILS.readlen
			}));
			UTILS.ERROR = true;
			return 0;
		}
		byte result = UTILS.tmpBuffer[UTILS.pos];
		UTILS.pos++;
		return result;
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x00067FC4 File Offset: 0x000661C4
	public static float READ_ANGLE()
	{
		if (UTILS.pos >= UTILS.readlen)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"PACKET: ",
				UTILS.tmpBuffer[1],
				" - POS = ",
				UTILS.pos,
				" LEN = ",
				UTILS.readlen
			}));
			UTILS.ERROR = true;
			return 0f;
		}
		float result = (float)UTILS.tmpBuffer[UTILS.pos] * 360f / 255f;
		UTILS.pos++;
		return result;
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x00068060 File Offset: 0x00066260
	public static float READ_COORD()
	{
		if (UTILS.pos + 2 > UTILS.readlen)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"PACKET: ",
				UTILS.tmpBuffer[1],
				" - POS = ",
				(UTILS.pos + 2).ToString(),
				" LEN = ",
				UTILS.readlen
			}));
			UTILS.ERROR = true;
			return 0f;
		}
		float result = (float)UTILS.DecodeShort(UTILS.tmpBuffer, UTILS.pos) * 0.015873017f;
		UTILS.pos += 2;
		return result;
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x00068100 File Offset: 0x00066300
	public static ushort READ_SHORT()
	{
		if (UTILS.pos + 2 > UTILS.readlen)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"PACKET: ",
				UTILS.tmpBuffer[1],
				" - POS = ",
				(UTILS.pos + 2).ToString(),
				" LEN = ",
				UTILS.readlen
			}));
			UTILS.ERROR = true;
			return 0;
		}
		ushort result = UTILS.DecodeShort(UTILS.tmpBuffer, UTILS.pos);
		UTILS.pos += 2;
		return result;
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x00068194 File Offset: 0x00066394
	public static int READ_LONG()
	{
		if (UTILS.pos + 4 > UTILS.readlen)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"PACKET: ",
				UTILS.tmpBuffer[1],
				" - POS = ",
				(UTILS.pos + 4).ToString(),
				" LEN = ",
				UTILS.readlen
			}));
			UTILS.ERROR = true;
			return 0;
		}
		int result = UTILS.DecodeInteger(UTILS.tmpBuffer, UTILS.pos);
		UTILS.pos += 4;
		return result;
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x00068228 File Offset: 0x00066428
	public static string READ_STRING()
	{
		int num = 0;
		int index = UTILS.pos;
		while (UTILS.pos < UTILS.readlen && UTILS.tmpBuffer[UTILS.pos] != 0)
		{
			num++;
			UTILS.pos++;
		}
		UTILS.pos++;
		if (num == 0)
		{
			return "";
		}
		return Encoding.UTF8.GetString(UTILS.tmpBuffer, index, num);
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x0006828E File Offset: 0x0006648E
	public static byte[] EncodeShort(short inShort)
	{
		return BitConverter.GetBytes(inShort);
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x00068296 File Offset: 0x00066496
	public static byte[] EncodeInteger(int inInt)
	{
		return BitConverter.GetBytes(inInt);
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x0006829E File Offset: 0x0006649E
	public static byte[] EncodeFloat(float inFloat)
	{
		return BitConverter.GetBytes(inFloat);
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x000682A8 File Offset: 0x000664A8
	public static byte[] EncodeStringUTF8(string inString)
	{
		UTF8Encoding utf8Encoding = new UTF8Encoding();
		int byteCount = utf8Encoding.GetByteCount(inString);
		byte[] array = new byte[byteCount];
		Buffer.BlockCopy(utf8Encoding.GetBytes(inString), 0, array, 0, byteCount);
		return array;
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x000682DC File Offset: 0x000664DC
	public static byte[] EncodeStringASCII(string inString)
	{
		ASCIIEncoding asciiencoding = new ASCIIEncoding();
		int byteCount = asciiencoding.GetByteCount(inString);
		byte[] array = new byte[byteCount];
		Buffer.BlockCopy(asciiencoding.GetBytes(inString), 0, array, 0, byteCount);
		return array;
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x00068310 File Offset: 0x00066510
	public static byte[] EncodeVector2(Vector2 inObject)
	{
		byte[] array = new byte[8];
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.x), 0, array, 0, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.y), 0, array, 4, 4);
		return array;
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x00068350 File Offset: 0x00066550
	public static byte[] EncodeVector3(Vector3 inObject)
	{
		byte[] array = new byte[12];
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.x), 0, array, 0, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.y), 0, array, 4, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.z), 0, array, 8, 4);
		return array;
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x000683A4 File Offset: 0x000665A4
	public static byte[] EncodeVector4(Vector4 inObject)
	{
		byte[] array = new byte[16];
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.x), 0, array, 0, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.y), 0, array, 4, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.z), 0, array, 8, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.w), 0, array, 12, 4);
		return array;
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x0006840C File Offset: 0x0006660C
	public static byte[] EncodeQuaternion(Quaternion inObject)
	{
		byte[] array = new byte[16];
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.x), 0, array, 0, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.y), 0, array, 4, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.z), 0, array, 8, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.w), 0, array, 12, 4);
		return array;
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x00068474 File Offset: 0x00066674
	public static byte[] EncodeColor(Color inObject)
	{
		byte[] array = new byte[16];
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.r), 0, array, 0, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.g), 0, array, 4, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.b), 0, array, 8, 4);
		Buffer.BlockCopy(BitConverter.GetBytes(inObject.a), 0, array, 12, 4);
		return array;
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x000684DB File Offset: 0x000666DB
	public static ushort DecodeShort(byte[] inBytes, int pos)
	{
		return BitConverter.ToUInt16(inBytes, pos);
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x000684E4 File Offset: 0x000666E4
	public static int DecodeInteger(byte[] inBytes, int pos)
	{
		return BitConverter.ToInt32(inBytes, pos);
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x000684ED File Offset: 0x000666ED
	public static string DecodeString(byte[] inBytes)
	{
		return Encoding.UTF8.GetString(inBytes);
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x000684FA File Offset: 0x000666FA
	public static float XRES(float val)
	{
		return val * ((float)Screen.width / 1024f);
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x0006850A File Offset: 0x0006670A
	public static float YRES(float val)
	{
		return val * ((float)Screen.height / 768f);
	}

	// Token: 0x04000C45 RID: 3141
	private static byte[] tmpBuffer;

	// Token: 0x04000C46 RID: 3142
	private static int pos;

	// Token: 0x04000C47 RID: 3143
	private static int readlen;

	// Token: 0x04000C48 RID: 3144
	public static bool ERROR;
}
