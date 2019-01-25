using Giveaway_Machine.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine
{
    class Program
    {
        private static void initializeProgram()
        {
            // Console Properties
            Console.SetWindowSize(130, Console.WindowHeight);
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Welcome text
            Console.WriteLine(" ______________________________________________________________________________________________________________________________");
            Console.WriteLine("|  ██████╗ ██╗██╗   ██╗███████╗ █████╗ ██╗    ██╗ █████╗ ██╗   ██╗    ███╗   ███╗ █████╗  ██████╗██╗  ██╗██╗███╗   ██╗███████╗ |");
            Console.WriteLine("| ██╔════╝ ██║██║   ██║██╔════╝██╔══██╗██║    ██║██╔══██╗╚██╗ ██╔╝    ████╗ ████║██╔══██╗██╔════╝██║  ██║██║████╗  ██║██╔════╝ |");
            Console.WriteLine("| ██║  ███╗██║██║   ██║█████╗  ███████║██║ █╗ ██║███████║ ╚████╔╝     ██╔████╔██║███████║██║     ███████║██║██╔██╗ ██║█████╗   | ");
            Console.WriteLine("| ██║   ██║██║╚██╗ ██╔╝██╔══╝  ██╔══██║██║███╗██║██╔══██║  ╚██╔╝      ██║╚██╔╝██║██╔══██║██║     ██╔══██║██║██║╚██╗██║██╔══╝   |");
            Console.WriteLine("| ╚██████╔╝██║ ╚████╔╝ ███████╗██║  ██║╚███╔███╔╝██║  ██║   ██║       ██║ ╚═╝ ██║██║  ██║╚██████╗██║  ██║██║██║ ╚████║███████╗ | ");
            Console.WriteLine("|  ╚═════╝ ╚═╝  ╚═══╝  ╚══════╝╚═╝  ╚═╝ ╚══╝╚══╝ ╚═╝  ╚═╝   ╚═╝       ╚═╝     ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚═╝╚═╝  ╚═══╝╚══════╝ | ");
            Console.WriteLine(" ‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            initializeProgram();
            Facade facade = new Facade();

            bool exit = false;
            while(!exit)
            {
                // First, reset the console color to its default
                Console.ResetColor();

                Console.WriteLine("");
                Console.WriteLine("Waiting for user input. Please enter help to show the commands.");

                // Reset color to the input color
                Console.ForegroundColor = ConsoleColor.White;

                // Read the input and handle it
                exit = facade.Parser.ParseCommand(Console.ReadLine());

            }
        }
    }
}
