using ConsoleShootEmUp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Enemies
{
    internal class DistributedSpawner : Entity, IDamageable, IActivatable
    {
        private double _spawnTimer = 0.0;
        private double _spawnDelay = 3.5;
        private int _spawnLimit = 5;
        private int _spawnCounter = 0;
        private Vector2 _spawnOffset = new(0, 0);
        private int _spawnRange;
        public bool Active { get; private set; } = true;
        public IActivator? Activator { get; private set; }
        private Func<Entity> _spawnFunc;
        private Vector2D _entityDirection;
        public enum SpawnDirection { Horizontal, Vertical, Around }
        private SpawnDirection _spawnDirection = SpawnDirection.Vertical;

        public int Health { get; private set; }

        public DistributedSpawner(Engine engine, Func<Entity> spawnFunc, Vector2D entityDirection) : base(engine)
        {
            DisplayChar = 'S';
            DisplayColor = ConsoleColor.DarkMagenta;
            BitMask = BitMasks.ENEMY;
            _spawnRange = Engine.Display.Height;
            _spawnFunc = spawnFunc;
            _entityDirection = entityDirection;
        }
        public override void Update(double deltaTime)
        {
            if (!Active) return;

            _spawnTimer += deltaTime;
            if (_spawnTimer >= _spawnDelay)
            {
                Vector2D globalPosition = GlobalPosition;
                Vector2D targetPosition;
                int targetRow;
                int targetColumn;
                switch (_spawnDirection)
                {
                    case SpawnDirection.Horizontal:
                        targetColumn = Random.Shared.Next(0, _spawnRange);
                        targetPosition = new Vector2D(_spawnOffset.X + targetColumn, _spawnOffset.Y + globalPosition.Y);
                        break;
                    case SpawnDirection.Vertical:
                        targetRow = Random.Shared.Next(0, _spawnRange);
                        targetPosition = new Vector2D(_spawnOffset.X + globalPosition.X, _spawnOffset.Y + targetRow);
                        break;
                    case SpawnDirection.Around:
                    default:
                        targetColumn = Random.Shared.Next(0, _spawnRange);
                        targetRow = Random.Shared.Next(0, _spawnRange);
                        targetPosition = new Vector2D(globalPosition.X + _spawnOffset.X + targetColumn - _spawnRange / 2, globalPosition.Y + _spawnOffset.Y + targetRow - _spawnRange / 2);
                        break;
                }
                EntitySpawnPackage package = new EntitySpawnPackage(Engine, targetPosition, _entityDirection, _spawnFunc);
                package.SetGlobalPosition(globalPosition);
                package.SetParent(Parent);
                Engine.AddEntity(package);
                _spawnTimer = 0.0;
                _spawnCounter++;
                if (_spawnCounter >= _spawnLimit)
                    Engine.RemoveEntity(this);
            }
        }
        public override void ExitEngine()
        {
            base.ExitEngine();
            Activator?.RemoveFromActivate(this);
        }
        public void SetSpawnLimit(int limit)
        {
            _spawnLimit = limit;
        }
        public void SetSpawnDirection(SpawnDirection direction)
        {
            _spawnDirection = direction;
        }
        public void SetSpawnOffset(Vector2 offset)
        {
            _spawnOffset = offset;
        }
        public void SetSpawnRange(int range)
        {
            if (range < 0)
                _spawnRange = 0;
            else
                _spawnRange = range;
        }
        public void SetSpawnDelay(double delay)
        {
            _spawnDelay = delay;
        }
        public void RandomizeTimer()
        {
            _spawnTimer = Random.Shared.NextDouble() * _spawnDelay;
        }
        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health <= 0)
                Death();
        }

        public void SetActive(bool active)
        {
            Active = active;
        }
        public void SetActivator(IActivator activator)
        {
            Activator = activator;
        }
        private void Death()
        {
            Engine.RemoveEntity(this);
            FindInParent<Game>()?.AddScore(100 + (100 * (_spawnLimit - _spawnCounter)));

        }
    }
}
