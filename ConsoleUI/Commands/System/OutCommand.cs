namespace ConsoleUI.Commands
{
    class OutCommand : Command
    {
        public OutCommand()
        {
            Name = "out";
            Description = "close application";
        }

        public override void Execute()
        {
            Shell.Disable();
        }
    }
}
