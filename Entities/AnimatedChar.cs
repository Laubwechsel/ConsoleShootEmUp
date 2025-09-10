using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities
{
    internal class AnimatedChar : Entity
    {
        private List<char> _chars = new();
        private List<ConsoleColor> _colors = new();
        private List<double> _durations = new();
        private double _timer = 0d;
        private int _state = 0;
        private double _lifeDuration = 0d;
        private double _lifeTimer;
        public AnimatedChar(Engine engine) : base(engine)
        {
        }
        public override void Update(double deltaTime)
        {
            _timer -= deltaTime;
            if (_timer <= 0)
                SetState(_state + 1);

            _lifeTimer += deltaTime;
            if (_lifeTimer >= _lifeDuration)
                Engine.RemoveEntity(this);
        }
        public void AddState(char c, ConsoleColor cc, double duration)
        {
            _chars.Add(c);
            _colors.Add(cc);
            _durations.Add(duration);
            _lifeDuration += duration;
        }
        public void SetState(int state)
        {
            if (_chars.Count == 0)
                return;
            state %= _chars.Count;
            _state = state;
            DisplayChar = _chars[_state];
            DisplayColor = _colors[_state];
            _timer = _durations[_state];
        }
        public void SetLifeDuration(double duration)
        {
            _lifeDuration = duration;
        }
    }
}
