using ConsoleUI.Commands;
using System;
using System.Collections.Generic;

namespace ConsoleUI
{
    static class Shell
    {
        public static List<Command> Commands { get; private set; }
        private static bool WorkStatus;

        private const bool ACTIVE = true;
        private const bool DISABLE = false;

        public static void Start()
        {
            WorkStatus = ACTIVE;

            PrintWarningMessage("Type 'help' to get a list of available commands");
            InitCommands();
            StartGettingCommands();
        }

        private static void InitCommands()
        {
            Commands = new List<Command>();

            Commands.Add(new HelpCommand());
            Commands.Add(new OutCommand());
            // add new commands here
        }

        private static void StartGettingCommands()
        {
            while (WorkStatus == ACTIVE)
            {
                string receivedCommand = GetCommand();

                bool commandIsFound = false;

                foreach (var command in Commands)
                    if (receivedCommand == command.Name)
                    {
                        commandIsFound = true;
                        command.Execute();
                    }

                if (!commandIsFound)
                    PrintErrorMessage("COMMAND NOT FOUND!");
            }
        }

        private static string GetCommand()
        {
            Console.Write(">>> ");
            return Console.ReadLine();
        }

        private static string GetData(string dataType)
        {
            Console.Write(dataType + ": ");
            return Console.ReadLine();
        }

        public static void Disable()
        {
            WorkStatus = DISABLE;
        }

        public static void PrintSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n" + message + "\n");
            Console.ResetColor();
        }

        public static void PrintWarningMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + message + "\n");
            Console.ResetColor();
        }

        public static void PrintErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + message + "\n");
            Console.ResetColor();
        }
    }
}
