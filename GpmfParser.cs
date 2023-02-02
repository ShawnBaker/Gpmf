using System;
using System.Runtime.InteropServices;
using System.Text;
using FFmpeg.AutoGen;

namespace FrozenNorth.Gpmf
{
	/// <summary>
	/// GPMF data parser.
	/// </summary>
	internal static unsafe class GpmfParser
	{
		/// <summary>
		/// Gets a list of GPMF items from an AVPacket's data.
		/// </summary>
		/// <param name="packet">AVPacket to parse the data of.</param>
		/// <returns>List of GPMF items.</returns>
		public static GpmfItemList GetItems(AVPacket packet)
		{
			IntPtr ptr = (IntPtr)packet.data;
			return GetItems(ref ptr, packet.size);
		}

		/// <summary>
		/// Gets a list of GPMF items from a pointer to data.
		/// </summary>
		/// <param name="ptr">Pointer to the data to parse.</param>
		/// <param name="len">Length of the data.</param>
		/// <returns>List of GPMF items.</returns>
		public static GpmfItemList GetItems(ref IntPtr ptr, int len)
		{
			GpmfItemList items = new GpmfItemList();
			int n = 0;
			while (n < len)
			{
				GpmfItem item = new GpmfItem(ref ptr);
				n += item.TypeSize.RoundedSize + 8;
				items.Add(item);
			}
			return items;
		}

