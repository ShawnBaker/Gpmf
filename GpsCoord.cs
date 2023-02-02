using System;
using System.Collections.Generic;

namespace FrozenNorth.Gpmf
{
    /// <summary>
    /// A set of GPS coordinates read from GPMF data.
    /// </summary>
    internal class GpsCoord
	{
		// instance variables
		public double Latitude;
		public double Longitude;
		public double Elevation;
		public double Speed2D;
		public double Speed3D;
        public DateTime Time;
        public double Precision;
        public uint Fix;

        /// <summary>
        /// Initializes the fields from values.
        /// </summary>
        /// <param name="latitude">Latitude.</param>
        /// <param name="longitude">Longitude.</param>
        /// <param name="elevation">Elevation.</param>
        /// <param name="speed2D">2D speed.</param>
        /// <param name="speed3D">3D speed.</param>
        public GpsCoord(double latitude, double longitude, double elevation, double speed2D, double speed3D)
		{
			Latitude = latitude;
			Longitude = longitude;
			Elevation = elevation;
			Speed2D = speed2D;
			Speed3D = speed3D;
		}

        /// <summary>
        /// Initializes the fields from values.
        /// </summary>
        /// <param name="latitude">Latitude.</param>
        /// <param name="longitude">Longitude.</param>
        /// <param name="elevation">Elevation.</param>
        /// <param name="speed2D">2D speed.</param>
        /// <param name="speed3D">3D speed.</param>
        /// <param name="time">Time.</param>
        /// <param name="precision">Precision.</param>
        /// <param name="fix">Fix.</param>
        public GpsCoord(double latitude, double longitude, double elevation, double speed2D, double speed3D,
                        DateTime time, double precision, uint fix)
        {
            Latitude = latitude;
            Longitude = longitude;
            Elevation = elevation;
            Speed2D = speed2D;
            Speed3D = speed3D;
            Time = time;
            Precision = precision;
            Fix = fix;
        }
    }

    /// <summary>
    /// A list of GPS coordinates read from GPMF data.
    /// </summary>
    internal class GpsCoordList : List<GpsCoord> { }
}
