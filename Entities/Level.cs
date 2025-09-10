using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleShootEmUp.Entities
{
    internal class Level : Entity
    {
        public Level(Engine engine) : base(engine)
        {
        }

        internal void Won()
        {
            FindInParent<Game>()?.FinishLevel(true);
        }
    }
}
