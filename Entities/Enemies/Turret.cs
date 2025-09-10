using ConsoleShootEmUp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Enemies
{
    internal class Turret : Entity, IActivatable, IDamageable
    {
        public bool Active { get; private set; } = true;
        public IActivator? Activator { get; private set; }

        public int Health { get; private set; }

        private Vector2 _shotDirection = Vector2.Up;
        private double _weaponTimer = 0d;
        private double _weaponCooldown = 2d;

        public Turret(Engine engine) : base(engine)
        {
            DisplayChar = 'T';
            DisplayColor = ConsoleColor.DarkMagenta;
        }
        public override void Update(double deltaTime)
        {
            if (!Active) return;
            _weaponTimer -= deltaTime;
            if (_weaponTimer <= 0)
            {
                _weaponTimer = _weaponCooldown;
                Vector2D shotPosition = GlobalPosition + _shotDirection;
                EnemyShot newShot = new EnemyShot(Engine);
                newShot.SetGlobalPosition(shotPosition);
                newShot.SetParent(Parent);
                newShot.SetDirection(_shotDirection);
                Engine.AddEntity(newShot);
            }
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
        public void SetShotDirection(Vector2 direction)
        {
            _shotDirection = direction;
        }
        public void RandomizeTimer()
        {
            RandomizeTimer(_weaponCooldown);
        }
        public void RandomizeTimer(double range)
        {
            _weaponTimer = Random.Shared.NextDouble() * range;
        }
        public void ResetTimer()
        {
            _weaponTimer = 0d;
        }
        public void SetWeaponCooldown(double cooldown)
        {
           _weaponCooldown= cooldown;
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
    }
}
