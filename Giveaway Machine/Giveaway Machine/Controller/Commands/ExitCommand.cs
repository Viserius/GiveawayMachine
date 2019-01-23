using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine.Controller.Commands
{
    class ExitCommand : Command
    {
        public override string Name { get; }
        public override string Description { get; }
        public override bool isExit { get; }

        public ExitCommand()
        {
            Name = "exit";
            Description = "Close this application";
            isExit = true;
        }

        public override void Execute(List<string> arguments)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;

            Console.WriteLine("The Application is now closing...");
            System.Threading.Thread.Sleep(1000);
        }
    }
}
