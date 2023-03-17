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
		public static GpmfItemList LoadMP4(string fileName)
		{
			GpmfItemList gpmfItems = new GpmfItemList();
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
								GpmfItemList items = GpmfParser.GetItems(p);
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
		public static string GetDeviceName(GpmfItemList gpmfItems, string defaultName = "GoPro")
		{
			GpmfItemList dvnmItems = gpmfItems.GetItems("DVNM");
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
		public static Gpx.Gpx GetGpx(GpmfItemList gpmfItems)
		{
			// get the GPS items
			string description = "";
			string units = null;
			Gps5ItemList gps5Items = new Gps5ItemList();
            Gps9ItemList gps9Items = new Gps9ItemList();
            GpmfItemList strmItems = gpmfItems.GetItems("STRM");
			foreach (GpmfItem item in strmItems)
			{
				GpmfItemList strm = item.Payload as GpmfItemList;
                GpmfItem gps5 = strm.Find("GPS5");
                GpmfItem gps9 = strm.Find("GPS9");
                if (gps5 != null)
				{
					Gps5Item gps = new Gps5Item(gps5.GetIntArray(), strm.GetItemIntArray("SCAL"), strm.GetItemDateTime("GPSU"),
												strm.GetItemUShort("GPSP") / 100.0, strm.GetItemUInt("GPSF"));
					gps5Items.Add(gps);
                }
				if (gps9 != null)
				{
                    Gps9Item gps = new Gps9Item((byte[])gps9.Payload, strm.GetItemIntArray("SCAL"));
					gps9Items.Add(gps);

                }
                if (gps5 != null || gps9 != null)
				{
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
			if (gps9Items.Count > 0)
			{
				for (int i = 0; i < gps9Items.Count; i++)
				{
					Gps9Item gps = gps9Items[i];
                    foreach (GpsCoord coord in gps.Coords)
                    {
                        GpxFix fix = GpxFix.None;
                        if (coord.Fix == 2) fix = GpxFix.TwoD;
                        else if (coord.Fix == 3) fix = GpxFix.ThreeD;
                        var point = new GpxPoint(coord.Latitude, coord.Longitude, coord.Elevation, coord.Time, fix, coord.Precision);
                        segment.Points.Add(point);
                    }
                }
            }
			else
			{
				for (int i = 0; i < gps5Items.Count; i++)
				{
					Gps5Item gps = gps5Items[i];
					DateTime gpsTime = gps.Time;
					double seconds = 1;
					if (i < gps5Items.Count - 1)
					{
						seconds = (gps5Items[i + 1].Time - gps.Time).TotalSeconds;
					}
					else if (i > 0)
					{
						seconds = (gpmfItems.Duration - (gps.Time - gps5Items[0].Time)).TotalSeconds;
						if (seconds <= 0 || seconds >= 2)
						{
							seconds = (gps.Time - gps5Items[i - 1].Time).TotalSeconds;
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
			}
			return gpx;
		}
	}
}
