using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Config
{
    static class DebugHelper
    {
        public static void PrintWarning(string warningLocation, string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("WARNING " + warningLocation + ": ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
        }
    }
}
