Micro:bit BLE Xamarin
=====================
A sample Xamarin app (Android/iOS) that demonstrates connecting with a BBC
Micro:bit and fetching sensor data.  

### [Play store listing](https://play.google.com/store/apps/details?id=com.sumitgouthaman.microbitble)

## What is the BBC Micro:bit
From [Wikipedia](https://en.wikipedia.org/wiki/Micro_Bit):
> The Micro Bit (also referred to as BBC Micro Bit, stylized
as micro:bit) is an ARM-based embedded system designed by the BBC for use in
computer education in the UK. The device has been given away free to every
year 7 pupil in the UK, and is also available for purchase by anyone.  

More details/videos on the  official [website](https://www.microbit.co.uk/).

## Instructions
* You need to flash a hex file on the Micro:bit that enables all the bluetooth
services you want.
* You can use the hex file provided by Martin Woolley for this Micro:bit BLUE
app. The file can be found linked in this
[article](http://blog.bluetooth.com/bbc-microbit/).
* You also need to pair the Micro:bit with your phone. The instructions for
doing this can be found at the link above.

## Known issues
1. Bearing value in the Magnetometer service doesn't seem to work.
1. Issues when multiple services are opened and closed in rapid succession.
This is probably related to how subscribing/un-subscribing to characteristics
works.
1. This app hasn't been tested on iOS yet. If you manage to try it out and it
works, let me know.

**Any bug fixes, feature additions are welcome!**