namespace SleepZone.Todos.WebApp
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // Host
            MDP.AspNetCore.Host.Create(args).Run();
        }
    }
}
