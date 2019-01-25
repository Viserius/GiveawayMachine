using Giveaway_Machine.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine.Controller.Commands
{
    class RunCommand : Command
    {
        public override string Name { get; }

        public override string Description { get; }

        public override bool isExit { get; }

        public RunCommand()
        {
            Name = "run";

            StringBuilder descriptionSB = new StringBuilder("Execute one of the giveaway runners.");
            descriptionSB.AppendLine();
            foreach(AbstractRunner runner in AbstractRunner.getRunners())
            {
                descriptionSB.AppendLine("            run " + runner.Name);
            }
            Description = descriptionSB.ToString();
        }

        public override void Execute(List<string> arguments)
        {
            if(arguments.Count < 1)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid arguments provided. Please type help for usage.");
                return;
            }
            throw new NotImplementedException();
        }
    }
}
