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

        public HelpCommand(Facade facade) : base(facade)
        {
            Name = "help";
            Description = "Show a list of available commands";
            isExit = false;
        }

        public override void Execute(List<string> arguments)
        {
            logger.Info("");
            logger.Info("The Following commands are available:");

            Dictionary<string, Command> commands = facade.CommandHandler.getCommands();
            foreach(KeyValuePair<string, Command> kv in commands)
            {
                logger.Info($"        {kv.Key} - {kv.Value.Description}");
            }
        }
    }
}
