using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Menus
{
    internal class MainMenu : Entity
    {
        private EntityChar _selector;
        private List<Entity> _options = new();
        private Vector2 _selectorOffset = new(1, 2);
        private int _selected = 0;
        public MainMenu(Engine engine) : base(engine)
        {
            _selector = new EntityChar(engine);
            AddChild(_selector);
            _selector.SetLocalPosition(_selectorOffset);
            _selector.SetChar('>');
            Engine.AddEntity(_selector);
            Entity levelTitle = SpawnWord("Level:");
            levelTitle.SetLocalPosition(2,1);
            Entity timeTitle= SpawnWord("Time:");
            timeTitle.SetLocalPosition(13,1);
            Entity highscorTitle=SpawnWord("Highscore:");
            highscorTitle.SetLocalPosition(22,1);
            AddLevelOption("Level 00");
            AddLevelOption("Level 01");
            AddLevelOption("Level 02");
            AddLevelOption("Level 03");
            AddLevelOption("Level 04");
            for (int i = 0; i < _options.Count; i++)
            {

                Vector2 position = _selectorOffset;
                position.Y += i;
                position.X += 1;
                _options[i].SetLocalPosition(position);
                if (i == _selected)
                    SetWordColor(_options[i], ConsoleColor.Green);
                if (!SaveFile.UnlockedLevel[i])
                    SetWordColor(_options[i], ConsoleColor.Red);
            }
        }
        public override void EnterEngine()
        {
            base.EnterEngine();
            Program.Input += HandleInput;
        }
        public override void ExitEngine()
        {
            base.ExitEngine();
            Program.Input -= HandleInput;
        }
        private void HandleInput(ConsoleKeyInfo info)
        {
            switch (info.Key)
            {
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    if (SaveFile.UnlockedLevel[_selected])
                        ((Game)Parent!).SpawnLevel(_selected);
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    MoveSelected(0);
                    break;
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    MoveSelected(_selected - 1);
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    MoveSelected(_options.Count - 1);
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    MoveSelected(_selected + 1);
                    break;
                case ConsoleKey.D0:
                case ConsoleKey.NumPad0:
                    MoveSelected(9);
                    break;
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    MoveSelected(0);
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    MoveSelected(1);
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    MoveSelected(2);
                    break;
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    MoveSelected(3);
                    break;
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    MoveSelected(4);
                    break;
                case ConsoleKey.D6:
                case ConsoleKey.NumPad6:
                    MoveSelected(5);
                    break;
                case ConsoleKey.D7:
                case ConsoleKey.NumPad7:
                    MoveSelected(6);
                    break;
                case ConsoleKey.D8:
                case ConsoleKey.NumPad8:
                    MoveSelected(7);
                    break;
                case ConsoleKey.D9:
                case ConsoleKey.NumPad9:
                    MoveSelected(8);
                    break;
                default:
                    break;
            }
        }
        private void MoveSelected(int to)
        {
            to = (to + _options.Count) % _options.Count;
            Vector2 pos = _selectorOffset;
            pos.Y += to;
            _selector.SetGlobalPosition(pos);
            if (SaveFile.UnlockedLevel[_selected])
                SetWordColor(_options[_selected], ConsoleColor.Gray);
            else
                SetWordColor(_options[_selected], ConsoleColor.Red);
            _selected = to;
            if (SaveFile.UnlockedLevel[_selected])
                SetWordColor(_options[_selected], ConsoleColor.Green);
            else
                SetWordColor(_options[_selected], ConsoleColor.DarkRed);
        }
        private Entity SpawnWord(string word)
        {
            Entity eWord = new Entity(Engine);
            Engine.AddEntity(eWord);
            for (int i = 0; i < word.Length; i++)
            {
                char c = word[i];
                EntityChar ec = new EntityChar(Engine);
                ec.SetChar(c);
                eWord.AddChild(ec);
                ec.SetLocalPosition(i, 0);
            }
            AddChild(eWord);
            return eWord;
        }
        private void SetWordColor(Entity word, ConsoleColor cc)
        {
            foreach (EntityChar entity in word.Children.Where(x=>x is EntityChar))
            {
                entity.SetColor(cc);
            }
        }
        private void AddLevelOption(string option)
        {
            Entity entity = SpawnWord(option);
            _options.Add(entity);

            string time = Display.FormatNumber((int)SaveFile.Times[_options.Count - 1], 6,Display.Trim.Front);
            string formattedTime = "";
            for (int i = time.Length - 2; i >= 0; i--)
            {
                formattedTime = time[i] + formattedTime;
                if (time.Length - i == 3)
                    formattedTime = '.' + formattedTime;
            }
            Entity level00Time = SpawnWord($"{formattedTime} s {Display.FormatNumber(SaveFile.Highscores[_options.Count-1], 9)} pt");
            entity.AddChild(level00Time);
            level00Time.SetLocalPosition(_selectorOffset.X + option.Length + 2, 0);

        }
    }
}
