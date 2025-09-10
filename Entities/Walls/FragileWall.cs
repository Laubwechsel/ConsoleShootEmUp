using ConsoleShootEmUp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities.Walls
{
    internal class FragileWall : Entity, IDamageable, IDirectionalDisplay
    {
        public int Health { get; protected set; } = 3;
        private ConsoleColor _invurnableColor;
        private ConsoleColor _vulnerableColor;
        private double _invulnerableTimer = 0d;
        private double _invulnerableDuration = 2d;
        public FragileWall(Engine engine) : base(engine)
        {
            DisplayChar = '|';
            _invurnableColor = ConsoleColor.DarkBlue;
            _vulnerableColor = ConsoleColor.Blue;
            DisplayColor = _vulnerableColor;
            MovementBlockMask = BitMasks.PLAYER;
            Z = 2;
        }

        public override void Update(double deltaTime)
        {
            if (DisplayColor == _invurnableColor)
            {
                _invulnerableTimer += deltaTime;
                if (_invulnerableTimer > _invulnerableDuration)
                    DisplayColor = _vulnerableColor;
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
        public void TakeDamage(int amount)
        {
            if (DisplayColor == _invurnableColor) return;
            Health -= amount;
            DisplayColor = _invurnableColor;
            if (Health <= 0)
                Engine.RemoveEntity(this);
            _invulnerableTimer = 0d;
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

