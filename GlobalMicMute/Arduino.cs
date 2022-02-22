using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace GlobalMicMute
{
    public class Arduino
    {
        private SerialPort Port { get; set; }
        private SerialDataReceivedEventHandler DataReceivedHandler;
        private string ComPort;
        private int Baud;
        private bool IsInit = false;

        public Arduino(string serialPort, int baud, SerialDataReceivedEventHandler dataReceivedHandler)
        {
            ComPort = serialPort;
            Baud = baud;    
            DataReceivedHandler = dataReceivedHandler;
            if (ComPort != "none")
                SetupSerialComm(ComPort, Baud);
            
        }

        public Arduino()
        {
            // dummy
        }
        public bool SetupSerialComm(string p, int b)
        {
            try
            {   
                if (Port != null) {
                   Port.Dispose();
                }
                Port = new SerialPort(p, b);
                Port.DtrEnable = true;
                Port.Open();
                Port.DataReceived += DataReceivedHandler;
                IsInit = true;
            }
            catch (Exception)
            {

               IsInit = false; 
            }
            return IsInit;
        }

        public void sendCommandToSerial(bool allMuted)
        {
            if (IsInit)
            {
                switch (allMuted)
                {
                    case true:
                        Port.Write("1");
                        break;
                    case false:
                        Port.Write("0");
                        break;
                }
            }
        }

    }
}
