using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FrozenNorth.Gpmf
{
	/// <summary>
	/// A GPS item read from GPMF GPS9 data.
	/// </summary>
	internal class Gps9Item
	{
		// instance variables
		public GpsCoordList Coords;

        /// <summary>
        /// Initializes the fields from encoded values.
        /// </summary>
        /// <param name="gps">Array of GPS values.</param>
        /// <param name="scale">Array of scale values to apply to the GPS values.</param>
        internal Gps9Item(byte[] gps, int[] scale)
		{
			Coords = new GpsCoordList();
			for (int i = 0; i < gps.Length; i += 32)
			{
                GCHandle pinnedArray = GCHandle.Alloc(gps, GCHandleType.Pinned);
                IntPtr ptr = pinnedArray.AddrOfPinnedObject();
                int latitude = GpmfParser.ReadInt32(ref ptr);
                int longitude = GpmfParser.ReadInt32(ref ptr);
                int elevation = GpmfParser.ReadInt32(ref ptr);
                int speed2d = GpmfParser.ReadInt32(ref ptr);
                int speed3d = GpmfParser.ReadInt32(ref ptr);
                int days = GpmfParser.ReadInt32(ref ptr);
                int ms = GpmfParser.ReadInt32(ref ptr);
                int dop = GpmfParser.ReadInt16(ref ptr);
                uint fix = GpmfParser.ReadUInt16(ref ptr);
                pinnedArray.Free();
                DateTime time = new DateTime(2000, 1, 1) + TimeSpan.FromDays(days / scale[5]) + TimeSpan.FromMilliseconds(ms);
                double ddop = dop * 1.0 / scale[7];
                fix = (uint)(fix / scale[8]);
                GpsCoord coord = new GpsCoord(latitude * 1.0 / scale[0], longitude * 1.0 / scale[1], elevation * 1.0 / scale[2],
												speed2d * 1.0 / scale[3], speed3d * 1.0 / scale[4], time, dop * 1.0 / scale[7], fix);
				Coords.Add(coord);
            }
        }
	}

    /// <summary>
    /// A list of GPS items read from GPMF data.
    /// </summary>
    internal class Gps9ItemList : List<Gps9Item> { }
}
