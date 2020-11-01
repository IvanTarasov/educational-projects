using System;

namespace ConsoleUI.Commands
{
    class HelpCommand : Command
    {
        public HelpCommand()
        {
            Name = "help";
            Description = "lists available commands";
        }

        public override void Execute()
        {
            foreach (var command in Shell.Commands)
            {
                Console.WriteLine(command.GetInfo());
            }
        }
    }
}
