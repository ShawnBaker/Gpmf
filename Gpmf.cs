using System;
using System.Runtime.InteropServices;
using FFmpeg.AutoGen;
using FrozenNorth.Gpx;

namespace FrozenNorth.Gpmf
{
	/// <summary>
	/// High level GPMF operations.
	/// </summary>
	public static unsafe class Gpmf
	{
		/// <summary>
		/// Loads the GPMF items from an MP4 file.
		/// </summary>
		/// <param name="fileName">Full path and file name.</param>
		/// <returns>List of GPMF items.</returns>
		public static GpmfItems LoadMP4(string fileName)
		{
			GpmfItems gpmfItems = new GpmfItems();
			AVFormatContext* formatContext = ffmpeg.avformat_alloc_context();
			ffmpeg.avformat_open_input(&formatContext, fileName, null, null);
			ffmpeg.avformat_find_stream_info(formatContext, null);
            gpmfItems.Duration = TimeSpan.FromSeconds((double)formatContext->duration / ffmpeg.AV_TIME_BASE);
            for (int i = 0; i < formatContext->nb_streams; i++)
			{
				AVStream* stream = formatContext->streams[i];
				if (stream->codecpar->codec_type == AVMediaType.AVMEDIA_TYPE_DATA)
				{
					AVDictionaryEntry* entry = ffmpeg.av_dict_get(stream->metadata, "handler_name", null, 0);
					string handler = Marshal.PtrToStringAnsi((IntPtr)entry->value + 1).Trim();
					if (handler == "GoPro MET")
					{
						AVPacket p;
						while (ffmpeg.av_read_frame(formatContext, &p) >= 0)
						{
							if (p.stream_index == i)
							{
								GpmfItems items = GpmfParser.GetItems(p);
								gpmfItems.AddRange(items);
							}
							ffmpeg.av_packet_unref(&p);
						}
					}
				}
			}
			return gpmfItems;
		}

		/// <summary>
		/// Gets the device name from a list of GPMF items.
		/// </summary>
		/// <param name="gpmfItems">List of GPMF items to search through.</param>
		/// <param name="defaultName">Default device name if one isn't found.</param>
		/// <returns>Device name.</returns>
		public static string GetDeviceName(GpmfItems gpmfItems, string defaultName = "GoPro")
		{
			GpmfItems dvnmItems = gpmfItems.GetItems("DVNM");
			foreach (GpmfItem item in dvnmItems)
			{
				string name = item.GetString();
				if (!string.IsNullOrEmpty(name))
				{
					return name;
				}
			}
			return defaultName;
		}

		/// <summary>
		/// Gets a Gpx object representing the GPS items within a list of GPMF items.
		/// </summary>
		/// <param name="gpmfItems">List of GPMF items to search through.</param>
		/// <returns>Gpx object.</returns>
		public static Gpx.Gpx GetGpx(GpmfItems gpmfItems)
		{
			// get the GPS items
			string description = "";
			string units = null;
			GpsItems gpsItems = new GpsItems();
			GpmfItems strmItems = gpmfItems.GetItems("STRM");
			foreach (GpmfItem item in strmItems)
			{
				GpmfItems strm = item.Payload as GpmfItems;
				GpmfItem gps5 = strm.Find("GPS5");
				if (gps5 != null)
				{
					GpsItem gps = new GpsItem(gps5.GetIntArray(), strm.GetItemIntArray("SCAL"), strm.GetItemDateTime("GPSU"),
												strm.GetItemUShort("GPSP") / 100.0, strm.GetItemUInt("GPSF"));
					gpsItems.Add(gps);
					if (string.IsNullOrEmpty(description))
					{
						string desc = strm.GetItemString("STNM");
						if (!string.IsNullOrEmpty(desc))
						{
							description = desc;
						}
                    }
                    if (string.IsNullOrEmpty(units))
                    {
                        string[] un = strm.GetItemStringArray("UNIT");
                        if (un != null && un.Length > 0)
                        {
                            units = string.Join(",", un);
                        }
                    }
                }
            }

			// create a GPX object containing the GPS points
			var gpx = new Gpx.Gpx();
			var track = new GpxTrack();
			track.Description = description;
			if (!string.IsNullOrEmpty(units))
			{
                if (!string.IsNullOrEmpty(track.Description))
				{
					track.Description += " - ";
                }
				track.Description += "[" + units + "]";
            }
            track.Source = GetDeviceName(gpmfItems);
            var segment = new GpxTrackSegment();
			track.Segments.Add(segment);
			gpx.Tracks.Add(track);
            for (int i = 0; i < gpsItems.Count; i++)
			{
				GpsItem gps = gpsItems[i];
				DateTime gpsTime = gps.Time;
				double seconds = 1;
				if (i < gpsItems.Count - 1)
				{
					seconds = (gpsItems[i + 1].Time - gps.Time).TotalSeconds;
				}
				else if (i > 0)
				{
                    seconds = (gpmfItems.Duration - (gps.Time - gpsItems[0].Time)).TotalSeconds;
					if (seconds <= 0 || seconds >= 2)
					{
                        seconds = (gps.Time - gpsItems[i - 1].Time).TotalSeconds;
                    }
                }
				TimeSpan interval = TimeSpan.FromSeconds(seconds / gps.Coords.Count);
				foreach (GpsCoord coord in gps.Coords)
				{
					GpxFix fix = GpxFix.None;
					if (gps.Fix == 2) fix = GpxFix.TwoD;
					else if (gps.Fix == 3) fix = GpxFix.ThreeD;
					var point = new GpxPoint(coord.Latitude, coord.Longitude, coord.Elevation, gpsTime, fix, gps.Precision);
					segment.Points.Add(point);
					gpsTime += interval;
				}
			}
			return gpx;
		}
	}
}
