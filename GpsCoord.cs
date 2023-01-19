using System.Collections.Generic;

namespace FrozenNorth.Gpmf
{
	/// <summary>
	/// A set of GPS coordinates read from GPMF data.
	/// </summary>
	public class GpsCoord
	{
		// instance variables
		public double Latitude;
		public double Longitude;
		public double Elevation;
		public double Speed2D;
		public double Speed3D;

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
	}

	/// <summary>
	/// A list of GPS coordinates read from GPMF data.
	/// </summary>
	public class GpsCoords : List<GpsCoord> { }
}
