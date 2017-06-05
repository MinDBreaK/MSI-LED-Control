using System.Drawing;

namespace MSI_LED_Custom
{
    abstract class LedManager
    {
        protected abstract void Run();
        abstract public void Start();
        abstract public void Stop();
        abstract public void Update(AnimationType newAnimation, Color ledColor);
        abstract protected void updateLed_NoAnimation();
        abstract protected void updateLed_Breathing();
        abstract protected void updateLed_Flashing();
        abstract protected void updateLed_DoubleFlashing();
        abstract protected void updateLed_Off();
        abstract protected void updateLed_TemperatureBased();
    }
}
