using ConsoleShootEmUp.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities
{
    internal class PlayerRepulsor : Entity
    {
        private double _direction;
        private double _speed = 5d;
        private List<Entity> _movedEnemies = new();
        public PlayerRepulsor(Engine engine, int direction) : base(engine)
        {
            DisplayChar = '_';
            BitMask = BitMasks.PLAYER_PROJECTLE;
            //CollisonInterpolation = engine.CollisonInterpolation.Interpolation;

            _direction = direction;
        }
        public override void Update(double deltaTime)
        {
            Move(0, _direction * _speed * deltaTime);
            if (!Engine.Display.BoundsCheck(GlobalPosition))
                Engine.RemoveEntity(this);

        }
        public override void OnCollisionEnter(Entity other)
        {
            if (other is EnemyShot)
            {
                Engine.RemoveEntity(other);
            }
            if (other is Enemy && !_movedEnemies.Contains(other))
            {
                other.Move(0, _direction);
                _movedEnemies.Add(other);
            }
        }
    }
}
