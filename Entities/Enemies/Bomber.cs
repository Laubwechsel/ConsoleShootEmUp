using ConsoleShootEmUp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Enemies
{
    internal class Bomber : Entity, IDamageable, IActivatable, IDirectionalMovement
    {
        private double _weaponTimer = 0d;
        private double _weaponCooldown = 1.8d;
        private double _speed = 0.5d;
        private Vector2D _direction = Vector2D.Left;
        public bool Active { get; private set; } = true;
        public IActivator? Activator { get; private set; }

        public int Health { get; protected set; } = 1;

        public Bomber(Engine engine) : base(engine)
        {
            DisplayChar = 'B';
            DisplayColor = ConsoleColor.DarkRed;
            BitMask = BitMasks.ENEMY;
            _weaponTimer = Random.Shared.NextDouble() * _weaponCooldown;

        }
        public override void Update(double deltaTime)
        {
            if (!Active) return;
            _weaponTimer += deltaTime;
            if (_weaponTimer > _weaponCooldown)
            {
                _weaponTimer = 0d;
                Vector2D shotPosition = GlobalPosition;
                shotPosition.Y += 1;
                EnemyShot newShot = new EnemyShot(Engine);
                FindInParent<Level>()!.AddChild(newShot);
                newShot.SetGlobalPosition(shotPosition);
                newShot.SetParent(FindInParent<Level>());
                newShot.SetDirection(Vector2.Down);
                Engine.AddEntity(newShot);
            }
            Move(_direction * (_speed * deltaTime));
            Vector2D globalPos = GlobalPosition;
            if (globalPos.X < 0 || globalPos.Y < 0 || globalPos.Y > Engine.Display.Height)
                Engine.RemoveEntity(this);
        }
        public override void OnCollisionEnter(Entity other)
        {
            if (other is Player player)
            {
                player.TakeDamage(1);
                Engine.RemoveEntity(this);
            }
        }
        public override void ExitEngine()
        {
            base.ExitEngine();
            Activator?.RemoveFromActivate(this);
        }
        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health <= 0)
                Death();
        }
        private void Death()
        {
            Engine.RemoveEntity(this);
            FindInParent<Game>()?.AddScore(100);
        }

        public void SetActive(bool active)
        {
            Active = active;
        }
        public void SetActivator(IActivator activator)
        {
            Activator = activator;
        }

        public void SetDirection(Vector2D direction)
        {
            _direction = direction.Normalized();
        }

    }
}
