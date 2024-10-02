using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200048E RID: 1166
	public abstract class TlsUtilities
	{
		// Token: 0x06002D70 RID: 11632 RVA: 0x0011BEEA File Offset: 0x0011A0EA
		public static void CheckUint8(int i)
		{
			if (!TlsUtilities.IsValidUint8(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x0011BEFC File Offset: 0x0011A0FC
		public static void CheckUint8(long i)
		{
			if (!TlsUtilities.IsValidUint8(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x0011BF0E File Offset: 0x0011A10E
		public static void CheckUint16(int i)
		{
			if (!TlsUtilities.IsValidUint16(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x0011BF20 File Offset: 0x0011A120
		public static void CheckUint16(long i)
		{
			if (!TlsUtilities.IsValidUint16(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x0011BF32 File Offset: 0x0011A132
		public static void CheckUint24(int i)
		{
			if (!TlsUtilities.IsValidUint24(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x0011BF44 File Offset: 0x0011A144
		public static void CheckUint24(long i)
		{
			if (!TlsUtilities.IsValidUint24(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x0011BF56 File Offset: 0x0011A156
		public static void CheckUint32(long i)
		{
			if (!TlsUtilities.IsValidUint32(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x0011BF68 File Offset: 0x0011A168
		public static void CheckUint48(long i)
		{
			if (!TlsUtilities.IsValidUint48(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x0011BF7A File Offset: 0x0011A17A
		public static void CheckUint64(long i)
		{
			if (!TlsUtilities.IsValidUint64(i))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x0011BF8C File Offset: 0x0011A18C
		public static bool IsValidUint8(int i)
		{
			return (i & 255) == i;
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x0011BF98 File Offset: 0x0011A198
		public static bool IsValidUint8(long i)
		{
			return (i & 255L) == i;
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x0011BFA5 File Offset: 0x0011A1A5
		public static bool IsValidUint16(int i)
		{
			return (i & 65535) == i;
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x0011BFB1 File Offset: 0x0011A1B1
		public static bool IsValidUint16(long i)
		{
			return (i & 65535L) == i;
		}

		// Token: 0x06002D7D RID: 11645 RVA: 0x0011BFBE File Offset: 0x0011A1BE
		public static bool IsValidUint24(int i)
		{
			return (i & 16777215) == i;
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x0011BFCA File Offset: 0x0011A1CA
		public static bool IsValidUint24(long i)
		{
			return (i & 16777215L) == i;
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x0011BFD7 File Offset: 0x0011A1D7
		public static bool IsValidUint32(long i)
		{
			return (i & (long)((ulong)-1)) == i;
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x0011BFE0 File Offset: 0x0011A1E0
		public static bool IsValidUint48(long i)
		{
			return (i & 281474976710655L) == i;
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x0006AE98 File Offset: 0x00069098
		public static bool IsValidUint64(long i)
		{
			return true;
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x0011BFF0 File Offset: 0x0011A1F0
		public static bool IsSsl(TlsContext context)
		{
			return context.ServerVersion.IsSsl;
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x0011BFFD File Offset: 0x0011A1FD
		public static bool IsTlsV11(ProtocolVersion version)
		{
			return ProtocolVersion.TLSv11.IsEqualOrEarlierVersionOf(version.GetEquivalentTLSVersion());
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x0011C00F File Offset: 0x0011A20F
		public static bool IsTlsV11(TlsContext context)
		{
			return TlsUtilities.IsTlsV11(context.ServerVersion);
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x0011C01C File Offset: 0x0011A21C
		public static bool IsTlsV12(ProtocolVersion version)
		{
			return ProtocolVersion.TLSv12.IsEqualOrEarlierVersionOf(version.GetEquivalentTLSVersion());
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x0011C02E File Offset: 0x0011A22E
		public static bool IsTlsV12(TlsContext context)
		{
			return TlsUtilities.IsTlsV12(context.ServerVersion);
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x0011C03B File Offset: 0x0011A23B
		public static void WriteUint8(byte i, Stream output)
		{
			output.WriteByte(i);
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x0011C044 File Offset: 0x0011A244
		public static void WriteUint8(byte i, byte[] buf, int offset)
		{
			buf[offset] = i;
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x0011C04A File Offset: 0x0011A24A
		public static void WriteUint16(int i, Stream output)
		{
			output.WriteByte((byte)(i >> 8));
			output.WriteByte((byte)i);
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x0010CD53 File Offset: 0x0010AF53
		public static void WriteUint16(int i, byte[] buf, int offset)
		{
			buf[offset] = (byte)(i >> 8);
			buf[offset + 1] = (byte)i;
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x0011C05E File Offset: 0x0011A25E
		public static void WriteUint24(int i, Stream output)
		{
			output.WriteByte((byte)(i >> 16));
			output.WriteByte((byte)(i >> 8));
			output.WriteByte((byte)i);
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x0011C07D File Offset: 0x0011A27D
		public static void WriteUint24(int i, byte[] buf, int offset)
		{
			buf[offset] = (byte)(i >> 16);
			buf[offset + 1] = (byte)(i >> 8);
			buf[offset + 2] = (byte)i;
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x0011C097 File Offset: 0x0011A297
		public static void WriteUint32(long i, Stream output)
		{
			output.WriteByte((byte)(i >> 24));
			output.WriteByte((byte)(i >> 16));
			output.WriteByte((byte)(i >> 8));
			output.WriteByte((byte)i);
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x0011C0C1 File Offset: 0x0011A2C1
		public static void WriteUint32(long i, byte[] buf, int offset)
		{
			buf[offset] = (byte)(i >> 24);
			buf[offset + 1] = (byte)(i >> 16);
			buf[offset + 2] = (byte)(i >> 8);
			buf[offset + 3] = (byte)i;
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x0011C0E5 File Offset: 0x0011A2E5
		public static void WriteUint48(long i, Stream output)
		{
			output.WriteByte((byte)(i >> 40));
			output.WriteByte((byte)(i >> 32));
			output.WriteByte((byte)(i >> 24));
			output.WriteByte((byte)(i >> 16));
			output.WriteByte((byte)(i >> 8));
			output.WriteByte((byte)i);
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x0011C125 File Offset: 0x0011A325
		public static void WriteUint48(long i, byte[] buf, int offset)
		{
			buf[offset] = (byte)(i >> 40);
			buf[offset + 1] = (byte)(i >> 32);
			buf[offset + 2] = (byte)(i >> 24);
			buf[offset + 3] = (byte)(i >> 16);
			buf[offset + 4] = (byte)(i >> 8);
			buf[offset + 5] = (byte)i;
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x0011C160 File Offset: 0x0011A360
		public static void WriteUint64(long i, Stream output)
		{
			output.WriteByte((byte)(i >> 56));
			output.WriteByte((byte)(i >> 48));
			output.WriteByte((byte)(i >> 40));
			output.WriteByte((byte)(i >> 32));
			output.WriteByte((byte)(i >> 24));
			output.WriteByte((byte)(i >> 16));
			output.WriteByte((byte)(i >> 8));
			output.WriteByte((byte)i);
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x0011C1C4 File Offset: 0x0011A3C4
		public static void WriteUint64(long i, byte[] buf, int offset)
		{
			buf[offset] = (byte)(i >> 56);
			buf[offset + 1] = (byte)(i >> 48);
			buf[offset + 2] = (byte)(i >> 40);
			buf[offset + 3] = (byte)(i >> 32);
			buf[offset + 4] = (byte)(i >> 24);
			buf[offset + 5] = (byte)(i >> 16);
			buf[offset + 6] = (byte)(i >> 8);
			buf[offset + 7] = (byte)i;
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x0011C21B File Offset: 0x0011A41B
		public static void WriteOpaque8(byte[] buf, Stream output)
		{
			TlsUtilities.WriteUint8((byte)buf.Length, output);
			output.Write(buf, 0, buf.Length);
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x0011C232 File Offset: 0x0011A432
		public static void WriteOpaque16(byte[] buf, Stream output)
		{
			TlsUtilities.WriteUint16(buf.Length, output);
			output.Write(buf, 0, buf.Length);
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x0011C248 File Offset: 0x0011A448
		public static void WriteOpaque24(byte[] buf, Stream output)
		{
			TlsUtilities.WriteUint24(buf.Length, output);
			output.Write(buf, 0, buf.Length);
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x0011C25E File Offset: 0x0011A45E
		public static void WriteUint8Array(byte[] uints, Stream output)
		{
			output.Write(uints, 0, uints.Length);
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x0011C26C File Offset: 0x0011A46C
		public static void WriteUint8Array(byte[] uints, byte[] buf, int offset)
		{
			for (int i = 0; i < uints.Length; i++)
			{
				TlsUtilities.WriteUint8(uints[i], buf, offset);
				offset++;
			}
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x0011C296 File Offset: 0x0011A496
		public static void WriteUint8ArrayWithUint8Length(byte[] uints, Stream output)
		{
			TlsUtilities.CheckUint8(uints.Length);
			TlsUtilities.WriteUint8((byte)uints.Length, output);
			TlsUtilities.WriteUint8Array(uints, output);
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x0011C2B1 File Offset: 0x0011A4B1
		public static void WriteUint8ArrayWithUint8Length(byte[] uints, byte[] buf, int offset)
		{
			TlsUtilities.CheckUint8(uints.Length);
			TlsUtilities.WriteUint8((byte)uints.Length, buf, offset);
			TlsUtilities.WriteUint8Array(uints, buf, offset + 1);
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x0011C2D0 File Offset: 0x0011A4D0
		public static void WriteUint16Array(int[] uints, Stream output)
		{
			for (int i = 0; i < uints.Length; i++)
			{
				TlsUtilities.WriteUint16(uints[i], output);
			}
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x0011C2F4 File Offset: 0x0011A4F4
		public static void WriteUint16Array(int[] uints, byte[] buf, int offset)
		{
			for (int i = 0; i < uints.Length; i++)
			{
				TlsUtilities.WriteUint16(uints[i], buf, offset);
				offset += 2;
			}
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x0011C31E File Offset: 0x0011A51E
		public static void WriteUint16ArrayWithUint16Length(int[] uints, Stream output)
		{
			int i = 2 * uints.Length;
			TlsUtilities.CheckUint16(i);
			TlsUtilities.WriteUint16(i, output);
			TlsUtilities.WriteUint16Array(uints, output);
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x0011C338 File Offset: 0x0011A538
		public static void WriteUint16ArrayWithUint16Length(int[] uints, byte[] buf, int offset)
		{
			int i = 2 * uints.Length;
			TlsUtilities.CheckUint16(i);
			TlsUtilities.WriteUint16(i, buf, offset);
			TlsUtilities.WriteUint16Array(uints, buf, offset + 2);
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x0011C356 File Offset: 0x0011A556
		public static byte DecodeUint8(byte[] buf)
		{
			if (buf == null)
			{
				throw new ArgumentNullException("buf");
			}
			if (buf.Length != 1)
			{
				throw new TlsFatalAlert(50);
			}
			return TlsUtilities.ReadUint8(buf, 0);
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x0011C37C File Offset: 0x0011A57C
		public static byte[] DecodeUint8ArrayWithUint8Length(byte[] buf)
		{
			if (buf == null)
			{
				throw new ArgumentNullException("buf");
			}
			int num = (int)TlsUtilities.ReadUint8(buf, 0);
			if (buf.Length != num + 1)
			{
				throw new TlsFatalAlert(50);
			}
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = TlsUtilities.ReadUint8(buf, i + 1);
			}
			return array;
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x0011C3CF File Offset: 0x0011A5CF
		public static byte[] EncodeOpaque8(byte[] buf)
		{
			TlsUtilities.CheckUint8(buf.Length);
			return Arrays.Prepend(buf, (byte)buf.Length);
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x0011C3E4 File Offset: 0x0011A5E4
		public static byte[] EncodeUint8(byte val)
		{
			TlsUtilities.CheckUint8((int)val);
			byte[] array = new byte[1];
			TlsUtilities.WriteUint8(val, array, 0);
			return array;
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x0011C408 File Offset: 0x0011A608
		public static byte[] EncodeUint8ArrayWithUint8Length(byte[] uints)
		{
			byte[] array = new byte[1 + uints.Length];
			TlsUtilities.WriteUint8ArrayWithUint8Length(uints, array, 0);
			return array;
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x0011C42C File Offset: 0x0011A62C
		public static byte[] EncodeUint16ArrayWithUint16Length(int[] uints)
		{
			int num = 2 * uints.Length;
			byte[] array = new byte[2 + num];
			TlsUtilities.WriteUint16ArrayWithUint16Length(uints, array, 0);
			return array;
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x0011C451 File Offset: 0x0011A651
		public static byte ReadUint8(Stream input)
		{
			int num = input.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException();
			}
			return (byte)num;
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x0011C464 File Offset: 0x0011A664
		public static byte ReadUint8(byte[] buf, int offset)
		{
			return buf[offset];
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x0011C46C File Offset: 0x0011A66C
		public static int ReadUint16(Stream input)
		{
			int num = input.ReadByte();
			int num2 = input.ReadByte();
			if (num2 < 0)
			{
				throw new EndOfStreamException();
			}
			return num << 8 | num2;
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x0011C494 File Offset: 0x0011A694
		public static int ReadUint16(byte[] buf, int offset)
		{
			return (int)buf[offset] << 8 | (int)buf[++offset];
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x0011C4A4 File Offset: 0x0011A6A4
		public static int ReadUint24(Stream input)
		{
			int num = input.ReadByte();
			int num2 = input.ReadByte();
			int num3 = input.ReadByte();
			if (num3 < 0)
			{
				throw new EndOfStreamException();
			}
			return num << 16 | num2 << 8 | num3;
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x0011C4D8 File Offset: 0x0011A6D8
		public static int ReadUint24(byte[] buf, int offset)
		{
			return (int)buf[offset] << 16 | (int)buf[++offset] << 8 | (int)buf[++offset];
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x0011C4F4 File Offset: 0x0011A6F4
		public static long ReadUint32(Stream input)
		{
			int num = input.ReadByte();
			int num2 = input.ReadByte();
			int num3 = input.ReadByte();
			int num4 = input.ReadByte();
			if (num4 < 0)
			{
				throw new EndOfStreamException();
			}
			return (long)((ulong)(num << 24 | num2 << 16 | num3 << 8 | num4));
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x0011C535 File Offset: 0x0011A735
		public static long ReadUint32(byte[] buf, int offset)
		{
			return (long)((ulong)((int)buf[offset] << 24 | (int)buf[++offset] << 16 | (int)buf[++offset] << 8 | (int)buf[++offset]));
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x0011C560 File Offset: 0x0011A760
		public static long ReadUint48(Stream input)
		{
			long num = (long)TlsUtilities.ReadUint24(input);
			int num2 = TlsUtilities.ReadUint24(input);
			return (num & (long)((ulong)-1)) << 24 | ((long)num2 & (long)((ulong)-1));
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x0011C588 File Offset: 0x0011A788
		public static long ReadUint48(byte[] buf, int offset)
		{
			long num = (long)TlsUtilities.ReadUint24(buf, offset);
			int num2 = TlsUtilities.ReadUint24(buf, offset + 3);
			return (num & (long)((ulong)-1)) << 24 | ((long)num2 & (long)((ulong)-1));
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x0011C5B4 File Offset: 0x0011A7B4
		public static byte[] ReadAllOrNothing(int length, Stream input)
		{
			if (length < 1)
			{
				return TlsUtilities.EmptyBytes;
			}
			byte[] array = new byte[length];
			int num = Streams.ReadFully(input, array);
			if (num == 0)
			{
				return null;
			}
			if (num != length)
			{
				throw new EndOfStreamException();
			}
			return array;
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x0011C5EC File Offset: 0x0011A7EC
		public static byte[] ReadFully(int length, Stream input)
		{
			if (length < 1)
			{
				return TlsUtilities.EmptyBytes;
			}
			byte[] array = new byte[length];
			if (length != Streams.ReadFully(input, array))
			{
				throw new EndOfStreamException();
			}
			return array;
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x0011C61B File Offset: 0x0011A81B
		public static void ReadFully(byte[] buf, Stream input)
		{
			if (Streams.ReadFully(input, buf, 0, buf.Length) < buf.Length)
			{
				throw new EndOfStreamException();
			}
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x0011C633 File Offset: 0x0011A833
		public static byte[] ReadOpaque8(Stream input)
		{
			byte[] array = new byte[(int)TlsUtilities.ReadUint8(input)];
			TlsUtilities.ReadFully(array, input);
			return array;
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x0011C647 File Offset: 0x0011A847
		public static byte[] ReadOpaque16(Stream input)
		{
			byte[] array = new byte[TlsUtilities.ReadUint16(input)];
			TlsUtilities.ReadFully(array, input);
			return array;
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x0011C65B File Offset: 0x0011A85B
		public static byte[] ReadOpaque24(Stream input)
		{
			return TlsUtilities.ReadFully(TlsUtilities.ReadUint24(input), input);
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x0011C66C File Offset: 0x0011A86C
		public static byte[] ReadUint8Array(int count, Stream input)
		{
			byte[] array = new byte[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = TlsUtilities.ReadUint8(input);
			}
			return array;
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x0011C698 File Offset: 0x0011A898
		public static int[] ReadUint16Array(int count, Stream input)
		{
			int[] array = new int[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = TlsUtilities.ReadUint16(input);
			}
			return array;
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x0011C6C2 File Offset: 0x0011A8C2
		public static ProtocolVersion ReadVersion(byte[] buf, int offset)
		{
			return ProtocolVersion.Get((int)buf[offset], (int)buf[offset + 1]);
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x0011C6D4 File Offset: 0x0011A8D4
		public static ProtocolVersion ReadVersion(Stream input)
		{
			int major = input.ReadByte();
			int num = input.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException();
			}
			return ProtocolVersion.Get(major, num);
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x0011C6FE File Offset: 0x0011A8FE
		public static int ReadVersionRaw(byte[] buf, int offset)
		{
			return (int)buf[offset] << 8 | (int)buf[offset + 1];
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x0011C70C File Offset: 0x0011A90C
		public static int ReadVersionRaw(Stream input)
		{
			int num = input.ReadByte();
			int num2 = input.ReadByte();
			if (num2 < 0)
			{
				throw new EndOfStreamException();
			}
			return num << 8 | num2;
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x0011C734 File Offset: 0x0011A934
		public static Asn1Object ReadAsn1Object(byte[] encoding)
		{
			MemoryStream memoryStream = new MemoryStream(encoding, false);
			Asn1Object asn1Object = new Asn1InputStream(memoryStream, encoding.Length).ReadObject();
			if (asn1Object == null)
			{
				throw new TlsFatalAlert(50);
			}
			if (memoryStream.Position != memoryStream.Length)
			{
				throw new TlsFatalAlert(50);
			}
			return asn1Object;
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x0011C77A File Offset: 0x0011A97A
		public static Asn1Object ReadDerObject(byte[] encoding)
		{
			Asn1Object asn1Object = TlsUtilities.ReadAsn1Object(encoding);
			if (!Arrays.AreEqual(asn1Object.GetEncoded("DER"), encoding))
			{
				throw new TlsFatalAlert(50);
			}
			return asn1Object;
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x0011C7A0 File Offset: 0x0011A9A0
		public static void WriteGmtUnixTime(byte[] buf, int offset)
		{
			int num = (int)(DateTimeUtilities.CurrentUnixMs() / 1000L);
			buf[offset] = (byte)(num >> 24);
			buf[offset + 1] = (byte)(num >> 16);
			buf[offset + 2] = (byte)(num >> 8);
			buf[offset + 3] = (byte)num;
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x0011C7DD File Offset: 0x0011A9DD
		public static void WriteVersion(ProtocolVersion version, Stream output)
		{
			output.WriteByte((byte)version.MajorVersion);
			output.WriteByte((byte)version.MinorVersion);
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x0011C7F9 File Offset: 0x0011A9F9
		public static void WriteVersion(ProtocolVersion version, byte[] buf, int offset)
		{
			buf[offset] = (byte)version.MajorVersion;
			buf[offset + 1] = (byte)version.MinorVersion;
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x0011C811 File Offset: 0x0011AA11
		public static IList GetAllSignatureAlgorithms()
		{
			IList list = Platform.CreateArrayList(4);
			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(3);
			return list;
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x0011C84D File Offset: 0x0011AA4D
		public static IList GetDefaultDssSignatureAlgorithms()
		{
			return TlsUtilities.VectorOfOne(new SignatureAndHashAlgorithm(2, 2));
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x0011C85B File Offset: 0x0011AA5B
		public static IList GetDefaultECDsaSignatureAlgorithms()
		{
			return TlsUtilities.VectorOfOne(new SignatureAndHashAlgorithm(2, 3));
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x0011C869 File Offset: 0x0011AA69
		public static IList GetDefaultRsaSignatureAlgorithms()
		{
			return TlsUtilities.VectorOfOne(new SignatureAndHashAlgorithm(2, 1));
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x0011C877 File Offset: 0x0011AA77
		public static byte[] GetExtensionData(IDictionary extensions, int extensionType)
		{
			if (extensions != null)
			{
				return (byte[])extensions[extensionType];
			}
			return null;
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x0011C890 File Offset: 0x0011AA90
		public static IList GetDefaultSupportedSignatureAlgorithms()
		{
			byte[] array = new byte[]
			{
				2,
				3,
				4,
				5,
				6
			};
			byte[] array2 = new byte[]
			{
				1,
				2,
				3
			};
			IList list = Platform.CreateArrayList();
			for (int i = 0; i < array2.Length; i++)
			{
				for (int j = 0; j < array.Length; j++)
				{
					list.Add(new SignatureAndHashAlgorithm(array[j], array2[i]));
				}
			}
			return list;
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x0011C8FC File Offset: 0x0011AAFC
		public static SignatureAndHashAlgorithm GetSignatureAndHashAlgorithm(TlsContext context, TlsSignerCredentials signerCredentials)
		{
			SignatureAndHashAlgorithm signatureAndHashAlgorithm = null;
			if (TlsUtilities.IsTlsV12(context))
			{
				signatureAndHashAlgorithm = signerCredentials.SignatureAndHashAlgorithm;
				if (signatureAndHashAlgorithm == null)
				{
					throw new TlsFatalAlert(80);
				}
			}
			return signatureAndHashAlgorithm;
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x0011C928 File Offset: 0x0011AB28
		public static bool HasExpectedEmptyExtensionData(IDictionary extensions, int extensionType, byte alertDescription)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, extensionType);
			if (extensionData == null)
			{
				return false;
			}
			if (extensionData.Length != 0)
			{
				throw new TlsFatalAlert(alertDescription);
			}
			return true;
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x0011C94E File Offset: 0x0011AB4E
		public static TlsSession ImportSession(byte[] sessionID, SessionParameters sessionParameters)
		{
			return new TlsSessionImpl(sessionID, sessionParameters);
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x0011C01C File Offset: 0x0011A21C
		public static bool IsSignatureAlgorithmsExtensionAllowed(ProtocolVersion clientVersion)
		{
			return ProtocolVersion.TLSv12.IsEqualOrEarlierVersionOf(clientVersion.GetEquivalentTLSVersion());
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x0011C957 File Offset: 0x0011AB57
		public static void AddSignatureAlgorithmsExtension(IDictionary extensions, IList supportedSignatureAlgorithms)
		{
			extensions[13] = TlsUtilities.CreateSignatureAlgorithmsExtension(supportedSignatureAlgorithms);
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x0011C96C File Offset: 0x0011AB6C
		public static IList GetSignatureAlgorithmsExtension(IDictionary extensions)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(extensions, 13);
			if (extensionData != null)
			{
				return TlsUtilities.ReadSignatureAlgorithmsExtension(extensionData);
			}
			return null;
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x0011C990 File Offset: 0x0011AB90
		public static byte[] CreateSignatureAlgorithmsExtension(IList supportedSignatureAlgorithms)
		{
			MemoryStream memoryStream = new MemoryStream();
			TlsUtilities.EncodeSupportedSignatureAlgorithms(supportedSignatureAlgorithms, false, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x0011C9B4 File Offset: 0x0011ABB4
		public static IList ReadSignatureAlgorithmsExtension(byte[] extensionData)
		{
			if (extensionData == null)
			{
				throw new ArgumentNullException("extensionData");
			}
			MemoryStream memoryStream = new MemoryStream(extensionData, false);
			IList result = TlsUtilities.ParseSupportedSignatureAlgorithms(false, memoryStream);
			TlsProtocol.AssertEmpty(memoryStream);
			return result;
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x0011C9E4 File Offset: 0x0011ABE4
		public static void EncodeSupportedSignatureAlgorithms(IList supportedSignatureAlgorithms, bool allowAnonymous, Stream output)
		{
			if (supportedSignatureAlgorithms == null)
			{
				throw new ArgumentNullException("supportedSignatureAlgorithms");
			}
			if (supportedSignatureAlgorithms.Count < 1 || supportedSignatureAlgorithms.Count >= 32768)
			{
				throw new ArgumentException("must have length from 1 to (2^15 - 1)", "supportedSignatureAlgorithms");
			}
			int i = 2 * supportedSignatureAlgorithms.Count;
			TlsUtilities.CheckUint16(i);
			TlsUtilities.WriteUint16(i, output);
			foreach (object obj in supportedSignatureAlgorithms)
			{
				SignatureAndHashAlgorithm signatureAndHashAlgorithm = (SignatureAndHashAlgorithm)obj;
				if (!allowAnonymous && signatureAndHashAlgorithm.Signature == 0)
				{
					throw new ArgumentException("SignatureAlgorithm.anonymous MUST NOT appear in the signature_algorithms extension");
				}
				signatureAndHashAlgorithm.Encode(output);
			}
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x0011CA98 File Offset: 0x0011AC98
		public static IList ParseSupportedSignatureAlgorithms(bool allowAnonymous, Stream input)
		{
			int num = TlsUtilities.ReadUint16(input);
			if (num < 2 || (num & 1) != 0)
			{
				throw new TlsFatalAlert(50);
			}
			int num2 = num / 2;
			IList list = Platform.CreateArrayList(num2);
			for (int i = 0; i < num2; i++)
			{
				SignatureAndHashAlgorithm signatureAndHashAlgorithm = SignatureAndHashAlgorithm.Parse(input);
				if (!allowAnonymous && signatureAndHashAlgorithm.Signature == 0)
				{
					throw new TlsFatalAlert(47);
				}
				list.Add(signatureAndHashAlgorithm);
			}
			return list;
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x0011CAFC File Offset: 0x0011ACFC
		public static void VerifySupportedSignatureAlgorithm(IList supportedSignatureAlgorithms, SignatureAndHashAlgorithm signatureAlgorithm)
		{
			if (supportedSignatureAlgorithms == null)
			{
				throw new ArgumentNullException("supportedSignatureAlgorithms");
			}
			if (supportedSignatureAlgorithms.Count < 1 || supportedSignatureAlgorithms.Count >= 32768)
			{
				throw new ArgumentException("must have length from 1 to (2^15 - 1)", "supportedSignatureAlgorithms");
			}
			if (signatureAlgorithm == null)
			{
				throw new ArgumentNullException("signatureAlgorithm");
			}
			if (signatureAlgorithm.Signature != 0)
			{
				foreach (object obj in supportedSignatureAlgorithms)
				{
					SignatureAndHashAlgorithm signatureAndHashAlgorithm = (SignatureAndHashAlgorithm)obj;
					if (signatureAndHashAlgorithm.Hash == signatureAlgorithm.Hash && signatureAndHashAlgorithm.Signature == signatureAlgorithm.Signature)
					{
						return;
					}
				}
			}
			throw new TlsFatalAlert(47);
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x0011CBBC File Offset: 0x0011ADBC
		public static byte[] PRF(TlsContext context, byte[] secret, string asciiLabel, byte[] seed, int size)
		{
			if (context.ServerVersion.IsSsl)
			{
				throw new InvalidOperationException("No PRF available for SSLv3 session");
			}
			byte[] array = Strings.ToByteArray(asciiLabel);
			byte[] array2 = TlsUtilities.Concat(array, seed);
			int prfAlgorithm = context.SecurityParameters.PrfAlgorithm;
			if (prfAlgorithm == 0)
			{
				return TlsUtilities.PRF_legacy(secret, array, array2, size);
			}
			IDigest digest = TlsUtilities.CreatePrfHash(prfAlgorithm);
			byte[] array3 = new byte[size];
			TlsUtilities.HMacHash(digest, secret, array2, array3);
			return array3;
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x0011CC24 File Offset: 0x0011AE24
		public static byte[] PRF_legacy(byte[] secret, string asciiLabel, byte[] seed, int size)
		{
			byte[] array = Strings.ToByteArray(asciiLabel);
			byte[] labelSeed = TlsUtilities.Concat(array, seed);
			return TlsUtilities.PRF_legacy(secret, array, labelSeed, size);
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x0011CC4C File Offset: 0x0011AE4C
		internal static byte[] PRF_legacy(byte[] secret, byte[] label, byte[] labelSeed, int size)
		{
			int num = (secret.Length + 1) / 2;
			byte[] array = new byte[num];
			byte[] array2 = new byte[num];
			Array.Copy(secret, 0, array, 0, num);
			Array.Copy(secret, secret.Length - num, array2, 0, num);
			byte[] array3 = new byte[size];
			byte[] array4 = new byte[size];
			TlsUtilities.HMacHash(TlsUtilities.CreateHash(1), array, labelSeed, array3);
			TlsUtilities.HMacHash(TlsUtilities.CreateHash(2), array2, labelSeed, array4);
			for (int i = 0; i < size; i++)
			{
				byte[] array5 = array3;
				int num2 = i;
				array5[num2] ^= array4[i];
			}
			return array3;
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x0011CCD8 File Offset: 0x0011AED8
		internal static byte[] Concat(byte[] a, byte[] b)
		{
			byte[] array = new byte[a.Length + b.Length];
			Array.Copy(a, 0, array, 0, a.Length);
			Array.Copy(b, 0, array, a.Length, b.Length);
			return array;
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x0011CD10 File Offset: 0x0011AF10
		internal static void HMacHash(IDigest digest, byte[] secret, byte[] seed, byte[] output)
		{
			HMac hmac = new HMac(digest);
			hmac.Init(new KeyParameter(secret));
			byte[] array = seed;
			int digestSize = digest.GetDigestSize();
			int num = (output.Length + digestSize - 1) / digestSize;
			byte[] array2 = new byte[hmac.GetMacSize()];
			byte[] array3 = new byte[hmac.GetMacSize()];
			for (int i = 0; i < num; i++)
			{
				hmac.BlockUpdate(array, 0, array.Length);
				hmac.DoFinal(array2, 0);
				array = array2;
				hmac.BlockUpdate(array, 0, array.Length);
				hmac.BlockUpdate(seed, 0, seed.Length);
				hmac.DoFinal(array3, 0);
				Array.Copy(array3, 0, output, digestSize * i, Math.Min(digestSize, output.Length - digestSize * i));
			}
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x0011CDC0 File Offset: 0x0011AFC0
		internal static void ValidateKeyUsage(X509CertificateStructure c, int keyUsageBits)
		{
			X509Extensions extensions = c.TbsCertificate.Extensions;
			if (extensions != null)
			{
				X509Extension extension = extensions.GetExtension(X509Extensions.KeyUsage);
				if (extension != null && ((int)KeyUsage.GetInstance(extension).GetBytes()[0] & keyUsageBits) != keyUsageBits)
				{
					throw new TlsFatalAlert(46);
				}
			}
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x0011CE08 File Offset: 0x0011B008
		internal static byte[] CalculateKeyBlock(TlsContext context, int size)
		{
			SecurityParameters securityParameters = context.SecurityParameters;
			byte[] masterSecret = securityParameters.MasterSecret;
			byte[] array = TlsUtilities.Concat(securityParameters.ServerRandom, securityParameters.ClientRandom);
			if (TlsUtilities.IsSsl(context))
			{
				return TlsUtilities.CalculateKeyBlock_Ssl(masterSecret, array, size);
			}
			return TlsUtilities.PRF(context, masterSecret, "key expansion", array, size);
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x0011CE54 File Offset: 0x0011B054
		internal static byte[] CalculateKeyBlock_Ssl(byte[] master_secret, byte[] random, int size)
		{
			IDigest digest = TlsUtilities.CreateHash(1);
			IDigest digest2 = TlsUtilities.CreateHash(2);
			int digestSize = digest.GetDigestSize();
			byte[] array = new byte[digest2.GetDigestSize()];
			byte[] array2 = new byte[size + digestSize];
			int num = 0;
			int i = 0;
			while (i < size)
			{
				byte[] array3 = TlsUtilities.SSL3_CONST[num];
				digest2.BlockUpdate(array3, 0, array3.Length);
				digest2.BlockUpdate(master_secret, 0, master_secret.Length);
				digest2.BlockUpdate(random, 0, random.Length);
				digest2.DoFinal(array, 0);
				digest.BlockUpdate(master_secret, 0, master_secret.Length);
				digest.BlockUpdate(array, 0, array.Length);
				digest.DoFinal(array2, i);
				i += digestSize;
				num++;
			}
			return Arrays.CopyOfRange(array2, 0, size);
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x0011CF08 File Offset: 0x0011B108
		internal static byte[] CalculateMasterSecret(TlsContext context, byte[] pre_master_secret)
		{
			SecurityParameters securityParameters = context.SecurityParameters;
			byte[] array = securityParameters.IsExtendedMasterSecret ? securityParameters.SessionHash : TlsUtilities.Concat(securityParameters.ClientRandom, securityParameters.ServerRandom);
			if (TlsUtilities.IsSsl(context))
			{
				return TlsUtilities.CalculateMasterSecret_Ssl(pre_master_secret, array);
			}
			string asciiLabel = securityParameters.IsExtendedMasterSecret ? ExporterLabel.extended_master_secret : "master secret";
			return TlsUtilities.PRF(context, pre_master_secret, asciiLabel, array, 48);
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x0011CF70 File Offset: 0x0011B170
		internal static byte[] CalculateMasterSecret_Ssl(byte[] pre_master_secret, byte[] random)
		{
			IDigest digest = TlsUtilities.CreateHash(1);
			IDigest digest2 = TlsUtilities.CreateHash(2);
			int digestSize = digest.GetDigestSize();
			byte[] array = new byte[digest2.GetDigestSize()];
			byte[] array2 = new byte[digestSize * 3];
			int num = 0;
			for (int i = 0; i < 3; i++)
			{
				byte[] array3 = TlsUtilities.SSL3_CONST[i];
				digest2.BlockUpdate(array3, 0, array3.Length);
				digest2.BlockUpdate(pre_master_secret, 0, pre_master_secret.Length);
				digest2.BlockUpdate(random, 0, random.Length);
				digest2.DoFinal(array, 0);
				digest.BlockUpdate(pre_master_secret, 0, pre_master_secret.Length);
				digest.BlockUpdate(array, 0, array.Length);
				digest.DoFinal(array2, num);
				num += digestSize;
			}
			return array2;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x0011D01C File Offset: 0x0011B21C
		internal static byte[] CalculateVerifyData(TlsContext context, string asciiLabel, byte[] handshakeHash)
		{
			if (TlsUtilities.IsSsl(context))
			{
				return handshakeHash;
			}
			SecurityParameters securityParameters = context.SecurityParameters;
			byte[] masterSecret = securityParameters.MasterSecret;
			int verifyDataLength = securityParameters.VerifyDataLength;
			return TlsUtilities.PRF(context, masterSecret, asciiLabel, handshakeHash, verifyDataLength);
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x0011D050 File Offset: 0x0011B250
		public static IDigest CreateHash(byte hashAlgorithm)
		{
			switch (hashAlgorithm)
			{
			case 1:
				return new MD5Digest();
			case 2:
				return new Sha1Digest();
			case 3:
				return new Sha224Digest();
			case 4:
				return new Sha256Digest();
			case 5:
				return new Sha384Digest();
			case 6:
				return new Sha512Digest();
			default:
				throw new ArgumentException("unknown HashAlgorithm", "hashAlgorithm");
			}
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x0011D0B4 File Offset: 0x0011B2B4
		public static IDigest CreateHash(SignatureAndHashAlgorithm signatureAndHashAlgorithm)
		{
			if (signatureAndHashAlgorithm != null)
			{
				return TlsUtilities.CreateHash(signatureAndHashAlgorithm.Hash);
			}
			return new CombinedHash();
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x0011D0D8 File Offset: 0x0011B2D8
		public static IDigest CloneHash(byte hashAlgorithm, IDigest hash)
		{
			switch (hashAlgorithm)
			{
			case 1:
				return new MD5Digest((MD5Digest)hash);
			case 2:
				return new Sha1Digest((Sha1Digest)hash);
			case 3:
				return new Sha224Digest((Sha224Digest)hash);
			case 4:
				return new Sha256Digest((Sha256Digest)hash);
			case 5:
				return new Sha384Digest((Sha384Digest)hash);
			case 6:
				return new Sha512Digest((Sha512Digest)hash);
			default:
				throw new ArgumentException("unknown HashAlgorithm", "hashAlgorithm");
			}
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x0011D15E File Offset: 0x0011B35E
		public static IDigest CreatePrfHash(int prfAlgorithm)
		{
			if (prfAlgorithm == 0)
			{
				return new CombinedHash();
			}
			return TlsUtilities.CreateHash(TlsUtilities.GetHashAlgorithmForPrfAlgorithm(prfAlgorithm));
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x0011D174 File Offset: 0x0011B374
		public static IDigest ClonePrfHash(int prfAlgorithm, IDigest hash)
		{
			if (prfAlgorithm == 0)
			{
				return new CombinedHash((CombinedHash)hash);
			}
			return TlsUtilities.CloneHash(TlsUtilities.GetHashAlgorithmForPrfAlgorithm(prfAlgorithm), hash);
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x0011D191 File Offset: 0x0011B391
		public static byte GetHashAlgorithmForPrfAlgorithm(int prfAlgorithm)
		{
			switch (prfAlgorithm)
			{
			case 0:
				throw new ArgumentException("legacy PRF not a valid algorithm", "prfAlgorithm");
			case 1:
				return 4;
			case 2:
				return 5;
			default:
				throw new ArgumentException("unknown PrfAlgorithm", "prfAlgorithm");
			}
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x0011D1CC File Offset: 0x0011B3CC
		public static DerObjectIdentifier GetOidForHashAlgorithm(byte hashAlgorithm)
		{
			switch (hashAlgorithm)
			{
			case 1:
				return PkcsObjectIdentifiers.MD5;
			case 2:
				return X509ObjectIdentifiers.IdSha1;
			case 3:
				return NistObjectIdentifiers.IdSha224;
			case 4:
				return NistObjectIdentifiers.IdSha256;
			case 5:
				return NistObjectIdentifiers.IdSha384;
			case 6:
				return NistObjectIdentifiers.IdSha512;
			default:
				throw new ArgumentException("unknown HashAlgorithm", "hashAlgorithm");
			}
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x0011D230 File Offset: 0x0011B430
		internal static short GetClientCertificateType(Certificate clientCertificate, Certificate serverCertificate)
		{
			if (clientCertificate.IsEmpty)
			{
				return -1;
			}
			X509CertificateStructure certificateAt = clientCertificate.GetCertificateAt(0);
			SubjectPublicKeyInfo subjectPublicKeyInfo = certificateAt.SubjectPublicKeyInfo;
			short result;
			try
			{
				AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(subjectPublicKeyInfo);
				if (asymmetricKeyParameter.IsPrivate)
				{
					throw new TlsFatalAlert(80);
				}
				if (asymmetricKeyParameter is RsaKeyParameters)
				{
					TlsUtilities.ValidateKeyUsage(certificateAt, 128);
					result = 1;
				}
				else if (asymmetricKeyParameter is DsaPublicKeyParameters)
				{
					TlsUtilities.ValidateKeyUsage(certificateAt, 128);
					result = 2;
				}
				else
				{
					if (!(asymmetricKeyParameter is ECPublicKeyParameters))
					{
						throw new TlsFatalAlert(43);
					}
					TlsUtilities.ValidateKeyUsage(certificateAt, 128);
					result = 64;
				}
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(43, alertCause);
			}
			return result;
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x0011D2D8 File Offset: 0x0011B4D8
		internal static void TrackHashAlgorithms(TlsHandshakeHash handshakeHash, IList supportedSignatureAlgorithms)
		{
			if (supportedSignatureAlgorithms != null)
			{
				foreach (object obj in supportedSignatureAlgorithms)
				{
					byte hash = ((SignatureAndHashAlgorithm)obj).Hash;
					if (HashAlgorithm.IsRecognized(hash))
					{
						handshakeHash.TrackHashAlgorithm(hash);
					}
				}
			}
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x0011D33C File Offset: 0x0011B53C
		public static bool HasSigningCapability(byte clientCertificateType)
		{
			return clientCertificateType - 1 <= 1 || clientCertificateType == 64;
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x0011D34C File Offset: 0x0011B54C
		public static TlsSigner CreateTlsSigner(byte clientCertificateType)
		{
			if (clientCertificateType == 1)
			{
				return new TlsRsaSigner();
			}
			if (clientCertificateType == 2)
			{
				return new TlsDssSigner();
			}
			if (clientCertificateType != 64)
			{
				throw new ArgumentException("not a type with signing capability", "clientCertificateType");
			}
			return new TlsECDsaSigner();
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x0011D380 File Offset: 0x0011B580
		private static byte[][] GenSsl3Const()
		{
			int num = 10;
			byte[][] array = new byte[num][];
			for (int i = 0; i < num; i++)
			{
				byte[] array2 = new byte[i + 1];
				Arrays.Fill(array2, (byte)(65 + i));
				array[i] = array2;
			}
			return array;
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x0011D3BC File Offset: 0x0011B5BC
		private static IList VectorOfOne(object obj)
		{
			IList list = Platform.CreateArrayList(1);
			list.Add(obj);
			return list;
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x0011D3CC File Offset: 0x0011B5CC
		public static int GetCipherType(int ciphersuite)
		{
			int encryptionAlgorithm = TlsUtilities.GetEncryptionAlgorithm(ciphersuite);
			switch (encryptionAlgorithm)
			{
			case 0:
			case 1:
			case 2:
				return 0;
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 12:
			case 13:
			case 14:
				return 1;
			case 10:
			case 11:
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
				break;
			default:
				if (encryptionAlgorithm - 103 > 1)
				{
					throw new TlsFatalAlert(80);
				}
				break;
			}
			return 2;
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x0011D454 File Offset: 0x0011B654
		public static int GetEncryptionAlgorithm(int ciphersuite)
		{
			if (ciphersuite <= 49327)
			{
				switch (ciphersuite)
				{
				case 1:
					return 0;
				case 2:
				case 44:
				case 45:
				case 46:
					return 0;
				case 3:
				case 6:
				case 7:
				case 8:
				case 9:
				case 11:
				case 12:
				case 14:
				case 15:
				case 17:
				case 18:
				case 20:
				case 21:
				case 23:
				case 25:
				case 26:
				case 28:
				case 29:
				case 30:
				case 31:
				case 32:
				case 33:
				case 34:
				case 35:
				case 36:
				case 37:
				case 38:
				case 39:
				case 40:
				case 41:
				case 42:
				case 43:
				case 71:
				case 72:
				case 73:
				case 74:
				case 75:
				case 76:
				case 77:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 83:
				case 84:
				case 85:
				case 86:
				case 87:
				case 88:
				case 89:
				case 90:
				case 91:
				case 92:
				case 93:
				case 94:
				case 95:
				case 96:
				case 97:
				case 98:
				case 99:
				case 100:
				case 101:
				case 102:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
				case 122:
				case 123:
				case 124:
				case 125:
				case 126:
				case 127:
				case 128:
				case 129:
				case 130:
				case 131:
					goto IL_6A4;
				case 4:
				case 24:
					return 2;
				case 5:
				case 138:
				case 142:
				case 146:
					return 2;
				case 10:
				case 13:
				case 16:
				case 19:
				case 22:
				case 27:
				case 139:
				case 143:
				case 147:
					break;
				case 47:
				case 48:
				case 49:
				case 50:
				case 51:
				case 52:
				case 60:
				case 62:
				case 63:
				case 64:
				case 103:
				case 108:
				case 140:
				case 144:
				case 148:
				case 174:
				case 178:
				case 182:
					return 8;
				case 53:
				case 54:
				case 55:
				case 56:
				case 57:
				case 58:
				case 61:
				case 104:
				case 105:
				case 106:
				case 107:
				case 109:
				case 141:
				case 145:
				case 149:
				case 175:
				case 179:
				case 183:
					return 9;
				case 59:
				case 176:
				case 180:
				case 184:
					return 0;
				case 65:
				case 66:
				case 67:
				case 68:
				case 69:
				case 70:
				case 186:
				case 187:
				case 188:
				case 189:
				case 190:
				case 191:
					return 12;
				case 132:
				case 133:
				case 134:
				case 135:
				case 136:
				case 137:
				case 192:
				case 193:
				case 194:
				case 195:
				case 196:
				case 197:
					return 13;
				case 150:
				case 151:
				case 152:
				case 153:
				case 154:
				case 155:
					return 14;
				case 156:
				case 158:
				case 160:
				case 162:
				case 164:
				case 166:
				case 168:
				case 170:
				case 172:
					return 10;
				case 157:
				case 159:
				case 161:
				case 163:
				case 165:
				case 167:
				case 169:
				case 171:
				case 173:
					return 11;
				case 177:
				case 181:
				case 185:
					return 0;
				default:
					switch (ciphersuite)
					{
					case 49153:
					case 49158:
					case 49163:
					case 49168:
					case 49173:
					case 49209:
						return 0;
					case 49154:
					case 49159:
					case 49164:
					case 49169:
					case 49174:
					case 49203:
						return 2;
					case 49155:
					case 49160:
					case 49165:
					case 49170:
					case 49175:
					case 49178:
					case 49179:
					case 49180:
					case 49204:
						break;
					case 49156:
					case 49161:
					case 49166:
					case 49171:
					case 49176:
					case 49181:
					case 49182:
					case 49183:
					case 49187:
					case 49189:
					case 49191:
					case 49193:
					case 49205:
					case 49207:
						return 8;
					case 49157:
					case 49162:
					case 49167:
					case 49172:
					case 49177:
					case 49184:
					case 49185:
					case 49186:
					case 49188:
					case 49190:
					case 49192:
					case 49194:
					case 49206:
					case 49208:
						return 9;
					case 49195:
					case 49197:
					case 49199:
					case 49201:
						return 10;
					case 49196:
					case 49198:
					case 49200:
					case 49202:
						return 11;
					case 49210:
						return 0;
					case 49211:
						return 0;
					case 49212:
					case 49213:
					case 49214:
					case 49215:
					case 49216:
					case 49217:
					case 49218:
					case 49219:
					case 49220:
					case 49221:
					case 49222:
					case 49223:
					case 49224:
					case 49225:
					case 49226:
					case 49227:
					case 49228:
					case 49229:
					case 49230:
					case 49231:
					case 49232:
					case 49233:
					case 49234:
					case 49235:
					case 49236:
					case 49237:
					case 49238:
					case 49239:
					case 49240:
					case 49241:
					case 49242:
					case 49243:
					case 49244:
					case 49245:
					case 49246:
					case 49247:
					case 49248:
					case 49249:
					case 49250:
					case 49251:
					case 49252:
					case 49253:
					case 49254:
					case 49255:
					case 49256:
					case 49257:
					case 49258:
					case 49259:
					case 49260:
					case 49261:
					case 49262:
					case 49263:
					case 49264:
					case 49265:
						goto IL_6A4;
					case 49266:
					case 49268:
					case 49270:
					case 49272:
					case 49300:
					case 49302:
					case 49304:
					case 49306:
						return 12;
					case 49267:
					case 49269:
					case 49271:
					case 49273:
					case 49301:
					case 49303:
					case 49305:
					case 49307:
						return 13;
					case 49274:
					case 49276:
					case 49278:
					case 49280:
					case 49282:
					case 49284:
					case 49286:
					case 49288:
					case 49290:
					case 49292:
					case 49294:
					case 49296:
					case 49298:
						return 19;
					case 49275:
					case 49277:
					case 49279:
					case 49281:
					case 49283:
					case 49285:
					case 49287:
					case 49289:
					case 49291:
					case 49293:
					case 49295:
					case 49297:
					case 49299:
						return 20;
					case 49308:
					case 49310:
					case 49316:
					case 49318:
					case 49324:
						return 15;
					case 49309:
					case 49311:
					case 49317:
					case 49319:
					case 49325:
						return 17;
					case 49312:
					case 49314:
					case 49320:
					case 49322:
					case 49326:
						return 16;
					case 49313:
					case 49315:
					case 49321:
					case 49323:
					case 49327:
						return 18;
					default:
						goto IL_6A4;
					}
					break;
				}
				return 7;
			}
			if (ciphersuite - 52392 <= 6)
			{
				return 21;
			}
			switch (ciphersuite)
			{
			case 65280:
			case 65282:
			case 65284:
			case 65296:
			case 65298:
			case 65300:
				return 103;
			case 65281:
			case 65283:
			case 65285:
			case 65297:
			case 65299:
			case 65301:
				return 104;
			}
			IL_6A4:
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x0011DB0C File Offset: 0x0011BD0C
		public static int GetKeyExchangeAlgorithm(int ciphersuite)
		{
			if (ciphersuite <= 49327)
			{
				switch (ciphersuite)
				{
				case 1:
				case 2:
				case 4:
				case 5:
				case 10:
				case 47:
				case 53:
				case 59:
				case 60:
				case 61:
				case 65:
				case 132:
				case 150:
				case 156:
				case 157:
				case 186:
				case 192:
					return 1;
				case 3:
				case 6:
				case 7:
				case 8:
				case 9:
				case 11:
				case 12:
				case 14:
				case 15:
				case 17:
				case 18:
				case 20:
				case 21:
				case 23:
				case 25:
				case 26:
				case 28:
				case 29:
				case 30:
				case 31:
				case 32:
				case 33:
				case 34:
				case 35:
				case 36:
				case 37:
				case 38:
				case 39:
				case 40:
				case 41:
				case 42:
				case 43:
				case 71:
				case 72:
				case 73:
				case 74:
				case 75:
				case 76:
				case 77:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 83:
				case 84:
				case 85:
				case 86:
				case 87:
				case 88:
				case 89:
				case 90:
				case 91:
				case 92:
				case 93:
				case 94:
				case 95:
				case 96:
				case 97:
				case 98:
				case 99:
				case 100:
				case 101:
				case 102:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
				case 122:
				case 123:
				case 124:
				case 125:
				case 126:
				case 127:
				case 128:
				case 129:
				case 130:
				case 131:
					goto IL_6B4;
				case 13:
				case 48:
				case 54:
				case 62:
				case 66:
				case 104:
				case 133:
				case 151:
				case 164:
				case 165:
				case 187:
				case 193:
					return 7;
				case 16:
				case 49:
				case 55:
				case 63:
				case 67:
				case 105:
				case 134:
				case 152:
				case 160:
				case 161:
				case 188:
				case 194:
					return 9;
				case 19:
				case 50:
				case 56:
				case 64:
				case 68:
				case 106:
				case 135:
				case 153:
				case 162:
				case 163:
				case 189:
				case 195:
					return 3;
				case 22:
				case 51:
				case 57:
				case 69:
				case 103:
				case 107:
				case 136:
				case 154:
				case 158:
				case 159:
				case 190:
				case 196:
					return 5;
				case 24:
				case 27:
				case 52:
				case 58:
				case 70:
				case 108:
				case 109:
				case 137:
				case 155:
				case 166:
				case 167:
				case 191:
				case 197:
					break;
				case 44:
				case 138:
				case 139:
				case 140:
				case 141:
				case 168:
				case 169:
				case 174:
				case 175:
				case 176:
				case 177:
					return 13;
				case 45:
				case 142:
				case 143:
				case 144:
				case 145:
				case 170:
				case 171:
				case 178:
				case 179:
				case 180:
				case 181:
					return 14;
				case 46:
				case 146:
				case 147:
				case 148:
				case 149:
				case 172:
				case 173:
				case 182:
				case 183:
				case 184:
				case 185:
					return 15;
				default:
					switch (ciphersuite)
					{
					case 49153:
					case 49154:
					case 49155:
					case 49156:
					case 49157:
					case 49189:
					case 49190:
					case 49197:
					case 49198:
					case 49268:
					case 49269:
					case 49288:
					case 49289:
						return 16;
					case 49158:
					case 49159:
					case 49160:
					case 49161:
					case 49162:
					case 49187:
					case 49188:
					case 49195:
					case 49196:
					case 49266:
					case 49267:
					case 49286:
					case 49287:
					case 49324:
					case 49325:
					case 49326:
					case 49327:
						return 17;
					case 49163:
					case 49164:
					case 49165:
					case 49166:
					case 49167:
					case 49193:
					case 49194:
					case 49201:
					case 49202:
					case 49272:
					case 49273:
					case 49292:
					case 49293:
						return 18;
					case 49168:
					case 49169:
					case 49170:
					case 49171:
					case 49172:
					case 49191:
					case 49192:
					case 49199:
					case 49200:
					case 49270:
					case 49271:
					case 49290:
					case 49291:
						return 19;
					case 49173:
					case 49174:
					case 49175:
					case 49176:
					case 49177:
						return 20;
					case 49178:
					case 49181:
					case 49184:
						return 21;
					case 49179:
					case 49182:
					case 49185:
						return 23;
					case 49180:
					case 49183:
					case 49186:
						return 22;
					case 49203:
					case 49204:
					case 49205:
					case 49206:
					case 49207:
					case 49208:
					case 49209:
					case 49210:
					case 49211:
					case 49306:
					case 49307:
						return 24;
					case 49212:
					case 49213:
					case 49214:
					case 49215:
					case 49216:
					case 49217:
					case 49218:
					case 49219:
					case 49220:
					case 49221:
					case 49222:
					case 49223:
					case 49224:
					case 49225:
					case 49226:
					case 49227:
					case 49228:
					case 49229:
					case 49230:
					case 49231:
					case 49232:
					case 49233:
					case 49234:
					case 49235:
					case 49236:
					case 49237:
					case 49238:
					case 49239:
					case 49240:
					case 49241:
					case 49242:
					case 49243:
					case 49244:
					case 49245:
					case 49246:
					case 49247:
					case 49248:
					case 49249:
					case 49250:
					case 49251:
					case 49252:
					case 49253:
					case 49254:
					case 49255:
					case 49256:
					case 49257:
					case 49258:
					case 49259:
					case 49260:
					case 49261:
					case 49262:
					case 49263:
					case 49264:
					case 49265:
						goto IL_6B4;
					case 49274:
					case 49275:
					case 49308:
					case 49309:
					case 49312:
					case 49313:
						return 1;
					case 49276:
					case 49277:
					case 49310:
					case 49311:
					case 49314:
					case 49315:
						return 5;
					case 49278:
					case 49279:
						return 9;
					case 49280:
					case 49281:
						return 3;
					case 49282:
					case 49283:
						return 7;
					case 49284:
					case 49285:
						break;
					case 49294:
					case 49295:
					case 49300:
					case 49301:
					case 49316:
					case 49317:
					case 49320:
					case 49321:
						return 13;
					case 49296:
					case 49297:
					case 49302:
					case 49303:
					case 49318:
					case 49319:
					case 49322:
					case 49323:
						return 14;
					case 49298:
					case 49299:
					case 49304:
					case 49305:
						return 15;
					default:
						goto IL_6B4;
					}
					break;
				}
				return 11;
			}
			switch (ciphersuite)
			{
			case 52392:
				return 19;
			case 52393:
				return 17;
			case 52394:
				return 5;
			case 52395:
				return 13;
			case 52396:
				return 24;
			case 52397:
				break;
			case 52398:
				return 1;
			default:
				switch (ciphersuite)
				{
				case 65280:
				case 65281:
					return 5;
				case 65282:
				case 65283:
					return 19;
				case 65284:
				case 65285:
					return 16;
				case 65286:
				case 65287:
				case 65288:
				case 65289:
				case 65290:
				case 65291:
				case 65292:
				case 65293:
				case 65294:
				case 65295:
					goto IL_6B4;
				case 65296:
				case 65297:
					return 13;
				case 65298:
				case 65299:
					break;
				case 65300:
				case 65301:
					return 24;
				default:
					goto IL_6B4;
				}
				break;
			}
			return 14;
			IL_6B4:
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x0011E1D4 File Offset: 0x0011C3D4
		public static int GetMacAlgorithm(int ciphersuite)
		{
			if (ciphersuite <= 49327)
			{
				switch (ciphersuite)
				{
				case 1:
				case 4:
				case 24:
					return 1;
				case 2:
				case 5:
				case 10:
				case 13:
				case 16:
				case 19:
				case 22:
				case 27:
				case 44:
				case 45:
				case 46:
				case 47:
				case 48:
				case 49:
				case 50:
				case 51:
				case 52:
				case 53:
				case 54:
				case 55:
				case 56:
				case 57:
				case 58:
				case 65:
				case 66:
				case 67:
				case 68:
				case 69:
				case 70:
				case 132:
				case 133:
				case 134:
				case 135:
				case 136:
				case 137:
				case 138:
				case 139:
				case 140:
				case 141:
				case 142:
				case 143:
				case 144:
				case 145:
				case 146:
				case 147:
				case 148:
				case 149:
				case 150:
				case 151:
				case 152:
				case 153:
				case 154:
				case 155:
					break;
				case 3:
				case 6:
				case 7:
				case 8:
				case 9:
				case 11:
				case 12:
				case 14:
				case 15:
				case 17:
				case 18:
				case 20:
				case 21:
				case 23:
				case 25:
				case 26:
				case 28:
				case 29:
				case 30:
				case 31:
				case 32:
				case 33:
				case 34:
				case 35:
				case 36:
				case 37:
				case 38:
				case 39:
				case 40:
				case 41:
				case 42:
				case 43:
				case 71:
				case 72:
				case 73:
				case 74:
				case 75:
				case 76:
				case 77:
				case 78:
				case 79:
				case 80:
				case 81:
				case 82:
				case 83:
				case 84:
				case 85:
				case 86:
				case 87:
				case 88:
				case 89:
				case 90:
				case 91:
				case 92:
				case 93:
				case 94:
				case 95:
				case 96:
				case 97:
				case 98:
				case 99:
				case 100:
				case 101:
				case 102:
				case 110:
				case 111:
				case 112:
				case 113:
				case 114:
				case 115:
				case 116:
				case 117:
				case 118:
				case 119:
				case 120:
				case 121:
				case 122:
				case 123:
				case 124:
				case 125:
				case 126:
				case 127:
				case 128:
				case 129:
				case 130:
				case 131:
					goto IL_619;
				case 59:
				case 60:
				case 61:
				case 62:
				case 63:
				case 64:
				case 103:
				case 104:
				case 105:
				case 106:
				case 107:
				case 108:
				case 109:
				case 174:
				case 176:
				case 178:
				case 180:
				case 182:
				case 184:
				case 186:
				case 187:
				case 188:
				case 189:
				case 190:
				case 191:
				case 192:
				case 193:
				case 194:
				case 195:
				case 196:
				case 197:
					return 3;
				case 156:
				case 157:
				case 158:
				case 159:
				case 160:
				case 161:
				case 162:
				case 163:
				case 164:
				case 165:
				case 166:
				case 167:
				case 168:
				case 169:
				case 170:
				case 171:
				case 172:
				case 173:
					return 0;
				case 175:
				case 177:
				case 179:
				case 181:
				case 183:
				case 185:
					return 4;
				default:
					switch (ciphersuite)
					{
					case 49153:
					case 49154:
					case 49155:
					case 49156:
					case 49157:
					case 49158:
					case 49159:
					case 49160:
					case 49161:
					case 49162:
					case 49163:
					case 49164:
					case 49165:
					case 49166:
					case 49167:
					case 49168:
					case 49169:
					case 49170:
					case 49171:
					case 49172:
					case 49173:
					case 49174:
					case 49175:
					case 49176:
					case 49177:
					case 49178:
					case 49179:
					case 49180:
					case 49181:
					case 49182:
					case 49183:
					case 49184:
					case 49185:
					case 49186:
					case 49203:
					case 49204:
					case 49205:
					case 49206:
					case 49209:
						break;
					case 49187:
					case 49189:
					case 49191:
					case 49193:
					case 49207:
					case 49210:
					case 49266:
					case 49268:
					case 49270:
					case 49272:
					case 49300:
					case 49302:
					case 49304:
					case 49306:
						return 3;
					case 49188:
					case 49190:
					case 49192:
					case 49194:
					case 49208:
					case 49211:
					case 49267:
					case 49269:
					case 49271:
					case 49273:
					case 49301:
					case 49303:
					case 49305:
					case 49307:
						return 4;
					case 49195:
					case 49196:
					case 49197:
					case 49198:
					case 49199:
					case 49200:
					case 49201:
					case 49202:
					case 49274:
					case 49275:
					case 49276:
					case 49277:
					case 49278:
					case 49279:
					case 49280:
					case 49281:
					case 49282:
					case 49283:
					case 49284:
					case 49285:
					case 49286:
					case 49287:
					case 49288:
					case 49289:
					case 49290:
					case 49291:
					case 49292:
					case 49293:
					case 49294:
					case 49295:
					case 49296:
					case 49297:
					case 49298:
					case 49299:
					case 49308:
					case 49309:
					case 49310:
					case 49311:
					case 49312:
					case 49313:
					case 49314:
					case 49315:
					case 49316:
					case 49317:
					case 49318:
					case 49319:
					case 49320:
					case 49321:
					case 49322:
					case 49323:
					case 49324:
					case 49325:
					case 49326:
					case 49327:
						return 0;
					case 49212:
					case 49213:
					case 49214:
					case 49215:
					case 49216:
					case 49217:
					case 49218:
					case 49219:
					case 49220:
					case 49221:
					case 49222:
					case 49223:
					case 49224:
					case 49225:
					case 49226:
					case 49227:
					case 49228:
					case 49229:
					case 49230:
					case 49231:
					case 49232:
					case 49233:
					case 49234:
					case 49235:
					case 49236:
					case 49237:
					case 49238:
					case 49239:
					case 49240:
					case 49241:
					case 49242:
					case 49243:
					case 49244:
					case 49245:
					case 49246:
					case 49247:
					case 49248:
					case 49249:
					case 49250:
					case 49251:
					case 49252:
					case 49253:
					case 49254:
					case 49255:
					case 49256:
					case 49257:
					case 49258:
					case 49259:
					case 49260:
					case 49261:
					case 49262:
					case 49263:
					case 49264:
					case 49265:
						goto IL_619;
					default:
						goto IL_619;
					}
					break;
				}
				return 2;
			}
			if (ciphersuite - 52392 > 6 && ciphersuite - 65280 > 5 && ciphersuite - 65296 > 5)
			{
				goto IL_619;
			}
			return 0;
			IL_619:
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x0011E804 File Offset: 0x0011CA04
		public static ProtocolVersion GetMinimumVersion(int ciphersuite)
		{
			if (ciphersuite <= 49202)
			{
				if (ciphersuite <= 109)
				{
					if (ciphersuite - 59 > 5 && ciphersuite - 103 > 6)
					{
						goto IL_84;
					}
				}
				else if (ciphersuite - 156 > 17 && ciphersuite - 186 > 11 && ciphersuite - 49187 > 15)
				{
					goto IL_84;
				}
			}
			else if (ciphersuite <= 49327)
			{
				if (ciphersuite - 49266 > 33 && ciphersuite - 49308 > 19)
				{
					goto IL_84;
				}
			}
			else if (ciphersuite - 52392 > 6 && ciphersuite - 65280 > 5 && ciphersuite - 65296 > 5)
			{
				goto IL_84;
			}
			return ProtocolVersion.TLSv12;
			IL_84:
			return ProtocolVersion.SSLv3;
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x0011E89A File Offset: 0x0011CA9A
		public static bool IsAeadCipherSuite(int ciphersuite)
		{
			return 2 == TlsUtilities.GetCipherType(ciphersuite);
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x0011E8A5 File Offset: 0x0011CAA5
		public static bool IsBlockCipherSuite(int ciphersuite)
		{
			return 1 == TlsUtilities.GetCipherType(ciphersuite);
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x0011E8B0 File Offset: 0x0011CAB0
		public static bool IsStreamCipherSuite(int ciphersuite)
		{
			return TlsUtilities.GetCipherType(ciphersuite) == 0;
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x0011E8BC File Offset: 0x0011CABC
		public static bool IsValidCipherSuiteForSignatureAlgorithms(int cipherSuite, IList sigAlgs)
		{
			int keyExchangeAlgorithm;
			try
			{
				keyExchangeAlgorithm = TlsUtilities.GetKeyExchangeAlgorithm(cipherSuite);
			}
			catch (IOException)
			{
				return true;
			}
			switch (keyExchangeAlgorithm)
			{
			case 3:
			case 4:
			case 22:
				return sigAlgs.Contains(2);
			case 5:
			case 6:
			case 19:
			case 23:
				return sigAlgs.Contains(1);
			case 11:
			case 12:
			case 20:
				return sigAlgs.Contains(0);
			case 17:
				return sigAlgs.Contains(3);
			}
			return true;
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x0011E980 File Offset: 0x0011CB80
		public static bool IsValidCipherSuiteForVersion(int cipherSuite, ProtocolVersion serverVersion)
		{
			return TlsUtilities.GetMinimumVersion(cipherSuite).IsEqualOrEarlierVersionOf(serverVersion.GetEquivalentTLSVersion());
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x0011E994 File Offset: 0x0011CB94
		public static IList GetUsableSignatureAlgorithms(IList sigHashAlgs)
		{
			if (sigHashAlgs == null)
			{
				return TlsUtilities.GetAllSignatureAlgorithms();
			}
			IList list = Platform.CreateArrayList(4);
			list.Add(0);
			foreach (object obj in sigHashAlgs)
			{
				byte signature = ((SignatureAndHashAlgorithm)obj).Signature;
				if (!list.Contains(signature))
				{
					list.Add(signature);
				}
			}
			return list;
		}

		// Token: 0x04001EB4 RID: 7860
		public static readonly byte[] EmptyBytes = new byte[0];

		// Token: 0x04001EB5 RID: 7861
		public static readonly short[] EmptyShorts = new short[0];

		// Token: 0x04001EB6 RID: 7862
		public static readonly int[] EmptyInts = new int[0];

		// Token: 0x04001EB7 RID: 7863
		public static readonly long[] EmptyLongs = new long[0];

		// Token: 0x04001EB8 RID: 7864
		internal static readonly byte[] SSL_CLIENT = new byte[]
		{
			67,
			76,
			78,
			84
		};

		// Token: 0x04001EB9 RID: 7865
		internal static readonly byte[] SSL_SERVER = new byte[]
		{
			83,
			82,
			86,
			82
		};

		// Token: 0x04001EBA RID: 7866
		internal static readonly byte[][] SSL3_CONST = TlsUtilities.GenSsl3Const();
	}
}
