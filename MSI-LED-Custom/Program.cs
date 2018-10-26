using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MSI_LED_Custom
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        /// 

        
        public static bool overwriteSecurityChecks;
        public static List<int> adapterIndexes;
        public static Manufacturer manufacturer;
        public static Color ledColor = Color.FromArgb(255, 255, 0, 0);
        public static AnimationType animationType = AnimationType.NoAnimation;
        public static LedManager_Common ledManager;
        public static AdlGraphicsInfo AdlGraphicsInfo;
        public static NdaGraphicsInfo NdaGraphicsInfo;

        public static int tempMin = 35;
        public static int tempMax = 70;


        public static string vendorCode    = "N/A";
        public static string deviceCode    = "N/A";
        public static string subVendorCode = "N/A";
        public static string[] args;
        static bool updateAll = false;


        [STAThread]
        static void Main(String[] args )
        {

            if (Properties.Settings.Default["Color"] is Color)
            {
                Program.ledColor = (Color)Properties.Settings.Default["Color"];
            }

            if (Properties.Settings.Default["Mode"] is int)
            {
                Program.animationType = (AnimationType)Properties.Settings.Default["Mode"];
            }

            if (Properties.Settings.Default["TempMin"] is int)
            {
                Program.tempMin = (int)Properties.Settings.Default["TempMin"];
            }

            if (Properties.Settings.Default["TempMax"] is int)
            {
                Program.tempMax = (int)Properties.Settings.Default["TempMax"];
            }

            Program.args = args;
            Program.adapterIndexes = new List<int>();
            //DO NOT MODIFY HERE. USE PARAMETER !!
            //You can also add the parameter "/overwriteSecurityChecks"
            overwriteSecurityChecks = false;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("overwriteSecurityChecks"))
                {
                    overwriteSecurityChecks = true;
                }
                else if (args[i].Equals("updateAll"))
                {
                    updateAll = true;
                }
            }

            int gpuCountNda = 0;
            if (_NDA.NDA_Initialize())
            {
                bool canGetGpuCount = _NDA.NDA_GetGPUCounts(out gpuCountNda);
                if (canGetGpuCount == false)
                {
                    return;
                }
                if (gpuCountNda > 0 && InitializeNvidiaAdapters(gpuCountNda))
                {
                    manufacturer = Manufacturer.Nvidia;
                }
            }


            if (gpuCountNda == 0 && _ADL.ADL_Initialize())
            {
                int gpuCountAdl;
                bool canGetGpuCount = _ADL.ADL_GetGPUCounts(out gpuCountAdl);
                if (canGetGpuCount == false)
                {
                    return;
                }
                if (gpuCountAdl > 0 && InitializeAmdAdapters(gpuCountAdl))
                {
                    manufacturer = Manufacturer.AMD;
                }
            }

            
            ledManager = new LedManager_Common(manufacturer, animationType);
            ledManager.InitLedManagers();
            ledManager.StartAll();
            ledManager.UpdateAll(ledColor, animationType, tempMin, tempMax);

            if (!updateAll)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            
            ledManager.StopAll();

        }


        private static bool InitializeAmdAdapters(int gpuCount)
        {
            for (int i = 0; i < gpuCount; i++)
            {
                if (_ADL.ADL_GetGraphicsInfo(i, out AdlGraphicsInfo) == false)
                {
                    return false;
                }

                // PCI\VEN_1002&DEV_67DF&SUBSYS_34111462&REV_CF\4&25438C51&0&0008
                var pnpSegments = AdlGraphicsInfo.Card_PNP.Split('\\');

                if (pnpSegments.Length < 2)
                {
                    continue;
                }

                // VEN_1002&DEV_67DF&SUBSYS_34111462&REV_CF
                var codeSegments = pnpSegments[1].Split('&');

                if (codeSegments.Length < 3)
                {
                    continue;
                }

                vendorCode = codeSegments[0].Substring(4, 4).ToUpper();
                deviceCode = codeSegments[1].Substring(4, 4).ToUpper();
                subVendorCode = codeSegments[2].Substring(11, 4).ToUpper();

                if (overwriteSecurityChecks)
                {
                    if (vendorCode.Equals(Constants.VendorCodeAmd, StringComparison.OrdinalIgnoreCase))
                    {
                        adapterIndexes.Add(i);
                    }
                }
                else if (vendorCode.Equals(Constants.VendorCodeAmd, StringComparison.OrdinalIgnoreCase)
                    && subVendorCode.Equals(Constants.SubVendorCodeMsi, StringComparison.OrdinalIgnoreCase)
                    && Constants.SupportedDeviceCodes.Any(dc => deviceCode.Equals(dc, StringComparison.OrdinalIgnoreCase)))
                {
                    adapterIndexes.Add(i);
                }
            }

            return true;
        }

        private static bool InitializeNvidiaAdapters(int gpuCount)
        {
            for (int i = 0; i < gpuCount; i++)
            {
                
                if (_NDA.NDA_GetGraphicsInfo(i, out NdaGraphicsInfo) == false)
                {
                    return false;
                }

                deviceCode = NdaGraphicsInfo.Card_pDeviceId.Substring(0, 4).ToUpper();
                vendorCode = NdaGraphicsInfo.Card_pDeviceId.Substring(4, 4).ToUpper();
                subVendorCode = NdaGraphicsInfo.Card_pSubSystemId.Substring(4, 4).ToUpper();

                if (overwriteSecurityChecks)
                {
                    if (vendorCode.Equals(Constants.VendorCodeNvidia, StringComparison.OrdinalIgnoreCase))
                    {
                        adapterIndexes.Add(i);
                    }
                }
                else if (vendorCode.Equals(Constants.VendorCodeNvidia, StringComparison.OrdinalIgnoreCase)
                    && subVendorCode.Equals(Constants.SubVendorCodeMsi, StringComparison.OrdinalIgnoreCase)
                    && Constants.SupportedDeviceCodes.Any(dc => deviceCode.Equals(dc, StringComparison.OrdinalIgnoreCase)))
                {
                    adapterIndexes.Add(i);
                }
            }
            return true;
        }
    }
}
