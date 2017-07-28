using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MSI_LED_Custom
{
    static class _NDA
    {
        [DllImport("Lib/NDA.dll", CharSet = CharSet.Unicode)]
        public static extern bool NDA_Initialize();

        [DllImport("Lib/NDA.dll", CharSet = CharSet.Unicode)]
        public static extern bool NDA_GetGPUCounts(out int gpuCounts);

        [DllImport("Lib/NDA.dll", CharSet = CharSet.Unicode)]
        public static extern bool NDA_GetGraphicsInfo(int iAdapterIndex, out NdaGraphicsInfo graphicsInfo);

        [DllImport("Lib/NDA.dll", CharSet = CharSet.Unicode)]
        public static extern bool NDA_SetIlluminationParm_RGB(int iAdapterIndex, int cmd, int led1, int led2, int ontime, int offtime, int time, int darktime, int bright, int r, int g, int b, bool one = false);
    }
}
