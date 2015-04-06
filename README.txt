There are two files here:
autonomy is a simple console application that will connect both
to the Mindwave headset and the BlueSmirf bluetooth antenna that
is hooked up to the Arduino Uno. It is a C# program, and was built 
in Visual Studio. Use other IDEs at you own discretion!

bluetooth_motors_go is the code for the Arduino Uno. It receives
the bluetooth signals from the laptop, and interprets the 
Mindwave data being sent.

Operation instructions:
1) Hook up the hardware.
2) Upload bluetooth_motors_go to the Uno.
3) Launch autonomy.