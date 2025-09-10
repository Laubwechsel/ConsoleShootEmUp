using ConsoleShootEmUp.Entities.Enemies;
using ConsoleShootEmUp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Walls
{
    internal class Wall : Entity, IDirectionalDisplay
    {
        public Wall(Engine engine) : base(engine)
        {
            DisplayChar = '|';
            DisplayColor = ConsoleColor.Gray;
            MovementBlockMask = BitMasks.PLAYER | BitMasks.ENEMY;
        }
        public void SetHorizontal(bool horizontal)
        {
            if (horizontal)
                DisplayChar = '-';
            else
                DisplayChar = '|';
        }
        public void SetBlocker()
        {
            DisplayChar = 'X';
        }
        public override void OnCollisionEnter(Entity other)
        {
            if ((other.BitMask & (BitMasks.PLAYER_PROJECTLE | BitMasks.ENEMY_PROJECTILE)) > 0)
                Engine.RemoveEntity(other, this);
            if (other is EnemyShot)
                Engine.RemoveEntity(other, this);
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
    }
}
