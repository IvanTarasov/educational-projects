namespace ConsoleUI
{
    abstract class Command
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual void Execute()
        {
            System.Console.WriteLine("Pass command");
        }

        public string GetInfo() {
            return Name + ": " + Description;
        } 
    }
}
