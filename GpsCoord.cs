using System.Collections.Generic;

namespace FrozenNorth.Gpmf
{
	public class GpsCoord
	{
		// instance variables
		public double Latitude;
		public double Longitude;
		public double Altitude;
		public double Speed2D;
		public double Speed3D;

		/// <summary>
		/// Initializes the fields from values.
		/// </summary>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		/// <param name="altitude">Altitude.</param>
		/// <param name="speed2D">2D speed.</param>
		/// <param name="speed3D">3D speed.</param>
		public GpsCoord(double latitude, double longitude, double altitude, double speed2D, double speed3D)
		{
			Latitude = latitude;
			Longitude = longitude;
			Altitude = altitude;
			Speed2D = speed2D;
			Speed3D = speed3D;
		}
	}

	public class GpsCoords : List<GpsCoord> { }
}
