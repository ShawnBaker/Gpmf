using System;
using System.Collections.Generic;

namespace FrozenNorth.Gpmf
{
	public class GpsItem
	{
		// instance variables
		public GpsCoords Coords;
		public string Description;
		public DateTime Time;
		public double Precision;
		public uint Fix;
		public string[] Units;

		/// <summary>
		/// Initializes the fields from encoded values.
		/// </summary>
		/// <param name="gps">Array of GPS values.</param>
		/// <param name="scale">Array of scale values to apply to the GPS values.</param>
		/// <param name="description">Description.</param>
		/// <param name="time">Time of the first GPS point.</param>
		/// <param name="precision">Precision of the data.</param>
		/// <param name="fix">Fix value.</param>
		/// <param name="units">Units of the GPS values.</param>
		public GpsItem(int[] gps, int[] scale, string description, DateTime time, double precision, uint fix, string[] units)
		{
			Coords = new GpsCoords();
			for (int i = 0; i < gps.Length; i += 5)
			{
				GpsCoord coord = new GpsCoord(gps[i] * 1.0 / scale[0], gps[i + 1] * 1.0 / scale[1], gps[i + 2] * 1.0 / scale[2],
												gps[i + 3] * 1.0 / scale[3], gps[i + 4] * 1.0 / scale[4]);
				Coords.Add(coord);
			}
			Description = description;
			Time = time;
			Precision = precision;
			Fix = fix;
			Units = units;
		}

		/// <summary>
		/// Gets the units as a comma separated string.
		/// </summary>
		public string UnitsString => string.Join(",", Units);
	}

	public class GpsItems : List<GpsItem> { }
}
