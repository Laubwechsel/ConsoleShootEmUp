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
            string title =
                            "............................................\n" +
                            ".CCCCCCC....SSSSSSS....EEEEEEE....UU.....UU.\n" +
                            ".CCCCCCC....SSSSSSS....EEEEEEE....UU.....UU.\n" +
                            ".CC.........SS.........EE.........UU.....UU.\n" +
                            ".CC.........SSSSSSS....EEEEEEE....UU.....UU.\n" +
                            ".CC.........SSSSSSS....EEEEEEE....UU.....UU.\n" +
                            ".CC..............SS....EE.........UU.....UU.\n" +
                            ".CCCCCCC....SSSSSSS....EEEEEEE.....UU...UU..\n" +
                            ".CCCCCCC....SSSSSSS....EEEEEEE......UUUUU...\n" +
                            "............................................\n";
            foreach (char item in title)
            {
                if (item == '.')
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                else
                    Console.ForegroundColor = ConsoleColor.White;
                Console.Write(item);

            }
            Console.WriteLine("It is recomended to maximise the console window");
            Console.WriteLine("Press ESC to Exit anytime");
            Console.WriteLine("Press any other key to start");
            if (OperatingSystem.IsWindows())
                Console.WriteLine("Zoom: use ctr + mouse wheel to change the games size");
            if (OperatingSystem.IsLinux())
                Console.WriteLine("Zoom: use use ctr + '+' or '-' to change the games size");
            if (OperatingSystem.IsIOS())
                Console.WriteLine("Zoom: use use cmd + '+' or '-' to change the games size");

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
