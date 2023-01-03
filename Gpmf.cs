using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Xml;
using FFmpeg.AutoGen;

namespace FrozenNorth.Gpmf
{
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
			GpmfItems strmItems = gpmfItems.GetItems("DVNM");
			foreach (GpmfItem item in strmItems)
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
		/// Gets a list of all the GPS items with a list of GPMF items.
		/// </summary>
		/// <param name="gpmfItems">List of GPMF items to search through.</param>
		/// <returns>List of GPS items.</returns>
		public static GpsItems GetGpsItems(GpmfItems gpmfItems)
		{
			GpsItems gpsItems = new GpsItems();
			GpmfItems strmItems = gpmfItems.GetItems("STRM");
			foreach (GpmfItem item in strmItems)
			{
				GpmfItems strm = item.Payload as GpmfItems;
				GpmfItem gps5 = strm.Find("GPS5");
				if (gps5 != null)
				{
					GpsItem gps = new GpsItem(gps5.GetIntArray(), strm.GetItemIntArray("SCAL"), strm.GetItemString("STNM"),
												strm.GetItemDateTime("GPSU"), strm.GetItemUShort("GPSP") / 100.0,
												strm.GetItemUInt("GPSF"), strm.GetItemStringArray("UNIT"));
					gpsItems.Add(gps);
				}
			}
			return gpsItems;
		}

		/// <summary>
		/// Saves a list of GPS items to a GPX file.
		/// </summary>
		/// <param name="videoFileName">Full path and name of the existing MP4 file.</param>
		/// <param name="gpxFileName">Full path and name of the GPX file to be created.</param>
		/// <param name="creator">Name of the program saving this file.</param>
		/// <param name="deviceName">Name of the device that the video file was created by.</param>
		/// <param name="items">List of GPS items.</param>
		/// <returns>Number of GPS coordinates that were saved.</returns>
		public static int SaveGPX(string videoFileName, string gpxFileName, string creator, string deviceName, GpsItems items)
		{
			int numCoords = 0;
			XmlWriterSettings settings = new XmlWriterSettings()
			{
				Indent = true,
				IndentChars = "\t"
			};
			XNamespace ns = "http://www.topografix.com/GPX/1/1";
			XmlWriter textWriter = XmlTextWriter.Create(gpxFileName, settings);
			textWriter.WriteStartDocument();
			textWriter.WriteStartElement("gpx", "http://www.topografix.com/GPX/1/1");
			textWriter.WriteAttributeString("version", "1.1");
			textWriter.WriteAttributeString("creator", creator);
			textWriter.WriteStartElement("trk");
			textWriter.WriteElementString("name", Path.GetFileName(videoFileName));
			textWriter.WriteElementString("desc", items[0].Description + " - [" + items[0].UnitsString + "]");
			textWriter.WriteElementString("src", deviceName);
			textWriter.WriteStartElement("trkseg");
			for (int i = 0; i < items.Count; i++)
			{
				GpsItem gps = items[i];
				DateTime gpsTime = gps.Time;
				double seconds = 1;
				if (i < items.Count - 1)
				{
					seconds = (items[i + 1].Time - gps.Time).TotalSeconds;
				}
				else if (i > 0)
				{
					seconds = (gps.Time - items[i - 1].Time).TotalSeconds;
				}
				TimeSpan interval = TimeSpan.FromSeconds(seconds / gps.Coords.Count);
				foreach (GpsCoord coord in gps.Coords)
				{
					textWriter.WriteStartElement("trkpt");
					textWriter.WriteAttributeString("lat", coord.Latitude.ToString());
					textWriter.WriteAttributeString("lon", coord.Longitude.ToString());
					textWriter.WriteElementString("ele", coord.Altitude.ToString());
					textWriter.WriteElementString("time", gpsTime.ToString("o"));
					textWriter.WriteElementString("fix", gps.Fix.ToString() + "d");
					textWriter.WriteElementString("hdop", gps.Precision.ToString());
					textWriter.WriteEndElement();
					gpsTime += interval;
					numCoords++;
				}
			}
			textWriter.WriteEndElement();
			textWriter.WriteEndElement();
			textWriter.WriteEndElement();
			textWriter.WriteEndDocument();
			textWriter.Close();
			return numCoords;
		}

	}
}
