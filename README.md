# MSI-LED-Control

Hi there ! 

This is a custom tool to control the LED system of the MSI Graphics Cards. 


## Which cards are supported ? 

For now, it have only been tested on the MSI RX480 Gaming X 8G. But if you tested it and you want to add your's on the list, just put a ticket !

nVidia has been added into the project, but I don't have any card from them. So if anyone want to test it feel free to clone the repo and try it.

## Where does this tool come from ? How does it works ? 

This tool is based on the good work of Vipeax here : https://github.com/Vipeax/MSI-LED-Tool
Unfortunatly, this tool use the librairies of MSI Gaming APP. If you want to figure out how to set the LED without, please share your work after ! It would be nice to you !

## What functions does it support ? 

For now, you can only set the animation, and the color of the MSI logo. But other functions will come soon.
You have the choice between : 
1. No Animation
2. Breathing
3. Blinking
4. Double Blinking
5. Off
6. Temperature Based

## Why there is no motherboard support ? 

The control of the motherboard need to communicate with a kernel driver, and I don't know anything about this, but you can read more about this here : https://github.com/nagisa/msi-rgb/

## Known issues
1. The updates might fail. Switch to OFF mode and turn it back on.
2. Updating the LED cause the GC to drop performances for a short time (<0.25s) but result in a short freeze. You should not use temperature based mode during a game. This is related to the librairies and the GC itself. This MIGHT me patched if someone find a way to avoid using MSI libraries.
