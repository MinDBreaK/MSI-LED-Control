using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;


namespace MSI_LED_Custom
{
    class LedManager_AMD_Side : LedManager
    {
        private Thread t;
        private bool shouldStop = false;
        private bool shouldUpdate = false;
        private Color ledColor;
        private List<int> adapterIndexes;
        private AnimationType animationType;
        private Mutex mutex;
        private AdlGraphicsInfo adlGraphicsInfo;
        private int[] temperatureLimits;
        private int lastTemperature = -1;

        public LedManager_AMD_Side()
        {
            this.adapterIndexes = Program.adapterIndexes;
            this.ledColor = Program.ledColor;
            this.temperatureLimits = new int[2];
            this.temperatureLimits[0] = Program.tempMin;
            this.temperatureLimits[1] = Program.tempMax;
            this.mutex = new Mutex();

        }
        protected override void Run()
        {
            while (!shouldStop)
            {
                switch (animationType)
                {
                    case AnimationType.NoAnimation:
                        this.UpdateLed_NoAnimation();
                        break;
                    case AnimationType.Breathing:
                        this.UpdateLed_Breathing();
                        break;
                    case AnimationType.Flashing:
                        this.UpdateLed_Flashing();
                        break;
                    case AnimationType.DoubleFlashing:
                        this.UpdateLed_DoubleFlashing();
                        break;
                    case AnimationType.Off:
                        this.UpdateLed_Off();
                        break;
                    case AnimationType.TemperatureBased:
                        this.UpdateLed_TemperatureBased();
                        break;
                    default:
                        this.UpdateLed_NoAnimation();
                        break;
                }
                Thread.Sleep(100);
                this.shouldUpdate = false;
            }
        }

        public override void Start()
        {
            this.t = new Thread(new ThreadStart(this.Run));
            t.Start();
        }

        public override void Stop()
        {
            this.shouldStop = true;
            this.shouldUpdate = true;
        }

        public override void Update(AnimationType newAnimation, Color ledColor, int tempMin, int tempMax)
        {
            this.animationType = newAnimation;
            this.ledColor = ledColor;
            this.temperatureLimits[0] = tempMin;
            this.temperatureLimits[1] = tempMax;
            this.shouldUpdate = true;
        }

        private void PatientlyWait()
        {
            while (!shouldStop && !shouldUpdate)
            {
                Thread.Sleep(1000);
            }
            return;
        }

        protected override void UpdateLed_NoAnimation()
        {
            for (int i = 0; i < this.adapterIndexes.Count; i++)
            {
                _ADL.ADL_SetIlluminationParm_RGB(adapterIndexes[i], 21, 1, 0, 0, 0, 4, 0, 0, ledColor.R, ledColor.G, ledColor.B, true);
            }
            this.PatientlyWait();
        }

        protected override void UpdateLed_Breathing()
        {
            for (int i = 0; i < this.adapterIndexes.Count; i++)
            {
                _ADL.ADL_SetIlluminationParm_RGB(adapterIndexes[i], 27, 1, 0, 0, 0, 4, 0, 0, ledColor.R, ledColor.G, ledColor.B, false);
            }
            this.PatientlyWait();
        }

        protected override void UpdateLed_Flashing()
        {
            for (int i = 0; i < this.adapterIndexes.Count; i++)
            {
                _ADL.ADL_SetIlluminationParm_RGB(adapterIndexes[i], 28, 1, 0, 100, 100, 4, 0, 0, ledColor.R, ledColor.G, ledColor.B, false);
            }
            this.PatientlyWait();
        }

        protected override void UpdateLed_DoubleFlashing()
        {
            for (int i = 0; i < this.adapterIndexes.Count; i++)
            {
                _ADL.ADL_SetIlluminationParm_RGB(adapterIndexes[i], 30, 1, 0, 10, 15, 4, 2, 0, ledColor.R, ledColor.G, ledColor.B, false);
            }
            this.PatientlyWait();
        }

        protected override void UpdateLed_Off()
        {
            for (int i = 0; i < this.adapterIndexes.Count; i++)
            {
                _ADL.ADL_SetIlluminationParm_RGB(adapterIndexes[i], 24, 1, 0, 0, 0, 0, 0, 0, ledColor.R, ledColor.G, ledColor.B, true);
            }
            this.PatientlyWait();
        }

        protected override void UpdateLed_TemperatureBased()
        {
            while (!shouldUpdate)
            {
                mutex.WaitOne();
                _ADL.ADL_GetGraphicsInfo(0, out adlGraphicsInfo);

                if ((this.lastTemperature > this.adlGraphicsInfo.GPU_Temperature_Current + 1) || (this.lastTemperature < this.adlGraphicsInfo.GPU_Temperature_Current - 1))
                {
                    this.lastTemperature = this.adlGraphicsInfo.GPU_Temperature_Current;
                    ledColor = GetColorForDeltaTemperature(this.adlGraphicsInfo.GPU_Temperature_Current);

                    for (int i = 0; i < this.adapterIndexes.Count; i++)
                    {
                        _ADL.ADL_SetIlluminationParm_RGB(adapterIndexes[i], 21, 1, 0, 0, 0, 4, 0, 0, ledColor.R, ledColor.G, ledColor.B, true);
                    }
                }
                mutex.ReleaseMutex();
                Thread.Sleep(2000);
            }
        }

        private Color GetColorForDeltaTemperature(int x)
        {
            return Color.FromArgb(255, GetRedForDeltaTemperature(x), GetGreenForDeltaTemperature(x), 0);
        }

        private int GetRedForDeltaTemperature(int x)
        {
            if (x < temperatureLimits[0])
            {
                return 0;
            }
            if (x > temperatureLimits[1])
            {
                return 255;
            }
            float a = 255f / (temperatureLimits[1] - temperatureLimits[0]);
            float b = 0 - a * temperatureLimits[0];
            int y = Convert.ToInt32(a * x + b);
            return y;
        }

        private int GetGreenForDeltaTemperature(int x)
        {
            if (x < temperatureLimits[0])
            {
                return 255;
            }
            if (x > temperatureLimits[1])
            {
                return 0;
            }
            float a = 10f / (temperatureLimits[1] - temperatureLimits[0]);
            float b = 0 - a * temperatureLimits[0];
            int y = Convert.ToInt32(a * x + b);
            return 10 - y;
        }
    }
}