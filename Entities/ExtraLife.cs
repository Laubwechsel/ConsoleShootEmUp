using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities
{
    internal class ExtraLife : Entity
    {
        public ExtraLife(Engine engine) : base(engine)
        {
            DisplayChar = '+';
            DisplayColor = ConsoleColor.Green;
        }
        public override void OnCollisionEnter(Entity other)
        {
            if (other is Player player)
            {
                player.Heal(1);
                FindInParent<Game>()?.AddScore(1000);
                Engine.RemoveEntity(this);
            }
        }
    }
}
