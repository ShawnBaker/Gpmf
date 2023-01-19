using System;
using System.Collections.Generic;

namespace FrozenNorth.Gpmf
{
	/// <summary>
	/// An item read from GPMF data.
	/// </summary>
	public class GpmfItem
	{
		// instance variables
		public string FourCC;
		public GpmfTypeSize TypeSize;
		public object Payload;

		/// <summary>
		/// Initializes the fields from values.
		/// </summary>
		/// <param name="fourcc">FourCC code.</param>
		/// <param name="typeSize">Size of the item's data.</param>
		public GpmfItem(string fourcc, GpmfTypeSize typeSize)
		{
			FourCC = fourcc;
			TypeSize = typeSize;
		}

		/// <summary>
		/// Initializes the fields from a pointer to encoded data.
		/// </summary>
		/// <param name="ptr">Encoded data.</param>
		public GpmfItem(ref IntPtr ptr)
		{
			FourCC = GpmfParser.ReadString(ref ptr, 4);
			TypeSize = new GpmfTypeSize(ref ptr);
			Payload = GpmfParser.ParsePayload(ref ptr, this);
		}

		#region String

		/// <summary>
		/// Gets the payload as a string.
		/// </summary>
		/// <returns>The payload as a string.</returns>
		public string GetString()
		{
			string s = string.Empty;
			try
			{
				s = Payload as string;
			}
			catch
			{
				s = string.Empty;
			}
			return s;
		}

		/// <summary>
		/// Gets the payload as an array of strings.
		/// </summary>
		/// <returns>The payload as an array of strings.</returns>
		public string[] GetStringArray()
		{
			string[] sa = new string[0];
			try
			{
				sa = (string[])Payload;
			}
			catch
			{
				sa = new string[0];
			}
			return sa;
		}

		#endregion
		#region Int

		/// <summary>
		/// Gets the payload as an int.
		/// </summary>
		/// <returns>The payload as an int.</returns>
		public int GetInt()
		{
			int i = 0;
			try
			{
				i = (int)Payload;
			}
			catch
			{
				i = 0;
			}
			return i;
		}

		/// <summary>
		/// Gets the payload as an array of ints.
		/// </summary>
		/// <returns>The payload as an array of ints.</returns>
		public int[] GetIntArray()
		{
			int[] ia = new int[0];
			try
			{
				ia = (int[])Payload;
			}
			catch
			{
				ia = new int[0];
			}
			return ia;
		}

		#endregion
		#region UInt

		/// <summary>
		/// Gets the payload as an uint.
		/// </summary>
		/// <returns>The payload as an uint.</returns>
		public uint GetUInt()
		{
			uint u = 0;
			try
			{
				u = (uint)Payload;
			}
			catch
			{
				u = 0;
			}
			return u;
		}

		/// <summary>
		/// Gets the payload as an array of uints.
		/// </summary>
		/// <returns>The payload as an array of uints.</returns>
		public uint[] GetUIntArray()
		{
			uint[] ua = new uint[0];
			try
			{
				ua = (uint[])Payload;
			}
			catch
			{
				ua = new uint[0];
			}
			return ua;
		}

		#endregion
		#region Short

		/// <summary>
		/// Gets the payload as a short.
		/// </summary>
		/// <returns>The payload as a short.</returns>
		public short GetShort()
		{
			short s = 0;
			try
			{
				s = (short)Payload;
			}
			catch
			{
				s = 0;
			}
			return s;
		}

		/// <summary>
		/// Gets the payload as an array of shorts.
		/// </summary>
		/// <returns>The payload as an array of shorts.</returns>
		public short[] GetShortArray()
		{
			short[] sa = new short[0];
			try
			{
				sa = (short[])Payload;
			}
			catch
			{
				sa = new short[0];
			}
			return sa;
		}

		#endregion
		#region UShort

		/// <summary>
		/// Gets the payload as a ushort.
		/// </summary>
		/// <returns>The payload as a ushort.</returns>
		public ushort GetUShort()
		{
			ushort u = 0;
			try
			{
				u = (ushort)Payload;
			}
			catch
			{
				u = 0;
			}
			return u;
		}

		/// <summary>
		/// Gets the payload as an array of ushorts.
		/// </summary>
		/// <returns>The payload as an array of ushorts.</returns>
		public ushort[] GetUShortArray()
		{
			ushort[] us = new ushort[0];
			try
			{
				us = (ushort[])Payload;
			}
			catch
			{
				us = new ushort[0];
			}
			return us;
		}

		#endregion
		#region Long

		/// <summary>
		/// Gets the payload as a long.
		/// </summary>
		/// <returns>The payload as a long.</returns>
		public long GetLong()
		{
			long s = 0;
			try
			{
				s = (long)Payload;
			}
			catch
			{
				s = 0;
			}
			return s;
		}

