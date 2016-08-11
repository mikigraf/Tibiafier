using System;
using System.Text;
using System.Threading.Tasks;
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

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);


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

        public string ReadString(long Address, IntPtr Handle, uint length = 32)
        {
            return ASCIIEncoding.Default.GetString(ReadBytes(Handle, Address, length)).Split('\0')[0];
        }

        public static void sendEmail(String fromEmail, String password, String toEmail, String sub, String message)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(toEmail);
            mail.Subject = sub;
            mail.Body = message;
            mail.IsBodyHtml = true;
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 465))
            {
                smtp.Credentials = new NetworkCredential(fromEmail, password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }

        static void Main(string[] args)
        {
            Process Tibia = Process.GetProcessesByName("Tibia")[0];
            IntPtr Handle = Tibia.Handle;
            UInt32 Base = (UInt32)Tibia.MainModule.BaseAddress.ToInt32();
            UInt32 CharacterName = 0x6D9050;
            UInt32 Experience = 0x53B768;
            UInt32 HealthAddress = 0x6D9000;
            UInt32 LevelAddress = 0x53B778;
            UInt32 Depot = 0x53BF4F;
            UInt32 Say = 0x04940DA8;

            Console.WriteLine("Current character: " + Convert.ToString(ReadInt32(Base + CharacterName, Handle)));
            Console.WriteLine("Experience: " + Convert.ToString(ReadInt32(Base + Experience, Handle)));
            Console.WriteLine("Current level: " + Convert.ToString(ReadInt32(Base + LevelAddress, Handle)));
            int levelBefore = 0;
            int currentLevel = 34;
            while (true)
            {
                Thread.Sleep(60000);
                Console.WriteLine("Current character: " + Convert.ToString(ReadInt32(Base + CharacterName, Handle)));
                Console.WriteLine("Experience: " + Convert.ToString(ReadInt32(Base + Experience, Handle)));
                Console.WriteLine("Current level: " + Convert.ToString(ReadInt32(Base + LevelAddress, Handle)));
                if (currentLevel > levelBefore)
                {
                    Console.WriteLine("Level up!");
                    levelBefore = currentLevel;
                }
            }

            Console.ReadLine();

        }
    }
}