using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBleApp
{
    public class Log
    {
        public static void Red(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
        }

        public static void Green(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
        }

        public static void Yellow(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
        }

        public static void Cyan(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(msg);
        }

        public static void Std(string msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);
        }
    }
}