		/// <summary>
		/// Gets the payload as an array of longs.
		/// </summary>
		/// <returns>The payload as an array of longs.</returns>
		public long[] GetLongArray()
		{
			long[] sa = new long[0];
			try
			{
				sa = (long[])Payload;
			}
			catch
			{
				sa = new long[0];
			}
			return sa;
		}

		#endregion
		#region ULong

		/// <summary>
		/// Gets the payload as a ulong.
		/// </summary>
		/// <returns>The payload as a ulong.</returns>
		public ulong GetULong()
		{
			ulong s = 0;
			try
			{
				s = (ulong)Payload;
			}
			catch
			{
				s = 0;
			}
			return s;
		}

		/// <summary>
		/// Gets the payload as an array of ulongs.
		/// </summary>
		/// <returns>The payload as an array of ulongs.</returns>
		public ulong[] GetULongArray()
		{
			ulong[] sa = new ulong[0];
			try
			{
				sa = (ulong[])Payload;
			}
			catch
			{
				sa = new ulong[0];
			}
			return sa;
		}

		#endregion
		#region DateTime

		public DateTime GetDateTime()
		{
			DateTime dt = DateTime.MinValue;
			try
			{
				dt = (DateTime)Payload;
			}
			catch
			{
				dt = DateTime.MinValue;
			}
			return dt;
		}

		#endregion
	}

	/// <summary>
	/// A list of items read from GPMF data.
	/// </summary>
	public class GpmfItems : List<GpmfItem>
	{
		public TimeSpan Duration = TimeSpan.Zero;

		/// <summary>
		/// Searches for an item with a specifc FourCC code.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Found item or null.</returns>
		public GpmfItem Find(string fourcc)
		{
			return Find(item => item.FourCC == fourcc);
		}

		#region String

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as a string.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as a string.</returns>
		public string GetItemString(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetString() : string.Empty;
		}

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as an array of strings.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as an array of strings.</returns>
		public string[] GetItemStringArray(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetStringArray() : new string[0];
		}

		#endregion
		#region Int

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as an int.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as an int.</returns>
		public int GetItemInt(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetInt() : 0;
		}

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as an array of ints.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as an array of ints.</returns>
		public int[] GetItemIntArray(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetIntArray() : new int[0];
		}

		#endregion
		#region UInt

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as an uint.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as an uint.</returns>
		public uint GetItemUInt(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetUInt() : 0;
		}

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as an array of uints.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as an array of uints.</returns>
		public uint[] GetItemUIntArray(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetUIntArray() : new uint[0];
		}

		#endregion
		#region Short

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as a short.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as a short.</returns>
		public short GetItemShort(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetShort() : (short)0;
		}

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as an array of shorts.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as an array of shorts.</returns>
		public short[] GetItemShortArray(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetShortArray() : new short[0];
		}

		#endregion
		#region UShort

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as a ushort.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as a ushort.</returns>
		public ushort GetItemUShort(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetUShort() : (ushort)0;
		}

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as an array of ushorts.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as an array of ushorts.</returns>
		public ushort[] GetItemUShortArray(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetUShortArray() : new ushort[0];
		}

		#endregion
		#region Long

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as a long.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as a long.</returns>
		public long GetItemLong(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetLong() : 0;
		}

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as an array of longs.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as an array of longs.</returns>
		public long[] GetItemLongArray(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetLongArray() : new long[0];
		}

		#endregion
		#region ULong

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as a ulong.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as a ulong.</returns>
		public ulong GetItemULong(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetULong() : 0;
		}

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as an array of ulongs.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as an array of ulongs.</returns>
		public ulong[] GetItemULongArray(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetULongArray() : new ulong[0];
		}

		#endregion
		#region DateTime

		/// <summary>
		/// Searches for an item with a specifc FourCC code and returns its payload as a DateTime.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>Payload as a DateTime.</returns>
		public DateTime GetItemDateTime(string fourcc)
		{
			GpmfItem item = Find(fourcc);
			return (item != null) ? item.GetDateTime() : DateTime.MinValue;
		}

		#endregion

		/// <summary>
		/// Recursively get all items with a specific FourCC code.
		/// </summary>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <returns>List of items.</returns>
		public GpmfItems GetItems(string fourcc)
		{
			GpmfItems items = new GpmfItems();
			GetItems(this, fourcc, items);
			return items;
		}

		/// <summary>
		/// Recursively get all items with a specific FourCC code.
		/// </summary>
		/// <param name="searchItems">List of items to search through.</param>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <param name="foundItems">List to add matching items to.</param>
		private void GetItems(GpmfItems searchItems, string fourcc, GpmfItems foundItems)
		{
			foreach (GpmfItem item in searchItems)
			{
				if (item.FourCC == fourcc)
				{
					foundItems.Add(item);
				}
				if (item.TypeSize.Type == 0)
				{
					GetItems(item.Payload as GpmfItems, fourcc, foundItems);
				}
			}
		}
	}
}
