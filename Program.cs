using ConsoleShootEmUp.Entities;

namespace ConsoleShootEmUp
{
    internal class Program
    {
        public static event Action<ConsoleKeyInfo>? Input;
        static void Main(string[] args)
        {
#if DEBUG
            SaveFile.Delete();
#endif
            SaveFile.Load();
            Engine engine = new();
            Game game = new Game(engine);
            engine.AddEntity(game);
#if RELEASE
            Console.WriteLine("It is recomended to maximise the console window");
            Console.WriteLine("Press ESC to Exit anytime");
            Console.WriteLine("Press any other key to start");
                ConsoleKeyInfo startInfo = Console.ReadKey(true);
                if (startInfo.Key == ConsoleKey.Escape)
                    return;
#endif
            engine.Start();

            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Escape)
                    break;
                Input?.Invoke(info);
            }
            SaveFile.Save();
        }
    }
}
