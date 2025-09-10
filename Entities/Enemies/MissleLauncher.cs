using ConsoleShootEmUp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Enemies
{
    internal class MissleLauncher : Entity,IActivatable
    {
        public bool Active { get; private set; } = true;
        public IActivator? Activator { get; private set; }
        private Vector2 _shotDirection = Vector2.Up;
        private double _weaponTimer = 0d;
        private double _weaponCooldown = 5d;
        private double _speed = 0.5;

        private Player _player;
        public MissleLauncher(Engine engine,Player player) : base(engine)
        {
            DisplayChar = 'M';
            DisplayColor = ConsoleColor.DarkRed;
            BitMask = BitMasks.ENEMY;
            _weaponTimer = Random.Shared.NextDouble() * _weaponCooldown;
            _player = player;
        }

        public override void Update(double deltaTime)
        {
            if (!Active) return;
            _weaponTimer += deltaTime;
            if (_weaponTimer > _weaponCooldown)
            {
                _weaponTimer = 0d;
                Vector2D shotPosition = GlobalPosition + _shotDirection;
                EnemyShot newShot = new EnemyShot(Engine);
                newShot.SetGlobalPosition(shotPosition);
                newShot.SetParent(Parent);
                newShot.SetDirection(_shotDirection);
                Engine.AddEntity(newShot);
            }
            Move(-_speed * deltaTime, 0);
            Vector2D globalPos = GlobalPosition;
            if (globalPos.X < 0 || globalPos.Y < 0 || globalPos.Y > Engine.Display.Height)
                Engine.RemoveEntity(this);

        }
        public override void ExitEngine()
        {
            base.ExitEngine();
            Activator?.RemoveFromActivate(this);
        }

        public void SetActivator(IActivator activator)
        {
            Activator = activator;
        }

        public void SetActive(bool active)
        {
            Active = active;
        }


    }
}
