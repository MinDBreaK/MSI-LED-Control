using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing;
using System.Diagnostics;

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
                Debug.WriteLine("Initilization of the AMD managers");
                ledList.Add( new LedManager_AMD_Side() );
                ledList.Add(new LedManager_AMD_Front());

            } 
        }

        public void StartAll()
        {
            Debug.WriteLine("Starting all the managers");
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

        public void UpdateAll(Color color, AnimationType aT)
        {
            this.ledColor = color;
            this.animationType = aT;

            foreach (LedManager lm in ledList)
            {
                lm.Update(animationType, ledColor);
            }
        }
    }
}
