using ConsoleShootEmUp.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Walls
{
    internal class Reflector : Entity
    {
        public Reflector(Engine engine) : base(engine)
        {
            DisplayChar = '|';
            DisplayColor = ConsoleColor.DarkRed;
            MovementBlockMask = BitMasks.PLAYER;
        }
        public override void OnCollisionEnter(Entity other)
        {
            if (other is PlayerShot ps)
            {
                Vector2D direction = ps.Direction*-1;
                Engine.RemoveEntity(other);
                EnemyShot reflect = new EnemyShot(Engine);
                FindInParent<Level>()!.AddChild(reflect);  
                reflect.SetLocalPosition(0, 0);
                reflect.SetDirection(direction);
                Engine.AddEntity(reflect);
            }
        }
        protected override void OnBlockOtherMovement(Entity other, Vector2D from, Vector2D to)
        {
            Vector2D otherP = other.GlobalPosition;
            Vector2D thisP = from;
            Vector2 delta = new Vector2(otherP) - new Vector2(thisP);
            other.Move(delta);
            if (other is Player player && ((int)Math.Round(GlobalPosition.X) <= 0))
                player.TakeDamage(int.MaxValue);
        }

        public void SetHorizontal(bool horizontal)
        {
            if (horizontal)
                DisplayChar = '-';
            else
                DisplayChar = '|';
        }
    }
}
