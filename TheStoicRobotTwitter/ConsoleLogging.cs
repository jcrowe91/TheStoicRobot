using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheStoicRobotTwitter
{
    public class ConsoleLogging
    {
        public static void PassMessage(string message, ConsoleColor consoleColor = ConsoleColor.White)
        {
            Console.ForegroundColor = consoleColor;

            Console.WriteLine(message);

            Console.ResetColor();
        }

        
    }
}
