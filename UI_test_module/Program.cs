using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Intrinsics.Arm;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;

namespace udpclient
{
    class Program
    {
        [StructLayout(LayoutKind.Sequential, Pack = 4)] // creates the least padding, due to the program now does the most efficient allignment

        struct Struct1
        {
            public byte addr;
            public byte op1;
            public byte op2;
            public int value;
        }


        static byte[] getBytes(Struct1 str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];

            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(str, ptr, true);
                Marshal.Copy(ptr, arr, 0, size);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return arr;
        }

        static void Main(string[] args)
        {

        Struct1 ms = new Struct1();
            Console.WriteLine("1. Stepper motor");
            Console.WriteLine("2. LED");




            string inputs = Console.ReadLine();

            while (inputs != "afslut")
            {

                Console.Write(">");
 
                if (inputs != null)
                {



                    ms.addr = byte.Parse(inputs);
                    if (inputs == "1")
                    {
                        UdpClient Client = new UdpClient();
                        IPEndPoint localEp = new IPEndPoint(IPAddress.Parse("192.168.1.123"), 70);


                        Console.WriteLine("direction");
                        inputs = Console.ReadLine();
                        ms.op1 = byte.Parse(inputs);
                        Console.WriteLine("how many turns?");
                        inputs = Console.ReadLine();
                        ms.op2 = byte.Parse(inputs);

                        ms.value = 32000;

                        var bytesent = getBytes(ms);
                        Client.Connect(localEp);
;
                        Client.Send(bytesent, bytesent.Length);
                        

                        Console.Write(inputs);
                        Console.WriteLine(" ---- sendt");

                        byte[] data = Client.Receive(ref localEp);
                        string text = Encoding.UTF8.GetString(data);
                        Console.Write(">");
                        Console.WriteLine(text + "---- modtaget");

                        Client.Close();
                        inputs = Console.ReadLine();



                    }




            


                }
            }
        }
    }
}