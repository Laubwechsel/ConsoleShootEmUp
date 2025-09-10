using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities
{
    internal class Tutorial : Entity
    {
        public Tutorial(Engine engine) : base(engine)
        {
        }
        public override void EnterEngine()
        {
            base.EnterEngine();
            ShowTutorial();
        }
        public override void ExitEngine()
        {
            base.ExitEngine();
            RemoveTurorial();
        }
        private void ShowTutorial()
        {
            Action<UIElement> func = Engine.Display.SetLowerUIElement;
            func(new(0, "Move:", " WASD or Arrow Keys"));
            func(new(1, "Fire:", " KJLI or 2468 on NumPad"));
            func(new(2, "Shield:", " Space or 0/5 on NumPad"));
            func(new(3, "P", " Player", ConsoleColor.Green));
            func(new(4, "+", " Extra Life", ConsoleColor.Green));
            func(new(5, "W", " Goal", ConsoleColor.Green));
            func(new(6, "| -", " Player Shot", ConsoleColor.Yellow));
            func(new(7, "E B", " Enemies", ConsoleColor.Red));
            func(new(6, "| - ", "Enemy Shot", ConsoleColor.Red));
            func(new(8, "S", " Spawner", ConsoleColor.DarkMagenta));
            func(new(9, "e", " Spawn packed", ConsoleColor.Magenta));
            func(new(10, "T", " Turret", ConsoleColor.DarkMagenta));
            func(new(11, "- | ", " Destroyable Wall", ConsoleColor.Blue));
            func(new(12, "- |  ", "Reflector", ConsoleColor.Red));
            func(new(13, "- | X", " Walls", ConsoleColor.Gray));

            if (OperatingSystem.IsWindows())
                func(new(15, "Zoom", "use ctr + mouse wheel to change the games size"));
            if (OperatingSystem.IsLinux())
                func(new(15, "Zoom", "use ctr + '+' or '-' to change the games size"));
            if (OperatingSystem.IsIOS())
                func(new(15, "Zoom", "use cmd + '+' or '-' to change the games size"));
        }
        private void RemoveTurorial()
        {
            Action<string> func = Engine.Display.RemoveLowerUIElement;
            func("Move:");
            func("Fire:");
            func("Shield:");
            func("P");
            func("+");
            func("W");
            func("| -");
            func("E B");
            func("| - ");
            func("S");
            func("e");
            func("T");
            func("- | ");
            func("- |  ");
            func("- | X");
            func("Zoom");
        }

    }
}
