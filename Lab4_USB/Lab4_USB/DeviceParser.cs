using System.Collections.Generic;
using System.Linq;
using System.IO;
using MediaDevices;

namespace Getting_USB_Devices
{
    class DeviceParser
    {
        public List<DeviceInfo> GetDevices()
        {
            List<DeviceInfo> devices = new List<DeviceInfo>();
            List<DriveInfo> drives = DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Removable).ToList();
            List<MediaDevice> mtpDevices = MediaDevice.GetDevices().ToList();
            foreach (MediaDevice device in mtpDevices)
            {
                device.Connect();
                if (device.DeviceType != DeviceType.Generic)
                {
                    devices.Add(new DeviceInfo(string.Empty/*device.Manufacturer + " " + device.Model + " " + */, device.FriendlyName, null, null, null, true));
                }
            }
            foreach (DriveInfo drive in drives)
            {
                devices.Add(new DeviceInfo(drive.Name.Split('\\')[0], drive.VolumeLabel, BytesToMegaBytesString(drive.TotalSize),
                BytesToMegaBytesString(drive.TotalFreeSpace),
                BytesToMegaBytesString(drive.TotalSize - drive.TotalFreeSpace), false));
            }
            return devices;
        }

        private string BytesToMegaBytesString(long value)
        {
            double megaBytes = (value / 1000) / 1000;
            return megaBytes.ToString() + " MB";
        }
    }
}
