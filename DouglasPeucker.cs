using System;
using System.Collections.Generic;

namespace FrozenNorth.Gpmf
{
	/// <summary>
	/// Performs a point reduction operation on a list of GPX points using the Douglas-Peucker algorithm.
	/// </summary>
	public class DouglasPeucker
	{
		/// <summary>
		/// Describes the function that calculates the distance for the GPX values being reduced.
		/// </summary>
		/// <param name="point1">Start point.</param>
		/// <param name="point2">Middle point.</param>
		/// <param name="point">End point.</param>
		/// <returns>The distance of the middle point from the line running through the begin and end points.</returns>
		public delegate double DistanceDelegate(GpxPoint point1, GpxPoint point2, GpxPoint point);

		/// <summary>
		/// Reduces the number of GPX points in a list using the Douglas-Peucker algorithm.
		/// </summary>
		/// <param name="points">LList of GPX points.</param>
		/// <param name="tolerance">Tolerance to use in the algorithm.</param>
		/// <param name="distanceDelegate">Distance caluclation delegate.</param>
		/// <returns>Reduced list of GPX points.</returns>
		public static GpxPoints Reduce(GpxPoints points, Double tolerance, DistanceDelegate distanceDelegate)
		{

			if (points == null || points.Count < 3)
				return points;

			Int32 firstPoint = 0;
			Int32 lastPoint = points.Count - 1;
			List<Int32> pointIndexsToKeep = new List<Int32>();
			pointIndexsToKeep.Add(firstPoint);
			pointIndexsToKeep.Add(lastPoint);
			while (points[firstPoint].Equals(points[lastPoint]))
			{
				lastPoint--;
			}

			Reduce(points, firstPoint, lastPoint, tolerance, ref pointIndexsToKeep, distanceDelegate);

			GpxPoints returnPoints = new GpxPoints();
			pointIndexsToKeep.Sort();
			foreach (Int32 index in pointIndexsToKeep)
			{
				returnPoints.Add(points[index]);
			}
			return returnPoints;
		}

		/// <summary>
		/// Recursive reduction method used by the top level method.
		/// </summary>
		/// <param name="points">List of points.</param>
		/// <param name="firstPoint">Index of the first point to look at.</param>
		/// <param name="lastPoint">Index of the last point to look at.</param>
		/// <param name="tolerance">Tolerance to use in the algorithm.</param>
		/// <param name="pointIndexsToKeep">Indicies of the points to keep.</param>
		/// <param name="distanceDelegate">Distance caluclation delegate.</param>
		private static void Reduce(GpxPoints points, Int32 firstPoint, Int32 lastPoint, Double tolerance,
									ref List<Int32> pointIndexsToKeep, DistanceDelegate distanceDelegate)
		{
			Double maxDistance = 0;
			Int32 indexFarthest = 0;

			for (Int32 index = firstPoint; index < lastPoint; index++)
			{
				Double distance = distanceDelegate(points[firstPoint], points[lastPoint], points[index]);
				if (distance > maxDistance)
				{
					maxDistance = distance;
					indexFarthest = index;
				}
			}
			if (maxDistance > tolerance && indexFarthest != 0)
			{
				pointIndexsToKeep.Add(indexFarthest);
				Reduce(points, firstPoint, indexFarthest, tolerance, ref pointIndexsToKeep, distanceDelegate);
				Reduce(points, indexFarthest, lastPoint, tolerance, ref pointIndexsToKeep, distanceDelegate);
			}
		}
	}
}
