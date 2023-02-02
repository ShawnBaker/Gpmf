using System;
using System.Collections.Generic;

namespace FrozenNorth.Gpmf
{
    /// <summary>
    /// A list of items read from GPMF data.
    /// </summary>
    public class GpmfItemList : List<GpmfItem>
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
		public GpmfItemList GetItems(string fourcc)
		{
			GpmfItemList items = new GpmfItemList();
			GetItems(this, fourcc, items);
			return items;
		}

		/// <summary>
		/// Recursively get all items with a specific FourCC code.
		/// </summary>
		/// <param name="searchItems">List of items to search through.</param>
		/// <param name="fourcc">FourCC code to search for.</param>
		/// <param name="foundItems">List to add matching items to.</param>
		private void GetItems(GpmfItemList searchItems, string fourcc, GpmfItemList foundItems)
		{
			foreach (GpmfItem item in searchItems)
			{
				if (item.FourCC == fourcc)
				{
					foundItems.Add(item);
				}
				if (item.TypeSize.Type == 0)
				{
					GetItems(item.Payload as GpmfItemList, fourcc, foundItems);
				}
			}
		}
	}
}
