using Giveaway_Machine.Application.Gleam;
using Giveaway_Machine.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Giveaway_Machine
{
    class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static Facade facade;
        private static bool exitSystem = false;

        private static void initializeProgram()
        {
            // Console Properties
            Console.SetWindowSize(180, Console.WindowHeight);
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Welcome text
            logger.Info(" ______________________________________________________________________________________________________________________________");
            logger.Info("|  ██████╗ ██╗██╗   ██╗███████╗ █████╗ ██╗    ██╗ █████╗ ██╗   ██╗    ███╗   ███╗ █████╗  ██████╗██╗  ██╗██╗███╗   ██╗███████╗ |");
            logger.Info("| ██╔════╝ ██║██║   ██║██╔════╝██╔══██╗██║    ██║██╔══██╗╚██╗ ██╔╝    ████╗ ████║██╔══██╗██╔════╝██║  ██║██║████╗  ██║██╔════╝ |");
            logger.Info("| ██║  ███╗██║██║   ██║█████╗  ███████║██║ █╗ ██║███████║ ╚████╔╝     ██╔████╔██║███████║██║     ███████║██║██╔██╗ ██║█████╗   | ");
            logger.Info("| ██║   ██║██║╚██╗ ██╔╝██╔══╝  ██╔══██║██║███╗██║██╔══██║  ╚██╔╝      ██║╚██╔╝██║██╔══██║██║     ██╔══██║██║██║╚██╗██║██╔══╝   |");
            logger.Info("| ╚██████╔╝██║ ╚████╔╝ ███████╗██║  ██║╚███╔███╔╝██║  ██║   ██║       ██║ ╚═╝ ██║██║  ██║╚██████╗██║  ██║██║██║ ╚████║███████╗ | ");
            logger.Info("|  ╚═════╝ ╚═╝  ╚═══╝  ╚══════╝╚═╝  ╚═╝ ╚══╝╚══╝ ╚═╝  ╚═╝   ╚═╝       ╚═╝     ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚═╝╚═╝  ╚═══╝╚══════╝ | ");
            logger.Info(" ‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾");
            logger.Info("");
        }

        static void Main(string[] args)
        {
            deleteOldLogs();
            initializeProgram();
            facade = new Facade();

            // Capture exit console
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            while(!exitSystem)
            {
                logger.Info("");
                logger.Info("Waiting for user input. Please enter help to show the commands.");

                // Read the input and handle it
                exitSystem = facade.CommandHandler.ParseCommand(Console.ReadLine());

            }
        }

        private static void deleteOldLogs()
        {
            logger.Debug("Checking if there are old logs we need to remove.");
            if(File.Exists("log.txt"))
            {
                logger.Debug("Now removing old logs...");
                File.Delete("log.txt");
            }
        }

        #region Trap application termination
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            logger.Info("Exiting system due to external CTRL-C, or process kill, or shutdown");

            //do your cleanup here
            facade.CommandHandler.ParseCommand("exit");

            logger.Info("Finished closing the application..");

            //allow main to run off
            exitSystem = true;

            //shutdown right away so there are no lingering threads
            Environment.Exit(-1);

            return true;
        }
        #endregion
    }
}
