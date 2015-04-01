using System;

using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Ports;

using NeuroSky.ThinkGear;
using NeuroSky.ThinkGear.Algorithms;


namespace testprogram {
    class Program {
        static Connector connector;
        static byte poorSig;
        static bool _continue;
        static SerialPort _serialPort;


        public static void Main(string[] args) 
        {
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);
            // Create a new SerialPort object with default settings.
            
            Console.Write("Connecting...");
            _serialPort = new SerialPort("COM5");
            try
            { 
                _serialPort.Open(); 
            }
            catch(IOException e)
            {
                Console.Write("COM5 failed to open\n");
                Console.Write(e.Message);
            }
            finally
            {
                if (_serialPort.IsOpen)
                {
                    Console.Write("Success!");
                    Console.Write("\nType QUIT to exit");
                }               
            }
            // Initialize a new Connector and add event handlers

            connector = new Connector();
            connector.DeviceConnected += new EventHandler(OnDeviceConnected);
            connector.DeviceConnectFail += new EventHandler(OnDeviceFail);
            connector.DeviceValidating += new EventHandler(OnDeviceValidating);

            // Scan for devices across COM ports
            // The COM port named will be the first COM port that is checked.
            //connector.ConnectScan("COM40");
            connector.ConnectScan("COM6");
            _continue = true;

            while (_continue)
            {
                message = Console.ReadLine();

                if (stringComparer.Equals("quit", message))
                {
                    _continue = false;
                }
                else
                {
                    _serialPort.WriteLine(message);
                }
                Read();
            }
            try
            {
                _serialPort.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine("COM5 did not close successfully.");
            }
   
            Console.WriteLine("HelloEEG!");

            // Initialize a new Connector and add event handlers

            connector = new Connector();
            connector.DeviceConnected += new EventHandler(OnDeviceConnected);
            connector.DeviceConnectFail += new EventHandler(OnDeviceFail);
            connector.DeviceValidating += new EventHandler(OnDeviceValidating);

            // Scan for devices across COM ports
            // The COM port named will be the first COM port that is checked.
            //connector.ConnectScan("COM40");
            connector.ConnectScan("COM6");

            // Blink detection needs to be manually turned on
            connector.setBlinkDetectionEnabled(true);
            Thread.Sleep(450000);

            System.Console.WriteLine("Goodbye.");
            connector.Close();
            Environment.Exit(0);
        }


        // Called when a device is connected 

        static void OnDeviceConnected(object sender, EventArgs e) {

            Connector.DeviceEventArgs de = (Connector.DeviceEventArgs)e;

            Console.WriteLine("Device found on: " + de.Device.PortName);
            de.Device.DataReceived += new EventHandler(OnDataReceived);

        }




        // Called when scanning fails

        static void OnDeviceFail(object sender, EventArgs e) {

            Console.WriteLine("No devices found! :(");

        }



        // Called when each port is being validated

        static void OnDeviceValidating(object sender, EventArgs e) {

            Console.WriteLine("Validating: ");

        }

        // Called when data is received from a device

        static void OnDataReceived(object sender, EventArgs e) {

            //Device d = (Device)sender;

            Device.DataEventArgs de = (Device.DataEventArgs)e;
            DataRow[] tempDataRowArray = de.DataRowArray;

            TGParser tgParser = new TGParser();
            tgParser.Read(de.DataRowArray);
            string fileName = "C:\\Users\\Sean\\Documents\\GitHub\\autonomy\\out.txt";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                //string line;
                //int size = tgParser.ParsedData.Length;
                //for (int i = 0; i < size; i++)
                //{
                //    string me = tgParser.ParsedData[i] + "\r\n";
                //    writer.Write(me);
                //}



                /* Loops through the newly parsed data of the connected headset*/
                // The comments below indicate and can be used to print out the different data outputs. 

                for (int i = 0; i < tgParser.ParsedData.Length; i++)
                {

                    if (tgParser.ParsedData[i].containskey("raw"))
                    {
                        //console.writeline("raw value:" + tgParser.parseddata[i]["raw"]);
                        writer.Write("raw value:" + tgParser.ParsedData[i]["raw"]);
                    }

                    if (tgParser.ParsedData[i].containskey("poorsignal"))
                    {

                        //the following line prints the time associated with the parsed data
                        //console.writeline("time:" + tgParser.parseddata[i]["time"]);

                        //a poor signal value of 0 indicates that your headset is fitting properly
                        //console.writeline("poor signal:" + tgParser.parseddata[i]["poorsignal"]);
                        writer.Write("poor signal:" + tgParser.ParsedData[i]["poorsignal"]);
                        poorsig = (byte)tgParser.ParsedData[i]["poorsignal"];
                    }


                    if (tgParser.ParsedData[i].containskey("attention"))
                    {
                        //console.writeline("att value:" + tgParser.parseddata[i]["attention"]);
                        writer.Write("att value:" + tgParser.ParsedData[i]["attention"]);
                    }


                    if (tgParser.ParsedData[i].containskey("meditation"))
                    {
                        //console.writeline("med value:" + tgParser.parseddata[i]["meditation"]);
                        writer.Write("med value:" + tgParser.ParsedData[i]["meditation"]);
                    }


                    if (tgParser.ParsedData[i].containskey("eegpowerdelta"))
                    {
                        //console.writeline("delta: " + tgParser.parseddata[i]["eegpowerdelta"]);
                        writer.Write("delta: " + tgParser.ParsedData[i]["eegpowerdelta"]);
                    }

                    if (tgParser.ParsedData[i].containskey("blinkstrength"))
                    {
                        //console.writeline("eyeblink " + tgParser.parseddata[i]["blinkstrength"]);
                        writer.Write("eyeblink " + tgParser.ParsedData[i]["blinkstrength"]);
                    }
                    //console.write("\r\n");
                }
            }

        }

        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }
    }

}



