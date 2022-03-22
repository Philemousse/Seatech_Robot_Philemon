using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceFailbot
{
    public class Robot
    {
        public string receivedText = "";
        public float ditanceTelemetreDroit;
        public float distanceTelemetreCentre;
        public float distanceTelemetreGauche;
        public Queue<byte> byteListReceived;

        public Robot()
        {
            Queue<byte> byteListReceived = new Queue<byte>();
        }
    }
}
