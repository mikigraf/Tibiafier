using System;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.IO;
using System.Drawing.Imaging;
using tessnet2;


namespace Tibiafier.Utils
{
    class Tibia
    {
        public Process Client { get; set; }

        public Tibia()
        {
            Client = Process.GetProcessesByName("Tibia")[0];
        }
    }
}
