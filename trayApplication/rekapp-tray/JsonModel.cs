using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rekapp_tray
{
    public class JsonModel
    {
        public string ReceiverIP { get; set; }
        public string ReceiverPort { get; set; }
        public string PackageCaptureTime { get; set; }
        public string PackageCaptureInterval { get; set; }
        public string InactivityThreshold { get; set; }

        public JsonModel()
        {

        }

        public JsonModel(string receiverIP, string receiverPort, string packageCaptureTime, string packageCaptureInterval, string inactivityThreshold)
        {
            ReceiverIP = receiverIP;
            ReceiverPort = receiverPort;
            PackageCaptureTime = packageCaptureTime;
            PackageCaptureInterval = packageCaptureInterval;
            InactivityThreshold = inactivityThreshold;
        }
    }
}
