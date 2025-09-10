using ConsoleShootEmUp.Entities.Enemies;
using ConsoleShootEmUp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Movers
{
    internal class Mover : Entity, IActivatable
    {
        private Vector2D _direction = new Vector2D(-1, 0);
        private double _noActiveMultiplier = 20d;
        private double _speed = 0.5;
        private bool _stop = false;

        public bool Active { get; private set; } = true;

        public IActivator? Activator { get; private set; }

        public Mover(Engine engine) : base(engine)
        {
            DisplayChar = Display.DEFAULT_EMPTY_SPACE;
            Z = int.MinValue;
        }
        public override void Update(double deltaTime)
        {
            if (_stop || !Active) return;
            double noActives = 1d;
            List<IActivator> activators = FindInChildren<IActivator>();
            foreach (IActivator activator in activators)
                if (activator.Active && !(activator as Entity)!.NotInEngine)
                {
                    noActives = 0d;
                    break;
                }
            if (activators.Count == 0)
            {
                noActives = 0d;
            }
            Vector2 oldPos = new(GlobalPosition);
            Move(deltaTime * (_speed + (_speed * (_noActiveMultiplier - 1) * noActives)) * _direction);
            Vector2 newPos = new(GlobalPosition);
            if (oldPos != newPos)
            {
                foreach (var item in FindInChildren<Turret>())
                {
                    item.ResetTimer();
                }
            }
        }
        public void StopMoving()
        {
            _stop = true;
        }
        public void StartMoving()
        {
            _stop = false;
        }
        public void SetDirection(Vector2D direction)
        {
            _direction = direction;
        }
        public void SetSpeed(double speed)
        {
            _speed = speed;
        }
        public void SetNoActiveMultiplier(double speed)
        {
            _noActiveMultiplier = speed;
        }

        public void SetActive(bool active)
        {
            Active = active;
        }

        public void SetActivator(IActivator activator)
        {
            Activator = activator;
        }
    }
}
