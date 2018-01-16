using System;
using System.Drawing;

namespace MSI_LED_Custom
{
    abstract class LedManager : IDisposable
    {
        protected abstract void Run();
        abstract public void Start();
        abstract public void Stop();
        abstract public void Update(AnimationType newAnimation, Color ledColor, int tempMin, int tempMax);
        abstract protected void UpdateLed_NoAnimation();
        abstract protected void UpdateLed_Breathing();
        abstract protected void UpdateLed_Flashing();
        abstract protected void UpdateLed_DoubleFlashing();
        abstract protected void UpdateLed_Off();
        abstract protected void UpdateLed_TemperatureBased();
        public void Dispose()
        {
            return;
        }
    }
}
