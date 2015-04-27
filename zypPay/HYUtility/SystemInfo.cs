using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Management;
using System.Text;

namespace HYUtility
{
    // <summary>
    /// 定义CPU的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CpuInfo
    {
        /// <summary>
        /// OEM ID
        /// </summary>
        public uint dwOemId;
        /// <summary>
        /// 页面大小
        /// </summary>
        public uint dwPageSize;
        public uint lpMinimumApplicationAddress;
        public uint lpMaximumApplicationAddress;
        public uint dwActiveProcessorMask;
        /// <summary>
        /// CPU个数
        /// </summary>
        public uint dwNumberOfProcessors;
        /// <summary>
        /// CPU类型
        /// </summary>
        public uint dwProcessorType;
        public uint dwAllocationGranularity;
        /// <summary>
        /// CPU等级
        /// </summary>
        public uint dwProcessorLevel;
        public uint dwProcessorRevision;
    }
   public class SystemInfo
    {
        private const int CHAR_COUNT = 128;
        public SystemInfo()
        {

        }
        [DllImport("kernel32")]
        private static extern void GetWindowsDirectory(StringBuilder WinDir, int count);

        [DllImport("kernel32")]
        private static extern void GetSystemDirectory(StringBuilder SysDir, int count);

        [DllImport("kernel32")]
        private static extern void GetSystemInfo(ref CpuInfo cpuInfo);

       
        /// <summary>
        /// 查询CPU编号
        /// </summary>
        /// <returns></returns>
        public string GetCpuId()
        {
            ManagementClass mClass = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mClass.GetInstances();
            string cpuId = null;
            foreach (ManagementObject mo in moc)
            {
                cpuId = mo.Properties["ProcessorId"].Value.ToString();
                break;
            }
            return cpuId;
        }

        /// <summary>
        /// 查询CPU编号
        /// </summary>
        /// <returns></returns>
        public static  string GetCPUId()
        {
            ManagementClass mClass = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mClass.GetInstances();
            string cpuId = null;
            foreach (ManagementObject mo in moc)
            {
                cpuId = mo.Properties["ProcessorId"].Value.ToString();
                break;
            }
            return cpuId;
        }

        /// <summary>
        /// 查询硬盘编号
        /// </summary>
        /// <returns></returns>
        public string GetMainHardDiskId()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            String hardDiskID = null;
            foreach (ManagementObject mo in searcher.Get())
            {
                hardDiskID = mo["SerialNumber"].ToString().Trim();
                break;
            }
            return hardDiskID;
        }

        /// <summary>
        /// 获取Windows目录
        /// </summary>
        /// <returns></returns>
        public string GetWinDirectory()
        {
            StringBuilder sBuilder = new StringBuilder(CHAR_COUNT);
            GetWindowsDirectory(sBuilder, CHAR_COUNT);
            return sBuilder.ToString();
        }

        /// <summary>
        /// 获取系统目录
        /// </summary>
        /// <returns></returns>
        public string GetSysDirectory()
        {
            StringBuilder sBuilder = new StringBuilder(CHAR_COUNT);
            GetSystemDirectory(sBuilder, CHAR_COUNT);
            return sBuilder.ToString();
        }

        /// <summary>
        /// 获取CPU信息
        /// </summary>
        /// <returns></returns>
        public CpuInfo GetCpuInfo()
        {
            CpuInfo cpuInfo = new CpuInfo();
            GetSystemInfo(ref cpuInfo);
            return cpuInfo;
        }

       
        /// <summary>
        /// 获取系统名称
        /// </summary>
        /// <returns></returns>
        public string GetOperationSystemInName()
        {
            OperatingSystem os = System.Environment.OSVersion;
            string osName = "UNKNOWN";
            switch (os.Platform)
            {
                case PlatformID.Win32Windows:
                    switch (os.Version.Minor)
                    {
                        case 0: osName = "Windows 95"; break;
                        case 10: osName = "Windows 98"; break;
                        case 90: osName = "Windows ME"; break;
                    }
                    break;
                case PlatformID.Win32NT:
                    switch (os.Version.Major)
                    {
                        case 3: osName = "Windws NT 3.51"; break;
                        case 4: osName = "Windows NT 4"; break;
                        case 5: if (os.Version.Minor == 0)
                            {
                                osName = "Windows 2000";
                            }
                            else if (os.Version.Minor == 1)
                            {
                                osName = "Windows XP";
                            }
                            else if (os.Version.Minor == 2)
                            {
                                osName = "Windows Server 2003";
                            }
                            break;
                        case 6: osName = "Longhorn"; break;
                    }
                    break;
            }
            return String.Format("{0},{1}", osName, os.Version.ToString());
        }
    }
}