		/// <summary>
		/// Parses the payload of a GPMF item.
		/// </summary>
		/// <param name="ptr">Pointer to te payload.</param>
		/// <param name="item">Length of the payload.</param>
		/// <returns>An object containing the parsed data.</returns>
		public static object ParsePayload(ref IntPtr ptr, GpmfItem item)
		{
			int n;
			string s;
			switch (item.TypeSize.Type)
			{
				case '\0':
					GpmfItemList items = GetItems(ref ptr, item.TypeSize.Size);
					ptr += item.TypeSize.RoundedSize - item.TypeSize.Size;
					return items;
				case 'c':
					if (item.FourCC == "UNIT")
					{
						n = item.TypeSize.Repeat;
						string[] strings = new string[n];
						for (int i = 0; i < n; i++)
						{
							strings[i] = ReadString(ref ptr, item.TypeSize.StructureSize);
						}
						ptr += item.TypeSize.RoundedSize - item.TypeSize.Size;
						return strings;
					}
					else
					{
						s = ReadString(ref ptr, item.TypeSize.Size);
						ptr += item.TypeSize.RoundedSize - item.TypeSize.Size;
						return s;
					}
				case 'd': // float64
					n = item.TypeSize.Size / 8;
					double[] doubles = new double[n];
					for (int i = 0; i < n; i++)
					{
						doubles[i] = ReadDouble(ref ptr);
					}
					ptr += item.TypeSize.RoundedSize - (n * 8);
					if (doubles.Length > 1)
						return doubles;
					else
						return doubles[0];
				case 'f': // float32
					n = item.TypeSize.Size / 4;
					float[] singles = new float[n];
					for (int i = 0; i < n; i++)
					{
						singles[i] = ReadFloat(ref ptr);
					}
					ptr += item.TypeSize.RoundedSize - (n * 4);
					if (singles.Length > 1)
						return singles;
					else
						return singles[0];
				case 'b': // int8
					n = item.TypeSize.Size;
					sbyte[] sbytes = new sbyte[n];
					for (int i = 0; i < n; i++)
					{
						sbytes[i] = (sbyte)Marshal.ReadByte(ptr);
						ptr += 1;
					}
					ptr += item.TypeSize.RoundedSize - item.TypeSize.Size;
					if (sbytes.Length > 1)
						return sbytes;
					else
						return sbytes[0];
				case 'B': // uint8
					n = item.TypeSize.Size;
					byte[] bytes = new byte[n];
					for (int i = 0; i < n; i++)
					{
						bytes[i] = Marshal.ReadByte(ptr);
						ptr += 1;
					}
					ptr += item.TypeSize.RoundedSize - item.TypeSize.Size;
					if (bytes.Length > 1)
						return bytes;
					else
						return bytes[0];
				case 's': // int16
					n = item.TypeSize.Size / 2;
					short[] shorts = new short[n];
					for (int i = 0; i < n; i++)
					{
						shorts[i] = ReadInt16(ref ptr);
					}
					ptr += item.TypeSize.RoundedSize - (n * 2);
					if (shorts.Length > 1)
						return shorts;
					else
						return shorts[0];
				case 'S': // uint16
					n = item.TypeSize.Size / 2;
					ushort[] ushorts = new ushort[n];
					for (int i = 0; i < n; i++)
					{
						ushorts[i] = ReadUInt16(ref ptr);
					}
					ptr += item.TypeSize.RoundedSize - (n * 2);
					if (ushorts.Length > 1)
						return ushorts;
					else
						return ushorts[0];
				case 'l': // int32
					n = item.TypeSize.Size / 4;
					int[] ints = new int[n];
					for (int i = 0; i < n; i++)
					{
						ints[i] = ReadInt32(ref ptr);
					}
					ptr += item.TypeSize.RoundedSize - (n * 4);
					if (ints.Length > 1)
						return ints;
					else
						return ints[0];
				case 'L': // uint32
					n = item.TypeSize.Size / 4;
					uint[] uints = new uint[n];
					for (int i = 0; i < n; i++)
					{
						uints[i] = ReadUInt32(ref ptr);
					}
					ptr += item.TypeSize.RoundedSize - (n * 4);
					if (uints.Length > 1)
						return uints;
					else
						return uints[0];
				case 'j': // int64
					n = item.TypeSize.Size / 8;
					long[] longs = new long[n];
					for (int i = 0; i < n; i++)
					{
						longs[i] = ReadInt64(ref ptr);
					}
					ptr += item.TypeSize.RoundedSize - (n * 8);
					if (longs.Length > 1)
						return longs;
					else
						return longs[0];
				case 'J': // uint64
					n = item.TypeSize.Size / 8;
					ulong[] ulongs = new ulong[n];
					for (int i = 0; i < n; i++)
					{
						ulongs[i] = ReadUInt64(ref ptr);
					}
					ptr += item.TypeSize.RoundedSize - (n * 8);
					if (ulongs.Length > 1)
						return ulongs;
					else
						return ulongs[0];
				case 'U':
					string dt = ReadString(ref ptr, item.TypeSize.Size);
					ptr += item.TypeSize.RoundedSize - item.TypeSize.Size;
					int ms = 0;
					if (dt.Length > 13)
					{
						ms = int.Parse(dt.Substring(13));
					}
					return new DateTime(int.Parse("20" + dt.Substring(0, 2)), int.Parse(dt.Substring(2, 2)), int.Parse(dt.Substring(4, 2)),
										int.Parse(dt.Substring(6, 2)), int.Parse(dt.Substring(8, 2)), int.Parse(dt.Substring(10, 2)),
										ms, DateTimeKind.Utc);
				default:
					byte[] b = ReadBytes(ref ptr, item.TypeSize.Size);
					ptr += item.TypeSize.RoundedSize - item.TypeSize.Size;
					return b;
			}
		}

		/// <summary>
		/// Reads a 16 bit signed integer.
		/// </summary>
		/// <param name="ptr">Pointer to the encoded data.</param>
		/// <returns>16 bit signed integer.</returns>
		public static short ReadInt16(ref IntPtr ptr)
		{
			return BitConverter.ToInt16(ReadBytesReverse(ref ptr, 2), 0);
		}

		/// <summary>
		/// Reads a 16 bit unsigned integer.
		/// </summary>
		/// <param name="ptr">Pointer to the encoded data.</param>
		/// <returns>16 bit unsigned integer.</returns>
		public static ushort ReadUInt16(ref IntPtr ptr)
		{
			return BitConverter.ToUInt16(ReadBytesReverse(ref ptr, 2), 0);
		}

