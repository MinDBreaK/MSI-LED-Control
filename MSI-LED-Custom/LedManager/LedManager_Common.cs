using System.Collections;
using System.Drawing;

namespace MSI_LED_Custom
{
    class LedManager_Common
    {
        private ArrayList ledList;
        private Manufacturer manfacturer;
        public AnimationType animationType;
        public Color ledColor;

        public LedManager_Common(Manufacturer m, AnimationType aT)
        {
            this.ledList = new ArrayList( );
            this.animationType = aT;
            this.manfacturer = m;
        }

        public void InitLedManagers()
        {
            if ( this.manfacturer == Manufacturer.AMD )
            {
                ledList.Add( new LedManager_AMD_Side() );
                ledList.Add( new LedManager_AMD_Front());
            } else if ( this.manfacturer == Manufacturer.Nvidia )
            {
                ledList.Add( new LedManager_NVD_Side() );
                ledList.Add( new LedManager_NVD_Front());
            }
        }

        public void StartAll()
        {
            foreach (LedManager lm in ledList)
            {
                lm.Start();
            }
        }

        public void StopAll()
        {
            foreach (LedManager lm in ledList)
            {
                lm.Stop();
            }
        }

        public void UpdateAll(Color color, AnimationType aT, int tempMin, int tempMax)
        {
            this.ledColor = color;
            this.animationType = aT;

            foreach (LedManager lm in ledList)
            {
                lm.Update(animationType, ledColor, tempMin, tempMax);
            }
        }
    }
}
