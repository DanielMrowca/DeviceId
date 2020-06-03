using System;
using System.Net;
using DeviceId.Components;

namespace DeviceId
{
    public static class DeviceInfo
    {
        /// <summary>
        ///     Return host name of device.
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
            => Dns.GetHostName();

        /// <summary>
        ///     Return current user name of device. 
        /// </summary>
        /// <returns>Current user name of device</returns>
        public static string GetUserName()
            => Environment.UserName;


        /// <summary>
        ///     Return the machine name of device.
        /// </summary>
        /// <returns>Machine name of device</returns>
        public static string GetMachineName()
            => Environment.MachineName;


        /// <summary>
        ///     Return the operating system version of device.
        /// </summary>
        /// <returns>Operating system version of device</returns>
        public static string GetOSVersion()
            => OS.Version;


        /// <summary>
        ///     Return the processor ID of device.
        /// </summary>
        /// <returns>Processor ID of device.</returns>
        public static string GetProcessorId()
        {
            IDeviceIdComponent comp;
            if (OS.IsWindows)
            {
                comp = new WmiDeviceIdComponent("ProcessorId", "Win32_Processor", "ProcessorId");
            }
            else if (OS.IsLinux)
            {
                comp = new FileDeviceIdComponent("ProcessorId", "/proc/cpuinfo", true);
            }
            else
            {
                comp = new UnsupportedDeviceIdComponent("ProcessorId");
            }

            return comp.GetValue();
        }

        /// <summary>
        ///     Return the motherboard serial number of device. On Linux, this requires root privilege.
        /// </summary>
        /// <returns>The motherboard serial number of devicee.</returns>
        public static string GetMotherboardSerialNumber()
        {
            IDeviceIdComponent comp;
            if (OS.IsWindows)
            {
                comp = new WmiDeviceIdComponent("MotherboardSerialNumber", "Win32_BaseBoard", "SerialNumber");
            }
            else if (OS.IsLinux)
            {
                comp = new FileDeviceIdComponent("MotherboardSerialNumber", "/sys/class/dmi/id/board_serial");
            }
            else
            {
                comp = new UnsupportedDeviceIdComponent("MotherboardSerialNumber");
            }

            return comp.GetValue();
        }

        /// <summary>
        ///     Return the system drive's serial number of device.
        /// </summary>
        /// <returns>The system drive's serial number of device.</returns>
        public static string GetSystemDriveSerialNumber()
        {
            IDeviceIdComponent comp;
            if (OS.IsWindows)
            {
                comp = new SystemDriveSerialNumberDeviceIdComponent();
            }
            else if (OS.IsLinux)
            {
                comp = new LinuxRootDriveSerialNumberDeviceIdComponent();
            }
            else
            {
                comp = new UnsupportedDeviceIdComponent("SystemDriveSerialNumber");
            }

            return comp.GetValue();
        }

        /// <summary>
        ///     Return the system UUID of device. On Linux, this requires root privilege.
        /// </summary>
        /// <returns>The system UUID of device.</returns>
        public static string GetSystemUUID()
        {
            IDeviceIdComponent comp;
            if (OS.IsWindows)
            {
                comp = new WmiDeviceIdComponent("SystemUUID", "Win32_ComputerSystemProduct", "UUID");
            }
            else if (OS.IsLinux)
            {
                comp = new FileDeviceIdComponent("SystemUUID", "/sys/class/dmi/id/product_uuid");
            }
            else
            {
                comp = new UnsupportedDeviceIdComponent("SystemUUID");
            }

            return comp.GetValue();
        }

        /// <summary>
        ///     Return the installation id of the OS.
        /// </summary>
        /// <returns>The installation id of the OS.</returns>
        public static string GetOSInstallationID()
        {
            IDeviceIdComponent comp;
            if (OS.IsWindows)
            {
                comp = new RegistryValueDeviceIdComponent("OSInstallationID", @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography", "MachineGuid");
            }
            else if (OS.IsLinux)
            {
                comp = new FileDeviceIdComponent("OSInstallationID", new string[] { "/var/lib/dbus/machine-id", "/etc/machine-id" });
            }
            else
            {
                comp = new UnsupportedDeviceIdComponent("OSInstallationID");
            }

            return comp.GetValue();
        }
    }
}
