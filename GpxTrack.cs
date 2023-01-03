using System.Collections.Generic;

namespace FrozenNorth.Gpmf
{
	/// <summary>
	/// A track of GPS data read from a GPX file.
	/// </summary>
	public class GpxTrack
	{
		// public variables
		public string Name = "";
		public string Description = "";
		public string Source = "";
		public GpxPoints Points = new GpxPoints();

		/// <summary>
		/// Clears the values.
		/// </summary>
		public void Clear()
		{
			Name = "";
			Description = "";
			Source = "";
			Points.Clear();
		}
	}

	/// <summary>
	/// A list of GPS data tracks read from a GPX file.
	/// </summary>
	public class GpxTracks : List<GpxTrack> { }
}
