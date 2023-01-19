using System;
using System.Collections.Generic;

namespace FrozenNorth.Gpmf
{
	/// <summary>
	/// A GPS item read from GPMF data.
	/// </summary>
	internal class GpsItem
	{
		// instance variables
		public GpsCoords Coords;
		public DateTime Time;
		public double Precision;
		public uint Fix;

        /// <summary>
        /// Initializes the fields from encoded values.
        /// </summary>
        /// <param name="gps">Array of GPS values.</param>
        /// <param name="scale">Array of scale values to apply to the GPS values.</param>
        /// <param name="time">Time of the first GPS point.</param>
        /// <param name="precision">Precision of the data.</param>
        /// <param name="fix">Fix value.</param>
        internal GpsItem(int[] gps, int[] scale, DateTime time, double precision, uint fix)
		{
			Coords = new GpsCoords();
			for (int i = 0; i < gps.Length; i += 5)
			{
				GpsCoord coord = new GpsCoord(gps[i] * 1.0 / scale[0], gps[i + 1] * 1.0 / scale[1], gps[i + 2] * 1.0 / scale[2],
												gps[i + 3] * 1.0 / scale[3], gps[i + 4] * 1.0 / scale[4]);
				Coords.Add(coord);
			}
			Time = time;
			Precision = precision;
			Fix = fix;
		}
	}

    /// <summary>
    /// A list of GPS items read from GPMF data.
    /// </summary>
    internal class GpsItems : List<GpsItem> { }
}
