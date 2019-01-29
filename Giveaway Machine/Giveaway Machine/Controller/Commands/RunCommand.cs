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

        public RunCommand(Facade facade) : base(facade)
        {
            Name = "run";

            StringBuilder descriptionSB = new StringBuilder("").AppendLine();
            descriptionSB.AppendLine("Execute one of the giveaway runners");
            foreach(KeyValuePair<string, AbstractRunner> kv in facade.RunnerHandler.getRunners())
            {
                descriptionSB.AppendLine("            run " + kv.Key);
            }
            Description = descriptionSB.ToString();
        }

        public override void Execute(List<string> arguments)
        {
            if(arguments.Count < 1)
            {
                logger.Warn("Invalid arguments provided. Please type help for usage.");
                return;
            }

            facade.RunnerHandler.startRunner(arguments[0]);
        }
    }
}
