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
        private static SerialPort port;
       
        public Arduino(string serialPort, int baud, SerialDataReceivedEventHandler dataReceivedHandler)
        {
            SetupSerialComm(serialPort, baud);
            port.DataReceived += dataReceivedHandler;
        }

        public Arduino()
        {
            // dummy
        }
        private void SetupSerialComm(string p, int b)
        {
            port = new SerialPort(p, b);
            port.DtrEnable = true;
            port.Open();
        }

        public void sendCommandToSerial(bool allMuted)
        {
            if (port != null)
            {
                switch (allMuted)
                {
                    case true:
                        port.Write("1");
                        break;
                    case false:
                        port.Write("0");
                        break;
                }
            }
        }

    }
}
