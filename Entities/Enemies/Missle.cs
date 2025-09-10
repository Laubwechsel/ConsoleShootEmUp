using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Enemies
{
    internal class Missle : Entity
    {
        private double _speed = 4d;
        private Entity _target;
        public Missle(Engine engine,Entity target) : base(engine)
        {
            DisplayChar = '~';
            DisplayColor = ConsoleColor.Red;
            BitMask = BitMasks.ENEMY_PROJECTILE;
            Z = 1;
            _target = target;
        }
        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
            Vector2D direction = _target.GlobalPosition - GlobalPosition;
            direction = direction.Normalized();

            Move(_speed * deltaTime * direction);
            if (!Engine.Display.BoundsCheck(GlobalPosition))
                Engine.RemoveEntity(this);

        }
        public override void OnCollisionEnter(Entity other)
        {
            if (other is not Player) return;
            Player player = (Player)other;

            player.TakeDamage(1);
            Engine.RemoveEntity(this);
        }
        public void SetTarget(Entity target)
        {
            _target = target;
        }
    
    }
}
