﻿using System;

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
            string name;
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            //Thread readThread = new Thread(Read);
            bool _continue;
            // Create a new SerialPort object with default settings.

            Console.WriteLine("*** HelloEEG ***!");
            Console.WriteLine("Type \'quit\' to exit.");
            
            _serialPort = new SerialPort("COM5");
            try
            {
                Console.Write("Connecting to Arduino...");
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
                    Console.Write("Success!\n");
                    //Console.Write("\nType QUIT to exit");
                }               
            }
                
            
            //Thread.Sleep(1);
            //readThread.Abort();
            //try
            //{
            //    _serialPort.Close();
            //}
            //catch (IOException e)
            //{
            //    Console.WriteLine("COM5 did not close successfully.");
            //}
   
            

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
            //Console.WriteLine("Type \'quit\' to exit.");
            //readThread.Start();

            while (_continue)
            {
                message = Console.ReadLine();

                if (stringComparer.Equals("quit", message))
                {
                    _continue = false;
                    _serialPort.Close();
                }
                else
                {
                    _serialPort.WriteLine(message);
                }
                //Read();
            }

            // Blink detection needs to be manually turned on
            //connector.setBlinkDetectionEnabled(true);
            //Thread.Sleep(450000);

            System.Console.WriteLine("Goodbye.");
            connector.Close();
            Environment.Exit(0);
        }


        // Called when a device is connected 

        static void OnDeviceConnected(object sender, EventArgs e) {

            Connector.DeviceEventArgs de = (Connector.DeviceEventArgs)e;

            Console.WriteLine("Mindwave Mobile...connected on " + de.Device.PortName);
            de.Device.DataReceived += new EventHandler(OnDataReceived);

        }




        // Called when scanning fails

        static void OnDeviceFail(object sender, EventArgs e) 
        {
            Console.WriteLine("No devices found! :(");
        }



        // Called when each port is being validated

        static void OnDeviceValidating(object sender, EventArgs e) {

            Console.WriteLine("Validating...");

        }

        // Called when data is received from a device

        static void OnDataReceived(object sender, EventArgs e) {

            //Device d = (Device)sender;

            Device.DataEventArgs de = (Device.DataEventArgs)e;
            DataRow[] tempDataRowArray = de.DataRowArray;

            TGParser tgParser = new TGParser();
            tgParser.Read(de.DataRowArray);

            

            /* Loops through the newly parsed data of the connected headset*/
			// The comments below indicate and can be used to print out the different data outputs. 

            for (int i = 0; i < tgParser.ParsedData.Length; i++)
            {
                if (tgParser.ParsedData[i].ContainsKey("Raw")){

                    //Console.WriteLine("Raw Value:" + tgParser.ParsedData[i]["Raw"]);

                }

                if (tgParser.ParsedData[i].ContainsKey("PoorSignal"))
                {
                    //The following line prints the Time associated with the parsed data                  
                    //A Poor Signal value of 0 indicates that your headset is fitting properly

                    poorSig = (byte)tgParser.ParsedData[i]["PoorSignal"];
                    try
                    {
                        _serialPort.Write("%" + tgParser.ParsedData[i]["PoorSignal"]);
                        //Console.WriteLine("Poor Signal:" + tgParser.ParsedData[i]["PoorSignal"]);
                        //Console.Write(tgParser.ParsedData[i]["PoorSignal"] + "%");
                    }
                    catch (System.InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                if (tgParser.ParsedData[i].ContainsKey("Attention")) 
                {
                    try
                    {
                       // Console.WriteLine("Att Value:" + tgParser.ParsedData[i]["Attention"]);
                        _serialPort.Write("^" + tgParser.ParsedData[i]["Attention"]);
                       // Console.Write("^" + tgParser.ParsedData[i]["Attention"]);
                    }
                    catch (System.InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                if (tgParser.ParsedData[i].ContainsKey("Meditation")) 
                {
                    try
                    {
                        // Console.WriteLine("Att Value:" + tgParser.ParsedData[i]["Attention"]);
                        _serialPort.Write("&" + tgParser.ParsedData[i]["Meditation"]);
                       // Console.Write("&" + tgParser.ParsedData[i]["Meditation"]);
                    }
                    catch (System.InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                if(tgParser.ParsedData[i].ContainsKey("EegPowerDelta")) 
                {
                    try
                    {
                        // Console.WriteLine("Att Value:" + tgParser.ParsedData[i]["Attention"]);
                        _serialPort.Write("$" + tgParser.ParsedData[i]["EegPowerDelta"]);
                        //Console.WriteLine("$" + tgParser.ParsedData[i]["EegPowerDelta"]*");
                    }
                    catch (System.InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (tgParser.ParsedData[i].ContainsKey("BlinkStrength"))
                {
                    //Console.WriteLine("Eyeblink " + tgParser.ParsedData[i]["BlinkStrength"]);
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



