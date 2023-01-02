using System.Runtime.InteropServices;

namespace Gpmf
{
	public class GpmfTypeSize
	{
		// instance variables
		public char Type;
		public int StructureSize;
		public int Repeat;

		/// <summary>
		/// Initializes the fields from values.
		/// </summary>
		/// <param name="type">GPMF type.</param>
		/// <param name="size">Number of bytes for one item.</param>
		/// <param name="repeat">Number of items.</param>
		public GpmfTypeSize(byte type, int size, int repeat)
		{
			Type = (char)type;
			StructureSize = size;
			Repeat = repeat;
		}

		/// <summary>
		/// Initializes the fields from a pointer to encoded data.
		/// </summary>
		/// <param name="ptr">Encoded data.</param>
		public GpmfTypeSize(ref IntPtr ptr)
		{
			Type = (char)Marshal.ReadByte(ptr);
			ptr += 1;
			StructureSize = (int)(uint)Marshal.ReadByte(ptr);
			ptr += 1;
			Repeat = GpmfParser.ReadUInt16(ref ptr);
		}

		/// <summary>
		/// Gets the total size.
		/// </summary>
		public int Size => StructureSize * Repeat;

		/// <summary>
		/// Gets the total size rounded up to the nearest multiple of 4.
		/// </summary>
		public int RoundedSize => (((Size - 1) >> 2) + 1) << 2;
	}
}
