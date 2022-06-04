using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheStoicRobotTwitter
{
    public class ConsoleLogging
    {
        public static void PassMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static void GreenConsole()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }

        public static void BlueConsole()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
        }
    }
}
