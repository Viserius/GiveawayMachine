using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine.Controller.Commands
{
    class HelpCommand : Command
    {
        public override string Name { get; }

        public override string Description { get; }

        public override bool isExit { get; }

        public HelpCommand()
        {
            Name = "help";
            Description = "Show a list of available commands";
            isExit = false;
        }

        public override void Execute(List<string> arguments)
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;

            Console.WriteLine("");
            Console.WriteLine("The Following commands are available:");

            List<Command> commands = getAllCommands();
            foreach(Command command in commands)
            {
                Console.WriteLine($"        {command.Name} - {command.Description}");
            }
        }
    }
}
