using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousProcessing
{
    public class UnblockedConsole
    {
        private static string result;
        private const string ExitCommand = "exit";
        private const string ShowCommand = "show";

        public static void Run()
        {
            Console.WriteLine("Calculating...");
            Task.Run(() => CalculateSlowly());

            Console.WriteLine("Enter command:");
            while (true)
            {
                string line = Console.ReadLine();

                if (ShowCommand.Equals(line, StringComparison.OrdinalIgnoreCase))
                {
                    if (result == null)
                    {
                        Console.WriteLine("Stil calculating, please wait.");
                    }
                    else
                    {
                        Console.WriteLine(result);
                    }
                }

                if (ExitCommand.Equals(line, StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }
        }

        private static void CalculateSlowly()
        {
            Thread.Sleep(5000);
            result = "42";
        }
    }
}
