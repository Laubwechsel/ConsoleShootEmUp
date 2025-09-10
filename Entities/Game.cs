using ConsoleShootEmUp.Entities.Menus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities
{
    internal class Game : Entity
    {
        public int Score
        {
            get => _score;
            set
            {
                lock (_scoreLock)
                {
                    _score = value;
                    Engine.Display.SetUpperUIElement(new(10, c_uiScoreString, Display.FormatNumber(Score, 9)));
                }
            }
        }
        private int _score = 0;
        private object _scoreLock = new();
        private const string c_uiScoreString = "Score:";

        private bool _inLevel = false;
        private Stopwatch _levelTimer = new();
        private const string c_uiTimeString = "Time:";
        private LevelBuilder _levelBuilder;

        private int _currentLevel = 0;

        public Game(Engine engine) : base(engine)
        {
            _levelBuilder = new(engine);
        }
        public override void EnterEngine()
        {
            base.EnterEngine();
            AddScore(0);
            ShowLevelSelect();
            Engine.Input += HandleInput;
        }
        public override void Update(double deltaTime)
        {
            if (_inLevel)
            {

                string time = Display.FormatNumber((int)_levelTimer.ElapsedMilliseconds, 6, Display.Trim.Front);
                string formattedTime = "";
                for (int i = time.Length - 2; i >= 0; i--)
                {
                    formattedTime = time[i] + formattedTime;
                    if (time.Length - i == 3)
                        formattedTime = '.' + formattedTime;
                }

                Engine.Display.SetUpperUIElement(new(0, c_uiTimeString, formattedTime));
            }
        }
        public void AddScore(int amount)
        {
            Score += amount;
        }
        public override void ExitEngine()
        {
            base.ExitEngine();
            Engine.Input -= HandleInput;
        }
        private void HandleInput(ConsoleKeyInfo info)
        {
            switch (info.Key)
            {
                case ConsoleKey.F10:
                    Engine.RemoveEntity(Children[0]);
                    Level level01 = _levelBuilder.BuildTestLevel();
                    AddChild(level01);
                    break;
            }
        }

        public void SpawnLevel(int level)
        {
            _currentLevel = level;
            _levelTimer.Restart();
            switch (level)
            {
                case 0:
                    Engine.RemoveEntity(Children[0]);
                    Engine.Display.ResizeDisplay(30, 11);
                    Level level00 = _levelBuilder.BuildLevel00();
                    AddChild(level00);
                    _inLevel = true;
                    break;
                case 1:
                    Engine.RemoveEntity(Children[0]);
                    Engine.Display.ResizeDisplay(30, 11);
                    Level level01 = _levelBuilder.BuildLevel01();
                    AddChild(level01);
                    _inLevel = true;
                    break;
                case 2:
                    Engine.RemoveEntity(Children[0]);
                    Engine.Display.ResizeDisplay(30, 11);
                    Level level02 = _levelBuilder.BuildLevel02();
                    AddChild(level02);
                    _inLevel = true;
                    break;
                case 3:
                    Engine.RemoveEntity(Children[0]);
                    Engine.Display.ResizeDisplay(10, 11);
                    Level level03 = _levelBuilder.BuildLevel03();
                    AddChild(level03);
                    _inLevel = true;
                    break;
                case 4:
                    Engine.RemoveEntity(Children[0]);
                    Engine.Display.ResizeDisplay(45, 17);
                    Level level04 = _levelBuilder.BuildLevel04();
                    AddChild(level04);
                    _inLevel = true;
                    break;
                default:
                    break;
            }
            AddScore(0);
        }
        public void FinishLevel(bool win)
        {
            _levelTimer.Stop();
            _inLevel = false;
            Engine.RemoveEntity(Children[0]);
            if (win)
            {
                if (SaveFile.UnlockedLevel.Count > _currentLevel + 1)
                    SaveFile.UnlockedLevel[_currentLevel + 1] = true;
                if (SaveFile.Highscores.Count > _currentLevel)
                    if (SaveFile.Highscores[_currentLevel] < Score)
                        SaveFile.Highscores[_currentLevel] = Score;
                if (SaveFile.Times.Count > _currentLevel)
                    if (SaveFile.Times[_currentLevel] > _levelTimer.ElapsedMilliseconds)
                        SaveFile.Times[_currentLevel] = (int)_levelTimer.ElapsedMilliseconds;
                SaveFile.Save();
            }
            Score = 0;
            Engine.Display.RemoveUpperUIElement(c_uiScoreString);
            Engine.Display.RemoveUpperUIElement(c_uiTimeString);
            ShowLevelSelect();
        }
        private void ShowLevelSelect()
        {
            MainMenu mm = new(Engine);
            Engine.AddEntity(mm);
            AddChild(mm);
            Engine.Display.ResizeDisplay(35, 9);
        }
    }
}
