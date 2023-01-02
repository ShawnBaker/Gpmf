# Gpmf
Extracts GPMF data from MP4 files.

# Usage

```
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
}
```