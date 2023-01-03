using System;
using System.Collections.Generic;

namespace FrozenNorth.Gpmf
{
	/// <summary>
	/// A point of GPS data read from a GPX file.
	/// </summary>
	public class GpxPoint
	{
		// public variables
		public DateTime Time = DateTime.MinValue;
		public double Latitude = 0;
		public double Longitude = 0;
		public double Altitude = 0;

		/// <summary>
		/// Creates an empty point.
		/// </summary>
		public GpxPoint() {}

		/// <summary>
		/// Creates a point from values.
		/// </summary>
		/// <param name="time">Time.</param>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		/// <param name="altitude">Altitude.</param>
		public GpxPoint(DateTime time, double latitude, double longitude, double altitude)
		{
			Time = time;
			Latitude = latitude;
			Longitude = longitude;
			Altitude = altitude;
		}

		/// <summary>
		/// Creates a clone of this point.
		/// </summary>
		/// <returns>A clone of this point</returns>
		public GpxPoint Clone()
		{
			return new GpxPoint(Time, Latitude, Longitude, Altitude);
		}
	}

	/// <summary>
	/// A list of GPS data points read from a GPX file.
	/// </summary>
	public class GpxPoints : List<GpxPoint>
	{
		/// <summary>
		/// Gets the time from the first point.
		/// </summary>
		/// <returns>The time from the first point.</returns>
		public DateTime StartTime
		{
			get
			{
				if (Count > 0)
				{
					return this[0].Time;
				}
				return DateTime.MinValue;
			}
		}

		/// <summary>
		/// Gets the time between the first and last points.
		/// </summary>
		/// <returns>The time between the first and last points.</returns>
		public TimeSpan TimeRange
		{
			get
			{
				if (Count > 1)
				{
					return this[Count - 1].Time - this[0].Time;
				}
				return TimeSpan.Zero;
			}
		}

		/// <summary>
		/// Gets the total distance between all points.
		/// </summary>
		/// <returns>The total distance between all points.</returns>
		public double Distance
		{
			get
			{
				double distance = 0;
				for (int i = 1; i < Count; i++)
				{
					distance += DistanceBetweenPoints(this[i - 1], this[i]);
				}
				return distance;
			}
		}

		/// <summary>
		/// Gets the total range in elevation and the lowest and highest elevations.
		/// </summary>
		/// <param name="low">The returned lowest elevation.</param>
		/// <param name="high">The returned highest elevation.</param>
		/// <returns>The total range in elevation.</returns>
		public double GetElevationRange(out double low, out double high)
		{
			if (Count > 0)
			{
				low = double.MaxValue;
				high = double.MinValue;
				foreach (GpxPoint point in this)
				{
					if (point.Altitude < low)
					{
						low = point.Altitude;
					}
					if (point.Altitude > high)
					{
						high = point.Altitude;
					}
				}
			}
			else
			{
				low = 0;
				high = 0;
			}
			return high - low;
		}

		/// <summary>
		/// Gets the difference between the lowest and highest elevations.
		/// </summary>
		/// <returns>The difference between the lowest and highest elevations.</returns>
		public double ElevationRange => GetElevationRange(out double low, out double high);

		/// <summary>
		/// Calculates the distance between two GPS corordinates.
		/// </summary>
		/// <param name="point1">First GPS coordinate.</param>
		/// <param name="point2">Second GPS coordinate.</param>
		/// <returns>The distance between the two GPS corordinates.</returns>
		public static double DistanceBetweenPoints(GpxPoint point1, GpxPoint point2)
		{
			var earthRadiusKm = 6371;

			var dLat = ToRadians(point2.Latitude - point1.Latitude);
			var dLon = ToRadians(point2.Longitude - point1.Longitude);

			double lat1 = ToRadians(point1.Latitude);
			double lat2 = ToRadians(point2.Latitude);

			var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
					Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
			var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			return earthRadiusKm * c;
		}

		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees">An angle in degrees.</param>
		/// <returns>The angle in radians.</returns>
		public static double ToRadians(double degrees)
		{
			return (Math.PI / 180) * degrees;
		}
	}
}
