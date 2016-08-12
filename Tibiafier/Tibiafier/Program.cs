using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Net.Mail;

namespace Tibiafier
{
    class Program
    {
        const int PROCESS_WM_READ = 0x0010;
        const int SW_SHOWMINNOACTIVE = 7;
        const String From = " ";
        const String Password = "";
        const String To = "";

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        static void MinimizeWindow(IntPtr handle)
        {
            ShowWindow(handle, SW_SHOWMINNOACTIVE);
        }

        public static byte[] ReadBytes(IntPtr Handle, Int64 Address, uint BytesToRead)
        {
            IntPtr ptrBytesRead;
            byte[] buffer = new byte[BytesToRead];
            ReadProcessMemory(Handle, new IntPtr(Address), buffer, BytesToRead, out ptrBytesRead);
            return buffer;
        }

        public static int ReadInt32(Int64 Address, IntPtr Handle)
        {
            return BitConverter.ToInt32(ReadBytes(Handle, Address, 4), 0);
        }

        public static string ReadString(Int64 Address, IntPtr Handle)
        {
            return BitConverter.ToString(ReadBytes(Handle, Address, 4), 0);
        }

        public string ReadString(long Address, IntPtr Handle, uint length = 32)
        {
            return ASCIIEncoding.Default.GetString(ReadBytes(Handle, Address, length)).Split('\0')[0];
        }

        public static void sendEmail(String fromEmail, String password, String toEmail, String sub, String message)
        {
            try
            {
                MailMessage mail = new MailMessage(fromEmail, toEmail);
                mail.To.Add(toEmail);
                mail.Subject = sub;
                mail.Body = message;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587 ;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromEmail, password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                Console.WriteLine(DateTime.Now.ToString() + " Notified!");
            }catch(Exception e)
            {
                Console.WriteLine("Failed to send the email");
            }
            
            
        }

        static void Main(string[] args)
        {
            Boolean notified = false;
            Utils.Tibia Client = new Utils.Tibia();
            Process Tibia = Client.Client;
            IntPtr Handle = Tibia.Handle;
            IntPtr TibiaMainWindow = Tibia.MainWindowHandle;
            UInt32 Base = (UInt32)Tibia.MainModule.BaseAddress.ToInt32();
            Utils.Offsets Offsets = new Utils.Offsets();

            while (true)
            {
                Thread.Sleep(1000);
                //Console.WriteLine("Current health: " + Convert.ToString(ReadInt32(Base + Offsets.XORAddress, Handle) ^ ReadInt32(Base + Offsets.HealthAddress, Handle)));
                //Console.WriteLine("Current mana: " + Convert.ToString(ReadInt32(Base + Offsets.ManaAddress, Handle) ^ ReadInt32(Base + Offsets.XORAddress, Handle)));
                //Console.WriteLine("Experience: " + Convert.ToString(ReadInt32(Base + Offsets.ExperienceAddress, Handle)));
                if((ReadInt32(Base + Offsets.XORAddress, Handle) ^ ReadInt32(Base + Offsets.HealthAddress, Handle)) == 0)
                {
                    if(notified == false)
                    {
                        sendEmail(From, Password, To, "[TIBIA] You died or logged out","Time: " + DateTime.Now.ToString());
                        notified = true;
                    }else
                    {
                        notified = true;
                    }
                }else
                {
                    notified = true;
                }
            }
            Console.ReadLine();
        }
    }
}