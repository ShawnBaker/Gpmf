# Gpmf
Extracts GPMF data from MP4 files.

# API

```
FrozenNorth.Gpmf

/// <summary>
/// Loads the GPMF items from an MP4 file.
/// </summary>
/// <param name="fileName">Full path and file name.</param>
/// <returns>List of GPMF items.</returns>
public static GpmfItems LoadMP4(string fileName)

/// <summary>
/// Gets the device name from a list of GPMF items.
/// </summary>
/// <param name="gpmfItems">List of GPMF items to search through.</param>
/// <param name="defaultName">Default device name if one isn't found.</param>
/// <returns>Device name.</returns>
public static string GetDeviceName(GpmfItems gpmfItems, string defaultName = "GoPro")

/// <summary>
/// Gets a Gpx object representing the GPS items within a list of GPMF items.
/// </summary>
/// <param name="gpmfItems">List of GPMF items to search through.</param>
/// <returns>Gpx object.</returns>
public static Gpx.Gpx GetGpx(GpmfItems gpmfItems)
```

# Classes

```
FrozenNorth.Gpmf

Gpmf
GpmfItem
GpmfItems
```

# Usage

In order to use this library in a project, the executable program must have access to the FFmpeg DLLs. The code below assumes that your top-level project contains a folder named FFmpeg, which includes the DLLs. Each of the DLL's must have __Build Action__ set to __Content__ and __Copy to Output Directory__ set to __Copy if newer__.

```
using FFmpeg.AutoGen;
using FrozenNorth.Gpx;
using FrozenNorth.Gpmf;

// get the file names
string videoFileName = ...
string gpxFileName = Path.ChangeExtension(videoFileName, "gpx");

// configure FFMPEG
Assembly assembly = Assembly.GetExecutingAssembly();
ffmpeg.RootPath = Path.Combine(Path.GetDirectoryName(assembly.Location), "FFmpeg");

// get the GPMF items
GpmfItems gpmfItems = Gpmf.LoadMP4(videoFileName);

// get the GPS items as a GPX object
Gpx gpx = Gpmf.GetGpx(gpmfItems);

// save the GPX object to a file
if (gpx.Tracks.Count > 0)
{
    Version version = assembly.GetName().Version;
    gpx.Creator = "ExtractGPX " + version.ToString((version.Revision != 0) ? 3 : 2);
    gpx.Tracks[0].Name = Path.GetFileName(videoFileName);
    GpxWriter.Save(gpx, gpxFileName);
}
else
{
    Console.WriteLine("ERROR: No GPS data found in MP4 file.");
}
```

# Attributions

This project uses the following NuGet packages:
- [FFmpeg.AutoGen](https://www.nuget.org/packages/FFmpeg.AutoGen)
- [FrozenNorth.Gpx](https://www.nuget.org/packages/FrozenNorth.Gpx)

The icon is from the [IconMarketPK](https://www.flaticon.com/authors/iconmarketpk) package on FlatIcon.

# License

MIT License

Copyright © 2023 Shawn Baker
