using System;
using UsbEject;

namespace Getting_USB_Devices
{
    class DeviceInfo
    {
        public string Directory { get; set; }
        public string Name { get; set; }
        public string TotalSpace { get; set; }
        public string FreeSpace { get; set; }
        public string OccupiedSpace { get; set; }
        public bool IsMtp { get; set; }

        public DeviceInfo(string directory, string name, string totalSpace, string freeSpace, string occupiedSpace, bool isMtp)
        {
            Directory = directory;
            Name = name;
            TotalSpace = totalSpace;
            FreeSpace = freeSpace;
            OccupiedSpace = occupiedSpace;
            IsMtp = isMtp;
        }

        public String Retrieval()
        {
            if (IsMtp)
            {
                return "This is mtp device";
            }
            else
            {
                try
                {
                    foreach (Volume dev in new VolumeDeviceClass().Devices)
                    {
                        if (dev.LogicalDrive.Length == 0)
                        {
                            break;
                        }
                        // is this volume a logical disk?
                        if (dev.LogicalDrive.Equals(Directory))
                        {
                            dev.Eject(true);

                            foreach (Volume dev1 in new VolumeDeviceClass().Devices) //check
                            {
                                try
                                {
                                    if (dev1.LogicalDrive.Equals(dev?.LogicalDrive))
                                    {
                                        return "Device is Busy";
                                    }
                                }
                                catch (NullReferenceException e)
                                {
                                    Console.WriteLine(e.ToString());
                                    return "Device was Remove";
                                }
                            }
                        }
                    }
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.ToString());
                    return "Exception";
                }
                return "Device was Retrieval";
            }
        }
    }
}

