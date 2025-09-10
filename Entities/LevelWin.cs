using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities
{
    internal class LevelWin : Entity
    {
        private bool _stopMover = true;
        public LevelWin(Engine engine) : base(engine)
        {
            DisplayChar = 'W';
            DisplayColor = ConsoleColor.Green;
        }
        public override void OnCollisionEnter(Entity other)
        {
            if (other is Player player)
            {
                Entity parent = Parent!;
                while (parent is not Level)
                {
                    parent = parent.Parent!;
                }
                Level level = (Level)parent ;
                level.Won();
            }
        }
        public void SetStopMover(bool stopMover)
        {
            _stopMover = stopMover;
        }
    }
}
