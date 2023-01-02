# Gpmf
Extracts GPMF data from MP4 files.

# Usage

```
using FFmpeg.AutoGen;
using Gpmf;

// get the file names
string videoFileName = ...
string gpxFileName = Path.ChangeExtension(videoFileName, "gpx");

// configure FFMPEG
string exeFile = Assembly.GetExecutingAssembly().Location;
ffmpeg.RootPath = Path.Combine(Path.GetDirectoryName(exeFile), "FFmpeg");

// get the GPMF and GPS items
GpmfItems gpmfItems = Gpmf.LoadMP4(videoFileName);
GpsItems gpsItems = Gpmf.GetGpsItems(gpmfItems);

// save the GPS items to a GPX file
if (gpsItems.Count > 0)
{
    Assembly assembly = Assembly.GetExecutingAssembly();
    Version version = assembly.GetName().Version;
    string creator = "Program Name " + version.ToString((version.Revision != 0) ? 3 : 2);
    int count = Gpmf.SaveGPX(videoFileName, gpxFileName, creator, Gpmf.GetDeviceName(gpmfItems), gpsItems);
    Console.WriteLine("{0} GPS points saved to {1}.", count, Path.GetFileName(gpxFileName));
}
else
{
    Console.WriteLine("ERROR: No GPS data found in MP4 file.");
}
```

# Attributions

This project uses the FFmpeg.AutoGen package.
The icon is from the IconMarketPK package on FlatIcon.