		/// <summary>
		/// Reads a 32 bit signed integer.
		/// </summary>
		/// <param name="ptr">Pointer to the encoded data.</param>
		/// <returns>32 bit signed integer.</returns>
		public static int ReadInt32(ref IntPtr ptr)
		{
			return BitConverter.ToInt32(ReadBytesReverse(ref ptr, 4), 0);
		}

		/// <summary>
		/// Reads a 32 bit unsigned integer.
		/// </summary>
		/// <param name="ptr">Pointer to the encoded data.</param>
		/// <returns>32 bit unsigned integer.</returns>
		public static uint ReadUInt32(ref IntPtr ptr)
		{
			return BitConverter.ToUInt32(ReadBytesReverse(ref ptr, 4), 0);
		}

		/// <summary>
		/// Reads a 64 bit signed integer.
		/// </summary>
		/// <param name="ptr">Pointer to the encoded data.</param>
		/// <returns>64 bit signed integer.</returns>
		public static long ReadInt64(ref IntPtr ptr)
		{
			return BitConverter.ToInt64(ReadBytesReverse(ref ptr, 8), 0);
		}

		/// <summary>
		/// Reads a 64 bit unsigned integer.
		/// </summary>
		/// <param name="ptr">Pointer to the encoded data.</param>
		/// <returns>64 bit unsigned integer.</returns>
		public static ulong ReadUInt64(ref IntPtr ptr)
		{
			return BitConverter.ToUInt64(ReadBytesReverse(ref ptr, 8), 0);
		}

		/// <summary>
		/// Reads a 32 bit floating point number.
		/// </summary>
		/// <param name="ptr">Pointer to the encoded data.</param>
		/// <returns>32 bit floating point number.</returns>
		public static float ReadFloat(ref IntPtr ptr)
		{
			return BitConverter.ToSingle(ReadBytesReverse(ref ptr, 4), 0);
		}

		/// <summary>
		/// Reads a 64 bit floating point number.
		/// </summary>
		/// <param name="ptr">Pointer to the encoded data.</param>
		/// <returns>64 bit floating point number.</returns>
		public static double ReadDouble(ref IntPtr ptr)
		{
			return BitConverter.ToDouble(ReadBytesReverse(ref ptr, 8), 0);
		}

		/// <summary>
		/// Reads a array of bytes.
		/// </summary>
		/// <param name="ptr">Pointer to the encoded data.</param>
		/// <param name="len">Length of the encoded data.</param>
		/// <returns>Array of bytes.</returns>
		public static byte[] ReadBytes(ref IntPtr ptr, int len)
		{
			byte[] bytes = new byte[len];
			Marshal.Copy(ptr, bytes, 0, len);
			ptr += len;
			return bytes;
		}

		/// <summary>
		/// Reads a array of bytes and reverses the order of the bytes.
		/// </summary>
		/// <param name="ptr">Pointer to the encoded data.</param>
		/// <param name="len">Length of the encoded data.</param>
		/// <returns>Array of bytes.</returns>
		public static byte[] ReadBytesReverse(ref IntPtr ptr, int len)
		{
			byte[] bytes = new byte[len];
			int j = len;
			for (int i = 0; i < len; i++)
			{
				bytes[--j] = Marshal.ReadByte(ptr);
				ptr += 1;
			}
			return bytes;
		}

		/// <summary>
		/// Reads a string.
		/// </summary>
		/// <param name="ptr">Pointer to the encoded data.</param>
		/// <param name="len">Length of the encoded data.</param>
		/// <returns>A string.</returns>
		public static string ReadString(ref IntPtr ptr, int len)
		{
			byte[] bytes = ReadBytes(ref ptr, len);
			string s = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
			int i = s.IndexOf('\0');
			if (i != -1)
			{
				s = s.Substring(0, i);
			}
			return s;
		}
	}
}
