using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace MSI_LED_Custom
{
    class LedUpdater
    {

        private bool updated = false;
        private bool shouldStop = false;

        private AnimationType animationType;
        private Mutex mutex;
        private Color ledColor;
        private Manufacturer manufacturer;
        private AdlGraphicsInfo adlGraphicsInfo;
        private int[] temperatureLimits;

        public LedUpdater()
        {
            this.animationType = Program.animationType;
            this.manufacturer = Program.manufacturer;
            this.mutex = new Mutex();
            this.ledColor = Program.ledColor;
            this.temperatureLimits = new int[10];
            this.temperatureLimits[0] = 40;
            this.temperatureLimits[1] = 75;
        }

        private void UpdateLeds(int cmd, int ledId, int time, int ontime = 0, int offtime = 0, int darkTime = 0)
        {
            for (int i = 0; i < Program.adapterIndexes.Count; i++)
            {
                bool oneCall = animationType != AnimationType.NoAnimation;

                if (manufacturer == Manufacturer.AMD)
                {
                    _ADL.ADL_SetIlluminationParm_RGB(i, cmd, ledId, 0, ontime, offtime, time, darkTime, 0, ledColor.R, ledColor.G, ledColor.B, oneCall);
                }                
            }
        }

        public void run()
        {
            while( !shouldStop )
            {
                switch( animationType )
                {
                    case AnimationType.NoAnimation:
                        while ( !updated && !shouldStop)
                        {
                            Thread.Sleep(2000);
                        }
                        break;
                    case AnimationType.Breathing:
                        while ( !updated && !shouldStop)
                        {
                            updateLedRight();
                            updateLedSide();
                            updateLedUnknown();
                            Thread.Sleep(3000);
                        }
                        break;

                }
            }
        }

        internal void setAnimation(int selectedIndex)
        {
            AnimationType foo = (AnimationType)Enum.ToObject(typeof(AnimationType), selectedIndex);
            if ( !Enum.IsDefined( typeof(AnimationType), foo) )
            {
                this.animationType = AnimationType.NoAnimation;
            }
        }

        public void updateFromEdition()
        {
            this.ledColor = Program.ledColor;
            this.animationType = Program.animationType;
            this.updated = true;
            this.updateLedRight();
            this.updateLedSide();
            this.updateLedUnknown();
            this.updated = false;
        }

        internal void setShouldStop(bool v)
        {
            this.shouldStop = v;
        }

        public void updateLedRight()
        {
            switch (animationType)
            {
                case AnimationType.NoAnimation:
                    UpdateLeds(21, 4, 4);
                    break;
                case AnimationType.Breathing:
                    UpdateLeds(27, 4, 7);
                    break;
                case AnimationType.Flashing:
                    UpdateLeds(28, 4, 0, 25, 100);
                    break;
                case AnimationType.DoubleFlashing:
                    UpdateLeds(30, 4, 0, 10, 10, 91);
                    break;
                case AnimationType.Off:
                    UpdateLeds(24, 4, 4);
                    break;
                case AnimationType.TemperatureBased:
                    switch (manufacturer)
                    {
                        case Manufacturer.Nvidia:
                            throw new NotImplementedException();
                            //break;
                        case Manufacturer.AMD:
                            this.mutex.WaitOne();
                            if (_ADL.ADL_GetGraphicsInfo(0, out adlGraphicsInfo))
                            {
                                int temperatureDelta = CalculateTemperatureDeltaHunderdBased(temperatureLimits[0],
                                    temperatureLimits[1], adlGraphicsInfo.GPU_Temperature_Current);
                                ledColor = GetColorForDeltaTemperature(temperatureDelta);
                                UpdateLeds(21, 4, 4);
                            }
                            mutex.ReleaseMutex();
                            break;
                    }
                    break;
            }
        }
    
        public void updateLedUnknown()
        {
            switch (animationType)
            {
                case AnimationType.NoAnimation:
                    UpdateLeds(21, 2, 4);
                    break;
                case AnimationType.Breathing:
                    UpdateLeds(27, 2, 7);
                    break;
                case AnimationType.Flashing:
                    UpdateLeds(28, 2, 0, 25, 100);
                    break;
                case AnimationType.DoubleFlashing:
                    UpdateLeds(30, 2, 0, 10, 10, 91);
                    break;
                case AnimationType.Off:
                    UpdateLeds(24, 2, 4);
                    break;
                case AnimationType.TemperatureBased:
                    switch (manufacturer)
                    {
                        case Manufacturer.Nvidia:

                            break;
                        case Manufacturer.AMD:
                            mutex.WaitOne();
                            if (_ADL.ADL_GetGraphicsInfo(0, out adlGraphicsInfo))
                            {
                                int temperatureDelta = CalculateTemperatureDeltaHunderdBased(temperatureLimits[0],
                                    temperatureLimits[1], adlGraphicsInfo.GPU_Temperature_Current);
                                ledColor = GetColorForDeltaTemperature(temperatureDelta);
                                UpdateLeds(21, 2, 4);
                            }
                            mutex.ReleaseMutex();
                            break;
                    }
                    break;
            }
            }

        public void updateLedSide()
        {
            switch (animationType)
            {
                case AnimationType.NoAnimation:
                    UpdateLeds(21, 1, 4);
                    break;
                case AnimationType.Breathing:
                    UpdateLeds(27, 1, 7);
                    break;
                case AnimationType.Flashing:
                    UpdateLeds(28, 1, 0, 25, 100);
                    break;
                case AnimationType.DoubleFlashing:
                    UpdateLeds(30, 1, 0, 10, 10, 91);
                    break;
                case AnimationType.Off:
                    UpdateLeds(24, 1, 4);
                    break;
                case AnimationType.TemperatureBased:
                    switch (manufacturer)
                    {
                        case Manufacturer.Nvidia:
                                
                            break;
                        case Manufacturer.AMD:
                            mutex.WaitOne();
                            if (_ADL.ADL_GetGraphicsInfo(0, out adlGraphicsInfo))
                            {
                                int temperatureDelta = CalculateTemperatureDeltaHunderdBased(temperatureLimits[0],
                                    temperatureLimits[1], adlGraphicsInfo.GPU_Temperature_Current);
                                ledColor = GetColorForDeltaTemperature(temperatureDelta);
                                UpdateLeds(21, 1, 4);
                            }
                            mutex.ReleaseMutex();
                            break;
                    }
                    break;
            }
        }

        private int CalculateTemperatureDeltaHunderdBased(object p1, object p2, int gPU_Temperature_Current)
        {
            throw new NotImplementedException();
        }

        private Color GetColorForDeltaTemperature(int temperatureDelta)
        {
            throw new NotImplementedException();
        }

        
    }
}
